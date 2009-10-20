/*
 * Preference.cs
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
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Boare.Lib.Vsq;
using Boare.Lib.AppUtil;
using Boare.Lib.Media;
using bocoree;
using bocoree.util;
using bocoree.io;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using java = bocoree;
    using boolean = System.Boolean;

    partial class Preference : BForm {
        java.awt.Font m_base_font;
        Font m_screen_font;
        Font m_counter_font;
        Vector<String> m_program_change = null;
        private Platform m_platform = Platform.Windows;
        private Vector<SingerConfig> m_utau_singers = new Vector<SingerConfig>();

        public Preference() {
            InitializeComponent();
            ApplyLanguage();

            comboVibratoLength.Items.Clear();
            foreach ( DefaultVibratoLength dvl in Enum.GetValues( typeof( DefaultVibratoLength ) ) ) {
                comboVibratoLength.Items.Add( DefaultVibratoLengthUtil.toString( dvl ) );
            }
            comboVibratoLength.SelectedIndex = 1;

            comboAutoVibratoMinLength.Items.Clear();
            foreach ( AutoVibratoMinLength avml in Enum.GetValues( typeof( AutoVibratoMinLength ) ) ) {
                comboAutoVibratoMinLength.Items.Add( AutoVibratoMinLengthUtil.toString( avml ) );
            }
            comboAutoVibratoMinLength.SelectedIndex = 0;

            comboAutoVibratoType1.Items.Clear();
            for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( SynthesizerType.VOCALOID1 ); itr.hasNext(); ) {
                VibratoConfig vconfig = (VibratoConfig)itr.next();
                comboAutoVibratoType1.Items.Add( vconfig );
            }
            if ( comboAutoVibratoType1.Items.Count > 0 ) {
                comboAutoVibratoType1.SelectedIndex = 0;
            }

            comboAutoVibratoType2.Items.Clear();
            for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( SynthesizerType.VOCALOID2 ); itr.hasNext(); ) {
                VibratoConfig vconfig = (VibratoConfig)itr.next();
                comboAutoVibratoType2.Items.Add( vconfig );
            }
            if ( comboAutoVibratoType2.Items.Count > 0 ) {
                comboAutoVibratoType2.SelectedIndex = 0;
            }

            comboDynamics.Items.Clear();
            comboAmplitude.Items.Clear();
            comboPeriod.Items.Clear();
            for ( Iterator itr = ClockResolution.iterator(); itr.hasNext(); ){
                ClockResolution cr = (ClockResolution)itr.next();
                comboDynamics.Items.Add( cr.ToString() );
                comboAmplitude.Items.Add( cr.ToString() );
                comboPeriod.Items.Add( cr.ToString() );
            }
            comboDynamics.SelectedIndex = 0;
            comboAmplitude.SelectedIndex = 0;
            comboPeriod.SelectedIndex = 0;

            comboLanguage.Items.Clear();
            String[] list = Messaging.getRegisteredLanguage();
            int index = 0;
            comboLanguage.Items.Add( "Default" );
            int count = 0;
            foreach ( String s in list ) {
                count++;
                comboLanguage.Items.Add( s );
                if ( s.Equals( Messaging.getLanguage() ) ) {
                    index = count;
                }
            }
            comboLanguage.SelectedIndex = index;

            SingerConfig[] dict = VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID2 );
            m_program_change = new Vector<String>();
            comboDefualtSinger.Items.Clear();
            foreach ( SingerConfig kvp in dict ) {
                m_program_change.add( kvp.VOICENAME );
                comboDefualtSinger.Items.Add( kvp.VOICENAME );
            }
            comboDefualtSinger.Enabled = (comboDefualtSinger.Items.Count > 0);
            if ( comboDefualtSinger.Items.Count > 0 ) {
                comboDefualtSinger.SelectedIndex = 0;
            }

            comboPlatform.Items.Clear();
#if DEBUG
            AppManager.debugWriteLine( "Preference.ctor()" );
            AppManager.debugWriteLine( "    Environment.OSVersion.Platform=" + Environment.OSVersion.Platform );
#endif
#if !DEBUG
            PlatformID platform = Environment.OSVersion.Platform;
            if ( platform == PlatformID.Win32NT ||
                 platform == PlatformID.Win32S ||
                 platform == PlatformID.Win32Windows ||
                 platform == PlatformID.WinCE ) {
                comboPlatform.Items.Add( Platform.Windows + "" );
                comboPlatform.Enabled = false;
                chkCommandKeyAsControl.Enabled = false;
            } else {
#endif
                foreach ( Platform p in Enum.GetValues( typeof( Platform ) ) ) {
                    comboPlatform.Items.Add( p + "" );
                }
#if !DEBUG
            }
#endif

            comboMidiInPortNumber.Items.Clear();
            bocoree.MIDIINCAPS[] midiins = MidiInDevice.GetMidiInDevices();
            for ( int i = 0; i < midiins.Length; i++ ) {
                comboMidiInPortNumber.Items.Add( midiins[i] );
            }
            if ( comboMidiInPortNumber.Items.Count == 0 ) {
                comboMidiInPortNumber.Enabled = false;
            } else {
                comboMidiInPortNumber.Enabled = true;
            }

            txtVOCALOID1.Text = VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID1 );
            txtVOCALOID2.Text = VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 );
        }

        public int AutoBackupIntervalMinutes {
            get {
                if ( chkAutoBackup.Checked ) {
                    return (int)numAutoBackupInterval.Value;
                } else {
                    return 0;
                }
            }
            set {
                if ( value <= 0 ) {
                    chkAutoBackup.Checked = false;
                } else {
                    chkAutoBackup.Checked = true;
                    numAutoBackupInterval.Value = value;
                }
            }
        }

        public boolean SelfDeRomantization {
            get {
                return chkTranslateRoman.Checked;
            }
            set {
                chkTranslateRoman.Checked = value;
            }
        }

        public boolean UseCustomFileDialog {
            get {
                return chkUseCustomFileDialog.Checked;
            }
            set {
                chkUseCustomFileDialog.Checked = value;
            }
        }

        public boolean InvokeWithWine {
            get {
                return chkInvokeWithWine.Checked;
            }
            set {
                chkInvokeWithWine.Checked = value;
            }
        }

        public int MidiInPort {
            get {
                if ( comboMidiInPortNumber.Enabled ) {
                    if ( comboMidiInPortNumber.SelectedIndex >= 0 ) {
                        return comboMidiInPortNumber.SelectedIndex;
                    } else {
                        return 0;
                    }
                } else {
                    return -1;
                }
            }
            set {
                if ( comboMidiInPortNumber.Enabled ) {
                    if ( 0 <= value && value < comboMidiInPortNumber.Items.Count ) {
                        comboMidiInPortNumber.SelectedIndex = value;
                    } else {
                        comboMidiInPortNumber.SelectedIndex = 0;
                    }
                }
            }
        }

        public boolean CurveVisibleVel {
            get {
                return chkVel.Checked;
            }
            set {
                chkVel.Checked = value;
            }
        }

        public boolean CurveVisibleAccent {
            get {
                return chkAccent.Checked;
            }
            set {
                chkAccent.Checked = value;
            }
        }

        public boolean CurveVisibleDecay {
            get {
                return chkDecay.Checked;
            }
            set {
                chkDecay.Checked = value;
            }
        }

        public boolean CurveVisibleVibratoRate {
            get {
                return chkVibratoRate.Checked;
            }
            set {
                chkVibratoRate.Checked = value;
            }
        }

        public boolean CurveVisibleVibratoDepth {
            get {
                return chkVibratoDepth.Checked;
            }
            set {
                chkVibratoDepth.Checked = value;
            }
        }

        public boolean CurveVisibleDyn {
            get {
                return chkDyn.Checked;
            }
            set {
                chkDyn.Checked = value;
            }
        }

        public boolean CurveVisibleBre {
            get {
                return chkBre.Checked;
            }
            set {
                chkBre.Checked = value;
            }
        }

        public boolean CurveVisibleBri {
            get {
                return chkBri.Checked;
            }
            set {
                chkBri.Checked = value;
            }
        }

        public boolean CurveVisibleCle {
            get {
                return chkCle.Checked;
            }
            set {
                chkCle.Checked = value;
            }
        }

        public boolean CurveVisibleOpe {
            get {
                return chkOpe.Checked;
            }
            set {
                chkOpe.Checked = value;
            }
        }

        public boolean CurveVisiblePor {
            get {
                return chkPor.Checked;
            }
            set {
                chkPor.Checked = value;
            }
        }

        public boolean CurveVisibleGen {
            get {
                return chkGen.Checked;
            }
            set {
                chkGen.Checked = value;
            }
        }

        public boolean CurveVisiblePit {
            get {
                return chkPit.Checked;
            }
            set {
                chkPit.Checked = value;
            }
        }

        public boolean CurveVisiblePbs {
            get {
                return chkPbs.Checked;
            }
            set {
                chkPbs.Checked = value;
            }
        }

        public boolean CurveVisibleFx2Depth {
            get {
                return chkFx2Depth.Checked;
            }
            set {
                chkFx2Depth.Checked = value;
            }
        }

        public boolean CurveVisibleHarmonics {
            get {
                return chkHarmonics.Checked;
            }
            set {
                chkHarmonics.Checked = value;
            }
        }

        public boolean CurveVisibleReso1 {
            get {
                return chkReso1.Checked;
            }
            set {
                chkReso1.Checked = value;
            }
        }

        public boolean CurveVisibleReso2 {
            get {
                return chkReso2.Checked;
            }
            set {
                chkReso2.Checked = value;
            }
        }
        
        public boolean CurveVisibleReso3 {
            get {
                return chkReso3.Checked;
            }
            set {
                chkReso3.Checked = value;
            }
        }

        public boolean CurveVisibleReso4 {
            get {
                return chkReso4.Checked;
            }
            set {
                chkReso4.Checked = value;
            }
        }

        public boolean CurveVisibleEnvelope {
            get {
                return chkEnvelope.Checked;
            }
            set {
                chkEnvelope.Checked = value;
            }
        }

        public boolean CurveSelectingQuantized {
            get {
                return chkCurveSelectingQuantized.Checked;
            }
            set {
                chkCurveSelectingQuantized.Checked = value;
            }
        }

        public boolean PlayPreviewWhenRightClick {
            get {
                return chkPlayPreviewWhenRightClick.Checked;
            }
            set {
                chkPlayPreviewWhenRightClick.Checked = value;
            }
        }

        public int MouseHoverTime {
            get {
                return (int)numMouseHoverTime.Value;
            }
            set {
                numMouseHoverTime.Value = value;
            }
        }

        public int PxTrackHeight {
            get {
                return (int)numTrackHeight.Value;
            }
            set {
                numTrackHeight.Value = value;
            }
        }

        public boolean KeepLyricInputMode {
            get {
                return chkKeepLyricInputMode.Checked;
            }
            set {
                chkKeepLyricInputMode.Checked = value;
            }
        }

        public Platform Platform {
            get {
                return m_platform;
            }
            set {
                m_platform = value;
                for ( int i = 0; i < comboPlatform.Items.Count; i++ ) {
                    String title = (String)comboPlatform.Items[i];
                    if ( title.Equals( m_platform + "" ) ) {
                        comboPlatform.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        public int MaximumFrameRate {
            get {
                return (int)numMaximumFrameRate.Value;
            }
            set {
                numMaximumFrameRate.Value = value;
            }
        }

        public boolean ScrollHorizontalOnWheel {
            get {
                return chkScrollHorizontal.Checked;
            }
            set {
                chkScrollHorizontal.Checked = value;
            }
        }

        public void ApplyLanguage() {
            Text = _( "Preference" );
            btnCancel.Text = _( "Cancel" );
            btnOK.Text = _( "OK" );
            try {
                openUtauCore.Filter = _( "Executable(*.exe)|*.exe" ) + "|" + _( "All Files(*.*)|*.*" );
            } catch {
                openUtauCore.Filter = "Executable(*.exe)|*.exe|All Files(*.*)|*.*";
            }

            folderBrowserSingers.Description = _( "Select Singer Directory" );

            #region tabSequence
            tabSequence.Text = _( "Sequence" );
            lblResolution.Text = _( "Resolution(VSTi)" );
            lblDynamics.Text = _( "Dynamics" ) + "(&D)";
            lblAmplitude.Text = _( "Vibrato Depth" ) + "(&R)";
            lblPeriod.Text = _( "Vibrato Rate" ) + "(&V)";
            lblVibratoConfig.Text = _( "Vibrato Settings" );
            lblVibratoLength.Text = _( "Default Vibrato Length" ) + "(&L)";
            groupAutoVibratoConfig.Text = _( "Auto Vibrato Settings" );
            chkEnableAutoVibrato.Text = _( "Enable Automatic Vibrato" ) + "(&E)";
            lblAutoVibratoMinLength.Text = _( "Minimum note length for Automatic Vibrato" ) + "(&M)";
            lblAutoVibratoType1.Text = _( "Vibrato Type" ) + ": VOCALOID1 (&T)";
            lblAutoVibratoType2.Text = _( "Vibrato Type" ) + ": VOCALOID2 (&T)";
            #endregion

            #region tabAudio
            //tabAudio.Text = _( "Audio Settings" );
            #endregion

            #region tabAnother
            tabAnother.Text = _( "Other Settings" );
            lblDefaultSinger.Text = _( "Default Singer" ) + "(&S)";
            lblPreSendTime.Text = _( "Pre-Send time" ) + "(&P)";
            lblWait.Text = _( "Waiting Time" ) + "(&W)";
            lblDefaultPremeasure.Text = _( "Default Pre-measure" ) + "(&M)";
            chkChasePastEvent.Text = _( "Chase Event" ) + "(&C)";
            lblSampleOutput.Text = _( "Playback Sample Sound" );
            chkEnableSampleOutput.Text = _( "Enable" ) + "(&E)";
            lblTiming.Text = _( "Timing" );
            lblPreSendTimeSample.Text = _( "Pre-Send Time for sample sound" ) + "(&G)";
            #endregion

            #region tabAppearance
            tabAppearance.Text = _( "Appearance" );
            groupFont.Text = _( "Font" );
            labelMenu.Text = _( "Menu / Lyrics" );
            labelScreen.Text = _( "Screen" );
            lblLanguage.Text = _( "UI Language" );
            btnChangeMenuFont.Text = _( "Change" );
            btnChangeScreenFont.Text = _( "Change" );
            lblTrackHeight.Text = _( "Track Height (pixel)" );
            groupVisibleCurve.Text = _( "Visible Control Curve" );
            #endregion

            #region tabOperation
            tabOperation.Text = _( "Operation" );
            labelWheelOrder.Text = _( "Mouse wheel Rate" );
            chkCursorFix.Text = _( "Fix Play Cursor to Center" );
            chkScrollHorizontal.Text = _( "Horizontal Scroll when Mouse wheel" );
            lblMaximumFrameRate.Text = _( "Maximum Frame Rate" );
            chkKeepLyricInputMode.Text = _( "Keep Lyric Input Mode" );
            lblMouseHoverTime.Text = _( "Waiting Time for Preview" );
            lblMilliSecond.Text = _( "milli second" );
            chkPlayPreviewWhenRightClick.Text = _( "Play Preview On Right Click" );
            chkCurveSelectingQuantized.Text = _( "Enable Quantize for Curve Selecting" );
            lblMidiInPort.Text = _( "MIDI In Port Number" );

            groupPianoroll.Text = _( "Piano Roll" );
            groupMisc.Text = _( "Misc" );
            #endregion

            #region tabPlatform
            tabPlatform.Text = _( "Platform" );
            
            groupPlatform.Text = _( "Platform" );
            lblPlatform.Text = _( "Current Platform" );
            chkCommandKeyAsControl.Text = _( "Use Command key as Control key" );
            chkUseCustomFileDialog.Text = _( "Use Custom File Dialog" );
            chkTranslateRoman.Text = _( "Translate Roman letters into Kana" );

            groupUtauCores.Text = _( "UTAU Cores" );
            chkInvokeWithWine.Text = _( "Invoke UTAU cores with Wine" );
            #endregion

            #region tabUtauSingers
            tabUtauSingers.Text = _( "UTAU Singers" );
            columnHeaderProgramChange.Text = _( "Program Change" );
            columnHeaderName.Text = _( "Name" );
            columnHeaderPath.Text = _( "Path" );
            btnAdd.Text = _( "Add" );
            btnRemove.Text = _( "Remove" );
            btnUp.Text = _( "Up" );
            btnDown.Text = _( "Down" );
            #endregion

            #region tabFile
            tabFile.Text = _( "File" );
            chkAutoBackup.Text = _( "Automatical Backup" );
            lblAutoBackupInterval.Text = _( "interval" );
            lblAutoBackupMinutes.Text = _( "minute(s)" );
            #endregion
        }

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public String Language {
            get {
                int index = comboLanguage.SelectedIndex;
                if ( 0 <= index && index < comboLanguage.Items.Count ) {
                    String title = (String)comboLanguage.Items[index];
                    if ( title.Equals( "Default" ) ) {
                        return "";
                    } else {
                        return title;
                    }
                } else {
                    return "";
                }
            }
        }

        public ClockResolution ControlCurveResolution {
            get {
                int count = -1;
                int index = comboDynamics.SelectedIndex;
                for ( Iterator itr = ClockResolution.iterator(); itr.hasNext(); ){
                    ClockResolution vt = (ClockResolution)itr.next();
                    count++;
                    if ( count == index ) {
                        return vt;
                    }
                }
                comboDynamics.SelectedIndex = 0;
                return ClockResolution.L30;
            }
            set {
                int count = -1;
                for ( Iterator itr = ClockResolution.iterator(); itr.hasNext(); ){
                    ClockResolution vt = (ClockResolution)itr.next();
                    count++;
                    if ( vt.Equals( value ) ) {
                        comboDynamics.SelectedIndex = count;
                        break;
                    }
                }
            }
        }

        public int PreSendTime {
            get {
                return (int)numPreSendTime.Value;
            }
            set {
                numPreSendTime.Value = value;
            }
        }

        public int PreMeasure {
            get {
                return comboDefaultPremeasure.SelectedIndex + 1;
            }
            set {
                comboDefaultPremeasure.SelectedIndex = value - 1;
            }
        }

        public boolean EnableAutoVibrato {
            get {
                return chkEnableAutoVibrato.Checked;
            }
            set {
                chkEnableAutoVibrato.Checked = value;
            }
        }

        public String AutoVibratoType1 {
            get {
                int count = -1;
                int index = comboAutoVibratoType1.SelectedIndex;
                if( 0 <= index ){
                    VibratoConfig vconfig = (VibratoConfig)comboAutoVibratoType1.SelectedItem;
                    return vconfig.contents.IconID;
                } else {
                    return "$04040001";
                }
            }
            set {
                for ( int i = 0; i < comboAutoVibratoType1.Items.Count; i++ ) {
                    VibratoConfig vconfig = (VibratoConfig)comboAutoVibratoType1.Items[i];
                    if ( vconfig.contents.IconID.Equals( value ) ) {
                        comboAutoVibratoType1.SelectedIndex = i;
                        return;
                    }
                }
                if ( comboAutoVibratoType1.Items.Count > 0 ) {
                    comboAutoVibratoType1.SelectedIndex = 0;
                }
            }
        }

        public String AutoVibratoType2 {
            get {
                int count = -1;
                int index = comboAutoVibratoType2.SelectedIndex;
                if ( 0 <= index ) {
                    VibratoConfig vconfig = (VibratoConfig)comboAutoVibratoType2.SelectedItem;
                    return vconfig.contents.IconID;
                } else {
                    return "$04040001";
                }
            }
            set {
                for ( int i = 0; i < comboAutoVibratoType2.Items.Count; i++ ) {
                    VibratoConfig vconfig = (VibratoConfig)comboAutoVibratoType2.Items[i];
                    if ( vconfig.contents.IconID.Equals( value ) ) {
                        comboAutoVibratoType2.SelectedIndex = i;
                        return;
                    }
                }
                if ( comboAutoVibratoType2.Items.Count > 0 ) {
                    comboAutoVibratoType2.SelectedIndex = 0;
                }
            }
        }

        public AutoVibratoMinLength AutoVibratoMinimumLength {
            get {
                int count = -1;
                int index = comboAutoVibratoMinLength.SelectedIndex;
                foreach ( AutoVibratoMinLength avml in Enum.GetValues( typeof( AutoVibratoMinLength ) ) ) {
                    count++;
                    if ( count == index ) {
                        return avml;
                    }
                }
                comboAutoVibratoMinLength.SelectedIndex = 0;
                return AutoVibratoMinLength.L1;
            }
            set {
                int count = -1;
                foreach ( AutoVibratoMinLength avml in Enum.GetValues( typeof( AutoVibratoMinLength ) ) ) {
                    count++;
                    if ( avml == value ) {
                        comboAutoVibratoMinLength.SelectedIndex = count;
                        break;
                    }
                }
            }
        }

        public DefaultVibratoLength DefaultVibratoLength {
            get {
                int count = -1;
                int index = comboVibratoLength.SelectedIndex;
                foreach ( DefaultVibratoLength vt in Enum.GetValues( typeof( DefaultVibratoLength ) ) ) {
                    count++;
                    if ( index == count ) {
                        return vt;
                    }
                }
                comboVibratoLength.SelectedIndex = 1;
                return DefaultVibratoLength.L66;
            }
            set {
                int count = -1;
                foreach ( DefaultVibratoLength dvl in Enum.GetValues( typeof( DefaultVibratoLength ) ) ) {
                    count++;
                    if ( dvl == value ) {
                        comboVibratoLength.SelectedIndex = count;
                        break;
                    }
                }
            }
        }

        public boolean CursorFixed {
            get {
                return chkCursorFix.Checked;
            }
            set {
                chkCursorFix.Checked = value;
            }
        }

        public int WheelOrder {
            get {
                return (int)numericUpDownEx1.Value;
            }
            set {
                if ( value < numericUpDownEx1.Minimum ) {
                    numericUpDownEx1.Value = numericUpDownEx1.Minimum;
                } else if ( numericUpDownEx1.Maximum < value ) {
                    numericUpDownEx1.Value = numericUpDownEx1.Maximum;
                } else {
                    numericUpDownEx1.Value = value;
                }
            }
        }

        public Font ScreenFont {
            get {
                return m_screen_font;
            }
            set {
                m_screen_font = value;
                labelScreenFontName.Text = m_screen_font.Name;
            }
        }

        public java.awt.Font getBaseFont() {
            return m_base_font;
        }

        public void setBaseFont( java.awt.Font value ) {
            m_base_font = value;
            labelMenuFontName.Text = m_base_font.getName();
            UpdateFonts( m_base_font.getName() );
        }

        public String DefaultSingerName {
            get {
                if ( comboDefualtSinger.SelectedIndex >= 0 ) {
                    return m_program_change.get( comboDefualtSinger.SelectedIndex );
                } else {
                    return "Miku";
                }
            }
            set {
                int index = -1;
                for ( int i = 0; i < m_program_change.size(); i++ ) {
                    if ( m_program_change.get( i ).Equals( value ) ) {
                        index = i;
                        break;
                    }
                }
                if ( index >= 0 ) {
                    comboDefualtSinger.SelectedIndex = index;
                }
            }
        }

        private void btnChangeMenuFont_Click( object sender, EventArgs e ) {
            fontDialog.Font = getBaseFont().font;
            if ( fontDialog.ShowDialog() == DialogResult.OK ) {
                m_base_font.font = (Font)fontDialog.Font.Clone();
            }
        }

        private void btnOK_Click( object sender, EventArgs e ) {
            this.DialogResult = DialogResult.OK;
        }

        private void btnChangeScreenFont_Click( object sender, EventArgs e ) {
            fontDialog.Font = ScreenFont;
            if ( fontDialog.ShowDialog() == DialogResult.OK ) {
                ScreenFont = (Font)fontDialog.Font.Clone();
            }
        }

        void UpdateFonts( String font_name ) {
            if ( font_name.Equals( "" ) ) {
                return;
            }
            Font font = new Font( font_name, tabSequence.Font.Size, tabSequence.Font.Unit );
            foreach ( Control ctrl in this.Controls ) {
                ctrl.Font = font;
            }
        }

        private void comboPlatform_SelectedIndexChanged( object sender, EventArgs e ) {
            String title = (String)comboPlatform.SelectedItem;
            foreach ( Platform p in Enum.GetValues( typeof( Platform ) ) ) {
                if ( title.Equals( p + "" ) ) {
                    m_platform = p;
                    chkCommandKeyAsControl.Enabled = p != Platform.Windows;
                    chkUseCustomFileDialog.Enabled = p != Platform.Windows;
                    break;
                }
            }
        }

        public boolean CommandKeyAsControl {
            get {
                return chkCommandKeyAsControl.Checked;
            }
            set {
                chkCommandKeyAsControl.Checked = value;
            }
        }

        private void btnResampler_Click( object sender, EventArgs e ) {
            if ( txtResampler.Text != "" && Directory.Exists( Path.GetDirectoryName( txtResampler.Text ) ) ) {
                openUtauCore.InitialDirectory = Path.GetDirectoryName( txtResampler.Text );
            }
            openUtauCore.FileName = "resampler.exe";
            DialogResult dr = DialogResult.Cancel;
            if ( AppManager.editorConfig.UseCustomFileDialog ) {
                using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Open ) ) {
                    fd.InitialDirectory = openUtauCore.InitialDirectory;
                    fd.Filter = openUtauCore.Filter;
                    dr = fd.ShowDialog();
                    if ( dr == DialogResult.OK ) {
                        openUtauCore.FileName = fd.FileName;
                    }
                }
            } else {
                dr = openUtauCore.ShowDialog();
            }
            if ( dr == DialogResult.OK ) {
                txtResampler.Text = openUtauCore.FileName;
            }
        }

        private void btnWavtool_Click( object sender, EventArgs e ) {
            if ( txtWavtool.Text != "" && Directory.Exists( Path.GetDirectoryName( txtWavtool.Text ) ) ) {
                openUtauCore.InitialDirectory = Path.GetDirectoryName( txtWavtool.Text );
            }
            openUtauCore.FileName = "wavtool.exe";
            DialogResult dr = DialogResult.Cancel;
            if ( AppManager.editorConfig.UseCustomFileDialog ) {
                using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Open ) ) {
                    fd.InitialDirectory = openUtauCore.InitialDirectory;
                    fd.Filter = openUtauCore.Filter;
                    dr = fd.ShowDialog();
                    if ( dr == DialogResult.OK ) {
                        openUtauCore.FileName = fd.FileName;
                    }
                }
            } else {
                dr = openUtauCore.ShowDialog();
            }
            if ( dr == DialogResult.OK ) {
                txtWavtool.Text = openUtauCore.FileName;
            }
        }

        public String PathResampler {
            get {
                return txtResampler.Text;
            }
            set {
                txtResampler.Text = value;
            }
        }

        public String PathWavtool {
            get {
                return txtWavtool.Text;
            }
            set {
                txtWavtool.Text = value;
            }
        }

        public Vector<SingerConfig> UtauSingers {
            get {
                return m_utau_singers;
            }
            set {
                m_utau_singers.clear();
                for ( int i = 0; i < value.size(); i++ ) {
                    m_utau_singers.add( (SingerConfig)value.get( i ).clone() );
                }
                UpdateUtauSingerList();
            }
        }

        private void UpdateUtauSingerList() {
            listSingers.Items.Clear();
            for ( int i = 0; i < m_utau_singers.size(); i++ ) {
                m_utau_singers.get( i ).Program = i;
                listSingers.Items.Add( new ListViewItem( new String[] { m_utau_singers.get( i ).Program.ToString(),
                                                                        m_utau_singers.get( i ).VOICENAME, 
                                                                        m_utau_singers.get( i ).VOICEIDSTR } ) );
            }
        }

        private void btnAdd_Click( object sender, EventArgs e ) {
            if ( folderBrowserSingers.ShowDialog() == DialogResult.OK ) {
                String dir = folderBrowserSingers.SelectedPath;
                SingerConfig sc = new SingerConfig();
                String character = Path.Combine( dir, "character.txt" );
                String name = "";
                sc.VOICEIDSTR = dir;
                if ( PortUtil.isFileExists( character ) ) {
                    using ( cp932reader sr = new cp932reader( character ) ) {
                        while ( sr.Peek() >= 0 ) {
                            String line = sr.ReadLine();
                            String[] spl = line.Split( '=' );
                            if ( spl.Length >= 2 ) {
                                if ( spl[0].ToLower().Equals( "name" ) ) {
                                    name = spl[1];
                                    break;
                                }
                            }
                        }
                    }
                }
                if ( name.Equals( "" ) ) {
                    name = "Unknown";
                }
                sc.VOICENAME = name;
                m_utau_singers.add( sc );
                UpdateUtauSingerList();
            }
        }

        private int UtauSingersSelectedIndex {
            get {
                int count = listSingers.SelectedIndices.Count;
                if ( count <= 0 ) {
                    return -1;
                } else {
                    return listSingers.SelectedIndices[0];
                }
            }
        }

        private void listSingers_SelectedIndexChanged( object sender, EventArgs e ) {
            int index = UtauSingersSelectedIndex;
            if ( index < 0 ) {
                btnRemove.Enabled = false;
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            } else {
                btnRemove.Enabled = true;
                btnUp.Enabled = 0 <= index - 1 && index - 1 < m_utau_singers.size();
                btnDown.Enabled = 0 <= index + 1 && index + 1 < m_utau_singers.size();
            }
        }

        private void btnRemove_Click( object sender, EventArgs e ) {
            int index = UtauSingersSelectedIndex;
            if ( 0 <= index && index < m_utau_singers.size() ) {
                m_utau_singers.removeElementAt( index );
            }
            UpdateUtauSingerList();
        }

        private void btnDown_Click( object sender, EventArgs e ) {
            int index = UtauSingersSelectedIndex;
#if DEBUG
            AppManager.debugWriteLine( "Preference.btnDown_Click; index=" + index );
#endif
            if ( 0 <= index && index + 1 < m_utau_singers.size() ) {
                SingerConfig buf = (SingerConfig)m_utau_singers.get( index ).clone();
                m_utau_singers.set( index, (SingerConfig)m_utau_singers.get( index + 1 ).clone() );
                m_utau_singers.set( index + 1, buf );
                UpdateUtauSingerList();
                listSingers.Items[index + 1].Selected = true;
            }
        }

        private void btnUp_Click( object sender, EventArgs e ) {
            int index = UtauSingersSelectedIndex;
#if DEBUG
            AppManager.debugWriteLine( "Preference.btnUp_Click; index=" + index );
#endif
            if ( 0 <= index - 1 && index < m_utau_singers.size() ) {
                SingerConfig buf = (SingerConfig)m_utau_singers.get( index ).clone();
                m_utau_singers.set( index, (SingerConfig)m_utau_singers.get( index - 1 ).clone() );
                m_utau_singers.set( index - 1, buf );
                UpdateUtauSingerList();
                listSingers.Items[index - 1].Selected = true;
            }
        }

        private void chkAutoBackup_CheckedChanged( object sender, EventArgs e ) {
            numAutoBackupInterval.Enabled = chkAutoBackup.Checked;
        }
    }

}
