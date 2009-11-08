#if JAVA
package org.kbinani.windows.forms;

import javax.swing.*;

public class BHScrollBar extends JScrollBar {
    public BHScrollBar(){
        super();
        setOrientation( JScrollBar.HORIZONTAL );
    }
}
#else
namespace bocoree.windows.forms{

    public class BHScrollBar : System.Windows.Forms.HScrollBar{
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
