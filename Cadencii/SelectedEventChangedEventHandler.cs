/*
 * SelectedEventChangedEventHandler.cs
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

public class SelectedEventChangedEventHandler extends BEventHandler{
    public SelectedEventChangedEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, Boolean.TYPE );
    }
    
    public SelectedEventChangedEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, Boolean.TYPE );
    }
}
#else
namespace Boare.Cadencii {

    public delegate void SelectedEventChangedEventHandler( object sender, bool selected_is_null );

}
#endif
