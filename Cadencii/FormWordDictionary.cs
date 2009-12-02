/*
 * FormWordDictionary.cs
 * Copyright (c) 2008-2009 kbinani
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

//INCLUDE-SECTION IMPORT ..\BuildJavaUI\src\FormWordDictionary.java

import java.util.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.java.util;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormWordDictionary extends BForm{
#else
    class FormWordDictionary : BForm {
#endif
        public FormWordDictionary() {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            ApplyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage() {
            setTitle( _( "User Dictionary Configuration" ) );
            lblAvailableDictionaries.setText( _( "Available Dictionaries" ) );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
            btnUp.setText( _( "Up" ) );
            btnDown.setText( _( "Down" ) );
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void FormWordDictionary_Load( Object sender, BEventArgs e ) {
            listDictionaries.Items.Clear();
            for ( int i = 0; i < SymbolTable.getCount(); i++ ) {
                String name = SymbolTable.getSymbolTable( i ).getName();
                boolean enabled = SymbolTable.getSymbolTable( i ).isEnabled();
                BListViewItem item = new BListViewItem( new String[] { name } );
                listDictionaries.addItem( "", item, enabled );
            }
        }

        public Vector<ValuePair<String, Boolean>> getResult() {
            Vector<ValuePair<String, Boolean>> ret = new Vector<ValuePair<String, Boolean>>();
            int count = listDictionaries.getItemCount( "" );
            for ( int i = 0; i < count; i++ ) {
                BListViewItem item = listDictionaries.getItemAt( "", i );
                ret.add( new ValuePair<String, Boolean>( item.getSubItemAt( 0 ), listDictionaries.isItemCheckedAt( "", i ) ) );
            }
            return ret;
        }

        private void btnOK_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }

        private void btnUp_Click( Object sender, BEventArgs e ) {
            int index = listDictionaries.getSelectedIndex( "" );
            if ( index >= 1 ) {
                listDictionaries.clearSelection( "" );
                String upper_name = listDictionaries.getItemAt( "", index - 1 ).getSubItemAt( 0 );
                boolean upper_enabled = listDictionaries.isItemCheckedAt( "", index - 1 );
                listDictionaries.setItemAt( "", index - 1, (BListViewItem)listDictionaries.getItemAt( "", index ).clone() );
                listDictionaries.setItemCheckedAt( "", index - 1, listDictionaries.isItemCheckedAt( "", index ) );
                listDictionaries.setItemAt( "", index, new BListViewItem( new String[]{ upper_name } ) );
                listDictionaries.setItemCheckedAt( "", index, upper_enabled );
                listDictionaries.setItemSelectedAt( "", index - 1, true );
            }
        }

        private void btnDown_Click( Object sender, BEventArgs e ) {
            int index = listDictionaries.getSelectedIndex( "" );
            if ( index + 1 < listDictionaries.getItemCount( "" ) ) {
                try {
                    listDictionaries.clearSelection( "" );
                    String lower_name = listDictionaries.getItemAt( "", index + 1 ).getSubItemAt( 0 );
                    boolean lower_enabled = listDictionaries.isItemCheckedAt( "", index + 1 );
                    listDictionaries.setItemAt( "", index + 1, (BListViewItem)listDictionaries.getItemAt( "", index ).clone() );
                    listDictionaries.setItemCheckedAt( "", index + 1, listDictionaries.isItemCheckedAt( "", index ) );
                    listDictionaries.setItemAt( "", index, new BListViewItem( new String[] { lower_name } ) );
                    listDictionaries.setItemCheckedAt( "", index, lower_enabled );
                    listDictionaries.setItemSelectedAt( "", index + 1, true );
                } catch ( Exception ex ) {
#if DEBUG
                    Console.WriteLine( "FormWordDictionary#btnDown_Click; ex=" + ex );
#endif
                }
            }
        }

        private void btnCancel_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }

        private void registerEventHandlers() {
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            this.btnUp.Click += new System.EventHandler( this.btnUp_Click );
            this.btnDown.Click += new System.EventHandler( this.btnDown_Click );
            this.Load += new System.EventHandler( this.FormWordDictionary_Load );
            btnCancel.Click += new EventHandler( btnCancel_Click );
        }

        private void setResources() {
        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ..\BuildJavaUI\src\FormWordDictionary.java
        //INCLUDE-SECTION METHOD ..\BuildJavaUI\src\FormWordDictionary.java
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem( "DEFAULT_JP" );
            this.listDictionaries = new bocoree.windows.forms.BListView();
            this.lblAvailableDictionaries = new bocoree.windows.forms.BLabel();
            this.btnOK = new bocoree.windows.forms.BButton();
            this.btnCancel = new bocoree.windows.forms.BButton();
            this.btnUp = new bocoree.windows.forms.BButton();
            this.btnDown = new bocoree.windows.forms.BButton();
            this.SuspendLayout();
            // 
            // listDictionaries
            // 
            this.listDictionaries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listDictionaries.CheckBoxes = true;
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup1.Name = null;
            this.listDictionaries.Groups.AddRange( new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1} );
            listViewItem1.Checked = true;
            listViewItem1.Group = listViewGroup1;
            listViewItem1.StateImageIndex = 1;
            this.listDictionaries.Items.AddRange( new System.Windows.Forms.ListViewItem[] {
            listViewItem1} );
            this.listDictionaries.Location = new System.Drawing.Point( 12, 33 );
            this.listDictionaries.Name = "listDictionaries";
            this.listDictionaries.Size = new System.Drawing.Size( 248, 186 );
            this.listDictionaries.TabIndex = 0;
            this.listDictionaries.UseCompatibleStateImageBehavior = false;
            this.listDictionaries.View = System.Windows.Forms.View.List;
            // 
            // lblAvailableDictionaries
            // 
            this.lblAvailableDictionaries.AutoSize = true;
            this.lblAvailableDictionaries.Location = new System.Drawing.Point( 12, 13 );
            this.lblAvailableDictionaries.Name = "lblAvailableDictionaries";
            this.lblAvailableDictionaries.Size = new System.Drawing.Size( 117, 12 );
            this.lblAvailableDictionaries.TabIndex = 1;
            this.lblAvailableDictionaries.Text = "Available Dictionaries";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 91, 277 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 185, 277 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point( 142, 229 );
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size( 56, 23 );
            this.btnUp.TabIndex = 5;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point( 204, 229 );
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size( 56, 23 );
            this.btnDown.TabIndex = 6;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // FormWordDictionary
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 272, 315 );
            this.Controls.Add( this.btnDown );
            this.Controls.Add( this.btnUp );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.lblAvailableDictionaries );
            this.Controls.Add( this.listDictionaries );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWordDictionary";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "User Dictionary Configuration";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BListView listDictionaries;
        private BLabel lblAvailableDictionaries;
        private BButton btnOK;
        private BButton btnCancel;
        private BButton btnUp;
        private BButton btnDown;
        #endregion

#endif
    }

#if !JAVA
}
#endif
