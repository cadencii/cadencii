/*
 * BLabel.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BLabel.java
#else
namespace bocoree.windows.forms{

    public class BLabel : System.Windows.Forms.Label {
        public string getText() {
            return base.Text;
        }

        public void setText( string value ) {
            base.Text = value;
        }
    }

}
#endif
