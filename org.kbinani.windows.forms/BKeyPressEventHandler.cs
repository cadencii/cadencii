/*
 * BKeyPressEventHandler.cs
 * Copyright © 2009-2011 kbinani
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

import com.github.cadencii.BEventHandler;

    public class BKeyPressEventHandler extends BEventHandler{
        public BKeyPressEventHandler( Object sender, String method_name )
        {
            base( sender, method_name, typeof( void ), typeof( Object ), typeof( KeyPressEventArgs ) )
            ;
        }

        public BKeyPressEventHandler( Type sender, String method_name )
        {
            base( sender, method_name, typeof( void ), typeof( Object ), typeof( KeyPressEventArgs ) )
            ;
        }
    }

#endif

