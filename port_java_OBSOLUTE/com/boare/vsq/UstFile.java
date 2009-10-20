/*
 * UstFile.java
 * Copyright (c) 2009 kbinani, PEX
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

import java.util.*;
import java.io.*;
import com.boare.corlib.*;

public class UstFile implements Cloneable {
    public Object tag;
    private float m_tempo = 120.00f;
    private String m_project_name = "";
    private String m_voice_dir = "";
    private String m_out_file = "";
    private String m_cache_dir = "";
    private String m_tool1 = "";
    private String m_tool2 = "";
    private Vector<UstTrack> m_tracks = new Vector<UstTrack>();
    private Vector<TempoTableEntry> m_tempo_table;
    
    public UstFile( String path ) throws IOException, Exception{
        StreamReader sr = new StreamReader( path, "SJIS" );
        String line = sr.readLine();
        if ( !line.equals( "[#SETTING]" ) ){
            throw new Exception( "invalid ust file" );
        }
        UstTrack track = new UstTrack();
        int type = 0; //0 => reading "SETTING" section
        while( true ) {
            UstEvent ue = null;
            if ( type == 1 ) {
                ue = new UstEvent();
            }
            if ( line.equals( "[#TRACKEND]" ) ){
                break;
            }
            line = sr.readLine(); // "[#" 直下の行
            while( !line.startsWith( "[#" ) ){
                String[] spl = line.split( "=", 2 );
                if ( type == 0 ) {
                    // reading "SETTING" section
                    if ( spl[0].equals( "Tempo" ) ){
                        m_tempo = 125f;
                        float v = 125f;
                        try{
                            v = Float.parseFloat( spl[1] );
                            m_tempo = v;
                        }catch( Exception ex ){
                        }
                    } else if ( spl[0].equals( "ProjectName" ) ){
                        m_project_name = spl[1];
                    } else if ( spl[0].equals( "VoiceDir" ) ){
                        m_voice_dir = spl[1];
                    } else if ( spl[0].equals( "OutFile" ) ){
                        m_out_file = spl[1];
                    } else if ( spl[0].equals( "CacheDir" ) ){
                        m_cache_dir = spl[1];
                    } else if ( spl[0].equals( "Tool1" ) ){
                        m_tool1 = spl[1];
                    } else if ( spl[0].equals( "Tool2" ) ){
                        m_tool2 = spl[1];
                    }
                } else if ( type == 1 ) {
                    // readin event section
                    if ( spl[0].equals( "Length" ) ){
                        ue.length = 0;
                        int v = 0;
                        try{
                            v = Integer.parseInt( spl[1] );
                            ue.length =v;
                        }catch( Exception ex ){
                        }
                    } else if ( spl[0].equals( "Lyric" ) ){
                        ue.lyric = spl[1];
                    } else if ( spl[0].equals( "NoteNum" ) ){
                        ue.note = 0;
                        int v = 0;
                        try{
                            v = Integer.parseInt( spl[1] );
                            ue.note = v;
                        }catch( Exception ex ){
                        }
                    } else if ( spl[0].equals( "Intensity" ) ){
                        ue.intensity = 64;
                        int v = 64;
                        try{
                            v = Integer.parseInt( spl[1] );
                            ue.intensity = v;
                        }catch( Exception ex ){
                        }
                    } else if ( spl[0].equals( "PBType" ) ){
                        ue.pbType = 5;
                        int v = 5;
                        try{
                            v = Integer.parseInt( spl[1] );
                            ue.pbType = v;
                        }catch( Exception ex ){
                        }
                    } else if ( spl[0].equals( "Piches" ) ){
                        String[] spl2 = spl[1].split( "," );
                        float[] t = new float[spl2.length];
                        for ( int i = 0; i < spl2.length; i++ ) {
                            float v = 0;
                            try{
                                v = Float.parseFloat( spl2[i] );
                                t[i] = v;
                            }catch( Exception ex ){
                            }
                        }
                        ue.pitches = t;
                    } else if ( spl[0].equals( "Tempo" ) ){
                        ue.tempo = 125f;
                        float v;
                        try{
                            v = Float.parseFloat( spl[1] );
                            ue.tempo = v;
                        }catch( Exception ex ){
                        }
                    } else if ( spl[0].equals( "VBR" ) ){
                        ue.vibrato = new UstVibrato( line );
                        /*
                        PBW=50,50,46,48,56,50,50,50,50
                        PBS=-87
                        PBY=-15.9,-20,-31.5,-26.6
                        PBM=,s,r,j,s,s,s,s,s
                        */
                    } else if ( spl[0].equals( "PBW" ) ||
                                spl[0].equals( "PBS" ) ||
                                spl[0].equals( "PBY" ) ||
                                spl[0].equals( "PBM" ) ){
                        if ( ue.portamento == null ) {
                            ue.portamento = new UstPortamento();
                        }
                        ue.portamento.parseLine( line );
                    } else if ( spl[0].equals( "Envelope" ) ){
                        ue.envelope = new UstEnvelope( line );
                        //PreUtterance=1
                        //VoiceOverlap=6
                    } else if ( spl[0].equals( "VoiceOverlap" ) ){
                        if ( !spl[1].equals( "" ) ){
                            try{
                                ue.voiceOverlap = Integer.parseInt( spl[1] );
                            }catch( Exception ex ){
                            }
                        }
                    } else if ( spl[0].equals( "PreUtterance" ) ){
                        if ( !spl[1].equals( "" ) ){
                            try{
                                ue.preUtterance = Integer.parseInt( spl[1] );
                            }catch( Exception ex ){
                            }
                        }
                    } else if ( spl[0].equals( "Flags" ) ){
                        ue.flags = line.substring( 6 );
                    }
                }
                line = sr.readLine();
            }
            if ( type == 0 ) {
                type = 1;
            } else if ( type == 1 ) {
                track.addEvent( ue );
            }
        }
        m_tracks.add( track );
        sr.close();
        updateTempoInfo();
    }

    private UstFile(){
    }

    public String getProjectName() {
        return m_project_name;
    }

    public int getBaseTempo() {
        return (int)(6e7 / m_tempo);
    }

    public double getTotalSec() {
        int max = 0;
        int c = m_tracks.size();
        for ( int track = 0; track < c; track++ ) {
            int count = 0;
            UstTrack item = m_tracks.get( track );
            int c2 = item.getEventCount();
            for ( int i = 0; i < c2; i++ ) {
                count += (int)item.getEvent( i ).length;
            }
            max = Math.max( max, count );
        }
        return getSecFromClock( max );
    }

    public Vector<TempoTableEntry> getTempoList() {
        return m_tempo_table;
    }

    public UstTrack getTrack( int track ) {
        return m_tracks.get( track );
    }

    public int getTrackCount() {
        return m_tracks.size();
    }

    /// <summary>
    /// TempoTableの[*].Timeの部分を更新します
    /// </summary>
    /// <returns></returns>
    public void updateTempoInfo() {
        m_tempo_table = new Vector<TempoTableEntry>();
        if ( m_tracks.size() <= 0 ) {
            return;
        }
        int clock = 0;
        double time = 0.0;
        int last_tempo_clock = 0;  //最後にTempo値が代入されていたイベントのクロック
        float last_tempo = m_tempo;   //最後に代入されていたテンポの値
        UstTrack track0 = m_tracks.get( 0 );
        int c = track0.getEventCount();
        for ( int i = 0; i < c; i++ ) {
            if ( track0.getEvent( i ).tempo > 0f ) {
                time += (clock - last_tempo_clock) / (8.0 * last_tempo);
                if ( m_tempo_table.size() == 0 && clock != 0 ) {
                    m_tempo_table.add( new TempoTableEntry( 0, (int)(6e7 / m_tempo), 0.0 ) );
                }
                m_tempo_table.add( new TempoTableEntry( clock, (int)(6e7 / track0.getEvent( i ).tempo), time ) );
                last_tempo = track0.getEvent( i ).tempo;
                last_tempo_clock = clock;
            }
            clock += (int)track0.getEvent( i ).length;
        }
    }

    /// <summary>
    /// 指定したクロックにおける、clock=0からの演奏経過時間(sec)
    /// </summary>
    /// <param name="clock"></param>
    /// <returns></returns>
    public double getSecFromClock( int clock ) {
        int c = m_tempo_table.size();
        for ( int i = c - 1; i >= 0; i-- ) {
            TempoTableEntry item = m_tempo_table.get( i );
            if ( item.clock < clock ) {
                double init = item.time;
                int dclock = clock - item.clock;
                double sec_per_clock1 = item.tempo * 1e-6 / 480.0;
                return init + dclock * sec_per_clock1;
            }
        }
        double sec_per_clock = 0.125 / m_tempo;
        return clock * sec_per_clock;
    }

    public void write( String file ) throws IOException{
        StreamWriter sw = new StreamWriter( file, "SJIS" );
        sw.writeLine( "[#SETTING]" );
        sw.writeLine( "Tempo=" + m_tempo );
        sw.writeLine( "Tracks=1" );
        sw.writeLine( "ProjectName=" + m_project_name );
        sw.writeLine( "VoiceDir=" + m_voice_dir );
        sw.writeLine( "OutFile=" + m_out_file );
        sw.writeLine( "CacheDir=" + m_cache_dir );
        sw.writeLine( "Tool1=" + m_tool1 );
        sw.writeLine( "Tool2=" + m_tool2 );
        UstTrack track0 = m_tracks.get( 0 );
        int c = track0.getEventCount();
        for ( int i = 0; i < c; i++ ) {
            track0.getEvent( i ).print( sw, i );
        }
        sw.writeLine( "[#TRACKEND]" );
        sw.close();
    }

    public Object clone(){
        UstFile ret = new UstFile();
        ret.m_tempo = m_tempo;
        ret.m_project_name = m_project_name;
        ret.m_voice_dir = m_voice_dir;
        ret.m_out_file = m_out_file;
        ret.m_cache_dir = m_cache_dir;
        ret.m_tool1 = m_tool1;
        ret.m_tool2 = m_tool2;
        int c = m_tracks.size();
        for ( int i = 0; i < c; i++ ) {
            ret.m_tracks.add( (UstTrack)m_tracks.get( i ).clone() );
        }
        ret.m_tempo_table = new Vector<TempoTableEntry>();
        c = m_tempo_table.size();
        for ( int i = 0; i < c; i++ ) {
            ret.m_tempo_table.add( (TempoTableEntry)m_tempo_table.get( i ).clone() );
        }
        return ret;
    }
}
