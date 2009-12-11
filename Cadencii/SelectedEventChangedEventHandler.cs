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
#else
using System;
using bocoree;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
#endif

#if JAVA
    public class SelectedEventChangedEventHandler extends BEventHandler{
#else
    public class SelectedEventChangedEventHandler : BEventHandler {
#endif
        public SelectedEventChangedEventHandler( Object sender, String method_name )
        //...きもちわるい
#if JAVA
        {
#else
            :
#endif
            base( sender, method_name, typeof( void ), typeof( Object ), typeof( boolean ) )
#if JAVA
            ;
#else
        {
#endif
        }

        public SelectedEventChangedEventHandler( Type sender, String method_name )
#if JAVA
        {
#else
            :
#endif
            base( sender, method_name, typeof( void ), typeof( Object ), typeof( boolean ) )
#if JAVA
            ;
#else
        {
#endif
        }
    }

#if !JAVA
}
#endif
