#if ENABLE_VOCALOID
/*
 * DllLoad.cs
 * Copyright (C) 2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Runtime.InteropServices;

namespace org.kbinani.cadencii{
    using boolean = System.Boolean;

    public class DllLoad {
        [DllImport( "util" )]
        private static extern boolean IsInitialized();
        [DllImport( "util" )]
        private static extern void InitializeDllLoad();
        [DllImport( "util" )]
        private static extern void KillDllLoad();
        [DllImport( "util" )]
        private static extern IntPtr GetDllProcAddress( IntPtr hModule, string lpProcName );
        [DllImport( "util" )]
        private static extern IntPtr LoadDllW( [MarshalAs( UnmanagedType.LPWStr )]string lpLibFileName );
        [DllImport( "util" )]
        private static extern IntPtr LoadDllA( [MarshalAs( UnmanagedType.LPStr )]string lpLibFileName );
        [DllImport( "util" )]
        private static extern boolean FreeDll( IntPtr hLibModule );

        private DllLoad(){
        }

        public static void terminate() {
            try {
                KillDllLoad();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "DllLoad#terminate; ex=" + ex );
            }
        }

        public static IntPtr getProcAddress( IntPtr hModule, string lpProcName ) {
            try {
                return GetDllProcAddress( hModule, lpProcName );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "DllLoad#getProcAddress; ex=" + ex );
            }
            return IntPtr.Zero;
        }

        public static boolean isInitialized() {
            try {
                return IsInitialized();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "DllLoad#isInitialized; ex=" + ex );
            }
            return false;
        }

        public static void initialize() {
            try {
                InitializeDllLoad();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "DllLoad#initialize; ex=" + ex );
            }
        }

        public static IntPtr loadDll( string lpLibFileName ) {
            IntPtr ret = IntPtr.Zero;
            try {
                ret = LoadDllA( lpLibFileName );
                return ret;
            } catch ( EntryPointNotFoundException ex ) {
                ret = IntPtr.Zero;
            } catch ( Exception ex1 ) {
                PortUtil.stderr.println( "DllLoad#loadDll; ex1=" + ex1 );
            }
            if ( ret == IntPtr.Zero ) {
                try {
                    ret = LoadDllW( lpLibFileName );
                } catch ( Exception ex ) {
                    ret = IntPtr.Zero;
                    PortUtil.stderr.println( "DllLoad#loadDll; ex=" + ex );
                }
            }
            return ret;
        }

        public static boolean freeDll( IntPtr hModule ) {
            try {
                return FreeDll( hModule );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "DllLoad#freeDll; ex=" + ex );
            }
            return false;
        }
    }

}
#endif