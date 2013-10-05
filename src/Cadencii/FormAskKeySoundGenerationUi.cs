/*
 * FormAskKeySoundGenerationUi.cs
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

    using System;

    /// <summary>
    /// FormAskKeySoundGenerationフォームのビューが実装すべきメソッドを規定します．
    /// </summary>
    public interface FormAskKeySoundGenerationUi : UiBase
    {
        [PureVirtualFunction]
        void setAlwaysPerformThisCheck(bool value);

        [PureVirtualFunction]
        bool isAlwaysPerformThisCheck();

        /// <summary>
        /// フォームを閉じます．
        /// valueがtrueのときダイアログの結果をCancelに，それ以外の場合はOKとなるようにします．
        /// </summary>
        [PureVirtualFunction]
        void close(bool value);

        /// <summary>
        /// メッセージの文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列．</param>
        [PureVirtualFunction]
        void setMessageLabelText(string value);

        /// <summary>
        /// チェックボックスの文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列．</param>
        [PureVirtualFunction]
        void setAlwaysPerformThisCheckCheckboxText(string value);

        /// <summary>
        /// 「はい」ボタンの文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列．</param>
        [PureVirtualFunction]
        void setYesButtonText(string value);

        /// <summary>
        /// 「いいえ」ボタンの文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列．</param>
        [PureVirtualFunction]
        void setNoButtonText(string value);
    };


}
