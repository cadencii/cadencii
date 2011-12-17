/*
 * BMenuItem.cs
 * Copyright © 2009-2011 kbinani
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
//INCLUDE ../BuildJavaUI/src/org/kbinani/windows/forms/BMenuItem.java
#else
#define ABSTRACT_BUTTON_ENABLE_IS_SELECTED
#define COMPONENT_PARENT_AS_OWNERITEM
#define COMPONENT_ENABLE_TOOL_TIP_TEXT
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using com.github.cadencii.javax.swing;

namespace com.github.cadencii.windows.forms {
    public class BMenuItem : System.Windows.Forms.ToolStripMenuItem, MenuElement {
        // root implementation of javax.swing.AbstractButton
        #region javax.swing.AbstractButton
        // root implementation of javax.swing.AbstractButton is in BMenuItem.cs
        public static int getMnemonicFromText( string text ) {
            if ( text.Length < 2 ) {
                return 0;
            }

            char lastc = text[0];
            int detected = 0;
            for ( int i = 1; i < text.Length; i++ ) {
                char c = text[i];
                if ( lastc == '&' && c != '&' ) {
                    c = Char.ToUpper( c );
                    int code = (int)c;
                    if ( 48 <= code && code <= 57 ) {
                        detected = code;
                    } else if ( 65 <= code && code <= 90 ) {
                        detected = code;
                    }
                }
                lastc = c;
            }
            return detected;
        }

        public static string setMnemonicFromText( string text, int value ) {
            if ( value == 0 ) {
                return text;
            }
            if ( (value < 48 || 57 < value) && (value < 65 || 90 < value) ) {
                return text;
            }

            if ( text.Length >= 2 ) {
                char lastc = text[0];
                int index = -1; // 第index文字目が、ニーモニック
                for ( int i = 1; i < text.Length; i++ ) {
                    char c = text[i];
                    if ( lastc == '&' && c != '&' ) {
                        index = i;
                    }
                    lastc = c;
                }

                if ( index >= 0 ) {
                    string newtext = text.Substring( 0, index ) + new string( (char)value, 1 ) + ((index + 1 < text.Length) ? text.Substring( index + 1 ) : "");
                    return newtext;
                }
            }
            text = text + "(&" + new string( (char)value, 1 ) + ")";
            return text;
        }

        public int getMnemonic() {
            return getMnemonicFromText( getText() );
        }

        public void setMnemonic( int value ) {
            setText( setMnemonicFromText( base.Text, value ) );
        }

        int m_mnemonic_index = -1;
        public void setDisplayedMnemonicIndex( int value ) {
            string text = getText();
            if ( 0 <= value && value < text.Length ) {
                m_mnemonic_index = value;
                setText( text );
            } else {
                m_mnemonic_index = -1;
            }
        }

        public int getDisplayedMnemonicIndex() {
            return m_mnemonic_index;
        }

        public string getText() {
            return base.Text.Replace( "&", "" );
        }

        public void setText( string text ) {
            if ( 0 <= m_mnemonic_index && m_mnemonic_index < text.Length ) {
                text = text.Substring( 0, m_mnemonic_index ) + "&" + (m_mnemonic_index + 1 < text.Length ? text.Substring( m_mnemonic_index ) : "");
            }
            base.Text = text;
        }

#if ABSTRACT_BUTTON_ENABLE_IS_SELECTED
        public bool isSelected() {
            return base.Checked;
        }

        public void setSelected( bool value ) {
            base.Checked = value;
        }
#endif

        public com.github.cadencii.java.awt.Icon getIcon() {
            com.github.cadencii.java.awt.Icon ret = new com.github.cadencii.java.awt.Icon();
            ret.image = base.Image;
            return ret;
        }

        public void setIcon( com.github.cadencii.java.awt.Icon value ) {
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
            return new com.github.cadencii.java.awt.Dimension( w, h );
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
            return new com.github.cadencii.java.awt.Dimension( w, h );
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

        public com.github.cadencii.java.awt.Rectangle getBounds() {
            System.Drawing.Rectangle r = base.Bounds;
            return new com.github.cadencii.java.awt.Rectangle( r.X, r.Y, r.Width, r.Height );
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

        public com.github.cadencii.java.awt.Dimension getSize() {
            return new com.github.cadencii.java.awt.Dimension( base.Size.Width, base.Size.Height );
        }

        public void setSize( int width, int height ) {
            base.Size = new System.Drawing.Size( width, height );
        }

        public void setSize( com.github.cadencii.java.awt.Dimension d ) {
            setSize( d.width, d.height );
        }

        public void setBackground( com.github.cadencii.java.awt.Color color ) {
            base.BackColor = System.Drawing.Color.FromArgb( color.getRed(), color.getGreen(), color.getBlue() );
        }

        public com.github.cadencii.java.awt.Color getBackground() {
            return new com.github.cadencii.java.awt.Color( base.BackColor.R, base.BackColor.G, base.BackColor.B );
        }

        public void setForeground( com.github.cadencii.java.awt.Color color ) {
            base.ForeColor = color.color;
        }

        public com.github.cadencii.java.awt.Color getForeground() {
            return new com.github.cadencii.java.awt.Color( base.ForeColor.R, base.ForeColor.G, base.ForeColor.B );
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

        public void setPreferredSize( com.github.cadencii.java.awt.Dimension size ) {
            base.Size = new System.Drawing.Size( size.width, size.height );
        }

        public com.github.cadencii.java.awt.Font getFont() {
            return new com.github.cadencii.java.awt.Font( base.Font );
        }

        public void setFont( com.github.cadencii.java.awt.Font font ) {
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
            base.ShortcutKeys = stroke.keys;
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
