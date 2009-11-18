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
#if JAVA
package org.kbinani.Cadencii;

#else
using System;
using System.Drawing;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using bocoree;
using bocoree.awt.event_;
using bocoree.util;
using bocoree.windows.forms;
using bocoreex.swing;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormShortcutKeys extends BForm{
#else
    public class FormShortcutKeys : BForm {
#endif
        private BMenuItem m_dumy;
        private TreeMap<String, ValuePair<String, BKeys[]>> m_dict;
        private TreeMap<String, ValuePair<String, BKeys[]>> m_first_dict;

        public FormShortcutKeys( TreeMap<String, ValuePair<String, BKeys[]>> dict ) {
            InitializeComponent();
            registerEventHandlers();
            setResources();
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
            for ( Iterator itr = src.keySet().iterator(); itr.hasNext(); ) {
                String name = (String)itr.next();
                String key = src.get( name ).getKey();
                BKeys[] values = src.get( name ).getValue();
                Vector<BKeys> cp = new Vector<BKeys>();
                foreach ( BKeys k in values ) {
                    cp.add( k );
                }
                dest.put( name, new ValuePair<String, BKeys[]>( key, cp.toArray( new BKeys[] { } ) ) );
            }
        }

        private void UpdateList() {
            list.Items.Clear();
            for ( Iterator itr = m_dict.keySet().iterator(); itr.hasNext(); ) {
                String display = (String)itr.next();
                Vector<BKeys> a = new Vector<BKeys>();
                foreach ( BKeys key in m_dict.get( display ).getValue() ) {
                    a.add( key );
                }
                try {
                    m_dumy.setAccelerator( PortUtil.getKeyStrokeFromBKeys( a.toArray( new BKeys[] { } ) ) );
                } catch {
                    a.clear();
                }
                ListViewItem item = new ListViewItem( new String[] { display, AppManager.getShortcutDisplayString( a.toArray( new BKeys[] { } ) ) } );
                String name = m_dict.get( display ).getKey();
                item.Name = name;
                //item.Tag = a;
                if ( name.StartsWith( "menuFile" ) ) {
                    item.Group = list.Groups["listGroupFile"];
                } else if ( name.StartsWith( "menuEdit" ) ) {
                    item.Group = list.Groups["listGroupEdit"];
                } else if ( name.StartsWith( "menuVisual" ) ) {
                    item.Group = list.Groups["listGroupVisual"];
                } else if ( name.StartsWith( "menuJob" ) ) {
                    item.Group = list.Groups["listGroupJob"];
                } else if ( name.StartsWith( "menuLyric" ) ) {
                    item.Group = list.Groups["listGroupLyric"];
                } else if ( name.StartsWith( "menuTrack" ) ) {
                    item.Group = list.Groups["listGroupTrack"];
                } else if ( name.StartsWith( "menuScript" ) ) {
                    item.Group = list.Groups["listGroupScript"];
                } else if ( name.StartsWith( "menuSetting" ) ) {
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
            for ( Iterator itr = AppManager.SHORTCUT_ACCEPTABLE.iterator(); itr.hasNext(); ) {
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
            } catch ( Exception ex ) {
                if ( ((e.KeyCode & Keys.Up) != Keys.Up) &&
                     ((e.KeyCode & Keys.Down) != Keys.Down) ) {
                    e.Handled = true;
                }
                return;
            }
            //list.Items[index].Tag = res;
            list.Items[index].SubItems[1].Text = AppManager.getShortcutDisplayString( capturelist.toArray( new BKeys[] { } ) );
            String display = list.Items[index].SubItems[0].Text;
            if ( m_dict.containsKey( display ) ) {
                m_dict.get( display ).setValue( capturelist.toArray( new BKeys[] { } ) );
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
                for ( Iterator itr = m_dict.keySet().iterator(); itr.hasNext(); ) {
                    String display = (String)itr.next();
                    if ( name.Equals( m_dict.get( display ).getKey() ) ) {
                        m_dict.get( display ).setValue( keys );
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

        private void registerEventHandlers() {
            this.list.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.list_PreviewKeyDown );
            this.list.KeyDown += new System.Windows.Forms.KeyEventHandler( this.list_KeyDown );
            this.btnLoadDefault.Click += new System.EventHandler( this.btnLoadDefault_Click );
            this.btnRevert.Click += new System.EventHandler( this.btnRevert_Click );
        }

        private void setResources() {
        }
#if JAVA
#else
        #region UI Impl for C#
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing ) {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup( "File", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup( "Edit", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup( "View", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup( "Job", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup( "Track", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup( "Lyric", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup( "Script", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup( "Setting", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup( "Help", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup10 = new System.Windows.Forms.ListViewGroup( "Other", System.Windows.Forms.HorizontalAlignment.Left );
            this.btnCancel = new BButton();
            this.btnOK = new BButton();
            this.list = new BListView();
            this.columnCommand = new System.Windows.Forms.ColumnHeader();
            this.columnShortcut = new System.Windows.Forms.ColumnHeader();
            this.btnLoadDefault = new BButton();
            this.btnRevert = new BButton();
            this.toolTip = new System.Windows.Forms.ToolTip( this.components );
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 325, 403 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point( 244, 403 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // list
            // 
            this.list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.list.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnCommand,
            this.columnShortcut} );
            this.list.FullRowSelect = true;
            listViewGroup1.Header = "File";
            listViewGroup1.Name = "listGroupFile";
            listViewGroup2.Header = "Edit";
            listViewGroup2.Name = "listGroupEdit";
            listViewGroup3.Header = "View";
            listViewGroup3.Name = "listGroupVisual";
            listViewGroup4.Header = "Job";
            listViewGroup4.Name = "listGroupJob";
            listViewGroup5.Header = "Track";
            listViewGroup5.Name = "listGroupTrack";
            listViewGroup6.Header = "Lyric";
            listViewGroup6.Name = "listGroupLyric";
            listViewGroup7.Header = "Script";
            listViewGroup7.Name = "listGroupScript";
            listViewGroup8.Header = "Setting";
            listViewGroup8.Name = "listGroupSetting";
            listViewGroup9.Header = "Help";
            listViewGroup9.Name = "listGroupHelp";
            listViewGroup10.Header = "Other";
            listViewGroup10.Name = "listGroupOther";
            this.list.Groups.AddRange( new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6,
            listViewGroup7,
            listViewGroup8,
            listViewGroup9,
            listViewGroup10} );
            this.list.Location = new System.Drawing.Point( 12, 12 );
            this.list.MultiSelect = false;
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size( 388, 343 );
            this.list.TabIndex = 9;
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.Details;
            // 
            // columnCommand
            // 
            this.columnCommand.Text = "Command";
            this.columnCommand.Width = 240;
            // 
            // columnShortcut
            // 
            this.columnShortcut.Text = "Shortcut Key";
            this.columnShortcut.Width = 140;
            // 
            // btnLoadDefault
            // 
            this.btnLoadDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadDefault.Location = new System.Drawing.Point( 113, 361 );
            this.btnLoadDefault.Name = "btnLoadDefault";
            this.btnLoadDefault.Size = new System.Drawing.Size( 95, 23 );
            this.btnLoadDefault.TabIndex = 11;
            this.btnLoadDefault.Text = "Load Default";
            this.btnLoadDefault.UseVisualStyleBackColor = true;
            // 
            // btnRevert
            // 
            this.btnRevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRevert.Location = new System.Drawing.Point( 12, 361 );
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size( 95, 23 );
            this.btnRevert.TabIndex = 10;
            this.btnRevert.Text = "Revert";
            this.btnRevert.UseVisualStyleBackColor = true;
            // 
            // FormShortcutKeys
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 412, 438 );
            this.Controls.Add( this.btnLoadDefault );
            this.Controls.Add( this.btnRevert );
            this.Controls.Add( this.list );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormShortcutKeys";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Shortcut Config";
            this.ResumeLayout( false );

        }

        #endregion

        private BButton btnCancel;
        private BButton btnOK;
        private BListView list;
        private System.Windows.Forms.ColumnHeader columnCommand;
        private System.Windows.Forms.ColumnHeader columnShortcut;
        private BButton btnLoadDefault;
        private BButton btnRevert;
        private System.Windows.Forms.ToolTip toolTip;
        #endregion
#endif
    }

#if !JAVA
}
#endif
