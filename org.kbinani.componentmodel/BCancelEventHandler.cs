/*
 * BCancelEventHandler.cs
 * Copyright © 2009-2010 kbinani
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
package org.kbinani.componentmodel;

import org.kbinani.BEventHandler;

    public class BCancelEventHandler extends BEventHandler{
        public BCancelEventHandler( Object sender, String method_name )
        {
            base( sender, method_name, typeof( void ), typeof( Object ), typeof( CancelEventArgs ) )
            ;
        }

        public BCancelEventHandler( Type sender, String method_name )
        {
            base( sender, method_name, typeof( void ), typeof( Object ), typeof( CancelEventArgs ) )
            ;
        }
    }

#endif
