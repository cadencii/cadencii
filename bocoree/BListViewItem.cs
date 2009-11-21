/*
 * BFontChooser.cs
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
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BListViewItem.java
#else
namespace bocoree.windows.forms {

    public class BListViewItem : System.Windows.Forms.ListViewItem {
        private string group = "";

        public BListViewItem( string[] values )
            : base( values ) {
        }

        public object clone() {
            return base.Clone();
        }

        public object getTag() {
            return base.Tag;
        }

        public void setTag( object value ) {
            base.Tag = value;
        }

        public int getSubItemCount() {
            return base.SubItems.Count;
        }

        public string getSubItemAt( int index ) {
            return base.SubItems[index].Text;
        }

        public void setSubItemAt( int index, string value ) {
            base.SubItems[index].Text = value;
        }
    }

}
#endif
