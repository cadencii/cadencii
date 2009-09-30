/*
 * FormInsertBar.Designer.cs
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
    partial class FormInsertBar {
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
            this.numPosition = new System.Windows.Forms.NumericUpDown();
            this.numLength = new System.Windows.Forms.NumericUpDown();
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblLength = new System.Windows.Forms.Label();
            this.lblThBar = new System.Windows.Forms.Label();
            this.lblBar = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblPositionPrefix = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            this.SuspendLayout();
            // 
            // numPosition
            // 
            this.numPosition.Location = new System.Drawing.Point( 78, 12 );
            this.numPosition.Name = "numPosition";
            this.numPosition.Size = new System.Drawing.Size( 52, 19 );
            this.numPosition.TabIndex = 0;
            // 
            // numLength
            // 
            this.numLength.Location = new System.Drawing.Point( 78, 37 );
            this.numLength.Maximum = new decimal( new int[] {
            32,
            0,
            0,
            0} );
            this.numLength.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numLength.Name = "numLength";
            this.numLength.Size = new System.Drawing.Size( 52, 19 );
            this.numLength.TabIndex = 1;
            this.numLength.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point( 12, 14 );
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size( 46, 12 );
            this.lblPosition.TabIndex = 2;
            this.lblPosition.Text = "Position";
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point( 12, 39 );
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size( 39, 12 );
            this.lblLength.TabIndex = 3;
            this.lblLength.Text = "Length";
            // 
            // lblThBar
            // 
            this.lblThBar.AutoSize = true;
            this.lblThBar.Location = new System.Drawing.Point( 136, 14 );
            this.lblThBar.Name = "lblThBar";
            this.lblThBar.Size = new System.Drawing.Size( 35, 12 );
            this.lblThBar.TabIndex = 4;
            this.lblThBar.Text = "th bar";
            // 
            // lblBar
            // 
            this.lblBar.AutoSize = true;
            this.lblBar.Location = new System.Drawing.Point( 136, 39 );
            this.lblBar.Name = "lblBar";
            this.lblBar.Size = new System.Drawing.Size( 21, 12 );
            this.lblBar.TabIndex = 5;
            this.lblBar.Text = "bar";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 134, 67 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 53, 67 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            // 
            // lblPositionPrefix
            // 
            this.lblPositionPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPositionPrefix.AutoSize = true;
            this.lblPositionPrefix.Location = new System.Drawing.Point( 62, 14 );
            this.lblPositionPrefix.Name = "lblPositionPrefix";
            this.lblPositionPrefix.Size = new System.Drawing.Size( 0, 12 );
            this.lblPositionPrefix.TabIndex = 8;
            this.lblPositionPrefix.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FormInsertBar
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 221, 102 );
            this.Controls.Add( this.lblPositionPrefix );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.lblBar );
            this.Controls.Add( this.lblThBar );
            this.Controls.Add( this.lblLength );
            this.Controls.Add( this.lblPosition );
            this.Controls.Add( this.numLength );
            this.Controls.Add( this.numPosition );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInsertBar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Insert bar";
            ((System.ComponentModel.ISupportInitialize)(this.numPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numPosition;
        private System.Windows.Forms.NumericUpDown numLength;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.Label lblThBar;
        private System.Windows.Forms.Label lblBar;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblPositionPrefix;
    }
}