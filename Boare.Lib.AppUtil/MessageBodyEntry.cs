/*
 * MessageBody.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Lib.AppUtil.
 *
 * Boare.Lib.AppUtil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.AppUtil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.apputil;

import java.util.*;
#else
using System;
using bocoree.java.util;

namespace Boare.Lib.AppUtil {
#endif

    public class MessageBodyEntry {
        public String message;
        public Vector<String> location = new Vector<String>();

        public MessageBodyEntry( String message_, String[] location_ ) {
            message = message_;
            for ( int i = 0; i < location_.Length; i++ ) {
                location.add( location_[i] );
            }
        }
    }

#if !JAVA
}
#endif
