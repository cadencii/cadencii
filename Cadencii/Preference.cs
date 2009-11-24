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

import java.awt.*;
import java.util.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.media.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using Boare.Lib.Media;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.java.awt;
using bocoree.java.io;
using bocoree.java.util;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
    using java = bocoree.java;
#endif

#if JAVA
    public class Preference extends BForm {
#else
    class Preference : BForm {
#endif
        Font m_base_font;
        Font m_screen_font;
        Font m_counter_font;
        Vector<String> m_program_change = null;
        private PlatformEnum m_platform = PlatformEnum.Windows;
        private Vector<SingerConfig> m_utau_singers = new Vector<SingerConfig>();
        private BFileChooser openUtauCore;
        private static int columnWidthHeaderProgramChange = 60;
        private static int columnWidthHeaderName = 100;
        private static int columnWidthHeaderPath = 250;
        private BFontChooser fontDialog;
        private BFolderBrowser folderBrowserSingers;

        public Preference() {
#if JAVA
            super();
            initialize();
            tabPane = new JTabbedPane();
            tabPane.addTab( "Sequence", getTabSequence() );
            tabPane.addTab( "Other Settings", getTabAnother() );
            tabPane.addTab( "Appearance", getTabAppearance() );
            tabPane.addTab( "Operation", getTabOperation() );
            tabPane.addTab( "Platform", getTabPlatform() );
            tabPane.addTab( "UTAU Singers", getTabUtauSingers() );
            tabPane.addTab( "File", getTabFile() );
            GridBagLayout layout = new GridBagLayout();
            getPanelUpper().setLayout( layout );
            GridBagConstraints gb = new GridBagConstraints();
            gb.weighty = 1;
            gb.weightx = 1;
            gb.insets = new Insets( 12, 12, 0, 12 );
            gb.fill = GridBagConstraints.BOTH;
            layout.setConstraints( tabPane, gb );
            this.getPanelUpper().add( tabPane );
            try{
                //UIManager.setLookAndFeel( "com.sun.java.swing.plaf.mac.MacLookAndFeel" );
                UIManager.setLookAndFeel( "com.sun.java.swing.plaf.windows.WindowsLookAndFeel" );
                SwingUtilities.updateComponentTreeUI( this );
            }catch( Exception ex ){
                System.err.println( "Preference#.ctor; ex=" + ex );
            }
#else
            InitializeComponent();
#endif
            fontDialog = new BFontChooser();
#if !JAVA
            fontDialog.dialog.AllowVectorFonts = false;
            fontDialog.dialog.AllowVerticalFonts = false;
            fontDialog.dialog.FontMustExist = true;
            fontDialog.dialog.ShowEffects = false;
#endif
            openUtauCore = new BFileChooser( "" );
            folderBrowserSingers = new BFolderBrowser();
            folderBrowserSingers.setNewFolderButtonVisible( false );
            ApplyLanguage();

            comboVibratoLength.removeAllItems();
            foreach ( DefaultVibratoLengthEnum dvl in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) ) {
                comboVibratoLength.addItem( DefaultVibratoLengthUtil.toString( dvl ) );
            }
            comboVibratoLength.setSelectedIndex( 1 );

            comboAutoVibratoMinLength.removeAllItems();
            foreach ( AutoVibratoMinLengthEnum avml in Enum.GetValues( typeof( AutoVibratoMinLengthEnum ) ) ) {
                comboAutoVibratoMinLength.addItem( AutoVibratoMinLengthUtil.toString( avml ) );
            }
            comboAutoVibratoMinLength.setSelectedIndex( 0 );

            comboAutoVibratoType1.removeAllItems();
            for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( SynthesizerType.VOCALOID1 ); itr.hasNext(); ) {
                VibratoConfig vconfig = (VibratoConfig)itr.next();
                comboAutoVibratoType1.addItem( vconfig );
            }
            if ( comboAutoVibratoType1.getItemCount() > 0 ) {
                comboAutoVibratoType1.setSelectedIndex( 0 );
            }

            comboAutoVibratoType2.removeAllItems();
            for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( SynthesizerType.VOCALOID2 ); itr.hasNext(); ) {
                VibratoConfig vconfig = (VibratoConfig)itr.next();
                comboAutoVibratoType2.addItem( vconfig );
            }
            if ( comboAutoVibratoType2.getItemCount() > 0 ) {
                comboAutoVibratoType2.setSelectedIndex( 0 );
            }

            comboDynamics.removeAllItems();
            comboAmplitude.removeAllItems();
            comboPeriod.removeAllItems();
            for ( Iterator itr = ClockResolution.iterator(); itr.hasNext(); ) {
                ClockResolution cr = (ClockResolution)itr.next();
                comboDynamics.addItem( cr.ToString() );
                comboAmplitude.addItem( cr.ToString() );
                comboPeriod.addItem( cr.ToString() );
            }
            comboDynamics.setSelectedIndex( 0 );
            comboAmplitude.setSelectedIndex( 0 );
            comboPeriod.setSelectedIndex( 0 );

            comboLanguage.removeAllItems();
            String[] list = Messaging.getRegisteredLanguage();
            int index = 0;
            comboLanguage.addItem( "Default" );
            int count = 0;
            foreach ( String s in list ) {
                count++;
                comboLanguage.addItem( s );
                if ( s.Equals( Messaging.getLanguage() ) ) {
                    index = count;
                }
            }
            comboLanguage.setSelectedIndex( index );

            SingerConfig[] dict = VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID2 );
            m_program_change = new Vector<String>();
            comboDefualtSinger.removeAllItems();
            foreach ( SingerConfig kvp in dict ) {
                m_program_change.add( kvp.VOICENAME );
                comboDefualtSinger.addItem( kvp.VOICENAME );
            }
            comboDefualtSinger.setEnabled( (comboDefualtSinger.getItemCount() > 0) );
            if ( comboDefualtSinger.getItemCount() > 0 ) {
                comboDefualtSinger.setSelectedIndex( 0 );
            }

            comboPlatform.removeAllItems();
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
            foreach ( PlatformEnum p in Enum.GetValues( typeof( PlatformEnum ) ) ) {
                comboPlatform.addItem( p + "" );
            }
#if !DEBUG
            }
#endif

            comboMidiInPortNumber.removeAllItems();
#if ENABLE_MIDI
            bocoree.MIDIINCAPS[] midiins = MidiInDevice.GetMidiInDevices();
            for ( int i = 0; i < midiins.Length; i++ ) {
                comboMidiInPortNumber.addItem( midiins[i] );
            }
            if ( comboMidiInPortNumber.getItemCount() == 0 ) {
                comboMidiInPortNumber.setEnabled( false );
            } else {
                comboMidiInPortNumber.setEnabled( true );
            }
#else
            comboMidiInPortNumber.Enabled = false;
#endif

            txtVOCALOID1.setText( VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID1 ) );
            txtVOCALOID2.setText( VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 ) );

            listSingers.setColumnWidth( 0, columnWidthHeaderProgramChange );
            listSingers.setColumnWidth( 1, columnWidthHeaderName );
            listSingers.setColumnWidth( 2, columnWidthHeaderPath );

            registerEventHandlers();
            setResources();
        }

        public boolean isUseSpaceKeyAsMiddleButtonModifier() {
            return chkUseSpaceKeyAsMiddleButtonModifier.isSelected();
        }

        public void setUseSpaceKeyAsMiddleButtonModifier( boolean value ) {
            chkUseSpaceKeyAsMiddleButtonModifier.setSelected( value );
        }

        public int getAutoBackupIntervalMinutes() {
            if ( chkAutoBackup.isSelected() ) {
                return (int)numAutoBackupInterval.Value;
            } else {
                return 0;
            }
        }

        public void setAutoBackupIntervalMinutes( int value ) {
            if ( value <= 0 ) {
                chkAutoBackup.setSelected( false );
            } else {
                chkAutoBackup.setSelected( true );
                numAutoBackupInterval.Value = value;
            }
        }

        public boolean isSelfDeRomantization() {
            return chkTranslateRoman.isSelected();
        }

        public void setSelfDeRomantization( boolean value ) {
            chkTranslateRoman.setSelected( value );
        }

        public boolean isInvokeWithWine() {
            return chkInvokeWithWine.isSelected();
        }

        public void setInvokeWithWine( boolean value ) {
            chkInvokeWithWine.setSelected( value );
        }

#if ENABLE_MIDI
        public int getMidiInPort() {
            if ( comboMidiInPortNumber.isEnabled() ) {
                if ( comboMidiInPortNumber.getSelectedIndex() >= 0 ) {
                    return comboMidiInPortNumber.getSelectedIndex();
                } else {
                    return 0;
                }
            } else {
                return -1;
            }
        }
#endif

#if ENABLE_MIDI
        public void setMidiInPort( int value ) {
            if ( comboMidiInPortNumber.isEnabled() ) {
                if ( 0 <= value && value < comboMidiInPortNumber.getItemCount() ) {
                    comboMidiInPortNumber.setSelectedIndex( value );
                } else {
                    comboMidiInPortNumber.setSelectedIndex( 0 );
                }
            }
        }
