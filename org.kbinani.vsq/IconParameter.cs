/*
 * IconParameter.cs
 * Copyright © 2009-2011 kbinani
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
import org.kbinani.*;
#else
using System;
using org.kbinani.java.io;

namespace org.kbinani.vsq
{
#endif

    /// <summary>
    /// アイコン設定ファイルである*.AICファイルを読み取ることで作成されるアイコン設定を表します。
    /// アイコン設定ファイルを使用するIconDynamicsHandle、NoteHeadHandle、およびVibratoHandleの基底クラスとなっています。
    /// <see cref="T:org.kbinani.vsq.IconDynamicsHandle"/>
    /// <see cref="T:org.kbinani.vsq.NoteHeadHandle"/>
    /// <see cref="T:org.kbinani.vsq.VibratoHandle"/>
    /// </summary>
#if JAVA
    public class IconParameter implements Serializable {
#else
    [Serializable]
    public class IconParameter
    {
#endif
        /// <summary>
        /// アイコン設定の種類を表します。
        /// </summary>
        public enum ArticulationType
        {
            /// <summary>
            /// ビブラート
            /// </summary>
            Vibrato,
            /// <summary>
            /// クレッシェンド、またはデクレッシェンド
            /// </summary>
            Crescendo,
            /// <summary>
            /// ピアノ、フォルテ等の強弱記号
            /// </summary>
            Dynaff,
            /// <summary>
            /// アタック
            /// </summary>
            NoteAttack,
            /// <summary>
            /// NoteTransition(詳細不明)
            /// </summary>
            NoteTransition,
        }

        /// <summary>
        /// アイコン設定の種類
        /// </summary>
        protected ArticulationType articulation;
        /// <summary>
        /// アイコンのボタンに使用される画像ファイルへの相対パス
        /// </summary>
        protected String button = "";
        /// <summary>
        /// キャプション
        /// </summary>
        protected String caption = "";

        /// <summary>
        /// ゲートタイム長さ
        /// </summary>
        protected int length;
        /// <summary>
        /// ビブラート深さの開始値
        /// </summary>
        protected int startDepth;
        /// <summary>
        /// ビブラート深さの終了値
        /// </summary>
        protected int endDepth;
        /// <summary>
        /// ビブラート速さの開始値
        /// </summary>
        protected int startRate;
        /// <summary>
        /// ビブラート速さの終了値
        /// </summary>
        protected int endRate;
        protected int startDyn;
        protected int endDyn;
        protected int duration;
        protected int depth;
        protected VibratoBPList dynBP;
        protected VibratoBPList depthBP;
        protected VibratoBPList rateBP;

        protected String buttonImageFullPath = "";

        protected IconParameter()
        {
        }

        protected IconParameter( String file )
        {
            if ( file == null ) {
                return;
            }
            if ( file.Equals( "" ) ) {
                return;
            }
            BufferedReader sr = null;
            try {
                sr = new BufferedReader( new InputStreamReader( new FileInputStream( file ), "Shift_JIS" ) );
                String line = "";
                String strDynBPNum = "";
                String strDynBPX = "";
                String strDynBPY = "";
                String strDepthBPNum = "";
                String strDepthBPX = "";
                String strDepthBPY = "";
                String strRateBPNum = "";
                String strRateBPX = "";
                String strRateBPY = "";
                while ( (line = sr.readLine()) != null ) {
                    // コメントを除去する
                    int indx_colon = line.IndexOf( ';' );
                    if ( indx_colon >= 0 ) {
                        line = str.sub( line, 0, indx_colon );
                    }
                    // セクション名の指定行
                    if ( line.StartsWith( "[" ) ) {
                        continue;
                    }
                    // イコールが含まれているかどうか
                    String[] spl = PortUtil.splitString( line, new char[] { '=' }, 2 );
                    if ( spl.Length != 2 ) {
                        continue;
                    }
                    String name = spl[0].Trim();// new char[]{ ' ', '\t' } );
                    String value = spl[1].Trim();// new char[]{ ' ', '\t' } );
                    if ( name.Equals( "Articulation" ) ) {
                        if ( value.Equals( "Vibrato" ) ) {
                            articulation = ArticulationType.Vibrato;
                        } else if ( value.Equals( "Crescendo" ) ) {
                            articulation = ArticulationType.Crescendo;
                        } else if ( value.Equals( "Dynaff" ) ) {
                            articulation = ArticulationType.Dynaff;
                        } else if ( value.Equals( "NoteAttack" ) ) {
                            articulation = ArticulationType.NoteAttack;
                        } else if ( value.Equals( "NoteTransition" ) ) {
                            articulation = ArticulationType.NoteTransition;
                        }
                    } else if ( name.Equals( "Button" ) ) {
                        button = value;
                    } else if ( name.Equals( "Caption" ) ) {
                        caption = value;
                    } else if ( name.Equals( "Length" ) ) {
                        try {
                            length = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "StartDepth" ) ) {
                        try {
                            startDepth = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "EndDepth" ) ) {
                        try {
                            endDepth = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "StartRate" ) ) {
                        try {
                            startRate = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "EndRate" ) ) {
                        try {
                            endRate = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "StartDyn" ) ) {
                        try {
                            startDyn = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "EndDyn" ) ) {
                        try {
                            endDyn = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "Duration" ) ) {
                        try {
                            duration = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "Depth" ) ) {
                        try {
                            depth = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "DynBPNum" ) ) {
                        strDynBPNum = value;
                    } else if ( name.Equals( "DynBPX" ) ) {
                        strDynBPX = value;
                    } else if ( name.Equals( "DynBPY" ) ) {
                        strDynBPY = value;
                    } else if ( name.Equals( "DepthBPNum" ) ) {
                        strDepthBPNum = value;
                    } else if ( name.Equals( "DepthBPX" ) ) {
                        strDepthBPX = value;
                    } else if ( name.Equals( "DepthBPY" ) ) {
                        strDepthBPY = value;
                    } else if ( name.Equals( "RateBPNum" ) ) {
                        strRateBPNum = value;
                    } else if ( name.Equals( "RateBPX" ) ) {
                        strRateBPX = value;
                    } else if ( name.Equals( "RateBPY" ) ) {
                        strRateBPY = value;
                    }
                }
                dynBP = getBPListFromText( strDynBPNum, strDynBPX, strDynBPY );
                rateBP = getBPListFromText( strRateBPNum, strRateBPX, strRateBPY );
                depthBP = getBPListFromText( strDepthBPNum, strDepthBPX, strDepthBPY );
            } catch ( Exception ex ) {
                serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
            } finally {
                if ( sr != null ) {
                    try {
                        sr.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        /// <summary>
        /// テキストデータからVibratoBPListを構築する
        /// </summary>
        /// <param name="strNum"></param>
        /// <param name="strBPX"></param>
        /// <param name="strBPY"></param>
        /// <returns></returns>
        private static VibratoBPList getBPListFromText( String strNum, String strBPX, String strBPY )
        {
            VibratoBPList ret = null;
            if ( strNum == null || (strNum != null && strNum.Equals( "" )) ) {
                return ret;
            }
            int num = 0;
            try {
                num = PortUtil.parseInt( strNum );
            } catch ( Exception ex ) {
                serr.println( "org.kbinani.vsq.IconParameter.getBPListFromText; ex=" + ex );
                num = 0;
            }
            String[] sx = PortUtil.splitString( strBPX, ',' );
            String[] sy = PortUtil.splitString( strBPY, ',' );
            int actNum = Math.Min( num, Math.Min( sx.Length, sy.Length ) );
            if ( actNum > 0 ) {
                float[] x = new float[actNum];
                int[] y = new int[actNum];
                for ( int i = 0; i < actNum; i++ ) {
                    try {
                        x[i] = PortUtil.parseFloat( sx[i] );
                        y[i] = PortUtil.parseInt( sy[i] );
                    } catch ( Exception ex ) {
                        serr.println( "org.kbinani.vsq.IconParameter.getBPListFromText; ex=" + ex );
                    }
                }
                ret = new VibratoBPList( x, y );
            }
            return ret;
        }

        public String getButton()
        {
            return button;
        }

        public String getButtonImageFullPath()
        {
            return buttonImageFullPath;
        }

        public void setButtonImageFullPath( String value )
        {
            buttonImageFullPath = value;
        }
    }

#if !JAVA
}
#endif
