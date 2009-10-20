/*
 * VsqMixer.java
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

import java.util.*;

/// <summary>
/// vsqファイルのメタテキストの[Mixer]セクションに記録される内容を取り扱う
/// </summary>
public class VsqMixer implements Cloneable {
    public int masterFeder;
    public int masterPanpot;
    public int masterMute;
    public int outputMode;

    /// <summary>
    /// vsqファイルの各トラックのfader, panpot, muteおよびoutputmode値を保持します
    /// </summary>
    public Vector<VsqMixerEntry> slave = new Vector<VsqMixerEntry>();

    public static String getGenericTypeName( String name ){
        if( name.equals( "slave" ) ){
            return "com.boare.vsq.VsqMixerEntry";
        }
        return "";
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "masterFeder" ) ){
            return "MasterFeder";
        }else if( name.equals( "masterPanpot" ) ){
            return "MasterPanpot";
        }else if( name.equals( "masterMute" ) ){
            return "MasterMute";
        }else if( name.equals( "outputMode" ) ){
            return "OutputMode";
        }else if( name.equals( "slave" ) ){
            return "Slave";
        }
        return name;
    }

    public Object clone() {
        VsqMixer res = new VsqMixer( masterFeder, masterPanpot, masterMute, outputMode );
        res.slave = new Vector<VsqMixerEntry>();
        int c = slave.size();
        for( int i = 0; i < c; i++ ){
            res.slave.add( (VsqMixerEntry)slave.get( i ).clone() );
        }
        return res;
    }

    /// <summary>
    /// 各パラメータを指定したコンストラクタ
    /// </summary>
    /// <param name="master_fader">MasterFader値</param>
    /// <param name="master_panpot">MasterPanpot値</param>
    /// <param name="master_mute">MasterMute値</param>
    /// <param name="output_mode">OutputMode値</param>
    public VsqMixer( int master_fader, int master_panpot, int master_mute, int output_mode ) {
        this.masterFeder = master_fader;
        this.masterMute = master_mute;
        this.masterPanpot = master_panpot;
        this.outputMode = output_mode;
        slave = new Vector<VsqMixerEntry>();
    }

    public VsqMixer(){
        this( 0, 0, 0, 0 );
    }

    /// <summary>
    /// テキストファイルからのコンストラクタ
    /// </summary>
    /// <param name="sr">読み込み対象</param>
    /// <param name="last_line">最後に読み込んだ行が返されます</param>
    public VsqMixer( TextMemoryStream sr, StringBuilder last_line ) {
        masterFeder = 0;
        masterPanpot = 0;
        masterMute = 0;
        outputMode = 0;
        //Tracks = 1;
        int tracks = 0;
        String[] spl;
        String buffer = "";
        last_line.setLength( 0 );
        last_line.append( sr.readLine() );
        while ( !last_line.toString().startsWith( "[" ) ) {
            spl = last_line.toString().split( "=" );
            String s = spl[0];
            if( s.equals( "MasterFeder" ) ){
                masterFeder = Integer.parseInt( spl[1] );
            }else if( s.equals( "MasterPanpot" ) ){
                masterPanpot = Integer.parseInt( spl[1] );
            }else if( s.equals( "MasterMute" ) ){
                masterMute = Integer.parseInt( spl[1] );
            }else if( s.equals( "OutputMode" ) ){
                outputMode = Integer.parseInt( spl[1] );
            }else if( s.equals( "Tracks" ) ){
                tracks = Integer.parseInt( spl[1] );
            }else{
                if ( spl[0].startsWith( "Feder" ) ||
                    spl[0].startsWith( "Panpot" ) ||
                    spl[0].startsWith( "Mute" ) ||
                    spl[0].startsWith( "Solo" ) ) {
                    buffer += spl[0] + "=" + spl[1] + "\n";
                }
            }
            if ( sr.peek() < 0 ) {
                break;
            }
            last_line.setLength( 0 );
            last_line.append( sr.readLine() );
        }

        slave = new Vector<VsqMixerEntry>();
        for ( int i = 0; i < tracks; i++ ) {
            slave.add( new VsqMixerEntry( 0, 0, 0, 0 ) );
        }
        spl = buffer.split( "\n"/*, StringSplitOptions.RemoveEmptyEntries*/ );
        String[] spl2;
        for ( int i = 0; i < spl.length; i++ ) {
            String ind = "";
            int index;
            spl2 = spl[i].split( "=" );
            if ( spl2[0].startsWith( "Feder" ) ) {
                ind = spl2[0].replace( "Feder", "" );
                index = Integer.parseInt( ind );
                slave.get( index ).feder = Integer.parseInt( spl2[1] );
            } else if ( spl2[0].startsWith( "Panpot" ) ) {
                ind = spl2[0].replace( "Panpot", "" );
                index = Integer.parseInt( ind );
                slave.get( index ).panpot = Integer.parseInt( spl2[1] );
            } else if ( spl2[0].startsWith( "Mute" ) ) {
                ind = spl2[0].replace( "Mute", "" );
                index = Integer.parseInt( ind );
                slave.get( index ).mute = Integer.parseInt( spl2[1] );
            } else if ( spl2[0].startsWith( "Solo" ) ) {
                ind = spl2[0].replace( "Solo", "" );
                index = Integer.parseInt( ind );
                slave.get( index ).solo = Integer.parseInt( spl2[1] );
            }

        }
    }

    /// <summary>
    /// このインスタンスをテキストファイルに出力します
    /// </summary>
    /// <param name="sw">出力対象</param>
    public void write( TextMemoryStream sw ) {
        sw.writeLine( "[Mixer]" );
        sw.writeLine( "MasterFeder=" + masterFeder );
        sw.writeLine( "MasterPanpot=" + masterPanpot );
        sw.writeLine( "MasterMute=" + masterMute );
        sw.writeLine( "OutputMode=" + outputMode );
        int c = slave.size();
        sw.writeLine( "Tracks=" + c );
        for ( int i = 0; i < c; i++ ) {
            VsqMixerEntry v = slave.get( i );
            sw.writeLine( "Feder" + i + "=" + v.feder );
            sw.writeLine( "Panpot" + i + "=" + v.panpot );
            sw.writeLine( "Mute" + i + "=" + v.mute );
            sw.writeLine( "Solo" + i + "=" + v.solo );
        }
    }
}
