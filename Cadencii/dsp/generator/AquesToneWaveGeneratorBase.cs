#if ENABLE_AQUESTONE
/*
 * AquesToneWaveGeneratorBase.cs
 * Copyright © 2010-2013 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Threading;
using cadencii.java.awt;
using cadencii.java.util;
using cadencii.media;
using cadencii.vsq;

namespace cadencii
{
    using boolean = System.Boolean;
    using Float = System.Single;
    using Integer = System.Int32;

#if JAVA
    public class AquesToneWaveGeneratorBase implements WaveGenerator
#else
    public abstract class AquesToneWaveGeneratorBase : WaveUnit, WaveGenerator
#endif
    {
        private const int VERSION = 0;
        private const int BUFLEN = 1024;

        private VsqFileEx mVsq = null;

        private WaveReceiver mReceiver = null;
        private int mTrack;
        private int mStartClock;
        private int mEndClock;
        private boolean mRunning = false;
        //private boolean mAbortRequired;
        private long mTotalSamples;
        private int mSampleRate;
        /// <summary>
        /// これまでに合成したサンプル数
        /// </summary>
        private long mTotalAppend;
        private int mTrimRemain;
        private double[] mBufferL = new double[BUFLEN];
        private double[] mBufferR = new double[BUFLEN];
        System.IO.StreamWriter log = null;

        protected abstract EventQueueSequence generateMidiEvent( VsqFileEx vsq, int track, int clock_start, int clock_end );

        protected abstract AquesToneDriverBase getDriver();

        protected AquesToneWaveGeneratorBase()
        {
        }

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
            if ( mTotalSamples <= 0 ) {
                return 0.0;
            } else {
                return mTotalAppend / (double)mTotalSamples;
            }
        }

        public override int getVersion()
        {
            return VERSION;
        }

        public override void setConfig( String parameter )
        {
            // do nothing
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        /// <param name="parameter"></param>
        public void init( VsqFileEx vsq, int track, int start_clock, int end_clock, int sample_rate )
        {
            getDriver().setSampleRate( sample_rate );
            mTrack = track;
            mStartClock = start_clock;
            mEndClock = end_clock;
            mSampleRate = sample_rate;

            this.mVsq = (VsqFileEx)vsq.clone();
            this.mVsq.updateTotalClocks();

            if ( mEndClock < this.mVsq.TotalClocks ) {
                this.mVsq.removePart( mEndClock, this.mVsq.TotalClocks + 480 );
            }

            double end_sec = mVsq.getSecFromClock( start_clock );
            double start_sec = mVsq.getSecFromClock( end_clock );

            double trim_sec = 0.0; // レンダリング結果から省かなければならない秒数。
            if ( mStartClock < this.mVsq.getPreMeasureClocks() ) {
                trim_sec = this.mVsq.getSecFromClock( mStartClock );
            } else {
                this.mVsq.removePart( mVsq.getPreMeasureClocks(), mStartClock );
                trim_sec = this.mVsq.getSecFromClock( this.mVsq.getPreMeasureClocks() );
            }
            this.mVsq.updateTotalClocks();

            mTrimRemain = (int)(trim_sec * mSampleRate);
            //mTrimRemain = 0;
#if DEBUG
            sout.println( "AeuqsToneWaveGenerator#init; mTrimRemain=" + mTrimRemain );
#endif
        }

        public void setReceiver( WaveReceiver r )
        {
            if ( mReceiver != null ) {
                mReceiver.end();
            }
            mReceiver = r;
        }

        /// <summary>
        /// beginメソッドを抜けるときの共通処理を行います
        /// </summary>
        private void exitBegin()
        {
            mRunning = false;
            //mAbortRequired = false;
            mReceiver.end();
        }

        public void begin( long total_samples, WorkerState state )
        {
            var mDriver = getDriver();
#if DEBUG
            sout.println( "AquesToneRenderingRunner#begin; (mDriver==null)=" + (mDriver == null) );
            String file = System.IO.Path.Combine( System.Windows.Forms.Application.StartupPath, "AquesToneWaveGenerator.txt" );
            log = new System.IO.StreamWriter( file );
            log.AutoFlush = true;
#endif
            if ( mDriver == null ) {
#if DEBUG
                log.WriteLine( "mDriver==null" );
                log.Close();
#endif
                exitBegin();
                state.reportComplete();
                return;
            }

#if DEBUG
            sout.println( "AquesToneRenderingRunner#begin; mDriver.loaded=" + mDriver.loaded );
#endif
            if ( !mDriver.loaded ) {
#if DEBUG
                log.WriteLine( "mDriver.loaded=" + mDriver.loaded );
                log.Close();
#endif
                exitBegin();
                state.reportComplete();
                return;
            }

            mRunning = true;
            //mAbortRequired = false;
            mTotalSamples = total_samples;
#if DEBUG
            sout.println( "AquesToneWaveGenerator#begin; mTotalSamples=" + mTotalSamples );
            log.WriteLine( "mTotalSamples=" + mTotalSamples );
            log.WriteLine( "mTrimRemain=" + mTrimRemain );
#endif

            VsqTrack track = mVsq.Track.get( mTrack );
            int BUFLEN = mSampleRate / 10;
            double[] left = new double[BUFLEN];
            double[] right = new double[BUFLEN];
            long saProcessed = 0;
            int saRemain = 0;
            int lastClock = 0; // 最後に処理されたゲートタイム

            // 最初にダミーの音を鳴らす
            // (最初に入るノイズを回避するためと、前回途中で再生停止した場合に無音から始まるようにするため)
            mDriver.resetAllParameters();
            mDriver.process( left, right, BUFLEN );
            MidiEvent f_noteon = new MidiEvent();
            f_noteon.firstByte = 0x90;
            f_noteon.data = new int[] { 0x40, 0x40 };
            f_noteon.clock = 0;
            mDriver.send( new MidiEvent[] { f_noteon } );
            mDriver.process( left, right, BUFLEN );
            MidiEvent f_noteoff = new MidiEvent();
            f_noteoff.firstByte = 0x80;
            f_noteoff.data = new int[] { 0x40, 0x7F };
            mDriver.send( new MidiEvent[] { f_noteoff } );
            for ( int i = 0; i < 3; i++ ) {
                mDriver.process( left, right, BUFLEN );
            }
#if DEBUG
            log.WriteLine( "pre-process done" );
            log.WriteLine( "-----------------------------------------------------" );
            VsqTrack vsq_track = mVsq.Track.get( mTrack );
            for ( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                log.WriteLine( "c" + item.Clock + "; " + item.ID.LyricHandle.L0.Phrase );
            }
#endif

            // レンダリング開始位置での、パラメータの値をセットしておく
            for ( Iterator<VsqEvent> itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
#if DEBUG
                sout.println( "AquesToneWaveGenerator#begin; item.Clock=" + item.Clock );
                log.WriteLine( "*********************************************************" );
                log.WriteLine( "item.Clock=" + item.Clock );
#endif
                long saNoteStart = (long)(mVsq.getSecFromClock( item.Clock ) * mSampleRate);
                long saNoteEnd = (long)(mVsq.getSecFromClock( item.Clock + item.ID.getLength() ) * mSampleRate);
#if DEBUG
                log.WriteLine( "saNoteStart=" + saNoteStart + "; saNoteEnd=" + saNoteEnd );
#endif

                EventQueueSequence list = generateMidiEvent( mVsq, mTrack, lastClock, item.Clock + item.ID.getLength() );
                lastClock = item.Clock + item.ID.Length + 1;
                for ( Iterator<Integer> itr2 = list.keyIterator(); itr2.hasNext(); ) {
                    // まず直前までの分を合成
                    Integer clock = itr2.next();
#if DEBUG
                    log.WriteLine( "-------------------------------------------------------" );
                    sout.println( "AquesToneWaveGenerator#begin;     clock=" + clock );
#endif
                    long saStart = (long)(mVsq.getSecFromClock( clock ) * mSampleRate);
                    saRemain = (int)(saStart - saProcessed);
#if DEBUG
                    log.WriteLine( "saStart=" + saStart );
                    log.WriteLine( "saRemain=" + saRemain );
#endif
                    while ( saRemain > 0 ) {
                        if ( state.isCancelRequested() ) {
                            goto heaven;
                        }
                        int len = saRemain > BUFLEN ? BUFLEN : saRemain;
                        mDriver.process( left, right, len );
                        waveIncoming( left, right, len );
                        saRemain -= len;
                        saProcessed += len;
                        state.reportProgress( saProcessed );
                        //mTotalAppend += len; <- waveIncomingで計算されるので
                    }

                    // MIDiイベントを送信
                    MidiEventQueue queue = list.get( clock );
                    // まずnoteoff
                    boolean noteoff_send = false;
                    if ( queue.noteoff.size() > 0 ) {
#if DEBUG
                        for ( int i = 0; i < queue.noteoff.size(); i++ ) {
                            String str = "";
                            MidiEvent itemi = queue.noteoff.get( i );
                            str += "0x" + PortUtil.toHexString( itemi.firstByte, 2 ) + " ";
                            for ( int j = 0; j < itemi.data.Length; j++ ) {
                                str += "0x" + PortUtil.toHexString( itemi.data[j], 2 ) + " ";
                            }
                            sout.println( typeof( AquesToneWaveGenerator ) + "#begin;         noteoff; " + str );
                        }
#endif
                        mDriver.send( queue.noteoff.toArray( new MidiEvent[] { } ) );
                        noteoff_send = true;
                    }
                    // parameterの変更
                    if ( queue.param.size() > 0 ) {
                        for ( Iterator<ParameterEvent> itr3 = queue.param.iterator(); itr3.hasNext(); ) {
                            ParameterEvent pe = itr3.next();
#if DEBUG
                            sout.println( typeof( AquesToneWaveGenerator ) + "#begin;         param;   index=" + pe.index + "; value=" + pe.value );
#endif
                            mDriver.setParameter( pe.index, pe.value );
                        }
                    }
                    // ついでnoteon
                    if ( queue.noteon.size() > 0 ) {
                        // 同ゲートタイムにピッチベンドも指定されている場合、同時に送信しないと反映されないようだ！
                        if ( queue.pit.size() > 0 ) {
                            queue.noteon.addAll( queue.pit );
                            queue.pit.clear();
                        }
#if DEBUG
                        for ( int i = 0; i < queue.noteon.size(); i++ ) {
                            String str = "";
                            MidiEvent itemi = queue.noteon.get( i );
                            str += "0x" + PortUtil.toHexString( itemi.firstByte, 2 ) + " ";
                            for ( int j = 0; j < itemi.data.Length; j++ ) {
                                str += "0x" + PortUtil.toHexString( itemi.data[j], 2 ) + " ";
                            }
                            sout.println( typeof( AquesToneWaveGenerator ) + "#begin;         noteon;  " + str );
                        }
#endif
                        mDriver.send( queue.noteon.toArray( new MidiEvent[] { } ) );
                    }
                    // PIT
                    if ( queue.pit.size() > 0 && !noteoff_send ) {
#if DEBUG
                        for ( int i = 0; i < queue.pit.size(); i++ ) {
                            String str = "";
                            MidiEvent itemi = queue.pit.get( i );
                            str += "0x" + PortUtil.toHexString( itemi.firstByte, 2 ) + " ";
                            for ( int j = 0; j < itemi.data.Length; j++ ) {
                                str += "0x" + PortUtil.toHexString( itemi.data[j], 2 ) + " ";
                            }
                            sout.println( typeof( AquesToneWaveGenerator ) + "#begin;         pit;     " + str );
                        }
#endif
                        mDriver.send( queue.pit.toArray( new MidiEvent[] { } ) );
                    }
                    if ( mDriver.getUi( mMainWindow ) != null ) {
                        mDriver.getUi( mMainWindow ).invalidateUi();
                    }
                }
            }

            // totalSamplesに足りなかったら、追加してレンダリング
            saRemain = (int)(mTotalSamples - mTotalAppend);
#if DEBUG
            sout.println( "AquesToneRenderingRunner#run; totalSamples=" + mTotalSamples + "; mTotalAppend=" + mTotalAppend + "; saRemain=" + saRemain );
#endif
            while ( saRemain > 0 ) {
                if ( state.isCancelRequested() ) {
                    goto heaven;
                }
                int len = saRemain > BUFLEN ? BUFLEN : saRemain;
                mDriver.process( left, right, len );
                waveIncoming( left, right, len );
                saRemain -= len;
                saProcessed += len;
                state.reportProgress( saProcessed );
                //mTotalAppend += len;
            }
        heaven:
#if DEBUG
            log.Close();
#endif
            exitBegin();
            state.reportComplete();
        }

        private void waveIncoming( double[] l, double[] r, int length )
        {
            //int length = l.Length;
            int offset = 0;
            if ( mTrimRemain > 0 ) {
                if ( length <= mTrimRemain ) {
                    mTrimRemain -= length;
                    return;
                } else {
                    offset = mTrimRemain;
                    mTrimRemain = 0;
                }
            }
            int remain = length - offset;
            while ( remain > 0 ) {
                int amount = (remain > BUFLEN) ? BUFLEN : remain;
                for ( int i = 0; i < amount; i++ ) {
                    mBufferL[i] = l[i + offset];
                    mBufferR[i] = r[i + offset];
                }
#if DEBUG
                log.WriteLine( "waveIncoming; sending " + amount + " samples..." );
#endif
                mReceiver.push( mBufferL, mBufferR, amount );
#if DEBUG
                log.WriteLine( "waveIncoming; ...done." );
#endif
                remain -= amount;
                offset += amount;
                mTotalAppend += amount;
            }
        }

        public long getPosition()
        {
            return mTotalAppend;
        }
    }

}
#endif
