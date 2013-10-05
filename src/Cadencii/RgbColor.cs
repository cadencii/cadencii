/*
 * RgbColor.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using cadencii.java.awt;

namespace cadencii
{

    public struct RgbColor
    {
        public int R;
        public int G;
        public int B;

        public RgbColor(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public Color getColor()
        {
            return new Color(R, G, B);
        }
    }

}
