/*
 * FormExportMusicXml.cs
 * Copyright (C) 2010 kbinani
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

import org.kbinani.*;
import org.kbinani.windows.forms.*;
import org.kbinani.apputil.*;

#else
using System;
using org.kbinani;
using org.kbinani.windows.forms;
using org.kbinani.apputil;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
#endif

#if JAVA
    public class FormExportMusicXml extends BForm {
#else
    public class FormExportMusicXml : BForm {
#endif
        private static boolean isChangeTempo = false;

        private BButton btnCancel;
        private BCheckBox chkChangeTempo;
        private System.ComponentModel.IContainer components;
        private BLabel lblDescription;
        private BButton btnOK;

        public FormExportMusicXml() {
            InitializeComponent();
            applyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
            registerEventHandlers();
            chkChangeTempo.setSelected( isChangeTempo );
        }

        private void applyLanguage() {
            setTitle( _( "Export MusicXML" ) );
            chkChangeTempo.setText( _( "Convert Gate-time" ) );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
            lblDescription.setText( _( "If checked, convert gate-time of notes in order to match the base-tempo" ) );
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void registerEventHandlers() {
            formClosingEvent.add( new BFormClosingEventHandler( this, "FormExportMusicXml_FormClosing" ) );
            btnOK.clickEvent.add( new BEventHandler( this, "btnOK_Click" ) );
            btnCancel.clickEvent.add( new BEventHandler( this, "btnCancel_Click" ) );
        }

        public void btnOK_Click( Object sender, EventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }

        public void btnCancel_Click( Object sender, EventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }

        public void FormExportMusicXml_FormClosing( Object sender, BFormClosingEventArgs e ) {
            isChangeTempo = isTempoConversionRequired();
        }

        public boolean isTempoConversionRequired() {
            return chkChangeTempo.isSelected();
        }

        private void InitializeComponent() {
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.chkChangeTempo = new org.kbinani.windows.forms.BCheckBox();
            this.lblDescription = new org.kbinani.windows.forms.BLabel();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 209, 134 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 128, 134 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // chkChangeTempo
            // 
            this.chkChangeTempo.AutoSize = true;
            this.chkChangeTempo.Location = new System.Drawing.Point( 12, 23 );
            this.chkChangeTempo.Name = "chkChangeTempo";
            this.chkChangeTempo.Size = new System.Drawing.Size( 120, 16 );
            this.chkChangeTempo.TabIndex = 6;
            this.chkChangeTempo.Text = "Convert Gate-time";
            this.chkChangeTempo.UseVisualStyleBackColor = true;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoEllipsis = true;
            this.lblDescription.Location = new System.Drawing.Point( 27, 42 );
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size( 257, 78 );
            this.lblDescription.TabIndex = 7;
            this.lblDescription.Text = "If checked, convert gate-time of notes in order to match the base-tempo";
            // 
            // FormExportMusicXml
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 296, 169 );
            this.Controls.Add( this.lblDescription );
            this.Controls.Add( this.chkChangeTempo );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormExportMusicXml";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Export MusicXML";
            this.ResumeLayout( false );
            this.PerformLayout();

        }
    }

#if !JAVA
}
#endif
