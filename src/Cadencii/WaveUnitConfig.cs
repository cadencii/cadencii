/*
 * WaveUnitConfig.cs
 * Copyright © 2011 kbinani
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
using System.Collections.Generic;
using System.Text;
using cadencii.vsq;

namespace cadencii
{

    /// <summary>
    /// WaveUnitの設定と，他のWaveUnitとの接続関係の情報を保持する
    /// </summary>
    public class WaveUnitConfig
    {
        public const string SEPARATOR = "\n";

        /// <summary>
        /// WaveUnitの設定値のキーと値の組のリストを保持する
        /// </summary>
        public List<WaveUnitConfigElement> Elements;

        public WaveUnitConfig()
        {
            this.Elements = new List<WaveUnitConfigElement>();
        }

        public string getConfigString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (WaveUnitConfigElement item in this.Elements) {
                sb.Append(SEPARATOR);
                sb.Append(item.toString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 設定値のキーと値の組を追加する
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        public void putElement(string key, string value)
        {
            foreach (WaveUnitConfigElement item in this.Elements) {
                if (key == item.getKey()) {
                    item.setValue(value);
                    return;
                }
            }
            WaveUnitConfigElement newItem = new WaveUnitConfigElement();
            newItem.setKey(key);
            newItem.setValue(value);
            this.Elements.Add(newItem);
        }

        /// <summary>
        /// 指定したキーに対応する値を取得する
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>値．存在しないキーを指定した場合空文字を返す</returns>
        public string getElement(string key)
        {
            foreach (WaveUnitConfigElement item in this.Elements) {
                if (key == item.getKey()) {
                    return item.getValue();
                }
            }
            return "";
        }
    }

}
