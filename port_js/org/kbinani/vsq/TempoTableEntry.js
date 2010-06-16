/*
 * TempoTableEntry.js
 * Copyright (C) 2008-2010 kbinani
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
if( org.kbinani.vsq.TempoTableEntry == undefined ){

    org.kbinani.vsq.TempoTableEntry = function(){
        this.Clock = 0;
        this.Tempo = 0;
        this.Time = 0.0;
        if( arguments.length == 3 ){
            this._init_3( arguments[0], arguments[1], arguments[2] );
        }
    };

    org.kbinani.vsq.TempoTableEntry.prototype = {
        /**
         * @return [string]
         */
        toString : function() {
            return "{Clock=" + this.Clock + ", Tempo=" + this.Tempo + ", Time=" + this.Time + "}";
        },

        /**
         * @return [object]
         */
        clone : function() {
            return new org.kbinani.vsq.TempoTableEntry( this.Clock, this.Tempo, this.Time );
        },

        /**
         * overload1
         * @return [TempoTableEntry]
         *
         * overload2
         * @param clock [int]
         * @param _tempo [int]
         * @param _time [int]
         * @return [TempoTableEntry]
         */
        _init_3 : function() {
            if( arguments.length == 3 ){
                this.Clock = arguments[0];
                this.Tempo = arguments[1];
                this.Time = arguments[2];
            }
            return this;
        },

        /**
         * @param entry [TempoTableEntry]
         * @return [int]
         */
        compareTo : function( entry ) {
            return this.Clock - entry.Clock;
        },

        /**
         * @param entry [TempoTableEntry]
         * @return [bool]
         */
        equals : function( entry ) {
            if ( this.Clock == entry.Clock ) {
                return true;
            } else {
                return false;
            }
        },
    };

    /**
     * @param a [TempoTableEntry]
     * @param b [TempoTableEntry]
     * @return [int]
     */
    org.kbinani.vsq.TempoTableEntry.compare = function( a, b ){
        return a.compareTo( b );
    };

}
