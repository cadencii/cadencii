/*
 * NoteNumberExpressionType.cs
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
namespace cadencii
{

    /// <summary>
    /// 音程を表現するときの表現形式を表す列挙型
    /// </summary>
    public enum NoteNumberExpressionType
    {
        /// <summary>
        /// 数値で表現(ex. 61)
        /// </summary>
        Numeric,
        /// <summary>
        /// 一般的な表現(ex. C#3)
        /// </summary>
        International,
        /// <summary>
        /// 日本語表記(ex. 嬰ハ3)
        /// </summary>
        Japanese,
        /// <summary>
        /// 日本語の固定ドレミ表記(ex. ド#3)
        /// </summary>
        JapaneseFixedDo,
        /// <summary>
        /// ドイツ語表記(ex. Cis3)
        /// </summary>
        Deutsche,
    }

}
