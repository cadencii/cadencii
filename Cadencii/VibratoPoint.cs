/*
 * VibratoPoint.cs
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
using System;

namespace Boare.Cadencii {

    public class VibratoPoint : IComparable<VibratoPoint> {
        public float X;
        public int Rate;
        public int Depth;

        public VibratoPoint( float x, int rate, int depth ) {
            X = x;
            Rate = rate;
            Depth = depth;
        }

        public int CompareTo( VibratoPoint item ) {
            float dif = X - item.X;
            if ( dif > 0 ) {
                return 1;
            } else if ( dif < 0 ) {
                return -1;
            } else {
                return 0;
            }
        }
    }

}
