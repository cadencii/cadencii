/*
 * OtoArgs.cs
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
package org.kbinani.cadencii;
#else
using System;

namespace Boare.Cadencii {
#endif

    /// <summary>
    /// 原音設定の引数．
    /// </summary>
    public struct OtoArgs {
        public String fileName;
        public String Alias;
        public float msOffset;
        public float msConsonant;
        public float msBlank;
        public float msPreUtterance;
        public float msOverlap;
    }

#if !JAVA
}
#endif
