#if JAVA

package org.kbinani.windows.forms;

import java.awt.*;
import javax.swing.*;

public class BComboBox extends JComboBox{
}
#else
using System;
using System.Windows.Forms;

namespace bocoree.windows.forms{

    public class BComboBox : ComboBox {
    }

}
#endif
