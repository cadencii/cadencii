/*
 * EmptyWaveGenerator.cs
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
namespace org.kbinani.cadencii.draft {

    /// <summary>
    /// 無音の波形を送信するWaveGenerator
    /// </summary>
    public class EmptyWaveGenerator : WaveUnit, WaveGenerator {
        private const int VERSION = 0;
        private const int BUFLEN = 1024;
        private WaveReceiver mReceiver = null;
        private bool mAbortRequested = false;

        public override int getVersion() {
            return VERSION;
        }

        public override void setConfig( string parameter ) {
            // do nothing
        }

        public void begin( long samples ) {
            if ( mReceiver == null ) return;
            double[] l = new double[BUFLEN];
            double[] r = new double[BUFLEN];
            for ( int i = 0; i < BUFLEN; i++ ) {
                l[i] = 0.0;
                r[i] = 0.0;
            }
            long remain = samples;
            while ( remain > 0 && !mAbortRequested ) {
                int amount = (remain > BUFLEN) ? BUFLEN : (int)remain;
                mReceiver.push( l, r, amount );
                remain -= amount;
            }
            mReceiver.end();
        }

        public void setReceiver( WaveReceiver receiver ) {
            mReceiver = receiver;
        }

        public void init( VsqFileEx vsq, int track, int start_clock, int end_clock ) {
        }

        public void stop() {
            mAbortRequested = true;
        }
    }

}
