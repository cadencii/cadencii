using System;
using System.Windows.Forms;
using cadencii.vsq;
using cadencii;
using System.Drawing;

class ResoXAmp : Form
{
    public static double AmplifyCoeffReso2 = 1.0;
    public static double AmplifyCoeffReso3 = 1.0;
    public static double AmplifyCoeffReso4 = 1.0;
    private Button btnOk;
    private Button btnCancel;
    private Label lblReso2Amp;
    private TextBox txtReso2Amp;
    private TextBox txtReso3Amp;
    private Label lblReso3Amp;
    private TextBox txtReso4Amp;
    private Label lblReso4Amp;

    public ResoXAmp()
    {
        InitializeComponent();
        txtReso2Amp.Text = AmplifyCoeffReso2 + "";
        txtReso3Amp.Text = AmplifyCoeffReso3 + "";
        txtReso4Amp.Text = AmplifyCoeffReso4 + "";
    }

    public static bool Edit(VsqFile vsq)
    {
        return edit(vsq);
    }

    public static bool edit(VsqFile vsq)
    {
        ResoXAmp form = new ResoXAmp();
        if (form.ShowDialog() != DialogResult.OK) {
            return false;
        }
        VsqTrack track = vsq.Track[AppManager.Selected];
        VsqBPList source = track.getCurve(CurveType.reso1amp.getName());
        VsqBPList reso2amp = (VsqBPList)track.getCurve(CurveType.reso2amp.getName()).clone();
        VsqBPList reso3amp = (VsqBPList)track.getCurve(CurveType.reso3amp.getName()).clone();
        VsqBPList reso4amp = (VsqBPList)track.getCurve(CurveType.reso4amp.getName()).clone();
        Console.WriteLine("AmplifyCoeffReso2=" + AmplifyCoeffReso2);
        Console.WriteLine("AmplifyCoeffReso3=" + AmplifyCoeffReso3);
        Console.WriteLine("AmplifyCoeffReso4=" + AmplifyCoeffReso4);
        amplify(source, reso2amp, AmplifyCoeffReso2);
        amplify(source, reso3amp, AmplifyCoeffReso3);
        amplify(source, reso4amp, AmplifyCoeffReso4);
        track.setCurve(CurveType.reso2amp.getName(), reso2amp);
        track.setCurve(CurveType.reso3amp.getName(), reso3amp);
        track.setCurve(CurveType.reso4amp.getName(), reso4amp);
        Console.WriteLine("reso2amp.getCount()=" + reso2amp.size());
        Console.WriteLine("reso3amp.getCount()=" + reso3amp.size());
        Console.WriteLine("reso4amp.getCount()=" + reso4amp.size());
        MessageBox.Show("done");
        return true;
    }

    private static void amplify(VsqBPList source, VsqBPList target, double amplify)
    {
        target.clear();
        int count = source.size();
        int min = target.getMinimum();
        int max = target.getMaximum();
        for (int i = 0; i < count; i++) {
            int clock = source.getKeyClock(i);
            int value = (int)(source.getElement(i) * amplify);
            if (value < min) {
                value = min;
            }
            if (max < value) {
                value = max;
            }
            target.add(clock, value);
        }
    }

