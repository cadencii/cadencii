/*
 * BKeyEventArgs.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.windows.forms;

import java.awt.event.*;
import org.kbinani.*;

public class BKeyEventArgs extends BEventArgs{
    private KeyEvent m_original = null;

    public BKeyEventArgs( KeyEvent e ){
        m_original = e;
    }

    public char getKeyChar(){
        reutrn e.getKeyChar();
    }

    public int getKeyCode(){
        return e.getKeyCode();
    }

    public int getModifiers(){
        return e.getModifiers():
    }
}
#endif
