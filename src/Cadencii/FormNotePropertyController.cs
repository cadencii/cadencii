/*
 * FormNotePropertyController.cs
 * Copyright © 2009-2011 kbinani
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
using System.Windows.Forms;
using cadencii.apputil;
using cadencii;
using cadencii.windows.forms;



namespace cadencii
{

    public class FormNotePropertyController : FormNotePropertyUiListener
    {
        private bool mPreviousAlwaysOnTop;
        private FormNotePropertyUi ui;
        private PropertyWindowListener propertyWindowListener;

        public FormNotePropertyController(PropertyWindowListener propertyWindowListener)
        {
            this.propertyWindowListener = propertyWindowListener;
            this.ui = (FormNotePropertyUi)new FormNotePropertyUiImpl(this);
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
        public void setPreviousAlwaysOnTop(bool value)
        {
            mPreviousAlwaysOnTop = value;
        }

        public void applyLanguage()
        {
            this.ui.setTitle(_("Note Property"));
        }

        public void applyShortcut(Keys value)
        {
            this.ui.setMenuCloseAccelerator(value);
        }

        public FormNotePropertyUi getUi()
        {
            return this.ui;
        }

        #endregion


        #region helper methods

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        #endregion


        #region FormNotePropertyUiListenerの実装

        public void onLoad()
        {
            this.ui.setAlwaysOnTop(true);
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

}
