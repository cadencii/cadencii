/*
 * FormMidiImExport.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Windows.Forms;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.windows.forms;

namespace cadencii
{
    public class FormMidiImExport : Form
    {
        public enum FormMidiMode
        {
            IMPORT,
            EXPORT,
            IMPORT_VSQ,
        }

        private FormMidiMode m_mode;
        private VsqFileEx m_vsq;
        private static int columnWidthTrack = 55;
        private static int columnWidthName = 217;
        private static int columnWidthNotes = 58;

        public FormMidiImExport()
        {
            InitializeComponent();
            applyLanguage();
            setMode(FormMidiMode.EXPORT);
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
            listTrack.SetColumnHeaders(new string[] { _("Track"), _("Name"), _("Notes") });
            listTrack.Columns[0].Width = columnWidthTrack;
            listTrack.Columns[1].Width = columnWidthName;
            listTrack.Columns[2].Width = columnWidthNotes;

            System.Drawing.Point p = btnCheckAll.Location;
            btnUncheckAll.Location = new System.Drawing.Point(p.X + btnCheckAll.Width + 6, p.Y);

            registerEventHandlers();
            setResources();
        }

        #region public methods
        public void applyLanguage()
        {
            if (m_mode == FormMidiMode.EXPORT) {
                this.Text = _("Midi Export");
            } else if (m_mode == FormMidiMode.IMPORT) {
                this.Text = _("Midi Import");
            } else {
                this.Text = _("VSQ/Vocaloid Midi Import");
            }
            groupMode.Text = _("Import Basis");
            radioGateTime.Text = _("gate-time");
            radioPlayTime.Text = _("play-time");
            listTrack.SetColumnHeaders(new string[] { _("Track"), _("Name"), _("Notes") });
            btnCheckAll.Text = _("Check All");
            btnUncheckAll.Text = _("Uncheck All");
            groupCommonOption.Text = _("Option");
            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
            chkTempo.Text = _("Tempo");
            chkBeat.Text = _("Beat");
            chkNote.Text = _("Note");
            chkLyric.Text = _("Lyrics");
            chkExportVocaloidNrpn.Text = _("vocaloid NRPN");
            lblOffset.Text = _("offset");
            if (radioGateTime.Checked) {
                lblOffsetUnit.Text = _("clocks");
            } else {
                lblOffsetUnit.Text = _("seconds");
            }
        }

        public double getOffsetSeconds()
        {
            double v = 0.0;
            try {
                v = double.Parse(txtOffset.Text);
            } catch (Exception ex) {
                Logger.write(typeof(FormMidiImExport) + ".getOffsetSeconds; ex=" + ex + "\n");
                serr.println("FormMidiImExport#getOffsetClocks; ex=" + ex);
            }
            return v;
        }

        public int getOffsetClocks()
        {
            int v = 0;
            try {
                v = int.Parse(txtOffset.Text);
            } catch (Exception ex) {
                Logger.write(typeof(FormMidiImExport) + ".getOffsetClocks; ex=" + ex + "\n");
                serr.println("FormMidiImExport#getOffsetClocks; ex=" + ex);
            }
            return v;
        }

        public bool isSecondBasis()
        {
            return radioPlayTime.Checked;
        }

        public FormMidiMode getMode()
        {
            return m_mode;
        }

        public void setMode(FormMidiMode value)
        {
            m_mode = value;
            chkExportVocaloidNrpn.Enabled = (m_mode == FormMidiMode.EXPORT);
            chkLyric.Enabled = (m_mode != FormMidiMode.IMPORT_VSQ);
            chkNote.Enabled = (m_mode != FormMidiMode.IMPORT_VSQ);
            chkPreMeasure.Enabled = (m_mode != FormMidiMode.IMPORT_VSQ);
            if (m_mode == FormMidiMode.EXPORT) {
                this.Text = _("Midi Export");
                chkPreMeasure.Text = _("Export pre-measure part");
                if (chkExportVocaloidNrpn.Checked) {
                    chkPreMeasure.Enabled = false;
                    AppManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus = chkPreMeasure.Checked;
                    chkPreMeasure.Checked = true;
                } else {
                    chkPreMeasure.Checked = AppManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus;
                }
                if (chkNote.Checked) {
                    chkMetaText.Enabled = false;
                    AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.Checked;
                    chkMetaText.Checked = false;
                } else {
                    chkMetaText.Checked = AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus;
                }
                groupMode.Enabled = false;
            } else if (m_mode == FormMidiMode.IMPORT) {
                this.Text = _("Midi Import");
                chkPreMeasure.Text = _("Inserting start at pre-measure");
                chkMetaText.Enabled = false;
                AppManager.editorConfig.MidiImExportConfigImport.LastMetatextCheckStatus = chkMetaText.Checked;
                chkMetaText.Checked = false;
                groupMode.Enabled = true;
            } else {
                this.Text = _("VSQ/Vocaloid Midi Import");
                chkPreMeasure.Text = _("Inserting start at pre-measure");
                chkPreMeasure.Checked = false;
                AppManager.editorConfig.MidiImExportConfigImportVsq.LastMetatextCheckStatus = chkMetaText.Checked;
                chkMetaText.Checked = true;
                groupMode.Enabled = false;
            }
        }

        public bool isVocaloidMetatext()
        {
            if (chkNote.Checked) {
                return false;
            } else {
                return chkMetaText.Checked;
            }
        }

        public bool isVocaloidNrpn()
        {
            return chkExportVocaloidNrpn.Checked;
        }

        public bool isTempo()
        {
            return chkTempo.Checked;
        }

        public void setTempo(bool value)
        {
            chkTempo.Checked = value;
        }

        public bool isTimesig()
        {
            return chkBeat.Checked;
        }

        public void setTimesig(bool value)
        {
            chkBeat.Checked = value;
        }

        public bool isNotes()
        {
            return chkNote.Checked;
        }

        public bool isLyric()
        {
            return chkLyric.Checked;
        }

        public bool isPreMeasure()
        {
            return chkPreMeasure.Checked;
        }
        #endregion

        #region helper methods
        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            btnCheckAll.Click += new EventHandler(btnCheckAll_Click);
            btnUncheckAll.Click += new EventHandler(btnUnckeckAll_Click);
            chkNote.CheckedChanged += new EventHandler(chkNote_CheckedChanged);
            chkMetaText.Click += new EventHandler(chkMetaText_Click);
            chkExportVocaloidNrpn.CheckedChanged += new EventHandler(chkExportVocaloidNrpn_CheckedChanged);
            chkExportVocaloidNrpn.CheckedChanged += new EventHandler(chkExportVocaloidNrpn_CheckedChanged);
            this.FormClosing += new FormClosingEventHandler(FormMidiImExport_FormClosing);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            radioGateTime.CheckedChanged += new EventHandler(radioGateTime_CheckedChanged);
            radioPlayTime.CheckedChanged += new EventHandler(radioPlayTime_CheckedChanged);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void btnCheckAll_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < listTrack.Items.Count; i++) {
                listTrack.Items[i].Checked = true;
            }
        }

        public void btnUnckeckAll_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < listTrack.Items.Count; i++) {
                listTrack.Items[i].Checked = false;
            }
        }

        public void chkExportVocaloidNrpn_CheckedChanged(Object sender, EventArgs e)
        {
            if (m_mode == FormMidiMode.EXPORT) {
                if (chkExportVocaloidNrpn.Checked) {
                    chkPreMeasure.Enabled = false;
                    AppManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus = chkPreMeasure.Checked;
                    chkPreMeasure.Checked = true;
                } else {
                    chkPreMeasure.Enabled = true;
                    chkPreMeasure.Checked = AppManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus;
                }
            }
        }

        public void chkNote_CheckedChanged(Object sender, EventArgs e)
        {
            if (m_mode == FormMidiMode.EXPORT) {
                if (chkNote.Checked) {
                    chkMetaText.Enabled = false;
                    AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.Checked;
                    chkMetaText.Checked = false;
                } else {
                    chkMetaText.Enabled = true;
                    chkMetaText.Checked = AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus;
                }
            }
        }

        public void chkMetaText_Click(Object sender, EventArgs e)
        {
            if (m_mode == FormMidiMode.EXPORT) {
                AppManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.Checked;
            }
        }

        public void FormMidiImExport_FormClosing(Object sender, FormClosingEventArgs e)
        {
            columnWidthTrack = listTrack.Columns[0].Width;
            columnWidthName = listTrack.Columns[1].Width;
            columnWidthNotes = listTrack.Columns[2].Width;
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public void radioGateTime_CheckedChanged(Object sender, EventArgs e)
        {
            if (radioGateTime.Checked) {
                lblOffsetUnit.Text = _("clocks");
                txtOffset.setType(NumberTextBox.ValueType.Integer);
            }
        }

        public void radioPlayTime_CheckedChanged(Object sender, EventArgs e)
        {
            if (radioPlayTime.Checked) {
                lblOffsetUnit.Text = _("seconds");
                txtOffset.setType(NumberTextBox.ValueType.Double);
            }
        }
        #endregion

        #region UI implementation
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.listTrack = new ListView();
            this.btnCheckAll = new Button();
            this.btnUncheckAll = new Button();
            this.chkBeat = new CheckBox();
            this.chkTempo = new CheckBox();
            this.chkNote = new CheckBox();
            this.chkLyric = new CheckBox();
            this.groupCommonOption = new System.Windows.Forms.GroupBox();
            this.chkMetaText = new CheckBox();
            this.chkPreMeasure = new CheckBox();
            this.chkExportVocaloidNrpn = new CheckBox();
            this.groupMode = new System.Windows.Forms.GroupBox();
            this.lblOffsetUnit = new Label();
            this.txtOffset = new cadencii.NumberTextBox();
            this.lblOffset = new Label();
            this.radioPlayTime = new RadioButton();
            this.radioGateTime = new RadioButton();
            this.groupCommonOption.SuspendLayout();
            this.groupMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(261, 435);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(180, 435);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
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
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup2.Header = "ListViewGroup";
            listViewGroup2.Name = null;
            listViewGroup3.Header = "ListViewGroup";
            listViewGroup3.Name = null;
            listViewGroup4.Header = "ListViewGroup";
            listViewGroup4.Name = null;
            this.listTrack.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4});
            this.listTrack.Location = new System.Drawing.Point(12, 41);
            this.listTrack.Name = "listTrack";
            this.listTrack.Size = new System.Drawing.Size(324, 216);
            this.listTrack.TabIndex = 6;
            this.listTrack.UseCompatibleStateImageBehavior = false;
            this.listTrack.View = System.Windows.Forms.View.Details;
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.AutoSize = true;
            this.btnCheckAll.Location = new System.Drawing.Point(12, 12);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnCheckAll.TabIndex = 7;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.AutoSize = true;
            this.btnUncheckAll.Location = new System.Drawing.Point(93, 12);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(77, 23);
            this.btnUncheckAll.TabIndex = 8;
            this.btnUncheckAll.Text = "Uncheck All";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            // 
            // chkBeat
            // 
            this.chkBeat.AutoSize = true;
            this.chkBeat.Checked = true;
            this.chkBeat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBeat.Location = new System.Drawing.Point(81, 18);
            this.chkBeat.Name = "chkBeat";
            this.chkBeat.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.chkBeat.Size = new System.Drawing.Size(58, 16);
            this.chkBeat.TabIndex = 9;
            this.chkBeat.Text = "Beat";
            this.chkBeat.UseVisualStyleBackColor = true;
            // 
            // chkTempo
            // 
            this.chkTempo.AutoSize = true;
            this.chkTempo.Checked = true;
            this.chkTempo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTempo.Location = new System.Drawing.Point(10, 18);
            this.chkTempo.Name = "chkTempo";
            this.chkTempo.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.chkTempo.Size = new System.Drawing.Size(68, 16);
            this.chkTempo.TabIndex = 10;
            this.chkTempo.Text = "Tempo";
            this.chkTempo.UseVisualStyleBackColor = true;
            // 
            // chkNote
            // 
            this.chkNote.AutoSize = true;
            this.chkNote.Checked = true;
            this.chkNote.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNote.Location = new System.Drawing.Point(10, 40);
            this.chkNote.Name = "chkNote";
            this.chkNote.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.chkNote.Size = new System.Drawing.Size(58, 16);
            this.chkNote.TabIndex = 11;
            this.chkNote.Text = "Note";
            this.chkNote.UseVisualStyleBackColor = true;
            // 
            // chkLyric
            // 
            this.chkLyric.AutoSize = true;
            this.chkLyric.Checked = true;
            this.chkLyric.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLyric.Location = new System.Drawing.Point(145, 18);
            this.chkLyric.Name = "chkLyric";
            this.chkLyric.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.chkLyric.Size = new System.Drawing.Size(65, 16);
            this.chkLyric.TabIndex = 12;
            this.chkLyric.Text = "Lyrics";
            this.chkLyric.UseVisualStyleBackColor = true;
            // 
            // groupCommonOption
            // 
            this.groupCommonOption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupCommonOption.Controls.Add(this.chkMetaText);
            this.groupCommonOption.Controls.Add(this.chkPreMeasure);
            this.groupCommonOption.Controls.Add(this.chkExportVocaloidNrpn);
            this.groupCommonOption.Controls.Add(this.chkLyric);
            this.groupCommonOption.Controls.Add(this.chkNote);
            this.groupCommonOption.Controls.Add(this.chkBeat);
            this.groupCommonOption.Controls.Add(this.chkTempo);
            this.groupCommonOption.Location = new System.Drawing.Point(12, 263);
            this.groupCommonOption.Name = "groupCommonOption";
            this.groupCommonOption.Size = new System.Drawing.Size(324, 88);
            this.groupCommonOption.TabIndex = 13;
            this.groupCommonOption.TabStop = false;
            this.groupCommonOption.Text = "Option";
            // 
            // chkMetaText
            // 
            this.chkMetaText.AutoSize = true;
            this.chkMetaText.Checked = true;
            this.chkMetaText.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMetaText.Location = new System.Drawing.Point(74, 40);
            this.chkMetaText.Name = "chkMetaText";
            this.chkMetaText.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.chkMetaText.Size = new System.Drawing.Size(131, 16);
            this.chkMetaText.TabIndex = 16;
            this.chkMetaText.Text = "vocaloid meta-text";
            this.chkMetaText.UseVisualStyleBackColor = true;
            // 
            // chkPreMeasure
            // 
            this.chkPreMeasure.AutoSize = true;
            this.chkPreMeasure.Checked = true;
            this.chkPreMeasure.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreMeasure.Location = new System.Drawing.Point(127, 62);
            this.chkPreMeasure.Name = "chkPreMeasure";
            this.chkPreMeasure.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.chkPreMeasure.Size = new System.Drawing.Size(160, 16);
            this.chkPreMeasure.TabIndex = 15;
            this.chkPreMeasure.Text = "Export pre-measure part";
            this.chkPreMeasure.UseVisualStyleBackColor = true;
            // 
            // chkExportVocaloidNrpn
            // 
            this.chkExportVocaloidNrpn.AutoSize = true;
            this.chkExportVocaloidNrpn.Checked = true;
            this.chkExportVocaloidNrpn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportVocaloidNrpn.Location = new System.Drawing.Point(10, 62);
            this.chkExportVocaloidNrpn.Name = "chkExportVocaloidNrpn";
            this.chkExportVocaloidNrpn.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.chkExportVocaloidNrpn.Size = new System.Drawing.Size(111, 16);
            this.chkExportVocaloidNrpn.TabIndex = 14;
            this.chkExportVocaloidNrpn.Text = "vocaloid NRPN";
            this.chkExportVocaloidNrpn.UseVisualStyleBackColor = true;
            // 
            // groupMode
            // 
            this.groupMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupMode.Controls.Add(this.lblOffsetUnit);
            this.groupMode.Controls.Add(this.txtOffset);
            this.groupMode.Controls.Add(this.lblOffset);
            this.groupMode.Controls.Add(this.radioPlayTime);
            this.groupMode.Controls.Add(this.radioGateTime);
            this.groupMode.Location = new System.Drawing.Point(12, 357);
            this.groupMode.Name = "groupMode";
            this.groupMode.Size = new System.Drawing.Size(324, 72);
            this.groupMode.TabIndex = 14;
            this.groupMode.TabStop = false;
            this.groupMode.Text = "Import Basis";
            // 
            // lblOffsetUnit
            // 
            this.lblOffsetUnit.AutoSize = true;
            this.lblOffsetUnit.Location = new System.Drawing.Point(187, 45);
            this.lblOffsetUnit.Name = "lblOffsetUnit";
            this.lblOffsetUnit.Size = new System.Drawing.Size(38, 12);
            this.lblOffsetUnit.TabIndex = 4;
            this.lblOffsetUnit.Text = "clocks";
            // 
            // txtOffset
            // 
            this.txtOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtOffset.ForeColor = System.Drawing.Color.Black;
            this.txtOffset.Location = new System.Drawing.Point(81, 42);
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(100, 19);
            this.txtOffset.TabIndex = 3;
            this.txtOffset.Text = "0";
            this.txtOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtOffset.Type = cadencii.NumberTextBox.ValueType.Integer;
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point(14, 45);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(35, 12);
            this.lblOffset.TabIndex = 2;
            this.lblOffset.Text = "offset";
            // 
            // radioPlayTime
            // 
            this.radioPlayTime.AutoSize = true;
            this.radioPlayTime.Location = new System.Drawing.Point(168, 18);
            this.radioPlayTime.Name = "radioPlayTime";
            this.radioPlayTime.Size = new System.Drawing.Size(72, 16);
            this.radioPlayTime.TabIndex = 1;
            this.radioPlayTime.TabStop = true;
            this.radioPlayTime.Text = "play-time";
            this.radioPlayTime.UseVisualStyleBackColor = true;
            // 
            // radioGateTime
            // 
            this.radioGateTime.AutoSize = true;
            this.radioGateTime.Checked = true;
            this.radioGateTime.Location = new System.Drawing.Point(10, 18);
            this.radioGateTime.Name = "radioGateTime";
            this.radioGateTime.Size = new System.Drawing.Size(73, 16);
            this.radioGateTime.TabIndex = 0;
            this.radioGateTime.TabStop = true;
            this.radioGateTime.Text = "gate-time";
            this.radioGateTime.UseVisualStyleBackColor = true;
            // 
            // FormMidiImExport
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(348, 470);
            this.Controls.Add(this.groupMode);
            this.Controls.Add(this.groupCommonOption);
            this.Controls.Add(this.btnUncheckAll);
            this.Controls.Add(this.btnCheckAll);
            this.Controls.Add(this.listTrack);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMidiImExport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormMidiInExport";
            this.groupCommonOption.ResumeLayout(false);
            this.groupCommonOption.PerformLayout();
            this.groupMode.ResumeLayout(false);
            this.groupMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Button btnUncheckAll;
        private CheckBox chkBeat;
        private CheckBox chkTempo;
        private CheckBox chkNote;
        private CheckBox chkLyric;
        private GroupBox groupCommonOption;
        private CheckBox chkExportVocaloidNrpn;
        public ListView listTrack;
        private CheckBox chkPreMeasure;
        private CheckBox chkMetaText;
        private GroupBox groupMode;
        private RadioButton radioPlayTime;
        private RadioButton radioGateTime;
        private Label lblOffset;
        private NumberTextBox txtOffset;
        private Label lblOffsetUnit;

        #endregion

    }

}
