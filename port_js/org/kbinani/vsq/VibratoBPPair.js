/*
 * VibratoBPPair.js
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
if( org.kbinani.vsq.VibratoBPPair == undefined ){

    org.kbinani.vsq.VibratoBPPair = function(){
        this.X = 0.0;
        this.Y = 0;
        if( arguments.length == 2 ){
            this.X = arguments[0];
            this.Y = arguments[1];
        }
    };

    org.kbinani.vsq.VibratoBPPair.compare = function( a, b ){
        return a.compareTo( b );
    };

    org.kbinani.vsq.VibratoBPPair.prototype = {
        /**
         * @param item [VibratoBPPair]
         * @return [int]
         */
        compareTo : function( item ) {
            var v = this.X - item.X;
            if ( v > 0.0 ) {
                return 1;
            } else if ( v < 0.0 ) {
                return -1;
            }
            return 0;
        },
    };

}
