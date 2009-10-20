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
package com.boare.cadencii;

import java.awt.*;

/// <summary>
/// toolStrip*の位置を保存します
/// </summary>
public class ToolStripLocation {
    public enum ParentPanel {
        Top,
        Bottom,
    }
    
    public Point location;
    public ParentPanel parent;

    public ToolStripLocation() {
        location = new Point( 0, 0 );
        parent = ParentPanel.Top;
    }

    public ToolStripLocation( Point aLocation, ParentPanel aParent ) {
        location = aLocation;
        parent = aParent;
    }
}
