#if ENABLE_AQUESTONE
/*
 * AquesToneDriverBase.cs
 * Copyright © 2009-2013 kbinani
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

import cadencii.*;
import cadencii.vsq.*;

#else

using System;
using System.Text;
using cadencii;
using cadencii.java.io;
using cadencii.vsq;

namespace cadencii
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public abstract class AquesToneDriverBase {
#else
    public abstract class AquesToneDriverBase : VSTiDriverBase
    {
#endif

#if ENABLE_AQUESTONE

        /// <summary>
        /// AquesTone VSTi の DLL パスを指定して初期化する
        /// </summary>
        /// <param name="dllPath">AquesTone VSTi の DLL パス</param>
        public AquesToneDriverBase( String dllPath )
        {
            path = dllPath;
            loaded = open( 44100, 44100 );
        }

        /// <summary>
        /// win.ini にて使用されるセクション名を取得する
        /// </summary>
        /// <returns>セクション名の文字列。両端の"[", "]"は含めない</returns>
        protected abstract String getConfigSectionKey();

        /// <summary>
        /// win.ini にて使用される、Koe ファイルを指定するキー名
        /// </summary>
        /// <returns>キー名の文字列</returns>
        protected abstract String getKoeConfigKey();

        /// <summary>
        /// Koe ファイルに書き込むファイルの中身を取得する
        /// </summary>
        /// <returns></returns>
        protected abstract String[] getKoeFileContents();

        public override boolean open( int block_size, int sample_rate )
        {
            int strlen = 260;
            StringBuilder sb = new StringBuilder( strlen );
            win32.GetProfileString( getConfigSectionKey(), getKoeConfigKey(), "", sb, (uint)strlen );
            String koe_old = sb.ToString();

            String required = prepareKoeFile();
            boolean refresh_winini = false;
            if ( !required.Equals( koe_old ) && !koe_old.Equals( "" ) ) {
                refresh_winini = true;
            }
            win32.WriteProfileString( getConfigSectionKey(), getKoeConfigKey(), required );
            boolean ret = false;
            try {
                ret = base.open( block_size, sample_rate );
            } catch ( Exception ex ) {
                ret = false;
                reportError( GetType() + ".open; ex=" + ex );
            }

            if ( refresh_winini ) {
                win32.WriteProfileString( getConfigSectionKey(), getKoeConfigKey(), koe_old );
            }
            return ret;
        }

        private String prepareKoeFile()
        {
            String ret = PortUtil.createTempFile();
            BufferedWriter bw = null;
            try {
                bw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( ret ), "Shift_JIS" ) );
                foreach ( String s in getKoeFileContents() ) {
                    bw.write( s ); bw.newLine();
                }
            } catch ( Exception ex ) {
                reportError( GetType() + ".getKoeFilePath; ex=" + ex );
            } finally {
                if ( bw != null ) {
                    try {
                        bw.close();
                    } catch ( Exception ex2 ) {
                        reportError( GetType() + ".getKoeFilePath; ex=" + ex2 );
                    }
                }
            }
            return ret;
        }

        protected void reportError( String s )
        {
            Logger.writeLine( s );
            serr.println( s );
        }

#endif
    }

#if !JAVA
}
#endif
#endif
