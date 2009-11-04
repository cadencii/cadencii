#if JAVA
package org.kbinani.windows.forms;

import javax.swing.*;

public class BSlider extends JSlider{
}
#else
namespace bocoree.windows.forms {

    public class BSlider : System.Windows.Forms.TrackBar {
    }

}
#endif
