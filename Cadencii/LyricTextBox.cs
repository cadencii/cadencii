/*
 * TextBoxEx.cs
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ../BuildJavaUI/src/org/kbinani/Cadencii/TextBoxEx.java
#else
using System;
using System.Windows.Forms;
using org.kbinani.windows.forms;
using org.kbinani;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;

    /// <summary>
    /// 歌詞入力用のテキストボックス
    /// </summary>
    public class TextBoxEx : BTextBox {
        private boolean compositioning = false;
        private String m_buf_text;
        private boolean m_phonetic_symbol_edit_mode;

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

        protected override boolean IsInputKey( Keys keyData ) {
            switch ( keyData ) {
                case Keys.Tab:
                case Keys.Tab | Keys.Shift:
                    break;
                default:
                    return base.IsInputKey( keyData );
            }
            return true;
        }

        /*protected override void WndProc( ref Message m ) {
            base.WndProc( ref m );
            if ( m.Msg == win32.WM_IME_STARTCOMPOSITION ) {
                compositioning = true;
#if DEBUG
                PortUtil.println( "TextBoxEx#WndProc; compositioning=" + compositioning );
#endif
            } else if ( m.Msg == win32.WM_IME_ENDCOMPOSITION ) {
                compositioning = false;
#if DEBUG
                PortUtil.println( "TextBoxEx#WndProc; compositioning=" + compositioning );
#endif
            }
        }

        /// <summary>
        /// IMEが変換中かどうかを調べます。
        /// </summary>
        /// <returns></returns>
        public boolean isComposition() {
            return compositioning;
        }*/
    }

}
#endif
