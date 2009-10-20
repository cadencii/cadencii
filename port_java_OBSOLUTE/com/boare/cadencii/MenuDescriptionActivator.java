/*
 * MenuDescriptionActivator.java
 * Copyright (c) 2009 kbinani
 *
 * This file is part of com.boare.cadencii.
 *
 * com.boare.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * com.boare.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.cadencii;

import java.awt.*;
import java.awt.event.*;
import javax.swing.*;
import com.boare.util.*;

public class MenuDescriptionActivator implements MouseListener{
	private JLabel m_label;
	private String m_message;

    public MenuDescriptionActivator( JLabel label, String message ){
		m_label = label;
        m_message = message;
	}

	public void mouseEntered( MouseEvent e ){
        String s = Messaging.getMessage( m_message );
        if( s == null || (s != null && s.equals( "" )) ){
            s = " ";
        }
		m_label.setText( s );
	}

    public void mouseClicked( MouseEvent e ){
    }

    public void mouseExited( MouseEvent e ){
    }
    
    public void mousePressed( MouseEvent e ){
    }
    
    public void mouseReleased( MouseEvent e ){
    }
}
