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
using System.Threading;
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;

    /// <summary>
    /// ドライバーからの波形を受け取るためのインターフェース
    /// </summary>
    public interface IWaveIncoming {
        /// <summary>
        /// ドライバから波形を受け取るためのコールバック関数
        /// </summary>
        /// <param name="l">左チャンネルの波形データ</param>
        /// <param name="r">右チャンネルの波形データ</param>
        /// <param name="length">波形データの長さ。配列の長さよりも短い場合がある</param>
        /// <returns>ドライバにアボート要求を行う場合true、そうでなければfalseを返す(そのように実装する)</returns>
        boolean waveIncomingImpl( double[] l, double[] r, int length );
    }
}

namespace org.kbinani.cadencii.draft {
    using boolean = System.Boolean;

    public class VocaloidWaveGenerator : WaveUnit, WaveGenerator, IWaveIncoming {
        private const int _BUFLEN = 1024;
        
        private long _position = 0;
        private VsqFileEx _vsq = null;
        private int _track;
        private int _start_clock;
        private int _end_clock;
        private long _total_samples;
        private boolean _abort_required = false;
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private WaveReceiver _receiver = null;
        private int _version = 0;
        private int _trim_remain = 0;
        private boolean mRendering = false;
        private VocaloidDriver mDriver = null;

        public void stop() {
            if ( mRendering ) {
                mDriver.abortRendering();
                _abort_required = true;
                while ( mRendering ) {
#if JAVA
                    Thread.sleep( 0 );
#else
                    Thread.Sleep( 0 );
#endif
                }
            }
        }

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

        /// <summary>
        /// VSTiドライバに呼んでもらう波形受け渡しのためのコールバック関数にして、IWaveIncomingインターフェースの実装。
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <param name="length"></param>
        public boolean waveIncomingImpl( double[] l, double[] r, int length ) {
            int offset = 0;
            if ( _trim_remain > 0 ) {
                // トリムしなくちゃいけない分がまだ残っている場合。トリム処理を行う。
                if ( length <= _trim_remain ) {
                    // 受け取った波形の長さをもってしても、トリム分が0にならない場合
                    _trim_remain -= length;
                    return false;
                } else {
                    // 受け取った波形の内の一部をトリムし、残りを波形レシーバに渡す
                    offset = _trim_remain;
                    // これにてトリム処理は終了なので。
                    _trim_remain = 0;
                }
            }
            int remain = length - offset;
            while ( remain > 0 ) {
                if ( _abort_required ) {
                    return true;
                }
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
            return false;
        }

        public void begin( long total_samples ) {
            // 渡されたVSQの、合成に不要な部分を削除する
            VsqFileEx split = (VsqFileEx)_vsq.clone();
            VsqTrack vsq_track = split.Track.get( _track );
            split.updateTotalClocks();
            if ( _end_clock < _vsq.TotalClocks ) {
                split.removePart( _end_clock, split.TotalClocks + 480 );
            }

            // 末尾に、ダミーの音符を加えておく
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
            vsq_track.addEvent( extra_note );

            // VSTiが渡してくる波形のうち、先頭からtrim_sec秒分だけ省かないといけない
            // プリセンドタイムがあるので、無条件に合成開始位置以前のデータを削除すると駄目なので。
            double trim_sec = 0.0;
            if ( _start_clock < split.getPreMeasureClocks() ) {
                // 合成開始位置が、プリメジャーよりも早い位置にある場合。
                // VSTiにはクロック0からのデータを渡し、クロック0から合成開始位置までをこのインスタンスでトリム処理する
                trim_sec = split.getSecFromClock( _start_clock );
            } else {
                // 合成開始位置が、プリメジャー以降にある場合。
                // プリメジャーの終了位置から合成開始位置までのデータを削除する
                split.removePart( _vsq.getPreMeasureClocks(), _start_clock );
                trim_sec = split.getSecFromClock( split.getPreMeasureClocks() );
            }
            split.updateTotalClocks();

            // 対象のトラックの合成を担当するVSTiを検索
            // トラックの合成エンジンの種類
            RendererKind s_working_renderer = VsqFileEx.getTrackRendererKind( vsq_track );
            mDriver = null;
            for ( int i = 0; i < VSTiProxy.vocaloidDriver.size(); i++ ) {
                if ( VSTiProxy.vocaloidDriver.get( i ).kind == s_working_renderer ) {
                    mDriver = VSTiProxy.vocaloidDriver.get( i );
                    break;
                }
            }
            // ドライバー見つからなかったらbail out
            if ( mDriver == null ) return;
            // ドライバーが読み込み完了していなかったらbail out
            if ( !mDriver.loaded ) return;

            // NRPNを作成
            int ms_present = mConfig.PreSendTime;
            VsqNrpn[] vsq_nrpn = VsqFile.generateNRPN( split, _track, ms_present );
            NrpnData[] nrpn = VsqNrpn.convert( vsq_nrpn );

            // 最初のテンポ指定を検索
            // VOCALOID VSTiが返してくる波形にはなぜかずれがある。このズレは最初のテンポで決まるので。
            float first_tempo = 125.0f;
            if ( split.TempoTable.size() > 0 ) {
                first_tempo = (float)(60e6 / (double)split.TempoTable.get( 0 ).Tempo);
            }
            // ずれるサンプル数
            int errorSamples = VSTiProxy.getErrorSamples( first_tempo );
            // 今後トリムする予定のサンプル数と、
            _trim_remain = errorSamples + (int)(trim_sec * VSTiProxy.SAMPLE_RATE);
            // 合計合成する予定のサンプル数を決める
            _total_samples = (long)((end_sec - start_sec) * VSTiProxy.SAMPLE_RATE) + errorSamples;

            // アボート要求フラグを初期化
            _abort_required = false;
            // 使いたいドライバーが使用中だった場合、ドライバーにアボート要求を送る。
            // アボートが終了するか、このインスタンス自身にアボート要求が来るまで待つ。
            if ( mDriver.isRendering() ) {
                // ドライバーにアボート要求
                mDriver.abortRendering();
                while ( mDriver.isRendering() && !_abort_required ) {
                    // 待つ
#if JAVA
                    Thread.sleep( 0 );
#else
                    System.Windows.Forms.Application.DoEvents();
#endif
                }
            }

            // ここにきて初めて再生中フラグが立つ
            mRendering = true;

            // 古いイベントをクリア
            mDriver.clearSendEvents();

            // ドライバーに渡すイベントを準備
            // まず、マスタートラックに渡すテンポ変更イベントを作成
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
            // 送る
            mDriver.sendEvent( masterEventsSrc, masterClocksSrc, 0 );

            // 次に、合成対象トラックの音符イベントを作成
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
            // 送る
            mDriver.sendEvent( bodyEventsSrc, bodyClocksSrc, 1 );

            // 合成を開始
            // 合成が終わるか、ドライバへのアボート要求が来るまでは制御は返らない
            // この
            mDriver.startRendering(
                _total_samples + _trim_remain + (int)(ms_present / 1000.0 * VSTiProxy.SAMPLE_RATE),
                false,
                VSTiProxy.SAMPLE_RATE,
                this );

            // ここに来るということは合成が終わったか、ドライバへのアボート要求が実行されたってこと。
            // このインスタンスが受け持っている波形レシーバに、処理終了を知らせる。
            _receiver.end();
            mRendering = false;
        }

        public long getPosition() {
            return _position;
        }
    }

}
#endif
