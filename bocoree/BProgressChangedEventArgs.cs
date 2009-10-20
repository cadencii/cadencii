/*
 * BProgressChangedEventArgs.cs
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

public class BProgressChangedEventArgs extends BEventArgs
{
    private int m_progress_percentage = 0;
    private Object m_user_state = null;

    public BProgressChangedEventArgs( int progressPercentage, Object userState )
    {
        m_progress_percentage = progressPercentage;
        m_user_state = userState;
    }

    public int getProgressPercentage()
    {
        return m_progress_percentage;
    }

    public Object getUserState()
    {
        return m_user_state;
    }

}
#endif
