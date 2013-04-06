#if ENABLE_AQUESTONE
/*
 * AquesToneWaveGenerator.cs
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
using System;
using System.Threading;
using com.github.cadencii.java.awt;
using com.github.cadencii.java.util;
using com.github.cadencii.media;
using com.github.cadencii.vsq;

namespace com.github.cadencii
{
    using boolean = System.Boolean;
    using Float = System.Single;
    using Integer = System.Int32;

#if JAVA
    public class AquesToneWaveGenerator implements WaveGenerator
#else
    public class AquesToneWaveGenerator : AquesToneWaveGeneratorBase
#endif
    {
        private const int VERSION = 0;
        private const int BUFLEN = 1024;

        private AquesToneDriver mDriver = null;
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

        protected override MidiEvent[] createNoteOnEvent( int note, int dynamics, String phrase )
        {
            // noteon MIDIイベントを作成
            String katakana = KanaDeRomanization.hiragana2katakana( KanaDeRomanization.Attach( phrase ) );
            int index = -1;
            for ( int i = 0; i < AquesToneDriver.PHONES.Length; i++ ) {
                if ( katakana.Equals( AquesToneDriver.PHONES[i] ) ) {
                    index = i;
                    break;
                }
            }

            if ( index < 0 ) {
                return new MidiEvent[] { };
            } else {
                // index行目に移動するコマンドを贈る
                MidiEvent moveline = new MidiEvent();
                moveline.firstByte = 0xb0;
                moveline.data = new[] { 0x0a, index };
                MidiEvent noteon = new MidiEvent();
                noteon.firstByte = 0x90;
                noteon.data = new int[] { note, dynamics };
                return new MidiEvent[] { moveline, noteon };
            }
        }
    }

}
#endif
