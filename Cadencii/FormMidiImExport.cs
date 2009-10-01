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

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    public partial class FormMidiImExport : Form {
        public enum FormMidiMode {
            Import,
            Export,
            ImportVsq,
        }

        private FormMidiMode m_mode;
        private VsqFileEx m_vsq;

        public FormMidiImExport() {
            InitializeComponent();
            ApplyLanguage();
            Mode = FormMidiMode.Export;
            Util.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }

        public void ApplyLanguage() {
            if ( m_mode == FormMidiMode.Export ) {
                this.Text = _( "Midi Export" );
            } else if ( m_mode == FormMidiMode.Import ) {
                this.Text = _( "Midi Import" );
            } else {
                this.Text = _( "VSQ/Vocaloid Midi Import" );
            }
            columnTrack.Text = _( "Track" );
            columnName.Text = _( "Name" );
            columnNumNotes.Text = _( "Notes" );
            btnCheckAll.Text = _( "Check All" );
            btnUnckeckAll.Text = _( "Uncheck All" );
            groupCommonOption.Text = _( "Option" );
            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
            chkTempo.Text = _( "Tempo" );
            chkBeat.Text = _( "Beat" );
            chkNote.Text = _( "Note" );
            chkLyric.Text = _( "Lyrics" );
            chkExportVocaloidNrpn.Text = _( "vocaloid NRPN" );
        }

        public FormMidiMode Mode {
            get {
                return m_mode;
            }
            set {
                m_mode = value;
                chkExportVocaloidNrpn.Enabled = (m_mode == FormMidiMode.Export);
                chkLyric.Enabled = (m_mode != FormMidiMode.ImportVsq);
                chkNote.Enabled = (m_mode != FormMidiMode.ImportVsq);
                chkPreMeasure.Enabled = (m_mode != FormMidiMode.ImportVsq);
                if ( m_mode == FormMidiMode.Export ) {
                    this.Text = _( "Midi Export" );
                    chkPreMeasure.Text = _( "Export pre-measure part" );
                    if ( chkExportVocaloidNrpn.Checked ) {
                        chkPreMeasure.Enabled = false;
                        AppManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus = chkPreMeasure.Checked;
                        chkPreMeasure.Checked = true;
                    } else {
                        chkPreMeasure.Checked = AppManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus;
                    }
                    if ( chkNote.Checked ) {
                        chkMetaText.Enabled = false;
                        AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.Checked;
                        chkMetaText.Checked = false;
                    } else {
                        chkMetaText.Checked = AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus;
                    }
                } else if ( m_mode == FormMidiMode.Import ) {
                    this.Text = _( "Midi Import" );
                    chkPreMeasure.Text = _( "Inserting start at pre-measure" );
                    chkMetaText.Enabled = false;
                    AppManager.editorConfig.MidiImExportConfigImport.LastMetatextCheckStatus = chkMetaText.Checked;
                    chkMetaText.Checked = false;
                } else {
                    this.Text = _( "VSQ/Vocaloid Midi Import" );
                    chkPreMeasure.Text = _( "Inserting start at pre-measure" );
                    chkPreMeasure.Checked = false;
                    AppManager.editorConfig.MidiImExportConfigImportVsq.LastMetatextCheckStatus = chkMetaText.Checked;
                    chkMetaText.Checked = true;
                }
            }
        }

        private static String _( String id ) {
            return Messaging.GetMessage( id );
        }

        public boolean VocaloidMetatext {
            get {
                return chkMetaText.Checked;
            }
        }

        public boolean VocaloidNrpn {
            get {
                return chkExportVocaloidNrpn.Checked;
            }
        }

        public boolean Tempo {
            get {
                return chkTempo.Checked;
            }
            set {
                chkTempo.Checked = value;
            }
        }

        public boolean Timesig {
            get {
                return chkBeat.Checked;
            }
            set {
                chkBeat.Checked = value;
            }
        }

        public boolean Notes {
            get {
                return chkNote.Checked;
            }
        }

        public boolean Lyric {
            get {
                return chkLyric.Checked;
            }
        }

        public boolean isPreMeasure() {
            return chkPreMeasure.Checked;
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
            if ( m_mode == FormMidiMode.Export ) {
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
            if ( m_mode == FormMidiMode.Export ) {
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
            if ( m_mode == FormMidiMode.Export ) {
                AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.Checked;
            }
        }
    }

}
