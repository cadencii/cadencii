/*
 * BTextArea.cs
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
//INCLUDE ../BuildJavaUI/src/org/kbinani/windows/forms/BTextArea.java
#else
namespace org.kbinani.windows.forms {

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
