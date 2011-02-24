#if ENABLE_VOCALOID
/*
 * VocaloidWaveGenerator.cs
 * Copyright © 2010-2011 kbinani
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
import java.io.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.media.*;
import org.kbinani.vsq.*;

#else

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

namespace org.kbinani.cadencii
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class VocaloidWaveGenerator extends WaveUnit implements WaveGenerator
#else
    public class VocaloidWaveGenerator : WaveUnit, WaveGenerator, IWaveIncoming
#endif
    {
        private const int BUFLEN = 1024;
        private const int VERSION = 0;

        private long mTotalAppend = 0;
        private VsqFileEx mVsq = null;
        private int mTrack;
        private int mStartClock;
        private int mEndClock;
        private long mTotalSamples;
        private boolean mAbortRequired = false;
        private double[] mBufferL = new double[BUFLEN];
        private double[] mBufferR = new double[BUFLEN];
        private WaveReceiver mReceiver = null;
        private int mTrimRemain = 0;
        private boolean mRunning = false;
#if !JAVA
        private VocaloidDriver mDriver = null;
#endif
        /// <summary>
        /// 波形処理ラインのサンプリング周波数
        /// </summary>
        private int mSampleRate;
        /// <summary>
        /// VOCALOID VSTiの実際のサンプリング周波数
        /// </summary>
        private int mDriverSampleRate;
        /// <summary>
        /// サンプリング周波数変換器
        /// </summary>
        private RateConvertContext mContext;

        public int getSampleRate()
        {
            return mSampleRate;
        }

        public boolean isRunning()
        {
            return mRunning;
        }

        public long getTotalSamples()
        {
            return mTotalSamples;
        }

        public double getProgress()
        {
            if ( mTotalSamples > 0 ) {
                return mTotalAppend / (double)mTotalSamples;
            } else {
                return 0.0;
            }
        }

        public void stop()
        {
            if ( mRunning ) {
#if !JAVA
                mDriver.abortRendering();
#endif
                mAbortRequired = true;
                while ( mRunning ) {
#if JAVA
                    try{
                        Thread.sleep( 100 );
                    }catch( Exception ex ){
                        ex.printStackTrace();
                        break;
                    }
#else
                    Thread.Sleep( 100 );
#endif
                }
            }
        }

        public override void setConfig( String parameter )
        {
            // do nothing
        }

        /// <summary>
        /// 初期化メソッド．
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track"></param>
        /// <param name="start_clock"></param>
        /// <param name="end_clock"></param>
        /// <param name="sample_rate">波形処理ラインのサンプリング周波数</param>
        public void init( VsqFileEx vsq, int track, int start_clock, int end_clock, int sample_rate )
        {
            mVsq = vsq;
            mTrack = track;
            mStartClock = start_clock;
            mEndClock = end_clock;
            mSampleRate = sample_rate;
            mDriverSampleRate = 44100;
            try{
                mContext = new RateConvertContext( mDriverSampleRate, mSampleRate );
            }catch( Exception ex ){
                try{
                    // 苦肉の策
                    mContext = new RateConvertContext( mDriverSampleRate, mDriverSampleRate );
                }catch( Exception ex2 ){
                }
            }
        }

        public override int getVersion()
        {
            return VERSION;
        }

        public void setReceiver( WaveReceiver r )
        {
            if ( mReceiver != null ) {
                mReceiver.end();
            }
            mReceiver = r;
        }

        /// <summary>
        /// VSTiドライバに呼んでもらう波形受け渡しのためのコールバック関数にして、IWaveIncomingインターフェースの実装。
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <param name="length"></param>
        public boolean waveIncomingImpl( double[] l, double[] r, int length )
        {
            int offset = 0;
            if ( mTrimRemain > 0 ) {
                // トリムしなくちゃいけない分がまだ残っている場合。トリム処理を行う。
                if ( length <= mTrimRemain ) {
                    // 受け取った波形の長さをもってしても、トリム分が0にならない場合
                    mTrimRemain -= length;
                    return false;
                } else {
                    // 受け取った波形の内の一部をトリムし、残りを波形レシーバに渡す
                    offset = mTrimRemain;
                    // これにてトリム処理は終了なので。
                    mTrimRemain = 0;
                }
            }
            int remain = length - offset;
            while ( remain > 0 ) {
                if ( mAbortRequired ) {
                    return true;
                }
                int amount = (remain > BUFLEN) ? BUFLEN : remain;
                for ( int i = 0; i < amount; i++ ) {
                    mBufferL[i] = l[i + offset];
                    mBufferR[i] = r[i + offset];
                }
                while ( RateConvertContext.convert( mContext, mBufferL, mBufferR, amount ) ) {
                    mReceiver.push( mContext.bufferLeft, mContext.bufferRight, mContext.length );
                    mTotalAppend += mContext.length;
                }
                remain -= amount;
                offset += amount;
            }
            return false;
        }

        public void begin( long total_samples )
        {
            // 渡されたVSQの、合成に不要な部分を削除する
            VsqFileEx split = (VsqFileEx)mVsq.clone();
            VsqTrack vsq_track = split.Track.get( mTrack );
            split.updateTotalClocks();
            if ( mEndClock < mVsq.TotalClocks ) {
                split.removePart( mEndClock, split.TotalClocks + 480 );
            }
            double start_sec = mVsq.getSecFromClock( mStartClock );
            double end_sec = mVsq.getSecFromClock( mEndClock );

            // VSTiが渡してくる波形のうち、先頭からtrim_sec秒分だけ省かないといけない
            // プリセンドタイムがあるので、無条件に合成開始位置以前のデータを削除すると駄目なので。
            double trim_sec = 0.0;
            if ( mStartClock < split.getPreMeasureClocks() ) {
                // 合成開始位置が、プリメジャーよりも早い位置にある場合。
                // VSTiにはクロック0からのデータを渡し、クロック0から合成開始位置までをこのインスタンスでトリム処理する
                trim_sec = split.getSecFromClock( mStartClock );
            } else {
                // 合成開始位置が、プリメジャー以降にある場合。
                // プリメジャーの終了位置から合成開始位置までのデータを削除する
                split.removePart( mVsq.getPreMeasureClocks(), mStartClock );
                trim_sec = split.getSecFromClock( split.getPreMeasureClocks() );
            }
            split.updateTotalClocks();

            // トラックの合成エンジンの種類
            RendererKind s_working_renderer = VsqFileEx.getTrackRendererKind( vsq_track );
#if !JAVA
            // 対象のトラックの合成を担当するVSTiを検索
            mDriver = null;
            for ( int i = 0; i < VSTiDllManager.vocaloidDriver.size(); i++ ) {
                if ( VSTiDllManager.vocaloidDriver.get( i ).kind == s_working_renderer ) {
                    mDriver = VSTiDllManager.vocaloidDriver.get( i );
                    break;
                }
            }
            // ドライバー見つからなかったらbail out
            if ( mDriver == null ) return;
            // ドライバーが読み込み完了していなかったらbail out
            if ( !mDriver.loaded ) return;
#endif

            // NRPNを作成
            int ms_present = mConfig.PreSendTime;
            VsqNrpn[] vsq_nrpn = VsqFile.generateNRPN( split, mTrack, ms_present );
            NrpnData[] nrpn = VsqNrpn.convert( vsq_nrpn );

            // 最初のテンポ指定を検索
            // VOCALOID VSTiが返してくる波形にはなぜかずれがある。このズレは最初のテンポで決まるので。
            float first_tempo = 125.0f;
            if ( split.TempoTable.size() > 0 ) {
                first_tempo = (float)(60e6 / (double)split.TempoTable.get( 0 ).Tempo);
            }
            // ずれるサンプル数
            int errorSamples = VSTiDllManager.getErrorSamples( first_tempo );
            // 今後トリムする予定のサンプル数と、
            mTrimRemain = errorSamples + (int)(trim_sec * mDriverSampleRate);
#if DEBUG
            sout.println( "VocaloidWaveGenerator#begin; trim_sec=" + trim_sec + "; mTrimRemain=" + mTrimRemain );
#endif
            // 合計合成する予定のサンプル数を決める
            mTotalSamples = (long)((end_sec - start_sec) * mDriverSampleRate) + errorSamples;
#if DEBUG
            sout.println( "VocaloidWaveGenerator#begin; mTotalSamples=" + mTotalSamples + "; start_sec,end_sec=" + start_sec + "," + end_sec + "; errorSamples=" + errorSamples );
#endif

            // アボート要求フラグを初期化
            mAbortRequired = false;
#if JAVA
            int ver = (s_working_renderer == RendererKind.VOCALOID2) ? 2 : 1;
            VocaloidDaemon vd = VSTiDllManager.vocaloidrvDaemon[ver - 1];
            if( vd == null ){
                return;
            }
            // 停止処理用のファイルが残っていたら消去する
            String stp = fsys.combine( vd.getTempPathUnixName(), "stop" );
#if DEBUG
            sout.println( "VocaloidWaveGenerator#begin; stp=" + stp + "; isFileExists=" + fsys.isFileExists( stp ) );
#endif
            if( fsys.isFileExists( stp ) ){
                try{
                    PortUtil.deleteFile( stp );
                }catch( Exception ex ){
                    ex.printStackTrace();
                }
            }
#else
            // 使いたいドライバーが使用中だった場合、ドライバーにアボート要求を送る。
            // アボートが終了するか、このインスタンス自身にアボート要求が来るまで待つ。
            if ( mDriver.isRendering() ) {
                // ドライバーにアボート要求
                mDriver.abortRendering();
                while ( mDriver.isRendering() && !mAbortRequired ) {
                    // 待つ
                    Thread.Sleep( 100 );
                }
            }
#endif

            // ここにきて初めて再生中フラグが立つ
            mRunning = true;

#if !JAVA
            // 古いイベントをクリア
            mDriver.clearSendEvents();
#endif
            // ドライバーに渡すイベントを準備
            // まず、マスタートラックに渡すテンポ変更イベントを作成
            int tempo_count = split.TempoTable.size();
            byte[] masterEventsSrc = new byte[tempo_count * 3];
            int[] masterClocksSrc = new int[tempo_count];
            int count = -3;
            for ( int i = 0; i < tempo_count; i++ ) {
                count += 3;
                TempoTableEntry itemi = split.TempoTable.get( i );
                masterClocksSrc[i] = itemi.Clock;
                byte b0 = (byte)(0xff & (itemi.Tempo >> 16));
                long u0 = (long)(itemi.Tempo - (b0 << 16));
                byte b1 = (byte)(0xff & (u0 >> 8));
                byte b2 = (byte)(0xff & (u0 - (u0 << 8)));
                masterEventsSrc[count] = b0;
                masterEventsSrc[count + 1] = b1;
                masterEventsSrc[count + 2] = b2;
            }
#if !JAVA
            // 送る
            mDriver.sendEvent( masterEventsSrc, masterClocksSrc, 0 );
#endif

            // 次に、合成対象トラックの音符イベントを作成
            int numEvents = nrpn.Length;
            byte[] bodyEventsSrc = new byte[numEvents * 3];
            int[] bodyClocksSrc = new int[numEvents];
            count = -3;
            int last_clock = 0;
            for ( int i = 0; i < numEvents; i++ ) {
                int c = nrpn[i].getClock();
                count += 3;
                bodyEventsSrc[count] = (byte)0xb0;
                bodyEventsSrc[count + 1] = (byte)(0xff & nrpn[i].getParameter());
                bodyEventsSrc[count + 2] = (byte)(0xff & nrpn[i].Value);
                bodyClocksSrc[i] = c;
                last_clock = c;
            }
#if !JAVA
            // 送る
            mDriver.sendEvent( bodyEventsSrc, bodyClocksSrc, 1 );
#endif

            // 合成を開始
            // 合成が終わるか、ドライバへのアボート要求が来るまでは制御は返らない
#if JAVA
            try{
                BufferedOutputStream out = vd.outputStream;// process.getOutputStream();
                BufferedInputStream in = vd.inputStream;
                // もしかしたら前回レンダリング時のが残っているかもしれないので，取り除く
                int avail = in.available();
#if DEBUG
                sout.println( "VocaloidWaveGenerator#begin; read trailing data of stdout; avail=" + avail );
#endif
                for( int i = 0; i < avail; i++ ){
                    in.read();
                }
                // コマンドを送信
                // マスタートラック
#if DEBUG
                sout.println( "VocaloidWaveGenerator#begin; send master" );
                RandomAccessFile fos_master =
                    new RandomAccessFile(
                        fsys.combine(
                            PortUtil.getApplicationStartupPath(),
                            "src_master.bin" ), "rw" ); 
#endif
                out.write( 0x01 );
                out.write( 0x04 );
                byte[] buf = PortUtil.getbytes_uint32_le( tempo_count );
                out.write( buf, 0, 4 );
                out.flush();
                count = 0;
#if DEBUG
                fos_master.write( 0x01 );
                fos_master.write( 0x04 );
                fos_master.write( buf, 0, 4 );
                int cnt = 0;
#endif
                for( int i = 0; i < tempo_count; i++ ){
                    buf = PortUtil.getbytes_uint32_le( masterClocksSrc[i] );
                    out.write( buf, 0, 4 );
                    out.write( masterEventsSrc, count, 3 );
#if DEBUG
                    fos_master.write( buf, 0, 4 );
                    fos_master.write( masterEventsSrc, count, 3 );
#endif
#if DEBUG
                    for( int j = 0; j < buf.length; j++ ){
                        if( buf[j] == -1 ){
                            sout.println( "VocaloidWaveGenerator#begin; byte value become -1 at " + cnt );
                        }
                        cnt++;
                    }
                    for( int j = count; j < count + 3; j++ ){
                        if( masterEventsSrc[j] == -1 ){
                            sout.println( "VocaloidWaveGenerator#begin; byte value become -1 at " + cnt );
                        }
                        cnt++;
                    }
#endif
                    count += 3;
                }
                out.flush();
#if DEBUG
                fos_master.close();
#endif
                // 本体トラック
#if DEBUG
                sout.println( "VocaloidWaveGenerator#begin; send body" );
                RandomAccessFile fos_body =
                    new RandomAccessFile(
                        fsys.combine(
                            PortUtil.getApplicationStartupPath(),
                            "src_body.bin" ), "rw" ); 
#endif
                out.write( 0x02 );
                out.write( 0x04 );
                buf = PortUtil.getbytes_uint32_le( numEvents );
                out.write( buf, 0, 4 );
                out.flush();
#if DEBUG
                fos_body.write( 0x02 );
                fos_body.write( 0x04 );
                fos_body.write( buf, 0, 4 );
#endif
                count = 0;
                for( int i = 0; i < numEvents; i++ ){
                    buf = PortUtil.getbytes_uint32_le( bodyClocksSrc[i] );
                    out.write( buf, 0, 4 );
                    out.write( bodyEventsSrc, count, 3 );
#if DEBUG
                    fos_body.write( buf, 0, 4 );
                    fos_body.write( bodyEventsSrc, count , 3 );
#endif
                    count += 3;
                }
                out.flush();
#if DEBUG
                fos_body.close();
#endif
                // 合成開始コマンド
#if DEBUG
                sout.println( "VocaloidWaveGenerator#begin; send synth command" );
#endif
                long act_total_samples = mTotalSamples + mTrimRemain;
                out.write( 0x03 );
                out.write( 0x08 );
                buf = PortUtil.getbytes_int64_le( act_total_samples );
                out.write( buf, 0, 8 );
#if DEBUG
                RandomAccessFile fos_synth =
                    new RandomAccessFile(
                        fsys.combine(
                            PortUtil.getApplicationStartupPath(),
                            "src_synth.bin" ), "rw" ); 
                fos_synth.write( 0x03 );
                fos_synth.write( 0x08 );
                fos_synth.write( buf, 0, 8 );
                fos_synth.close();
#endif
                out.flush();
                long remain = act_total_samples;
                final int BUFLEN = 1024;
                double[] l = new double[BUFLEN];
                double[] r = new double[BUFLEN];
#if DEBUG
                long total_read_bytes = 0;
#endif
                while( remain > 0 ){
                    if( mAbortRequired ){
                        break;
                    }
                    int amount = remain > BUFLEN ? BUFLEN : (int)remain;
                    for( int i = 0; i < amount; i++ ){
                        // 4バイト以上のデータが読み込めるようになるまで待機
                        while( in.available() < 4 && !mAbortRequired ){
                            Thread.sleep( 100 );
                        }
                        if( mAbortRequired ){
                            break;
                        }
                        int lh = in.read();
                        int ll = in.read();
                        int rh = in.read();
                        int rl = in.read();
#if DEBUG
                        total_read_bytes += 4;
#endif
                        short il = (short)(0xffff & ((0xff & lh) << 8) | (0xff & ll));
                        short ir = (short)(0xffff & ((0xff & rh) << 8) | (0xff & rl));
                        l[i] = il / 32768.0;
                        r[i] = ir / 32768.0;
                    }
                    if( mAbortRequired ){
                        break;
                    }
                    waveIncomingImpl( l, r, amount );
                    remain -= amount;
                }

#if DEBUG
                sout.println( "VocaloidWaveGenerator#begin; total_read_bytes=" + total_read_bytes );
#endif
                if( mAbortRequired ){
                    // デーモンに合成処理の停止を要求
                    String monitor_dir = vd.getTempPathUnixName();
                    String stop = fsys.combine( monitor_dir, "stop" );
                    (new FileOutputStream( stop )).close();
                }
                
                // 途中でアボートした場合に備え，取り残しのstdoutを読み取っておく
                remain = in.available();
#if DEBUG
                sout.println( "VocaloidWaveGenerator#begin; read trailing stdout; remain=" + remain );
#endif
                for( long i = 0; i < remain; i++ ){
                    in.read();
                }
            }catch( Exception ex ){
                ex.printStackTrace();
            }
#else // JAVA
#if DEBUG
            // master
            RandomAccessFile fos_master =
                new RandomAccessFile(
                    fsys.combine(
                        PortUtil.getApplicationStartupPath(),
                        "src_master.bin" ), "rw" ); 
            fos_master.write( 0x01 );
            fos_master.write( 0x04 );
            fos_master.write( buf, 0, 4 );
            for( int i = 0; i < tempo_count; i++ ){
                buf = PortUtil.getbytes_uint32_le( masterClocksSrc[i] );
                fos_master.write( buf, 0, 4 );
                fos_master.write( masterEventsSrc, count, 3 );
                count += 3;
            }
            fos_master.close();
            // body
            RandomAccessFile fos_body =
                new RandomAccessFile(
                    fsys.combine(
                        PortUtil.getApplicationStartupPath(),
                        "src_body.bin" ), "rw" ); 
            buf = PortUtil.getbytes_uint32_le( numEvents );
            fos_body.write( 0x02 );
            fos_body.write( 0x04 );
            fos_body.write( buf, 0, 4 );
            count = 0;
            for( int i = 0; i < numEvents; i++ ){
                buf = PortUtil.getbytes_uint32_le( bodyClocksSrc[i] );
                fos_body.write( buf, 0, 4 );
                fos_body.write( bodyEventsSrc, count , 3 );
                count += 3;
            }
            fos_body.close();
            // synth
            long act_total_samples = mTotalSamples + mTrimRemain;
            out.write( 0x03 );
            out.write( 0x08 );
            buf = PortUtil.getbytes_int64_le( act_total_samples );
            out.write( buf, 0, 8 );
            RandomAccessFile fos_synth =
                new RandomAccessFile(
                    fsys.combine(
                        PortUtil.getApplicationStartupPath(),
                        "src_synth.bin" ), "rw" ); 
            fos_synth.write( 0x03 );
            fos_synth.write( 0x08 );
            fos_synth.write( buf, 0, 8 );
            fos_synth.close();
#endif
            mDriver.startRendering(
                mTotalSamples + mTrimRemain + (int)(ms_present / 1000.0 * mDriverSampleRate),
                false,
                mDriverSampleRate,
                this );
#endif // !JAVA

            // ここに来るということは合成が終わったか、ドライバへのアボート要求が実行されたってこと。
            // このインスタンスが受け持っている波形レシーバに、処理終了を知らせる。
            mReceiver.end();
            mRunning = false;
        }

        public long getPosition()
        {
            return mTotalAppend;
        }
    }

#if !JAVA
}
#endif
#endif
