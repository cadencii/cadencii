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
using System;
using org.kbinani;
using org.kbinani.windows.forms;
using org.kbinani.apputil;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;

    public class FormExportMusicXml : BForm {
        private static boolean isChangeTempo = false;

        private BButton btnCancel;
        private BCheckBox chkChangeTempo;
        private System.Windows.Forms.ToolTip toolTip;
        private System.ComponentModel.IContainer components;
        private BButton btnOK;

        public FormExportMusicXml() {
            InitializeComponent();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
            registerEventHandlers();
            chkChangeTempo.setSelected( isChangeTempo );
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
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.chkChangeTempo = new org.kbinani.windows.forms.BCheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip( this.components );
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 167, 57 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point( 86, 57 );
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
            this.chkChangeTempo.Size = new System.Drawing.Size( 102, 16 );
            this.chkChangeTempo.TabIndex = 6;
            this.chkChangeTempo.Text = "Convert Tempo";
            this.toolTip.SetToolTip( this.chkChangeTempo, "If checked, convert gate-time of notes in order to match the base-tempo" );
            this.chkChangeTempo.UseVisualStyleBackColor = true;
            // 
            // FormExportMusicXml
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 254, 97 );
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

}
