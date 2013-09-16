/*
 * MuteChangedEventHandler.cs
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

public class MuteChangedEventHandler extends BEventHandler{
    public MuteChangedEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Integer.TYPE, Boolean.TYPE );
    }
    
    public MuteChangedEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Integer.TYPE, Boolean.TYPE );
    }
}
#else
using System;

namespace cadencii {

    public delegate void MuteChangedEventHandler( int track, bool mute );

}
#endif
