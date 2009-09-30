/*
 * VibratoBPPair.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace Boare.Lib.Vsq {

    [Serializable]
    public class VibratoBPPair : IComparable<VibratoBPPair> {
        public float X;
        public int Y;

        public VibratoBPPair() {
        }

        public VibratoBPPair( float x, int y ) {
            X = x;
            Y = y;
        }

        public int CompareTo( VibratoBPPair item ) {
            float v = X - item.X;
            if ( v > 0.0f ) {
                return 1;
            } else if ( v < 0.0f ) {
                return -1;
            }
            return 0;
        }
    }

}
