/*
 * WaveUnit.cs
 * Copyright (C) 2010 kbinani
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

import java.awt.*;
#else
using System;
using org.kbinani.java.awt;

namespace org.kbinani.cadencii.draft {
#endif

    /// <summary>
    /// インターフェースWaveReceiver, WaveSender, WaveGeneratorを持つクラスの基底クラス．
    /// </summary>
    public abstract class WaveUnit {
        /// <summary>
        /// このユニットを画面に描くときの、基本となる描画幅。単位はピクセル
        /// この値を使うかどうかは任意
        /// </summary>
        protected const int BASE_WIDTH = 150;

        /// <summary>
        /// このユニットを画面に描くときの、入出力ポート1個分の描画高さの標準値。単位はピクセル
        /// この値を使うかどうかは任意
        /// </summary>
        protected const int BASE_HEIGHT_PER_PORTS = 40;

        /// <summary>
        /// エディターの設定値
        /// </summary>
        protected EditorConfig mConfig;

        /// <summary>
        /// バージョンを表す整数を返す．
        /// 実装上，setConfigに渡す文字列の書式が変わったとき，バージョンを増やすようにする．
        /// </summary>
        /// <returns></returns>
        public abstract int getVersion();

        /// <summary>
        /// 設定を行う．parameterの1字目は，parameterを分割するのに利用する文字を付ける．
        /// 例えば，3個の整数を受け取る実装の場合，次の2つは同じ意味になる(そのように実装する)．
        ///     1)    parameter = "\n1\n2\n3"
        ///     2)    parameter = "\t1\t2\t3"
        /// </summary>
        /// <param name="parameter"></param>
        public abstract void setConfig( String parameter );

        /// <summary>
        /// このユニットを指定した位置に描画します。
        /// </summary>
        /// <param name="graphics">描画に使用するグラフィクス</param>
        /// <param name="x">描画する位置のx座標</param>
        /// <param name="y">描画する位置のy座標</param>
        /// <returns>描画された装置図に外接する四角形のサイズ</returns>
        public abstract Dimension paintTo( Graphics2D graphics, int x, int y );

        /// <summary>
        /// このユニットの入力ポートの個数を取得します
        /// </summary>
        /// <returns></returns>
        public abstract int getNumPortsIn();

        /// <summary>
        /// このユニットの出力ポートの個数を取得します
        /// </summary>
        /// <returns></returns>
        public abstract int getNuMPortsOut();

        /// <summary>
        /// スコアエディタ全体の設定値を設定する．
        /// </summary>
        /// <param name="config"></param>
        public void setGlobalConfig( EditorConfig config ) {
            mConfig = config;
        }

        /// <summary>
        /// このユニットを画面に描くときの基本となる描画サイズを取得します
        /// </summary>
        /// <returns></returns>
        protected Dimension getBaseDimension() {
            int ports_in = getNumPortsIn();
            int ports_out = getNuMPortsOut();
            int ports = (ports_in > ports_out) ? ports_in : ports_out;
            ports++;
            return new Dimension( BASE_WIDTH, ports * BASE_HEIGHT_PER_PORTS );
        }
    }

#if !JAVA
}
#endif
