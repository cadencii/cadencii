/*
 * FormNotePropertyUi.cs
 * Copyright © 2012 kbinani
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

    public interface FormNotePropertyUi
    {
        /// <summary>
        /// プロパティウィンドウのトップレベルコンポーネントを追加する
        /// </summary>
        /// <param name="c">追加するコンポーネント</param>
        [PureVirtualFunction]
        void addComponent(object c);

        /// <summary>
        /// プロパティウィンドウが最小化された状態かどうかを取得する
        /// </summary>
        /// <returns></returns>
        [PureVirtualFunction]
        bool isWindowMinimized();

        /// <summary>
        /// ウィンドウのサイズを標準状態に戻す
        /// </summary>
        [PureVirtualFunction]
        void deiconfyWindow();

        /// <summary>
        /// ウィンドウのタイトル文字列を設定する
        /// </summary>
        /// <param name="title"></param>
        [PureVirtualFunction]
        void setTitle(string title);

        /// <summary>
        /// ウィンドウを常に最前面に表示するかどうかを設定する
        /// </summary>
        /// <param name="alwaysOnTop"></param>
        [PureVirtualFunction]
        void setAlwaysOnTop(bool alwaysOnTop);

        /// <summary>
        /// ウィンドウを常に最前面に表示するかどうか
        /// </summary>
        /// <returns></returns>
        [PureVirtualFunction]
        bool isAlwaysOnTop();

        /// <summary>
        /// ウィンドウを閉じる
        /// </summary>
        [PureVirtualFunction]
        void close();

        /// <summary>
        /// ウィンドウを閉じるメニューのショートカットキーを設定する
        /// </summary>
        /// <param name="value"></param>
        [PureVirtualFunction]
        void setMenuCloseAccelerator(System.Windows.Forms.Keys value);

        /// <summary>
        /// ウィンドウが可視状態かどうかを設定する
        /// </summary>
        /// <param name="visible"></param>
        [PureVirtualFunction]
        void setVisible(bool visible);

        /// <summary>
        /// ウィンドウが可視状態かどうかを取得する
        /// </summary>
        /// <returns></returns>
        [PureVirtualFunction]
        bool isVisible();

        /// <summary>
        /// ウィンドウの位置とサイズを設定する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [PureVirtualFunction]
        void setBounds(int x, int y, int width, int height);

        /// <summary>
        /// ウィンドウ位置のX座標を取得する
        /// </summary>
        /// <returns></returns>
        [PureVirtualFunction]
        int getX();

        /// <summary>
        /// ウィンドウ位置のY座標を取得する
        /// </summary>
        /// <returns></returns>
        [PureVirtualFunction]
        int getY();

        /// <summary>
        /// ウィンドウの幅を取得する
        /// </summary>
        /// <returns></returns>
        [PureVirtualFunction]
        int getWidth();

        /// <summary>
        /// ウィンドウの高を取得する
        /// </summary>
        /// <returns></returns>
        [PureVirtualFunction]
        int getHeight();

        /// <summary>
        /// このウィンドウが含まれるスクリーンの位置のX座標を取得する
        /// </summary>
        /// <returns></returns>
        [PureVirtualFunction]
        int getWorkingAreaX();

        /// <summary>
        /// このウィンドウが含まれるスクリーンの位置のY座標を取得する
        /// </summary>
        /// <returns></returns>
        [PureVirtualFunction]
        int getWorkingAreaY();

        /// <summary>
        /// このウィンドウが含まれるスクリーンの幅を取得する
        /// </summary>
        /// <returns></returns>
        [PureVirtualFunction]
        int getWorkingAreaWidth();

        /// <summary>
        /// このウィンドウが含まれるスクリーンの高さを取得する
        /// </summary>
        /// <returns></returns>
        [PureVirtualFunction]
        int getWorkingAreaHeight();

        /// <summary>
        /// ウィンドウを破棄することなく、非表示状態にする
        /// </summary>
        [PureVirtualFunction]
        void hideWindow();
    }

}

