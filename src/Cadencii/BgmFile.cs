/*
 * BgmFile.cs
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

    public class BgmFile : ICloneable
    {
        public string file;
        public int feder;
        public int panpot;
        public int mute;
        public bool startAfterPremeasure = true;
        public double readOffsetSeconds = 0.0;

        public Object clone()
        {
            BgmFile ret = new BgmFile();
            ret.feder = feder;
            ret.file = file;
            ret.panpot = panpot;
            ret.mute = mute;
            ret.startAfterPremeasure = startAfterPremeasure;
            ret.readOffsetSeconds = readOffsetSeconds;
            return ret;
        }

        public Object Clone()
        {
            return clone();
        }
    }

}
