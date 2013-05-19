/*
 * BListBox.cs
 * Copyright © 2011 kbinani
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
//INCLUDE ./BListBox.java
#else
using System;
using System.Windows.Forms;

namespace cadencii.windows.forms
{

    public class BListBox : ListBox
    {
        public int getItemCount()
        {
            return this.Items.Count;
        }

        public object getItemAt( int index )
        {
            return this.Items[index];
        }

        public void setItemAt( int index, object item )
        {
            this.Items[index] = item;
        }

        public void removeItemAt( int index )
        {
            this.Items.RemoveAt( index );
        }

        public void addItem( object item )
        {
            this.Items.Add( item );
        }

        public int getSelectedIndex()
        {
            return this.SelectedIndex;
        }

        public void setSelectedIndex( int index )
        {
            this.SelectedIndex = index;
        }

        public void clearSelection()
        {
            this.SelectedIndices.Clear();
        }
    }

}
#endif
