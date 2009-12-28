/*
 * BPreviewKeyDownEventHandler.cs
 * Copyright (C) 2009 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
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
    public class BPreviewKeyDownEventHandler extends BEventHandler{
#else
    public class BPreviewKeyDownEventHandler : BEventHandler {
#endif
        public BPreviewKeyDownEventHandler( Object sender, String method_name )
#if JAVA
        {
#else
            :
#endif
            base( sender, method_name, typeof( void ), typeof( Object ), typeof( PreviewKeyDownEventArgs ) )
#if JAVA
            ;
#else
        {
#endif
        }

        public BPreviewKeyDownEventHandler( Type sender, String method_name )
#if JAVA
        {
#else
            :
#endif
        base( sender, method_name, typeof( void ), typeof( Object ), typeof( PreviewKeyDownEventArgs ) )
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
