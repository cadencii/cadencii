/*
 * BFontChooser.cs
 * Copyright (C) 2009-2010 kbinani
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
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BFontChooser.java
#else
using System;
using System.Windows.Forms;
using org.kbinani.java.awt;

namespace org.kbinani.windows.forms {

    public class BFontChooser {
        public FontDialog dialog = null;
        private BDialogResult m_result = BDialogResult.CANCEL;

        public BFontChooser() {
            dialog = new FontDialog();
        }

        public void setVisible( bool value ) {
            if ( value ) {
                DialogResult dr = dialog.ShowDialog();
                if ( dr == DialogResult.OK ) {
                    m_result = BDialogResult.OK;
                } else if ( dr == DialogResult.Cancel ) {
                    m_result = BDialogResult.CANCEL;
                }
            } else {
                //do nothing
            }
        }

        public BDialogResult getDialogResult() {
            return m_result;
        }

        public void setSelectedFont( Font font ) {
            dialog.Font = font.font;
        }

        public Font getSelectedFont() {
            return new Font( dialog.Font );
        }
    }

}
#endif
