/*
 * FormProjectProperty.cs
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/Cadencii/FormTrackProperty.java

import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using org.kbinani.apputil;
using org.kbinani;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using BEventArgs = System.EventArgs;
#endif

#if JAVA
    public class FormTrackProperty extends BDialog {
#else
    public class FormTrackProperty : BDialog {
#endif
        private int m_master_tuning;

        public FormTrackProperty( int master_tuning_in_cent ) {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            applyLanguage();
            m_master_tuning = master_tuning_in_cent;
            txtMasterTuning.setText( master_tuning_in_cent + "" );
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        #region public methods
        public void applyLanguage() {
            lblMasterTuning.setText( _( "Master Tuning in Cent" ) );
            setTitle( _( "Track Property" ) );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
        }

        public int getMasterTuningInCent() {
            return m_master_tuning;
        }
        #endregion

        #region helper methods
        private String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void registerEventHandlers() {
            txtMasterTuning.textChangedEvent.add( new BEventHandler( this, "txtMasterTuning_TextChanged" ) );
            btnOK.clickEvent.add( new BEventHandler( this, "btnOK_Click" ) );
            btnCancel.clickEvent.add( new BEventHandler( this, "btnCancel_Click" ) );
        }

        private void setResources() {
        }
        #endregion

        #region event handlers
        public void txtMasterTuning_TextChanged( Object sender, BEventArgs e ) {
            int v = m_master_tuning;
            try {
                v = PortUtil.parseInt( txtMasterTuning.getText() );
                m_master_tuning = v;
            } catch ( Exception ex ) {
            }
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
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/Cadencii/FormTrackProperty.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/Cadencii/FormTrackProperty.java
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
            this.btnOK = new BButton();
            this.btnCancel = new BButton();
            this.lblMasterTuning = new BLabel();
            this.txtMasterTuning = new BTextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point( 92, 62 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 88, 23 );
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 186, 62 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 88, 23 );
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblMasterTuning
            // 
            this.lblMasterTuning.AutoSize = true;
            this.lblMasterTuning.Location = new System.Drawing.Point( 15, 14 );
            this.lblMasterTuning.Name = "lblMasterTuning";
            this.lblMasterTuning.Size = new System.Drawing.Size( 119, 12 );
            this.lblMasterTuning.TabIndex = 28;
            this.lblMasterTuning.Text = "Master Tuning in Cent";
            // 
            // txtMasterTuning
            // 
            this.txtMasterTuning.Location = new System.Drawing.Point( 46, 29 );
            this.txtMasterTuning.Name = "txtMasterTuning";
            this.txtMasterTuning.Size = new System.Drawing.Size( 187, 19 );
            this.txtMasterTuning.TabIndex = 29;
            // 
            // FormTrackProperty
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 286, 97 );
            this.Controls.Add( this.txtMasterTuning );
            this.Controls.Add( this.lblMasterTuning );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTrackProperty";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Project Property";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BButton btnOK;
        private BButton btnCancel;
        private BLabel lblMasterTuning;
        private BTextBox txtMasterTuning;
        #endregion
#endif
        #endregion

    }

#if !JAVA
}
#endif
