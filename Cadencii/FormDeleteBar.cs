/*
 * FormDeleteBar.cs
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

using Boare.Lib.AppUtil;

namespace Boare.Cadencii {

    partial class FormDeleteBar : Form {
        public FormDeleteBar( int max_barcount ) {
            InitializeComponent();
            ApplyLanguage();
            numStart.Maximum = max_barcount;
            numEnd.Maximum = max_barcount;
            Util.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }
        
        public void ApplyLanguage() {
            Text = _( "Delete Bars" );
            lblStart.Text = _( "Start" );
            lblEnd.Text = _( "End" );
            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
        }
        
        public static String _( String id ) {
            return Messaging.GetMessage( id );
        }
                
        public int Start {
            get {
                return (int)numStart.Value;
            }
            set {
                numStart.Value = value;
            }
        }
        
        public int End {
            get {
                return (int)numEnd.Value;
            }
            set {
                numEnd.Value = value;
            }
        }
        
        private void btnOK_Click( object sender, EventArgs e ) {
            DialogResult = DialogResult.OK;
        }
    }

}
