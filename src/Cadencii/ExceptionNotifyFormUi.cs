/*
 * ExceptionNotifyFormUi.cs
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

    public interface ExceptionNotifyFormUi : UiBase
    {
        /// <summary>
        /// ウィンドウのタイトルを設定する
        /// </summary>
        /// <param name="vlaue"></param>
        [PureVirtualFunction]
        void setTitle(string vlaue);

        /// <summary>
        /// 説明文を設定する
        /// </summary>
        /// <param name="value"></param>
        [PureVirtualFunction]
        void setDescription(string value);

        /// <summary>
        /// 例外情報を設定する
        /// </summary>
        /// <param name="value"></param>
        [PureVirtualFunction]
        void setExceptionMessage(string value);

        [PureVirtualFunction]
        void setCancelButtonText(string value);

        [PureVirtualFunction]
        void setSendButtonText(string value);

        /// <summary>
        /// ダイアログを閉じる
        /// </summary>
        [PureVirtualFunction]
        void close();
    }

}
