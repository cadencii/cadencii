#if ENABLE_SCRIPT
/*
 * ScriptServer.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Collections.Generic;
using cadencii.apputil;
using cadencii.java.io;
using cadencii.java.util;



namespace cadencii
{
    /// <summary>
    /// スクリプトを管理するクラス
    /// </summary>
    public static class ScriptServer
    {
        private static SortedDictionary<string, ScriptInvoker> scripts = new SortedDictionary<string, ScriptInvoker>();

        /// <summary>
        /// 指定したIDのスクリプトを再読込みするか、または新規の場合読み込んで追加します。
        /// </summary>
        /// <param name="id"></param>
        public static void reload(string id)
        {
            string dir = Utility.getScriptPath();
            string file = Path.Combine(dir, id);
#if DEBUG
            sout.println("ScriptServer#reload; file=" + file + "; isFileExists(file)=" + System.IO.File.Exists(file));
#endif
            if (!System.IO.File.Exists(file)) {
                return;
            }

            ScriptInvoker si = (new PluginLoader()).loadScript(file);
            scripts[id] = si;
        }

        /// <summary>
        /// スクリプトを読み込み、コンパイルします。
        /// </summary>
        public static void reload()
        {
            // 拡張子がcs, txtのファイルを列挙
            string dir = Utility.getScriptPath();
            List<string> files = new List<string>();
            files.AddRange(new List<string>(PortUtil.listFiles(dir, ".txt")));
            files.AddRange(new List<string>(PortUtil.listFiles(dir, ".cs")));

            // 既存のスクリプトに無いまたは新しいやつはロード。
            List<string> added = new List<string>(); //追加または更新が行われたスクリプトのID
            foreach (string file in files) {
                string id = PortUtil.getFileName(file);
                double time = PortUtil.getFileLastModified(file);
                added.Add(id);

                bool loadthis = true;
                if (scripts.ContainsKey(id)) {
                    double otime = scripts[id].fileTimestamp;
                    if (time <= otime) {
                        // 前回コンパイルした時点でのスクリプトファイルよりも更新日が同じか古い。
                        loadthis = false;
                    }
                }

                // ロードする処理
                if (!loadthis) {
                    continue;
                }

                ScriptInvoker si = (new PluginLoader()).loadScript(file);
                scripts[id] = si;
            }

            // 削除されたスクリプトがあれば登録を解除する
            bool changed = true;
            while (changed) {
                changed = false;
                foreach (var id in scripts.Keys) {
                    if (!added.Contains(id)) {
                        scripts.Remove(id);
                        changed = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// スクリプトを実行します。
        /// </summary>
        /// <param name="evsd"></param>
        public static bool invokeScript(string id, VsqFileEx vsq)
        {
            ScriptInvoker script_invoker = null;
            if (scripts.ContainsKey(id)) {
                script_invoker = scripts[id];
            } else {
                return false;
            }
            if (script_invoker != null && script_invoker.scriptDelegate != null) {
                try {
                    VsqFileEx work = (VsqFileEx)vsq.clone();
                    ScriptReturnStatus ret = ScriptReturnStatus.ERROR;
                    if (script_invoker.scriptDelegate is EditVsqScriptDelegate) {
                        bool b_ret = ((EditVsqScriptDelegate)script_invoker.scriptDelegate).Invoke(work);
                        if (b_ret) {
                            ret = ScriptReturnStatus.EDITED;
                        } else {
                            ret = ScriptReturnStatus.ERROR;
                        }
                    } else if (script_invoker.scriptDelegate is EditVsqScriptDelegateEx) {
                        bool b_ret = ((EditVsqScriptDelegateEx)script_invoker.scriptDelegate).Invoke(work);
                        if (b_ret) {
                            ret = ScriptReturnStatus.EDITED;
                        } else {
                            ret = ScriptReturnStatus.ERROR;
                        }
                    } else if (script_invoker.scriptDelegate is EditVsqScriptDelegateWithStatus) {
                        ret = ((EditVsqScriptDelegateWithStatus)script_invoker.scriptDelegate).Invoke(work);
                    } else if (script_invoker.scriptDelegate is EditVsqScriptDelegateExWithStatus) {
                        ret = ((EditVsqScriptDelegateExWithStatus)script_invoker.scriptDelegate).Invoke(work);
                    } else {
                        ret = ScriptReturnStatus.ERROR;
                    }
                    if (ret == ScriptReturnStatus.ERROR) {
                        AppManager.showMessageBox(_("Script aborted"), "Cadencii", cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, cadencii.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE);
                    } else if (ret == ScriptReturnStatus.EDITED) {
                        CadenciiCommand run = VsqFileEx.generateCommandReplace(work);
                        AppManager.editHistory.register(vsq.executeCommand(run));
                    }
                    string config_file = configFileNameFromScriptFileName(script_invoker.ScriptFile);
                    FileStream fs = null;
                    bool delete_xml_when_exit = false; // xmlを消すときtrue
                    try {
                        fs = new FileStream(config_file, FileMode.Create, FileAccess.Write);
                        script_invoker.Serializer.serialize(fs, null);
                    } catch (Exception ex) {
                        serr.println("AppManager#invokeScript; ex=" + ex);
                        delete_xml_when_exit = true;
                    } finally {
                        if (fs != null) {
                            try {
                                fs.Close();
                                if (delete_xml_when_exit) {
                                    PortUtil.deleteFile(config_file);
                                }
                            } catch (Exception ex2) {
                                serr.println("AppManager#invokeScript; ex2=" + ex2);
                            }
                        }
                    }
                    return (ret == ScriptReturnStatus.EDITED);
                } catch (Exception ex) {
                    AppManager.showMessageBox(_("Script runtime error:") + " " + ex, _("Error"), cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, cadencii.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE);
                    serr.println("AppManager#invokeScript; ex=" + ex);
                }
            } else {
                AppManager.showMessageBox(_("Script compilation failed."), _("Error"), cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE);
            }
            return false;
        }

        /// <summary>
        /// スクリプトのファイル名から、そのスクリプトの設定ファイルの名前を決定します。
        /// </summary>
        /// <param name="script_file"></param>
        /// <returns></returns>
        public static string configFileNameFromScriptFileName(string script_file)
        {
            string dir = Path.Combine(Utility.getApplicationDataPath(), "script");
            if (!Directory.Exists(dir)) {
                PortUtil.createDirectory(dir);
            }
            return Path.Combine(dir, PortUtil.getFileNameWithoutExtension(script_file) + ".config");
        }

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        /// <summary>
        /// 読み込まれたスクリプトのIDを順に返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> getScriptIdIterator()
        {
            return scripts.Keys;
        }

        /// <summary>
        /// 指定したIDが示すスクリプトの、表示上の名称を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getDisplayName(string id)
        {
            if (scripts.ContainsKey(id)) {
                ScriptInvoker invoker = scripts[id];
                if (invoker.getDisplayNameDelegate != null) {
                    string ret = "";
                    try {
                        ret = invoker.getDisplayNameDelegate();
                    } catch (Exception ex) {
                        serr.println("ScriptServer#getDisplayName; ex=" + ex);
                        ret = PortUtil.getFileNameWithoutExtension(id);
                    }
                    return ret;
                }
            }
            return PortUtil.getFileNameWithoutExtension(id);
        }

        /// <summary>
        /// 指定したIDが示すスクリプトの、コンパイルした時点でのソースコードの更新日を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static double getTimestamp(string id)
        {
            if (scripts.ContainsKey(id)) {
                return scripts[id].fileTimestamp;
            } else {
                return 0;
            }
        }

        /// <summary>
        /// 指定したIDが示すスクリプトが利用可能かどうかを表すbool値を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool isAvailable(string id)
        {
            if (scripts.ContainsKey(id)) {
                return scripts[id].scriptDelegate != null;
            } else {
                return false;
            }
        }

        /// <summary>
        /// 指定したIDが示すスクリプトの、コンパイル時のメッセージを取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getCompileMessage(string id)
        {
            if (scripts.ContainsKey(id)) {
                return scripts[id].ErrorMessage;
            } else {
                return "";
            }
        }
    }

}
#endif
