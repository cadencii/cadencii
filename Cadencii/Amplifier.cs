/*
 * Amplifier.cs
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
    public class Amplifier implements WaveSender, WaveReceiver {
#else
    public class Amplifier : WaveSender, WaveReceiver {
#endif
        private const int _BUFLEN = 1024;
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private double _amp_l = 1.0;
        private double _amp_r = 1.0;
        private long _position = 0;
        private WaveReceiver _receiver = null;
        private WaveSender _sender = null;

        public void setReceiver( WaveReceiver r ) {
            if ( _receiver != null ) {
                _receiver.end();
            }
            _receiver = r;
        }

        public void setSender( WaveSender s ) {
            if ( _sender != null ) {
                _sender.end();
            }
            _sender = s;
        }

        public long getPosition() {
            return _position;
        }

        public void end() {
            if ( _receiver != null ) {
                _receiver.end();
            }
            if ( _sender != null ) {
                _sender.end();
            }
        }

        public void setAmplify( double amp_left, double amp_right ) {
            _amp_l = amp_left;
            _amp_r = amp_right;
        }

        public void push( double[] l, double[] r, int length ) {
            if ( _receiver == null ) {
                _position += length;
                return;
            }

            int remain = length;
            while ( remain > 0 ) {
                int amount = (remain > _BUFLEN) ? _BUFLEN : remain;
                for ( int i = 0; i < amount; i++ ) {
                    _buffer_l[i] = 0.0;
                    _buffer_r[i] = 0.0;
                }
                int offset = length - remain;

                // 左チャンネル
                if ( _amp_l != 0.0 ) {
                    if ( _amp_l == 1.0 ) {
                        // 増幅率1の場合
                        for ( int i = 0; i < amount; i++ ) {
                            _buffer_l[i] = l[i + offset];
                        }
                    } else {
                        for ( int i = 0; i < amount; i++ ) {
                            _buffer_l[i] = l[i + offset] * _amp_l;
                        }
                    }
                    for ( int i = 0; i < amount; i++ ) {
                        if ( _buffer_l[i] > 1.0 ) {
                            _buffer_l[i] = 1.0;
                        } else if ( _buffer_l[i] < -1.0 ) {
                            _buffer_l[i] = -1.0;
                        }
                    }
                }

                // 右チャンネル
                if ( _amp_r != 0.0 ) {
                    if ( _amp_r == 1.0 ) {
                        // 増幅率1の場合
                        for ( int i = 0; i < amount; i++ ) {
                            _buffer_r[i] = r[i + offset];
                        }
                    } else {
                        for ( int i = 0; i < amount; i++ ) {
                            _buffer_r[i] = r[i + offset] * _amp_r;
                        }
                    }
                    for ( int i = 0; i < amount; i++ ) {
                        if ( _buffer_r[i] > 1.0 ) {
                            _buffer_r[i] = 1.0;
                        } else if ( _buffer_r[i] < -1.0 ) {
                            _buffer_r[i] = -1.0;
                        }
                    }
                }

                remain -= amount;
                _position += amount;
                _receiver.push( _buffer_l, _buffer_r, amount );
            }
        }

        public void pull( double[] l, double[] r, int length ) {
            for ( int i = 0; i < length; i++ ){
                r[i] = 0.0;
                l[i] = 0.0;
            }
            if ( _receiver != null ) {
                int remain = length;
                while ( remain > 0 ) {
                    int amount = (remain > _BUFLEN) ? _BUFLEN : remain;
                    int offset = length - remain;
                    _sender.pull( _buffer_l, _buffer_r, amount );
                    for ( int i = 0; i < amount; i++ ) {
                        l[i + offset] += _buffer_l[i];
                        r[i + offset] += _buffer_r[i];
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
