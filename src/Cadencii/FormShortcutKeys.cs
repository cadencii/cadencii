/*
 * FormShortcutKeys.cs
 * Copyright © 2009-2011 kbinani
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
#if JAVA
package cadencii;

//INCLUDE-SECTION IMPORT ./ui/java/FormShortcutKeys.java

import java.awt.event.*;
import java.util.*;
import javax.swing.*;
import cadencii.*;
import cadencii.apputil.*;
import cadencii.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using cadencii.apputil;
using cadencii;
using cadencii.java.util;
using cadencii.windows.forms;

namespace cadencii
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormShortcutKeys extends BDialog {
#else
    public class FormShortcutKeys : Form
    {
#endif
        /// <summary>
        /// カテゴリーのリスト
        /// </summary>
        private static readonly String[] mCategories = new String[]{
            "menuFile", "menuEdit", "menuVisual", "menuJob", "menuLyric", "menuTrack",
            "menuScript", "menuSetting", "menuHelp", ".other" };
        private static int mColumnWidthCommand = 272;
        private static int mColumnWidthShortcutKey = 177;
        private static int mWindowWidth = 541;
        private static int mWindowHeight = 572;

        private TreeMap<String, ValuePair<String, Keys[]>> mDict;
        private TreeMap<String, ValuePair<String, Keys[]>> mFirstDict;
        private Vector<String> mFieldName = new Vector<String>();
        private FormMain mMainForm = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dict">メニューアイテムの表示文字列をキーとする，メニューアイテムのフィールド名とショートカットキーのペアを格納したマップ</param>
        public FormShortcutKeys( TreeMap<String, ValuePair<String, Keys[]>> dict, FormMain main_form )
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
            sout.println( "FormShortcutKeys#.ctor; mColumnWidthCommand=" + mColumnWidthCommand + "; mColumnWidthShortcutKey=" + mColumnWidthShortcutKey );
#endif
            mMainForm = main_form;
            list.SetColumnHeaders( new String[] { _( "Command" ), _( "Shortcut Key" ) } );
            list.Columns[0].Width = mColumnWidthCommand;
            list.Columns[1].Width = mColumnWidthShortcutKey;

            applyLanguage();
            setResources();

            mDict = dict;
            comboCategory.SelectedIndex = 0;
            mFirstDict = new TreeMap<String, ValuePair<String, Keys[]>>();
            copyDict( mDict, mFirstDict );

            comboEditKey.Items.Clear();
            comboEditKey.Items.Add( Keys.None );
            // アルファベット順になるように一度配列に入れて並べ替える
            int size = AppManager.SHORTCUT_ACCEPTABLE.size();
            Keys[] keys = new Keys[size];
            for ( int i = 0; i < size; i++ ){
                keys[i] = AppManager.SHORTCUT_ACCEPTABLE.get( i );
            }
            boolean changed = true;
            while( changed ){
                changed = false;
                for( int i = 0; i < size - 1; i++ ){
                    for( int j = i + 1; j < size; j++ ){
                        String itemi = keys[i] + "";
                        String itemj = keys[j] + "";
#if JAVA
                        if( itemi.compareTo( itemj ) > 0 ){
#else
                        if( itemi.CompareTo( itemj ) > 0 ){
#endif
                            Keys t = keys[i];
                            keys[i] = keys[j];
                            keys[j] = t;
                            changed = true;
                        }
                    }
                }
            }
            foreach( Keys key in keys ){
                comboEditKey.Items.Add( key );
            }
            this.Size = new System.Drawing.Size( mWindowWidth, mWindowHeight );

            registerEventHandlers();
            updateList();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        #region public methods
        public void applyLanguage()
        {
            this.Text = _( "Shortcut Config" );

            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
            btnRevert.Text = _( "Revert" );
            btnLoadDefault.Text = _( "Load Default" );

            list.SetColumnHeaders( new String[] { _( "Command" ), _( "Shortcut Key" ) } );

            labelCategory.Text = _( "Category" );
            int selected = comboCategory.SelectedIndex;
            comboCategory.Items.Clear();
            foreach ( String category in mCategories ) {
                String c = category;
                if ( category == "menuFile" ) {
                    c = _( "File" );
                } else if ( category == "menuEdit" ) {
                    c = _( "Edit" );
                } else if ( category == "menuVisual" ) {
                    c = _( "Visual" );
                } else if ( category == "menuJob" ) {
                    c = _( "Job" );
                } else if ( category == "menuLyric" ) {
                    c = _( "Lyric" );
                } else if ( category == "menuTrack" ) {
                    c = _( "Track" );
                } else if ( category == "menuScript" ) {
                    c = _( "Script" );
                } else if ( category == "menuSetting" ){
                    c = _( "Setting" );
                } else if ( category == "menuHelp" ) {
                    c = _( "Help" );
                } else {
                    c = _( "Other" );
                }
                comboCategory.Items.Add( c );
            }
            if ( comboCategory.Items.Count <= selected ) {
                selected = comboCategory.Items.Count - 1;
            }
            comboCategory.SelectedIndex = selected;

            labelCommand.Text = _( "Command" );
            labelEdit.Text = _( "Edit" );
            labelEditKey.Text = _( "Key:" );
            labelEditModifier.Text = _( "Modifier:" );
        }

        public TreeMap<String, ValuePair<String, Keys[]>> getResult()
        {
            TreeMap<String, ValuePair<String, Keys[]>> ret = new TreeMap<String, ValuePair<String, Keys[]>>();
            copyDict( mDict, ret );
            return ret;
        }
        #endregion

        #region helper methods
        private static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        private static void copyDict( TreeMap<String, ValuePair<String, Keys[]>> src, TreeMap<String, ValuePair<String, Keys[]>> dest )
        {
            dest.clear();
            for ( Iterator<String> itr = src.keySet().iterator(); itr.hasNext(); ) {
                String name = itr.next();
                String key = src.get( name ).getKey();
                Keys[] values = src.get( name ).getValue();
                Vector<Keys> cp = new Vector<Keys>();
                foreach ( Keys k in values ) {
                    cp.add( k );
                }
                dest.put( name, new ValuePair<String, Keys[]>( key, cp.toArray( new Keys[] { } ) ) );
            }
        }

        /// <summary>
        /// リストを更新します
        /// </summary>
        private void updateList()
        {
            list.SelectedIndexChanged -= new EventHandler( list_SelectedIndexChanged );
            list.Items.Clear();
            list.SelectedIndexChanged += new EventHandler( list_SelectedIndexChanged );
            mFieldName.clear();

            // 現在のカテゴリーを取得
            int selected = comboCategory.SelectedIndex;
            if ( selected < 0 ) {
                selected = 0;
            }
            String category = mCategories[selected];

            // 現在のカテゴリーに合致するものについてのみ，リストに追加
            for ( Iterator<String> itr = mDict.keySet().iterator(); itr.hasNext(); ) {
                String display = itr.next();
                ValuePair<String, Keys[]> item = mDict.get( display );
                String field_name = item.getKey();
                Keys[] keys = item.getValue();
                boolean add_this_one = false;
                if ( category == ".other" ) {
                    add_this_one = true;
                    for ( int i = 0; i < mCategories.Length; i++ ) {
                        String c = mCategories[i];
                        if ( c == ".other" ) {
                            continue;
                        }
                        if ( field_name.StartsWith( c ) ) {
                            add_this_one = false;
                            break;
                        }
                    }
                } else {
                    if ( field_name.StartsWith( category ) ) {
                        add_this_one = true;
                    }
                }
                if ( add_this_one ) {
                     list.AddRow( new String[] { display, Utility.getShortcutDisplayString( keys ) } );
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
            int size = list.Items.Count;
            for ( int i = 0; i < size; i++ ) {
                //BListViewItem list_item = list.getItemAt( i );
                String field_name = mFieldName.get( i );
                String key_display = list.Items[i].SubItems[1].Text;
                if ( key_display == "" ){
                    // ショートカットキーが割り当てられていないのでスルー
                    list.Items[i].BackColor = System.Drawing.Color.White;
                    continue;
                }

                boolean found = false;
                for ( Iterator<String> itr = mDict.keySet().iterator(); itr.hasNext(); ) {
                    String display1 = itr.next();
                    ValuePair<String, Keys[]> item1 = mDict.get( display1 );
                    String field_name1 = item1.getKey();
                    if ( field_name == field_name1 ) {
                        // 自分自身なのでスルー
                        continue;
                    }
                    Keys[] keys1 = item1.getValue();
                    String key_display1 = Utility.getShortcutDisplayString( keys1 );
                    if ( key_display == key_display1 ) {
                        // 同じキーが割り当てられてる！！
                        found = true;
                        break;
                    }
                }

                // 背景色を変える
                if ( found ) {
                    list.Items[i].BackColor = System.Drawing.Color.Yellow;
                } else {
                    list.Items[i].BackColor = System.Drawing.Color.White;
                }
            }
        }

        private void registerEventHandlers()
        {
            btnLoadDefault.Click += new EventHandler( btnLoadDefault_Click );
            btnRevert.Click += new EventHandler( btnRevert_Click );
            this.FormClosing += new FormClosingEventHandler( FormShortcutKeys_FormClosing );
            btnOK.Click += new EventHandler( btnOK_Click );
            btnCancel.Click += new EventHandler( btnCancel_Click );
            comboCategory.SelectedIndexChanged += new EventHandler( comboCategory_SelectedIndexChanged );
            list.SelectedIndexChanged += new EventHandler( list_SelectedIndexChanged );
            this.SizeChanged += new EventHandler( FormShortcutKeys_SizeChanged );
            reRegisterHandlers();
        }

        private void unRegisterHandlers()
        {
            comboEditKey.SelectedIndexChanged -= new EventHandler( comboEditKey_SelectedIndexChanged );
            checkCommand.CheckedChanged -= new EventHandler( handleModifier_CheckedChanged );
            checkShift.CheckedChanged -= new EventHandler( handleModifier_CheckedChanged );
            checkControl.CheckedChanged -= new EventHandler( handleModifier_CheckedChanged );
            checkOption.CheckedChanged -= new EventHandler( handleModifier_CheckedChanged );
        }
        
        private void reRegisterHandlers()
        {
            comboEditKey.SelectedIndexChanged += new EventHandler( comboEditKey_SelectedIndexChanged );
            checkCommand.CheckedChanged += new EventHandler( handleModifier_CheckedChanged );
            checkShift.CheckedChanged += new EventHandler( handleModifier_CheckedChanged );
            checkControl.CheckedChanged += new EventHandler( handleModifier_CheckedChanged );
            checkOption.CheckedChanged += new EventHandler( handleModifier_CheckedChanged );
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void FormShortcutKeys_SizeChanged( Object sender, EventArgs e )
        {
            mWindowWidth = this.Width;
            mWindowHeight = this.Height;
        }
        
        public void handleModifier_CheckedChanged( Object sender, EventArgs e )
        {
            updateSelectionKeys();
        }

        public void comboEditKey_SelectedIndexChanged( Object sender, EventArgs e )
        {
            updateSelectionKeys();
        }
        
        /// <summary>
        /// 現在選択中のコマンドのショートカットキーを，comboEditKey, 
        /// checkCommand, checkShift, checkControl, checkControlの状態にあわせて変更します．
        /// </summary>
        private void updateSelectionKeys()
        {
            int indx = comboEditKey.SelectedIndex;
            if( indx < 0 ){
                return;
            }
            if( list.SelectedIndices.Count == 0 ){
                return;
            }
            int indx_row = list.SelectedIndices[0];
            Keys key = (Keys)comboEditKey.Items[indx];
            String display = list.Items[indx_row].SubItems[0].Text;
            if ( !mDict.containsKey( display ) ) {
                return;
            }
            Vector<Keys> capturelist = new Vector<Keys>();
            if( key != Keys.None ){
                capturelist.add( key );
                if (checkCommand.Checked) {
                    capturelist.add( Keys.Menu );
                }
                if (checkShift.Checked) {
                    capturelist.add( Keys.Shift );
                }
                if( checkControl.Checked ){
                    capturelist.add( Keys.Control );
                }
                if( checkOption.Checked ){
                    capturelist.add( Keys.Alt );
                }
            }
            Keys[] keys = capturelist.toArray( new Keys[] { } );
            mDict.get( display ).setValue( keys );
            list.Items[indx_row].SubItems[1].Text = Utility.getShortcutDisplayString( keys ); 
        } 

        public void list_SelectedIndexChanged( Object sender, EventArgs e )
        {
            if( list.SelectedIndices.Count == 0 ){
                return;
            }
            int indx = list.SelectedIndices[0];
            String display = list.Items[indx].SubItems[0].Text;
            if( !mDict.containsKey( display ) ){
                return;
            }
            unRegisterHandlers();
            ValuePair<String, Keys[]> item = mDict.get( display );
            Keys[] keys = item.getValue();
            Vector<Keys> vkeys = new Vector<Keys>( keys );
            checkCommand.Checked = vkeys.contains( Keys.Menu );
            checkShift.Checked = vkeys.contains( Keys.Shift );
            checkControl.Checked = vkeys.contains( Keys.Control );
            checkOption.Checked = vkeys.contains( Keys.Alt );
            int size = comboEditKey.Items.Count;
            comboEditKey.SelectedIndex = -1;
            for( int i = 0; i < size; i++ ){
                Keys k = (Keys)comboEditKey.Items[i];
                if( vkeys.contains( k ) ){
                    comboEditKey.SelectedIndex = i;
                    break;
                }
            }
            reRegisterHandlers();
        }
        
        public void comboCategory_SelectedIndexChanged( Object sender, EventArgs e )
        {
            int selected = comboCategory.SelectedIndex;
#if DEBUG
            sout.println( "FormShortcutKeys#comboCategory_selectedIndexChanged; selected=" + selected );
#endif
            if ( selected < 0 ) {
                comboCategory.SelectedIndex = 0;
                //updateList();
                return;
            }
            updateList();
        }

        public void btnRevert_Click( Object sender, EventArgs e )
        {
            copyDict( mFirstDict, mDict );
            updateList();
        }

        public void btnLoadDefault_Click( Object sender, EventArgs e )
        {
            Vector<ValuePairOfStringArrayOfKeys> defaults = mMainForm.getDefaultShortcutKeys();
            for ( int i = 0; i < defaults.size(); i++ ) {
                String name = defaults.get( i ).Key;
                Keys[] keys = defaults.get( i ).Value;
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

        public void FormShortcutKeys_FormClosing( Object sender, FormClosingEventArgs e )
        {
            mColumnWidthCommand = list.Columns[0].Width;
            mColumnWidthShortcutKey = list.Columns[1].Width;
#if DEBUG
            sout.println( "FormShortCurKeys#FormShortcutKeys_FormClosing; columnWidthCommand,columnWidthShortcutKey=" + mColumnWidthCommand + "," + mColumnWidthShortcutKey );
#endif
        }

        public void btnCancel_Click( Object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public void btnOK_Click( Object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        #endregion

        #region UI implementation
#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ./ui/java/FormShortcutKeys.java
        //INCLUDE-SECTION METHOD ./ui/java/FormShortcutKeys.java
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
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.list = new ListView();
            this.btnLoadDefault = new Button();
            this.btnRevert = new Button();
            this.toolTip = new System.Windows.Forms.ToolTip( this.components );
            this.labelCategory = new Label();
            this.comboCategory = new System.Windows.Forms.ComboBox();
            this.labelCommand = new Label();
            this.labelEdit = new Label();
            this.labelEditKey = new Label();
            this.labelEditModifier = new Label();
            this.comboEditKey = new System.Windows.Forms.ComboBox();
            this.checkCommand = new CheckBox();
            this.checkShift = new CheckBox();
            this.checkControl = new CheckBox();
            this.checkOption = new CheckBox();
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
            this.list.Location = new System.Drawing.Point( 36, 76 );
            this.list.MultiSelect = false;
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size( 364, 182 );
            this.list.TabIndex = 9;
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.Details;
            // 
            // btnLoadDefault
            // 
            this.btnLoadDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadDefault.Location = new System.Drawing.Point( 113, 361 );
            this.btnLoadDefault.Name = "btnLoadDefault";
            this.btnLoadDefault.Size = new System.Drawing.Size( 122, 23 );
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
            this.labelCategory.Size = new System.Drawing.Size( 51, 12 );
            this.labelCategory.TabIndex = 12;
            this.labelCategory.Text = "Category";
            // 
            // comboCategory
            // 
            this.comboCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.Location = new System.Drawing.Point( 36, 27 );
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size( 364, 20 );
            this.comboCategory.TabIndex = 13;
            // 
            // labelCommand
            // 
            this.labelCommand.AutoSize = true;
            this.labelCommand.Location = new System.Drawing.Point( 12, 61 );
            this.labelCommand.Name = "labelCommand";
            this.labelCommand.Size = new System.Drawing.Size( 55, 12 );
            this.labelCommand.TabIndex = 14;
            this.labelCommand.Text = "Command";
            // 
            // labelEdit
            // 
            this.labelEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelEdit.AutoSize = true;
            this.labelEdit.Location = new System.Drawing.Point( 12, 276 );
            this.labelEdit.Name = "labelEdit";
            this.labelEdit.Size = new System.Drawing.Size( 25, 12 );
            this.labelEdit.TabIndex = 15;
            this.labelEdit.Text = "Edit";
            // 
            // labelEditKey
            // 
            this.labelEditKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelEditKey.AutoSize = true;
            this.labelEditKey.Location = new System.Drawing.Point( 34, 293 );
            this.labelEditKey.Name = "labelEditKey";
            this.labelEditKey.Size = new System.Drawing.Size( 26, 12 );
            this.labelEditKey.TabIndex = 16;
            this.labelEditKey.Text = "Key:";
            // 
            // labelEditModifier
            // 
            this.labelEditModifier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelEditModifier.AutoSize = true;
            this.labelEditModifier.Location = new System.Drawing.Point( 34, 317 );
            this.labelEditModifier.Name = "labelEditModifier";
            this.labelEditModifier.Size = new System.Drawing.Size( 48, 12 );
            this.labelEditModifier.TabIndex = 17;
            this.labelEditModifier.Text = "Modifier:";
            // 
            // comboEditKey
            // 
            this.comboEditKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboEditKey.FormattingEnabled = true;
            this.comboEditKey.Location = new System.Drawing.Point( 101, 290 );
            this.comboEditKey.Name = "comboEditKey";
            this.comboEditKey.Size = new System.Drawing.Size( 299, 20 );
            this.comboEditKey.TabIndex = 18;
            // 
            // checkCommand
            // 
            this.checkCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkCommand.AutoSize = true;
            this.checkCommand.Location = new System.Drawing.Point( 101, 316 );
            this.checkCommand.Name = "checkCommand";
            this.checkCommand.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 0 );
            this.checkCommand.Size = new System.Drawing.Size( 82, 16 );
            this.checkCommand.TabIndex = 153;
            this.checkCommand.Text = "command";
            this.checkCommand.UseVisualStyleBackColor = true;
            // 
            // checkShift
            // 
            this.checkShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkShift.AutoSize = true;
            this.checkShift.Location = new System.Drawing.Point( 189, 316 );
            this.checkShift.Name = "checkShift";
            this.checkShift.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 0 );
            this.checkShift.Size = new System.Drawing.Size( 57, 16 );
            this.checkShift.TabIndex = 154;
            this.checkShift.Text = "shift";
            this.checkShift.UseVisualStyleBackColor = true;
            // 
            // checkControl
            // 
            this.checkControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkControl.AutoSize = true;
            this.checkControl.Location = new System.Drawing.Point( 252, 316 );
            this.checkControl.Name = "checkControl";
            this.checkControl.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 0 );
            this.checkControl.Size = new System.Drawing.Size( 69, 16 );
            this.checkControl.TabIndex = 155;
            this.checkControl.Text = "control";
            this.checkControl.UseVisualStyleBackColor = true;
            // 
            // checkOption
            // 
            this.checkOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkOption.AutoSize = true;
            this.checkOption.Location = new System.Drawing.Point( 325, 316 );
            this.checkOption.Name = "checkOption";
            this.checkOption.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 0 );
            this.checkOption.Size = new System.Drawing.Size( 65, 16 );
            this.checkOption.TabIndex = 156;
            this.checkOption.Text = "option";
            this.checkOption.UseVisualStyleBackColor = true;
            // 
            // FormShortcutKeys
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 412, 438 );
            this.Controls.Add( this.checkOption );
            this.Controls.Add( this.checkControl );
            this.Controls.Add( this.checkShift );
            this.Controls.Add( this.checkCommand );
            this.Controls.Add( this.comboEditKey );
            this.Controls.Add( this.labelEditModifier );
            this.Controls.Add( this.labelEditKey );
            this.Controls.Add( this.labelEdit );
            this.Controls.Add( this.labelCommand );
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

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private ListView list;
        private System.Windows.Forms.Button btnLoadDefault;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.ToolTip toolTip;
        private Label labelCategory;
        private ComboBox comboCategory;
        private Label labelCommand;
        private Label labelEdit;
        private Label labelEditKey;
        private Label labelEditModifier;
        private ComboBox comboEditKey;
        private System.Windows.Forms.CheckBox checkCommand;
        private System.Windows.Forms.CheckBox checkShift;
        private System.Windows.Forms.CheckBox checkControl;
        private System.Windows.Forms.CheckBox checkOption;

        #endregion
#endif
        #endregion

    }

#if !JAVA
}
#endif
