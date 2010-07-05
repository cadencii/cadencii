package org.kbinani.componentmodel;

import org.kbinani.BEventArgs;

public class BDoWorkEventArgs extends BEventArgs{
    public Object Argument = null;
    public Object Result = null;

    public BDoWorkEventArgs( Object argument ){
        Argument = argument;
    }
}
