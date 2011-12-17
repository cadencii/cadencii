#if !JAVA
/*
 * BToolStripTextBox.cs
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
namespace com.github.cadencii.windows.forms {

    public class BToolStripTextBox : System.Windows.Forms.ToolStripTextBox {
        public void setText( string value ) {
            base.Text = value;
        }

        public string getText() {
            return base.Text;
        }
    }

}
#endif
