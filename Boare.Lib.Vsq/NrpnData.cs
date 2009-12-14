/*
 * NrpnData.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;
#else
namespace org.kbinani.vsq {
#endif

    public class NrpnData {
        int m_clock;
        byte m_parameter;
        public byte Value;

        public NrpnData( int clock_, byte parameter, byte value ) {
            m_clock = clock_;
            m_parameter = parameter;
            Value = value;
        }

        public int getClock() {
            return m_clock;
        }

        public byte getParameter() {
            return m_parameter;
        }
    }

#if !JAVA
}
#endif
