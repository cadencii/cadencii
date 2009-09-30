/*
 * PlaySound.cs
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

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    static class PlaySound {
        private static bool s_initialized = false;
        private static int s_sample_rate = 44100;

        [DllImport( "PlaySound" )]
        private static extern void SoundInit( int block_size, int sample_rate );

        [DllImport( "PlaySound" )]
        private static extern unsafe void SoundAppend( double* left, double* right, int length );

        [DllImport( "PlaySound" )]
        private static extern void SoundWaitForExit();

        [DllImport( "PlaySound" )]
        private static extern double SoundGetPosition();

        [DllImport( "PlaySound" )]
        private static extern boolean SoundIsBusy();

        [DllImport( "PlaySound" )]
        private static extern void SoundReset();

        [DllImport( "PlaySound" )]
        private static extern void SoundSetResolution( int resolution );

        public static int SampleRate {
            get {
                return s_sample_rate;
            }
        }

        public static void SetResolution( int resolution ) {
            try {
                SoundSetResolution( resolution );
            } catch {
            }
        }

        public static void WaitForExit() {
            try {
                SoundWaitForExit();
            } catch {
            }
        }

        public static void Init( int block_size, int sample_rate ) {
            if ( s_initialized ) {
                return;
            }
            s_sample_rate = sample_rate;
            try {
                SoundInit( block_size, sample_rate );
                s_initialized = true;
            } catch ( Exception ex ){
                bocoree.debug.push_log( "PlaySound.Init; ex=" + ex );
            }
        }

        public static unsafe void Append( double[] left, double[] right, int length ) {
            if ( length <= 0 ) {
                return;
            }
            try{
                fixed ( double* pl = &left[0] )
                fixed ( double* pr = &right[0] ) {
                    SoundAppend( pl, pr, length );
                }
            } catch ( Exception ex ) {
                bocoree.debug.push_log( "PlaySound#Append; ex=" + ex );
            }
        }

        public static double GetPosition() {
            double ret = -1;
            try {
                ret = SoundGetPosition();
            } catch ( Exception ex ) {
#if DEBUG
                bocoree.debug.push_log( "PlaySound.GetPosition; ex=" + ex );
#endif
            }
            return ret;
        }

        public static void Reset() {
            try {
                SoundReset();
            } catch ( Exception ex ) {
#if DEBUG
                bocoree.debug.push_log( "PlaySound.Reset; ex=" + ex );
#endif
            }
        }
    }

}
