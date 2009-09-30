/*
 * FormConfigUtauVoiceConfig.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.EditOtoIni.
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
using System.Windows.Forms;

namespace Boare.EditOtoIni {

    public class FormConfigUtauVoiceConfig {
        public Rectangle Bounds = new Rectangle( 0, 0, 714, 533 );
        public FormWindowState State = FormWindowState.Normal;
        public float InnerSplitterDistancePercentage = 70.0f;
        public float OuterSplitterDistancePercentage = 60.0f;
        public int WaveViewScale;
        public int ColumnWidthFileName = 75;
        public int ColumnWidthAlias = 42;
        public int ColumnWidthOffset = 50;
        public int ColumnWidthConsonant = 72;
        public int ColumnWidthBlank = 51;
        public int ColumnWidthPreUtterance = 92;
        public int ColumnWidthOverlap = 61;
        public int ColumnWidthFrq = 51;
        public int ColumnWidthStf = 60;
    }

}
