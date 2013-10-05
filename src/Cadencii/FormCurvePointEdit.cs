/*
 * FormCurvePointEdit.cs
 * Copyright © 2008-2011 kbinani
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
using System.Collections.Generic;
using cadencii.apputil;
using cadencii.vsq;
using cadencii;
using cadencii.windows.forms;
using cadencii.java.util;

namespace cadencii
{

    public class FormCurvePointEdit : Form
    {
        private long m_editing_id = -1;
        private CurveType m_curve;
        private bool m_changed = false;
        private FormMain mMainWindow = null;

        public FormCurvePointEdit(FormMain main_window, long editing_id, CurveType curve)
        {
            InitializeComponent();
            mMainWindow = main_window;
            registerEventHandlers();
            setResources();
            applyLanguage();
            m_editing_id = editing_id;
            m_curve = curve;

            VsqBPPairSearchContext context = AppManager.getVsqFile().Track[AppManager.getSelected()].getCurve(m_curve.getName()).findElement(m_editing_id);
            txtDataPointClock.Text = context.clock + "";
            txtDataPointValue.Text = context.point.value + "";
            txtDataPointValue.SelectAll();

            btnUndo.Enabled = AppManager.editHistory.hasUndoHistory();
            btnRedo.Enabled = AppManager.editHistory.hasRedoHistory();
        }

        #region public methods
        public void applyLanguage()
        {
            this.Text = _("Edit Value");
            lblDataPointClock.Text = _("Clock");
            lblDataPointValue.Text = _("Value");
            btnApply.Text = _("Apply");
            btnExit.Text = _("Exit");
        }
        #endregion

        #region helper methods
        private string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void applyValue(bool mode_clock)
        {
            if (!m_changed) {
                return;
            }
            int value = m_curve.getDefault();
            try {
                value = int.Parse(txtDataPointValue.Text);
            } catch (Exception ex) {
                Logger.write(typeof(FormCurvePointEdit) + ".applyValue; ex=" + ex + "\n");
                return;
            }
            if (value < m_curve.getMinimum()) {
                value = m_curve.getMinimum();
            } else if (m_curve.getMaximum() < value) {
                value = m_curve.getMaximum();
            }

            int clock = 0;
            try {
                clock = int.Parse(txtDataPointClock.Text);
            } catch (Exception ex) {
                Logger.write(typeof(FormCurvePointEdit) + ".applyValue; ex=" + ex + "\n");
                return;
            }

            int selected = AppManager.getSelected();
            VsqTrack vsq_track = AppManager.getVsqFile().Track[selected];
            VsqBPList src = vsq_track.getCurve(m_curve.getName());
            VsqBPList list = (VsqBPList)src.clone();

            VsqBPPairSearchContext context = list.findElement(m_editing_id);
            list.move(context.clock, clock, value);
            CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandTrackCurveReplace(selected,
                                                                                                    m_curve.getName(),
                                                                                                    list));
            EditedZone zone = new EditedZone();
            Utility.compareList(zone, new VsqBPListComparisonContext(list, src));
            List<EditedZoneUnit> zoneUnits = new List<EditedZoneUnit>();
            foreach (var item in zone.iterator()) {
                zoneUnits.Add(item);
            }
            AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));

            txtDataPointClock.Text = clock + "";
            txtDataPointValue.Text = value + "";

            if (mMainWindow != null) {
                mMainWindow.setEdited(true);
                mMainWindow.ensureVisible(clock);
                mMainWindow.refreshScreen();
            }

            if (mode_clock) {
                txtDataPointClock.SelectAll();
            } else {
                txtDataPointValue.SelectAll();
            }

            btnUndo.Enabled = AppManager.editHistory.hasUndoHistory();
            btnRedo.Enabled = AppManager.editHistory.hasRedoHistory();
            m_changed = false;
        }


        private void setResources()
        {
        }

        private void registerEventHandlers()
        {
            btnForward.Click += new EventHandler(commonButton_Click);
            btnBackward.Click += new EventHandler(commonButton_Click);
            btnBackward2.Click += new EventHandler(commonButton_Click);
            btnForward2.Click += new EventHandler(commonButton_Click);
            btnApply.Click += new EventHandler(btnApply_Click);
            txtDataPointClock.TextChanged += new EventHandler(commonTextBox_TextChanged);
            txtDataPointClock.KeyUp += new KeyEventHandler(commonTextBox_KeyUp);
            txtDataPointValue.TextChanged += new EventHandler(commonTextBox_TextChanged);
            txtDataPointValue.KeyUp += new KeyEventHandler(commonTextBox_KeyUp);
            btnBackward3.Click += new EventHandler(commonButton_Click);
            btnForward3.Click += new EventHandler(commonButton_Click);
            btnUndo.Click += new EventHandler(handleUndoRedo_Click);
            btnRedo.Click += new EventHandler(handleUndoRedo_Click);
            btnExit.Click += new EventHandler(btnExit_Click);
        }
        #endregion

        #region event handlers
        public void commonTextBox_KeyUp(Object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & System.Windows.Forms.Keys.Enter) != System.Windows.Forms.Keys.Enter) {
                return;
            }
            applyValue((sender == txtDataPointClock));
        }

        public void commonButton_Click(Object sender, EventArgs e)
        {
            VsqBPList list = AppManager.getVsqFile().Track[AppManager.getSelected()].getCurve(m_curve.getName());
            VsqBPPairSearchContext search = list.findElement(m_editing_id);
            int index = search.index;
            if (sender == btnForward) {
                index++;
            } else if (sender == btnBackward) {
                index--;
            } else if (sender == btnBackward2) {
                index -= 5;
            } else if (sender == btnForward2) {
                index += 5;
            } else if (sender == btnForward3) {
                index += 10;
            } else if (sender == btnBackward3) {
                index -= 10;
            }

            if (index < 0) {
                index = 0;
            }

            if (list.size() <= index) {
                index = list.size() - 1;
            }

            VsqBPPair bp = list.getElementB(index);
            m_editing_id = bp.id;
            int clock = list.getKeyClock(index);
            txtDataPointClock.TextChanged -= commonTextBox_TextChanged;
            txtDataPointValue.TextChanged -= commonTextBox_TextChanged;
            txtDataPointClock.Text = clock + "";
            txtDataPointValue.Text = bp.value + "";
            txtDataPointClock.TextChanged += commonTextBox_TextChanged;
            txtDataPointValue.TextChanged += commonTextBox_TextChanged;

            txtDataPointValue.Focus();
            txtDataPointValue.SelectAll();

            AppManager.itemSelection.clearPoint();
            AppManager.itemSelection.addPoint(m_curve, bp.id);
            if (mMainWindow != null) {
                mMainWindow.ensureVisible(clock);
                mMainWindow.refreshScreen();
            }
        }

        public void btnApply_Click(Object sender, EventArgs e)
        {
            applyValue(true);
        }

        public void commonTextBox_TextChanged(Object sender, EventArgs e)
        {
            m_changed = true;
        }

        public void handleUndoRedo_Click(Object sender, EventArgs e)
        {
            if (sender == btnUndo) {
                AppManager.undo();
            } else if (sender == btnRedo) {
                AppManager.redo();
            } else {
                return;
            }
            VsqFileEx vsq = AppManager.getVsqFile();
            bool exists = false;
            if (vsq != null) {
                exists = vsq.Track[AppManager.getSelected()].getCurve(m_curve.getName()).findElement(m_editing_id).index >= 0;
            }
#if DEBUG
            sout.println("FormCurvePointEdit#handleUndoRedo_Click; exists=" + exists);
#endif
            txtDataPointClock.Enabled = exists;
            txtDataPointValue.Enabled = exists;
            btnApply.Enabled = exists;
            btnBackward.Enabled = exists;
            btnBackward2.Enabled = exists;
            btnBackward3.Enabled = exists;
            btnForward.Enabled = exists;
            btnForward2.Enabled = exists;
            btnForward3.Enabled = exists;

            if (exists) {
                AppManager.itemSelection.clearPoint();
                AppManager.itemSelection.addPoint(m_curve, m_editing_id);
            }

            if (mMainWindow != null) {
                mMainWindow.updateDrawObjectList();
                mMainWindow.refreshScreen();
            }
            btnUndo.Enabled = AppManager.editHistory.hasUndoHistory();
            btnRedo.Enabled = AppManager.editHistory.hasRedoHistory();
        }

        public void btnExit_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
            this.btnForward = new Button();
            this.btnBackward = new Button();
            this.lblDataPointValue = new Label();
            this.lblDataPointClock = new Label();
            this.btnExit = new Button();
            this.btnBackward2 = new Button();
            this.btnForward2 = new Button();
            this.btnApply = new Button();
            this.txtDataPointClock = new cadencii.NumberTextBox();
            this.txtDataPointValue = new cadencii.NumberTextBox();
            this.btnBackward3 = new Button();
            this.btnForward3 = new Button();
            this.btnUndo = new Button();
            this.btnRedo = new Button();
            this.SuspendLayout();
            // 
            // btnForward
            // 
            this.btnForward.Location = new System.Drawing.Point(120, 12);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(35, 30);
            this.btnForward.TabIndex = 6;
            this.btnForward.Text = ">";
            this.btnForward.UseVisualStyleBackColor = true;
            // 
            // btnBackward
            // 
            this.btnBackward.Location = new System.Drawing.Point(84, 12);
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size(35, 30);
            this.btnBackward.TabIndex = 5;
            this.btnBackward.Text = "<";
            this.btnBackward.UseVisualStyleBackColor = true;
            // 
            // lblDataPointValue
            // 
            this.lblDataPointValue.AutoSize = true;
            this.lblDataPointValue.Location = new System.Drawing.Point(49, 55);
            this.lblDataPointValue.Name = "lblDataPointValue";
            this.lblDataPointValue.Size = new System.Drawing.Size(34, 12);
            this.lblDataPointValue.TabIndex = 16;
            this.lblDataPointValue.Text = "Value";
            // 
            // lblDataPointClock
            // 
            this.lblDataPointClock.AutoSize = true;
            this.lblDataPointClock.Location = new System.Drawing.Point(49, 80);
            this.lblDataPointClock.Name = "lblDataPointClock";
            this.lblDataPointClock.Size = new System.Drawing.Size(34, 12);
            this.lblDataPointClock.TabIndex = 15;
            this.lblDataPointClock.Text = "Clock";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(152, 109);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnBackward2
            // 
            this.btnBackward2.Location = new System.Drawing.Point(48, 12);
            this.btnBackward2.Name = "btnBackward2";
            this.btnBackward2.Size = new System.Drawing.Size(35, 30);
            this.btnBackward2.TabIndex = 4;
            this.btnBackward2.Text = "<5";
            this.btnBackward2.UseVisualStyleBackColor = true;
            // 
            // btnForward2
            // 
            this.btnForward2.Location = new System.Drawing.Point(156, 12);
            this.btnForward2.Name = "btnForward2";
            this.btnForward2.Size = new System.Drawing.Size(35, 30);
            this.btnForward2.TabIndex = 7;
            this.btnForward2.Text = "5>";
            this.btnForward2.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(71, 109);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 17;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // txtDataPointClock
            // 
            this.txtDataPointClock.Location = new System.Drawing.Point(89, 77);
            this.txtDataPointClock.Name = "txtDataPointClock";
            this.txtDataPointClock.Size = new System.Drawing.Size(71, 19);
            this.txtDataPointClock.TabIndex = 2;
            this.txtDataPointClock.Type = cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDataPointValue
            // 
            this.txtDataPointValue.Location = new System.Drawing.Point(89, 52);
            this.txtDataPointValue.Name = "txtDataPointValue";
            this.txtDataPointValue.Size = new System.Drawing.Size(71, 19);
            this.txtDataPointValue.TabIndex = 1;
            this.txtDataPointValue.Type = cadencii.NumberTextBox.ValueType.Integer;
            // 
            // btnBackward3
            // 
            this.btnBackward3.Location = new System.Drawing.Point(12, 12);
            this.btnBackward3.Name = "btnBackward3";
            this.btnBackward3.Size = new System.Drawing.Size(35, 30);
            this.btnBackward3.TabIndex = 18;
            this.btnBackward3.Text = "<10";
            this.btnBackward3.UseVisualStyleBackColor = true;
            // 
            // btnForward3
            // 
            this.btnForward3.Location = new System.Drawing.Point(192, 12);
            this.btnForward3.Name = "btnForward3";
            this.btnForward3.Size = new System.Drawing.Size(35, 30);
            this.btnForward3.TabIndex = 19;
            this.btnForward3.Text = "10>";
            this.btnForward3.UseVisualStyleBackColor = true;
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(178, 50);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(49, 23);
            this.btnUndo.TabIndex = 20;
            this.btnUndo.Text = "undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            // 
            // btnRedo
            // 
            this.btnRedo.Location = new System.Drawing.Point(178, 75);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(49, 23);
            this.btnRedo.TabIndex = 21;
            this.btnRedo.Text = "redo";
            this.btnRedo.UseVisualStyleBackColor = true;
            // 
            // FormCurvePointEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(239, 144);
            this.Controls.Add(this.btnRedo);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.btnForward3);
            this.Controls.Add(this.btnBackward3);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnForward2);
            this.Controls.Add(this.btnBackward2);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblDataPointValue);
            this.Controls.Add(this.btnForward);
            this.Controls.Add(this.txtDataPointClock);
            this.Controls.Add(this.btnBackward);
            this.Controls.Add(this.lblDataPointClock);
            this.Controls.Add(this.txtDataPointValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCurvePointEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormCurvePointEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnBackward;
        private Label lblDataPointValue;
        private NumberTextBox txtDataPointClock;
        private Label lblDataPointClock;
        private NumberTextBox txtDataPointValue;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnBackward2;
        private System.Windows.Forms.Button btnForward2;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnBackward3;
        private System.Windows.Forms.Button btnForward3;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnRedo;

        #endregion

    }

}
