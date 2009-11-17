package org.kbinani.windows.forms;

import javax.swing.JSplitPane;

public class BSplitPane extends JSplitPane {
    private static final long serialVersionUID = -7485943135051893345L;

    public boolean isSplitterFixed(){
        return super.isEnabled();
    }
    
    public void setSplitterFixed( boolean value ){
        super.setEnabled( value );
    }
}
