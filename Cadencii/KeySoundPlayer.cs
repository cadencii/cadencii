/*
 * KeySoundPlayer.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import org.kbinani.*;
import org.kbinani.media.*;
#else
using System;
using org.kbinani.media;
using bocoree;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    public class KeySoundPlayer {
        /// <summary>
        /// 鍵盤を押した時に音を鳴らすためのプレイヤー
        /// </summary>
        private static BSoundPlayer[] m_sound_previewer = new BSoundPlayer[48];
        private static BSoundPlayer m_temp_player = null;
        private static boolean[] m_prepared = new boolean[127];
        
        public static void Init() {
            String cache_path = PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "cache" );
            for ( int i = 0; i <= 126; i++ ) {
                String path = PortUtil.combinePath( cache_path, i + ".wav" );
                if ( PortUtil.isFileExists( path ) ) {
                    m_prepared[i] = true;
                    if ( 36 <= i && i <= 83 ) {
                        try {
                            m_sound_previewer[i - 36] = new BSoundPlayer( path );
                        } catch( Exception ex ) {
#if JAVA
                            System.err.println( "KeySoundPlayer#.ctor; ex=" + ex );
#endif
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
                        m_sound_previewer[note - 36].play();
                    } catch( Exception ex ) {
#if JAVA
                        System.err.println( "KeySoundPlayer#Play; ex=" + ex );
#endif
                    }
                }
            } else {
                if ( m_temp_player == null ) {
                    m_temp_player = new BSoundPlayer();
                }
                String path = PortUtil.combinePath( PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "cache" ), note + ".wav" );
                if ( PortUtil.isFileExists( path ) ) {
                    m_temp_player.setSoundLocation( path );
                    m_temp_player.play();
                }
            }
        }
    }

#if !JAVA
}
#endif
