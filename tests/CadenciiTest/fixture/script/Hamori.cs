// Hamori.cs for Cadencii 3.1.4
using System.Windows.Forms;
using org.kbinani.vsq;

public class Hamori : Form
{
    private Button btnExecute;
    private ComboBox cbbSlide;
    private ComboBox cbbBaseCode;
    private System.ComponentModel.IContainer components = null;

    public Hamori()
    {
        InitializeComponent();
        // プロパティウィンドウではComboBox#SelectedIndexは設定できないみたいなのでここで指定
        this.cbbBaseCode.SelectedIndex = 0;
        this.cbbSlide.SelectedIndex = 4;
    }

    public static bool Edit(VsqFile vsq)
    {
        using (Hamori d = new Hamori()) {
            if (d.ShowDialog() != DialogResult.OK) {
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
        this.btnExecute = new Button();
        this.cbbSlide = new ComboBox();
        this.cbbBaseCode = new ComboBox();
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
        this.cbbSlide.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cbbSlide.Items.AddRange(new object[] {
            "7度上",
            "6度上",
            "5度上",
            "4度上",
            "3度上",
            "2度上",
            "",
            "2度下",
            "3度下",
            "4度下",
            "5度下",
            "6度下",
            "7度下"});
        this.cbbSlide.Location = new System.Drawing.Point(140, 4);
        this.cbbSlide.Name = "cbbSlide";
        this.cbbSlide.Size = new System.Drawing.Size(62, 20);
        this.cbbSlide.TabIndex = 6;
        // 
        // cbbBaseCode
        // 
        this.cbbBaseCode.DisplayMember = "0";
        this.cbbBaseCode.DropDownStyle = ComboBoxStyle.DropDownList;
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
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(292, 29);
        this.Controls.Add(this.btnExecute);
        this.Controls.Add(this.cbbSlide);
        this.Controls.Add(this.cbbBaseCode);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "Hamori";
        this.SizeGripStyle = SizeGripStyle.Hide;
        this.Text = "ハモリ";
        this.ResumeLayout(false);
    }

    private static void hamori(VsqFile vsq, int basecode, int opt)
    {
        int[][] steps = new int[][] {
        // C Major    C     D     E  F     G     A     B
        // 7度上
         new int[] { 11,11,10,10,10,11,11,10,10,10,10,10 },
        // 6度上
         new int[] {  9, 9, 9, 9, 8, 9, 9, 9, 9, 8, 8, 9 },
        // 5度上
         new int[] {  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 6 },
        // 4度上
         new int[] {  5, 5, 5, 5, 5, 6, 5, 5, 5, 5, 5, 5 },
        // 3度上
         new int[] {  4, 4, 3, 3, 3, 4, 4, 4, 4, 3, 3, 3 },
        // 2度上
         new int[] {  2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 }
        };
        int[] step;
        if (opt == 6) return;
        if (opt < 6)
            step = steps[opt];
        else {
            step = steps[opt - 7];
            for (int i = 0; i < 12; i++) step[i] -= 12;
        }

        int note, tmp;
        VsqTrack track = vsq.Track[AppManager.Selected];
        for (Iterator<SelectedEventEntry> itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
            VsqEvent item = track.findEventFromID(((SelectedEventEntry)itr.next()).original.InternalID);
            if (item.ID.type == VsqIDType.Anote) {
                tmp = (item.ID.Note + 12 - basecode) % 12;
                note = item.ID.Note + step[tmp];
                if (note < 0) note = 0;
                if (127 < note) note = 127;
                item.ID.Note = note;
            }
        }
    }

    private void btnExecute_Click(object sender, System.EventArgs e)
    {
        this.DialogResult = DialogResult.OK;
        this.Close();
    }
}
