package com.github.cadencii.componentmodel;

import com.github.cadencii.BEventArgs;

public class BDoWorkEventArgs extends BEventArgs{
    public Object Argument = null;
    public Object Result = null;

    public BDoWorkEventArgs( Object argument ){
        Argument = argument;
    }
}
