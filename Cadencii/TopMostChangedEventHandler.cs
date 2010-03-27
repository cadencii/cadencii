/*
 * TopMostChangedEventHandler.cs
 * Copyright (C) 2009-2010 kbinani
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
#if JAVA
package org.kbinani.cadencii;

import org.kbinani.BEventHandler;

public class TopMostChangedEventHandler extends BEventHandler{
    public TopMostChangedEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, Boolean.TYPE );
    }
    
    public TopMostChangedEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, Boolean.TYPE );
    }
}
#else
using System;

namespace org.kbinani.cadencii {

    public class TopMostChangedEventHandler : BEventHandler {
        public TopMostChangedEventHandler( Object sender, String method_name )
            : base( sender, method_name, typeof( void ), typeof( object ), typeof( bool ) ) {
        }

        public TopMostChangedEventHandler( Type sender, String method_name )
            : base( sender, method_name, typeof( void ), typeof( object ), typeof( bool ) ) {
        }
    }

}
#endif
