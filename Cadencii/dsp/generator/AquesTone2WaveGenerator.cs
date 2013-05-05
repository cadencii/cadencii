#if ENABLE_AQUESTONE
/*
 * AquesToneWave2Generator.cs
 * Copyright © 2013 kbinani
 *
 * This file is part of com.github.cadencii.
 *
 * com.github.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * com.github.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Threading;
using System.Collections.Generic;
using com.github.cadencii.java.awt;
using com.github.cadencii.java.util;
using com.github.cadencii.media;
using com.github.cadencii.vsq;

namespace com.github.cadencii
{
    using boolean = System.Boolean;
    using Float = System.Single;
    using Integer = System.Int32;

    public class AquesTone2WaveGenerator : WaveUnit, WaveGenerator
    {
        private AquesTone2Driver driver_;
        private bool is_running_;
        private long total_samples_;
        private long position_;
        private int sample_rate_;
        private WaveReceiver receiver_;

        private VsqFileEx sequence_;
        private int track_index_;
        private long start_clock_;
        private long end_clock_;

        public AquesTone2WaveGenerator( AquesTone2Driver driver )
        {
            driver_ = driver;
        }

        public bool isRunning() { return is_running_; }

        public long getTotalSamples() { return total_samples_; }

        public long getPosition() { return position_; }

        public double getProgress()
        {
            return position_ / total_samples_ * 100.0;
        }

        public void init( VsqFileEx sequence, int track, int start_clock, int end_clock, int sample_rate )
        {
            sequence_ = (VsqFileEx)sequence.Clone();
            track_index_ = track;
            start_clock_ = start_clock;
            end_clock_ = end_clock;
            sample_rate_ = sample_rate;
            is_running_ = false;
            position_ = 0;
        }

        public int getSampleRate() { return sample_rate_; }

        public void setReceiver( WaveReceiver receiver )
        {
            if (receiver_ != null) receiver_.end();
            receiver_ = receiver;
        }

        public override void setConfig( string config ) {}

        public override int getVersion() { return 0; }

        protected EventQueueSequence generateMidiEvent( VsqFileEx vsq, Integer trackIndex, Integer clock_start, Integer clock_end )
        {
            var result = new EventQueueSequence();
            
            var track = vsq.Track[trackIndex];
            foreach ( var item in vsq.Track[trackIndex].MetaText.Events.Events ) {
                if ( item.ID.type != VsqIDType.Anote ) continue;
                var note = item.ID.Note;
                {
                    var clock = item.Clock;
                    var queue = result.get( clock );
                    var noteOn = driver_.createNoteOnEvent( item.ID.Note,
                                                            item.ID.Dynamics,
                                                            item.ID.LyricHandle.L0.Phrase );
                    queue.noteon.AddRange( noteOn );
                }
                {
                    var clock = item.Clock + item.ID.Length;
                    var queue = result.get( clock );
                    var noteOff = new MidiEvent();
                    noteOff.clock = clock;
                    noteOff.firstByte = 0x80;
                    noteOff.data = new int[] { item.ID.Note, 0x40 };
                    queue.noteoff.add( noteOff );
                }
            }

            return result;
        }

        public void begin( long total_samples, WorkerState state )
        {
            is_running_ = true;
            total_samples_ = total_samples;
            const int BUFLEN = 1024;
            double[] left = new double[BUFLEN];
            double[] right = new double[BUFLEN];

            if ( driver_.getUi( null ) == null ) {
                throw new InvalidOperationException("plugin ui を main view のスレッドで作成した後、このメソッドを呼ばなくてはならない。");
            }

            var eventQueue = generateMidiEvent( sequence_, track_index_, (int)start_clock_, (int)end_clock_ );
            foreach( var sequence_item in eventQueue.getSequence() ) {
                var clock = sequence_item.Key;
                var queue = sequence_item.Value;

                long to_sample = (long)(sequence_.getSecFromClock( clock ) * sample_rate_);
                if (to_sample >= total_samples_){
                    to_sample = total_samples_;
                }
                doSynthesis( to_sample, left, right );
                if ( position_ >= total_samples_ ) break;

                // noteOff, noteOn の順に分けて send する方法も考えられるが、正しく動作しない。
                // このため、いったん一つの配列にまとめてから send する必要がある。
                var events = new List<MidiEvent>();
                events.AddRange( queue.noteoff );
                events.AddRange( queue.noteon );
                driver_.send( events.ToArray() );

                //TODO: のこりのイベント送る処理
            }

            doSynthesis( total_samples, left, right );

            receiver_.end();
            is_running_ = false;
        }

        private void doSynthesis( long to_sample, double[] left, double[] right )
        {
            int buffer_length = left.Length;
            long remain = to_sample - position_;
            while ( 0 < remain ){
                int process = buffer_length < remain ? buffer_length : (int)remain;
                Array.Clear( left, 0, process );
                Array.Clear( right, 0, process );
                driver_.process( left, right, process );
                if ( receiver_ != null ) {
                    receiver_.push( left, right, process );
                }
                remain -= process;
            }
            position_ = to_sample;
        }
    }

}
#endif
