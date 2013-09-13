package com.github.cadencii.windows.forms;

import javax.swing.JScrollBar;

public class BVScrollBar extends BScrollBar{
    private static final long serialVersionUID = 1L;

    public BVScrollBar(){
        super( JScrollBar.VERTICAL );
    }

    public void setVisibleAmount( int value ){
        super.setVisibleAmount( value );
        int unit_increment = value / 10;
        if( unit_increment <= 0 ){
            unit_increment = 1;
        }
        setUnitIncrement( unit_increment );
        setBlockIncrement( value );
    }
}
