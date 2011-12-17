/*
 * RgbColor.cs
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
package com.github.cadencii;

import java.awt.*;
#else
using com.github.cadencii.java.awt;

namespace com.github.cadencii {
#endif

    public struct RgbColor {
        public int R;
        public int G;
        public int B;

        public RgbColor( int r, int g, int b ) {
            R = r;
            G = g;
            B = b;
        }

#if JAVA
        public RgbColor() {
            // XmlSrializeのために必要
            this( 0, 0, 0 );
        }
#endif

        public Color getColor() {
            return new Color( R, G, B );
        }
    }

#if !JAVA
}
#endif
