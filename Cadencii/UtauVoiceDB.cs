/*
 * UtauVoiceDB.cs
 * Copyright (c) 2009 kbinani
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
#if JAVA
package org.kbinani.Cadencii;

import java.util.*;
#else
using System;
using bocoree;
using bocoree.java.util;
using bocoree.java.io;

namespace Boare.Cadencii {
#endif

    public class UtauVoiceDB {
        private Vector<OtoArgs> m_configs = new Vector<OtoArgs>();
        private String m_name = "Unknown";

        public UtauVoiceDB( String oto_ini ) {
            if ( !PortUtil.isFileExists( oto_ini ) ) {
                return;
            }

            // oto.ini読込み
            BufferedReader sr = null;
            try {
                sr = new BufferedReader( new InputStreamReader( new FileInputStream( oto_ini ), "Shift_JIS" ) );
                String line;
                while ( sr.ready() ) {
                    try {
                        line = sr.readLine();
                        String[] spl = line.Split( '=' );
                        String file_name = spl[0]; // あ.wav
                        String a2 = spl[1]; // ,0,36,64,0,0
                        String a1 = PortUtil.getFileNameWithoutExtension( file_name );
                        spl = a2.Split( ',' );
                        OtoArgs oa = new OtoArgs();
                        oa.fileName = file_name;
                        oa.Alias = spl[0];
                        oa.msOffset = PortUtil.parseInt( spl[1] );
                        oa.msConsonant = PortUtil.parseInt( spl[2] );
                        oa.msBlank = PortUtil.parseInt( spl[3] );
                        oa.msPreUtterance = PortUtil.parseInt( spl[4] );
                        oa.msOverlap = PortUtil.parseInt( spl[5] );
                        m_configs.add( oa );
                    } catch ( Exception ex3 ) {
                    }
                }
            } catch ( Exception ex ) {
            } finally {
                if ( sr != null ) {
                    try {
                        sr.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }

            // character.txt読込み
            String character = PortUtil.combinePath( PortUtil.getDirectoryName( oto_ini ), "character.txt" );
            if ( PortUtil.isFileExists( character ) ) {
                BufferedReader sr2 = null;
                try {
                    sr2 = new BufferedReader( new InputStreamReader( new FileInputStream( character ), "Shift_JIS" ) );
                    String line = "";
                    while ( (line = sr2.readLine()) != null ) {
                        String[] spl = line.Split( '=' );
                        if ( spl.Length > 1 ) {
                            if ( spl[0].ToLower() == "name" ) {
                                m_name = spl[1];
                            }
                        }
                    }
                } catch ( Exception ex ) {
                } finally {
                    if ( sr2 != null ) {
                        try {
                            sr2.close();
                        } catch ( Exception ex2 ) {
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 指定した歌詞に合致する、エイリアスを考慮した原音設定を取得します
        /// </summary>
        /// <param name="lyric"></param>
        /// <returns></returns>
        public OtoArgs attachFileNameFromLyric( String lyric ) {
            int count = m_configs.size();
            for ( Iterator itr = m_configs.iterator(); itr.hasNext(); ) {
                OtoArgs item = (OtoArgs)itr.next();
                if ( PortUtil.getFileNameWithoutExtension( item.fileName ).Equals( lyric ) ) {
                    return item;
                }
                if ( item.Alias.Equals( lyric ) ) {
                    return item;
                }
            }
            return new OtoArgs();
        }

        public String getName() {
            return m_name;
        }
    }

#if !JAVA
}
#endif
