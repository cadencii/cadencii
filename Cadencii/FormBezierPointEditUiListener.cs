/*
 * FormBezierPointEditUiListener.cs
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
package org.kbinani.cadencii;

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/cadencii/FormBezierPointEdit.java

import java.awt.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.apputil;
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.util;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
#endif

#if JAVA
    public interface FormBezierPointEditUiListener
#else
    public interface FormBezierPointEditUiListener
#endif
    {
    }

#if !JAVA
}
#endif
