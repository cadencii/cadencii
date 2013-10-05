/*
 * VersionString.cs
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

namespace cadencii
{

    /// <summary>
    /// メジャー，マイナー，およびメンテナンス番号によるバージョン番号を表すクラス
    /// </summary>
    class VersionString : IComparable<VersionString>
    {
        /// <summary>
        /// メジャーバージョンを表す
        /// </summary>
        public int major;
        /// <summary>
        /// マイナーバージョンを表す
        /// </summary>
        public int minor;
        /// <summary>
        /// メンテナンス番号を表す
        /// </summary>
        public int build;
        /// <summary>
        /// コンストラクタに渡された文字列のキャッシュ
        /// </summary>
        private string mRawString = "0.0.0";

        /// <summary>
        /// 「メジャー.マイナー.メンテナンス」の記法に基づく文字列をパースし，新しいインスタンスを作成します
        /// </summary>
        /// <param name="str"></param>
        public VersionString(string s)
        {
            mRawString = s;
            string[] spl = PortUtil.splitString(s, '.');
            if (spl.Length >= 1) {
                try {
                    major = int.Parse(spl[0]);
                } catch (Exception ex) {
                }
            }
            if (spl.Length >= 2) {
                try {
                    minor = int.Parse(spl[1]);
                } catch (Exception ex) {
                }
            }
            if (spl.Length >= 3) {
                try {
                    build = int.Parse(spl[2]);
                } catch (Exception ex) {
                }
            }
        }

        /// <summary>
        /// このインスタンス生成時に渡された文字列を取得します
        /// </summary>
        /// <returns></returns>
        public string getRawString()
        {
            return mRawString;
        }

        /// <summary>
        /// このインスタンスを文字列で表現したものを取得します
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            return major + "." + minor + "." + build;
        }

        /// <summary>
        /// このインスタンスと，指定したバージョンを比較します
        /// </summary>
        /// <param name="item"></param>
        /// <returns>このインスタンスの表すバージョンに対して，指定したバージョンが同じであれば0，新しければ正の値，それ以外は負の値を返します</returns>
        public int compareTo(VersionString item)
        {
            if (item == null) {
                return -1;
            }
            if (this.major == item.major) {
                if (this.minor == item.minor) {
                    return this.build - item.build;
                } else {
                    return this.minor - item.minor;
                }
            } else {
                return this.major - item.major;
            }
        }

        public override string ToString()
        {
            return this.toString();
        }

        public int CompareTo(VersionString item)
        {
            return this.compareTo(item);
        }
    }

}
