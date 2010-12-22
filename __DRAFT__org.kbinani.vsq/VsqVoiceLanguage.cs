/*
 * VsqVoiceLanguage.cs
 * Copyright (C) 2008-2010 kbinani
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

    /// <summary>
    /// Represents the voice language of singer.
    /// </summary>
    public enum VsqVoiceLanguage {
        /// <summary>
        /// Japanese
        /// </summary>
        Japanese,
        /// <summary>
        /// English
        /// </summary>
        English,
    }

#if !JAVA
}
#endif
