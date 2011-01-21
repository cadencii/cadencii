/*
 * BListView.cs
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
//INCLUDE ../BuildJavaUI/src/org/kbinani/windows/forms/BListView.java
#else
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using org.kbinani.java.awt;

namespace org.kbinani.windows.forms.obsolete {

    public class BListView : System.Windows.Forms.ListView {
        private List<ColumnHeader> m_headers = new List<ColumnHeader>();

        public BListView() {
            ListViewGroup def = new ListViewGroup();
            def.Name = "";
            base.Groups.Add( def );
        }

        public void ensureRowVisible( String group, int row ) {
            ListViewGroup g = getGroupFromName( group );
            g.Items[row].EnsureVisible();
        }

        public void setGroupHeader( String group, String header ) {
            ListViewGroup g = getGroupFromName( group );
            g.Header = header;
        }

        public void setItemBackColorAt( String group, int index, Color color ) {
            ListViewGroup g = getGroupFromName( group );
            g.Items[index].BackColor = color.color;
        }

        public void clear() {
            List<ColumnHeader> cache = new List<ColumnHeader>();
            foreach ( ColumnHeader hdr in base.Columns ) {
                cache.Add( hdr );
            }
            base.Clear();
            foreach ( ColumnHeader hdr in cache ) {
                base.Columns.Add( hdr );
            }
        }

        public string getGroupNameAt( int index ) {
            return base.Groups[index].Name;
        }

        public int getGroupCount() {
            return base.Groups.Count;
        }

        public void setItemAt( string group, int index, BListViewItem item ) {
            ListViewGroup g = getGroupFromName( group );
            g.Items[index] = item;
        }

        public void removeItemAt( string group, int index ) {
            ListViewGroup g = getGroupFromName( group );
            g.Items.RemoveAt( index );
        }

        public BListViewItem getItemAt( string group, int index ) {
            ListViewGroup g = getGroupFromName( group );
            return (BListViewItem)g.Items[index];
        }

        public int getItemCount( String group ) {
            ListViewGroup g = getGroupFromName( group );
            return g.Items.Count;
        }

        private ListViewGroup getGroupFromName( string name ) {
            if ( name == null ) {
                name = "";
            }
            foreach ( ListViewGroup group in base.Groups ) {
                if ( name == group.Name ) {
                    if ( name == "" ) {
                        group.Header = "Default";
                    }
                    return group;
                }
            }
            System.Windows.Forms.ListViewGroup g = new System.Windows.Forms.ListViewGroup();
            g.Name = name;
            if ( name == "" ) {
                g.Header = "Default";
            } else {
                g.Header = name;
            }
            base.Groups.Add( g );
            return g;
        }

        public Boolean isItemCheckedAt( string group, int index ) {
            ListViewGroup g = getGroupFromName( group );
            return g.Items[index].Checked;
        }

        public void setItemCheckedAt( string group, int index, bool value ) {
            ListViewGroup g = getGroupFromName( group );
            g.Items[index].Checked = value;
        }

        public int getSelectedIndex( string group ) {
            ListViewGroup g = getGroupFromName( group );
            int count = g.Items.Count;
            for ( int i = 0; i < count; i++ ) {
                if ( g.Items[i].Selected ) {
                    return i;
                }
            }
            return -1;
        }

        public void clearSelection( string group ) {
            ListViewGroup g = getGroupFromName( group );
            foreach ( ListViewItem item in g.Items ) {
                item.Selected = false;
            }
        }

        public void setItemSelectedAt( string group, int index, bool value ) {
            ListViewGroup g = getGroupFromName( group );
            g.Items[index].Selected = value;
        }

        public void addItem( string group, BListViewItem item, bool value ) {
            int dif = item.getSubItemCount() - base.Columns.Count;
            for ( int i = 0; i < dif; i++ ) {
                ColumnHeader hdr = new ColumnHeader();
                m_headers.Add( hdr );
                base.Columns.Add( hdr );
            }
            ListViewGroup g = getGroupFromName( group );
            item.Checked = value;
            item.Group = g;
            base.Items.Add( item );
        }

        public void addItem( string group, BListViewItem item ) {
            addItem( group, item, false );
        }

        public bool isMultiSelect() {
            return base.MultiSelect;
        }

        public void setMultiSelect( bool value ) {
            base.MultiSelect = value;
        }

        public void setColumnHeaders( string[] headers ) {
            int dif = headers.Length - base.Columns.Count;
            for ( int i = 0; i < dif; i++ ) {
                ColumnHeader hdr = new ColumnHeader();
                m_headers.Add( hdr );
                base.Columns.Add( hdr );
            }
            for ( int i = 0; i < headers.Length; i++ ) {
                base.Columns[i].Text = headers[i];
                base.Columns[i].Name = headers[i];
            }
        }

        public void setColumnWidth( int index, int width ) {
            base.Columns[index].Width = width;
        }

        public int getColumnWidth( int index ) {
            return base.Columns[index].Width;
        }

        public string[] getColumnHeaders() {
            int len = base.Columns.Count;
            string[] ret = new string[len];
            for ( int i = 0; i < len; i++ ) {
                ret[i] = base.Columns[i].Text;
            }
            return ret;
        }

        public bool isCheckBoxes() {
            return base.CheckBoxes;
        }

        public void setCheckBoxes( bool value ) {
            base.CheckBoxes = value;
        }

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
    }

}

namespace org.kbinani.windows.forms
{
    public class BListView : System.Windows.Forms.ListView
    {
        public void ensureRowVisible( int row )
        {
            this.EnsureVisible( row );
        }

        public void setRowBackColor( int row, Color color )
        {
            this.Items[row].BackColor = color.color;
        }

        public void clear()
        {
            this.Items.Clear();
        }

        public void setItemAt( int row, int column, string item )
        {
            this.Items[row].SubItems[column].Text = item;
        }

        public void removeRow( int row )
        {
            this.Items.RemoveAt( row );
        }

        public String getItemAt( int row, int column )
        {
            return this.Items[row].SubItems[column].Text;
        }

        public int getItemCountRow()
        {
            return this.Items.Count;
        }

        public Boolean isRowChecked( int row )
        {
            return this.Items[row].Checked;
        }

        public void setRowChecked( int row, bool value )
        {
            this.Items[row].Checked = value;
        }

        public int getSelectedRow()
        {
            if ( this.SelectedIndices.Count <= 0 ){
                return -1;
            }else{
                return this.SelectedIndices[0];
            }
        }

        public void clearSelection()
        {
            this.SelectedIndices.Clear();
        }

        public void setSelectedRow( int row )
        {
            if ( !this.Items[row].Selected ) {
                this.SelectedIndices.Clear();
                this.Items[row].Selected = true;
            }
        }

        public void addItem( string[] items, bool selected )
        {
            ListViewItem item = new ListViewItem( items );
            item.Checked = selected;
            if ( this.Columns.Count < items.Length ) {
                for ( int i = this.Columns.Count; i < items.Length; i++ ) {
                    this.Columns.Add( "" );
                }
            }
            this.Items.Add( item );
        }

        public void addItem( string[] item )
        {
            addItem( item, false );
        }

        public bool isMultiSelect()
        {
            return base.MultiSelect;
        }

        public void setMultiSelect( bool value )
        {
            base.MultiSelect = value;
        }

        public void setColumnHeaders( string[] headers )
        {
            if ( this.Columns.Count < headers.Length ) {
                for ( int i = this.Columns.Count; i < headers.Length; i++ ) {
                    this.Columns.Add( "" );
                }
            }
            for ( int i = 0; i < headers.Length; i++ ) {
                this.Columns[i].Text = headers[i];
            }
        }

        public void setColumnWidth( int index, int width )
        {
            base.Columns[index].Width = width;
        }

        public int getColumnWidth( int index )
        {
            return base.Columns[index].Width;
        }

        public string[] getColumnHeaders()
        {
            int len = base.Columns.Count;
            string[] ret = new string[len];
            for ( int i = 0; i < len; i++ ) {
                ret[i] = base.Columns[i].Text;
            }
            return ret;
        }

        public bool isCheckBoxes()
        {
            return base.CheckBoxes;
        }

        public void setCheckBoxes( bool value )
        {
            base.CheckBoxes = value;
        }

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
            return new org.kbinani.java.awt.Dimension( w, h );
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
            return new org.kbinani.java.awt.Dimension( w, h );
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

        public bool isVisible()
        {
            return base.Visible;
        }

        public void setVisible( bool value )
        {
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

        public org.kbinani.java.awt.Rectangle getBounds()
        {
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

        public int getWidth()
        {
            return base.Width;
        }

        public int getHeight()
        {
            return base.Height;
        }

        public org.kbinani.java.awt.Dimension getSize()
        {
            return new org.kbinani.java.awt.Dimension( base.Size.Width, base.Size.Height );
        }

        public void setSize( int width, int height )
        {
            base.Size = new System.Drawing.Size( width, height );
        }

        public void setSize( org.kbinani.java.awt.Dimension d )
        {
            setSize( d.width, d.height );
        }

        public void setBackground( org.kbinani.java.awt.Color color )
        {
            base.BackColor = System.Drawing.Color.FromArgb( color.getRed(), color.getGreen(), color.getBlue() );
        }

        public org.kbinani.java.awt.Color getBackground()
        {
            return new org.kbinani.java.awt.Color( base.BackColor.R, base.BackColor.G, base.BackColor.B );
        }

        public void setForeground( org.kbinani.java.awt.Color color )
        {
            base.ForeColor = color.color;
        }

        public org.kbinani.java.awt.Color getForeground()
        {
            return new org.kbinani.java.awt.Color( base.ForeColor.R, base.ForeColor.G, base.ForeColor.B );
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
        public void requestFocus() {
            base.Focus();
        }

        public bool isFocusOwner() {
            return base.Focused;
        }
#endif

        public void setPreferredSize( org.kbinani.java.awt.Dimension size )
        {
            base.Size = new System.Drawing.Size( size.width, size.height );
        }

        public org.kbinani.java.awt.Font getFont()
        {
            return new org.kbinani.java.awt.Font( base.Font );
        }

        public void setFont( org.kbinani.java.awt.Font font )
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
    }
}
#endif
