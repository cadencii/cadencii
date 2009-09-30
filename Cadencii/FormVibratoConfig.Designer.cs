/*
 * FormVibratoConfig.Designer.cs
 * Copyright (c) 2008 kbinani
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
    partial class FormVibratoConfig {
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
            this.lblVibratoLength = new System.Windows.Forms.Label();
            this.lblVibratoType = new System.Windows.Forms.Label();
            this.txtVibratoLength = new Boare.Cadencii.NumberTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboVibratoType = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblVibratoLength
            // 
            this.lblVibratoLength.AutoSize = true;
            this.lblVibratoLength.Location = new System.Drawing.Point( 12, 15 );
            this.lblVibratoLength.Name = "lblVibratoLength";
            this.lblVibratoLength.Size = new System.Drawing.Size( 94, 12 );
            this.lblVibratoLength.TabIndex = 0;
            this.lblVibratoLength.Text = "Vibrato Length(&L)";
            // 
            // lblVibratoType
            // 
            this.lblVibratoType.AutoSize = true;
            this.lblVibratoType.Location = new System.Drawing.Point( 12, 38 );
            this.lblVibratoType.Name = "lblVibratoType";
            this.lblVibratoType.Size = new System.Drawing.Size( 86, 12 );
            this.lblVibratoType.TabIndex = 1;
            this.lblVibratoType.Text = "Vibrato Type(&T)";
            // 
            // txtVibratoLength
            // 
            this.txtVibratoLength.Enabled = false;
            this.txtVibratoLength.Location = new System.Drawing.Point( 143, 12 );
            this.txtVibratoLength.Name = "txtVibratoLength";
            this.txtVibratoLength.Size = new System.Drawing.Size( 61, 19 );
            this.txtVibratoLength.TabIndex = 2;
            this.txtVibratoLength.Type = Boare.Cadencii.NumberTextBox.ValueType.Integer;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 210, 15 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 49, 12 );
            this.label3.TabIndex = 3;
            this.label3.Text = "%(0-100)";
            // 
            // comboVibratoType
            // 
            this.comboVibratoType.FormattingEnabled = true;
            this.comboVibratoType.Location = new System.Drawing.Point( 143, 35 );
            this.comboVibratoType.Name = "comboVibratoType";
            this.comboVibratoType.Size = new System.Drawing.Size( 167, 20 );
            this.comboVibratoType.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 240, 71 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 159, 71 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            // 
            // FormVibratoConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 327, 106 );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.comboVibratoType );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.txtVibratoLength );
            this.Controls.Add( this.lblVibratoType );
            this.Controls.Add( this.lblVibratoLength );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVibratoConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Vibrato property";
            this.ResumeLayout( false );
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label lblVibratoLength;
        private System.Windows.Forms.Label lblVibratoType;
        private NumberTextBox txtVibratoLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboVibratoType;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}