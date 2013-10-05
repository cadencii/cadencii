/*
 * KeySoundPlayer.cs
 * Copyright © 2008-2011 kbinani
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
using cadencii.media;
using cadencii;



namespace cadencii
{

    public class KeySoundPlayer
    {
        /// <summary>
        /// 鍵盤を押した時に音を鳴らすためのプレイヤー
        /// </summary>
        private static BSoundPlayer[] m_sound_previewer;
        private static BSoundPlayer m_temp_player;
        private static bool[] m_prepared;

        public static void init()
        {
#if DEBUG
            sout.println("KeySoundPlayer#init");
#endif
            m_sound_previewer = new BSoundPlayer[48];
            m_temp_player = null;
            m_prepared = new bool[127];
            string cache_path = Utility.getKeySoundPath();
            for (int i = 0; i <= 126; i++) {
                string path = Path.Combine(cache_path, i + ".wav");
                if (File.Exists(path)) {
                    m_prepared[i] = true;
                    if (36 <= i && i <= 83) {
                        try {
                            m_sound_previewer[i - 36] = new BSoundPlayer(path);
                        } catch (Exception ex) {
                            serr.println("KeySoundPlayer#init; ex=" + ex);
                        }
                    }
                } else {
                    m_prepared[i] = false;
                }
            }
        }

        public static void play(int note)
        {
            if (note < 0 || 127 <= note) {
                return;
            }
            if (!m_prepared[note]) {
                return;
            }
            if (36 <= note && note <= 83) {
                if (m_sound_previewer[note - 36] != null) {
                    try {
                        m_sound_previewer[note - 36].play();
                    } catch (Exception ex) {
                        serr.println("KeySoundPlayer#play; ex=" + ex);
                    }
                }
            } else {
                if (m_temp_player == null) {
                    m_temp_player = new BSoundPlayer();
                }
                string path = Path.Combine(Utility.getKeySoundPath(), note + ".wav");
                if (File.Exists(path)) {
                    m_temp_player.setSoundLocation(path);
                    m_temp_player.play();
                }
            }
        }
    }

}
