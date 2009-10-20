/*
 * VolumeTracker.Designer.cs
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
    partial class VolumeTracker {
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.trackFeder = new System.Windows.Forms.TrackBar();
            this.trackPanpot = new System.Windows.Forms.TrackBar();
            this.txtPanpot = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtFeder = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackFeder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPanpot)).BeginInit();
            this.SuspendLayout();
            // 
            // trackFeder
            // 
            this.trackFeder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackFeder.AutoSize = false;
            this.trackFeder.Location = new System.Drawing.Point( 21, 35 );
            this.trackFeder.Maximum = 151;
            this.trackFeder.Minimum = 26;
            this.trackFeder.Name = "trackFeder";
            this.trackFeder.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackFeder.Size = new System.Drawing.Size( 45, 144 );
            this.trackFeder.TabIndex = 0;
            this.trackFeder.TickFrequency = 10;
            this.trackFeder.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackFeder.Value = 100;
            this.trackFeder.ValueChanged += new System.EventHandler( this.trackFeder_ValueChanged );
            // 
            // trackPanpot
            // 
            this.trackPanpot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackPanpot.AutoSize = false;
            this.trackPanpot.Location = new System.Drawing.Point( 3, 185 );
            this.trackPanpot.Margin = new System.Windows.Forms.Padding( 3, 3, 3, 0 );
            this.trackPanpot.Maximum = 64;
            this.trackPanpot.Minimum = -64;
            this.trackPanpot.Name = "trackPanpot";
            this.trackPanpot.Size = new System.Drawing.Size( 79, 21 );
            this.trackPanpot.TabIndex = 2;
            this.trackPanpot.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackPanpot.ValueChanged += new System.EventHandler( this.trackPanpot_ValueChanged );
            // 
            // lblPanpot
            // 
            this.txtPanpot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPanpot.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))) );
            this.txtPanpot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPanpot.Location = new System.Drawing.Point( 10, 206 );
            this.txtPanpot.Margin = new System.Windows.Forms.Padding( 10, 0, 10, 0 );
            this.txtPanpot.Name = "lblPanpot";
            this.txtPanpot.Size = new System.Drawing.Size( 65, 19 );
            this.txtPanpot.TabIndex = 3;
            this.txtPanpot.Text = "0";
            this.txtPanpot.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPanpot.KeyDown += new System.Windows.Forms.KeyEventHandler( this.txtPanpot_KeyDown );
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.White;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTitle.Location = new System.Drawing.Point( 0, 238 );
            this.lblTitle.Margin = new System.Windows.Forms.Padding( 0 );
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size( 85, 23 );
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "TITLE";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFeder
            // 
            this.txtFeder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFeder.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))) );
            this.txtFeder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFeder.Location = new System.Drawing.Point( 3, 10 );
            this.txtFeder.Name = "txtFeder";
            this.txtFeder.Size = new System.Drawing.Size( 79, 19 );
            this.txtFeder.TabIndex = 5;
            this.txtFeder.Text = "0";
            this.txtFeder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtFeder.KeyDown += new System.Windows.Forms.KeyEventHandler( this.txtFeder_KeyDown );
            // 
            // VolumeTracker
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))) );
            this.Controls.Add( this.txtFeder );
            this.Controls.Add( this.lblTitle );
            this.Controls.Add( this.txtPanpot );
            this.Controls.Add( this.trackPanpot );
            this.Controls.Add( this.trackFeder );
            this.DoubleBuffered = true;
            this.Name = "VolumeTracker";
            this.Size = new System.Drawing.Size( 85, 261 );
            this.Resize += new System.EventHandler( this.VolumeTracker_Resize );
            ((System.ComponentModel.ISupportInitialize)(this.trackFeder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPanpot)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackFeder;
        private System.Windows.Forms.TrackBar trackPanpot;
        private System.Windows.Forms.TextBox txtPanpot;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtFeder;
    }
}
