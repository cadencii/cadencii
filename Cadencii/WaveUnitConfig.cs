/*
 * WaveUnitConfig.cs
 * Copyright © 2011 kbinani
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

import org.kbinani.vsq.*;
#else

using System.Text;
using org.kbinani.vsq;

namespace org.kbinani.cadencii
{
#endif

    /// <summary>
    /// WaveUnitの設定と，他のWaveUnitとの接続関係の情報を保持する
    /// </summary>
    public class WaveUnitConfig
    {
        public const string SEPARATOR = "\n";

        public WaveUnitConfigElement[] Elements;

        public WaveUnitConfig()
        {
            this.Elements = new WaveUnitConfigElement[] { };
        }

        public string getConfigString()
        {
            StringBuilder sb = new StringBuilder();
            foreach( WaveUnitConfigElement item in this.Elements ) {
                sb.Append( SEPARATOR );
                sb.Append( item.toString() );
            }
            return sb.ToString();
        }
    }

#if !JAVA
}
#endif
