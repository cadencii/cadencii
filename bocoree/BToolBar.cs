/*
 * BToolBar.cs
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
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BToolBar.java
#else
namespace bocoree.windows.forms {
    public class BToolBar : System.Windows.Forms.ToolStrip {
        public int getComponentCount() {
            return base.Items.Count;
        }

        public System.Windows.Forms.ToolStripItem getComponentAtIndex( int index ) {
            return base.Items[index];
        }

        public void add( System.Windows.Forms.ToolStripItem value ) {
            base.Items.Add( value );
        }

        public void addSeparator() {
            base.Items.Add( new System.Windows.Forms.ToolStripSeparator() );
        }
    }
}
#endif
