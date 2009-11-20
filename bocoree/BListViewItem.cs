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
