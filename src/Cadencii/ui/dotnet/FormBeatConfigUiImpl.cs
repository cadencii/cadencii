/*
 * FormBeatConfigUiImpl.cs
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
using cadencii.apputil;
using cadencii;

namespace cadencii
{

    public class FormBeatConfigUiImpl : Form, FormBeatConfigUi
    {
        private FormBeatConfigUiListener mListener;

        public FormBeatConfigUiImpl(FormBeatConfigUiListener listener)
        {
            mListener = listener;
            InitializeComponent();
        }


        #region FormBeatConfigUiインターフェースの実装

        public void close()
        {
            this.Close();
        }

        public int getWidth()
        {
            return this.Width;
        }

        public int getHeight()
        {
            return this.Height;
        }

        public void setLocation(int x, int y)
        {
            this.Location = new System.Drawing.Point(x, y);
        }

        public void setDialogResult(bool value)
        {
            if (value) {
                this.DialogResult = DialogResult.OK;
            } else {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        public int getSelectedIndexDenominatorCombobox()
        {
            return comboDenominator.SelectedIndex;
        }

        public bool isCheckedEndCheckbox()
        {
            return chkEnd.Checked;
        }

        public void setTextBar2Label(string value)
        {
            lblBar2.Text = value;
        }

        public void setTextBar1Label(string value)
        {
            lblBar1.Text = value;
        }

        public void setTextStartLabel(string value)
        {
            lblStart.Text = value;
        }

        public void setTextOkButton(string value)
        {
            btnOK.Text = value;
        }

        public void setTextCancelButton(string value)
        {
            btnCancel.Text = value;
        }

        public void setTextBeatGroup(string value)
        {
            groupBeat.Text = value;
        }

        public int getValueEndNum()
        {
            return (int)numEnd.Value;
        }

        public bool isEnabledEndCheckbox()
        {
            return chkEnd.Enabled;
        }

        public void setTextEndCheckbox(string value)
        {
            chkEnd.Text = value;
        }

        public int getValueNumeratorNum()
        {
            return (int)numNumerator.Value;
        }

        public void setFont(string fontName, float fontSize)
        {
            Util.applyFontRecurse(this, new cadencii.java.awt.Font(new System.Drawing.Font(fontName, fontSize)));
        }

        public void setTextPositionGroup(string value)
        {
            groupPosition.Text = value;
        }

        public int getMaximumStartNum()
        {
            return (int)numStart.Maximum;
        }

        public int getMinimumStartNum()
        {
            return (int)numStart.Minimum;
        }

        public void setValueStartNum(int value)
        {
            numStart.Value = new decimal(value);
        }

        public int getValueStartNum()
        {
            return (int)numStart.Value;
        }

        public int getMinimumNumeratorNum()
        {
            return (int)numNumerator.Minimum;
        }

        public int getMaximumNumeratorNum()
        {
            return (int)numNumerator.Maximum;
        }

        public void setValueNumeratorNum(int value)
        {
            numNumerator.Value = new decimal(value);
        }

        public void setSelectedIndexDenominatorCombobox(int value)
        {
            comboDenominator.SelectedIndex = value;
        }

        public void addItemDenominatorCombobox(string value)
        {
            comboDenominator.Items.Add(value);
        }

        public void setMinimumStartNum(int value)
        {
            numStart.Minimum = value;
        }

        public void setMaximumStartNum(int value)
        {
            numStart.Maximum = value;
        }

        public void setMinimumEndNum(int value)
        {
            numEnd.Minimum = value;
        }

        public void setMaximumEndNum(int value)
        {
            numEnd.Maximum = value;
        }

        public void setValueEndNum(int value)
        {
            numEnd.Value = new decimal(value);
        }

        public int getMinimumEndNum()
        {
            return (int)numEnd.Minimum;
        }

        public int getMaximumEndNum()
        {
            return (int)numEnd.Maximum;
        }

        public void setTitle(string value)
        {
            this.Text = value;
        }

        public int showDialog(object parent)
        {
            DialogResult ret;
            if (parent == null || (parent != null && !(parent is Form))) {
                ret = base.ShowDialog();
            } else {
                Form form = (Form)parent;
                ret = base.ShowDialog(form);
            }
            if (ret == DialogResult.OK || ret == DialogResult.Yes) {
                return 1;
            } else {
                return 0;
            }
        }

        public void removeAllItemsDenominatorCombobox()
        {
            comboDenominator.Items.Clear();
        }

        public void setEnabledEndCheckbox(bool value)
        {
            chkEnd.Enabled = value;
        }

        public void setEnabledStartNum(bool value)
        {
            numStart.Enabled = value;
        }

        public void setEnabledEndNum(bool value)
        {
            numEnd.Enabled = value;
        }

        #endregion



        #region イベントハンドラーの実装

        public void chkEnd_CheckedChanged(Object sender, EventArgs e)
        {
            mListener.checkboxEndCheckedChangedSlot();
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
            mListener.buttonOkClickedSlot();
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            mListener.buttonCancelClickedSlot();
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
            this.groupPosition = new System.Windows.Forms.GroupBox();
            this.lblBar2 = new System.Windows.Forms.Label();
            this.lblBar1 = new System.Windows.Forms.Label();
            this.numEnd = new cadencii.NumericUpDownEx();
            this.numStart = new cadencii.NumericUpDownEx();
            this.chkEnd = new System.Windows.Forms.CheckBox();
            this.lblStart = new System.Windows.Forms.Label();
            this.groupBeat = new System.Windows.Forms.GroupBox();
            this.comboDenominator = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numNumerator = new cadencii.NumericUpDownEx();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupPosition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).BeginInit();
            this.groupBeat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNumerator)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPosition
            // 
            this.groupPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPosition.Controls.Add(this.lblBar2);
            this.groupPosition.Controls.Add(this.lblBar1);
            this.groupPosition.Controls.Add(this.numEnd);
            this.groupPosition.Controls.Add(this.numStart);
            this.groupPosition.Controls.Add(this.chkEnd);
            this.groupPosition.Controls.Add(this.lblStart);
            this.groupPosition.Location = new System.Drawing.Point(7, 7);
            this.groupPosition.Name = "groupPosition";
            this.groupPosition.Size = new System.Drawing.Size(261, 71);
            this.groupPosition.TabIndex = 0;
            this.groupPosition.TabStop = false;
            this.groupPosition.Text = "Position";
            // 
            // lblBar2
            // 
            this.lblBar2.AutoSize = true;
            this.lblBar2.Location = new System.Drawing.Point(176, 46);
            this.lblBar2.Name = "lblBar2";
            this.lblBar2.Size = new System.Drawing.Size(48, 12);
            this.lblBar2.TabIndex = 5;
            this.lblBar2.Text = "Measure";
            // 
            // lblBar1
            // 
            this.lblBar1.AutoSize = true;
            this.lblBar1.Location = new System.Drawing.Point(176, 20);
            this.lblBar1.Name = "lblBar1";
            this.lblBar1.Size = new System.Drawing.Size(48, 12);
            this.lblBar1.TabIndex = 4;
            this.lblBar1.Text = "Measure";
            // 
            // numEnd
            // 
            this.numEnd.Location = new System.Drawing.Point(97, 44);
            this.numEnd.Name = "numEnd";
            this.numEnd.Size = new System.Drawing.Size(73, 19);
            this.numEnd.TabIndex = 6;
            // 
            // numStart
            // 
            this.numStart.Location = new System.Drawing.Point(97, 18);
            this.numStart.Name = "numStart";
            this.numStart.Size = new System.Drawing.Size(73, 19);
            this.numStart.TabIndex = 7;
            // 
            // chkEnd
            // 
            this.chkEnd.AutoSize = true;
            this.chkEnd.Location = new System.Drawing.Point(23, 45);
            this.chkEnd.Name = "chkEnd";
            this.chkEnd.Size = new System.Drawing.Size(52, 16);
            this.chkEnd.TabIndex = 1;
            this.chkEnd.Text = "To(&T)";
            this.chkEnd.UseVisualStyleBackColor = true;
            this.chkEnd.CheckedChanged += new System.EventHandler(this.chkEnd_CheckedChanged);
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(21, 20);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(46, 12);
            this.lblStart.TabIndex = 0;
            this.lblStart.Text = "From(&F)";
            // 
            // groupBeat
            // 
            this.groupBeat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBeat.Controls.Add(this.comboDenominator);
            this.groupBeat.Controls.Add(this.label2);
            this.groupBeat.Controls.Add(this.label1);
            this.groupBeat.Controls.Add(this.numNumerator);
            this.groupBeat.Location = new System.Drawing.Point(7, 84);
            this.groupBeat.Name = "groupBeat";
            this.groupBeat.Size = new System.Drawing.Size(261, 55);
            this.groupBeat.TabIndex = 1;
            this.groupBeat.TabStop = false;
            this.groupBeat.Text = "Beat";
            // 
            // comboDenominator
            // 
            this.comboDenominator.FormattingEnabled = true;
            this.comboDenominator.Location = new System.Drawing.Point(182, 20);
            this.comboDenominator.Name = "comboDenominator";
            this.comboDenominator.Size = new System.Drawing.Size(73, 20);
            this.comboDenominator.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "/";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(102, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "(1 - 255)";
            // 
            // numNumerator
            // 
            this.numNumerator.Location = new System.Drawing.Point(23, 21);
            this.numNumerator.Name = "numNumerator";
            this.numNumerator.Size = new System.Drawing.Size(73, 19);
            this.numNumerator.TabIndex = 9;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(113, 150);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(194, 150);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormBeatConfigUiImpl
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(278, 182);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBeat);
            this.Controls.Add(this.groupPosition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBeatConfigUiImpl";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Beat Change";
            this.groupPosition.ResumeLayout(false);
            this.groupPosition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).EndInit();
            this.groupBeat.ResumeLayout(false);
            this.groupBeat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNumerator)).EndInit();
            this.ResumeLayout(false);

        }

        private GroupBox groupPosition;
        private GroupBox groupBeat;
        private Button btnOK;
        private Button btnCancel;
        private NumericUpDownEx numStart;
        private CheckBox chkEnd;
        private Label lblStart;
        private Label lblBar2;
        private Label lblBar1;
        private NumericUpDownEx numEnd;
        private Label label1;
        private NumericUpDownEx numNumerator;
        private Label label2;
        private ComboBox comboDenominator;

        #endregion
    }

}
