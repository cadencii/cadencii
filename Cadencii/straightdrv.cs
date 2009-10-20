/*
 * straightdrv.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Runtime.InteropServices;
using System.Text;
using bocoree;

namespace Boare.Cadencii {

    public static class straightdrv {
        static bool first_init_call = true;

        [DllImport( "straightdrv.dll")]
        private static extern int straightdrvInit( string text, string oto_ini );
        [DllImport( "straightdrv.dll" )]
        private static extern double straightdrvGetProgress();
        [DllImport( "straightdrv.dll" )]
        private static extern unsafe double* straightdrvSynthesize( int* length );
        [DllImport( "straightdrv.dll" )]
        private static extern void straightdrvAbort();
        [DllImport( "straightdrv.dll" )]
        private static extern void straightdrvUninitialize();

        public static void uninitialize() {
            try {
                straightdrvUninitialize();
            } catch ( Exception ex ) {
#if DEBUG
                PortUtil.println( "straightdrv#uninitialize; ex=" + ex );
#endif
            }
        }

        public static void abort() {
            try {
                straightdrvAbort();
            } catch ( Exception ex ) {
#if DEBUG
                PortUtil.println( "straightdrv#abort; ex=" + ex );
#endif
            }
        }

        public static double[] synthesize() {
            unsafe {
                try {
                    int length;
                    double* ret = straightdrvSynthesize( &length );
#if DEBUG
                    PortUtil.println( "straightdrv#synthesize; length=" + length );
#endif
                    double[] buf = new double[length];
                    for ( int i = 0; i < length; i++ ) {
                        buf[i] = ret[i];
                    }
                    return buf;
                } catch ( Exception ex ) {
#if DEBUG
                    PortUtil.println( "straightdrv#synthesize; ex=" + ex );
#endif
                    return new double[0];
                }
            }
        }

        public static bool init( string text, string oto_ini ) {
            try {
                if ( straightdrvInit( text, oto_ini ) == 0 ) {
                    return false;
                } else {
                    return true;
                }
            } catch ( Exception ex ) {
#if DEBUG
                PortUtil.println( "straightdrv#init; ex=" + ex );
#endif
                return false;
            }
        }

        public static double getProgress() {
            try {
                return straightdrvGetProgress();
            } catch ( Exception ex ) {
#if DEBUG
                PortUtil.println( "straightdrv#getProgress; ex=" + ex );
#endif
                return 0.0;
            }
        }
    }

}
