#if !JAVA
/*
 * IPaletteTool.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using cadencii.vsq;

namespace cadencii {

    public interface IPaletteTool {
        bool edit( VsqTrack track, int[] event_internal_ids, MouseButtons button );
        String getName( String language );
        String getDescription( String language );
        bool hasDialog();
        DialogResult openDialog();
        Bitmap getIcon();
        void applyLanguage( String language );
    }

}
#endif
