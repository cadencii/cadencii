/*
 * WaveViewRealoadRequiredEventArgs.cs
 * Copyright © 2011 kbinani
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
//INCLUDE ../BuildJavaUI/src/org/kbinani/cadencii/WaveViewRealoadRequiredEventArgs.java

#else
using System;

namespace com.github.cadencii{

    public class WaveViewRealoadRequiredEventArgs
    {
        public int track;
        public String file;
        public double secStart;
        public double secEnd;
    }

}
#endif
