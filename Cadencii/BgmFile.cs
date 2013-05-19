/*
 * BgmFile.cs
 * Copyright © 2009-2011 kbinani
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
#if JAVA
package cadencii;
#else
using System;

namespace cadencii {
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
        public Object Clone() {
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
