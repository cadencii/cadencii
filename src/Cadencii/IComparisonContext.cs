/*
 * IComparisonContext.cs
 * Copyright © 2010-2011 kbinani
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
    /// 2つのタイムラインを比較するのに必要な機能を実装するためのインターフェース．
    /// </summary>
    public interface IComparisonContext
    {
        int getNextIndex1();
        int getNextIndex2();
        Object getElementAt1(int index);
        Object getElementAt2(int index);
        bool hasNext1();
        bool hasNext2();
        int getClockFrom(Object obj);
        bool equals(Object obj1, Object obj2);
    }

}
