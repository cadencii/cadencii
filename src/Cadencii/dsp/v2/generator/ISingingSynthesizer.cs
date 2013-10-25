/*
 * ISingingSynthesizer.cs
 * Copyright © 2013 kbinani
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
using cadencii.vsq;

namespace cadencii.dsp.v2.generator
{
    public interface ISingingSynthesizer : IWaveGenerator
    {
        void beginSession(VsqFile sequence, int track_index, int sample_rate);
        void endSession();
    }
}
