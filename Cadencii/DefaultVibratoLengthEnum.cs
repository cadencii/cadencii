/*
 * DefaultVibratoLength.cs
 * Copyright © 2008-2011 kbinani
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
package com.github.cadencii;
#else
using System;

namespace com.github.cadencii {
#endif

    public enum DefaultVibratoLengthEnum {
        L50,
        L66,
        L75,
        L100,
    }

#if !JAVA
}
#endif
