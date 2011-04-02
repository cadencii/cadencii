/*
 * FormAskKeySoundGeneration.cs
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
package org.kbinani.cadencii;

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/cadencii/FormAskKeySoundGeneration.java

import org.kbinani.*;
import org.kbinani.windows.forms.*;
import org.kbinani.apputil.*;
#else
using System;
using org.kbinani.windows.forms;
using org.kbinani.apputil;

namespace org.kbinani.cadencii
{
    using boolean = System.Boolean;
    using BEventArgs = System.EventArgs;
    using BEventHandler = System.EventHandler;
#endif

#if JAVA
    public class FormAskKeySoundGeneration implements IFormAskKeySoundGenerationControl
#else
    public class FormAskKeySoundGeneration : IFormAskKeySoundGenerationControl
#endif
    {
        private FormAskKeySoundGenerationUi mUi = null;

        #region public methods
        public void setupUi( FormAskKeySoundGenerationUi ui )
        {
            mUi = ui;
        }

        public FormAskKeySoundGenerationUi getUi()
        {
            return mUi;
        }

        public void buttonCancelClickedSlot()
        {
            mUi.close( true );
        }

        public void buttonOkClickedSlot()
        {
            mUi.close( false );
        }
        #endregion
    }

#if !JAVA
}
#endif
