/*
 * PrefixMap.cs
 * Copyright © 2013 kbinani
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
using System.IO;
using System.Text;
using System.Linq;

namespace cadencii.utau
{
    /// <summary>
    /// Represents prefix.map information.
    /// </summary>
    public class PrefixMap
    {
        class Config
        {
            public string prefix_;
            public string suffix_;
        }

        private Dictionary<int, Config> configs_ = new Dictionary<int, Config>();

        public PrefixMap(string prefixmap_file_path)
        {
            var encoding = Encoding.GetEncoding("Shift_JIS");
            using (var stream = new StreamReader(prefixmap_file_path, encoding)) {
                stream.EachLines((line) => {
                    var parameters = line.Split(new[] { '\t' }, 3);
                    if (parameters.Length == 3) {
                        var config = new Config();

                        var note_string = parameters[0];
                        config.prefix_ = parameters[1];
                        config.suffix_ = parameters[2];
                        int note = NoteNumberPropertyConverter.parse(note_string);
                        configs_[note] = config;
                    }
                });
            }
        }

        public string getMappedLyric(string lyric, int note_number)
        {
            if (configs_.ContainsKey(note_number)) {
                var config = configs_[note_number];
                return config.prefix_ + lyric + config.suffix_;
            } else {
                return lyric;
            }
        }
    }
}
