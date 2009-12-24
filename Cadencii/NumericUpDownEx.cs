/*
 * NumericUpDownEx.cs
 * Copyright (C) 2008-2009 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
#endif

    /// <summary>
    /// MouseWheelÇ≈IncrementÇ∏Ç¬ílÇëùå∏Ç≥ÇπÇÈÇ±Ç∆ÇÃÇ≈Ç´ÇÈNumericUpDown
    /// </summary>
#if JAVA
    public class NumericUpDownEx extends BNumericUpDown{
#else
    public class NumericUpDownEx : BNumericUpDown {
#endif
        public NumericUpDownEx() {
#if !JAVA
            this.GotFocus += new EventHandler( NumericUpDownEx_GotFocus );
#endif
        }

#if !JAVA
        private void NumericUpDownEx_GotFocus( object sender, EventArgs e ) {
            this.Select( 0, 10 );
        }
#endif

#if !JAVA
        protected override void OnMouseWheel( MouseEventArgs e ) {
            decimal new_val;
            if ( e.Delta > 0 ) {
                new_val = this.Value + this.Increment;
            } else if ( e.Delta < 0 ) {
                new_val = this.Value - this.Increment;
            } else {
                return;
            }
            if ( this.Minimum <= new_val && new_val <= this.Maximum ) {
                this.Value = new_val;
            }
        }
#endif
    }

#if !JAVA
}
#endif