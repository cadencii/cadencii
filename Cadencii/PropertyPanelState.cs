/*
 * PropertyPanelState.cs
 * Copyright (C) 2009-2010 kbinani
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
package org.kbinani.cadencii;

import java.util.*;
import java.io.*;
import org.kbinani.*;
import org.kbinani.windows.forms.*;
import org.kbinani.xml.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.java.io;
using org.kbinani.windows.forms;
using org.kbinani.xml;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    public class PropertyPanelState {
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
