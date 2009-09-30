/*
 * ToolStripLocation.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System.Drawing;

namespace Boare.Cadencii {

    /// <summary>
    /// toolStrip*の位置を保存します
    /// </summary>
    public class ToolStripLocation {
        public enum ParentPanel {
            Top,
            Bottom,
        }
        
        public Point Location;
        public ParentPanel Parent;

        public ToolStripLocation() {
            Location = new Point( 0, 0 );
            Parent = ParentPanel.Top;
        }

        public ToolStripLocation( Point location, ParentPanel parent ) {
            Location = location;
            Parent = parent;
        }
    }

}
