/*
 * BDoWorkEventArgs.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.componentModel;

import org.kbinani.*;

public class BDoWorkEventArgs extends BEventArgs
{
    private Object m_argument = null;
    private Object m_result = null;

    public BDoWorkEventArgs( Object argument )
    {
        m_argument = argument;
    }

    public Object getArgument()
    {
        return m_argument;
    }

    public Object getResult()
    {
        return m_result;
    }

    public void setResult( Object value )
    {
        m_result = value;
    }
}
#endif
