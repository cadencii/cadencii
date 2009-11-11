/*
 * BToolBar.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.windows.forms;

import javax.swing.*;

public class BToolBar extends JToolBar
{
}
#else
namespace bocoree.windows.forms {
    public class a {
        System.Windows.Forms.ContainerControl f;
    }

    public class BToolBar : System.Windows.Forms.ToolStrip {
        public int getComponentCount() {
            return base.Items.Count;
        }

        public System.Windows.Forms.ToolStripItem getComponentAtIndex( int index ) {
            return base.Items[index];
        }
    }
}
#endif
