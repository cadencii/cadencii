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
#if JAVA
package org.kbinani.Cadencii;

import java.awt.*;
import java.util.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using Boare.Lib.AppUtil;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
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

        private void btnCheckAll_Click( Object sender, BEventArgs e ) {
            for ( int i = 0; i < ListTrack.Items.Count; i++ ) {
                ListTrack.Items[i].Checked = true;
            }
        }

        private void btnUnckeckAll_Click( Object sender, BEventArgs e ) {
            for ( int i = 0; i < ListTrack.Items.Count; i++ ) {
                ListTrack.Items[i].Checked = false;
            }
        }

        private void chkExportVocaloidNrpn_CheckedChanged( Object sender, BEventArgs e ) {
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

        private void chkNote_CheckedChanged( Object sender, BEventArgs e ) {
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

        private void chkMetaText_Click( Object sender, BEventArgs e ) {
            if ( m_mode == FormMidiMode.EXPORT ) {
                AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.Checked;
            }
        }
#if JAVA
        #region UI Impl for Java
	    private JPanel jContentPane = null;
	    private JPanel jPanel = null;
	    private JButton btnCheckAll = null;
	    private JButton btnUncheckAll = null;
	    private JLabel jLabel = null;
	    private JTable jTable = null;
	    private JPanel jPanel1 = null;
	    private JCheckBox chkTempo = null;
	    private JPanel jPanel2 = null;
	    private JButton btnOK = null;
	    private JButton btnCancel = null;
	    private JPanel jPanel3 = null;
	    private JCheckBox chkBeat = null;
	    private JCheckBox chkLyric = null;
	    private JLabel jLabel1 = null;
	    private JPanel chkMetaText = null;
	    private JCheckBox chkNote = null;
	    private JCheckBox jCheckBox11 = null;
	    private JLabel jLabel11 = null;
	    private JPanel jPanel32 = null;
	    private JCheckBox chkExportVocaloidNrpn = null;
	    private JCheckBox chkPreMeasure = null;
	    private JLabel jLabel12 = null;
	    /**
	     * This is the default constructor
	     */
	    public FormMidiImExport() {
		    super();
		    initialize();
	    }

	    /**
	     * This method initializes this
	     * 
	     * @return void
	     */
	    private void initialize() {
		    this.setSize(357, 488);
		    this.setContentPane(getJContentPane());
		    this.setTitle("JFrame");
	    }

	    /**
	     * This method initializes jContentPane
	     * 
	     * @return javax.swing.JPanel
	     */
	    private JPanel getJContentPane() {
		    if (jContentPane == null) {
			    GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			    gridBagConstraints13.gridx = 0;
			    gridBagConstraints13.anchor = GridBagConstraints.EAST;
			    gridBagConstraints13.insets = new Insets(12, 0, 12, 0);
			    gridBagConstraints13.gridy = 3;
			    GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			    gridBagConstraints12.gridx = 0;
			    gridBagConstraints12.weightx = 1.0D;
			    gridBagConstraints12.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints12.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints12.gridy = 2;
			    GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			    gridBagConstraints4.fill = GridBagConstraints.BOTH;
			    gridBagConstraints4.gridy = 1;
			    gridBagConstraints4.weightx = 1.0;
			    gridBagConstraints4.weighty = 1.0;
			    gridBagConstraints4.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints4.gridx = 0;
			    GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			    gridBagConstraints3.insets = new Insets(12, 12, 6, 12);
			    gridBagConstraints3.gridy = 0;
			    gridBagConstraints3.ipadx = 146;
			    gridBagConstraints3.gridx = 0;
			    jContentPane = new JPanel();
			    jContentPane.setLayout(new GridBagLayout());
			    jContentPane.add(getJPanel(), gridBagConstraints3);
			    jContentPane.add(getJTable(), gridBagConstraints4);
			    jContentPane.add(getJPanel1(), gridBagConstraints12);
			    jContentPane.add(getJPanel2(), gridBagConstraints13);
		    }
		    return jContentPane;
	    }

	    /**
	     * This method initializes jPanel	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel() {
		    if (jPanel == null) {
			    GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			    gridBagConstraints2.gridx = 2;
			    gridBagConstraints2.weightx = 1.0D;
			    gridBagConstraints2.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints2.insets = new Insets(0, 0, 0, 0);
			    gridBagConstraints2.gridy = 0;
			    jLabel = new JLabel();
			    jLabel.setText(" ");
			    GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			    gridBagConstraints1.gridx = 1;
			    gridBagConstraints1.insets = new Insets(3, 3, 3, 3);
			    gridBagConstraints1.gridy = 0;
			    GridBagConstraints gridBagConstraints = new GridBagConstraints();
			    gridBagConstraints.gridx = 0;
			    gridBagConstraints.weighty = 0.0D;
			    gridBagConstraints.fill = GridBagConstraints.NONE;
			    gridBagConstraints.insets = new Insets(3, 3, 3, 3);
			    gridBagConstraints.gridy = 0;
			    jPanel = new JPanel();
			    jPanel.setLayout(new GridBagLayout());
			    jPanel.add(getBtnCheckAll(), gridBagConstraints);
			    jPanel.add(getBtnUncheckAll(), gridBagConstraints1);
			    jPanel.add(jLabel, gridBagConstraints2);
		    }
		    return jPanel;
	    }

	    /**
	     * This method initializes btnCheckAll	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnCheckAll() {
		    if (btnCheckAll == null) {
			    btnCheckAll = new JButton();
			    btnCheckAll.setText("Check All");
			    btnCheckAll.setName("btnCheckAll");
			    btnCheckAll.setPreferredSize(new Dimension(87, 23));
		    }
		    return btnCheckAll;
	    }

	    /**
	     * This method initializes btnUncheckAll	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnUncheckAll() {
		    if (btnUncheckAll == null) {
			    btnUncheckAll = new JButton();
			    btnUncheckAll.setText("Uncheck All");
			    btnUncheckAll.setName("btnUncheckAll");
			    btnUncheckAll.setPreferredSize(new Dimension(101, 23));
		    }
		    return btnUncheckAll;
	    }

	    /**
	     * This method initializes jTable	
	     * 	
	     * @return javax.swing.JTable	
	     */
	    private JTable getJTable() {
		    if (jTable == null) {
			    jTable = new JTable();
		    }
		    return jTable;
	    }

	    /**
	     * This method initializes jPanel1	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel1() {
		    if (jPanel1 == null) {
			    GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			    gridBagConstraints11.gridx = 0;
			    gridBagConstraints11.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints11.weightx = 1.0D;
			    gridBagConstraints11.anchor = GridBagConstraints.WEST;
			    gridBagConstraints11.gridy = 2;
			    GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			    gridBagConstraints10.gridx = 0;
			    gridBagConstraints10.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints10.weightx = 1.0D;
			    gridBagConstraints10.anchor = GridBagConstraints.WEST;
			    gridBagConstraints10.gridy = 1;
			    GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			    gridBagConstraints9.gridx = 0;
			    gridBagConstraints9.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints9.weightx = 1.0D;
			    gridBagConstraints9.gridy = 0;
			    jPanel1 = new JPanel();
			    jPanel1.setLayout(new GridBagLayout());
			    jPanel1.setBorder(BorderFactory.createTitledBorder(null, "Option", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			    jPanel1.add(getJPanel3(), gridBagConstraints9);
			    jPanel1.add(getChkMetaText(), gridBagConstraints10);
			    jPanel1.add(getJPanel32(), gridBagConstraints11);
		    }
		    return jPanel1;
	    }

	    /**
	     * This method initializes chkTempo	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkTempo() {
		    if (chkTempo == null) {
			    chkTempo = new JCheckBox();
			    chkTempo.setText("Tempo");
			    chkTempo.setName("chkTempo");
		    }
		    return chkTempo;
	    }

	    /**
	     * This method initializes jPanel2	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel2() {
		    if (jPanel2 == null) {
			    GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			    gridBagConstraints31.insets = new Insets(0, 0, 0, 12);
			    gridBagConstraints31.gridy = 0;
			    gridBagConstraints31.gridx = 1;
			    GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
			    gridBagConstraints21.insets = new Insets(0, 0, 0, 16);
			    gridBagConstraints21.gridy = 0;
			    gridBagConstraints21.gridx = 0;
			    jPanel2 = new JPanel();
			    jPanel2.setLayout(new GridBagLayout());
			    jPanel2.add(getBtnOK(), gridBagConstraints21);
			    jPanel2.add(getBtnCancel(), gridBagConstraints31);
		    }
		    return jPanel2;
	    }

	    /**
	     * This method initializes btnOK	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnOK() {
		    if (btnOK == null) {
			    btnOK = new JButton();
			    btnOK.setText("OK");
		    }
		    return btnOK;
	    }

	    /**
	     * This method initializes btnCancel	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnCancel() {
		    if (btnCancel == null) {
			    btnCancel = new JButton();
			    btnCancel.setText("Cancel");
		    }
		    return btnCancel;
	    }

	    /**
	     * This method initializes jPanel3	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel3() {
		    if (jPanel3 == null) {
			    GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			    gridBagConstraints8.gridx = 3;
			    gridBagConstraints8.weightx = 1.0D;
			    gridBagConstraints8.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints8.gridy = 0;
			    jLabel1 = new JLabel();
			    jLabel1.setText(" ");
			    GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			    gridBagConstraints7.gridx = 2;
			    gridBagConstraints7.gridy = 0;
			    GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			    gridBagConstraints6.gridx = 1;
			    gridBagConstraints6.gridy = 0;
			    GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			    gridBagConstraints5.gridx = 0;
			    gridBagConstraints5.gridy = 0;
			    jPanel3 = new JPanel();
			    jPanel3.setLayout(new GridBagLayout());
			    jPanel3.add(getChkTempo(), gridBagConstraints5);
			    jPanel3.add(getChkBeat(), gridBagConstraints6);
			    jPanel3.add(getChkLyric(), gridBagConstraints7);
			    jPanel3.add(jLabel1, gridBagConstraints8);
		    }
		    return jPanel3;
	    }

	    /**
	     * This method initializes chkBeat	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkBeat() {
		    if (chkBeat == null) {
			    chkBeat = new JCheckBox();
			    chkBeat.setText("Beat");
		    }
		    return chkBeat;
	    }

	    /**
	     * This method initializes chkLyric	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkLyric() {
		    if (chkLyric == null) {
			    chkLyric = new JCheckBox();
			    chkLyric.setText("Lyrics");
		    }
		    return chkLyric;
	    }

	    /**
	     * This method initializes chkMetaText	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getChkMetaText() {
		    if (chkMetaText == null) {
			    GridBagConstraints gridBagConstraints81 = new GridBagConstraints();
			    gridBagConstraints81.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints81.gridy = 0;
			    gridBagConstraints81.weightx = 1.0D;
			    gridBagConstraints81.gridx = 3;
			    jLabel11 = new JLabel();
			    jLabel11.setText(" ");
			    GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
			    gridBagConstraints61.gridx = 1;
			    gridBagConstraints61.gridy = 0;
			    GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
			    gridBagConstraints51.gridx = 0;
			    gridBagConstraints51.gridy = 0;
			    chkMetaText = new JPanel();
			    chkMetaText.setLayout(new GridBagLayout());
			    chkMetaText.add(getChkNote(), gridBagConstraints51);
			    chkMetaText.add(getJCheckBox11(), gridBagConstraints61);
			    chkMetaText.add(jLabel11, gridBagConstraints81);
		    }
		    return chkMetaText;
	    }

	    /**
	     * This method initializes chkNote	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkNote() {
		    if (chkNote == null) {
			    chkNote = new JCheckBox();
			    chkNote.setText("Note");
		    }
		    return chkNote;
	    }

	    /**
	     * This method initializes jCheckBox11	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getJCheckBox11() {
		    if (jCheckBox11 == null) {
			    jCheckBox11 = new JCheckBox();
			    jCheckBox11.setText("vocaloid meta-text");
		    }
		    return jCheckBox11;
	    }

	    /**
	     * This method initializes jPanel32	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel32() {
		    if (jPanel32 == null) {
			    GridBagConstraints gridBagConstraints82 = new GridBagConstraints();
			    gridBagConstraints82.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints82.gridy = 0;
			    gridBagConstraints82.weightx = 1.0D;
			    gridBagConstraints82.gridx = 3;
			    jLabel12 = new JLabel();
			    jLabel12.setText(" ");
			    GridBagConstraints gridBagConstraints62 = new GridBagConstraints();
			    gridBagConstraints62.gridx = 1;
			    gridBagConstraints62.gridy = 0;
			    GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
			    gridBagConstraints52.gridx = 0;
			    gridBagConstraints52.gridy = 0;
			    jPanel32 = new JPanel();
			    jPanel32.setLayout(new GridBagLayout());
			    jPanel32.add(getChkExportVocaloidNrpn(), gridBagConstraints52);
			    jPanel32.add(getChkPreMeasure(), gridBagConstraints62);
			    jPanel32.add(jLabel12, gridBagConstraints82);
		    }
		    return jPanel32;
	    }

	    /**
	     * This method initializes chkExportVocaloidNrpn	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkExportVocaloidNrpn() {
		    if (chkExportVocaloidNrpn == null) {
			    chkExportVocaloidNrpn = new JCheckBox();
			    chkExportVocaloidNrpn.setText("vocaloid NRPN");
		    }
		    return chkExportVocaloidNrpn;
	    }

	    /**
	     * This method initializes chkPreMeasure	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkPreMeasure() {
		    if (chkPreMeasure == null) {
			    chkPreMeasure = new JCheckBox();
			    chkPreMeasure.setText("Export pre-measure part");
		    }
		    return chkPreMeasure;
	    }

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
            this.btnCancel = new BButton();
            this.btnOK = new BButton();
            this.ListTrack = new System.Windows.Forms.ListView();
            this.columnTrack = new System.Windows.Forms.ColumnHeader();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnNumNotes = new System.Windows.Forms.ColumnHeader();
            this.btnCheckAll = new BButton();
            this.btnUnckeckAll = new BButton();
            this.chkBeat = new BCheckBox();
            this.chkTempo = new BCheckBox();
            this.chkNote = new BCheckBox();
            this.chkLyric = new BCheckBox();
            this.groupCommonOption = new System.Windows.Forms.GroupBox();
            this.chkMetaText = new BCheckBox();
            this.chkPreMeasure = new BCheckBox();
            this.chkExportVocaloidNrpn = new BCheckBox();
            this.groupCommonOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 261, 423 );
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
            this.btnOK.Location = new System.Drawing.Point( 180, 423 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // ListTrack
            // 
            this.ListTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ListTrack.CheckBoxes = true;
            this.ListTrack.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnTrack,
            this.columnName,
            this.columnNumNotes} );
            this.ListTrack.FullRowSelect = true;
            this.ListTrack.Location = new System.Drawing.Point( 12, 41 );
            this.ListTrack.Name = "ListTrack";
            this.ListTrack.Size = new System.Drawing.Size( 324, 282 );
            this.ListTrack.TabIndex = 6;
            this.ListTrack.UseCompatibleStateImageBehavior = false;
            this.ListTrack.View = System.Windows.Forms.View.Details;
            // 
            // columnTrack
            // 
            this.columnTrack.Text = "track";
            this.columnTrack.Width = 54;
            // 
            // columnName
            // 
            this.columnName.Text = "name";
            this.columnName.Width = 122;
            // 
            // columnNumNotes
            // 
            this.columnNumNotes.Text = "notes";
            this.columnNumNotes.Width = 126;
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
            this.btnCheckAll.Click += new System.EventHandler( this.btnCheckAll_Click );
            // 
            // btnUnckeckAll
            // 
            this.btnUnckeckAll.AutoSize = true;
            this.btnUnckeckAll.Location = new System.Drawing.Point( 93, 12 );
            this.btnUnckeckAll.Name = "btnUnckeckAll";
            this.btnUnckeckAll.Size = new System.Drawing.Size( 77, 23 );
            this.btnUnckeckAll.TabIndex = 8;
            this.btnUnckeckAll.Text = "Uncheck All";
            this.btnUnckeckAll.UseVisualStyleBackColor = true;
            this.btnUnckeckAll.Click += new System.EventHandler( this.btnUnckeckAll_Click );
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
            this.chkNote.CheckedChanged += new System.EventHandler( this.chkNote_CheckedChanged );
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
            this.groupCommonOption.Location = new System.Drawing.Point( 12, 329 );
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
            this.chkMetaText.Click += new System.EventHandler( this.chkMetaText_Click );
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
            this.chkExportVocaloidNrpn.CheckedChanged += new System.EventHandler( this.chkExportVocaloidNrpn_CheckedChanged );
            // 
            // FormMidiImExport
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 348, 458 );
            this.Controls.Add( this.groupCommonOption );
            this.Controls.Add( this.btnUnckeckAll );
            this.Controls.Add( this.btnCheckAll );
            this.Controls.Add( this.ListTrack );
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
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BButton btnCancel;
        private BButton btnOK;
        private System.Windows.Forms.ColumnHeader columnTrack;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnNumNotes;
        private BButton btnCheckAll;
        private BButton btnUnckeckAll;
        private BCheckBox chkBeat;
        private BCheckBox chkTempo;
        private BCheckBox chkNote;
        private BCheckBox chkLyric;
        private System.Windows.Forms.GroupBox groupCommonOption;
        private BCheckBox chkExportVocaloidNrpn;
        public System.Windows.Forms.ListView ListTrack;
        private BCheckBox chkPreMeasure;
        private BCheckBox chkMetaText;

        #endregion
#endif
    }

#if !JAVA
}
#endif
