/*
 * FormGameControlerConfig.Designer.cs
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
namespace Boare.Cadencii {
    using boolean = System.Boolean;
    partial class FormGameControlerConfig {
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer( this.components );
            this.pictButton = new System.Windows.Forms.PictureBox();
            this.progressCount = new System.Windows.Forms.ProgressBar();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictButton)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point( 15, 17 );
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size( 9, 12 );
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = " ";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler( this.timer_Tick );
            // 
            // pictButton
            // 
            this.pictButton.Location = new System.Drawing.Point( 12, 49 );
            this.pictButton.Name = "pictButton";
            this.pictButton.Size = new System.Drawing.Size( 316, 36 );
            this.pictButton.TabIndex = 1;
            this.pictButton.TabStop = false;
            // 
            // progressCount
            // 
            this.progressCount.Location = new System.Drawing.Point( 12, 101 );
            this.progressCount.Maximum = 8;
            this.progressCount.Name = "progressCount";
            this.progressCount.Size = new System.Drawing.Size( 316, 13 );
            this.progressCount.TabIndex = 2;
            this.progressCount.Value = 1;
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point( 27, 127 );
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size( 75, 23 );
            this.btnSkip.TabIndex = 3;
            this.btnSkip.Text = "Skip";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler( this.btnSkip_Click );
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point( 172, 159 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 253, 159 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnReset.Location = new System.Drawing.Point( 197, 127 );
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size( 131, 23 );
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Reset And Exit";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler( this.btnReset_Click );
            // 
            // FormGameControlerConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 340, 200 );
            this.Controls.Add( this.btnReset );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnSkip );
            this.Controls.Add( this.progressCount );
            this.Controls.Add( this.pictButton );
            this.Controls.Add( this.lblMessage );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGameControlerConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Game Controler Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.pictButton)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.PictureBox pictButton;
        private System.Windows.Forms.ProgressBar progressCount;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;

    }
}