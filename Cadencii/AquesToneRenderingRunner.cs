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
        private int sampleRate;
        private int trimMilliSec;
        private long totalSamples;
        private boolean modeInfinite;
        private WaveWriter waveWriter;
        private double waveReadOffsetSeconds;
        private Vector<WaveReader> readers;
        private boolean directPlay;
        private boolean reflectAmp2Wave;

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
                                         boolean reflect_amp_to_wave ) {
            this.driver = driver;
            this.track = track;
            tempDir = temp_dir;
            sampleRate = sample_rate;
            trimMilliSec = trim_msec;
            totalSamples = total_samples;
            modeInfinite = mode_infinite;
            waveWriter = wave_writer;
            waveReadOffsetSeconds = wave_read_offset_seconds;
            this.readers = readers;
            directPlay = direct_play;
            reflectAmp2Wave = reflect_amp_to_wave;
        }

        public void run() {
        }

        public double getElapsedSeconds() {
            return 0.0;
        }

        public bool isRendering() {
            return false;
        }

        public double computeRemainingSeconds() {
            return 0.0;
        }

        public void abortRendering() {
        }

        public double getProgress() {
            return 0.0;
        }
    }

}
#endif
