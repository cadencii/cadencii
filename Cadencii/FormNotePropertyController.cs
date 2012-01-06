/*
 * FormNoteProperty.cs
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

package com.github.cadencii;

import javax.swing.*;
import com.github.cadencii.*;
import com.github.cadencii.apputil.*;
import com.github.cadencii.windows.forms.*;
import com.github.cadencii.ui.*;

#else

using System;
using com.github.cadencii.javax.swing;
using com.github.cadencii.apputil;
using com.github.cadencii;
using com.github.cadencii.windows.forms;

namespace com.github.cadencii
{
#endif

#if JAVA
    public class FormNotePropertyController implements FormNotePropertyUiListener
#else
    public class FormNotePropertyController : FormNotePropertyUiListener
#endif
    {
        private bool mPreviousAlwaysOnTop;
        private FormNotePropertyUi ui;
        private PropertyWindowListener propertyWindowListener;

        public FormNotePropertyController( PropertyWindowListener propertyWindowListener )
        {
            this.propertyWindowListener = propertyWindowListener;
            this.ui = (FormNotePropertyUi)new FormNotePropertyUiImpl( this );
            applyLanguage();
        }


        #region public methods

        /// <summary>
        /// AlwaysOnTopが強制的にfalseにされる直前の，AlwaysOnTop値を取得します．
        /// </summary>
        public bool getPreviousAlwaysOnTop()
        {
            return mPreviousAlwaysOnTop;
        }

        /// <summary>
        /// AlwaysOnTopが強制的にfalseにされる直前の，AlwaysOnTop値を設定しておきます．
        /// </summary>
        public void setPreviousAlwaysOnTop( bool value )
        {
            mPreviousAlwaysOnTop = value;
        }

        public void applyLanguage()
        {
            this.ui.setTitle( _( "Note Property" ) );
        }

        public void applyShortcut( KeyStroke value )
        {
            this.ui.setMenuCloseAccelerator( value );
        }

        public FormNotePropertyUi getUi()
        {
            return this.ui;
        }

        #endregion


        #region helper methods

        private static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        #endregion


        #region FormNotePropertyUiListenerの実装

        public void onLoad()
        {
            this.ui.setAlwaysOnTop( true );
        }

        public void menuCloseClick()
        {
            this.ui.close();
        }

        public void windowStateChanged()
        {
            this.propertyWindowListener.propertyWindowStateChanged();
        }

        public void locationOrSizeChanged()
        {
            this.propertyWindowListener.propertyWindowLocationOrSizeChanged();
        }

        public void formClosing()
        {
            this.propertyWindowListener.propertyWindowFormClosing();
        }

        #endregion

    }

#if !JAVA
}
#endif
