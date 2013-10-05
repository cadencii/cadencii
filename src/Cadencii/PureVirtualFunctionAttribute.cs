/*
 * PureVirtualFunctionAttribute.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace cadencii
{

    /// <summary>
    /// メソッドが純粋仮想関数であることをpp_cs2javaに通知するための属性です．
    /// 通知したいメソッドの直前の行にこの属性を記述してください．
    /// </summary>
    /// <example>
    /// C#のコードに以下のように記述したとします．
    /// <code>
    /// interface Foo{
    ///     [PureVirtualFunction]
    ///     void someMethod();
    /// }
    /// </code>
    /// すると，pp_cs2javaに--replace-cppオプションを付けて処理させると次のようになります．
    /// <code>
    /// interface Foo{
    ///     virtual void someMethod() = 0;
    /// }
    /// </code>
    /// </example>
    public class PureVirtualFunctionAttribute : Attribute { }

}
