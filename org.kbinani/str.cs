/*
 * str.cs
 * Copyright c 2011 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani;

import java.text.*;
import java.util.*;
#else
#if !__cplusplus
using System;
using System.IO;
using System.Collections.Generic;

#endif
namespace org
{
    namespace kbinani
    {
#if !__cplusplus
        using int8_t = System.SByte;
        using int16_t = System.Int16;
        using int32_t = System.Int32;
        using int64_t = System.Int64;
        using uint8_t = System.Byte;
        using uint16_t = System.UInt16;
        using uint32_t = System.UInt32;
        using uint64_t = System.UInt64;
#endif
#endif

#if __cplusplus
        class str
#else
        public class str
#endif
        {
            private str()
            {
            }
#if __cplusplus
        public:
#endif

#if JAVA
            public static boolean endsWith( String s, String search )
#elif __cplusplus
            static bool endsWith( string s, string search )
#else
            public static bool endsWith( string s, string search )
#endif
            {
#if JAVA
                return s.endsWith( search );
#elif __cplusplus
                string::size_type indx = s.rfind( search );
                if( indx == string::npos ){
                    return false;
                }else{
                    return (indx == s.length() - search.length());
                }
#else
                return s.EndsWith( search );
#endif
            }

#if JAVA
            public static boolean startsWith( String s, String search )
#elif __cplusplus
            static bool startsWith( string s, string search )
#else
            public static bool startsWith( string s, string search )
#endif
            {
#if JAVA
                return s.startsWith( search );
#elif __cplusplus
                return (s.find( search ) == 0);
#else
                return s.StartsWith( search );
#endif
            }

#if __cplusplus
            static string sub( string s, int start )
#else
            public static String sub( String s, int start )
#endif
            {
#if JAVA
                return s.substring( start );
#elif __cplusplus
                return s.substr( start );
#else
                return s.Substring( start );
#endif
            }

#if __cplusplus
            static string sub( string s, int start, int length )
#else
            public static String sub( String s, int start, int length )
#endif
            {
#if JAVA
                return s.substring( start, start + length );
#elif __cplusplus
                return s.substr( start, length );
#else
                return s.Substring( start, length );
#endif
            }

#if __cplusplus
            static int find( string s, string search )
#else
            public static int find( String s, String search )
#endif
            {
                return find( s, search, 0 );
            }


#if __cplusplus
            static int find( string s, string search, int index )
#else
            public static int find( String s, String search, int index )
#endif
            {
#if JAVA
                return s.indexOf( search, index );
#elif __cplusplus
                string::size_type indx = s.find( search, (string::size_type)index );
                if( indx == string::npos ){
                    return -1;
                }else{
                    return (int)indx;
                }
#else
                return s.IndexOf( search, index );
#endif
            }

#if JAVA
            public static int split( String s, Vector<String> dst, Vector<String> splitter, boolean ignore_empty )
#elif __cplusplus
            static int split( string s, vector<string> &dst, vector<string> &splitter, bool ignore_empty )
#else
            public static int split( string s, List<string> dst, List<string> splitter, bool ignore_empty )
#endif
            {
                int len = vec.size( splitter );
                vec.clear( dst );
                if ( len == 0 ) {
                    vec.add( dst, s );
                    return 1;
                }
#if JAVA
                String remain = s;
#else
                string remain = s;
#endif
                int index = find( remain, vec.get( splitter, 0 ), 0 );
                int i = 1;
                while ( index < 0 && i < len ) {
                    index = find( remain, vec.get( splitter, i ), 0 );
                    i++;
                }
                int added_count = 0;
                while ( index >= 0 ) {
                    if ( !ignore_empty || (ignore_empty && index > 0) ) {
                        vec.add( dst, sub( remain, 0, index ) );
                        added_count++;
                    }
                    remain = sub( remain, index + len );
                    index = find( remain, vec.get( splitter, 0 ) );
                    i = 1;
                    while ( index < 0 && i < len ) {
                        index = find( remain, vec.get( splitter, i ) );
                        i++;
                    }
                }
                if ( !ignore_empty || (ignore_empty && length( remain ) > 0) ) {
                    vec.add( dst, remain );
                }
                return added_count;
            }

#if JAVA
            public static int split( String s, Vector<String> dst, String s1, boolean ignore_empty )
#elif __cplusplus
            static int split( string s, vector<string> &dst, string s1, bool ignore_empty )
#else
            public static int split( string s, List<string> dst, string s1, bool ignore_empty )
#endif
            {
#if JAVA
                Vector<String> splitter = new Vector<String>();
#elif __cplusplus
                vector<string> splitter;
#else
                List<String> splitter = new List<string>();
#endif
                vec.add( splitter, s1 );
                return split( s, dst, splitter, ignore_empty );
            }

#if JAVA
            public static double tof( String s )
#elif __cplusplus
            static double tof( string s )
#else
            public static double tof( string s )
#endif
            {
#if JAVA
                return Double.parseDouble( s );
#elif __cplusplus
                return atof( s.c_str() );
#else
                return double.Parse( s );
#endif
            }

#if JAVA
            public static int toi( String s )
#elif __cplusplus
            static int toi( string s )
#else
            public static int toi( string s )
#endif
            {
#if JAVA
                return Integer.parseInt( s );
#elif __cplusplus
                return atoi( s.c_str() );
#else
                return int.Parse( s );
#endif
            }

#if JAVA
            public static int length( String a )
#elif __cplusplus
            static int length( string a )
#else
            public static int length( string a )
#endif
            {
#if JAVA
                return a.length();
#elif __cplusplus
                return a.length();
#else
                return a.Length;
#endif
            }

#if JAVA
            public static boolean compare( String a, String b )
#elif __cplusplus
            static bool compare( string a, string b )
#else
            public static bool compare( string a, string b )
#endif
            {
#if JAVA
                if ( a == null || b == null ) {
                    return false;
                }
                return a.equals( b );
#else
                return a == b;
#endif
            }

#if JAVA
            public static String format( long value, int digits )
#elif __cplusplus
            static string format( long value, int digits )
#else
            public static string format( long value, int digits )
#endif
            {
                return format( value, digits, 10 );
            }

#if JAVA
            public static String format( long value, int digits, int base_num )
#elif __cplusplus
            static string format( long value, int digits, int base_num )
#else
            public static string format( long value, int digits, int base_num )
#endif
            {
#if JAVA
                String format = "";
                for( int i = 0; i < digits; i++ ){
                    format += "0";
                }
                DecimalFormat df = new DecimalFormat( format );
                return df.format( value );
#elif __cplusplus
                char b[20] = "";
                string t = itoa( digits, b, 10 );
                string fmt = "%0" + t + (base_num == 16 ? "X" : "d");
                char *buf = new char[digits + 1];
                sprintf( buf, fmt.c_str(), value );
                string ret = buf;
                delete [] buf;
                return ret;
#else
                string format = "#";
                switch ( base_num ) {
                    case 10: {
                        format = "D" + digits;
                        break;
                    }
                    case 16: {
                        format = "X" + digits;
                        break;
                    }
                }
                return value.ToString( format );
#endif
            }
        };

#if !JAVA
    }
}
#endif
