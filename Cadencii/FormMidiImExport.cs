/*
 * FormMidiImExport.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

//INCLUDE-SECTION IMPORT ..\BuildJavaUI\src\org\kbinani\Cadencii\FormMidiImExport.java

import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.apputil;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
#endif

#if JAVA
    public class FormMidiImExport extends BForm {
#else
    public class FormMidiImExport : BForm {
#endif
        public enum FormMidiMode {
            IMPORT,
            EXPORT,
            IMPORT_VSQ,
        }

        private FormMidiMode m_mode;
        private VsqFileEx m_vsq;
        private static int columnWidthTrack = 54;
        private static int columnWidthName = 122;
        private BLabel lblOffset;
        private NumberTextBox txtOffset;
        private BLabel lblOffsetUnit;
        private static int columnWidthNotes = 126;

        public FormMidiImExport() {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            ApplyLanguage();
            setMode( FormMidiMode.EXPORT );
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
            listTrack.setColumnHeaders( new String[] { _( "Track" ), _( "Name" ), _( "Notes" ) } );
            listTrack.setColumnWidth( 0, columnWidthTrack );
            listTrack.setColumnWidth( 1, columnWidthName );
            listTrack.setColumnWidth( 2, columnWidthNotes );

            registerEventHandlers();
            setResources();
        }

        public void ApplyLanguage() {
            if ( m_mode == FormMidiMode.EXPORT ) {
                setTitle( _( "Midi Export" ) );
            } else if ( m_mode == FormMidiMode.IMPORT ) {
                setTitle( _( "Midi Import" ) );
            } else {
                setTitle( _( "VSQ/Vocaloid Midi Import" ) );
            }
            groupMode.setTitle( _( "Import Basis" ) );
            radioGateTime.setText( _( "gate-time" ) );
            radioPlayTime.setText( _( "play-time" ) );
            listTrack.setColumnHeaders( new String[] { _( "Track" ), _( "Name" ), _( "Notes" ) } );
            btnCheckAll.setText( _( "Check All" ) );
            btnUncheckAll.setText( _( "Uncheck All" ) );
            groupCommonOption.setTitle( _( "Option" ) );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
            chkTempo.setText( _( "Tempo" ) );
            chkBeat.setText( _( "Beat" ) );
            chkNote.setText( _( "Note" ) );
            chkLyric.setText( _( "Lyrics" ) );
            chkExportVocaloidNrpn.setText( _( "vocaloid NRPN" ) );
            lblOffset.setText( _( "offset" ) );
            if ( radioGateTime.isSelected() ) {
                lblOffsetUnit.setText( _( "clocks" ) );
            } else {
                lblOffsetUnit.setText( _( "seconds" ) );
            }
        }

        public double getOffsetSeconds() {
            double v = 0.0;
            try {
                v = PortUtil.parseDouble( txtOffset.getText() );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "FormMidiImExport#getOffsetClocks; ex=" + ex );
            }
            return v;
        }

        public int getOffsetClocks() {
            int v = 0;
            try {
                v = PortUtil.parseInt( txtOffset.getText() );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "FormMidiImExport#getOffsetClocks; ex=" + ex );
            }
            return v;
        }

        public boolean isSecondBasis() {
            return radioPlayTime.isSelected();
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
                setTitle( _( "Midi Export" ) );
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
                groupMode.setEnabled( false );
            } else if ( m_mode == FormMidiMode.IMPORT ) {
                setTitle( _( "Midi Import" ) );
                chkPreMeasure.setText( _( "Inserting start at pre-measure" ) );
                chkMetaText.setEnabled( false );
                AppManager.editorConfig.MidiImExportConfigImport.LastMetatextCheckStatus = chkMetaText.isSelected();
                chkMetaText.setSelected( false );
                groupMode.setEnabled( true );
            } else {
                setTitle( _( "VSQ/Vocaloid Midi Import" ) );
                chkPreMeasure.setText( _( "Inserting start at pre-measure" ) );
                chkPreMeasure.setSelected( false );
                AppManager.editorConfig.MidiImExportConfigImportVsq.LastMetatextCheckStatus = chkMetaText.isSelected();
                chkMetaText.setSelected( true );
                groupMode.setEnabled( false );
            }
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public boolean isVocaloidMetatext() {
            if ( chkNote.isSelected() ) {
                return false;
            } else {
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

        private void btnCheckAll_Click( Object sender, BEventArgs e ) {
            for ( int i = 0; i < listTrack.getItemCount( "" ); i++ ) {
                listTrack.setItemCheckedAt( "", i, true );
            }
        }

        private void btnUnckeckAll_Click( Object sender, BEventArgs e ) {
            for ( int i = 0; i < listTrack.getItemCount( "" ); i++ ) {
                listTrack.setItemCheckedAt( "", i, false );
            }
        }

        private void chkExportVocaloidNrpn_CheckedChanged( Object sender, BEventArgs e ) {
            if ( m_mode == FormMidiMode.EXPORT ) {
                if ( chkExportVocaloidNrpn.isSelected() ) {
                    chkPreMeasure.setEnabled( false );
                    AppManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus = chkPreMeasure.isSelected();
                    chkPreMeasure.setSelected( true );
                } else {
                    chkPreMeasure.setEnabled( true );
                    chkPreMeasure.setSelected( AppManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus );
                }
            }
        }

        private void chkNote_CheckedChanged( Object sender, BEventArgs e ) {
            if ( m_mode == FormMidiMode.EXPORT ) {
                if ( chkNote.isSelected() ) {
                    chkMetaText.setEnabled( false );
                    AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.isSelected();
                    chkMetaText.setSelected( false );
                } else {
                    chkMetaText.setEnabled( true );
                    chkMetaText.setSelected( AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus );
                }
            }
        }

        private void chkMetaText_Click( Object sender, BEventArgs e ) {
            if ( m_mode == FormMidiMode.EXPORT ) {
                AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.isSelected();
            }
        }

        private void FormMidiImExport_FormClosing( Object sender, BFormClosingEventArgs e ) {
            columnWidthTrack = listTrack.getColumnWidth( 0 );
            columnWidthName = listTrack.getColumnWidth( 1 );
            columnWidthNotes = listTrack.getColumnWidth( 2 );
        }

        private void btnCancel_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }

        private void btnOK_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }

        public void radioGateTime_CheckedChanged( Object sender, EventArgs e ) {
            if ( radioGateTime.isSelected() ) {
                lblOffsetUnit.setText( _( "clocks" ) );
                txtOffset.setType( NumberTextBox.ValueType.Integer );
            }
        }

        public void radioPlayTime_CheckedChanged( Object sender, EventArgs e ) {
            if ( radioPlayTime.isSelected() ) {
                lblOffsetUnit.setText( _( "seconds" ) );
                txtOffset.setType( NumberTextBox.ValueType.Double );
            }
        }

        private void registerEventHandlers() {
#if JAVA
            this.btnCheckAll.clickEvent.add( new BEventHandler( this, "btnCheckAll_Click" ) );
            this.btnUncheckAll.clickEvent.add( new BEventHandler( this, "btnUnckeckAll_Click" ) );
            this.chkNote.checkedChangedEvent.add( new BEventHandler( this, "chkNote_CheckedChanged" ) );
            this.chkMetaText.clickEvent.add( new BEventHandler( this, "chkMetaText_Click" ) );
            this.chkExportVocaloidNrpn.checkedChangedEvent.add( new BEventHandler( this, "chkExportVocaloidNrpn_CheckedChanged" ) );
#else
            this.btnCheckAll.Click += new System.EventHandler( this.btnCheckAll_Click );
            this.btnUncheckAll.Click += new System.EventHandler( this.btnUnckeckAll_Click );
            this.chkNote.CheckedChanged += new System.EventHandler( this.chkNote_CheckedChanged );
            this.chkMetaText.Click += new System.EventHandler( this.chkMetaText_Click );
            this.chkExportVocaloidNrpn.CheckedChanged += new System.EventHandler( this.chkExportVocaloidNrpn_CheckedChanged );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( FormMidiImExport_FormClosing );
            btnOK.Click += new EventHandler( btnOK_Click );
            btnCancel.Click += new EventHandler( btnCancel_Click );
#endif
            radioGateTime.checkedChangedEvent.add( new BEventHandler( this, "radioGateTime_CheckedChanged" ) );
            radioPlayTime.checkedChangedEvent.add( new BEventHandler( this, "radioPlayTime_CheckedChanged" ) );
        }

        private void setResources() {
        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormMidiImExport.java
        //INCLUDE-SECTION METHOD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormMidiImExport.java
        #endregion
#else
        #region UI Impl for C#
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing ) {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.listTrack = new org.kbinani.windows.forms.BListView();
            this.btnCheckAll = new org.kbinani.windows.forms.BButton();
            this.btnUncheckAll = new org.kbinani.windows.forms.BButton();
            this.chkBeat = new org.kbinani.windows.forms.BCheckBox();
            this.chkTempo = new org.kbinani.windows.forms.BCheckBox();
            this.chkNote = new org.kbinani.windows.forms.BCheckBox();
            this.chkLyric = new org.kbinani.windows.forms.BCheckBox();
            this.groupCommonOption = new org.kbinani.windows.forms.BGroupBox();
            this.chkMetaText = new org.kbinani.windows.forms.BCheckBox();
            this.chkPreMeasure = new org.kbinani.windows.forms.BCheckBox();
            this.chkExportVocaloidNrpn = new org.kbinani.windows.forms.BCheckBox();
            this.groupMode = new org.kbinani.windows.forms.BGroupBox();
            this.radioPlayTime = new org.kbinani.windows.forms.BRadioButton();
            this.radioGateTime = new org.kbinani.windows.forms.BRadioButton();
            this.lblOffset = new org.kbinani.windows.forms.BLabel();
            this.txtOffset = new org.kbinani.cadencii.NumberTextBox();
            this.lblOffsetUnit = new org.kbinani.windows.forms.BLabel();
            this.groupCommonOption.SuspendLayout();
            this.groupMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 261, 435 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point( 180, 435 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // listTrack
            // 
            this.listTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listTrack.CheckBoxes = true;
            this.listTrack.FullRowSelect = true;
            listViewGroup7.Header = "ListViewGroup";
            listViewGroup8.Header = "ListViewGroup";
            listViewGroup8.Name = null;
            listViewGroup9.Header = "ListViewGroup";
            listViewGroup9.Name = null;
            this.listTrack.Groups.AddRange( new System.Windows.Forms.ListViewGroup[] {
            listViewGroup7,
            listViewGroup8,
            listViewGroup9} );
            this.listTrack.Location = new System.Drawing.Point( 12, 41 );
            this.listTrack.Name = "listTrack";
            this.listTrack.Size = new System.Drawing.Size( 324, 216 );
            this.listTrack.TabIndex = 6;
            this.listTrack.UseCompatibleStateImageBehavior = false;
            this.listTrack.View = System.Windows.Forms.View.Details;
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.AutoSize = true;
            this.btnCheckAll.Location = new System.Drawing.Point( 12, 12 );
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size( 75, 23 );
            this.btnCheckAll.TabIndex = 7;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.AutoSize = true;
            this.btnUncheckAll.Location = new System.Drawing.Point( 93, 12 );
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size( 77, 23 );
            this.btnUncheckAll.TabIndex = 8;
            this.btnUncheckAll.Text = "Uncheck All";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            // 
            // chkBeat
            // 
            this.chkBeat.AutoSize = true;
            this.chkBeat.Checked = true;
            this.chkBeat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBeat.Location = new System.Drawing.Point( 81, 18 );
            this.chkBeat.Name = "chkBeat";
            this.chkBeat.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 0 );
            this.chkBeat.Size = new System.Drawing.Size( 58, 16 );
            this.chkBeat.TabIndex = 9;
            this.chkBeat.Text = "Beat";
            this.chkBeat.UseVisualStyleBackColor = true;
            // 
            // chkTempo
            // 
            this.chkTempo.AutoSize = true;
            this.chkTempo.Checked = true;
            this.chkTempo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTempo.Location = new System.Drawing.Point( 10, 18 );
            this.chkTempo.Name = "chkTempo";
            this.chkTempo.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 0 );
            this.chkTempo.Size = new System.Drawing.Size( 68, 16 );
            this.chkTempo.TabIndex = 10;
            this.chkTempo.Text = "Tempo";
            this.chkTempo.UseVisualStyleBackColor = true;
            // 
            // chkNote
            // 
            this.chkNote.AutoSize = true;
            this.chkNote.Checked = true;
            this.chkNote.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNote.Location = new System.Drawing.Point( 10, 40 );
            this.chkNote.Name = "chkNote";
            this.chkNote.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 0 );
            this.chkNote.Size = new System.Drawing.Size( 58, 16 );
            this.chkNote.TabIndex = 11;
            this.chkNote.Text = "Note";
            this.chkNote.UseVisualStyleBackColor = true;
            // 
            // chkLyric
            // 
            this.chkLyric.AutoSize = true;
            this.chkLyric.Checked = true;
            this.chkLyric.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLyric.Location = new System.Drawing.Point( 145, 18 );
            this.chkLyric.Name = "chkLyric";
            this.chkLyric.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 0 );
            this.chkLyric.Size = new System.Drawing.Size( 65, 16 );
            this.chkLyric.TabIndex = 12;
            this.chkLyric.Text = "Lyrics";
            this.chkLyric.UseVisualStyleBackColor = true;
            // 
            // groupCommonOption
            // 
            this.groupCommonOption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupCommonOption.Controls.Add( this.chkMetaText );
            this.groupCommonOption.Controls.Add( this.chkPreMeasure );
            this.groupCommonOption.Controls.Add( this.chkExportVocaloidNrpn );
            this.groupCommonOption.Controls.Add( this.chkLyric );
            this.groupCommonOption.Controls.Add( this.chkNote );
            this.groupCommonOption.Controls.Add( this.chkBeat );
            this.groupCommonOption.Controls.Add( this.chkTempo );
            this.groupCommonOption.Location = new System.Drawing.Point( 12, 263 );
            this.groupCommonOption.Name = "groupCommonOption";
            this.groupCommonOption.Size = new System.Drawing.Size( 324, 88 );
            this.groupCommonOption.TabIndex = 13;
            this.groupCommonOption.TabStop = false;
            this.groupCommonOption.Text = "Option";
            // 
            // chkMetaText
            // 
            this.chkMetaText.AutoSize = true;
            this.chkMetaText.Checked = true;
            this.chkMetaText.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMetaText.Location = new System.Drawing.Point( 74, 40 );
            this.chkMetaText.Name = "chkMetaText";
            this.chkMetaText.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 0 );
            this.chkMetaText.Size = new System.Drawing.Size( 131, 16 );
            this.chkMetaText.TabIndex = 16;
            this.chkMetaText.Text = "vocaloid meta-text";
            this.chkMetaText.UseVisualStyleBackColor = true;
            // 
            // chkPreMeasure
            // 
            this.chkPreMeasure.AutoSize = true;
            this.chkPreMeasure.Checked = true;
            this.chkPreMeasure.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreMeasure.Location = new System.Drawing.Point( 127, 62 );
            this.chkPreMeasure.Name = "chkPreMeasure";
            this.chkPreMeasure.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 0 );
            this.chkPreMeasure.Size = new System.Drawing.Size( 160, 16 );
            this.chkPreMeasure.TabIndex = 15;
            this.chkPreMeasure.Text = "Export pre-measure part";
            this.chkPreMeasure.UseVisualStyleBackColor = true;
            // 
            // chkExportVocaloidNrpn
            // 
            this.chkExportVocaloidNrpn.AutoSize = true;
            this.chkExportVocaloidNrpn.Checked = true;
            this.chkExportVocaloidNrpn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportVocaloidNrpn.Location = new System.Drawing.Point( 10, 62 );
            this.chkExportVocaloidNrpn.Name = "chkExportVocaloidNrpn";
            this.chkExportVocaloidNrpn.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 0 );
            this.chkExportVocaloidNrpn.Size = new System.Drawing.Size( 111, 16 );
            this.chkExportVocaloidNrpn.TabIndex = 14;
            this.chkExportVocaloidNrpn.Text = "vocaloid NRPN";
            this.chkExportVocaloidNrpn.UseVisualStyleBackColor = true;
            // 
            // groupMode
            // 
            this.groupMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupMode.Controls.Add( this.lblOffsetUnit );
            this.groupMode.Controls.Add( this.txtOffset );
            this.groupMode.Controls.Add( this.lblOffset );
            this.groupMode.Controls.Add( this.radioPlayTime );
            this.groupMode.Controls.Add( this.radioGateTime );
            this.groupMode.Location = new System.Drawing.Point( 12, 357 );
            this.groupMode.Name = "groupMode";
            this.groupMode.Size = new System.Drawing.Size( 324, 72 );
            this.groupMode.TabIndex = 14;
            this.groupMode.TabStop = false;
            this.groupMode.Text = "Import Basis";
            // 
            // radioPlayTime
            // 
            this.radioPlayTime.AutoSize = true;
            this.radioPlayTime.Location = new System.Drawing.Point( 168, 18 );
            this.radioPlayTime.Name = "radioPlayTime";
            this.radioPlayTime.Size = new System.Drawing.Size( 72, 16 );
            this.radioPlayTime.TabIndex = 1;
            this.radioPlayTime.TabStop = true;
            this.radioPlayTime.Text = "play-time";
            this.radioPlayTime.UseVisualStyleBackColor = true;
            // 
            // radioGateTime
            // 
            this.radioGateTime.AutoSize = true;
            this.radioGateTime.Checked = true;
            this.radioGateTime.Location = new System.Drawing.Point( 10, 18 );
            this.radioGateTime.Name = "radioGateTime";
            this.radioGateTime.Size = new System.Drawing.Size( 73, 16 );
            this.radioGateTime.TabIndex = 0;
            this.radioGateTime.TabStop = true;
            this.radioGateTime.Text = "gate-time";
            this.radioGateTime.UseVisualStyleBackColor = true;
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point( 14, 45 );
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size( 35, 12 );
            this.lblOffset.TabIndex = 2;
            this.lblOffset.Text = "offset";
            // 
            // txtOffset
            // 
            this.txtOffset.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))) );
            this.txtOffset.ForeColor = System.Drawing.Color.Black;
            this.txtOffset.Location = new System.Drawing.Point( 81, 42 );
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size( 100, 19 );
            this.txtOffset.TabIndex = 3;
            this.txtOffset.Text = "0";
            this.txtOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtOffset.Type = org.kbinani.cadencii.NumberTextBox.ValueType.Integer;
            // 
            // lblOffsetUnit
            // 
            this.lblOffsetUnit.AutoSize = true;
            this.lblOffsetUnit.Location = new System.Drawing.Point( 187, 45 );
            this.lblOffsetUnit.Name = "lblOffsetUnit";
            this.lblOffsetUnit.Size = new System.Drawing.Size( 38, 12 );
            this.lblOffsetUnit.TabIndex = 4;
            this.lblOffsetUnit.Text = "clocks";
            // 
            // FormMidiImExport
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 348, 470 );
            this.Controls.Add( this.groupMode );
            this.Controls.Add( this.groupCommonOption );
            this.Controls.Add( this.btnUncheckAll );
            this.Controls.Add( this.btnCheckAll );
            this.Controls.Add( this.listTrack );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMidiImExport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormMidiInExport";
            this.groupCommonOption.ResumeLayout( false );
            this.groupCommonOption.PerformLayout();
            this.groupMode.ResumeLayout( false );
            this.groupMode.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BButton btnCancel;
        private BButton btnOK;
        private BButton btnCheckAll;
        private BButton btnUncheckAll;
        private BCheckBox chkBeat;
        private BCheckBox chkTempo;
        private BCheckBox chkNote;
        private BCheckBox chkLyric;
        private BGroupBox groupCommonOption;
        private BCheckBox chkExportVocaloidNrpn;
        public BListView listTrack;
        private BCheckBox chkPreMeasure;
        private BCheckBox chkMetaText;
        private BGroupBox groupMode;
        private BRadioButton radioPlayTime;
        private BRadioButton radioGateTime;

        #endregion
#endif
    }

#if !JAVA
}
#endif
