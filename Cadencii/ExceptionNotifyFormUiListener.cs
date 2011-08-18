﻿/*
 * ExceptionNotifyFormUiListener.cs
 * Copyright © 2011 kbinani
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

#else

namespace org
{
    namespace kbinani
    {
        namespace cadencii
        {

#endif

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

#if !JAVA
        }
    }
}
#endif
