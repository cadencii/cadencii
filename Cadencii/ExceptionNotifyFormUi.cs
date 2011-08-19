/*
 * ExceptionNotifyFormUi.cs
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

using System;

namespace org
{
    namespace kbinani
    {
        namespace cadencii
        {

#endif

#if JAVA
            public interface ExceptionNotifyFormUi extends UiBase
#else
            public interface ExceptionNotifyFormUi : UiBase
#endif
            {
                /// <summary>
                /// ウィンドウのタイトルを設定する
                /// </summary>
                /// <param name="vlaue"></param>
                void setTitle( String vlaue );

                /// <summary>
                /// 説明文を設定する
                /// </summary>
                /// <param name="value"></param>
                void setDescription( String value );

                /// <summary>
                /// 例外情報を設定する
                /// </summary>
                /// <param name="value"></param>
                void setExceptionMessage( String value );

                void setCancelButtonText( String value );

                void setSendButtonText( String value );

                /// <summary>
                /// ダイアログを閉じる
                /// </summary>
                void close();
            }

#if !JAVA
        }
    }
}
#endif
