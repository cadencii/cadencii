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
using System.Windows.Forms;

namespace bocoree.windows.forms {

    public class BListView : System.Windows.Forms.ListView {
        public BListView() {
            ListViewGroup def = new ListViewGroup();
            def.Name = "";
            base.Groups.Add( def );
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

        public void removeElementAt( string group, int index ) {
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
                    return group;
                }
            }
            return null;
        }

        public Boolean isItemSelectedAt( string group, int index ) {
            ListViewGroup g = getGroupFromName( group );
            return g.Items[index].Checked;
        }

        public void addItem( string group, BListViewItem item ) {
            if ( group != "" ) {
                System.Windows.Forms.ListViewGroup g = new System.Windows.Forms.ListViewGroup();
                g.Name = group;
                base.Groups.Add( g );
                item.Group = g;
            }
            base.Items.Add( item );
        }

        public bool isMultiSelect() {
            return base.MultiSelect;
        }

        public void setMultiSelect( bool value ) {
            base.MultiSelect = value;
        }

        public void setColumnHeaders( string[] headers ) {
            int len = Math.Min( headers.Length, base.Columns.Count );
            for ( int i = 0; i < len; i++ ) {
                base.Columns[i].Text = headers[i];
            }
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
