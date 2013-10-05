using System.Collections.Generic;
using cadencii;

public class Hamori : System.Windows.Forms.Form
{
    private System.Windows.Forms.Button btnExecute;
    private System.Windows.Forms.ComboBox cbbSlide;
    private System.Windows.Forms.ComboBox cbbBaseCode;
    private System.ComponentModel.IContainer components = null;

    public Hamori()
    {
        InitializeComponent();
        // プロパティウィンドウではComboBox#SelectedIndexは設定できないみたいなのでここで指定
        this.cbbBaseCode.SelectedIndex = 0;
        this.cbbSlide.SelectedIndex = 0;
    }

    public static bool Edit(cadencii.vsq.VsqFile vsq)
    {
        using (Hamori d = new Hamori()) {
            if (d.ShowDialog() != System.Windows.Forms.DialogResult.OK) {
                return false;
            } else {
                hamori(vsq, d.cbbBaseCode.SelectedIndex, d.cbbSlide.SelectedIndex);
                return true;
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.btnExecute = new System.Windows.Forms.Button();
        this.cbbSlide = new System.Windows.Forms.ComboBox();
        this.cbbBaseCode = new System.Windows.Forms.ComboBox();
        this.SuspendLayout();
        // 
        // btnExecute
        // 
        this.btnExecute.Location = new System.Drawing.Point(235, 4);
        this.btnExecute.Name = "btnExecute";
        this.btnExecute.Size = new System.Drawing.Size(50, 20);
        this.btnExecute.TabIndex = 7;
        this.btnExecute.Text = "実行";
        this.btnExecute.UseVisualStyleBackColor = true;
        this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
        // 
        // cbbSlide
        // 
        this.cbbSlide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbSlide.Items.AddRange(new object[] {
            "3度上",
            "5度上",
            "4度下"});
        this.cbbSlide.Location = new System.Drawing.Point(140, 4);
        this.cbbSlide.Name = "cbbSlide";
        this.cbbSlide.Size = new System.Drawing.Size(62, 20);
        this.cbbSlide.TabIndex = 6;
        // 
        // cbbBaseCode
        // 
        this.cbbBaseCode.DisplayMember = "0";
        this.cbbBaseCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbBaseCode.Items.AddRange(new object[] {
            "C  Major / A  minor",
            "C# Major / A# minor",
            "D  Major / B  minor",
            "Eb Major / C  minor",
            "E  Major / C# minor",
            "F  Major / D  minor",
            "F# Major / Eb minor",
            "G  Major / E  minor",
            "G# Major / F  minor",
            "A  Major / F# minor",
            "Bb Major / G  minor",
            "B  Major / G# minor"});
        this.cbbBaseCode.Location = new System.Drawing.Point(6, 4);
        this.cbbBaseCode.Name = "cbbBaseCode";
        this.cbbBaseCode.Size = new System.Drawing.Size(130, 20);
        this.cbbBaseCode.TabIndex = 5;
        // 
        // Hamori
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(292, 29);
        this.Controls.Add(this.btnExecute);
        this.Controls.Add(this.cbbSlide);
        this.Controls.Add(this.cbbBaseCode);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "Hamori";
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.Text = "ハモリ";
        this.ResumeLayout(false);

    }

    private static void hamori(cadencii.vsq.VsqFile vsq, int basecode, int opt)
    {
        // opt : 0 -> 3度上, 1 -> 5度上, 2 -> 4度下
        //            4(or 3)     7           -5
        // 3度上(Cmaj)
        // C     D     E  F     G     A     B
        // 4, 4, 3, 3, 3, 4, 4, 4, 4, 3, 3, 3
        int step = (new int[] { 4, 7, -5 })[opt];

        int note;
        Dictionary<int, int> target_ids = new Dictionary<int, int>();
        foreach (var see in AppManager.itemSelection.getEventIterator()) {
            target_ids.Add(see.original.InternalID, 0);
        }
        int track = AppManager.getSelected();
        int tmp;
        if (opt == 0) {
            for (int j = 0; j < vsq.Track[track].getEventCount(); j++) {
                cadencii.vsq.VsqEvent item = vsq.Track[track].getEvent(j);
                if (item.ID.type == cadencii.vsq.VsqIDType.Anote && target_ids.ContainsKey(item.InternalID)) {
                    tmp = (item.ID.Note + 12 - basecode) % 12;
                    step = ((1 < tmp && tmp < 5) || 8 < tmp) ? 3 : 4;
                    note = item.ID.Note + step;
                    if (note < 0)
                        note = 0;
                    if (127 < note)
                        note = 127;
                    item.ID.Note = note;
                }
            }
        } else {
            for (int j = 0; j < vsq.Track[track].getEventCount(); j++) {
                cadencii.vsq.VsqEvent item = vsq.Track[track].getEvent(j);
                if (item.ID.type == cadencii.vsq.VsqIDType.Anote && target_ids.ContainsKey(item.InternalID)) {
                    tmp = (item.ID.Note + 12 - basecode) % 12;
                    tmp = tmp == 11 ? step - 1 : step;
                    note = item.ID.Note + tmp;
                    if (note < 0)
                        note = 0;
                    if (127 < note)
                        note = 127;
                    item.ID.Note = note;
                }
            }
        }
    }

    private void btnExecute_Click(object sender, System.EventArgs e)
    {
        this.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.Close();
    }
}

