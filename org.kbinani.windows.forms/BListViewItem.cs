/*
 * BFontChooser.cs
 * Copyright (C) 2009-2010 kbinani
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
//INCLUDE ../BuildJavaUI/src/org/kbinani/windows/forms/BListViewItem.java
#else
namespace org.kbinani.windows.forms {

    public class BListViewItem : System.Windows.Forms.ListViewItem {
        private string group = "";

        public BListViewItem( string[] values )
            : base( values ) {
        }

        public BListViewItem()
            : base() {
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

        public string getName() {
            return base.Name;
        }

        public void setName( string value ) {
            base.Name = value;
        }
    }

}
#endif
