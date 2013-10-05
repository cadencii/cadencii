/*
 * EditMode.cs
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
using System;

namespace cadencii
{

    /// <summary>
    /// ピアノロール画面の編集モード
    /// </summary>
    public enum EditMode
    {
        /// <summary>
        /// 何も編集して無い状態
        /// </summary>
        NONE,
        /// <summary>
        /// 真ん中ボタンでドラッグ中
        /// </summary>
        MIDDLE_DRAG,
        /// <summary>
        /// エントリを追加中
        /// </summary>
        ADD_ENTRY,
        /// <summary>
        /// エントリを移動中
        /// </summary>
        MOVE_ENTRY,
        /// <summary>
        /// エントリ移動に向け、マウスが動くのを待機中
        /// </summary>
        MOVE_ENTRY_WAIT_MOVE,
        /// <summary>
        /// コントロールカーブも同時移動するモードで、エントリを移動中
        /// </summary>
        MOVE_ENTRY_WHOLE,
        /// <summary>
        /// コントロールカーブも同時移動するモードで、マウスが動くのを待機中
        /// </summary>
        MOVE_ENTRY_WHOLE_WAIT_MOVE,
        /// <summary>
        /// エントリの左端(開始時刻)を編集中
        /// </summary>
        EDIT_LEFT_EDGE,
        /// <summary>
        /// エントリの右端(終了時刻)を編集中
        /// </summary>
        EDIT_RIGHT_EDGE,
        /// <summary>
        /// 固定長音符を追加
        /// </summary>
        ADD_FIXED_LENGTH_ENTRY,
        /// <summary>
        /// ビブラートの有効範囲を編集中
        /// </summary>
        EDIT_VIBRATO_DELAY,
        /// <summary>
        /// アイコンパレットのアイテムをドラッグ＆ドロップ中
        /// </summary>
        DRAG_DROP,
        /// <summary>
        /// MTCスレーブ状態で，同期再生中
        /// </summary>
        REALTIME_MTC,
        /// <summary>
        /// ピアノロール上でカーブを描くモード
        /// </summary>
        CURVE_ON_PIANOROLL,
        /// <summary>
        /// ステップ入力中
        /// </summary>
        STEP_SEQUENCER,
    }

}
