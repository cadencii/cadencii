/*
 * PlayPositionSpecifier.cs
 * Copyright (c) 2008-2009 kbinani
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
namespace Boare.Cadencii {
#endif

    public struct PlayPositionSpecifier {
        public int barCount;
        public int beat;
        public int clock;
        public int numerator;
        public int denominator;
        public int tempo;
    }

#if !JAVA
}
#endif
