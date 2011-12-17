/*
 * IWaveReceiver.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of org.kbinani.media.
 *
 * org.kbinani.media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package com.github.cadencii.media;

#else
namespace com.github.cadencii.media {
#endif

    public interface IWaveReceiver {
        void append( double[] left, double[] right, int length );
        int getSampleRate();
        void close();
    }

#if !JAVA
}
#endif
