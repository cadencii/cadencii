/*
 * ByteArrayInputStream.js
 * Copyright (C) 2010 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.ByteArrayInputStream == undefined ){

    /**
     * @param byte_array [byte[]]
     */
    org.kbinani.ByteArrayInputStream = function( byte_array ){
        this.byte_array = byte_array;
        /**
         * [long]
         */
        this.index = -1;
    };

    org.kbinani.ByteArrayInputStream.prototype = {
        /**
         * @return [int]
         */
        read : function(){
            this.index++;
            if( this.index < this.byte_array.length ){
                return this.byte_array[this.index];
            }else{
                return -1;
            }
        },

        /**
         * @retrurn [long]
         */
        getFilePointer : function(){
            return this.index + 1;
        },

        /**
         * @param pos [long]
         * @return [void]
         */
        seek : function( pos ){
            this.index = pos - 1;
        },

        /**
         * @param byte_array [byte[]]
         * @param offset [int]
         * @param length [int]
         * @return read length [int]
         */
        readArray : function( byte_array, offset, length ){
            var i = 0;
            for( i = 0; i < length; i++ ){
                var c = this.read();
                if( c < 0 ){
                    return i;
                }
                byte_array[i + offset] = c;
            }
            return i;
        },
    };

}
