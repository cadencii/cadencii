/*
 * ToolStripLocation.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii;

import java.awt.*;
import cadencii.*;
import cadencii.xml.*;
#else
using cadencii;
using cadencii.java.awt;
using cadencii.xml;

namespace cadencii {
#endif

    /// <summary>
    /// toolStrip*の位置を保存します
    /// </summary>
    public class ToolStripLocation {
        public enum ParentPanel {
            Top,
            Bottom,
        }

        public XmlPoint Location;
        public ParentPanel Parent;

        public ToolStripLocation() {
            Location = new XmlPoint( 0, 0 );
            Parent = ParentPanel.Top;
        }

        public ToolStripLocation( Point location, ParentPanel parent ) {
            Location = new XmlPoint( location );
            Parent = parent;
        }
    }

#if !JAVA
}
#endif
