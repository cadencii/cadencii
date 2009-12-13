package org.kbinani.componentmodel;

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
