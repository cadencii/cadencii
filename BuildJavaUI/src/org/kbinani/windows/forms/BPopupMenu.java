package org.kbinani.windows.forms;

import javax.swing.JPopupMenu;

public class BPopupMenu extends JPopupMenu {
    private static final long serialVersionUID = 363411779635481115L;
    private Object tag = null;

    public Object getTag(){
        return tag;
    }
    
    public void setTag( Object value ){
        tag = value;
    }
}
