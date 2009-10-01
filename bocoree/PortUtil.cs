/*
 * PortUtil.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package com.boare;

import java.util.*;
#else
using System;
using System.Collections.Generic;

namespace bocoree {
    using boolean = System.Boolean;
#endif
    public class PortUtil {
#if JAVA
        private final int INT_MAX = Integer.MAX_VALUE;
#else
        private const int INT_MAX = int.MaxValue;
#endif

        public static String[] listFiles( String directory, String search ) {
#if JAVA

#else
            return System.IO.Directory.GetFiles( directory, search );
#endif
        }

        public static void deleteFile( String path ) {
#if JAVA
            new File( path ).delete();
#else
            System.IO.File.Delete( path );
#endif
        }

        public static boolean isFileExists( String path ) {
#if JAVA
            return (new File( path )).exists();
#else
            return System.IO.File.Exists( path );
#endif
        }

        public static String combinePath( String path1, String path2 ) {
#if JAVA
            if( path1.endsWith( File.separator ) ){
                path1 = path1.substring( 0, path1.length() - 1 );
            }
            if( path2.startsWith( File.separator ) ){
                path2 = path2.substring( 1 );
            }
            return path1 + File.separator + path2;
#else
            return System.IO.Path.Combine( path1, path2 );
        }
#endif

        public static int parseInt( String value ) {
#if JAVA
            return Integer.parseInt( value );
#else
            return int.Parse( value );
#endif
        }

        public static float parseFloat( String value ) {
#if JAVA
            return Float.parseFloat( value );
#else
            return float.Parse( value );
#endif
        }

        public static double parseDouble( String value ) {
#if JAVA
            return Double.parseDouble( value );
#else
            return double.Parse( value );
#endif
        }

        public static String[] splitString( String s, params char[] separator ) {
            return splitStringCorB( s, separator, INT_MAX, false );
        }

        public static String[] splitString( String s, char[] separator, int count ) {
            return splitStringCorB( s, separator, count, false );
        }

        public static String[] splitString( String s, char[] separator, boolean ignore_empty_entries ) {
            return splitStringCorB( s, separator, INT_MAX, ignore_empty_entries );
        }

        public static String[] splitString( String s, String[] separator, boolean ignore_empty_entries ) {
            return splitStringCorA( s, separator, INT_MAX, ignore_empty_entries );
        }

        public static String[] splitString( String s, char[] separator, int count, boolean ignore_empty_entries ) {
            return splitStringCorB( s, separator, count, ignore_empty_entries );
        }

        public static String[] splitString( String s, String[] separator, int count, boolean ignore_empty_entries ) {
            return splitStringCorA( s, separator, count, ignore_empty_entries );
        }

        private static String[] splitStringCorB( String s, char[] separator, int count, boolean ignore_empty_entries ) {
#if JAVA
            int length = separator.length;
#else
            int length = separator.Length;
#endif
            String[] spl = new String[length];
            for ( int i = 0; i < length; i++ ) {
                spl[i] = separator[i] + "";
            }
            return splitStringCorA( s, spl, count, false );
        }
        
        private static String[] splitStringCorA( String s, String[] separator, int count, boolean ignore_empty_entries ) {
#if JAVA
            if( separator.length == 0 ){
                return new String[]{ s };
            }
            Vector<String> ret = new Vector<String>();
            String remain = s;
            int len = separator.length();
            int index = remain.indexOf( separator[0] );
            int i = 1;
            while( index < 0 && i < separator.length ){
                index = remain.indexOf( separator[i] );
            }
            int added_count = 0;
            while( index >= 0 ){
                if( !ignore_empty_entries || (ignore_empty_entries && index > 0) ){
                    if( added_count + 1 == count ){
                        break;
                    }else{
                        ret.add( remain.substring( 0, index ) );
                    }
                    added_count++;
                }
                remain = remain.substring( index + len );
                index = remain.indexOf( separator[0] );
                i = 1;
                while( index < 0 && i < separator.length ){
                    index = remain.indexOf( separator[i] );
                }
            }
            if( !ignore_empty_entries || (ignore_empty_entries && remain.length() > 0) ){
                ret.add( remain );
            }
            return ret.toArray( new String[]{} );
#else
            return s.Split( separator, count, (ignore_empty_entries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None) );
#endif
        }
    }
#if !JAVA
}
#endif
