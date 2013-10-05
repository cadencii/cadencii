using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using cadencii;
using cadencii.apputil;
using cadencii;



class UtauPluginManager : Form
{
    delegate void VoidDelegate();

    private Button btnOk;
    private Button btnAdd;
    private Button btnRemove;
    private Button btnCancel;
    /// <summary>
    /// この部分は、Utau Plugin Invoker.csについて、以下のテキスト処理を行ったもの。
    ///     1. 「"」を「\"」に置換（テキストモード）
    ///     2. 「\n」を「\\n" +\n"」に置換（正規表現モード）
    ///     3. 「Utau_Plugin_Invoker」を「{0}」に置換。
    ///     4. 「s_plugin_txt_path = @"E:\Program Files\UTAU\..(一部略)..";」を「s_plugin_txt_path = @"{1}";」に書き換え。
    /// または、付属のツールで次のように処理する
    ///     ParseUtauPluginInvoker.exe ".\ScriptImplement\Utau Plugin Invoker.cs" out.txt
    /// </summary>
    private static readonly string TEXT = "@@TEXT@@";
    private ListView listPlugins;
    private ColumnHeader headerName;
    private ColumnHeader headerPath;
    private OpenFileDialog openFileDialog;

    /// <summary>
    /// plugin.txtのパスのリスト
    /// </summary>
    public static List<string> Plugins = new List<string>();
    public static int ColumnWidthName = 100;
    public static int ColumnWidthPath = 200;
    public static int DialogWidth = 295;
    public static int DialogHeight = 352;
    private static List<string> oldPlugins = null;

    public UtauPluginManager()
    {
        InitializeComponent();
        if (ColumnWidthName > 0) {
            headerName.Width = ColumnWidthName;
        }
        if (ColumnWidthPath > 0) {
            headerPath.Width = ColumnWidthPath;
        }
        if (DialogWidth > 0) {
            Width = DialogWidth;
        }
        if (DialogHeight > 0) {
            Height = DialogHeight;
        }

        btnAdd.Text = _("Add");
        btnRemove.Text = _("Remove");
        btnOk.Text = _("OK");
        btnCancel.Text = _("Cancel");
        headerName.Text = _("Name");
        headerPath.Text = _("Path");

        oldPlugins = new List<string>();
        Encoding sjis = Encoding.GetEncoding("Shift_JIS");
        foreach (string s in getPlugins()) {
            if (!System.IO.File.Exists(s)) {
                continue;
            }
            string name = getPluginName(s);
            if (name != "") {
                listPlugins.Items.Add(new ListViewItem(new string[] { name, s }));
            }
            oldPlugins.Add(s);
        }
    }

    private static List<string> getPlugins()
    {
        if (Plugins == null) {
            Plugins = new List<string>();
        }
        return Plugins;
    }

    public static ScriptReturnStatus Edit(VsqFileEx vsq)
    {
        UtauPluginManager dialog = new UtauPluginManager();
        dialog.ShowDialog();
        return ScriptReturnStatus.NOT_EDITED;
    }

