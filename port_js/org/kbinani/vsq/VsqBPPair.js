/*
 * VsqBPPair.js
 * Copyright (C) 2009-2010 kbinani
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
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.VsqBPPair == undefined ){

    org.kbinani.vsq.VsqBPPair = function( _value, _id ){
        this.value = _value;
        this.id = _id;
    };

    org.kbinani.vsq.VsqBPPair.prototype = {
        /**
         * @return [object]
         */
        clone : function(){
            return new org.kbinani.vsq.VsqBPPair( this.value, this.id );
        },
    };

}
