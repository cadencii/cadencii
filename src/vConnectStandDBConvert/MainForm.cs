using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using cadencii.apputil;

namespace cadencii.vconnect
{
    public partial class MainForm : Form
    {
        class ConvertParameter
        {
            public readonly string source_;
            public readonly string destination_;

            public ConvertParameter(string source, string destination)
            {
                source_ = source;
                destination_ = destination;
            }
        }

        private bool stop_requested_ = false;
        private object mutex_ = new object();
        private Process converter_ = null;
        private const int LINE_BUFFER_LENGTH = 500;
        private string[] line_buffer_ = new string[LINE_BUFFER_LENGTH];
        private int line_buffer_index_ = 0;
        private string start_button_start_text_;
        private string start_button_cancel_text_;

        public MainForm()
        {
            InitializeComponent();
            ApplyLanguage();
        }

        private void buttonSelectSourceDb_Click(object sender, EventArgs e)
        {
            if (openUtauDbDialog.ShowDialog() == DialogResult.OK) {
                textSourceDb.Text = openUtauDbDialog.FileName;
                ValidateDirectorySelectedStatus();
            }
        }

        private void buttonSelectDestinationDb_Click(object sender, EventArgs e)
        {
            if (saveVConnectDbDialog.ShowDialog() == DialogResult.OK) {
                textDestinationDb.Text = saveVConnectDbDialog.SelectedPath;
                ValidateDirectorySelectedStatus();
            }
        }

        private void ValidateDirectorySelectedStatus()
        {
            buttonStart.Enabled = File.Exists(textSourceDb.Text) && Directory.Exists(textDestinationDb.Text);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (converter_ == null) {
                Start();
            } else {
                Stop();
            }
        }

        private void PushLineBuffer(string line)
        {
            line_buffer_[line_buffer_index_] = line + "\r\n";
            line_buffer_index_ = (line_buffer_index_ + 1) % LINE_BUFFER_LENGTH;
            string text = "";

            int index = line_buffer_index_;
            for (int i = 0; i < LINE_BUFFER_LENGTH; ++i) {
                text += line_buffer_[index % LINE_BUFFER_LENGTH];
                ++index;
            }
            textLineBuffer.Text = text;
            textLineBuffer.Select(textLineBuffer.Text.Length - 1, 0);
            textLineBuffer.ScrollToCaret();
        }

        private void DoConvert(object argument)
        {
            if (argument == null) { return; }
            var parameter = argument as ConvertParameter;
            if (parameter == null) { return;}
            string source = parameter.source_;
            string destination = parameter.destination_;

            Thread monitor_thread = null;
            using (converter_ = new Process()) {
                converter_.StartInfo.FileName = Path.Combine(Application.StartupPath, "vConnect-STAND.exe");
                converter_.StartInfo.Arguments = string.Format("-c -i \"{0}\" -o \"{1}\"", source, destination);
                converter_.StartInfo.CreateNoWindow = true;
                converter_.StartInfo.UseShellExecute = false;
                converter_.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                converter_.StartInfo.RedirectStandardError = true;

                converter_.Start();

                var starter = new ParameterizedThreadStart(MonitorProcessOutput);
                monitor_thread = new Thread(starter);
                monitor_thread.Start(converter_.StandardError);

                converter_.WaitForExit();
            }

            converter_ = null;

            monitor_thread.Join();

            Action<string> set_start_button_text = (text) => {
                buttonStart.Text = text;
            };
            Invoke(set_start_button_text, new object[] { start_button_start_text_ });

            lock (mutex_) {
                if (!stop_requested_) {
                    MessageBox.Show(_("Completed"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void MonitorProcessOutput(object argument)
        {
            var reader = argument as StreamReader;
            while (!reader.EndOfStream) {
                string line = reader.ReadLine();
                lock (mutex_) {
                    if (stop_requested_) {
                        break;
                    }
                }
                Action<string> push_line = new Action<string>(PushLineBuffer);
                this.Invoke(push_line, new object[] { line });
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void Stop()
        {
            if (converter_ != null) {
                converter_.Kill();
                converter_ = null;
            }
            lock (mutex_) {
                stop_requested_ = true;
            }
            buttonStart.Text = start_button_start_text_;
        }

        private void Start()
        {
            MessageBox.Show(_("This process will create a copy of source UTAU DB.\n\nPlease make sure that you are not in violation of the license terms of the DB."),
                            _("Notice"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
            lock (mutex_) {
                stop_requested_ = false;
            }
            textLineBuffer.Text = "";
            buttonStart.Text = start_button_cancel_text_;
            var starter = new ParameterizedThreadStart(DoConvert);
            var thread = new Thread(starter);
            var parameter = new ConvertParameter(textSourceDb.Text, textDestinationDb.Text);
            thread.Start(parameter);
        }

        private void ApplyLanguage()
        {
            Messaging.loadMessages();
            Messaging.setLanguage(Messaging.getRuntimeLanguageName());

            Text = _("UTAU DB Converter for vConnect-STAND");
            groupSourceDb.Text = _("Select oto.ini file of source UTAU database");
            buttonSelectSourceDb.Text = _("Select");

            groupDestinationDb.Text = _("Select destination directory");
            buttonSelectDestinationDb.Text = _("Select");

            start_button_start_text_ = _("Convert");
            start_button_cancel_text_ = _("Cancel");

            buttonStart.Text = start_button_start_text_;
        }
    }
}
