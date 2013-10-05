/*
 * VsqMixer.cs
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
using cadencii;
using cadencii.java.util;

namespace cadencii.vsq
{

    /// <summary>
    /// vsqファイルのメタテキストの[Mixer]セクションに記録される内容を取り扱う
    /// </summary>
    [Serializable]
    public class VsqMixer : ICloneable
    {
        public const int FEDER_MIN = -898;
        public const int FEDER_MAX = 55;

        public int MasterFeder;
        public int MasterPanpot;
        public int MasterMute;
        public int OutputMode;

        /// <summary>
        /// vsqファイルの各トラックのfader, panpot, muteおよびoutputmode値を保持します
        /// </summary>
        public List<VsqMixerEntry> Slave = new List<VsqMixerEntry>();

        /// <summary>
        /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
        /// 要素名を取得します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getXmlElementName(string name)
        {
            return name;
        }

        public Object clone()
        {
            VsqMixer res = new VsqMixer(MasterFeder, MasterPanpot, MasterMute, OutputMode);
            res.Slave = new List<VsqMixerEntry>();
            foreach (var item in Slave) {
                res.Slave.Add((VsqMixerEntry)item.clone());
            }
            return res;
        }

        public object Clone()
        {
            return clone();
        }

        /// <summary>
        /// 各パラメータを指定したコンストラクタ
        /// </summary>
        /// <param name="master_fader">MasterFader値</param>
        /// <param name="master_panpot">MasterPanpot値</param>
        /// <param name="master_mute">MasterMute値</param>
        /// <param name="output_mode">OutputMode値</param>
        public VsqMixer(int master_fader, int master_panpot, int master_mute, int output_mode)
        {
            this.MasterFeder = master_fader;
            this.MasterMute = master_mute;
            this.MasterPanpot = master_panpot;
            this.OutputMode = output_mode;
            Slave = new List<VsqMixerEntry>();
        }

        public VsqMixer()
            : this(0, 0, 0, 0)
        {
        }

        /// <summary>
        /// テキストファイルからのコンストラクタ
        /// </summary>
        /// <param name="sr">読み込み対象</param>
        /// <param name="last_line">最後に読み込んだ行が返されます</param>
        public VsqMixer(TextStream sr, ByRef<string> last_line)
        {
            MasterFeder = 0;
            MasterPanpot = 0;
            MasterMute = 0;
            OutputMode = 0;
            //Tracks = 1;
            int tracks = 0;
            string[] spl;
            string buffer = "";
            last_line.value = sr.readLine();
            while (!last_line.value.StartsWith("[")) {
                spl = PortUtil.splitString(last_line.value, new char[] { '=' });
                if (spl[0].Equals("MasterFeder")) {
                    MasterFeder = int.Parse(spl[1]);
                } else if (spl[0].Equals("MasterPanpot")) {
                    MasterPanpot = int.Parse(spl[1]);
                } else if (spl[0].Equals("MasterMute")) {
                    MasterMute = int.Parse(spl[1]);
                } else if (spl[0].Equals("OutputMode")) {
                    OutputMode = int.Parse(spl[1]);
                } else if (spl[0].Equals("Tracks")) {
                    tracks = int.Parse(spl[1]);
                } else {
                    if (spl[0].StartsWith("Feder") ||
                         spl[0].StartsWith("Panpot") ||
                         spl[0].StartsWith("Mute") ||
                         spl[0].StartsWith("Solo")) {
                        buffer += spl[0] + "=" + spl[1] + "\n";
                    }
                }
                if (!sr.ready()) {
                    break;
                }
                last_line.value = sr.readLine().ToString();
            }

            Slave = new List<VsqMixerEntry>();
            for (int i = 0; i < tracks; i++) {
                Slave.Add(new VsqMixerEntry(0, 0, 0, 0));
            }
            spl = PortUtil.splitString(buffer, new string[] { "\n" }, true);
            string[] spl2;
            for (int i = 0; i < spl.Length; i++) {
                string ind = "";
                int index;
                spl2 = PortUtil.splitString(spl[i], new char[] { '=' });
                if (spl2[0].StartsWith("Feder")) {
                    ind = spl2[0].Replace("Feder", "");
                    index = int.Parse(ind);
                    Slave[index].Feder = int.Parse(spl2[1]);
                } else if (spl2[0].StartsWith("Panpot")) {
                    ind = spl2[0].Replace("Panpot", "");
                    index = int.Parse(ind);
                    Slave[index].Panpot = int.Parse(spl2[1]);
                } else if (spl2[0].StartsWith("Mute")) {
                    ind = spl2[0].Replace("Mute", "");
                    index = int.Parse(ind);
                    Slave[index].Mute = int.Parse(spl2[1]);
                } else if (spl2[0].StartsWith("Solo")) {
                    ind = spl2[0].Replace("Solo", "");
                    index = int.Parse(ind);
                    Slave[index].Solo = int.Parse(spl2[1]);
                }

            }
        }

        /// <summary>
        /// このインスタンスをテキストファイルに出力します
        /// </summary>
        /// <param name="sw">出力対象</param>
        public void write(ITextWriter sw)
        {
            sw.writeLine("[Mixer]");
            sw.writeLine("MasterFeder=" + MasterFeder);
            sw.writeLine("MasterPanpot=" + MasterPanpot);
            sw.writeLine("MasterMute=" + MasterMute);
            sw.writeLine("OutputMode=" + OutputMode);
            int count = Slave.Count;
            sw.writeLine("Tracks=" + count);
            for (int i = 0; i < count; i++) {
                VsqMixerEntry item = Slave[i];
                sw.writeLine("Feder" + i + "=" + item.Feder);
                sw.writeLine("Panpot" + i + "=" + item.Panpot);
                sw.writeLine("Mute" + i + "=" + item.Mute);
                sw.writeLine("Solo" + i + "=" + item.Solo);
            }
        }
    }

}
