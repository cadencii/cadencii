/*
 * PureVirtualFunctionAttribute.cs
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
#if CSHARP
using System;

namespace org.kbinani.cadencii
{

    /// <summary>
    /// メソッドが純粋仮想関数であることをpp_cs2javaに通知するための属性です．
    /// 通知したいメソッドの直前の行にこの属性を記述してください．
    /// </summary>
    public class PureVirtualFunctionAttribute : Attribute
    {
    }

}
#endif
