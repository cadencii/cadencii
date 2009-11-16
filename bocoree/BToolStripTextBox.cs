namespace bocoree.windows.forms {

    public class BToolStripTextBox : System.Windows.Forms.ToolStripTextBox {
        public void setText( string value ) {
            base.Text = value;
        }

        public string getText() {
            return base.Text;
        }
    }

}