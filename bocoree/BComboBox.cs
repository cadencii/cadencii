#if JAVA
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BComboBox.java
#else
using System;
using System.Windows.Forms;

namespace bocoree.windows.forms{

    public class BComboBox : ComboBox {
        public void addItem( object item ) {
            base.Items.Add( item );
        }

        public object getItemAt( int index ) {
            return base.Items[index];
        }

        public int getItemCount() {
            return base.Items.Count;
        }

        public int getSelectedIndex() {
            return base.SelectedIndex;
        }

        public object getSelectedItem() {
            return base.SelectedItem;
        }

        public void insertItemAt( object item, int index ) {
            base.Items.Insert( index, item );
        }

        public void removeAllItems() {
            base.Items.Clear();
        }

        public void removeItem( object item ) {
            base.Items.Remove( item );
        }

        public void removeItemAt( int index ) {
            base.Items.RemoveAt( index );
        }

        public void setSelectedItem( object item ) {
            base.SelectedItem = item;
        }

        public void setSelectedIndex( int index ) {
            base.SelectedIndex = index;
        }

        #region java.awt.Component
        // root implementation of java.awt.Component is in BForm.cs
        public void setBounds( int x, int y, int width, int height ) {
            base.Bounds = new System.Drawing.Rectangle( x, y, width, height );
        }

        public void setBounds( bocoree.awt.Rectangle rc ) {
            base.Bounds = new System.Drawing.Rectangle( rc.x, rc.y, rc.width, rc.height );
        }

        public bocoree.awt.Cursor getCursor() {
            System.Windows.Forms.Cursor c = base.Cursor;
            bocoree.awt.Cursor ret = null;
            if ( c.Equals( System.Windows.Forms.Cursors.Arrow ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.DEFAULT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Cross ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.CROSSHAIR_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Default ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.DEFAULT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Hand ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.HAND_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.IBeam ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.TEXT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanEast ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.E_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNE ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.NE_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNorth ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.N_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNW ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.NW_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSE ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.SE_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSouth ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.S_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSW ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.SW_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanWest ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.W_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.SizeAll ) ) {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.MOVE_CURSOR );
            } else {
                ret = new bocoree.awt.Cursor( bocoree.awt.Cursor.CUSTOM_CURSOR );
            }
            ret.cursor = c;
            return ret;
        }

        public void setCursor( bocoree.awt.Cursor value ) {
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
        public bocoree.awt.Point getLocationOnScreen() {
            System.Drawing.Point p = base.PointToScreen( base.Location );
            return new bocoree.awt.Point( p.X, p.Y );
        }

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

        public void setPreferredSize( bocoree.awt.Dimension size ) {
            base.Size = new System.Drawing.Size( size.width, size.height );
        }

        public bocoree.awt.Font getFont() {
            return new bocoree.awt.Font( base.Font );
        }

        public void setFont( bocoree.awt.Font font ) {
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
