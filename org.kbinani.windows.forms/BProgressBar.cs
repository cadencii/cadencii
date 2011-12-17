/*
 * BProgressBar.cs
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
//INCLUDE ../BuildJavaUI/src/org/kbinani/windows/forms/BProgressBar.java
#else
namespace com.github.cadencii.windows.forms {

    public class BProgressBar : System.Windows.Forms.ProgressBar {
        public int getMaximum() {
            return base.Maximum;
        }

        public void setMaximum( int value ) {
            base.Maximum = value;
        }

        public int getMinimum() {
            return base.Minimum;
        }

        public void setMinimum( int value ) {
            base.Minimum = value;
        }

        public int getValue() {
            return base.Value;
        }

        public void setValue( int value ) {
            base.Value = value;
        }
    }

}
#endif
