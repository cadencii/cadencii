/*
 * SamplingFrequencyConverter.cs
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
import java.util.*;
#else
using System;
using org.kbinani.media;
using org.kbinani.java.util;
using org.kbinani.java.awt;

namespace org.kbinani.cadencii
{
#endif
    /*
    /// <summary>
    /// サンプリング周波数変換器
    /// </summary>
#if JAVA
    public class SamplingFrequencyConverter extends WaveUnit implements WaveSender, WaveReceiver {
#else
    public class SamplingFrequencyConverter : WaveUnit, WaveSender, WaveReceiver
    {
#endif
        /// <summary>
        /// 動作モードを表す
        /// </summary>
        enum Status
        {
            /// <summary>
            /// 不明
            /// </summary>
            UNKNOWN,
            /// <summary>
            /// WaveReceiverモード
            /// </summary>
            PUSH,
            /// <summary>
            /// WaveSenderモード
            /// </summary>
            PULL,
        }

        private const int _BUFLEN = 1024;

        private Status mMode = Status.UNKNOWN;
        private double[] mBufferL = new double[_BUFLEN];
        private double[] mBufferR = new double[_BUFLEN];
        /// <summary>
        /// サンプリング周波数変換によって余った波形を保存しておくためのキャッシュ(左チャンネル)
        /// </summary>
        private double[] mBufferL2 = new double[_BUFLEN];
        /// <summary>
        /// サンプリング周波数変換によって余った波形を保存しておくためのキャッシュ(右チャンネル)
        /// </summary>
        private double[] mBufferR2 = new double[_BUFLEN];
        /// <summary>
        /// サンプリング周波数変換によって余った波形を保存しておくためのキャッシュのサンプル数．
        /// mBufferL2.Lengthなどとは必ずしも一致しないので注意．
        /// </summary>
        private int mCacheLength = 0;
        private long mPosition = 0;
        private WaveReceiver mReceiver = null;
        private WaveSender mSender = null;
        private int mVersion = 0;
        private BasicStroke mStroke = null;
        private int mRateFrom = 44100;
        private int mRateTo = 44100;
        private RateConvertContext mContext;

        public SamplingFrequencyConverter( int rate_from, int rate_to )
        {
            mRateFrom = rate_from;
            mRateTo = rate_to;
            mContext = new RateConvertContext( rate_from, rate_to );
        }

        public override int getVersion()
        {
            return mVersion;
        }

        public override void setConfig( String parameter )
        {
            // do nothing (ı _ ı )
        }

        public void setReceiver( WaveReceiver r )
        {
            if ( mReceiver != null ) {
                mReceiver.end();
            }
            mReceiver = r;
        }

        public void setSender( WaveSender s )
        {
            if ( mSender != null ) {
                mSender.end();
            }
            mSender = s;
        }

        public long getPosition()
        {
            return mPosition;
        }

        public void end()
        {
            if ( mReceiver != null ) {
                mReceiver.end();
            }
            if ( mSender != null ) {
                mSender.end();
            }
        }

        public void push( double[] l, double[] r, int length )
        {
            if ( mMode == Status.PULL ){
                return;
            }
            if ( mMode == Status.UNKNOWN ) {
                mMode = Status.PUSH;
            }
            while ( RateConvertContext.convert( mContext, l, r, length ) ) {
                mReceiver.push( mContext.bufferLeft, mContext.bufferRight, mContext.length );
            }
        }

        public void pull( double[] l, double[] r, int length )
        {
            if ( mMode == Status.PUSH ) {
                return;
            }
            if ( mMode == Status.UNKNOWN ) {
                mMode = Status.PULL;
            }

            int offset = 0;
            // 前回のpull呼び出しで発生した余剰分のキャッシュが残っていれば，コピーする
            if ( mCacheLength > 0 ) {
                int len = mCacheLength > length ? length : mCacheLength;
                for ( int i = 0; i < len; i++ ) {
                    l[i] = mBufferL2[i];
                    r[i] = mBufferR2[i];
                }
                offset += len;
                mCacheLength -= len;
            }

            if ( mCacheLength > 0 ) {
                // この場合キャッシュの一部だけ使ったので，ずらす
                for ( int i = 0; i < offset; i++ ) {
                    mBufferL2[i] = mBufferL2[i + offset];
                    mBufferR2[i] = mBufferR2[i + offset];
                }
            }
            if ( offset >= length ) {
                // キャッシュからのコピーでlength分のデータが確保できたので，処理を戻す
                return;
            }

            // (length + offset)サンプル分のデータが欲しいんやけど．
            // mSenderからおよそ何サンプルのデータを取ってくればいいか？
            int abstract_length = (int)((double)(length - offset) / (double)mRateTo * (double)mRateFrom);
            // とってこなくちゃならないサンプル数を厳密に計算する
            int estimated = RateConvertContext.estimateResultSamples( mContext, abstract_length );
            while ( estimated < length - offset ) {
                abstract_length++;
                estimated = RateConvertContext.estimateResultSamples( mContext, abstract_length );
            }

            int remain = estimated;
            while ( remain > 0 ) {
                // このループ内では，amountサンプルのデータをmSenderから抜いてきて，
                // それを変換してl, rに格納する．余ったデータはキャッシュに入れる
                int amount = remain > _BUFLEN ? _BUFLEN : remain;
                mSender.pull( mBufferL, mBufferR, amount );
                while ( RateConvertContext.convert( mContext, mBufferL, mBufferR, amount ) ) {
                    int num = mContext.length;
                    int delta = 0; // キャッシュに入れるデータのサンプル数
                    if ( offset + mContext.length >= length ) {
                        // l, rに入りきらないので入る分だけ入れるようにする
                        num = length - offset;
                    }
                    delta = mContext.length - num;
                    for ( int i = 0; i < num; i++ ) {
                        l[offset + i] = mContext.bufferLeft[i];
                        r[offset + i] = mContext.bufferRight[i];
                    }
                    offset += num;

                    // 余った分をキャッシュに入れる
                    if ( delta > 0 ) {
                        int req_len = delta + mCacheLength;
                        // キャッシュが足りてなかったら確保(左チャンネル)
                        if ( mBufferL2 == null ) {
                            mBufferL2 = new double[req_len];
                        } else if ( req_len > mBufferL2.Length ) {
                            Array.Resize( ref mBufferL2, req_len );
                        }
                        // キャッシュが足りてなかったら確保(右チャンネル)
                        if ( mBufferR2 == null ) {
                            mBufferR2 = new double[req_len];
                        } else if ( req_len > mBufferR2.Length ) {
                            Array.Resize( ref mBufferR2, req_len );
                        }
                        // キャッシュにコピー
                        for ( int i = 0; i < delta; i++ ) {
                            mBufferL2[i + mCacheLength] = mContext.bufferLeft[mContext.length - delta + i];
                            mBufferR2[i + mCacheLength] = mContext.bufferRight[mContext.length - delta + i];
                        }
                        mCacheLength += delta;
                    }
                }
                remain -= amount;
            }
        }
    }

#if DEBUG
    public class TestSamplingFrequencyConverter
    {
        public class DumySender : WaveUnit, WaveSender
        {
            const double hz = 440.0;
            const double period = 1.0 / hz;

            private int mRate = 44100;
            private long mProcessed = 0;
            private System.IO.StreamWriter mWriter = null;

            public DumySender( string log_file_name )
            {
                string dir = System.Windows.Forms.Application.StartupPath;
                mWriter = new System.IO.StreamWriter( System.IO.Path.Combine( dir, log_file_name ) );
            }

            public void pull( double[] left, double[] right, int length )
            {
                for ( int i = 0; i < length; i++ ) {
                    double t = (double)(mProcessed + i) / (double)mRate;
                    double x = Math.Sin( 2.0 * Math.PI * t / period );
                    left[i] = x;
                    right[i] = x;
                    mWriter.WriteLine( x );
                }
                mProcessed += length;
            }

            public void setSender( WaveSender s )
            {
            }

            public void end()
            {
                if ( mWriter != null ) {
                    mWriter.Close();
                }
            }

            public override int getVersion()
            {
                return 0;
            }

            public override void paintTo( Graphics graphics, int x, int y, int width, int height )
            {
                base.paintTo( graphics, x, y, width, height );
            }

            public override void setConfig( string parameter )
            {
            }

            public override void setGlobalConfig( EditorConfig config )
            {
                base.setGlobalConfig( config );
            }

            public override void setMainWindow( FormMain main_window )
            {
                base.setMainWindow( main_window );
            }
        }

        public class DumyReceiver : WaveUnit, WaveReceiver
        {
            private System.IO.StreamWriter mWriter = null;

            public DumyReceiver( string log_file_name )
            {
                string dir = System.Windows.Forms.Application.StartupPath;
                mWriter = new System.IO.StreamWriter( System.IO.Path.Combine( dir, log_file_name ) );
            }

            public void end()
            {
                if ( mWriter != null ) {
                    mWriter.Close();
                }
            }

            public void setReceiver( WaveReceiver r )
            {
            }

            public void push( double[] left, double[] right, int length )
            {
                for ( int i = 0; i < length; i++ ) {
                    mWriter.WriteLine( left[i] + "\t" + right[i] );
                }
            }

            public override int getVersion()
            {
                return 0;
            }

            public override void paintTo( Graphics graphics, int x, int y, int width, int height )
            {
                base.paintTo( graphics, x, y, width, height );
            }

            public override void setConfig( string parameter )
            {
            }

            public override void setGlobalConfig( EditorConfig config )
            {
                base.setGlobalConfig( config );
            }

            public override void setMainWindow( FormMain main_window )
            {
                base.setMainWindow( main_window );
            }
        }

        public static void run()
        {
            // push動作の確認
            {
                WaveSenderDriver wsd = new WaveSenderDriver();
                DumySender ds = new DumySender( "TestSamplingFrequencyConverter_push_src.txt" );
                SamplingFrequencyConverter sfc = new SamplingFrequencyConverter( 44100, 48000 );
                DumyReceiver dr = new DumyReceiver( "TestSamplingFrequencyConverter_push_dst.txt" );
                // 回路を組む
                wsd.setSender( ds );
                wsd.setReceiver( sfc );
                sfc.setReceiver( dr );
                wsd.begin( 100000 );
            }

            // pull動作の確認
            {
                EmptyWaveGenerator e = new EmptyWaveGenerator();
                Mixer m = new Mixer();
                DumyReceiver dr = new DumyReceiver( "TestSamplingFrequencyConverter_pull_dst.txt" );
                SamplingFrequencyConverter sfc = new SamplingFrequencyConverter( 44100, 48000 );
                DumySender ds = new DumySender( "TestSamplingFrequencyConverter_pull_src.txt" );
                // 回路を組む
                e.setReceiver( m );
                m.setReceiver( dr );
                m.setSender( sfc );
                sfc.setSender( ds );
                e.begin( 100000 );
            }
        }
    }
#endif
    */
#if !JAVA
}
#endif
