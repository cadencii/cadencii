/*
 * CircuitConfigEntry.cs
 * Copyright (C) 2010 kbinani
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
    /// シンセサイザ間の接続情報を保持するクラス．
    /// </summary>
    public class CircuitConfigEntry {
        /// <summary>
        /// 接続の種類
        /// </summary>
        public CircuitConnectionKind ConnectionKind;

        public CircuitConfigEntry() {
        }

        public CircuitConfigEntry( CircuitConnectionKind connection_kind ) {
            ConnectionKind = connection_kind;
        }
    }

#if !JAVA
}
#endif
