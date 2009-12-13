namespace bocoree.windows.forms {

    public class BToolStripTextBox : System.Windows.Forms.ToolStripTextBox {
        #region event impl KeyDown
        // root impl of KeyDown event is in BTextBox
        public BEvent<BKeyEventHandler> keyDownEvent = new BEvent<BKeyEventHandler>();
        protected override void OnKeyDown( System.Windows.Forms.KeyEventArgs e ) {
            base.OnKeyDown( e );
            keyDownEvent.raise( this, e );
        }
        #endregion

        public void setText( string value ) {
            base.Text = value;
        }

        public string getText() {
            return base.Text;
        }
    }

}