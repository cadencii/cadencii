/*
 * SymbolTable.cs
 * Copyright © 2008-2011 kbinani
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
using cadencii.java.io;
using cadencii.java.util;

namespace cadencii
{
    namespace vsq
    {


        /// <summary>
        /// 歌詞から発音記号列を引き当てるための辞書を表現するクラス
        /// </summary>
        public class SymbolTable : ICloneable
        {
            /// <summary>
            /// 辞書本体
            /// </summary>
            private SortedDictionary<string, SymbolTableEntry> mDict;
            /// <summary>
            /// 辞書の名前
            /// </summary>
            private string mName;
            /// <summary>
            /// 辞書を有効とするかどうか
            /// </summary>
            private bool mEnabled;
            /// <summary>
            /// 英単語の分節分割などにより，この辞書を使うことによって最大いくつの発音記号列に分割されるか
            /// </summary>
            private int mMaxDivisions = 1;

            #region static field
            /// <summary>
            /// 辞書のリスト，辞書の優先順位の順番で格納
            /// </summary>
            private static List<SymbolTable> mTable = new List<SymbolTable>();
            /// <summary>
            /// VOCALOID2のシステム辞書を読み込んだかどうか
            /// </summary>
            private static bool mInitialized = false;
            #endregion

            #region Static Method and Property
            /// <summary>
            /// 英単語の分節分割などにより，登録されている辞書を使うことによって最大いくつの発音記号列に分割されるか
            /// </summary>
            /// <returns></returns>
            public static int getMaxDivisions()
            {
                int max = 1;
                int size = mTable.Count;
                for (int i = 0; i < size; i++) {
                    SymbolTable table = mTable[i];
                    max = Math.Max(max, table.mMaxDivisions);
                }
                return max;
            }

            /// <summary>
            /// 指定した優先順位の辞書本体を取得します
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public static SymbolTable getSymbolTable(int index)
            {
                if (!mInitialized) {
                    loadSystemDictionaries();
                }
                if (0 <= index && index < mTable.Count) {
                    return mTable[index];
                } else {
                    return null;
                }
            }

            /// <summary>
            /// 指定した辞書ファイルを読み込みます。
            /// </summary>
            /// <param name="dictionary_file"></param>
            /// <param name="name"></param>
            public static void loadDictionary(string dictionary_file, string name)
            {
                SymbolTable table = new SymbolTable(dictionary_file, false, true, "UTF-8");
                table.mName = name;
                mTable.Add(table);
            }

            /// <summary>
            /// VOCALOID2システムが使用する辞書を読み込みます。
            /// </summary>
            public static void loadSystemDictionaries()
            {
                if (mInitialized) {
                    return;
                }
                // 辞書フォルダからの読込み
                string editor_path = VocaloSysUtil.getEditorPath(SynthesizerType.VOCALOID2);
                if (editor_path != "") {
                    string path = Path.Combine(PortUtil.getDirectoryName(editor_path), "UDIC");
                    if (!Directory.Exists(path)) {
                        return;
                    }
                    string[] files = PortUtil.listFiles(path, "*.udc");
                    for (int i = 0; i < files.Length; i++) {
                        files[i] = PortUtil.getFileName(files[i]);
                        string dict = Path.Combine(path, files[i]);
                        mTable.Add(new SymbolTable(dict, true, false, "Shift_JIS"));
                    }
                }
                mInitialized = true;
            }

            /// <summary>
            /// 指定したディレクトリにある拡張辞書ファイル(拡張子*.eudc)を全て読み込みます
            /// </summary>
            /// <param name="directory"></param>
            public static void loadAllDictionaries(string directory)
            {
                // 起動ディレクトリ
                if (Directory.Exists(directory)) {
                    string[] files2 = PortUtil.listFiles(directory, "*.eudc");
                    for (int i = 0; i < files2.Length; i++) {
                        files2[i] = PortUtil.getFileName(files2[i]);
                        string dict = Path.Combine(directory, files2[i]);
                        mTable.Add(new SymbolTable(dict, true, false, "UTF-8"));
                    }
                }
            }

            /// <summary>
            /// 指定した歌詞から、発音記号列を引き当てます
            /// </summary>
            /// <param name="phrase"></param>
            /// <returns></returns>
            public static SymbolTableEntry attatch(string phrase)
            {
                int size = mTable.Count;
                for (int i = 0; i < size; i++) {
                    SymbolTable table = mTable[i];
                    if (table.isEnabled()) {
                        SymbolTableEntry ret = table.attatchImp(phrase);
                        if (ret != null) {
                            return ret;
                        }
                    }
                }
                return null;
            }

            [Obsolete]
            public static bool attatch(string phrase, out string result)
            {
                var entry = attatch(phrase);
                if (entry == null) {
                    result = "a";
                    return false;
                } else {
                    result = entry.getSymbol();
                    return true;
                }
            }

            [Obsolete]
            public static bool attatch(string phrase, ByRef<string> result)
            {
                var entry = attatch(phrase);
                if (entry == null) {
                    result.value = "a";
                    return false;
                } else {
                    result.value = entry.getSymbol();
                    return true;
                }
            }

            /// <summary>
            /// 登録されている辞書の個数を取得します
            /// </summary>
            /// <returns></returns>
            public static int getCount()
            {
                if (!mInitialized) {
                    loadSystemDictionaries();
                }
                return mTable.Count;
            }

            /// <summary>
            /// 辞書の優先順位と有効・無効を一括設定します
            /// </summary>
            /// <param name="list">辞書の名前・有効かどうかを表したValuePairを、辞書の優先順位の順番に格納したリスト</param>
            public static void changeOrder(List<ValuePair<string, Boolean>> list)
            {
                // 現在の辞書をバッファに退避
                List<SymbolTable> buff = new List<SymbolTable>();
                int size = mTable.Count;
                for (int i = 0; i < size; i++) {
                    buff.Add(mTable[i]);
                }

                // 現在の辞書をいったんクリア
                mTable.Clear();

                int count = list.Count;
                for (int i = 0; i < count; i++) {
                    ValuePair<string, Boolean> itemi = list[i];
                    for (int j = 0; j < size; j++) {
                        SymbolTable table = buff[j];
                        if (table.getName().Equals(itemi.getKey())) {
                            table.setEnabled(itemi.getValue());
                            mTable.Add(table);
                            break;
                        }
                    }
                }
            }
            #endregion

            /// <summary>
            /// この辞書のディープ・コピーを取得します
            /// </summary>
            /// <returns></returns>
            public object Clone()
            {
                return clone();
            }

            /// <summary>
            /// この辞書のディープ・コピーを取得します
            /// </summary>
            /// <returns></returns>
            public Object clone()
            {
                SymbolTable ret = new SymbolTable();
                ret.mDict = new SortedDictionary<string, SymbolTableEntry>();
                foreach (var key in mDict.Keys) {
                    ret.mDict[key] = (SymbolTableEntry)mDict[key].clone();
                }
                ret.mName = mName;
                ret.mEnabled = mEnabled;
                ret.mMaxDivisions = mMaxDivisions;
                return ret;
            }

            /// <summary>
            /// 使ってはいけないコンストラクタ
            /// </summary>
            private SymbolTable()
            {
            }

            /// <summary>
            /// 辞書の名前を取得します
            /// </summary>
            /// <returns></returns>
            public string getName()
            {
                return mName;
            }

            /// <summary>
            /// 辞書が有効かどうかを取得します
            /// </summary>
            /// <returns></returns>
            public bool isEnabled()
            {
                return mEnabled;
            }

            /// <summary>
            /// 辞書が有効かどうかを設定します
            /// </summary>
            /// <param name="value"></param>
            public void setEnabled(bool value)
            {
                mEnabled = value;
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="path">読み込む辞書ファイルのパス</param>
            /// <param name="is_udc_mode">VOCALOID2仕様の辞書ファイルかどうか</param>
            /// <param name="enabled">辞書ファイルを有効とするかどうか</param>
            /// <param name="encoding">辞書ファイルのテキストエンコーディング</param>
            public SymbolTable(string path, bool is_udc_mode, bool enabled, string encoding)
            {
                mDict = new SortedDictionary<string, SymbolTableEntry>();
                mEnabled = enabled;
                if (!System.IO.File.Exists(path)) {
                    return;
                }
                mName = PortUtil.getFileName(path);
                StreamReader sr = null;
                try {
                    sr = new StreamReader(path, Encoding.GetEncoding(encoding));
                    if (sr == null) {
                        return;
                    }
                    string line;
                    while ((line = sr.ReadLine()) != null) {
                        if (line.StartsWith("//")) {
                            continue;
                        }
                        string key = "";
                        string word = "";
                        string symbol = "";
                        if (is_udc_mode) {
                            string[] spl = PortUtil.splitString(line, new string[] { "\t" }, 2, true);
                            if (spl.Length >= 2) {
                                key = spl[0].ToLower();
                                word = key;
                                symbol = spl[1];
                            }
                        } else {
                            string[] spl = PortUtil.splitString(line, new string[] { "\t\t" }, 2, true);
                            if (spl.Length >= 2) {
                                string[] spl_word = PortUtil.splitString(spl[0], '\t');
                                mMaxDivisions = Math.Max(spl_word.Length, mMaxDivisions);
                                key = spl[0].Replace("-\t", "");
                                word = spl[0];
                                symbol = spl[1];
                            }
                        }
                        if (!key.Equals("")) {
                            if (!mDict.ContainsKey(key)) {
                                mDict[key] = new SymbolTableEntry(word, symbol);
                            }
                        }
                    }
                } catch (Exception ex) {
                    serr.println("SymbolTable#.ctor; ex=" + ex);
                } finally {
                    if (sr != null) {
                        try {
                            sr.Close();
                        } catch (Exception ex2) {
                            serr.println("SymbolTable#.ctor; ex=" + ex2);
                        }
                    }
                }
            }

            /// <summary>
            /// 指定した文字列から、発音記号列を引き当てます
            /// </summary>
            /// <param name="phrase"></param>
            /// <returns></returns>
            private SymbolTableEntry attatchImp(string phrase)
            {
                string s = phrase.ToLower();
                if (mDict.ContainsKey(s)) {
                    return mDict[s];
                } else {
                    return null;
                }
            }
        }

    }
}
