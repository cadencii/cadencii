/*
 * XmlSerializable.cs
 * Copyright (C) 2009 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.xml;
#else
using System;

namespace org.kbinani.xml {
    using boolean = System.Boolean;
#endif

    public interface XmlSerializable {
        String getXmlElementName( String name );
        boolean isXmlIgnored( String name );
        String getGenericTypeName( String name );
    }

#if !JAVA
}
#endif
