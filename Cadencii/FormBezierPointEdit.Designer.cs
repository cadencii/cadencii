/*
 * FormBezierPointEdit.Designer.cs
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
namespace Boare.Cadencii {
    using boolean = System.Boolean;
    partial class FormBezierPointEdit {
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkEnableSmooth = new System.Windows.Forms.CheckBox();
            this.lblLeftValue = new System.Windows.Forms.Label();
            this.lblLeftClock = new System.Windows.Forms.Label();
            this.groupLeft = new System.Windows.Forms.GroupBox();
            this.btnLeft = new System.Windows.Forms.Button();
            this.groupDataPoint = new System.Windows.Forms.GroupBox();
            this.btnDataPoint = new System.Windows.Forms.Button();
            this.lblDataPointValue = new System.Windows.Forms.Label();
            this.lblDataPointClock = new System.Windows.Forms.Label();
            this.groupRight = new System.Windows.Forms.GroupBox();
            this.btnRight = new System.Windows.Forms.Button();
            this.lblRightValue = new System.Windows.Forms.Label();
            this.lblRightClock = new System.Windows.Forms.Label();
            this.btnBackward = new System.Windows.Forms.Button();
            this.btnForward = new System.Windows.Forms.Button();
            this.txtRightClock = new Boare.Cadencii.NumberTextBox();
            this.txtRightValue = new Boare.Cadencii.NumberTextBox();
            this.txtDataPointClock = new Boare.Cadencii.NumberTextBox();
            this.txtDataPointValue = new Boare.Cadencii.NumberTextBox();
            this.txtLeftClock = new Boare.Cadencii.NumberTextBox();
            this.txtLeftValue = new Boare.Cadencii.NumberTextBox();
            this.groupLeft.SuspendLayout();
            this.groupDataPoint.SuspendLayout();
            this.groupRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 374, 163 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 293, 163 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            // 
            // chkEnableSmooth
            // 
            this.chkEnableSmooth.AutoSize = true;
            this.chkEnableSmooth.Location = new System.Drawing.Point( 196, 12 );
            this.chkEnableSmooth.Name = "chkEnableSmooth";
            this.chkEnableSmooth.Size = new System.Drawing.Size( 62, 16 );
            this.chkEnableSmooth.TabIndex = 3;
            this.chkEnableSmooth.Text = "Smooth";
            this.chkEnableSmooth.UseVisualStyleBackColor = true;
            this.chkEnableSmooth.CheckedChanged += new System.EventHandler( this.chkEnableSmooth_CheckedChanged );
            // 
            // lblLeftValue
            // 
            this.lblLeftValue.AutoSize = true;
            this.lblLeftValue.Location = new System.Drawing.Point( 16, 54 );
            this.lblLeftValue.Name = "lblLeftValue";
            this.lblLeftValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblLeftValue.TabIndex = 16;
            this.lblLeftValue.Text = "Value";
            // 
            // lblLeftClock
            // 
            this.lblLeftClock.AutoSize = true;
            this.lblLeftClock.Location = new System.Drawing.Point( 16, 29 );
            this.lblLeftClock.Name = "lblLeftClock";
            this.lblLeftClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblLeftClock.TabIndex = 15;
            this.lblLeftClock.Text = "Clock";
            // 
            // groupLeft
            // 
            this.groupLeft.Controls.Add( this.btnLeft );
            this.groupLeft.Controls.Add( this.lblLeftValue );
            this.groupLeft.Controls.Add( this.txtLeftClock );
            this.groupLeft.Controls.Add( this.lblLeftClock );
            this.groupLeft.Controls.Add( this.txtLeftValue );
            this.groupLeft.Location = new System.Drawing.Point( 14, 38 );
            this.groupLeft.Name = "groupLeft";
            this.groupLeft.Size = new System.Drawing.Size( 141, 113 );
            this.groupLeft.TabIndex = 17;
            this.groupLeft.TabStop = false;
            this.groupLeft.Text = "Left Control Point";
            // 
            // btnLeft
            // 
            this.btnLeft.Image = global::Boare.Cadencii.Properties.Resources.target__pencil;
            this.btnLeft.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLeft.Location = new System.Drawing.Point( 18, 76 );
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size( 109, 27 );
            this.btnLeft.TabIndex = 18;
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.MouseMove += new System.Windows.Forms.MouseEventHandler( this.common_MouseMove );
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnLeft_MouseDown );
            this.btnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler( this.common_MouseUp );
            // 
            // groupDataPoint
            // 
            this.groupDataPoint.Controls.Add( this.btnDataPoint );
            this.groupDataPoint.Controls.Add( this.lblDataPointValue );
            this.groupDataPoint.Controls.Add( this.txtDataPointClock );
            this.groupDataPoint.Controls.Add( this.lblDataPointClock );
            this.groupDataPoint.Controls.Add( this.txtDataPointValue );
            this.groupDataPoint.Location = new System.Drawing.Point( 161, 38 );
            this.groupDataPoint.Name = "groupDataPoint";
            this.groupDataPoint.Size = new System.Drawing.Size( 141, 113 );
            this.groupDataPoint.TabIndex = 18;
            this.groupDataPoint.TabStop = false;
            this.groupDataPoint.Text = "Data Point";
            // 
            // btnDataPoint
            // 
            this.btnDataPoint.Image = global::Boare.Cadencii.Properties.Resources.target__pencil;
            this.btnDataPoint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDataPoint.Location = new System.Drawing.Point( 18, 76 );
            this.btnDataPoint.Name = "btnDataPoint";
            this.btnDataPoint.Size = new System.Drawing.Size( 109, 27 );
            this.btnDataPoint.TabIndex = 17;
            this.btnDataPoint.UseVisualStyleBackColor = true;
            this.btnDataPoint.MouseMove += new System.Windows.Forms.MouseEventHandler( this.common_MouseMove );
            this.btnDataPoint.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnDataPoint_MouseDown );
            this.btnDataPoint.MouseUp += new System.Windows.Forms.MouseEventHandler( this.common_MouseUp );
            // 
            // lblDataPointValue
            // 
            this.lblDataPointValue.AutoSize = true;
            this.lblDataPointValue.Location = new System.Drawing.Point( 16, 54 );
            this.lblDataPointValue.Name = "lblDataPointValue";
            this.lblDataPointValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblDataPointValue.TabIndex = 16;
            this.lblDataPointValue.Text = "Value";
            // 
            // lblDataPointClock
            // 
            this.lblDataPointClock.AutoSize = true;
            this.lblDataPointClock.Location = new System.Drawing.Point( 16, 29 );
            this.lblDataPointClock.Name = "lblDataPointClock";
            this.lblDataPointClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblDataPointClock.TabIndex = 15;
            this.lblDataPointClock.Text = "Clock";
            // 
            // groupRight
            // 
            this.groupRight.Controls.Add( this.btnRight );
            this.groupRight.Controls.Add( this.lblRightValue );
            this.groupRight.Controls.Add( this.txtRightClock );
            this.groupRight.Controls.Add( this.lblRightClock );
            this.groupRight.Controls.Add( this.txtRightValue );
            this.groupRight.Location = new System.Drawing.Point( 308, 38 );
            this.groupRight.Name = "groupRight";
            this.groupRight.Size = new System.Drawing.Size( 141, 113 );
            this.groupRight.TabIndex = 19;
            this.groupRight.TabStop = false;
            this.groupRight.Text = "Right Control Point";
            // 
            // btnRight
            // 
            this.btnRight.Image = global::Boare.Cadencii.Properties.Resources.target__pencil;
            this.btnRight.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRight.Location = new System.Drawing.Point( 18, 76 );
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size( 109, 27 );
            this.btnRight.TabIndex = 18;
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.MouseMove += new System.Windows.Forms.MouseEventHandler( this.common_MouseMove );
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnRight_MouseDown );
            this.btnRight.MouseUp += new System.Windows.Forms.MouseEventHandler( this.common_MouseUp );
            // 
            // lblRightValue
            // 
            this.lblRightValue.AutoSize = true;
            this.lblRightValue.Location = new System.Drawing.Point( 16, 54 );
            this.lblRightValue.Name = "lblRightValue";
            this.lblRightValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblRightValue.TabIndex = 16;
            this.lblRightValue.Text = "Value";
            // 
            // lblRightClock
            // 
            this.lblRightClock.AutoSize = true;
            this.lblRightClock.Location = new System.Drawing.Point( 16, 29 );
            this.lblRightClock.Name = "lblRightClock";
            this.lblRightClock.Size = new System.Drawing.Size( 34, 12 );
            this.lblRightClock.TabIndex = 15;
            this.lblRightClock.Text = "Clock";
            // 
            // btnBackward
            // 
            this.btnBackward.Location = new System.Drawing.Point( 99, 8 );
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size( 75, 23 );
            this.btnBackward.TabIndex = 20;
            this.btnBackward.Text = "<<";
            this.btnBackward.UseVisualStyleBackColor = true;
            this.btnBackward.Click += new System.EventHandler( this.btnBackward_Click );
            // 
            // btnForward
            // 
            this.btnForward.Location = new System.Drawing.Point( 290, 9 );
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size( 75, 23 );
            this.btnForward.TabIndex = 21;
            this.btnForward.Text = ">>";
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.Click += new System.EventHandler( this.btnForward_Click );
            // 
            // txtRightClock
            // 
            this.txtRightClock.Enabled = false;
            this.txtRightClock.Location = new System.Drawing.Point( 56, 26 );
            this.txtRightClock.Name = "txtRightClock";
            this.txtRightClock.Size = new System.Drawing.Size( 71, 19 );
            this.txtRightClock.TabIndex = 6;
            this.txtRightClock.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtRightValue
            // 
            this.txtRightValue.Enabled = false;
            this.txtRightValue.Location = new System.Drawing.Point( 56, 51 );
            this.txtRightValue.Name = "txtRightValue";
            this.txtRightValue.Size = new System.Drawing.Size( 71, 19 );
            this.txtRightValue.TabIndex = 7;
            this.txtRightValue.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDataPointClock
            // 
            this.txtDataPointClock.Location = new System.Drawing.Point( 56, 26 );
            this.txtDataPointClock.Name = "txtDataPointClock";
            this.txtDataPointClock.Size = new System.Drawing.Size( 71, 19 );
            this.txtDataPointClock.TabIndex = 1;
            this.txtDataPointClock.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDataPointValue
            // 
            this.txtDataPointValue.Location = new System.Drawing.Point( 56, 51 );
            this.txtDataPointValue.Name = "txtDataPointValue";
            this.txtDataPointValue.Size = new System.Drawing.Size( 71, 19 );
            this.txtDataPointValue.TabIndex = 2;
            this.txtDataPointValue.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtLeftClock
            // 
            this.txtLeftClock.BackColor = System.Drawing.SystemColors.Window;
            this.txtLeftClock.Enabled = false;
            this.txtLeftClock.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtLeftClock.Location = new System.Drawing.Point( 56, 26 );
            this.txtLeftClock.Name = "txtLeftClock";
            this.txtLeftClock.Size = new System.Drawing.Size( 71, 19 );
            this.txtLeftClock.TabIndex = 4;
            this.txtLeftClock.Text = "0";
            this.txtLeftClock.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtLeftValue
            // 
            this.txtLeftValue.BackColor = System.Drawing.SystemColors.Window;
            this.txtLeftValue.Enabled = false;
            this.txtLeftValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtLeftValue.Location = new System.Drawing.Point( 56, 51 );
            this.txtLeftValue.Name = "txtLeftValue";
            this.txtLeftValue.Size = new System.Drawing.Size( 71, 19 );
            this.txtLeftValue.TabIndex = 5;
            this.txtLeftValue.Text = "0";
            this.txtLeftValue.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // FormBezierPointEdit
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 463, 201 );
            this.Controls.Add( this.btnForward );
            this.Controls.Add( this.btnBackward );
            this.Controls.Add( this.groupRight );
            this.Controls.Add( this.groupDataPoint );
            this.Controls.Add( this.groupLeft );
            this.Controls.Add( this.chkEnableSmooth );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBezierPointEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Bezier Data Point";
            this.groupLeft.ResumeLayout( false );
            this.groupLeft.PerformLayout();
            this.groupDataPoint.ResumeLayout( false );
            this.groupDataPoint.PerformLayout();
            this.groupRight.ResumeLayout( false );
            this.groupRight.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkEnableSmooth;
        private System.Windows.Forms.Label lblLeftValue;
        private System.Windows.Forms.Label lblLeftClock;
        private NumberTextBox txtLeftValue;
        private NumberTextBox txtLeftClock;
        private System.Windows.Forms.GroupBox groupLeft;
        private System.Windows.Forms.GroupBox groupDataPoint;
        private System.Windows.Forms.Label lblDataPointValue;
        private NumberTextBox txtDataPointClock;
        private System.Windows.Forms.Label lblDataPointClock;
        private NumberTextBox txtDataPointValue;
        private System.Windows.Forms.GroupBox groupRight;
        private System.Windows.Forms.Label lblRightValue;
        private NumberTextBox txtRightClock;
        private System.Windows.Forms.Label lblRightClock;
        private NumberTextBox txtRightValue;
        private System.Windows.Forms.Button btnDataPoint;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnBackward;
        private System.Windows.Forms.Button btnForward;
    }
}