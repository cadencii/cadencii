/*
* VsqIDType.cs
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
#else
namespace com.github.cadencii.vsq
{
#endif

    public enum VsqIDType
    {
        Singer,
        Anote,
        Aicon,
        Unknown
    }

#if !JAVA
}
#endif
