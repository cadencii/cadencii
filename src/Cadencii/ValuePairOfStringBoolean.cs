/*
 * ValuePairOfStringBoolean.cs
 * Copyright © 2010-2011 kbinani
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

namespace cadencii
{

    /// <summary>
    /// ValuePair&lt;String,Boolean&gt;をXMLシリアライズするためのクラス
    /// </summary>
    public class ValuePairOfStringBoolean
    {
        private string _key;
        private Boolean _value;

        /// <summary>
        /// デフォルトのコンストラクタ．
        /// Key="", Value=falseで初期化されます．
        /// </summary>
        public ValuePairOfStringBoolean()
            : this("", false)
        {
        }

        /// <summary>
        /// 初期値を指定したコンストラクタ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public ValuePairOfStringBoolean(string key, Boolean value)
        {
            _key = key;
            _value = value;
        }

        /// <summary>
        /// キー値を取得します
        /// </summary>
        /// <returns></returns>
        public string getKey()
        {
            return _key;
        }

        /// <summary>
        /// キー値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setKey(string value)
        {
            _key = value;
        }

        /// <summary>
        /// 値を取得します
        /// </summary>
        /// <returns></returns>
        public Boolean getValue()
        {
            return _value;
        }

        /// <summary>
        /// 値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setValue(Boolean value)
        {
            _value = value;
        }
    }

}
