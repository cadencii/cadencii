/*
 * BTextBox.cs
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

import javax.swing.*;

public class BTextBox extends JTextField{
    public boolean isImeModeOn()
    {
        return getInputContext().isCompositionEnabled();
    }

    public void setImeModeOn( boolean value )
    {
        getInputContext().setCompositionEnabled( value );
    }
}
#else
#define COMPONENT_ENABLE_LOCATION
namespace bocoree.windows.forms
{
    public class BTextBox : System.Windows.Forms.TextBox {
        #region java.awt.Component
#if COMPONENT_PARENT_AS_OWNERITEM
        public object getParent() {
            return base.OwnerItem;
        }
#else
        public object getParent()
        {
            return base.Parent;
        }
#endif

        public string getName()
        {
            return base.Name;
        }

        public void setName( string value )
        {
            base.Name = value;
        }

#if COMPONENT_ENABLE_LOCATION
        public bocoree.awt.Point getLocation()
        {
            System.Drawing.Point loc = this.Location;
            return new bocoree.awt.Point( loc.X, loc.Y );
        }

        public void setLocation( int x, int y )
        {
            base.Location = new System.Drawing.Point( x, y );
        }

        public void setLocation( bocoree.awt.Point p )
        {
            base.Location = new System.Drawing.Point( p.x, p.y );
        }
#endif

        public bocoree.awt.Rectangle getBounds()
        {
            System.Drawing.Rectangle r = base.Bounds;
            return new bocoree.awt.Rectangle( r.X, r.Y, r.Width, r.Height );
        }

#if COMPONENT_ENABLE_X
        public int getX()
        {
            return base.Left;
        }
#endif

#if COMPONENT_ENABLE_Y
        public int getY()
        {
            return base.Top;
        }
#endif

        public int getWidth()
        {
            return base.Width;
        }

        public int getHeight()
        {
            return base.Height;
        }

        public bocoree.awt.Dimension getSize()
        {
            return new bocoree.awt.Dimension( base.Size.Width, base.Size.Height );
        }

        public void setSize( int width, int height )
        {
            base.Size = new System.Drawing.Size( width, height );
        }

        public void setSize( bocoree.awt.Dimension d )
        {
            setSize( d.width, d.height );
        }

        public void setBackground( bocoree.awt.Color color )
        {
            base.BackColor = System.Drawing.Color.FromArgb( color.getRed(), color.getGreen(), color.getBlue() );
        }

        public bocoree.awt.Color getBackground()
        {
            return new bocoree.awt.Color( base.BackColor.R, base.BackColor.G, base.BackColor.B );
        }

        public void setForeground( bocoree.awt.Color color )
        {
            base.ForeColor = color.color;
        }

        public bocoree.awt.Color getForeground()
        {
            return new bocoree.awt.Color( base.ForeColor.R, base.ForeColor.G, base.ForeColor.B );
        }

        public void setFont( bocoree.awt.Font font )
        {
            base.Font = font.font;
        }

        public bool getEnabled()
        {
            return base.Enabled;
        }

        public void setEnabled( bool value )
        {
            base.Enabled = value;
        }
        #endregion

        #region java.awt.TextComponent
        public string getText() {
            return base.Text;
        }

        public void setText( string value ) {
            base.Text = value;
        }
        #endregion

        public bool isImeModeOn()
        {
            return base.ImeMode == System.Windows.Forms.ImeMode.Hiragana;
        }

        public void setImeModeOn( bool value )
        {
            base.ImeMode = value ? System.Windows.Forms.ImeMode.Hiragana : System.Windows.Forms.ImeMode.Off;
        }
    }
}
#endif
