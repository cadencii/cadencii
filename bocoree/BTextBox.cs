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

import java.awt.event.*;
import javax.swing.*;
import org.kbinani.*;

public class BTextBox extends JTextField implements KeyListener{
    public BTextBox(){
        super();
        addKeyListener( this );
    }

    public boolean isImeModeOn(){
        return getInputContext().isCompositionEnabled();
    }

    public void setImeModeOn( boolean value ){
        getInputContext().setCompositionEnabled( value );
    }

    /* root implementation of bocoree.windows.forms.[component] */
    /* REGION bocoree.windows.forms.[component] */
    /* root implementation of bocoree.windows.forms.[component] is in BTextBox.cs */
    private Object m_tag = null;

    public Object getTag(){
        return m_tag;
    }

    public void setTag( Object value ){
        m_tag = value;
    }
    /* END REGION */

    /* REGION java.awt.Component */
    /* root implementation of java.awt.Component is in BForm.cs(java) */
    public BEvent<BKeyEventHandler> keyUpEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyEventHandler> keyDownEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyEventHandler> keyPressedEvent = new BEvent<BKeyEventHandler>();

    public void keyPressed( KeyEvent e0 ){
        try{
            BKeyEventArgs e = new BKeyEventArgs( e0 );
            keyDownEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BForm#keyPressed; ex=" + ex );
        }
    }

    public void keyReleased( KeyEvent e0 ){
        try{
            BKeyEventArgs e = new BKeyEventArgs( e0 );
            keyUpEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BForm#keyReleased; ex=" + ex );
        }
    }

    public void keyTyped( KeyEvent e0 ){
        try{
            BKeyEventArgs e = new BKeyEventArgs( e0 );
            keyPressedEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BForm#keyTyped; ex=" + ex );
        }
    }
    /* END REGION java.awt.Component */
}
#else
#define COMPONENT_ENABLE_LOCATION
namespace bocoree.windows.forms{

    public class BTextBox : System.Windows.Forms.TextBox {
        #region java.awt.Component
        // root implementation of java.awt.Component is in BForm.cs
        public bool isVisible() {
            return base.Visible;
        }

        public void setVisible( bool value ) {
            base.Visible = value;
        }

#if COMPONENT_ENABLE_TOOL_TIP_TEXT
        public void setToolTipText( string value )
        {
            base.ToolTipText = value;
        }

        public string getToolTipText()
        {
            return base.ToolTipText;
        }
#endif

#if COMPONENT_PARENT_AS_OWNERITEM
        public object getParent() {
            return base.OwnerItem;
        }
#else
        public object getParent() {
            return base.Parent;
        }
#endif

        public string getName() {
            return base.Name;
        }

        public void setName( string value ) {
            base.Name = value;
        }

#if COMPONENT_ENABLE_LOCATION
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
#endif

        public bocoree.awt.Rectangle getBounds() {
            System.Drawing.Rectangle r = base.Bounds;
            return new bocoree.awt.Rectangle( r.X, r.Y, r.Width, r.Height );
        }

#if COMPONENT_ENABLE_X
        public int getX() {
            return base.Left;
        }
#endif

#if COMPONENT_ENABLE_Y
        public int getY() {
            return base.Top;
        }
#endif

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

        public bocoree.awt.Color getBackground() {
            return new bocoree.awt.Color( base.BackColor.R, base.BackColor.G, base.BackColor.B );
        }

        public void setForeground( bocoree.awt.Color color ) {
            base.ForeColor = color.color;
        }

        public bocoree.awt.Color getForeground() {
            return new bocoree.awt.Color( base.ForeColor.R, base.ForeColor.G, base.ForeColor.B );
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

        public void requestFocus() {
            base.Focus();
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

        #region javax.swing.JComponent
        // root implementation of javax.swing.JComponent is in BForm.cs
        public bool requestFocusInWindow() {
            return base.Focus();
        }
        #endregion

        public bool isImeModeOn() {
            return base.ImeMode == System.Windows.Forms.ImeMode.Hiragana;
        }

        public void setImeModeOn( bool value ){
            base.ImeMode = value ? System.Windows.Forms.ImeMode.Hiragana : System.Windows.Forms.ImeMode.Off;
        }

        public void selectAll() {
            base.SelectAll();
        }

        public void setTag( object value ) {
            base.Tag = value;
        }

        public object getTag() {
            return base.Tag;
        }
    }

}
#endif
