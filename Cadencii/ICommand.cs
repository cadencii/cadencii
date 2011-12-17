/*
 * ICommand.cs
 * Copyright © 2009-2011 kbinani
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

import java.util.*;
#else
using System.Collections.Generic;
using com.github.cadencii;
using com.github.cadencii.java.util;

namespace com.github.cadencii {
#endif

    public interface ICommand {
        /// <summary>
        /// 子コマンドのリスト
        /// </summary>
        Vector<ICommand> getChild();

        /// <summary>
        /// 親コマンドへの参照
        /// </summary>
        ICommand getParent();
        void setParent( ICommand value );
    }

#if !JAVA
}
#endif
