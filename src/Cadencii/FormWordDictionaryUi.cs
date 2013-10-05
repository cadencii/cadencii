/*
 * FormWordDictionaryUi.cs
 * Copyright © 2011 kbinani
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
    public interface FormWordDictionaryUi : UiBase
    {
        /// <summary>
        /// ウィンドウのタイトル文字列を設定します
        /// </summary>
        /// <param name="value">設定する文字列</param>
        [PureVirtualFunction]
        void setTitle(string value);

        /// <summary>
        /// ダイアログの戻り値を設定します．
        /// </summary>
        /// <param name="value">ダイアログの戻り値を「キャンセル」にする場合はfalseを，それ以外はtreuを設定します．</param>
        [PureVirtualFunction]
        void setDialogResult(bool value);

        /// <summary>
        /// TODO: comment
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [PureVirtualFunction]
        void setSize(int width, int height);

        /// <summary>
        /// ウィンドウの幅を取得します
        /// </summary>
        /// <returns>ウィンドウの幅(単位はピクセル)</returns>
        [PureVirtualFunction]
        int getWidth();

        /// <summary>
        /// ウィンドウの高さを取得します
        /// </summary>
        /// <returns>ウィンドウの高さ(単位はピクセル)</returns>
        [PureVirtualFunction]
        int getHeight();

        /// <summary>
        /// ウィンドウの位置を設定します
        /// </summary>
        /// <param name="x">ウィンドウのx座標</param>
        /// <param name="y">ウィンドウのy座標</param>
        [PureVirtualFunction]
        void setLocation(int x, int y);

        /// <summary>
        /// ウィンドウを閉じます
        /// </summary>
        [PureVirtualFunction]
        void close();

        /// <summary>
        /// TODO: comment
        /// </summary>
        /// <returns></returns>
        [PureVirtualFunction]
        int listDictionariesGetSelectedRow();

        /// <summary>
        /// リストに登録されたアイテムの個数を取得します
        /// </summary>
        /// <returns>アイテムの個数</returns>
        [PureVirtualFunction]
        int listDictionariesGetItemCountRow();

        /// <summary>
        /// TODO: comment
        /// </summary>
        [PureVirtualFunction]
        void listDictionariesClear();

        /// <summary>
        /// TODO: comment
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [PureVirtualFunction]
        string listDictionariesGetItemAt(int row);

        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        [PureVirtualFunction]
        bool listDictionariesIsRowChecked(int row);

        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        [PureVirtualFunction]
        void listDictionariesSetItemAt(int row, string value);

        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="row"></param>
        /// <param name="value"></param>
        [PureVirtualFunction]
        void listDictionariesSetRowChecked(int row, bool isChecked);

        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="row"></param>
        [PureVirtualFunction]
        void listDictionariesSetSelectedRow(int row);

        /// <summary>
        /// TODO: comment
        /// </summary>
        [PureVirtualFunction]
        void listDictionariesClearSelection();

        /// <summary>
        /// TODO: comment
        /// </summary>
        /// <param name="value"></param>
        /// <param name="selected"></param>
        [PureVirtualFunction]
        void listDictionariesAddRow(string value, bool isChecked);

        /// <summary>
        /// 「利用可能な辞書」という意味の説明文の文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列</param>
        [PureVirtualFunction]
        void labelAvailableDictionariesSetText(string value);

        /// <summary>
        /// OKボタンの表示文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列</param>
        [PureVirtualFunction]
        void buttonOkSetText(string value);

        /// <summary>
        /// Cancelボタンの表示文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列</param>
        [PureVirtualFunction]
        void buttonCancelSetText(string value);

        /// <summary>
        /// Upボタンの表示文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列</param>
        [PureVirtualFunction]
        void buttonUpSetText(string value);

        /// <summary>
        /// Downボタンの表示文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列</param>
        [PureVirtualFunction]
        void buttonDownSetText(string value);
    };

}
