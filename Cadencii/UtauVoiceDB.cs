/*
 * UtauVoiceDB.cs
 * Copyright (C) 2009-2010 kbinani
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

import java.util.*;
import java.io.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.java.io;

namespace org.kbinani.cadencii {
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
                    line = sr.readLine();
                    String[] spl = PortUtil.splitString( line, '=' );
                    if ( spl.Length < 2 ) {
                        continue;
                    }
                    String file_name = spl[0]; // あ.wav
                    String a2 = spl[1]; // ,0,36,64,0,0
                    String a1 = PortUtil.getFileNameWithoutExtension( file_name );
                    spl = PortUtil.splitString( a2, ',' );
                    if ( spl.Length < 6 ) {
                        continue;
                    }
                    OtoArgs oa = new OtoArgs();
                    oa.fileName = file_name;
                    oa.Alias = spl[0];
                    try {
                        oa.msOffset = PortUtil.parseFloat( spl[1] );
                    } catch ( Exception ex ) {
                        oa.msOffset = 0;
                    }
                    try {
                        oa.msConsonant = PortUtil.parseFloat( spl[2] );
                    } catch ( Exception ex ) {
                        oa.msConsonant = 0;
                    }
                    try {
                        oa.msBlank = PortUtil.parseFloat( spl[3] );
                    } catch ( Exception ex ) {
                        oa.msBlank = 0;
                    }
                    try {
                        oa.msPreUtterance = PortUtil.parseFloat( spl[4] );
                    } catch ( Exception ex ) {
                        oa.msPreUtterance = 0;
                    }
                    try {
                        oa.msOverlap = PortUtil.parseFloat( spl[5] );
                    } catch ( Exception ex ) {
                        oa.msOverlap = 0;
                    }
                    m_configs.add( oa );
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "UtauVoiceDB#.ctor; ex=" + ex );
            } finally {
                if ( sr != null ) {
                    try {
                        sr.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "UtauVoiceDB#.ctor; ex2=" + ex2 );
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
                        String[] spl = PortUtil.splitString(  line, '=' );
                        if ( spl.Length > 1 ) {
                            if ( spl[0].ToLower().Equals( "name" ) ) {
                                m_name = spl[1];
                            }
                        }
                    }
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "UtauVoiceDB#.ctor; ex=" + ex );
                } finally {
                    if ( sr2 != null ) {
                        try {
                            sr2.close();
                        } catch ( Exception ex2 ) {
                            PortUtil.stderr.println( "UtauVoiceDB#.ctor; ex2=" + ex2 );
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
