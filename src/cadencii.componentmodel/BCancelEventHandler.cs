/*
 * BCancelEventHandler.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.componentmodel.
 *
 * cadencii.componentmodel is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.componentmodel is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii.componentmodel;

import cadencii.BEventHandler;

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
