/*
 * Platform.cs
 * Copyright © 2008-2011 kbinani
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
namespace com.github.cadencii {
#endif

    /// <summary>
    /// プラットフォームを表す列挙型．
    /// （互換性のため，Javaの命名規則に基づくリファクタリングは未実施．）
    /// </summary>
    public enum PlatformEnum {
        Windows,
        Macintosh,
        Linux,
    }

#if !JAVA
}
#endif
