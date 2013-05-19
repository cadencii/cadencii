/*
 * BFileChooser.cs
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
//INCLUDE ./BFileChooser.java
#else
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace cadencii.windows.forms {

    public class BFileChooser : IDisposable {
        public const int APPROVE_OPTION = 0;
        public const int CANCEL_OPTION = 1;
        public const int ERROR_OPTION = -1;

        private List<string> m_filters = new List<string>();
        private OpenFileDialog m_open = new OpenFileDialog();
        private SaveFileDialog m_save = new SaveFileDialog();
        private string m_current_filter = "";
        private string m_selected_file = "";

        public BFileChooser() {
        }

        public void addFileFilter( String filter ) {
            m_filters.Add( filter );
        }

        public String getFileFilter() {
            return m_current_filter;
        }

        public void setFileFilter( String value ) {
            m_current_filter = value;
        }

        public void clearChoosableFileFilter() {
            m_filters.Clear();
        }

        public string[] getChoosableFileFilter() {
            return m_filters.ToArray();
        }

        public String getSelectedFile() {
            return m_selected_file;
        }

        public void setSelectedFile( String value ) {
            m_selected_file = value;
            String dir = "";
            if ( value != null && value.Length > 0 ) {
                dir = PortUtil.getDirectoryName( value );
            }
            String name = PortUtil.getFileName( value );
            m_open.FileName = name;
            m_open.InitialDirectory = dir;
            m_save.FileName = name;
            m_save.InitialDirectory = dir;
        }

        public int showOpenDialog( Control parent ) {
            string filter = "";
            int count = 0;
            int selected = -1;
            foreach ( string f in m_filters ) {
                filter += (count == 0 ? "" : "|") + f;
                if ( f == m_current_filter ) {
                    selected = count;
                }
                count++;
            }
            m_open.Filter = filter;
            if ( selected >= 0 ) {
                m_open.FilterIndex = selected + 1;
            }
            DialogResult dr = m_open.ShowDialog();
            m_selected_file = m_open.FileName;
            int filter_index = m_open.FilterIndex - 1;
            if ( 0 <= filter_index && filter_index < m_filters.Count ) {
                m_current_filter = m_filters[filter_index];
            }
            if ( dr == DialogResult.OK ) {
                return APPROVE_OPTION;
            } else if ( dr == DialogResult.Cancel ) {
                return CANCEL_OPTION;
            } else {
                return ERROR_OPTION;
            }
        }

        public int showSaveDialog( Control parent ) {
            string filter = "";
            int count = 0;
            int selected = -1;
            foreach ( string f in m_filters ) {
                filter += (count == 0 ? "" : "|") + f;
                if ( f == m_current_filter ) {
                    selected = count;
                }
                count++;
            }
            m_save.Filter = filter;
            if ( selected >= 0 ) {
                m_save.FilterIndex = selected + 1;
            }
            DialogResult dr = m_save.ShowDialog();
            m_selected_file = m_save.FileName;
            int filter_index = m_save.FilterIndex - 1;
            if ( 0 <= filter_index && filter_index < m_filters.Count ) {
                m_current_filter = m_filters[filter_index];
            }
            if ( dr == DialogResult.OK ) {
                return APPROVE_OPTION;
            } else if ( dr == DialogResult.Cancel ) {
                return CANCEL_OPTION;
            } else {
                return ERROR_OPTION;
            }
        }

        public void setDialogTitle( String value ) {
            m_open.Title = value;
            m_save.Title = value;
        }

        public void Dispose() {
            if ( m_open == null ) {
                m_open.Dispose();
            }
            if ( m_save == null ) {
                m_save.Dispose();
            }
        }
    }

}
#endif
