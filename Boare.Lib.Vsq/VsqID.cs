/*
* VsqMetaText/ID.cs
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
using System.Collections.Generic;
using System.Text;
using System.IO;

using bocoree;
using Boare.Lib.Vsq;

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;

    /// <summary>
    /// メタテキストに埋め込まれるIDを表すクラス。
    /// </summary>
    [Serializable]
    public class VsqID : ICloneable {
        public const int MAX_NOTE_LENGTH = 16383;
        internal int value;
        public VsqIDType type;
        internal int IconHandle_index;
        public IconHandle IconHandle;
        private int m_length;
        public int Note;
        public int Dynamics;
        public int PMBendDepth;
        public int PMBendLength;
        public int PMbPortamentoUse;
        public int DEMdecGainRate;
        public int DEMaccent;
        internal int LyricHandle_index;
        public LyricHandle LyricHandle;
        internal int VibratoHandle_index;
        public VibratoHandle VibratoHandle;
        public int VibratoDelay;
        internal int NoteHeadHandle_index;
        public NoteHeadHandle NoteHeadHandle;
        public int pMeanOnsetFirstNote = 0x0a;
        public int vMeanNoteTransition = 0x0c;
        public int d4mean = 0x18;
        public int pMeanEndingNote = 0x0c;

        public static VsqID EOS = new VsqID( -1 );

        public int Length {
            get {
                return m_length;
            }
            set {
                m_length = value;
            }
        }

        /// <summary>
        /// このインスタンスの簡易コピーを取得します。
        /// </summary>
        /// <returns>このインスタンスの簡易コピー</returns>
        public Object clone() {
            VsqID result = new VsqID( this.value );
            result.type = this.type;
            if ( this.IconHandle != null ) {
                result.IconHandle = (IconHandle)this.IconHandle.Clone();
            }
            result.Length = this.Length;
            result.Note = this.Note;
            result.Dynamics = this.Dynamics;
            result.PMBendDepth = this.PMBendDepth;
            result.PMBendLength = this.PMBendLength;
            result.PMbPortamentoUse = this.PMbPortamentoUse;
            result.DEMdecGainRate = this.DEMdecGainRate;
            result.DEMaccent = this.DEMaccent;
            result.d4mean = this.d4mean;
            result.pMeanOnsetFirstNote = this.pMeanOnsetFirstNote;
            result.vMeanNoteTransition = this.vMeanNoteTransition;
            result.pMeanEndingNote = this.pMeanEndingNote;
            if ( this.LyricHandle != null ) {
                result.LyricHandle = (LyricHandle)this.LyricHandle.Clone();
            }
            if ( this.VibratoHandle != null ) {
                result.VibratoHandle = (VibratoHandle)this.VibratoHandle.Clone();
            }
            result.VibratoDelay = this.VibratoDelay;
            if ( NoteHeadHandle != null ) {
                result.NoteHeadHandle = (NoteHeadHandle)NoteHeadHandle.Clone();
            }
            return result;
        }

        public object Clone() {
            return clone();
        }

        /// <summary>
        /// IDの番号（ID#****の****）を指定したコンストラクタ。
        /// </summary>
        /// <param name="a_value">IDの番号</param>
        public VsqID( int a_value ) {
            value = a_value;
        }

        public VsqID()
            : this( 0 ) {
        }

        /// <summary>
        /// テキストファイルからのコンストラクタ
        /// </summary>
        /// <param name="sr">読み込み対象</param>
        /// <param name="value"></param>
        /// <param name="last_line">読み込んだ最後の行が返されます</param>
        public VsqID( TextMemoryStream sr, int value, ref String last_line ) {
            String[] spl;
            this.value = value;
            this.type = VsqIDType.Unknown;
            this.IconHandle_index = -2;
            this.LyricHandle_index = -1;
            this.VibratoHandle_index = -1;
            this.NoteHeadHandle_index = -1;
            this.Length = 0;
            this.Note = 0;
            this.Dynamics = 0;
            this.PMBendDepth = 0;
            this.PMBendLength = 0;
            this.PMbPortamentoUse = 0;
            this.DEMdecGainRate = 0;
            this.DEMaccent = 0;
            //this.LyricHandle_index = -2;
            //this.VibratoHandle_index = -2;
            this.VibratoDelay = 0;
            last_line = sr.readLine();
            while ( !last_line.StartsWith( "[" ) ) {
                spl = PortUtil.splitString( last_line, new char[] { '=' } );
                switch ( spl[0] ) {
                    case "Type":
                        if ( spl[1].Equals( "Anote" ) ) {
                            type = VsqIDType.Anote;
                        } else if ( spl[1].Equals( "Singer" ) ) {
                            type = VsqIDType.Singer;
                        } else {
                            type = VsqIDType.Unknown;
                        }
                        break;
                    case "Length":
                        this.Length = PortUtil.parseInt( spl[1] );
                        break;
                    case "Note#":
                        this.Note = PortUtil.parseInt( spl[1] );
                        break;
                    case "Dynamics":
                        this.Dynamics = PortUtil.parseInt( spl[1] );
                        break;
                    case "PMBendDepth":
                        this.PMBendDepth = PortUtil.parseInt( spl[1] );
                        break;
                    case "PMBendLength":
                        this.PMBendLength = PortUtil.parseInt( spl[1] );
                        break;
                    case "DEMdecGainRate":
                        this.DEMdecGainRate = PortUtil.parseInt( spl[1] );
                        break;
                    case "DEMaccent":
                        this.DEMaccent = PortUtil.parseInt( spl[1] );
                        break;
                    case "LyricHandle":
                        this.LyricHandle_index = VsqHandle.HandleIndexFromString( spl[1] );
                        break;
                    case "IconHandle":
                        this.IconHandle_index = VsqHandle.HandleIndexFromString( spl[1] );
                        break;
                    case "VibratoHandle":
                        this.VibratoHandle_index = VsqHandle.HandleIndexFromString( spl[1] );
                        break;
                    case "VibratoDelay":
                        this.VibratoDelay = PortUtil.parseInt( spl[1] );
                        break;
                    case "PMbPortamentoUse":
                        PMbPortamentoUse = PortUtil.parseInt( spl[1] );
                        break;
                    case "NoteHeadHandle":
                        NoteHeadHandle_index = VsqHandle.HandleIndexFromString( spl[1] );
                        break;

                }
                if ( sr.peek() < 0 ) {
                    break;
                }
                last_line = sr.readLine();
            }
        }

        public override String ToString() {
            String ret = "{Type=" + type;
            switch ( type ) {
                case VsqIDType.Anote:
                    ret += ", Length=" + Length;
                    ret += ", Note#=" + Note;
                    ret += ", Dynamics=" + Dynamics;
                    ret += ", PMBendDepth=" + PMBendDepth;
                    ret += ", PMBendLength=" + PMBendLength;
                    ret += ", PMbPortamentoUse=" + PMbPortamentoUse;
                    ret += ", DEMdecGainRate=" + DEMdecGainRate;
                    ret += ", DEMaccent=" + DEMaccent;
                    if ( LyricHandle != null ) {
                        ret += ", LyricHandle=h#" + LyricHandle_index.ToString( "0000" );
                    }
                    if ( VibratoHandle != null ) {
                        ret += ", VibratoHandle=h#" + VibratoHandle_index.ToString( "0000" );
                        ret += ", VibratoDelay=" + VibratoDelay;
                    }
                    break;
                case VsqIDType.Singer:
                    ret += ", IconHandle=h#" + IconHandle_index.ToString( "0000" );
                    break;
            }
            ret += "}";
            return ret;
        }

        /* /// <summary>
        /// インスタンスをテキストファイルに出力します
        /// </summary>
        /// <param name="sw">出力先</param>
        public void write( TextMemoryStream sw ) {
            sw.writeLine( "[ID#" + value.ToString( "0000" ) + "]" );
            sw.writeLine( "Type=" + type );
            switch( type ){
                case VsqIDType.Anote:
                    sw.writeLine( "Length=" + Length );
                    sw.writeLine( "Note#=" + Note );
                    sw.writeLine( "Dynamics=" + Dynamics );
                    sw.writeLine( "PMBendDepth=" + PMBendDepth );
                    sw.writeLine( "PMBendLength=" + PMBendLength );
                    sw.writeLine( "PMbPortamentoUse=" + PMbPortamentoUse );
                    sw.writeLine( "DEMdecGainRate=" + DEMdecGainRate );
                    sw.writeLine( "DEMaccent=" + DEMaccent );
                    if ( LyricHandle != null ) {
                        sw.writeLine( "LyricHandle=h#" + LyricHandle_index.ToString( "0000" ) );
                    }
                    if ( VibratoHandle != null ) {
                        sw.writeLine( "VibratoHandle=h#" + VibratoHandle_index.ToString( "0000" ) );
                        sw.writeLine( "VibratoDelay=" + VibratoDelay );
                    }
                    if ( NoteHeadHandle != null ) {
                        sw.writeLine( "NoteHeadHandle=h#" + NoteHeadHandle_index.ToString( "0000" ) );
                    }
                    break;
                case VsqIDType.Singer:
                    sw.writeLine( "IconHandle=h#" + IconHandle_index.ToString( "0000" ) );
                    break;
            }
        }*/

        /// <summary>
        /// VsqIDを構築するテストを行います。
        /// </summary>
        /// <returns>テストに成功すればtrue、そうでなければfalseを返します</returns>
        public static boolean test() {
            String fpath = Path.GetTempFileName();
            using ( StreamWriter sw = new StreamWriter( fpath, false, Encoding.Unicode ) ) {
                sw.WriteLine( "Type=Anote" );
                sw.WriteLine( "Length=320" );
                sw.WriteLine( "Note#=67" );
                sw.WriteLine( "Dynamics=64" );
                sw.WriteLine( "PMBendDepth=8" );
                sw.WriteLine( "PMBendLength=1" );
                sw.WriteLine( "PMbPortamentoUse=1" );
                sw.WriteLine( "DEMdecGainRate=50" );
                sw.WriteLine( "DEMaccent=50" );
                sw.WriteLine( "LyricHandle=h#0111" );
                sw.WriteLine( "[ID#0104]" );
            }

            String last_line = "";
            boolean result;
            using ( TextMemoryStream sr = new TextMemoryStream( fpath, Encoding.Unicode ) ) {
                VsqID vsqID = new VsqID( sr, 103, ref last_line );
                if ( vsqID.type == VsqIDType.Anote &&
                    vsqID.Length == 320 &&
                    vsqID.Note == 67 &&
                    vsqID.Dynamics == 64 &&
                    vsqID.PMBendDepth == 8 &&
                    vsqID.PMBendLength == 1 &&
                    vsqID.PMbPortamentoUse == 1 &&
                    vsqID.DEMdecGainRate == 50 &&
                    vsqID.DEMaccent == 50 &&
                    vsqID.LyricHandle_index == 111 &&
                    last_line.Equals( "[ID#0104]" ) ) {
                    result = true;
                } else {
                    result = false;
                }
            }
            PortUtil.deleteFile( fpath );
            return result;
        }

    }

}
