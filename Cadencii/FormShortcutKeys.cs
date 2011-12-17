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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/cadencii/FormShortcutKeys.java

import java.awt.event.*;
import java.util.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using com.github.cadencii.apputil;
using com.github.cadencii;
using com.github.cadencii.java.awt.event_;
using com.github.cadencii.java.util;
using com.github.cadencii.javax.swing;
using com.github.cadencii.windows.forms;

namespace com.github.cadencii
{
    using BEventArgs = System.EventArgs;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
    using BKeyEventArgs = System.Windows.Forms.KeyEventArgs;
    using BEventHandler = System.EventHandler;
    using boolean = System.Boolean;
    using BPreviewKeyDownEventArgs = System.Windows.Forms.PreviewKeyDownEventArgs;
    using BFormClosingEventHandler = System.Windows.Forms.FormClosingEventHandler;
    using BKeyEventHandler = System.Windows.Forms.KeyEventHandler;
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
        private static int mColumnWidthCommand = 272;
        private static int mColumnWidthShortcutKey = 177;
        private static int mWindowWidth = 541;
        private static int mWindowHeight = 572;

        private TreeMap<String, ValuePair<String, BKeys[]>> mDict;
        private TreeMap<String, ValuePair<String, BKeys[]>> mFirstDict;
        private Vector<String> mFieldName = new Vector<String>();
        private FormMain mMainForm = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dict">メニューアイテムの表示文字列をキーとする，メニューアイテムのフィールド名とショートカットキーのペアを格納したマップ</param>
        public FormShortcutKeys( TreeMap<String, ValuePair<String, BKeys[]>> dict, FormMain main_form )
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
            list.setColumnHeaders( new String[] { _( "Command" ), _( "Shortcut Key" ) } );
            list.setColumnWidth( 0, mColumnWidthCommand );
            list.setColumnWidth( 1, mColumnWidthShortcutKey );

            applyLanguage();
            setResources();

            mDict = dict;
            comboCategory.setSelectedIndex( 0 );
            mFirstDict = new TreeMap<String, ValuePair<String, BKeys[]>>();
            copyDict( mDict, mFirstDict );

