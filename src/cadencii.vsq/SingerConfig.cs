/*
 * SingerConfig.cs
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
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii.vsq
{

    public class SingerConfig : ICloneable
    {
        public string ID = "";
        public string FORMAT = "";
        /// <summary>
        /// VOCALOIDの場合，音源のディレクトリへのパス．
        /// UTAUの場合，oto.iniが保存されているディレクトリへのパス
        /// </summary>
        public string VOICEIDSTR = "";
        public string VOICENAME = "Unknown";
        public int Breathiness;
        public int Brightness;
        public int Clearness;
        public int Opening;
        public int GenderFactor;
        public int Program;
        public int Resonance1Amplitude;
        public int Resonance1Frequency;
        public int Resonance1BandWidth;
        public int Resonance2Amplitude;
        public int Resonance2Frequency;
        public int Resonance2BandWidth;
        public int Resonance3Amplitude;
        public int Resonance3Frequency;
        public int Resonance3BandWidth;
        public int Resonance4Amplitude;
        public int Resonance4Frequency;
        public int Resonance4BandWidth;
        public int Harmonics;
        public string VvdPath = "";
        public int Language;

        public SingerConfig()
        {
        }

        public SingerConfig(string voiceName, int language, int program)
        {
            VOICENAME = voiceName;
            Language = language;
            Program = program;
        }

        public Object clone()
        {
            SingerConfig ret = new SingerConfig();
            ret.ID = ID;
            ret.FORMAT = FORMAT;
            ret.VOICEIDSTR = VOICEIDSTR;
            ret.VOICENAME = VOICENAME;
            ret.Breathiness = Breathiness;
            ret.Brightness = Brightness;
            ret.Clearness = Clearness;
            ret.Opening = Opening;
            ret.GenderFactor = GenderFactor;
            ret.Program = Program;
            ret.Resonance1Amplitude = Resonance1Amplitude;
            ret.Resonance1Frequency = Resonance1Frequency;
            ret.Resonance1BandWidth = Resonance1BandWidth;
            ret.Resonance2Amplitude = Resonance2Amplitude;
            ret.Resonance2Frequency = Resonance2Frequency;
            ret.Resonance2BandWidth = Resonance2BandWidth;
            ret.Resonance3Amplitude = Resonance3Amplitude;
            ret.Resonance3Frequency = Resonance3Frequency;
            ret.Resonance3BandWidth = Resonance3BandWidth;
            ret.Resonance4Amplitude = Resonance4Amplitude;
            ret.Resonance4Frequency = Resonance4Frequency;
            ret.Resonance4BandWidth = Resonance4BandWidth;
            ret.Harmonics = Harmonics;
            ret.VvdPath = VvdPath;
            ret.Language = Language;
            return ret;
        }

        public object Clone()
        {
            return clone();
        }

        public static SingerConfig fromVvd(string file, int language, int program)
        {
            SingerConfig sc = new SingerConfig();
            sc.ID = "VOCALOID:VIRTUAL:VOICE";
            sc.FORMAT = "2.0.0.0";
            sc.VOICEIDSTR = "";
            sc.VOICENAME = "Miku";
            sc.Breathiness = 0;
            sc.Brightness = 0;
            sc.Clearness = 0;
            sc.Opening = 0;
            sc.GenderFactor = 0;
            sc.VvdPath = file;
            sc.Language = language;
            sc.Program = program;
            Stream fs = null;
            try {
                fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                int length = (int)fs.Length;
                byte[] dat = new byte[length];
                fs.Read(dat, 0, length);
                TransCodeUtil.decodeBytes(dat);
                int[] idat = new int[length];
                for (int i = 0; i < length; i++) {
                    idat[i] = dat[i];
                }
                string str1 = PortUtil.getDecodedString("Shift_JIS", idat);
#if DEBUG
                sout.println("SingerConfig.readSingerConfig; str1=" + str1);
#endif
                string crlf = "" + (char)0x0d + "" + (char)0x0a;
                string[] spl = PortUtil.splitString(str1, new string[] { crlf }, true);

                int count = spl.Length;
                for (int i = 0; i < spl.Length; i++) {
                    string s = spl[i];
                    int first = s.IndexOf('"');
                    int first_end = get_quated_string(s, first);
                    int second = s.IndexOf('"', first_end + 1);
                    int second_end = get_quated_string(s, second);
                    char[] chs = s.ToCharArray();
                    string id = new string(chs, first, first_end - first + 1);
                    string value = new string(chs, second, second_end - second + 1);
                    id = id.Substring(1, PortUtil.getStringLength(id) - 2);
                    value = value.Substring(1, PortUtil.getStringLength(value) - 2);
                    value = value.Replace("\\" + "\"", "\"");
                    int parsed_int = 64;
                    try {
                        parsed_int = int.Parse(value);
                    } catch (Exception ex) {
                    }
                    if (id.Equals("ID")) {
                        sc.ID = value;
                    } else if (id.Equals("FORMAT")) {
                        sc.FORMAT = value;
                    } else if (id.Equals("VOICEIDSTR")) {
                        sc.VOICEIDSTR = value;
                    } else if (id.Equals("VOICENAME")) {
                        sc.VOICENAME = value;
                    } else if (id.Equals("Breathiness") || id.Equals("Noise")) {
                        sc.Breathiness = parsed_int;
                    } else if (id.Equals("Brightness")) {
                        sc.Brightness = parsed_int;
                    } else if (id.Equals("Clearness")) {
                        sc.Clearness = parsed_int;
                    } else if (id.Equals("Opening")) {
                        sc.Opening = parsed_int;
                    } else if (id.Equals("Gender:Factor")) {
                        sc.GenderFactor = parsed_int;
                    } else if (id.Equals("Resonance1:Frequency")) {
                        sc.Resonance1Frequency = parsed_int;
                    } else if (id.Equals("Resonance1:Band:Width")) {
                        sc.Resonance1BandWidth = parsed_int;
                    } else if (id.Equals("Resonance1:Amplitude")) {
                        sc.Resonance1Amplitude = parsed_int;
                    } else if (id.Equals("Resonance2:Frequency")) {
                        sc.Resonance2Frequency = parsed_int;
                    } else if (id.Equals("Resonance2:Band:Width")) {
                        sc.Resonance2BandWidth = parsed_int;
                    } else if (id.Equals("Resonance2:Amplitude")) {
                        sc.Resonance2Amplitude = parsed_int;
                    } else if (id.Equals("Resonance3:Frequency")) {
                        sc.Resonance3Frequency = parsed_int;
                    } else if (id.Equals("Resonance3:Band:Width")) {
                        sc.Resonance3BandWidth = parsed_int;
                    } else if (id.Equals("Resonance3:Amplitude")) {
                        sc.Resonance3Amplitude = parsed_int;
                    } else if (id.Equals("Resonance4:Frequency")) {
                        sc.Resonance4Frequency = parsed_int;
                    } else if (id.Equals("Resonance4:Band:Width")) {
                        sc.Resonance4BandWidth = parsed_int;
                    } else if (id.Equals("Resonance4:Amplitude")) {
                        sc.Resonance4Amplitude = parsed_int;
                    } else if (id.Equals("Harmonics")) {
                        sc.Harmonics = parsed_int;
                    }
                }
            } catch (Exception ex) {

            } finally {
                if (fs != null) {
                    try {
                        fs.Close();
                    } catch (Exception ex2) {
                    }
                }
            }
            return sc;
        }

        /// <summary>
        /// 位置positionにある'"'から，次に現れる'"'の位置を調べる．エスケープされた\"はスキップされる．'"'が見つからなかった場合-1を返す
        /// </summary>
        /// <param name="s"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        static int get_quated_string(string s, int position)
        {
            if (position < 0) {
                return -1;
            }
            char[] chs = s.ToCharArray();
            if (position >= chs.Length) {
                return -1;
            }
            if (chs[position] != '"') {
                return -1;
            }
            int end = -1;
            for (int i = position + 1; i < chs.Length; i++) {
                if (chs[i] == '"' && chs[i - 1] != '\\') {
                    end = i;
                    break;
                }
            }
            return end;
        }

        public string[] ToStringArray()
        {
            List<string> ret = new List<string>();
            ret.Add("\"ID\":=:\"" + ID + "\"");
            ret.Add("\"FORMAT\":=:\"" + FORMAT + "\"");
            ret.Add("\"VOICEIDSTR\":=:\"" + VOICEIDSTR + "\"");
            ret.Add("\"VOICENAME\":=:\"" + VOICENAME.Replace("\"", "\\\"") + "\"");
            ret.Add("\"Breathiness\":=:\"" + Breathiness + "\"");
            ret.Add("\"Brightness\":=:\"" + Brightness + "\"");
            ret.Add("\"Clearness\":=:\"" + Clearness + "\"");
            ret.Add("\"Opening\":=:\"" + Opening + "\"");
            ret.Add("\"Gender:Factor\":=:\"" + GenderFactor + "\"");
            return ret.ToArray();
        }

        public override string ToString()
        {
            return toString();
        }

        public string toString()
        {
            string[] r = ToStringArray();
            string ret = "";
            int count = r.Length;
            for (int i = 0; i < count; i++) {
                string s = r[i];
                ret += s + "\n";
            }
            return ret;
        }
    }

}
