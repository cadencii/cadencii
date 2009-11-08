#if JAVA
package org.kbinani.windows.forms;

import javax.swing.*;

public class BVScrollBar extends JScrollBar {
    public BVScrollBar(){
        super();
        setOrientation( JScrollBar.VERTICAL );
    }
}
#else
namespace bocoree.windows.forms {

    public class BVScrollBar : System.Windows.Forms.VScrollBar {
        // root implementation of javax.swing.ScrollBar
        #region javax.swing.ScrollBar
        public void setMaximum( int value ) {
            base.Maximum = value;
        }

        public int getMaximum() {
            return base.Maximum;
        }

        public void setMinimum( int value ) {
            base.Minimum = value;
        }

        public int getMinimum() {
            return base.Minimum;
        }

        public int getValue() {
            return base.Value;
        }

        public void setValue( int value ) {
            base.Value = value;
        }

        public int getVisibleAmount() {
            return base.LargeChange;
        }

        public void setVisibleAmount( int value ) {
            base.LargeChange = value;
        }
        #endregion
    }

}
#endif
