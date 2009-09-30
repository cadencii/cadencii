/*
 * IPaletteTool.cs
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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Boare.Lib.Vsq;

namespace Boare.Cadencii {

    using boolean = Boolean;

    public interface IPaletteTool {
        boolean edit( VsqTrack track, int[] event_internal_ids, MouseButtons button );
        String getName( String language );
        String getDescription( String language );
        boolean hasDialog();
        DialogResult openDialog();
        Bitmap getIcon();
        void applyLanguage( String language );
    }

}
