/*
 * FormImportLyric.cs
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

using Boare.Lib.AppUtil;
using bocoree;

namespace Boare.Cadencii {

    partial class FormImportLyric : Form {
        private int m_max_notes = 1;

        public FormImportLyric( int max_notes ) {
            InitializeComponent();
            ApplyLanguage();
            String notes = (max_notes > 1) ? " [notes]" : " [note]";
            lblNotes.Text = "Max : " + max_notes + notes;
            m_max_notes = max_notes;
            Misc.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }

        public void ApplyLanguage() {
            Text = _( "Import lyrics" );
            btnCancel.Text = _( "Cancel" );
            btnOK.Text = _( "OK" );
        }

        public static String _( String id ) {
            return Messaging.GetMessage( id );
        }
        
        public String[] GetLetters() {
            Vector<char> _SMALL = new Vector<char>( new char[] { 'ぁ', 'ぃ', 'ぅ', 'ぇ', 'ぉ', 'ゃ', 'ゅ', 'ょ', 'ァ', 'ィ', 'ゥ', 'ェ', 'ォ', 'ャ', 'ュ', 'ョ' } );
            String tmp = "";
            for ( int i = 0; i < m_max_notes; i++ ) {
                if ( i >= txtLyrics.Lines.Length ) {
                    break;
                }
                tmp += txtLyrics.Lines[i] + " ";
            }
            String[] spl = tmp.Split( new char[] { '\n', '\t', ' ', '　', '\r' }, StringSplitOptions.RemoveEmptyEntries );
            Vector<String> ret = new Vector<String>();
            foreach ( String s in spl ) {
                char[] list = s.ToCharArray();
                String t = "";
                int i = -1;
                while( i + 1 < list.Length ){
                    i++;
                    if ( 0x41 <= list[i] && list[i] <= 0x176 ) {
                        t += list[i].ToString();
                    } else {
                        if ( t.Length > 0 ) {
                            ret.add( t );
                            t = "";
                        }
                        if ( i + 1 < list.Length ) {
                            if ( _SMALL.contains( list[i + 1] ) ) {
                                // 次の文字が拗音の場合
                                ret.add( list[i].ToString() + list[i + 1].ToString() );
                                i++;
                            } else {
                                ret.add( list[i].ToString() );
                            }
                        } else {
                            ret.add( list[i].ToString() );
                        }
                    }
                }
                if ( t.Length > 0 ) {
                    ret.add( t );
                }
            }
            return ret.toArray( new String[]{} );
        }

        private void btnOK_Click( object sender, EventArgs e ) {
            this.DialogResult = DialogResult.OK;
        }
    }

}
