/*
 * VsqHandle.js
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.VsqHandle == undefined ){

    /// <summary>
    /// ハンドルを取り扱います。ハンドルにはLyricHandle、VibratoHandle、IconHandleおよびNoteHeadHandleがある
    /// </summary>

    org.kbinani.vsq.VsqHandle = function(){
        this.m_type = org.kbinani.vsq.VsqHandleType.Lyric;
        this.Index = 0;
        this.IconID = "";
        this.IDS = "";
        this.L0 = null;
        this.Trailing = new Array();
        this.Original = 0;
        this.Caption = "";
        this.Length = 0;
        this.StartDepth = 0;
        this.DepthBP = null;
        this.StartRate = 0;
        this.RateBP = null;
        this.Language = 0;
        this.Program = 0;
        this.Duration = 0;
        this.Depth = 0;
        this.StartDyn = 0;
        this.EndDyn = 0;
        this.DynBP = null;
        /// <summary>
        /// 歌詞・発音記号列の前後にクォーテーションマークを付けるかどうか
        /// </summary>
        this.addQuotationMark = true;
        if( arguments.length == 3 ){
            this._init_3( arguments[0], arguments[1], arguments[2] );
        }
    };

    /**
     * ハンドル指定子（例えば"h#0123"という文字列）からハンドル番号を取得します
     *
     * @param _string [string] ハンドル指定子
     * @return [int] ハンドル番号
     */
    org.kbinani.vsq.VsqHandle.handleIndexFromString = function( _string ) {
        var spl = _string.split( "#" );
        return parseInt( spl[1], 10 );
    };

    org.kbinani.vsq.VsqHandle.prototype = {
        /**
         * @return [int]
         */
        getLength : function() {
            return this.Length;
        },

        /**
         * @param value [int]
         * @return [void]
         */
        setLength : function( value ) {
            this.Length = value;
        },

        /**
         * @return [LyricHandle]
         */
        castToLyricHandle : function() {
            var ret = new org.kbinani.vsq.LyricHandle();
            ret.L0 = this.L0;
            ret.Index = this.Index;
            ret.Trailing = this.Trailing;
            return ret;
        },

        /**
         * @return [VsqHandle]
         */
        castToVibratoHandle : function() {
            var ret = new org.kbinani.vsq.VibratoHandle();
            ret.Index = this.Index;
            ret.setCaption( this.Caption );
            ret.setDepthBP( this.DepthBP.clone() );
            ret.IconID = this.IconID;
            ret.IDS = this.IDS;
            ret.Index = this.Index;
            ret.setLength( this.Length );
            ret.Original = this.Original;
            ret.setRateBP( this.RateBP.clone() );
            ret.setStartDepth( this.StartDepth );
            ret.setStartRate( this.StartRate );
            return ret;
        },

        /**
         * @return [IconHandle]
         */
        castToIconHandle : function() {
            var ret = new org.kbinani.vsq.IconHandle();
            ret.Index = this.Index;
            ret.Caption = this.Caption;
            ret.IconID = this.IconID;
            ret.IDS = this.IDS;
            ret.Index = this.Index;
            ret.Language = this.Language;
            ret.setLength( this.Length );
            ret.Original = this.Original;
            ret.Program = this.Program;
            return ret;
        },

        /**
         * @return [NoteHeadHandle]
         */
        castToNoteHeadHandle : function() {
            var ret = new org.kbinani.vsq.NoteHeadHandle();
            ret.setCaption( this.Caption );
            ret.setDepth( this.Depth );
            ret.setDuration( this.Duration );
            ret.IconID = this.IconID;
            ret.IDS = this.IDS;
            ret.setLength( this.getLength() );
            ret.Original = this.Original;
            return ret;
        },

        /**
         * @return [IconDynamicsHandle]
         */
        castToIconDynamicsHandle : function() {
            var ret = new org.kbinani.vsq.IconDynamicsHandle();
            ret.IDS = this.IDS;
            ret.IconID = this.IconID;
            ret.Original = this.Original;
            ret.setCaption( this.Caption );
            ret.setDynBP( this.DynBP );
            ret.setEndDyn( this.EndDyn );
            ret.setLength( this.getLength() );
            ret.setStartDyn( this.StartDyn );
            return ret;
        },

        /*
        TODO: VsqHandle#write
        /// <summary>
        /// インスタンスをストリームに書き込みます。
        /// encode=trueの場合、2バイト文字をエンコードして出力します。
        /// </summary>
        /// <param name="sw">書き込み対象</param>
        public void write( ITextWriter sw )
#if JAVA
            throws IOException
#endif
        {
            sw.writeLine( this.toString() );
        }

        public void write( BufferedWriter sw )
#if JAVA
            throws IOException
#endif
        {
            write( new WrappedStreamWriter( sw ) );
        }*/

        /**
         * FileStreamから読み込みながらコンストラクト
         * @param sr [TextStream]読み込み対象
         * @param index [int]
         * @param last_line [ByRef<string>]
         */
        _init_3 : function( sr, index, last_line ) {
            this.Index = index;
            var spl;
            var spl2;

            // default値で梅
            this.m_type = org.kbinani.vsq.VsqHandleType.Vibrato;
            this.IconID = "";
            this.IDS = "normal";
            this.L0 = new org.kbinani.vsq.Lyric( "" );
            this.Original = 0;
            this.Caption = "";
            this.Length = 0;
            this.StartDepth = 0;
            this.DepthBP = null;
            this.StartRate = 0;
            this.RateBP = null;
            this.Language = 0;
            this.Program = 0;
            this.Duration = 0;
            this.Depth = 64;

            var tmpDepthBPX = "";
            var tmpDepthBPY = "";
            var tmpDepthBPNum = "";

            var tmpRateBPX = "";
            var tmpRateBPY = "";
            var tmpRateBPNum = "";

            var tmpDynBPX = "";
            var tmpDynBPY = "";
            var tmpDynBPNum = "";

            // "["にぶち当たるまで読込む
            last_line.value = sr.readLine();
            while ( last_line.value.indexOf( "[" ) !== 0 ) {
                spl = last_line.value.split( '=' );
                var search = spl[0];
                if ( search == "Language" ) {
                    this.m_type = org.kbinani.vsq.VsqHandleType.Singer;
                    this.Language = parseInt( spl[1], 10 );
                } else if ( search == "Program" ) {
                    this.Program = parseInt( spl[1], 10 );
                } else if ( search == "IconID" ) {
                    this.IconID = spl[1];
                } else if ( search == "IDS" ) {
                    this.IDS = spl[1];
                } else if ( search == "Original" ) {
                    this.Original = parseInt( spl[1], 10 );
                } else if ( search == "Caption" ) {
                    this.Caption = spl[1];
                    for ( var i = 2; i < spl.length; i++ ) {
                        this.Caption += "=" + spl[i];
                    }
                } else if ( search == "Length" ) {
                    this.Length = parseInt( spl[1], 10 );
                } else if ( search == "StartDepth" ) {
                    this.StartDepth = parseInt( spl[1], 10 );
                } else if ( search == "DepthBPNum" ) {
                    tmpDepthBPNum = spl[1];
                } else if ( search == "DepthBPX" ) {
                    tmpDepthBPX = spl[1];
                } else if ( search == "DepthBPY" ) {
                    tmpDepthBPY = spl[1];
                } else if ( search == "StartRate" ) {
                    this.m_type = org.kbinani.vsq.VsqHandleType.Vibrato;
                    this.StartRate = parseInt( spl[1], 10 );
                } else if ( search == "RateBPNum" ) {
                    tmpRateBPNum = spl[1];
                } else if ( search == "RateBPX" ) {
                    tmpRateBPX = spl[1];
                } else if ( search == "RateBPY" ) {
                    tmpRateBPY = spl[1];
                } else if ( search == "Duration" ) {
                    this.m_type = org.kbinani.vsq.VsqHandleType.NoteHeadHandle;
                    this.Duration = parseInt( spl[1], 10 );
                } else if ( search == "Depth" ) {
                    this.Duration = parseInt( spl[1], 10 );
                } else if ( search == "StartDyn" ) {
                    this.m_type = org.kbinani.vsq.VsqHandleType.DynamicsHandle;
                    this.StartDyn = parseInt( spl[1], 10 );
                } else if ( search == "EndDyn" ) {
                    this.m_type = org.kbinani.vsq.VsqHandleType.DynamicsHandle;
                    this.EndDyn = parseInt( spl[1], 10 );
                } else if ( search == "DynBPNum" ) {
                    tmpDynBPNum = spl[1];
                } else if ( search == "DynBPX" ) {
                    tmpDynBPX = spl[1];
                } else if ( search == "DynBPY" ) {
                    tmpDynBPY = spl[1];
                } else if ( search.indexOf( "L" ) === 0 && search.length >= 2 ) {
                    var num = search.substring( 1 );
                    var vals = new org.kbinani.ByRef( 0 );
                    if ( org.kbinani.PortUtil.tryParseInt( num, vals ) ) {
                        var lyric = new org.kbinani.vsq.Lyric( spl[1] );
                        this.m_type = org.kbinani.vsq.VsqHandleType.Lyric;
                        var index = vals.value;
                        if ( index == 0 ) {
                            this.L0 = lyric;
                        } else {
                            this.Trailing[index - 1] = lyric;
                        }
                    }
                }
                if ( !sr.ready() ) {
                    break;
                }
                last_line.value = sr.readLine();
            }

            // RateBPX, RateBPYの設定
            if ( this.m_type == org.kbinani.vsq.VsqHandleType.Vibrato ) {
                if ( tmpRateBPNum != "" ) {
                    this.RateBP = new org.kbinani.vsq.VibratoBPList( tmpRateBPNum, tmpRateBPX, tmpRateBPY );
                } else {
                    this.RateBP = new org.kbinani.vsq.VibratoBPList();
                }

                // DepthBPX, DepthBPYの設定
                if ( tmpDepthBPNum != "" ) {
                    this.DepthBP = new org.kbinani.vsq.VibratoBPList( tmpDepthBPNum, tmpDepthBPX, tmpDepthBPY );
                } else {
                    this.DepthBP = new org.kbinani.vsq.VibratoBPList();
                }
            } else {
                this.DepthBP = new org.kbinani.vsq.VibratoBPList();
                this.RateBP = new org.kbinani.vsq.VibratoBPList();
            }

            if ( tmpDynBPNum != "" ) {
                this.DynBP = new org.kbinani.vsq.VibratoBPList( tmpDynBPNum, tmpDynBPX, tmpDynBPY );
            } else {
                this.DynBP = new org.kbinani.vsq.VibratoBPList();
            }
        },

        /*
        //TODO: org.kbinani.vsq.VsqHandle.print
        /// <summary>
        /// インスタンスをテキストファイルに出力します
        /// </summary>
        /// <param name="sw">出力先</param>
        public void print( BufferedWriter sw )
#if JAVA
            throws IOException
#endif
        {
            String result = toString();
            sw.write( result );
            sw.newLine();
        }*/

        /* /// <summary>
        /// インスタンスをコンソール画面に出力します
        /// </summary>
        private void print() {
            String result = toString();
            PortUtil.println( result );
        }*/

        /**
         * インスタンスを文字列に変換します
         *
         * @return [string] インスタンスを変換した文字列
         */
        toString : function() {
            var result = "";
            result += "[h#" + sprintf( "%04d", this.Index ) + "]";
            if ( this.m_type == org.kbinani.vsq.VsqHandleType.Lyric ) {
                result += "\n" + "L0=" + this.L0.toString( this.addQuotationMark );
                var c = this.Trailing.length;
                for( var i = 0; i < c; i++ ){
                    result += "\n" + "L" + (i + 1) + this.Trailing[i].toString( this.addQuotationMark );
                }
            } else if ( this.m_type == org.kbinani.vsq.VsqHandleType.Vibrato ) {
                result += "\n" + "IconID=" + this.IconID + "\n";
                result += "IDS=" + this.IDS + "\n";
                result += "Original=" + this.Original + "\n";
                result += "Caption=" + this.Caption + "\n";
                result += "Length=" + this.Length + "\n";
                result += "StartDepth=" + this.StartDepth + "\n";
                result += "DepthBPNum=" + this.DepthBP.getCount() + "\n";
                if ( this.DepthBP.getCount() > 0 ) {
                    result += "DepthBPX=" + org.kbinani.PortUtil.sprintf( "%.6f", this.DepthBP.getElement( 0 ).X );
                    for ( var i = 1; i < this.DepthBP.getCount(); i++ ) {
                        result += "," + org.kbinani.PortUtil.sprintf( "%.6f", this.DepthBP.getElement( i ).X );
                    }
                    result += "\n" + "DepthBPY=" + this.DepthBP.getElement( 0 ).Y;
                    for ( var i = 1; i < this.DepthBP.getCount(); i++ ) {
                        result += "," + this.DepthBP.getElement( i ).Y;
                    }
                    result += "\n";
                }
                result += "StartRate=" + this.StartRate + "\n";
                result += "RateBPNum=" + this.RateBP.getCount();
                if ( this.RateBP.getCount() > 0 ) {
                    result += "\n" + "RateBPX=" + org.kbinani.PortUtil.sprintf( "%.6f", this.RateBP.getElement( 0 ).X );
                    for ( var i = 1; i < this.RateBP.getCount(); i++ ) {
                        result += "," + org.kbinani.PortUtil.sprintf( "%.6f", this.RateBP.getElement( i ).X );
                    }
                    result += "\n" + "RateBPY=" + this.RateBP.getElement( 0 ).Y;
                    for ( var i = 1; i < this.RateBP.getCount(); i++ ) {
                        result += "," + this.RateBP.getElement( i ).Y;
                    }
                }
            } else if ( this.m_type == org.kbinani.vsq.VsqHandleType.Singer ) {
                result += "\n" + "IconID=" + this.IconID + "\n";
                result += "IDS=" + this.IDS + "\n";
                result += "Original=" + this.Original + "\n";
                result += "Caption=" + this.Caption + "\n";
                result += "Length=" + this.Length + "\n";
                result += "Language=" + this.Language + "\n";
                result += "Program=" + this.Program;
            } else if ( this.m_type == org.kbinani.vsq.VsqHandleType.NoteHeadHandle ) {
                result += "\n" + "IconID=" + this.IconID + "\n";
                result += "IDS=" + this.IDS + "\n";
                result += "Original=" + this.Original + "\n";
                result += "Caption=" + this.Caption + "\n";
                result += "Length=" + this.Length + "\n";
                result += "Duration=" + this.Duration + "\n";
                result += "Depth=" + this.Depth;
            } else if ( this.m_type == org.kbinani.vsq.VsqHandleType.DynamicsHandle ) {
                result += "\n" + "IconID=" + this.IconID +"\n";
                result += "IDS=" + this.IDS + "\n";
                result += "Original=" + this.Original + "\n";
                result += "Caption=" + this.Caption + "\n";
                result += "StartDyn=" + this.StartDyn + "\n";
                result += "EndDyn=" + this.EndDyn + "\n";
                result += "Length=" + this.Length + "\n";
                if ( this.DynBP != null ) {
                    if ( this.DynBP.getCount() <= 0 ) {
                        result += "DynBPNum=0";
                    } else {
                        result += "DynBPX=" + org.kbinani.PortUtil.sprintf( "%.6f", this.DynBP.getElement( 0 ).X );
                        var c = this.DynBP.getCount();
                        for ( var i = 1; i < c; i++ ) {
                            result += "," + org.kbinani.PortUtil.sprintf( "%.6f", this.DynBP.getElement( i ).X );
                        }
                        result += "\n" + "DynBPY=" + this.DynBP.getElement( 0 ).Y;
                        for ( var i = 1; i < c; i++ ) {
                            result += "," + this.DynBP.getElement( i ).Y;
                        }
                    }
                } else {
                    result += "DynBPNum=0";
                }
            }
            return result;
        },
    };

}
