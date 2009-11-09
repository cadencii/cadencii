/*
 * BForm.cs
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

import org.kbinani.*;
import javax.swing.*;
import java.awt.event.*;
import java.lang.reflect.*;

public class BForm extends JFrame implements WindowListener, KeyListener{
    public BEvent<BEventHandler> formClosingEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> formClosedEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> activatedEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> deactivateEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> loadEvent = new BEvent<BEventHandler>();
    private BDialogResult m_result = BDialogResult.CANCEL;
    private boolean m_closed = false;

    public BForm(){
        this( "" );
    }

    public BForm( String title ){
        super( title );
        addWindowListener( this );
        addKeyListener( this );
    }

    public void windowActivated( WindowEvent e ){
        try{
            activatedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BForm#windowActivated; ex=" + ex );
        }
    }

    public void windowClosed( WindowEvent e ){
        m_closed = true;
        try{
            formClosedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BForm#windowClosed; ex=" + ex );
        }
    }

    public void windowClosing( WindowEvent e ){
        try{
            formClosingEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BForm#windowClosing; ex=" + ex );
        }
    }

    public void windowDeactivated( WindowEvent e ){
        try{
            deactivateEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BForm#windowDeactivated; ex=" + ex );
        }
    }

    public void windowDeiconified( WindowEvent e ){
    }

    public void windowIconified( WindowEvent e ){
    }

    public void windowOpened( WindowEvent e ){
        try{
            loadEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BForm#windowOpened; ex=" + ex );
        }
    }

    public class ShowDialogRunner implements Runnable{
        public void run(){
            show();
            while( !m_closed ){
                try{
                    Thread.sleep( 100 );
                }catch( Exception ex ){
                    break;
                }
            }
            hide();
        }
    }

    public BDialogResult showDialog(){
        try{
            Thread t = new Thread( new ShowDialogRunner() );
            t.start();
            t.join();
        }catch( Exception ex ){
            System.out.println( "BForm#showDialog; ex=" + ex );
        }
        return m_result;
    }

    public BDialogResult getDialogResult(){
        return m_result;
    }

    public void setDialogResult( BDialogResult value ){
        m_closed = true;
        m_result = value;
    }

    /* root implementation of java.awt.Component */
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
#define COMPONENT_ENABLE_Y
#define COMPONENT_ENABLE_X
namespace bocoree.windows.forms {

    public class BForm : System.Windows.Forms.Form {
        protected BDialogResult m_result = BDialogResult.CANCEL;

        public void setDialogResult( BDialogResult value ) {
            switch ( value ) {
                case BDialogResult.YES:
                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                    break;
                case BDialogResult.NO:
                    this.DialogResult = System.Windows.Forms.DialogResult.No;
                    break;
                case BDialogResult.OK:
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    break;
                case BDialogResult.CANCEL:
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    break;
            }
        }

        public BDialogResult getDialogResult() {
            return m_result;
        }

        public BDialogResult showDialog() {
            System.Windows.Forms.DialogResult dr = base.ShowDialog();
            if ( dr == System.Windows.Forms.DialogResult.OK ) {
                m_result = BDialogResult.OK;
            } else if ( dr == System.Windows.Forms.DialogResult.Cancel ) {
                m_result = BDialogResult.CANCEL;
            } else if ( dr == System.Windows.Forms.DialogResult.Yes ) {
                m_result = BDialogResult.YES;
            } else if ( dr == System.Windows.Forms.DialogResult.No ) {
                m_result = BDialogResult.NO;
            }
            return m_result;
        }

        // root implementation of java.awt.Component
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
        #endregion

        // root implementation of java.awt.Window
        #region java.awt.Window
        // root implementation of java.awt.Window is in BForm.cs
        public void setBounds( int x, int y, int width, int height ) {
            base.Bounds = new System.Drawing.Rectangle( x, y, width, height );
        }

        public void setBounds( bocoree.awt.Rectangle rc ) {
            base.Bounds = new System.Drawing.Rectangle( rc.x, rc.y, rc.width, rc.height );
        }

        public void setMinimumSize( bocoree.awt.Dimension size ) {
            base.MinimumSize = new System.Drawing.Size( size.width, size.height );
        }
        #endregion

        // root implementation of java.awt.Frame
        #region java.awt.Frame
        // root implementation of java.awt.Frame is in BForm.cs
        public const int CROSSHAIR_CURSOR = 1;
        public const int DEFAULT_CURSOR = 0;
        public const int E_RESIZE_CURSOR = 11;
        public const int HAND_CURSOR = 12;
        public const int ICONIFIED = 1;
        public const int MAXIMIZED_BOTH = 6;
        public const int MAXIMIZED_HORIZ = 2;
        public const int MAXIMIZED_VERT = 4;
        public const int MOVE_CURSOR = 13;
        public const int N_RESIZE_CURSOR = 8;
        public const int NE_RESIZE_CURSOR = 7;
        public const int NORMAL = 0;
        public const int NW_RESIZE_CURSOR = 6;
        public const int S_RESIZE_CURSOR = 9;
        public const int SE_RESIZE_CURSOR = 5;
        public const int SW_RESIZE_CURSOR = 4;
        public const int TEXT_CURSOR = 2;
        public const int W_RESIZE_CURSOR = 10;
        public const int WAIT_CURSOR = 3;

        public void setIconImage( System.Drawing.Icon icon ) {
            base.Icon = icon;
        }

        public System.Drawing.Icon getIconImage() {
            return base.Icon;
        }

        public int getState() {
            if ( base.WindowState == System.Windows.Forms.FormWindowState.Minimized ) {
                return ICONIFIED;
            } else {
                return NORMAL;
            }
        }

        public void setState( int state ) {
            if ( state == ICONIFIED ) {
                if ( base.WindowState != System.Windows.Forms.FormWindowState.Minimized ) {
                    base.WindowState = System.Windows.Forms.FormWindowState.Minimized;
                }
            } else {
                if ( base.WindowState == System.Windows.Forms.FormWindowState.Minimized ) {
                    base.WindowState = System.Windows.Forms.FormWindowState.Normal;
                }
            }
        }

        public int getExtendedState() {
            if ( base.WindowState == System.Windows.Forms.FormWindowState.Maximized ) {
                return MAXIMIZED_BOTH;
            } else if ( base.WindowState == System.Windows.Forms.FormWindowState.Minimized ) {
                return ICONIFIED;
            } else {
                return NORMAL;
            }
        }

        public void setExtendedState( int value ) {
            if ( value == ICONIFIED ) {
                base.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            } else if ( value == MAXIMIZED_BOTH ) {
                base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            } else {
                base.WindowState = System.Windows.Forms.FormWindowState.Normal;
            }
        }

        public string getTitle() {
            return base.Text;
        }

        public void setTitle( string value ) {
            base.Text = value;
        }
        #endregion

        // root implementation of javax.swing.JComponent
        #region javax.swing.JComponent
        // root implementation of javax.swing.JComponent is in BForm.cs
        public bool requestFocusInWindow() {
            return base.Focus();
        }
        #endregion
    }

}
#endif
