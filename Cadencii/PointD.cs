/*
 * PointD.cs
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

    [Serializable]
    public struct PointD {
        private double m_x;
        private double m_y;

        public PointD( double x, double y ) {
            m_x = x;
            m_y = y;
        }

        public System.Drawing.Point ToPoint() {
            return new System.Drawing.Point( (int)m_x, (int)m_y );
        }

        public System.Drawing.PointF ToPointF() {
            return new System.Drawing.PointF( (float)m_x, (float)m_y );
        }

        public double X {
            get {
                return m_x;
            }
            set {
                m_x = value;
            }
        }

        public double Y {
            get {
                return m_y;
            }
            set {
                m_y = value;
            }
        }
    }

}
