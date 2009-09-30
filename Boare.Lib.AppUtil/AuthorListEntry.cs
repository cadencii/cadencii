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
using System.Drawing;

namespace Boare.Lib.AppUtil {

    public class AuthorListEntry {
        string m_name;
        FontStyle m_style;

        public AuthorListEntry( string name, FontStyle style )
            : this( name ) {
            m_style = style;
        }

        public AuthorListEntry( string name ) {
            m_name = name;
            m_style = FontStyle.Regular;
        }

        public AuthorListEntry() {
            m_name = "";
            m_style = FontStyle.Regular;
        }

        public string Name {
            get {
                return m_name;
            }
        }

        public FontStyle Style {
            get {
                return m_style;
            }
        }
    }

}
