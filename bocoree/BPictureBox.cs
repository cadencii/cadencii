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
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BPictureBox.java
#else
#define COMPONENT_ENABLE_REPAINT
#define COMPONENT_ENABLE_LOCATION
#define COMPONENT_ENABLE_FOCUS

using System.Windows.Forms;

namespace bocoree.windows.forms {

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

        public void setImage( bocoree.java.awt.Image value ) {
            if ( value != null ) {
                if ( value.image != null ) {
                    base.Image = value.image;
                }
            } else {
                base.Image = null;
            }
        }

        public bocoree.java.awt.Image getImage() {
            if ( base.Image == null ) {
                return null;
            } else {
                bocoree.java.awt.Image ret = new bocoree.java.awt.Image();
                ret.image = base.Image;
                return ret;
            }
        }

        #region java.awt.Component
        // root implementation of java.awt.Component is in BForm.cs
        public java.awt.Dimension getMinimumSize() {
            return new bocoree.java.awt.Dimension( base.MinimumSize.Width, base.MinimumSize.Height );
        }

        public void setMinimumSize( java.awt.Dimension value ) {
            base.MinimumSize = new System.Drawing.Size( value.width, value.height );
        }

        public java.awt.Dimension getMaximumSize() {
            return new bocoree.java.awt.Dimension( base.MaximumSize.Width, base.MaximumSize.Height );
        }

        public void setMaximumSize( java.awt.Dimension value ) {
            base.MaximumSize = new System.Drawing.Size( value.width, value.height );
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
#endif

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
        public void setBounds( int x, int y, int width, int height ) {
            base.Bounds = new System.Drawing.Rectangle( x, y, width, height );
        }

        public void setBounds( bocoree.java.awt.Rectangle rc ) {
            base.Bounds = new System.Drawing.Rectangle( rc.x, rc.y, rc.width, rc.height );
        }

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

#if COMPONENT_ENABLE_FOCUS
        public void requestFocus() {
            base.Focus();
        }

        public bool isFocusOwner() {
            return base.Focused;
        }
#endif

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
    }

}
#endif
