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
#if DEBUG
            sout.println( "VocaloidWaveGenerator#waveIncomingImpl; length=" + length + "; mTrimRemain=" + mTrimRemain );
#endif
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
#if DEBUG
            sout.println( "VocaloidWaveGenerator#waveIncomingImpl; remain=" + remain );
#endif
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
#if DEBUG
                    sout.println( "VocaloidWaveGenerator#waveIncomingImpl; mContext.length=" + mContext.length );
#endif
                    mReceiver.push( mContext.bufferLeft, mContext.bufferRight, mContext.length );
                    mTotalAppend += mContext.length;
                }
                //mReceiver.push( mBufferL, mBufferR, amount );
                remain -= amount;
                offset += amount;
                //mTotalAppend += amount;
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

#if !JAVA
            // 対象のトラックの合成を担当するVSTiを検索
            // トラックの合成エンジンの種類
            RendererKind s_working_renderer = VsqFileEx.getTrackRendererKind( vsq_track );
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
#if !JAVA
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
            for ( int i = 0; i < split.TempoTable.size(); i++ ) {
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
            // 送る
#if JAVA
#if DEBUG
            String midi_master_file = fsys.combine( PortUtil.getApplicationStartupPath(), "midi_master.bin" );
#else
            String midi_master_file = PortUtil.createTempFile();
#endif
            RandomAccessFile fs = null;
            try{
                fs = new RandomAccessFile( midi_master_file, "rw" );
                count = 0;
                for( int i = 0; i < tempo_count; i++ ){
                    byte[] buf = PortUtil.getbytes_int32_le( masterClocksSrc[i] );
                    fs.write( buf, 0, 4 );
                    fs.write( masterEventsSrc, count, 3 );
                    count += 3;
                }
            }catch( Exception ex ){
                ex.printStackTrace();
            }finally{
                if( fs != null ){
                    try{
                        fs.close();
                    }catch( Exception ex2 ){
                        ex2.printStackTrace();
                    }
                }
            }
#else
            mDriver.sendEvent( masterEventsSrc, masterClocksSrc, 0 );
#endif

            // 次に、合成対象トラックの音符イベントを作成
            int numEvents = nrpn.Length;
            byte[] bodyEventsSrc = new byte[numEvents * 3];
            int[] bodyClocksSrc = new int[numEvents];
            count = -3;
            int last_clock = 0;
            for ( int i = 0; i < numEvents; i++ ) {
                count += 3;
                bodyEventsSrc[count] = (byte)0xb0;
                bodyEventsSrc[count + 1] = nrpn[i].getParameter();
                bodyEventsSrc[count + 2] = nrpn[i].Value;
                bodyClocksSrc[i] = nrpn[i].getClock();
                last_clock = nrpn[i].getClock();
            }
            // 送る
#if JAVA
#if DEBUG
            String midi_body_file = fsys.combine( PortUtil.getApplicationStartupPath(), "midi_body.bin" );
#else
            String midi_body_file = PortUtil.createTempFile();
#endif
            try{
                fs = new RandomAccessFile( midi_body_file, "rw" );
                count = 0;
                for( int i = 0; i < numEvents; i++ ){
                    byte[] buf = PortUtil.getbytes_int32_le( bodyClocksSrc[i] );
                    fs.write( buf, 0, 4 );
                    fs.write( bodyEventsSrc, count, 3 );
                    count += 3;
                }
            }catch( Exception ex ){
                ex.printStackTrace();
            }finally{
                if( fs != null ){
                    try{
                        fs.close();
                    }catch( Exception ex2 ){
                        ex2.printStackTrace();
                    }
                }
            }
#else
            mDriver.sendEvent( bodyEventsSrc, bodyClocksSrc, 1 );
#endif

            // 合成を開始
            // 合成が終わるか、ドライバへのアボート要求が来るまでは制御は返らない
            // この
#if JAVA
            // /bin/sh vocaloidrv.sh WINEPREFIX WINETOP vocaloidrv.exe midi_master.bin midi_body.bin TOTAL_SAMPLES
            String vocaloidrv_sh =
                normalizePath( fsys.combine( PortUtil.getApplicationStartupPath(), "vocaloidrv.sh" ) );
            
            String wine_prefix = 
                normalizePath( VSTiDllManager.WinePrefix );
            
            String wine_top =
                normalizePath( VSTiDllManager.WineTop );
            
            String vocaloidrv_exe =
                normalizePath( fsys.combine( PortUtil.getApplicationStartupPath(), "vocaloidrv.exe" ) );
            
            String dll =
                normalizePath( VSTiDllManager.WineVocaloid2Dll );

#if DEBUG
            String wave = fsys.combine( PortUtil.getApplicationStartupPath(), "out.wav" );
#else
            String wave = PortUtil.createTempFile();
#endif
            if( fsys.isFileExists( wave ) ){
                try{
                    PortUtil.deleteFile( wave );
                }catch( Exception ex ){
                }
            }

            String[] list = new String[]{
                "/bin/sh",
                vocaloidrv_sh,
                wine_prefix,
                wine_top,
                vocaloidrv_exe,
                dll,
                midi_master_file,
                midi_body_file,
                "" + total_samples,
                wave,
            };
#if DEBUG
            sout.println( "VocaloidWaveGenerator#begin; list=" );
            for( String s : list ){
                sout.println( "    " + s );
            }
#endif
            ProcessBuilder pb = new ProcessBuilder( list );
            try{
                Process process = pb.start();
                while( true ){
                    if( mAbortRequired ){
                        process.destroy();
                        break;
                    }
                    try{
                        int ecode = process.exitValue();
                    }catch( Exception ex ){
                        //ex.printStackTrace();
                        continue;
                    }
                    break;
                }
            }catch( Exception ex ){
                ex.printStackTrace();
            }
            
            // 出力されてきたwaveを読み込む
            WaveReader wr = null;
            try{
                wr = new WaveReader( wave );
                // 最初のmTrimRemain分は捨てられるが，その分も読み込まないといけない
                long remain = total_samples;
                final int BUFLEN = 1024;
                double[] l = new double[BUFLEN];
                double[] r = new double[BUFLEN];
                // トリムの分をwaveIncomingImplで処理するのではなく，ここでヤッてしまう
                long offset = mTrimRemain;
                mTrimRemain = 0;
                while( remain > 0 && !mAbortRequired ){
                    int amount = (remain > BUFLEN) ? BUFLEN : (int)remain;
#if DEBUG
                    sout.println( "VocaloidWaveGenerator#begi; remain=" + remain + "; offset=" + offset + "; amount=" + amount );
#endif
                    wr.read( offset, amount, l, r );
                    waveIncomingImpl( l, r, amount );
                    offset += amount;
                    remain -= amount;
                }
            }catch( Exception ex ){
                ex.printStackTrace();
            }finally{
                if( wr != null ){
                    try{
                        wr.close();
                    }catch( Exception ex2 ){
                        ex2.printStackTrace();
                    }
                }
            }
            
            // いらなくなったwaveなどを削除
            try{
                PortUtil.deleteFile( wave );
            }catch( Exception ex ){
            }
            try{
                PortUtil.deleteFile( midi_master_file );
            }catch( Exception ex ){
            }
            try{
                PortUtil.deleteFile( midi_body_file );
            }catch( Exception ex ){
            }
#else
            mDriver.startRendering(
                mTotalSamples + mTrimRemain + (int)(ms_present / 1000.0 * mDriverSampleRate),
                false,
                mDriverSampleRate,
                this );
#endif

            // ここに来るということは合成が終わったか、ドライバへのアボート要求が実行されたってこと。
            // このインスタンスが受け持っている波形レシーバに、処理終了を知らせる。
            mReceiver.end();
            mRunning = false;
        }

        public long getPosition()
        {
            return mTotalAppend;
        }

        private static String normalizePath( String path )
        {
            if( path.indexOf( "~" ) >= 0 ){
                String usr = System.getProperty( "user.name" );
                String tild = "/Users/" + usr;
                path = path.replace( "~", tild );
            }
            path = path.replace( "\\", "\\\\\\\\" );
            //path = path.replace( " ", "\\ " );
            return path;
        }
    }

#if !JAVA
}
#endif
#endif
