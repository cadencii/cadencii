/*
 * AutoVibratoMinLengthUtil.cs
 * Copyright (C) 2009-2010 kbinani
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

    public class AutoVibratoMinLengthUtil {
        public static String toString( AutoVibratoMinLengthEnum value ) {
            if ( value == AutoVibratoMinLengthEnum.L1 ) {
                return "1";
            } else if ( value == AutoVibratoMinLengthEnum.L2 ) {
                return "2";
            } else if ( value == AutoVibratoMinLengthEnum.L3 ) {
                return "3";
            } else {
                return "4";
            }
        }

        public static int getValue( AutoVibratoMinLengthEnum value ) {
            if ( value == AutoVibratoMinLengthEnum.L1 ) {
                return 1;
            } else if ( value == AutoVibratoMinLengthEnum.L2 ) {
                return 2;
            } else if ( value == AutoVibratoMinLengthEnum.L3 ) {
                return 3;
            } else {
                return 4;
            }
        }
    }

#if !JAVA
}
#endif
