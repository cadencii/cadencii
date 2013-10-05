/*
 * BHScrollBar.Designer.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace cadencii.apputil
{
    partial class OBSOLUTE_BVScrollBar
    {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.vScroll = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            // 
            // hScroll
            // 
            this.vScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScroll.Location = new System.Drawing.Point(0, 0);
            this.vScroll.Name = "hScroll";
            this.vScroll.Size = new System.Drawing.Size(16, 205);
            this.vScroll.TabIndex = 0;
            this.vScroll.ValueChanged += new System.EventHandler(this.vScroll_ValueChanged);
            // 
            // BHScrollBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vScroll);
            this.Name = "BHScrollBar";
            this.Size = new System.Drawing.Size(16, 205);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar vScroll;

    }
}
