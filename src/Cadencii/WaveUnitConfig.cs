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
package cadencii;

import java.util.*;
import cadencii.vsq.*;
#else

using System.Collections.Generic;
using System.Text;
using cadencii.vsq;

namespace cadencii
{
#endif

    /// <summary>
    /// WaveUnitの設定と，他のWaveUnitとの接続関係の情報を保持する
    /// </summary>
    public class WaveUnitConfig
    {
        public const string SEPARATOR = "\n";

        /// <summary>
        /// WaveUnitの設定値のキーと値の組のリストを保持する
        /// </summary>
#if JAVA
        public Vector<WaveUnitConfigElement> Elements;
#else
        public List<WaveUnitConfigElement> Elements;
#endif

        public WaveUnitConfig()
        {
#if JAVA
            this.Elements = new Vector<WaveUnitConfigElement>();
#else
            this.Elements = new List<WaveUnitConfigElement>();
#endif
        }

        public string getConfigString()
        {
            StringBuilder sb = new StringBuilder();
            foreach( WaveUnitConfigElement item in this.Elements ) {
#if JAVA
                sb.append( SEPARATOR );
                sb.append( item.toString() );
#else
                sb.Append( SEPARATOR );
                sb.Append( item.toString() );
#endif
            }
            return sb.ToString();
        }

        /// <summary>
        /// 設定値のキーと値の組を追加する
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        public void putElement( string key, string value )
#if JAVA
            throws Exception
#endif
        {
            foreach( WaveUnitConfigElement item in this.Elements ) {
                if( str.compare( key, item.getKey() ) ) {
                    item.setValue( value );
                    return;
                }
            }
            WaveUnitConfigElement newItem = new WaveUnitConfigElement();
            newItem.setKey( key );
            newItem.setValue( value );
            vec.add( this.Elements, newItem );
        }

        /// <summary>
        /// 指定したキーに対応する値を取得する
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>値．存在しないキーを指定した場合空文字を返す</returns>
        public string getElement( string key )
        {
            foreach( WaveUnitConfigElement item in this.Elements ) {
                if( str.compare( key, item.getKey() ) ) {
                    return item.getValue();
                }
            }
            return "";
        }
    }

#if !JAVA
}
#endif
