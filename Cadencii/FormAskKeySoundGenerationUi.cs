/*
 * FormAskKeySoundGenerationUi.cs
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

#elif __cplusplus

namespace org{ namespace kbinani{ namespace cadencii{

#else

using System;
using org.kbinani.windows.forms;
using org.kbinani.apputil;

namespace org.kbinani.cadencii
{
    using boolean = System.Boolean;
    using BEventArgs = System.EventArgs;
    using BEventHandler = System.EventHandler;
#endif

    /// <summary>
    /// FormAskKeySoundGenerationフォームのビューが実装すべきメソッドを規定します．
    /// </summary>
#if JAVA
    public interface FormAskKeySoundGenerationUi extends UiBase
#elif __cplusplus
    abstract class FormAskKeySoundGenerationUi : UiBase
#else
    public interface FormAskKeySoundGenerationUi : UiBase
#endif
    {
        void setAlwaysPerformThisCheck( boolean value );

        boolean isAlwaysPerformThisCheck();

        void close( boolean value );

        /// <summary>
        /// メッセージの文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列．</param>
        void setMessageLabelText( String value );

        /// <summary>
        /// チェックボックスの文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列．</param>
        void setAlwaysPerformThisCheckCheckboxText( String value );

        /// <summary>
        /// 「はい」ボタンの文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列．</param>
        void setYesButtonText( String value );

        /// <summary>
        /// 「いいえ」ボタンの文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列．</param>
        void setNoButtonText( String value );
    }

#if __cplusplus
}   }   }
#elif CSHARP
}
#endif
