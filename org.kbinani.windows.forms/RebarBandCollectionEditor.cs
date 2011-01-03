#if !MONO
/*
 * RebarBandCollectionEditor.cs
 * Copyright © Anthony Baraff
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of org.kbinani.windows.forms.
 *
 * org.kbinani.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace org.kbinani.windows.forms {
    /// <summary>
    /// Summary description for BandCollectionEditor.
    /// </summary>
    public class BandCollectionEditor : System.ComponentModel.Design.CollectionEditor {
        public BandCollectionEditor( System.Type type )
            : base( type ) {
        }

        protected override object SetItems( object editValue, object[] value ) {
            foreach ( RebarBand band in value ) {
                if ( !band.Created )
                    ((RebarBandCollection)editValue).Add( band );
            }
            return editValue;
        }

    }
}
#endif
