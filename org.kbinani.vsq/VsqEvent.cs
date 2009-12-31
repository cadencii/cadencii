/*
 * VsqEvent.cs
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.io.*;
import java.util.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.java.io;

namespace org.kbinani.vsq {
#endif

    /// <summary>
    /// vsqファイルのメタテキスト内に記述されるイベント。
    /// </summary>
#if JAVA
    public class VsqEvent implements Comparable<VsqEvent>, Cloneable, Serializable {
#else
    [Serializable]
    public class VsqEvent : IComparable<VsqEvent>, ICloneable {
#endif
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
        public void write( TextMemoryStream sw )
#if JAVA
            throws IOException
#endif
        {
            Vector<String> def = new Vector<String>( Arrays.asList( new String[]{ "Length",
                                                                   "Note#",
                                                                   "Dynamics",
                                                                   "PMBendDepth",
                                                                   "PMBendLength",
                                                                   "PMbPortamentoUse",
                                                                   "DEMdecGainRate",
                                                                   "DEMaccent" } ) );
            write( sw, def );
        }

        public void write( TextMemoryStream writer, Vector<String> print_targets )
#if JAVA
            throws IOException
#endif
        {
            writeCor( writer, print_targets );
        }

        public void write( BufferedWriter writer, Vector<String> print_targets )
#if JAVA
            throws IOException
#endif
        {
            writeCor( new WrappedStreamWriter( writer ), print_targets );
        }

        private void writeCor( ITextWriter writer, Vector<String> print_targets )
#if JAVA
            throws IOException
#endif
        {
            writer.writeLine( "[ID#" + PortUtil.formatDecimal( "0000", ID.value ) + "]" );
            writer.writeLine( "Type=" + ID.type );
            if ( ID.type == VsqIDType.Anote ) {
                if ( print_targets.contains( "Length" ) ) writer.writeLine( "Length=" + ID.getLength() );
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
                    writer.writeLine( "LyricHandle=h#" + PortUtil.formatDecimal( "0000", ID.LyricHandle_index ) );
                }
                if ( ID.VibratoHandle != null ) {
                    writer.writeLine( "VibratoHandle=h#" + PortUtil.formatDecimal( "0000", ID.VibratoHandle_index ) );
                    writer.writeLine( "VibratoDelay=" + ID.VibratoDelay );
                }
                if ( ID.NoteHeadHandle != null ) {
                    writer.writeLine( "NoteHeadHandle=h#" + PortUtil.formatDecimal( "0000", ID.NoteHeadHandle_index ) );
                }
            } else if ( ID.type == VsqIDType.Singer ) {
                writer.writeLine( "IconHandle=h#" + PortUtil.formatDecimal( "0000", ID.IconHandle_index ) );
            } else if ( ID.type == VsqIDType.Aicon ) {
                writer.writeLine( "IconHandle=h#" + PortUtil.formatDecimal( "0000", ID.IconHandle_index ) );
                writer.writeLine( "Note#" + ID.Note );
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
                ret.UstEvent = (UstEvent)UstEvent.clone();
            }
            ret.Tag = Tag;
            return ret;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

#if !JAVA
        public int CompareTo( VsqEvent item ) {
            return compareTo( item );
        }
#endif

        public int compareTo( VsqEvent item ) {
            int ret = this.Clock - item.Clock;
            if ( ret == 0 ) {
                if ( this.ID != null && item.ID != null ) {
#if JAVA
                    return this.ID.type.ordinal() - item.ID.type.ordinal();
#else
                    return (int)this.ID.type - (int)item.ID.type;
#endif
                } else {
                    return ret;
                }
            } else {
                return ret;
            }
        }

        public VsqEvent( String line ) {
            String[] spl = PortUtil.splitString( line, new char[] { '=' } );
            Clock = PortUtil.parseInt( spl[0] );
            if ( spl[1].Equals( "EOS" ) ) {
                ID = VsqID.EOS;
            }
        }

#if JAVA
        public VsqEvent(){
            this( 0, new VsqID() );
#else
        public VsqEvent()
            : this( 0, new VsqID() ) {
#endif
        }

        public VsqEvent( int clock, VsqID id /*, int internal_id*/ ) {
            Clock = clock;
            ID = id;
            //InternalID = internal_id;
            InternalID = 0;
        }
    }

#if !JAVA
}
#endif
