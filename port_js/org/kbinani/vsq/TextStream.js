/*
 * TextStream.js
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
if( org.kbinani.vsq.TextStream == undefined ){

    org.kbinani.vsq.TextStream = function(){
        this.array = new Array();
        this.length = 0;
        this.position = -1;
    };

    org.kbinani.vsq.TextStream.prototype = {
        /**
         * @return [int]
         */
        getPointer : function() {
            return this.position;
        },

        /**
         * @param value [int]
         * @return [void]
         */
        setPointer : function( value ) {
            this.position = value;
        },

        /**
         * @return [char]
         */
        get : function() {
            this.position++;
            return this.array[this.position];
        },

        /**
         * @return [string]
         */
        readLine : function() {
            var sb = "";
            // '\n'が来るまで読み込み
            this.position++;
            for ( ; this.position < this.length; this.position++ ) {
                var c = this.array[this.position];
                if ( c == '\n' ) {
                    break;
                }
                sb += c;
            }
            return sb;
        },

        /**
         * @return [bool]
         */
        ready : function() {
            if ( 0 <= this.position + 1 && this.position + 1 < this.length ) {
                return true;
            } else {
                return false;
            }
        },

        /**
         * @param length [int]
         * @return [void]
         */
        _ensureCapacity : function( _length ) {
            if ( _length > this.array.length ) {
                var add = _length  - this.array.length;
                for( var i = 0; i < add; i++ ){
                    this.array.push( " " );
                }
            }
        },

        /**
         * @param str [string]
         * @return [void]
         */
        write : function( str ) {
            var len = str.length;
            var newSize = this.length + len;
            var offset = length;
            _ensureCapacity( newSize );
            for ( var i = 0; i < len; i++ ) {
                this.array[offset + i] = str.charAt( i );
            }
            this.length = newSize;
        },

        /**
         * @param str [string]
         * @return [void]
         */
        writeLine : function( str ) {
            var len = str.length;
            var newSize = length + len + 1;
            var offset = length;
            _ensureCapacity( newSize );
            for ( var i = 0; i < len; i++ ) {
                this.array[offset + i] = str.charAt( i );
            }
            this.array[offset + len] = '\n';
            this.length = newSize;
        },

        /**
         * @return [void]
         */
        close : function() {
            this.array = null;
            this.length = 0;
        },
    };

}
