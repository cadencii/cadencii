/*
 * DynamicsMode.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/
#if JAVA
package com.github.cadencii.vsq;
#elif __cplusplus
namespace org { namespace kbinani { namespace vsq {
#else
namespace com.github.cadencii.vsq
{
#endif

    /// <summary>
    /// VOCALOID1における、ダイナミクスモードを表す定数を格納するためのクラスです。
    /// </summary>
    public class DynamicsMode
    {
        /// <summary>
        /// デフォルトのダイナミクスモードです。DYNカーブが非表示になるモードです。
        /// </summary>
        public const int Standard = 0;
        /// <summary>
        /// エキスパートモードです。DYNカーブが表示されます。
        /// </summary>
        public const int Expert = 1;

        private DynamicsMode()
        {
        }
    };

#if JAVA
#elif __cplusplus
} } }
#else
}
#endif
