/*
 * FormShortcutKeys.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/Cadencii/FormShortcutKeys.java

import java.awt.event.*;
import java.util.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using org.kbinani.apputil;
using org.kbinani;
using org.kbinani.java.awt.event_;
using org.kbinani.java.util;
using org.kbinani.javax.swing;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using BEventArgs = System.EventArgs;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
    using BKeyEventArgs = System.Windows.Forms.KeyEventArgs;
    using boolean = System.Boolean;
    using BPreviewKeyDownEventArgs = System.Windows.Forms.PreviewKeyDownEventArgs;
    using java = org.kbinani.java;
#endif

#if JAVA
    public class FormShortcutKeys extends BDialog {
#else
    public class FormShortcutKeys : BDialog {
#endif
        private TreeMap<String, ValuePair<String, BKeys[]>> mDict;
        private TreeMap<String, ValuePair<String, BKeys[]>> mFirstDict;
        private static int mColumnWidthCommand = 240;
        private static int mColumnWidthShortcutKey = 140;

        public FormShortcutKeys( TreeMap<String, ValuePair<String, BKeys[]>> dict ) {
#if JAVA
            super();
#endif
            try {
#if JAVA
                initialize();
#else
                InitializeComponent();
#endif
            } catch ( Exception ex ) {
#if DEBUG
                PortUtil.stderr.println( "FormShortcutKeys#.ctor; ex=" + ex );
#endif
            }

#if DEBUG
            PortUtil.println( "FormShortcutKeys#.ctor; dict.size()=" + dict.size() );
#endif
            list.setColumnHeaders( new String[] { "Command", "Shortcut Key" } );
            list.setColumnWidth( 0, mColumnWidthCommand );
            list.setColumnWidth( 1, mColumnWidthShortcutKey );

            registerEventHandlers();
            setResources();
            applyLanguage();

            mDict = dict;
            mFirstDict = new TreeMap<String, ValuePair<String, BKeys[]>>();
            copyDict( mDict, mFirstDict );
            updateList();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        #region public methods
        public void applyLanguage() {
            setTitle( _( "Shortcut Config" ) );

            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
            btnRevert.setText( _( "Revert" ) );
            btnLoadDefault.setText( _( "Load Default" ) );

            list.setColumnHeaders( new String[] { _( "Command" ), _( "Shortcut Key" ) } );
#if JAVA
            System.err.println( "info; FormShortcutKeys#applyLanguage; \"toolTip.SetToolTip( list, _( \"Select command and hit key(s) you want to set.\\nHit Backspace if you want to remove shortcut key.\" ) )" );
#else
            toolTip.SetToolTip( list, _( "Select command and hit key(s) you want to set.\nHit Backspace if you want to remove shortcut key." ) );
#endif

            int num_groups = list.getGroupCount();
            for ( int i = 0; i < num_groups; i++ ) {
                String name = list.getGroupNameAt( i );
                if ( name.Equals( "listGroupFile" ) ) {
                    list.setGroupHeader( name, _( "File" ) );
                } else if ( name.Equals( "listGroupEdit" ) ) {
                    list.setGroupHeader( name, _( "Edit" ) );
                } else if ( name.Equals( "listGroupVisual" ) ) {
                    list.setGroupHeader( name, _( "View" ) );
                } else if ( name.Equals( "listGroupJob" ) ) {
                    list.setGroupHeader( name, _( "Job" ) );
                } else if ( name.Equals( "listGroupLyric" ) ) {
                    list.setGroupHeader( name, _( "Lyrics" ) );
                } else if ( name.Equals( "listGroupSetting" ) ) {
                    list.setGroupHeader( name, _( "Setting" ) );
                } else if ( name.Equals( "listGroupHelp" ) ) {
                    list.setGroupHeader( name, _( "Help" ) );
                } else if ( name.Equals( "listGroupTrack" ) ) {
                    list.setGroupHeader( name, _( "Track" ) );
                } else if ( name.Equals( "listGroupScript" ) ) {
                    list.setGroupHeader( name, _( "Script" ) );
                } else if ( name.Equals( "listGroupOther" ) ) {
                    list.setGroupHeader( name, _( "Others" ) );
                }
            }
        }

        public TreeMap<String, ValuePair<String, BKeys[]>> getResult() {
            TreeMap<String, ValuePair<String, BKeys[]>> ret = new TreeMap<String, ValuePair<String, BKeys[]>>();
            copyDict( mDict, ret );
            return ret;
        }
        #endregion

        #region helper methods
        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private static void copyDict( TreeMap<String, ValuePair<String, BKeys[]>> src, TreeMap<String, ValuePair<String, BKeys[]>> dest ) {
            dest.clear();
            for ( Iterator<String> itr = src.keySet().iterator(); itr.hasNext(); ) {
                String name = itr.next();
                String key = src.get( name ).getKey();
                BKeys[] values = src.get( name ).getValue();
                Vector<BKeys> cp = new Vector<BKeys>();
                foreach ( BKeys k in values ) {
                    cp.add( k );
                }
                dest.put( name, new ValuePair<String, BKeys[]>( key, cp.toArray( new BKeys[] { } ) ) );
            }
        }

        private void updateList() {
            list.clear();
            for ( Iterator<String> itr = mDict.keySet().iterator(); itr.hasNext(); ) {
                String display = itr.next();
                Vector<BKeys> a = new Vector<BKeys>();
                foreach ( BKeys key in mDict.get( display ).getValue() ) {
                    a.add( key );
                }

                BListViewItem item = new BListViewItem( new String[] { display, Utility.getShortcutDisplayString( a.toArray( new BKeys[] { } ) ) } );
                String name = mDict.get( display ).getKey();
                item.setName( name );
                String group = "";
                if ( name.StartsWith( "menuFile" ) ) {
                    group = "listGroupFile";
                } else if ( name.StartsWith( "menuEdit" ) ) {
                    group = "listGroupEdit";
                } else if ( name.StartsWith( "menuVisual" ) ) {
                    group = "listGroupVisual";
                } else if ( name.StartsWith( "menuJob" ) ) {
                    group = "listGroupJob";
                } else if ( name.StartsWith( "menuLyric" ) ) {
                    group = "listGroupLyric";
                } else if ( name.StartsWith( "menuTrack" ) ) {
                    group = "listGroupTrack";
                } else if ( name.StartsWith( "menuScript" ) ) {
                    group = "listGroupScript";
                } else if ( name.StartsWith( "menuSetting" ) ) {
                    group = "listGroupSetting";
                } else if ( name.StartsWith( "menuHelp" ) ) {
                    group = "listGroupHelp";
                } else {
                    group = "listGroupOther";
                }
#if DEBUG
                PortUtil.println( "FormShortcutKeys#UpdateList; name=" + name + "; group=" + group );
#endif
                list.addItem( group, item );
            }
            updateColor();
            applyLanguage();
        }

        private void updateColor() {
            int num_groups = list.getGroupCount();
            for ( int k = 0; k < num_groups; k++ ) {
                String name = list.getGroupNameAt( k );
                for ( int i = 0; i < list.getItemCount( name ); i++ ) {
                    String compare = list.getItemAt( name, i ).getSubItemAt( 1 );
                    if ( compare.Equals( "" ) ) {
                        list.setItemBackColorAt( name, i, java.awt.Color.white );
                        continue;
                    }
                    boolean found = false;
                    for ( int n = 0; n < num_groups; n++ ) {
                        String search_name = list.getGroupNameAt( n );
                        for ( int j = 0; j < list.getItemCount( search_name ); j++ ) {
                            if ( n == k && i == j ) {
                                continue;
                            }
                            if ( compare.Equals( list.getItemAt( search_name, j ).getSubItemAt( 1 ) ) ) {
                                found = true;
                                break;
                            }
                        }
                        if ( found ) {
                            break;
                        }
                    }
                    if ( found ) {
                        list.setItemBackColorAt( name, i, java.awt.Color.yellow );
                    } else {
                        list.setItemBackColorAt( name, i, java.awt.Color.white );
                    }
                }
            }
        }

        private void registerEventHandlers() {
#if JAVA
#else
            this.list.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.list_PreviewKeyDown );
            this.list.KeyDown += new System.Windows.Forms.KeyEventHandler( this.list_KeyDown );
#endif
            btnLoadDefault.Click += new EventHandler( btnLoadDefault_Click );
            btnRevert.Click += new EventHandler( btnRevert_Click );
            FormClosing += new System.Windows.Forms.FormClosingEventHandler( FormShortcutKeys_FormClosing );
            btnOK.Click += new EventHandler( btnOK_Click );
            btnCancel.Click += new EventHandler( btnCancel_Click );
        }

        private void setResources() {
        }
        #endregion

        #region event handlers
        public void list_PreviewKeyDown( Object sender, BPreviewKeyDownEventArgs e ) {
        }

        public void list_KeyDown( Object sender, BKeyEventArgs e ) {
            String selected_group = "";
            int selected_index = -1;
            int num_groups = list.getGroupCount();
            for ( int i = 0; i < num_groups; i++ ) {
                String name = list.getGroupNameAt( i );
                int indx = list.getSelectedIndex( name );
                if ( indx >= 0 ) {
                    selected_group = name;
                    selected_index = indx;
                    break;
                }
            }

            if ( selected_index < 0 ) {
                return;
            }
            int index = selected_index;
#if JAVA
            KeyStroke stroke = KeyStroke.getKeyStroke( e.getKeyCode(), e.getModifiers() );
#else
            KeyStroke stroke = KeyStroke.getKeyStroke( 0, 0 );
            stroke.keys = e.KeyCode | e.Modifiers;
#endif
            int code = stroke.getKeyCode();
            int modifier = stroke.getModifiers();

            Vector<BKeys> capturelist = new Vector<BKeys>();
            BKeys capture = BKeys.None;
            for ( Iterator<BKeys> itr = AppManager.SHORTCUT_ACCEPTABLE.iterator(); itr.hasNext(); ) {
                BKeys k = itr.next();
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

            BListViewItem item = list.getItemAt( selected_group, index );
            item.setSubItemAt( 1, Utility.getShortcutDisplayString( capturelist.toArray( new BKeys[] { } ) ) );
            list.setItemAt( selected_group, index, item );
            String display = list.getItemAt( selected_group, index ).getSubItemAt( 0 );
            if ( mDict.containsKey( display ) ) {
                mDict.get( display ).setValue( capturelist.toArray( new BKeys[] { } ) );
            }
            updateColor();
#if !JAVA
            e.Handled = true;
#endif
        }

        public void btnRevert_Click( Object sender, BEventArgs e ) {
            copyDict( mFirstDict, mDict );
            updateList();
        }

        public void btnLoadDefault_Click( Object sender, BEventArgs e ) {
            for ( int i = 0; i < EditorConfig.DEFAULT_SHORTCUT_KEYS.size(); i++ ) {
                String name = EditorConfig.DEFAULT_SHORTCUT_KEYS.get( i ).Key;
                BKeys[] keys = EditorConfig.DEFAULT_SHORTCUT_KEYS.get( i ).Value;
                for ( Iterator<String> itr = mDict.keySet().iterator(); itr.hasNext(); ) {
                    String display = itr.next();
                    if ( name.Equals( mDict.get( display ).getKey() ) ) {
                        mDict.get( display ).setValue( keys );
                        break;
                    }
                }
            }
            updateList();
        }

        public void FormShortcutKeys_FormClosing( Object sender, BFormClosingEventArgs e ) {
            mColumnWidthCommand = list.getColumnWidth( 0 );
            mColumnWidthShortcutKey = list.getColumnWidth( 1 );
#if DEBUG
            PortUtil.println( "FormShortCurKeys#FormShortcutKeys_FormClosing; columnWidthCommand,columnWidthShortcutKey=" + mColumnWidthCommand + "," + mColumnWidthShortcutKey );
#endif
        }

        public void btnCancel_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }

        public void btnOK_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }
        #endregion

        #region UI implementation
#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/Cadencii/FormShortcutKeys.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/Cadencii/FormShortcutKeys.java
        #endregion
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
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.list = new org.kbinani.windows.forms.BListView();
            this.btnLoadDefault = new org.kbinani.windows.forms.BButton();
            this.btnRevert = new org.kbinani.windows.forms.BButton();
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
            this.list.FullRowSelect = true;
            this.list.Location = new System.Drawing.Point( 12, 12 );
            this.list.MultiSelect = false;
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size( 388, 343 );
            this.list.TabIndex = 9;
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.Details;
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
        private BButton btnLoadDefault;
        private BButton btnRevert;
        private System.Windows.Forms.ToolTip toolTip;
        #endregion
#endif
        #endregion

    }

#if !JAVA
}
#endif
