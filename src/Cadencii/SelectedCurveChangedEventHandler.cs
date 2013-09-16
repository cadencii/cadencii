/*
 * SelectedCurveChangedEventHandler.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii;

import cadencii.BEventHandler;

public class SelectedCurveChangedEventHandler extends BEventHandler{
    public SelectedCurveChangedEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, CurveType.class );
    }
    
    public SelectedCurveChangedEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, CurveType.class );
    }
}
#else
using System;

namespace cadencii {

    public delegate void SelectedCurveChangedEventHandler( Object sender, CurveType curve_type );

}
#endif
