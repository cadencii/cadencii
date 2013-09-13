/*
 * UstPortamentoPoint.cs
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
package cadencii.vsq;

import java.io.*;
#else

using System;

namespace cadencii.vsq
{
#endif

#if JAVA
    public class UstPortamentoPoint implements Serializable
#else
    [Serializable]
    public struct UstPortamentoPoint
#endif
    {
        public int Step;
        public float Value;
        public UstPortamentoType Type;
    }

#if !JAVA
}
#endif
