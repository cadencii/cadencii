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
using bocoree.util;
using bocoree.windows.forms;
using bocoreex.swing;
using bocoree.awt.event_;

namespace Boare.Cadencii {
    using java = bocoree;
    using boolean = System.Boolean;

    public partial class FormShortcutKeys : BForm {
        private BMenuItem m_dumy;
        private TreeMap<String, ValuePair<String, BKeys[]>> m_dict;
        private TreeMap<String, ValuePair<String, BKeys[]>> m_first_dict;

        public FormShortcutKeys( TreeMap<String, ValuePair<String, BKeys[]>> dict ) {
            InitializeComponent();
            m_dict = dict;
            m_dumy = new BMenuItem();
            m_dumy.ShowShortcutKeys = true;
            m_first_dict = new TreeMap<String, ValuePair<String, BKeys[]>>();
            CopyDict( m_dict, ref m_first_dict );
            ApplyLanguage();
            UpdateList();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() ); 
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
            return Messaging.getMessage( id );
        }

        public TreeMap<String, ValuePair<String, BKeys[]>> Result {
            get {
                TreeMap<String, ValuePair<String, BKeys[]>> ret = new TreeMap<String, ValuePair<String, BKeys[]>>();
                CopyDict( m_dict, ref ret );
                return ret;
            }
        }

        private static void CopyDict( TreeMap<String, ValuePair<String, BKeys[]>> src, ref TreeMap<String, ValuePair<String, BKeys[]>> dest ) {
            dest.clear();
            for ( Iterator itr = src.keySet().iterator(); itr.hasNext(); ){
                String name = (String)itr.next();
                String key = src.get( name ).Key;
                BKeys[] values = src.get( name ).Value;
                Vector<BKeys> cp = new Vector<BKeys>();
                foreach ( BKeys k in values ) {
                    cp.add( k );
                }
                dest.put( name, new ValuePair<String, BKeys[]>( key, cp.toArray( new BKeys[]{} ) ) );
            }
        }

        private void UpdateList() {
            list.Items.Clear();
            for ( Iterator itr = m_dict.keySet().iterator(); itr.hasNext(); ){
                String display = (String)itr.next();
                Vector<BKeys> a = new Vector<BKeys>();
                foreach( BKeys key in m_dict.get( display ).Value ){
                    a.add( key );
                }
                try {
                    m_dumy.setAccelerator( PortUtil.getKeyStrokeFromBKeys( a.toArray( new BKeys[]{} ) ) );
                } catch {
                    a.clear();
                }
                ListViewItem item = new ListViewItem( new String[] { display, AppManager.getShortcutDisplayString( a.toArray( new BKeys[]{} ) ) } );
                String name = m_dict.get( display ).Key;
                item.Name = name;
                //item.Tag = a;
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
#if DEBUG
            PortUtil.println( "FormShortcutKeys#list_KeyDown; e.KeyCode=" + e.KeyCode );
#endif
            int index = list.SelectedIndices[0];
            KeyStroke stroke = KeyStroke.getKeyStroke( 0, 0 );
            stroke.keys = e.KeyCode | e.Modifiers;
            int code = stroke.getKeyCode();
            int modifier = stroke.getModifiers();

            Vector<BKeys> capturelist = new Vector<BKeys>();
            BKeys capture = BKeys.None;
            for( Iterator itr = AppManager.SHORTCUT_ACCEPTABLE.iterator(); itr.hasNext() ; ){
                BKeys k = (BKeys)itr.next();
#if JAVA
                if( code == k.getValue() ){
#else
                if ( code == (int)k ) {
#endif
                    capturelist.add( k );
                    if ( (modifier & InputEvent.ALT_MASK) == InputEvent.ALT_MASK ) {
                        capturelist.add( BKeys.Alt );
                    }
                    if ( (modifier & InputEvent.CTRL_MASK) == InputEvent.CTRL_MASK ) {
                        capturelist.add( BKeys.Control );
                    }
                    if ( (modifier & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK ) {
                        capturelist.add( BKeys.Shift );
                    }
                    capture = k;
                    break;
                }
            }

            // 指定されたキーの組み合わせが、ショートカットとして適切かどうか判定
            try {
#if JAVA
                m_dumy.setAccelerator( KeyStroke.getKeyStrok( capture.getValue(), modifier ) );
#else
                m_dumy.setAccelerator( KeyStroke.getKeyStroke( (int)capture, modifier ) );
#endif
            } catch( Exception ex ) {
                if ( ((e.KeyCode & Keys.Up) != Keys.Up) &&
                     ((e.KeyCode & Keys.Down) != Keys.Down) ) {
                    e.Handled = true;
                }
                return;
            }
            //list.Items[index].Tag = res;
            list.Items[index].SubItems[1].Text = AppManager.getShortcutDisplayString( capturelist.toArray( new BKeys[]{} ) );
            String display = list.Items[index].SubItems[0].Text;
            if ( m_dict.containsKey( display ) ) {
                m_dict.get( display ).Value = capturelist.toArray( new BKeys[]{} );
            }
            UpdateColor();
            e.Handled = true;
        }

        private void btnRevert_Click( object sender, EventArgs e ) {
            CopyDict( m_first_dict, ref m_dict );
            UpdateList();
        }

        private void btnLoadDefault_Click( object sender, EventArgs e ) {
            for ( int i = 0; i < EditorConfig.DEFAULT_SHORTCUT_KEYS.size(); i++ ) {
                String name = EditorConfig.DEFAULT_SHORTCUT_KEYS.get( i ).Key;
                BKeys[] keys = EditorConfig.DEFAULT_SHORTCUT_KEYS.get( i ).Value;
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
