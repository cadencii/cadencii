package org.kbinani.componentmodel;

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
