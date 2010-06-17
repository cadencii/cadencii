/*
 * VsqBarLineType.js
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
if( org.kbinani == undefined ) org.kbiani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.VsqBarLineType == undefined ){

    org.kbinani.vsq.VsqBarLineType = function(){
        this._m_clock = 0;
        this._m_is_separator = false;
        this._m_denominator = 0;
        this._m_numerator = 0;
        this._m_bar_count = 0;
        if( arguments.length == 5 ){
            this._init_5( arguments[0], arguments[1], arguments[2], arguments[3], arguments[4] );
        }
    };

    org.kbinani.vsq.VsqBarLineType.prototype = {
        /**
         * @return [int]
         */
        getBarCount : function() {
            return this._m_bar_count;
        },

        /**
         * @return [int]
         */
        getLocalDenominator : function() {
            return this._m_denominator;
        },

        /**
         * @return [int]
         */
        getLocalNumerator : function() {
            return this._m_numerator;
        },

        /**
         * @return [int]
         */
        clock : function() {
            return this._m_clock;
        },

        /**
         * @return [bool]
         */
        isSeparator : function() {
            return this._m_is_separator;
        },

        /**
         * @param clock [int]
         * @param is_separator [bool]
         * @param denominator [int]
         * @param numerator [int]
         * @param bar_count [int]
         * @return [void]
         */
        _init_5 : function( clock, is_separator, denominator, numerator, bar_count ) {
            this._m_clock = clock;
            this._m_is_separator = is_separator;
            this._m_denominator = denominator;
            this._m_numerator = numerator;
            this._m_bar_count = bar_count;
        },
    };

}
