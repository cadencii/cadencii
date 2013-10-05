using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using cadencii.vsq;

public class Time_Shift : Form
{
    public Time_Shift()
    {
        InitializeComponent();
    }

    public static bool Edit(VsqFile vsq)
    {
        Time_Shift ts = new Time_Shift();
        if (ts.ShowDialog() != DialogResult.OK) {
            return false;
        }
        return true;
    }

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

    #region Windows フォーム デザイナで生成されたコード

    /// <summary>
    /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
    /// コード エディタで変更しないでください。
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Text = "Time_Shift";
    }

    #endregion

}
