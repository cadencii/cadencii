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
package com.github.cadencii.vsq;

import java.io.*;
import com.github.cadencii.*;
#else
using System;
using com.github.cadencii.java.io;

namespace com.github.cadencii.vsq
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
    public class IconParameter implements Serializable
#else
    [Serializable]
    public class IconParameter
#endif
    {
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
            if ( str.compare( file, "" ) ) {
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
                    if ( str.compare( name, "Articulation" ) ) {
                        if ( str.compare( value, "Vibrato" ) ) {
                            articulation = ArticulationType.Vibrato;
                        } else if ( str.compare( value, "Crescendo" ) ) {
                            articulation = ArticulationType.Crescendo;
                        } else if ( str.compare( value, "Dynaff" ) ) {
                            articulation = ArticulationType.Dynaff;
                        } else if ( str.compare( value, "NoteAttack" ) ) {
                            articulation = ArticulationType.NoteAttack;
                        } else if ( str.compare( value, "NoteTransition" ) ) {
                            articulation = ArticulationType.NoteTransition;
                        }
                    } else if ( str.compare( name, "Button" ) ) {
                        button = value;
                    } else if ( str.compare( name, "Caption" ) ) {
                        caption = value;
                    } else if ( str.compare( name, "Length" ) ) {
                        try {
                            length = str.toi( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( str.compare( name, "StartDepth" ) ) {
                        try {
                            startDepth = str.toi( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( str.compare( name, "EndDepth" ) ) {
                        try {
                            endDepth = str.toi( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( str.compare( name, "StartRate" ) ) {
                        try {
                            startRate = str.toi( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( str.compare( name, "EndRate" ) ) {
                        try {
                            endRate = str.toi( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( str.compare( name, "StartDyn" ) ) {
                        try {
                            startDyn = str.toi( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( str.compare( name, "EndDyn" ) ) {
                        try {
                            endDyn = str.toi( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( str.compare( name, "Duration" ) ) {
                        try {
                            duration = str.toi( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( str.compare( name, "Depth" ) ) {
                        try {
                            depth = str.toi( value );
                        } catch ( Exception ex ) {
                            serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( str.compare( name, "DynBPNum" ) ) {
                        strDynBPNum = value;
                    } else if ( str.compare( name, "DynBPX" ) ) {
                        strDynBPX = value;
                    } else if ( str.compare( name, "DynBPY" ) ) {
                        strDynBPY = value;
                    } else if ( str.compare( name, "DepthBPNum" ) ) {
                        strDepthBPNum = value;
                    } else if ( str.compare( name, "DepthBPX" ) ) {
                        strDepthBPX = value;
                    } else if ( str.compare( name, "DepthBPY" ) ) {
                        strDepthBPY = value;
                    } else if ( str.compare( name, "RateBPNum" ) ) {
                        strRateBPNum = value;
                    } else if ( str.compare( name, "RateBPX" ) ) {
                        strRateBPX = value;
                    } else if ( str.compare( name, "RateBPY" ) ) {
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
            if ( strNum == null || (strNum != null && str.compare( strNum, "" )) ) {
                return ret;
            }
            int num = 0;
            try {
                num = str.toi( strNum );
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
                        x[i] = (float)str.tof( sx[i] );
                        y[i] = str.toi( sy[i] );
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
