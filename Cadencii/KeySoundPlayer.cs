/*
 * KeySoundPlayer.cs
 * Copyright © 2008-2011 kbinani
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
using org.kbinani;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    public class KeySoundPlayer {
        /// <summary>
        /// 鍵盤を押した時に音を鳴らすためのプレイヤー
        /// </summary>
        private static BSoundPlayer[] m_sound_previewer;
        private static BSoundPlayer m_temp_player;
        private static boolean[] m_prepared;
        
        public static void init() {
#if DEBUG
            PortUtil.println( "KeySoundPlayer#init" );
#endif
            m_sound_previewer = new BSoundPlayer[48];
            m_temp_player = null;
            m_prepared = new boolean[127];
            String cache_path = Utility.getKeySoundPath();
            for ( int i = 0; i <= 126; i++ ) {
                String path = fsys.combine( cache_path, i + ".wav" );
                if ( PortUtil.isFileExists( path ) ) {
                    m_prepared[i] = true;
                    if ( 36 <= i && i <= 83 ) {
                        try {
                            m_sound_previewer[i - 36] = new BSoundPlayer( path );
                        } catch( Exception ex ) {
                            PortUtil.stderr.println( "KeySoundPlayer#init; ex=" + ex );
                        }
                    }
                } else {
                    m_prepared[i] = false;
                }
            }
        }

        public static void play( int note ) {
            if ( note < 0 || 127 <= note ) {
                return;
            }
            if ( !m_prepared[note] ) {
                return;
            }
            if ( 36 <= note && note <= 83 ) {
                if ( m_sound_previewer[note - 36] != null ) {
                    try {
                        m_sound_previewer[note - 36].play();
                    } catch( Exception ex ) {
                        PortUtil.stderr.println( "KeySoundPlayer#play; ex=" + ex );
                    }
                }
            } else {
                if ( m_temp_player == null ) {
                    m_temp_player = new BSoundPlayer();
                }
                String path = fsys.combine( Utility.getKeySoundPath(), note + ".wav" );
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
