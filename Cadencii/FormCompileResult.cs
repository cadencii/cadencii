/*
 * FormCompileResult.cs
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
using System.Windows.Forms;

using Boare.Lib.AppUtil;

namespace Boare.Cadencii {

    public partial class FormCompileResult : Form {
        public FormCompileResult( String message, String errors ) {
            InitializeComponent();
            ApplyLanguage();
            label1.Text = message;
            textBox1.Text = errors;
            Misc.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }

        public void ApplyLanguage() {
            this.textBox1.Text = _( "Script Compilation Result" );
        }

        private static String _( String id ) {
            return Messaging.GetMessage( id );
        }
    }

}
