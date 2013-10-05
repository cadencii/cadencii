/*
 * Messaging.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Collections.Generic;
using cadencii;
using cadencii.java.util;

namespace cadencii.apputil
{
    public class Messaging
    {
        private static string s_lang = "";
        private static List<MessageBody> s_messages = new List<MessageBody>();

        public static string[] getKeys(string lang)
        {
            foreach (var dict in s_messages) {
                if (lang.Equals(dict.lang)) {
                    List<string> list = new List<string>();
                    foreach (var key in dict.list.Keys) {
                        list.Add(key);
                    }
                    return list.ToArray();
                }
            }
            return null;
        }

        public static string[] getRegisteredLanguage()
        {
            List<string> res = new List<string>();
            foreach (var dict in s_messages) {
                res.Add(dict.lang);
            }
            return res.ToArray();
        }

        public static string getLanguage()
        {
            if (!s_lang.Equals("")) {
                return s_lang;
            } else {
                s_lang = "en";
                return s_lang;
            }
        }

        public static void setLanguage(string value)
        {
            if (!value.Equals("")) {
                s_lang = value;
            } else {
                s_lang = "en";
            }
        }

        /// <summary>
        /// 現在の実行ディレクトリにある言語設定ファイルを全て読込み、メッセージリストに追加します
        /// </summary>
        public static void loadMessages()
        {
            loadMessages(PortUtil.getApplicationStartupPath());
        }

        /// <summary>
        /// 指定されたディレクトリにある言語設定ファイルを全て読込み、メッセージリストに追加します
        /// </summary>
        /// <param name="directory"></param>
        public static void loadMessages(string directory)
        {
            s_messages.Clear();
            string[] files = PortUtil.listFiles(directory, ".po");
            for (int i = 0; i < files.Length; i++) {
                string name = PortUtil.getFileName(files[i]);
                string fname = Path.Combine(directory, name);
                appendFromFile(fname);
            }
        }

        public static void appendFromFile(string file)
        {
            s_messages.Add(new MessageBody(PortUtil.getFileNameWithoutExtension(file), file));
        }

        public static MessageBodyEntry getMessageDetail(string id)
        {
            if (s_lang.Equals("")) {
                s_lang = "en";
            }
            foreach (var mb in s_messages) {
                if (mb.lang.Equals(s_lang)) {
                    return mb.getMessageDetail(id);
                }
            }
            return new MessageBodyEntry(id, new string[] { });
        }

        public static string getMessage(string id)
        {
            if (s_lang.Equals("")) {
                s_lang = "en";
            }
            foreach (var mb in s_messages) {
                if (mb.lang.Equals(s_lang)) {
                    return mb.getMessage(id);
                }
            }
            return id;
        }

        public static string getRuntimeLanguageName()
        {
            var name = System.Windows.Forms.Application.CurrentCulture.Name;
            if (name == "ja" || name.StartsWith("ja-")) {
                return "ja";
            } else {
                return name;
            }
        }
    }

}
