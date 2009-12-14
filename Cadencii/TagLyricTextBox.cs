/*
 * TagLyricTextBox.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

#else
using System;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    public class TagLyricTextBox {
        private String m_buf_text;
        private boolean m_phonetic_symbol_edit_mode;

        public TagLyricTextBox( String buffer_text, boolean phonetic_symbol_edit_mode ) {
            m_buf_text = buffer_text;
            m_phonetic_symbol_edit_mode = phonetic_symbol_edit_mode;
        }

        public boolean isPhoneticSymbolEditMode() {
            return m_phonetic_symbol_edit_mode;
        }

        public void setPhoneticSymbolEditMode( boolean value ) {
            m_phonetic_symbol_edit_mode = value;
        }

        public String getBufferText() {
            return m_buf_text;
        }

        public void setBufferText( String value ) {
            m_buf_text = value;
        }
    }

#if !JAVA
}
#endif
