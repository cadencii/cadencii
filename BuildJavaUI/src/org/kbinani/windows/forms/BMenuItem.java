package org.kbinani.windows.forms;

import javax.swing.JMenuItem;

public class BMenuItem extends JMenuItem{
    private Object tag;
    
    public Object getTag(){
        return tag;
    }
    
    public void setTag( Object value ){
        tag = value;
    }
}
