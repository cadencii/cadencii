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
using System;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    class TagLyricTextBox {
        private String m_buf_text;
        private boolean m_phonetic_symbol_edit_mode;

        public TagLyricTextBox( String buffer_text, boolean phonetic_symbol_edit_mode ) {
            m_buf_text = buffer_text;
            m_phonetic_symbol_edit_mode = phonetic_symbol_edit_mode;
        }

        public boolean PhoneticSymbolEditMode {
            get {
                return m_phonetic_symbol_edit_mode;
            }
            set {
                m_phonetic_symbol_edit_mode = value;
            }
        }

        public String BufferText {
            get {
                return m_buf_text;
            }
            set {
                m_buf_text = value;
            }
        }
    }

}
