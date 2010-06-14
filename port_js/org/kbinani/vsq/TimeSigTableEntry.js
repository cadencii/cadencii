/*
 * TimeSigTableEntry.js
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
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.TimeSigTableEntry == undefined ){

    org.kbinani.vsq.TimeSigTableEntry = function(){
        /// <summary>
        /// クロック数
        /// </summary>
        this.Clock = 0;
        /// <summary>
        /// 拍子の分子
        /// </summary>
        this.Numerator = 4;
        /// <summary>
        /// 拍子の分母
        /// </summary>
        this.Denominator = 4;
        /// <summary>
        /// 何小節目か
        /// </summary>
        this.BarCount = 0;
        if( arguments.length == 4 ){
            this._init_4( arguments[0], arguments[1], arguments[2], arguments[3] );
        }
    };

    org.kbinani.vsq.TimeSigTableEntry.prototype = {
        /**
         * @param clock [int]
         * @param numerator [int]
         * @param denominator [int]
         * @param bar_count [int]
         * @return [TimeSigTableEntry]
         */
        _init_4 : function(){
            if( arguments.length == 4 ){
                this.Clock = arguments[0];
                this.Numerator = arguments[1];
                this.Denominator = arguments[2];
                this.BarCount = arguments[3];
            }
            return this;
        },

        /**
         * @return [string]
         */
        toString : function() {
            return "{Clock=" + this.Clock + ", Numerator=" + this.Numerator + ", Denominator=" + this.Denominator + ", BarCount=" + this.BarCount + "}";
        },

        /**
         * @return [object]
         */
        clone : function() {
            return new org.kbinani.vsq.TimeSigTableEntry( this.Clock, this.Numerator, this.Denominator, this.BarCount );
        },

        /**
         * @param item [TimeSigTableEntry]
         * @return [int]
         */
        compareTo : function( item ) {
            return this.BarCount - item.BarCount;
        },
    };

    /**
     * @param a [TimeSigTableEntry]
     * @param b [TimeSigTableEntry]
     * @return [int]
     */
    org.kbinani.vsq.TimeSigTableEntry.compare = function( a, b ){
        return a.compareTo( b );
    };

}
