using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

using cadencii;
using cadencii.vsq;
using cadencii;
using cadencii.java.util;
using cadencii.windows.forms;



namespace cadencii
{

    public partial class DivideNote : Form, IPaletteTool
    {
        public static int Numerator = 1;
        public static int Denominator = 32;
        public static string Modifier = "Control";

        private Bitmap m_icon = null;
        private Label lblLength;
        private Button btnOK;
        private Button btnCancel;
        private ComboBox comboDenominator;
        private static string iconbase64 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAOpJ" +
                     "REFUeNqcU8sKgzAQ3GgQesi10I8q9Cv6Zx78Ko+FnoQefEXbnQWDxrXaDiyJs7PjZo2mKIorEV3oPzwoz/P7OwKbrjgNqLV93xPv" +
                     "JQBjDHnvF5wG6FArBuM4Lgxi7qtB27YiRgBJklDMaZh0wWAYhpDUOA2qAVprmmbXADoxgBjC2GDObRlAZ+O34Wx1Xe92AJ0YxOI0" +
                     "TQ8dATrUrjrA/sgQkQ8dzM+LFQZ7M0BOOqiqSi7M9M2x77puwWlAHrW2LEsZSJZlYTi4YXNua4ioNc65Gz+c//kVucOn4fXE4Tjs" +
                     "j/We4/URYADaX0JH423PoQAAAABJRU5ErkJggg==";
        private Label lblNumerator;
        private Label lblDenominator;
        private TextBox txtNumerator;
        private Label lblModifierKey;
        private ComboBox comboKeys;
        private static int[] _DENOMI = new int[] { 1, 2, 4, 8, 16, 32, 64, 128 };
        private static string[] MODIFIER = new string[] { "None", "Control", "Shift", "Alt" };

        public DivideNote()
        {
            InitializeComponent();
            comboDenominator.Items.Clear();
            for (int i = 0; i < _DENOMI.Length; i++) {
                comboDenominator.Items.Add(_DENOMI[i].ToString());
            }
            txtNumerator.Text = "1";

            // ショートカットキー用のコンボボックスを更新
            comboKeys.Items.Clear();
            int selected = -1;
            int count = -1;
            for (int i = 0; i < MODIFIER.Length; i++) {
                string item = MODIFIER[i];
                comboKeys.Items.Add(item);
                count++;
                if (item.Equals(getModifier())) {
                    selected = count;
                }
            }
            if (selected >= 0) {
                comboKeys.SelectedIndex = selected;
            }

            // アイコンを作成
            byte[] b = Base64.decode(iconbase64);
            using (MemoryStream ms = new MemoryStream(b)) {
                m_icon = new Bitmap(ms);
            }
        }

        private static string getModifier()
        {
            if (Modifier == null) {
                Modifier = "Control";
            }
            return Modifier;
        }

        public void applyLanguage(string language)
        {
            this.Text = getName(language);
            if (language == "ja") {
                lblDenominator.Text = "分母";
                lblNumerator.Text = "分子";
                lblLength.Text = "最初の音符の長さ";
                btnCancel.Text = "キャンセル";
                btnOK.Text = "OK";
                lblModifierKey.Text = "[子音+母音]+[母音]分割操作のための修飾キー";
            } else {
                lblDenominator.Text = "Denominator";
                lblNumerator.Text = "Numerator";
                lblLength.Text = "Length of precursor note";
                btnCancel.Text = "Cancel";
                btnOK.Text = "OK";
                lblModifierKey.Text = "Modifier key for separating [consonant+vowel]+[vowel]";
            }
        }

