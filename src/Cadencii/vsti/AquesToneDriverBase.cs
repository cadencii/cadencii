#if ENABLE_AQUESTONE
/*
 * AquesToneDriverBase.cs
 * Copyright © 2009-2013 kbinani
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
using System.Text;
using System.IO;
using cadencii;
using cadencii.java.io;
using cadencii.vsq;

namespace cadencii
{

    public abstract class AquesToneDriverBase : VSTiDriverBase
    {

#if ENABLE_AQUESTONE

        /// <summary>
        /// AquesTone VSTi の DLL パスを指定して初期化する
        /// </summary>
        /// <param name="dllPath">AquesTone VSTi の DLL パス</param>
        public AquesToneDriverBase(string dllPath)
        {
            path = dllPath;
            loaded = open(44100, 44100);
        }

        /// <summary>
        /// win.ini にて使用されるセクション名を取得する
        /// </summary>
        /// <returns>セクション名の文字列。両端の"[", "]"は含めない</returns>
        protected abstract string getConfigSectionKey();

        /// <summary>
        /// win.ini にて使用される、Koe ファイルを指定するキー名
        /// </summary>
        /// <returns>キー名の文字列</returns>
        protected abstract string getKoeConfigKey();

        /// <summary>
        /// Koe ファイルに書き込むファイルの中身を取得する
        /// </summary>
        /// <returns></returns>
        protected abstract string[] getKoeFileContents();

        public override bool open(int block_size, int sample_rate)
        {
            int strlen = 260;
            StringBuilder sb = new StringBuilder(strlen);
            win32.GetProfileString(getConfigSectionKey(), getKoeConfigKey(), "", sb, (uint)strlen);
            string koe_old = sb.ToString();

            string required = prepareKoeFile();
            bool refresh_winini = false;
            if (!required.Equals(koe_old) && !koe_old.Equals("")) {
                refresh_winini = true;
            }
            win32.WriteProfileString(getConfigSectionKey(), getKoeConfigKey(), required);
            bool ret = false;
            try {
                ret = base.open(block_size, sample_rate);
            } catch (Exception ex) {
                ret = false;
                reportError(GetType() + ".open; ex=" + ex);
            }

            if (refresh_winini) {
                win32.WriteProfileString(getConfigSectionKey(), getKoeConfigKey(), koe_old);
            }
            return ret;
        }

        private string prepareKoeFile()
        {
            string ret = PortUtil.createTempFile();
            StreamWriter bw = null;
            try {
                bw = new StreamWriter(ret, false, Encoding.GetEncoding("Shift_JIS"));
                foreach (string s in getKoeFileContents()) {
                    bw.WriteLine(s);
                }
            } catch (Exception ex) {
                reportError(GetType() + ".getKoeFilePath; ex=" + ex);
            } finally {
                if (bw != null) {
                    try {
                        bw.Close();
                    } catch (Exception ex2) {
                        reportError(GetType() + ".getKoeFilePath; ex=" + ex2);
                    }
                }
            }
            return ret;
        }

        protected void reportError(string s)
        {
            Logger.writeLine(s);
            serr.println(s);
        }

#endif
    }

}
#endif
