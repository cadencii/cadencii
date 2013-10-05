/*
 * SequenceConfig.cs
 * Copyright © 2009-2011 kbinani
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

namespace cadencii
{

    public class SequenceConfig
    {
        public int SamplingRate = 44100;
        /// <summary>
        /// waveファイル出力時のチャンネル数（1または2）
        /// <version>3.3+</version>
        /// </summary>
        public int WaveFileOutputChannel = 2;
        /// <summary>
        /// waveファイル出力時に、全トラックをmixして出力するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public bool WaveFileOutputFromMasterTrack = false;
        /// <summary>
        /// スタートマーカーの位置(clock)
        /// version 3.3+
        /// </summary>
        public int StartMarker = 0;
        /// <summary>
        /// スタートマーカーが有効かどうか
        /// version 3.3+
        /// </summary>
        public bool StartMarkerEnabled = false;
        /// <summary>
        /// エンドマーカーの位置(clock)
        /// version 3.3+
        /// </summary>
        public int EndMarker = 0;
        /// <summary>
        /// エンドマーカーが有効かどうか
        /// version 3.3+
        /// </summary>
        public bool EndMarkerEnabled = false;

        public bool equals(SequenceConfig item)
        {
            if (item == null) {
                return false;
            }
            if (this.SamplingRate != item.SamplingRate) {
                return false;
            }
            if (this.WaveFileOutputFromMasterTrack != item.WaveFileOutputFromMasterTrack) {
                return false;
            }
            if (this.WaveFileOutputChannel != item.WaveFileOutputChannel) {
                return false;
            }
            return true;
        }

        public Object clone()
        {
            SequenceConfig config = new SequenceConfig();
            config.SamplingRate = this.SamplingRate;
            config.WaveFileOutputChannel = this.WaveFileOutputChannel;
            config.WaveFileOutputFromMasterTrack = this.WaveFileOutputFromMasterTrack;
            return config;
        }
    }

}
