/*
 * FormInsertBar.cs
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

    partial class FormInsertBar : Form {
        public FormInsertBar( int max_position ) {
            InitializeComponent();
            ApplyLanguage();
            numPosition.Maximum = max_position;
            Util.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }

        public void ApplyLanguage() {
            Text = _( "Insert Bars" );
            String th_prefix = _( "_PREFIX_TH_" );
            if ( th_prefix.Equals( "_PREFIX_TH_" ) ) {
                lblPositionPrefix.Text = "";
            } else {
                lblPositionPrefix.Text = th_prefix;
            }
            lblPosition.Text = _( "Position" );
            lblLength.Text = _( "Length" );
            lblThBar.Text = _( "th bar" );
            lblBar.Text = _( "bar" );
            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
        }

        public static String _( String id ) {
            return Messaging.GetMessage( id );
        }

        public int Length {
            get {
                return (int)numLength.Value;
            }
            set {
                numLength.Value = value;
            }
        }

        public int Position {
            get {
                return (int)numPosition.Value;
            }
            set {
                numPosition.Value = value;
            }
        }

        private void btnOK_Click( object sender, EventArgs e ) {
            DialogResult = DialogResult.OK;
        }
    }

}
