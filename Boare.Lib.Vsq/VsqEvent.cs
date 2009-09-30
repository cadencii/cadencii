/*
 * VsqEvent.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;

using bocoree;

namespace Boare.Lib.Vsq {

    /// <summary>
    /// vsqファイルのメタテキスト内に記述されるイベント。
    /// </summary>
    [Serializable]
    public class VsqEvent : IComparable<VsqEvent>, ICloneable {
        public String Tag;
        /// <summary>
        /// 内部で使用するインスタンス固有のID
        /// </summary>
        public int InternalID;
        public int Clock;
        public VsqID ID;
        public UstEvent UstEvent = new UstEvent();

        /// <summary>
        /// インスタンスをテキストファイルに出力します
        /// </summary>
        /// <param name="sw">出力先</param>
        public void write( TextMemoryStream sw ) {
            Vector<String> def = new Vector<String>( new String[]{ "Length",
                                                                   "Note#",
                                                                   "Dynamics",
                                                                   "PMBendDepth",
                                                                   "PMBendLength",
                                                                   "PMbPortamentoUse",
                                                                   "DEMdecGainRate",
                                                                   "DEMaccent" } );
            write( sw, def );
        }

        public void write( TextMemoryStream writer, Vector<String> print_targets ) {
            writeCor( writer, print_targets );
        }

        public void write( StreamWriter writer, Vector<String> print_targets ) {
            writeCor( new WrappedStreamWriter( writer ), print_targets );
        }

        private void writeCor( ITextWriter writer, Vector<String> print_targets ) {
            writer.writeLine( "[ID#" + ID.value.ToString( "0000" ) + "]" );
            writer.writeLine( "Type=" + ID.type );
            switch ( ID.type ) {
                case VsqIDType.Anote:
                    if ( print_targets.contains( "Length" ) ) writer.writeLine( "Length=" + ID.Length );
                    if ( print_targets.contains( "Note#" ) ) writer.writeLine( "Note#=" + ID.Note );
                    if ( print_targets.contains( "Dynamics" ) ) writer.writeLine( "Dynamics=" + ID.Dynamics );
                    if ( print_targets.contains( "PMBendDepth" ) ) writer.writeLine( "PMBendDepth=" + ID.PMBendDepth );
                    if ( print_targets.contains( "PMBendLength" ) ) writer.writeLine( "PMBendLength=" + ID.PMBendLength );
                    if ( print_targets.contains( "PMbPortamentoUse" ) ) writer.writeLine( "PMbPortamentoUse=" + ID.PMbPortamentoUse );
                    if ( print_targets.contains( "DEMdecGainRate" ) ) writer.writeLine( "DEMdecGainRate=" + ID.DEMdecGainRate );
                    if ( print_targets.contains( "DEMaccent" ) ) writer.writeLine( "DEMaccent=" + ID.DEMaccent );
                    if ( print_targets.contains( "PreUtterance" ) ) writer.writeLine( "PreUtterance=" + UstEvent.PreUtterance );
                    if ( print_targets.contains( "VoiceOverlap" ) ) writer.writeLine( "VoiceOverlap=" + UstEvent.VoiceOverlap );
                    if ( ID.LyricHandle != null ) {
                        writer.writeLine( "LyricHandle=h#" + ID.LyricHandle_index.ToString( "0000" ) );
                    }
                    if ( ID.VibratoHandle != null ) {
                        writer.writeLine( "VibratoHandle=h#" + ID.VibratoHandle_index.ToString( "0000" ) );
                        writer.writeLine( "VibratoDelay=" + ID.VibratoDelay );
                    }
                    if ( ID.NoteHeadHandle != null ) {
                        writer.writeLine( "NoteHeadHandle=h#" + ID.NoteHeadHandle_index.ToString( "0000" ) );
                    }
                    break;
                case VsqIDType.Singer:
                    writer.writeLine( "IconHandle=h#" + ID.IconHandle_index.ToString( "0000" ) );
                    break;
            }
        }

        /// <summary>
        /// このオブジェクトのコピーを作成します
        /// </summary>
        /// <returns></returns>
        public Object clone() {
            VsqEvent ret = new VsqEvent( Clock, (VsqID)ID.clone() );
            ret.InternalID = InternalID;
            if ( UstEvent != null ) {
                ret.UstEvent = (UstEvent)UstEvent.Clone();
            }
            ret.Tag = Tag;
            return ret;
        }

        public object Clone() {
            return clone();
        }

        public int CompareTo( VsqEvent item ) {
            int ret = this.Clock - item.Clock;
            if ( ret == 0 ) {
                if ( this.ID != null && item.ID != null ) {
                    return (int)this.ID.type - (int)item.ID.type;
                } else {
                    return ret;
                }
            } else {
                return ret;
            }
        }

        public VsqEvent( String line ) {
            String[] spl = line.Split( new char[] { '=' } );
            Clock = int.Parse( spl[0] );
            if ( spl[1].Equals( "EOS" ) ) {
                ID = VsqID.EOS;
            }
        }

        public VsqEvent()
            : this( 0, new VsqID() ) {
        }

        public VsqEvent( int clock, VsqID id /*, int internal_id*/ ) {
            Clock = clock;
            ID = id;
            //InternalID = internal_id;
            InternalID = 0;
        }
    }

}
