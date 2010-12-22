/*
 * RendererKindUtil.cs
 * Copyright © 2010 kbinani
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

    /// <summary>
    /// 歌声合成システムの種類
    /// </summary>
    public class RendererKindUtil {
        const String VOCALOID1_100 = "VOCALOID1 [1.0]";
        const String VOCALOID1_101 = "VOCALOID1 [1.1]";
        const String VOCALOID2 = "VOCALOID2";
        const String STRAIGHT_UTAU = "Straight x UTAU";
        const String UTAU = "UTAU";
        const String AQUES_TONE = "AquesTone";

        public static String getString( RendererKind value ) {
            if ( value == RendererKind.VOCALOID1_100 ) {
                return VOCALOID1_100;
            } else if ( value == RendererKind.VOCALOID1_101 ) {
                return VOCALOID1_101;
            } else if ( value == RendererKind.VOCALOID2 ) {
                return VOCALOID2;
            } else if ( value == RendererKind.VCNT ) {
                return STRAIGHT_UTAU;
            } else if ( value == RendererKind.UTAU ) {
                return UTAU;
            } else if ( value == RendererKind.AQUES_TONE ) {
                return AQUES_TONE;
            } else {
                return "";
            }
        }

        public static RendererKind fromString( String value ) {
            if ( value == null ) {
                return RendererKind.NULL;
            }
            if ( value.Equals( VOCALOID1_100 ) ) {
                return RendererKind.VOCALOID1_100;
            } else if ( value.Equals( VOCALOID1_101 ) ) {
                return RendererKind.VOCALOID1_101;
            } else if ( value.Equals( VOCALOID2 ) ) {
                return RendererKind.VOCALOID2;
            } else if ( value.Equals( STRAIGHT_UTAU ) ) {
                return RendererKind.VCNT;
            } else if ( value.Equals( UTAU ) ) {
                return RendererKind.UTAU;
            } else if ( value.Equals( AQUES_TONE ) ) {
                return RendererKind.AQUES_TONE;
            } else {
                return RendererKind.NULL;
            }
        }
    }

#if !JAVA
}
#endif
