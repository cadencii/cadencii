/*
 * StfQueueArgs.cs
 * Copyright © 2009-2010 kbinani
 *
 * This file is part of org.kbinani.editotoini.
 *
 * org.kbinani.editotoini is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.editotoini is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.editotoini;
#else
using System;

namespace org.kbinani.editotoini {
#endif

    public struct StfQueueArgs {
        public String waveName;
        public String offset;
        public String blank;
    }

#if !JAVA
}
#endif
