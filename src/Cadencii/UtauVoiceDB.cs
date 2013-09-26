/*
 * UtauVoiceDB.cs
 * Copyright © 2009-2011 kbinani
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
#if JAVA
package cadencii;

import java.util.*;
import java.io.*;
import cadencii.*;
import cadencii.vsq.*;
#else
using System;
using System.IO;
using System.Collections.Generic;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;
using cadencii.vsq;

namespace cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// UTAUの原音設定を表すクラス
    /// </summary>
    public class UtauVoiceDB {
        private List<OtoArgs> _configs = new List<OtoArgs>();
        private String _name = "Unknown";

        /// <summary>
        /// コンストラクタ．
        /// </summary>
        /// <param name="singer_config"></param>
        public UtauVoiceDB( SingerConfig singer_config ) {
            _name = singer_config.VOICENAME;
            String oto_ini = Path.Combine( singer_config.VOICEIDSTR, "oto.ini" );
            readOtoIni( oto_ini );
        }

        /// <summary>
        /// 原音設定ファイルを読み込みます．
        /// </summary>
        /// <param name="oto_ini">原音設定のパス</param>
        private void readOtoIni( String oto_ini ) {
            if (!System.IO.File.Exists(oto_ini)) {
                return;
            }

            // oto.ini読込み
            String dir = PortUtil.getDirectoryName( oto_ini );
            foreach ( String encoding in AppManager.TEXT_ENCODINGS_IN_UTAU ) {
                BufferedReader sr = null;
                try {
                    sr = new BufferedReader( new InputStreamReader( new FileInputStream( oto_ini ), encoding ) );
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

                        // ファイルがちゃんとあるかどうか？
                        String fullpath = Path.Combine( dir, file_name );
                        if (!System.IO.File.Exists(fullpath)) {
                            continue;
                        }

                        OtoArgs oa = new OtoArgs();
                        oa.fileName = file_name;
                        oa.Alias = spl[0];
                        try {
                            oa.msOffset = (float)double.Parse( spl[1] );
                        } catch ( Exception ex ) {
                            oa.msOffset = 0;
                        }
                        try {
                            oa.msConsonant = (float)double.Parse( spl[2] );
                        } catch ( Exception ex ) {
                            oa.msConsonant = 0;
                        }
                        try {
                            oa.msBlank = (float)double.Parse( spl[3] );
                        } catch ( Exception ex ) {
                            oa.msBlank = 0;
                        }
                        try {
                            oa.msPreUtterance = (float)double.Parse( spl[4] );
                        } catch ( Exception ex ) {
                            oa.msPreUtterance = 0;
                        }
                        try {
                            oa.msOverlap = (float)double.Parse( spl[5] );
                        } catch ( Exception ex ) {
                            oa.msOverlap = 0;
                        }

                        // 重複登録が無いかチェック
                        boolean found = false;
                        foreach ( OtoArgs o in _configs ) {
#if JAVA
                            if ( o == null ) {
                                continue;
                            }
#endif
                            if ( o.equals( oa ) ) {
                                found = true;
                                break;
                            }
                        }
                        if ( !found ) {
                            _configs.Add( oa );
                        }
                    }
                } catch ( Exception ex ) {
                    //serr.println( "UtauVoiceDB#.ctor; ex=" + ex );
                } finally {
                    if ( sr != null ) {
                        try {
                            sr.close();
                        } catch ( Exception ex2 ) {
                            serr.println( "UtauVoiceDB#.ctor; ex2=" + ex2 );
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
            int count = _configs.Count;
            foreach (var item in _configs) {
                if ( PortUtil.getFileNameWithoutExtension( item.fileName ) == lyric ) {
                    return item;
                }
                if ( item.Alias == lyric ) {
                    return item;
                }
            }
            return new OtoArgs();
        }

        /// <summary>
        /// この原音の名称を取得します．
        /// </summary>
        /// <returns>この原音の名称</returns>
        public String getName() {
            return _name;
        }
    }

#if !JAVA
}
#endif
