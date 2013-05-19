/*
 * UstEventProperty.cs
 * Copyright © 2011 kbinani
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
    public class UstEventProperty implements Serializable
#else
    [Serializable]
    public class UstEventProperty
#endif
    {
        public String Name;
        public String Value;

        public UstEventProperty()
        {
        }

        public UstEventProperty( String name, String value )
        {
            Name = name;
            Value = value;
        }
    }

#if !JAVA
}
#endif
