/*
 * VsqMetaText/Handle.cs
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

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;

    [Serializable]
    public class IconHandle : ICloneable {
        public String Caption = "";
        public String IconID = "";
        public String IDS = "";
        public int Index;
        public int Length;
        public int Original;
        public int Program;
        public int Language;

        public IconHandle() {
        }

        public object Clone() {
            IconHandle ret = new IconHandle();
            ret.Caption = Caption;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Index = Index;
            ret.Language = Language;
            ret.Length = Length;
            ret.Original = Original;
            ret.Program = Program;
            return ret;
        }

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Singer;
            ret.Caption = Caption;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Index = Index;
            ret.Language = Language;
            ret.Length = Length;
            ret.Program = Program;
            return ret;
        }
    }

    [Serializable]
    public class LyricHandle : ICloneable {
        public Lyric L0;
        public int Index;

        public LyricHandle() {
        }

        /// <summary>
        /// type = Lyric用のhandleのコンストラクタ
        /// </summary>
        /// <param name="phrase">歌詞</param>
        /// <param name="phonetic_symbol">発音記号</param>
        public LyricHandle( String phrase, String phonetic_symbol ) {
            L0 = new Lyric( phrase, phonetic_symbol );
        }

        public object Clone() {
            LyricHandle ret = new LyricHandle();
            ret.Index = Index;
            ret.L0 = (Lyric)L0.Clone();
            return ret;
        }

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Lyric;
            ret.L0 = (Lyric)L0.Clone();
            ret.Index = Index;
            return ret;
        }
    }


    [Serializable]
    public class VibratoHandle : ICloneable {
        public int StartDepth;
        public VibratoBPList DepthBP;
        public int StartRate;
        public VibratoBPList RateBP;
        public int Index;
        public String IconID = "";
        public String IDS = "";
        public int Original;
        public String Caption = "";
        public int Length;

        public VibratoHandle(){
            StartRate = 64;
            StartDepth = 64;
            RateBP = new VibratoBPList();
            DepthBP = new VibratoBPList();
        }

        public String getDisplayString() {
            String s = IDS;
            if ( !Caption.Equals( "" ) ) {
                s += " (" + Caption + ")";
            }
            return s;
        }

        /*public static VibratoHandle[] fromVED( String ved_file, int original ){
            
        }*/

        public object Clone() {
            return clone();
        }

        public Object clone() {
            VibratoHandle result = new VibratoHandle();
            result.Index = Index;
            result.IconID = IconID;
            result.IDS = this.IDS;
            result.Original = this.Original;
            result.Caption = this.Caption;
            result.Length = this.Length;
            result.StartDepth = this.StartDepth;
            result.DepthBP = (VibratoBPList)DepthBP.Clone();
            result.StartRate = this.StartRate;
            result.RateBP = (VibratoBPList)RateBP.Clone();
            return result;
        }

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Vibrato;
            ret.Index = Index;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.Caption = Caption;
            ret.Length = Length;
            ret.StartDepth = StartDepth;
            ret.StartRate = StartRate;
            ret.DepthBP = (VibratoBPList)DepthBP.Clone();
            ret.RateBP = (VibratoBPList)RateBP.Clone();
            return ret;
        }
    }

    [Serializable]
    public class NoteHeadHandle : ICloneable {
        public int Index;
        public String IconID = "";
        public String IDS = "";
        public int Original;
        public String Caption = "";
        public int Length;
        public int Duration;
        public int Depth;

        public NoteHeadHandle() {
        }

        public String getDisplayString() {
            String s = IDS;
            if ( !Caption.Equals( "" ) ) {
                s += " (" + Caption + ")";
            }
            return s;
        }
        
        public object Clone() {
            return clone();
        }

        public Object clone() {
            NoteHeadHandle result = new NoteHeadHandle();
            result.Index = Index;
            result.IconID = IconID;
            result.IDS = this.IDS;
            result.Original = this.Original;
            result.Caption = this.Caption;
            result.Length = this.Length;
            result.Duration = Duration;
            result.Depth = Depth;
            return result;
        }

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.NoteHeadHandle;
            ret.Index = Index;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.Caption = Caption;
            ret.Length = Length;
            ret.Duration = Duration;
            ret.Depth = Depth;
            return ret;
        }
    }

    /// <summary>
    /// ハンドルを取り扱います。ハンドルにはLyricHandle、VibratoHandle、IconHandleおよびNoteHeadHandleがある
    /// </summary>
    [Serializable]
    public class VsqHandle {
        public VsqHandleType m_type;
        public int Index;
        public String IconID = "";
        public String IDS = "";
        public Lyric L0;
        public int Original;
        public String Caption = "";
        public int Length;
        public int StartDepth;
        public VibratoBPList DepthBP;
        public int StartRate;
        public VibratoBPList RateBP;
        public int Language;
        public int Program;
        public int Duration;
        public int Depth;

        public LyricHandle castToLyricHandle() {
            LyricHandle ret = new LyricHandle();
            ret.L0 = (Lyric)L0;
            ret.Index = Index;
            return ret;
        }

        public VibratoHandle castToVibratoHandle() {
            VibratoHandle ret = new VibratoHandle();
            ret.Index = Index;
            ret.Caption = Caption;
            ret.DepthBP = (VibratoBPList)DepthBP.Clone();
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Index = Index;
            ret.Length = Length;
            ret.Original = Original;
            ret.RateBP = (VibratoBPList)RateBP.Clone();
            ret.StartDepth = StartDepth;
            ret.StartRate = StartRate;
            return ret;
        }

        public IconHandle castToIconHandle() {
            IconHandle ret = new IconHandle();
            ret.Index = Index;
            ret.Caption = Caption;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Index = Index;
            ret.Language = Language;
            ret.Length = Length;
            ret.Original = Original;
            ret.Program = Program;
            return ret;
        }

        public NoteHeadHandle castToNoteHeadHandle() {
            NoteHeadHandle ret = new NoteHeadHandle();
            ret.Caption = Caption;
            ret.Depth = Depth;
            ret.Duration = Duration;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Length = Length;
            ret.Original = Original;
            return ret;
        }

        internal VsqHandle() {
        }
        
        /// <summary>
        /// インスタンスをストリームに書き込みます。
        /// encode=trueの場合、2バイト文字をエンコードして出力します。
        /// </summary>
        /// <param name="sw">書き込み対象</param>
        /// <param name="encode">2バイト文字をエンコードするか否かを指定するフラグ</param>
        private void writeCor( ITextWriter sw, boolean encode ) {
            sw.writeLine( this.ToString( encode ) );
        }

        public void write( TextMemoryStream sw, boolean encode ) {
            writeCor( sw, encode );
        }

        public void write( StreamWriter sw, boolean encode ) {
            writeCor( new WrappedStreamWriter( sw ), encode );
        }

        /// <summary>
        /// FileStreamから読み込みながらコンストラクト
        /// </summary>
        /// <param name="sr">読み込み対象</param>
        public VsqHandle( TextMemoryStream sr, int value, ref String last_line ) {
            this.Index = value;
            String[] spl;
            String[] spl2;

            // default値で梅
            m_type = VsqHandleType.Vibrato;
            IconID = "";
            IDS = "normal";
            L0 = new Lyric( "" );
            Original = 0;
            Caption = "";
            Length = 0;
            StartDepth = 0;
            DepthBP = null;
            int depth_bp_num = 0;
            StartRate = 0;
            RateBP = null;
            int rate_bp_num = 0;
            Language = 0;
            Program = 0;
            Duration = 0;
            Depth = 64;

            String tmpDepthBPX = "";
            String tmpDepthBPY = "";
            String tmpRateBPX = "";
            String tmpRateBPY = "";

            // "["にぶち当たるまで読込む
            last_line = sr.readLine();
            while ( !last_line.StartsWith( "[" ) ) {
                spl = last_line.Split( new char[] { '=' } );
                switch ( spl[0] ) {
                    case "Language":
                        m_type = VsqHandleType.Singer;
                        Language = int.Parse( spl[1] );
                        break;
                    case "Program":
                        Program = int.Parse( spl[1] );
                        break;
                    case "IconID":
                        IconID = spl[1];
                        break;
                    case "IDS":
                        IDS = spl[1];
                        break;
                    case "Original":
                       Original = int.Parse( spl[1] );
                        break;
                    case "Caption":
                        Caption = spl[1];
                        for ( int i = 2; i < spl.Length; i++ ) {
                            Caption += "=" + spl[i];
                        }
                        break;
                    case "Length":
                        Length = int.Parse( spl[1] );
                        break;
                    case "StartDepth":
                        StartDepth = int.Parse( spl[1] );
                        break;
                    case "DepthBPNum":
                        depth_bp_num = int.Parse( spl[1] );
                        break;
                    case "DepthBPX":
                        tmpDepthBPX = spl[1];
                        break;
                    case "DepthBPY":
                        tmpDepthBPY = spl[1];
                        break;
                    case "StartRate":
                        m_type = VsqHandleType.Vibrato;
                        StartRate = int.Parse( spl[1] );
                        break;
                    case "RateBPNum":
                        rate_bp_num = int.Parse( spl[1] );
                        break;
                    case "RateBPX":
                        tmpRateBPX = spl[1];
                        break;
                    case "RateBPY":
                        tmpRateBPY = spl[1];
                        break;
                    case "L0":
                        m_type = VsqHandleType.Lyric;
#if DEBUG
                        Console.WriteLine( "VsqHandle#.ctor(TextMemoryStream,int,ref String); line=" + spl[1] );
#endif
                        L0 = new Lyric( spl[1] );
                        break;
                    case "Duration":
                        m_type = VsqHandleType.NoteHeadHandle;
                        Duration = int.Parse( spl[1] );
                        break;
                    case "Depth":
                        Duration = int.Parse( spl[1] );
                        break;
                }
                if ( sr.peek() < 0 ) {
                    break;
                }
                last_line = sr.readLine();
            }
            /*if ( IDS != "normal" ) {
                m_type = VsqHandleType.Singer;
            } else if ( IconID != "" ) {
                m_type = VsqHandleType.Vibrato;
            } else {
                m_type = VsqHandleType.Lyric;
            }*/

            // RateBPX, RateBPYの設定
            if ( m_type == VsqHandleType.Vibrato ) {
                if ( rate_bp_num > 0 ) {
                    float[] rate_bp_x = new float[rate_bp_num];
                    spl2 = tmpRateBPX.Split( new char[] { ',' } );
                    for ( int i = 0; i < rate_bp_num; i++ ) {
                        rate_bp_x[i] = float.Parse( spl2[i] );
                    }

                    int[] rate_bp_y = new int[rate_bp_num];
                    spl2 = tmpRateBPY.Split( new char[] { ',' } );
                    for ( int i = 0; i < rate_bp_num; i++ ) {
                        rate_bp_y[i] = int.Parse( spl2[i] );
                    }
                    RateBP = new VibratoBPList( rate_bp_x, rate_bp_y );
                } else {
                    //m_rate_bp_x = null;
                    //m_rate_bp_y = null;
                    RateBP = new VibratoBPList();
                }

                // DepthBPX, DepthBPYの設定
                if ( depth_bp_num > 0 ) {
                    float[] depth_bp_x = new float[depth_bp_num];
                    spl2 = tmpDepthBPX.Split( new char[] { ',' } );
                    for ( int i = 0; i < depth_bp_num; i++ ) {
                        depth_bp_x[i] = float.Parse( spl2[i] );
                    }

                    int[] depth_bp_y = new int[depth_bp_num];
                    spl2 = tmpDepthBPY.Split( new char[] { ',' } );
                    for ( int i = 0; i < depth_bp_num; i++ ) {
                        depth_bp_y[i] = int.Parse( spl2[i] );
                    }
                    DepthBP = new VibratoBPList( depth_bp_x, depth_bp_y );
                } else {
                    DepthBP = new VibratoBPList();
                    //m_depth_bp_x = null;
                    //m_depth_bp_y = null;
                }
            } else {
                DepthBP = new VibratoBPList();
                RateBP = new VibratoBPList();
            }
        }

        /// <summary>
        /// ハンドル指定子（例えば"h#0123"という文字列）からハンドル番号を取得します
        /// </summary>
        /// <param name="_string">ハンドル指定子</param>
        /// <returns>ハンドル番号</returns>
        public static int HandleIndexFromString( String _string ) {
            String[] spl = _string.Split( new char[] { '#' } );
            return int.Parse( spl[1] );
        }

        /// <summary>
        /// インスタンスをテキストファイルに出力します
        /// </summary>
        /// <param name="sw">出力先</param>
        public void Print( StreamWriter sw ) {
            String result = this.ToString();
            sw.WriteLine( result );
        }

        /// <summary>
        /// インスタンスをコンソール画面に出力します
        /// </summary>
        private void Print() {
            String result = this.ToString();
            Console.WriteLine( result );
        }

        /// <summary>
        /// インスタンスを文字列に変換します
        /// </summary>
        /// <param name="encode">2バイト文字をエンコードするか否かを指定するフラグ</param>
        /// <returns>インスタンスを変換した文字列</returns>
        public String ToString( boolean encode ) {
            String result = "";
            result += "[h#" + Index.ToString( "0000" ) + "]";
            switch ( m_type ) {
                case VsqHandleType.Lyric:
                    result += Environment.NewLine + "L0=" + L0.ToString( encode );
                    break;
                case VsqHandleType.Vibrato:
                    result += Environment.NewLine + "IconID=" + IconID + Environment.NewLine;
                    result += "IDS=" + IDS + Environment.NewLine;
                    result += "Original=" + Original + Environment.NewLine;
                    result += "Caption=" + Caption + Environment.NewLine;
                    result += "Length=" + Length + Environment.NewLine;
                    result += "StartDepth=" + StartDepth + Environment.NewLine;
                    result += "DepthBPNum=" + DepthBP.getCount() + Environment.NewLine;
                    if ( DepthBP.getCount() > 0 ) {
                        result += "DepthBPX=" + DepthBP.getElement( 0 ).X.ToString( "0.000000" );
                        for ( int i = 1; i < DepthBP.getCount(); i++ ) {
                            result += "," + DepthBP.getElement( i ).X.ToString( "0.000000" );
                        }
                        result += Environment.NewLine + "DepthBPY=" + DepthBP.getElement( 0 ).Y;
                        for ( int i = 1; i < DepthBP.getCount(); i++ ) {
                            result += "," + DepthBP.getElement( i ).Y;
                        }
                        result += Environment.NewLine;
                    }
                    result += "StartRate=" + StartRate + Environment.NewLine;
                    result += "RateBPNum=" + RateBP.getCount();
                    if ( RateBP.getCount() > 0 ) {
                        result += Environment.NewLine + "RateBPX=" + RateBP.getElement( 0 ).X.ToString( "0.000000" );
                        for ( int i = 1; i < RateBP.getCount(); i++ ) {
                            result += "," + RateBP.getElement( i ).X.ToString( "0.000000" );
                        }
                        result += Environment.NewLine + "RateBPY=" + RateBP.getElement( 0 ).Y;
                        for ( int i = 1; i < RateBP.getCount(); i++ ) {
                            result += "," + RateBP.getElement( i ).Y;
                        }
                    }
                    break;
                case VsqHandleType.Singer:
                    result += Environment.NewLine + "IconID=" + IconID + Environment.NewLine;
                    result += "IDS=" + IDS + Environment.NewLine;
                    result += "Original=" + Original + Environment.NewLine;
                    result += "Caption=" + Caption + Environment.NewLine;
                    result += "Length=" + Length + Environment.NewLine;
                    result += "Language=" + Language + Environment.NewLine;
                    result += "Program=" + Program;
                    break;
                case VsqHandleType.NoteHeadHandle:
                    result += Environment.NewLine + "IconID=" + IconID + Environment.NewLine;
                    result += "IDS=" + IDS + Environment.NewLine;
                    result += "Original=" + Original + Environment.NewLine;
                    result += "Caption=" + Caption + Environment.NewLine;
                    result += "Length=" + Length + Environment.NewLine;
                    result += "Duration=" + Duration + Environment.NewLine;
                    result += "Depth=" + Depth;
                    break;
                default:
                    break;
            }
            return result;
        }
    }

}
