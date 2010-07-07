/*
 * AutoVibratoMinLength.cs
 * Copyright (C) 2008-2010 kbinani
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
namespace org.kbinani.cadencii {
#endif

    /// <summary>
    /// ビブラートの自動追加を行うかどうかを決める音符長さの閾値を表す列挙型
    /// </summary>
    public enum AutoVibratoMinLengthEnum {
        /// <summary>
        /// 1拍
        /// </summary>
        L1,
        /// <summary>
        /// 2拍
        /// </summary>
        L2,
        /// <summary>
        /// 3拍
        /// </summary>
        L3,
        /// <summary>
        /// 4拍
        /// </summary>
        L4,
    }

#if !JAVA
}
#endif
