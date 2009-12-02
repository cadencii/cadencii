/*
 * StateChangeRequiredEventHandler.cs
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
#if JAVA
package org.kbinani.Cadencii;

import org.kbinani.BEventHandler;

public class StateChangeRequiredEventHandler extends BEventHandler{
    public StateChangeRequiredEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, PanelState.class );
    }
    
    public StateChangeRequiredEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, PanelState.class );
    }
}
#else
using System;

namespace Boare.Cadencii {

    public delegate void StateChangeRequiredEventHandler( Object sender, PanelState state );

}
#endif
