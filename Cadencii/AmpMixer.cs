/*
 * AmpMixer.cs
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
#else
using org.kbinani.java.util;

namespace org.kbinani.cadencii{
#endif

    /// <summary>
    /// 増幅器＆ミキサーの実装
    /// </summary>
#if JAVA
    public class AmpMixer implements WaveSender, WaveReceiver {
#else
    public class AmpMixer : WaveSender, WaveReceiver {
#endif
        private const int _BUFLEN = 1024;
        private Vector<PassiveWaveSender> _passive_wave_senders = new Vector<PassiveWaveSender>();
        private Vector<WaveReceiver> _receivers = new Vector<WaveReceiver>();
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private double _amp_l = 1.0;
        private double _amp_r = 1.0;
        private double[] _buffer2_l = new double[_BUFLEN];
        private double[] _buffer2_r = new double[_BUFLEN];
        private long _position = 0;

        public long getPosition() {
            return _position;
        }

        public void clearPassiveWaveSender() {
            _passive_wave_senders.clear();
        }

        public void begin( long samples ) {
            // do nothing
        }

        public void end() {
            foreach ( PassiveWaveSender s in _passive_wave_senders ) {
                s.end();
            }
            foreach ( WaveReceiver r in _receivers ) {
                r.end();
            }
        }

        public void clearReceiver() {
            _receivers.clear();
        }

        public void setAmplify( double amp_left, double amp_right ) {
            _amp_l = amp_left;
            _amp_r = amp_right;
        }

        public void addPassiveWaveSender( PassiveWaveSender g ) {
            if ( g == null ) {
                return;
            }
            if ( !_passive_wave_senders.contains( g ) ) {
                _passive_wave_senders.add( g );
            }
        }

        public void removePassiveWaveSender( PassiveWaveSender g ) {
            if ( g == null ) {
                return;
            }
            if ( _passive_wave_senders.contains( g ) ) {
                _passive_wave_senders.remove( g );
            }
        }

        public void addReceiver( WaveReceiver r ) {
            if ( r == null ) {
                return;
            }
            if ( !_receivers.contains( r ) ) {
                _receivers.add( r );
            }
        }

        public void removeReceiver( WaveReceiver r ) {
            if ( r == null ) {
                return;
            }
            if ( _receivers.contains( r ) ) {
                _receivers.remove( r );
            }
        }

        public void push( double[] l, double[] r, int length ) {
            if ( _receivers.size() <= 0 ) {
                return;
            }

            int remain = length;
            while ( remain > 0 ) {
                int amount = (remain > _BUFLEN) ? _BUFLEN : remain;
                for ( int i = 0; i < amount; i++ ) {
                    _buffer_l[i] = 0.0;
                    _buffer_r[i] = 0.0;
                }
                if ( _amp_l != 0.0 || _amp_r != 0.0 ) {
                    // 左右どちらかのチャンネルの増幅率が0でない場合にのみ、波形を取にいく
                    foreach ( PassiveWaveSender sender in _passive_wave_senders ) {
                        sender.pull( _buffer2_l, _buffer2_r, amount );
                        for ( int i = 0; i < amount; i++ ) {
                            _buffer_l[i] += _buffer2_l[i];
                            _buffer_r[i] += _buffer2_r[i];
                        }
                    }
                }
                int offset = length - remain;

                // 左チャンネル
                if ( _amp_l == 0.0 ) {
                    // 増幅率0の場合
                    for ( int i = 0; i < amount; i++ ) {
                        _buffer2_l[i] = 0.0;
                    }
                } else {
                    if ( _amp_l == 1.0 ) {
                        // 増幅率1の場合
                        for ( int i = 0; i < amount; i++ ) {
                            _buffer2_l[i] = l[i + offset] + _buffer_l[i];
                        }
                    } else {
                        for ( int i = 0; i < amount; i++ ) {
                            _buffer2_l[i] = (l[i + offset] + _buffer_l[i]) * _amp_l;
                        }
                    }
                    for ( int i = 0; i < amount; i++ ) {
                        if ( _buffer2_l[i] > 1.0 ) {
                            _buffer2_l[i] = 1.0;
                        } else if ( _buffer2_l[i] < -1.0 ) {
                            _buffer2_l[i] = -1.0;
                        }
                    }
                }

                // 右チャンネル
                if ( _amp_r == 0.0 ) {
                    // 増幅率0の場合
                    for ( int i = 0; i < amount; i++ ) {
                        _buffer2_l[i] = 0.0;
                    }
                } else {
                    if ( _amp_r == 1.0 ) {
                        // 増幅率1の場合
                        for ( int i = 0; i < amount; i++ ) {
                            _buffer2_r[i] = r[i + offset] + _buffer_r[i];
                        }
                    } else {
                        for ( int i = 0; i < amount; i++ ) {
                            _buffer2_r[i] = (r[i + offset] + _buffer_r[i]) * _amp_r;
                        }
                    }
                    for ( int i = 0; i < amount; i++ ) {
                        if ( _buffer2_r[i] > 1.0 ) {
                            _buffer2_r[i] = 1.0;
                        } else if ( _buffer2_r[i] < -1.0 ) {
                            _buffer2_r[i] = -1.0;
                        }
                    }
                }

                remain -= amount;
                _position += amount;
                foreach ( WaveReceiver receiver in _receivers ) {
                    for ( int i = 0; i < amount; i++ ) {
                        _buffer_l[i] = _buffer2_l[i];
                        _buffer_r[i] = _buffer2_r[i];
                    }
                    receiver.push( _buffer_l, _buffer_r, amount );
                }
            }
        }

        public void pull( double[] r, double[] l, int length ) {
            for ( int i = 0; i < length; i++ ){
                r[i] = 0.0;
                l[i] = 0.0;
            }
            if ( _passive_wave_senders.size() > 0 ) {
                int remain = length;
                while ( remain > 0 ) {
                    int amount = (remain > _BUFLEN) ? _BUFLEN : remain;
                    int offset = length - remain;
                    foreach ( PassiveWaveSender sender in _passive_wave_senders ) {
                        sender.pull( _buffer_l, _buffer_r, amount );
                        for ( int i = 0; i < amount; i++ ) {
                            l[i + offset] += _buffer_l[i];
                            r[i + offset] += _buffer_r[i];
                        }
                    }
                    remain -= amount;
                }
            }
            if ( _amp_l != 1.0 ) {
                for ( int i = 0; i < length; i++ ) {
                    l[i] *= _amp_l;
                }
            }
            if ( _amp_r != 1.0 ) {
                for ( int i = 0; i < length; i++ ) {
                    r[i] *= _amp_r;
                }
            }
            for ( int i = 0; i < length; i++ ) {
                if ( l[i] > 1.0 ) {
                    l[i] = 1.0;
                } else if (l[i] < -1.0 ) {
                    l[i] = -1.0;
                }
                if ( r[i] > 1.0 ) {
                    r[i] = 1.0;
                } else if ( r[i] < -1.0 ) {
                    r[i] = -1.0;
                }
            }
        }
    }

#if !JAVA
}
#endif
