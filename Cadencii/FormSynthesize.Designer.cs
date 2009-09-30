/*
 * FormSynthesize.Designer.cs
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
using System;

namespace Boare.Cadencii {

    using boolean = Boolean;
    
    partial class FormSynthesize {
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
            this.components = new System.ComponentModel.Container();
            this.progressWhole = new System.Windows.Forms.ProgressBar();
            this.lblSynthesizing = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressOne = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.bgWork = new System.ComponentModel.BackgroundWorker();
            this.timer = new System.Windows.Forms.Timer( this.components );
            this.lblTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressWhole
            // 
            this.progressWhole.Location = new System.Drawing.Point( 12, 49 );
            this.progressWhole.Name = "progressWhole";
            this.progressWhole.Size = new System.Drawing.Size( 345, 23 );
            this.progressWhole.TabIndex = 0;
            // 
            // lblSynthesizing
            // 
            this.lblSynthesizing.AutoSize = true;
            this.lblSynthesizing.Location = new System.Drawing.Point( 12, 9 );
            this.lblSynthesizing.Name = "lblSynthesizing";
            this.lblSynthesizing.Size = new System.Drawing.Size( 98, 12 );
            this.lblSynthesizing.TabIndex = 1;
            this.lblSynthesizing.Text = "now synthesizing...";
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point( 336, 9 );
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size( 23, 12 );
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "1/1";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressOne
            // 
            this.progressOne.Location = new System.Drawing.Point( 12, 78 );
            this.progressOne.Name = "progressOne";
            this.progressOne.Size = new System.Drawing.Size( 345, 23 );
            this.progressOne.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 277, 113 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 80, 23 );
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // bgWork
            // 
            this.bgWork.WorkerReportsProgress = true;
            this.bgWork.WorkerSupportsCancellation = true;
            this.bgWork.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWork_DoWork );
            this.bgWork.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.bgWork_RunWorkerCompleted );
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler( this.timer_Tick );
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point( 12, 29 );
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size( 137, 12 );
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "remaining 0s (elapsed 0s)";
            // 
            // FormSynthesize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 371, 158 );
            this.Controls.Add( this.lblTime );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.progressOne );
            this.Controls.Add( this.lblProgress );
            this.Controls.Add( this.lblSynthesizing );
            this.Controls.Add( this.progressWhole );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSynthesize";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Synthesize";
            this.Load += new System.EventHandler( this.FormSynthesize_Load );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormSynthesize_FormClosing );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressWhole;
        private System.Windows.Forms.Label lblSynthesizing;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressOne;
        private System.Windows.Forms.Button btnCancel;
        private System.ComponentModel.BackgroundWorker bgWork;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lblTime;
    }
}