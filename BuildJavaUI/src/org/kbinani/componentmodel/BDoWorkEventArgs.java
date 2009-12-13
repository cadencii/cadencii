package org.kbinani.componentmodel;

import org.kbinani.BEventArgs;

public class BDoWorkEventArgs extends BEventArgs{
    private Object m_argument = null;
    private Object m_result = null;

    public BDoWorkEventArgs( Object argument ){
        m_argument = argument;
    }

    public Object getArgument(){
        return m_argument;
    }

    public Object getResult(){
        return m_result;
    }

    public void setResult( Object value ){
        m_result = value;
    }
}