#endif

        public boolean isCurveVisibleVel() {
            return chkVel.isSelected();
        }

        public void setCurveVisibleVel( boolean value ) {
            chkVel.setSelected( value );
        }

        public boolean isCurveVisibleAccent() {
            return chkAccent.isSelected();
        }

        public void setCurveVisibleAccent( boolean value ) {
            chkAccent.setSelected( value );
        }

        public boolean isCurveVisibleDecay() {
            return chkDecay.isSelected();
        }

        public void setCurveVisibleDecay( boolean value ) {
            chkDecay.setSelected( value );
        }

        public boolean isCurveVisibleVibratoRate() {
            return chkVibratoRate.isSelected();
        }

        public void setCurveVisibleVibratoRate( boolean value ) {
            chkVibratoRate.setSelected( value );
        }

        public boolean isCurveVisibleVibratoDepth() {
            return chkVibratoDepth.isSelected();
        }

        public void setCurveVisibleVibratoDepth( boolean value ) {
            chkVibratoDepth.setSelected( value );
        }

        public boolean isCurveVisibleDyn() {
            return chkDyn.isSelected();
        }

        public void setCurveVisibleDyn( boolean value ) {
            chkDyn.setSelected( value );
        }

        public boolean isCurveVisibleBre() {
            return chkBre.isSelected();
        }

        public void setCurveVisibleBre( boolean value ) {
            chkBre.setSelected( value );
        }

        public boolean isCurveVisibleBri() {
            return chkBri.isSelected();
        }

        public void setCurveVisibleBri( boolean value ) {
            chkBri.setSelected( value );
        }

        public boolean isCurveVisibleCle() {
            return chkCle.isSelected();
        }

        public void setCurveVisibleCle( boolean value ) {
            chkCle.setSelected( value );
        }

        public boolean isCurveVisibleOpe() {
            return chkOpe.isSelected();
        }

        public void setCurveVisibleOpe( boolean value ) {
            chkOpe.setSelected( value );
        }

        public boolean isCurveVisiblePor() {
            return chkPor.isSelected();
        }

        public void setCurveVisiblePor( boolean value ) {
            chkPor.setSelected( value );
        }

        public boolean isCurveVisibleGen() {
            return chkGen.isSelected();
        }

        public void setCurveVisibleGen( boolean value ) {
            chkGen.setSelected( value );
        }

        public boolean isCurveVisiblePit() {
            return chkPit.isSelected();
        }

        public void setCurveVisiblePit( boolean value ) {
            chkPit.setSelected( value );
        }

        public boolean isCurveVisiblePbs() {
            return chkPbs.isSelected();
        }

        public void setCurveVisiblePbs( boolean value ) {
            chkPbs.setSelected( value );
        }

        public boolean isCurveVisibleFx2Depth() {
            return chkFx2Depth.isSelected();
        }

        public void setCurveVisibleFx2Depth( boolean value ) {
            chkFx2Depth.setSelected( value );
        }

        public boolean isCurveVisibleHarmonics() {
            return chkHarmonics.isSelected();
        }

        public void setCurveVisibleHarmonics( boolean value ) {
            chkHarmonics.setSelected( value );
        }

        public boolean isCurveVisibleReso1() {
            return chkReso1.isSelected();
        }

        public void setCurveVisibleReso1( boolean value ) {
            chkReso1.setSelected( value );
        }

        public boolean isCurveVisibleReso2() {
            return chkReso2.isSelected();
        }

        public void setCurveVisibleReso2( boolean value ) {
            chkReso2.setSelected( value );
        }

        public boolean isCurveVisibleReso3() {
            return chkReso3.isSelected();
        }

        public void setCurveVisibleReso3( boolean value ) {
            chkReso3.setSelected( value );
        }

        public boolean isCurveVisibleReso4() {
            return chkReso4.isSelected();
        }

        public void setCurveVisibleReso4( boolean value ) {
            chkReso4.setSelected( value );
        }

        public boolean isCurveVisibleEnvelope() {
            return chkEnvelope.isSelected();
        }

        public void setCurveVisibleEnvelope( boolean value ) {
            chkEnvelope.setSelected( value );
        }

        public boolean isCurveSelectingQuantized() {
            return chkCurveSelectingQuantized.isSelected();
        }

        public void setCurveSelectingQuantized( boolean value ) {
            chkCurveSelectingQuantized.setSelected( value );
        }

        public boolean isPlayPreviewWhenRightClick() {
            return chkPlayPreviewWhenRightClick.isSelected();
        }

        public void setPlayPreviewWhenRightClick( boolean value ) {
            chkPlayPreviewWhenRightClick.setSelected( value );
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
            return chkKeepLyricInputMode.isSelected();
        }

        public void setKeepLyricInputMode( boolean value ) {
            chkKeepLyricInputMode.setSelected( value );
        }

        public PlatformEnum getPlatform() {
            return m_platform;
        }

        public void setPlatform( PlatformEnum value ) {
            m_platform = value;
            for ( int i = 0; i < comboPlatform.getItemCount(); i++ ) {
                String title = (String)comboPlatform.getItemAt( i );
                if ( title.Equals( m_platform + "" ) ) {
                    comboPlatform.setSelectedIndex( i );
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
            return chkScrollHorizontal.isSelected();
        }

        public void setScrollHorizontalOnWheel( boolean value ) {
            chkScrollHorizontal.setSelected( value );
        }

        public void ApplyLanguage() {
            Text = _( "Preference" );
            btnCancel.setText( _( "Cancel" ) );
            btnOK.setText( _( "OK" ) );
            openUtauCore.clearChoosableFileFilter();
            try {
                openUtauCore.addFileFilter( _( "Executable(*.exe)|*.exe" ) );
                openUtauCore.addFileFilter( _( "All Files(*.*)|*.*" ) );
            } catch ( Exception ex ) {
                openUtauCore.addFileFilter( "Executable(*.exe)|*.exe" );
                openUtauCore.addFileFilter( "All Files(*.*)|*.*" );
            }

            folderBrowserSingers.setDescription( _( "Select Singer Directory" ) );

            #region tabSequence
            tabSequence.Text = _( "Sequence" );
            lblResolution.Text = _( "Resolution(VSTi)" );
            lblDynamics.Text = _( "Dynamics" ) + "(&D)";
            lblAmplitude.Text = _( "Vibrato Depth" ) + "(&R)";
            lblPeriod.Text = _( "Vibrato Rate" ) + "(&V)";
            lblVibratoConfig.Text = _( "Vibrato Settings" );
            lblVibratoLength.Text = _( "Default Vibrato Length" ) + "(&L)";
            groupAutoVibratoConfig.setTitle( _( "Auto Vibrato Settings" ) );
            chkEnableAutoVibrato.setText( _( "Enable Automatic Vibrato" ) + "(&E)" );
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
            chkChasePastEvent.setText( _( "Chase Event" ) + "(&C)" );
            lblSampleOutput.Text = _( "Playback Sample Sound" );
            chkEnableSampleOutput.setText( _( "Enable" ) + "(&E)" );
            lblTiming.Text = _( "Timing" );
            lblPreSendTimeSample.Text = _( "Pre-Send Time for sample sound" ) + "(&G)";
            #endregion

            #region tabAppearance
            tabAppearance.Text = _( "Appearance" );
            groupFont.setTitle( _( "Font" ) );
            labelMenu.Text = _( "Menu / Lyrics" );
            labelScreen.Text = _( "Screen" );
            lblLanguage.Text = _( "UI Language" );
            btnChangeMenuFont.setText( _( "Change" ) );
            btnChangeScreenFont.setText( _( "Change" ) );
            lblTrackHeight.Text = _( "Track Height (pixel)" );
            groupVisibleCurve.setTitle( _( "Visible Control Curve" ) );
            #endregion

            #region tabOperation
            tabOperation.Text = _( "Operation" );
            labelWheelOrder.Text = _( "Mouse wheel Rate" );
            chkCursorFix.setText( _( "Fix Play Cursor to Center" ) );
            chkScrollHorizontal.setText( _( "Horizontal Scroll when Mouse wheel" ) );
            lblMaximumFrameRate.Text = _( "Maximum Frame Rate" );
            chkKeepLyricInputMode.setText( _( "Keep Lyric Input Mode" ) );
            lblMouseHoverTime.Text = _( "Waiting Time for Preview" );
            lblMilliSecond.Text = _( "milli second" );
            chkPlayPreviewWhenRightClick.setText( _( "Play Preview On Right Click" ) );
            chkCurveSelectingQuantized.setText( _( "Enable Quantize for Curve Selecting" ) );
            lblMidiInPort.Text = _( "MIDI In Port Number" );
            chkUseSpaceKeyAsMiddleButtonModifier.setText( _( "Use space key as Middle button modifier" ) );

            groupPianoroll.setTitle( _( "Piano Roll" ) );
            groupMisc.setTitle( _( "Misc" ) );
            #endregion

            #region tabPlatform
            tabPlatform.Text = _( "Platform" );

            groupPlatform.setTitle( _( "Platform" ) );
            lblPlatform.Text = _( "Current Platform" );
            chkCommandKeyAsControl.setText( _( "Use Command key as Control key" ) );
            chkTranslateRoman.setText( _( "Translate Roman letters into Kana" ) );

            groupUtauCores.setTitle( _( "UTAU Cores" ) );
            chkInvokeWithWine.setText( _( "Invoke UTAU cores with Wine" ) );
            #endregion

            #region tabUtauSingers
            tabUtauSingers.Text = _( "UTAU Singers" );
            listSingers.setColumnHeaders( new String[]{ _( "Program Change" ), _( "Name" ), _( "Path" ) } );
            btnAdd.setText( _( "Add" ) );
            btnRemove.setText( _( "Remove" ) );
            btnUp.setText( _( "Up" ) );
            btnDown.setText( _( "Down" ) );
            #endregion

            #region tabFile
            tabFile.Text = _( "File" );
            chkAutoBackup.setText( _( "Automatical Backup" ) );
            lblAutoBackupInterval.Text = _( "interval" );
            lblAutoBackupMinutes.Text = _( "minute(s)" );
            #endregion
        }

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public String getLanguage() {
            int index = comboLanguage.getSelectedIndex();
            if ( 0 <= index && index < comboLanguage.getItemCount() ) {
                String title = (String)comboLanguage.getItemAt( index );
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
            int index = comboDynamics.getSelectedIndex();
            for ( Iterator itr = ClockResolution.iterator(); itr.hasNext(); ) {
                ClockResolution vt = (ClockResolution)itr.next();
                count++;
                if ( count == index ) {
                    return vt;
                }
            }
            comboDynamics.setSelectedIndex( 0 );
            return ClockResolution.L30;
        }

        public void setControlCurveResolution( ClockResolution value ) {
            int count = -1;
            for ( Iterator itr = ClockResolution.iterator(); itr.hasNext(); ) {
                ClockResolution vt = (ClockResolution)itr.next();
                count++;
                if ( vt.Equals( value ) ) {
                    comboDynamics.setSelectedIndex( count );
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
            return comboDefaultPremeasure.getSelectedIndex() + 1;
        }

        public void setPreMeasure( int value ) {
            comboDefaultPremeasure.setSelectedIndex( value - 1 );
        }

        public boolean isEnableAutoVibrato() {
            return chkEnableAutoVibrato.isSelected();
        }

        public void setEnableAutoVibrato( boolean value ) {
            chkEnableAutoVibrato.setSelected( value );
        }

        public String getAutoVibratoType1() {
            int count = -1;
            int index = comboAutoVibratoType1.getSelectedIndex();
            if ( 0 <= index ) {
                VibratoConfig vconfig = (VibratoConfig)comboAutoVibratoType1.getSelectedItem();
                return vconfig.contents.IconID;
            } else {
                return "$04040001";
            }
        }

        public void setAutoVibratoType1( String value ) {
            for ( int i = 0; i < comboAutoVibratoType1.getItemCount(); i++ ) {
                VibratoConfig vconfig = (VibratoConfig)comboAutoVibratoType1.getItemAt( i );
                if ( vconfig.contents.IconID.Equals( value ) ) {
                    comboAutoVibratoType1.setSelectedIndex( i );
                    return;
                }
            }
            if ( comboAutoVibratoType1.getItemCount() > 0 ) {
                comboAutoVibratoType1.setSelectedIndex( 0 );
            }
        }

        public String getAutoVibratoType2() {
            int count = -1;
            int index = comboAutoVibratoType2.getSelectedIndex();
            if ( 0 <= index ) {
                VibratoConfig vconfig = (VibratoConfig)comboAutoVibratoType2.getSelectedItem();
                return vconfig.contents.IconID;
            } else {
                return "$04040001";
            }
        }

        public void setAutoVibratoType2( String value ) {
            for ( int i = 0; i < comboAutoVibratoType2.getItemCount(); i++ ) {
                VibratoConfig vconfig = (VibratoConfig)comboAutoVibratoType2.getItemAt( i );
                if ( vconfig.contents.IconID.Equals( value ) ) {
                    comboAutoVibratoType2.setSelectedIndex( i );
                    return;
                }
            }
            if ( comboAutoVibratoType2.getItemCount() > 0 ) {
                comboAutoVibratoType2.setSelectedIndex( 0 );
            }
        }

        public AutoVibratoMinLengthEnum getAutoVibratoMinimumLength() {
            int count = -1;
            int index = comboAutoVibratoMinLength.getSelectedIndex();
            foreach ( AutoVibratoMinLengthEnum avml in Enum.GetValues( typeof( AutoVibratoMinLengthEnum ) ) ) {
                count++;
                if ( count == index ) {
                    return avml;
                }
            }
            comboAutoVibratoMinLength.setSelectedIndex( 0 );
            return AutoVibratoMinLengthEnum.L1;
        }

        public void setAutoVibratoMinimumLength( AutoVibratoMinLengthEnum value ) {
            int count = -1;
            foreach ( AutoVibratoMinLengthEnum avml in Enum.GetValues( typeof( AutoVibratoMinLengthEnum ) ) ) {
                count++;
                if ( avml == value ) {
                    comboAutoVibratoMinLength.setSelectedIndex( count );
                    break;
                }
            }
        }

        public DefaultVibratoLengthEnum getDefaultVibratoLength() {
            int count = -1;
            int index = comboVibratoLength.getSelectedIndex();
            foreach ( DefaultVibratoLengthEnum vt in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) ) {
                count++;
                if ( index == count ) {
                    return vt;
                }
            }
            comboVibratoLength.setSelectedIndex( 1 );
            return DefaultVibratoLengthEnum.L66;
        }

        public void setDefaultVibratoLength( DefaultVibratoLengthEnum value ) {
            int count = -1;
            foreach ( DefaultVibratoLengthEnum dvl in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) ) {
                count++;
                if ( dvl == value ) {
                    comboVibratoLength.SelectedIndex = count;
                    break;
                }
            }
        }

        public boolean isCursorFixed() {
            return chkCursorFix.isSelected();
        }

        public void setCursorFixed( boolean value ) {
            chkCursorFix.setSelected( value );
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
            if ( comboDefualtSinger.getSelectedIndex() >= 0 ) {
                return m_program_change.get( comboDefualtSinger.getSelectedIndex() );
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
                comboDefualtSinger.setSelectedIndex( index );
            }
        }

        private void btnChangeMenuFont_Click( Object sender, BEventArgs e ) {
            fontDialog.setSelectedFont( getBaseFont() );
            if ( fontDialog.showDialog() == BDialogResult.OK ) {
                m_base_font = fontDialog.getSelectedFont();
            }
        }

        private void btnOK_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }

        private void btnChangeScreenFont_Click( Object sender, BEventArgs e ) {
            fontDialog.setSelectedFont( m_screen_font );
            if ( fontDialog.showDialog() == BDialogResult.OK ) {
                m_screen_font = fontDialog.getSelectedFont();
            }
        }

        void UpdateFonts( String font_name ) {
            if ( font_name.Equals( "" ) ) {
                return;
            }
            Font font = new Font( font_name, java.awt.Font.PLAIN, (int)tabSequence.Font.Size );
            Util.applyFontRecurse( this, font );
        }

        private void comboPlatform_SelectedIndexChanged( Object sender, BEventArgs e ) {
            String title = (String)comboPlatform.getSelectedItem();
            foreach ( PlatformEnum p in Enum.GetValues( typeof( PlatformEnum ) ) ) {
                if ( title.Equals( p + "" ) ) {
                    m_platform = p;
                    chkCommandKeyAsControl.setEnabled( p != PlatformEnum.Windows );
                    break;
                }
            }
        }

        public boolean isCommandKeyAsControl() {
            return chkCommandKeyAsControl.isSelected();
        }

        public void setCommandKeyAsControl( boolean value ) {
            chkCommandKeyAsControl.setSelected( value );
        }

        private void btnResampler_Click( Object sender, BEventArgs e ) {
            if ( txtResampler.Text != "" && PortUtil.isDirectoryExists( PortUtil.getDirectoryName( txtResampler.Text ) ) ) {
                openUtauCore.setInitialDirectory( PortUtil.getDirectoryName( txtResampler.Text ) );
            }
            openUtauCore.setSelectedFile( "resampler.exe" );
            int dr = openUtauCore.showOpenDialog( this );
            if ( dr == BFileChooser.APPROVE_OPTION ) {
                txtResampler.Text = openUtauCore.getSelectedFile();
            }
        }

        private void btnWavtool_Click( Object sender, BEventArgs e ) {
            if ( txtWavtool.Text != "" && PortUtil.isDirectoryExists( PortUtil.getDirectoryName( txtWavtool.Text ) ) ) {
                openUtauCore.setInitialDirectory( PortUtil.getDirectoryName( txtWavtool.Text ) );
            }
            openUtauCore.setSelectedFile( "wavtool.exe" );
            int dr = openUtauCore.showOpenDialog( this );
            if ( dr == BFileChooser.APPROVE_OPTION ) {
                txtWavtool.Text = openUtauCore.getSelectedFile();
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
            listSingers.clear();
            for ( int i = 0; i < m_utau_singers.size(); i++ ) {
                m_utau_singers.get( i ).Program = i;
                listSingers.addItem( "", new BListViewItem( new String[] { m_utau_singers.get( i ).Program.ToString(),
                                                                           m_utau_singers.get( i ).VOICENAME, 
                                                                           m_utau_singers.get( i ).VOICEIDSTR } ) );
            }
        }

        private void btnAdd_Click( Object sender, BEventArgs e ) {
            if ( folderBrowserSingers.showDialog() == BDialogResult.OK ) {
                String dir = folderBrowserSingers.getSelectedPath();
                SingerConfig sc = new SingerConfig();
                String character = PortUtil.combinePath( dir, "character.txt" );
                String name = "";
                sc.VOICEIDSTR = dir;
                if ( PortUtil.isFileExists( character ) ) {
                    BufferedReader sr = null;
                    try {
                        sr = new BufferedReader( new InputStreamReader( new FileInputStream( character ), "Shift_JIS" ) );
                        String line = "";
                        while ( (line = sr.readLine()) != null ) {
                            String[] spl = line.Split( '=' );
                            if ( spl.Length >= 2 ) {
                                if ( spl[0].ToLower().Equals( "name" ) ) {
                                    name = spl[1];
                                    break;
                                }
                            }
                        }
                    } catch ( Exception ex ) {
                    } finally {
                        if ( sr != null ) {
                            try {
                                sr.close();
                            } catch ( Exception ex ) {
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
            return listSingers.getSelectedIndex( "" );
        }

        private void listSingers_SelectedIndexChanged( Object sender, BEventArgs e ) {
            int index = getUtauSingersSelectedIndex();
            if ( index < 0 ) {
                btnRemove.setEnabled( false );
                btnUp.setEnabled( false );
                btnDown.setEnabled( false );
            } else {
                btnRemove.setEnabled( true );
                btnUp.setEnabled( 0 <= index - 1 && index - 1 < m_utau_singers.size() );
                btnDown.setEnabled( 0 <= index + 1 && index + 1 < m_utau_singers.size() );
            }
        }

        private void btnRemove_Click( Object sender, BEventArgs e ) {
            int index = getUtauSingersSelectedIndex();
            if ( 0 <= index && index < m_utau_singers.size() ) {
                m_utau_singers.removeElementAt( index );
            }
            UpdateUtauSingerList();
        }

        private void btnDown_Click( Object sender, BEventArgs e ) {
            int index = getUtauSingersSelectedIndex();
#if DEBUG
            AppManager.debugWriteLine( "Preference.btnDown_Click; index=" + index );
#endif
            if ( 0 <= index && index + 1 < m_utau_singers.size() ) {
                SingerConfig buf = (SingerConfig)m_utau_singers.get( index ).clone();
                m_utau_singers.set( index, (SingerConfig)m_utau_singers.get( index + 1 ).clone() );
                m_utau_singers.set( index + 1, buf );
                UpdateUtauSingerList();
                listSingers.setItemSelectedAt( "", index + 1, true );
            }
        }

        private void btnUp_Click( Object sender, BEventArgs e ) {
            int index = getUtauSingersSelectedIndex();
#if DEBUG
            AppManager.debugWriteLine( "Preference.btnUp_Click; index=" + index );
#endif
            if ( 0 <= index - 1 && index < m_utau_singers.size() ) {
                SingerConfig buf = (SingerConfig)m_utau_singers.get( index ).clone();
                m_utau_singers.set( index, (SingerConfig)m_utau_singers.get( index - 1 ).clone() );
                m_utau_singers.set( index - 1, buf );
                UpdateUtauSingerList();
                listSingers.setItemSelectedAt( "", index - 1, true );
            }
        }

        private void chkAutoBackup_CheckedChanged( Object sender, BEventArgs e ) {
            numAutoBackupInterval.Enabled = chkAutoBackup.isSelected();
        }

        private void Preference_FormClosing( object sender, FormClosingEventArgs e ) {
            columnWidthHeaderProgramChange = listSingers.getColumnWidth( 0 );
            columnWidthHeaderName = listSingers.getColumnWidth( 1 );
            columnWidthHeaderPath = listSingers.getColumnWidth( 2 );
        }

        private void registerEventHandlers() {
            this.btnChangeScreenFont.Click += new System.EventHandler( this.btnChangeScreenFont_Click );
            this.btnChangeMenuFont.Click += new System.EventHandler( this.btnChangeMenuFont_Click );
            this.btnWavtool.Click += new System.EventHandler( this.btnWavtool_Click );
            this.btnResampler.Click += new System.EventHandler( this.btnResampler_Click );
            this.comboPlatform.SelectedIndexChanged += new System.EventHandler( this.comboPlatform_SelectedIndexChanged );
            this.btnRemove.Click += new System.EventHandler( this.btnRemove_Click );
            this.btnAdd.Click += new System.EventHandler( this.btnAdd_Click );
            this.btnUp.Click += new System.EventHandler( this.btnUp_Click );
            this.btnDown.Click += new System.EventHandler( this.btnDown_Click );
            this.listSingers.SelectedIndexChanged += new System.EventHandler( this.listSingers_SelectedIndexChanged );
            this.chkAutoBackup.CheckedChanged += new System.EventHandler( this.chkAutoBackup_CheckedChanged );
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            this.FormClosing += new FormClosingEventHandler( Preference_FormClosing );
        }

        private void setResources() {
        }

#if JAVA
        #region UI Impl for Java
	    private JPanel tabSequence = null;
	    private JLabel lblResolution = null;
	    private JPanel jPanel = null;
	    private JLabel lblDynamics = null;
	    private JComboBox comboDynamics = null;
	    private JLabel lblAmplitude = null;
	    private JComboBox comboAmplitude = null;
	    private JLabel lblPeriod = null;
	    private JComboBox comboPeriod = null;
	    private JLabel jLabel1 = null;
	    private JLabel jLabel11 = null;
	    private JLabel jLabel12 = null;
	    private JLabel lblVibratoConfig = null;
	    private JPanel jPanel1 = null;
	    private JLabel lblVibratoLength = null;
	    private JComboBox comboVibratoLength = null;
	    private JLabel jLabel13 = null;
	    private JPanel groupAutoVibratoConfig = null;
	    private JPanel jPanel3 = null;
	    private JCheckBox chkEnableAutoVibrato = null;
	    private JLabel lblAutoVibratoMinLength = null;
	    private JComboBox comboAutoVibratoMinLength = null;
	    private JLabel jLabel4 = null;
	    private JPanel jPanel4 = null;
	    private JLabel lblAutoVibratoType1 = null;
	    private JComboBox comboAutoVibratoType1 = null;
	    private JLabel lblAutoVibratoType2 = null;
	    private JComboBox comboAutoVibratoType2 = null;
	    private JPanel tabAnother = null;
	    private JLabel lblDefaultSinger = null;
	    private JComboBox comboDefualtSinger = null;
	    private JLabel lblPreSendTime = null;
	    private JComboBox numPreSendTime = null;
	    private JLabel jLabel8 = null;
	    private JLabel lblWait = null;
	    private JComboBox numWait = null;
	    private JLabel jLabel81 = null;
	    private JLabel lblDefaultPremeasure = null;
	    private JComboBox comboDefaultPremeasure = null;
	    private JLabel jLabel9 = null;
	    private JPanel tabAppearance = null;
	    private JPanel groupFont = null;
	    private JLabel labelMenu = null;
	    private JLabel labelMenuFontName = null;
	    private JButton btnChangeMenuFont = null;
	    private JLabel labelScreen = null;
	    private JLabel labelScreenFontName = null;
	    private JButton btnChangeScreenFont = null;
	    private JPanel jPanel7 = null;
	    private JLabel lblLanguage = null;
	    private JComboBox comboLanguage = null;
	    private JPanel jPanel71 = null;
	    private JLabel lblTrackHeight = null;
	    private JComboBox numTrackHeight = null;
	    private JPanel groupVisibleCurve = null;
	    private JCheckBox chkAccent = null;
	    private JCheckBox chkDecay = null;
	    private JCheckBox chkVibratoRate = null;
	    private JCheckBox chkVibratoDepth = null;
	    private JCheckBox chkVel = null;
	    private JCheckBox chkDyn = null;
	    private JCheckBox chkBre = null;
	    private JCheckBox chkBri = null;
	    private JCheckBox chkCle = null;
	    private JCheckBox chkOpe = null;
	    private JCheckBox chkGen = null;
	    private JCheckBox chkPor = null;
	    private JCheckBox chkPit = null;
	    private JCheckBox chkPbs = null;
	    private JCheckBox chkHarmonics = null;
	    private JCheckBox chkFx2Depth = null;
	    private JCheckBox chkReso1 = null;
	    private JCheckBox chkReso2 = null;
	    private JCheckBox chkReso3 = null;
	    private JCheckBox chkReso4 = null;
	    private JCheckBox chkEnvelope = null;
	    private JPanel tabOperation = null;
	    private JPanel groupPianoroll = null;
	    private JLabel labelWheelOrder = null;
	    private JComboBox numericUpDownEx1 = null;
	    private JCheckBox chkCursorFix = null;
	    private JCheckBox chkScrollHorizontal = null;
	    private JCheckBox chkKeepLyricInputMode = null;
	    private JCheckBox chkPlayPreviewWhenRightClick = null;
	    private JCheckBox chkCurveSelectingQuantized = null;
	    private JCheckBox chkUseSpaceKeyAsMiddleButtonModifier = null;
	    private JPanel groupMisc = null;
	    private JLabel lblMaximumFrameRate = null;
	    private JComboBox numMaximumFrameRate = null;
	    private JLabel lblMilliSecond = null;
	    private JLabel lblMouseHoverTime = null;
	    private JComboBox numMouseHoverTime = null;
	    private JLabel lblMidiInPort = null;
	    private JComboBox comboMidiInPortNumber = null;
	    private JPanel tabPlatform = null;
	    private JPanel groupPlatform = null;
	    private JLabel lblPlatform = null;
	    private JComboBox comboPlatform = null;
	    private JCheckBox chkCommandKeyAsControl = null;
	    private JCheckBox chkTranslateRoman = null;
	    private JPanel groupVsti = null;
	    private JLabel lblVOCALOID1 = null;
	    private JTextField txtVOCALOID1 = null;
	    private JLabel lblVOCALOID2 = null;
	    private JTextField txtVOCALOID2 = null;
	    private JPanel groupUtauCores = null;
	    private JLabel lblResampler = null;
	    private JTextField txtResampler = null;
	    private JButton btnResampler = null;
	    private JLabel lblWavtool = null;
	    private JTextField txtWavtool = null;
	    private JButton btnWavtool = null;
	    private JCheckBox chkInvokeWithWine = null;
	    private JPanel tabUtauSingers = null;
	    private JTable listSingers = null;
	    private JButton btnAdd = null;
	    private JButton btnRemove = null;
	    private JButton btnUp = null;
	    private JButton btnDown = null;
	    private JPanel jPanel17 = null;
	    private JPanel jPanel18 = null;
	    private JPanel tabFile = null;
	    private JCheckBox chkAutoBackup = null;
	    private JLabel lblAutoBackupInterval = null;
	    private JLabel lblAutoBackupMinutes = null;
	    private JPanel jPanel20 = null;
	    private JPanel panelLower = null;
	    private JButton btnOK = null;
	    private JButton btnCancel = null;
	    private JPanel jPanel5 = null;
	    private JPanel panelUpper = null;
        private JTabbedPane tabPane = null;
	    private JTextField numAutoBackupInterval = null;

	    /**
	     * This method initializes this
	     * 
	     * @return void
	     */
	    private void initialize() {
		    this.setSize(504, 496);
		    this.setContentPane(getJPanel5());
		    this.setTitle("Preference");
	    }

	    /**
	     * This method initializes tabSequence
	     * 
	     * @return javax.swing.JPanel
	     */
	    private JPanel getTabSequence() {
		    if (tabSequence == null) {
			    GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
			    gridBagConstraints20.gridx = 0;
			    gridBagConstraints20.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints20.anchor = GridBagConstraints.NORTHWEST;
			    gridBagConstraints20.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints20.weighty = 1.0D;
			    gridBagConstraints20.gridy = 3;
			    GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			    gridBagConstraints31.gridx = 0;
			    gridBagConstraints31.anchor = GridBagConstraints.WEST;
			    gridBagConstraints31.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints31.weightx = 1.0D;
			    gridBagConstraints31.gridy = 2;
			    lblVibratoConfig = new JLabel();
			    lblVibratoConfig.setText("Vibrato Setting");
			    GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
			    gridBagConstraints21.anchor = GridBagConstraints.WEST;
			    gridBagConstraints21.gridy = 1;
			    gridBagConstraints21.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints21.gridx = 0;
			    GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			    gridBagConstraints1.anchor = GridBagConstraints.WEST;
			    gridBagConstraints1.gridx = 0;
			    gridBagConstraints1.gridy = 0;
			    gridBagConstraints1.weightx = 1.0D;
			    gridBagConstraints1.insets = new Insets(12, 12, 3, 0);
			    lblResolution = new JLabel();
			    lblResolution.setText("Resolution(VSTi)");
			    tabSequence = new JPanel();
			    tabSequence.setLayout(new GridBagLayout());
			    tabSequence.add(lblResolution, gridBagConstraints1);
			    tabSequence.add(getJPanel(), gridBagConstraints21);
			    tabSequence.add(lblVibratoConfig, gridBagConstraints31);
			    tabSequence.add(getJPanel1(), gridBagConstraints20);
		    }
		    return tabSequence;
	    }

	    /**
	     * This method initializes jPanel	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel() {
		    if (jPanel == null) {
			    GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			    gridBagConstraints10.gridx = 4;
			    gridBagConstraints10.anchor = GridBagConstraints.WEST;
			    gridBagConstraints10.weightx = 1.0D;
			    gridBagConstraints10.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints10.gridy = 3;
			    jLabel12 = new JLabel();
			    jLabel12.setText("clocks");
			    GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			    gridBagConstraints9.gridx = 4;
			    gridBagConstraints9.anchor = GridBagConstraints.WEST;
			    gridBagConstraints9.weightx = 1.0D;
			    gridBagConstraints9.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints9.gridy = 1;
			    jLabel11 = new JLabel();
			    jLabel11.setText("clocks");
			    GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			    gridBagConstraints8.gridx = 4;
			    gridBagConstraints8.weightx = 1.0D;
			    gridBagConstraints8.anchor = GridBagConstraints.WEST;
			    gridBagConstraints8.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints8.gridy = 0;
			    jLabel1 = new JLabel();
			    jLabel1.setText("clocks");
			    GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			    gridBagConstraints7.fill = GridBagConstraints.NONE;
			    gridBagConstraints7.gridy = 3;
			    gridBagConstraints7.weightx = 0.0D;
			    gridBagConstraints7.anchor = GridBagConstraints.WEST;
			    gridBagConstraints7.insets = new Insets(3, 24, 3, 0);
			    gridBagConstraints7.gridx = 3;
			    GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			    gridBagConstraints6.gridx = 0;
			    gridBagConstraints6.anchor = GridBagConstraints.WEST;
			    gridBagConstraints6.insets = new Insets(0, 24, 0, 0);
			    gridBagConstraints6.gridy = 3;
			    lblPeriod = new JLabel();
			    lblPeriod.setText("Vibrato Rate");
			    GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			    gridBagConstraints5.fill = GridBagConstraints.NONE;
			    gridBagConstraints5.gridy = 1;
			    gridBagConstraints5.weightx = 0.0D;
			    gridBagConstraints5.anchor = GridBagConstraints.WEST;
			    gridBagConstraints5.insets = new Insets(3, 24, 3, 0);
			    gridBagConstraints5.gridx = 3;
			    GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			    gridBagConstraints4.gridx = 0;
			    gridBagConstraints4.anchor = GridBagConstraints.WEST;
			    gridBagConstraints4.insets = new Insets(0, 24, 0, 0);
			    gridBagConstraints4.gridy = 1;
			    lblAmplitude = new JLabel();
			    lblAmplitude.setText("Vibrato Depth");
			    GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			    gridBagConstraints3.fill = GridBagConstraints.NONE;
			    gridBagConstraints3.gridy = 0;
			    gridBagConstraints3.weightx = 0.0D;
			    gridBagConstraints3.anchor = GridBagConstraints.WEST;
			    gridBagConstraints3.insets = new Insets(3, 24, 3, 0);
			    gridBagConstraints3.gridx = 3;
			    GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			    gridBagConstraints2.gridx = 0;
			    gridBagConstraints2.anchor = GridBagConstraints.WEST;
			    gridBagConstraints2.insets = new Insets(0, 24, 0, 0);
			    gridBagConstraints2.gridy = 0;
			    lblDynamics = new JLabel();
			    lblDynamics.setText("Dynamics");
			    jPanel = new JPanel();
			    jPanel.setLayout(new GridBagLayout());
			    jPanel.add(lblDynamics, gridBagConstraints2);
			    jPanel.add(getComboDynamics(), gridBagConstraints3);
			    jPanel.add(lblAmplitude, gridBagConstraints4);
			    jPanel.add(getComboAmplitude(), gridBagConstraints5);
			    jPanel.add(lblPeriod, gridBagConstraints6);
			    jPanel.add(getComboPeriod(), gridBagConstraints7);
			    jPanel.add(jLabel1, gridBagConstraints8);
			    jPanel.add(jLabel11, gridBagConstraints9);
			    jPanel.add(jLabel12, gridBagConstraints10);
		    }
		    return jPanel;
	    }

	    /**
	     * This method initializes comboDynamics	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboDynamics() {
		    if (comboDynamics == null) {
			    comboDynamics = new JComboBox();
			    comboDynamics.setPreferredSize(new Dimension(101, 20));
		    }
		    return comboDynamics;
	    }

	    /**
	     * This method initializes comboAmplitude	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboAmplitude() {
		    if (comboAmplitude == null) {
			    comboAmplitude = new JComboBox();
			    comboAmplitude.setPreferredSize(new Dimension(101, 20));
		    }
		    return comboAmplitude;
	    }

	    /**
	     * This method initializes comboPeriod	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboPeriod() {
		    if (comboPeriod == null) {
			    comboPeriod = new JComboBox();
			    comboPeriod.setPreferredSize(new Dimension(101, 20));
		    }
		    return comboPeriod;
	    }

	    /**
	     * This method initializes jPanel1	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel1() {
		    if (jPanel1 == null) {
			    GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
			    gridBagConstraints19.gridx = 0;
			    gridBagConstraints19.gridwidth = 5;
			    gridBagConstraints19.anchor = GridBagConstraints.WEST;
			    gridBagConstraints19.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints19.insets = new Insets(3, 24, 3, 12);
			    gridBagConstraints19.gridy = 1;
			    GridBagConstraints gridBagConstraints81 = new GridBagConstraints();
			    gridBagConstraints81.anchor = GridBagConstraints.WEST;
			    gridBagConstraints81.gridx = 4;
			    gridBagConstraints81.gridy = 0;
			    gridBagConstraints81.weightx = 1.0D;
			    gridBagConstraints81.insets = new Insets(0, 12, 0, 0);
			    jLabel13 = new JLabel();
			    jLabel13.setText("%");
			    GridBagConstraints gridBagConstraints32 = new GridBagConstraints();
			    gridBagConstraints32.anchor = GridBagConstraints.WEST;
			    gridBagConstraints32.insets = new Insets(3, 24, 3, 0);
			    gridBagConstraints32.gridx = 3;
			    gridBagConstraints32.gridy = 0;
			    gridBagConstraints32.weightx = 0.0D;
			    gridBagConstraints32.fill = GridBagConstraints.NONE;
			    GridBagConstraints gridBagConstraints22 = new GridBagConstraints();
			    gridBagConstraints22.anchor = GridBagConstraints.WEST;
			    gridBagConstraints22.gridx = 0;
			    gridBagConstraints22.gridy = 0;
			    gridBagConstraints22.insets = new Insets(0, 24, 0, 0);
			    lblVibratoLength = new JLabel();
			    lblVibratoLength.setText("Default Vibrato Length");
			    jPanel1 = new JPanel();
			    jPanel1.setLayout(new GridBagLayout());
			    jPanel1.add(lblVibratoLength, gridBagConstraints22);
			    jPanel1.add(getComboVibratoLength(), gridBagConstraints32);
			    jPanel1.add(jLabel13, gridBagConstraints81);
			    jPanel1.add(getGroupAutoVibratoConfig(), gridBagConstraints19);
		    }
		    return jPanel1;
	    }

	    /**
	     * This method initializes comboVibratoLength	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboVibratoLength() {
		    if (comboVibratoLength == null) {
			    comboVibratoLength = new JComboBox();
			    comboVibratoLength.setPreferredSize(new Dimension(101, 20));
		    }
		    return comboVibratoLength;
	    }

	    /**
	     * This method initializes groupAutoVibratoConfig	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupAutoVibratoConfig() {
		    if (groupAutoVibratoConfig == null) {
			    GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
			    gridBagConstraints51.gridx = 0;
			    gridBagConstraints51.anchor = GridBagConstraints.WEST;
			    gridBagConstraints51.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints51.gridy = 2;
			    GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
			    gridBagConstraints14.gridx = 0;
			    gridBagConstraints14.anchor = GridBagConstraints.WEST;
			    gridBagConstraints14.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints14.gridy = 1;
			    GridBagConstraints gridBagConstraints = new GridBagConstraints();
			    gridBagConstraints.gridx = 0;
			    gridBagConstraints.anchor = GridBagConstraints.WEST;
			    gridBagConstraints.weightx = 1.0D;
			    gridBagConstraints.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints.gridy = 0;
			    groupAutoVibratoConfig = new JPanel();
			    groupAutoVibratoConfig.setLayout(new GridBagLayout());
			    groupAutoVibratoConfig.setBorder(BorderFactory.createTitledBorder(null, "Auto Vibrato Settings", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			    groupAutoVibratoConfig.add(getChkEnableAutoVibrato(), gridBagConstraints);
			    groupAutoVibratoConfig.add(getJPanel3(), gridBagConstraints14);
			    groupAutoVibratoConfig.add(getJPanel4(), gridBagConstraints51);
		    }
		    return groupAutoVibratoConfig;
	    }

	    /**
	     * This method initializes jPanel3	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel3() {
		    if (jPanel3 == null) {
			    GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			    gridBagConstraints13.gridx = 2;
			    gridBagConstraints13.weightx = 1.0D;
			    gridBagConstraints13.anchor = GridBagConstraints.WEST;
			    gridBagConstraints13.gridy = 0;
			    jLabel4 = new JLabel();
			    jLabel4.setText("beat");
			    GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			    gridBagConstraints12.fill = GridBagConstraints.NONE;
			    gridBagConstraints12.gridy = 0;
			    gridBagConstraints12.weightx = 0.0D;
			    gridBagConstraints12.insets = new Insets(0, 12, 0, 12);
			    gridBagConstraints12.gridx = 1;
			    GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			    gridBagConstraints11.gridx = 0;
			    gridBagConstraints11.gridy = 0;
			    lblAutoVibratoMinLength = new JLabel();
			    lblAutoVibratoMinLength.setText("Minimum note length for Automatic Vibrato");
			    jPanel3 = new JPanel();
			    jPanel3.setLayout(new GridBagLayout());
			    jPanel3.add(lblAutoVibratoMinLength, gridBagConstraints11);
			    jPanel3.add(getComboAutoVibratoMinLength(), gridBagConstraints12);
			    jPanel3.add(jLabel4, gridBagConstraints13);
		    }
		    return jPanel3;
	    }

	    /**
	     * This method initializes chkEnableAutoVibrato	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkEnableAutoVibrato() {
		    if (chkEnableAutoVibrato == null) {
			    chkEnableAutoVibrato = new JCheckBox();
			    chkEnableAutoVibrato.setText("Enable Automatic Vibrato");
		    }
		    return chkEnableAutoVibrato;
	    }

	    /**
	     * This method initializes comboAutoVibratoMinLength	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboAutoVibratoMinLength() {
		    if (comboAutoVibratoMinLength == null) {
			    comboAutoVibratoMinLength = new JComboBox();
			    comboAutoVibratoMinLength.setPreferredSize(new Dimension(66, 20));
		    }
		    return comboAutoVibratoMinLength;
	    }

	    /**
	     * This method initializes jPanel4	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel4() {
		    if (jPanel4 == null) {
			    GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
			    gridBagConstraints18.fill = GridBagConstraints.NONE;
			    gridBagConstraints18.gridy = 1;
			    gridBagConstraints18.weightx = 1.0;
			    gridBagConstraints18.anchor = GridBagConstraints.WEST;
			    gridBagConstraints18.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints18.gridx = 1;
			    GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
			    gridBagConstraints17.gridx = 0;
			    gridBagConstraints17.gridy = 1;
			    lblAutoVibratoType2 = new JLabel();
			    lblAutoVibratoType2.setText("Vibrato Type VOCALOID2");
			    GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
			    gridBagConstraints16.fill = GridBagConstraints.NONE;
			    gridBagConstraints16.gridy = 0;
			    gridBagConstraints16.weightx = 1.0;
			    gridBagConstraints16.anchor = GridBagConstraints.WEST;
			    gridBagConstraints16.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints16.gridx = 1;
			    GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
			    gridBagConstraints15.gridx = 0;
			    gridBagConstraints15.gridy = 0;
			    lblAutoVibratoType1 = new JLabel();
			    lblAutoVibratoType1.setText("Vibrato Type VOCALOID1");
			    jPanel4 = new JPanel();
			    jPanel4.setLayout(new GridBagLayout());
			    jPanel4.add(lblAutoVibratoType1, gridBagConstraints15);
			    jPanel4.add(getComboAutoVibratoType1(), gridBagConstraints16);
			    jPanel4.add(lblAutoVibratoType2, gridBagConstraints17);
			    jPanel4.add(getComboAutoVibratoType2(), gridBagConstraints18);
		    }
		    return jPanel4;
	    }

	    /**
	     * This method initializes comboAutoVibratoType1	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboAutoVibratoType1() {
		    if (comboAutoVibratoType1 == null) {
			    comboAutoVibratoType1 = new JComboBox();
			    comboAutoVibratoType1.setPreferredSize(new Dimension(101, 20));
		    }
		    return comboAutoVibratoType1;
	    }

	    /**
	     * This method initializes comboAutoVibratoType2	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboAutoVibratoType2() {
		    if (comboAutoVibratoType2 == null) {
			    comboAutoVibratoType2 = new JComboBox();
			    comboAutoVibratoType2.setPreferredSize(new Dimension(101, 20));
		    }
		    return comboAutoVibratoType2;
	    }

	    /**
	     * This method initializes tabAnother	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getTabAnother() {
		    if (tabAnother == null) {
			    GridBagConstraints gridBagConstraints35 = new GridBagConstraints();
			    gridBagConstraints35.gridx = 0;
			    gridBagConstraints35.weighty = 1.0D;
			    gridBagConstraints35.gridy = 4;
			    jLabel9 = new JLabel();
			    jLabel9.setText("   ");
			    GridBagConstraints gridBagConstraints34 = new GridBagConstraints();
			    gridBagConstraints34.fill = GridBagConstraints.NONE;
			    gridBagConstraints34.gridy = 3;
			    gridBagConstraints34.weightx = 1.0;
			    gridBagConstraints34.gridwidth = 2;
			    gridBagConstraints34.anchor = GridBagConstraints.WEST;
			    gridBagConstraints34.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints34.gridx = 1;
			    GridBagConstraints gridBagConstraints33 = new GridBagConstraints();
			    gridBagConstraints33.gridx = 0;
			    gridBagConstraints33.anchor = GridBagConstraints.WEST;
			    gridBagConstraints33.insets = new Insets(3, 24, 3, 0);
			    gridBagConstraints33.gridy = 3;
			    lblDefaultPremeasure = new JLabel();
			    lblDefaultPremeasure.setText("Default Pre-measure");
			    GridBagConstraints gridBagConstraints30 = new GridBagConstraints();
			    gridBagConstraints30.gridx = 2;
			    gridBagConstraints30.anchor = GridBagConstraints.WEST;
			    gridBagConstraints30.weightx = 1.0D;
			    gridBagConstraints30.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints30.gridy = 2;
			    jLabel81 = new JLabel();
			    jLabel81.setText("msec(200-2000)");
			    GridBagConstraints gridBagConstraints29 = new GridBagConstraints();
			    gridBagConstraints29.fill = GridBagConstraints.NONE;
			    gridBagConstraints29.gridy = 2;
			    gridBagConstraints29.weightx = 0.0D;
			    gridBagConstraints29.anchor = GridBagConstraints.WEST;
			    gridBagConstraints29.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints29.gridx = 1;
			    GridBagConstraints gridBagConstraints28 = new GridBagConstraints();
			    gridBagConstraints28.gridx = 0;
			    gridBagConstraints28.anchor = GridBagConstraints.WEST;
			    gridBagConstraints28.insets = new Insets(0, 24, 0, 0);
			    gridBagConstraints28.gridy = 2;
			    lblWait = new JLabel();
			    lblWait.setText("Waiting Time");
			    GridBagConstraints gridBagConstraints27 = new GridBagConstraints();
			    gridBagConstraints27.gridx = 2;
			    gridBagConstraints27.anchor = GridBagConstraints.WEST;
			    gridBagConstraints27.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints27.weightx = 1.0D;
			    gridBagConstraints27.gridy = 1;
			    jLabel8 = new JLabel();
			    jLabel8.setText("msec(500-5000)");
			    GridBagConstraints gridBagConstraints26 = new GridBagConstraints();
			    gridBagConstraints26.fill = GridBagConstraints.NONE;
			    gridBagConstraints26.gridy = 1;
			    gridBagConstraints26.weightx = 0.0D;
			    gridBagConstraints26.anchor = GridBagConstraints.WEST;
			    gridBagConstraints26.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints26.gridx = 1;
			    GridBagConstraints gridBagConstraints25 = new GridBagConstraints();
			    gridBagConstraints25.gridx = 0;
			    gridBagConstraints25.anchor = GridBagConstraints.WEST;
			    gridBagConstraints25.insets = new Insets(0, 24, 0, 0);
			    gridBagConstraints25.gridy = 1;
			    lblPreSendTime = new JLabel();
			    lblPreSendTime.setText("Pre-Send time");
			    GridBagConstraints gridBagConstraints24 = new GridBagConstraints();
			    gridBagConstraints24.fill = GridBagConstraints.NONE;
			    gridBagConstraints24.gridy = 0;
			    gridBagConstraints24.weightx = 1.0;
			    gridBagConstraints24.anchor = GridBagConstraints.WEST;
			    gridBagConstraints24.insets = new Insets(12, 12, 3, 0);
			    gridBagConstraints24.gridwidth = 2;
			    gridBagConstraints24.gridx = 1;
			    GridBagConstraints gridBagConstraints23 = new GridBagConstraints();
			    gridBagConstraints23.gridx = 0;
			    gridBagConstraints23.anchor = GridBagConstraints.WEST;
			    gridBagConstraints23.insets = new Insets(12, 24, 0, 0);
			    gridBagConstraints23.gridy = 0;
			    lblDefaultSinger = new JLabel();
			    lblDefaultSinger.setText("Default Singer");
			    tabAnother = new JPanel();
			    tabAnother.setLayout(new GridBagLayout());
			    tabAnother.add(lblDefaultSinger, gridBagConstraints23);
			    tabAnother.add(getComboDefualtSinger(), gridBagConstraints24);
			    tabAnother.add(lblPreSendTime, gridBagConstraints25);
			    tabAnother.add(getNumPreSendTime(), gridBagConstraints26);
			    tabAnother.add(jLabel8, gridBagConstraints27);
			    tabAnother.add(lblWait, gridBagConstraints28);
			    tabAnother.add(getNumWait(), gridBagConstraints29);
			    tabAnother.add(jLabel81, gridBagConstraints30);
			    tabAnother.add(lblDefaultPremeasure, gridBagConstraints33);
			    tabAnother.add(getComboDefaultPremeasure(), gridBagConstraints34);
			    tabAnother.add(jLabel9, gridBagConstraints35);
		    }
		    return tabAnother;
	    }

	    /**
	     * This method initializes comboDefualtSinger	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboDefualtSinger() {
		    if (comboDefualtSinger == null) {
			    comboDefualtSinger = new JComboBox();
			    comboDefualtSinger.setPreferredSize(new Dimension(222, 20));
		    }
		    return comboDefualtSinger;
	    }

	    /**
	     * This method initializes numPreSendTime	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getNumPreSendTime() {
		    if (numPreSendTime == null) {
			    numPreSendTime = new JComboBox();
			    numPreSendTime.setPreferredSize(new Dimension(68, 20));
		    }
		    return numPreSendTime;
	    }

	    /**
	     * This method initializes numWait	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getNumWait() {
		    if (numWait == null) {
			    numWait = new JComboBox();
			    numWait.setPreferredSize(new Dimension(68, 20));
		    }
		    return numWait;
	    }

	    /**
	     * This method initializes comboDefaultPremeasure	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboDefaultPremeasure() {
		    if (comboDefaultPremeasure == null) {
			    comboDefaultPremeasure = new JComboBox();
			    comboDefaultPremeasure.setPreferredSize(new Dimension(68, 20));
		    }
		    return comboDefaultPremeasure;
	    }

	    /**
	     * This method initializes tabAppearance	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getTabAppearance() {
		    if (tabAppearance == null) {
			    GridBagConstraints gridBagConstraints69 = new GridBagConstraints();
			    gridBagConstraints69.gridx = 0;
			    gridBagConstraints69.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints69.weighty = 1.0D;
			    gridBagConstraints69.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints69.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints69.gridy = 3;
			    GridBagConstraints gridBagConstraints46 = new GridBagConstraints();
			    gridBagConstraints46.gridx = 0;
			    gridBagConstraints46.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints46.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints46.gridy = 2;
			    GridBagConstraints gridBagConstraints45 = new GridBagConstraints();
			    gridBagConstraints45.gridx = 0;
			    gridBagConstraints45.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints45.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints45.gridy = 1;
			    GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
			    gridBagConstraints42.gridx = 0;
			    gridBagConstraints42.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints42.weightx = 1.0D;
			    gridBagConstraints42.insets = new Insets(12, 12, 3, 12);
			    gridBagConstraints42.gridy = 0;
			    tabAppearance = new JPanel();
			    tabAppearance.setLayout(new GridBagLayout());
			    tabAppearance.add(getGroupFont(), gridBagConstraints42);
			    tabAppearance.add(getJPanel7(), gridBagConstraints45);
			    tabAppearance.add(getJPanel71(), gridBagConstraints46);
			    tabAppearance.add(getGroupVisibleCurve(), gridBagConstraints69);
		    }
		    return tabAppearance;
	    }

	    /**
	     * This method initializes groupFont	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupFont() {
		    if (groupFont == null) {
			    GridBagConstraints gridBagConstraints41 = new GridBagConstraints();
			    gridBagConstraints41.gridx = 2;
			    gridBagConstraints41.weightx = 1.0D;
			    gridBagConstraints41.anchor = GridBagConstraints.WEST;
			    gridBagConstraints41.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints41.gridy = 1;
			    GridBagConstraints gridBagConstraints40 = new GridBagConstraints();
			    gridBagConstraints40.gridx = 1;
			    gridBagConstraints40.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints40.anchor = GridBagConstraints.WEST;
			    gridBagConstraints40.gridy = 1;
			    labelScreenFontName = new JLabel();
			    labelScreenFontName.setText("MS UI Gothic");
			    GridBagConstraints gridBagConstraints39 = new GridBagConstraints();
			    gridBagConstraints39.gridx = 0;
			    gridBagConstraints39.anchor = GridBagConstraints.WEST;
			    gridBagConstraints39.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints39.gridy = 1;
			    labelScreen = new JLabel();
			    labelScreen.setText("Screen");
			    GridBagConstraints gridBagConstraints38 = new GridBagConstraints();
			    gridBagConstraints38.gridx = 2;
			    gridBagConstraints38.weightx = 1.0D;
			    gridBagConstraints38.anchor = GridBagConstraints.WEST;
			    gridBagConstraints38.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints38.gridy = 0;
			    GridBagConstraints gridBagConstraints37 = new GridBagConstraints();
			    gridBagConstraints37.gridx = 1;
			    gridBagConstraints37.anchor = GridBagConstraints.WEST;
			    gridBagConstraints37.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints37.gridy = 0;
			    labelMenuFontName = new JLabel();
			    labelMenuFontName.setText("MS UI Gothic");
			    GridBagConstraints gridBagConstraints36 = new GridBagConstraints();
			    gridBagConstraints36.gridx = 0;
			    gridBagConstraints36.anchor = GridBagConstraints.WEST;
			    gridBagConstraints36.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints36.gridy = 0;
			    labelMenu = new JLabel();
			    labelMenu.setText("Menu/Lyrics");
			    groupFont = new JPanel();
			    groupFont.setLayout(new GridBagLayout());
			    groupFont.setBorder(BorderFactory.createTitledBorder(null, "Font", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			    groupFont.add(labelMenu, gridBagConstraints36);
			    groupFont.add(labelMenuFontName, gridBagConstraints37);
			    groupFont.add(getBtnChangeMenuFont(), gridBagConstraints38);
			    groupFont.add(labelScreen, gridBagConstraints39);
			    groupFont.add(labelScreenFontName, gridBagConstraints40);
			    groupFont.add(getBtnChangeScreenFont(), gridBagConstraints41);
		    }
		    return groupFont;
	    }

	    /**
	     * This method initializes btnChangeMenuFont	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnChangeMenuFont() {
		    if (btnChangeMenuFont == null) {
			    btnChangeMenuFont = new JButton();
			    btnChangeMenuFont.setText("Change");
			    btnChangeMenuFont.setPreferredSize(new Dimension(85, 23));
		    }
		    return btnChangeMenuFont;
	    }

	    /**
	     * This method initializes btnChangeScreenFont	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnChangeScreenFont() {
		    if (btnChangeScreenFont == null) {
			    btnChangeScreenFont = new JButton();
			    btnChangeScreenFont.setPreferredSize(new Dimension(85, 23));
			    btnChangeScreenFont.setText("Change");
		    }
		    return btnChangeScreenFont;
	    }

	    /**
	     * This method initializes jPanel7	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel7() {
		    if (jPanel7 == null) {
			    GridBagConstraints gridBagConstraints44 = new GridBagConstraints();
			    gridBagConstraints44.fill = GridBagConstraints.NONE;
			    gridBagConstraints44.gridy = 0;
			    gridBagConstraints44.weightx = 1.0;
			    gridBagConstraints44.anchor = GridBagConstraints.WEST;
			    gridBagConstraints44.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints44.gridx = 1;
			    GridBagConstraints gridBagConstraints43 = new GridBagConstraints();
			    gridBagConstraints43.gridx = 0;
			    gridBagConstraints43.anchor = GridBagConstraints.WEST;
			    gridBagConstraints43.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints43.gridy = 0;
			    lblLanguage = new JLabel();
			    lblLanguage.setText("UI Language");
			    jPanel7 = new JPanel();
			    jPanel7.setLayout(new GridBagLayout());
			    jPanel7.add(lblLanguage, gridBagConstraints43);
			    jPanel7.add(getComboLanguage(), gridBagConstraints44);
		    }
		    return jPanel7;
	    }

	    /**
	     * This method initializes comboLanguage	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboLanguage() {
		    if (comboLanguage == null) {
			    comboLanguage = new JComboBox();
			    comboLanguage.setPreferredSize(new Dimension(121, 20));
		    }
		    return comboLanguage;
	    }

	    /**
	     * This method initializes jPanel71	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel71() {
		    if (jPanel71 == null) {
			    GridBagConstraints gridBagConstraints441 = new GridBagConstraints();
			    gridBagConstraints441.anchor = GridBagConstraints.WEST;
			    gridBagConstraints441.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints441.gridx = 1;
			    gridBagConstraints441.gridy = 0;
			    gridBagConstraints441.weightx = 1.0;
			    gridBagConstraints441.fill = GridBagConstraints.NONE;
			    GridBagConstraints gridBagConstraints431 = new GridBagConstraints();
			    gridBagConstraints431.anchor = GridBagConstraints.WEST;
			    gridBagConstraints431.gridx = 0;
			    gridBagConstraints431.gridy = 0;
			    gridBagConstraints431.insets = new Insets(3, 12, 3, 0);
			    lblTrackHeight = new JLabel();
			    lblTrackHeight.setText("Track Height(pixel)");
			    jPanel71 = new JPanel();
			    jPanel71.setLayout(new GridBagLayout());
			    jPanel71.add(lblTrackHeight, gridBagConstraints431);
			    jPanel71.add(getNumTrackHeight(), gridBagConstraints441);
		    }
		    return jPanel71;
	    }

	    /**
	     * This method initializes numTrackHeight	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getNumTrackHeight() {
		    if (numTrackHeight == null) {
			    numTrackHeight = new JComboBox();
			    numTrackHeight.setPreferredSize(new Dimension(121, 20));
		    }
		    return numTrackHeight;
	    }

	    /**
	     * This method initializes groupVisibleCurve	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupVisibleCurve() {
		    if (groupVisibleCurve == null) {
			    GridBagConstraints gridBagConstraints68 = new GridBagConstraints();
			    gridBagConstraints68.gridx = 0;
			    gridBagConstraints68.anchor = GridBagConstraints.WEST;
			    gridBagConstraints68.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints68.gridy = 5;
			    GridBagConstraints gridBagConstraints67 = new GridBagConstraints();
			    gridBagConstraints67.gridx = 3;
			    gridBagConstraints67.anchor = GridBagConstraints.WEST;
			    gridBagConstraints67.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints67.gridy = 4;
			    GridBagConstraints gridBagConstraints66 = new GridBagConstraints();
			    gridBagConstraints66.gridx = 2;
			    gridBagConstraints66.anchor = GridBagConstraints.WEST;
			    gridBagConstraints66.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints66.gridy = 4;
			    GridBagConstraints gridBagConstraints65 = new GridBagConstraints();
			    gridBagConstraints65.gridx = 1;
			    gridBagConstraints65.anchor = GridBagConstraints.WEST;
			    gridBagConstraints65.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints65.gridy = 4;
			    GridBagConstraints gridBagConstraints64 = new GridBagConstraints();
			    gridBagConstraints64.gridx = 0;
			    gridBagConstraints64.anchor = GridBagConstraints.WEST;
			    gridBagConstraints64.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints64.gridy = 4;
			    GridBagConstraints gridBagConstraints63 = new GridBagConstraints();
			    gridBagConstraints63.gridx = 3;
			    gridBagConstraints63.anchor = GridBagConstraints.WEST;
			    gridBagConstraints63.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints63.gridy = 3;
			    GridBagConstraints gridBagConstraints62 = new GridBagConstraints();
			    gridBagConstraints62.gridx = 2;
			    gridBagConstraints62.anchor = GridBagConstraints.WEST;
			    gridBagConstraints62.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints62.gridy = 3;
			    GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
			    gridBagConstraints61.gridx = 1;
			    gridBagConstraints61.anchor = GridBagConstraints.WEST;
			    gridBagConstraints61.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints61.gridy = 3;
			    GridBagConstraints gridBagConstraints60 = new GridBagConstraints();
			    gridBagConstraints60.gridx = 0;
			    gridBagConstraints60.anchor = GridBagConstraints.WEST;
			    gridBagConstraints60.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints60.gridy = 3;
			    GridBagConstraints gridBagConstraints59 = new GridBagConstraints();
			    gridBagConstraints59.gridx = 3;
			    gridBagConstraints59.anchor = GridBagConstraints.WEST;
			    gridBagConstraints59.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints59.gridy = 2;
			    GridBagConstraints gridBagConstraints58 = new GridBagConstraints();
			    gridBagConstraints58.gridx = 2;
			    gridBagConstraints58.anchor = GridBagConstraints.WEST;
			    gridBagConstraints58.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints58.gridy = 2;
			    GridBagConstraints gridBagConstraints57 = new GridBagConstraints();
			    gridBagConstraints57.gridx = 1;
			    gridBagConstraints57.anchor = GridBagConstraints.WEST;
			    gridBagConstraints57.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints57.gridy = 2;
			    GridBagConstraints gridBagConstraints56 = new GridBagConstraints();
			    gridBagConstraints56.gridx = 0;
			    gridBagConstraints56.anchor = GridBagConstraints.WEST;
			    gridBagConstraints56.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints56.gridy = 2;
			    GridBagConstraints gridBagConstraints55 = new GridBagConstraints();
			    gridBagConstraints55.gridx = 3;
			    gridBagConstraints55.anchor = GridBagConstraints.WEST;
			    gridBagConstraints55.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints55.gridy = 1;
			    GridBagConstraints gridBagConstraints54 = new GridBagConstraints();
			    gridBagConstraints54.gridx = 2;
			    gridBagConstraints54.anchor = GridBagConstraints.WEST;
			    gridBagConstraints54.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints54.gridy = 1;
			    GridBagConstraints gridBagConstraints53 = new GridBagConstraints();
			    gridBagConstraints53.gridx = 1;
			    gridBagConstraints53.anchor = GridBagConstraints.WEST;
			    gridBagConstraints53.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints53.gridy = 1;
			    GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
			    gridBagConstraints52.gridx = 0;
			    gridBagConstraints52.anchor = GridBagConstraints.WEST;
			    gridBagConstraints52.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints52.gridy = 1;
			    GridBagConstraints gridBagConstraints50 = new GridBagConstraints();
			    gridBagConstraints50.gridx = 3;
			    gridBagConstraints50.anchor = GridBagConstraints.WEST;
			    gridBagConstraints50.weightx = 0.25D;
			    gridBagConstraints50.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints50.gridy = 0;
			    GridBagConstraints gridBagConstraints49 = new GridBagConstraints();
			    gridBagConstraints49.gridx = 2;
			    gridBagConstraints49.anchor = GridBagConstraints.WEST;
			    gridBagConstraints49.weightx = 0.25D;
			    gridBagConstraints49.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints49.gridy = 0;
			    GridBagConstraints gridBagConstraints48 = new GridBagConstraints();
			    gridBagConstraints48.gridx = 1;
			    gridBagConstraints48.anchor = GridBagConstraints.WEST;
			    gridBagConstraints48.weightx = 0.25D;
			    gridBagConstraints48.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints48.gridy = 0;
			    GridBagConstraints gridBagConstraints47 = new GridBagConstraints();
			    gridBagConstraints47.gridx = 0;
			    gridBagConstraints47.anchor = GridBagConstraints.WEST;
			    gridBagConstraints47.weightx = 0.25D;
			    gridBagConstraints47.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints47.gridy = 0;
			    groupVisibleCurve = new JPanel();
			    groupVisibleCurve.setLayout(new GridBagLayout());
			    groupVisibleCurve.setBorder(BorderFactory.createTitledBorder(null, "Visible Control Curve", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			    groupVisibleCurve.add(getChkAccent(), gridBagConstraints47);
			    groupVisibleCurve.add(getChkDecay(), gridBagConstraints48);
			    groupVisibleCurve.add(getChkVibratoRate(), gridBagConstraints49);
			    groupVisibleCurve.add(getChkVibratoDepth(), gridBagConstraints50);
			    groupVisibleCurve.add(getChkVel(), gridBagConstraints52);
			    groupVisibleCurve.add(getChkDyn(), gridBagConstraints53);
			    groupVisibleCurve.add(getChkBre(), gridBagConstraints54);
			    groupVisibleCurve.add(getChkBri(), gridBagConstraints55);
			    groupVisibleCurve.add(getChkCle(), gridBagConstraints56);
			    groupVisibleCurve.add(getChkOpe(), gridBagConstraints57);
			    groupVisibleCurve.add(getChkGen(), gridBagConstraints58);
			    groupVisibleCurve.add(getChkPor(), gridBagConstraints59);
			    groupVisibleCurve.add(getChkPit(), gridBagConstraints60);
			    groupVisibleCurve.add(getChkPbs(), gridBagConstraints61);
			    groupVisibleCurve.add(getChkHarmonics(), gridBagConstraints62);
			    groupVisibleCurve.add(getChkFx2Depth(), gridBagConstraints63);
			    groupVisibleCurve.add(getChkReso1(), gridBagConstraints64);
			    groupVisibleCurve.add(getChkReso2(), gridBagConstraints65);
			    groupVisibleCurve.add(getChkReso3(), gridBagConstraints66);
			    groupVisibleCurve.add(getChkReso4(), gridBagConstraints67);
			    groupVisibleCurve.add(getChkEnvelope(), gridBagConstraints68);
		    }
		    return groupVisibleCurve;
	    }

	    /**
	     * This method initializes chkAccent	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkAccent() {
		    if (chkAccent == null) {
			    chkAccent = new JCheckBox();
			    chkAccent.setText("Accent");
		    }
		    return chkAccent;
	    }

	    /**
	     * This method initializes chkDecay	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkDecay() {
		    if (chkDecay == null) {
			    chkDecay = new JCheckBox();
			    chkDecay.setText("Decay");
		    }
		    return chkDecay;
	    }

	    /**
	     * This method initializes chkVibratoRate	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkVibratoRate() {
		    if (chkVibratoRate == null) {
			    chkVibratoRate = new JCheckBox();
			    chkVibratoRate.setText("VibratoRate");
		    }
		    return chkVibratoRate;
	    }

	    /**
	     * This method initializes chkVibratoDepth	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkVibratoDepth() {
		    if (chkVibratoDepth == null) {
			    chkVibratoDepth = new JCheckBox();
			    chkVibratoDepth.setText("VibratoDepth");
		    }
		    return chkVibratoDepth;
	    }

	    /**
	     * This method initializes chkVel	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkVel() {
		    if (chkVel == null) {
			    chkVel = new JCheckBox();
			    chkVel.setText("VEL");
		    }
		    return chkVel;
	    }

	    /**
	     * This method initializes chkDyn	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkDyn() {
		    if (chkDyn == null) {
			    chkDyn = new JCheckBox();
			    chkDyn.setText("DYN");
		    }
		    return chkDyn;
	    }

	    /**
	     * This method initializes chkBre	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkBre() {
		    if (chkBre == null) {
			    chkBre = new JCheckBox();
			    chkBre.setText("BRE");
		    }
		    return chkBre;
	    }

	    /**
	     * This method initializes chkBri	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkBri() {
		    if (chkBri == null) {
			    chkBri = new JCheckBox();
			    chkBri.setText("BRI");
		    }
		    return chkBri;
	    }

	    /**
	     * This method initializes chkCle	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkCle() {
		    if (chkCle == null) {
			    chkCle = new JCheckBox();
			    chkCle.setText("CLE");
		    }
		    return chkCle;
	    }

	    /**
	     * This method initializes chkOpe	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkOpe() {
		    if (chkOpe == null) {
			    chkOpe = new JCheckBox();
			    chkOpe.setText("OPE");
		    }
		    return chkOpe;
	    }

	    /**
	     * This method initializes chkGen	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkGen() {
		    if (chkGen == null) {
			    chkGen = new JCheckBox();
			    chkGen.setText("GEN");
		    }
		    return chkGen;
	    }

	    /**
	     * This method initializes chkPor	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkPor() {
		    if (chkPor == null) {
			    chkPor = new JCheckBox();
			    chkPor.setText("POR");
		    }
		    return chkPor;
	    }

	    /**
	     * This method initializes chkPit	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkPit() {
		    if (chkPit == null) {
			    chkPit = new JCheckBox();
			    chkPit.setText("PIT");
		    }
		    return chkPit;
	    }

	    /**
	     * This method initializes chkPbs	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkPbs() {
		    if (chkPbs == null) {
			    chkPbs = new JCheckBox();
			    chkPbs.setText("PBS");
		    }
		    return chkPbs;
	    }

	    /**
	     * This method initializes chkHarmonics	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkHarmonics() {
		    if (chkHarmonics == null) {
			    chkHarmonics = new JCheckBox();
			    chkHarmonics.setText("Harmonics");
		    }
		    return chkHarmonics;
	    }

	    /**
	     * This method initializes chkFx2Depth	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkFx2Depth() {
		    if (chkFx2Depth == null) {
			    chkFx2Depth = new JCheckBox();
			    chkFx2Depth.setText("FX2Depth");
		    }
		    return chkFx2Depth;
	    }

	    /**
	     * This method initializes chkReso1	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkReso1() {
		    if (chkReso1 == null) {
			    chkReso1 = new JCheckBox();
			    chkReso1.setText("Reso1");
		    }
		    return chkReso1;
	    }

	    /**
	     * This method initializes chkReso2	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkReso2() {
		    if (chkReso2 == null) {
			    chkReso2 = new JCheckBox();
			    chkReso2.setText("Reso2");
		    }
		    return chkReso2;
	    }

	    /**
	     * This method initializes chkReso3	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkReso3() {
		    if (chkReso3 == null) {
			    chkReso3 = new JCheckBox();
			    chkReso3.setText("Reso3");
		    }
		    return chkReso3;
	    }

	    /**
	     * This method initializes chkReso4	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkReso4() {
		    if (chkReso4 == null) {
			    chkReso4 = new JCheckBox();
			    chkReso4.setText("Reso4");
		    }
		    return chkReso4;
	    }

	    /**
	     * This method initializes chkEnvelope	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkEnvelope() {
		    if (chkEnvelope == null) {
			    chkEnvelope = new JCheckBox();
			    chkEnvelope.setText("Envelope");
		    }
		    return chkEnvelope;
	    }

	    /**
	     * This method initializes tabOperation	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getTabOperation() {
		    if (tabOperation == null) {
			    GridBagConstraints gridBagConstraints87 = new GridBagConstraints();
			    gridBagConstraints87.gridx = 0;
			    gridBagConstraints87.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints87.weighty = 1.0D;
			    gridBagConstraints87.weightx = 1.0D;
			    gridBagConstraints87.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints87.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints87.gridy = 1;
			    GridBagConstraints gridBagConstraints78 = new GridBagConstraints();
			    gridBagConstraints78.gridx = 0;
			    gridBagConstraints78.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints78.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints78.weightx = 1.0D;
			    gridBagConstraints78.insets = new Insets(12, 12, 3, 12);
			    gridBagConstraints78.gridy = 0;
			    tabOperation = new JPanel();
			    tabOperation.setLayout(new GridBagLayout());
			    tabOperation.add(getGroupPianoroll(), gridBagConstraints78);
			    tabOperation.add(getGroupMisc(), gridBagConstraints87);
		    }
		    return tabOperation;
	    }

	    /**
	     * This method initializes groupPianoroll	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupPianoroll() {
		    if (groupPianoroll == null) {
			    GridBagConstraints gridBagConstraints77 = new GridBagConstraints();
			    gridBagConstraints77.gridx = 0;
			    gridBagConstraints77.anchor = GridBagConstraints.WEST;
			    gridBagConstraints77.gridwidth = 2;
			    gridBagConstraints77.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints77.gridy = 6;
			    GridBagConstraints gridBagConstraints76 = new GridBagConstraints();
			    gridBagConstraints76.gridx = 0;
			    gridBagConstraints76.anchor = GridBagConstraints.WEST;
			    gridBagConstraints76.gridwidth = 2;
			    gridBagConstraints76.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints76.gridy = 5;
			    GridBagConstraints gridBagConstraints75 = new GridBagConstraints();
			    gridBagConstraints75.gridx = 0;
			    gridBagConstraints75.anchor = GridBagConstraints.WEST;
			    gridBagConstraints75.gridwidth = 2;
			    gridBagConstraints75.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints75.gridy = 4;
			    GridBagConstraints gridBagConstraints74 = new GridBagConstraints();
			    gridBagConstraints74.gridx = 0;
			    gridBagConstraints74.gridwidth = 2;
			    gridBagConstraints74.anchor = GridBagConstraints.WEST;
			    gridBagConstraints74.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints74.gridy = 3;
			    GridBagConstraints gridBagConstraints73 = new GridBagConstraints();
			    gridBagConstraints73.gridx = 0;
			    gridBagConstraints73.gridwidth = 2;
			    gridBagConstraints73.anchor = GridBagConstraints.WEST;
			    gridBagConstraints73.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints73.gridy = 2;
			    GridBagConstraints gridBagConstraints72 = new GridBagConstraints();
			    gridBagConstraints72.gridx = 0;
			    gridBagConstraints72.weightx = 0.0D;
			    gridBagConstraints72.gridwidth = 2;
			    gridBagConstraints72.anchor = GridBagConstraints.WEST;
			    gridBagConstraints72.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints72.gridy = 1;
			    GridBagConstraints gridBagConstraints71 = new GridBagConstraints();
			    gridBagConstraints71.fill = GridBagConstraints.NONE;
			    gridBagConstraints71.gridy = 0;
			    gridBagConstraints71.weightx = 1.0;
			    gridBagConstraints71.anchor = GridBagConstraints.WEST;
			    gridBagConstraints71.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints71.gridx = 1;
			    GridBagConstraints gridBagConstraints70 = new GridBagConstraints();
			    gridBagConstraints70.gridx = 0;
			    gridBagConstraints70.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints70.anchor = GridBagConstraints.WEST;
			    gridBagConstraints70.gridy = 0;
			    labelWheelOrder = new JLabel();
			    labelWheelOrder.setText("Mouse wheel Rate");
			    groupPianoroll = new JPanel();
			    groupPianoroll.setLayout(new GridBagLayout());
			    groupPianoroll.setBorder(BorderFactory.createTitledBorder(null, "Piano Roll", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			    groupPianoroll.add(labelWheelOrder, gridBagConstraints70);
			    groupPianoroll.add(getNumericUpDownEx1(), gridBagConstraints71);
			    groupPianoroll.add(getChkCursorFix(), gridBagConstraints72);
			    groupPianoroll.add(getChkScrollHorizontal(), gridBagConstraints73);
			    groupPianoroll.add(getChkKeepLyricInputMode(), gridBagConstraints74);
			    groupPianoroll.add(getChkPlayPreviewWhenRightClick(), gridBagConstraints75);
			    groupPianoroll.add(getChkCurveSelectingQuantized(), gridBagConstraints76);
			    groupPianoroll.add(getChkUseSpaceKeyAsMiddleButtonModifier(), gridBagConstraints77);
		    }
		    return groupPianoroll;
	    }

	    /**
	     * This method initializes numericUpDownEx1	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getNumericUpDownEx1() {
		    if (numericUpDownEx1 == null) {
			    numericUpDownEx1 = new JComboBox();
			    numericUpDownEx1.setPreferredSize(new Dimension(120, 20));
		    }
		    return numericUpDownEx1;
	    }

	    /**
	     * This method initializes chkCursorFix	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkCursorFix() {
		    if (chkCursorFix == null) {
			    chkCursorFix = new JCheckBox();
			    chkCursorFix.setText("Fix Play Cursor to Center");
		    }
		    return chkCursorFix;
	    }

	    /**
	     * This method initializes chkScrollHorizontal	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkScrollHorizontal() {
		    if (chkScrollHorizontal == null) {
			    chkScrollHorizontal = new JCheckBox();
			    chkScrollHorizontal.setText("Horizontal Scroll when Mouse wheel");
		    }
		    return chkScrollHorizontal;
	    }

	    /**
	     * This method initializes chkKeepLyricInputMode	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkKeepLyricInputMode() {
		    if (chkKeepLyricInputMode == null) {
			    chkKeepLyricInputMode = new JCheckBox();
			    chkKeepLyricInputMode.setText("Keep Lyric Input Mode");
		    }
		    return chkKeepLyricInputMode;
	    }

	    /**
	     * This method initializes chkPlayPreviewWhenRightClick	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkPlayPreviewWhenRightClick() {
		    if (chkPlayPreviewWhenRightClick == null) {
			    chkPlayPreviewWhenRightClick = new JCheckBox();
			    chkPlayPreviewWhenRightClick.setText("Play Preview On Right Click");
		    }
		    return chkPlayPreviewWhenRightClick;
	    }

	    /**
	     * This method initializes chkCurveSelectingQuantized	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkCurveSelectingQuantized() {
		    if (chkCurveSelectingQuantized == null) {
			    chkCurveSelectingQuantized = new JCheckBox();
			    chkCurveSelectingQuantized.setText("Enable Quantize for Curve Selecting");
		    }
		    return chkCurveSelectingQuantized;
	    }

	    /**
	     * This method initializes chkUseSpaceKeyAsMiddleButtonModifier	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkUseSpaceKeyAsMiddleButtonModifier() {
		    if (chkUseSpaceKeyAsMiddleButtonModifier == null) {
			    chkUseSpaceKeyAsMiddleButtonModifier = new JCheckBox();
			    chkUseSpaceKeyAsMiddleButtonModifier.setText("Use space key as Middle button modifier");
		    }
		    return chkUseSpaceKeyAsMiddleButtonModifier;
	    }

	    /**
	     * This method initializes groupMisc	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupMisc() {
		    if (groupMisc == null) {
			    GridBagConstraints gridBagConstraints86 = new GridBagConstraints();
			    gridBagConstraints86.fill = GridBagConstraints.NONE;
			    gridBagConstraints86.gridy = 2;
			    gridBagConstraints86.weightx = 1.0D;
			    gridBagConstraints86.anchor = GridBagConstraints.WEST;
			    gridBagConstraints86.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints86.gridwidth = 2;
			    gridBagConstraints86.gridx = 1;
			    GridBagConstraints gridBagConstraints85 = new GridBagConstraints();
			    gridBagConstraints85.gridx = 0;
			    gridBagConstraints85.anchor = GridBagConstraints.WEST;
			    gridBagConstraints85.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints85.gridy = 2;
			    lblMidiInPort = new JLabel();
			    lblMidiInPort.setText("MIDI In Port Number");
			    GridBagConstraints gridBagConstraints84 = new GridBagConstraints();
			    gridBagConstraints84.fill = GridBagConstraints.NONE;
			    gridBagConstraints84.gridy = 1;
			    gridBagConstraints84.weightx = 1.0D;
			    gridBagConstraints84.gridwidth = 2;
			    gridBagConstraints84.anchor = GridBagConstraints.WEST;
			    gridBagConstraints84.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints84.gridx = 1;
			    GridBagConstraints gridBagConstraints83 = new GridBagConstraints();
			    gridBagConstraints83.gridx = 0;
			    gridBagConstraints83.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints83.gridy = 1;
			    lblMouseHoverTime = new JLabel();
			    lblMouseHoverTime.setText("Waiting Time for Preview");
			    GridBagConstraints gridBagConstraints82 = new GridBagConstraints();
			    gridBagConstraints82.gridx = 2;
			    gridBagConstraints82.anchor = GridBagConstraints.WEST;
			    gridBagConstraints82.insets = new Insets(3, 3, 3, 0);
			    gridBagConstraints82.weightx = 1.0D;
			    gridBagConstraints82.gridy = 0;
			    lblMilliSecond = new JLabel();
			    lblMilliSecond.setText("milli second");
			    GridBagConstraints gridBagConstraints80 = new GridBagConstraints();
			    gridBagConstraints80.fill = GridBagConstraints.NONE;
			    gridBagConstraints80.gridy = 0;
			    gridBagConstraints80.weightx = 1.0;
			    gridBagConstraints80.anchor = GridBagConstraints.WEST;
			    gridBagConstraints80.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints80.gridx = 1;
			    GridBagConstraints gridBagConstraints79 = new GridBagConstraints();
			    gridBagConstraints79.gridx = 0;
			    gridBagConstraints79.anchor = GridBagConstraints.WEST;
			    gridBagConstraints79.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints79.gridy = 0;
			    lblMaximumFrameRate = new JLabel();
			    lblMaximumFrameRate.setText("Maximum Frame Rate");
			    groupMisc = new JPanel();
			    groupMisc.setLayout(new GridBagLayout());
			    groupMisc.setBorder(BorderFactory.createTitledBorder(null, "Misc", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			    groupMisc.add(lblMaximumFrameRate, gridBagConstraints79);
			    groupMisc.add(getNumMaximumFrameRate(), gridBagConstraints80);
			    groupMisc.add(lblMilliSecond, gridBagConstraints82);
			    groupMisc.add(lblMouseHoverTime, gridBagConstraints83);
			    groupMisc.add(getNumMouseHoverTime(), gridBagConstraints84);
			    groupMisc.add(lblMidiInPort, gridBagConstraints85);
			    groupMisc.add(getComboMidiInPortNumber(), gridBagConstraints86);
		    }
		    return groupMisc;
	    }

	    /**
	     * This method initializes numMaximumFrameRate	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getNumMaximumFrameRate() {
		    if (numMaximumFrameRate == null) {
			    numMaximumFrameRate = new JComboBox();
			    numMaximumFrameRate.setPreferredSize(new Dimension(120, 20));
		    }
		    return numMaximumFrameRate;
	    }

	    /**
	     * This method initializes numMouseHoverTime	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getNumMouseHoverTime() {
		    if (numMouseHoverTime == null) {
			    numMouseHoverTime = new JComboBox();
			    numMouseHoverTime.setPreferredSize(new Dimension(120, 20));
		    }
		    return numMouseHoverTime;
	    }

	    /**
	     * This method initializes comboMidiInPortNumber	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboMidiInPortNumber() {
		    if (comboMidiInPortNumber == null) {
			    comboMidiInPortNumber = new JComboBox();
			    comboMidiInPortNumber.setPreferredSize(new Dimension(239, 20));
		    }
		    return comboMidiInPortNumber;
	    }

	    /**
	     * This method initializes tabPlatform	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getTabPlatform() {
		    if (tabPlatform == null) {
			    GridBagConstraints gridBagConstraints106 = new GridBagConstraints();
			    gridBagConstraints106.gridx = 0;
			    gridBagConstraints106.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints106.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints106.weighty = 1.0D;
			    gridBagConstraints106.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints106.gridy = 2;
			    GridBagConstraints gridBagConstraints98 = new GridBagConstraints();
			    gridBagConstraints98.gridx = 0;
			    gridBagConstraints98.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints98.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints98.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints98.gridy = 1;
			    GridBagConstraints gridBagConstraints93 = new GridBagConstraints();
			    gridBagConstraints93.gridx = 0;
			    gridBagConstraints93.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints93.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints93.weightx = 1.0D;
			    gridBagConstraints93.insets = new Insets(12, 12, 3, 12);
			    gridBagConstraints93.gridy = 0;
			    tabPlatform = new JPanel();
			    tabPlatform.setLayout(new GridBagLayout());
			    tabPlatform.add(getGroupPlatform(), gridBagConstraints93);
			    tabPlatform.add(getGroupVsti(), gridBagConstraints98);
			    tabPlatform.add(getGroupUtauCores(), gridBagConstraints106);
		    }
		    return tabPlatform;
	    }

	    /**
	     * This method initializes groupPlatform	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupPlatform() {
		    if (groupPlatform == null) {
			    GridBagConstraints gridBagConstraints92 = new GridBagConstraints();
			    gridBagConstraints92.gridx = 0;
			    gridBagConstraints92.gridwidth = 2;
			    gridBagConstraints92.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints92.anchor = GridBagConstraints.WEST;
			    gridBagConstraints92.gridy = 3;
			    GridBagConstraints gridBagConstraints91 = new GridBagConstraints();
			    gridBagConstraints91.gridx = 0;
			    gridBagConstraints91.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints91.anchor = GridBagConstraints.WEST;
			    gridBagConstraints91.gridwidth = 2;
			    gridBagConstraints91.gridy = 2;
			    GridBagConstraints gridBagConstraints89 = new GridBagConstraints();
			    gridBagConstraints89.fill = GridBagConstraints.NONE;
			    gridBagConstraints89.gridy = 0;
			    gridBagConstraints89.weightx = 1.0;
			    gridBagConstraints89.anchor = GridBagConstraints.WEST;
			    gridBagConstraints89.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints89.gridx = 1;
			    GridBagConstraints gridBagConstraints88 = new GridBagConstraints();
			    gridBagConstraints88.gridx = 0;
			    gridBagConstraints88.anchor = GridBagConstraints.WEST;
			    gridBagConstraints88.insets = new Insets(3, 12, 3, 0);
			    gridBagConstraints88.gridy = 0;
			    lblPlatform = new JLabel();
			    lblPlatform.setText("Current Platform");
			    groupPlatform = new JPanel();
			    groupPlatform.setLayout(new GridBagLayout());
			    groupPlatform.setBorder(BorderFactory.createTitledBorder(null, "Platform", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			    groupPlatform.add(lblPlatform, gridBagConstraints88);
			    groupPlatform.add(getComboPlatform(), gridBagConstraints89);
			    groupPlatform.add(getChkCommandKeyAsControl(), gridBagConstraints91);
			    groupPlatform.add(getChkTranslateRoman(), gridBagConstraints92);
		    }
		    return groupPlatform;
	    }

	    /**
	     * This method initializes comboPlatform	
	     * 	
	     * @return javax.swing.JComboBox	
	     */
	    private JComboBox getComboPlatform() {
		    if (comboPlatform == null) {
			    comboPlatform = new JComboBox();
			    comboPlatform.setPreferredSize(new Dimension(121, 20));
		    }
		    return comboPlatform;
	    }

	    /**
	     * This method initializes chkCommandKeyAsControl	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkCommandKeyAsControl() {
		    if (chkCommandKeyAsControl == null) {
			    chkCommandKeyAsControl = new JCheckBox();
			    chkCommandKeyAsControl.setText("Use Command key as Control key");
		    }
		    return chkCommandKeyAsControl;
	    }

	    /**
	     * This method initializes chkTranslateRoman	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkTranslateRoman() {
		    if (chkTranslateRoman == null) {
			    chkTranslateRoman = new JCheckBox();
			    chkTranslateRoman.setText("Translate Roman letters into Kana");
		    }
		    return chkTranslateRoman;
	    }

	    /**
	     * This method initializes groupVsti	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupVsti() {
		    if (groupVsti == null) {
			    GridBagConstraints gridBagConstraints97 = new GridBagConstraints();
			    gridBagConstraints97.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints97.gridy = 1;
			    gridBagConstraints97.weightx = 1.0;
			    gridBagConstraints97.anchor = GridBagConstraints.WEST;
			    gridBagConstraints97.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints97.gridx = 1;
			    GridBagConstraints gridBagConstraints96 = new GridBagConstraints();
			    gridBagConstraints96.gridx = 0;
			    gridBagConstraints96.anchor = GridBagConstraints.WEST;
			    gridBagConstraints96.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints96.gridy = 1;
			    lblVOCALOID2 = new JLabel();
			    lblVOCALOID2.setText("VOCALOID2");
			    GridBagConstraints gridBagConstraints95 = new GridBagConstraints();
			    gridBagConstraints95.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints95.gridy = 0;
			    gridBagConstraints95.weightx = 1.0;
			    gridBagConstraints95.anchor = GridBagConstraints.WEST;
			    gridBagConstraints95.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints95.gridx = 1;
			    GridBagConstraints gridBagConstraints94 = new GridBagConstraints();
			    gridBagConstraints94.gridx = 0;
			    gridBagConstraints94.anchor = GridBagConstraints.WEST;
			    gridBagConstraints94.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints94.gridy = 0;
			    lblVOCALOID1 = new JLabel();
			    lblVOCALOID1.setText("VOCALOID1");
			    groupVsti = new JPanel();
			    groupVsti.setLayout(new GridBagLayout());
			    groupVsti.setBorder(BorderFactory.createTitledBorder(null, "VST Instruments", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			    groupVsti.add(lblVOCALOID1, gridBagConstraints94);
			    groupVsti.add(getTxtVOCALOID1(), gridBagConstraints95);
			    groupVsti.add(lblVOCALOID2, gridBagConstraints96);
			    groupVsti.add(getTxtVOCALOID2(), gridBagConstraints97);
		    }
		    return groupVsti;
	    }

	    /**
	     * This method initializes txtVOCALOID1	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtVOCALOID1() {
		    if (txtVOCALOID1 == null) {
			    txtVOCALOID1 = new JTextField();
		    }
		    return txtVOCALOID1;
	    }

	    /**
	     * This method initializes txtVOCALOID2	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtVOCALOID2() {
		    if (txtVOCALOID2 == null) {
			    txtVOCALOID2 = new JTextField();
		    }
		    return txtVOCALOID2;
	    }

	    /**
	     * This method initializes groupUtauCores	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getGroupUtauCores() {
		    if (groupUtauCores == null) {
			    GridBagConstraints gridBagConstraints105 = new GridBagConstraints();
			    gridBagConstraints105.gridx = 0;
			    gridBagConstraints105.gridwidth = 3;
			    gridBagConstraints105.anchor = GridBagConstraints.WEST;
			    gridBagConstraints105.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints105.gridy = 2;
			    GridBagConstraints gridBagConstraints104 = new GridBagConstraints();
			    gridBagConstraints104.gridx = 2;
			    gridBagConstraints104.insets = new Insets(3, 3, 3, 3);
			    gridBagConstraints104.gridy = 1;
			    GridBagConstraints gridBagConstraints103 = new GridBagConstraints();
			    gridBagConstraints103.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints103.gridy = 1;
			    gridBagConstraints103.weightx = 1.0;
			    gridBagConstraints103.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints103.gridx = 1;
			    GridBagConstraints gridBagConstraints102 = new GridBagConstraints();
			    gridBagConstraints102.gridx = 0;
			    gridBagConstraints102.gridy = 1;
			    lblWavtool = new JLabel();
			    lblWavtool.setText("wavtool");
			    GridBagConstraints gridBagConstraints101 = new GridBagConstraints();
			    gridBagConstraints101.gridx = 2;
			    gridBagConstraints101.insets = new Insets(3, 3, 3, 3);
			    gridBagConstraints101.gridy = 0;
			    GridBagConstraints gridBagConstraints100 = new GridBagConstraints();
			    gridBagConstraints100.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints100.gridy = 0;
			    gridBagConstraints100.weightx = 1.0;
			    gridBagConstraints100.insets = new Insets(3, 12, 3, 12);
			    gridBagConstraints100.anchor = GridBagConstraints.WEST;
			    gridBagConstraints100.gridx = 1;
			    GridBagConstraints gridBagConstraints99 = new GridBagConstraints();
			    gridBagConstraints99.gridx = 0;
			    gridBagConstraints99.anchor = GridBagConstraints.WEST;
			    gridBagConstraints99.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints99.gridy = 0;
			    lblResampler = new JLabel();
			    lblResampler.setText("resampler");
			    groupUtauCores = new JPanel();
			    groupUtauCores.setLayout(new GridBagLayout());
			    groupUtauCores.setBorder(BorderFactory.createTitledBorder(null, "UTAU Cores", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			    groupUtauCores.add(lblResampler, gridBagConstraints99);
			    groupUtauCores.add(getTxtResampler(), gridBagConstraints100);
			    groupUtauCores.add(getBtnResampler(), gridBagConstraints101);
			    groupUtauCores.add(lblWavtool, gridBagConstraints102);
			    groupUtauCores.add(getTxtWavtool(), gridBagConstraints103);
			    groupUtauCores.add(getBtnWavtool(), gridBagConstraints104);
			    groupUtauCores.add(getChkInvokeWithWine(), gridBagConstraints105);
		    }
		    return groupUtauCores;
	    }

	    /**
	     * This method initializes txtResampler	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtResampler() {
		    if (txtResampler == null) {
			    txtResampler = new JTextField();
		    }
		    return txtResampler;
	    }

	    /**
	     * This method initializes btnResampler	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnResampler() {
		    if (btnResampler == null) {
			    btnResampler = new JButton();
			    btnResampler.setText("...");
			    btnResampler.setPreferredSize(new Dimension(41, 23));
		    }
		    return btnResampler;
	    }

	    /**
	     * This method initializes txtWavtool	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtWavtool() {
		    if (txtWavtool == null) {
			    txtWavtool = new JTextField();
		    }
		    return txtWavtool;
	    }

	    /**
	     * This method initializes btnWavtool	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnWavtool() {
		    if (btnWavtool == null) {
			    btnWavtool = new JButton();
			    btnWavtool.setPreferredSize(new Dimension(41, 23));
			    btnWavtool.setText("...");
		    }
		    return btnWavtool;
	    }

	    /**
	     * This method initializes chkInvokeWithWine	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkInvokeWithWine() {
		    if (chkInvokeWithWine == null) {
			    chkInvokeWithWine = new JCheckBox();
			    chkInvokeWithWine.setText("Invoke UTAU cores with Wine");
		    }
		    return chkInvokeWithWine;
	    }

	    /**
	     * This method initializes tabUtauSingers	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getTabUtauSingers() {
		    if (tabUtauSingers == null) {
			    GridBagConstraints gridBagConstraints113 = new GridBagConstraints();
			    gridBagConstraints113.gridx = 1;
			    gridBagConstraints113.anchor = GridBagConstraints.EAST;
			    gridBagConstraints113.insets = new Insets(0, 12, 12, 12);
			    gridBagConstraints113.gridy = 1;
			    GridBagConstraints gridBagConstraints112 = new GridBagConstraints();
			    gridBagConstraints112.gridx = 0;
			    gridBagConstraints112.anchor = GridBagConstraints.WEST;
			    gridBagConstraints112.insets = new Insets(0, 12, 12, 12);
			    gridBagConstraints112.gridy = 1;
			    GridBagConstraints gridBagConstraints107 = new GridBagConstraints();
			    gridBagConstraints107.fill = GridBagConstraints.BOTH;
			    gridBagConstraints107.gridy = 0;
			    gridBagConstraints107.weightx = 1.0;
			    gridBagConstraints107.weighty = 1.0D;
			    gridBagConstraints107.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints107.insets = new Insets(12, 12, 12, 12);
			    gridBagConstraints107.gridwidth = 2;
			    gridBagConstraints107.gridx = 0;
			    tabUtauSingers = new JPanel();
			    tabUtauSingers.setLayout(new GridBagLayout());
			    tabUtauSingers.add(getListSingers(), gridBagConstraints107);
			    tabUtauSingers.add(getJPanel17(), gridBagConstraints112);
			    tabUtauSingers.add(getJPanel18(), gridBagConstraints113);
		    }
		    return tabUtauSingers;
	    }

	    /**
	     * This method initializes listSingers	
	     * 	
	     * @return javax.swing.JTable	
	     */
	    private JTable getListSingers() {
		    if (listSingers == null) {
			    listSingers = new JTable();
		    }
		    return listSingers;
	    }

	    /**
	     * This method initializes btnAdd	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnAdd() {
		    if (btnAdd == null) {
			    btnAdd = new JButton();
			    btnAdd.setText("Add");
			    btnAdd.setPreferredSize(new Dimension(85, 23));
		    }
		    return btnAdd;
	    }

	    /**
	     * This method initializes btnRemove	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnRemove() {
		    if (btnRemove == null) {
			    btnRemove = new JButton();
			    btnRemove.setText("Remove");
			    btnRemove.setPreferredSize(new Dimension(85, 23));
		    }
		    return btnRemove;
	    }

	    /**
	     * This method initializes btnUp	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnUp() {
		    if (btnUp == null) {
			    btnUp = new JButton();
			    btnUp.setText("Up");
			    btnUp.setPreferredSize(new Dimension(75, 23));
		    }
		    return btnUp;
	    }

	    /**
	     * This method initializes btnDown	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnDown() {
		    if (btnDown == null) {
			    btnDown = new JButton();
			    btnDown.setText("Down");
			    btnDown.setPreferredSize(new Dimension(75, 23));
		    }
		    return btnDown;
	    }

	    /**
	     * This method initializes jPanel17	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel17() {
		    if (jPanel17 == null) {
			    GridBagConstraints gridBagConstraints109 = new GridBagConstraints();
			    gridBagConstraints109.anchor = GridBagConstraints.WEST;
			    gridBagConstraints109.gridy = 0;
			    gridBagConstraints109.insets = new Insets(0, 12, 0, 0);
			    gridBagConstraints109.gridx = 1;
			    GridBagConstraints gridBagConstraints108 = new GridBagConstraints();
			    gridBagConstraints108.anchor = GridBagConstraints.WEST;
			    gridBagConstraints108.gridy = 0;
			    gridBagConstraints108.gridx = 0;
			    jPanel17 = new JPanel();
			    jPanel17.setLayout(new GridBagLayout());
			    jPanel17.add(getBtnAdd(), gridBagConstraints108);
			    jPanel17.add(getBtnRemove(), gridBagConstraints109);
		    }
		    return jPanel17;
	    }

	    /**
	     * This method initializes jPanel18	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel18() {
		    if (jPanel18 == null) {
			    GridBagConstraints gridBagConstraints111 = new GridBagConstraints();
			    gridBagConstraints111.anchor = GridBagConstraints.EAST;
			    gridBagConstraints111.gridy = 0;
			    gridBagConstraints111.gridx = 1;
			    GridBagConstraints gridBagConstraints110 = new GridBagConstraints();
			    gridBagConstraints110.anchor = GridBagConstraints.EAST;
			    gridBagConstraints110.gridy = 0;
			    gridBagConstraints110.insets = new Insets(0, 0, 0, 12);
			    gridBagConstraints110.gridx = 0;
			    jPanel18 = new JPanel();
			    jPanel18.setLayout(new GridBagLayout());
			    jPanel18.add(getBtnUp(), gridBagConstraints110);
			    jPanel18.add(getBtnDown(), gridBagConstraints111);
		    }
		    return jPanel18;
	    }

	    /**
	     * This method initializes tabFile	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getTabFile() {
		    if (tabFile == null) {
			    GridBagConstraints gridBagConstraints118 = new GridBagConstraints();
			    gridBagConstraints118.gridx = 0;
			    gridBagConstraints118.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints118.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints118.weighty = 1.0D;
			    gridBagConstraints118.weightx = 1.0D;
			    gridBagConstraints118.gridy = 0;
			    lblAutoBackupInterval = new JLabel();
			    lblAutoBackupInterval.setText("interval");
			    tabFile = new JPanel();
			    tabFile.setLayout(new GridBagLayout());
			    tabFile.add(getJPanel20(), gridBagConstraints118);
		    }
		    return tabFile;
	    }

	    /**
	     * This method initializes chkAutoBackup	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkAutoBackup() {
		    if (chkAutoBackup == null) {
			    chkAutoBackup = new JCheckBox();
			    chkAutoBackup.setText("Automatical Backup");
		    }
		    return chkAutoBackup;
	    }

	    /**
	     * This method initializes lblAutoBackupMinutes	
	     * 	
	     * @return javax.swing.JLabel	
	     */
	    private JLabel getLblAutoBackupMinutes() {
		    if (lblAutoBackupMinutes == null) {
			    lblAutoBackupMinutes = new JLabel();
			    lblAutoBackupMinutes.setText("minutes");
		    }
		    return lblAutoBackupMinutes;
	    }

	    /**
	     * This method initializes jPanel20	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel20() {
		    if (jPanel20 == null) {
			    GridBagConstraints gridBagConstraints121 = new GridBagConstraints();
			    gridBagConstraints121.fill = GridBagConstraints.NONE;
			    gridBagConstraints121.gridy = 0;
			    gridBagConstraints121.weightx = 1.0;
			    gridBagConstraints121.insets = new Insets(3, 6, 3, 12);
			    gridBagConstraints121.gridx = 2;
			    GridBagConstraints gridBagConstraints117 = new GridBagConstraints();
			    gridBagConstraints117.anchor = GridBagConstraints.WEST;
			    gridBagConstraints117.gridy = 0;
			    gridBagConstraints117.weightx = 1.0D;
			    gridBagConstraints117.gridx = 3;
			    GridBagConstraints gridBagConstraints116 = new GridBagConstraints();
			    gridBagConstraints116.anchor = GridBagConstraints.WEST;
			    gridBagConstraints116.insets = new Insets(3, 6, 3, 6);
			    gridBagConstraints116.gridx = 2;
			    gridBagConstraints116.gridy = 0;
			    gridBagConstraints116.weightx = 0.0D;
			    gridBagConstraints116.weighty = 1.0;
			    gridBagConstraints116.fill = GridBagConstraints.NONE;
			    GridBagConstraints gridBagConstraints115 = new GridBagConstraints();
			    gridBagConstraints115.anchor = GridBagConstraints.WEST;
			    gridBagConstraints115.gridx = 1;
			    gridBagConstraints115.gridy = 0;
			    gridBagConstraints115.insets = new Insets(0, 24, 0, 0);
			    GridBagConstraints gridBagConstraints114 = new GridBagConstraints();
			    gridBagConstraints114.anchor = GridBagConstraints.WEST;
			    gridBagConstraints114.gridx = 0;
			    gridBagConstraints114.gridy = 0;
			    gridBagConstraints114.insets = new Insets(3, 12, 3, 0);
			    jPanel20 = new JPanel();
			    jPanel20.setLayout(new GridBagLayout());
			    jPanel20.add(getChkAutoBackup(), gridBagConstraints114);
			    jPanel20.add(lblAutoBackupInterval, gridBagConstraints115);
			    jPanel20.add(getLblAutoBackupMinutes(), gridBagConstraints117);
			    jPanel20.add(getNumAutoBackupInterval(), gridBagConstraints121);
		    }
		    return jPanel20;
	    }

	    /**
	     * This method initializes panelLower	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getPanelLower() {
		    if (panelLower == null) {
			    GridBagConstraints gridBagConstraints311 = new GridBagConstraints();
			    gridBagConstraints311.insets = new Insets(0, 0, 0, 12);
			    gridBagConstraints311.gridy = 1;
			    gridBagConstraints311.gridx = 1;
			    GridBagConstraints gridBagConstraints211 = new GridBagConstraints();
			    gridBagConstraints211.insets = new Insets(0, 0, 0, 16);
			    gridBagConstraints211.gridy = 1;
			    gridBagConstraints211.gridx = 0;
			    panelLower = new JPanel();
			    panelLower.setLayout(new GridBagLayout());
			    panelLower.add(getBtnOK(), gridBagConstraints211);
			    panelLower.add(getBtnCancel(), gridBagConstraints311);
		    }
		    return panelLower;
	    }

	    /**
	     * This method initializes btnOK	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnOK() {
		    if (btnOK == null) {
			    btnOK = new JButton();
			    btnOK.setText("OK");
		    }
		    return btnOK;
	    }

	    /**
	     * This method initializes btnCancel	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnCancel() {
		    if (btnCancel == null) {
			    btnCancel = new JButton();
			    btnCancel.setText("Cancel");
		    }
		    return btnCancel;
	    }

	    /**
	     * This method initializes jPanel5	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel5() {
		    if (jPanel5 == null) {
			    GridBagConstraints gridBagConstraints120 = new GridBagConstraints();
			    gridBagConstraints120.gridx = 0;
			    gridBagConstraints120.fill = GridBagConstraints.BOTH;
			    gridBagConstraints120.weightx = 1.0D;
			    gridBagConstraints120.weighty = 1.0D;
			    gridBagConstraints120.gridy = 0;
			    GridBagConstraints gridBagConstraints119 = new GridBagConstraints();
			    gridBagConstraints119.gridx = 0;
			    gridBagConstraints119.anchor = GridBagConstraints.EAST;
			    gridBagConstraints119.insets = new Insets(12, 0, 12, 0);
			    gridBagConstraints119.gridy = 1;
			    jPanel5 = new JPanel();
			    jPanel5.setLayout(new GridBagLayout());
			    jPanel5.add(getPanelUpper(), gridBagConstraints120);
			    jPanel5.add(getPanelLower(), gridBagConstraints119);
		    }
		    return jPanel5;
	    }

	    /**
	     * This method initializes panelUpper	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getPanelUpper() {
		    if (panelUpper == null) {
			    panelUpper = new JPanel();
			    panelUpper.setLayout(new GridBagLayout());
		    }
		    return panelUpper;
	    }

	    /**
	     * This method initializes numAutoBackupInterval	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getNumAutoBackupInterval() {
		    if (numAutoBackupInterval == null) {
			    numAutoBackupInterval = new JTextField();
			    numAutoBackupInterval.setPreferredSize(new Dimension(69, 20));
		    }
		    return numAutoBackupInterval;
	    }
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
            this.tabPreference = new System.Windows.Forms.TabControl();
            this.tabSequence = new System.Windows.Forms.TabPage();
            this.label5 = new bocoree.windows.forms.BLabel();
            this.label4 = new bocoree.windows.forms.BLabel();
            this.label2 = new bocoree.windows.forms.BLabel();
            this.comboPeriod = new bocoree.windows.forms.BComboBox();
            this.comboAmplitude = new bocoree.windows.forms.BComboBox();
            this.comboDynamics = new bocoree.windows.forms.BComboBox();
            this.lblPeriod = new bocoree.windows.forms.BLabel();
            this.lblAmplitude = new bocoree.windows.forms.BLabel();
            this.lblDynamics = new bocoree.windows.forms.BLabel();
            this.label1 = new bocoree.windows.forms.BLabel();
            this.lblResolution = new bocoree.windows.forms.BLabel();
            this.label7 = new bocoree.windows.forms.BLabel();
            this.groupAutoVibratoConfig = new BGroupBox();
            this.comboAutoVibratoType2 = new bocoree.windows.forms.BComboBox();
            this.lblAutoVibratoType2 = new bocoree.windows.forms.BLabel();
            this.label6 = new bocoree.windows.forms.BLabel();
            this.comboAutoVibratoType1 = new bocoree.windows.forms.BComboBox();
            this.comboAutoVibratoMinLength = new bocoree.windows.forms.BComboBox();
            this.lblAutoVibratoType1 = new bocoree.windows.forms.BLabel();
            this.lblAutoVibratoMinLength = new bocoree.windows.forms.BLabel();
            this.chkEnableAutoVibrato = new bocoree.windows.forms.BCheckBox();
            this.label3 = new bocoree.windows.forms.BLabel();
            this.comboVibratoLength = new bocoree.windows.forms.BComboBox();
            this.lblVibratoLength = new bocoree.windows.forms.BLabel();
            this.lblVibratoConfig = new bocoree.windows.forms.BLabel();
            this.tabAnother = new System.Windows.Forms.TabPage();
            this.label15 = new bocoree.windows.forms.BLabel();
            this.label14 = new bocoree.windows.forms.BLabel();
            this.label13 = new bocoree.windows.forms.BLabel();
            this.label12 = new bocoree.windows.forms.BLabel();
            this.comboDefaultPremeasure = new bocoree.windows.forms.BComboBox();
            this.comboDefualtSinger = new bocoree.windows.forms.BComboBox();
            this.label11 = new bocoree.windows.forms.BLabel();
            this.lblPreSendTimeSample = new bocoree.windows.forms.BLabel();
            this.lblTiming = new bocoree.windows.forms.BLabel();
            this.chkEnableSampleOutput = new bocoree.windows.forms.BCheckBox();
            this.lblSampleOutput = new bocoree.windows.forms.BLabel();
            this.chkChasePastEvent = new bocoree.windows.forms.BCheckBox();
            this.lblDefaultPremeasure = new bocoree.windows.forms.BLabel();
            this.lblWait = new bocoree.windows.forms.BLabel();
            this.lblPreSendTime = new bocoree.windows.forms.BLabel();
            this.lblDefaultSinger = new bocoree.windows.forms.BLabel();
            this.numPreSendTimeSample = new Boare.Cadencii.NumericUpDownEx();
            this.numTiming = new Boare.Cadencii.NumericUpDownEx();
            this.numWait = new Boare.Cadencii.NumericUpDownEx();
            this.numPreSendTime = new Boare.Cadencii.NumericUpDownEx();
            this.tabAppearance = new System.Windows.Forms.TabPage();
            this.groupFont = new BGroupBox();
            this.labelMenu = new bocoree.windows.forms.BLabel();
            this.labelScreenFontName = new bocoree.windows.forms.BLabel();
            this.btnChangeScreenFont = new bocoree.windows.forms.BButton();
            this.labelScreen = new bocoree.windows.forms.BLabel();
            this.labelMenuFontName = new bocoree.windows.forms.BLabel();
            this.btnChangeMenuFont = new bocoree.windows.forms.BButton();
            this.groupVisibleCurve = new BGroupBox();
            this.chkEnvelope = new bocoree.windows.forms.BCheckBox();
            this.chkPbs = new bocoree.windows.forms.BCheckBox();
            this.chkReso4 = new bocoree.windows.forms.BCheckBox();
            this.chkReso3 = new bocoree.windows.forms.BCheckBox();
            this.chkReso2 = new bocoree.windows.forms.BCheckBox();
            this.chkReso1 = new bocoree.windows.forms.BCheckBox();
            this.chkFx2Depth = new bocoree.windows.forms.BCheckBox();
            this.chkHarmonics = new bocoree.windows.forms.BCheckBox();
            this.chkPit = new bocoree.windows.forms.BCheckBox();
            this.chkPor = new bocoree.windows.forms.BCheckBox();
            this.chkGen = new bocoree.windows.forms.BCheckBox();
            this.chkOpe = new bocoree.windows.forms.BCheckBox();
            this.chkCle = new bocoree.windows.forms.BCheckBox();
            this.chkBri = new bocoree.windows.forms.BCheckBox();
            this.chkBre = new bocoree.windows.forms.BCheckBox();
            this.chkDyn = new bocoree.windows.forms.BCheckBox();
            this.chkVel = new bocoree.windows.forms.BCheckBox();
            this.chkVibratoDepth = new bocoree.windows.forms.BCheckBox();
            this.chkVibratoRate = new bocoree.windows.forms.BCheckBox();
            this.chkDecay = new bocoree.windows.forms.BCheckBox();
            this.chkAccent = new bocoree.windows.forms.BCheckBox();
            this.lblTrackHeight = new bocoree.windows.forms.BLabel();
            this.comboLanguage = new bocoree.windows.forms.BComboBox();
            this.lblLanguage = new bocoree.windows.forms.BLabel();
            this.numTrackHeight = new Boare.Cadencii.NumericUpDownEx();
            this.tabOperation = new System.Windows.Forms.TabPage();
            this.groupMisc = new BGroupBox();
            this.lblMaximumFrameRate = new bocoree.windows.forms.BLabel();
            this.comboMidiInPortNumber = new bocoree.windows.forms.BComboBox();
            this.numMaximumFrameRate = new Boare.Cadencii.NumericUpDownEx();
            this.lblMidiInPort = new bocoree.windows.forms.BLabel();
            this.lblMouseHoverTime = new bocoree.windows.forms.BLabel();
            this.lblMilliSecond = new bocoree.windows.forms.BLabel();
            this.numMouseHoverTime = new Boare.Cadencii.NumericUpDownEx();
            this.groupPianoroll = new BGroupBox();
            this.chkUseSpaceKeyAsMiddleButtonModifier = new bocoree.windows.forms.BCheckBox();
            this.labelWheelOrder = new bocoree.windows.forms.BLabel();
            this.numericUpDownEx1 = new Boare.Cadencii.NumericUpDownEx();
            this.chkCursorFix = new bocoree.windows.forms.BCheckBox();
            this.chkCurveSelectingQuantized = new bocoree.windows.forms.BCheckBox();
            this.chkScrollHorizontal = new bocoree.windows.forms.BCheckBox();
            this.chkPlayPreviewWhenRightClick = new bocoree.windows.forms.BCheckBox();
            this.chkKeepLyricInputMode = new bocoree.windows.forms.BCheckBox();
            this.tabPlatform = new System.Windows.Forms.TabPage();
            this.groupUtauCores = new BGroupBox();
            this.lblResampler = new bocoree.windows.forms.BLabel();
            this.chkInvokeWithWine = new bocoree.windows.forms.BCheckBox();
            this.btnWavtool = new bocoree.windows.forms.BButton();
            this.txtResampler = new bocoree.windows.forms.BTextBox();
            this.lblWavtool = new bocoree.windows.forms.BLabel();
            this.btnResampler = new bocoree.windows.forms.BButton();
            this.txtWavtool = new bocoree.windows.forms.BTextBox();
            this.groupVsti = new BGroupBox();
            this.txtVOCALOID2 = new bocoree.windows.forms.BTextBox();
            this.txtVOCALOID1 = new bocoree.windows.forms.BTextBox();
            this.lblVOCALOID2 = new bocoree.windows.forms.BLabel();
            this.lblVOCALOID1 = new bocoree.windows.forms.BLabel();
            this.groupPlatform = new BGroupBox();
            this.chkTranslateRoman = new bocoree.windows.forms.BCheckBox();
            this.comboPlatform = new bocoree.windows.forms.BComboBox();
            this.lblPlatform = new bocoree.windows.forms.BLabel();
            this.chkCommandKeyAsControl = new bocoree.windows.forms.BCheckBox();
            this.tabUtauSingers = new System.Windows.Forms.TabPage();
            this.btnRemove = new bocoree.windows.forms.BButton();
            this.btnAdd = new bocoree.windows.forms.BButton();
            this.btnUp = new bocoree.windows.forms.BButton();
            this.btnDown = new bocoree.windows.forms.BButton();
            this.listSingers = new BListView();
            this.tabFile = new System.Windows.Forms.TabPage();
            this.lblAutoBackupMinutes = new bocoree.windows.forms.BLabel();
            this.numAutoBackupInterval = new Boare.Cadencii.NumericUpDownEx();
            this.lblAutoBackupInterval = new bocoree.windows.forms.BLabel();
            this.chkAutoBackup = new bocoree.windows.forms.BCheckBox();
            this.btnCancel = new bocoree.windows.forms.BButton();
            this.btnOK = new bocoree.windows.forms.BButton();
            this.tabPreference.SuspendLayout();
            this.tabSequence.SuspendLayout();
            this.groupAutoVibratoConfig.SuspendLayout();
            this.tabAnother.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPreSendTimeSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTiming)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreSendTime)).BeginInit();
            this.tabAppearance.SuspendLayout();
            this.groupFont.SuspendLayout();
            this.groupVisibleCurve.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTrackHeight)).BeginInit();
            this.tabOperation.SuspendLayout();
            this.groupMisc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaximumFrameRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMouseHoverTime)).BeginInit();
            this.groupPianoroll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEx1)).BeginInit();
            this.tabPlatform.SuspendLayout();
            this.groupUtauCores.SuspendLayout();
            this.groupVsti.SuspendLayout();
            this.groupPlatform.SuspendLayout();
            this.tabUtauSingers.SuspendLayout();
            this.tabFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoBackupInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPreference
            // 
            this.tabPreference.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPreference.Controls.Add( this.tabSequence );
            this.tabPreference.Controls.Add( this.tabAnother );
            this.tabPreference.Controls.Add( this.tabAppearance );
            this.tabPreference.Controls.Add( this.tabOperation );
            this.tabPreference.Controls.Add( this.tabPlatform );
            this.tabPreference.Controls.Add( this.tabUtauSingers );
            this.tabPreference.Controls.Add( this.tabFile );
            this.tabPreference.Location = new System.Drawing.Point( 7, 7 );
            this.tabPreference.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 3 );
            this.tabPreference.Multiline = true;
            this.tabPreference.Name = "tabPreference";
            this.tabPreference.SelectedIndex = 0;
            this.tabPreference.Size = new System.Drawing.Size( 462, 393 );
            this.tabPreference.TabIndex = 0;
            // 
            // tabSequence
            // 
            this.tabSequence.Controls.Add( this.label5 );
            this.tabSequence.Controls.Add( this.label4 );
            this.tabSequence.Controls.Add( this.label2 );
            this.tabSequence.Controls.Add( this.comboPeriod );
            this.tabSequence.Controls.Add( this.comboAmplitude );
            this.tabSequence.Controls.Add( this.comboDynamics );
            this.tabSequence.Controls.Add( this.lblPeriod );
            this.tabSequence.Controls.Add( this.lblAmplitude );
            this.tabSequence.Controls.Add( this.lblDynamics );
            this.tabSequence.Controls.Add( this.label1 );
            this.tabSequence.Controls.Add( this.lblResolution );
            this.tabSequence.Controls.Add( this.label7 );
            this.tabSequence.Controls.Add( this.groupAutoVibratoConfig );
            this.tabSequence.Controls.Add( this.label3 );
            this.tabSequence.Controls.Add( this.comboVibratoLength );
            this.tabSequence.Controls.Add( this.lblVibratoLength );
            this.tabSequence.Controls.Add( this.lblVibratoConfig );
            this.tabSequence.Location = new System.Drawing.Point( 4, 38 );
            this.tabSequence.Name = "tabSequence";
            this.tabSequence.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabSequence.Size = new System.Drawing.Size( 454, 351 );
            this.tabSequence.TabIndex = 0;
            this.tabSequence.Text = "Sequence";
            this.tabSequence.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 286, 96 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 38, 12 );
            this.label5.TabIndex = 34;
            this.label5.Text = "clocks";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 286, 67 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 38, 12 );
            this.label4.TabIndex = 33;
            this.label4.Text = "clocks";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 286, 38 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 38, 12 );
            this.label2.TabIndex = 32;
            this.label2.Text = "clocks";
            // 
            // comboPeriod
            // 
            this.comboPeriod.Enabled = false;
            this.comboPeriod.FormattingEnabled = true;
            this.comboPeriod.Location = new System.Drawing.Point( 179, 93 );
            this.comboPeriod.Name = "comboPeriod";
            this.comboPeriod.Size = new System.Drawing.Size( 101, 20 );
            this.comboPeriod.TabIndex = 3;
            // 
            // comboAmplitude
            // 
            this.comboAmplitude.Enabled = false;
            this.comboAmplitude.FormattingEnabled = true;
            this.comboAmplitude.Location = new System.Drawing.Point( 179, 63 );
            this.comboAmplitude.Name = "comboAmplitude";
            this.comboAmplitude.Size = new System.Drawing.Size( 101, 20 );
            this.comboAmplitude.TabIndex = 2;
            // 
            // comboDynamics
            // 
            this.comboDynamics.FormattingEnabled = true;
            this.comboDynamics.Location = new System.Drawing.Point( 179, 35 );
            this.comboDynamics.Name = "comboDynamics";
            this.comboDynamics.Size = new System.Drawing.Size( 101, 20 );
            this.comboDynamics.TabIndex = 1;
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point( 35, 96 );
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size( 86, 12 );
            this.lblPeriod.TabIndex = 28;
            this.lblPeriod.Text = "Vibrato Rate(&V)";
            // 
            // lblAmplitude
            // 
            this.lblAmplitude.AutoSize = true;
            this.lblAmplitude.Location = new System.Drawing.Point( 35, 67 );
            this.lblAmplitude.Name = "lblAmplitude";
            this.lblAmplitude.Size = new System.Drawing.Size( 92, 12 );
            this.lblAmplitude.TabIndex = 27;
            this.lblAmplitude.Text = "Vibrato Depth(&R)";
            // 
            // lblDynamics
            // 
            this.lblDynamics.AutoSize = true;
            this.lblDynamics.Location = new System.Drawing.Point( 35, 38 );
            this.lblDynamics.Name = "lblDynamics";
            this.lblDynamics.Size = new System.Drawing.Size( 71, 12 );
            this.lblDynamics.TabIndex = 26;
            this.lblDynamics.Text = "Dynamics(&D)";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label1.Location = new System.Drawing.Point( 115, 17 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 323, 1 );
            this.label1.TabIndex = 25;
            // 
            // lblResolution
            // 
            this.lblResolution.AutoSize = true;
            this.lblResolution.Location = new System.Drawing.Point( 15, 13 );
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new System.Drawing.Size( 92, 12 );
            this.lblResolution.TabIndex = 24;
            this.lblResolution.Text = "Resolution(VSTi)";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label7.Location = new System.Drawing.Point( 115, 137 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 323, 1 );
            this.label7.TabIndex = 5;
            // 
            // groupAutoVibratoConfig
            // 
            this.groupAutoVibratoConfig.Controls.Add( this.comboAutoVibratoType2 );
            this.groupAutoVibratoConfig.Controls.Add( this.lblAutoVibratoType2 );
            this.groupAutoVibratoConfig.Controls.Add( this.label6 );
            this.groupAutoVibratoConfig.Controls.Add( this.comboAutoVibratoType1 );
            this.groupAutoVibratoConfig.Controls.Add( this.comboAutoVibratoMinLength );
            this.groupAutoVibratoConfig.Controls.Add( this.lblAutoVibratoType1 );
            this.groupAutoVibratoConfig.Controls.Add( this.lblAutoVibratoMinLength );
            this.groupAutoVibratoConfig.Controls.Add( this.chkEnableAutoVibrato );
            this.groupAutoVibratoConfig.Location = new System.Drawing.Point( 37, 191 );
            this.groupAutoVibratoConfig.Name = "groupAutoVibratoConfig";
            this.groupAutoVibratoConfig.Size = new System.Drawing.Size( 401, 140 );
            this.groupAutoVibratoConfig.TabIndex = 5;
            this.groupAutoVibratoConfig.TabStop = false;
            this.groupAutoVibratoConfig.Text = "Auto Vibrato Settings";
            // 
            // comboAutoVibratoType2
            // 
            this.comboAutoVibratoType2.FormattingEnabled = true;
            this.comboAutoVibratoType2.Items.AddRange( new object[] {
            "[Normal] Type 1",
            "[Normal] Type 2",
            "[Normal] Type 3",
            "[Normal] Type 4",
            "[Extreme] Type 1",
            "[Extreme] Type 2",
            "[Extreme] Type 3",
            "[Extreme] Type 4",
            "[Fast] Type 1",
            "[Fast] Type 2",
            "[Fast] Type 3",
            "[Fast] Type 4",
            "[Slight] Type 1",
            "[Slight] Type 2",
            "[Slight] Type 3",
            "[Slight] Type 4"} );
            this.comboAutoVibratoType2.Location = new System.Drawing.Point( 209, 105 );
            this.comboAutoVibratoType2.Name = "comboAutoVibratoType2";
            this.comboAutoVibratoType2.Size = new System.Drawing.Size( 182, 20 );
            this.comboAutoVibratoType2.TabIndex = 28;
            // 
            // lblAutoVibratoType2
            // 
            this.lblAutoVibratoType2.AutoSize = true;
            this.lblAutoVibratoType2.Location = new System.Drawing.Point( 19, 108 );
            this.lblAutoVibratoType2.Name = "lblAutoVibratoType2";
            this.lblAutoVibratoType2.Size = new System.Drawing.Size( 155, 12 );
            this.lblAutoVibratoType2.TabIndex = 29;
            this.lblAutoVibratoType2.Text = "Vibrato Type: VOCALOID2(&T)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 364, 54 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 27, 12 );
            this.label6.TabIndex = 26;
            this.label6.Text = "beat";
            // 
            // comboAutoVibratoType1
            // 
            this.comboAutoVibratoType1.FormattingEnabled = true;
            this.comboAutoVibratoType1.Items.AddRange( new object[] {
            "[Normal] Type 1",
            "[Normal] Type 2",
            "[Normal] Type 3",
            "[Normal] Type 4",
            "[Extreme] Type 1",
            "[Extreme] Type 2",
            "[Extreme] Type 3",
            "[Extreme] Type 4",
            "[Fast] Type 1",
            "[Fast] Type 2",
            "[Fast] Type 3",
            "[Fast] Type 4",
            "[Slight] Type 1",
            "[Slight] Type 2",
            "[Slight] Type 3",
            "[Slight] Type 4"} );
            this.comboAutoVibratoType1.Location = new System.Drawing.Point( 209, 79 );
            this.comboAutoVibratoType1.Name = "comboAutoVibratoType1";
            this.comboAutoVibratoType1.Size = new System.Drawing.Size( 182, 20 );
            this.comboAutoVibratoType1.TabIndex = 8;
            // 
            // comboAutoVibratoMinLength
            // 
            this.comboAutoVibratoMinLength.FormattingEnabled = true;
            this.comboAutoVibratoMinLength.Items.AddRange( new object[] {
            "1",
            "2",
            "3",
            "4"} );
            this.comboAutoVibratoMinLength.Location = new System.Drawing.Point( 292, 51 );
            this.comboAutoVibratoMinLength.Name = "comboAutoVibratoMinLength";
            this.comboAutoVibratoMinLength.Size = new System.Drawing.Size( 66, 20 );
            this.comboAutoVibratoMinLength.TabIndex = 7;
            // 
            // lblAutoVibratoType1
            // 
            this.lblAutoVibratoType1.AutoSize = true;
            this.lblAutoVibratoType1.Location = new System.Drawing.Point( 19, 82 );
            this.lblAutoVibratoType1.Name = "lblAutoVibratoType1";
            this.lblAutoVibratoType1.Size = new System.Drawing.Size( 155, 12 );
            this.lblAutoVibratoType1.TabIndex = 27;
            this.lblAutoVibratoType1.Text = "Vibrato Type: VOCALOID1(&T)";
            // 
            // lblAutoVibratoMinLength
            // 
            this.lblAutoVibratoMinLength.AutoSize = true;
            this.lblAutoVibratoMinLength.Location = new System.Drawing.Point( 19, 54 );
            this.lblAutoVibratoMinLength.Name = "lblAutoVibratoMinLength";
            this.lblAutoVibratoMinLength.Size = new System.Drawing.Size( 243, 12 );
            this.lblAutoVibratoMinLength.TabIndex = 24;
            this.lblAutoVibratoMinLength.Text = "Minimum note length for Automatic Vibrato(&M)";
            // 
            // chkEnableAutoVibrato
            // 
            this.chkEnableAutoVibrato.AutoSize = true;
            this.chkEnableAutoVibrato.Location = new System.Drawing.Point( 19, 22 );
            this.chkEnableAutoVibrato.Name = "chkEnableAutoVibrato";
            this.chkEnableAutoVibrato.Size = new System.Drawing.Size( 170, 16 );
            this.chkEnableAutoVibrato.TabIndex = 6;
            this.chkEnableAutoVibrato.Text = "Enable Automatic Vibrato(&E)";
            this.chkEnableAutoVibrato.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 311, 162 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 11, 12 );
            this.label3.TabIndex = 22;
            this.label3.Text = "%";
            // 
            // comboVibratoLength
            // 
            this.comboVibratoLength.FormattingEnabled = true;
            this.comboVibratoLength.Items.AddRange( new object[] {
            "50",
            "66",
            "75",
            "100"} );
            this.comboVibratoLength.Location = new System.Drawing.Point( 214, 159 );
            this.comboVibratoLength.Name = "comboVibratoLength";
            this.comboVibratoLength.Size = new System.Drawing.Size( 86, 20 );
            this.comboVibratoLength.TabIndex = 4;
            // 
            // lblVibratoLength
            // 
            this.lblVibratoLength.AutoSize = true;
            this.lblVibratoLength.Location = new System.Drawing.Point( 35, 162 );
            this.lblVibratoLength.Name = "lblVibratoLength";
            this.lblVibratoLength.Size = new System.Drawing.Size( 135, 12 );
            this.lblVibratoLength.TabIndex = 20;
            this.lblVibratoLength.Text = "Default Vibrato Length(&L)";
            // 
            // lblVibratoConfig
            // 
            this.lblVibratoConfig.AutoSize = true;
            this.lblVibratoConfig.Location = new System.Drawing.Point( 15, 133 );
            this.lblVibratoConfig.Name = "lblVibratoConfig";
            this.lblVibratoConfig.Size = new System.Drawing.Size( 88, 12 );
            this.lblVibratoConfig.TabIndex = 0;
            this.lblVibratoConfig.Text = "Vibrato Settings";
            // 
            // tabAnother
            // 
            this.tabAnother.Controls.Add( this.label15 );
            this.tabAnother.Controls.Add( this.label14 );
            this.tabAnother.Controls.Add( this.label13 );
            this.tabAnother.Controls.Add( this.label12 );
            this.tabAnother.Controls.Add( this.comboDefaultPremeasure );
            this.tabAnother.Controls.Add( this.comboDefualtSinger );
            this.tabAnother.Controls.Add( this.label11 );
            this.tabAnother.Controls.Add( this.lblPreSendTimeSample );
            this.tabAnother.Controls.Add( this.lblTiming );
            this.tabAnother.Controls.Add( this.chkEnableSampleOutput );
            this.tabAnother.Controls.Add( this.lblSampleOutput );
            this.tabAnother.Controls.Add( this.chkChasePastEvent );
            this.tabAnother.Controls.Add( this.lblDefaultPremeasure );
            this.tabAnother.Controls.Add( this.lblWait );
            this.tabAnother.Controls.Add( this.lblPreSendTime );
            this.tabAnother.Controls.Add( this.lblDefaultSinger );
            this.tabAnother.Controls.Add( this.numPreSendTimeSample );
            this.tabAnother.Controls.Add( this.numTiming );
            this.tabAnother.Controls.Add( this.numWait );
            this.tabAnother.Controls.Add( this.numPreSendTime );
            this.tabAnother.Location = new System.Drawing.Point( 4, 38 );
            this.tabAnother.Name = "tabAnother";
            this.tabAnother.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabAnother.Size = new System.Drawing.Size( 454, 351 );
            this.tabAnother.TabIndex = 2;
            this.tabAnother.Text = "Other Settings";
            this.tabAnother.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point( 372, 292 );
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size( 76, 12 );
            this.label15.TabIndex = 21;
            this.label15.Text = "msec(50-500)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point( 305, 261 );
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size( 88, 12 );
            this.label14.TabIndex = 19;
            this.label14.Text = "msec(500-1500)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point( 305, 89 );
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size( 88, 12 );
            this.label13.TabIndex = 17;
            this.label13.Text = "msec(200-2000)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point( 305, 58 );
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size( 88, 12 );
            this.label12.TabIndex = 16;
            this.label12.Text = "msec(500-5000)";
            // 
            // comboDefaultPremeasure
            // 
            this.comboDefaultPremeasure.FormattingEnabled = true;
            this.comboDefaultPremeasure.Items.AddRange( new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"} );
            this.comboDefaultPremeasure.Location = new System.Drawing.Point( 216, 117 );
            this.comboDefaultPremeasure.Name = "comboDefaultPremeasure";
            this.comboDefaultPremeasure.Size = new System.Drawing.Size( 68, 20 );
            this.comboDefaultPremeasure.TabIndex = 23;
            // 
            // comboDefualtSinger
            // 
            this.comboDefualtSinger.Enabled = false;
            this.comboDefualtSinger.FormattingEnabled = true;
            this.comboDefualtSinger.Location = new System.Drawing.Point( 216, 24 );
            this.comboDefualtSinger.Name = "comboDefualtSinger";
            this.comboDefualtSinger.Size = new System.Drawing.Size( 222, 20 );
            this.comboDefualtSinger.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label11.Location = new System.Drawing.Point( 147, 199 );
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size( 291, 1 );
            this.label11.TabIndex = 9;
            // 
            // lblPreSendTimeSample
            // 
            this.lblPreSendTimeSample.AutoSize = true;
            this.lblPreSendTimeSample.Location = new System.Drawing.Point( 55, 292 );
            this.lblPreSendTimeSample.Name = "lblPreSendTimeSample";
            this.lblPreSendTimeSample.Size = new System.Drawing.Size( 190, 12 );
            this.lblPreSendTimeSample.TabIndex = 8;
            this.lblPreSendTimeSample.Text = "Pre-Send Time for sample sound(&G)";
            // 
            // lblTiming
            // 
            this.lblTiming.AutoSize = true;
            this.lblTiming.Location = new System.Drawing.Point( 55, 261 );
            this.lblTiming.Name = "lblTiming";
            this.lblTiming.Size = new System.Drawing.Size( 39, 12 );
            this.lblTiming.TabIndex = 7;
            this.lblTiming.Text = "Timing";
            // 
            // chkEnableSampleOutput
            // 
            this.chkEnableSampleOutput.AutoSize = true;
            this.chkEnableSampleOutput.Enabled = false;
            this.chkEnableSampleOutput.Location = new System.Drawing.Point( 31, 226 );
            this.chkEnableSampleOutput.Name = "chkEnableSampleOutput";
            this.chkEnableSampleOutput.Size = new System.Drawing.Size( 73, 16 );
            this.chkEnableSampleOutput.TabIndex = 25;
            this.chkEnableSampleOutput.Text = "Enable(&E)";
            this.chkEnableSampleOutput.UseVisualStyleBackColor = true;
            // 
            // lblSampleOutput
            // 
            this.lblSampleOutput.AutoSize = true;
            this.lblSampleOutput.Location = new System.Drawing.Point( 16, 195 );
            this.lblSampleOutput.Name = "lblSampleOutput";
            this.lblSampleOutput.Size = new System.Drawing.Size( 127, 12 );
            this.lblSampleOutput.TabIndex = 5;
            this.lblSampleOutput.Text = "Playback Sample Sound";
            // 
            // chkChasePastEvent
            // 
            this.chkChasePastEvent.AutoSize = true;
            this.chkChasePastEvent.Enabled = false;
            this.chkChasePastEvent.Location = new System.Drawing.Point( 31, 151 );
            this.chkChasePastEvent.Name = "chkChasePastEvent";
            this.chkChasePastEvent.Size = new System.Drawing.Size( 105, 16 );
            this.chkChasePastEvent.TabIndex = 24;
            this.chkChasePastEvent.Text = "Chase Event(&C)";
            this.chkChasePastEvent.UseVisualStyleBackColor = true;
            // 
            // lblDefaultPremeasure
            // 
            this.lblDefaultPremeasure.AutoSize = true;
            this.lblDefaultPremeasure.Location = new System.Drawing.Point( 29, 120 );
            this.lblDefaultPremeasure.Name = "lblDefaultPremeasure";
            this.lblDefaultPremeasure.Size = new System.Drawing.Size( 129, 12 );
            this.lblDefaultPremeasure.TabIndex = 3;
            this.lblDefaultPremeasure.Text = "Default Pre-measure(&M)";
            // 
            // lblWait
            // 
            this.lblWait.AutoSize = true;
            this.lblWait.Location = new System.Drawing.Point( 29, 89 );
            this.lblWait.Name = "lblWait";
            this.lblWait.Size = new System.Drawing.Size( 88, 12 );
            this.lblWait.TabIndex = 2;
            this.lblWait.Text = "Waiting Time(&W)";
            // 
            // lblPreSendTime
            // 
            this.lblPreSendTime.AutoSize = true;
            this.lblPreSendTime.Location = new System.Drawing.Point( 29, 58 );
            this.lblPreSendTime.Name = "lblPreSendTime";
            this.lblPreSendTime.Size = new System.Drawing.Size( 94, 12 );
            this.lblPreSendTime.TabIndex = 1;
            this.lblPreSendTime.Text = "Pre-Send time(&P)";
            // 
            // lblDefaultSinger
            // 
            this.lblDefaultSinger.AutoSize = true;
            this.lblDefaultSinger.Location = new System.Drawing.Point( 29, 27 );
            this.lblDefaultSinger.Name = "lblDefaultSinger";
            this.lblDefaultSinger.Size = new System.Drawing.Size( 93, 12 );
            this.lblDefaultSinger.TabIndex = 0;
            this.lblDefaultSinger.Text = "Default Singer(&S)";
            // 
            // numPreSendTimeSample
            // 
            this.numPreSendTimeSample.Enabled = false;
            this.numPreSendTimeSample.Location = new System.Drawing.Point( 289, 290 );
            this.numPreSendTimeSample.Maximum = new decimal( new int[] {
            500,
            0,
            0,
            0} );
            this.numPreSendTimeSample.Minimum = new decimal( new int[] {
            50,
            0,
            0,
            0} );
            this.numPreSendTimeSample.Name = "numPreSendTimeSample";
            this.numPreSendTimeSample.Size = new System.Drawing.Size( 68, 19 );
            this.numPreSendTimeSample.TabIndex = 27;
            this.numPreSendTimeSample.Value = new decimal( new int[] {
            50,
            0,
            0,
            0} );
            // 
            // numTiming
            // 
            this.numTiming.Enabled = false;
            this.numTiming.Location = new System.Drawing.Point( 216, 259 );
            this.numTiming.Maximum = new decimal( new int[] {
            1500,
            0,
            0,
            0} );
            this.numTiming.Minimum = new decimal( new int[] {
            500,
            0,
            0,
            0} );
            this.numTiming.Name = "numTiming";
            this.numTiming.Size = new System.Drawing.Size( 68, 19 );
            this.numTiming.TabIndex = 26;
            this.numTiming.Value = new decimal( new int[] {
            500,
            0,
            0,
            0} );
            // 
            // numWait
            // 
            this.numWait.Enabled = false;
            this.numWait.Location = new System.Drawing.Point( 216, 87 );
            this.numWait.Maximum = new decimal( new int[] {
            2000,
            0,
            0,
            0} );
            this.numWait.Minimum = new decimal( new int[] {
            200,
            0,
            0,
            0} );
            this.numWait.Name = "numWait";
            this.numWait.Size = new System.Drawing.Size( 68, 19 );
            this.numWait.TabIndex = 22;
            this.numWait.Value = new decimal( new int[] {
            300,
            0,
            0,
            0} );
            // 
            // numPreSendTime
            // 
            this.numPreSendTime.Location = new System.Drawing.Point( 216, 56 );
            this.numPreSendTime.Maximum = new decimal( new int[] {
            5000,
            0,
            0,
            0} );
            this.numPreSendTime.Minimum = new decimal( new int[] {
            500,
            0,
            0,
            0} );
            this.numPreSendTime.Name = "numPreSendTime";
            this.numPreSendTime.Size = new System.Drawing.Size( 68, 19 );
            this.numPreSendTime.TabIndex = 21;
            this.numPreSendTime.Value = new decimal( new int[] {
            500,
            0,
            0,
            0} );
            // 
            // tabAppearance
            // 
            this.tabAppearance.Controls.Add( this.groupFont );
            this.tabAppearance.Controls.Add( this.groupVisibleCurve );
            this.tabAppearance.Controls.Add( this.lblTrackHeight );
            this.tabAppearance.Controls.Add( this.comboLanguage );
            this.tabAppearance.Controls.Add( this.lblLanguage );
            this.tabAppearance.Controls.Add( this.numTrackHeight );
            this.tabAppearance.Location = new System.Drawing.Point( 4, 38 );
            this.tabAppearance.Name = "tabAppearance";
            this.tabAppearance.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabAppearance.Size = new System.Drawing.Size( 454, 351 );
            this.tabAppearance.TabIndex = 3;
            this.tabAppearance.Text = "Appearance";
            this.tabAppearance.UseVisualStyleBackColor = true;
            // 
            // groupFont
            // 
            this.groupFont.Controls.Add( this.labelMenu );
            this.groupFont.Controls.Add( this.labelScreenFontName );
            this.groupFont.Controls.Add( this.btnChangeScreenFont );
            this.groupFont.Controls.Add( this.labelScreen );
            this.groupFont.Controls.Add( this.labelMenuFontName );
            this.groupFont.Controls.Add( this.btnChangeMenuFont );
            this.groupFont.Location = new System.Drawing.Point( 23, 9 );
            this.groupFont.Name = "groupFont";
            this.groupFont.Size = new System.Drawing.Size( 407, 78 );
            this.groupFont.TabIndex = 40;
            this.groupFont.TabStop = false;
            this.groupFont.Text = "Font";
            // 
            // labelMenu
            // 
            this.labelMenu.AutoSize = true;
            this.labelMenu.Location = new System.Drawing.Point( 15, 22 );
            this.labelMenu.Name = "labelMenu";
            this.labelMenu.Size = new System.Drawing.Size( 77, 12 );
            this.labelMenu.TabIndex = 0;
            this.labelMenu.Text = "Menu / Lyrics";
            // 
            // labelScreenFontName
            // 
            this.labelScreenFontName.AutoSize = true;
            this.labelScreenFontName.Location = new System.Drawing.Point( 111, 49 );
            this.labelScreenFontName.Name = "labelScreenFontName";
            this.labelScreenFontName.Size = new System.Drawing.Size( 35, 12 );
            this.labelScreenFontName.TabIndex = 4;
            this.labelScreenFontName.Text = "label1";
            // 
            // btnChangeScreenFont
            // 
            this.btnChangeScreenFont.Location = new System.Drawing.Point( 270, 44 );
            this.btnChangeScreenFont.Name = "btnChangeScreenFont";
            this.btnChangeScreenFont.Size = new System.Drawing.Size( 75, 23 );
            this.btnChangeScreenFont.TabIndex = 42;
            this.btnChangeScreenFont.Text = "Change";
            this.btnChangeScreenFont.UseVisualStyleBackColor = true;
            // 
            // labelScreen
            // 
            this.labelScreen.AutoSize = true;
            this.labelScreen.Location = new System.Drawing.Point( 15, 49 );
            this.labelScreen.Name = "labelScreen";
            this.labelScreen.Size = new System.Drawing.Size( 40, 12 );
            this.labelScreen.TabIndex = 3;
            this.labelScreen.Text = "Screen";
            // 
            // labelMenuFontName
            // 
            this.labelMenuFontName.AutoSize = true;
            this.labelMenuFontName.Location = new System.Drawing.Point( 111, 22 );
            this.labelMenuFontName.Name = "labelMenuFontName";
            this.labelMenuFontName.Size = new System.Drawing.Size( 35, 12 );
            this.labelMenuFontName.TabIndex = 2;
            this.labelMenuFontName.Text = "label1";
            // 
            // btnChangeMenuFont
            // 
            this.btnChangeMenuFont.Location = new System.Drawing.Point( 270, 17 );
            this.btnChangeMenuFont.Name = "btnChangeMenuFont";
            this.btnChangeMenuFont.Size = new System.Drawing.Size( 75, 23 );
            this.btnChangeMenuFont.TabIndex = 41;
            this.btnChangeMenuFont.Text = "Change";
            this.btnChangeMenuFont.UseVisualStyleBackColor = true;
            // 
            // groupVisibleCurve
            // 
            this.groupVisibleCurve.Controls.Add( this.chkEnvelope );
            this.groupVisibleCurve.Controls.Add( this.chkPbs );
            this.groupVisibleCurve.Controls.Add( this.chkReso4 );
            this.groupVisibleCurve.Controls.Add( this.chkReso3 );
            this.groupVisibleCurve.Controls.Add( this.chkReso2 );
            this.groupVisibleCurve.Controls.Add( this.chkReso1 );
            this.groupVisibleCurve.Controls.Add( this.chkFx2Depth );
            this.groupVisibleCurve.Controls.Add( this.chkHarmonics );
            this.groupVisibleCurve.Controls.Add( this.chkPit );
            this.groupVisibleCurve.Controls.Add( this.chkPor );
            this.groupVisibleCurve.Controls.Add( this.chkGen );
            this.groupVisibleCurve.Controls.Add( this.chkOpe );
            this.groupVisibleCurve.Controls.Add( this.chkCle );
            this.groupVisibleCurve.Controls.Add( this.chkBri );
            this.groupVisibleCurve.Controls.Add( this.chkBre );
            this.groupVisibleCurve.Controls.Add( this.chkDyn );
            this.groupVisibleCurve.Controls.Add( this.chkVel );
            this.groupVisibleCurve.Controls.Add( this.chkVibratoDepth );
            this.groupVisibleCurve.Controls.Add( this.chkVibratoRate );
            this.groupVisibleCurve.Controls.Add( this.chkDecay );
            this.groupVisibleCurve.Controls.Add( this.chkAccent );
            this.groupVisibleCurve.Location = new System.Drawing.Point( 23, 153 );
            this.groupVisibleCurve.Name = "groupVisibleCurve";
            this.groupVisibleCurve.Size = new System.Drawing.Size( 407, 169 );
            this.groupVisibleCurve.TabIndex = 46;
            this.groupVisibleCurve.TabStop = false;
            this.groupVisibleCurve.Text = "Visible Control Curve";
            // 
            // chkEnvelope
            // 
            this.chkEnvelope.AutoSize = true;
            this.chkEnvelope.Location = new System.Drawing.Point( 17, 140 );
            this.chkEnvelope.Name = "chkEnvelope";
            this.chkEnvelope.Size = new System.Drawing.Size( 70, 16 );
            this.chkEnvelope.TabIndex = 67;
            this.chkEnvelope.Text = "Envelope";
            this.chkEnvelope.UseVisualStyleBackColor = true;
            // 
            // chkPbs
            // 
            this.chkPbs.AutoSize = true;
            this.chkPbs.Location = new System.Drawing.Point( 114, 94 );
            this.chkPbs.Name = "chkPbs";
            this.chkPbs.Size = new System.Drawing.Size( 46, 16 );
            this.chkPbs.TabIndex = 60;
            this.chkPbs.Text = "PBS";
            this.chkPbs.UseVisualStyleBackColor = true;
            // 
            // chkReso4
            // 
            this.chkReso4.AutoSize = true;
            this.chkReso4.Location = new System.Drawing.Point( 308, 118 );
            this.chkReso4.Name = "chkReso4";
            this.chkReso4.Size = new System.Drawing.Size( 60, 16 );
            this.chkReso4.TabIndex = 66;
            this.chkReso4.Text = "Reso 4";
            this.chkReso4.UseVisualStyleBackColor = true;
            // 
            // chkReso3
            // 
            this.chkReso3.AutoSize = true;
            this.chkReso3.Location = new System.Drawing.Point( 211, 118 );
            this.chkReso3.Name = "chkReso3";
            this.chkReso3.Size = new System.Drawing.Size( 60, 16 );
            this.chkReso3.TabIndex = 65;
            this.chkReso3.Text = "Reso 3";
            this.chkReso3.UseVisualStyleBackColor = true;
            // 
            // chkReso2
            // 
            this.chkReso2.AutoSize = true;
            this.chkReso2.Location = new System.Drawing.Point( 113, 118 );
            this.chkReso2.Name = "chkReso2";
            this.chkReso2.Size = new System.Drawing.Size( 60, 16 );
            this.chkReso2.TabIndex = 64;
            this.chkReso2.Text = "Reso 2";
            this.chkReso2.UseVisualStyleBackColor = true;
            // 
            // chkReso1
            // 
            this.chkReso1.AutoSize = true;
            this.chkReso1.Location = new System.Drawing.Point( 17, 118 );
            this.chkReso1.Name = "chkReso1";
            this.chkReso1.Size = new System.Drawing.Size( 60, 16 );
            this.chkReso1.TabIndex = 63;
            this.chkReso1.Text = "Reso 1";
            this.chkReso1.UseVisualStyleBackColor = true;
            // 
            // chkFx2Depth
            // 
            this.chkFx2Depth.AutoSize = true;
            this.chkFx2Depth.Location = new System.Drawing.Point( 308, 94 );
            this.chkFx2Depth.Name = "chkFx2Depth";
            this.chkFx2Depth.Size = new System.Drawing.Size( 74, 16 );
            this.chkFx2Depth.TabIndex = 62;
            this.chkFx2Depth.Text = "FX2Depth";
            this.chkFx2Depth.UseVisualStyleBackColor = true;
            // 
            // chkHarmonics
            // 
            this.chkHarmonics.AutoSize = true;
            this.chkHarmonics.Location = new System.Drawing.Point( 211, 94 );
            this.chkHarmonics.Name = "chkHarmonics";
            this.chkHarmonics.Size = new System.Drawing.Size( 78, 16 );
            this.chkHarmonics.TabIndex = 61;
            this.chkHarmonics.Text = "Harmonics";
            this.chkHarmonics.UseVisualStyleBackColor = true;
            // 
            // chkPit
            // 
            this.chkPit.AutoSize = true;
            this.chkPit.Location = new System.Drawing.Point( 17, 94 );
            this.chkPit.Name = "chkPit";
            this.chkPit.Size = new System.Drawing.Size( 41, 16 );
            this.chkPit.TabIndex = 59;
            this.chkPit.Text = "PIT";
            this.chkPit.UseVisualStyleBackColor = true;
            // 
            // chkPor
            // 
            this.chkPor.AutoSize = true;
            this.chkPor.Location = new System.Drawing.Point( 308, 70 );
            this.chkPor.Name = "chkPor";
            this.chkPor.Size = new System.Drawing.Size( 47, 16 );
            this.chkPor.TabIndex = 58;
            this.chkPor.Text = "POR";
            this.chkPor.UseVisualStyleBackColor = true;
            // 
            // chkGen
            // 
            this.chkGen.AutoSize = true;
            this.chkGen.Location = new System.Drawing.Point( 211, 70 );
            this.chkGen.Name = "chkGen";
            this.chkGen.Size = new System.Drawing.Size( 47, 16 );
            this.chkGen.TabIndex = 57;
            this.chkGen.Text = "GEN";
            this.chkGen.UseVisualStyleBackColor = true;
            // 
            // chkOpe
            // 
            this.chkOpe.AutoSize = true;
            this.chkOpe.Location = new System.Drawing.Point( 114, 70 );
            this.chkOpe.Name = "chkOpe";
            this.chkOpe.Size = new System.Drawing.Size( 46, 16 );
            this.chkOpe.TabIndex = 56;
            this.chkOpe.Text = "OPE";
            this.chkOpe.UseVisualStyleBackColor = true;
            // 
            // chkCle
            // 
            this.chkCle.AutoSize = true;
            this.chkCle.Location = new System.Drawing.Point( 17, 70 );
            this.chkCle.Name = "chkCle";
            this.chkCle.Size = new System.Drawing.Size( 45, 16 );
            this.chkCle.TabIndex = 55;
            this.chkCle.Text = "CLE";
            this.chkCle.UseVisualStyleBackColor = true;
            // 
            // chkBri
            // 
            this.chkBri.AutoSize = true;
            this.chkBri.Location = new System.Drawing.Point( 308, 46 );
            this.chkBri.Name = "chkBri";
            this.chkBri.Size = new System.Drawing.Size( 43, 16 );
            this.chkBri.TabIndex = 54;
            this.chkBri.Text = "BRI";
            this.chkBri.UseVisualStyleBackColor = true;
            // 
            // chkBre
            // 
            this.chkBre.AutoSize = true;
            this.chkBre.Location = new System.Drawing.Point( 211, 46 );
            this.chkBre.Name = "chkBre";
            this.chkBre.Size = new System.Drawing.Size( 47, 16 );
            this.chkBre.TabIndex = 53;
            this.chkBre.Text = "BRE";
            this.chkBre.UseVisualStyleBackColor = true;
            // 
            // chkDyn
            // 
            this.chkDyn.AutoSize = true;
            this.chkDyn.Location = new System.Drawing.Point( 114, 46 );
            this.chkDyn.Name = "chkDyn";
            this.chkDyn.Size = new System.Drawing.Size( 47, 16 );
            this.chkDyn.TabIndex = 52;
            this.chkDyn.Text = "DYN";
            this.chkDyn.UseVisualStyleBackColor = true;
            // 
            // chkVel
            // 
            this.chkVel.AutoSize = true;
            this.chkVel.Location = new System.Drawing.Point( 17, 46 );
            this.chkVel.Name = "chkVel";
            this.chkVel.Size = new System.Drawing.Size( 45, 16 );
            this.chkVel.TabIndex = 51;
            this.chkVel.Text = "VEL";
            this.chkVel.UseVisualStyleBackColor = true;
            // 
            // chkVibratoDepth
            // 
            this.chkVibratoDepth.AutoSize = true;
            this.chkVibratoDepth.Location = new System.Drawing.Point( 308, 22 );
            this.chkVibratoDepth.Name = "chkVibratoDepth";
            this.chkVibratoDepth.Size = new System.Drawing.Size( 91, 16 );
            this.chkVibratoDepth.TabIndex = 50;
            this.chkVibratoDepth.Text = "VibratoDepth";
            this.chkVibratoDepth.UseVisualStyleBackColor = true;
            // 
            // chkVibratoRate
            // 
            this.chkVibratoRate.AutoSize = true;
            this.chkVibratoRate.Location = new System.Drawing.Point( 211, 22 );
            this.chkVibratoRate.Name = "chkVibratoRate";
            this.chkVibratoRate.Size = new System.Drawing.Size( 85, 16 );
            this.chkVibratoRate.TabIndex = 49;
            this.chkVibratoRate.Text = "VibratoRate";
            this.chkVibratoRate.UseVisualStyleBackColor = true;
            // 
            // chkDecay
            // 
            this.chkDecay.AutoSize = true;
            this.chkDecay.Location = new System.Drawing.Point( 114, 22 );
            this.chkDecay.Name = "chkDecay";
            this.chkDecay.Size = new System.Drawing.Size( 56, 16 );
            this.chkDecay.TabIndex = 48;
            this.chkDecay.Text = "Decay";
            this.chkDecay.UseVisualStyleBackColor = true;
            // 
            // chkAccent
            // 
            this.chkAccent.AutoSize = true;
            this.chkAccent.Location = new System.Drawing.Point( 17, 22 );
            this.chkAccent.Name = "chkAccent";
            this.chkAccent.Size = new System.Drawing.Size( 60, 16 );
            this.chkAccent.TabIndex = 47;
            this.chkAccent.Text = "Accent";
            this.chkAccent.UseVisualStyleBackColor = true;
            // 
            // lblTrackHeight
            // 
            this.lblTrackHeight.AutoSize = true;
            this.lblTrackHeight.Location = new System.Drawing.Point( 31, 130 );
            this.lblTrackHeight.Name = "lblTrackHeight";
            this.lblTrackHeight.Size = new System.Drawing.Size( 107, 12 );
            this.lblTrackHeight.TabIndex = 13;
            this.lblTrackHeight.Text = "Track Height (pixel)";
            // 
            // comboLanguage
            // 
            this.comboLanguage.FormattingEnabled = true;
            this.comboLanguage.Location = new System.Drawing.Point( 148, 99 );
            this.comboLanguage.Name = "comboLanguage";
            this.comboLanguage.Size = new System.Drawing.Size( 121, 20 );
            this.comboLanguage.TabIndex = 44;
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point( 31, 102 );
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size( 68, 12 );
            this.lblLanguage.TabIndex = 9;
            this.lblLanguage.Text = "UI Language";
            // 
            // numTrackHeight
            // 
            this.numTrackHeight.Location = new System.Drawing.Point( 210, 128 );
            this.numTrackHeight.Maximum = new decimal( new int[] {
            50,
            0,
            0,
            0} );
            this.numTrackHeight.Minimum = new decimal( new int[] {
            5,
            0,
            0,
            0} );
            this.numTrackHeight.Name = "numTrackHeight";
            this.numTrackHeight.Size = new System.Drawing.Size( 120, 19 );
            this.numTrackHeight.TabIndex = 45;
            this.numTrackHeight.Value = new decimal( new int[] {
            14,
            0,
            0,
            0} );
            // 
            // tabOperation
            // 
            this.tabOperation.Controls.Add( this.groupMisc );
            this.tabOperation.Controls.Add( this.groupPianoroll );
            this.tabOperation.Location = new System.Drawing.Point( 4, 38 );
            this.tabOperation.Name = "tabOperation";
            this.tabOperation.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabOperation.Size = new System.Drawing.Size( 454, 351 );
            this.tabOperation.TabIndex = 5;
            this.tabOperation.Text = "Operation";
            this.tabOperation.UseVisualStyleBackColor = true;
            // 
            // groupMisc
            // 
            this.groupMisc.Controls.Add( this.lblMaximumFrameRate );
            this.groupMisc.Controls.Add( this.comboMidiInPortNumber );
            this.groupMisc.Controls.Add( this.numMaximumFrameRate );
            this.groupMisc.Controls.Add( this.lblMidiInPort );
            this.groupMisc.Controls.Add( this.lblMouseHoverTime );
            this.groupMisc.Controls.Add( this.lblMilliSecond );
            this.groupMisc.Controls.Add( this.numMouseHoverTime );
            this.groupMisc.Location = new System.Drawing.Point( 6, 227 );
            this.groupMisc.Name = "groupMisc";
            this.groupMisc.Size = new System.Drawing.Size( 442, 109 );
            this.groupMisc.TabIndex = 91;
            this.groupMisc.TabStop = false;
            this.groupMisc.Text = "Misc";
            // 
            // lblMaximumFrameRate
            // 
            this.lblMaximumFrameRate.AutoSize = true;
            this.lblMaximumFrameRate.Location = new System.Drawing.Point( 16, 23 );
            this.lblMaximumFrameRate.Name = "lblMaximumFrameRate";
            this.lblMaximumFrameRate.Size = new System.Drawing.Size( 117, 12 );
            this.lblMaximumFrameRate.TabIndex = 5;
            this.lblMaximumFrameRate.Text = "Maximum Frame Rate";
            // 
            // comboMidiInPortNumber
            // 
            this.comboMidiInPortNumber.FormattingEnabled = true;
            this.comboMidiInPortNumber.Location = new System.Drawing.Point( 176, 74 );
            this.comboMidiInPortNumber.Name = "comboMidiInPortNumber";
            this.comboMidiInPortNumber.Size = new System.Drawing.Size( 239, 20 );
            this.comboMidiInPortNumber.TabIndex = 89;
            // 
            // numMaximumFrameRate
            // 
            this.numMaximumFrameRate.Location = new System.Drawing.Point( 176, 21 );
            this.numMaximumFrameRate.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.numMaximumFrameRate.Minimum = new decimal( new int[] {
            5,
            0,
            0,
            0} );
            this.numMaximumFrameRate.Name = "numMaximumFrameRate";
            this.numMaximumFrameRate.Size = new System.Drawing.Size( 120, 19 );
            this.numMaximumFrameRate.TabIndex = 84;
            this.numMaximumFrameRate.Value = new decimal( new int[] {
            30,
            0,
            0,
            0} );
            // 
            // lblMidiInPort
            // 
            this.lblMidiInPort.AutoSize = true;
            this.lblMidiInPort.Location = new System.Drawing.Point( 16, 77 );
            this.lblMidiInPort.Name = "lblMidiInPort";
            this.lblMidiInPort.Size = new System.Drawing.Size( 109, 12 );
            this.lblMidiInPort.TabIndex = 13;
            this.lblMidiInPort.Text = "MIDI In Port Number";
            // 
            // lblMouseHoverTime
            // 
            this.lblMouseHoverTime.AutoSize = true;
            this.lblMouseHoverTime.Location = new System.Drawing.Point( 16, 50 );
            this.lblMouseHoverTime.Name = "lblMouseHoverTime";
            this.lblMouseHoverTime.Size = new System.Drawing.Size( 133, 12 );
            this.lblMouseHoverTime.TabIndex = 8;
            this.lblMouseHoverTime.Text = "Waiting Time for Preview";
            // 
            // lblMilliSecond
            // 
            this.lblMilliSecond.AutoSize = true;
            this.lblMilliSecond.Location = new System.Drawing.Point( 302, 23 );
            this.lblMilliSecond.Name = "lblMilliSecond";
            this.lblMilliSecond.Size = new System.Drawing.Size( 66, 12 );
            this.lblMilliSecond.TabIndex = 10;
            this.lblMilliSecond.Text = "milli second";
            // 
            // numMouseHoverTime
            // 
            this.numMouseHoverTime.Location = new System.Drawing.Point( 176, 48 );
            this.numMouseHoverTime.Maximum = new decimal( new int[] {
            2000,
            0,
            0,
            0} );
            this.numMouseHoverTime.Name = "numMouseHoverTime";
            this.numMouseHoverTime.Size = new System.Drawing.Size( 120, 19 );
            this.numMouseHoverTime.TabIndex = 86;
            this.numMouseHoverTime.Value = new decimal( new int[] {
            500,
            0,
            0,
            0} );
            // 
            // groupPianoroll
            // 
            this.groupPianoroll.Controls.Add( this.chkUseSpaceKeyAsMiddleButtonModifier );
            this.groupPianoroll.Controls.Add( this.labelWheelOrder );
            this.groupPianoroll.Controls.Add( this.numericUpDownEx1 );
            this.groupPianoroll.Controls.Add( this.chkCursorFix );
            this.groupPianoroll.Controls.Add( this.chkCurveSelectingQuantized );
            this.groupPianoroll.Controls.Add( this.chkScrollHorizontal );
            this.groupPianoroll.Controls.Add( this.chkPlayPreviewWhenRightClick );
            this.groupPianoroll.Controls.Add( this.chkKeepLyricInputMode );
            this.groupPianoroll.Location = new System.Drawing.Point( 6, 6 );
            this.groupPianoroll.Name = "groupPianoroll";
            this.groupPianoroll.Size = new System.Drawing.Size( 442, 215 );
            this.groupPianoroll.TabIndex = 90;
            this.groupPianoroll.TabStop = false;
            this.groupPianoroll.Text = "Piano Roll";
            // 
            // chkUseSpaceKeyAsMiddleButtonModifier
            // 
            this.chkUseSpaceKeyAsMiddleButtonModifier.AutoSize = true;
            this.chkUseSpaceKeyAsMiddleButtonModifier.Location = new System.Drawing.Point( 16, 181 );
            this.chkUseSpaceKeyAsMiddleButtonModifier.Name = "chkUseSpaceKeyAsMiddleButtonModifier";
            this.chkUseSpaceKeyAsMiddleButtonModifier.Size = new System.Drawing.Size( 234, 16 );
            this.chkUseSpaceKeyAsMiddleButtonModifier.TabIndex = 89;
            this.chkUseSpaceKeyAsMiddleButtonModifier.Text = "Use space key as Middle button modifier";
            this.chkUseSpaceKeyAsMiddleButtonModifier.UseVisualStyleBackColor = true;
            // 
            // labelWheelOrder
            // 
            this.labelWheelOrder.AutoSize = true;
            this.labelWheelOrder.Location = new System.Drawing.Point( 16, 23 );
            this.labelWheelOrder.Name = "labelWheelOrder";
            this.labelWheelOrder.Size = new System.Drawing.Size( 99, 12 );
            this.labelWheelOrder.TabIndex = 1;
            this.labelWheelOrder.Text = "Mouse wheel Rate";
            // 
            // numericUpDownEx1
            // 
            this.numericUpDownEx1.Location = new System.Drawing.Point( 199, 21 );
            this.numericUpDownEx1.Maximum = new decimal( new int[] {
            50,
            0,
            0,
            0} );
            this.numericUpDownEx1.Minimum = new decimal( new int[] {
            5,
            0,
            0,
            0} );
            this.numericUpDownEx1.Name = "numericUpDownEx1";
            this.numericUpDownEx1.Size = new System.Drawing.Size( 120, 19 );
            this.numericUpDownEx1.TabIndex = 80;
            this.numericUpDownEx1.Value = new decimal( new int[] {
            20,
            0,
            0,
            0} );
            // 
            // chkCursorFix
            // 
            this.chkCursorFix.AutoSize = true;
            this.chkCursorFix.Location = new System.Drawing.Point( 16, 46 );
            this.chkCursorFix.Name = "chkCursorFix";
            this.chkCursorFix.Size = new System.Drawing.Size( 156, 16 );
            this.chkCursorFix.TabIndex = 81;
            this.chkCursorFix.Text = "Fix Play Cursor to Center";
            this.chkCursorFix.UseVisualStyleBackColor = true;
            // 
            // chkCurveSelectingQuantized
            // 
            this.chkCurveSelectingQuantized.AutoSize = true;
            this.chkCurveSelectingQuantized.Location = new System.Drawing.Point( 16, 154 );
            this.chkCurveSelectingQuantized.Name = "chkCurveSelectingQuantized";
            this.chkCurveSelectingQuantized.Size = new System.Drawing.Size( 209, 16 );
            this.chkCurveSelectingQuantized.TabIndex = 88;
            this.chkCurveSelectingQuantized.Text = "Enable Quantize for Curve Selecting";
            this.chkCurveSelectingQuantized.UseVisualStyleBackColor = true;
            // 
            // chkScrollHorizontal
            // 
            this.chkScrollHorizontal.AutoSize = true;
            this.chkScrollHorizontal.Location = new System.Drawing.Point( 16, 73 );
            this.chkScrollHorizontal.Name = "chkScrollHorizontal";
            this.chkScrollHorizontal.Size = new System.Drawing.Size( 208, 16 );
            this.chkScrollHorizontal.TabIndex = 82;
            this.chkScrollHorizontal.Text = "Horizontal Scroll when Mouse wheel";
            this.chkScrollHorizontal.UseVisualStyleBackColor = true;
            // 
            // chkPlayPreviewWhenRightClick
            // 
            this.chkPlayPreviewWhenRightClick.AutoSize = true;
            this.chkPlayPreviewWhenRightClick.Location = new System.Drawing.Point( 16, 127 );
            this.chkPlayPreviewWhenRightClick.Name = "chkPlayPreviewWhenRightClick";
            this.chkPlayPreviewWhenRightClick.Size = new System.Drawing.Size( 169, 16 );
            this.chkPlayPreviewWhenRightClick.TabIndex = 87;
            this.chkPlayPreviewWhenRightClick.Text = "Play Preview On Right Click";
            this.chkPlayPreviewWhenRightClick.UseVisualStyleBackColor = true;
            // 
            // chkKeepLyricInputMode
            // 
            this.chkKeepLyricInputMode.AutoSize = true;
            this.chkKeepLyricInputMode.Location = new System.Drawing.Point( 16, 100 );
            this.chkKeepLyricInputMode.Name = "chkKeepLyricInputMode";
            this.chkKeepLyricInputMode.Size = new System.Drawing.Size( 138, 16 );
            this.chkKeepLyricInputMode.TabIndex = 85;
            this.chkKeepLyricInputMode.Text = "Keep Lyric Input Mode";
            this.chkKeepLyricInputMode.UseVisualStyleBackColor = true;
            // 
            // tabPlatform
            // 
            this.tabPlatform.Controls.Add( this.groupUtauCores );
            this.tabPlatform.Controls.Add( this.groupVsti );
            this.tabPlatform.Controls.Add( this.groupPlatform );
            this.tabPlatform.Location = new System.Drawing.Point( 4, 38 );
            this.tabPlatform.Name = "tabPlatform";
            this.tabPlatform.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabPlatform.Size = new System.Drawing.Size( 454, 351 );
            this.tabPlatform.TabIndex = 4;
            this.tabPlatform.Text = "Platform";
            this.tabPlatform.UseVisualStyleBackColor = true;
            // 
            // groupUtauCores
            // 
            this.groupUtauCores.Controls.Add( this.lblResampler );
            this.groupUtauCores.Controls.Add( this.chkInvokeWithWine );
            this.groupUtauCores.Controls.Add( this.btnWavtool );
            this.groupUtauCores.Controls.Add( this.txtResampler );
            this.groupUtauCores.Controls.Add( this.lblWavtool );
            this.groupUtauCores.Controls.Add( this.btnResampler );
            this.groupUtauCores.Controls.Add( this.txtWavtool );
            this.groupUtauCores.Location = new System.Drawing.Point( 6, 207 );
            this.groupUtauCores.Name = "groupUtauCores";
            this.groupUtauCores.Size = new System.Drawing.Size( 442, 114 );
            this.groupUtauCores.TabIndex = 108;
            this.groupUtauCores.TabStop = false;
            this.groupUtauCores.Text = "UTAU Cores";
            // 
            // lblResampler
            // 
            this.lblResampler.AutoSize = true;
            this.lblResampler.Location = new System.Drawing.Point( 17, 25 );
            this.lblResampler.Name = "lblResampler";
            this.lblResampler.Size = new System.Drawing.Size( 55, 12 );
            this.lblResampler.TabIndex = 111;
            this.lblResampler.Text = "resampler";
            // 
            // chkInvokeWithWine
            // 
            this.chkInvokeWithWine.AutoSize = true;
            this.chkInvokeWithWine.Location = new System.Drawing.Point( 19, 79 );
            this.chkInvokeWithWine.Name = "chkInvokeWithWine";
            this.chkInvokeWithWine.Size = new System.Drawing.Size( 177, 16 );
            this.chkInvokeWithWine.TabIndex = 113;
            this.chkInvokeWithWine.Text = "Invoke UTAU cores with Wine";
            this.chkInvokeWithWine.UseVisualStyleBackColor = true;
            // 
            // btnWavtool
            // 
            this.btnWavtool.Location = new System.Drawing.Point( 386, 47 );
            this.btnWavtool.Name = "btnWavtool";
            this.btnWavtool.Size = new System.Drawing.Size( 41, 23 );
            this.btnWavtool.TabIndex = 112;
            this.btnWavtool.Text = "...";
            this.btnWavtool.UseVisualStyleBackColor = true;
            // 
            // txtResampler
            // 
            this.txtResampler.Location = new System.Drawing.Point( 100, 22 );
            this.txtResampler.Name = "txtResampler";
            this.txtResampler.Size = new System.Drawing.Size( 280, 19 );
            this.txtResampler.TabIndex = 109;
            // 
            // lblWavtool
            // 
            this.lblWavtool.AutoSize = true;
            this.lblWavtool.Location = new System.Drawing.Point( 17, 52 );
            this.lblWavtool.Name = "lblWavtool";
            this.lblWavtool.Size = new System.Drawing.Size( 44, 12 );
            this.lblWavtool.TabIndex = 114;
            this.lblWavtool.Text = "wavtool";
            // 
            // btnResampler
            // 
            this.btnResampler.Location = new System.Drawing.Point( 386, 20 );
            this.btnResampler.Name = "btnResampler";
            this.btnResampler.Size = new System.Drawing.Size( 41, 23 );
            this.btnResampler.TabIndex = 110;
            this.btnResampler.Text = "...";
            this.btnResampler.UseVisualStyleBackColor = true;
            // 
            // txtWavtool
            // 
            this.txtWavtool.Location = new System.Drawing.Point( 100, 49 );
            this.txtWavtool.Name = "txtWavtool";
            this.txtWavtool.Size = new System.Drawing.Size( 280, 19 );
            this.txtWavtool.TabIndex = 111;
            // 
            // groupVsti
            // 
            this.groupVsti.Controls.Add( this.txtVOCALOID2 );
            this.groupVsti.Controls.Add( this.txtVOCALOID1 );
            this.groupVsti.Controls.Add( this.lblVOCALOID2 );
            this.groupVsti.Controls.Add( this.lblVOCALOID1 );
            this.groupVsti.Location = new System.Drawing.Point( 6, 120 );
            this.groupVsti.Name = "groupVsti";
            this.groupVsti.Size = new System.Drawing.Size( 442, 81 );
            this.groupVsti.TabIndex = 105;
            this.groupVsti.TabStop = false;
            this.groupVsti.Text = "VST Instruments";
            // 
            // txtVOCALOID2
            // 
            this.txtVOCALOID2.Location = new System.Drawing.Point( 99, 46 );
            this.txtVOCALOID2.Name = "txtVOCALOID2";
            this.txtVOCALOID2.ReadOnly = true;
            this.txtVOCALOID2.Size = new System.Drawing.Size( 290, 19 );
            this.txtVOCALOID2.TabIndex = 107;
            // 
            // txtVOCALOID1
            // 
            this.txtVOCALOID1.Location = new System.Drawing.Point( 99, 21 );
            this.txtVOCALOID1.Name = "txtVOCALOID1";
            this.txtVOCALOID1.ReadOnly = true;
            this.txtVOCALOID1.Size = new System.Drawing.Size( 290, 19 );
            this.txtVOCALOID1.TabIndex = 106;
            // 
            // lblVOCALOID2
            // 
            this.lblVOCALOID2.AutoSize = true;
            this.lblVOCALOID2.Location = new System.Drawing.Point( 16, 49 );
            this.lblVOCALOID2.Name = "lblVOCALOID2";
            this.lblVOCALOID2.Size = new System.Drawing.Size( 68, 12 );
            this.lblVOCALOID2.TabIndex = 1;
            this.lblVOCALOID2.Text = "VOCALOID2";
            // 
            // lblVOCALOID1
            // 
            this.lblVOCALOID1.AutoSize = true;
            this.lblVOCALOID1.Location = new System.Drawing.Point( 16, 24 );
            this.lblVOCALOID1.Name = "lblVOCALOID1";
            this.lblVOCALOID1.Size = new System.Drawing.Size( 68, 12 );
            this.lblVOCALOID1.TabIndex = 0;
            this.lblVOCALOID1.Text = "VOCALOID1";
            // 
            // groupPlatform
            // 
            this.groupPlatform.Controls.Add( this.chkTranslateRoman );
            this.groupPlatform.Controls.Add( this.comboPlatform );
            this.groupPlatform.Controls.Add( this.lblPlatform );
            this.groupPlatform.Controls.Add( this.chkCommandKeyAsControl );
            this.groupPlatform.Location = new System.Drawing.Point( 6, 6 );
            this.groupPlatform.Name = "groupPlatform";
            this.groupPlatform.Size = new System.Drawing.Size( 442, 108 );
            this.groupPlatform.TabIndex = 100;
            this.groupPlatform.TabStop = false;
            this.groupPlatform.Text = "Platform";
            // 
            // chkTranslateRoman
            // 
            this.chkTranslateRoman.AutoSize = true;
            this.chkTranslateRoman.Location = new System.Drawing.Point( 18, 76 );
            this.chkTranslateRoman.Name = "chkTranslateRoman";
            this.chkTranslateRoman.Size = new System.Drawing.Size( 200, 16 );
            this.chkTranslateRoman.TabIndex = 104;
            this.chkTranslateRoman.Text = "Translate Roman letters into Kana";
            this.chkTranslateRoman.UseVisualStyleBackColor = true;
            // 
            // comboPlatform
            // 
            this.comboPlatform.FormattingEnabled = true;
            this.comboPlatform.Location = new System.Drawing.Point( 156, 19 );
            this.comboPlatform.Name = "comboPlatform";
            this.comboPlatform.Size = new System.Drawing.Size( 121, 20 );
            this.comboPlatform.TabIndex = 101;
            // 
            // lblPlatform
            // 
            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Location = new System.Drawing.Point( 16, 22 );
            this.lblPlatform.Name = "lblPlatform";
            this.lblPlatform.Size = new System.Drawing.Size( 90, 12 );
            this.lblPlatform.TabIndex = 1;
            this.lblPlatform.Text = "Current Platform";
            // 
            // chkCommandKeyAsControl
            // 
            this.chkCommandKeyAsControl.AutoSize = true;
            this.chkCommandKeyAsControl.Enabled = false;
            this.chkCommandKeyAsControl.Location = new System.Drawing.Point( 18, 49 );
            this.chkCommandKeyAsControl.Name = "chkCommandKeyAsControl";
            this.chkCommandKeyAsControl.Size = new System.Drawing.Size( 199, 16 );
            this.chkCommandKeyAsControl.TabIndex = 103;
            this.chkCommandKeyAsControl.Text = "Use Command key as Control key";
            this.chkCommandKeyAsControl.UseVisualStyleBackColor = true;
            // 
            // tabUtauSingers
            // 
            this.tabUtauSingers.AutoScroll = true;
            this.tabUtauSingers.Controls.Add( this.btnRemove );
            this.tabUtauSingers.Controls.Add( this.btnAdd );
            this.tabUtauSingers.Controls.Add( this.btnUp );
            this.tabUtauSingers.Controls.Add( this.btnDown );
            this.tabUtauSingers.Controls.Add( this.listSingers );
            this.tabUtauSingers.Location = new System.Drawing.Point( 4, 38 );
            this.tabUtauSingers.Name = "tabUtauSingers";
            this.tabUtauSingers.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabUtauSingers.Size = new System.Drawing.Size( 454, 351 );
            this.tabUtauSingers.TabIndex = 6;
            this.tabUtauSingers.Text = "UTAU Singers";
            this.tabUtauSingers.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Location = new System.Drawing.Point( 98, 307 );
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size( 75, 23 );
            this.btnRemove.TabIndex = 122;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point( 17, 307 );
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size( 75, 23 );
            this.btnAdd.TabIndex = 121;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point( 279, 307 );
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size( 75, 23 );
            this.btnUp.TabIndex = 123;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point( 360, 307 );
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size( 75, 23 );
            this.btnDown.TabIndex = 124;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // listSingers
            // 
            this.listSingers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listSingers.FullRowSelect = true;
            this.listSingers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listSingers.Location = new System.Drawing.Point( 17, 23 );
            this.listSingers.MultiSelect = false;
            this.listSingers.Name = "listSingers";
            this.listSingers.Size = new System.Drawing.Size( 418, 277 );
            this.listSingers.TabIndex = 120;
            this.listSingers.UseCompatibleStateImageBehavior = false;
            this.listSingers.View = System.Windows.Forms.View.Details;
            // 
            // tabFile
            // 
            this.tabFile.Controls.Add( this.lblAutoBackupMinutes );
            this.tabFile.Controls.Add( this.numAutoBackupInterval );
            this.tabFile.Controls.Add( this.lblAutoBackupInterval );
            this.tabFile.Controls.Add( this.chkAutoBackup );
            this.tabFile.Location = new System.Drawing.Point( 4, 38 );
            this.tabFile.Name = "tabFile";
            this.tabFile.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabFile.Size = new System.Drawing.Size( 454, 351 );
            this.tabFile.TabIndex = 7;
            this.tabFile.Text = "File";
            this.tabFile.UseVisualStyleBackColor = true;
            // 
            // lblAutoBackupMinutes
            // 
            this.lblAutoBackupMinutes.AutoSize = true;
            this.lblAutoBackupMinutes.Location = new System.Drawing.Point( 318, 20 );
            this.lblAutoBackupMinutes.Name = "lblAutoBackupMinutes";
            this.lblAutoBackupMinutes.Size = new System.Drawing.Size( 53, 12 );
            this.lblAutoBackupMinutes.TabIndex = 3;
            this.lblAutoBackupMinutes.Text = "minute(s)";
            // 
            // numAutoBackupInterval
            // 
            this.numAutoBackupInterval.Location = new System.Drawing.Point( 243, 18 );
            this.numAutoBackupInterval.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numAutoBackupInterval.Name = "numAutoBackupInterval";
            this.numAutoBackupInterval.Size = new System.Drawing.Size( 69, 19 );
            this.numAutoBackupInterval.TabIndex = 2;
            this.numAutoBackupInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numAutoBackupInterval.Value = new decimal( new int[] {
            10,
            0,
            0,
            0} );
            // 
            // lblAutoBackupInterval
            // 
            this.lblAutoBackupInterval.AutoSize = true;
            this.lblAutoBackupInterval.Location = new System.Drawing.Point( 194, 20 );
            this.lblAutoBackupInterval.Name = "lblAutoBackupInterval";
            this.lblAutoBackupInterval.Size = new System.Drawing.Size( 43, 12 );
            this.lblAutoBackupInterval.TabIndex = 1;
            this.lblAutoBackupInterval.Text = "interval";
            // 
            // chkAutoBackup
            // 
            this.chkAutoBackup.AutoSize = true;
            this.chkAutoBackup.Location = new System.Drawing.Point( 18, 19 );
            this.chkAutoBackup.Name = "chkAutoBackup";
            this.chkAutoBackup.Size = new System.Drawing.Size( 127, 16 );
            this.chkAutoBackup.TabIndex = 0;
            this.chkAutoBackup.Text = "Automatical Backup";
            this.chkAutoBackup.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 374, 416 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 88, 23 );
            this.btnCancel.TabIndex = 201;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 280, 416 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 88, 23 );
            this.btnOK.TabIndex = 200;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // Preference
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 96F, 96F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 475, 455 );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.tabPreference );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Preference";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Preference";
            this.tabPreference.ResumeLayout( false );
            this.tabSequence.ResumeLayout( false );
            this.tabSequence.PerformLayout();
            this.groupAutoVibratoConfig.ResumeLayout( false );
            this.groupAutoVibratoConfig.PerformLayout();
            this.tabAnother.ResumeLayout( false );
            this.tabAnother.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPreSendTimeSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTiming)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreSendTime)).EndInit();
            this.tabAppearance.ResumeLayout( false );
            this.tabAppearance.PerformLayout();
            this.groupFont.ResumeLayout( false );
            this.groupFont.PerformLayout();
            this.groupVisibleCurve.ResumeLayout( false );
            this.groupVisibleCurve.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTrackHeight)).EndInit();
            this.tabOperation.ResumeLayout( false );
            this.groupMisc.ResumeLayout( false );
            this.groupMisc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaximumFrameRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMouseHoverTime)).EndInit();
            this.groupPianoroll.ResumeLayout( false );
            this.groupPianoroll.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEx1)).EndInit();
            this.tabPlatform.ResumeLayout( false );
            this.groupUtauCores.ResumeLayout( false );
            this.groupUtauCores.PerformLayout();
            this.groupVsti.ResumeLayout( false );
            this.groupVsti.PerformLayout();
            this.groupPlatform.ResumeLayout( false );
            this.groupPlatform.PerformLayout();
            this.tabUtauSingers.ResumeLayout( false );
            this.tabFile.ResumeLayout( false );
            this.tabFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoBackupInterval)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.TabControl tabPreference;
        private System.Windows.Forms.TabPage tabSequence;
        private BButton btnCancel;
        private BButton btnOK;
        private System.Windows.Forms.TabPage tabAnother;
        private System.Windows.Forms.TabPage tabAppearance;
        private BButton btnChangeMenuFont;
        private BLabel labelMenu;
        private BLabel labelMenuFontName;
        private BLabel labelScreen;
        private BButton btnChangeScreenFont;
        private BLabel labelScreenFontName;
        private BLabel lblVibratoConfig;
        private BComboBox comboVibratoLength;
        private BLabel lblVibratoLength;
        private BGroupBox groupAutoVibratoConfig;
        private BLabel label3;
        private BLabel lblAutoVibratoType1;
        private BLabel lblAutoVibratoMinLength;
        private BCheckBox chkEnableAutoVibrato;
        private BLabel label6;
        private BComboBox comboAutoVibratoType1;
        private BComboBox comboAutoVibratoMinLength;
        private BLabel label7;
        private BCheckBox chkChasePastEvent;
        private BLabel lblDefaultPremeasure;
        private BLabel lblWait;
        private BLabel lblPreSendTime;
        private BLabel lblDefaultSinger;
        private BLabel lblPreSendTimeSample;
        private BLabel lblTiming;
        private BCheckBox chkEnableSampleOutput;
        private BLabel lblSampleOutput;
        private BLabel label11;
        private BComboBox comboDefualtSinger;
        private BLabel label12;
        private BComboBox comboDefaultPremeasure;
        private NumericUpDownEx numWait;
        private NumericUpDownEx numPreSendTime;
        private BLabel label15;
        private NumericUpDownEx numPreSendTimeSample;
        private BLabel label14;
        private NumericUpDownEx numTiming;
        private BLabel label13;
        private BLabel lblPeriod;
        private BLabel lblAmplitude;
        private BLabel lblDynamics;
        private BLabel label1;
        private BLabel lblResolution;
        private BComboBox comboPeriod;
        private BComboBox comboAmplitude;
        private BComboBox comboDynamics;
        private BLabel label5;
        private BLabel label4;
        private BLabel label2;
        private BComboBox comboLanguage;
        private BLabel lblLanguage;
        private System.Windows.Forms.TabPage tabPlatform;
        private BLabel lblPlatform;
        private BComboBox comboPlatform;
        private BGroupBox groupPlatform;
        private BCheckBox chkCommandKeyAsControl;
        private System.Windows.Forms.TabPage tabOperation;
        private NumericUpDownEx numMaximumFrameRate;
        private BLabel lblMaximumFrameRate;
        private BLabel labelWheelOrder;
        private NumericUpDownEx numericUpDownEx1;
        private BCheckBox chkScrollHorizontal;
        private BCheckBox chkCursorFix;
        private BCheckBox chkKeepLyricInputMode;
        private BLabel lblTrackHeight;
        private NumericUpDownEx numTrackHeight;
        private BLabel lblMouseHoverTime;
        private NumericUpDownEx numMouseHoverTime;
        private BLabel lblMilliSecond;
        private BCheckBox chkPlayPreviewWhenRightClick;
        private BCheckBox chkCurveSelectingQuantized;
        private BGroupBox groupVisibleCurve;
        private BGroupBox groupFont;
        private BCheckBox chkBri;
        private BCheckBox chkBre;
        private BCheckBox chkDyn;
        private BCheckBox chkVel;
        private BCheckBox chkVibratoDepth;
        private BCheckBox chkVibratoRate;
        private BCheckBox chkDecay;
        private BCheckBox chkAccent;
        private BCheckBox chkPit;
        private BCheckBox chkPor;
        private BCheckBox chkGen;
        private BCheckBox chkOpe;
        private BCheckBox chkCle;
        private BLabel lblMidiInPort;
        private BComboBox comboMidiInPortNumber;
        private BGroupBox groupVsti;
        private BLabel lblVOCALOID2;
        private BLabel lblVOCALOID1;
        private BTextBox txtVOCALOID2;
        private BTextBox txtVOCALOID1;
        private BCheckBox chkFx2Depth;
        private BCheckBox chkHarmonics;
        private BCheckBox chkReso2;
        private BCheckBox chkReso1;
        private BCheckBox chkReso4;
        private BCheckBox chkReso3;
        private System.Windows.Forms.TabPage tabUtauSingers;
        private BListView listSingers;
        private BGroupBox groupUtauCores;
        private BLabel lblResampler;
        private BCheckBox chkInvokeWithWine;
        private BButton btnWavtool;
        private BTextBox txtResampler;
        private BLabel lblWavtool;
        private BButton btnResampler;
        private BTextBox txtWavtool;
        private BButton btnUp;
        private BButton btnDown;
        private BButton btnAdd;
        private BGroupBox groupPianoroll;
        private BGroupBox groupMisc;
        private BButton btnRemove;
        private BCheckBox chkTranslateRoman;
        private BCheckBox chkPbs;
        private BCheckBox chkEnvelope;
        private System.Windows.Forms.TabPage tabFile;
        private BCheckBox chkAutoBackup;
        private BLabel lblAutoBackupInterval;
        private NumericUpDownEx numAutoBackupInterval;
        private BLabel lblAutoBackupMinutes;
        private BComboBox comboAutoVibratoType2;
        private BLabel lblAutoVibratoType2;
        private BCheckBox chkUseSpaceKeyAsMiddleButtonModifier;
        #endregion
#endif
    }

#if !JAVA
}
#endif
