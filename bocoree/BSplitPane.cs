/*
 * BSplitPane.cs
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
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BSplitPane.java
#else
namespace bocoree.windows.forms {

    public class BSplitPane : System.Windows.Forms.SplitContainer {
        public int getDividerLocation() {
            return base.SplitterDistance;
        }

        public int getDividerSize() {
            return base.SplitterWidth;
        }

        public void setDividerLocation( int value ) {
            base.SplitterDistance = value;
        }

        public void setDividerSize( int value ) {
            base.SplitterWidth = value;
        }
    }

}
#endif
