package org.kbinani.windows.forms;

import java.awt.event.KeyEvent;
import org.kbinani.BEventArgs;

public class BKeyPreviewEventArgs extends BEventArgs{
    private KeyEvent m_original = null;
    public boolean Alt;
    public boolean Control;
    public int KeyValue;
    public boolean Shift;
    
    public BKeyPreviewEventArgs( KeyEvent e ){
        m_original = e;
        
        Alt = m_original.isAltDown();
        Control = m_original.isControlDown();
        KeyValue = m_original.getKeyCode();
        Shift = m_original.isShiftDown();
    }
    
}
