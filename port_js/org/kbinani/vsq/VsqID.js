/*
 * VsqID.js
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
if( org.kbinani.vsq.VsqID == undefined ){

    /// <summary>
    /// メタテキストに埋め込まれるIDを表すクラス。
    /// </summary>

    org.kbinani.vsq.VsqID = function(){
        this.value = -1;
        this.IconHandle_index = 0;
        this.LyricHandle_index = 0;
        this.VibratoHandle_index = 0;
        this.NoteHeadHandle_index = 0;
        this.type = org.kbinani.vsq.VsqIDType.Note;
        /**
         * [IconHandle]
         */
        this.IconHandle = null;
        this._length = 0;
        this.Note = 0;
        this.Dynamics = 0;
        this.PMBendDepth = 0;
        this.PMBendLength = 0;
        this.PMbPortamentoUse = 0;
        this.DEMdecGainRate = 0;
        this.DEMaccent = 0;
        /**
         * [LyricHandle]
         */
        this.LyricHandle = null;
        /**
         * [VibratoHandle]
         */
        this.VibratoHandle = null;
        this.VibratoDelay = 0;
        /**
         * [NoteHeadHandle]
         */
        this.NoteHeadHandle = null;
        this.pMeanOnsetFirstNote = 0x0a;
        this.vMeanNoteTransition = 0x0c;
        this.d4mean = 0x18;
        this.pMeanEndingNote = 0x0c;
        /**
         * [IconDynamicsHandle]
         */
        this.IconDynamicsHandle = null;
    };

    org.kbinani.vsq.VsqID.EOS = new org.kbinani.vsq.VsqID();
    /**
     * ミリ秒で表した、音符の最大長さ
     */
    org.kbinani.vsq.VsqID.MAX_NOTE_MILLISEC_LENGTH = 16383;

    org.kbinani.vsq.VsqID.prototype = {
        /**
         * @param value [int]
         * @return [VsqID]
         */
        _init_1 : function( value ){
            this.value = value;
        },

        /**
         * テキストファイルからのコンストラクタ
         * @param sr [TextStream] 読み込み対象
         * @param value [int] 
         * @param last_line [ByRef<string>] 読み込んだ最後の行が返されます
         * @return [VsqID]
         */
        _init_3 : function( sr, value, last_line ){
            var sr = arguments[0];
            var value = arguments[1];
            var last_line = arguments[2];
            var spl;
            this.value = value;
            this.type = VsqIDType.Unknown;
            this.IconHandle_index = -2;
            this.LyricHandle_index = -1;
            this.VibratoHandle_index = -1;
            this.NoteHeadHandle_index = -1;
            this.setLength( 0 );
            this.Note = 0;
            this.Dynamics = 64;
            this.PMBendDepth = 8;
            this.PMBendLength = 0;
            this.PMbPortamentoUse = 0;
            this.DEMdecGainRate = 50;
            this.DEMaccent = 50;
            this.VibratoDelay = 0;
            last_line.value = sr.readLine();
            while ( last_line.indexOf( "[" ) !== 0 ) {
                spl = last_line.value.split( '=' );
                var search = spl[0];
                if ( search == "Type" ) {
                    if ( spl[1] == "Anote" ) {
                        type = org.kbinani.vsq.VsqIDType.Anote;
                    } else if ( spl[1] == "Singer" ) {
                        type = org.kbinani.vsq.VsqIDType.Singer;
                    } else if ( spl[1] == "Aicon" ) {
                        type = org.kbinani.vsq.VsqIDType.Aicon;
                    } else {
                        type = org.kbinani.vsq.VsqIDType.Unknown;
                    }
                } else if ( search == "Length" ) {
                    this.setLength( parseInt( spl[1], 10 ) );
                } else if ( search == "Note#" ) {
                    this.Note = parseInt( spl[1], 10 );
                } else if ( search == "Dynamics" ) {
                    this.Dynamics = parseInt( spl[1], 10 );
                } else if ( search == "PMBendDepth" ) {
                    this.PMBendDepth = parseInt( spl[1], 10 );
                } else if ( search == "PMBendLength" ) {
                    this.PMBendLength = parseInt( spl[1], 10 );
                } else if ( search == "DEMdecGainRate" ) {
                    this.DEMdecGainRate = parseInt( spl[1], 10 );
                } else if ( search ==  "DEMaccent" ) {
                    this.DEMaccent = parseInt( spl[1], 10 );
                } else if ( search == "LyricHandle" ) {
                    this.LyricHandle_index = org.kbinani.vsq.VsqHandle.handleIndexFromString( spl[1] );
                } else if ( search == "IconHandle" ) {
                    this.IconHandle_index = org.kbinani.vsq.VsqHandle.handleIndexFromString( spl[1] );
                } else if ( search == "VibratoHandle" ) {
                    this.VibratoHandle_index = org.kbinani.vsq.VsqHandle.handleIndexFromString( spl[1] );
                } else if ( search == "VibratoDelay" ) {
                    this.VibratoDelay = parseInt( spl[1], 10 );
                } else if ( search == "PMbPortamentoUse" ) {
                    this.PMbPortamentoUse = parseInt( spl[1], 10 );
                } else if ( search == "NoteHeadHandle" ) {
                    this.NoteHeadHandle_index = org.kbinani.vsq.VsqHandle.handleIndexFromString( spl[1] );
                }
                if ( !sr.ready() ) {
                    break;
                }
                last_line.value = sr.readLine();
            }
        },

        /**
         * @return [int]
         */
        getLength : function() {
            return this._length;
        },

        /**
         * @param value [int]
         * @return [void]
         */
        setLength : function( value ) {
            this._length = value;
        },

        /**
         * このインスタンスの簡易コピーを取得します。
         *
         * @return [object] このインスタンスの簡易コピー
         */
        clone : function() {
            var result = new org.kbinani.vsq.VsqID( this.value );
            result.type = this.type;
            if ( this.IconHandle != null ) {
                result.IconHandle = this.IconHandle.clone();
            }
            result.setLength( getLength() );
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
                result.LyricHandle = this.LyricHandle.clone();
            }
            if ( this.VibratoHandle != null ) {
                result.VibratoHandle = this.VibratoHandle.clone();
            }
            result.VibratoDelay = this.VibratoDelay;
            if ( this.NoteHeadHandle != null ) {
                result.NoteHeadHandle = this.NoteHeadHandle.clone();
            }
            if ( this.IconDynamicsHandle != null ) {
                result.IconDynamicsHandle = this.IconDynamicsHandle.clone();
            }
            return result;
        },

        /**
         * @return [string]
         */
        toString : function() {
            var ret = "{Type=" + this.type;
            if ( this.type == org.kbinani.vsq.VsqIDType.Anote ) {
                ret += ", Length=" + getLength();
                ret += ", Note#=" + this.Note;
                ret += ", Dynamics=" + this.Dynamics;
                ret += ", PMBendDepth=" + this.PMBendDepth;
                ret += ", PMBendLength=" + this.PMBendLength;
                ret += ", PMbPortamentoUse=" + this.PMbPortamentoUse;
                ret += ", DEMdecGainRate=" + this.DEMdecGainRate;
                ret += ", DEMaccent=" + this.DEMaccent;
                if ( this.LyricHandle != null ) {
                    ret += ", LyricHandle=h#" + org.kbinani.PortUtil.sprintf( "%04d", this.LyricHandle_index );
                }
                if ( this.VibratoHandle != null ) {
                    ret += ", VibratoHandle=h#" + org.kbinani.PortUtil.sprintf( "%04d", this.VibratoHandle_index );
                    ret += ", VibratoDelay=" + this.VibratoDelay;
                }
            } else if ( this.type == org.kbinani.vsq.VsqIDType.Singer ) {
                ret += ", IconHandle=h#" + org.kbinani.PortUtil.sprintf( "%04d", this.IconHandle_index );
            }
            ret += "}";
            return ret;
        },
    };

    /**
     * @param name [string]
     * @return [bool]
     */
    org.kbinani.vsq.VsqID.isXmlIgnored = function( name ){
        if( name == "IconHandle_index" ){
            return true;
        }else if( name == "value" ){
            return true;
        }else if( name == "LyricHandle_index" ){
            return true;
        }else if( name == "NoteHeadHandle_index" ){
            return true;
        }else if( name == "VibratoHandle_index" ){
            return true;
        }
        return false;
    };

    /**
     * @param name [string]
     * @return [string]
     */
    org.kbinani.vsq.VsqID.getXmlElementName = function( name ){
        return name;
    };

}
