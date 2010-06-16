/*
 * DrawObject.js
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.cadencii == undefined ) org.kbinani.cadencii = {};
if( org.kbinani.cadencii.DrawObject == undefined ){

    /// <summary>
    /// 画面に描画するアイテムを表します
    /// </summary>
    org.kbinani.cadencii.DrawObject = function(){
        this.pxRectangle = new org.kbinani.java.awt.Rectangle();
        this.text = "";
        this.accent = 0;
        this.internalID = 0;
        /// <summary>
        /// 音符の先頭から，ビブラート開始位置までの長さ(単位：ピクセル)
        /// </summary>
        this.pxVibratoDelay = 0;
        /// <summary>
        /// このアイテムが他のアイテムと再生時にオーバーラップするかどうかを表すフラグ
        /// </summary>
        this.overlappe = false;
        this.symbolProtected = false;
        this.vibRate = null;
        this.vibDepth = null;
        this.vibStartRate = 0;
        this.vibStartDepth = 0;
        this.note = 0;
        this.ustEnvelope = null;
        /// <summary>
        /// 音符の長さ（クロック）
        /// </summary>
        this.length = 0;
        /// <summary>
        /// アイテムの位置
        /// </summary>
        this.clock = 0;
        this.type = 0;
        /// <summary>
        /// UTAUモード、またはStraight x UTAUモードにて、歌詞から*.frqまたはanalyzed\*.stfを引き当てられたかどうか。
        /// これがfalseのとき、ピアノロール上で警告色で描かれる
        /// </summary>
        this.isValid = false;
        if( arguments.length == 17 ){
            this._init_17( arguments[0], arguments[1], arguments[2], arguments[3], arguments[4], 
                           arguments[5], arguments[6], arguments[7], arguments[8], arguments[9],
                           arguments[10], arguments[11], arguments[12], arguments[13], arguments[14],
                           arguments[15], arguments[16] );
        }
    };

    org.kbinani.cadencii.DrawObject.prototype = {
        /**
         * @param type [DrawObjectType]
         * @param rect [Rectangle]
         * @param text_ [string]
         * @param accent_ [int]
         * @param internal_id [int]
         * @param vibrato_delay [int]
         * @param overwrapped [bool]
         * @param symbol_protected [bool]
         * @param vib_rate [VibratoBPList]
         * @param vib_depth [VibratoBPList]
         * @param vib_start_rate [int]
         * @param vib_start_depth [int]
         * @param note_ [int]
         * @param ust_envelope [UstEnvelope]
         * @param length [int]
         * @param clock [int]
         * @param isValid [bool]
         */
        _init_17 : function( type,
                           rect, 
                           text_,
                           accent_,
                           internal_id,
                           vibrato_delay,
                           overwrapped, 
                           symbol_protected,
                           vib_rate,
                           vib_depth,
                           vib_start_rate,
                           vib_start_depth,
                           note_,
                           ust_envelope,
                           length,
                           clock,
                           isValid ) {
            this.type = type;
            this.pxRectangle = rect;
            this.text = text_;
            this.accent = accent_;
            this.internalID = internal_id;
            this.pxVibratoDelay = vibrato_delay;
            this.overlappe = overwrapped;
            this.symbolProtected = symbol_protected;
            this.vibRate = vib_rate;
            this.vibDepth = vib_depth;
            this.vibStartRate = vib_start_rate;
            this.vibStartDepth = vib_start_depth;
            this.note = note_;
            this.ustEnvelope = ust_envelope;
            this.length = length;
            this.clock = clock;
            this.isValid = isValid;
        },

        /**
         * @param item [DrawObject]
         */
        compareTo : function( item ) {
            return this.pxRectangle.x - item.pxRectangle.x;
        },

    };

    org.kbinani.cadencii.DrawObject.compare = function( a, b ){
        return a.compareTo( b );
    };
}
