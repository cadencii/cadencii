/*
 * PropertyPanelState.cs
 * Copyright (C) 2009-2010 kbinani
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
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.java.io;
using org.kbinani.windows.forms;
using org.kbinani.xml;

namespace org.kbinani.cadencii {
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
        public int DockWidth = 200;

        /// <summary>
        /// このクラスの指定した名前のプロパティが総称型引数を用いる型である場合に，
        /// その型の限定名を返します．それ以外の場合は空文字を返します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getGenericTypeName( String name ) {
            if ( name != null ) {
                if ( name.Equals( "ExpandStatus" ) ) {
                    return "org.kbinani.cadencii.ValuePairOfStringBoolean";
                }
            }
            return "";
        }
    }

#if !JAVA
}
#endif
