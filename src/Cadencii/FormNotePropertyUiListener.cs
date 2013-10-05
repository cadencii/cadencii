/*
 * FormNotePropertyUiListener.cs
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

    public interface FormNotePropertyUiListener
    {
        /// <summary>
        /// ウィンドウの読み込みが完了したとき呼ばれる
        /// </summary>
        [PureVirtualFunction]
        void onLoad();

        /// <summary>
        /// 閉じるメニューが押されたとき呼ばれる
        /// </summary>
        [PureVirtualFunction]
        void menuCloseClick();

        /// <summary>
        /// ウィンドウの表示状態(最小化/通常)が変わったとき呼ばれる
        /// </summary>
        [PureVirtualFunction]
        void windowStateChanged();

        /// <summary>
        /// ウィンドウの位置またはサイズが変わったとき呼ばれる
        /// </summary>
        [PureVirtualFunction]
        void locationOrSizeChanged();

        /// <summary>
        /// ウィンドウが閉じようとしているとき呼ばれる
        /// </summary>
        [PureVirtualFunction]
        void formClosing();
    }

}
