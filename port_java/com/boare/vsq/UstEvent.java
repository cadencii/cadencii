/*
 * UstEvent.java
 * Copyright (c) 2009 kbinani
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

import java.text.*;
import java.io.*;
import com.boare.corlib.*;

public class UstEvent implements Cloneable {
    public String tag;
    public int length = 0;
    public String lyric = "";
    public int note = -1;
    public int intensity = -1;
    public int pbType = -1;
    public float[] pitches = null;
    public float tempo = -1;
    public UstVibrato vibrato = null;
    public UstPortamento portamento = null;
    public int preUtterance = 0;
    public int voiceOverlap = 0;
    public UstEnvelope envelope = null;
    public String flags = "";
    public int moduration = 100;

    public UstEvent(){
    }

    public static boolean isXmlIgnored( String name ){
        if( name.equals( "tag" ) ){
            return true;
        }
        return false;
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "length" ) ){
            return "Length";
        }else if( name.equals( "lyric" ) ){
            return "Lyric";
        }else if( name.equals( "note" ) ){
            return "Note";
        }else if( name.equals( "intensity" ) ){
            return "Intensity";
        }else if( name.equals( "pbType" ) ){
            return "PBType";
        }else if( name.equals( "pitches" ) ){
            return "Pitches";
        }else if( name.equals( "tempo" ) ){
            return "Tempo";
        }else if( name.equals( "vibrato" ) ){
            return "Vibrato";
        }else if( name.equals( "portamento" ) ){
            return "Portamento";
        }else if( name.equals( "preUtterance" ) ){
            return "PreUtterance";
        }else if( name.equals( "voiceOverlap" ) ){
            return "VoiceOverlap";
        }else if( name.equals( "envelope" ) ){
            return "Envelope";
        }else if( name.equals( "moduration" ) ){
            return "Moduration";
        }else if( name.equals( "flags" ) ){
            return "Flags";
        }
        return name;
    }

    public Object clone() {
        UstEvent ret = new UstEvent();
        ret.length = length;
        ret.lyric = lyric;
        ret.note = note;
        ret.intensity = intensity;
        ret.pbType = pbType;
        if ( pitches != null ) {
            ret.pitches = new float[pitches.length];
            for ( int i = 0; i < pitches.length; i++ ) {
                ret.pitches[i] = pitches[i];
            }
        }
        ret.tempo = tempo;
        if ( vibrato != null ) {
            ret.vibrato = (UstVibrato)vibrato.clone();
        }
        if ( portamento != null ) {
            ret.portamento = (UstPortamento)portamento.clone();
        }
        if ( envelope != null ) {
            ret.envelope = (UstEnvelope)envelope.clone();
        }
        ret.preUtterance = preUtterance;
        ret.voiceOverlap = voiceOverlap;
        ret.flags = flags;
        ret.moduration = moduration;
        ret.tag = tag;
        return ret;
    }

    public void print( StreamWriter sw, int index ) throws IOException{
        sw.writeLine( (new DecimalFormat( "0000" )).format( index ) );
        sw.writeLine( "Length=" + length );
        sw.writeLine( "Lyric=" + lyric );
        sw.writeLine( "NoteNum=" + note );
        if ( intensity >= 0 ) {
            sw.writeLine( "Intensity=" + intensity );
        }
        if ( pbType >= 0 && pitches != null ) {
            sw.writeLine( "PBType=" + pbType );
            sw.write( "Piches=" );
            for ( int i = 0; i < pitches.length; i++ ) {
                if ( i == 0 ) {
                    sw.write( pitches[i] + "" );
                } else {
                    sw.write( "," + pitches[i] );
                }
            }
            sw.writeLine();
        }
        if ( tempo > 0 ) {
            sw.writeLine( "Tempo=" + tempo );
        }
        if ( vibrato != null ) {
            sw.writeLine( vibrato.toString() );
        }
        if ( portamento != null ) {
            portamento.print( sw );
        }
        if ( envelope != null ) {
            if ( preUtterance >= 0 ) {
                sw.writeLine( "PreUtterance=" + preUtterance );
            }
            if ( voiceOverlap != 0 ) {
                sw.writeLine( "VoiceOverlap=" + voiceOverlap );
            }
            sw.writeLine( envelope.toString() );
        }
        if ( !flags.equals( "" ) ) {
            sw.writeLine( "Flags=" + flags );
        }
        if ( moduration >= 0 ) {
            sw.writeLine( "Moduration=" + moduration );
        }
    }
}
