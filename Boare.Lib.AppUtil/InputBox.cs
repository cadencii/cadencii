/*
 * InputBox.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Lib.AppUtil.
 *
 * Boare.Lib.AppUtil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.AppUtil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Windows.Forms;

namespace Boare.Lib.AppUtil {
    public partial class InputBox : Form {
        public InputBox( string message ) {
            InitializeComponent();
            lblMessage.Text = message;
        }

        public string Result {
            get {
                return txtInput.Text;
            }
            set {
                txtInput.Text = value;
            }
        }

        private void btnOk_Click( object sender, EventArgs e ) {
            DialogResult = DialogResult.OK;
        }
    }
}
