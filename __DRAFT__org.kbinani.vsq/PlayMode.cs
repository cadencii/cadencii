/*
 * PlayMode.cs
 * Copyright (C) 2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/
#if JAVA
package org.kbinani.vsq;

#else
namespace org.kbinani.vsq {
#endif

    public class PlayMode {
        /// <summary>
        /// トラックはミュートされる．(-1)
        /// </summary>
        public const int Off = -1;
        /// <summary>
        /// トラックは合成された後再生される(0)
        /// </summary>
        public const int PlayAfterSynth = 0;
        /// <summary>
        /// トラックは合成しながら再生される(1)
        /// </summary>
        public const int PlayWithSynth = 1;

        private PlayMode() {
        }
    }

#if !JAVA
}
#endif
