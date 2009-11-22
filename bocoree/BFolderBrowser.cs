/*
 * BFolderBrowser.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BFolderBrowser.java
#else
using System;
using System.Windows.Forms;

namespace bocoree.windows.forms {

    public class BFolderBrowser {
        public FolderBrowserDialog dialog;

        public BFolderBrowser() {
            dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.Desktop;
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

        public BDialogResult showDialog() {
            if ( dialog.ShowDialog() == DialogResult.OK ) {
                return BDialogResult.OK;
            } else {
                return BDialogResult.CANCEL;
            }
        }
    }

}
#endif
