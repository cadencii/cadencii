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
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BForm.java
#else
#define COMPONENT_ENABLE_LOCATION
#define COMPONENT_ENABLE_Y
#define COMPONENT_ENABLE_X

using System;

namespace bocoree.windows.forms {
    using boolean = System.Boolean;

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

        public void close() {
            base.Close();
        }

        public bocoree.java.awt.Dimension getClientSize() {
            System.Drawing.Size s = base.Size;
            return new bocoree.java.awt.Dimension( s.Width, s.Height );
        }

        // root implementation: common APIs of org.kbinani.*
        #region common APIs of org.kbinani.*
        // root implementation is in BForm.cs
        public bocoree.java.awt.Point pointToScreen( bocoree.java.awt.Point point_on_client ) {
            bocoree.java.awt.Point p = getLocationOnScreen();
            return new bocoree.java.awt.Point( p.x + point_on_client.x, p.y + point_on_client.y );
        }

        public bocoree.java.awt.Point pointToClient( bocoree.java.awt.Point point_on_screen ) {
            bocoree.java.awt.Point p = getLocationOnScreen();
            return new bocoree.java.awt.Point( point_on_screen.x - p.x, point_on_screen.y - p.y );
        }
        #endregion

        // root implementation of java.awt.Component
        #region java.awt.Component
        // root implementation of java.awt.Component is in BForm.cs
        public void invalidate() {
            base.Invalidate();
        }

        public void repaint() {
            base.Refresh();
        }

        public void setBounds( int x, int y, int width, int height ) {
            base.Bounds = new System.Drawing.Rectangle( x, y, width, height );
        }

        public void setBounds( bocoree.java.awt.Rectangle rc ) {
            base.Bounds = new System.Drawing.Rectangle( rc.x, rc.y, rc.width, rc.height );
        }

        public bocoree.java.awt.Cursor getCursor() {
            System.Windows.Forms.Cursor c = base.Cursor;
            bocoree.java.awt.Cursor ret = null;
            if( c.Equals( System.Windows.Forms.Cursors.Arrow ) ){
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.DEFAULT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Cross ) ){
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.CROSSHAIR_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Default ) ){
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.DEFAULT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Hand ) ){
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.HAND_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.IBeam ) ) {
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.TEXT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanEast ) ) {
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.E_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNE ) ) {
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.NE_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNorth ) ) {
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.N_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNW ) ) {
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.NW_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSE ) ) {
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.SE_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSouth ) ) {
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.S_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSW ) ) {
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.SW_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanWest ) ) {
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.W_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.SizeAll ) ) {
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.MOVE_CURSOR );
            } else {
                ret = new bocoree.java.awt.Cursor( bocoree.java.awt.Cursor.CUSTOM_CURSOR );
            }
            ret.cursor = c;
            return ret;
        }

        public void setCursor( bocoree.java.awt.Cursor value ) {
            base.Cursor = value.cursor;
        }

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
        public bocoree.java.awt.Point getLocationOnScreen() {
            System.Drawing.Point p = base.PointToScreen( base.Location );
            return new bocoree.java.awt.Point( p.X, p.Y );
        }

        public bocoree.java.awt.Point getLocation() {
            System.Drawing.Point loc = this.Location;
            return new bocoree.java.awt.Point( loc.X, loc.Y );
        }

        public void setLocation( int x, int y ) {
            base.Location = new System.Drawing.Point( x, y );
        }

        public void setLocation( bocoree.java.awt.Point p ) {
            base.Location = new System.Drawing.Point( p.x, p.y );
        }
#endif

        public bocoree.java.awt.Rectangle getBounds() {
            System.Drawing.Rectangle r = base.Bounds;
            return new bocoree.java.awt.Rectangle( r.X, r.Y, r.Width, r.Height );
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

        public bocoree.java.awt.Dimension getSize() {
            return new bocoree.java.awt.Dimension( base.Size.Width, base.Size.Height );
        }

        public void setSize( int width, int height ) {
            base.Size = new System.Drawing.Size( width, height );
        }

        public void setSize( bocoree.java.awt.Dimension d ) {
            setSize( d.width, d.height );
        }

        public void setBackground( bocoree.java.awt.Color color ) {
            base.BackColor = System.Drawing.Color.FromArgb( color.getRed(), color.getGreen(), color.getBlue() );
        }

        public bocoree.java.awt.Color getBackground() {
            return new bocoree.java.awt.Color( base.BackColor.R, base.BackColor.G, base.BackColor.B );
        }

        public void setForeground( bocoree.java.awt.Color color ) {
            base.ForeColor = color.color;
        }

        public bocoree.java.awt.Color getForeground() {
            return new bocoree.java.awt.Color( base.ForeColor.R, base.ForeColor.G, base.ForeColor.B );
        }

        public bool isEnabled() {
            return base.Enabled;
        }

        public void setEnabled( bool value ) {
            base.Enabled = value;
        }

        public void requestFocus() {
            base.Focus();
        }

        public bool isFocusOwner() {
            return base.Focused;
        }

        public void setPreferredSize( bocoree.java.awt.Dimension size ) {
            base.Size = new System.Drawing.Size( size.width, size.height );
        }

        public bocoree.java.awt.Font getFont() {
            return new bocoree.java.awt.Font( base.Font );
        }

        public void setFont( bocoree.java.awt.Font font ) {
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
        public void setMinimumSize( bocoree.java.awt.Dimension size ) {
            base.MinimumSize = new System.Drawing.Size( size.width, size.height );
        }

        public bocoree.java.awt.Dimension getMinimumSize() {
            return new bocoree.java.awt.Dimension( base.MinimumSize.Width, base.MinimumSize.Height );
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
