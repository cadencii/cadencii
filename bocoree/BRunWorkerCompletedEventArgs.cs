/*
 * BRunWorkerCompletedEventArgs.cs
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

public class BRunWorkerCompletedEventArgs extends BEventArgs
{
    private Object m_result = null;
    private Exception m_error = null;
    private boolean m_cancelled = false;

    public BRunWorkerCompletedEventArgs( Object result, Exception error, boolean cancelled )
    {
        m_result = result;
        m_error = error;
        m_cancelled = cancelled;
    }

    public Object getResult(){
        return m_result;
    }

    public Object getUserState(){
        return null;
    }
}
#endif
