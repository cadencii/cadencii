/*
 * BPPair.js
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
if( org.kbinani.vsq.BPPair == undefined ){

	org.kbinani.vsq.BPPair = function( clock, value ){
	    this.clock = clock;
	    this.value = value;
	}

    org.kbinani.vsq.BPPair.prototype = {
	    compareTo : function( item ){
	        if ( this.clock > item.clock ) {
	            return 1;
	        } else if ( this.clock < item.clock ) {
	            return -1;
	        } else {
	            return 0;
	        }
	    },

	    toString : function(){
	        return "{" + this.clock + ", " + this.value + "}";
	    },
	};

}
