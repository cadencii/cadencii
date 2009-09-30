/*
 * EditMode.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.cadencii;

/// <summary>
/// ピアノロール画面の編集モード
/// </summary>
public enum EditMode {
    /// <summary>
    /// 何も編集して無い状態
    /// </summary>
    None,
    /// <summary>
    /// 真ん中ボタンでドラッグ中
    /// </summary>
    MiddleDrag,
    /// <summary>
    /// エントリを追加中
    /// </summary>
    AddEntry,
    /// <summary>
    /// エントリを移動中
    /// </summary>
    MoveEntry,
    /// <summary>
    /// エントリ移動に向け、マウスが動くのを待機中
    /// </summary>
    MoveEntryWaitMove,
    /// <summary>
    /// エントリの左端(開始時刻)を編集中
    /// </summary>
    EditLeftEdge,
    /// <summary>
    /// エントリの右端(終了時刻)を編集中
    /// </summary>
    EditRightEdge,
    /// <summary>
    /// 固定長音符を追加
    /// </summary>
    AddFixedLengthEntry,
    /// <summary>
    /// ビブラートの有効範囲を編集中
    /// </summary>
    EditVibratoDelay,
    /// <summary>
    /// リアルタイム音符入力
    /// </summary>
    Realtime,
}
