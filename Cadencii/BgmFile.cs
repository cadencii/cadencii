/*
 * BgmFile.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.Cadencii;
#else
using System;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
#endif

    public class BgmFile : ICloneable {
        public String file;
        public int feder;
        public int panpot;
        public int mute;
        public boolean startAfterPremeasure = true;
        public double readOffsetSeconds = 0.0;

        public Object clone() {
            BgmFile ret = new BgmFile();
            ret.feder = feder;
            ret.file = file;
            ret.panpot = panpot;
            ret.mute = mute;
            ret.startAfterPremeasure = startAfterPremeasure;
            ret.readOffsetSeconds = readOffsetSeconds;
            return ret;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif