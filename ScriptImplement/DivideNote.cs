using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

using Boare.Cadencii;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.util;

namespace Boare.Cadencii {

    public partial class DivideNote : Form, IPaletteTool {
        private Bitmap m_icon = null;
        public static int Numerator = 1;
        public static int Denominator = 32;
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
        private static int[] _DENOMI = new int[] { 1, 2, 4, 8, 16, 32, 64, 128 };

        public DivideNote() {
            InitializeComponent();
            comboDenominator.Items.Clear();
            for ( int i = 0; i < _DENOMI.Length; i++ ) {
                comboDenominator.Items.Add( _DENOMI[i].ToString() );
            }
            txtNumerator.Text = "1";
            // アイコンを作成
            byte[] b = Base64.decode( iconbase64 );
            using ( MemoryStream ms = new MemoryStream( b ) ) {
                m_icon = new Bitmap( ms );
            }
        }

        public void applyLanguage( string language ) {
            this.Text = getName( language );
            if ( language == "ja" ) {
                lblDenominator.Text = "分母";
                lblNumerator.Text = "分子";
                lblLength.Text = "最初の音符の長さ";
                btnCancel.Text = "キャンセル";
                btnOK.Text = "OK";
            } else {
                lblDenominator.Text = "Denominator";
                lblNumerator.Text = "Numerator";
                lblLength.Text = "Length of precursor note";
                btnCancel.Text = "Cancel";
                btnOK.Text = "OK";
            }
        }

        public bool edit( VsqTrack track, int[] event_internal_ids, MouseButtons button ) {
            //TODO: buttonで処理を分ける
            bool edited = false;
            int divide_threshold = Numerator * 480 * 4 / Denominator;
            Console.WriteLine( "s_divide_threshold=" + divide_threshold );
            foreach ( int id in event_internal_ids ) {
                for ( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = (VsqEvent)itr.next();
                    if ( ve.InternalID == id ) {
                        if ( ve.ID.Length >= divide_threshold * 2 ) {
                            Console.WriteLine( "before; clock=" + ve.Clock + "; length=" + ve.ID.Length );
                            VsqEvent add = (VsqEvent)ve.clone();
                            int length = ve.ID.Length;
                            string[] symbol = ve.ID.LyricHandle.L0.getPhoneticSymbolList();
                            for ( int i = 0; i < symbol.Length; i++ ) {
                                Console.WriteLine( "symbol[" + i + "]=" + symbol[i] );
                            }
                            ve.ID.Length = divide_threshold;
                            add.Clock = ve.Clock + divide_threshold;
                            add.ID.Length = length - divide_threshold;
                            if ( add.ID.VibratoHandle != null ) {
                                if ( add.ID.VibratoDelay >= add.ID.Length ) {
                                    add.ID.VibratoHandle = null;
                                }
                            }
                            if ( ve.ID.VibratoHandle != null ) {
                                if ( ve.ID.VibratoDelay >= ve.ID.Length ) {
                                    ve.ID.VibratoHandle = null;
                                }
                            }
                            if ( symbol.Length >= 2 ) {
                                if ( button == MouseButtons.Middle && !VsqPhoneticSymbol.isConsonant( symbol[1] ) ) {
                                    ve.ID.LyricHandle.L0.setPhoneticSymbol( symbol[0] + " " + symbol[1] );
                                } else {
                                    ve.ID.LyricHandle.L0.setPhoneticSymbol( symbol[0] );
                                }
                                string symbol2 = "";
                                for ( int i = 1; i < symbol.Length; i++ ) {
                                    symbol2 += ((i == 1) ? "" : " ") + symbol[i];
                                }
                                Console.WriteLine( "symbol2=" + symbol2 );
                                add.ID.LyricHandle.L0.setPhoneticSymbol( symbol2 );
                            }
                            track.addEvent( add );
                            edited = true;
                        }
                        break;
                    }
                }
            }
            return edited;
        }

        public string getName( string language ) {
            if ( language.ToLower() == "ja" ) {
                return "音符分割";
            } else {
                return "Devide Note";
            }
        }

