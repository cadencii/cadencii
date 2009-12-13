/*
 * BCancelEventHandler.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.componentModel;

import org.kbinani.BEventHandler;
#else
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace bocoree.componentModel {
#endif

#if JAVA
    public class BCancelEventHandler extends BEventHandler{
#else
    public class BCancelEventHandler : BEventHandler {
#endif
        public BCancelEventHandler( Object sender, String method_name )
#if JAVA
        {
#else
            :
#endif
            base( sender, method_name, typeof( void ), typeof( Object ), typeof( CancelEventArgs ) )
#if JAVA
            ;
#else
        {
#endif
        }

        public BCancelEventHandler( Type sender, String method_name )
#if JAVA
        {
#else
            :
#endif
            base( sender, method_name, typeof( void ), typeof( Object ), typeof( CancelEventArgs ) )
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
