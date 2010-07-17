/*
 * MonitorWaveReceiver.cs
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
#if JAVA
package org.kbinani.cadencii;

import java.util.*;
import org.kbinani.media.*;
#else
using org.kbinani.java.util;
using org.kbinani.media;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// スピーカへの出力を行う波形受信器
    /// </summary>
    public class MonitorWaveReceiver : WaveReceiver {
        private Vector<PassiveWaveSender> _passive_wave_sender = new Vector<PassiveWaveSender>();
        private boolean _first_call = true;

        public void push( double[] l, double[] r, int length ) {
            if ( _first_call ) {
                PlaySound.init();
                PlaySound.prepare( VSTiProxy.SAMPLE_RATE );
                _first_call = false;
            }
            PlaySound.append( l, r, length );
        }

        public void end() {
            PlaySound.exit();
        }

        public void addPassiveWaveSender( PassiveWaveSender s ) {
            if ( s == null ) {
                return;
            }
            if ( !_passive_wave_sender.contains( s ) ) {
                _passive_wave_sender.add( s );
            }
        }

        public void removePassiveWaveSender( PassiveWaveSender s ) {
            if ( s == null ) {
                return;
            }
            if ( _passive_wave_sender.contains( s ) ) {
                _passive_wave_sender.remove( s );
            }
        }

        public void clearPassiveWaveSender() {
            _passive_wave_sender.clear();
        }
    }

#if !JAVA
}
#endif
