/*
 * WaveUnitConfigElement.cs
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
using System;
using System.Text;
using cadencii.vsq;

namespace cadencii
{

    /// <summary>
    /// WaveUnitの設定項目1個分を表現する
    /// </summary>
    public class WaveUnitConfigElement
    {
        /// <summary>
        /// キーと値を区切るのに使用する文字列
        /// </summary>
        public const string SEPARATOR = ":";

        /// <summary>
        /// 設定項目のキー
        /// </summary>
        protected string key;

        /// <summary>
        /// 設定項目の値
        /// </summary>
        protected string value;

        /// <summary>
        /// 設定項目のキーを取得する
        /// </summary>
        /// <returns>設定項目のキー</returns>
        public string getKey()
        {
            if (this.key == null) {
                return "";
            } else {
                return this.key;
            }
        }

        /// <summary>
        /// 設定項目のキーを設定する
        /// </summary>
        /// <param name="value">設定項目のキー</param>
        public void setKey(string value)
        {
            if (value == null) {
                throw new Exception("key must not be null");
            }
            if (value.Length == 0) {
                throw new Exception("key must not be empty");
            }
            if (value.IndexOf(SEPARATOR, 0) >= 0) {
                throw new Exception("key must not contain \":\"");
            }
            this.key = value;
        }

        /// <summary>
        /// 設定項目の値を取得する
        /// </summary>
        /// <returns>設定項目の値</returns>
        public string getValue()
        {
            if (this.value == null) {
                return "";
            } else {
                return this.value;
            }
        }

        /// <summary>
        /// 設定項目の値を設定する
        /// </summary>
        /// <param name="value">設定項目の値</param>
        public void setValue(string value)
        {
            if (value == null) {
                value = "";
            }
            if (value.IndexOf(SEPARATOR, 0) >= 0) {
                throw new Exception("value must not contain \":\"");
            }
            this.value = value;
        }

        /// <summary>
        /// 設定項目のキーと値をつなげた文字列を返す
        /// </summary>
        /// <returns>"キー:値"という形式の文字列</returns>
        public string toString()
        {
            return this.key + SEPARATOR + this.value;
        }

        public string Key
        {
            get
            {
                return getKey();
            }
            set
            {
                setKey(value);
            }
        }

        public string Value
        {
            get
            {
                return getValue();
            }
            set
            {
                setValue(value);
            }
        }

        /// <summary>
        /// オーバーライドされる
        /// 設定項目のキーと値をつなげた文字列を返す
        /// </summary>
        /// <returns>"キー:値"という形式の文字列</returns>
        public override string ToString()
        {
            return this.toString();
        }
    }

}
