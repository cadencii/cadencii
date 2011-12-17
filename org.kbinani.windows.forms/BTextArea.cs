/*
 * BTextArea.cs
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
//INCLUDE ./BTextArea.java
#else
namespace com.github.cadencii.windows.forms {

    public class BTextArea : System.Windows.Forms.TextBox {
        public BTextArea() {
            base.Multiline = true;
            base.AcceptsReturn = true;
            base.AcceptsTab = true;
            base.WordWrap = false;
            base.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        }

        public int getLineStartOffset( int line ) {
            return base.GetFirstCharIndexFromLine( line );
        }

        public int getLineEndOffset( int line ) {
            return base.GetFirstCharIndexFromLine( line ) + base.Lines[line].Length;
        }

        public int getLineOfOffset( int offset ) {
            return base.GetLineFromCharIndex( offset );
        }

        public int getLineCount() {
            return base.Lines.Length;
        }

        public string getText() {
            return base.Text;
        }

        public string getText( int offset, int len ) {
            return base.Text.Substring( offset, len );
        }
    }

}
#endif
