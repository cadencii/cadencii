/*
 * BToolStripDropDownButton.cs
 * Copyright (C) 2009-2010 kbinani
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
using System;

namespace org.kbinani.windows.forms {

    public class BToolStripDropDownButton : System.Windows.Forms.ToolStripDropDownButton {
        public BToolStripDropDownButton() {
            DropDownOpening += __handleDropDownOpening;
        }

        #region event impl DropDownOpening
        // root impl of DropDownOpening event is in BMenuItem
        public BEvent<BEventHandler> dropDownOpeningEvent = new BEvent<BEventHandler>();
        // warning: to use this event, register event handler in constructor
        private void __handleDropDownOpening( object sender, EventArgs e ) {
            dropDownOpeningEvent.raise( this, e );
        }
        #endregion

        public void setText( string value ) {
            base.Text = value;
        }
    }

}
