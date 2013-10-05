/*
 * RendererKindUtil.cs
 * Copyright © 2010-2011 kbinani
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
using System.Linq;
using cadencii;
using cadencii.java.awt;



namespace cadencii
{

    /// <summary>
    /// 歌声合成システムの種類
    /// </summary>
    public static class RendererKindUtil
    {
        /// <summary>
        /// 歌声合成システム、ビューのための設定値を保持する
        /// </summary>
        struct Config
        {
            public Config(string display,
                           string version,
                           Color background,
                           Color dark_background,
                           Color bar,
                           Color beat)
            {
                display_ = display;
                version_ = version;
                background_ = background;
                dark_background_ = dark_background;
                bar_ = bar;
                beat_ = beat;
            }

            /// <summary>
            /// 画面表記
            /// </summary>
            public string display_;
            /// <summary>
            /// VSQ に保存する際の、バージョン文字列
            /// </summary>
            public string version_;
            /// <summary>
            /// ピアノロールの背景色
            /// </summary>
            public Color background_;
            /// <summary>
            /// ピアノロールの黒鍵部分の背景色
            /// </summary>
            public Color dark_background_;
            /// <summary>
            /// 小節を区切る縦線の色
            /// </summary>
            public Color bar_;
            /// <summary>
            /// 拍を区切る縦線の色
            /// </summary>
            public Color beat_;

            public static readonly Config empty = new Config("VOCALOID2", "DSB301", new Color(240, 240, 240), new Color(212, 212, 212), new Color(161, 157, 136), new Color(209, 204, 172));
        }

        static readonly Dictionary<RendererKind, Config> configs_ = new Dictionary<RendererKind, Config>(){
            { RendererKind.VOCALOID1,     new Config( "VOCALOID1",      "DSB202", new Color( 240, 235, 214 ), new Color( 210, 205, 172 ), new Color( 161, 157, 136 ), new Color( 209, 204, 172 ) ) },
            { RendererKind.VOCALOID2,     new Config( "VOCALOID2",      "DSB301", new Color( 240, 240, 240 ), new Color( 212, 212, 212 ), new Color( 161, 157, 136 ), new Color( 209, 204, 172 ) ) },
            { RendererKind.STRAIGHT_UTAU, new Config( "vConnect-STAND", "STR000", new Color( 240, 240, 240 ), new Color( 212, 212, 212 ), new Color( 255, 153, 0 ), new Color( 128, 128, 255 ) ) },
            { RendererKind.UTAU,          new Config( "UTAU",           "UTU000", new Color( 240, 240, 240 ), new Color( 212, 212, 212 ), new Color( 255, 64, 255 ), new Color( 128, 128, 255 ) ) },
            { RendererKind.AQUES_TONE,    new Config( "AquesTone",      "AQT000", new Color( 240, 240, 240 ), new Color( 212, 212, 212 ), new Color( 7, 107, 175 ), new Color( 234, 190, 62 ) ) },
            { RendererKind.AQUES_TONE2,   new Config( "AquesTone2",     "AQT001", new Color( 240, 240, 240 ), new Color( 212, 212, 212 ), new Color( 237, 52, 36 ), new Color( 247, 162, 121 ) ) },
        };

        /// <summary>
        /// 画面にだす合成システム名を取得する
        /// </summary>
        /// <param name="value">合成システム</param>
        /// <returns>合成システム名。取得できない場合は空文字を返す</returns>
        public static string getString(this RendererKind value)
        {
            Config config;
            if (configs_.TryGetValue(value, out config)) {
                return config.display_;
            }
            return "";
        }

        /// <summary>
        /// 画面表記から、合成システム種類を取得する
        /// </summary>
        /// <param name="value">画面表記</param>
        /// <returns>合成システムの種類。取得できない場合は RendererKind.NULL を返す</returns>
        public static RendererKind fromString(string value)
        {
            try {
                return configs_.Where((item) => item.Value.display_ == value).Select((item) => item.Key).First();
            } catch (Exception e) {
                return RendererKind.NULL;
            }
        }

        /// <summary>
        /// 指定した音声合成システムを識別する文字列(DSB301, DSB202等)を取得します
        /// </summary>
        /// <param name="kind">歌声合成システムの種類</param>
        /// <returns>歌声合成システムを識別する文字列(VOCALOID2=DSB301, VOCALOID1[1.0,1.1]=DSB202, AquesTone=AQT000, Straight x UTAU=STR000, UTAU=UTAU000)</returns>
        public static string getVersionString(this RendererKind kind)
        {
            Config config;
            if (configs_.TryGetValue(kind, out config)) {
                return config.version_;
            }
            return "";
        }

        /// <summary>
        /// ピアノロールの背景色を取得する
        /// </summary>
        /// <param name="kind">歌声合成システムの種類</param>
        /// <returns>背景色。取得できない場合はデフォルトの背景色を返す</returns>
        public static Color getPianorollBackground(this RendererKind kind)
        {
            Config config;
            if (configs_.TryGetValue(kind, out config)) {
                return config.background_;
            }
            return Config.empty.background_;
        }

        /// <summary>
        /// ピアノロールの黒鍵部分の背景色を取得する
        /// </summary>
        /// <param name="kind">歌声合成システムの種類</param>
        /// <returns>背景色。取得できない場合はデフォルトの背景色を返す</returns>
        public static Color getPianorollDarkBackground(this RendererKind kind)
        {
            Config config;
            if (configs_.TryGetValue(kind, out config)) {
                return config.dark_background_;
            }
            return Config.empty.dark_background_;
        }

        /// <summary>
        /// ピアノロールの小節の区切り線の色を取得する
        /// </summary>
        /// <param name="kind">歌声合成システムの種類</param>
        /// <returns>区切り線の色。取得できない場合はデフォルトの色を返す</returns>
        public static Color getPianorollBar(this RendererKind kind)
        {
            Config config;
            if (configs_.TryGetValue(kind, out config)) {
                return config.bar_;
            }
            return Config.empty.bar_;
        }

        /// <summary>
        /// ピアノロールの拍の区切り線の色を取得する
        /// </summary>
        /// <param name="kind">歌声合成システムの種類</param>
        /// <returns>区切り線の色。取得できない場合はデフォルトの色を返す</returns>
        public static Color getPianorollBeat(this RendererKind kind)
        {
            Config config;
            if (configs_.TryGetValue(kind, out config)) {
                return config.beat_;
            }
            return Config.empty.beat_;
        }
    }

}
