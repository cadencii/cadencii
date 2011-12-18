package com.github.cadencii.windows.forms;

import java.awt.event.KeyEvent;
import com.github.cadencii.BEventArgs;

public class BKeyPressEventArgs extends BEventArgs{
    private KeyEvent m_original = null;
    public boolean Alt;
    public boolean Control;
    public int KeyValue;
    public boolean Shift;
    
    public BKeyPressEventArgs( KeyEvent e ){
        m_original = e;
        
        Alt = m_original.isAltDown();
        Control = m_original.isControlDown();
        KeyValue = m_original.getKeyCode();
        Shift = m_original.isShiftDown();
    }
    
}