        public bool edit(VsqTrack track, int[] event_internal_ids, MouseButtons button)
        {
            bool edited = false;
            try {
                int divide_threshold = Numerator * 480 * 4 / Denominator;
                Console.WriteLine("s_divide_threshold=" + divide_threshold);
                Keys modifier = Control.ModifierKeys;
                bool middle_mode = button == MouseButtons.Middle;
                if (getModifier().Equals("Alt")) {
                    if ((modifier & Keys.Alt) == Keys.Alt) {
                        middle_mode = true;
                    }
                } else if (getModifier().Equals("Control")) {
                    if ((modifier & Keys.Control) == Keys.Control) {
                        middle_mode = true;
                    }
                } else if (getModifier().Equals("Shift")) {
                    if ((modifier & Keys.Shift) == Keys.Shift) {
                        middle_mode = true;
                    }
                }
                Console.WriteLine("DivideNote#edit; (event_internal_ids==null)=" + (event_internal_ids == null));
                foreach (int id in event_internal_ids) {
                    Console.WriteLine("DivideNote#edit; (track==null)=" + (track == null));
                    foreach (var ve in track.getNoteEventIterator()) {
                        Console.WriteLine("DivideNote#edit; (ve==null)=" + (ve == null));
                        if (ve.InternalID == id) {
                            Console.WriteLine("DivideNote#edit; (ve.ID==null)=" + (ve.ID == null));
                            if (ve.ID.Length >= divide_threshold * 2) {
                                Console.WriteLine("before; clock=" + ve.Clock + "; length=" + ve.ID.Length);
                                VsqEvent add = (VsqEvent)ve.clone();
                                int length = ve.ID.Length;
                                List<string> symbol = ve.ID.LyricHandle.L0.getPhoneticSymbolList();
                                for (int i = 0; i < symbol.Count; i++) {
                                    Console.WriteLine("symbol[" + i + "]=" + symbol[i]);
                                }
                                ve.ID.Length = divide_threshold;
                                add.Clock = ve.Clock + divide_threshold;
                                add.ID.Length = length - divide_threshold;
                                if (add.ID.VibratoHandle != null) {
                                    if (add.ID.VibratoDelay >= add.ID.Length) {
                                        add.ID.VibratoHandle = null;
                                    }
                                }
                                if (ve.ID.VibratoHandle != null) {
                                    if (ve.ID.VibratoDelay >= ve.ID.Length) {
                                        ve.ID.VibratoHandle = null;
                                    }
                                }
                                if (symbol.Count >= 2) {
                                    if (middle_mode && !VsqPhoneticSymbol.isConsonant(symbol[1])) {
                                        ve.ID.LyricHandle.L0.setPhoneticSymbol(symbol[0] + " " + symbol[1]);
                                    } else {
                                        ve.ID.LyricHandle.L0.setPhoneticSymbol(symbol[0]);
                                    }
                                    string symbol2 = "";
                                    for (int i = 1; i < symbol.Count; i++) {
                                        symbol2 += ((i == 1) ? "" : " ") + symbol[i];
                                    }
                                    Console.WriteLine("symbol2=" + symbol2);
                                    add.ID.LyricHandle.L0.setPhoneticSymbol(symbol2);
                                }
                                track.addEvent(add);
                                edited = true;
                            }
                            break;
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("DivideNote#edit; ex=" + ex);
            }
            return edited;
        }

        public string getName(string language)
        {
            if (language.ToLower() == "ja") {
                return "音符分割";
            } else {
                return "Devide Note";
            }
        }

        public string getDescription(string language)
        {
            if (language.ToLower() == "ja") {
                return "音符を「母音-母音」または「子音-母音」のペアに分割します";
            } else {
                return "devides note into a phoneme pair, 'vowel-vowel' or 'consonant-vowel'";
            }
        }

        public bool hasDialog()
        {
            return true;
        }

        public DialogResult openDialog()
        {
            int num = Numerator;
            int den = Denominator;
            string key = getModifier();
            DialogResult ret = this.ShowDialog();
            if (ret != DialogResult.OK) {
                // 元に戻す
                Numerator = num;
                Denominator = den;
                Modifier = key;
            }
            return ret;
        }

        public Bitmap getIcon()
        {
            return m_icon;
        }

        private void InitializeComponent()
        {
            this.lblLength = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comboDenominator = new System.Windows.Forms.ComboBox();
            this.lblNumerator = new System.Windows.Forms.Label();
            this.lblDenominator = new System.Windows.Forms.Label();
            this.txtNumerator = new System.Windows.Forms.TextBox();
            this.lblModifierKey = new System.Windows.Forms.Label();
            this.comboKeys = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblLength
            // 
            this.lblLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLength.AutoEllipsis = true;
            this.lblLength.Location = new System.Drawing.Point(12, 18);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(282, 19);
            this.lblLength.TabIndex = 0;
            this.lblLength.Text = "Length of precursor note";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(112, 165);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(206, 165);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // comboDenominator
            // 
            this.comboDenominator.FormattingEnabled = true;
            this.comboDenominator.Location = new System.Drawing.Point(101, 67);
            this.comboDenominator.Name = "comboDenominator";
            this.comboDenominator.Size = new System.Drawing.Size(108, 20);
            this.comboDenominator.TabIndex = 2;
            this.comboDenominator.SelectedIndexChanged += new System.EventHandler(this.comboLength_SelectedIndexChanged);
            // 
            // lblNumerator
            // 
            this.lblNumerator.AutoSize = true;
            this.lblNumerator.Location = new System.Drawing.Point(26, 43);
            this.lblNumerator.Name = "lblNumerator";
            this.lblNumerator.Size = new System.Drawing.Size(58, 12);
            this.lblNumerator.TabIndex = 29;
            this.lblNumerator.Text = "Numerator";
            // 
            // lblDenominator
            // 
            this.lblDenominator.AutoSize = true;
            this.lblDenominator.Location = new System.Drawing.Point(26, 70);
            this.lblDenominator.Name = "lblDenominator";
            this.lblDenominator.Size = new System.Drawing.Size(69, 12);
            this.lblDenominator.TabIndex = 30;
            this.lblDenominator.Text = "Denominator";
            // 
            // txtNumerator
            // 
            this.txtNumerator.Location = new System.Drawing.Point(101, 40);
            this.txtNumerator.Name = "txtNumerator";
            this.txtNumerator.Size = new System.Drawing.Size(108, 19);
            this.txtNumerator.TabIndex = 1;
            this.txtNumerator.Text = "1";
            this.txtNumerator.TextChanged += new System.EventHandler(this.txtNumerator_TextChanged);
            // 
            // lblModifierKey
            // 
            this.lblModifierKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModifierKey.AutoEllipsis = true;
            this.lblModifierKey.Location = new System.Drawing.Point(12, 103);
            this.lblModifierKey.Name = "lblModifierKey";
            this.lblModifierKey.Size = new System.Drawing.Size(282, 23);
            this.lblModifierKey.TabIndex = 31;
            this.lblModifierKey.Text = "Modifier key for separating [consonant+vowel][vowel]";
            // 
            // comboKeys
            // 
            this.comboKeys.FormattingEnabled = true;
            this.comboKeys.Location = new System.Drawing.Point(28, 125);
            this.comboKeys.Name = "comboKeys";
            this.comboKeys.Size = new System.Drawing.Size(108, 20);
            this.comboKeys.TabIndex = 32;
            this.comboKeys.SelectedIndexChanged += new System.EventHandler(this.comboKeys_SelectedIndexChanged);
            // 
            // DivideNote
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(306, 200);
            this.Controls.Add(this.comboKeys);
            this.Controls.Add(this.lblModifierKey);
            this.Controls.Add(this.txtNumerator);
            this.Controls.Add(this.lblDenominator);
            this.Controls.Add(this.lblNumerator);
            this.Controls.Add(this.comboDenominator);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblLength);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DivideNote";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Divide Note";
            this.Load += new System.EventHandler(this.DivideNote_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void comboLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboDenominator.SelectedIndex < 0) {
                Denominator = 32;
                for (int i = 0; i < _DENOMI.Length; i++) {
                    if (_DENOMI[i] == 32) {
                        comboDenominator.SelectedIndex = i;
                        break;
                    }
                }
            } else {
                Denominator = _DENOMI[comboDenominator.SelectedIndex];
            }
        }

        private void DivideNote_Load(object sender, EventArgs e)
        {
            txtNumerator.Text = Numerator.ToString();
            for (int i = 0; i < _DENOMI.Length; i++) {
                if (Denominator == _DENOMI[i]) {
                    comboDenominator.SelectedIndex = i;
                    break;
                }
            }
        }

        private void txtNumerator_TextChanged(object sender, EventArgs e)
        {
            int v = Numerator;
            if (int.TryParse(txtNumerator.Text, out v)) {
                Numerator = v;
            }
        }

        private void comboKeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboKeys.SelectedIndex;
            if (index < 0) {
                return;
            }
            Modifier = (string)comboKeys.Items[index];
        }
    }

}
