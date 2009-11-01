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
#if JAVA
package org.kbinani.Cadencii;

#else
using System;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using Boare.Lib.Media;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.awt;
using bocoree.util;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
    using java = bocoree;
#endif

#if JAVA
    public class Preference extends BForm {
#else
    partial class Preference : BForm {
#endif
        Font m_base_font;
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
            for ( Iterator itr = ClockResolution.iterator(); itr.hasNext(); ) {
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

        public bool isUseSpaceKeyAsMiddleButtonModifier() {
            return chkUseSpaceKeyAsMiddleButtonModifier.Checked;
        }

        public void setUseSpaceKeyAsMiddleButtonModifier( boolean value ) {
            chkUseSpaceKeyAsMiddleButtonModifier.Checked = value;
        }

        public int getAutoBackupIntervalMinutes() {
            if ( chkAutoBackup.Checked ) {
                return (int)numAutoBackupInterval.Value;
            } else {
                return 0;
            }
        }

        public void setAutoBackupIntervalMinutes( int value ) {
            if ( value <= 0 ) {
                chkAutoBackup.Checked = false;
            } else {
                chkAutoBackup.Checked = true;
                numAutoBackupInterval.Value = value;
            }
        }

        public boolean isSelfDeRomantization() {
            return chkTranslateRoman.Checked;
        }

        public void setSelfDeRomantization( boolean value ) {
            chkTranslateRoman.Checked = value;
        }

        public boolean isUseCustomFileDialog() {
            return chkUseCustomFileDialog.Checked;
        }

        public void setUseCustomFileDialog( boolean value ) {
            chkUseCustomFileDialog.Checked = value;
        }

        public boolean isInvokeWithWine() {
            return chkInvokeWithWine.Checked;
        }

        public void setInvokeWithWine( boolean value ) {
            chkInvokeWithWine.Checked = value;
        }

        public int getMidiInPort() {
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

        public void setMidiInPort( int value ) {
            if ( comboMidiInPortNumber.Enabled ) {
                if ( 0 <= value && value < comboMidiInPortNumber.Items.Count ) {
                    comboMidiInPortNumber.SelectedIndex = value;
                } else {
                    comboMidiInPortNumber.SelectedIndex = 0;
                }
            }
        }

        public boolean isCurveVisibleVel() {
            return chkVel.Checked;
        }

        public void setCurveVisibleVel( boolean value ) {
            chkVel.Checked = value;
        }

        public boolean isCurveVisibleAccent() {
            return chkAccent.Checked;
        }

        public void setCurveVisibleAccent( boolean value ) {
            chkAccent.Checked = value;
        }

        public boolean isCurveVisibleDecay() {
            return chkDecay.Checked;
        }

        public void setCurveVisibleDecay( boolean value ) {
            chkDecay.Checked = value;
        }

        public boolean isCurveVisibleVibratoRate() {
            return chkVibratoRate.Checked;
        }

        public void setCurveVisibleVibratoRate( boolean value ) {
            chkVibratoRate.Checked = value;
        }

        public boolean isCurveVisibleVibratoDepth() {
            return chkVibratoDepth.Checked;
        }

        public void setCurveVisibleVibratoDepth( boolean value ) {
            chkVibratoDepth.Checked = value;
        }

        public boolean isCurveVisibleDyn() {
            return chkDyn.Checked;
        }

        public void setCurveVisibleDyn( boolean value ) {
            chkDyn.Checked = value;
        }

        public boolean isCurveVisibleBre() {
            return chkBre.Checked;
        }

        public void setCurveVisibleBre( boolean value ) {
            chkBre.Checked = value;
        }

        public boolean isCurveVisibleBri() {
            return chkBri.Checked;
        }

        public void setCurveVisibleBri( boolean value ) {
            chkBri.Checked = value;
        }

        public boolean isCurveVisibleCle() {
            return chkCle.Checked;
        }

        public void setCurveVisibleCle( boolean value ) {
            chkCle.Checked = value;
        }

        public boolean isCurveVisibleOpe() {
            return chkOpe.Checked;
        }

        public void setCurveVisibleOpe( boolean value ) {
            chkOpe.Checked = value;
        }

        public boolean isCurveVisiblePor() {
            return chkPor.Checked;
        }

        public void setCurveVisiblePor( boolean value ) {
            chkPor.Checked = value;
        }

        public boolean isCurveVisibleGen() {
            return chkGen.Checked;
        }

        public void setCurveVisibleGen( boolean value ) {
            chkGen.Checked = value;
        }

        public boolean isCurveVisiblePit() {
            return chkPit.Checked;
        }

        public void setCurveVisiblePit( boolean value ) {
            chkPit.Checked = value;
        }

        public boolean isCurveVisiblePbs() {
            return chkPbs.Checked;
        }

        public void setCurveVisiblePbs( boolean value ) {
            chkPbs.Checked = value;
        }

        public boolean isCurveVisibleFx2Depth() {
            return chkFx2Depth.Checked;
        }

        public void setCurveVisibleFx2Depth( boolean value ) {
            chkFx2Depth.Checked = value;
        }

        public boolean isCurveVisibleHarmonics() {
            return chkHarmonics.Checked;
        }

        public void setCurveVisibleHarmonics( boolean value ) {
            chkHarmonics.Checked = value;
        }

        public boolean isCurveVisibleReso1() {
            return chkReso1.Checked;
        }

        public void setCurveVisibleReso1( boolean value ) {
            chkReso1.Checked = value;
        }

        public boolean isCurveVisibleReso2() {
            return chkReso2.Checked;
        }

        public void setCurveVisibleReso2( boolean value ) {
            chkReso2.Checked = value;
        }

        public boolean isCurveVisibleReso3() {
            return chkReso3.Checked;
        }

        public void setCurveVisibleReso3( boolean value ) {
            chkReso3.Checked = value;
        }

        public boolean isCurveVisibleReso4() {
            return chkReso4.Checked;
        }

        public void setCurveVisibleReso4( boolean value ) {
            chkReso4.Checked = value;
        }

        public boolean isCurveVisibleEnvelope() {
            return chkEnvelope.Checked;
        }

        public void setCurveVisibleEnvelope( boolean value ) {
            chkEnvelope.Checked = value;
        }

        public boolean isCurveSelectingQuantized() {
            return chkCurveSelectingQuantized.Checked;
        }

        public void setCurveSelectingQuantized( boolean value ) {
            chkCurveSelectingQuantized.Checked = value;
        }

        public boolean isPlayPreviewWhenRightClick() {
            return chkPlayPreviewWhenRightClick.Checked;
        }

        public void setPlayPreviewWhenRightClick( boolean value ) {
            chkPlayPreviewWhenRightClick.Checked = value;
        }

        public int getMouseHoverTime() {
            return (int)numMouseHoverTime.Value;
        }

        public void setMouseHoverTime( int value ) {
            numMouseHoverTime.Value = value;
        }

        public int getPxTrackHeight() {
            return (int)numTrackHeight.Value;
        }

        public void setPxTrackHeight( int value ) {
            numTrackHeight.Value = value;
        }

        public boolean isKeepLyricInputMode() {
            return chkKeepLyricInputMode.Checked;
        }

        public void setKeepLyricInputMode( boolean value ) {
            chkKeepLyricInputMode.Checked = value;
        }

        public Platform getPlatform() {
            return m_platform;
        }

        public void setPlatform( Platform value ) {
            m_platform = value;
            for ( int i = 0; i < comboPlatform.Items.Count; i++ ) {
                String title = (String)comboPlatform.Items[i];
                if ( title.Equals( m_platform + "" ) ) {
                    comboPlatform.SelectedIndex = i;
                    break;
                }
            }
        }

        public int getMaximumFrameRate() {
            return (int)numMaximumFrameRate.Value;
        }

        public void setMaximumFrameRate( int value ) {
            numMaximumFrameRate.Value = value;
        }

        public boolean isScrollHorizontalOnWheel() {
            return chkScrollHorizontal.Checked;
        }

        public void setScrollHorizontalOnWheel( boolean value ) {
            chkScrollHorizontal.Checked = value;
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
            chkUseSpaceKeyAsMiddleButtonModifier.Text = _( "Use space key as Middle button modifier" );

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

        public String getLanguage() {
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

        public ClockResolution getControlCurveResolution() {
            int count = -1;
            int index = comboDynamics.SelectedIndex;
            for ( Iterator itr = ClockResolution.iterator(); itr.hasNext(); ) {
                ClockResolution vt = (ClockResolution)itr.next();
                count++;
                if ( count == index ) {
                    return vt;
                }
            }
            comboDynamics.SelectedIndex = 0;
            return ClockResolution.L30;
        }

        public void setControlCurveResolution( ClockResolution value ) {
            int count = -1;
            for ( Iterator itr = ClockResolution.iterator(); itr.hasNext(); ) {
                ClockResolution vt = (ClockResolution)itr.next();
                count++;
                if ( vt.Equals( value ) ) {
                    comboDynamics.SelectedIndex = count;
                    break;
                }
            }
        }

        public int getPreSendTime() {
            return (int)numPreSendTime.Value;
        }

        public void setPreSendTime( int value ) {
            numPreSendTime.Value = value;
        }

        public int getPreMeasure() {
            return comboDefaultPremeasure.SelectedIndex + 1;
        }

        public void setPreMeasure( int value ) {
            comboDefaultPremeasure.SelectedIndex = value - 1;
        }

        public boolean isEnableAutoVibrato() {
            return chkEnableAutoVibrato.Checked;
        }

        public void setEnableAutoVibrato( boolean value ) {
            chkEnableAutoVibrato.Checked = value;
        }

        public String getAutoVibratoType1() {
            int count = -1;
            int index = comboAutoVibratoType1.SelectedIndex;
            if ( 0 <= index ) {
                VibratoConfig vconfig = (VibratoConfig)comboAutoVibratoType1.SelectedItem;
                return vconfig.contents.IconID;
            } else {
                return "$04040001";
            }
        }

        public void setAutoVibratoType1( String value ) {
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

        public String getAutoVibratoType2() {
            int count = -1;
            int index = comboAutoVibratoType2.SelectedIndex;
            if ( 0 <= index ) {
                VibratoConfig vconfig = (VibratoConfig)comboAutoVibratoType2.SelectedItem;
                return vconfig.contents.IconID;
            } else {
                return "$04040001";
            }
        }

        public void setAutoVibratoType2( String value ) {
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

        public AutoVibratoMinLength getAutoVibratoMinimumLength() {
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

        public void setAutoVibratoMinimumLength( AutoVibratoMinLength value ) {
            int count = -1;
            foreach ( AutoVibratoMinLength avml in Enum.GetValues( typeof( AutoVibratoMinLength ) ) ) {
                count++;
                if ( avml == value ) {
                    comboAutoVibratoMinLength.SelectedIndex = count;
                    break;
                }
            }
        }

        public DefaultVibratoLength getDefaultVibratoLength() {
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

        public void setDefaultVibratoLength( DefaultVibratoLength value ) {
            int count = -1;
            foreach ( DefaultVibratoLength dvl in Enum.GetValues( typeof( DefaultVibratoLength ) ) ) {
                count++;
                if ( dvl == value ) {
                    comboVibratoLength.SelectedIndex = count;
                    break;
                }
            }
        }

        public boolean isCursorFixed() {
            return chkCursorFix.Checked;
        }

        public void setCursorFixed( boolean value ) {
            chkCursorFix.Checked = value;
        }

        public int getWheelOrder() {
            return (int)numericUpDownEx1.Value;
        }

        public void setWheelOrder( int value ) {
            if ( value < numericUpDownEx1.Minimum ) {
                numericUpDownEx1.Value = numericUpDownEx1.Minimum;
            } else if ( numericUpDownEx1.Maximum < value ) {
                numericUpDownEx1.Value = numericUpDownEx1.Maximum;
            } else {
                numericUpDownEx1.Value = value;
            }
        }

        public Font getScreenFont() {
            return m_screen_font;
        }

        public void setScreenFont( Font value ) {
            m_screen_font = value;
            labelScreenFontName.Text = m_screen_font.getName();
        }

        public java.awt.Font getBaseFont() {
            return m_base_font;
        }

        public void setBaseFont( java.awt.Font value ) {
            m_base_font = value;
            labelMenuFontName.Text = m_base_font.getName();
            UpdateFonts( m_base_font.getName() );
        }

        public String getDefaultSingerName() {
            if ( comboDefualtSinger.SelectedIndex >= 0 ) {
                return m_program_change.get( comboDefualtSinger.SelectedIndex );
            } else {
                return "Miku";
            }
        }

        public void setDefaultSingerName( String value ) {
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

        private void btnChangeMenuFont_Click( object sender, EventArgs e ) {
            fontDialog.Font = getBaseFont().font;
            if ( fontDialog.ShowDialog() == DialogResult.OK ) {
                m_base_font.font = (System.Drawing.Font)fontDialog.Font.Clone();
            }
        }

        private void btnOK_Click( object sender, EventArgs e ) {
            this.DialogResult = DialogResult.OK;
        }

        private void btnChangeScreenFont_Click( object sender, EventArgs e ) {
            fontDialog.Font = m_screen_font.font;
            if ( fontDialog.ShowDialog() == DialogResult.OK ) {
                m_screen_font = (Font)fontDialog.Font.Clone();
            }
        }

        void UpdateFonts( String font_name ) {
            if ( font_name.Equals( "" ) ) {
                return;
            }
            Font font = new Font( font_name, java.awt.Font.PLAIN, (int)tabSequence.Font.Size );
            Util.applyFontRecurse( this, font );
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

        public boolean isCommandKeyAsControl() {
            return chkCommandKeyAsControl.Checked;
        }

        public void setCommandKeyAsControl( boolean value ) {
            chkCommandKeyAsControl.Checked = value;
        }

        private void btnResampler_Click( object sender, EventArgs e ) {
            if ( txtResampler.Text != "" && PortUtil.isDirectoryExists( PortUtil.getDirectoryName( txtResampler.Text ) ) ) {
                openUtauCore.InitialDirectory = PortUtil.getDirectoryName( txtResampler.Text );
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
            if ( txtWavtool.Text != "" && PortUtil.isDirectoryExists( PortUtil.getDirectoryName( txtWavtool.Text ) ) ) {
                openUtauCore.InitialDirectory = PortUtil.getDirectoryName( txtWavtool.Text );
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

        public String getPathResampler() {
            return txtResampler.Text;
        }

        public void setPathResampler( String value ) {
            txtResampler.Text = value;
        }

        public String getPathWavtool() {
            return txtWavtool.Text;
        }

        public void setPathWavtool( String value ) {
            txtWavtool.Text = value;
        }

        public Vector<SingerConfig> getUtauSingers() {
            return m_utau_singers;
        }

        public void setUtauSingers( Vector<SingerConfig> value ) {
            m_utau_singers.clear();
            for ( int i = 0; i < value.size(); i++ ) {
                m_utau_singers.add( (SingerConfig)value.get( i ).clone() );
            }
            UpdateUtauSingerList();
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
                String character = PortUtil.combinePath( dir, "character.txt" );
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

        private int getUtauSingersSelectedIndex() {
            int count = listSingers.SelectedIndices.Count;
            if ( count <= 0 ) {
                return -1;
            } else {
                return listSingers.SelectedIndices[0];
            }
        }

        private void listSingers_SelectedIndexChanged( object sender, EventArgs e ) {
            int index = getUtauSingersSelectedIndex();
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
            int index = getUtauSingersSelectedIndex();
            if ( 0 <= index && index < m_utau_singers.size() ) {
                m_utau_singers.removeElementAt( index );
            }
            UpdateUtauSingerList();
        }

        private void btnDown_Click( object sender, EventArgs e ) {
            int index = getUtauSingersSelectedIndex();
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
            int index = getUtauSingersSelectedIndex();
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

#if !JAVA
}
#endif