    private void InitializeComponent()
    {
        this.btnOk = new System.Windows.Forms.Button();
        this.btnAdd = new System.Windows.Forms.Button();
        this.btnRemove = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.listPlugins = new System.Windows.Forms.ListView();
        this.headerName = new System.Windows.Forms.ColumnHeader();
        this.headerPath = new System.Windows.Forms.ColumnHeader();
        this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
        this.SuspendLayout();
        // 
        // btnOk
        // 
        this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnOk.Location = new System.Drawing.Point(119, 283);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(75, 23);
        this.btnOk.TabIndex = 7;
        this.btnOk.Text = "OK";
        this.btnOk.UseVisualStyleBackColor = true;
        this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
        // 
        // btnAdd
        // 
        this.btnAdd.Location = new System.Drawing.Point(12, 12);
        this.btnAdd.Name = "btnAdd";
        this.btnAdd.Size = new System.Drawing.Size(75, 23);
        this.btnAdd.TabIndex = 9;
        this.btnAdd.Text = "Add";
        this.btnAdd.UseVisualStyleBackColor = true;
        this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
        // 
        // btnRemove
        // 
        this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnRemove.Location = new System.Drawing.Point(200, 12);
        this.btnRemove.Name = "btnRemove";
        this.btnRemove.Size = new System.Drawing.Size(75, 23);
        this.btnRemove.TabIndex = 10;
        this.btnRemove.Text = "Remove";
        this.btnRemove.UseVisualStyleBackColor = true;
        this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point(200, 283);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 23);
        this.btnCancel.TabIndex = 11;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        // 
        // listPlugins
        // 
        this.listPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.listPlugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerName,
            this.headerPath});
        this.listPlugins.FullRowSelect = true;
        this.listPlugins.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
        this.listPlugins.Location = new System.Drawing.Point(12, 41);
        this.listPlugins.Name = "listPlugins";
        this.listPlugins.Size = new System.Drawing.Size(263, 236);
        this.listPlugins.TabIndex = 12;
        this.listPlugins.UseCompatibleStateImageBehavior = false;
        this.listPlugins.View = System.Windows.Forms.View.Details;
        // 
        // headerName
        // 
        this.headerName.Text = "Name";
        // 
        // headerPath
        // 
        this.headerPath.Text = "Path";
        // 
        // openFileDialog
        // 
        this.openFileDialog.FileName = "plugin.txt";
        // 
        // UtauPluginManager
        // 
        this.AcceptButton = this.btnOk;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(287, 318);
        this.Controls.Add(this.listPlugins);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnRemove);
        this.Controls.Add(this.btnAdd);
        this.Controls.Add(this.btnOk);
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "UtauPluginManager";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "UTAU Plugin Manager";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UtauPluginManager_FormClosing);
        this.ResumeLayout(false);

    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() != DialogResult.OK) {
            return;
        }

        string file = openFileDialog.FileName;
        string name = getPluginName(file);
        if (name == "") {
            return;
        }

        foreach (ListViewItem litem in listPlugins.Items) {
            if (litem.SubItems.Count < 2) {
                continue;
            }
            string tname = litem.SubItems[0].Text;
            string tfile = litem.SubItems[1].Text;

            if (tname == name) {
                var dr =
                    AppManager.showMessageBox(string.Format(_("Script named '{0}' is already registered. Overwrite?"), name),
                                               "UTAU Plugin Manager",
                                               cadencii.windows.forms.Utility.MSGBOX_OK_CANCEL_OPTION,
                                               cadencii.windows.forms.Utility.MSGBOX_QUESTION_MESSAGE);
                if (dr != DialogResult.Yes) {
                    return;
                }

                listPlugins.Items.Remove(litem);
                foreach (string f in getPlugins()) {
                    if (f == tfile) {
                        getPlugins().Remove(f);
                        break;
                    }
                }
            }
        }

        listPlugins.Items.Add(new ListViewItem(new string[] { name, file }));
        getPlugins().Add(file);
    }

    private static string getPluginName(string plugin_txt_file)
    {
        if (!File.Exists(plugin_txt_file)) {
            return "";
        }

        string name = "";
        using (StreamReader sr = new StreamReader(plugin_txt_file, Encoding.GetEncoding("Shift_JIS"))) {
            string line = "";
            while ((line = sr.ReadLine()) != null) {
                if (line.StartsWith("name=")) {
                    name = line.Substring(5).Trim();
                    break;
                }
            }
        }
        return name;
    }

    private static string _(string id)
    {
        string lang = Messaging.getLanguage();
        if (lang != "en") {
            if (id == "Script named '{0}' is already registered. Overwrite?") {
                if (lang == "ja") {
                    return "'{0}' という名前のスクリプトは既に登録されています。上書きしますか？";
                }
            } else if (id == "Remove '{0}'?") {
                if (lang == "ja") {
                    return "'{0}' を削除しますか？";
                }
            } else if (id == "Add") {
                if (lang == "ja") {
                    return "追加";
                }
            } else if (id == "Remove") {
                if (lang == "ja") {
                    return "削除";
                }
            } else if (id == "OK") {
                if (lang == "ja") {
                    return "了解";
                }
            } else if (id == "Cancel") {
                if (lang == "ja") {
                    return "取消";
                }
            } else if (id == "Name") {
                if (lang == "ja") {
                    return "名称";
                }
            } else if (id == "Path") {
                if (lang == "ja") {
                    return "保存場所";
                }
            }
        }
        return id;
    }

    private void UtauPluginManager_FormClosing(object sender, FormClosingEventArgs e)
    {
        ColumnWidthName = headerName.Width;
        ColumnWidthPath = headerPath.Width;
        if (WindowState == FormWindowState.Normal) {
            DialogWidth = Width;
            DialogHeight = Height;
        }
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
        int count = listPlugins.SelectedIndices.Count;
        if (count <= 0) {
            return;
        }

        int indx = listPlugins.SelectedIndices[0];
        ListViewItem litem = listPlugins.Items[indx];
        if (litem.SubItems.Count < 2) {
            return;
        }
        string name = litem.SubItems[0].Text;
        string path = litem.SubItems[1].Text;

        var dr =
            AppManager.showMessageBox(string.Format(_("Remove '{0}'?"), name),
                                       "UTAU Plugin Manager",
                                       cadencii.windows.forms.Utility.MSGBOX_YES_NO_OPTION,
                                       cadencii.windows.forms.Utility.MSGBOX_QUESTION_MESSAGE);
        if (dr != DialogResult.Yes) {
            return;
        }

        listPlugins.Items.Remove(litem);
        if (getPlugins().Contains(path)) {
            getPlugins().Remove(path);
        }
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (oldPlugins != null) {
            // 削除されたスクリプトをアンインストールする
            foreach (string file in oldPlugins) {
                if (!getPlugins().Contains(file)) {
                    string name = getPluginName(file);
                    if (name != "") {
                        string script_path = Path.Combine(Utility.getScriptPath(), name + ".txt");
                        if (File.Exists(script_path)) {
                            PortUtil.deleteFile(script_path);
                        }
                    }
                }
            }
        }

        foreach (string file in getPlugins()) {
            if (!File.Exists(file)) {
                continue;
            }

            string name = getPluginName(file);
            if (name == "") {
                continue;
            }
            char[] invalid_classname = new char[] { ' ', '!', '#', '$', '%', '&', '\'', '(', ')', '=', '-', '~', '^', '`', '@', '{', '}', '[', ']', '+', '*', ';', '.' };
            foreach (char c in invalid_classname) {
                name = name.Replace(c, '_');
            }
            string text = TEXT;
            string code = text.Replace("{0}", name);
            code = code.Replace("{1}", file);
            using (StreamWriter sw = new StreamWriter(Path.Combine(Utility.getScriptPath(), name + ".txt"))) {
                sw.WriteLine(code);
            }
        }

        if (AppManager.mMainWindow != null) {
            VoidDelegate deleg = new VoidDelegate(AppManager.mMainWindow.updateScriptShortcut);
            if (deleg != null) {
                AppManager.mMainWindow.Invoke(deleg);
            }
        }
    }

    public static string GetDisplayName()
    {
        string lang = Messaging.getLanguage();
        if (lang == "ja") {
            return "UTAU用プラグインをインストール";
        } else {
            return "Install UTAU Plugin";
        }
    }
}
