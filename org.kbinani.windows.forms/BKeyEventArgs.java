package com.github.cadencii.windows.forms;

import java.awt.event.KeyEvent;
import org.kbinani.BEventArgs;

public class BKeyEventArgs extends BEventArgs{
    private KeyEvent m_original = null;
    public boolean Alt;
    public boolean Control;
    public int KeyValue;
    public boolean Shift;
    public int KeyCode;
    
    public BKeyEventArgs( KeyEvent e ){
        m_original = e;
        
        Alt = m_original.isAltDown();
        Control = m_original.isControlDown();
        KeyValue = m_original.getKeyCode();
        Shift = m_original.isShiftDown();
        KeyCode = m_original.getKeyCode();
    }
    
    public char getKeyChar(){
        if( m_original == null ){
            return '\0';
        }else{
            return m_original.getKeyChar();
        }
    }
    
    public int getKeyCode(){
        if( m_original == null ){
            return 0;
        }else{
            return m_original.getKeyCode();
        }
    }
    
    public int getModifiers(){
        if( m_original == null ){
            return 0;
        }else{
            return m_original.getModifiers();
        }
    }
}
