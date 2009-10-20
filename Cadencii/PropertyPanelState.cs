/*
 * PropertyPanelState.cs
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
#if JAVA
package org.kbinani.Cadencii;

import java.util.*;
import java.io.*;
import org.kbinani.*;
import org.kbinani.windows.forms.*;
#else
using System;
using bocoree;
using bocoree.util;
using bocoree.io;
using bocoree.windows.forms;

namespace Boare.Cadencii
{
    using boolean = System.Boolean;
#endif

    public class PropertyPanelState
    {
        public PanelState State = PanelState.Docked;
        public XmlRectangle Bounds = new XmlRectangle( 0, 0, 200, 300 );
        public Vector<ValuePair<String, Boolean>> ExpandStatus = new Vector<ValuePair<String, Boolean>>();
        public NoteNumberExpressionType LastUsedNoteNumberExpression = NoteNumberExpressionType.International;
        public BFormWindowState WindowState = BFormWindowState.Normal;
        public int DockWidth = 200;
    }

#if !JAVA
}
#endif
