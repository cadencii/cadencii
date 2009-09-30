/*
 * VersionInfo.Designer.cs
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
using System.Drawing;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    partial class VersionInfo {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing ) {
            if( disposing && (components != null) ) {
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
            this.btnFlip = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer( this.components );
            this.btnSaveAuthorList = new System.Windows.Forms.Button();
            this.lblVstLogo = new System.Windows.Forms.Label();
            this.pictVstLogo = new System.Windows.Forms.PictureBox();
            this.lblStraightAcknowledgement = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictVstLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFlip
            // 
            this.btnFlip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFlip.Location = new System.Drawing.Point( 13, 391 );
            this.btnFlip.Name = "btnFlip";
            this.btnFlip.Size = new System.Drawing.Size( 75, 21 );
            this.btnFlip.TabIndex = 2;
            this.btnFlip.Text = "クレジット";
            this.btnFlip.UseVisualStyleBackColor = true;
            this.btnFlip.Click += new System.EventHandler( this.btnFlip_Click );
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point( 211, 391 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 21 );
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            // 
            // timer
            // 
            this.timer.Interval = 30;
            this.timer.Tick += new System.EventHandler( this.timer_Tick );
            // 
            // btnSaveAuthorList
            // 
            this.btnSaveAuthorList.Location = new System.Drawing.Point( 123, 391 );
            this.btnSaveAuthorList.Name = "btnSaveAuthorList";
            this.btnSaveAuthorList.Size = new System.Drawing.Size( 43, 21 );
            this.btnSaveAuthorList.TabIndex = 3;
            this.btnSaveAuthorList.Text = "button1";
            this.btnSaveAuthorList.UseVisualStyleBackColor = true;
            this.btnSaveAuthorList.Visible = false;
            // 
            // lblVstLogo
            // 
            this.lblVstLogo.BackColor = System.Drawing.Color.White;
            this.lblVstLogo.Location = new System.Drawing.Point( 25, 238 );
            this.lblVstLogo.Name = "lblVstLogo";
            this.lblVstLogo.Size = new System.Drawing.Size( 263, 32 );
            this.lblVstLogo.TabIndex = 5;
            this.lblVstLogo.Text = "VST PlugIn Technology by Steinberg Media Technologies GmbH";
            // 
            // pictVstLogo
            // 
            this.pictVstLogo.BackColor = System.Drawing.Color.White;
            this.pictVstLogo.Image = global::Boare.Cadencii.Properties.Resources.VSTonWht;
            this.pictVstLogo.Location = new System.Drawing.Point( 27, 265 );
            this.pictVstLogo.Name = "pictVstLogo";
            this.pictVstLogo.Size = new System.Drawing.Size( 88, 60 );
            this.pictVstLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictVstLogo.TabIndex = 4;
            this.pictVstLogo.TabStop = false;
            // 
            // lblStraightAcknowledgement
            // 
            this.lblStraightAcknowledgement.BackColor = System.Drawing.Color.White;
            this.lblStraightAcknowledgement.Location = new System.Drawing.Point( 25, 328 );
            this.lblStraightAcknowledgement.Name = "lblStraightAcknowledgement";
            this.lblStraightAcknowledgement.Size = new System.Drawing.Size( 263, 40 );
            this.lblStraightAcknowledgement.TabIndex = 6;
            this.lblStraightAcknowledgement.Text = "Components of Cadencii, \"straightdrv.exe\" and \"straightVoiceDB.exe\", are powererd" +
                " by STRAIGHT LIBRARY.";
            // 
            // VersionInfo
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size( 300, 419 );
            this.Controls.Add( this.pictVstLogo );
            this.Controls.Add( this.lblStraightAcknowledgement );
            this.Controls.Add( this.lblVstLogo );
            this.Controls.Add( this.btnSaveAuthorList );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnFlip );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size( 306, 451 );
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size( 306, 451 );
            this.Name = "VersionInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VersionInfoEx";
            this.Paint += new System.Windows.Forms.PaintEventHandler( this.VersionInfoEx_Paint );
            this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.VersionInfoEx_KeyDown );
            this.FontChanged += new System.EventHandler( this.VersionInfoEx_FontChanged );
            ((System.ComponentModel.ISupportInitialize)(this.pictVstLogo)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button btnFlip;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnSaveAuthorList;
        private System.Windows.Forms.PictureBox pictVstLogo;
        private System.Windows.Forms.Label lblVstLogo;
        private System.Windows.Forms.Label lblStraightAcknowledgement;
    }
}