/*
 * PropertyPanelState.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import java.util.*;
import java.io.*;
import org.kbinani.*;
import org.kbinani.windows.forms.*;
import org.kbinani.xml.*;
#else
using System;
using com.github.cadencii;
using com.github.cadencii.java.util;
using com.github.cadencii.java.io;
using com.github.cadencii.windows.forms;
using com.github.cadencii.xml;

namespace com.github.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// プロパティウィンドウの状態を表すクラス
    /// </summary>
    public class PropertyPanelState {
        /// <summary>
        /// プロパティパネルの状態を表す
        /// </summary>
        public PanelState State = PanelState.Docked;
        /// <summary>
        /// プロパティウィンドウの位置と大きさ
        /// </summary>
        public XmlRectangle Bounds = new XmlRectangle( 0, 0, 200, 300 );
        /// <summary>
        /// プロパティの表示項目の展開・縮小状態を格納したリスト
        /// </summary>
#if JAVA
        @XmlGenericType( ValuePairOfStringBoolean.class )
#endif
        public Vector<ValuePairOfStringBoolean> ExpandStatus = new Vector<ValuePairOfStringBoolean>();
        /// <summary>
        /// 音階の表現形式
        /// </summary>
        public NoteNumberExpressionType LastUsedNoteNumberExpression = NoteNumberExpressionType.International;
        /// <summary>
        /// プロパティパネルがウィンドウに分離された状態における，ウィンドウの表示状態
        /// </summary>
        public BFormWindowState WindowState = BFormWindowState.Normal;
        /// <summary>
        /// プロパティパネルがドッキングされた状態における表示幅(ピクセル)
        /// </summary>
        public int DockWidth = 240;
    }

#if !JAVA
}
#endif
