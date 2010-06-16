/*
 * ByRef.js
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
if( org.kbinani.ByRef == undefined ){

    org.kbinani.ByRef = function(){
        this.value = null;
        if( arguments.length == 1 ){
            this._init_1( arguments[0] );
        }
    };

    org.kbinani.ByRef.prototype = {
        _init_1 : function( value ){
            this.value = value;
            return this;
        },
    };

}
