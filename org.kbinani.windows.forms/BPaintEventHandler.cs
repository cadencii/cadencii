/*
 * BPaintEventHandler.cs
 * Copyright (C) 2009-2010 kbinani
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
#if JAVA
package org.kbinani.windows.forms;

import org.kbinani.BEventHandler;
#else
using System;
using System.Windows.Forms;

namespace org.kbinani.windows.forms {
#endif

#if JAVA
    public class BPaintEventHandler extends BEventHandler{
#else
    public class BPaintEventHandler : BEventHandler {
#endif
        public BPaintEventHandler( Object sender, String method_name )
#if JAVA
        {
#else
            :
#endif
 base( sender, method_name, typeof( void ), typeof( Object ), typeof( PaintEventArgs ) )
#if JAVA
            ;
#else
 {
#endif
        }

        public BPaintEventHandler( Type sender, String method_name )
#if JAVA
        {
#else
            :
#endif
 base( sender, method_name, typeof( void ), typeof( Object ), typeof( PaintEventArgs ) )
#if JAVA
            ;
#else
 {
#endif
        }
    }

#if !JAVA
}
#endif
