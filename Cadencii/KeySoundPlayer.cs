/*
 * KeySoundPlayer.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Media;
using System.Windows.Forms;

using Boare.Lib.Media;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    class KeySoundPlayer {
        /// <summary>
        /// 鍵盤を押した時に音を鳴らすためのプレイヤー
        /// </summary>
        private static SoundPlayer[] m_sound_previewer = new SoundPlayer[48];
        private static SoundPlayer m_temp_player = null;
        private static boolean[] m_prepared = new boolean[127];
        
        public static void Init() {
            String cache_path = Path.Combine( Application.StartupPath, "cache" );
            for ( int i = 0; i <= 126; i++ ) {
                String path = Path.Combine( cache_path, i + ".wav" );
                if ( File.Exists( path ) ) {
                    m_prepared[i] = true;
                    if ( 36 <= i && i <= 83 ) {
                        try {
                            m_sound_previewer[i - 36] = new SoundPlayer( path );
                        } catch {
                        }
                    }
                } else {
                    m_prepared[i] = false;
                }
            }
        }

        public static void Play( int note ) {
            if ( note < 0 || 127 <= note ) {
                return;
            }
#if DEBUG
            AppManager.debugWriteLine( "KeySoundPlayer+Play" );
            AppManager.debugWriteLine( "    note=" + note );
            AppManager.debugWriteLine( "    m_prepared[note]=" + m_prepared[note] );
#endif
            if ( !m_prepared[note] ) {
                return;
            }
            if ( 36 <= note && note <= 83 ) {
                if ( m_sound_previewer[note - 36] != null ) {
                    try {
                        m_sound_previewer[note - 36].Play();
                    } catch {
                    }
                }
            } else {
                if ( m_temp_player == null ) {
                    m_temp_player = new SoundPlayer();
                }
                String path = Path.Combine( Path.Combine( Application.StartupPath, "cache" ), note + ".wav" );
                if ( File.Exists( path ) ) {
                    m_temp_player.SoundLocation = path;
                    m_temp_player.Play();
                }
            }
        }
    }

}
