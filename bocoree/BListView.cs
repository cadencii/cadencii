#if JAVA
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BListView.java
#else
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
    }

}
#endif
