/*
 * FileDialog.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    public partial class FileDialog : Form {
        private String m_initial_directory = "";
        private String m_current_directory = "";
        private SortType m_sort_type = SortType.Name;
        private Vector<String> m_history = new Vector<String>();
        private boolean m_multiselect = false;
        private DialogMode m_mode = DialogMode.Open;
        private Vector<String> m_selected_item = new Vector<String>();
        private Vector<String> m_filter = new Vector<String>();

        public enum DialogMode {
            Open,
            Save,
        }

        private enum SortType {
            Name,
            Size,
            Type,
            Date,
        }

        public FileDialog( DialogMode mode ) {
            m_mode = mode;
            InitializeComponent();
            if ( m_current_directory.Equals( "" ) ) {
                m_current_directory = InitialDirectory;
            }
            UpdateFileList();
#if DEBUG
            OpenFileDialog ofd = new OpenFileDialog();
#endif
        }

        public int FilterIndex {
            get {
                return comboFileType.SelectedIndex + 1;
            }
            set {
                if ( 0 <= value - 1 && value - 1 < comboFileType.Items.Count ) {
                    comboFileType.SelectedIndex = value - 1;
                }
            }
        }

        public String Title {
            get {
                return this.Text;
            }
            set {
                this.Text = value;
            }
        }

        private String CurrentFilterExtension {
            get {
                if ( m_filter.size() <= 0 ) {
                    return "";
                } else {
                    if ( 0 <= comboFileType.SelectedIndex && comboFileType.SelectedIndex < m_filter.size() ) {
                        String filter = m_filter.get( comboFileType.SelectedIndex );
                        String[] spl = filter.Split( '|' );
                        if ( spl.Length < 2 ) {
                            return "";
                        }
                        int index = spl[1].LastIndexOf( "." );
                        if ( index < 0 ) {
                            return "";
                        }
                        filter = spl[1].Substring( index );
                        if ( filter.StartsWith( ".*" ) ) {
                            return "";
                        }
                        return filter;
                    } else {
                        return "";
                    }
                }
            }
        }

        public String Filter {
            set {
                String[] spl = value.Split( '|' );
                if ( spl.Length % 2 != 0 ) {
                    throw new ApplicationException( "invalid filter format" );
                }
                m_filter.clear();
                comboFileType.Items.Clear();
                for ( int i = 0; i < spl.Length; i += 2 ) {
                    m_filter.add( spl[i] + "|" + spl[i + 1] );
                    comboFileType.Items.Add( spl[i] );
                }
                if ( spl.Length > 0 ) {
                    comboFileType.SelectedIndex = 0;
                }
            }
            get {
                String ret = "";
                for ( int i = 0; i < m_filter.size(); i++ ) {
                    ret += (i == 0 ? "" : "|") + m_filter.get( i );
                }
                return ret;
            }
        }

        private void UpdateFileList() {
            listFiles.Items.Clear();
            if ( listFiles.SmallImageList != null ) {
                listFiles.SmallImageList.Images.Clear();
            } else {
                listFiles.SmallImageList = new ImageList();
            }
            listFiles.SmallImageList.ColorDepth = ColorDepth.Depth32Bit;
            listFiles.SmallImageList.ImageSize = new Size( 16, 16 );
            if ( listFiles.LargeImageList != null ) {
                listFiles.LargeImageList.Images.Clear();
            } else {
                listFiles.LargeImageList = new ImageList();
            }
            listFiles.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;

            if ( m_current_directory.Equals( "" ) ) {
                m_current_directory = InitialDirectory;
            }
            if ( m_current_directory.Equals( "MY_COMPUTER" ) ) {
                listFiles.SmallImageList.Images.Add( "DRIVE", Properties.Resources.drive );
                String[] drives = Environment.GetLogicalDrives();
                for ( int i = 0; i < drives.Length; i++ ) {
                    listFiles.Items.Add( drives[i], "DRIVE" );
                }
            } else {
                // ディレクトリ一覧
                String[] dirnames = Directory.GetDirectories( m_current_directory );
                Vector<DirectoryInfo> dirs = new Vector<DirectoryInfo>();
                for ( int i = 0; i < dirnames.Length; i++ ) {
                    dirs.add( new DirectoryInfo( dirnames[i] ) );
                }
                boolean changed = true;
                while ( changed ) {
                    changed = false;
                    for ( int i = 0; i < dirs.size() - 1; i++ ) {
                        boolean swap = false;
                        switch ( m_sort_type ) {
                            case SortType.Name:
                                swap = dirs.get( i ).Name.CompareTo( dirs.get( i + 1 ).Name ) > 0;
                                break;
                            case SortType.Date:
                                swap = dirs.get( i ).LastWriteTimeUtc > dirs.get( i + 1 ).LastWriteTimeUtc;
                                break;
                        }
                        if ( swap ) {
                            DirectoryInfo di = dirs.get( i );
                            dirs.set( i, dirs.get( i + 1 ) );
                            dirs.set( i + 1, di );
                            changed = true;
                        }
                    }
                }
                listFiles.SmallImageList.Images.Add( "FOLDER", Properties.Resources.folder );

                for ( int i = 0; i < dirs.size(); i++ ) {
                    listFiles.Items.Add( dirs.get( i ).Name, "FOLDER" );
                }

                // ファイル一覧
                String[] filenames = Directory.GetFiles( m_current_directory );
                //TreeMap<String, String> extensions = new TreeMap<String, String>();
                Vector<FileInfo> files = new Vector<FileInfo>();
                String filter = CurrentFilterExtension;
                for ( int i = 0; i < filenames.Length; i++ ) {
                    if ( filter.Equals( "" ) ) {
                        files.add( new FileInfo( filenames[i] ) );
                    } else {
                        String ext = Path.GetExtension( filenames[i] ).ToLower();
                        if ( ext.Equals( filter ) ) {
                            files.add( new FileInfo( filenames[i] ) );
                        }
                    }
                    /*
                    if ( ext != "" ) {
                        if ( !extensions.ContainsKey( ext ) ) {
                            extensions.Add( ext, filenames[i] );
                        }
                    }*/
                }
                changed = true;
                while ( changed ) {
                    changed = false;
                    for ( int i = 0; i < files.size() - 1; i++ ) {
                        boolean swap = false;
                        switch ( m_sort_type ) {
                            case SortType.Name:
                                swap = files.get( i ).Name.CompareTo( files.get( i + 1 ).Name ) > 0;
                                break;
                            case SortType.Date:
                                swap = files.get( i ).LastWriteTimeUtc > files.get( i + 1 ).LastWriteTimeUtc;
                                break;
                        }
                        if ( swap ) {
                            FileInfo di = files.get( i );
                            files.set( i, files.get( i + 1 ) );
                            files.set( i + 1, di );
                            changed = true;
                        }
                    }
                }

                /*foreach ( String key in extensions.Keys ) {
                    listFiles.SmallImageList.Images.Add( key, Icon.ExtractAssociatedIcon( extensions[key] ) );
                }*/
                for ( int i = 0; i < files.size(); i++ ) {
                    String ext = Path.GetExtension( files.get( i ).FullName ).ToLower();
                    listFiles.SmallImageList.Images.Add( files.get( i ).FullName, Icon.ExtractAssociatedIcon( files.get( i ).FullName ) );
                    //if ( extensions.ContainsKey( ext ) ) {
                        listFiles.Items.Add( files.get( i ).Name, files.get( i ).FullName );
                    //} else {
                        //listFiles.Items.Add( files[i].Name );
                    //}
                }
            }
        }

        public boolean Multiselect {
            get {
                return m_multiselect;
            }
            set {
                m_multiselect = value;
                listFiles.MultiSelect = m_multiselect;
            }
        }

        public String InitialDirectory {
            get {
                if ( m_initial_directory.Equals( "" ) ) {
                    m_initial_directory = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments );
                    m_current_directory = m_initial_directory;
                }
                return m_initial_directory;
            }
            set {
                m_initial_directory = value;
                m_current_directory = m_initial_directory;
            }
        }

        private void FileDialog_Load( object sender, EventArgs e ) {
            UpdateFileList();
        }

        private void btnUp_Click( object sender, EventArgs e ) {
            m_history.add( m_current_directory );
            if ( m_current_directory.Equals( "MY_COMPUTER" ) ) {
                m_current_directory = Environment.GetFolderPath( Environment.SpecialFolder.Desktop );
            } else if ( Path.GetPathRoot( m_current_directory ).ToLower().Equals( m_current_directory.ToLower() ) ) {
                m_current_directory = "MY_COMPUTER";
            } else {
                m_current_directory = Path.GetDirectoryName( m_current_directory );
            }
            UpdateFileList();
            UpdateButtonStatus();
        }

        private void UpdateButtonStatus() {
            btnUp.Enabled = m_current_directory != Environment.GetFolderPath( Environment.SpecialFolder.Desktop );
            btnPrev.Enabled = m_history.size() > 0;
            chkDesktop.Checked = false;
            chkMyComputer.Checked = false;
            //radioMyNetwork.Checked = false;
            chkPersonal.Checked = false;
            //radioRecent.Checked = false;
            btnNew.Enabled = (m_current_directory != "MY_COMPUTER") && (m_current_directory != "MY_NETWORK");
            listFiles.MultiSelect = (m_multiselect && m_current_directory != "MY_COMPUTER" && m_current_directory != "MY_NETWORK");
            if ( m_current_directory.Equals( Environment.GetFolderPath( Environment.SpecialFolder.Desktop ) ) ) {
                chkDesktop.Checked = true;
                return;
            }
            if ( m_current_directory.Equals( "MY_COMPUTER" ) ) {
                chkMyComputer.Checked = true;
                return;
            }
            if ( m_current_directory.Equals( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) ) ) {
                chkPersonal.Checked = true;
                return;
            }
            /*if ( m_current_directory == Environment.GetFolderPath( Environment.SpecialFolder.Recent ) ) {
                radioRecent.Checked = true;
                return;
            }
            if ( m_current_directory == "MY_NETWORK" ) {
                radioMyNetwork.Checked = true;
                return;
            }*/
        }

        private void btnPrev_Click( object sender, EventArgs e ) {
            if ( m_history.size() > 0 ) {
                m_current_directory = m_history.get( m_history.size() - 1 );
                m_history.removeElementAt( m_history.size() - 1 );
            }
            UpdateFileList();
            UpdateButtonStatus();
        }

        private void radioRecent_Click( object sender, EventArgs e ) {
            m_history.add( m_current_directory );
            m_current_directory = Environment.GetFolderPath( Environment.SpecialFolder.Recent );
            UpdateFileList();
            UpdateButtonStatus();
        }

        private void radioDesktop_Click( object sender, EventArgs e ) {
            m_history.add( m_current_directory );
            m_current_directory = Environment.GetFolderPath( Environment.SpecialFolder.Desktop );
            UpdateFileList();
            UpdateButtonStatus();
        }

        private void radioPersonal_Click( object sender, EventArgs e ) {
            m_history.add( m_current_directory );
            m_current_directory = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments );
            UpdateFileList();
            UpdateButtonStatus();
        }

        private void radioMyComputer_Click( object sender, EventArgs e ) {
            m_history.add( m_current_directory );
            m_current_directory = "MY_COMPUTER";
            UpdateFileList();
            UpdateButtonStatus();
        }

        private void listFiles_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e ) {
            if ( listFiles.SelectedItems.Count != 1 ) {
                return;
            }
            if ( e.KeyCode == Keys.Enter ) {
                ProcessListViewItemClicked( listFiles.SelectedItems[0] );
            }
        }

        private void listFiles_MouseDoubleClick( object sender, MouseEventArgs e ) {
            ListViewItem item = listFiles.GetItemAt( e.X, e.Y );
            if ( item != null ) {
                ProcessListViewItemClicked( item );
            }
        }

        private void ProcessListViewItemClicked( ListViewItem listviewitem ) {
            String item = listviewitem.Text;
            if ( m_current_directory.Equals( "MY_COMPUTER" ) ) {
                boolean available = true;
                try {
                    String[] dirs = Directory.GetDirectories( item );
                } catch {
                    available = false;
                }
                if ( !available ) {
                    return;
                }
                m_history.add( m_current_directory );
                m_current_directory = item;
            } else if ( m_current_directory.Equals( "MY_NETWORK" ) ) {
                //todo:
            } else {
                String next = Path.Combine( m_current_directory, item );
                if ( Directory.Exists( next ) ) {
                    m_history.add( m_current_directory );
                    m_current_directory = next;
                } else if ( PortUtil.isFileExists( next ) ) {
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            UpdateFileList();
            UpdateButtonStatus();
        }

        public String[] FileNames {
            get {
                String[] ret = new String[m_selected_item.size()];
                for ( int i = 0; i < m_selected_item.size(); i++ ) {
                    ret[i] = Path.Combine( m_current_directory, m_selected_item.get( i ) );
                }
                return ret;
            }
        }

        public String FileName {
            get {
                if ( m_selected_item.size() == 0 ) {
                    String ret = m_current_directory;
                    if ( m_current_directory.Equals( "MY_COMPUTER" ) || m_current_directory.Equals( "MY_NETWORK" ) ) {
                        ret = "";
                    }
                    return ret;
                } else {
                    return Path.Combine( m_current_directory, m_selected_item.get( 0 ) );
                }
            }
            set {
                if ( value != "" ) {
                    m_initial_directory = Path.GetDirectoryName( value );
                    m_current_directory = m_initial_directory;
                    comboFileName.Text = Path.GetFileName( value );
                }
            }
        }

        private void listFiles_ItemSelectionChanged( object sender, ListViewItemSelectionChangedEventArgs e ) {
            StringBuilder sb = new StringBuilder();
            int count = listFiles.SelectedItems.Count;
            if ( count <= 0 ) {
                m_selected_item.clear();
                return;
            } else if ( count == 1 ) {
                ListViewItem lvi = listFiles.SelectedItems[0];
                if ( lvi.ImageKey != "DRIVE" && lvi.ImageKey != "FOLDER" ) {
                    m_selected_item.clear();
                    m_selected_item.add( lvi.Text );
                    comboFileName.Text = lvi.Text;
                }
            } else {
                m_selected_item.clear();
                boolean first = true;
                foreach ( ListViewItem lvi in listFiles.SelectedItems ) {
                    if ( lvi.ImageKey != "DRIVE" && lvi.ImageKey != "FOLDER" ) {
                        m_selected_item.add( lvi.Text );
                        sb.Append( (first ? "" : " ") + "\"" + lvi.Text + "\"" );
                        first = false;
                    }
                }
                comboFileName.Text = sb.ToString();
                sb = null;
            }
        }

        private void comboFileType_SelectedIndexChanged( object sender, EventArgs e ) {
            UpdateFileList();
        }
    }

}
