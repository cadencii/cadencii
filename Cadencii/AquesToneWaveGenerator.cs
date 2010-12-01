#if ENABLE_AQUESTONE
/*
 * AquesToneWaveGenerator.cs
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
using System.Threading;
using org.kbinani.java.awt;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using Float = System.Single;
    using Integer = System.Int32;

#if JAVA
    public class AquesToneWaveGenerator implements WaveGenerator {
#else
    public class AquesToneWaveGenerator : WaveUnit, WaveGenerator {
#endif
        private const int VERSION = 0;
        private const int BUFLEN = 1024;

        private AquesToneDriver mDriver = null;
        private VsqFileEx mVsq = null;

        private WaveReceiver mReceiver = null;
        private int mTrack;
        private int mStartClock;
        private int mEndClock;
        private boolean mRunning = false;
        private boolean mAbortRequired;
        private long mTotalSamples;
        /// <summary>
        /// これまでに合成したサンプル数
        /// </summary>
        private long mTotalAppend;
        private int mTrimRemain;
        private double[] mBufferL = new double[BUFLEN];
        private double[] mBufferR = new double[BUFLEN];

        public boolean isRunning() {
            return mRunning;
        }

        /// <summary>
        /// ドライバのパラメータの変更要求
        /// </summary>
        private class ParameterEvent {
            public int index;
            public float value;
        }

        private class MidiEventQueue {
            public Vector<MidiEvent> noteoff;
            //public ParameterEvent singer;
            public Vector<MidiEvent> noteon;
            public Vector<MidiEvent> pit;
            //public ParameterEvent pbs;
            public Vector<ParameterEvent> param;
        }

        public long getTotalSamples() {
            return mTotalSamples;
        }

        public double getProgress() {
            if ( mTotalSamples <= 0 ) {
                return 0.0;
            } else {
                return mTotalAppend / (double)mTotalSamples;
            }
        }

        public void stop() {
            if ( mRunning ) {
                mAbortRequired = true;
                while ( mRunning ) {
                    Thread.Sleep( 100 );
                }
            }
        }

        public override int getVersion() {
            return VERSION;
        }

        public override void setConfig( string parameter ) {
            // do nothing
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        /// <param name="parameter"></param>
        public void init( VsqFileEx vsq, int track, int start_clock, int end_clock ) {
            mDriver = AquesToneDriver.getInstance();
            mTrack = track;
            mStartClock = start_clock;
            mEndClock = end_clock;

            this.mVsq = (VsqFileEx)vsq.clone();
            this.mVsq.updateTotalClocks();

            if ( mEndClock < this.mVsq.TotalClocks ) {
                this.mVsq.removePart( mEndClock, this.mVsq.TotalClocks + 480 );
            }

            double end_sec = vsq.getSecFromClock( start_clock );
            double start_sec = vsq.getSecFromClock( end_clock );

            double trim_sec = 0.0; // レンダリング結果から省かなければならない秒数。
            if ( mStartClock < this.mVsq.getPreMeasureClocks() ) {
                trim_sec = this.mVsq.getSecFromClock( mStartClock );
            } else {
                this.mVsq.removePart( vsq.getPreMeasureClocks(), mStartClock );
                trim_sec = this.mVsq.getSecFromClock( this.mVsq.getPreMeasureClocks() );
            }
            this.mVsq.updateTotalClocks();

            mTrimRemain = (int)(trim_sec * VSTiDllManager.SAMPLE_RATE);
        }

        public void setReceiver( WaveReceiver r ) {
            if ( mReceiver != null ) {
                mReceiver.end();
            }
            mReceiver = r;
        }

        public void begin( long total_samples ) {
            if ( mDriver == null ) {
                return;
            }

            if ( !mDriver.loaded ) {
                return;
            }

            mRunning = true;
            mAbortRequired = false;
            mTotalSamples = total_samples;

            VsqTrack track = mVsq.Track.get( mTrack );
            int BUFLEN = VSTiDllManager.SAMPLE_RATE / 10;
            double[] left = new double[BUFLEN];
            double[] right = new double[BUFLEN];
            //long saProcessed = 0; // 
            int saRemain = 0;
            int lastClock = 0; // 最後に処理されたゲートタイム

            // 最初にダミーの音を鳴らす
            // (最初に入るノイズを回避するためと、前回途中で再生停止した場合に無音から始まるようにするため)
            mDriver.resetAllParameters();
            mDriver.process( left, right, BUFLEN );
            MidiEvent f_noteon = new MidiEvent();
            f_noteon.firstByte = 0x90;
            f_noteon.data = new int[] { 0x40, 0x40 };
            mDriver.send( new MidiEvent[] { f_noteon } );
            mDriver.process( left, right, BUFLEN );
            MidiEvent f_noteoff = new MidiEvent();
            f_noteoff.firstByte = 0x80;
            f_noteoff.data = new int[] { 0x40, 0x7F };
            mDriver.send( new MidiEvent[] { f_noteoff } );
            for ( int i = 0; i < 3; i++ ) {
                mDriver.process( left, right, BUFLEN );
            }

            // レンダリング開始位置での、パラメータの値をセットしておく
            for ( Iterator<VsqEvent> itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                long saNoteStart = (long)(mVsq.getSecFromClock( item.Clock ) * VSTiDllManager.SAMPLE_RATE);
                long saNoteEnd = (long)(mVsq.getSecFromClock( item.Clock + item.ID.getLength() ) * VSTiDllManager.SAMPLE_RATE);

                TreeMap<Integer, MidiEventQueue> list = generateMidiEvent( mVsq, mTrack, lastClock, item.Clock + item.ID.getLength() );
                lastClock = item.Clock + item.ID.Length + 1;
                for ( Iterator<Integer> itr2 = list.keySet().iterator(); itr2.hasNext(); ) {
                    // まず直前までの分を合成
                    Integer clock = itr2.next();
                    long saStart = (long)(mVsq.getSecFromClock( clock ) * VSTiDllManager.SAMPLE_RATE);
                    saRemain = (int)(saStart - mTotalAppend);
                    while ( saRemain > 0 ) {
                        if ( mAbortRequired ) {
                            goto end_label;
                        }
                        int len = saRemain > BUFLEN ? BUFLEN : saRemain;
                        mDriver.process( left, right, len );
                        waveIncoming( left, right, len );
                        saRemain -= len;
                        //mTotalAppend += len; <- waveIncomingで計算されるので
                    }

                    // MIDiイベントを送信
                    MidiEventQueue queue = list.get( clock );
                    // まずnoteoff
                    boolean noteoff_send = false;
                    if ( queue.noteoff != null ) {
                        mDriver.send( queue.noteoff.toArray( new MidiEvent[] { } ) );
                        noteoff_send = true;
                    }
                    // parameterの変更
                    if ( queue.param != null ) {
                        for ( Iterator<ParameterEvent> itr3 = queue.param.iterator(); itr3.hasNext(); ) {
                            ParameterEvent pe = itr3.next();
                            mDriver.setParameter( pe.index, pe.value );
                        }
                    }
                    // ついでnoteon
                    if ( queue.noteon != null && queue.noteon.size() > 0 ) {
                        // 同ゲートタイムにピッチベンドも指定されている場合、同時に送信しないと反映されないようだ！
                        if ( queue.pit != null && queue.pit.size() > 0 ) {
                            queue.noteon.addAll( queue.pit );
                            queue.pit.clear();
                        }
                        mDriver.send( queue.noteon.toArray( new MidiEvent[] { } ) );
                    }
                    // PIT
                    if ( queue.pit != null && queue.pit.size() > 0 && !noteoff_send ) {
                        mDriver.send( queue.pit.toArray( new MidiEvent[] { } ) );
                    }
                    if ( mDriver.getUi() != null ) {
                        mDriver.getUi().invalidateUi();
                    }
                }
            }

            // totalSamplesに足りなかったら、追加してレンダリング
            saRemain = (int)(mTotalSamples - mTotalAppend);
#if DEBUG
            PortUtil.println( "AquesToneRenderingRunner#run; totalSamples=" + mTotalSamples + "; mTotalAppend=" + mTotalAppend + "; saRemain=" + saRemain );
#endif
            while ( saRemain > 0 ) {
                if ( mAbortRequired ) {
                    goto end_label;
                }
                int len = saRemain > BUFLEN ? BUFLEN : saRemain;
                mDriver.process( left, right, len );
                waveIncoming( left, right, len );
                saRemain -= len;
                mTotalAppend += len;
            }
        end_label:
            mRunning = false;
            mAbortRequired = false;
            mReceiver.end();
        }

        private void waveIncoming( double[] l, double[] r, int length ) {
            //int length = l.Length;
            int offset = 0;
            if ( mTrimRemain > 0 ) {
                if ( length <= mTrimRemain ) {
                    mTrimRemain -= length;
                    return;
                } else {
                    mTrimRemain = 0;
                    offset += length -= mTrimRemain;
                }
            }
            int remain = length - offset;
            while ( remain > 0 ) {
                int amount = (remain > BUFLEN) ? BUFLEN : remain;
                for ( int i = 0; i < amount; i++ ) {
                    mBufferL[i] = l[i + offset];
                    mBufferR[i] = r[i + offset];
                }
                mReceiver.push( mBufferL, mBufferR, amount );
                remain -= amount;
                offset += amount;
                mTotalAppend += amount;
            }
        }

        public long getPosition() {
            return mTotalAppend;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track"></param>
        /// <param name="clock_start"></param>
        /// <param name="clock_end"></param>
        /// <returns></returns>
        private TreeMap<Integer, MidiEventQueue> generateMidiEvent( VsqFileEx vsq, int track, int clock_start, int clock_end ) {
            TreeMap<Integer, MidiEventQueue> list = new TreeMap<Integer, MidiEventQueue>();
            VsqTrack t = vsq.Track.get( track );

            // 歌手変更
            for ( Iterator<VsqEvent> itr = t.getSingerEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                if ( clock_start <= item.Clock && item.Clock <= clock_end ) {
                    if ( item.ID.IconHandle == null ) {
                        continue;
                    }
                    int program = item.ID.IconHandle.Program;
                    if ( 0 > program || program >= AquesToneDriver.SINGERS.Length ) {
                        program = 0;
                    }
                    ParameterEvent singer = new ParameterEvent();
                    singer.index = mDriver.phontParameterIndex;
                    singer.value = program + 0.01f;
                    if ( !list.containsKey( item.Clock ) ) {
                        list.put( item.Clock, new MidiEventQueue() );
                    }
                    MidiEventQueue queue = list.get( item.Clock );
                    if ( queue.param == null ) {
                        queue.param = new Vector<ParameterEvent>();
                    }
                    queue.param.add( singer );
                } else if ( clock_end < item.Clock ) {
                    break;
                }
            }

            // ノートon, off
            Vector<Point> pit_send = new Vector<Point>(); // PITが追加されたゲートタイム。音符先頭の分を重複して送信するのを回避するために必要。
            VsqBPList pit = t.getCurve( "pit" );
            VsqBPList pbs = t.getCurve( "pbs" );
            VsqBPList dyn = t.getCurve( "dyn" );
            VsqBPList bre = t.getCurve( "bre" );
            VsqBPList cle = t.getCurve( "cle" );
            VsqBPList por = t.getCurve( "por" );
            for ( Iterator<VsqEvent> itr = t.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                int endclock = item.Clock + item.ID.getLength();
                boolean contains_start = clock_start <= item.Clock && item.Clock <= clock_end;
                boolean contains_end = clock_start <= endclock && endclock <= clock_end;
                if ( contains_start || contains_end ) {
                    if ( contains_start ) {
                        #region contains_start
                        // noteonのゲートタイムが，範囲に入っている
                        // noteon MIDIイベントを作成
                        String lyric = item.ID.LyricHandle.L0.Phrase;
                        String katakana = KanaDeRomanization.hiragana2katakana( KanaDeRomanization.Attach( lyric ) );
                        int index = -1;
                        for ( int i = 0; i < AquesToneDriver.PHONES.Length; i++ ) {
                            if ( katakana.Equals( AquesToneDriver.PHONES[i] ) ) {
                                index = i;
                                break;
                            }
                        }
                        if ( index >= 0 ) {
                            if ( !list.containsKey( item.Clock ) ) {
                                list.put( item.Clock, new MidiEventQueue() );
                            }
                            MidiEventQueue queue = list.get( item.Clock );
                            if ( queue.noteon == null ) {
                                queue.noteon = new Vector<MidiEvent>();
                            }

                            // index行目に移動するコマンドを贈る
                            MidiEvent moveline = new MidiEvent();
                            moveline.firstByte = 0xb0;
                            moveline.data = new [] { 0x0a, index };
                            MidiEvent noteon = new MidiEvent();
                            noteon.firstByte = 0x90;
                            noteon.data = new int[] { item.ID.Note, item.ID.Dynamics };
                            Vector<MidiEvent> add = Arrays.asList( new MidiEvent[] { moveline, noteon } );
                            queue.noteon.addAll( add );
                            pit_send.add( new Point( item.Clock, item.Clock ) );
                        }

                        /* 音符頭で設定するパラメータ */
                        // Release
                        MidiEventQueue q = null;
                        if ( !list.containsKey( item.Clock ) ) {
                            q = new MidiEventQueue();
                        } else {
                            q = list.get( item.Clock );
                        }
                        if ( q.param == null ) {
                            q.param = new Vector<ParameterEvent>();
                        }

                        String strRelease = VsqFileEx.getEventTag( item, VsqFileEx.TAG_VSQEVENT_AQUESTONE_RELEASE );
                        int release = 64;
                        try {
                            release = PortUtil.parseInt( strRelease );
                        } catch ( Exception ex ) {
                            Logger.write( typeof( AquesToneWaveGenerator ) + ".generateMidiEvent; ex=" + ex + "\n" );
                            release = 64;
                        }
                        ParameterEvent pe = new ParameterEvent();
                        pe.index = mDriver.releaseParameterIndex;
                        pe.value = release / 127.0f;
                        q.param.add( pe );

                        // dyn
                        int dynAtStart = dyn.getValue( item.Clock );
                        ParameterEvent peDyn = new ParameterEvent();
                        peDyn.index = mDriver.volumeParameterIndex;
                        peDyn.value = (float)(dynAtStart - dyn.getMinimum()) / (float)(dyn.getMaximum() - dyn.getMinimum());
                        q.param.add( peDyn );

                        // bre
                        int breAtStart = bre.getValue( item.Clock );
                        ParameterEvent peBre = new ParameterEvent();
                        peBre.index = mDriver.haskyParameterIndex;
                        peBre.value = (float)(breAtStart - bre.getMinimum()) / (float)(bre.getMaximum() - bre.getMinimum());
                        q.param.add( peBre );

                        // cle
                        int cleAtStart = cle.getValue( item.Clock );
                        ParameterEvent peCle = new ParameterEvent();
                        peCle.index = mDriver.resonancParameterIndex;
                        peCle.value = (float)(cleAtStart - cle.getMinimum()) / (float)(cle.getMaximum() - cle.getMinimum());
                        q.param.add( peCle );

                        // por
                        int porAtStart = por.getValue( item.Clock );
                        ParameterEvent pePor = new ParameterEvent();
                        pePor.index = mDriver.portaTimeParameterIndex;
                        pePor.value = (float)(porAtStart - por.getMinimum()) / (float)(por.getMaximum() - por.getMinimum());
                        q.param.add( pePor );
                        #endregion
                    }

                    // ビブラート
                    // ビブラートが存在する場合、PBSは勝手に変更する。
                    if ( item.ID.VibratoHandle == null ) {
                        if ( contains_start ) {
                            // 音符頭のPIT, PBSを強制的に指定
                            int notehead_pit = pit.getValue( item.Clock );
                            MidiEvent pit0 = getPitMidiEvent( notehead_pit );
                            if ( !list.containsKey( item.Clock ) ) {
                                list.put( item.Clock, new MidiEventQueue() );
                            }
                            MidiEventQueue queue = list.get( item.Clock );
                            if ( queue.pit == null ) {
                                queue.pit = new Vector<MidiEvent>();
                            } else {
                                queue.pit.clear();
                            }
                            queue.pit.add( pit0 );
                            int notehead_pbs = pbs.getValue( item.Clock );
                            ParameterEvent pe = new ParameterEvent();
                            pe.index = mDriver.bendLblParameterIndex;
                            pe.value = notehead_pbs / 13.0f;
                            if ( queue.param == null ) {
                                queue.param = new Vector<ParameterEvent>();
                            }
                            queue.param.add( pe );
                        }
                    } else {
                        int delta_clock = 5;  //ピッチを取得するクロック間隔
                        int tempo = 120;
                        double sec_start_act = vsq.getSecFromClock( item.Clock );
                        double sec_end_act = vsq.getSecFromClock( item.Clock + item.ID.getLength() );
                        double delta_sec = delta_clock / (8.0 * tempo); //ピッチを取得する時間間隔
                        float pitmax = 0.0f;
                        int st = item.Clock;
                        if ( st < clock_start ) {
                            st = clock_start;
                        }
                        int end = item.Clock + item.ID.getLength();
                        if ( clock_end < end ){
                            end = clock_end;
                        }
                        pit_send.add( new Point( st, end ) );
                        // ビブラートが始まるまでのピッチを取得
                        double sec_vibstart = vsq.getSecFromClock( item.Clock + item.ID.VibratoDelay );
                        int pit_count = (int)((sec_vibstart - sec_start_act) / delta_sec);
                        TreeMap<Integer, Float> pit_change = new TreeMap<Integer, Float>();
                        for ( int i = 0; i < pit_count; i++ ) {
                            double gtime = sec_start_act + delta_sec * i;
                            int clock = (int)vsq.getClockFromSec( gtime );
                            float pvalue = (float)t.getPitchAt( clock );
                            pitmax = Math.Max( pitmax, Math.Abs( pvalue ) );
                            pit_change.put( clock, pvalue );
                        }
                        // ビブラート部分のピッチを取得
                        Vector<PointD> ret = new Vector<PointD>();
                        Iterator<PointD> itr2 = new VibratoPointIteratorBySec(
                            vsq,
                            item.ID.VibratoHandle.getRateBP(),
                            item.ID.VibratoHandle.getStartRate(),
                            item.ID.VibratoHandle.getDepthBP(),
                            item.ID.VibratoHandle.getStartDepth(),
                            item.Clock + item.ID.VibratoDelay,
                            item.ID.getLength() - item.ID.VibratoDelay,
                            (float)delta_sec );
                        for ( ; itr2.hasNext(); ) {
                            PointD p = itr2.next();
                            float gtime = (float)p.getX();
                            int clock = (int)vsq.getClockFromSec( gtime );
                            float pvalue = (float)(t.getPitchAt( clock ) + p.getY() * 100.0);
                            pitmax = Math.Max( pitmax, Math.Abs( pvalue ) );
                            pit_change.put( clock, pvalue );
                        }

                        // ピッチベンドの最大値を実現するのに必要なPBS
                        int required_pbs = (int)Math.Ceiling( pitmax / 100.0 );
#if DEBUG
                        PortUtil.println( "AquesToneRenderingRunner#generateMidiEvent; required_pbs=" + required_pbs );
#endif
                        if ( required_pbs > 13 ){
                            required_pbs = 13;
                        }
                        if ( !list.containsKey( item.Clock ) ) {
                            list.put( item.Clock, new MidiEventQueue() );
                        }
                        MidiEventQueue queue = list.get( item.Clock );
                        ParameterEvent pe = new ParameterEvent();
                        pe.index = mDriver.bendLblParameterIndex;
                        pe.value = required_pbs / 13.0f;
                        if ( queue.param == null ) {
                            queue.param = new Vector<ParameterEvent>();
                        }
                        queue.param.add( pe );

                        // PITを順次追加
                        for ( Iterator<Integer> itr3 = pit_change.keySet().iterator(); itr3.hasNext(); ) {
                            Integer clock = itr3.next();
                            if ( clock_start <= clock && clock <= clock_end ) {
                                float pvalue = pit_change.get( clock );
                                int pit_value = (int)(8192.0 / (double)required_pbs * pvalue / 100.0);
                                if ( !list.containsKey( clock ) ) {
                                    list.put( clock, new MidiEventQueue() );
                                }
                                MidiEventQueue q = list.get( clock );
                                MidiEvent me = getPitMidiEvent( pit_value );
                                if ( q.pit == null ) {
                                    q.pit = new Vector<MidiEvent>();
                                } else {
                                    q.pit.clear();
                                }
                                q.pit.add( me );
                            } else if ( clock_end < clock ) {
                                break;
                            }
                        }
                    }

                    //pit_send.add( pit_send_p );

                    // noteoff MIDIイベントを作成
                    if ( contains_end ) {
                        MidiEvent noteoff = new MidiEvent();
                        noteoff.firstByte = 0x80;
                        noteoff.data = new int[] { item.ID.Note, 0x40 }; // ここのvel
                        Vector<MidiEvent> a_noteoff = Arrays.asList( new MidiEvent[] { noteoff } );
                        if ( !list.containsKey( endclock ) ) {
                            list.put( endclock, new MidiEventQueue() );
                        }
                        MidiEventQueue q = list.get( endclock );
                        if ( q.noteoff == null ) {
                            q.noteoff = new Vector<MidiEvent>();
                        }
                        q.noteoff.addAll( a_noteoff );
                        pit_send.add( new Point( endclock, endclock ) ); // PITの送信を抑制するために必要
                    }
                }

                if ( clock_end < item.Clock ) {
                    break;
                }
            }

            // pitch bend sensitivity
            // RPNで送信するのが上手くいかないので、parameterを直接いぢる
            if ( pbs != null ) {
                int keycount = pbs.size();
                for ( int i = 0; i < keycount; i++ ) {
                    int clock = pbs.getKeyClock( i );
                    if ( clock_start <= clock && clock <= clock_end ) {
                        int value = pbs.getElementA( i );
                        ParameterEvent pbse = new ParameterEvent();
                        pbse.index = mDriver.bendLblParameterIndex;
                        pbse.value = value / 13.0f;
                        MidiEventQueue queue = null;
                        if ( list.containsKey( clock ) ) {
                            queue = list.get( clock );
                        }else{
                            queue = new MidiEventQueue();
                        }
                        if ( queue.param == null ) {
                            queue.param = new Vector<ParameterEvent>();
                        }
                        queue.param.add( pbse );
                        list.put( clock, queue );
                    } else if ( clock_end < clock ) {
                        break;
                    }
                }
            }

            // pitch bend
            if ( pit != null ) {
                int keycount = pit.size();
                for ( int i = 0; i < keycount; i++ ) {
                    int clock = pit.getKeyClock( i );
                    if ( clock_start <= clock && clock <= clock_end ) {
                        boolean contains = false;
                        for ( Iterator<Point> itr = pit_send.iterator(); itr.hasNext(); ) {
                            Point p = itr.next();
                            if ( p.x <= clock && clock <= p.y ) {
                                contains = true;
                                break;
                            }
                        }
                        if ( contains ) {
                            continue;
                        }
                        int value = pit.getElementA( i );
                        MidiEvent pbs0 = getPitMidiEvent( value );
                        MidiEventQueue queue = null;
                        if ( list.containsKey( clock ) ) {
                            queue = list.get( clock );
                        }else{
                            queue = new MidiEventQueue();
                        }
                        if ( queue.pit == null ) {
                            queue.pit = new Vector<MidiEvent>();
                        } else {
                            queue.pit.clear();
                        }
                        queue.pit.add( pbs0 );
                        list.put( clock, queue );
                    } else if ( clock_end < clock ) {
                        break;
                    }
                }
            }

            appendParameterEvents( list, dyn, mDriver.volumeParameterIndex, clock_start, clock_end );
            appendParameterEvents( list, bre, mDriver.haskyParameterIndex, clock_start, clock_end );
            appendParameterEvents( list, cle, mDriver.resonancParameterIndex, clock_start, clock_end );
            appendParameterEvents( list, por, mDriver.portaTimeParameterIndex, clock_start, clock_end );

            return list;
        }

        private static void appendParameterEvents( TreeMap<Integer, MidiEventQueue> list, VsqBPList cle, int parameter_index, int clock_start, int clock_end ) {
            int max = cle.getMaximum();
            int min = cle.getMinimum();
            float order = 1.0f / (float)(max - min);
            if ( cle != null ) {
                int keycount = cle.size();
                for ( int i = 0; i < keycount; i++ ) {
                    int clock = cle.getKeyClock( i );
                    if ( clock_start <= clock && clock <= clock_end ) {
                        int value = cle.getElementA( i );
                        MidiEventQueue queue = null;
                        if ( list.containsKey( clock ) ) {
                            queue = list.get( clock );
                        } else {
                            queue = new MidiEventQueue();
                        }
                        if ( queue.param == null ) {
                            queue.param = new Vector<ParameterEvent>();
                        }
                        ParameterEvent pe = new ParameterEvent();
                        pe.index = parameter_index;
                        pe.value = (value - min) * order;
                        queue.param.add( pe );
                        list.put( clock, queue );
                    } else if ( clock_end < clock ) {
                        break;
                    }
                }
            }
        }

        private static MidiEvent getPitMidiEvent( int pitch_bend ) {
            int value = (0x3fff & (pitch_bend + 0x2000));
            int msb = 0xff & (value >> 7);
            int lsb = 0xff & (value - (msb << 7));
            MidiEvent pbs0 = new MidiEvent();
            pbs0.firstByte = 0xE0;
            pbs0.data = new int[] { lsb, msb };
            return pbs0;
        }
    }

}
#endif
