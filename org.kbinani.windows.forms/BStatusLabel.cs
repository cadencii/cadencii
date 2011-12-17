/*
 * BStatusLabel.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.windows.forms.
 *
 * org.kbinani.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ../BuildJavaUI/src/org/kbinani/windows/forms/BStatusLabel.java
#else
namespace com.github.cadencii.windows.forms {

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

        public com.github.cadencii.java.awt.Icon getIcon() {
            return new com.github.cadencii.java.awt.ImageIcon( base.Image );
        }

        public void setIcon( com.github.cadencii.java.awt.Icon value ) {
            if ( value == null ) {
                base.Image = null;
            } else {
                base.Image = value.image;
            }
        }
    }

}
#endif
