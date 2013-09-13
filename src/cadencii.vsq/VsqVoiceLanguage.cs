/*
 * VsqVoiceLanguage.cs
 * Copyright © 2008-2011 kbinani
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
package cadencii.vsq;
#else
namespace cadencii.vsq
{
#endif

    /// <summary>
    /// Represents the voice language of singer.
    /// </summary>
    public enum VsqVoiceLanguage
    {
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
