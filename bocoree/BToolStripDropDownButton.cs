using System;

namespace bocoree.windows.forms {

    public class BToolStripDropDownButton : System.Windows.Forms.ToolStripDropDownButton {
        public BToolStripDropDownButton() {
            DropDownOpening += __handleDropDownOpening;
        }

        #region event impl DropDownOpening
        // root impl of DropDownOpening event is in BMenuItem
        public BEvent<BEventHandler> dropDownOpeningEvent = new BEvent<BEventHandler>();
        // warning: to use this event, register event handler in constructor
        private void __handleDropDownOpening( object sender, EventArgs e ) {
            dropDownOpeningEvent.raise( this, e );
        }
        #endregion

        public void setText( string value ) {
            base.Text = value;
        }
    }

}
