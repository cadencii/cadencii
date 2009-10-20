/*
 * BCheckBox.cs
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

public class BCheckBox extends JCheckBox{
}
#else
#define ABSTRACT_BUTTON_ENABLE_IS_SELECTED
namespace bocoree.windows.forms {
    public class BCheckBox : System.Windows.Forms.CheckBox{
        #region java.awt.Component
        public bocoree.awt.Point getLocation() {
            System.Drawing.Point loc = this.Location;
            return new bocoree.awt.Point( loc.X, loc.Y );
        }

        public void setLocation( int x, int y ) {
            base.Location = new System.Drawing.Point( x, y );
        }

        public void setLocation( bocoree.awt.Point p ) {
            base.Location = new System.Drawing.Point( p.x, p.y );
        }

        public bocoree.awt.Rectangle getBounds() {
            System.Drawing.Rectangle r = base.Bounds;
            return new bocoree.awt.Rectangle( r.X, r.Y, r.Width, r.Height );
        }

        public int getX() {
            return base.Left;
        }

        public int getY() {
            return base.Top;
        }

        public int getWidth() {
            return base.Width;
        }

        public int getHeight() {
            return base.Height;
        }

        public bocoree.awt.Dimension getSize() {
            return new bocoree.awt.Dimension( base.Size.Width, base.Size.Height );
        }

        public void setSize( int width, int height ) {
            base.Size = new System.Drawing.Size( width, height );
        }

        public void setSize( bocoree.awt.Dimension d ) {
            setSize( d.width, d.height );
        }

        public void setBackground( bocoree.awt.Color color ) {
            base.BackColor = System.Drawing.Color.FromArgb( color.getRed(), color.getGreen(), color.getBlue() );
        }

        public void setFont( bocoree.awt.Font font ) {
            base.Font = font.font;
        }

        public bool getEnabled() {
            return base.Enabled;
        }

        public void setEnabled( bool value ) {
            base.Enabled = value;
        }
        #endregion

        #region javax.swing.AbstractButton
        public string getText() {
            return base.Text;
        }

        public void setText( string value ) {
            base.Text = value;
        }

#if ABSTRACT_BUTTON_ENABLE_IS_SELECTED
        public bool isSelected() {
            return base.Checked;
        }

        public void setSelected( bool value ) {
            base.Checked = true;
        }
#endif
        #endregion
    }
}
#endif
