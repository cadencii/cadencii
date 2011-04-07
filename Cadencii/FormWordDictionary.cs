/*
 * FormWordDictionary.cs
 * Copyright © 2008-2011 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/cadencii/FormWordDictionary.java

import java.util.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.apputil;
using org.kbinani.vsq;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
    using BEventHandler = System.EventHandler;
    using BFormClosingEventHandler = System.Windows.Forms.FormClosingEventHandler;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
#endif

#if JAVA
    public class FormWordDictionary extends BDialog {
#else
    class FormWordDictionary : BDialog
    {
#endif
        private static int mColumnWidth = 256;
        private static int mWidth = 327;
        private static int mHeight = 404;

        public FormWordDictionary()
        {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            applyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
            this.setSize( mWidth, mHeight );
            listDictionaries.setColumnWidth( 0, mColumnWidth );
        }

        #region public methods
        public void applyLanguage()
        {
            setTitle( _( "User Dictionary Configuration" ) );
            lblAvailableDictionaries.setText( _( "Available Dictionaries" ) );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
            btnUp.setText( _( "Up" ) );
            btnDown.setText( _( "Down" ) );
            listDictionaries.setColumnHeaders( new String[]{ _( "Name of dictionary" ) } );
        }

        public Vector<ValuePair<String, Boolean>> getResult()
        {
            Vector<ValuePair<String, Boolean>> ret = new Vector<ValuePair<String, Boolean>>();
            int count = listDictionaries.getItemCountRow();
#if DEBUG
            sout.println( "FormWordDictionary#getResult; count=" + count );
#endif
            for ( int i = 0; i < count; i++ ) {
                String name = listDictionaries.getItemAt( i, 0 );
                
                ret.add( new ValuePair<String, Boolean>(
                    listDictionaries.getItemAt( i, 0 ), listDictionaries.isRowChecked( i ) ) );
            }
            return ret;
        }
        #endregion

        #region helper methods
        private static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        private void registerEventHandlers()
        {
            this.Load += new BEventHandler( FormWordDictionary_Load );
            this.FormClosing += new BFormClosingEventHandler( FormWordDictionary_FormClosing );
            btnOK.Click += new BEventHandler( btnOK_Click );
            btnUp.Click += new BEventHandler( btnUp_Click );
            btnDown.Click += new BEventHandler( btnDown_Click );
            btnCancel.Click += new BEventHandler( btnCancel_Click );
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void FormWordDictionary_FormClosing( Object sender, BFormClosingEventArgs e )
        {
            mColumnWidth = listDictionaries.getColumnWidth( 0 );
            mWidth = getWidth();
            mHeight = getHeight();
        }

        public void FormWordDictionary_Load( Object sender, BEventArgs e )
        {
            listDictionaries.clear();
            for ( int i = 0; i < SymbolTable.getCount(); i++ ) {
                String name = SymbolTable.getSymbolTable( i ).getName();
                boolean enabled = SymbolTable.getSymbolTable( i ).isEnabled();
                listDictionaries.addRow( new String[]{ name }, enabled );
            }
        }

        public void btnOK_Click( Object sender, BEventArgs e )
        {
            setDialogResult( BDialogResult.OK );
        }

        public void btnUp_Click( Object sender, BEventArgs e )
        {
            int index = listDictionaries.getSelectedRow();
            if ( index >= 1 ) {
                try {
                    listDictionaries.clearSelection();
                    String upper_name = listDictionaries.getItemAt( index - 1, 0 );
                    boolean upper_enabled = listDictionaries.isRowChecked( index - 1 );
                    String lower_name = listDictionaries.getItemAt( index, 0 );
                    boolean lower_enabled = listDictionaries.isRowChecked( index );

                    listDictionaries.setItemAt( index - 1, 0, lower_name );
                    listDictionaries.setRowChecked( index - 1, lower_enabled );
                    listDictionaries.setItemAt( index, 0, upper_name );
                    listDictionaries.setRowChecked( index, upper_enabled );

                    listDictionaries.setSelectedRow( index - 1 );
                } catch ( Exception ex ) {
                    serr.println( "FormWordDictionary#btnUp_Click; ex=" + ex );
                }
            }
        }

        public void btnDown_Click( Object sender, BEventArgs e )
        {
            int index = listDictionaries.getSelectedRow();
            if ( 0 <= index && index + 1 < listDictionaries.getItemCountRow() ) {
                try {
                    listDictionaries.clearSelection();
                    String upper_name = listDictionaries.getItemAt( index, 0 );
                    boolean upper_enabled = listDictionaries.isRowChecked( index );
                    String lower_name = listDictionaries.getItemAt( index + 1, 0 );
                    boolean lower_enabled = listDictionaries.isRowChecked( index + 1 );

                    listDictionaries.setItemAt( index + 1, 0, upper_name );
                    listDictionaries.setRowChecked( index + 1, upper_enabled );
                    listDictionaries.setItemAt( index, 0, lower_name );
                    listDictionaries.setRowChecked( index, lower_enabled );

                    listDictionaries.setSelectedRow( index + 1 );
                } catch ( Exception ex ) {
                    serr.println( "FormWordDictionary#btnDown_Click; ex=" + ex );
                }
            }
        }

        public void btnCancel_Click( Object sender, BEventArgs e )
        {
            setDialogResult( BDialogResult.CANCEL );
        }
        #endregion

        #region UI implementation
#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/cadencii/FormWordDictionary.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/cadencii/FormWordDictionary.java
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem( "DEFAULT_JP" );
            this.listDictionaries = new org.kbinani.windows.forms.BListView();
            this.lblAvailableDictionaries = new org.kbinani.windows.forms.BLabel();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnUp = new org.kbinani.windows.forms.BButton();
            this.btnDown = new org.kbinani.windows.forms.BButton();
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
            listViewGroup2.Header = "ListViewGroup";
            listViewGroup2.Name = null;
            listViewGroup3.Header = "ListViewGroup";
            listViewGroup3.Name = null;
            this.listDictionaries.Groups.AddRange( new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3} );
            this.listDictionaries.HideSelection = false;
            listViewItem1.Checked = true;
            listViewItem1.Group = listViewGroup3;
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
        #endregion

    }

#if !JAVA
}
#endif
