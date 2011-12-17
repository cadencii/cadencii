/*
 * RendererKind.cs
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

    /// <summary>
    /// 歌声合成システムの種類
    /// </summary>
    public enum RendererKind
    {
        VOCALOID1,
        /// <summary>
        /// Synthesize Engine 1.0のVOCALOID1
        /// </summary>
        VOCALOID1_100,
        /// <summary>
        /// Synthesize Engine 1.1のVOCALOID1
        /// </summary>
        VOCALOID1_101,
        /// <summary>
        /// VOCALOID2
        /// </summary>
        VOCALOID2,
        /// <summary>
        /// AquesTone
        /// </summary>
        AQUES_TONE,
        /// <summary>
        /// UTAU
        /// </summary>
        UTAU,
        /// <summary>
        /// vConnect-STAND
        /// </summary>
        VCNT,
        /// <summary>
        /// 何もしない歌声合成システム(EmptyRenderingRunner)
        /// </summary>
        NULL,
    }

#else
using System;

namespace com.github.cadencii{

    /// <summary>
    /// 歌声合成システムの種類
    /// </summary>
    public enum RendererKind : int
    {
        /// <summary>
        /// VOCALOID1
        /// </summary>
        VOCALOID1 = 1,
        /// <summary>
        /// Synthesize Engine 1.0のVOCALOID1．過去のバージョンとの互換性のために残存．
        /// </summary>
        VOCALOID1_100 = 1,
        /// <summary>
        /// Synthesize Engine 1.1のVOCALOID1．過去のバージョンとの互換性のために残存．
        /// </summary>
        VOCALOID1_101 = 1,
        /// <summary>
        /// VOCALOID2
        /// </summary>
        VOCALOID2 = 0,
        /// <summary>
        /// AquesTone
        /// </summary>
        AQUES_TONE = 3,
        /// <summary>
        /// UTAU
        /// </summary>
        UTAU = 4,
        /// <summary>
        /// STRAIGHT X UTAU
        /// </summary>
        STRAIGHT_UTAU = 5,
        /// <summary>
        /// vConnect-STAND
        /// </summary>
        VCNT = 5,
        /// <summary>
        /// 何もしない歌声合成システム(EmptyRenderingRunner)
        /// </summary>
        NULL = -1,
    }

}
#endif
