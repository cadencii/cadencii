/*
 * BFontChooser.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BFontChooser.java
#else
using System;
using System.Windows.Forms;
using bocoree.java.awt;

namespace bocoree.windows.forms {

    public class BFontChooser {
        public FontDialog dialog = null;

        public BFontChooser() {
            dialog = new FontDialog();
        }

        public BDialogResult showDialog() {
            DialogResult dr = dialog.ShowDialog();
            if ( dr == DialogResult.OK ) {
                return BDialogResult.OK;
            } else if ( dr == DialogResult.Cancel ) {
                return BDialogResult.CANCEL;
            }
            return BDialogResult.CANCEL;
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
