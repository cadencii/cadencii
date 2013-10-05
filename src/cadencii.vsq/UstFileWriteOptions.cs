/*
 * UstFileWriteOptions.cs
 * Copyright © 2010-2011 kbinani
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

namespace cadencii.vsq
{

    /// <summary>
    /// UstFileクラスのwriteメソッドで出力する際の詳細オプションを表します
    /// </summary>
    public class UstFileWriteOptions
    {
        /// <summary>
        /// [#TRACKEND]セクションを出力するかどうか
        /// </summary>
        public bool trackEnd;
        /// <summary>
        /// [#SETTING]セクションのTempoエントリーを出力するかどうか
        /// </summary>
        public bool settingTempo;
        /// <summary>
        /// [#SETTING]セクションのTracksエントリーを出力するかどうか
        /// </summary>
        public bool settingTracks;
        /// <summary>
        /// [#SETTING]セクションのProjectNameエントリーを出力するかどうか
        /// </summary>
        public bool settingProjectName;
        /// <summary>
        /// [#SETTING]セクションのVoiceDirエントリーを出力するかどうか
        /// </summary>
        public bool settingVoiceDir;
        /// <summary>
        /// [#SETTING]セクションのOutFileエントリーを出力するかどうか
        /// </summary>
        public bool settingOutFile;
        /// <summary>
        /// [#SETTING]セクションのCacheDirエントリーを出力するかどうか
        /// </summary>
        public bool settingCacheDir;
        /// <summary>
        /// [#SETTING]セクションのTool1エントリーを出力するかどうか
        /// </summary>
        public bool settingTool1;
        /// <summary>
        /// [#SETTING]セクションのTool2エントリーを出力するかどうか
        /// </summary>
        public bool settingTool2;
    }

}
