/*
 * RgbColor.js
 * Copyright (C) 2009-2010 kbinani
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
if( org.kbinani.cadencii.RgbColor == undefined ){

    org.kbinani.cadencii.RgbColor = function(){
        this.R = 0;
        this.G = 0;
        this.B = 0;
        if( arguments.length == 3 ){
            this._init_3( arguments[0], arguments[1], arguments[2] );
        }
    };

    org.kbinani.cadencii.RgbColor.prototype = {
        _init_3 : function( r, g, b ) {
            this.R = r;
            this.G = g;
            this.B = b;
        },

        /**
         * @return [org.kbinani.java.awt.Color]
         */
        getColor : function() {
            return new org.kbinani.java.awt.Color( this.R, this.G, this.B );
        },
    };

}
