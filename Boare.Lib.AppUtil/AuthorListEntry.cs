/*
 * AuthorListEntry.cs
 * Copyright (c) 2007-2009 kbinani
 *
 * This file is part of Boare.Lib.AppUtil.
 *
 * Boare.Lib.AppUtil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.AppUtil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.apputil;

import java.awt.*;
#else
using System;
using bocoree.java.awt;

namespace Boare.Lib.AppUtil {
#endif

    public class AuthorListEntry {
        String m_name;
        int m_style;

        public AuthorListEntry( String name, int style ) {
            m_name = name;
            m_style = style;
        }

#if JAVA
        public AuthorListEntry( String name ){
            this( name, Font.PLAIN );
#else
        public AuthorListEntry( String name )
            : this( name, Font.PLAIN ) {
#endif
        }

        public AuthorListEntry() {
            m_name = "";
            m_style = Font.PLAIN;
        }

        public String getName() {
            return m_name;
        }

        public int getStyle() {
            return m_style;
        }
    }

#if !JAVA
}
#endif
