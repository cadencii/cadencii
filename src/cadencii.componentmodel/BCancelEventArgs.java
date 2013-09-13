package com.github.cadencii.componentmodel;

import com.github.cadencii.BEventArgs;

public class BCancelEventArgs extends BEventArgs{
    public boolean Cancel = false;

    public BCancelEventArgs( boolean value ){
        Cancel = value;
    }

    public BCancelEventArgs(){
        this( false );
    }
}
