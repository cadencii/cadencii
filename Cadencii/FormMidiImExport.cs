/*
 * FormMidiImExport.cs
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using boolean = System.Boolean;

    public partial class FormMidiImExport : BForm {
        public enum FormMidiMode {
            IMPORT,
            EXPORT,
            IMPORT_VSQ,
        }

        private FormMidiMode m_mode;
        private VsqFileEx m_vsq;

        public FormMidiImExport() {
            InitializeComponent();
            ApplyLanguage();
            setMode( FormMidiMode.EXPORT );
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage() {
            if ( m_mode == FormMidiMode.EXPORT ) {
                setTitle( _( "Midi Export" ) );
            } else if ( m_mode == FormMidiMode.IMPORT ) {
                setTitle( _( "Midi Import" ) );
            } else {
                setTitle( _( "VSQ/Vocaloid Midi Import" ) );
            }
            columnTrack.Text = _( "Track" );
            columnName.Text = _( "Name" );
            columnNumNotes.Text = _( "Notes" );
            btnCheckAll.setText( _( "Check All" ) );
            btnUnckeckAll.setText( _( "Uncheck All" ) );
            groupCommonOption.Text = _( "Option" );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
            chkTempo.setText( _( "Tempo" ) );
            chkBeat.setText( _( "Beat" ) );
            chkNote.setText( _( "Note" ) );
            chkLyric.setText( _( "Lyrics" ) );
            chkExportVocaloidNrpn.setText( _( "vocaloid NRPN" ) );
        }

        public FormMidiMode getMode() {
                return m_mode;
        }

        public void setMode( FormMidiMode value ) {
            m_mode = value;
            chkExportVocaloidNrpn.setEnabled( (m_mode == FormMidiMode.EXPORT) );
            chkLyric.setEnabled( (m_mode != FormMidiMode.IMPORT_VSQ) );
            chkNote.setEnabled( (m_mode != FormMidiMode.IMPORT_VSQ) );
            chkPreMeasure.setEnabled( (m_mode != FormMidiMode.IMPORT_VSQ) );
            if ( m_mode == FormMidiMode.EXPORT ) {
                this.Text = _( "Midi Export" );
                chkPreMeasure.setText( _( "Export pre-measure part" ) );
                if ( chkExportVocaloidNrpn.isSelected() ) {
                    chkPreMeasure.setEnabled( false );
                    AppManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus = chkPreMeasure.isSelected();
                    chkPreMeasure.setSelected( true );
                } else {
                    chkPreMeasure.setSelected( AppManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus );
                }
                if ( chkNote.isSelected() ) {
                    chkMetaText.setEnabled( false );
                    AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.isSelected();
                    chkMetaText.setSelected( false );
                } else {
                    chkMetaText.setSelected( AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus );
                }
            } else if ( m_mode == FormMidiMode.IMPORT ) {
                setTitle( _( "Midi Import" ) );
                chkPreMeasure.setText( _( "Inserting start at pre-measure" ) );
                chkMetaText.setEnabled( false );
                AppManager.editorConfig.MidiImExportConfigImport.LastMetatextCheckStatus = chkMetaText.isSelected();
                chkMetaText.setSelected( false );
            } else {
                setTitle( _( "VSQ/Vocaloid Midi Import" ) );
                chkPreMeasure.setText( _( "Inserting start at pre-measure" ) );
                chkPreMeasure.setSelected( false );
                AppManager.editorConfig.MidiImExportConfigImportVsq.LastMetatextCheckStatus = chkMetaText.isSelected();
                chkMetaText.setSelected( true );
            }
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public boolean isVocaloidMetatext() {
            if ( chkNote.isSelected() )
            {
                return false;
            }
            else
            {
                return chkMetaText.isSelected();
            }
        }

        public boolean isVocaloidNrpn() {
            return chkExportVocaloidNrpn.isSelected();
        }

        public boolean isTempo() {
            return chkTempo.isSelected();
        }

        public void setTempo( boolean value ) {
            chkTempo.setSelected( value );
        }

        public boolean isTimesig() {
            return chkBeat.isSelected();
        }

        public void setTimesig( boolean value ) {
            chkBeat.setSelected( value );
        }

        public boolean isNotes() {
            return chkNote.isSelected();
        }

        public boolean isLyric() {
            return chkLyric.isSelected();
        }

        public boolean isPreMeasure() {
            return chkPreMeasure.isSelected();
        }

        private void btnCheckAll_Click( object sender, EventArgs e ) {
            for ( int i = 0; i < ListTrack.Items.Count; i++ ) {
                ListTrack.Items[i].Checked = true;
            }
        }

        private void btnUnckeckAll_Click( object sender, EventArgs e ) {
            for ( int i = 0; i < ListTrack.Items.Count; i++ ) {
                ListTrack.Items[i].Checked = false;
            }
        }

        private void chkExportVocaloidNrpn_CheckedChanged( object sender, EventArgs e ) {
            if ( m_mode == FormMidiMode.EXPORT ) {
                if ( chkExportVocaloidNrpn.Checked ) {
                    chkPreMeasure.Enabled = false;
                    AppManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus = chkPreMeasure.Checked;
                    chkPreMeasure.Checked = true;
                } else {
                    chkPreMeasure.Enabled = true;
                    chkPreMeasure.Checked = AppManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus;
                }
            }
        }

        private void chkNote_CheckedChanged( object sender, EventArgs e ) {
            if ( m_mode == FormMidiMode.EXPORT ) {
                if ( chkNote.Checked ) {
                    chkMetaText.Enabled = false;
                    AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.Checked;
                    chkMetaText.Checked = false;
                } else {
                    chkMetaText.Enabled = true;
                    chkMetaText.Checked = AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus;
                }
            }
        }

        private void chkMetaText_Click( object sender, EventArgs e ) {
            if ( m_mode == FormMidiMode.EXPORT ) {
                AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.Checked;
            }
        }
    }

}
