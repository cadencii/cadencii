/*
 * BDialog.cs
 * Copyright ﾂｩ 2010-2011 kbinani
 *
 * This file is part of org.kbinani.windows.forms.
 *
 * org.kbinani.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ../BuildJavaUI/src/org/kbinani/windows/forms/BDialog.java
#else
#define COMPONENT_ENABLE_LOCATION
#define COMPONENT_ENABLE_Y
#define COMPONENT_ENABLE_X
#define COMPONENT_ENABLE_FOCUS
#define COMPONENT_ENABLE_CURSOR
#define COMPONENT_ENABLE_REPAINT
#define COMPONENT_ENABLE_MINMAX_SIZE
#define COMPONENT_DISABLE_VISIBLE
#define DISABLE_EXTENDED_STATE
using System;

namespace com.github.cadencii.windows.forms
{
    using boolean = System.Boolean;

    public class BDialog : System.Windows.Forms.Form
    {
        protected BDialogResult m_result = BDialogResult.CANCEL;
        private bool m_is_modal = false;

        public BDialog()
            : this( false )
        {
        }

        public BDialog( bool is_modal )
            : base()
        {
            m_is_modal = is_modal;
        }

        #region WindowStateChanged event
        // root implementation: WindowStateChanged is in BForm
        public event EventHandler WindowStateChanged;
        protected System.Windows.Forms.FormWindowState mWindowState = System.Windows.Forms.FormWindowState.Normal;
        protected override void OnSizeChanged( EventArgs e )
        {
            base.OnSizeChanged( e );
            if ( mWindowState != this.WindowState ) {
                if ( WindowStateChanged != null ) {
                    WindowStateChanged.Invoke( this, new EventArgs() );
                }
            }
            mWindowState = this.WindowState;
        }
        #endregion

        public virtual void setVisible( bool value )
        {
            if ( value ) {
                if ( m_is_modal ) {
                    System.Windows.Forms.DialogResult ret = base.ShowDialog();
                    switch ( ret ) {
                        case System.Windows.Forms.DialogResult.Yes:
                        m_result = BDialogResult.YES;
                        break;
                        case System.Windows.Forms.DialogResult.No:
                        m_result = BDialogResult.NO;
                        break;
                        case System.Windows.Forms.DialogResult.OK:
                        m_result = BDialogResult.OK;
                        break;
                        case System.Windows.Forms.DialogResult.Cancel:
                        m_result = BDialogResult.CANCEL;
                        break;
                    }
                } else {
                    base.Show();
                }
            } else {
                base.Hide();
            }
        }

        public bool isVisible()
        {
            return base.Visible;
        }

        /*public bool isModal() {
            return m_is_modal;
        }

        public void setModal( boolean value ) {
            m_is_modal = value;
        }*/

        public void setDialogResult( BDialogResult value )
        {
            switch ( value ) {
                case BDialogResult.YES: {
                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                    break;
                }
                case BDialogResult.NO: {
                    this.DialogResult = System.Windows.Forms.DialogResult.No;
                    break;
                }
                case BDialogResult.OK: {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    break;
                }
                case BDialogResult.CANCEL: {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    break;
                }
            }
        }

        public BDialogResult getDialogResult()
        {
            return m_result;
        }

        private BDialogResult processDialogResult( System.Windows.Forms.DialogResult dr )
        {
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

        public virtual BDialogResult showDialog()
        {
            return processDialogResult( base.ShowDialog() );
        }

#if JAVA
        public virtual BDialogResult showDialog( Component parent )
#else
        public virtual BDialogResult showDialog( System.Windows.Forms.Form parent )
#endif
        {
            return processDialogResult( base.ShowDialog( parent ) );
        }

        public void close()
        {
            base.Close();
        }

        public com.github.cadencii.java.awt.Dimension getClientSize()
        {
            System.Drawing.Size s = base.Size;
            return new com.github.cadencii.java.awt.Dimension( s.Width, s.Height );
        }

        // root implementation: common APIs of org.kbinani.*
        #region common APIs of org.kbinani.*
        // root implementation is in BForm.cs
        public java.awt.Point pointToScreen( java.awt.Point point_on_client )
        {
            java.awt.Point p = getLocationOnScreen();
            return new java.awt.Point( p.x + point_on_client.x, p.y + point_on_client.y );
        }

        public java.awt.Point pointToClient( java.awt.Point point_on_screen )
        {
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
        public Object getTag()
        {
            return base.Tag;
        }

        public void setTag( Object value )
        {
            base.Tag = value;
        }
#endif
        #endregion

        // root implementation of java.awt.Component
        #region java.awt.Component
        // root implementation of java.awt.Component is in BForm.cs
        public java.awt.Dimension getMinimumSize()
        {
#if COMPONENT_ENABLE_MINMAX_SIZE
            int w = base.MinimumSize.Width;
            int h = base.MinimumSize.Height;
#else
            int w = 0;
            int h = 0;
#endif
            return new com.github.cadencii.java.awt.Dimension( w, h );
        }

        public void setMinimumSize( java.awt.Dimension value )
        {
#if COMPONENT_ENABLE_MINMAX_SIZE
            base.MinimumSize = new System.Drawing.Size( value.width, value.height );
#endif
        }

        public java.awt.Dimension getMaximumSize()
        {
#if COMPONENT_ENABLE_MINMAX_SIZE
            int w = base.MaximumSize.Width;
            int h = base.MaximumSize.Height;
#else
            int w = int.MaxValue;
            int h = int.MaxValue;
#endif
            return new com.github.cadencii.java.awt.Dimension( w, h );
        }

        public void setMaximumSize( java.awt.Dimension value )
        {
#if COMPONENT_ENABLE_MINMAX_SIZE
            base.MaximumSize = new System.Drawing.Size( value.width, value.height );
#endif
        }

        public void invalidate()
        {
            base.Invalidate();
        }

#if COMPONENT_ENABLE_REPAINT
        public void repaint()
        {
            base.Refresh();
        }
#endif

#if COMPONENT_ENABLE_CURSOR
        public com.github.cadencii.java.awt.Cursor getCursor()
        {
            System.Windows.Forms.Cursor c = base.Cursor;
            com.github.cadencii.java.awt.Cursor ret = null;
            if ( c.Equals( System.Windows.Forms.Cursors.Arrow ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.DEFAULT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Cross ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.CROSSHAIR_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Default ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.DEFAULT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Hand ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.HAND_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.IBeam ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.TEXT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanEast ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.E_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNE ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.NE_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNorth ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.N_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNW ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.NW_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSE ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.SE_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSouth ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.S_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSW ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.SW_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanWest ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.W_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.SizeAll ) ) {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.MOVE_CURSOR );
            } else {
                ret = new com.github.cadencii.java.awt.Cursor( com.github.cadencii.java.awt.Cursor.CUSTOM_CURSOR );
            }
            ret.cursor = c;
            return ret;
        }

        public void setCursor( com.github.cadencii.java.awt.Cursor value )
        {
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
        public void setBounds( int x, int y, int width, int height )
        {
            base.Bounds = new System.Drawing.Rectangle( x, y, width, height );
        }

        public void setBounds( com.github.cadencii.java.awt.Rectangle rc )
        {
            base.Bounds = new System.Drawing.Rectangle( rc.x, rc.y, rc.width, rc.height );
        }

        public com.github.cadencii.java.awt.Point getLocationOnScreen()
        {
            System.Drawing.Point p = base.PointToScreen( new System.Drawing.Point( 0, 0 ) );
            return new com.github.cadencii.java.awt.Point( p.X, p.Y );
        }

        public com.github.cadencii.java.awt.Point getLocation()
        {
            System.Drawing.Point loc = this.Location;
            return new com.github.cadencii.java.awt.Point( loc.X, loc.Y );
        }

        public void setLocation( int x, int y )
        {
            base.Location = new System.Drawing.Point( x, y );
        }

        public void setLocation( com.github.cadencii.java.awt.Point p )
        {
            base.Location = new System.Drawing.Point( p.x, p.y );
        }
#endif

        public com.github.cadencii.java.awt.Rectangle getBounds()
        {
            System.Drawing.Rectangle r = base.Bounds;
            return new com.github.cadencii.java.awt.Rectangle( r.X, r.Y, r.Width, r.Height );
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

        public com.github.cadencii.java.awt.Dimension getSize()
        {
            return new com.github.cadencii.java.awt.Dimension( base.Size.Width, base.Size.Height );
        }

        public void setSize( int width, int height )
        {
            base.Size = new System.Drawing.Size( width, height );
        }

        public void setSize( com.github.cadencii.java.awt.Dimension d )
        {
            setSize( d.width, d.height );
        }

        public void setBackground( com.github.cadencii.java.awt.Color color )
        {
            base.BackColor = System.Drawing.Color.FromArgb( color.getRed(), color.getGreen(), color.getBlue() );
        }

        public com.github.cadencii.java.awt.Color getBackground()
        {
            return new com.github.cadencii.java.awt.Color( base.BackColor.R, base.BackColor.G, base.BackColor.B );
        }

        public void setForeground( com.github.cadencii.java.awt.Color color )
        {
            base.ForeColor = color.color;
        }

        public com.github.cadencii.java.awt.Color getForeground()
        {
            return new com.github.cadencii.java.awt.Color( base.ForeColor.R, base.ForeColor.G, base.ForeColor.B );
        }

        public bool isEnabled()
        {
            return base.Enabled;
        }

        public void setEnabled( bool value )
        {
            base.Enabled = value;
        }

#if COMPONENT_ENABLE_FOCUS
        public void requestFocus()
        {
            base.Focus();
        }

        public bool isFocusOwner()
        {
            return base.Focused;
        }
#endif

        public void setPreferredSize( com.github.cadencii.java.awt.Dimension size )
        {
            base.Size = new System.Drawing.Size( size.width, size.height );
        }

        public com.github.cadencii.java.awt.Font getFont()
        {
            return new com.github.cadencii.java.awt.Font( base.Font );
        }

        public void setFont( com.github.cadencii.java.awt.Font font )
        {
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
        public void toFront()
        {
            base.BringToFront();
        }

        public void setAlwaysOnTop( boolean alwaysOnTop )
        {
            base.TopMost = alwaysOnTop;
        }

        public boolean isAlwaysOnTop()
        {
            return base.TopMost;
        }
        #endregion

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

        public void setIconImage( System.Drawing.Icon icon )
        {
            base.Icon = icon;
        }

        public System.Drawing.Icon getIconImage()
        {
            return base.Icon;
        }

        public int getState()
        {
            if ( base.WindowState == System.Windows.Forms.FormWindowState.Minimized ) {
                return ICONIFIED;
            } else {
                return NORMAL;
            }
        }

        public void setState( int state )
        {
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

#if !DISABLE_EXTENDED_STATE
        public int getExtendedState() {
            if ( base.WindowState == System.Windows.Forms.FormWindowState.Maximized ) {
                return MAXIMIZED_BOTH;
            } else if ( base.WindowState == System.Windows.Forms.FormWindowState.Minimized ) {
                return ICONIFIED;
            } else {
                return NORMAL;
            }
        }
#endif

        public void setExtendedState( int value )
        {
            if ( value == ICONIFIED ) {
                base.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            } else if ( value == MAXIMIZED_BOTH ) {
                base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            } else {
                base.WindowState = System.Windows.Forms.FormWindowState.Normal;
            }
        }

        public string getTitle()
        {
            return base.Text;
        }

        public void setTitle( string value )
        {
            base.Text = value;
        }
        #endregion

        // root implementation of javax.swing.JComponent
        #region javax.swing.JComponent
        // root implementation of javax.swing.JComponent is in BForm.cs
        public bool requestFocusInWindow()
        {
            return base.Focus();
        }
        #endregion
    }

}
#endif
