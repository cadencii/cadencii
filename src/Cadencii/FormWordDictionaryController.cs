/*
 * FormWordDictionaryController.cs
 * Copyright © 2011 kbinani
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
using System.Collections.Generic;

namespace cadencii
{
    using System;
    using System.Windows.Forms;
    using cadencii.apputil;
    using cadencii.vsq;
    using cadencii;
    using cadencii.java.util;
    using cadencii.windows.forms;

    public class FormWordDictionaryController : ControllerBase, FormWordDictionaryUiListener
    {
        private FormWordDictionaryUiImpl ui;
        private static int mColumnWidth = 256;
        private static int mWidth = 327;
        private static int mHeight = 404;

        public FormWordDictionaryController()
        {
            ui = new FormWordDictionaryUiImpl(this);
            applyLanguage();
            ui.setSize(mWidth, mHeight);
            ui.listDictionariesSetColumnWidth(mColumnWidth);
        }


        #region FormWordDictionaryUiListenerの実装

        public void buttonCancelClick()
        {
            ui.setDialogResult(false);
        }

        public void buttonDownClick()
        {
            int index = ui.listDictionariesGetSelectedRow();
            if (0 <= index && index + 1 < ui.listDictionariesGetItemCountRow()) {
                try {
                    ui.listDictionariesClear();
                    string upper_name = ui.listDictionariesGetItemAt(index);
                    bool upper_enabled = ui.listDictionariesIsRowChecked(index);
                    string lower_name = ui.listDictionariesGetItemAt(index + 1);
                    bool lower_enabled = ui.listDictionariesIsRowChecked(index + 1);

                    ui.listDictionariesSetItemAt(index + 1, upper_name);
                    ui.listDictionariesSetRowChecked(index + 1, upper_enabled);
                    ui.listDictionariesSetItemAt(index, lower_name);
                    ui.listDictionariesSetRowChecked(index, lower_enabled);

                    ui.listDictionariesSetSelectedRow(index + 1);
                } catch (Exception ex) {
                    serr.println("FormWordDictionary#btnDown_Click; ex=" + ex);
                }
            }
        }

        public void buttonUpClick()
        {
            int index = ui.listDictionariesGetSelectedRow();
            if (index >= 1) {
                try {
                    ui.listDictionariesClearSelection();
                    string upper_name = ui.listDictionariesGetItemAt(index - 1);
                    bool upper_enabled = ui.listDictionariesIsRowChecked(index - 1);
                    string lower_name = ui.listDictionariesGetItemAt(index);
                    bool lower_enabled = ui.listDictionariesIsRowChecked(index);

                    ui.listDictionariesSetItemAt(index - 1, lower_name);
                    ui.listDictionariesSetRowChecked(index - 1, lower_enabled);
                    ui.listDictionariesSetItemAt(index, upper_name);
                    ui.listDictionariesSetRowChecked(index, upper_enabled);

                    ui.listDictionariesSetSelectedRow(index - 1);
                } catch (Exception ex) {
                    serr.println("FormWordDictionary#btnUp_Click; ex=" + ex);
                }
            }
        }

        public void buttonOkClick()
        {
            ui.setDialogResult(true);
        }

        public void formLoad()
        {
            ui.listDictionariesClear();
            for (int i = 0; i < SymbolTable.getCount(); i++) {
                string name = SymbolTable.getSymbolTable(i).getName();
                bool enabled = SymbolTable.getSymbolTable(i).isEnabled();
                ui.listDictionariesAddRow(name, enabled);
            }
        }

        public void formClosing()
        {
            mColumnWidth = ui.listDictionariesGetColumnWidth();
            mWidth = ui.getWidth();
            mHeight = ui.getHeight();
        }

        #endregion


        #region public methods

        public void close()
        {
            ui.close();
        }

        public UiBase getUi()
        {
            return ui;
        }

        public int getWidth()
        {
            return ui.getWidth();
        }

        public int getHeight()
        {
            return ui.getHeight();
        }

        public void setLocation(int x, int y)
        {
            ui.setLocation(x, y);
        }

        public void applyLanguage()
        {
            ui.setTitle(_("User Dictionary Configuration"));
            ui.labelAvailableDictionariesSetText(_("Available Dictionaries"));
            ui.buttonOkSetText(_("OK"));
            ui.buttonCancelSetText(_("Cancel"));
            ui.buttonUpSetText(_("Up"));
            ui.buttonDownSetText(_("Down"));
        }

        public List<ValuePair<string, Boolean>> getResult()
        {
            List<ValuePair<string, Boolean>> ret = new List<ValuePair<string, Boolean>>();
            int count = ui.listDictionariesGetItemCountRow();
#if DEBUG
            sout.println("FormWordDictionary#getResult; count=" + count);
#endif
            for (int i = 0; i < count; i++) {
                string name = ui.listDictionariesGetItemAt(i);

                ret.Add(new ValuePair<string, Boolean>(
                    ui.listDictionariesGetItemAt(i), ui.listDictionariesIsRowChecked(i)));
            }
            return ret;
        }

        #endregion


        #region private methods

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        #endregion
    }

}
