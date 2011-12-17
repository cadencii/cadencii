/*
 * RendererKindUtil.cs
 * Copyright © 2010-2011 kbinani
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

import org.kbinani.*;
#else
using System;
using com.github.cadencii;

namespace com.github.cadencii {
#endif

    /// <summary>
    /// 歌声合成システムの種類
    /// </summary>
    public class RendererKindUtil {
        const String VOCALOID1_100 = "VOCALOID1 [1.0]";
        const String VOCALOID1_101 = "VOCALOID1 [1.1]";
        const String VOCALOID1 = "VOCALOID1";
        const String VOCALOID2 = "VOCALOID2";
        const String VCNT = "vConnect-STAND";
        const String UTAU = "UTAU";
        const String AQUES_TONE = "AquesTone";

        public static String getString( RendererKind value ) {
            if ( value == RendererKind.VOCALOID1_100 ) {
                return VOCALOID1;
            } else if ( value == RendererKind.VOCALOID1_101 ) {
                return VOCALOID1;
            } else if ( value == RendererKind.VOCALOID1 ){
                return VOCALOID1;
            } else if ( value == RendererKind.VOCALOID2 ) {
                return VOCALOID2;
            } else if ( value == RendererKind.VCNT ) {
                return VCNT;
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
            if ( str.compare( value, VOCALOID1_100 ) ) {
                return RendererKind.VOCALOID1;
            } else if ( str.compare( value, VOCALOID1_101 ) ) {
                return RendererKind.VOCALOID1;
            } else if ( str.compare( value, VOCALOID1 ) ){
                return RendererKind.VOCALOID1;
            } else if ( str.compare( value, VOCALOID2 ) ) {
                return RendererKind.VOCALOID2;
            } else if ( str.compare( value, VCNT ) ) {
                return RendererKind.VCNT;
            } else if ( str.compare( value, UTAU ) ) {
                return RendererKind.UTAU;
            } else if ( str.compare( value, AQUES_TONE ) ) {
                return RendererKind.AQUES_TONE;
            } else {
                return RendererKind.NULL;
            }
        }
    }

#if !JAVA
}
#endif
