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
using System.IO;
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

        /// <summary>
        /// コンストラクタ．
        /// </summary>
        /// <param name="singer_config"></param>
        public UtauVoiceDB(SingerConfig singer_config)
        {
            name_ = singer_config.VOICENAME;
            string oto_ini = Path.Combine(singer_config.VOICEIDSTR, "oto.ini");
            root_ = new Oto(oto_ini, singer_config.VOICEIDSTR);
        }

        /// <summary>
        /// 指定した歌詞に合致する、エイリアスを考慮した原音設定を取得します
        /// </summary>
        /// <param name="lyric"></param>
        /// <returns></returns>
        public OtoArgs attachFileNameFromLyric(string lyric, int note_number)
        {
            return root_.attachFileNameFromLyric(lyric);
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
