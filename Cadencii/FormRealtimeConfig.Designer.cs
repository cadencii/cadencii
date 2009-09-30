/*
 * FormRealtimeConfig.Designer.cs
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
    partial class FormRealtimeConfig {
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
            this.timer = new System.Windows.Forms.Timer( this.components );
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblRealTimeInput = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.numSpeed = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 10;
            this.timer.Tick += new System.EventHandler( this.timer_Tick );
            // 
            // btnStart
            // 
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point( 25, 52 );
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size( 120, 33 );
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler( this.btnStart_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point( 196, 52 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 120, 33 );
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblRealTimeInput
            // 
            this.lblRealTimeInput.Font = new System.Drawing.Font( "Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.lblRealTimeInput.Location = new System.Drawing.Point( 23, 9 );
            this.lblRealTimeInput.Name = "lblRealTimeInput";
            this.lblRealTimeInput.Size = new System.Drawing.Size( 293, 28 );
            this.lblRealTimeInput.TabIndex = 2;
            this.lblRealTimeInput.Text = "Realtime Input";
            this.lblRealTimeInput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point( 49, 114 );
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size( 36, 12 );
            this.lblSpeed.TabIndex = 3;
            this.lblSpeed.Text = "Speed";
            // 
            // numSpeed
            // 
            this.numSpeed.DecimalPlaces = 1;
            this.numSpeed.Increment = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
            this.numSpeed.Location = new System.Drawing.Point( 107, 112 );
            this.numSpeed.Maximum = new decimal( new int[] {
            30,
            0,
            0,
            65536} );
            this.numSpeed.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
            this.numSpeed.Name = "numSpeed";
            this.numSpeed.Size = new System.Drawing.Size( 120, 19 );
            this.numSpeed.TabIndex = 4;
            this.numSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSpeed.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // FormRealtimeConfig
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 347, 161 );
            this.Controls.Add( this.numSpeed );
            this.Controls.Add( this.lblSpeed );
            this.Controls.Add( this.lblRealTimeInput );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnStart );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRealtimeConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormRealtimeConfig";
            this.Load += new System.EventHandler( this.FormRealtimeConfig_Load );
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRealTimeInput;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.NumericUpDown numSpeed;
    }
}