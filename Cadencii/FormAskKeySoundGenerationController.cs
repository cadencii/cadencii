/*
 * FormAskKeySoundGenerationController.cs
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
    public class FormAskKeySoundGenerationController extends ControllerBase implements FormAskKeySoundGenerationUiListener
#else
    public class FormAskKeySoundGenerationController : ControllerBase, FormAskKeySoundGenerationUiListener
#endif
    {
        private FormAskKeySoundGenerationUi mUi = null;

        #region public methods
        public void setupUi( FormAskKeySoundGenerationUi ui )
        {
            mUi = ui;
            applyLanguage();
        }

        public FormAskKeySoundGenerationUi getUi()
        {
            return mUi;
        }

        public void applyLanguage()
        {
            mUi.setMessageLabelText( _( "It seems some key-board sounds are missing. Do you want to re-generate them now?" ) );
            mUi.setAlwaysPerformThisCheckCheckboxText( _( "Always perform this check when starting Cadencii." ) );
            mUi.setYesButtonText( _( "Yes" ) );
            mUi.setNoButtonText( _( "No" ) );
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

        #region private methods
        private static String _( String message )
        {
            return Messaging.getMessage( message );
        }
        #endregion
    }

#if !JAVA
}
#endif
