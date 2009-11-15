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
    }

}
#endif
