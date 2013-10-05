/*
 * SingerConfigSys.cs
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
using System.IO;
using System.Collections.Generic;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii.vsq
{
    public class SingerConfigSys
    {
        public const int MAX_SINGERS = 0x4000;

        private List<SingerConfig> m_installed_singers = new List<SingerConfig>();
        private List<SingerConfig> m_singer_configs = new List<SingerConfig>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path_voicedb">音源のデータディレクトリ(ex:"C:\Program Files\VOCALOID2\voicedbdir")</param>
        /// <param name="path_installed_singers">音源のインストールディレクトリ(ex:new String[]{ "C:\Program Files\VOCALOID2\voicedbdir\BXXXXXXXXXXXXXXX", "D:\singers\BNXXXXXXXXXX" })</param>
        public SingerConfigSys(string path_voicedb, string[] path_installed_singers)
        {
            m_installed_singers = new List<SingerConfig>();
            m_singer_configs = new List<SingerConfig>();
            string map = Path.Combine(path_voicedb, "voice.map");
            if (!System.IO.File.Exists(map)) {
                return;
            }

            // インストールされている歌手の情報を読み取る。miku.vvd等から。
            for (int j = 0; j < path_installed_singers.Length; j++) {
                string ipath = path_installed_singers[j];
#if DEBUG
                sout.println("SingerConfigSys#.ctor; path_installed_singers[" + j + "]=" + path_installed_singers[j]);
#endif
                //TODO: ここでエラー起こる場合があるよ。SingerConfigSys::.ctor
                //      実際にディレクトリがある場合にのみ，ファイルのリストアップをするようにした．
                //      これで治っているかどうか要確認
                if (Directory.Exists(ipath)) {
                    string[] vvds = PortUtil.listFiles(ipath, "*.vvd");
                    if (vvds.Length > 0) {
                        SingerConfig installed = SingerConfig.fromVvd(vvds[0], 0, 0);
                        m_installed_singers.Add(installed);
                        break;
                    }
                }
            }

            // voice.mapから、プログラムチェンジ、バンクセレクトと音源との紐付け情報を読み出す。
            FileStream fs = null;
            try {
                fs = new FileStream(map, FileMode.Open, FileAccess.Read);
                byte[] dat = new byte[8];
                fs.Seek(0x20, SeekOrigin.Begin);
                for (int language = 0; language < 0x80; language++) {
                    for (int program = 0; program < 0x80; program++) {
                        fs.Read(dat, 0, 8);
                        long value = PortUtil.make_int64_le(dat);
                        if (value >= 1) {
                            string vvd = Path.Combine(path_voicedb, "vvoice" + value + ".vvd");
                            SingerConfig item = SingerConfig.fromVvd(vvd, language, program);
                            m_singer_configs.Add(item);
                        }
                    }
                }
            } catch (Exception ex) {
                serr.println("SingerConfigSys#.ctor; ex=" + ex);
            } finally {
                if (fs != null) {
                    try {
                        fs.Close();
                    } catch (Exception ex2) {
                        serr.println("SingerConfigSys#.ctor; ex2=" + ex2);
                    }
                }
            }

            // m_singer_configsの情報から、m_installed_singersの歌唱言語情報を類推する
            foreach (var sc in m_installed_singers) {
                string searchid = sc.VOICEIDSTR;
                foreach (var sc2 in m_singer_configs) {
                    if (sc2.VOICEIDSTR.Equals(searchid)) {
                        sc.Language = sc2.Language;
                        break;
                    }
                }
            }
        }

        public SingerConfig[] getInstalledSingers()
        {
            return m_installed_singers.ToArray();
        }

        /// <summary>
        /// Gets the VsqID of program change.
        /// </summary>
        /// <param name="program_change"></param>
        /// <returns></returns>        
        public VsqID getSingerID(int language, int program)
        {
            VsqID ret = new VsqID(0);
            ret.type = VsqIDType.Singer;
            SingerConfig sc = null;
            for (int i = 0; i < m_singer_configs.Count; i++) {
                SingerConfig itemi = m_singer_configs[i];
                if (itemi.Language == language && itemi.Program == program) {
                    sc = itemi;
                    break;
                }
            }
            if (sc == null) {
                sc = new SingerConfig();
            }
            ret.IconHandle = new IconHandle();
            ret.IconHandle.IconID = "$0701" + PortUtil.toHexString(sc.Language, 2) + PortUtil.toHexString(sc.Program, 2);
            ret.IconHandle.IDS = sc.VOICENAME;
            ret.IconHandle.Index = 0;
            ret.IconHandle.Language = sc.Language;
            ret.IconHandle.setLength(1);
            ret.IconHandle.Original = sc.Language << 8 | sc.Program;
            ret.IconHandle.Program = sc.Program;
            ret.IconHandle.Caption = "";
            return ret;
        }

        /// <summary>
        /// Gets the singer information of pecified program change.
        /// </summary>
        /// <param name="program_change"></param>
        /// <returns></returns>
        public SingerConfig getSingerInfo(int language, int program)
        {
            foreach (var item in m_installed_singers) {
                if (item.Language == language && item.Program == program) {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the list of singer configs.
        /// </summary>
        /// <returns></returns>
        public SingerConfig[] getSingerConfigs()
        {
            return m_singer_configs.ToArray();
        }
    }

}