            comboEditKey.removeAllItems();
            comboEditKey.addItem( BKeys.None );
            // アルファベット順になるように一度配列に入れて並べ替える
            int size = AppManager.SHORTCUT_ACCEPTABLE.size();
            BKeys[] keys = new BKeys[size];
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
                            BKeys t = keys[i];
                            keys[i] = keys[j];
                            keys[j] = t;
                            changed = true;
                        }
                    }
                }
            }
            foreach( BKeys key in keys ){
                comboEditKey.addItem( key );
            }
            this.setSize( mWindowWidth, mWindowHeight );

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

            labelCategory.setText( _( "Category" ) );
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

            labelCommand.setText( _( "Command" ) );
            labelEdit.setText( _( "Edit" ) );
            labelEditKey.setText( _( "Key:" ) );
            labelEditModifier.setText( _( "Modifier:" ) );
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
            list.SelectedIndexChanged -= new BEventHandler( list_SelectedIndexChanged );
            list.clear();
            list.SelectedIndexChanged += new BEventHandler( list_SelectedIndexChanged );
            mFieldName.clear();

            // 現在のカテゴリーを取得
            int selected = comboCategory.getSelectedIndex();
            if ( selected < 0 ) {
                selected = 0;
            }
            String category = mCategories[selected];

            // 現在のカテゴリーに合致するものについてのみ，リストに追加
            for ( Iterator<String> itr = mDict.keySet().iterator(); itr.hasNext(); ) {
                String display = itr.next();
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
                     list.addRow( new String[] { display, Utility.getShortcutDisplayString( keys ) } );
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
            //this.list.KeyDown += new BKeyEventHandler( list_KeyDown );
            btnLoadDefault.Click += new BEventHandler( btnLoadDefault_Click );
            btnRevert.Click += new BEventHandler( btnRevert_Click );
            this.FormClosing += new BFormClosingEventHandler( FormShortcutKeys_FormClosing );
            btnOK.Click += new BEventHandler( btnOK_Click );
            btnCancel.Click += new BEventHandler( btnCancel_Click );
            comboCategory.SelectedIndexChanged += new BEventHandler( comboCategory_SelectedIndexChanged );
            list.SelectedIndexChanged += new BEventHandler( list_SelectedIndexChanged );
            this.SizeChanged += new BEventHandler( FormShortcutKeys_SizeChanged );
            reRegisterHandlers();
        }

        private void unRegisterHandlers()
        {
            comboEditKey.SelectedIndexChanged -= new BEventHandler( comboEditKey_SelectedIndexChanged );
            checkCommand.CheckedChanged -= new BEventHandler( handleModifier_CheckedChanged );
            checkShift.CheckedChanged -= new BEventHandler( handleModifier_CheckedChanged );
            checkControl.CheckedChanged -= new BEventHandler( handleModifier_CheckedChanged );
            checkOption.CheckedChanged -= new BEventHandler( handleModifier_CheckedChanged );
        }
        
        private void reRegisterHandlers()
        {
            comboEditKey.SelectedIndexChanged += new BEventHandler( comboEditKey_SelectedIndexChanged );
            checkCommand.CheckedChanged += new BEventHandler( handleModifier_CheckedChanged );
            checkShift.CheckedChanged += new BEventHandler( handleModifier_CheckedChanged );
            checkControl.CheckedChanged += new BEventHandler( handleModifier_CheckedChanged );
            checkOption.CheckedChanged += new BEventHandler( handleModifier_CheckedChanged );
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void FormShortcutKeys_SizeChanged( Object sender, BEventArgs e )
        {
            mWindowWidth = getWidth();
            mWindowHeight = getHeight();
        }
        
        public void handleModifier_CheckedChanged( Object sender, BEventArgs e )
        {
            updateSelectionKeys();
        }

        public void comboEditKey_SelectedIndexChanged( Object sender, BEventArgs e )
        {
            updateSelectionKeys();
        }
        
        /// <summary>
        /// 現在選択中のコマンドのショートカットキーを，comboEditKey, 
        /// checkCommand, checkShift, checkControl, checkControlの状態にあわせて変更します．
        /// </summary>
        private void updateSelectionKeys()
        {
            int indx = comboEditKey.getSelectedIndex();
            if( indx < 0 ){
                return;
            }
            int indx_row = list.getSelectedRow();
            if( indx_row < 0 ){
                return;
            }
            BKeys key = (BKeys)comboEditKey.getItemAt( indx );
            String display = list.getItemAt( indx_row, 0 );
            if ( !mDict.containsKey( display ) ) {
                return;
            }
            Vector<BKeys> capturelist = new Vector<BKeys>();
            if( key != BKeys.None ){
                capturelist.add( key );
                if( checkCommand.isSelected() ){
                    capturelist.add( BKeys.Menu );
                }
                if( checkShift.isSelected() ){
                    capturelist.add( BKeys.Shift );
                }
                if( checkControl.isSelected() ){
                    capturelist.add( BKeys.Control );
                }
                if( checkOption.isSelected() ){
                    capturelist.add( BKeys.Alt );
                }
            }
            BKeys[] keys = capturelist.toArray( new BKeys[] { } );
            mDict.get( display ).setValue( keys );
            list.setItemAt( indx_row, 1, Utility.getShortcutDisplayString( keys ) ); 
        } 

        public void list_SelectedIndexChanged( Object sender, BEventArgs e )
        {
            int indx = list.getSelectedRow();
            if( indx < 0 ){
                return;
            }
            String display = list.getItemAt( indx, 0 );
            if( !mDict.containsKey( display ) ){
                return;
            }
            unRegisterHandlers();
            ValuePair<String, BKeys[]> item = mDict.get( display );
            BKeys[] keys = item.getValue();
            Vector<BKeys> vkeys = new Vector<BKeys>( Arrays.asList( keys ) );
            checkCommand.setSelected( vkeys.contains( BKeys.Menu ) );
            checkShift.setSelected( vkeys.contains( BKeys.Shift ) );
            checkControl.setSelected( vkeys.contains( BKeys.Control ) );
            checkOption.setSelected( vkeys.contains( BKeys.Alt ) );
            int size = comboEditKey.getItemCount();
            comboEditKey.setSelectedIndex( -1 );
            for( int i = 0; i < size; i++ ){
                BKeys k = (BKeys)comboEditKey.getItemAt( i );
                if( vkeys.contains( k ) ){
                    comboEditKey.setSelectedIndex( i );
                    break;
                }
            }
            reRegisterHandlers();
        }
        
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

        public void btnRevert_Click( Object sender, BEventArgs e )
        {
            copyDict( mFirstDict, mDict );
            updateList();
        }

        public void btnLoadDefault_Click( Object sender, BEventArgs e )
        {
            Vector<ValuePairOfStringArrayOfKeys> defaults = mMainForm.getDefaultShortcutKeys();
            for ( int i = 0; i < defaults.size(); i++ ) {
                String name = defaults.get( i ).Key;
                BKeys[] keys = defaults.get( i ).Value;
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
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/cadencii/FormShortcutKeys.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/cadencii/FormShortcutKeys.java
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
            this.btnCancel = new com.github.cadencii.windows.forms.BButton();
            this.btnOK = new com.github.cadencii.windows.forms.BButton();
            this.list = new com.github.cadencii.windows.forms.BListView();
            this.btnLoadDefault = new com.github.cadencii.windows.forms.BButton();
            this.btnRevert = new com.github.cadencii.windows.forms.BButton();
            this.toolTip = new System.Windows.Forms.ToolTip( this.components );
            this.labelCategory = new com.github.cadencii.windows.forms.BLabel();
            this.comboCategory = new com.github.cadencii.windows.forms.BComboBox();
            this.labelCommand = new com.github.cadencii.windows.forms.BLabel();
            this.labelEdit = new com.github.cadencii.windows.forms.BLabel();
            this.labelEditKey = new com.github.cadencii.windows.forms.BLabel();
            this.labelEditModifier = new com.github.cadencii.windows.forms.BLabel();
            this.comboEditKey = new com.github.cadencii.windows.forms.BComboBox();
            this.checkCommand = new com.github.cadencii.windows.forms.BCheckBox();
            this.checkShift = new com.github.cadencii.windows.forms.BCheckBox();
            this.checkControl = new com.github.cadencii.windows.forms.BCheckBox();
            this.checkOption = new com.github.cadencii.windows.forms.BCheckBox();
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

        private BButton btnCancel;
        private BButton btnOK;
        private BListView list;
        private BButton btnLoadDefault;
        private BButton btnRevert;
        private System.Windows.Forms.ToolTip toolTip;
        private BLabel labelCategory;
        private BComboBox comboCategory;
        private com.github.cadencii.windows.forms.BLabel labelCommand;
        private com.github.cadencii.windows.forms.BLabel labelEdit;
        private com.github.cadencii.windows.forms.BLabel labelEditKey;
        private com.github.cadencii.windows.forms.BLabel labelEditModifier;
        private com.github.cadencii.windows.forms.BComboBox comboEditKey;
        private BCheckBox checkCommand;
        private BCheckBox checkShift;
        private BCheckBox checkControl;
        private BCheckBox checkOption;

        #endregion
#endif
        #endregion

    }

#if !JAVA
}
#endif
