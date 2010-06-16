/*
 * LyricHandle.js
 * Copyright (C) 2008-2010 kbinani
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
if( org.kbinani.vsq.LyricHandle == undefined ){

    /// <summary>
    /// type = Lyric用のhandleのコンストラクタ
    /// </summary>
    /// <param name="phrase">歌詞</param>
    /// <param name="phonetic_symbol">発音記号</param>
    org.kbinani.vsq.LyricHandle = function(){
        if( arguments.length >= 2 ){
            var phrase = arguments[0];
            var phonetic_symbol = arguments[1];
            this.L0 = new org.kbinani.vsq.Lyric( phrase, phonetic_symbol );
        }else{
            this.L0 = new org.kbinani.vsq.Lyric();
        }
        this.Index = 0;
        this.Trailing = new Array();
    };

    org.kbinani.vsq.LyricHandle.prototype = {
        /**
         * @param index [int]
         * @return [Lyric]
         */
        getLyricAt : function( index ){
            if( index == 0 ){
                return this.L0;
            }else{
                return this.Trailing[index - 1];
            }
        },

        /**
         * @param index [int]
         * @param value [Lyric]
         * @return [void]
         */
        setLyricAt : function( index, value ){
            if( index == 0 ){
                this.L0 = value;
            }else{
                this.Trailing[index - 1] = value;
            }
        },

        /**
         * @return [int]
         */
        getCount : function(){
            return this.Trailing.length + 1;
        },

        /**
         * @return [Lyric]
         */
        clone : function() {
            var ret = new org.kbinani.vsq.LyricHandle();
            ret.Index = this.Index;
            ret.L0 = this.L0.clone();
            var c = this.Trailing.length;
            for( var i = 0; i < c; i++ ){
                var buf = this.Trailing[i].clone();
                ret.Trailing.push( buf );
            }
            return ret;
        },

        /**
         * @return [VsqHandle]
         */
        castToVsqHandle : function() {
            var ret = new VsqHandle();
            ret.m_type = org.kbinani.vsq.VsqHandleType.Lyric;
            ret.L0 = this.L0.clone();
            ret.Trailing = this.Trailing;
            ret.Index = this.Index;
            return ret;
        },
    };

}
