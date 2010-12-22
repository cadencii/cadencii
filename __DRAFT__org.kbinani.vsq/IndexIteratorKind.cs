/*
 * IndexItertorKind.cs
 * Copyright (C) 2010 kbinani
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

#else
namespace org.kbinani.vsq {
#endif

    public class IndexIteratorKind {
        public const int SINGER = 1 << 0;
        public const int NOTE = 1 << 1;
        public const int CRESCEND = 1 << 2;
        public const int DECRESCEND = 1 << 3;
        public const int DYNAFF = 1 << 4;

        private IndexIteratorKind() {
        }
    }

#if !JAVA
}
#endif
