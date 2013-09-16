/*
 * VsqHandleType.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii.vsq;
#elif __cplusplus
namespace org { namespace kbinani { namespace vsq {
#else
namespace cadencii.vsq
{
#endif

#if __cplusplus
    enum VsqHandleType
#else
    public enum VsqHandleType
#endif
    {
        Lyric,
        Vibrato,
        Singer,
        NoteHeadHandle,
        DynamicsHandle,
#if __cplusplus
    };
#else
    }
#endif

#if JAVA
#elif __cplusplus
} } }
#else
}
#endif
