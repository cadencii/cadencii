/*
 * ArrayIterator.js
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
if( org.kbinani == undefined ) org.kbiani = {};
if( org.kbinani.ArrayIterator == undefined ){

    org.kbinani.ArrayIterator = function(){
        this._array = null;
        this._pos = -1;
        if( arguments.length == 1 ){
            this._array = arguments[0];
        }
    };

    org.kbinani.ArrayIterator.prototype = {
        /**
         * @param list [Array]
         * @return [ArrayIterator]
         */
        _init_1 : function( list ){
            this._array = list;
            this._pos = -1;
            return this;
        },

        /**
         * @return [bool]
         */
        hasNext : function(){
            if( this._pos + 1 < this._array.length ){
                return true;
            }else{
                return false;
            }
        },

        /**
         * @return [object]
         */
        next : function(){
            this._pos++;
            return this._array[this._pos];
        },

        /**
         * @return [void]
         */
        remove : function(){
            if( 0 < this._pos && this._pos < this._array.length ){
                for( var i = this._pos; i < this._array.length - 1; i++ ){
                    this._array[i] = this._array[i + 1];
                }
                this._array.pop();
            }
        },
    };

}