        public string getDescription( string language ) {
            if ( language.ToLower() == "ja" ) {
                return "音符を「母音-母音」または「子音-母音」のペアに分割します";
            } else {
                return "devides note into a phoneme pair, 'vowel-vowel' or 'consonant-vowel'";
            }
        }

        public bool hasDialog() {
            return true;
        }

        public DialogResult openDialog() {
            int num = Numerator;
            int den = Denominator;
            DialogResult ret = this.ShowDialog();
            if ( ret != DialogResult.OK ) {
                // 元に戻す
                Numerator = num;
                Denominator = den;
            }
            return ret;
        }

        public Bitmap getIcon() {
            return m_icon;
        }

        private void InitializeComponent() {
            this.lblLength = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comboDenominator = new System.Windows.Forms.ComboBox();
            this.lblNumerator = new System.Windows.Forms.Label();
            this.lblDenominator = new System.Windows.Forms.Label();
            this.txtNumerator = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point( 12, 18 );
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size( 131, 12 );
            this.lblLength.TabIndex = 0;
            this.lblLength.Text = "Length of precursor note";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point( 62, 109 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 88, 23 );
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 156, 109 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 88, 23 );
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // comboDenominator
            // 
            this.comboDenominator.FormattingEnabled = true;
            this.comboDenominator.Location = new System.Drawing.Point( 101, 67 );
            this.comboDenominator.Name = "comboDenominator";
            this.comboDenominator.Size = new System.Drawing.Size( 108, 20 );
            this.comboDenominator.TabIndex = 2;
            this.comboDenominator.SelectedIndexChanged += new System.EventHandler( this.comboLength_SelectedIndexChanged );
            // 
            // lblNumerator
            // 
            this.lblNumerator.AutoSize = true;
            this.lblNumerator.Location = new System.Drawing.Point( 26, 43 );
            this.lblNumerator.Name = "lblNumerator";
            this.lblNumerator.Size = new System.Drawing.Size( 58, 12 );
            this.lblNumerator.TabIndex = 29;
            this.lblNumerator.Text = "Numerator";
            // 
            // lblDenominator
            // 
            this.lblDenominator.AutoSize = true;
            this.lblDenominator.Location = new System.Drawing.Point( 26, 70 );
            this.lblDenominator.Name = "lblDenominator";
            this.lblDenominator.Size = new System.Drawing.Size( 69, 12 );
            this.lblDenominator.TabIndex = 30;
            this.lblDenominator.Text = "Denominator";
            // 
            // txtNumerator
            // 
            this.txtNumerator.Location = new System.Drawing.Point( 101, 40 );
            this.txtNumerator.Name = "txtNumerator";
            this.txtNumerator.Size = new System.Drawing.Size( 108, 19 );
            this.txtNumerator.TabIndex = 1;
            this.txtNumerator.Text = "1";
            this.txtNumerator.TextChanged += new System.EventHandler( this.txtNumerator_TextChanged );
            // 
            // DivideNote
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 256, 144 );
            this.Controls.Add( this.txtNumerator );
            this.Controls.Add( this.lblDenominator );
            this.Controls.Add( this.lblNumerator );
            this.Controls.Add( this.comboDenominator );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.lblLength );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DivideNote";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Divide Note";
            this.Load += new System.EventHandler( this.DivideNote_Load );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        private void comboLength_SelectedIndexChanged( object sender, EventArgs e ) {
            if ( comboDenominator.SelectedIndex < 0 ) {
                Denominator = 32;
                for ( int i = 0; i < _DENOMI.Length; i++ ) {
                    if ( _DENOMI[i] == 32 ) {
                        comboDenominator.SelectedIndex = i;
                        break;
                    }
                }
            } else {
                Denominator = _DENOMI[comboDenominator.SelectedIndex];
            }
        }

        private void DivideNote_Load( object sender, EventArgs e ) {
            txtNumerator.Text = Numerator.ToString();
            for ( int i = 0; i < _DENOMI.Length; i++ ) {
                if ( Denominator == _DENOMI[i] ) {
                    comboDenominator.SelectedIndex = i;
                    break;
                }
            }
        }

        private void txtNumerator_TextChanged( object sender, EventArgs e ) {
            int v = Numerator;
            if ( int.TryParse( txtNumerator.Text, out v ) ) {
                Numerator = v;
            }
        }
    }

}
