/*
 * FormAskKeySoundGeneration.cs
 * Copyright © 2010 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/cadencii/FormAskKeySoundGeneration.java

import org.kbinani.*;
import org.kbinani.windows.forms.*;
import org.kbinani.apputil.*;
#else
using System;
using org.kbinani.windows.forms;
using org.kbinani.apputil;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using BEventArgs = System.EventArgs;
#endif

#if JAVA
    public class FormAskKeySoundGeneration extends BDialog {
#else
    public class FormAskKeySoundGeneration : BDialog {
#endif
        public FormAskKeySoundGeneration() {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            applyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        #region public methods
        public void applyLanguage() {
            lblMessage.setText( _( "It seems some key-board sounds are missing. Do you want to re-generate them now?" ) );
            chkAlwaysPerformThisCheck.setText( _( "Always perform this check when starting Cadencii." ) );
            btnYes.setText( _( "Yes" ) );
            btnNo.setText( _( "No" ) );
        }

        public void setAlwaysPerformThisCheck( boolean value ) {
            chkAlwaysPerformThisCheck.setSelected( value );
        }

        public boolean isAlwaysPerformThisCheck() {
            return chkAlwaysPerformThisCheck.isSelected();
        }
        #endregion

        #region helper methods
        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void registerEventHandlers() {
            btnYes.Click += new EventHandler( btnYes_Click );
            btnNo.Click += new EventHandler( btnNo_Click );
        }
        #endregion

        #region event handlers
        public void btnYes_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.YES );
        }

        public void btnNo_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.NO );
        }
        #endregion

        #region UI implementation
#if JAVA
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/cadencii/FormAskKeySoundGeneration.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/cadencii/FormAskKeySoundGeneration.java
#else
        private void InitializeComponent() {
            this.btnNo = new org.kbinani.windows.forms.BButton();
            this.btnYes = new org.kbinani.windows.forms.BButton();
            this.chkAlwaysPerformThisCheck = new org.kbinani.windows.forms.BCheckBox();
            this.lblMessage = new org.kbinani.windows.forms.BLabel();
            this.SuspendLayout();
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNo.Location = new System.Drawing.Point( 183, 112 );
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size( 75, 23 );
            this.btnNo.TabIndex = 5;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = true;
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnYes.Location = new System.Drawing.Point( 63, 112 );
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size( 75, 23 );
            this.btnYes.TabIndex = 4;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysPerformThisCheck
            // 
            this.chkAlwaysPerformThisCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysPerformThisCheck.AutoSize = true;
            this.chkAlwaysPerformThisCheck.Checked = true;
            this.chkAlwaysPerformThisCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlwaysPerformThisCheck.Location = new System.Drawing.Point( 14, 77 );
            this.chkAlwaysPerformThisCheck.Name = "chkAlwaysPerformThisCheck";
            this.chkAlwaysPerformThisCheck.Size = new System.Drawing.Size( 284, 16 );
            this.chkAlwaysPerformThisCheck.TabIndex = 6;
            this.chkAlwaysPerformThisCheck.Text = "Always perform this check when starting Cadencii.";
            this.chkAlwaysPerformThisCheck.UseVisualStyleBackColor = true;
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.AutoEllipsis = true;
            this.lblMessage.Location = new System.Drawing.Point( 12, 21 );
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size( 302, 53 );
            this.lblMessage.TabIndex = 7;
            this.lblMessage.Text = "It seems some key-board sounds are missing. Do you want to re-generate them now?";
            // 
            // FormAskKeySoundGeneration
            // 
            this.CancelButton = this.btnNo;
            this.ClientSize = new System.Drawing.Size( 326, 147 );
            this.Controls.Add( this.lblMessage );
            this.Controls.Add( this.chkAlwaysPerformThisCheck );
            this.Controls.Add( this.btnNo );
            this.Controls.Add( this.btnYes );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAskKeySoundGeneration";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        private BButton btnNo;
        private BCheckBox chkAlwaysPerformThisCheck;
        private BLabel lblMessage;
        private BButton btnYes;

#endif
        #endregion
    }

#if !JAVA
}
#endif
