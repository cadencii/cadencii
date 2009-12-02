/*
 * RenderRequiredEventHandler.cs
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

public class RenderRequiredEventHandler extends BEventHandler{
    public RenderRequiredEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, int[].class );
    }
    
    public RenderRequiredEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, int[].class );
    }
}
#else
using System;

namespace Boare.Cadencii {

    public delegate void RenderRequiredEventHandler( Object sender, int[] tracks );

}
#endif