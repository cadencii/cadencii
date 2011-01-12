/*
 * UstPortamentoType.cs
 * Copyright © 2009-2011 kbinani
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
namespace org.kbinani.vsq
{
#endif

    public enum UstPortamentoType
    {
        /// <summary>
        /// S型．表記は''(空文字)
        /// </summary>
        S,
        /// <summary>
        /// 直線型．表記は's'
        /// </summary>
        Linear,
        /// <summary>
        /// R型．表記は'r'
        /// </summary>
        R,
        /// <summary>
        /// J型．表記は'j'
        /// </summary>
        J,
    }

#if !JAVA
}
#endif
