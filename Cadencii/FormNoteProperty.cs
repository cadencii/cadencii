/*
 * FormNoteProperty.cs
 * Copyright (c) 2009 kbinani
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
using System.Collections.Generic;
using System.Windows.Forms;

using Boare.Lib.Vsq;
using Boare.Lib.AppUtil;

namespace Boare.Cadencii {

    public partial class FormNoteProperty : Form {
        public FormNoteProperty() {
            InitializeComponent();
            ApplyLanguage();
            Misc.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }

        public void ApplyLanguage() {
            this.Text = _( "Note Property" );
        }

        private static String _( String id ) {
            return Messaging.GetMessage( id );
        }

        public Keys FormCloseShortcutKey {
            get {
                return menuClose.ShortcutKeys;
            }
            set {
                menuClose.ShortcutKeys = value;
            }
        }

        private void menuClose_Click( object sender, EventArgs e ) {
            this.Close();
        }
    }

}
