/*
 * NrpnData.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

public class NrpnData {
    int m_clock;
    int m_parameter;
    public int value;

    public NrpnData( int clock_, int parameter, int value_ ) {
        m_clock = clock_;
        m_parameter = parameter;
        value = value_;
    }

    public int getClock(){
        return m_clock;
    }

    public int getParameter(){
        return m_parameter;
    }
}
