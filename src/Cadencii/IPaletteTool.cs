#if !JAVA
/*
 * IPaletteTool.cs
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using cadencii.vsq;

namespace cadencii {

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
#endif
