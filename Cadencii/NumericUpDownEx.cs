/*
 * NumericUpDownEx.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Windows.Forms;

namespace Boare.Cadencii {
    
    /// <summary>
    /// MouseWheelÇ≈IncrementÇ∏Ç¬ílÇëùå∏Ç≥ÇπÇÈÇ±Ç∆ÇÃÇ≈Ç´ÇÈNumericUpDown
    /// </summary>
    partial class NumericUpDownEx : NumericUpDown {
        public NumericUpDownEx() {
            //InitializeComponent();
            this.GotFocus += new EventHandler( NumericUpDownEx_GotFocus );
        }


        private void NumericUpDownEx_GotFocus( object sender, EventArgs e ) {
            this.Select( 0, 10 );
        }


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
    }

}
