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
                [PureVirtualFunction]
                void setTitle( string vlaue );

                /// <summary>
                /// 説明文を設定する
                /// </summary>
                /// <param name="value"></param>
                [PureVirtualFunction]
                void setDescription( string value );

                /// <summary>
                /// 例外情報を設定する
                /// </summary>
                /// <param name="value"></param>
                [PureVirtualFunction]
                void setExceptionMessage( string value );

                [PureVirtualFunction]
                void setCancelButtonText( string value );

                [PureVirtualFunction]
                void setSendButtonText( string value );
            }

#if !JAVA
        }
    }
}
#endif
