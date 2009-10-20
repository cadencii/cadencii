/*
 * BezierControlType.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.cadencii;

public enum BezierControlType {
    /// <summary>
    /// 制御点無し
    /// </summary>
    None,
    /// <summary>
    /// このタイプの制御点を移動させると、逆サイドの制御点も自動的に移動する
    /// </summary>
    Normal,
    /// <summary>
    /// このタイプの制御点を移動させても、逆サイドの制御点には影響しない
    /// </summary>
    Master,
}
