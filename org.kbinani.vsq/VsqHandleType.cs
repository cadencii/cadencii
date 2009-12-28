/*
* VsqHandleType.cs
* Copyright (C) 2008-2009 kbinani
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
#else
namespace org.kbinani.vsq {
#endif

    public enum VsqHandleType {
        Lyric,
        Vibrato,
        Singer,
        NoteHeadHandle,
    }

#if !JAVA
}
#endif
