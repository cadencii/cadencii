/*
 * IconHandle.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace cadencii.vsq
{
    /// <summary>
    /// 歌手設定を表します。
    /// </summary>
    [Serializable]
    public class IconHandle : ICloneable
    {
        /// <summary>
        /// キャプション。
        /// </summary>
        public string Caption;
        /// <summary>
        /// この歌手設定を一意に識別するためのIDです。
        /// </summary>
        public string IconID;
        /// <summary>
        /// ユーザ・フレンドリー名。
        /// このフィールドの値は、他の歌手設定のユーザ・フレンドリー名と重複する場合があります。
        /// </summary>
        public string IDS;
        public int Index;
        /// <summary>
        /// ゲートタイム長さ。
        /// </summary>
        public int Length;
        public int Original;
        public int Program;
        public int Language;

        /// <summary>
        /// 新しい歌手設定のインスタンスを初期化します。
        /// </summary>
        public IconHandle()
        {
            Caption = "";
            IconID = "";
            IDS = "";
        }

        /// <summary>
        /// ゲートタイム長さを取得します。
        /// </summary>
        /// <returns></returns>
        public int getLength()
        {
            return Length;
        }

        /// <summary>
        /// ゲートタイム長さを設定します。
        /// </summary>
        /// <param name="value"></param>
        public void setLength(int value)
        {
            Length = value;
        }

        /// <summary>
        /// このインスタンスと、指定された歌手変更のインスタンスが等しいかどうかを判定します。
        /// </summary>
        /// <param name="item">比較対象の歌手変更。</param>
        /// <returns>このインスタンスと、比較対象の歌手変更が等しければtrue、そうでなければfalseを返します。</returns>
        public bool equals(IconHandle item)
        {
            if (item == null) {
                return false;
            } else {
                return IconID.Equals(item.IconID);
            }
        }

        /// <summary>
        /// このインスタンスのコピーを作成します。
        /// </summary>
        /// <returns></returns>
        public Object clone()
        {
            IconHandle ret = new IconHandle();
            ret.Caption = Caption;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Index = Index;
            ret.Language = Language;
            ret.setLength(Length);
            ret.Original = Original;
            ret.Program = Program;
            return ret;
        }

        /// <summary>
        /// このインスタンスのコピーを作成します。
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return clone();
        }

    }

}
