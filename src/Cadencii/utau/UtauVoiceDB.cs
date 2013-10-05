/*
 * UtauVoiceDB.cs
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
using System.IO;
using System.Collections.Generic;
using cadencii.vsq;

namespace cadencii.utau
{
    /// <summary>
    /// UTAUの原音設定を表すクラス
    /// </summary>
    public class UtauVoiceDB
    {
        private string name_ = "Unknown";
        private Oto root_;
        private PrefixMap prefixmap_;
        private List<Oto> sub_ = new List<Oto>();

        /// <summary>
        /// コンストラクタ．
        /// </summary>
        /// <param name="singer_config"></param>
        public UtauVoiceDB(SingerConfig singer_config)
        {
            name_ = singer_config.VOICENAME;

            string oto_ini = Path.Combine(singer_config.VOICEIDSTR, "oto.ini");
            root_ = new Oto(oto_ini, singer_config.VOICEIDSTR);

            var prefixmap = Path.Combine(singer_config.VOICEIDSTR, "prefix.map");
            if (File.Exists(prefixmap)) {
                prefixmap_ = new PrefixMap(prefixmap);
            }

            foreach (var directory in Directory.EnumerateDirectories(singer_config.VOICEIDSTR)) {
                var maybe_oto_file = Path.Combine(directory, "oto.ini");
                if (File.Exists(maybe_oto_file)) {
                    var oto = new Oto(maybe_oto_file, singer_config.VOICEIDSTR);
                    sub_.Add(oto);
                }
            }
        }

        /// <summary>
        /// 指定した歌詞に合致する、エイリアスを考慮した原音設定を取得します
        /// </summary>
        /// <param name="lyric"></param>
        /// <returns></returns>
        public OtoArgs attachFileNameFromLyric(string lyric, int note_number)
        {
            Func<string, OtoArgs> get_oto_arg = (l) => {
                var result = root_.attachFileNameFromLyric(l);
                if (!result.isEmpty()) {
                    return result;
                }
                foreach (var oto in sub_) {
                    var args = oto.attachFileNameFromLyric(l);
                    if (!args.isEmpty()) {
                        return args;
                    }
                }
                return new OtoArgs();
            };

            // first, try finding mapped lyric.
            if (prefixmap_ != null) {
                var mapped_lyric = prefixmap_.getMappedLyric(lyric, note_number);
                var args = get_oto_arg(mapped_lyric);
                if (!args.isEmpty()) {
                    return args;
                }
            }

            return get_oto_arg(lyric);
        }

        /// <summary>
        /// この原音の名称を取得します．
        /// </summary>
        /// <returns>この原音の名称</returns>
        public string getName()
        {
            return name_;
        }
    }
}
