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

namespace bocoree.windows.forms {

    public class BListView : System.Windows.Forms.ListView {
        public void setItemAt( int index, BListViewItem item ) {
            base.Items[index] = item;
        }

        public void removeElementAt( int index ) {
            base.Items.RemoveAt( index );
        }

        public BListViewItem getItemAt( int index ) {
            return (BListViewItem)base.Items[index];
        }

        public int getItemCount() {
            return base.Items.Count;
        }

        public void addItem( BListViewItem item ) {
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
    }

}
#endif
