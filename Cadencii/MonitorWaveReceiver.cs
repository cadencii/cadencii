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

import java.awt.*;
import java.util.*;
import org.kbinani.media.*;
#else
using System;
using org.kbinani.java.awt;
using org.kbinani.java.util;
using org.kbinani.media;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// スピーカへの出力を行う波形受信器
    /// </summary>
#if JAVA
    public class MonitorWaveReceiver extends WaveUnit implements WaveReceiver {
#else
    public class MonitorWaveReceiver : WaveUnit, WaveReceiver {
#endif
        private const int _BUFLEN = 1024;

        private static MonitorWaveReceiver _singleton = null;

        private boolean _first_call = true;
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private double[] _buffer2_l = new double[_BUFLEN];
        private double[] _buffer2_r = new double[_BUFLEN];
        private WaveReceiver _receiver = null;
        private int _version = 0;

        private MonitorWaveReceiver() {
        }

        public static MonitorWaveReceiver getInstance() {
            if ( _singleton == null ) {
                _singleton = new MonitorWaveReceiver();
            }
            _singleton.end();
            _singleton._first_call = true;
            return _singleton;
        }

        public override void setConfig( String parameter ) {
            // do nothing
        }

        public override int getVersion() {
            return _version;
        }

        public void setReceiver( WaveReceiver r ) {
            if ( _receiver != null ) {
                _receiver.end();
            }
            _receiver = r;
        }

        public void push( double[] l, double[] r, int length ) {
            if ( _first_call ) {
                PlaySound.init();
                PlaySound.prepare( VSTiProxy.SAMPLE_RATE );
                _first_call = false;
            }
            PlaySound.append( l, r, length );
            if ( _receiver != null ) {
                _receiver.push( l, r, length );
            }
        }

        public void end() {
            PlaySound.exit();
            if ( _receiver != null ) {
                _receiver.end();
            }
        }
    }

#if !JAVA
}
#endif
