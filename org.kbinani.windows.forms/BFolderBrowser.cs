/*
 * BFolderBrowser.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.windows.forms.
 *
 * org.kbinani.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ../BuildJavaUI/src/org/kbinani/windows/forms/BFolderBrowser.java
#else
using System;
using System.Windows.Forms;

namespace com.github.cadencii.windows.forms {

    public class BFolderBrowser {
        public FolderBrowserDialog dialog;

        public BFolderBrowser() {
            dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.Desktop;
        }

        public void close()
        {
            if ( dialog != null ) {
                dialog.Dispose();
            }
        }

        public bool isNewFolderButtonVisible() {
            return dialog.ShowNewFolderButton;
        }

        public void setNewFolderButtonVisible( bool value ) {
            dialog.ShowNewFolderButton = value;
        }

        public string getDescription() {
            return dialog.Description;
        }

        public void setDescription( string value ) {
            dialog.Description = value;
        }

        public string getSelectedPath() {
            return dialog.SelectedPath;
        }

        public void setSelectedPath( string value ) {
            dialog.SelectedPath = value;
        }

        public BDialogResult showDialog( System.Windows.Forms.Form parent )
        {
            DialogResult ret = dialog.ShowDialog( parent );
            if ( ret == DialogResult.OK ) {
                return BDialogResult.OK;
            }
            return BDialogResult.CANCEL;
        }
    }

}
#endif
