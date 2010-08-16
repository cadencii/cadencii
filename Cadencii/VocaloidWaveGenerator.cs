#if ENABLE_VOCALOID
/*
 * VocaloidWaveGenerator.cs
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
using System.Windows.Forms;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;

namespace org.kbinani.cadencii{
    using boolean = System.Boolean;

    public interface IWaveIncoming {
        void waveIncomingImpl( double[] l, double[] r );
    }

    public class VocaloidWaveGenerator : WaveUnit, WaveGenerator, IWaveIncoming {
        private const int _BUFLEN = 1024;
        
        private long _position = 0;
        private VsqFileEx _vsq = null;
        private int _track;
        private int _start_clock;
        private int _end_clock;
        private int _presend_milli_sec;
        private long _total_samples;
        private boolean _abort_required = false;
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private WaveReceiver _receiver = null;
        private int _version = 0;

        // RenderingRunner
        private int _trim_remain = 0;

        public override void setConfig( String parameter ) {
            // do nothing
        }

        /// <summary>
        /// 初期化メソッド．
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track"></param>
        /// <param name="start_clock"></param>
        /// <param name="end_clock"></param>
        public void init( VsqFileEx vsq, int track, int start_clock, int end_clock ) {
            _vsq = vsq;
            _track = track;
            _start_clock = start_clock;
            _end_clock = end_clock;
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

        public void waveIncomingImpl( double[] l, double[] r ) {
            int length = l.Length;
            int offset = 0;
            if ( _trim_remain > 0 ) {
                if ( length <= _trim_remain ) {
                    _trim_remain -= length;
                    return;
                } else {
                    _trim_remain = 0;
                    offset += length -= _trim_remain;
                }
            }
            int remain = length - offset;
            while ( remain > 0 ) {
                int amount = (remain > _BUFLEN) ? _BUFLEN : remain;
                for ( int i = 0; i < amount; i++ ) {
                    _buffer_l[i] = l[i + offset];
                    _buffer_r[i] = r[i + offset];
                }
                _receiver.push( _buffer_l, _buffer_r, amount );
                remain -= amount;
                offset += amount;
                _position += amount;
            }
        }

        public void begin( long total_samples ) {
            #region 旧VSTiProxyの実装分
            RendererKind s_working_renderer = VsqFileEx.getTrackRendererKind( _vsq.Track.get( _track ) );
            VsqFileEx split = (VsqFileEx)_vsq.clone();
            split.updateTotalClocks();

            if ( _end_clock < _vsq.TotalClocks ) {
                split.removePart( _end_clock, split.TotalClocks + 480 );
            }

            double end_sec = _vsq.getSecFromClock( _start_clock );
            double start_sec = _vsq.getSecFromClock( _end_clock );
            int extra_note_clock = (int)_vsq.getClockFromSec( end_sec + 10.0 );
            int extra_note_clock_end = (int)_vsq.getClockFromSec( end_sec + 10.0 + 3.1 ); //ブロックサイズが1秒分で、バッファの個数が3だから +3.1f。0.1fは安全のため。
            VsqEvent extra_note = new VsqEvent( extra_note_clock, new VsqID( 0 ) );
            extra_note.ID.type = VsqIDType.Anote;
            extra_note.ID.Note = 60;
            extra_note.ID.setLength( extra_note_clock_end - extra_note_clock );
            extra_note.ID.VibratoHandle = null;
            extra_note.ID.LyricHandle = new LyricHandle( "a", "a" );
            split.Track.get( _track ).addEvent( extra_note );

            double trim_sec = 0.0; // レンダリング結果から省かなければならない秒数。
            if ( _start_clock < split.getPreMeasureClocks() ) {
                trim_sec = split.getSecFromClock( _start_clock );
            } else {
                split.removePart( _vsq.getPreMeasureClocks(), _start_clock );
                trim_sec = split.getSecFromClock( split.getPreMeasureClocks() );
            }
            split.updateTotalClocks();
            _total_samples = (long)((end_sec - start_sec) * VSTiProxy.SAMPLE_RATE);
            
            VocaloidDriver driver = null;
            for ( int i = 0; i < VSTiProxy.vocaloidDriver.size(); i++ ) {
                if ( VSTiProxy.vocaloidDriver.get( i ).kind == s_working_renderer ) {
                    driver = VSTiProxy.vocaloidDriver.get( i );
                    break;
                }
            }
            VsqNrpn[] vsq_nrpn = VsqFile.generateNRPN( split, _track, _presend_milli_sec );
            NrpnData[] nrpn = VsqNrpn.convert( vsq_nrpn );
            #endregion

            #region VocaloidRenderingRunner.ctor
            float first_tempo = 125.0f;
            if ( split.TempoTable.size() > 0 ) {
                first_tempo = (float)(60e6 / (double)split.TempoTable.get( 0 ).Tempo);
            }
            int errorSamples = VSTiProxy.getErrorSamples( first_tempo );
            _trim_remain = errorSamples + (int)(trim_sec * VSTiProxy.SAMPLE_RATE);
            _total_samples += errorSamples;
            #endregion

            #region VocaloidRenderingRunner.run()
            _abort_required = false;
            if ( driver == null ) {
                return;
            }
            if ( !driver.loaded ) {
                return;
            }
            if ( driver.isRendering() ) {
                driver.abortRendering();
                while ( driver.isRendering() && !_abort_required ) {
#if JAVA
                    Thread.sleep( 0 );
#else
                    System.Windows.Forms.Application.DoEvents();
#endif
                }
            }

            // 古いイベントをクリア
            driver.clearSendEvents();

            int tempo_count = split.TempoTable.size();
            byte[] masterEventsSrc = new byte[tempo_count * 3];
            int[] masterClocksSrc = new int[tempo_count];
            int count = -3;
            for ( int i = 0; i < split.TempoTable.size(); i++ ) {
                count += 3;
                TempoTableEntry itemi = split.TempoTable.get( i );
                masterClocksSrc[i] = itemi.Clock;
                byte b0 = (byte)(itemi.Tempo >> 16);
                uint u0 = (uint)(itemi.Tempo - (b0 << 16));
                byte b1 = (byte)(u0 >> 8);
                byte b2 = (byte)(u0 - (u0 << 8));
                masterEventsSrc[count] = b0;
                masterEventsSrc[count + 1] = b1;
                masterEventsSrc[count + 2] = b2;
            }
            driver.sendEvent( masterEventsSrc, masterClocksSrc, 0 );

            int numEvents = nrpn.Length;
            byte[] bodyEventsSrc = new byte[numEvents * 3];
            int[] bodyClocksSrc = new int[numEvents];
            count = -3;
            int last_clock = 0;
            for ( int i = 0; i < numEvents; i++ ) {
                count += 3;
                bodyEventsSrc[count] = 0xb0;
                bodyEventsSrc[count + 1] = nrpn[i].getParameter();
                bodyEventsSrc[count + 2] = nrpn[i].Value;
                bodyClocksSrc[i] = nrpn[i].getClock();
                last_clock = nrpn[i].getClock();
            }

            int index = tempo_count - 1;
            for ( int i = tempo_count - 1; i >= 0; i-- ) {
                if ( split.TempoTable.get( i ).Clock < last_clock ) {
                    index = i;
                    break;
                }
            }
            int last_tempo = split.TempoTable.get( index ).Tempo;

            driver.sendEvent( bodyEventsSrc, bodyClocksSrc, 1 );

            driver.startRendering( 
                _total_samples + _trim_remain + (int)(_presend_milli_sec / 1000.0 * VSTiProxy.SAMPLE_RATE),
                false,
                VSTiProxy.SAMPLE_RATE,
                this );

            _receiver.end();
            #endregion
        }

        public long getPosition() {
            return _position;
        }
    }

}
#endif
