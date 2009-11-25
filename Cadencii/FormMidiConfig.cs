#if ENABLE_MIDI
/*
 * FormMidiConfig.cs
 * Copyright (c) 2009 kbinani
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
#else
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using bocoree;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormMidiConfig extends BForm {
#else
    public unsafe class FormMidiConfig : BForm {
#endif
        private byte m_program_normal;
        private byte m_program_bell;
        private byte m_note_normal;
        private byte m_note_bell;
        private boolean m_ring_bell;
        private int m_pre_utterance;
        private uint m_device_metronome;
        private uint m_device_general;
        private double m_preview_started;
        private float m_speed;
        private boolean m_metronome_enabled_init_stat;

        public FormMidiConfig() {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            ApplyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );

            m_program_normal = MidiPlayer.ProgramNormal;
            m_program_bell = MidiPlayer.ProgramBell;
            m_note_normal = MidiPlayer.NoteNormal;
            m_note_bell = MidiPlayer.NoteBell;
            m_pre_utterance = MidiPlayer.PreUtterance;
            m_ring_bell = MidiPlayer.RingBell;
            m_device_metronome = MidiPlayer.DeviceMetronome;
            m_device_general = MidiPlayer.DeviceGeneral;
            m_speed = MidiPlayer.GetSpeed();

            m_metronome_enabled_init_stat = AppManager.editorConfig.MetronomeEnabled;
            AppManager.editorConfig.MetronomeEnabled = true;
            MidiPlayer.SetSpeed( 1.0f, PortUtil.getCurrentTime() );

            comboDeviceMetronome.removeAllItems();
            comboDeviceGeneral.removeAllItems();
            try {
                uint num_devs = win32.midiOutGetNumDevs();
                for ( uint i = 0; i < num_devs; i++ ) {
                    MIDIOUTCAPSA caps = new MIDIOUTCAPSA();
                    win32.midiOutGetDevCapsA( i, ref caps, (uint)Marshal.SizeOf( caps ) );
                    comboDeviceMetronome.addItem( i + ": " + caps.szPname );
                    comboDeviceGeneral.addItem( i + ": " + caps.szPname );
                }
            } catch {
            }
            if ( MidiPlayer.DeviceGeneral < comboDeviceGeneral.getItemCount() ) {
                comboDeviceGeneral.setSelectedItem( comboDeviceGeneral.getItemAt( (int)MidiPlayer.DeviceGeneral ) );
            }
            if ( MidiPlayer.DeviceMetronome < comboDeviceMetronome.getItemCount() ) {
                comboDeviceMetronome.setSelectedItem( comboDeviceMetronome.getItemAt( (int)MidiPlayer.DeviceMetronome ) );
            }

            numNoteNormal.Value = (decimal)MidiPlayer.NoteNormal;
            numNoteBell.Value = (decimal)MidiPlayer.NoteBell;
            numProgramNormal.Value = (decimal)MidiPlayer.ProgramNormal;
            numProgramBell.Value = (decimal)MidiPlayer.ProgramBell;
            numPreUtterance.Value = (decimal)MidiPlayer.PreUtterance;
            chkRingBell.setSelected( MidiPlayer.RingBell );
        }

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public void ApplyLanguage() {
            setTitle( _( "Metronome Config" ) );
            lblDeviceGeneral.setText( _( "MIDI Device" ) );
            lblDeviceMetronome.setText( _( "MIDI Device" ) );
            groupMetronome.setTitle( _( "Metronome" ) );
            lblNoteNormal.setText( _( "Note#" ) );
            lblNoteBell.setText( _( "Note# (Bell)" ) );
            lblProgramNormal.setText( _( "Program#" ) );
            lblProgramBell.setText( _( "Program# (Bell)" ) );
            lblPreUtterance.setText( _( "Pre Utterance" ) );
            chkPreview.setText( _( "Preview" ) );
            chkRingBell.setText( _( "Ring Bell" ) );

            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
        }

        private void numProgramNormal_ValueChanged( object sender, EventArgs e ) {
            MidiPlayer.ProgramNormal = (byte)numProgramNormal.Value;
        }

        private void numProgramBell_ValueChanged( object sender, EventArgs e ) {
            MidiPlayer.ProgramBell = (byte)numProgramBell.Value;
        }

        private void numNoteNormal_ValueChanged( object sender, EventArgs e ) {
            MidiPlayer.NoteNormal = (byte)numNoteNormal.Value;
        }

        private void numNoteBell_ValueChanged( object sender, EventArgs e ) {
            MidiPlayer.NoteBell = (byte)numNoteBell.Value;
        }

        private void numPreUtterance_ValueChanged( object sender, EventArgs e ) {
            MidiPlayer.PreUtterance = (int)numPreUtterance.Value;
        }

        private void comboDeviceMetronome_SelectedIndexChanged( object sender, EventArgs e ) {
            int index = comboDeviceMetronome.getSelectedIndex();
            if ( 0 <= index ) {
                MidiPlayer.DeviceMetronome = (uint)index;
            }
        }

        private void comboDeviceGeneral_SelectedIndexChanged( object sender, EventArgs e ) {
            int index = comboDeviceGeneral.getSelectedIndex();
            if ( 0 <= index ) {
                MidiPlayer.DeviceGeneral = (uint)index;
            }
        }

        private void chkRingBell_CheckedChanged( object sender, EventArgs e ) {
            MidiPlayer.RingBell = chkRingBell.isSelected();
        }

        private void chkPreview_CheckedChanged( object sender, EventArgs e ) {
            if ( chkPreview.isSelected() ) {
                m_preview_started = PortUtil.getCurrentTime();
                MidiPlayer.Start( new VsqFileEx( "Miku", 2, 4, 4, 500000 ), 0, m_preview_started );
            } else {
                MidiPlayer.Stop();
            }
        }

        private void FormMidiConfig_FormClosing( object sender, FormClosingEventArgs e ) {
            MidiPlayer.Stop();
            MidiPlayer.SetSpeed( m_speed, PortUtil.getCurrentTime() );
            if ( getDialogResult() == BDialogResult.OK ) {
                AppManager.editorConfig.MidiRingBell = MidiPlayer.RingBell;
                AppManager.editorConfig.MidiDeviceGeneral.PortNumber = (int)MidiPlayer.DeviceGeneral;
                AppManager.editorConfig.MidiDeviceMetronome.PortNumber = (int)MidiPlayer.DeviceMetronome;
                AppManager.editorConfig.MidiNoteBell = MidiPlayer.NoteBell;
                AppManager.editorConfig.MidiNoteNormal = MidiPlayer.NoteNormal;
                AppManager.editorConfig.MidiPreUtterance = MidiPlayer.PreUtterance;
                AppManager.editorConfig.MidiProgramBell = MidiPlayer.ProgramBell;
                AppManager.editorConfig.MidiProgramNormal = MidiPlayer.ProgramNormal;
            } else {
                // キャンセルなら最初の状態に復帰させる
                MidiPlayer.RingBell = m_ring_bell;
                MidiPlayer.DeviceGeneral = m_device_general;
                MidiPlayer.DeviceMetronome = m_device_metronome;
                MidiPlayer.NoteBell = m_note_bell;
                MidiPlayer.NoteNormal = m_note_normal;
                MidiPlayer.PreUtterance = m_pre_utterance;
                MidiPlayer.ProgramBell = m_program_bell;
                MidiPlayer.ProgramNormal = m_program_normal;
            }
            AppManager.editorConfig.MetronomeEnabled = m_metronome_enabled_init_stat;
        }

        private void registerEventHandlers() {
            this.chkRingBell.CheckedChanged += new System.EventHandler( this.chkRingBell_CheckedChanged );
            this.chkPreview.CheckedChanged += new System.EventHandler( this.chkPreview_CheckedChanged );
            this.comboDeviceMetronome.SelectedIndexChanged += new System.EventHandler( this.comboDeviceMetronome_SelectedIndexChanged );
            this.comboDeviceGeneral.SelectedIndexChanged += new System.EventHandler( this.comboDeviceGeneral_SelectedIndexChanged );
            this.numPreUtterance.ValueChanged += new System.EventHandler( this.numPreUtterance_ValueChanged );
            this.numNoteBell.ValueChanged += new System.EventHandler( this.numNoteBell_ValueChanged );
            this.numNoteNormal.ValueChanged += new System.EventHandler( this.numNoteNormal_ValueChanged );
            this.numProgramBell.ValueChanged += new System.EventHandler( this.numProgramBell_ValueChanged );
            this.numProgramNormal.ValueChanged += new System.EventHandler( this.numProgramNormal_ValueChanged );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormMidiConfig_FormClosing );
        }

        private void setResources() {
        }

#if JAVA
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
            this.lblDeviceMetronome = new BLabel();
            this.lblProgramNormal = new BLabel();
            this.lblProgramBell = new BLabel();
            this.lblNoteNormal = new BLabel();
            this.lblNoteBell = new BLabel();
            this.chkRingBell = new BCheckBox();
            this.lblPreUtterance = new BLabel();
            this.lblMillisec = new BLabel();
            this.btnOK = new BButton();
            this.btnCancel = new BButton();
            this.chkPreview = new BCheckBox();
            this.comboDeviceMetronome = new BComboBox();
            this.groupMetronome = new BGroupBox();
            this.lblDeviceGeneral = new BLabel();
            this.comboDeviceGeneral = new BComboBox();
            this.numPreUtterance = new Boare.Cadencii.NumericUpDownEx();
            this.numNoteBell = new Boare.Cadencii.NumericUpDownEx();
            this.numNoteNormal = new Boare.Cadencii.NumericUpDownEx();
            this.numProgramBell = new Boare.Cadencii.NumericUpDownEx();
            this.numProgramNormal = new Boare.Cadencii.NumericUpDownEx();
            this.groupMetronome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPreUtterance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNoteBell)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNoteNormal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numProgramBell)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numProgramNormal)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDeviceMetronome
            // 
            this.lblDeviceMetronome.AutoSize = true;
            this.lblDeviceMetronome.Location = new System.Drawing.Point( 15, 24 );
            this.lblDeviceMetronome.Name = "lblDeviceMetronome";
            this.lblDeviceMetronome.Size = new System.Drawing.Size( 67, 12 );
            this.lblDeviceMetronome.TabIndex = 0;
            this.lblDeviceMetronome.Text = "MIDI Device";
            // 
            // lblProgramNormal
            // 
            this.lblProgramNormal.AutoSize = true;
            this.lblProgramNormal.Location = new System.Drawing.Point( 15, 53 );
            this.lblProgramNormal.Name = "lblProgramNormal";
            this.lblProgramNormal.Size = new System.Drawing.Size( 53, 12 );
            this.lblProgramNormal.TabIndex = 2;
            this.lblProgramNormal.Text = "Program#";
            // 
            // lblProgramBell
            // 
            this.lblProgramBell.AutoSize = true;
            this.lblProgramBell.Location = new System.Drawing.Point( 15, 82 );
            this.lblProgramBell.Name = "lblProgramBell";
            this.lblProgramBell.Size = new System.Drawing.Size( 85, 12 );
            this.lblProgramBell.TabIndex = 5;
            this.lblProgramBell.Text = "Program# (Bell)";
            // 
            // lblNoteNormal
            // 
            this.lblNoteNormal.AutoSize = true;
            this.lblNoteNormal.Location = new System.Drawing.Point( 15, 111 );
            this.lblNoteNormal.Name = "lblNoteNormal";
            this.lblNoteNormal.Size = new System.Drawing.Size( 35, 12 );
            this.lblNoteNormal.TabIndex = 7;
            this.lblNoteNormal.Text = "Note#";
            // 
            // lblNoteBell
            // 
            this.lblNoteBell.AutoSize = true;
            this.lblNoteBell.Location = new System.Drawing.Point( 15, 140 );
            this.lblNoteBell.Name = "lblNoteBell";
            this.lblNoteBell.Size = new System.Drawing.Size( 67, 12 );
            this.lblNoteBell.TabIndex = 8;
            this.lblNoteBell.Text = "Note# (Bell)";
            // 
            // chkRingBell
            // 
            this.chkRingBell.AutoSize = true;
            this.chkRingBell.Location = new System.Drawing.Point( 17, 198 );
            this.chkRingBell.Name = "chkRingBell";
            this.chkRingBell.Size = new System.Drawing.Size( 71, 16 );
            this.chkRingBell.TabIndex = 7;
            this.chkRingBell.Text = "Ring Bell";
            this.chkRingBell.UseVisualStyleBackColor = true;
            // 
            // lblPreUtterance
            // 
            this.lblPreUtterance.AutoSize = true;
            this.lblPreUtterance.Location = new System.Drawing.Point( 15, 169 );
            this.lblPreUtterance.Name = "lblPreUtterance";
            this.lblPreUtterance.Size = new System.Drawing.Size( 76, 12 );
            this.lblPreUtterance.TabIndex = 12;
            this.lblPreUtterance.Text = "Pre Utterance";
            // 
            // lblMillisec
            // 
            this.lblMillisec.AutoSize = true;
            this.lblMillisec.Location = new System.Drawing.Point( 260, 169 );
            this.lblMillisec.Name = "lblMillisec";
            this.lblMillisec.Size = new System.Drawing.Size( 44, 12 );
            this.lblMillisec.TabIndex = 14;
            this.lblMillisec.Text = "millisec";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point( 193, 343 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 274, 343 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkPreview
            // 
            this.chkPreview.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkPreview.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkPreview.Location = new System.Drawing.Point( 17, 227 );
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size( 75, 23 );
            this.chkPreview.TabIndex = 8;
            this.chkPreview.Text = "Preview";
            this.chkPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkPreview.UseVisualStyleBackColor = true;
            // 
            // comboDeviceMetronome
            // 
            this.comboDeviceMetronome.FormattingEnabled = true;
            this.comboDeviceMetronome.Location = new System.Drawing.Point( 134, 21 );
            this.comboDeviceMetronome.Name = "comboDeviceMetronome";
            this.comboDeviceMetronome.Size = new System.Drawing.Size( 188, 20 );
            this.comboDeviceMetronome.TabIndex = 1;
            // 
            // groupMetronome
            // 
            this.groupMetronome.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupMetronome.Controls.Add( this.lblDeviceMetronome );
            this.groupMetronome.Controls.Add( this.chkPreview );
            this.groupMetronome.Controls.Add( this.comboDeviceMetronome );
            this.groupMetronome.Controls.Add( this.lblProgramNormal );
            this.groupMetronome.Controls.Add( this.numPreUtterance );
            this.groupMetronome.Controls.Add( this.lblProgramBell );
            this.groupMetronome.Controls.Add( this.numNoteBell );
            this.groupMetronome.Controls.Add( this.lblNoteNormal );
            this.groupMetronome.Controls.Add( this.numNoteNormal );
            this.groupMetronome.Controls.Add( this.lblNoteBell );
            this.groupMetronome.Controls.Add( this.numProgramBell );
            this.groupMetronome.Controls.Add( this.chkRingBell );
            this.groupMetronome.Controls.Add( this.numProgramNormal );
            this.groupMetronome.Controls.Add( this.lblPreUtterance );
            this.groupMetronome.Controls.Add( this.lblMillisec );
            this.groupMetronome.Location = new System.Drawing.Point( 12, 58 );
            this.groupMetronome.Name = "groupMetronome";
            this.groupMetronome.Size = new System.Drawing.Size( 336, 268 );
            this.groupMetronome.TabIndex = 1;
            this.groupMetronome.TabStop = false;
            this.groupMetronome.Text = "Metronome";
            // 
            // lblDeviceGeneral
            // 
            this.lblDeviceGeneral.AutoSize = true;
            this.lblDeviceGeneral.Location = new System.Drawing.Point( 27, 20 );
            this.lblDeviceGeneral.Name = "lblDeviceGeneral";
            this.lblDeviceGeneral.Size = new System.Drawing.Size( 67, 12 );
            this.lblDeviceGeneral.TabIndex = 25;
            this.lblDeviceGeneral.Text = "MIDI Device";
            // 
            // comboDeviceGeneral
            // 
            this.comboDeviceGeneral.FormattingEnabled = true;
            this.comboDeviceGeneral.Location = new System.Drawing.Point( 146, 17 );
            this.comboDeviceGeneral.Name = "comboDeviceGeneral";
            this.comboDeviceGeneral.Size = new System.Drawing.Size( 188, 20 );
            this.comboDeviceGeneral.TabIndex = 0;
            // 
            // numPreUtterance
            // 
            this.numPreUtterance.Location = new System.Drawing.Point( 134, 167 );
            this.numPreUtterance.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.numPreUtterance.Minimum = new decimal( new int[] {
            1000,
            0,
            0,
            -2147483648} );
            this.numPreUtterance.Name = "numPreUtterance";
            this.numPreUtterance.Size = new System.Drawing.Size( 120, 19 );
            this.numPreUtterance.TabIndex = 6;
            // 
            // numNoteBell
            // 
            this.numNoteBell.Location = new System.Drawing.Point( 134, 138 );
            this.numNoteBell.Maximum = new decimal( new int[] {
            127,
            0,
            0,
            0} );
            this.numNoteBell.Name = "numNoteBell";
            this.numNoteBell.Size = new System.Drawing.Size( 100, 19 );
            this.numNoteBell.TabIndex = 5;
            // 
            // numNoteNormal
            // 
            this.numNoteNormal.Location = new System.Drawing.Point( 134, 109 );
            this.numNoteNormal.Maximum = new decimal( new int[] {
            127,
            0,
            0,
            0} );
            this.numNoteNormal.Name = "numNoteNormal";
            this.numNoteNormal.Size = new System.Drawing.Size( 100, 19 );
            this.numNoteNormal.TabIndex = 4;
            // 
            // numProgramBell
            // 
            this.numProgramBell.Location = new System.Drawing.Point( 134, 80 );
            this.numProgramBell.Maximum = new decimal( new int[] {
            127,
            0,
            0,
            0} );
            this.numProgramBell.Name = "numProgramBell";
            this.numProgramBell.Size = new System.Drawing.Size( 100, 19 );
            this.numProgramBell.TabIndex = 3;
            // 
            // numProgramNormal
            // 
            this.numProgramNormal.Location = new System.Drawing.Point( 134, 51 );
            this.numProgramNormal.Maximum = new decimal( new int[] {
            127,
            0,
            0,
            0} );
            this.numProgramNormal.Name = "numProgramNormal";
            this.numProgramNormal.Size = new System.Drawing.Size( 100, 19 );
            this.numProgramNormal.TabIndex = 2;
            // 
            // FormMidiConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 361, 378 );
            this.Controls.Add( this.lblDeviceGeneral );
            this.Controls.Add( this.comboDeviceGeneral );
            this.Controls.Add( this.groupMetronome );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMidiConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Metronome Config";
            this.groupMetronome.ResumeLayout( false );
            this.groupMetronome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPreUtterance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNoteBell)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNoteNormal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numProgramBell)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numProgramNormal)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BLabel lblDeviceMetronome;
        private BLabel lblProgramNormal;
        private BLabel lblProgramBell;
        private BLabel lblNoteNormal;
        private BLabel lblNoteBell;
        private BCheckBox chkRingBell;
        private BLabel lblPreUtterance;
        private BLabel lblMillisec;
        private BButton btnOK;
        private BButton btnCancel;
        private BCheckBox chkPreview;
        private NumericUpDownEx numProgramNormal;
        private NumericUpDownEx numProgramBell;
        private NumericUpDownEx numNoteNormal;
        private NumericUpDownEx numNoteBell;
        private NumericUpDownEx numPreUtterance;
        private BComboBox comboDeviceMetronome;
        private BGroupBox groupMetronome;
        private BLabel lblDeviceGeneral;
        private BComboBox comboDeviceGeneral;
        #endregion
#endif
    }

#if !JAVA
}
#endif
#endif
