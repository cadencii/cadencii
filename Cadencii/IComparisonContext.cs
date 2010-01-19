#if JAVA
package org.kbinani.cadencii;

#else
using System;

namespace org.kbinani.cadencii {
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
