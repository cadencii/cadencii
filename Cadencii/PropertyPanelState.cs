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
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    public class PropertyPanelState {
        public enum PanelState {
            Hidden,
            Window,
            Docked,
        }

        public PanelState State = PanelState.Docked;
        public Rectangle Bounds = new Rectangle( 0, 0, 200, 300 );
        public Vector<ValuePair<String, boolean>> ExpandStatus = new Vector<ValuePair<String,boolean>>();
        public NoteNumberExpressionType LastUsedNoteNumberExpression = NoteNumberExpressionType.International;
        public FormWindowState WindowState = FormWindowState.Normal;
        public int DockWidth = 200;
    }

}
