/*
 * FormMainUi.cs
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

#endif

            /// <summary>
            /// メイン画面の実装クラスが持つべきメソッドを定義するインターフェース
            /// </summary>
            public interface FormMainUi
            {
                /// <summary>
                /// ピアノロールの部品にフォーカスを持たせる
                /// </summary>
                [PureVirtualFunction]
                void focusPianoRoll();
            }

#if !JAVA
    }
}
#endif
