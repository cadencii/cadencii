/*
 * FormShortcutKeys.cs
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
using System.Windows.Forms;
using System.Drawing;

using Boare.Lib.AppUtil;
using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    public partial class FormShortcutKeys : Form {
        private ToolStripMenuItem m_dumy;
        private TreeMap<String, ValuePair<String, Keys[]>> m_dict;
        private TreeMap<String, ValuePair<String, Keys[]>> m_first_dict;

        public FormShortcutKeys( TreeMap<String, ValuePair<String, Keys[]>> dict ) {
            InitializeComponent();
            m_dict = dict;
            m_dumy = new ToolStripMenuItem();
            m_dumy.ShowShortcutKeys = true;
            m_first_dict = new TreeMap<String, ValuePair<String, Keys[]>>();
            CopyDict( m_dict, ref m_first_dict );
            ApplyLanguage();
            UpdateList();
            Util.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont ); 
        }

        public void ApplyLanguage() {
            this.Text = _( "Shortcut Config" );

            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
            btnRevert.Text = _( "Revert" );
            btnLoadDefault.Text = _( "Load Default" );

            columnCommand.Text = _( "Command" );
            columnShortcut.Text = _( "Shortcut Key" );

            toolTip.SetToolTip( list, _( "Select command and hit key(s) you want to set.\nHit Backspace if you want to remove shortcut key." ) );

            list.Groups["listGroupFile"].Header = _( "File" );
            list.Groups["listGroupEdit"].Header = _( "Edit" );
            list.Groups["listGroupVisual"].Header = _( "View" );
            list.Groups["listGroupJob"].Header = _( "Job" );
            list.Groups["listGroupLyric"].Header = _( "Lyrics" );
            list.Groups["listGroupSetting"].Header = _( "Setting" );
            list.Groups["listGroupHelp"].Header = _( "Help" );
            list.Groups["listGroupTrack"].Header = _( "Track" );
            list.Groups["listGroupScript"].Header = _( "Script" );
            list.Groups["listGroupOther"].Header = _( "Others" );
        }

        private static String _( String id ) {
            return Messaging.GetMessage( id );
        }

        public TreeMap<String, ValuePair<String, Keys[]>> Result {
            get {
                TreeMap<String, ValuePair<String, Keys[]>> ret = new TreeMap<String, ValuePair<String, Keys[]>>();
                CopyDict( m_dict, ref ret );
                return ret;
            }
        }

        private static void CopyDict( TreeMap<String, ValuePair<String, Keys[]>> src, ref TreeMap<String, ValuePair<String, Keys[]>> dest ) {
            dest.clear();
            for ( Iterator itr = src.keySet().iterator(); itr.hasNext(); ){
                String name = (String)itr.next();
                String key = src.get( name ).Key;
                Keys[] values = src.get( name ).Value;
                Vector<Keys> cp = new Vector<Keys>();
                foreach ( Keys k in values ) {
                    cp.add( k );
                }
                dest.put( name, new ValuePair<String, Keys[]>( key, cp.toArray( new Keys[]{} ) ) );
            }
        }

        private void UpdateList() {
            list.Items.Clear();
            for ( Iterator itr = m_dict.keySet().iterator(); itr.hasNext(); ){
                String display = (String)itr.next();
                Keys k = Keys.None;
                Vector<Keys> a = new Vector<Keys>();
                foreach( Keys key in m_dict.get( display ).Value ){
                    a.add( key );
                    k = k | key;
                }
                try {
                    m_dumy.ShortcutKeys = k;
                } catch {
                    k = Keys.None;
                }
                ListViewItem item = new ListViewItem( new String[] { display, AppManager.getShortcutDisplayString( a.toArray( new Keys[]{} ) ) } );
                String name = m_dict.get( display ).Key;
                item.Name = name;
                item.Tag = k;
                if ( name.StartsWith( "menuFile" ) ) {
                    item.Group = list.Groups["listGroupFile"];
                } else if ( name.StartsWith( "menuEdit" ) ) {
                    item.Group = list.Groups["listGroupEdit"];
                } else if ( name.StartsWith( "menuVisual" ) ){
                    item.Group = list.Groups["listGroupVisual"];
                } else if ( name.StartsWith( "menuJob" ) ) {
                    item.Group = list.Groups["listGroupJob"];
                } else if ( name.StartsWith( "menuLyric" ) ){
                    item.Group = list.Groups["listGroupLyric"];
                } else if ( name.StartsWith( "menuTrack" ) ){
                    item.Group = list.Groups["listGroupTrack"];
                } else if ( name.StartsWith( "menuScript" ) ){
                    item.Group = list.Groups["listGroupScript"];
                } else if ( name.StartsWith( "menuSetting" ) ){
                    item.Group = list.Groups["listGroupSetting"];
                } else if ( name.StartsWith( "menuHelp" ) ) {
                    item.Group = list.Groups["listGroupHelp"];
                } else {
                    item.Group = list.Groups["listGroupOther"];
                }
                list.Items.Add( item );
            }
            UpdateColor();
        }

        private void list_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e ) {
        }

        private void list_KeyDown( object sender, KeyEventArgs e ) {
            if ( list.SelectedIndices.Count <= 0 ) {
                return;
            }
            int index = list.SelectedIndices[0];
            Keys code = e.KeyCode;
            Keys capture = Keys.None;
            Vector<Keys> capturelist = new Vector<Keys>();
            for( Iterator itr = AppManager.SHORTCUT_ACCEPTABLE.iterator(); itr.hasNext() ; ){
                Keys k = (Keys)itr.next();
                if ( code == k ) {
                    capturelist.add( k );
                    capture = k;
                    break;
                }
            }
            Keys res = capture;
            if ( (e.Modifiers & Keys.Menu) == Keys.Menu ) {
                res = res | Keys.Menu;
                capturelist.add( Keys.Menu );
            }
            if ( (e.Modifiers & Keys.Control) == Keys.Control ) {
                res = res | Keys.Control;
                capturelist.add( Keys.Control );
            }
            if ( (e.Modifiers & Keys.Shift) == Keys.Shift ) {
                res = res | Keys.Shift;
                capturelist.add( Keys.Shift );
            }
            if ( (e.Modifiers & Keys.Alt) == Keys.Alt ) {
                res = res | Keys.Alt;
                capturelist.add( Keys.Alt );
            }

            // 指定されたキーの組み合わせが、ショートカットとして適切かどうか判定
            try {
                m_dumy.ShortcutKeys = res;
            } catch {
                return;
            }
            list.Items[index].Tag = res;
            list.Items[index].SubItems[1].Text = AppManager.getShortcutDisplayString( capturelist.toArray( new Keys[]{} ) );
            String display = list.Items[index].SubItems[0].Text;
            if ( m_dict.containsKey( display ) ) {
                m_dict.get( display ).Value = capturelist.toArray( new Keys[]{} );
            }
            UpdateColor();
        }

        private void btnRevert_Click( object sender, EventArgs e ) {
            CopyDict( m_first_dict, ref m_dict );
            UpdateList();
        }

        private void btnLoadDefault_Click( object sender, EventArgs e ) {
            for ( int i = 0; i < EditorConfig.DEFAULT_SHORTCUT_KEYS.size(); i++ ) {
                String name = EditorConfig.DEFAULT_SHORTCUT_KEYS.get( i ).Key;
                Keys[] keys = EditorConfig.DEFAULT_SHORTCUT_KEYS.get( i ).Value;
                for ( Iterator itr = m_dict.keySet().iterator(); itr.hasNext(); ){
                    String display = (String)itr.next();
                    if ( name.Equals( m_dict.get( display ).Key ) ) {
                        m_dict.get( display ).Value = keys;
                        break;
                    }
                }
            }
            UpdateList();
        }

        private void UpdateColor() {
            for ( int i = 0; i < list.Items.Count; i++ ) {
                String compare = list.Items[i].SubItems[1].Text;
                if ( compare.Equals( "" ) ) {
                    list.Items[i].BackColor = SystemColors.Window;
                    continue;
                }
                boolean found = false;
                for ( int j = 0; j < list.Items.Count; j++ ) {
                    if ( i == j ) {
                        continue;
                    }
                    if ( compare.Equals( list.Items[j].SubItems[1].Text ) ) {
                        found = true;
                        break;
                    }
                }
                if ( found ) {
                    list.Items[i].BackColor = Color.Yellow;
                } else {
                    list.Items[i].BackColor = SystemColors.Window;
                }
            }
        }
    }

}
