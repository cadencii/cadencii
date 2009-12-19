#if ENABLE_AQUESTONE
/*
 * AquesToneRenderingRunner.cs
 * Copyright (c) 2009 kbinani
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
using bocoree.java.util;
using org.kbinani.vsq;
using org.kbinani.media;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;

    public class AquesToneRenderingRunner : RenderingRunner {
        private AquesToneDriver driver = null;
        private int track;
        private String tempDir;
        private boolean modeInfinite;
        /*private int sampleRate;
        private int trimMilliSec;
        private long totalSamples;
        private WaveWriter waveWriter;
        private double waveReadOffsetSeconds;
        private Vector<WaveReader> readers;
        private boolean directPlay;
        private boolean reflectAmp2Wave;*/

        public AquesToneRenderingRunner( AquesToneDriver driver,
                                         VsqFileEx vsq,
                                         int track,
                                         String temp_dir,
                                         int sample_rate,
                                         int trim_msec,
                                         long total_samples,
                                         boolean mode_infinite,
                                         WaveWriter wave_writer,
                                         double wave_read_offset_seconds,
                                         Vector<WaveReader> readers,
                                         boolean direct_play,
                                         boolean reflect_amp_to_wave ) : base( track, reflect_amp_to_wave, wave_writer, wave_read_offset_seconds, readers, direct_play, trim_msec, sample_rate ){
            this.driver = driver;
            tempDir = temp_dir;
            modeInfinite = mode_infinite;
        }

        public override void run() {
        }

        public override double getElapsedSeconds() {
            return 0.0;
        }

        public override bool isRendering() {
            return false;
        }

        public override double computeRemainingSeconds() {
            return 0.0;
        }

        public override void abortRendering() {
        }

        public override double getProgress() {
            return 0.0;
        }
    }

}
#endif
