/*
 * Base64.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii;

public class Base64{
    static final char TABLE[] = {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
        'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
        'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
        'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f',
        'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
        'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
        'w', 'x', 'y', 'z', '0', '1', '2', '3',
        '4', '5', '6', '7', '8', '9', '+', '/'
    };

    private Base64(){
    }

    public static String encode( byte[] value ){
        StringBuilder ret = new StringBuilder();
        int buf = 0;
        int filled = 0;  // bufに格納されているビット数
        int count = 0;
        for( int i = 0; i < value.length; i++ ){
            int v = value[i] & 0xff;
            if( filled == 0 ){
                buf = 0x3f & (v >>> 2);
                ret.append( TABLE[buf] );
                buf = 0x3f & (v << 4);
                filled = 2;
                count++;
            }else if( filled == 2 ){
                buf = buf | (v >>> 4);
                ret.append( TABLE[buf] );
                buf = 0x3f & (v << 2);
                filled = 4;
                count++;
            }else if( filled == 4 ){
                buf = buf | (v >>> 6);
                ret.append( TABLE[buf] );
                buf = 0x3f & v;
                ret.append( TABLE[buf] );
                filled = 0;
                count += 2;
            }
        }
        if( filled > 0 ){
            ret.append( TABLE[buf] );
            count++;
        }
        int r = count & 0x03;
        if( r != 0 ){
            for( int i = 0; i < 4 - r; i++ ){
                ret.append( "=" );
            }
        }
        return ret.toString();
    }

    public static byte[] decode( String value ){
        int index = value.lastIndexOf( '=' );
        int len = 0;
        int c = value.length();
        if( index < 0 ){
            len = c * 6;
        }else{
            len = index * 6;
        }
        int total;
        if( (len & 0x07) == 0 ){
            total = len >>> 3;
        }else{
            total = (len >>> 3) + 1;
        }
        byte[] ret = new byte[total];
        int buf = 0;
        int filled = 0;
        index = 0;
        for( int i = 0; i < c; i++ ){
            char ch = value.charAt( i );
            if( ch == '=' ){
                break;
            }
            int v = decodeUnit( ch );
            if( filled == 0 ){
                buf = v << 2;
                filled = 6;
            }else if( filled == 6 ){
                buf = buf | (v >>> 4);
                ret[index] = (byte)buf;
                buf = v << 4;
                filled = 4;
                index++;
            }else if( filled == 4 ){
                buf = buf | (v >>> 2);
                ret[index] = (byte)buf;
                buf = v << 6;
                filled = 2;
                index++;
            }else if( filled == 2 ){
                buf = buf | v;
                ret[index] = (byte)buf;
                buf = 0;
                filled = 0;
                index++;
            }
        }
        if( filled > 0 ){
            ret[index] = (byte)buf;
        }
        return ret;
    }

    private static int decodeUnit( char c ){
        int code = (int)c;
        if( 97 <= code ){
            return code - 71;
        }else if( 65 <= code ){
            return code - 65;
        }else if( 48 <= code ){
            return code + 4;
        }else if( code == 43 ){
            return 62;
        }else{
            return 63;
        }
    }
}
#else
using System;

namespace cadencii {
    public static class Base64 {
        public static string encode( byte[] value ) {
            return Convert.ToBase64String( value );
        }

        public static byte[] decode( string value ) {
            return Convert.FromBase64String( value );
        }
    }
}
#endif
