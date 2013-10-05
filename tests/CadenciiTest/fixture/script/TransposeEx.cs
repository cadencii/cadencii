// TransposeEx.cs for Cadencii 3.1.4
using System.Windows.Forms;
using org.kbinani.vsq;

public class TransposeEx : Form
{
    private Button btnExec;
    private NumericUpDown numUpDown;

    public TransposeEx()
    {
        InitializeComponent();
    }

    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.btnExec = new Button();
        this.numUpDown = new NumericUpDown();
        ((System.ComponentModel.ISupportInitialize)(this.numUpDown)).BeginInit();
        this.SuspendLayout();
        //
        // btnExec
        //
        this.btnExec.Location = new System.Drawing.Point(142, 6);
        this.btnExec.Name = "btnExec";
        this.btnExec.Size = new System.Drawing.Size(66, 20);
        this.btnExec.TabIndex = 1;
        this.btnExec.Text = "Execute";
        this.btnExec.UseVisualStyleBackColor = true;
        this.btnExec.Click += new System.EventHandler(this.btnExec_Click);
        //
        // numUpDown
        //
        this.numUpDown.Location = new System.Drawing.Point(12, 6);
        this.numUpDown.Maximum = 2;
        this.numUpDown.Minimum = -2;
        this.numUpDown.Name = "numUpDown";
        this.numUpDown.Size = new System.Drawing.Size(77, 19);
        this.numUpDown.TabIndex = 2;
        this.numUpDown.Value = 1;
        //
        // mainForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(220, 31);
        this.Controls.Add(this.numUpDown);
        this.Controls.Add(this.btnExec);
        this.Name = "mainForm";
        this.Text = "Transpose";
        ((System.ComponentModel.ISupportInitialize)(this.numUpDown)).EndInit();
        this.ResumeLayout(false);
    }

    private void btnExec_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    //---------------------------------------------
    public static bool Edit(VsqFile vsq)
    {
        using (TransposeEx d = new TransposeEx()) {
            if (d.ShowDialog() != DialogResult.OK) {
                return false;
            } else {
                int step = (int)d.numUpDown.Value;
                if (step != 0) transpose(vsq, step);
                return true;
            }
        }
    }

    private static void transpose(VsqFile vsq, int step)
    {
        int note;
        VsqTrack track = vsq.Track[AppManager.Selected];
        for (Iterator<SelectedEventEntry> itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
            VsqEvent item = track.findEventFromID(((SelectedEventEntry)itr.next()).original.InternalID);
            if (item.ID.type == VsqIDType.Anote) {
                note = item.ID.Note + step;
                if (note < 0) note = 0;
                if (127 < note) note = 127;
                item.ID.Note = note;
            }
        }
    }
}
