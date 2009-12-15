/*
 * BMenuItem.cs
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
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BMenuItem.java
#else
#define ABSTRACT_BUTTON_ENABLE_IS_SELECTED
#define COMPONENT_PARENT_AS_OWNERITEM
#define COMPONENT_ENABLE_TOOL_TIP_TEXT
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using bocoree.javax.swing;

namespace bocoree.windows.forms {
    public class BMenuItem : System.Windows.Forms.ToolStripMenuItem, MenuElement {
        public BMenuItem() {
            DropDownOpening += __handleDropDownOpening;
        }

        #region event impl Click
        // root impl of Click event is in BCheckBox
        public BEvent<BEventHandler> clickEvent = new BEvent<BEventHandler>();
        protected override void OnClick( System.EventArgs e ) {
            base.OnClick( e );
            clickEvent.raise( this, e );
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

        #region event impl MouseEnter
        // root impl of MouseEnter event is in BButton
        public BEvent<BEventHandler> mouseEnterEvent = new BEvent<BEventHandler>();
        protected override void OnMouseEnter( System.EventArgs e ) {
            base.OnMouseEnter( e );
            mouseEnterEvent.raise( this, e );
        }
        #endregion

        // root impl of DropDownOpening event is in BMenuItem
        #region event impl DropDownOpening
        // root impl of DropDownOpening event is in BMenuItem
        public BEvent<BEventHandler> dropDownOpeningEvent = new BEvent<BEventHandler>();
        // warning: to use this event, register event handler in constructor
        private void __handleDropDownOpening( object sender, EventArgs e ) {
            dropDownOpeningEvent.raise( this, e );
        }
        #endregion

        #region event impl CheckedChanged
        // root impl of CheckedChanged event is in BCheckBox
        public BEvent<BEventHandler> checkedChangedEvent = new BEvent<BEventHandler>();
        protected override void OnCheckedChanged( System.EventArgs e ) {
            base.OnCheckedChanged( e );
            checkedChangedEvent.raise( this, e );
        }
        #endregion

        // root implementation of javax.swing.AbstractButton
        #region javax.swing.AbstractButton
        // root implementation of javax.swing.AbstractButton is in BMenuItem.cs
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
            base.Checked = value;
        }
#endif
        public bocoree.java.awt.Icon getIcon() {
            bocoree.java.awt.Icon ret = new bocoree.java.awt.Icon();
            ret.image = base.Image;
            return ret;
        }

        public void setIcon( bocoree.java.awt.Icon value ) {
            if ( value == null ) {
                base.Image = null;
            } else {
                base.Image = value.image;
            }
        }
        #endregion

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
            return new bocoree.java.awt.Dimension( w, h );
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
            return new bocoree.java.awt.Dimension( w, h );
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
        public void setToolTipText( string value ) {
            base.ToolTipText = value;
        }

        public String getToolTipText() {
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
            System.Drawing.Point p = base.PointToScreen( new System.Drawing.Point( 0, 0 ) );
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

        #region javax.swing.MenuElement
        public MenuElement[] getSubElements() {
            List<MenuElement> list = new List<MenuElement>();
            foreach ( ToolStripItem item in base.DropDownItems ) {
                if ( item is MenuElement ) {
                    list.Add( (MenuElement)item );
                }
            }
            return list.ToArray();
        }
        #endregion

        public bool isCheckOnClick() {
            return base.CheckOnClick;
        }

        public void setCheckOnClick( bool value ) {
            base.CheckOnClick = value;
        }

        public KeyStroke getAccelerator() {
            KeyStroke ret = KeyStroke.getKeyStroke( 0, 0 );
            ret.keys = base.ShortcutKeys;
            return ret;
        }

        public void setAccelerator( KeyStroke stroke ) {
            try {
                base.ShortcutKeys = stroke.keys;
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "BMenuItem#setAccelerator; ex=" + ex );
            }
        }

        public void add( ToolStripItem item ) {
            base.DropDownItems.Add( item );
        }

        public void addSeparator() {
            base.DropDownItems.Add( new ToolStripSeparator() );
        }

        public void removeAll() {
            base.DropDownItems.Clear();
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
