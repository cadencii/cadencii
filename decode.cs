/*
 * SingerConfig.cs
 * Copyright (c) 2008-2009 kbinani
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
using System;
using System.IO;
using System.Collections.Generic;

public class decode {
    public static void Main( string[] args ){
        if( args.Length <= 1 ){
            return;
        }
        string file = args[0];
        string result = args[1];
        FileStream fs = new FileStream( file, FileMode.Open, FileAccess.Read );
        byte[] buf = new byte[fs.Length];
        fs.Read( buf, 0, buf.Length );
        fs.Close();
        decode_vvd_bytes( ref buf );
        for ( int i = 0; i < buf.Length - 1; i++ ) {
            if ( buf[i] == 0x17 && buf[i + 1] == 0x10 ) {
                buf[i] = 0x0d;
                buf[i + 1] = 0x0a;
            }
        }
        FileStream fsout = new FileStream( result, FileMode.Create );
        fsout.Write( buf, 0, buf.Length );
        fsout.Close();
    }

    public static void decode_vvd_bytes( ref byte[] dat ) {
        for ( int i = 0; i < dat.Length; i++ ) {
            byte M = (byte)(dat[i] >> 4);
            byte L = (byte)(dat[i] - (M << 4));
            byte newM = endecode_vvd_m( M );
            byte newL = endecode_vvd_l( L );
            dat[i] = (byte)((newM << 4) | newL);
        }
    }

    static byte endecode_vvd_l( byte value ) {
        switch ( value ) {
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
        switch ( value ) {
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
}
