/*
* VsqHandleType.cs
* Copyright (C) 2008-2011 kbinani
*
* This file is part of org.kbinani.vsq.
*
* Boare.Lib.Vsq is free software; you can redistribute it and/or
* modify it under the terms of the BSD License.
*
* Boare.Lib.Vsq is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/
#if JAVA
package org.kbinani.vsq;
#elif __cplusplus
namespace org { namespace kbinani { namespace vsq {
#else
namespace org.kbinani.vsq
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
