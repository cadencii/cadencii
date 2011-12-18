/*
 * Utility.cs
 * Copyright © 2010-2011 kbinani
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
package com.github.cadencii.windows.forms;

import javax.swing.*;
#else
using System;

namespace com.github.cadencii.windows.forms {
#endif

    public class Utility {
        public const int MSGBOX_DEFAULT_OPTION = -1;
        public const int MSGBOX_YES_NO_OPTION = 0;
        public const int MSGBOX_YES_NO_CANCEL_OPTION = 1;
        public const int MSGBOX_OK_CANCEL_OPTION = 2;

        public const int MSGBOX_ERROR_MESSAGE = 0;
        public const int MSGBOX_INFORMATION_MESSAGE = 1;
        public const int MSGBOX_WARNING_MESSAGE = 2;
        public const int MSGBOX_QUESTION_MESSAGE = 3;
        public const int MSGBOX_PLAIN_MESSAGE = -1;

        public static BDialogResult showMessageBox( String text, String caption, int optionType, int messageType ) {
            BDialogResult ret = BDialogResult.CANCEL;
#if JAVA
            int r = JOptionPane.showConfirmDialog( null, text, caption, optionType, messageType );
            if ( r == JOptionPane.YES_OPTION ){
                ret = BDialogResult.YES;
            } else if ( r == JOptionPane.NO_OPTION ){
                ret = BDialogResult.NO;
            } else if ( r == JOptionPane.CANCEL_OPTION ){
                ret = BDialogResult.CANCEL;
            } else if ( r == JOptionPane.OK_OPTION ){
                ret = BDialogResult.OK;
            } else if ( r == JOptionPane.CLOSED_OPTION ){
                ret = BDialogResult.CANCEL;
            }
#else
            System.Windows.Forms.MessageBoxButtons btn = System.Windows.Forms.MessageBoxButtons.OK;
            if ( optionType == MSGBOX_YES_NO_CANCEL_OPTION ) {
                btn = System.Windows.Forms.MessageBoxButtons.YesNoCancel;
            } else if ( optionType == MSGBOX_YES_NO_OPTION ) {
                btn = System.Windows.Forms.MessageBoxButtons.YesNo;
            } else if ( optionType == MSGBOX_OK_CANCEL_OPTION ) {
                btn = System.Windows.Forms.MessageBoxButtons.OKCancel;
            } else {
                btn = System.Windows.Forms.MessageBoxButtons.OK;
            }

            System.Windows.Forms.MessageBoxIcon icon = System.Windows.Forms.MessageBoxIcon.None;
            if ( messageType == MSGBOX_ERROR_MESSAGE ) {
                icon = System.Windows.Forms.MessageBoxIcon.Error;
            } else if ( messageType == MSGBOX_INFORMATION_MESSAGE ) {
                icon = System.Windows.Forms.MessageBoxIcon.Information;
            } else if ( messageType == MSGBOX_PLAIN_MESSAGE ) {
                icon = System.Windows.Forms.MessageBoxIcon.None;
            } else if ( messageType == MSGBOX_QUESTION_MESSAGE ) {
                icon = System.Windows.Forms.MessageBoxIcon.Question;
            } else if ( messageType == MSGBOX_WARNING_MESSAGE ) {
                icon = System.Windows.Forms.MessageBoxIcon.Warning;
            }

            System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show( text, caption, btn, icon );
            if ( dr == System.Windows.Forms.DialogResult.OK ) {
                ret = BDialogResult.OK;
            } else if ( dr == System.Windows.Forms.DialogResult.Cancel ) {
                ret = BDialogResult.CANCEL;
            } else if ( dr == System.Windows.Forms.DialogResult.Yes ) {
                ret = BDialogResult.YES;
            } else if ( dr == System.Windows.Forms.DialogResult.No ) {
                ret = BDialogResult.NO;
            }
#endif
            return ret;
        }

    }

#if !JAVA
}
#endif
