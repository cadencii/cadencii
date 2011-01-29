/*
 * FormShortcutKeys.cs
 * Copyright © 2009-2011 kbinani
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

namespace org.kbinani.cadencii
{
    using BEventArgs = System.EventArgs;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
    using BKeyEventArgs = System.Windows.Forms.KeyEventArgs;
    using BEventHandler = System.EventHandler;
    using boolean = System.Boolean;
    using BPreviewKeyDownEventArgs = System.Windows.Forms.PreviewKeyDownEventArgs;
    using BFormClosingEventHandler = System.Windows.Forms.FormClosingEventHandler;
    using BKeyEventHandler = System.Windows.Forms.KeyEventHandler;
    using java = org.kbinani.java;
#endif

#if JAVA
    public class FormShortcutKeys extends BDialog {
#else
    public class FormShortcutKeys : BDialog
    {
#endif
        /// <summary>
        /// カテゴリーのリスト
        /// </summary>
        private static readonly String[] mCategories = new String[]{
            "menuFile", "menuEdit", "menuVisual", "menuJob", "menuLyric", "menuTrack",
            "menuScript", "menuSetting", "menuHelp", ".other" };
        private static int mColumnWidthCommand = 240;
        private static int mColumnWidthShortcutKey = 140;

        private TreeMap<String, ValuePair<String, BKeys[]>> mDict;
        private TreeMap<String, ValuePair<String, BKeys[]>> mFirstDict;
        private Vector<String> mFieldName = new Vector<String>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dict">メニューアイテムの表示文字列をキーとする，メニューアイテムのフィールド名とショートカットキーのペアを格納したマップ</param>
        public FormShortcutKeys( TreeMap<String, ValuePair<String, BKeys[]>> dict )
        {
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
                serr.println( "FormShortcutKeys#.ctor; ex=" + ex );
#endif
            }

#if DEBUG
            sout.println( "FormShortcutKeys#.ctor; dict.size()=" + dict.size() );
#endif
            list.setColumnHeaders( new String[] { "Command", "Shortcut Key" } );
            list.setColumnWidth( 0, mColumnWidthCommand );
            list.setColumnWidth( 1, mColumnWidthShortcutKey );

            setResources();
            applyLanguage();
#if DEBUG
            sout.println( "FormShortcutKeys#.ctor; dict=" );
            for ( Iterator<String> itr = dict.keySet().iterator(); itr.hasNext(); ) {
                String disp = itr.next();
                sout.println( "    disp=" + disp );
            }
#endif
            mDict = dict;
            comboCategory.setSelectedIndex( 0 );
            mFirstDict = new TreeMap<String, ValuePair<String, BKeys[]>>();
            copyDict( mDict, mFirstDict );

            registerEventHandlers();
            updateList();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        #region public methods
        public void applyLanguage()
        {
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

            labelCategory.setText( _( "Select category of menu" ) );
            int selected = comboCategory.getSelectedIndex();
            comboCategory.removeAllItems();
            foreach ( String category in mCategories ) {
                String c = category;
                if ( str.compare( category, "menuFile" ) ) {
                    c = _( "File" );
                } else if ( str.compare( category, "menuEdit" ) ) {
                    c = _( "Edit" );
                } else if ( str.compare( category, "menuVisual" ) ) {
                    c = _( "Visual" );
                } else if ( str.compare( category, "menuJob" ) ) {
                    c = _( "Job" );
                } else if ( str.compare( category, "menuLyric" ) ) {
                    c = _( "Lyric" );
                } else if ( str.compare( category, "menuTrack" ) ) {
                    c = _( "Track" );
                } else if ( str.compare( category, "menuScript" ) ) {
                    c = _( "Script" );
                } else if ( str.compare( category, "menuSetting" ) ){
                    c = _( "Setting" );
                } else if ( str.compare( category, "menuHelp" ) ) {
                    c = _( "Help" );
                } else {
                    c = _( "Other" );
                }
                comboCategory.addItem( c );
            }
            if ( comboCategory.getItemCount() <= selected ) {
                selected = comboCategory.getItemCount() - 1;
            }
            comboCategory.setSelectedIndex( selected );
        }

        public TreeMap<String, ValuePair<String, BKeys[]>> getResult()
        {
            TreeMap<String, ValuePair<String, BKeys[]>> ret = new TreeMap<String, ValuePair<String, BKeys[]>>();
            copyDict( mDict, ret );
            return ret;
        }
        #endregion

        #region helper methods
        private static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        private static void copyDict( TreeMap<String, ValuePair<String, BKeys[]>> src, TreeMap<String, ValuePair<String, BKeys[]>> dest )
        {
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

        /// <summary>
        /// リストを更新します
        /// </summary>
        private void updateList()
        {
            list.clear();
            mFieldName.clear();

            // 現在のカテゴリーを取得
            int selected = comboCategory.getSelectedIndex();
            if ( selected < 0 ) {
                selected = 0;
            }
            String category = mCategories[selected];

            // 現在のカテゴリーに合致するものについてのみ，リストに追加
#if DEBUG
            sout.println( "FormShortcutKey#updateList; mDict=" );
#endif
            for ( Iterator<String> itr = mDict.keySet().iterator(); itr.hasNext(); ) {
                String display = itr.next();
#if DEBUG
                sout.println( "  display=" + display );
#endif
                ValuePair<String, BKeys[]> item = mDict.get( display );
                String field_name = item.getKey();
                BKeys[] keys = item.getValue();
                boolean add_this_one = false;
                if ( str.compare( category, ".other" ) ) {
                    add_this_one = true;
                    for ( int i = 0; i < mCategories.Length; i++ ) {
                        String c = mCategories[i];
                        if ( str.compare( c, ".other" ) ) {
                            continue;
                        }
                        if ( str.startsWith( field_name, c ) ) {
                            add_this_one = false;
                            break;
                        }
                    }
                } else {
                    if ( str.startsWith( field_name, category ) ) {
                        add_this_one = true;
                    }
                }
                if ( add_this_one ) {
                     list.addItem( new String[] { display, Utility.getShortcutDisplayString( keys ) } );
                     mFieldName.add( field_name );
                }
            }

            updateColor();
            //applyLanguage();
        }

        /// <summary>
        /// リストアイテムの背景色を更新します．
        /// 2つ以上のメニューに対して同じショートカットが割り当てられた場合に警告色で表示する．
        /// </summary>
        private void updateColor()
        {
            int size = list.getItemCountRow();
            for ( int i = 0; i < size; i++ ) {
                //BListViewItem list_item = list.getItemAt( i );
                String field_name = mFieldName.get( i );
                String key_display = list.getItemAt( i, 1 );
                if ( str.compare( key_display, "" ) ){
                    // ショートカットキーが割り当てられていないのでスルー
                    list.setRowBackColor( i, java.awt.Color.white );
                    continue;
                }

                boolean found = false;
                for ( Iterator<String> itr = mDict.keySet().iterator(); itr.hasNext(); ) {
                    String display1 = itr.next();
                    ValuePair<String, BKeys[]> item1 = mDict.get( display1 );
                    String field_name1 = item1.getKey();
                    if ( str.compare( field_name, field_name1 ) ) {
                        // 自分自身なのでスルー
                        continue;
                    }
                    BKeys[] keys1 = item1.getValue();
                    String key_display1 = Utility.getShortcutDisplayString( keys1 );
                    if ( str.compare( key_display, key_display1 ) ) {
                        // 同じキーが割り当てられてる！！
                        found = true;
                        break;
                    }
                }

                // 背景色を変える
                if ( found ) {
                    list.setRowBackColor( i, java.awt.Color.yellow );
                } else {
                    list.setRowBackColor(  i, java.awt.Color.white );
                }
            }
        }

        private void registerEventHandlers()
        {
            this.list.KeyDown += new BKeyEventHandler( list_KeyDown );
            btnLoadDefault.Click += new BEventHandler( btnLoadDefault_Click );
            btnRevert.Click += new BEventHandler( btnRevert_Click );
            this.FormClosing += new BFormClosingEventHandler( FormShortcutKeys_FormClosing );
            btnOK.Click += new BEventHandler( btnOK_Click );
            btnCancel.Click += new BEventHandler( btnCancel_Click );
            comboCategory.SelectedIndexChanged += new BEventHandler( comboCategory_SelectedIndexChanged );
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void comboCategory_SelectedIndexChanged( Object sender, EventArgs e )
        {
            int selected = comboCategory.getSelectedIndex();
#if DEBUG
            sout.println( "FormShortcutKeys#comboCategory_selectedIndexChanged; selected=" + selected );
#endif
            if ( selected < 0 ) {
                comboCategory.setSelectedIndex( 0 );
                //updateList();
                return;
            }
            updateList();
        }

        public void list_KeyDown( Object sender, BKeyEventArgs e )
        {
            int index = list.getSelectedRow();
            if ( index < 0 ) {
                return;
            }
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
                    if ( (modifier & InputEvent.META_MASK) == InputEvent.META_MASK ) {
                        capturelist.add( BKeys.Menu );
                    }
                    capture = k;
                    break;
                }
            }

            //BListViewItem item = list.getItemAt( index );
            list.setItemAt( index, 1, Utility.getShortcutDisplayString( capturelist.toArray( new BKeys[] { } ) ) );
            //list.setItemAt( index, item );
            String display = list.getItemAt( index, 0 );
            if ( mDict.containsKey( display ) ) {
                mDict.get( display ).setValue( capturelist.toArray( new BKeys[] { } ) );
            }
            updateColor();
#if !JAVA
            e.Handled = true;
#endif
        }

        public void btnRevert_Click( Object sender, BEventArgs e )
        {
            copyDict( mFirstDict, mDict );
            updateList();
        }

        public void btnLoadDefault_Click( Object sender, BEventArgs e )
        {
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

        public void FormShortcutKeys_FormClosing( Object sender, BFormClosingEventArgs e )
        {
            mColumnWidthCommand = list.getColumnWidth( 0 );
            mColumnWidthShortcutKey = list.getColumnWidth( 1 );
#if DEBUG
            sout.println( "FormShortCurKeys#FormShortcutKeys_FormClosing; columnWidthCommand,columnWidthShortcutKey=" + mColumnWidthCommand + "," + mColumnWidthShortcutKey );
#endif
        }

        public void btnCancel_Click( Object sender, BEventArgs e )
        {
            setDialogResult( BDialogResult.CANCEL );
        }

        public void btnOK_Click( Object sender, BEventArgs e )
        {
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
        protected override void Dispose( boolean disposing )
        {
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.list = new org.kbinani.windows.forms.BListView();
            this.btnLoadDefault = new org.kbinani.windows.forms.BButton();
            this.btnRevert = new org.kbinani.windows.forms.BButton();
            this.toolTip = new System.Windows.Forms.ToolTip( this.components );
            this.labelCategory = new org.kbinani.windows.forms.BLabel();
            this.comboCategory = new org.kbinani.windows.forms.BComboBox();
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
            this.list.Location = new System.Drawing.Point( 12, 53 );
            this.list.MultiSelect = false;
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size( 388, 302 );
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
            // labelCategory
            // 
            this.labelCategory.AutoSize = true;
            this.labelCategory.Location = new System.Drawing.Point( 12, 12 );
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size( 130, 12 );
            this.labelCategory.TabIndex = 12;
            this.labelCategory.Text = "Select category of menu";
            // 
            // comboCategory
            // 
            this.comboCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.Location = new System.Drawing.Point( 12, 27 );
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size( 388, 20 );
            this.comboCategory.TabIndex = 13;
            // 
            // FormShortcutKeys
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 412, 438 );
            this.Controls.Add( this.comboCategory );
            this.Controls.Add( this.labelCategory );
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
            this.PerformLayout();

        }

        #endregion

        private BButton btnCancel;
        private BButton btnOK;
        private BListView list;
        private BButton btnLoadDefault;
        private BButton btnRevert;
        private System.Windows.Forms.ToolTip toolTip;
        private BLabel labelCategory;
        private BComboBox comboCategory;

        #endregion
#endif
        #endregion

    }

#if !JAVA
}
#endif
