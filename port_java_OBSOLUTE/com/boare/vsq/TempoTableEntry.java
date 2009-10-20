/*
 * TempoTableEntry.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

public class TempoTableEntry implements Comparable<TempoTableEntry>, Cloneable {
    public int clock;
    public int tempo;
    public double time;

    public static String getXmlElementName( String name ){
        if( name.equals( "clock" ) ){
            return "Clock";
        }else if( name.equals( "tempo" ) ){
            return "Tempo";
        }else if( name.equals( "time" ) ){
            return "Time";
        }
        return name;
    }

    public Object clone() {
        return new TempoTableEntry( clock, tempo, time );
    }

    public TempoTableEntry( int clock_, int tempo_, double time_ ) {
        this.clock = clock_;
        this.tempo = tempo_;
        this.time = time_;
    }

    public TempoTableEntry() {
    }

    public int compareTo( TempoTableEntry entry ) {
        return this.clock - entry.clock;
    }

    public boolean equals( TempoTableEntry entry ) {
        if ( this.clock == entry.clock ) {
            return true;
        } else {
            return false;
        }
    }
}
