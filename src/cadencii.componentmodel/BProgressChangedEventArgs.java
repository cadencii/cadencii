package com.github.cadencii.componentmodel;

import com.github.cadencii.*;

public class BProgressChangedEventArgs extends BEventArgs{
    public int ProgressPercentage = 0;
    public Object UserState = null;

    public BProgressChangedEventArgs( int progressPercentage, Object userState ){
        ProgressPercentage = progressPercentage;
        UserState = userState;
    }
}
