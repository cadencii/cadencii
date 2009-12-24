/*
 * RenderRequiredEventHandler.cs
 * Copyright (C) 2009 kbinani
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

namespace org.kbinani.cadencii {

    public delegate void RenderRequiredEventHandler( Object sender, int[] tracks );

}
#endif