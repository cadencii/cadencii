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

#else

namespace org
{
    namespace kbinani
    {
        namespace cadencii
        {

#if __cplusplus
            using namespace org::kbinani::cadencii;      
#else

            using System;
            using org.kbinani.windows.forms;
            using org.kbinani.apputil;

            using boolean = System.Boolean;
            using BEventArgs = System.EventArgs;
            using BEventHandler = System.EventHandler;
#endif
#endif

            /// <summary>
            /// FormAskKeySoundGenerationフォームのビューが実装すべきメソッドを規定します．
            /// </summary>
#if JAVA
            public interface FormAskKeySoundGenerationUi extends UiBase
#elif __cplusplus
            class FormAskKeySoundGenerationUi : UiBase
#else
            public interface FormAskKeySoundGenerationUi : UiBase
#endif
            {
#if __cplusplus
                virtual void setAlwaysPerformThisCheck( bool value ) = 0;
#else
                void setAlwaysPerformThisCheck( boolean value );
#endif

#if __cplusplus
                virtual bool isAlwaysPerformThisCheck() = 0;
#else
                boolean isAlwaysPerformThisCheck();
#endif

                /// <summary>
                /// フォームを閉じます．
                /// valueがtrueのときダイアログの結果をCancelに，それ以外の場合はOKとなるようにします．
                /// </summary>
#if __cplusplus
                virtual void close( bool value ) = 0;
#else
                void close( boolean value );
#endif

                /// <summary>
                /// メッセージの文字列を設定します．
                /// </summary>
                /// <param name="value">設定する文字列．</param>
#if __cplusplus
                virtual void setMessageLabelText( QString value ) = 0;
#else
                void setMessageLabelText( String value );
#endif

                /// <summary>
                /// チェックボックスの文字列を設定します．
                /// </summary>
                /// <param name="value">設定する文字列．</param>
#if __cplusplus
                virtual void setAlwaysPerformThisCheckCheckboxText( QString value ) = 0;
#else
                void setAlwaysPerformThisCheckCheckboxText( String value );
#endif

                /// <summary>
                /// 「はい」ボタンの文字列を設定します．
                /// </summary>
                /// <param name="value">設定する文字列．</param>
#if __cplusplus
                virtual void setYesButtonText( QString value ) = 0;
#else
                void setYesButtonText( String value );
#endif

                /// <summary>
                /// 「いいえ」ボタンの文字列を設定します．
                /// </summary>
                /// <param name="value">設定する文字列．</param>
#if __cplusplus
                virtual void setNoButtonText( QString value ) = 0;
#else
                void setNoButtonText( String value );
#endif
            };

#if !JAVA
        }
    }
}
#endif
