/*
 * BButton.cs
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
//INCLUDE ../BuildJavaUI/src/org/kbinani/windows/forms/BRadioButton.java
#else
#define ABSTRACT_BUTTON_ENABLE_IS_SELECTED

namespace org.kbinani.windows.forms{

    public class BRadioButton : System.Windows.Forms.RadioButton {
        #region event impl CheckedChanged
        // root impl of CheckedChanged event is in BCheckBox
        public BEvent<BEventHandler> checkedChangedEvent = new BEvent<BEventHandler>();
        protected override void OnCheckedChanged( System.EventArgs e ) {
            base.OnCheckedChanged( e );
            checkedChangedEvent.raise( this, e );
        }
        #endregion

        #region javax.swing.AbstractButton
        // root implementation of javax.swing.AbstractButton is in BMenuItem.cs
        public string getText() {
            return base.Text;
        }

        public void setText( string value ) {
            base.Text = value;
        }
#if ABSTRACT_BUTTON_ENABLE_IS_SELECTED
        public bool isSelected() {
            return base.Checked;
        }

        public void setSelected( bool value ) {
            base.Checked = value;
        }
#endif
        public org.kbinani.java.awt.Icon getIcon() {
            org.kbinani.java.awt.Icon ret = new org.kbinani.java.awt.Icon();
            ret.image = base.Image;
            return ret;
        }

        public void setIcon( org.kbinani.java.awt.Icon value ) {
            if ( value == null ) {
                base.Image = null;
            } else {
                base.Image = value.image;
            }
        }
        #endregion
    }

}
#endif
