/*
 * FormWordDictionary.cs
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
using System.Collections.Generic;
using System.Windows.Forms;
using Boare.Lib.Vsq;
using Boare.Lib.AppUtil;
using bocoree;
using bocoree.util;
using bocoree.io;
using bocoree.windows.forms;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    partial class FormWordDictionary : BForm {
        public FormWordDictionary() {
            InitializeComponent();
            ApplyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage() {
            Text = _( "User Dictionary Configuration" );
            lblAvailableDictionaries.Text = _( "Available Dictionaries" );
            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
            btnUp.Text = _( "Up" );
            btnDown.Text = _( "Down" );
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void FormWordDictionary_Load( object sender, EventArgs e ) {
            listDictionaries.Items.Clear();
            for ( int i = 0; i < SymbolTable.getCount(); i++ ) {
                String name = SymbolTable.getSymbolTable( i ).getName();
                boolean enabled = SymbolTable.getSymbolTable( i ).isEnabled();
                listDictionaries.Items.Add( name, enabled );
            }
        }

        public Vector<ValuePair<String, Boolean>> Result {
            get {
                Vector<ValuePair<String, Boolean>> ret = new Vector<ValuePair<String, Boolean>>();
                for ( int i = 0; i < listDictionaries.Items.Count; i++ ) {
                    ret.add( new ValuePair<String, Boolean>( (String)listDictionaries.Items[i], listDictionaries.GetItemChecked( i ) ) );
                }
                return ret;
            }
        }

        private void btnOK_Click( object sender, EventArgs e ) {
            DialogResult = DialogResult.OK;
        }

        private void btnUp_Click( object sender, EventArgs e ) {
            int index = listDictionaries.SelectedIndex;
            if ( index >= 1 ) {
                listDictionaries.ClearSelected();
                String upper_name = (String)listDictionaries.Items[index - 1];
                boolean upper_enabled = listDictionaries.GetItemChecked( index - 1 );
                listDictionaries.Items[index - 1] = (String)listDictionaries.Items[index];
                listDictionaries.SetItemChecked( index - 1, listDictionaries.GetItemChecked( index ) );
                listDictionaries.Items[index] = upper_name;
                listDictionaries.SetItemChecked( index, upper_enabled );
                listDictionaries.SetSelected( index - 1, true );
            }
        }

        private void btnDown_Click( object sender, EventArgs e ) {
            int index = listDictionaries.SelectedIndex;
            if ( index + 1 < listDictionaries.Items.Count ) {
                listDictionaries.ClearSelected();
                String lower_name = (String)listDictionaries.Items[index + 1];
                boolean lower_enabled = listDictionaries.CheckedIndices.Contains( index + 1 );
                listDictionaries.Items[index + 1] = (String)listDictionaries.Items[index];
                listDictionaries.SetItemChecked( index + 1, listDictionaries.GetItemChecked( index ) );
                listDictionaries.Items[index] = lower_name;
                listDictionaries.SetItemChecked( index, lower_enabled );
                listDictionaries.SetSelected( index + 1, true );
            }
        }
    }

}
