/*
 * ValuePair.js
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.ValuePair == undefined ){

    org.kbinani.ValuePair = function(){
        this._key = null;
        this._value = null;
    };

    org.kbinani.ValuePair.prototype = {
        init : function(){
            if( arguments.length == 2 ){
                this._key = arguments[0];
                this._value = arguments[1];
            }
            return this;
        },

        /**
         * @return [object]
         */
        getKey : function() {
            return this._key;
        },

        /**
         * @param value [object]
         * @return [void]
         */
        setKey : function( value ){
            this._key = value;
        },

        /**
         * @return [object]
         */
        getValue : function(){
            return this._value;
        },

        /**
         * @param v [object]
         * @return [void]
         */
        setValue : function( v ) {
            this._value = v;
        },
    };

}
