/*
 * BStatusLabel.cs
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
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BStatusLabel.java
#else
namespace org.kbinani.windows.forms {

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

        public org.kbinani.java.awt.Icon getIcon() {
            return new org.kbinani.java.awt.ImageIcon( base.Image );
        }

        public void setIcon( org.kbinani.java.awt.Icon value ) {
            if ( value == null ) {
                base.Image = null;
            } else {
                base.Image = value.image;
            }
        }
    }

}
#endif
