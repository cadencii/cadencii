package org.kbinani.windows.forms;

import java.awt.event.KeyEvent;
import org.kbinani.BEventArgs;

public class BKeyEventArgs extends BEventArgs{
    private KeyEvent m_original = null;
    
    public BKeyEventArgs( KeyEvent e ){
        m_original = e;
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
