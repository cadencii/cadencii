/*
 * CircuitConnectionKind.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

#else
using System;

namespace org.kbinani.cadencii.draft {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 回路の接続方法を指定する
    /// </summary>
    public enum CircuitConnectionKind {
        /// <summary>
        /// 相手をレシーバとして接続する
        /// </summary>
        RECEIVER,
        /// <summary>
        /// 相手をセンダーとして接続する
        /// </summary>
        SENDER,
    }

#if !JAVA
}
#endif
