/*
 * PencilMode.cs
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
using System;
using System.Collections.Generic;
using System.Text;

namespace Boare.Cadencii {

    using boolean = Boolean;

    struct PencilMode {
        private PencilModeEnum m_mode;
        private boolean m_triplet;
        private boolean m_dot;

        public int GetUnitLength() {
            int b = (int)m_mode;
            if ( m_triplet ) {
                b = b * 2 / 3;
            } else if ( m_dot ) {
                b = b + b / 2;
            }
            return b;
        }


        public PencilModeEnum Mode {
            get {
                return m_mode;
            }
            set {
                m_mode = value;
            }
        }


        public boolean Triplet {
            get {
                return m_triplet;
            }
            set {
                m_triplet = value;
                if ( m_triplet && m_dot ) {
                    m_dot = false;
                }
            }
        }


        public boolean Dot {
            get {
                return m_dot;
            }
            set {
                m_dot = value;
                if ( m_dot && m_triplet ) {
                    m_triplet = false;
                }
            }
        }
    }
}
