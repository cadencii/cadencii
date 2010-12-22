/*
 * ClockResolution.cs
 * Copyright © 2008-2010 kbinani
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
package org.kbinani.cadencii;

#else
using System;

namespace org.kbinani.cadencii {
#endif

    public enum ClockResolution {
        L1,
        L2,
        L4,
        L8,
        L16,
        L30,
        L60,
        L90,
        L120,
        L240,
        L480,
        Free,
    }

#if !JAVA
}
#endif
