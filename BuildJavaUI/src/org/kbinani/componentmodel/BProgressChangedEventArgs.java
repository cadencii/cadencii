package org.kbinani.componentmodel;

import org.kbinani.*;

public class BProgressChangedEventArgs extends BEventArgs{
    public int ProgressPercentage = 0;
    public Object UserState = null;

    public BProgressChangedEventArgs( int progressPercentage, Object userState ){
        ProgressPercentage = progressPercentage;
        UserState = userState;
    }
}
