/*
 * FormBezierPointEditUi.cs
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/cadencii/FormBezierPointEdit.java

import java.awt.*;
import java.util.*;
import com.github.cadencii.*;
import com.github.cadencii.apputil.*;
import com.github.cadencii.windows.forms.*;
#else
using System;
using com.github.cadencii.apputil;
using com.github.cadencii;
using com.github.cadencii.java.awt;
using com.github.cadencii.java.util;
using com.github.cadencii.windows.forms;

namespace com.github.cadencii
{
    using BEventArgs = System.EventArgs;
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using BEventHandler = System.EventHandler;
    using BMouseEventHandler = System.Windows.Forms.MouseEventHandler;
    using boolean = System.Boolean;
    using BMouseButtons = System.Windows.Forms.MouseButtons;
#endif

#if JAVA
    public interface FormBezierPointEditUi extends UiBase
#else
    public interface FormBezierPointEditUi : UiBase
#endif
    {
    }

#if !JAVA
}
#endif
