/*
 * FormBezierPointEditUiImpl.cs
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
using cadencii.java.awt;
using cadencii.java.util;



namespace cadencii
{
    public class FormBezierPointEditUiImpl : Form, FormBezierPointEditUi
    {
        private FormBezierPointEditUiListener listener;

        public FormBezierPointEditUiImpl(FormBezierPointEditUiListener listener)
        {
            this.listener = listener;
            InitializeComponent();
            registerEventHandlers();
            setResources();
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
        }


        #region UiBaseインターフェースの実装

        public int showDialog(object obj)
        {
            System.Windows.Forms.DialogResult ret;
            if (obj == null || (obj != null && !(obj is System.Windows.Forms.Form))) {
                ret = base.ShowDialog();
            } else {
                System.Windows.Forms.Form form = (System.Windows.Forms.Form)obj;
                ret = base.ShowDialog(form);
            }
            if (ret == System.Windows.Forms.DialogResult.OK || ret == System.Windows.Forms.DialogResult.Yes) {
                return 1;
            } else {
                return 0;
            }
        }

        #endregion


        #region FormBezierPointEditUiインターフェースの実装

        public void setDataPointValueText(string value)
        {
            txtDataPointValue.Text = value;
        }

        public void setDataPointClockText(string value)
        {
            txtDataPointClock.Text = value;
        }

        public void setRightValueText(string value)
        {
            txtRightValue.Text = value;
        }

        public void setRightClockText(string value)
        {
            txtRightClock.Text = value;
        }

        public void setLeftValueText(string value)
        {
            txtLeftValue.Text = value;
        }

        public void setLeftClockText(string value)
        {
            txtLeftClock.Text = value;
        }

        public void setLeftClockEnabled(bool value)
        {
            txtLeftClock.Enabled = value;
        }

        public bool isEnableSmoothSelected()
        {
            return chkEnableSmooth.Checked;
        }

        public void setEnableSmoothSelected(bool value)
        {
            chkEnableSmooth.Checked = value;
        }

        public void setRightButtonEnabled(bool value)
        {
            btnRight.Enabled = value;
        }

        public void setRightValueEnabled(bool value)
        {
            txtRightValue.Enabled = value;
        }

        public void setRightClockEnabled(bool value)
        {
            txtRightClock.Enabled = value;
        }

        public void setLeftButtonEnabled(bool value)
        {
            btnLeft.Enabled = value;
        }

        public void setLeftValueEnabled(bool value)
        {
            txtLeftValue.Enabled = value;
        }

        public string getRightValueText()
        {
            return txtRightValue.Text;
        }

        public string getRightClockText()
        {
            return txtRightClock.Text;
        }

        public string getLeftValueText()
        {
            return txtLeftValue.Text;
        }

        public string getLeftClockText()
        {
            return txtLeftClock.Text;
        }

        public string getDataPointValueText()
        {
            return txtDataPointValue.Text;
        }

        public string getDataPointClockText()
        {
            return txtDataPointClock.Text;
        }

        public void setCheckboxEnableSmoothText(string value)
        {
            chkEnableSmooth.Text = value;
        }

        public void setLabelRightValueText(string value)
        {
            lblRightValue.Text = value;
        }

        public void setGroupRightTitle(string value)
        {
            groupRight.Text = value;
        }

        public void setLabelRightClockText(string value)
        {
            lblRightClock.Text = value;
        }

        public void setLabelLeftValueText(string value)
        {
            lblLeftValue.Text = value;
        }

        public void setLabelLeftClockText(string value)
        {
            lblLeftClock.Text = value;
        }

        public void setGroupLeftTitle(string value)
        {
            groupLeft.Text = value;
        }

        public void setLabelDataPointValueText(string value)
        {
            lblDataPointValue.Text = value;
        }

        public void setLabelDataPointClockText(string value)
        {
            lblDataPointClock.Text = value;
        }

        public void setGroupDataPointTitle(string value)
        {
            groupDataPoint.Text = value;
        }

        public void setDialogResult(bool result)
        {
            if (result) {
                this.DialogResult = DialogResult.OK;
            } else {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        public void setOpacity(double opacity)
        {
            this.Opacity = opacity;
        }

        public void close()
        {
            this.Close();
        }

        public void setTitle(string value)
        {
            this.Text = value;
        }

        #endregion


        #region helper methods

        private static string _(string message)
        {
            return Messaging.getMessage(message);
        }

        private void registerEventHandlers()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            chkEnableSmooth.CheckedChanged += new EventHandler(chkEnableSmooth_CheckedChanged);
            btnLeft.MouseMove += new MouseEventHandler(common_MouseMove);
            btnLeft.MouseDown += new MouseEventHandler(handleOperationButtonMouseDown);
            btnLeft.MouseUp += new MouseEventHandler(common_MouseUp);
            btnDataPoint.MouseMove += new MouseEventHandler(common_MouseMove);
            btnDataPoint.MouseDown += new MouseEventHandler(handleOperationButtonMouseDown);
            btnDataPoint.MouseUp += new MouseEventHandler(common_MouseUp);
            btnRight.MouseMove += new MouseEventHandler(common_MouseMove);
            btnRight.MouseDown += new MouseEventHandler(handleOperationButtonMouseDown);
            btnRight.MouseUp += new MouseEventHandler(common_MouseUp);
            btnBackward.Click += new EventHandler(btnBackward_Click);
            btnForward.Click += new EventHandler(btnForward_Click);
        }

        private void setResources()
        {
            this.btnLeft.Image = Properties.Resources.target__pencil;
            this.btnDataPoint.Image = Properties.Resources.target__pencil;
            this.btnRight.Image = Properties.Resources.target__pencil;
        }
        #endregion


        #region event handlers

        public void btnOK_Click(object sender, EventArgs e)
        {
            this.listener.buttonOkClick();
        }

        public void chkEnableSmooth_CheckedChanged(object sender, EventArgs e)
        {
            this.listener.checkboxEnableSmoothCheckedChanged();
        }

        public void handleOperationButtonMouseDown(object sender, MouseEventArgs e)
        {
            if (sender == btnLeft) {
                this.listener.buttonLeftMouseDown();
            } else if (sender == btnRight) {
                this.listener.buttonRightMouseDown();
            } else {
                this.listener.buttonCenterMouseDown();
            }
        }

        public void common_MouseUp(object sender, MouseEventArgs e)
        {
            this.listener.buttonsMouseUp();
        }

        public void common_MouseMove(object sender, MouseEventArgs e)
        {
            this.listener.buttonsMouseMove();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            this.listener.buttonForwardClick();
        }

        private void btnBackward_Click(object sender, EventArgs e)
        {
            this.listener.buttonBackwardClick();
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            this.listener.buttonCancelClick();
        }

        #endregion

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
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.chkEnableSmooth = new CheckBox();
            this.lblLeftValue = new Label();
            this.lblLeftClock = new Label();
            this.groupLeft = new GroupBox();
            this.btnLeft = new Button();
            this.txtLeftClock = new NumberTextBox();
            this.txtLeftValue = new NumberTextBox();
            this.groupDataPoint = new GroupBox();
            this.btnDataPoint = new Button();
            this.lblDataPointValue = new Label();
            this.txtDataPointClock = new NumberTextBox();
            this.lblDataPointClock = new Label();
            this.txtDataPointValue = new NumberTextBox();
            this.groupRight = new GroupBox();
            this.btnRight = new Button();
            this.lblRightValue = new Label();
            this.txtRightClock = new NumberTextBox();
            this.lblRightClock = new Label();
            this.txtRightValue = new NumberTextBox();
            this.btnBackward = new Button();
            this.btnForward = new Button();
            this.groupLeft.SuspendLayout();
            this.groupDataPoint.SuspendLayout();
            this.groupRight.SuspendLayout();
            this.SuspendLayout();
            //
            // btnCancel
            //
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(374, 170);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // btnOK
            //
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.AutoSize = true;
            this.btnOK.Location = new System.Drawing.Point(293, 170);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            //
            // chkEnableSmooth
            //
            this.chkEnableSmooth.AutoSize = true;
            this.chkEnableSmooth.Location = new System.Drawing.Point(196, 12);
            this.chkEnableSmooth.Name = "chkEnableSmooth";
            this.chkEnableSmooth.Size = new System.Drawing.Size(62, 16);
            this.chkEnableSmooth.TabIndex = 2;
            this.chkEnableSmooth.Text = "Smooth";
            this.chkEnableSmooth.UseVisualStyleBackColor = true;
            //
            // lblLeftValue
            //
            this.lblLeftValue.AutoSize = true;
            this.lblLeftValue.Location = new System.Drawing.Point(12, 54);
            this.lblLeftValue.Name = "lblLeftValue";
            this.lblLeftValue.Size = new System.Drawing.Size(34, 12);
            this.lblLeftValue.TabIndex = 16;
            this.lblLeftValue.Text = "Value";
            //
            // lblLeftClock
            //
            this.lblLeftClock.AutoSize = true;
            this.lblLeftClock.Location = new System.Drawing.Point(12, 29);
            this.lblLeftClock.Name = "lblLeftClock";
            this.lblLeftClock.Size = new System.Drawing.Size(34, 12);
            this.lblLeftClock.TabIndex = 15;
            this.lblLeftClock.Text = "Clock";
            //
            // groupLeft
            //
            this.groupLeft.AutoSize = true;
            this.groupLeft.Controls.Add(this.btnLeft);
            this.groupLeft.Controls.Add(this.lblLeftValue);
            this.groupLeft.Controls.Add(this.txtLeftClock);
            this.groupLeft.Controls.Add(this.lblLeftClock);
            this.groupLeft.Controls.Add(this.txtLeftValue);
            this.groupLeft.Location = new System.Drawing.Point(14, 38);
            this.groupLeft.Name = "groupLeft";
            this.groupLeft.Size = new System.Drawing.Size(141, 121);
            this.groupLeft.TabIndex = 17;
            this.groupLeft.TabStop = false;
            this.groupLeft.Text = "Left Control Point";
            //
            // btnLeft
            //
            this.btnLeft.AutoSize = true;
            this.btnLeft.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLeft.Location = new System.Drawing.Point(14, 76);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(113, 27);
            this.btnLeft.TabIndex = 6;
            this.btnLeft.UseVisualStyleBackColor = true;
            //
            // txtLeftClock
            //
            this.txtLeftClock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtLeftClock.Enabled = false;
            this.txtLeftClock.ForeColor = System.Drawing.Color.Black;
            this.txtLeftClock.Location = new System.Drawing.Point(66, 26);
            this.txtLeftClock.Name = "txtLeftClock";
            this.txtLeftClock.Size = new System.Drawing.Size(61, 19);
            this.txtLeftClock.TabIndex = 4;
            this.txtLeftClock.Text = "0";
            this.txtLeftClock.Type = NumberTextBox.ValueType.Integer;
            //
            // txtLeftValue
            //
            this.txtLeftValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtLeftValue.Enabled = false;
            this.txtLeftValue.ForeColor = System.Drawing.Color.Black;
            this.txtLeftValue.Location = new System.Drawing.Point(66, 51);
            this.txtLeftValue.Name = "txtLeftValue";
            this.txtLeftValue.Size = new System.Drawing.Size(61, 19);
            this.txtLeftValue.TabIndex = 5;
            this.txtLeftValue.Text = "0";
            this.txtLeftValue.Type = NumberTextBox.ValueType.Integer;
            //
            // groupDataPoint
            //
            this.groupDataPoint.AutoSize = true;
            this.groupDataPoint.Controls.Add(this.btnDataPoint);
            this.groupDataPoint.Controls.Add(this.lblDataPointValue);
            this.groupDataPoint.Controls.Add(this.txtDataPointClock);
            this.groupDataPoint.Controls.Add(this.lblDataPointClock);
            this.groupDataPoint.Controls.Add(this.txtDataPointValue);
            this.groupDataPoint.Location = new System.Drawing.Point(161, 38);
            this.groupDataPoint.Name = "groupDataPoint";
            this.groupDataPoint.Size = new System.Drawing.Size(141, 121);
            this.groupDataPoint.TabIndex = 18;
            this.groupDataPoint.TabStop = false;
            this.groupDataPoint.Text = "Data Point";
            //
            // btnDataPoint
            //
            this.btnDataPoint.AutoSize = true;
            this.btnDataPoint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDataPoint.Location = new System.Drawing.Point(14, 76);
            this.btnDataPoint.Name = "btnDataPoint";
            this.btnDataPoint.Size = new System.Drawing.Size(113, 27);
            this.btnDataPoint.TabIndex = 9;
            this.btnDataPoint.UseVisualStyleBackColor = true;
            //
            // lblDataPointValue
            //
            this.lblDataPointValue.AutoSize = true;
            this.lblDataPointValue.Location = new System.Drawing.Point(12, 54);
            this.lblDataPointValue.Name = "lblDataPointValue";
            this.lblDataPointValue.Size = new System.Drawing.Size(34, 12);
            this.lblDataPointValue.TabIndex = 16;
            this.lblDataPointValue.Text = "Value";
            //
            // txtDataPointClock
            //
            this.txtDataPointClock.Location = new System.Drawing.Point(66, 26);
            this.txtDataPointClock.Name = "txtDataPointClock";
            this.txtDataPointClock.Size = new System.Drawing.Size(61, 19);
            this.txtDataPointClock.TabIndex = 7;
            this.txtDataPointClock.Type = NumberTextBox.ValueType.Integer;
            //
            // lblDataPointClock
            //
            this.lblDataPointClock.AutoSize = true;
            this.lblDataPointClock.Location = new System.Drawing.Point(12, 29);
            this.lblDataPointClock.Name = "lblDataPointClock";
            this.lblDataPointClock.Size = new System.Drawing.Size(34, 12);
            this.lblDataPointClock.TabIndex = 15;
            this.lblDataPointClock.Text = "Clock";
            //
            // txtDataPointValue
            //
            this.txtDataPointValue.Location = new System.Drawing.Point(66, 51);
            this.txtDataPointValue.Name = "txtDataPointValue";
            this.txtDataPointValue.Size = new System.Drawing.Size(61, 19);
            this.txtDataPointValue.TabIndex = 8;
            this.txtDataPointValue.Type = NumberTextBox.ValueType.Integer;
            //
            // groupRight
            //
            this.groupRight.AutoSize = true;
            this.groupRight.Controls.Add(this.btnRight);
            this.groupRight.Controls.Add(this.lblRightValue);
            this.groupRight.Controls.Add(this.txtRightClock);
            this.groupRight.Controls.Add(this.lblRightClock);
            this.groupRight.Controls.Add(this.txtRightValue);
            this.groupRight.Location = new System.Drawing.Point(308, 38);
            this.groupRight.Name = "groupRight";
            this.groupRight.Size = new System.Drawing.Size(141, 121);
            this.groupRight.TabIndex = 19;
            this.groupRight.TabStop = false;
            this.groupRight.Text = "Right Control Point";
            //
            // btnRight
            //
            this.btnRight.AutoSize = true;
            this.btnRight.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRight.Location = new System.Drawing.Point(14, 76);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(113, 27);
            this.btnRight.TabIndex = 12;
            this.btnRight.UseVisualStyleBackColor = true;
            //
            // lblRightValue
            //
            this.lblRightValue.AutoSize = true;
            this.lblRightValue.Location = new System.Drawing.Point(12, 54);
            this.lblRightValue.Name = "lblRightValue";
            this.lblRightValue.Size = new System.Drawing.Size(34, 12);
            this.lblRightValue.TabIndex = 16;
            this.lblRightValue.Text = "Value";
            //
            // txtRightClock
            //
            this.txtRightClock.Enabled = false;
            this.txtRightClock.Location = new System.Drawing.Point(66, 26);
            this.txtRightClock.Name = "txtRightClock";
            this.txtRightClock.Size = new System.Drawing.Size(61, 19);
            this.txtRightClock.TabIndex = 10;
            this.txtRightClock.Type = NumberTextBox.ValueType.Integer;
            //
            // lblRightClock
            //
            this.lblRightClock.AutoSize = true;
            this.lblRightClock.Location = new System.Drawing.Point(12, 29);
            this.lblRightClock.Name = "lblRightClock";
            this.lblRightClock.Size = new System.Drawing.Size(34, 12);
            this.lblRightClock.TabIndex = 15;
            this.lblRightClock.Text = "Clock";
            //
            // txtRightValue
            //
            this.txtRightValue.Enabled = false;
            this.txtRightValue.Location = new System.Drawing.Point(66, 51);
            this.txtRightValue.Name = "txtRightValue";
            this.txtRightValue.Size = new System.Drawing.Size(61, 19);
            this.txtRightValue.TabIndex = 11;
            this.txtRightValue.Type = NumberTextBox.ValueType.Integer;
            //
            // btnBackward
            //
            this.btnBackward.AutoSize = true;
            this.btnBackward.Location = new System.Drawing.Point(99, 8);
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size(75, 23);
            this.btnBackward.TabIndex = 1;
            this.btnBackward.Text = "<<";
            this.btnBackward.UseVisualStyleBackColor = true;
            //
            // btnForward
            //
            this.btnForward.AutoSize = true;
            this.btnForward.Location = new System.Drawing.Point(290, 9);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(75, 23);
            this.btnForward.TabIndex = 3;
            this.btnForward.Text = ">>";
            this.btnForward.UseVisualStyleBackColor = true;
            //
            // FormBezierPointEdit
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(463, 208);
            this.Controls.Add(this.btnForward);
            this.Controls.Add(this.btnBackward);
            this.Controls.Add(this.groupRight);
            this.Controls.Add(this.groupDataPoint);
            this.Controls.Add(this.groupLeft);
            this.Controls.Add(this.chkEnableSmooth);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBezierPointEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Bezier Data Point";
            this.groupLeft.ResumeLayout(false);
            this.groupLeft.PerformLayout();
            this.groupDataPoint.ResumeLayout(false);
            this.groupDataPoint.PerformLayout();
            this.groupRight.ResumeLayout(false);
            this.groupRight.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Button btnCancel;
        private Button btnOK;
        private CheckBox chkEnableSmooth;
        private Label lblLeftValue;
        private Label lblLeftClock;
        private NumberTextBox txtLeftValue;
        private NumberTextBox txtLeftClock;
        private GroupBox groupLeft;
        private GroupBox groupDataPoint;
        private Label lblDataPointValue;
        private NumberTextBox txtDataPointClock;
        private Label lblDataPointClock;
        private NumberTextBox txtDataPointValue;
        private GroupBox groupRight;
        private Label lblRightValue;
        private NumberTextBox txtRightClock;
        private Label lblRightClock;
        private NumberTextBox txtRightValue;
        private Button btnDataPoint;
        private Button btnLeft;
        private Button btnRight;
        private Button btnBackward;
        private Button btnForward;

    }

}
