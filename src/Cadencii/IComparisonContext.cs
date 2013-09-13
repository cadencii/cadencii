/*
 * IComparisonContext.cs
 * Copyright © 2010-2011 kbinani
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
package cadencii;

#else
using System;

namespace cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 2つのタイムラインを比較するのに必要な機能を実装するためのインターフェース．
    /// </summary>
    public interface IComparisonContext {
        int getNextIndex1();
        int getNextIndex2();
        Object getElementAt1( int index );
        Object getElementAt2( int index );
        boolean hasNext1();
        boolean hasNext2();
        int getClockFrom( Object obj );
        boolean equals( Object obj1, Object obj2 );
    }

#if !JAVA
}
#endif
