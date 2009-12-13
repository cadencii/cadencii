package org.kbinani.componentModel;

import org.kbinani.BEventArgs;

public class BCancelEventArgs extends BEventArgs{
    public boolean Cancel = false;
    
    public BCancelEventArgs( boolean value ){
        Cancel = value;
    }

    public BCancelEventArgs(){
        this( false );
    }
}
