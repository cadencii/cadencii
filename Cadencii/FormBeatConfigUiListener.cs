/*
 * FormBeatConfig.cs
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/cadencii/FormBeatConfig.java

import java.awt.event.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.java.awt.event_;
using org.kbinani.apputil;
using org.kbinani;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
    using boolean = System.Boolean;
    using BEventArgs = System.EventArgs;
    using BEventHandler = System.EventHandler;
#endif

    public interface FormBeatConfigUiListener
    {
        void buttonCancelClickedSlot();

        void buttonOkClickedSlot();

        void checkboxEndCheckedChangedSlot();
    }

#if !JAVA
}
#endif
