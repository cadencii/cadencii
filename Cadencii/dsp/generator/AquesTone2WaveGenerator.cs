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
using com.github.cadencii.java.awt;
using com.github.cadencii.java.util;
using com.github.cadencii.media;
using com.github.cadencii.vsq;

namespace com.github.cadencii
{
    using boolean = System.Boolean;
    using Float = System.Single;
    using Integer = System.Int32;

    public class AquesTone2WaveGenerator : AquesToneWaveGeneratorBase
    {
        private AquesTone2Driver driver;

        public AquesTone2WaveGenerator( AquesTone2Driver driver )
        {
            this.driver = driver;
        }

        protected override EventQueueSequence generateMidiEvent( VsqFileEx vsq, Integer track, Integer clock_start, Integer clock_end )
        {
            throw new NotImplementedException();
        }

        protected override AquesToneDriverBase getDriver()
        {
            return driver;
        }
    }

}
#endif
