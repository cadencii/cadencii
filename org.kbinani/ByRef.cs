/*
 * ByRef.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package com.github.cadencii;
#else
using System;

namespace com.github.cadencii {
#endif

    public class ByRef<T> {
        public T value;

        public ByRef() {
        }

        public ByRef( T value_ ) {
            value = value_;
        }
    }

#if !JAVA
}
#endif
