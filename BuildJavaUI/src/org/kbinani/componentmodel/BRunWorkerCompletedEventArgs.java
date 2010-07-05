package org.kbinani.componentmodel;

import org.kbinani.*;

public class BRunWorkerCompletedEventArgs extends BEventArgs{
    private Object m_result = null;
    private Exception m_error = null;
    private boolean m_cancelled = false;

    public BRunWorkerCompletedEventArgs( Object result, Exception error, boolean cancelled ){
        m_result = result;
        setError(error);
        setCancelled( cancelled );
    }

    public Object getResult(){
        return m_result;
    }

    public void setCancelled(boolean m_cancelled) {
        this.m_cancelled = m_cancelled;
    }

    public boolean isCancelled() {
        return m_cancelled;
    }

    public void setError(Exception m_error) {
        this.m_error = m_error;
    }

    public Exception getError() {
        return m_error;
    }
}
