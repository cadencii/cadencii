/*
 * LyricTextBox.cs
 * Copyright © 2008-2011 kbinani
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
//INCLUDE ./LyricTextBox.java
#else
using System;
using System.Windows.Forms;
using com.github.cadencii.windows.forms;
using com.github.cadencii;

namespace com.github.cadencii {
    using boolean = System.Boolean;

    /// <summary>
    /// 歌詞入力用のテキストボックス
    /// </summary>
    public class LyricTextBox : BTextBox {
        private String m_buf_text;
        private boolean m_phonetic_symbol_edit_mode;

        /// <summary>
        /// 発音記号を編集するモードかどうかを表すブール値を取得します
        /// </summary>
        /// <returns></returns>
        public boolean isPhoneticSymbolEditMode() {
            return m_phonetic_symbol_edit_mode;
        }

        /// <summary>
        /// 発音記号を編集するモードかどうかを表すブール値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setPhoneticSymbolEditMode( boolean value ) {
            m_phonetic_symbol_edit_mode = value;
        }

        /// <summary>
        /// バッファーテキストを取得します
        /// (バッファーテキストには，発音記号モードでは歌詞，歌詞モードでは発音記号がそれぞれ格納される)
        /// </summary>
        /// <returns></returns>
        public String getBufferText() {
            return m_buf_text;
        }

        /// <summary>
        /// バッファーテキストを設定します
        /// (バッファーテキストには，発音記号モードでは歌詞，歌詞モードでは発音記号がそれぞれ格納される)
        /// </summary>
        /// <param name="value"></param>
        public void setBufferText( String value ) {
            m_buf_text = value;
        }

        /// <summary>
        /// オーバーライド．(Tab)または(Tab+Shift)も入力キーとみなすようオーバーライドされている
        /// </summary>
        /// <param name="keyData">キーの値の一つ</param>
        /// <returns>指定されているキーが入力キーである場合は true．それ以外の場合は false．</returns>
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
    }

}
#endif
