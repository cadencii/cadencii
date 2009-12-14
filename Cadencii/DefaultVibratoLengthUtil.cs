/*
 * DefaultVibratoLengthUtil.cs
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
package org.kbinani.cadencii;

#else
using System;

namespace org.kbinani.cadencii {
#endif

    public class DefaultVibratoLengthUtil {
        public static String toString( DefaultVibratoLengthEnum value ) {
            if ( value == DefaultVibratoLengthEnum.L50 ) {
                return "50";
            } else if ( value == DefaultVibratoLengthEnum.L66 ) {
                return "66";
            } else if ( value == DefaultVibratoLengthEnum.L75 ) {
                return "75";
            } else {
                return "100";
            }
        }
    }

#if !JAVA
}
#endif
