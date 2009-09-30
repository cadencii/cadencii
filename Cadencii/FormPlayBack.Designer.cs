/*
 * FormPlayBack.Designer.cs
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
    partial class FormPlayBack {
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
            this.txtCalcNum = new System.Windows.Forms.TextBox();
            this.btnRecalculation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtCalcNum
            // 
            this.txtCalcNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCalcNum.Location = new System.Drawing.Point( 300, 12 );
            this.txtCalcNum.Name = "txtCalcNum";
            this.txtCalcNum.Size = new System.Drawing.Size( 72, 19 );
            this.txtCalcNum.TabIndex = 0;
            this.txtCalcNum.Text = "100";
            // 
            // btnRecalculation
            // 
            this.btnRecalculation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecalculation.Location = new System.Drawing.Point( 297, 37 );
            this.btnRecalculation.Name = "btnRecalculation";
            this.btnRecalculation.Size = new System.Drawing.Size( 75, 23 );
            this.btnRecalculation.TabIndex = 1;
            this.btnRecalculation.Text = "再計算";
            this.btnRecalculation.UseVisualStyleBackColor = true;
            this.btnRecalculation.Click += new System.EventHandler( this.btnRecalculation_Click );
            // 
            // FormPlayBack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 384, 357 );
            this.Controls.Add( this.btnRecalculation );
            this.Controls.Add( this.txtCalcNum );
            this.DoubleBuffered = true;
            this.Name = "FormPlayBack";
            this.Text = "FormPlayBack";
            this.Paint += new System.Windows.Forms.PaintEventHandler( this.FormPlayBack_Paint );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCalcNum;
        private System.Windows.Forms.Button btnRecalculation;
    }
}