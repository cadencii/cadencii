#if JAVA
package org.kbinani.windows.forms;

import java.awt.*;
import java.io.*;
import javax.swing.*;

public class BFileChooser{
    private JFileChooser m_dialog;

    public void addFileFilter( String filter ){
        // TODO: [not implemented yet at BFileChooser#addFileFilter]
    }

    public String getFileFilter(){
        return ""; // TODO: [fake return at BFileChooser#getFileFilter]
    }

    public void clearChoosableFileFilter() {
        // TODO: [not implemented yet; BFileChooser#clearChoosableFileFilter]
    }

    public String getSelectedFile(){
        File f = m_dialog.getSelectedFile();
        return f.getAbsolutePath();
    }

    public int showOpenDialog( Component parent ){
        return m_dialog.showOpenDialog( parent );
    }

    public int showSaveDialog( Component parent ){
        return m_dialog.showSaveDialog( parent );
    }
}
#else
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace bocoree.windows.forms {

    public class BFileChooser {
        public const int APPROVE_OPTION = 0;
        public const int CANCEL_OPTION = 1;
        public const int ERROR_OPTION = -1;

        private string m_current_directory = "";
        private List<string> m_filters = new List<string>();
        private OpenFileDialog m_open = new OpenFileDialog();
        private SaveFileDialog m_save = new SaveFileDialog();
        private string m_current_filter = "";
        private string m_selected_file = "";

        public BFileChooser( String currentDirectoryPath ) {
            m_current_directory = currentDirectoryPath;
        }

        public void addFileFilter( String filter ) {
            m_filters.Add( filter );
        }

        public String getFileFilter() {
            return m_current_filter;
        }

        public void clearChoosableFileFilter() {
            m_filters.Clear();
        }

        public String getSelectedFile() {
            return m_selected_file;
        }

        public void setSelectedFile( String value ) {
            m_selected_file = value;
            m_open.FileName = m_selected_file;
            m_save.FileName = m_selected_file;
        }

        public int showOpenDialog( Control parent ) {
            m_open.InitialDirectory = m_current_directory;
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
                m_open.FilterIndex = selected;
            }
            DialogResult dr = m_open.ShowDialog();
            m_selected_file = m_open.FileName;
            int filter_index = m_open.FilterIndex;
            if ( 0 <= filter_index && filter_index < m_filters.Count ) {
                m_current_filter = m_filters[filter_index];
            }
            m_current_directory = Path.GetDirectoryName( m_selected_file );
            if ( dr == DialogResult.OK ) {
                return APPROVE_OPTION;
            } else if ( dr == DialogResult.Cancel ) {
                return CANCEL_OPTION;
            } else {
                return ERROR_OPTION;
            }
        }

        public int showSaveDialog( Control parent ) {
            m_save.InitialDirectory = m_current_directory;
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
                m_save.FilterIndex = selected;
            }
            DialogResult dr = m_save.ShowDialog();
            m_selected_file = m_save.FileName;
            int filter_index = m_save.FilterIndex;
            if ( 0 <= filter_index && filter_index < m_filters.Count ) {
                m_current_filter = m_filters[filter_index];
            }
            m_current_directory = Path.GetDirectoryName( m_selected_file );
            if ( dr == DialogResult.OK ) {
                return APPROVE_OPTION;
            } else if ( dr == DialogResult.Cancel ) {
                return CANCEL_OPTION;
            } else {
                return ERROR_OPTION;
            }
        }
    }

}
#endif
