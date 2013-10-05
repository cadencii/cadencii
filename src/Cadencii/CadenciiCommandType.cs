/*
 * CadenciiCommandType.cs
 * Copyright © 2008-2011 kbinani
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
    /// VsqFileExクラスのための編集コマンドの種類を表す列挙型
    /// </summary>
    public enum CadenciiCommandType
    {
        /// <summary>
        /// org.kbinani.vsqネイティブの編集コマンド
        /// </summary>
        VSQ_COMMAND,
        /// <summary>
        /// ベジエ曲線の追加
        /// </summary>
        BEZIER_CHAIN_ADD,
        /// <summary>
        /// ベジエ曲線の削除
        /// </summary>
        BEZIER_CHAIN_DELETE,
        /// <summary>
        /// ベジエ曲線の置換
        /// </summary>
        BEZIER_CHAIN_REPLACE,
        /// <summary>
        /// VsqFileEx全体の置換
        /// </summary>
        REPLACE,
        /// <summary>
        /// ベジエ曲線の一括置換
        /// </summary>
        ATTACHED_CURVE_REPLACE_RANGE,
        /// <summary>
        /// トラックの追加
        /// </summary>
        TRACK_ADD,
        /// <summary>
        /// トラックの削除
        /// </summary>
        TRACK_DELETE,
        /// <summary>
        /// トラックの置換
        /// </summary>
        TRACK_REPLACE,
        /// <summary>
        /// BGMの編集
        /// </summary>
        BGM_UPDATE,
        /// <summary>
        /// シーケンスの設定を
        /// </summary>
        CHANGE_SEQUENCE_CONFIG,
    }

}
