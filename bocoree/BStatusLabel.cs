/*
 * BStatusLabel.cs
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
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BStatusLabel.java
#else
namespace bocoree.windows.forms {

    public class BStatusLabel : System.Windows.Forms.ToolStripStatusLabel {
        public void setText( string value ) {
            base.Text = value;
        }

        public string getText() {
            return base.Text;
        }

        public void setToolTipText( string value ) {
            base.ToolTipText = value;
        }

        public string getToolTipText() {
            return base.ToolTipText;
        }
    }

}
#endif
