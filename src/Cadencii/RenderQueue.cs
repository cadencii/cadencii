/*
 * RenderQueue.cs
 * Copyright © 2009-2011 kbinani
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
using System.Collections.Generic;
using cadencii.java.util;
using cadencii.utau;



namespace cadencii
{

    public class RenderQueue
    {
        private List<string> _resampler_arg = new List<string>();
        public List<string> WavtoolArgPrefix = new List<string>();
        public List<string> WavtoolArgSuffix = new List<string>();
        //public String WavtoolArgPrefix;
        //public String WavtoolArgSuffix;
        public OtoArgs Oto;
        //public double secEnd;
        public double secStart;
        public string FileName;
        public bool ResamplerFinished;
        /// <summary>
        /// MD5ハッシュによるファイル名の生成元となる文字列
        /// </summary>
        public string hashSource;

        /// <summary>
        /// このキューの引数リストに、引数を1つ追加します
        /// </summary>
        /// <param name="value"></param>
        public void appendArg(string value)
        {
            _resampler_arg.Add(value);
        }

        /// <summary>
        /// このキューの引数リストに、指定された引数をすべて追加します
        /// </summary>
        /// <param name="args"></param>
        public void appendArgRange(string[] args)
        {
            foreach (string s in args) {
                _resampler_arg.Add(s);
            }
        }

        /// <summary>
        /// このキューの引数リストを、文字列配列の形式で取得します
        /// </summary>
        /// <returns></returns>
        public string[] getResamplerArg()
        {
            return _resampler_arg.ToArray();
        }

        /// <summary>
        /// このキューの引数リストを、スペースで繋げた文字列形式で取得します
        /// </summary>
        /// <returns></returns>
        public string getResamplerArgString()
        {
            string ret = "";
            int c = _resampler_arg.Count;
            for (int i = 0; i < c; i++) {
                ret += _resampler_arg[i] + ((i < c - 1) ? " " : "");
            }
            return ret;
        }
    }

}
