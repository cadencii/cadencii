/*
 * ExceptionNotifyFormUiListener.cs
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

    public interface ExceptionNotifyFormUiListener
    {
        /// <summary>
        /// 例外情報を送信するボタンが押された
        /// </summary>
        [PureVirtualFunction]
        void sendButtonClick();

        /// <summary>
        /// キャンセルボタンが押された
        /// </summary>
        [PureVirtualFunction]
        void cancelButtonClick();
    }

}
