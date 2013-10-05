/*
 * NrpnData.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace cadencii.vsq
{

    public class NrpnData
    {
        int m_clock;
        byte m_parameter;
        public byte Value;

        public NrpnData(int clock_, byte parameter, byte value)
        {
            m_clock = clock_;
            m_parameter = parameter;
            Value = value;
        }

        public int getClock()
        {
            return m_clock;
        }

        public byte getParameter()
        {
            return m_parameter;
        }
    }

}
