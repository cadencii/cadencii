/*
 * PropertyPanelState.cs
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
using System;
using System.Collections.Generic;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;
using cadencii.windows.forms;
using cadencii.xml;

namespace cadencii
{

    /// <summary>
    /// プロパティウィンドウの状態を表すクラス
    /// </summary>
    public class PropertyPanelState
    {
        /// <summary>
        /// プロパティパネルの状態を表す
        /// </summary>
        public PanelState State = PanelState.Docked;
        /// <summary>
        /// プロパティウィンドウの位置と大きさ
        /// </summary>
        public XmlRectangle Bounds = new XmlRectangle(0, 0, 200, 300);
        /// <summary>
        /// プロパティの表示項目の展開・縮小状態を格納したリスト
        /// </summary>
        public List<ValuePairOfStringBoolean> ExpandStatus = new List<ValuePairOfStringBoolean>();
        /// <summary>
        /// 音階の表現形式
        /// </summary>
        public NoteNumberExpressionType LastUsedNoteNumberExpression = NoteNumberExpressionType.International;
        /// <summary>
        /// プロパティパネルがウィンドウに分離された状態における，ウィンドウの表示状態
        /// </summary>
        public System.Windows.Forms.FormWindowState WindowState = System.Windows.Forms.FormWindowState.Normal;
        /// <summary>
        /// プロパティパネルがドッキングされた状態における表示幅(ピクセル)
        /// </summary>
        public int DockWidth = 240;
    }

}
