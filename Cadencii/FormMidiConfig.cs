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
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Boare.Lib.AppUtil;
using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    public unsafe partial class FormMidiConfig : Form {
        private byte m_program_normal;
        private byte m_program_bell;
        private byte m_note_normal;
        private byte m_note_bell;
        private boolean m_ring_bell;
        private int m_pre_utterance;
        private uint m_device_metronome;
        private uint m_device_general;
        private DateTime m_preview_started;
        private float m_speed;

        public FormMidiConfig() {
            InitializeComponent();
            ApplyLanguage();
            Misc.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );

            m_program_normal = MidiPlayer.ProgramNormal;
            m_program_bell = MidiPlayer.ProgramBell;
            m_note_normal = MidiPlayer.NoteNormal;
            m_note_bell = MidiPlayer.NoteBell;
            m_pre_utterance = MidiPlayer.PreUtterance;
            m_ring_bell = MidiPlayer.RingBell;
            m_device_metronome = MidiPlayer.DeviceMetronome;
            m_device_general = MidiPlayer.DeviceGeneral;
            m_speed = MidiPlayer.GetSpeed();

            MidiPlayer.SetSpeed( 1.0f, DateTime.Now );

            comboDeviceMetronome.Items.Clear();
            comboDeviceGeneral.Items.Clear();
            try {
                uint num_devs = windows.midiOutGetNumDevs();
                for ( uint i = 0; i < num_devs; i++ ) {
                    MIDIOUTCAPSA caps = new MIDIOUTCAPSA();
                    windows.midiOutGetDevCapsA( i, ref caps, (uint)Marshal.SizeOf( caps ) );
                    comboDeviceMetronome.Items.Add( i + ": " + caps.szPname );
                    comboDeviceGeneral.Items.Add( i + ": " + caps.szPname );
                }
            } catch {
            }
            if ( MidiPlayer.DeviceGeneral < comboDeviceGeneral.Items.Count ) {
                comboDeviceGeneral.SelectedIndex = (int)MidiPlayer.DeviceGeneral;
            }
            if ( MidiPlayer.DeviceMetronome < comboDeviceMetronome.Items.Count ) {
                comboDeviceMetronome.SelectedIndex = (int)MidiPlayer.DeviceMetronome;
            }

            numNoteNormal.Value = (decimal)MidiPlayer.NoteNormal;
            numNoteBell.Value = (decimal)MidiPlayer.NoteBell;
            numProgramNormal.Value = (decimal)MidiPlayer.ProgramNormal;
            numProgramBell.Value = (decimal)MidiPlayer.ProgramBell;
            numPreUtterance.Value = (decimal)MidiPlayer.PreUtterance;
            chkRingBell.Checked = MidiPlayer.RingBell;
        }

        public static String _( String id ) {
            return Messaging.GetMessage( id );
        }

        public void ApplyLanguage() {
            this.Text = _( "Metronome Config" );
            lblDeviceGeneral.Text = _( "MIDI Device" );
            lblDeviceMetronome.Text = _( "MIDI Device" );
            groupMetronome.Text = _( "Metronome" );
            lblNoteNormal.Text = _( "Note#" );
            lblNoteBell.Text = _( "Note# (Bell)" );
            lblProgramNormal.Text = _( "Program#" );
            lblProgramBell.Text = _( "Program# (Bell)" );
            lblPreUtterance.Text = _( "Pre Utterance" );
            chkPreview.Text = _( "Preview" );
            chkRingBell.Text = _( "Ring Bell" );

            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
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
            int index = comboDeviceMetronome.SelectedIndex;
            if ( 0 <= index ) {
                MidiPlayer.DeviceMetronome = (uint)index;
            }
        }

        private void comboDeviceGeneral_SelectedIndexChanged( object sender, EventArgs e ) {
            int index = comboDeviceGeneral.SelectedIndex;
            if ( 0 <= index ) {
                MidiPlayer.DeviceGeneral = (uint)index;
            }
        }

        private void chkRingBell_CheckedChanged( object sender, EventArgs e ) {
            MidiPlayer.RingBell = chkRingBell.Checked;
        }

        private void chkPreview_CheckedChanged( object sender, EventArgs e ) {
            if ( chkPreview.Checked ) {
                m_preview_started = DateTime.Now;
                MidiPlayer.Start( new VsqFileEx( "Miku", 2, 4, 4, 500000 ), 0, m_preview_started );
            } else {
                MidiPlayer.Stop();
            }
        }

        private void FormMidiConfig_FormClosing( object sender, FormClosingEventArgs e ) {
            MidiPlayer.Stop();
            MidiPlayer.SetSpeed( m_speed, DateTime.Now );
            if ( this.DialogResult == DialogResult.OK ) {
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
        }
    }

}
