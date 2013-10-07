/*
 * VocaloSysUtil.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii.vsq
{

    /// <summary>
    /// VOCALOID / VOCALOID2システムについての情報を取得するユーティリティ。
    /// </summary>
    public class VocaloSysUtil
    {
        private static SortedDictionary<SynthesizerType, SingerConfigSys> s_singer_config_sys = new SortedDictionary<SynthesizerType, SingerConfigSys>();
        private static SortedDictionary<SynthesizerType, ExpressionConfigSys> s_exp_config_sys = new SortedDictionary<SynthesizerType, ExpressionConfigSys>();
        private static SortedDictionary<SynthesizerType, string> s_path_vsti = new SortedDictionary<SynthesizerType, string>();
        private static SortedDictionary<SynthesizerType, string> s_path_editor = new SortedDictionary<SynthesizerType, string>();
        private static Boolean isInitialized = false;
        /// <summary>
        /// VOCALOID1の、デフォルトのSynthesize Engineバージョン。1.0の場合100, 1.1の場合101。規定では100(1.0)。
        /// initメソッドにおいて、VOCALOID.iniから読み取る
        /// </summary>
        private static int defaultDseVersion = 100;
        /// <summary>
        /// VOCALOID1にて、バージョン1.1のSynthesize Engineが利用可能かどうか。
        /// 既定ではfalse。DSE1_1.dllが存在するかどうかで判定。
        /// </summary>
        private static bool dseVersion101Available = false;
        private static readonly string header1 = "HKLM\\SOFTWARE\\VOCALOID";
        private static readonly string header2 = "HKLM\\SOFTWARE\\VOCALOID2";

        private VocaloSysUtil()
        {
        }

        public static SingerConfigSys getSingerConfigSys(SynthesizerType type)
        {
            if (s_singer_config_sys.ContainsKey(type)) {
                return s_singer_config_sys[type];
            } else {
                return null;
            }
        }

        /// <summary>
        /// VOCALOID1にて、バージョン1.1のSynthesize Engineが利用可能かどうか。
        /// 既定ではfalse。DSE1_1.dllが存在するかどうかで判定。
        /// </summary>
        public static bool isDSEVersion101Available()
        {
            return dseVersion101Available;
        }

        /// <summary>
        /// VOCALOID1の、デフォルトのSynthesize Engineバージョンを取得します。
        /// 1.0の場合100, 1.1の場合101。規定では100(1.0)。
        /// </summary>
        public static int getDefaultDseVersion()
        {
            if (!isInitialized) {
                serr.println("VocaloSysUtil#getDefaultDseVersion; not initialized yet");
                return 0;
            }
            return defaultDseVersion;
        }

        /// <summary>
        /// インストールされているVOCALOID / VOCALOID2についての情報を読み込み、初期化します．
        /// C#はこっちを呼べばOK
        /// </summary>
        public static void init()
        {
            if (isInitialized) {
                return;
            }
            List<string> reg_list = new List<string>();
            initPrint("SOFTWARE\\VOCALOID", header1, reg_list);
            initPrint("SOFTWARE\\VOCALOID2", header2, reg_list);
            init(reg_list);
        }

        /// <summary>
        /// Windowsのレジストリ・エントリを列挙した文字列のリストを指定し，初期化します
        /// パラメータreg_listの中身は，例えば
        /// "HKLM\SOFTWARE\VOCALOID2\DATABASE\EXPRESSION\\tEXPRESSIONDIR\\tC:\Program Files\VOCALOID2\expdbdir"
        /// のような文字列です．
        /// </summary>
        /// <param name="reg_list">レジストリ・エントリのリスト</param>
        public static void init(List<string> reg_list)
        {
            if (reg_list == null) {
                return;
            }
            if (isInitialized) {
                return;
            }

            // reg_listを，VOCALOIDとVOCALOID2の部分に分離する
            List<string> dir1 = new List<string>();
            List<string> dir2 = new List<string>();
            foreach (string s in reg_list) {
                if (s.StartsWith(header1 + "\\") ||
                    s.StartsWith(header1 + "\t")) {
                    dir1.Add(s);
                } else if (s.StartsWith(header2 + "\\") ||
                           s.StartsWith(header2 + "\t")) {
                    dir2.Add(s);
                }
            }

            ExpressionConfigSys exp_config_sys1 = null;
            try {
                ByRef<string> path_voicedb1 = new ByRef<string>("");
                ByRef<string> path_expdb1 = new ByRef<string>("");
                List<string> installed_singers1 = new List<string>();

                // テキストファイルにレジストリの内容をプリントアウト
                bool close = false;
                ByRef<string> path_vsti = new ByRef<string>("");
                ByRef<string> path_editor = new ByRef<string>("");
                initExtract(dir1,
                         header1,
                         path_vsti,
                         path_voicedb1,
                         path_expdb1,
                         path_editor,
                         installed_singers1);
                s_path_vsti[SynthesizerType.VOCALOID1] = path_vsti.value;
                s_path_editor[SynthesizerType.VOCALOID1] = path_editor.value;
                string[] act_installed_singers1 = installed_singers1.ToArray();
                string act_path_voicedb1 = path_voicedb1.value;
                string act_path_editor1 = path_editor.value;
                string act_path_expdb1 = path_expdb1.value;
                string act_vsti1 = path_vsti.value;
                string expression_map1 = Path.Combine(act_path_expdb1, "expression.map");
                SingerConfigSys singer_config_sys =
                    new SingerConfigSys(act_path_voicedb1, act_installed_singers1);
                if (System.IO.File.Exists(expression_map1)) {
                    exp_config_sys1 = new ExpressionConfigSys(act_path_editor1, act_path_expdb1);
                }
                s_singer_config_sys[SynthesizerType.VOCALOID1] = singer_config_sys;

                // DSE1_1.dllがあるかどうか？
                if (!act_vsti1.Equals("")) {
                    string path_dll = PortUtil.getDirectoryName(act_vsti1);
                    string dse1_1 = Path.Combine(path_dll, "DSE1_1.dll");
                    dseVersion101Available = System.IO.File.Exists(dse1_1);
                } else {
                    dseVersion101Available = false;
                }

                // VOCALOID.iniから、DSEVersionを取得
                if (act_path_editor1 != null && !act_path_editor1.Equals("") &&
                     System.IO.File.Exists(act_path_editor1)) {
                    string dir = PortUtil.getDirectoryName(act_path_editor1);
                    string ini = Path.Combine(dir, "VOCALOID.ini");
                    if (System.IO.File.Exists(ini)) {
                        StreamReader br = null;
                        try {
                            br = new StreamReader(ini, Encoding.GetEncoding("Shift_JIS"));
                            string line;
                            while ((line = br.ReadLine()) != null) {
                                if (line == null) continue;
                                if (line.Equals("")) continue;
                                if (line.StartsWith("DSEVersion")) {
                                    string[] spl = PortUtil.splitString(line, '=');
                                    if (spl.Length >= 2) {
                                        string str_dse_version = spl[1];
                                        try {
                                            defaultDseVersion = int.Parse(str_dse_version);
                                        } catch (Exception ex) {
                                            serr.println("VocaloSysUtil#init; ex=" + ex);
                                            defaultDseVersion = 100;
                                        }
                                    }
                                    break;
                                }
                            }
                        } catch (Exception ex) {
                            serr.println("VocaloSysUtil#init; ex=" + ex);
                        } finally {
                            if (br != null) {
                                try {
                                    br.Close();
                                } catch (Exception ex2) {
                                    serr.println("VocaloSysUtil#init; ex2=" + ex2);
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                serr.println("VocaloSysUtil#init; ex=" + ex);
                SingerConfigSys singer_config_sys = new SingerConfigSys("", new string[] { });
                exp_config_sys1 = null;
                s_singer_config_sys[SynthesizerType.VOCALOID1] = singer_config_sys;
            }
            if (exp_config_sys1 == null) {
                exp_config_sys1 = ExpressionConfigSys.getVocaloid1Default();
            }
            s_exp_config_sys[SynthesizerType.VOCALOID1] = exp_config_sys1;

            ExpressionConfigSys exp_config_sys2 = null;
            try {
                ByRef<string> path_voicedb2 = new ByRef<string>("");
                ByRef<string> path_expdb2 = new ByRef<string>("");
                List<string> installed_singers2 = new List<string>();

                // レジストリの中身をファイルに出力
                bool close = false;
                ByRef<string> path_vsti = new ByRef<string>("");
                ByRef<string> path_editor = new ByRef<string>("");
                initExtract(dir2,
                         header2,
                         path_vsti,
                         path_voicedb2,
                         path_expdb2,
                         path_editor,
                         installed_singers2);
                s_path_vsti[SynthesizerType.VOCALOID2] = path_vsti.value;
                s_path_editor[SynthesizerType.VOCALOID2] = path_editor.value;
                string[] act_installed_singers2 = installed_singers2.ToArray();
                string act_path_expdb2 = path_expdb2.value;
                string act_path_voicedb2 = path_voicedb2.value;
                string act_path_editor2 = path_editor.value;
                string act_vsti2 = path_vsti.value;
                string expression_map2 = Path.Combine(act_path_expdb2, "expression.map");
                SingerConfigSys singer_config_sys = new SingerConfigSys(act_path_voicedb2, act_installed_singers2);
                if (System.IO.File.Exists(expression_map2)) {
                    exp_config_sys2 = new ExpressionConfigSys(act_path_editor2, act_path_expdb2);
                }
                s_singer_config_sys[SynthesizerType.VOCALOID2] = singer_config_sys;
            } catch (Exception ex) {
                serr.println("VocaloSysUtil..cctor; ex=" + ex);
                SingerConfigSys singer_config_sys = new SingerConfigSys("", new string[] { });
                exp_config_sys2 = null;
                s_singer_config_sys[SynthesizerType.VOCALOID2] = singer_config_sys;
            }
            if (exp_config_sys2 == null) {
#if DEBUG
                sout.println("VocaloSysUtil#.ctor; loading default ExpressionConfigSys...");
#endif
                exp_config_sys2 = ExpressionConfigSys.getVocaloid2Default();
            }
            s_exp_config_sys[SynthesizerType.VOCALOID2] = exp_config_sys2;

            isInitialized = true;
        }

        /// <summary>
        /// ビブラートのプリセットタイプから，VibratoHandleを作成します
        /// </summary>
        /// <param name="icon_id"></param>
        /// <param name="vibrato_length"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static VibratoHandle getDefaultVibratoHandle(string icon_id, int vibrato_length, SynthesizerType type)
        {
            if (!isInitialized) {
                serr.println("VocaloSysUtil#getDefaultVibratoHandle; not initialized yet");
                return null;
            }
            if (s_exp_config_sys.ContainsKey(type)) {
                foreach (var vconfig in s_exp_config_sys[type].vibratoConfigIterator()) {
                    if (vconfig.IconID.Equals(icon_id)) {
                        VibratoHandle ret = (VibratoHandle)vconfig.clone();
                        ret.setLength(vibrato_length);
                        return ret;
                    }
                }
            }
            VibratoHandle empty = new VibratoHandle();
            empty.IconID = "$04040000";
            return empty;
        }

        private static void initExtract(List<string> dir,
                                     string header,
                                     ByRef<string> path_vsti,
                                     ByRef<string> path_voicedb,
                                     ByRef<string> path_expdb,
                                     ByRef<string> path_editor,
                                     List<string> installed_singers)
        {
            List<string> application = new List<string>();
            List<string> expression = new List<string>();
            List<string> voice = new List<string>();
            path_vsti.value = "";
            path_expdb.value = "";
            path_voicedb.value = "";
            path_editor.value = "";
            foreach (var s in dir) {
                if (s.StartsWith(header + "\\APPLICATION")) {
                    application.Add(s.Substring(PortUtil.getStringLength(header + "\\APPLICATION")));
                } else if (s.StartsWith(header + "\\DATABASE\\EXPRESSION")) {
                    expression.Add(s.Substring(PortUtil.getStringLength(header + "\\DATABASE\\EXPRESSION")));
                } else if (s.StartsWith(header + "\\DATABASE\\VOICE")) {
                    voice.Add(s.Substring(PortUtil.getStringLength(header + "\\DATABASE\\VOICE\\")));
                }
            }

            // path_vstiを取得
            foreach (var s in application) {
                string[] spl = PortUtil.splitString(s, '\t');
                if (spl.Length >= 3 && spl[1].Equals("PATH")) {
                    if (spl[2].ToLower().EndsWith(".dll")) {
                        path_vsti.value = spl[2];
                    } else if (spl[2].ToLower().EndsWith(".exe")) {
                        path_editor.value = spl[2];
                    }
                }
            }

            // path_vicedbを取得
            SortedDictionary<string, string> install_dirs = new SortedDictionary<string, string>();
            foreach (var s in voice) {
                string[] spl = PortUtil.splitString(s, '\t');
                if (spl.Length < 2) {
                    continue;
                }

                if (spl[0].Equals("VOICEDIR")) {
                    path_voicedb.value = spl[1];
                } else if (spl.Length >= 3) {
                    string[] spl2 = PortUtil.splitString(spl[0], '\\');
                    if (spl2.Length == 1) {
                        string id = spl2[0]; // BHXXXXXXXXXXXXみたいなシリアル
                        if (!install_dirs.ContainsKey(id)) {
                            install_dirs[id] = "";
                        }
                        if (spl[1].Equals("INSTALLDIR")) {
                            // VOCALOID1の場合は、ここには到達しないはず
                            string installdir = spl[2];
                            install_dirs[id] = Path.Combine(installdir, id);
                        }
                    }
                }
            }

            // installed_singersに追加
            foreach (var id in install_dirs.Keys) {
                string install = install_dirs[id];
                if (install.Equals("")) {
                    install = Path.Combine(path_voicedb.value, id);
                }
                installed_singers.Add(install);
            }

            // path_expdbを取得
            List<string> exp_ids = new List<string>();
            // 最初はpath_expdbの取得と、id（BHXXXXXXXXXXXXXXXX）のようなシリアルを取得
            foreach (var s in expression) {
                string[] spl = PortUtil.splitString(s, new char[] { '\t' }, true);
                if (spl.Length >= 3) {
                    if (spl[1].Equals("EXPRESSIONDIR")) {
                        path_expdb.value = spl[2];
                    } else if (spl.Length >= 3) {
                        string[] spl2 = PortUtil.splitString(spl[0], '\\');
                        if (spl2.Length == 1) {
                            if (!exp_ids.Contains(spl2[0])) {
                                exp_ids.Add(spl2[0]);
                            }
                        }
                    }
                }
            }
#if DEBUG
            sout.println("path_vsti=" + path_vsti.value);
            sout.println("path_voicedb=" + path_voicedb.value);
            sout.println("path_expdb=" + path_expdb.value);
            sout.println("installed_singers=");
#endif
        }

        /// <summary>
        /// レジストリkey内の値を再帰的に検索し、ファイルfpに順次出力する
        /// </summary>
        /// <param name="reg_key_name"></param>
        /// <param name="parent_name"></param>
        /// <param name="list"></param>
        private static void initPrint(string reg_key_name, string parent_name, List<string> list)
        {
            try {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(reg_key_name, false);
                if (key == null) {
                    return;
                }

                // 直下のキー内を再帰的にリストアップ
                string[] subkeys = key.GetSubKeyNames();
                foreach (string s in subkeys) {
                    initPrint(reg_key_name + "\\" + s, parent_name + "\\" + s, list);
                }

                // 直下の値を出力
                string[] valuenames = key.GetValueNames();
                foreach (string s in valuenames) {
                    RegistryValueKind kind = key.GetValueKind(s);
                    if (kind == RegistryValueKind.String) {
                        string str = parent_name + "\t" + s + "\t" + (string)key.GetValue(s, "");
                        list.Add(str);
                    }
                }
                key.Close();
            } catch (Exception ex) {
                serr.println("VocaloSysUtil#initPrint; ex=" + ex);
            }
        }

        /// <summary>
        /// アタック設定を順に返す反復子を取得します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<NoteHeadHandle> attackConfigIterator(SynthesizerType type)
        {
            if (!isInitialized) {
                serr.println("VocaloSysUtil#attackConfigIterator; not initialized yet");
                return null;
            }
            if (s_exp_config_sys.ContainsKey(type)) {
                return s_exp_config_sys[type].attackConfigIterator();
            } else {
                return new List<NoteHeadHandle>();
            }
        }

        /// <summary>
        /// ビブラート設定を順に返す反復子を取得します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<VibratoHandle> vibratoConfigIterator(SynthesizerType type)
        {
            if (!isInitialized) {
                serr.println("VocaloSysUtil#vibratoConfigIterator; not initialized yet");
                return null;
            }
            if (s_exp_config_sys.ContainsKey(type)) {
                return s_exp_config_sys[type].vibratoConfigIterator();
            } else {
                return new List<VibratoHandle>();
            }
        }

        /// <summary>
        /// 強弱記号設定を順に返す反復子を取得します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<IconDynamicsHandle> dynamicsConfigIterator(SynthesizerType type)
        {
            if (!isInitialized) {
                serr.println("VocaloSysUtil#dynamicsConfigIterator; not initialized yet");
                return null;
            }
            if (s_exp_config_sys.ContainsKey(type)) {
                return s_exp_config_sys[type].dynamicsConfigIterator();
            } else {
                return new List<IconDynamicsHandle>();
            }
        }

        /// <summary>
        /// 指定した歌声合成システムに登録されている指定した名前の歌手について、その派生元の歌手名を取得します。
        /// </summary>
        /// <param name="language"></param>
        /// <param name="program"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string getOriginalSinger(int language, int program, SynthesizerType type)
        {
            if (!isInitialized) {
                serr.println("VocaloSysUtil#getOriginalSinger; not initialized yet");
                return null;
            }
            string voiceidstr = "";
            if (!s_singer_config_sys.ContainsKey(type)) {
                return "";
            }
            SingerConfigSys scs = s_singer_config_sys[type];
            SingerConfig[] singer_configs = scs.getSingerConfigs();
            for (int i = 0; i < singer_configs.Length; i++) {
                if (language == singer_configs[i].Language && program == singer_configs[i].Program) {
                    voiceidstr = singer_configs[i].VOICEIDSTR;
                    break;
                }
            }
            if (voiceidstr.Equals("")) {
                return "";
            }
            SingerConfig[] installed_singers = scs.getInstalledSingers();
            for (int i = 0; i < installed_singers.Length; i++) {
                if (voiceidstr.Equals(installed_singers[i].VOICEIDSTR)) {
                    return installed_singers[i].VOICENAME;
                }
            }
            return "";
        }

        /// <summary>
        /// 指定した歌声合成システムに登録されている指定した名前の歌手について、その歌手を表現するVsqIDのインスタンスを取得します。
        /// </summary>
        /// <param name="language"></param>
        /// <param name="program"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static VsqID getSingerID(int language, int program, SynthesizerType type)
        {
            if (!isInitialized) {
                serr.println("VocaloSysUtil#getSingerID; not initialized yet");
                return null;
            }
            if (!s_singer_config_sys.ContainsKey(type)) {
                return null;
            } else {
                return s_singer_config_sys[type].getSingerID(language, program);
            }
        }

        /// <summary>
        /// 指定した歌声合成システムの、エディタの実行ファイルのパスを取得します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string getEditorPath(SynthesizerType type)
        {
            if (!isInitialized) {
                serr.println("VocaloSysUtil#getEditorPath; not initialized yet");
                return "";
            }
            if (!s_path_editor.ContainsKey(type)) {
                return "";
            } else {
                return s_path_editor[type];
            }
        }

        /// <summary>
        /// 指定した歌声合成システムの、VSTi DLL本体のパスを取得します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string getDllPathVsti(SynthesizerType type)
        {
            if (!isInitialized) {
                serr.println("VocaloSysUtil#getDllPathVsti; not initialized yet");
                return "";
            }
            if (!s_path_vsti.ContainsKey(type)) {
                return "";
            } else {
                return s_path_vsti[type];
            }
        }

        /// <summary>
        /// 指定された歌声合成システムに登録されている歌手設定のリストを取得します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SingerConfig[] getSingerConfigs(SynthesizerType type)
        {
            if (!isInitialized) {
                serr.println("VocaloSysUtil#getSingerConfigs; not initialized yet");
                return new SingerConfig[] { };
            }
            if (!s_singer_config_sys.ContainsKey(type)) {
                return new SingerConfig[] { };
            } else {
                return s_singer_config_sys[type].getSingerConfigs();
            }
        }

        /// <summary>
        /// 指定した名前の歌手の歌唱言語を取得します。
        /// </summary>
        /// <param name="name">name of singer</param>
        /// <returns></returns>
        public static VsqVoiceLanguage getLanguageFromName(string name)
        {
            if (!isInitialized) {
                serr.println("VocaloSysUtil#getLanguageFromName; not initialized yet");
                return VsqVoiceLanguage.Japanese;
            }
            string search = name.ToLower();
            if (search.Equals("meiko") ||
                search.Equals("kaito") ||
                search.Equals("miku") ||
                search.Equals("rin") ||
                search.Equals("len") ||
                search.Equals("rin_act2") ||
                search.Equals("len_act2") ||
                search.Equals("gackpoid") ||
                search.Equals("luka_jpn") ||
                search.Equals("megpoid") ||
                search.Equals("sfa2_miki") ||
                search.Equals("yuki") ||
                search.Equals("kiyoteru") ||
                search.Equals("miku_sweet") ||
                search.Equals("miku_dark") ||
                search.Equals("miku_soft") ||
                search.Equals("miku_light") ||
                search.Equals("miku_vivid") ||
                search.Equals("miku_solid")) {
                return VsqVoiceLanguage.Japanese;
            } else if (search.Equals("sweet_ann") ||
                search.Equals("prima") ||
                search.Equals("luka_eng") ||
                search.Equals("sonika") ||
                search.Equals("lola") ||
                search.Equals("leon") ||
                search.Equals("miriam") ||
                search.Equals("big_al")) {
                return VsqVoiceLanguage.English;
            }
            return VsqVoiceLanguage.Japanese;
        }

        /// <summary>
        /// 指定したPAN値における、左チャンネルの増幅率を取得します。
        /// </summary>
        /// <param name="pan"></param>
        /// <returns></returns>
        public static double getAmplifyCoeffFromPanLeft(int pan)
        {
            return pan / -64.0 + 1.0;
        }

        /// <summary>
        /// 指定したPAN値における、右チャンネルの増幅率を取得します。
        /// </summary>
        /// <param name="pan"></param>
        /// <returns></returns>
        public static double getAmplifyCoeffFromPanRight(int pan)
        {
            return pan / 64.0 + 1.0;
        }

        /// <summary>
        /// 指定したFEDER値における、増幅率を取得します。
        /// </summary>
        /// <param name="feder"></param>
        /// <returns></returns>
        public static double getAmplifyCoeffFromFeder(int feder)
        {
            return Math.Exp(1.18448420e-01 * feder / 10.0);
        }
    }

}
