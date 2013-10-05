/*
 * PencilMode.cs
 * Copyright © 2008-2011 kbinani
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
using System;

namespace cadencii
{

    public struct PencilMode
    {
        private PencilModeEnum m_mode;
        private bool m_triplet;
        private bool m_dot;

        public int getUnitLength()
        {
            int b = 1;
            if (m_mode == PencilModeEnum.L1) {
                b = 1920;
            } else if (m_mode == PencilModeEnum.L2) {
                b = 960;
            } else if (m_mode == PencilModeEnum.L4) {
                b = 480;
            } else if (m_mode == PencilModeEnum.L8) {
                b = 240;
            } else if (m_mode == PencilModeEnum.L16) {
                b = 120;
            } else if (m_mode == PencilModeEnum.L32) {
                b = 60;
            } else if (m_mode == PencilModeEnum.L64) {
                b = 30;
            } else if (m_mode == PencilModeEnum.L128) {
                b = 15;
            }
            if (m_triplet) {
                b = b * 2 / 3;
            } else if (m_dot) {
                b = b + b / 2;
            }
            return b;
        }

        public PencilModeEnum getMode()
        {
            return m_mode;
        }

        public void setMode(PencilModeEnum value)
        {
            m_mode = value;
        }

        public bool isTriplet()
        {
            return m_triplet;
        }

        public void setTriplet(bool value)
        {
            m_triplet = value;
            if (m_triplet && m_dot) {
                m_dot = false;
            }
        }

        public bool isDot()
        {
            return m_dot;
        }

        public void setDot(bool value)
        {
            m_dot = value;
            if (m_dot && m_triplet) {
                m_triplet = false;
            }
        }
    }

}
