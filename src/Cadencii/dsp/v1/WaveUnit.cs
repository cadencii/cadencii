/*
 * WaveUnit.cs
 * Copyright © 2010-2011 kbinani
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
using cadencii.java.awt;

namespace cadencii
{

    /// <summary>
    /// インターフェースWaveReceiver, WaveSender, WaveGeneratorを持つクラスの基底クラス．
    /// </summary>
    public abstract class WaveUnit
    {
        /// <summary>
        /// このユニットを画面に描くときの、基本となる描画幅。単位はピクセル
        /// この値を使うかどうかは任意
        /// </summary>
        public const int BASE_WIDTH = 150;

        /// <summary>
        /// このユニットを画面に描くときの、入出力ポート1個分の描画高さの標準値。単位はピクセル
        /// この値を使うかどうかは任意
        /// </summary>
        public const int BASE_HEIGHT_PER_PORTS = 20;

        /// <summary>
        /// エディターの設定値
        /// </summary>
        protected EditorConfig mConfig;

        /// <summary>
        /// メインウィンドウへの参照
        /// </summary>
        protected FormMain mMainWindow;

        /// <summary>
        /// 描画用のストローク
        /// </summary>
        private BasicStroke mStroke;

        /// <summary>
        /// 描画用のフォント
        /// </summary>
        private Font mFont;

        /// <summary>
        /// この波形処理ユニットが属している回路のルートにある波形合成器への参照を保持する
        /// </summary>
        protected WaveGenerator mRoot = null;

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
        public abstract void setConfig(string parameter);

        /// <summary>
        /// この波形処理ユニットが属している回路のルートにある波形合成器を設定します
        /// </summary>
        /// <param name="root"></param>
        public virtual void setRoot(WaveGenerator root)
        {
            mRoot = root;
        }

        /// <summary>
        /// この波形処理ユニットが属している回路のルートにある波形合成器を取得します
        /// </summary>
        /// <returns></returns>
        public virtual WaveGenerator getRoot()
        {
            return mRoot;
        }

        /// <summary>
        /// スコアエディタ全体の設定値を設定する．
        /// </summary>
        /// <param name="config"></param>
        public virtual void setGlobalConfig(EditorConfig config)
        {
            mConfig = config;
        }

        /// <summary>
        /// メインウィンドウへの参照を設定します
        /// </summary>
        public virtual void setMainWindow(FormMain main_window)
        {
            mMainWindow = main_window;
        }

        /// <summary>
        /// このユニットを指定した位置に描画します。
        /// </summary>
        /// <param name="graphics">描画に使用するグラフィクス</param>
        /// <param name="x">描画する位置のx座標</param>
        /// <param name="y">描画する位置のy座標</param>
        /// <returns>描画された装置図に外接する四角形のサイズ</returns>
        public virtual void paintTo(Graphics2D graphics, int x, int y, int width, int height)
        {
            // 現在の描画時のストローク、色を保存しておく
            Stroke old_stroke = graphics.getStroke();
            Color old_color = graphics.getColor();

            // 描画用のストロークが初期化してなかったら初期化
            if (mStroke == null) {
                mStroke = new BasicStroke();
            }

            if (mFont == null) {
                mFont = new Font("Arial", Font.PLAIN, 10);
            }

            // 枠と背景を描画
            paintBackground(graphics, mStroke, x, y, width, height, Color.black, PortUtil.Pink);

            // デバイス名を書く
            string typename = this.GetType().Name;
            PortUtil.drawStringEx(
                (Graphics)graphics, typename, mFont,
                new Rectangle(x, y, width, height),
                PortUtil.STRING_ALIGN_CENTER, PortUtil.STRING_ALIGN_CENTER);

            // 描画時のストローク、色を元に戻す
            graphics.setStroke(old_stroke);
            graphics.setColor(old_color);
        }

        protected void paintBackground(Graphics2D graphics, Stroke stroke, int x, int y, int width, int height, Color border, Color background)
        {
            // 背景を塗りつぶし
            graphics.setColor(background);
            graphics.fillRect(x, y, width, height);

            // 枠線を描く
            graphics.setStroke(stroke);
            graphics.setColor(border);
            graphics.drawRect(x, y, width, height);
        }
    }

}
