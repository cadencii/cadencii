/*
 * SoloChangedEventHandler.cs
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

import java.lang.reflect.*;
import org.kbinani.*;

public class SoloChangedEventHandler extends BEventHandler{
    public SoloChangedEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Integer.TYPE, Boolean.TYPE );
    }
    
    public SoloChangedEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Integer.TYPE, Boolean.TYPE );
    }
}
#else
namespace Boare.Cadencii {

    public delegate void SoloChangedEventHandler( int track, bool solo );

}
#endif
