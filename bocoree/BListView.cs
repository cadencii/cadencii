/*
 * BListView.cs
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
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BListView.java
#else
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using bocoree.awt;

namespace bocoree.windows.forms {

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
            foreach ( ListViewGroup group in base.Groups ) {
                if ( name == group.Name ) {
                    if ( name == "" ) {
                        group.Header = "Default";
                    }
                    return group;
                }
            }
#if DEBUG
            Console.WriteLine( "BListView#getGroupFromName; creating new group" );
#endif
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
#if DEBUG
            //Console.WriteLine( "BListView#addItem; columnWidths;" + base.Columns[0].Width + "," + base.Columns[1].Width );
#endif
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
#if DEBUG
            Console.WriteLine( "BListView#setColumnHeaders; before; base.Columns.Count=" + base.Columns.Count );
#endif
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
#if DEBUG
            Console.WriteLine( "BListView#setColumnHeaders; after; base.Columns.Count=" + base.Columns.Count );
#endif
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
    }

}
#endif
