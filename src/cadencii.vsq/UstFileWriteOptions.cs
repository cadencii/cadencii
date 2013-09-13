/*
 * UstFileWriteOptions.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii.vsq;

#else
using System;

namespace cadencii.vsq
{
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// UstFileクラスのwriteメソッドで出力する際の詳細オプションを表します
    /// </summary>
    public class UstFileWriteOptions
    {
        /// <summary>
        /// [#TRACKEND]セクションを出力するかどうか
        /// </summary>
        public boolean trackEnd;
        /// <summary>
        /// [#SETTING]セクションのTempoエントリーを出力するかどうか
        /// </summary>
        public boolean settingTempo;
        /// <summary>
        /// [#SETTING]セクションのTracksエントリーを出力するかどうか
        /// </summary>
        public boolean settingTracks;
        /// <summary>
        /// [#SETTING]セクションのProjectNameエントリーを出力するかどうか
        /// </summary>
        public boolean settingProjectName;
        /// <summary>
        /// [#SETTING]セクションのVoiceDirエントリーを出力するかどうか
        /// </summary>
        public boolean settingVoiceDir;
        /// <summary>
        /// [#SETTING]セクションのOutFileエントリーを出力するかどうか
        /// </summary>
        public boolean settingOutFile;
        /// <summary>
        /// [#SETTING]セクションのCacheDirエントリーを出力するかどうか
        /// </summary>
        public boolean settingCacheDir;
        /// <summary>
        /// [#SETTING]セクションのTool1エントリーを出力するかどうか
        /// </summary>
        public boolean settingTool1;
        /// <summary>
        /// [#SETTING]セクションのTool2エントリーを出力するかどうか
        /// </summary>
        public boolean settingTool2;
    }

#if !JAVA
}
#endif
