/*
 * IconParameter.js
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.IconParameter == undefined ){

    /// <summary>
    /// アイコン設定ファイルである*.AICファイルを読み取ることで作成されるアイコン設定を表します。
    /// アイコン設定ファイルを使用するIconDynamicsHandle、NoteHeadHandle、およびVibratoHandleの基底クラスとなっています。
    /// <see cref="T:org.kbinani.vsq.IconDynamicsHandle"/>
    /// <see cref="T:org.kbinani.vsq.NoteHeadHandle"/>
    /// <see cref="T:org.kbinani.vsq.VibratoHandle"/>
    /// </summary>

    org.kbinani.vsq.IconParameter = function(){
        /// <summary>
        /// アイコン設定の種類
        /// [ArticulationType]
        /// </summary>
        this.articulation = org.kbinani.vsq.ArticulationType.Dynaff;
        /// <summary>
        /// アイコンのボタンに使用される画像ファイルへの相対パス
        /// </summary>
        this.button = "";
        /// <summary>
        /// キャプション
        /// </summary>
        this.caption = "";

        /// <summary>
        /// ゲートタイム長さ
        /// </summary>
        this.length = 0;
        /// <summary>
        /// ビブラート深さの開始値
        /// </summary>
        this.startDepth = 64;
        /// <summary>
        /// ビブラート深さの終了値
        /// </summary>
        this.endDepth = 64;
        /// <summary>
        /// ビブラート速さの開始値
        /// </summary>
        this.startRate = 64;
        /// <summary>
        /// ビブラート速さの終了値
        /// </summary>
        this.endRate = 64;
        this.startDyn = 64;
        this.endDyn = 64;
        this.duration = 1;
        this.depth = 64;
        this.dynBP = null;
        this.depthBP = null;
        this.rateBP = null;
        this.buttonImageFullPath = "";
    };

    /*protected IconParameter( String file ) {
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
                    line = line.Substring( 0, indx_colon );
                }
                // セクション名の指定行
                if ( line.StartsWith( "[" ) ) {
                    continue;
                }
                // イコールが含まれているかどうか
                String[] spl = PortUtil.splitString( line, new char[]{ '=' }, 2 );
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
                        PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                    }
                } else if ( name.Equals( "StartDepth" ) ) {
                    try {
                        startDepth = PortUtil.parseInt( value );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                    }
                } else if ( name.Equals( "EndDepth" ) ) {
                    try {
                        endDepth = PortUtil.parseInt( value );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                    }
                } else if ( name.Equals( "StartRate" ) ) {
                    try {
                        startRate = PortUtil.parseInt( value );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                    }
                } else if ( name.Equals( "EndRate" ) ) {
                    try {
                        endRate = PortUtil.parseInt( value );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                    }
                } else if ( name.Equals( "StartDyn" ) ) {
                    try {
                        startDyn = PortUtil.parseInt( value );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                    }
                } else if ( name.Equals( "EndDyn" ) ) {
                    try {
                        endDyn = PortUtil.parseInt( value );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                    }
                } else if ( name.Equals( "Duration" ) ) {
                    try {
                        duration = PortUtil.parseInt( value );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                    }
                } else if ( name.Equals( "Depth" ) ) {
                    try {
                        depth = PortUtil.parseInt( value );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
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
            if ( !strDynBPNum.Equals( "" ) ) {
                int num = 0;
                try {
                    num = PortUtil.parseInt( strDynBPNum );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                    num = 0;
                }
                String[] strBPX = PortUtil.splitString( strDynBPX, ',' );
                String[] strBPY = PortUtil.splitString( strDynBPY, ',' );
                int actNum = Math.Min( num, Math.Min( strBPX.Length, strBPY.Length ) );
                if ( actNum > 0 ) {
                    float[] x = new float[actNum];
                    int[] y = new int[actNum];
                    for ( int i = 0; i < actNum; i++ ) {
                        try {
                            x[i] = PortUtil.parseFloat( strBPX[i] );
                            y[i] = PortUtil.parseInt( strBPY[i] );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    }
                    dynBP = new VibratoBPList( x, y );
                }
            }
        } catch ( Exception ex ) {
            PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
        }
    }*/

    org.kbinani.vsq.IconParameter.prototype = {
        /**
         * @return [String]
         */
        getButton : function() {
            return this.button;
        },

        /**
         * @return [String]
         */
        getButtonImageFullPath : function() {
            return this.buttonImageFullPath;
        },

        /**
         * @param value [String]
         * @return [void]
         */
        setButtonImageFullPath : function( value ) {
            this.buttonImageFullPath = value;
        },
    };

    /// <summary>
    /// アイコン設定の種類を表します。
    /// </summary>
    org.kbinani.vsq.ArticulationType = {};

    /// <summary>
    /// ビブラート
    /// </summary>
    org.kbinani.vsq.ArticulationType.Vibrato = 0;
    /// <summary>
    /// クレッシェンド、またはデクレッシェンド
    /// </summary>
    org.kbinani.vsq.ArticulationType.Crescendo = 1;
    /// <summary>
    /// ピアノ、フォルテ等の強弱記号
    /// </summary>
    org.kbinani.vsq.ArticulationType.Dynaff = 2;
    /// <summary>
    /// アタック
    /// </summary>
    org.kbinani.vsq.ArticulationType.NoteAttack = 3;
    /// <summary>
    /// NoteTransition(詳細不明)
    /// </summary>
    org.kbinani.vsq.ArticulationType.NoteTransition = 4;

}
