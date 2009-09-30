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
using System;

namespace Boare.Cadencii {

    /// <summary>
    /// 原音設定の引数．
    /// </summary>
    public struct OtoArgs {
        public String fileName;
        public String Alias;
        public int msOffset;
        public int msConsonant;
        public int msBlank;
        public int msPreUtterance;
        public int msOverlap;
    }

}
