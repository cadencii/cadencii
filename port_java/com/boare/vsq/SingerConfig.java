/*
 * SingerConfig.java
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

import java.io.*;
import java.util.*;

public class SingerConfig implements Cloneable {
    public String ID = "";
    public String FORMAT = "";
    public String  VOICEIDSTR = "";
    public String VOICENAME = "Unknown";
    public int breathiness;
    public int brightness;
    public int clearness;
    public int opening;
    public int genderFactor;
    public int original;
    public int program;

    public SingerConfig() {
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "breathiness" ) ){
            return "Breathiness";
        }else if( name.equals( "brightness" ) ){
            return "Brightness";
        }else if( name.equals( "clearness" ) ){
            return "Clearness";
        }else if( name.equals( "opening" ) ){
            return "Opening";
        }else if( name.equals( "genderFactor" ) ){
            return "GenderFacgor";
        }else if( name.equals( "original" ) ){
            return "Original";
        }else if( name.equals( "program" ) ){
            return "Program";
        }
        return name;
    }

    public Object clone() {
        SingerConfig ret = new SingerConfig();
        ret.ID = ID;
        ret.FORMAT = FORMAT;
        ret.VOICEIDSTR = VOICEIDSTR;
        ret.VOICENAME = VOICENAME;
        ret.breathiness = breathiness;
        ret.brightness = brightness;
        ret.clearness = clearness;
        ret.opening = opening;
        ret.genderFactor = genderFactor;
        ret.original = original;
        ret.program = program;
        return ret;
    }

    public static void decode_vvd_bytes( byte[] dat ) {
        for ( int i = 0; i < dat.length; i++ ) {
            byte M = (byte)((0xff & dat[i]) >>> 4);
            byte L = (byte)(dat[i] - ((0xff & M) << 4));
            byte newM = endecode_vvd_m( M );
            byte newL = endecode_vvd_l( L );
            dat[i] = (byte)(((newM & 0xff) << 4) | (0xff & newL));
        }
    }

    static byte endecode_vvd_l( byte value ) {
        switch ( 0xff & value ) {
            case 0x0:
                return 0xa;
            case 0x1:
                return 0xb;
            case 0x2:
                return 0x8;
            case 0x3:
                return 0x9;
            case 0x4:
                return 0xe;
            case 0x5:
                return 0xf;
            case 0x6:
                return 0xc;
            case 0x7:
                return 0xd;
            case 0x8:
                return 0x2;
            case 0x9:
                return 0x3;
            case 0xa:
                return 0x0;
            case 0xb:
                return 0x1;
            case 0xc:
                return 0x6;
            case 0xd:
                return 0x7;
            case 0xe:
                return 0x4;
            case 0xf:
                return 0x5;
        }
        return 0x0;
    }

    static byte endecode_vvd_m( byte value ) {
        switch ( 0xff & value ) {
            case 0x0:
                return 0x1;
            case 0x1:
                return 0x0;
            case 0x2:
                return 0x3;
            case 0x3:
                return 0x2;
            case 0x4:
                return 0x5;
            case 0x5:
                return 0x4;
            case 0x6:
                return 0x7;
            case 0x7:
                return 0x6;
            case 0x8:
                return 0x9;
            case 0x9:
                return 0x8;
            case 0xa:
                return 0xb;
            case 0xb:
                return 0xa;
            case 0xc:
                return 0xd;
            case 0xd:
                return 0xc;
            case 0xe:
                return 0xf;
            case 0xf:
                return 0xe;
        }
        return 0x0;
    }

    public static SingerConfig readSingerConfig( String file, int original ) throws IOException{
        SingerConfig sc = new SingerConfig();
        //original = original;
        sc.ID = "VOCALOID:VIRTUAL:VOICE";
        sc.FORMAT = "2.0.0.0";
        sc.VOICEIDSTR = "";
        sc.VOICENAME = "Miku";
        sc.breathiness = 0;
        sc.brightness = 0;
        sc.clearness = 0;
        sc.opening = 0;
        sc.genderFactor = 0;
        sc.original = original; //original = 0;
        RandomAccessFile fs = null;
        try {
            fs = new RandomAccessFile( file, "r" );
            int length = (int)fs.length();
            byte[] dat = new byte[length];
            fs.read( dat, 0, length );
            decode_vvd_bytes( dat );
            for ( int i = 0; i < dat.length - 1; i++ ) {
                if ( (0xff & dat[i]) == 0x17 && (0xff & dat[i + 1]) == 0x10 ) {
                    dat[i] = 0x0d;
                    dat[i + 1] = 0x0a;
                }
            }
            String str = new String( dat, "SJIS" );
            String crlf = (char)0x0d + "" + (char)0x0a;
            String[] spl = str.split( crlf );

            for ( String s : spl ) {
//System.out.println( "s=" + s );
                int first = s.indexOf( "\"" );
                int first_end = get_quated_string( s, first );
                int second = s.indexOf( "\"", first_end + 1 );
                int second_end = get_quated_string( s, second );
                char[] chs = s.toCharArray();
                String id = new String( chs, first, first_end - first + 1 );
                String value = new String( chs, second, second_end - second + 1 );
                id = id.substring( 1, id.length() - 1 );
                value = value.substring( 1, value.length() - 1 );
                value = value.replace( "\\\"", "\"" );
//System.out.println( "id=" + id + "; value=" + value );
                if ( id.equals( "ID" ) ) {
                    sc.ID = value;
                } else if ( id.equals( "FORMAT" ) ) {
                    sc.FORMAT = value;
                } else if ( id.equals( "VOICEIDSTR" ) ) {
                    sc.VOICEIDSTR = value;
                } else if ( id.equals( "VOICENAME" ) ) {
                    sc.VOICENAME = value;
                } else if ( id.equals( "Breathiness" ) ) {
                    try {
                        sc.breathiness = Integer.parseInt( value );
                    } catch( Exception ex ){
                    }
                } else if ( id.equals( "Brightness" ) ) {
                    try {
                        sc.brightness = Integer.parseInt( value );
                    } catch( Exception ex ){
                    }
                } else if ( id.equals( "Clearness" ) ) {
                    try {
                        sc.clearness = Integer.parseInt( value );
                    } catch( Exception ex ){
                    }
                } else if ( id.equals( "Opening" ) ) {
                    try {
                        sc.opening = Integer.parseInt( value );
                    } catch( Exception ex ){
                    }
                } else if ( id.equals( "Gender:Factor" ) ) {
                    try {
                        sc.genderFactor = Integer.parseInt( value );
                    } catch( Exception ex ){
                    }
                }
            }
        } catch( Exception ex ){
            System.out.println( "SingerConfig.readSingerConfig; ex=" + ex );
        }
        if ( fs != null ) {
            fs.close();
        }
        return sc;
    }

    /// <summary>
    /// 位置positionにある'"'から，次に現れる'"'の位置を調べる．エスケープされた\"はスキップされる．'"'が見つからなかった場合-1を返す
    /// </summary>
    /// <param name="s"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    static int get_quated_string( String s, int position ) {
        if ( position < 0 ) {
            return -1;
        }
        char[] chs = s.toCharArray();
        if ( position >= chs.length ) {
            return -1;
        }
        if ( chs[position] != '"' ) {
            return -1;
        }
        int end = -1;
        for ( int i = position + 1; i < chs.length; i++ ) {
            if ( chs[i] == '"' && chs[i - 1] != '\\' ) {
                end = i;
                break;
            }
        }
        return end;
    }

    public String[] toStringArray() {
        Vector<String> ret = new Vector<String>();
        ret.add( "\"ID\":=:\"" + ID + "\"" );
        ret.add( "\"FORMAT\":=:\"" + FORMAT + "\"" );
        ret.add( "\"VOICEIDSTR\":=:\"" + VOICEIDSTR + "\"" );
        ret.add( "\"VOICENAME\":=:\"" + VOICENAME.replace( "\"", "\\\"" ) + "\"" );
        ret.add( "\"Breathiness\":=:\"" + breathiness + "\"" );
        ret.add( "\"Brightness\":=:\"" + brightness + "\"" );
        ret.add( "\"Clearness\":=:\"" + clearness + "\"" );
        ret.add( "\"Opening\":=:\"" + opening + "\"" );
        ret.add( "\"Gender:Factor\":=:\"" + genderFactor + "\"" );
        return ret.toArray( new String[]{} );
    }

    public String toString() {
        String[] r = toStringArray();
        String ret = "";
        for ( String s :  r ) {
            ret += s + "\n";
        }
        return ret;
    }
}
