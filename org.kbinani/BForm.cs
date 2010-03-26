/*
 * BForm.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ../BuildJavaUI/src/org/kbinani/windows/forms/BForm.java
#else
#define COMPONENT_ENABLE_LOCATION
#define COMPONENT_ENABLE_Y
#define COMPONENT_ENABLE_X
#define COMPONENT_ENABLE_FOCUS
#define COMPONENT_ENABLE_CURSOR
#define COMPONENT_ENABLE_REPAINT
#define COMPONENT_ENABLE_MINMAX_SIZE

using System;

namespace org.kbinani.windows.forms {
    using boolean = System.Boolean;

    public class BForm : System.Windows.Forms.Form {
        // root impl of Load event
        #region event impl Load
        // root impl of Load event is in BForm.cs
        public BEvent<BEventHandler> loadEvent = new BEvent<BEventHandler>();
        protected override void OnLoad( EventArgs e ) {
            base.OnLoad( e );
            loadEvent.raise( this, e );
        }
        #endregion

        // root implf of Activated
        #region event impl Activated
        // root implf of Activated is in BForm
        public BEvent<BEventHandler> activatedEvent = new BEvent<BEventHandler>();
        protected override void OnActivated( EventArgs e ) {
            base.OnActivated( e );
            activatedEvent.raise( this, e );
        }
        #endregion

        // root implf of Deactivate
        #region event impl Deactivate
        // root implf of Deactivate is in BForm
        public BEvent<BEventHandler> deactivateEvent = new BEvent<BEventHandler>();
        protected override void OnDeactivate( EventArgs e ) {
            base.OnDeactivate( e );
            deactivateEvent.raise( this, e );
        }
        #endregion

        // root implf of FormClosed
        #region event impl FormClosed
        // root implf of FormClosed is in BForm
        public BEvent<BFormClosedEventHandler> formClosedEvent = new BEvent<BFormClosedEventHandler>();
        protected override void OnFormClosed( System.Windows.Forms.FormClosedEventArgs e ) {
            base.OnFormClosed( e );
            formClosedEvent.raise( this, e );
        }
        #endregion

        // root implf of FormClosing
        #region event impl FormClosing
        // root implf of FormClosing is in BForm
        public BEvent<BFormClosingEventHandler> formClosingEvent = new BEvent<BFormClosingEventHandler>();
        protected override void OnFormClosing( System.Windows.Forms.FormClosingEventArgs e ) {
            base.OnFormClosing( e );
            formClosingEvent.raise( this, e );
        }
        #endregion

        #region event impl PreviewKeyDown
        // root implf of PreviewKeyDown is in BButton
        public BEvent<BPreviewKeyDownEventHandler> previewKeyDownEvent = new BEvent<BPreviewKeyDownEventHandler>();
        protected override void OnPreviewKeyDown( System.Windows.Forms.PreviewKeyDownEventArgs e ) {
            base.OnPreviewKeyDown( e );
            previewKeyDownEvent.raise( this, e );
        }
        #endregion

        #region event impl KeyUp
        // root impl of KeyUp event is in BPictureBox
        public BEvent<BKeyEventHandler> keyUpEvent = new BEvent<BKeyEventHandler>();
        protected override void OnKeyUp( System.Windows.Forms.KeyEventArgs e ) {
            base.OnKeyUp( e );
            keyUpEvent.raise( this, e );
        }
        #endregion

        #region event impl MouseMove
        // root impl of MouseMove event is in BButton
        public BEvent<BMouseEventHandler> mouseMoveEvent = new BEvent<BMouseEventHandler>();
        protected override void OnMouseMove( System.Windows.Forms.MouseEventArgs mevent ) {
            base.OnMouseMove( mevent );
            mouseMoveEvent.raise( this, mevent );
        }
        #endregion

        #region event impl MouseDown
        // root impl of MouseDown event is in BButton
        public BEvent<BMouseEventHandler> mouseDownEvent = new BEvent<BMouseEventHandler>();
        protected override void OnMouseDown( System.Windows.Forms.MouseEventArgs mevent ) {
            base.OnMouseDown( mevent );
            mouseDownEvent.raise( this, mevent );
        }
        #endregion

        #region event impl MouseUp
        // root impl of MouseUp event is in BButton
        public BEvent<BMouseEventHandler> mouseUpEvent = new BEvent<BMouseEventHandler>();
        protected override void OnMouseUp( System.Windows.Forms.MouseEventArgs mevent ) {
            base.OnMouseUp( mevent );
            mouseUpEvent.raise( this, mevent );
        }
        #endregion

        #region event impl LocationChanged
        // root implf of PreviewKeyDown is in BButton
        public BEvent<BEventHandler> locationChangedEvent = new BEvent<BEventHandler>();
        protected override void OnLocationChanged( EventArgs e ) {
            base.OnLocationChanged( e );
            locationChangedEvent.raise( this, e );
        }
        #endregion

        public BForm()
            : base() {
        }

        public void close() {
            base.Close();
        }

        public org.kbinani.java.awt.Dimension getClientSize() {
            System.Drawing.Size s = base.Size;
            return new org.kbinani.java.awt.Dimension( s.Width, s.Height );
        }

        // root implementation: common APIs of org.kbinani.*
        #region common APIs of org.kbinani.*
        // root implementation is in BForm.cs
        public java.awt.Point pointToScreen( java.awt.Point point_on_client ) {
            java.awt.Point p = getLocationOnScreen();
            return new java.awt.Point( p.x + point_on_client.x, p.y + point_on_client.y );
        }

        public java.awt.Point pointToClient( java.awt.Point point_on_screen ) {
            java.awt.Point p = getLocationOnScreen();
            return new java.awt.Point( point_on_screen.x - p.x, point_on_screen.y - p.y );
        }

#if JAVA
        Object tag = null;
        public Object getTag(){
            return tag;
        }

        public void setTag( Object value ){
            tag = value;
        }
#else
        public Object getTag() {
            return base.Tag;
        }

        public void setTag( Object value ) {
            base.Tag = value;
        }
#endif
        #endregion

        // root implementation of java.awt.Component
        #region java.awt.Component
        // root implementation of java.awt.Component is in BForm.cs
        public java.awt.Dimension getMinimumSize() {
#if COMPONENT_ENABLE_MINMAX_SIZE
            int w = base.MinimumSize.Width;
            int h = base.MinimumSize.Height;
#else
            int w = 0;
            int h = 0;
#endif
            return new org.kbinani.java.awt.Dimension( w, h );
        }

        public void setMinimumSize( java.awt.Dimension value ) {
#if COMPONENT_ENABLE_MINMAX_SIZE
            base.MinimumSize = new System.Drawing.Size( value.width, value.height );
#endif
        }

        public java.awt.Dimension getMaximumSize() {
#if COMPONENT_ENABLE_MINMAX_SIZE
            int w = base.MaximumSize.Width;
            int h = base.MaximumSize.Height;
#else
            int w = int.MaxValue;
            int h = int.MaxValue;
#endif
            return new org.kbinani.java.awt.Dimension( w, h );
        }

        public void setMaximumSize( java.awt.Dimension value ) {
#if COMPONENT_ENABLE_MINMAX_SIZE
            base.MaximumSize = new System.Drawing.Size( value.width, value.height );
#endif
        }

        public void invalidate() {
            base.Invalidate();
        }

#if COMPONENT_ENABLE_REPAINT
        public void repaint() {
            base.Refresh();
        }
#endif

#if COMPONENT_ENABLE_CURSOR
        public org.kbinani.java.awt.Cursor getCursor() {
            System.Windows.Forms.Cursor c = base.Cursor;
            org.kbinani.java.awt.Cursor ret = null;
            if( c.Equals( System.Windows.Forms.Cursors.Arrow ) ){
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.DEFAULT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Cross ) ){
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.CROSSHAIR_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Default ) ){
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.DEFAULT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Hand ) ){
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.HAND_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.IBeam ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.TEXT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanEast ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.E_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNE ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.NE_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNorth ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.N_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNW ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.NW_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSE ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.SE_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSouth ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.S_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSW ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.SW_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanWest ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.W_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.SizeAll ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.MOVE_CURSOR );
            } else {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.CUSTOM_CURSOR );
            }
            ret.cursor = c;
            return ret;
        }

        public void setCursor( org.kbinani.java.awt.Cursor value ) {
            base.Cursor = value.cursor;
        }
#endif

#if !COMPONENT_DISABLE_VISIBLE
        public bool isVisible() {
            return base.Visible;
        }

        public void setVisible( bool value ) {
            base.Visible = value;
        }
#endif

#if COMPONENT_ENABLE_TOOL_TIP_TEXT
        public void setToolTipText( string value )
        {
            base.ToolTipText = value;
        }

        public String getToolTipText()
        {
            return base.ToolTipText;
        }
#endif

#if COMPONENT_PARENT_AS_OWNERITEM
        public Object getParent() {
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
        public void setBounds( int x, int y, int width, int height ) {
            base.Bounds = new System.Drawing.Rectangle( x, y, width, height );
        }

        public void setBounds( org.kbinani.java.awt.Rectangle rc ) {
            base.Bounds = new System.Drawing.Rectangle( rc.x, rc.y, rc.width, rc.height );
        }

        public org.kbinani.java.awt.Point getLocationOnScreen() {
            System.Drawing.Point p = base.PointToScreen( new System.Drawing.Point( 0, 0 ) );
            return new org.kbinani.java.awt.Point( p.X, p.Y );
        }

        public org.kbinani.java.awt.Point getLocation() {
            System.Drawing.Point loc = this.Location;
            return new org.kbinani.java.awt.Point( loc.X, loc.Y );
        }

        public void setLocation( int x, int y ) {
            base.Location = new System.Drawing.Point( x, y );
        }

        public void setLocation( org.kbinani.java.awt.Point p ) {
            base.Location = new System.Drawing.Point( p.x, p.y );
        }
#endif

        public org.kbinani.java.awt.Rectangle getBounds() {
            System.Drawing.Rectangle r = base.Bounds;
            return new org.kbinani.java.awt.Rectangle( r.X, r.Y, r.Width, r.Height );
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

        public org.kbinani.java.awt.Dimension getSize() {
            return new org.kbinani.java.awt.Dimension( base.Size.Width, base.Size.Height );
        }

        public void setSize( int width, int height ) {
            base.Size = new System.Drawing.Size( width, height );
        }

        public void setSize( org.kbinani.java.awt.Dimension d ) {
            setSize( d.width, d.height );
        }

        public void setBackground( org.kbinani.java.awt.Color color ) {
            base.BackColor = System.Drawing.Color.FromArgb( color.getRed(), color.getGreen(), color.getBlue() );
        }

        public org.kbinani.java.awt.Color getBackground() {
            return new org.kbinani.java.awt.Color( base.BackColor.R, base.BackColor.G, base.BackColor.B );
        }

        public void setForeground( org.kbinani.java.awt.Color color ) {
            base.ForeColor = color.color;
        }

        public org.kbinani.java.awt.Color getForeground() {
            return new org.kbinani.java.awt.Color( base.ForeColor.R, base.ForeColor.G, base.ForeColor.B );
        }

        public bool isEnabled() {
            return base.Enabled;
        }

        public void setEnabled( bool value ) {
            base.Enabled = value;
        }

#if COMPONENT_ENABLE_FOCUS
        public void requestFocus() {
            base.Focus();
        }

        public bool isFocusOwner() {
            return base.Focused;
        }
#endif

        public void setPreferredSize( org.kbinani.java.awt.Dimension size ) {
            base.Size = new System.Drawing.Size( size.width, size.height );
        }

        public org.kbinani.java.awt.Font getFont() {
            return new org.kbinani.java.awt.Font( base.Font );
        }

        public void setFont( org.kbinani.java.awt.Font font ) {
            if ( font == null ) {
                return;
            }
            if ( font.font == null ) {
                return;
            }
            base.Font = font.font;
        }
        #endregion

        // root implementation of java.awt.Window
        #region java.awt.Window
        // root implementation of java.awt.Window is in BForm.cs
        public void toFront() {
            base.BringToFront();
        }
        
        public void setAlwaysOnTop( boolean alwaysOnTop ) {
            base.TopMost = alwaysOnTop;
        }

        public boolean isAlwaysOnTop() {
            return base.TopMost;
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
