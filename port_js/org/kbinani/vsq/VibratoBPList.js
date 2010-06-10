/*
 * VibratoBPList.js
 * Copyright (C) 2009-2010 kbinani
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
if( org.kbinani.vsq.VibratoBPList == undefined ){

    org.kbinani.vsq.VibratoBPList = function(){
        this.m_list = new Array();
        if( arguments.length == 3 ){
            var strNum = arguments[0];
            var strBPX = arguments[1];
            var strBPY = arguments[2];
            var num = 0;
            num = parseInt( strNum, 10 );
            if( isNaN( num ) ){
                num = 0;
            }
            var bpx = strBPX.split( ',' );
            var bpy = strBPY.split( ',' );
            var actNum = Math.min( num, Math.min( bpx.length, bpy.length ) );
            if ( actNum > 0 ) {
                var x = new Array( actNum );
                var y = new Array( actNum );
                for ( var i = 0; i < actNum; i++ ) {
                    x[i] = parseFloat( bpx[i] );
                    y[i] = parseInt( bpy[i] );
                }

                var len = Math.min( x.length, y.length );
                for ( var i = 0; i < len; i++ ) {
                    this.m_list.push( new org.kbinani.vsq.VibratoBPPair( x[i], y[i] ) );
                }
                this.m_list.sort( org.kbinani.vsq.VibratoBPPair.compare );
            }
        }else if( arguments.length == 2 ){
            var x = arguments[0];//float[]
            var y = arguments[1];//int[]
            var len = Math.min( x.length, y.length );
            for ( var i = 0; i < len; i++ ) {
                this.m_list.push( new org.kbinani.vsq.VibratoBPPair( x[i], y[i] ) );
            }
            this.m_list.sort( org.kbinani.vsq.VibratoBPPair.compare );
        }
    };

    org.kbinani.vsq.VibratoBPList.prototype = {
        /**
         * @param x [float]
         * @param default_value [int]
         * @return [int]
         */
        getValue : function( x, default_value ) {
            if ( this.m_list.length <= 0 ) {
                return default_value;
            }
            var index = -1;
            var size = m_list.length;
            for ( var i = 0; i < size; i++ ) {
                if ( x < this.m_list[i].X ) {
                    break;
                }
                index = i;
            }
            if ( index == -1 ) {
                return default_value;
            } else {
                return this.m_list[index].Y;
            }
        },

        /**
         * @return [object]
         */
        clone : function() {
            var ret = new org.kbinani.vsq.VibratoBPList();
            for ( var i = 0; i < m_list.length; i++ ) {
                ret.m_list.push( new org.kbinani.vsqVibratoBPPair( this.m_list[i].X, this.m_list[i].Y ) );
            }
            return ret;
        },

        /**
         * @return [int]
         */
        getCount : function() {
            return this.m_list.length;
        },

        /**
         * @param index [int]
         * @return [VibratoBPPair]
         */
        getElement : function( index ) {
            return this.m_list[index];
        },

        /**
         * @param index [int]
         * @param value [VibratoBPPair]
         * @return [void]
         */
        setElement : function( ndex, value ) {
            this.m_list[index] = value;
        },

        /**
         * @return [String]
         */
        getData : function() {
            var ret = "";
            for ( var i = 0; i < this.m_list.length; i++ ) {
                ret += (i == 0 ? "" : ",") + m_list[i].X + "=" + m_list[i].Y;
            }
            return ret;
        },

        /**
         * @param value [String]
         * @return [void]
         */
        setData : function( value ) {
            var c = this.m_list.length;
            for( var i = 0; i < c; i++ ){
                this.m_list.shift();
            }
            var spl = value.split( ',' );
            for ( var i = 0; i < spl.length; i++ ) {
                var spl2 = spl[i].split( '=' );
                if ( spl2.length < 2 ) {
                    continue;
                }
                this.m_list.push( new org.kbinani.vsq.VibratoBPPair( parseFloat( spl2[0] ), parseInt( spl2[1], 10 ) ) );
            }
        },
    };

}
