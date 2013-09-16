/*
 * IWaveReceiver.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.media.
 *
 * cadencii.media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii.media;

#else
namespace cadencii.media {
#endif

    public interface IWaveReceiver {
        void append( double[] left, double[] right, int length );
        int getSampleRate();
        void close();
    }

#if !JAVA
}
#endif