    private void InitializeComponent()
    {
        this.btnOk = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.lblReso2Amp = new System.Windows.Forms.Label();
        this.txtReso2Amp = new System.Windows.Forms.TextBox();
        this.txtReso3Amp = new System.Windows.Forms.TextBox();
        this.lblReso3Amp = new System.Windows.Forms.Label();
        this.txtReso4Amp = new System.Windows.Forms.TextBox();
        this.lblReso4Amp = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // btnOk
        // 
        this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnOk.Location = new System.Drawing.Point(102, 102);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(75, 23);
        this.btnOk.TabIndex = 0;
        this.btnOk.Text = "OK";
        this.btnOk.UseVisualStyleBackColor = true;
        // 
        // btnCancel
        // 
        this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point(183, 102);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 23);
        this.btnCancel.TabIndex = 1;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        // 
        // lblReso2Amp
        // 
        this.lblReso2Amp.AutoSize = true;
        this.lblReso2Amp.Location = new System.Drawing.Point(12, 15);
        this.lblReso2Amp.Name = "lblReso2Amp";
        this.lblReso2Amp.Size = new System.Drawing.Size(127, 12);
        this.lblReso2Amp.TabIndex = 2;
        this.lblReso2Amp.Text = "reso2amp / reso1amp =";
        // 
        // txtReso2Amp
        // 
        this.txtReso2Amp.ForeColor = System.Drawing.SystemColors.WindowText;
        this.txtReso2Amp.Location = new System.Drawing.Point(145, 12);
        this.txtReso2Amp.Name = "txtReso2Amp";
        this.txtReso2Amp.Size = new System.Drawing.Size(100, 19);
        this.txtReso2Amp.TabIndex = 3;
        this.txtReso2Amp.Text = "1.0";
        this.txtReso2Amp.TextChanged += new System.EventHandler(this.txtReso2Amp_TextChanged);
        // 
        // txtReso3Amp
        // 
        this.txtReso3Amp.Location = new System.Drawing.Point(145, 37);
        this.txtReso3Amp.Name = "txtReso3Amp";
        this.txtReso3Amp.Size = new System.Drawing.Size(100, 19);
        this.txtReso3Amp.TabIndex = 5;
        this.txtReso3Amp.Text = "1.0";
        this.txtReso3Amp.TextChanged += new System.EventHandler(this.txtReso3Amp_TextChanged);
        // 
        // lblReso3Amp
        // 
        this.lblReso3Amp.AutoSize = true;
        this.lblReso3Amp.Location = new System.Drawing.Point(12, 40);
        this.lblReso3Amp.Name = "lblReso3Amp";
        this.lblReso3Amp.Size = new System.Drawing.Size(127, 12);
        this.lblReso3Amp.TabIndex = 4;
        this.lblReso3Amp.Text = "reso3amp / reso1amp =";
        // 
        // txtReso4Amp
        // 
        this.txtReso4Amp.Location = new System.Drawing.Point(145, 62);
        this.txtReso4Amp.Name = "txtReso4Amp";
        this.txtReso4Amp.Size = new System.Drawing.Size(100, 19);
        this.txtReso4Amp.TabIndex = 7;
        this.txtReso4Amp.Text = "1.0";
        this.txtReso4Amp.TextChanged += new System.EventHandler(this.txtReso4Amp_TextChanged);
        // 
        // lblReso4Amp
        // 
        this.lblReso4Amp.AutoSize = true;
        this.lblReso4Amp.Location = new System.Drawing.Point(12, 65);
        this.lblReso4Amp.Name = "lblReso4Amp";
        this.lblReso4Amp.Size = new System.Drawing.Size(127, 12);
        this.lblReso4Amp.TabIndex = 6;
        this.lblReso4Amp.Text = "reso4amp / reso1amp =";
        // 
        // ResoXAmp
        // 
        this.AcceptButton = this.btnOk;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(270, 137);
        this.Controls.Add(this.txtReso4Amp);
        this.Controls.Add(this.lblReso4Amp);
        this.Controls.Add(this.txtReso3Amp);
        this.Controls.Add(this.lblReso3Amp);
        this.Controls.Add(this.txtReso2Amp);
        this.Controls.Add(this.lblReso2Amp);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnOk);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.Name = "ResoXAmp";
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    private void txtReso2Amp_TextChanged(object sender, EventArgs e)
    {
        double draft = 1.0;
        if (double.TryParse(txtReso2Amp.Text, out draft)) {
            AmplifyCoeffReso2 = draft;
            txtReso2Amp.BackColor = SystemColors.Window;
            txtReso2Amp.ForeColor = SystemColors.WindowText;
        } else {
            txtReso2Amp.BackColor = Color.LightCoral;
            txtReso2Amp.ForeColor = Color.White;
        }
    }

    private void txtReso3Amp_TextChanged(object sender, EventArgs e)
    {
        double draft = 1.0;
        if (double.TryParse(txtReso3Amp.Text, out draft)) {
            AmplifyCoeffReso3 = draft;
            txtReso3Amp.BackColor = SystemColors.Window;
            txtReso3Amp.ForeColor = SystemColors.WindowText;
        } else {
            txtReso3Amp.BackColor = Color.LightCoral;
            txtReso3Amp.ForeColor = Color.White;
        }
    }

    private void txtReso4Amp_TextChanged(object sender, EventArgs e)
    {
        double draft = 1.0;
        if (double.TryParse(txtReso4Amp.Text, out draft)) {
            AmplifyCoeffReso4 = draft;
            txtReso4Amp.BackColor = SystemColors.Window;
            txtReso4Amp.ForeColor = SystemColors.WindowText;
        } else {
            txtReso4Amp.BackColor = Color.LightCoral;
            txtReso4Amp.ForeColor = Color.White;
        }
    }
}
