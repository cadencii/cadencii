/*
 * FormGenerateStf.Designer.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.EditOtoIni.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace Boare.EditOtoIni {
    partial class FormGenerateStf {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( bool disposing ) {
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
            this.lblPercent = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.bgWork = new System.ComponentModel.BackgroundWorker();
            this.lblTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point( 12, 17 );
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size( 21, 12 );
            this.lblPercent.TabIndex = 0;
            this.lblPercent.Text = "0 %";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point( 14, 50 );
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size( 345, 23 );
            this.progressBar.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 284, 86 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler( this.btnCancel_Click );
            // 
            // bgWork
            // 
            this.bgWork.WorkerReportsProgress = true;
            this.bgWork.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWork_DoWork );
            this.bgWork.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.bgWork_RunWorkerCompleted );
            this.bgWork.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler( this.bgWork_ProgressChanged );
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point( 12, 35 );
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size( 137, 12 );
            this.lblTime.TabIndex = 3;
            this.lblTime.Text = "remaining 0s (elapsed 0s)";
            // 
            // FormGenerateStf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 371, 121 );
            this.Controls.Add( this.lblTime );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.progressBar );
            this.Controls.Add( this.lblPercent );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGenerateStf";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormGenerateStf";
            this.Load += new System.EventHandler( this.FormGenerateStf_Load );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnCancel;
        private System.ComponentModel.BackgroundWorker bgWork;
        private System.Windows.Forms.Label lblTime;
    }
}