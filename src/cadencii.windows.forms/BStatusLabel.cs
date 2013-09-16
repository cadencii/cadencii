/*
 * BStatusLabel.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.windows.forms.
 *
 * cadencii.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ./BStatusLabel.java
#else
namespace cadencii.windows.forms {

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

        public cadencii.java.awt.Icon getIcon() {
            return new cadencii.java.awt.ImageIcon( base.Image );
        }

        public void setIcon( cadencii.java.awt.Icon value ) {
            if ( value == null ) {
                base.Image = null;
            } else {
                base.Image = value.image;
            }
        }
    }

}
#endif
