/*
 * EditorStatus.cs
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
package org.kbinani.Cadencii;
#else
namespace Boare.Cadencii {
    using boolean = System.Boolean;
#endif

    public class EditorStatus {
        /// <summary>
        /// トラックのレンダリングが必要かどうかを表すフラグ
        /// </summary>
        public boolean[] renderRequired = new boolean[16];

        public EditorStatus() {
            for ( int i = 0; i < renderRequired.Length; i++ ) {
                renderRequired[i] = false;
            }
        }

        public EditorStatus clone() {
            EditorStatus ret = new EditorStatus();
            for ( int i = 0; i < renderRequired.Length; i++ ) {
                ret.renderRequired[i] = renderRequired[i];
            }
            return ret;
        }
    }

#if!JAVA
}
#endif
