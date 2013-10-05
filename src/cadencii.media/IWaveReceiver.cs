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
namespace cadencii.media
{

    public interface IWaveReceiver
    {
        void append(double[] left, double[] right, int length);
        int getSampleRate();
        void close();
    }

}
