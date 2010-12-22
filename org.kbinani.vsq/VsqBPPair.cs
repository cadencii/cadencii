/*
 * VsqBPPair.cs
 * Copyright © 2009-2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.io.*;
#else
using System;

namespace org.kbinani.vsq {
#endif

#if JAVA
    public class VsqBPPair implements Cloneable, Serializable {
#else
    [Serializable]
    public struct VsqBPPair {
#endif
        public int value;
        public long id;

        public VsqBPPair( int value_, long id_ ) {
            value = value_;
            id = id_;
        }

#if JAVA
        public Object clone(){
            return new VsqBPPair( value, id );
        }
#endif
    }

#if !JAVA
}
#endif
