/*
 * BPictureBox.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.Cadencii;

import java.awt.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.windows.forms.*;

public class BPictureBox extends JPanel{
    public BEvent keyDownEvent;
    public BEvent keyUpEvent;
    private Image m_image;

    public BPictureBox(){
        keyDownEvent = new BEvent();
        keyUpEvent = new BEvent();
    }

    public Image getImage(){
        return m_image;
    }

    public void setImage( Image img ){
        m_image = img;
    }

    public void paint( Graphics g1 ){
        Graphics2D g = (Graphics2D)g1;
    }
}
#else
using System.Windows.Forms;

namespace Boare.Cadencii {

    /// <summary>
    /// KeyDownとKeyUpを受信できるPictureBox
    /// </summary>
    public class BPictureBox : PictureBox {
        public event KeyEventHandler BKeyDown;
        public event KeyEventHandler BKeyUp;
        
        protected override void OnKeyDown( KeyEventArgs e ) {
            if ( BKeyDown != null ) {
                BKeyDown( this, e );
            }
        }

        protected override void OnKeyUp( KeyEventArgs e ) {
            if ( BKeyUp != null ) {
                BKeyUp( this, e );
            }
        }

        protected override void OnMouseDown( MouseEventArgs e ) {
            base.OnMouseDown( e );
            this.Focus();
        }
    }

}
#endif
