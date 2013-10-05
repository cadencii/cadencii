/*
 * MessageBody.cs
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
using System.Collections.Generic;
using System.IO;
using System.Text;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii.apputil
{

    public class MessageBody
    {
        public string lang;
        public string poHeader = "";
        public SortedDictionary<string, MessageBodyEntry> list = new SortedDictionary<string, MessageBodyEntry>();

        public MessageBody(string lang_)
        {
            lang = lang_;
        }

        public MessageBody(string lang, string[] ids, string[] messages)
        {
            this.lang = lang;
            list = new SortedDictionary<string, MessageBodyEntry>();
            for (int i = 0; i < ids.Length; i++) {
                list[ids[i]] = new MessageBodyEntry(messages[i], new string[] { });
            }
        }

        public MessageBody(string lang_, string file)
        {
            lang = lang_;
            poHeader = "";
            StreamReader sr = null;
            try {
                sr = new StreamReader(file, Encoding.GetEncoding("UTF-8"));
                string line2 = "";
                while ((line2 = sr.ReadLine()) != null) {
                    ByRef<string> msgid = new ByRef<string>("");
                    string first_line = line2;
                    ByRef<string[]> location = new ByRef<string[]>();
                    string last_line = readTillMessageEnd(sr, first_line, "msgid", msgid, location);
                    ByRef<string> msgstr = new ByRef<string>("");
                    ByRef<string[]> location_dumy = new ByRef<string[]>();
                    last_line = readTillMessageEnd(sr, last_line, "msgstr", msgstr, location_dumy);
                    if (PortUtil.getStringLength(msgid.value) > 0) {
                        list[msgid.value] = new MessageBodyEntry(msgstr.value, location.value);
                    } else {
                        poHeader = msgstr.value;
                        string[] spl = PortUtil.splitString(poHeader, new char[] { (char)0x0d, (char)0x0a }, true);
                        poHeader = "";
                        int count = 0;
                        for (int i = 0; i < spl.Length; i++) {
                            string line = spl[i];
                            string[] spl2 = PortUtil.splitString(line, new char[] { ':' }, 2);
                            if (spl2.Length == 2) {
                                string name = spl2[0].Trim();
                                string ct = "Content-Type";
                                string cte = "Content-Transfer-Encoding";
                                if (name.ToLower().Equals(ct.ToLower())) {
                                    poHeader += (count == 0 ? "" : "\n") + "Content-Type: text/plain; charset=UTF-8";
                                } else if (name.ToLower().Equals(cte.ToLower())) {
                                    poHeader += (count == 0 ? "" : "\n") + "Content-Transfer-Encoding: 8bit";
                                } else {
                                    poHeader += (count == 0 ? "" : "\n") + line;
                                }
                            } else {
                                poHeader += (count == 0 ? "" : "\n") + line;
                            }
                            count++;
                        }
                    }
                }
            } catch (Exception ex) {
            } finally {
                if (sr != null) {
                    try {
                        sr.Close();
                    } catch (Exception ex2) {
                    }
                }
            }
        }

        public string getMessage(string id)
        {
            if (list.ContainsKey(id)) {
                string ret = list[id].message;
                if (ret.Equals("")) {
                    return id;
                } else {
                    return list[id].message;
                }
            }
            return id;
        }

        public MessageBodyEntry getMessageDetail(string id)
        {
            if (list.ContainsKey(id)) {
                string ret = list[id].message;
                if (ret.Equals("")) {
                    return new MessageBodyEntry(id, new string[] { });
                } else {
                    return list[id];
                }
            }
            return new MessageBodyEntry(id, new string[] { });
        }

        public void write(string file)
        {
            StreamWriter sw = null;
            try {
                sw = new StreamWriter(file, false, new UTF8Encoding(false));
                if (!poHeader.Equals("")) {
                    sw.WriteLine("msgid \"\"");
                    sw.WriteLine("msgstr \"\"");
                    string[] spl = PortUtil.splitString(poHeader, new char[] { (char)0x0d, (char)0x0a }, true);
                    for (int i = 0; i < spl.Length; i++) {
                        string line = spl[i];
                        sw.WriteLine("\"" + line + "\\" + "n\"");
                    }
                    sw.WriteLine();
                } else {
                    sw.WriteLine("msgid \"\"");
                    sw.WriteLine("msgstr \"\"");
                    sw.WriteLine("\"Content-Type: text/plain; charset=UTF-8\\" + "n\"");
                    sw.WriteLine("\"Content-Transfer-Encoding: 8bit\\" + "n\"");
                    sw.WriteLine();
                }
                foreach (var key in list.Keys) {
                    string skey = key.Replace("\n", "\\n\"\n\"");
                    MessageBodyEntry mbe = list[key];
                    string s = mbe.message;
                    List<string> location = mbe.location;
                    int count = location.Count;
                    for (int i = 0; i < count; i++) {
                        sw.WriteLine("#: " + location[i]);
                    }
                    sw.WriteLine("msgid \"" + skey + "\"");
                    s = s.Replace("\n", "\\n\"\n\"");
                    sw.WriteLine("msgstr \"" + s + "\"");
                    sw.WriteLine();
                }
            } catch (Exception ex) {
            } finally {
                if (sw != null) {
                    try {
                        sw.Close();
                    } catch (Exception ex2) {
                    }
                }
            }
        }

        private static void separateEntryAndMessage(string source, ByRef<string> entry, ByRef<string> message)
        {
            string line = source.Trim();
            entry.value = "";
            message.value = "";
            if (PortUtil.getStringLength(line) <= 0) {
                return;
            }
            int index_space = line.IndexOf(' ');
            int index_dquoter = line.IndexOf('"');
            int index = Math.Min(index_dquoter, index_space);
            entry.value = line.Substring(0, index);
            message.value = line.Substring(index_dquoter + 1);
            message.value = message.value.Substring(0, PortUtil.getStringLength(message.value) - 1);
        }

        private static string readTillMessageEnd(StreamReader sr, string first_line, string entry, ByRef<string> msg, ByRef<string[]> locations)
        {
            msg.value = "";
            string line = first_line;
            List<string> location = new List<string>();
            bool entry_found = false;
            if (line.StartsWith(entry)) {
                // 1行目がすでに"entry"の行だった場合
                ByRef<string> dum = new ByRef<string>("");
                ByRef<string> dum2 = new ByRef<string>("");
                separateEntryAndMessage(line, dum, dum2);
                msg.value += dum2.value;
            } else {
                while ((line = sr.ReadLine()) != null) {
                    if (line.StartsWith("#:")) {
                        line = line.Substring(2).Trim();
                        location.Add(line);
                    } else if (line.StartsWith(entry)) {
                        ByRef<string> dum = new ByRef<string>("");
                        ByRef<string> dum2 = new ByRef<string>("");
                        separateEntryAndMessage(line, dum, dum2);
                        msg.value += dum2.value;
                        break;
                    }
                }
            }
            locations.value = location.ToArray();
            string ret = "";
            while ((line = sr.ReadLine()) != null) {
                if (!line.StartsWith("\"")) {
                    msg.value = msg.value.Replace("\\\"", "\"");
                    msg.value = msg.value.Replace("\\n", "\n");
                    return line;
                }
                int index = line.LastIndexOf("\"");
                msg.value += line.Substring(1, index - 1);
            }
            msg.value = msg.value.Replace("\\\"", "\"");
            msg.value = msg.value.Replace("\\n", "\n");
            if (line == null) {
                line = "";
            }
            return line;
        }
    }

}
