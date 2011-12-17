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

package com.github.cadencii;

#else

namespace com.github
{
    namespace cadencii
    {

#if __cplusplus
            using namespace std;
            using namespace org::kbinani::cadencii;
#else
            using System;
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
                [PureVirtualFunction]
                void setAlwaysPerformThisCheck( bool value );

                [PureVirtualFunction]
                bool isAlwaysPerformThisCheck();

                /// <summary>
                /// フォームを閉じます．
                /// valueがtrueのときダイアログの結果をCancelに，それ以外の場合はOKとなるようにします．
                /// </summary>
                [PureVirtualFunction]
                void close( bool value );

                /// <summary>
                /// メッセージの文字列を設定します．
                /// </summary>
                /// <param name="value">設定する文字列．</param>
                [PureVirtualFunction]
                void setMessageLabelText( string value );

                /// <summary>
                /// チェックボックスの文字列を設定します．
                /// </summary>
                /// <param name="value">設定する文字列．</param>
                [PureVirtualFunction]
                void setAlwaysPerformThisCheckCheckboxText( string value );

                /// <summary>
                /// 「はい」ボタンの文字列を設定します．
                /// </summary>
                /// <param name="value">設定する文字列．</param>
                [PureVirtualFunction]
                void setYesButtonText( string value );

                /// <summary>
                /// 「いいえ」ボタンの文字列を設定します．
                /// </summary>
                /// <param name="value">設定する文字列．</param>
                [PureVirtualFunction]
                void setNoButtonText( string value );
            };

#if !JAVA

    }
}

#endif
