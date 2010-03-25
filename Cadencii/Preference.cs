/*
 * Preference.cs
 * Copyright (C) 2008-2010 kbinani
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

//INCLUDE-SECTION IMPORT ..\BuildJavaUI\src\org\kbinani\Cadencii\Preference.java

import java.awt.*;
import java.util.*;
import java.io.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.media.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.apputil;
using org.kbinani.java.awt;
using org.kbinani.java.io;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using BEventArgs = System.EventArgs;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class Preference extends BDialog {
#else
    class Preference : BDialog {
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
            //INCLUDE-SECTION CTOR ..\BuildJavaUI\src\org\kbinani\Cadencii\Preference.java
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
            applyLanguage();

            comboVibratoLength.removeAllItems();
#if JAVA
            for( DefaultVibratoLengthEnum dvl : DefaultVibratoLengthEnum.values() )
#else
            foreach ( DefaultVibratoLengthEnum dvl in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) )
#endif
            {
                comboVibratoLength.addItem( DefaultVibratoLengthUtil.toString( dvl ) );
            }
            comboVibratoLength.setSelectedIndex( 1 );

            comboAutoVibratoMinLength.removeAllItems();
#if JAVA
            for( AutoVibratoMinLengthEnum avml : AutoVibratoMinLengthEnum.values() )
#else
            foreach ( AutoVibratoMinLengthEnum avml in Enum.GetValues( typeof( AutoVibratoMinLengthEnum ) ) )
#endif
            {
                comboAutoVibratoMinLength.addItem( AutoVibratoMinLengthUtil.toString( avml ) );
            }
            comboAutoVibratoMinLength.setSelectedIndex( 0 );

            comboAutoVibratoType1.removeAllItems();
            for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( SynthesizerType.VOCALOID1 ); itr.hasNext(); ) {
                VibratoHandle vconfig = itr.next();
                comboAutoVibratoType1.addItem( vconfig );
            }
            if ( comboAutoVibratoType1.getItemCount() > 0 ) {
                comboAutoVibratoType1.setSelectedIndex( 0 );
            }

            comboAutoVibratoType2.removeAllItems();
            for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( SynthesizerType.VOCALOID2 ); itr.hasNext(); ) {
                VibratoHandle vconfig = itr.next();
                comboAutoVibratoType2.addItem( vconfig );
            }
            if ( comboAutoVibratoType2.getItemCount() > 0 ) {
                comboAutoVibratoType2.setSelectedIndex( 0 );
            }

            comboDynamics.removeAllItems();
            comboAmplitude.removeAllItems();
            comboPeriod.removeAllItems();
            for ( Iterator<ClockResolution> itr = ClockResolutionUtility.iterator(); itr.hasNext(); ) {
                ClockResolution cr = itr.next();
                comboDynamics.addItem( ClockResolutionUtility.toString( cr ) );
                comboAmplitude.addItem( ClockResolutionUtility.toString( cr ) );
                comboPeriod.addItem( ClockResolutionUtility.toString( cr ) );
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

#if DEBUG
#if JAVA
            for( PlatformEnum p : PlatformEnum.values() )
#else
            foreach( PlatformEnum p in Enum.GetValues( typeof( PlatformEnum ) ) )
#endif
            {
                comboPlatform.addItem( p + "" );
            }
#else // #if DEBUG
#if JAVA
            String osname = System.getProperty( "os.name" );
            if( osname.indexOf( "Windows" ) >= 0 ){
                comboPlatform.addItem( PlatformEnum.Windows + "" );
                comboPlatform.setEnabled( false );
                chkCommandKeyAsControl.setEnabled( false );
            }else{
                for( PlatformEnum p : PlatformEnum.values() ){
                    comboPlatform.addItem( p + "" );
                }
            }
#else // #if JAVA
            PlatformID platform = Environment.OSVersion.Platform;
            if ( platform == PlatformID.Win32NT ||
                 platform == PlatformID.Win32S ||
                 platform == PlatformID.Win32Windows ||
                 platform == PlatformID.WinCE ) {
                comboPlatform.addItem( PlatformEnum.Windows + "" );
                comboPlatform.setEnabled( false );
                chkCommandKeyAsControl.setEnabled( false );
            } else {
                foreach ( PlatformEnum p in Enum.GetValues( typeof( PlatformEnum ) ) ) {
                    comboPlatform.addItem( p + "" );
                }
            }
#endif // #if JAVA
#endif // #if DEBUG

            comboMidiInPortNumber.removeAllItems();
#if ENABLE_MIDI
            MIDIINCAPS[] midiins = MidiInDevice.GetMidiInDevices();
            for ( int i = 0; i < midiins.Length; i++ ) {
                comboMidiInPortNumber.addItem( midiins[i] );
            }
            if ( midiins.Length <= 0 ) {
                comboMidiInPortNumber.setEnabled( false );
            } else {
                comboMidiInPortNumber.setEnabled( true );
            }
#else
            comboMidiInPortNumber.setEnabled( false );
#endif

#if ENABLE_MTC
            MIDIINCAPS[] midiins2 = MidiInDevice.GetMidiInDevices();
            for ( int i = 0; i < midiins2.Length; i++ ) {
                comboMtcMidiInPortNumber.addItem( midiins2[i] );
            }
            if ( midiins2.Length <= 0 ){
                comboMtcMidiInPortNumber.setEnabled( false );
            } else {
                comboMtcMidiInPortNumber.setEnabled( true );
            }
#else
            comboMtcMidiInPortNumber.setEnabled( false );
#endif

            txtVOCALOID1.setText( VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID1 ) );
            txtVOCALOID2.setText( VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 ) );

            listSingers.setColumnWidth( 0, columnWidthHeaderProgramChange );
            listSingers.setColumnWidth( 1, columnWidthHeaderName );
            listSingers.setColumnWidth( 2, columnWidthHeaderPath );

            if ( VocaloSysUtil.getDefaultDseVersion() == 100 ) {
                chkLoadVocaloid100.setText( "VOCALOID1 [1.0] (" + _( "primary" ) + ")" );
                chkLoadVocaloid101.setText( "VOCALOID1 [1.1] (" + _( "secondary" ) + ")" );
            } else {
                chkLoadVocaloid100.setText( "VOCALOID1 [1.0] (" + _( "secondary" ) + ")" );
                chkLoadVocaloid101.setText( "VOCALOID1 [1.1] (" + _( "primary" ) + ")" );
            }

            if ( VocaloSysUtil.isDSEVersion101Available() ) {
                chkLoadSecondaryVOCALOID1.setEnabled( true );
            } else {
                chkLoadSecondaryVOCALOID1.setEnabled( false );
                chkLoadVocaloid101.setEnabled( false );
            }
            chkLoadSecondaryVOCALOID1_CheckedChanged( null, null );

            registerEventHandlers();
            setResources();
        }

        #region public methods
        /// <summary>
        /// バッファーサイズの設定値（単位：ミリ秒）を取得します。
        /// </summary>
        /// <returns></returns>
        public int getBufferSize() {
            return (int)numBuffer.getValue();
        }

        /// <summary>
        /// バッファーサイズの設定値（単位：ミリ秒）を設定します。
        /// </summary>
        /// <param name="value"></param>
        public void setBufferSize( int value ) {
            numBuffer.setValue( value );
        }

        public boolean isLoadSecondaryVocaloid1Dll() {
            return chkLoadSecondaryVOCALOID1.isSelected();
        }

        public void setLoadSecondaryVocaloid1Dll( boolean value ) {
            chkLoadSecondaryVOCALOID1.setSelected( value );
        }

        public boolean isLoadVocaloid100() {
            if ( chkLoadVocaloid100.isEnabled() ) {
                return chkLoadVocaloid100.isSelected();
            } else {
                return false;
            }
        }

        public void setLoadVocaloid100( boolean value ) {
            if ( chkLoadVocaloid100.isEnabled() ) {
                chkLoadVocaloid100.setSelected( value );
            }
        }

        public boolean isLoadVocaloid101() {
            if ( chkLoadVocaloid101.isEnabled() ) {
                return chkLoadVocaloid101.isSelected();
            } else {
                return false;
            }
        }

        public void setLoadVocaloid101( boolean value ) {
            if ( chkLoadVocaloid101.isEnabled() ) {
                chkLoadVocaloid101.setSelected( value );
            }
        }

        public boolean isLoadVocaloid2() {
            return chkLoadVocaloid2.isSelected();
        }

        public void setLoadVocaloid2( boolean value ) {
            chkLoadVocaloid2.setSelected( value );
        }

        public boolean isLoadAquesTone() {
            return chkLoadAquesTone.isSelected();
        }

        public void setLoadAquesTone( boolean value ) {
            chkLoadAquesTone.setSelected( value );
        }

        public boolean isUseProjectCache() {
            return chkKeepProjectCache.isSelected();
        }

        public void setUseProjectCache( boolean value ) {
            chkKeepProjectCache.setSelected( value );
        }

        public boolean isUseSpaceKeyAsMiddleButtonModifier() {
            return chkUseSpaceKeyAsMiddleButtonModifier.isSelected();
        }

        public void setUseSpaceKeyAsMiddleButtonModifier( boolean value ) {
            chkUseSpaceKeyAsMiddleButtonModifier.setSelected( value );
        }

        public int getAutoBackupIntervalMinutes() {
            if ( chkAutoBackup.isSelected() ) {
                return (int)numAutoBackupInterval.getValue();
            } else {
                return 0;
            }
        }

        public void setAutoBackupIntervalMinutes( int value ) {
            if ( value <= 0 ) {
                chkAutoBackup.setSelected( false );
            } else {
                chkAutoBackup.setSelected( true );
                numAutoBackupInterval.setValue( value );
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

#if ENABLE_MTC
        public int getMtcMidiInPort() {
            if ( comboMtcMidiInPortNumber.isEnabled() ) {
                int indx = comboMtcMidiInPortNumber.getSelectedIndex();
                if ( indx >= 0 ) {
                    return indx;
                } else {
                    return 0;
                }
            } else {
                return -1;
            }
        }
#endif

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

#if ENABLE_MTC
        public void setMtcMidiInPort( int value ){
            if ( comboMtcMidiInPortNumber.isEnabled() ){
                if ( 0 <= value && value < comboMtcMidiInPortNumber.getItemCount() ){
                    comboMtcMidiInPortNumber.setSelectedIndex( value );
                } else {
                    comboMtcMidiInPortNumber.setSelectedIndex( 0 );
                }
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
            return (int)numMouseHoverTime.getValue();
        }

        public void setMouseHoverTime( int value ) {
            numMouseHoverTime.setValue( value );
        }

        public int getPxTrackHeight() {
            return (int)numTrackHeight.getValue();
        }

        public void setPxTrackHeight( int value ) {
            numTrackHeight.setValue( value );
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
            return (int)numMaximumFrameRate.getValue();
        }

        public void setMaximumFrameRate( int value ) {
            numMaximumFrameRate.setValue( value );
        }

        public boolean isScrollHorizontalOnWheel() {
            return chkScrollHorizontal.isSelected();
        }

        public void setScrollHorizontalOnWheel( boolean value ) {
            chkScrollHorizontal.setSelected( value );
        }

        public void applyLanguage() {
            setTitle( _( "Preference" ) );
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

            #region tabのタイトル
#if JAVA
            int count = tabPane.getTabCount();
            for( int i = 0; i < count; i++ ){
                Component c = tabPane.getComponentAt( i );
                if( !(c instanceof BPanel) ){
                    continue;
                }
                BPanel p = (BPanel)c;
                if( p == tabSequence ){
                    tabPane.setTitleAt( i, _( "Sequence" ) );
                }else if( p == tabAnother ){
                    tabPane.setTitleAt( i, _( "Other Settings" ) );
                }else if( p == tabAppearance ){
                    tabPane.setTitleAt( i, _( "Appearance" ) );
                }else if( p == tabOperation ){
                    tabPane.setTitleAt( i, _( "Operation" ) );
                }else if( p == tabPlatform ){
                    tabPane.setTitleAt( i, _( "Platform" ) );
                }else if( p == tabUtauSingers ){
                    tabPane.setTitleAt( i, _( "UTAU Singers" ) );
                }else if( p == tabFile ){
                    tabPane.setTitleAt( i, _( "File" ) );
                }
            }
#else
            tabSequence.Text = _( "Sequence" );
            tabAnother.Text = _( "Other Settings" );
            tabAppearance.Text = _( "Appearance" );
            tabOperation.Text = _( "Operation" );
            tabPlatform.Text = _( "Platform" );
            tabUtauSingers.Text = _( "UTAU Singers" );
            tabFile.Text = _( "File" );
            tabSingingSynth.Text = _( "Synthesizer" );
#endif
            #endregion

            #region tabSequence
            lblResolution.setText( _( "Resolution(VSTi)" ) );
            lblDynamics.setText( _( "Dynamics" ) + "(&D)" );
            lblAmplitude.setText( _( "Vibrato Depth" ) + "(&R)" );
            lblPeriod.setText( _( "Vibrato Rate" ) + "(&V)" );
            lblVibratoConfig.setText( _( "Vibrato Settings" ) );
            lblVibratoLength.setText( _( "Default Vibrato Length" ) + "(&L)" );
            groupAutoVibratoConfig.setTitle( _( "Auto Vibrato Settings" ) );
            chkEnableAutoVibrato.setText( _( "Enable Automatic Vibrato" ) + "(&E)" );
            lblAutoVibratoMinLength.setText( _( "Minimum note length for Automatic Vibrato" ) + "(&M)" );
            lblAutoVibratoType1.setText( _( "Vibrato Type" ) + ": VOCALOID1 (&T)" );
            lblAutoVibratoType2.setText( _( "Vibrato Type" ) + ": VOCALOID2 (&T)" );
            #endregion

            #region tabAnother
            lblDefaultSinger.setText( _( "Default Singer" ) + "(&S)" );
            lblPreSendTime.setText( _( "Pre-Send time" ) + "(&P)" );
            lblWait.setText( _( "Waiting Time" ) + "(&W)" );
            lblDefaultPremeasure.setText( _( "Default Pre-measure" ) + "(&M)" );
            chkChasePastEvent.setText( _( "Chase Event" ) + "(&C)" );
            groupWaveFileOutput.setTitle( _( "Wave File Output" ) );
            lblChannel.setText( _( "Channel" ) + "(&C)" );
            radioMasterTrack.setText( _( "Master Track" ) );
            radioCurrentTrack.setText( _( "Current Track" ) );
            #endregion

            #region tabAppearance
            groupFont.setTitle( _( "Font" ) );
            labelMenu.setText( _( "Menu / Lyrics" ) );
            labelScreen.setText( _( "Screen" ) );
            lblLanguage.setText( _( "UI Language" ) );
            btnChangeMenuFont.setText( _( "Change" ) );
            btnChangeScreenFont.setText( _( "Change" ) );
            lblTrackHeight.setText( _( "Track Height (pixel)" ) );
            groupVisibleCurve.setTitle( _( "Visible Control Curve" ) );
            #endregion

            #region tabOperation
            labelWheelOrder.setText( _( "Mouse wheel Rate" ) );
            chkCursorFix.setText( _( "Fix Play Cursor to Center" ) );
            chkScrollHorizontal.setText( _( "Horizontal Scroll when Mouse wheel" ) );
            lblMaximumFrameRate.setText( _( "Maximum Frame Rate" ) );
            chkKeepLyricInputMode.setText( _( "Keep Lyric Input Mode" ) );
            lblMouseHoverTime.setText( _( "Waiting Time for Preview" ) );
            lblMilliSecond.setText( _( "milli second" ) );
            chkPlayPreviewWhenRightClick.setText( _( "Play Preview On Right Click" ) );
            chkCurveSelectingQuantized.setText( _( "Enable Quantize for Curve Selecting" ) );
            lblMidiInPort.setText( _( "MIDI In Port Number" ) );
            chkUseSpaceKeyAsMiddleButtonModifier.setText( _( "Use space key as Middle button modifier" ) );

            groupPianoroll.setTitle( _( "Piano Roll" ) );
            groupMisc.setTitle( _( "Misc" ) );
            #endregion

            #region tabPlatform
            groupPlatform.setTitle( _( "Platform" ) );
            lblPlatform.setText( _( "Current Platform" ) );
            chkCommandKeyAsControl.setText( _( "Use Command key as Control key" ) );
            chkTranslateRoman.setText( _( "Translate Roman letters into Kana" ) );

            groupUtauCores.setTitle( _( "UTAU Cores" ) );
            chkInvokeWithWine.setText( _( "Invoke UTAU cores with Wine" ) );
            #endregion

            #region tabUtauSingers
            listSingers.setColumnHeaders( new String[] { _( "Program Change" ), _( "Name" ), _( "Path" ) } );
            btnAdd.setText( _( "Add" ) );
            btnRemove.setText( _( "Remove" ) );
            btnUp.setText( _( "Up" ) );
            btnDown.setText( _( "Down" ) );
            #endregion

            #region tabFile
            chkAutoBackup.setText( _( "Automatical Backup" ) );
            lblAutoBackupInterval.setText( _( "interval" ) );
            lblAutoBackupMinutes.setText( _( "minute(s)" ) );
            chkKeepProjectCache.setText( _( "Keep Project Cache" ) );
            #endregion

            #region tabSingingSynth
            groupSynthesizerDll.setTitle( _( "Synthesizer DLL Usage" ) );
            chkLoadSecondaryVOCALOID1.setText( _( "Load secondary VOCALOID1 DLL" ) );
            #endregion
        }

        public boolean isWaveFileOutputFromMasterTrack() {
            return radioMasterTrack.isSelected();
        }

        public void setWaveFileOutputFromMasterTrack( boolean value ) {
            radioMasterTrack.setSelected( value );
            radioCurrentTrack.setSelected( !value );
        }

        public int getWaveFileOutputChannel() {
            if ( comboChannel.getSelectedIndex() <= 0 ) {
                return 1;
            } else {
                return 2;
            }
        }

        public void setWaveFileOutputChannel( int value ) {
            if ( value == 1 ) {
                comboChannel.setSelectedIndex( 0 );
            } else {
                comboChannel.setSelectedIndex( 1 );
            }
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
            for ( Iterator<ClockResolution> itr = ClockResolutionUtility.iterator(); itr.hasNext(); ) {
                ClockResolution vt = itr.next();
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
            for ( Iterator<ClockResolution> itr = ClockResolutionUtility.iterator(); itr.hasNext(); ) {
                ClockResolution vt = itr.next();
                count++;
                if ( vt.Equals( value ) ) {
                    comboDynamics.setSelectedIndex( count );
                    break;
                }
            }
        }

        public int getPreSendTime() {
            return (int)numPreSendTime.getValue();
        }

        public void setPreSendTime( int value ) {
            numPreSendTime.setValue( value );
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
                VibratoHandle vconfig = (VibratoHandle)comboAutoVibratoType1.getSelectedItem();
                return vconfig.IconID;
            } else {
                return "$04040001";
            }
        }

        public void setAutoVibratoType1( String value ) {
            for ( int i = 0; i < comboAutoVibratoType1.getItemCount(); i++ ) {
                VibratoHandle vconfig = (VibratoHandle)comboAutoVibratoType1.getItemAt( i );
                if ( vconfig.IconID.Equals( value ) ) {
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
                VibratoHandle vconfig = (VibratoHandle)comboAutoVibratoType2.getSelectedItem();
                return vconfig.IconID;
            } else {
                return "$04040001";
            }
        }

        public void setAutoVibratoType2( String value ) {
            for ( int i = 0; i < comboAutoVibratoType2.getItemCount(); i++ ) {
                VibratoHandle vconfig = (VibratoHandle)comboAutoVibratoType2.getItemAt( i );
                if ( vconfig.IconID.Equals( value ) ) {
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
#if JAVA
            for( AutoVibratoMinLengthEnum avml : AutoVibratoMinLengthEnum.values() )
#else
            foreach ( AutoVibratoMinLengthEnum avml in Enum.GetValues( typeof( AutoVibratoMinLengthEnum ) ) )
#endif
 {
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
#if JAVA
            for( AutoVibratoMinLengthEnum avml : AutoVibratoMinLengthEnum.values() )
#else
            foreach ( AutoVibratoMinLengthEnum avml in Enum.GetValues( typeof( AutoVibratoMinLengthEnum ) ) )
#endif
 {
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
#if JAVA
            for( DefaultVibratoLengthEnum vt : DefaultVibratoLengthEnum.values() )
#else
            foreach ( DefaultVibratoLengthEnum vt in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) )
#endif
 {
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
#if JAVA
            for( DefaultVibratoLengthEnum dvl : DefaultVibratoLengthEnum.values() )
#else
            foreach ( DefaultVibratoLengthEnum dvl in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) )
#endif
 {
                count++;
                if ( dvl == value ) {
                    comboVibratoLength.setSelectedIndex( count );
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
            return (int)numericUpDownEx1.getValue();
        }

        public void setWheelOrder( int value ) {
            if ( value < numericUpDownEx1.getMinimum() ) {
                numericUpDownEx1.setValue( numericUpDownEx1.getMinimum() );
            } else if ( numericUpDownEx1.getMaximum() < value ) {
                numericUpDownEx1.setValue( numericUpDownEx1.getMaximum() );
            } else {
                numericUpDownEx1.setValue( value );
            }
        }

        public Font getScreenFont() {
            return m_screen_font;
        }

        public void setScreenFont( Font value ) {
            m_screen_font = value;
            labelScreenFontName.setText( m_screen_font.getName() );
        }

        public java.awt.Font getBaseFont() {
            return m_base_font;
        }

        public void setBaseFont( java.awt.Font value ) {
            m_base_font = value;
            labelMenuFontName.setText( m_base_font.getName() );
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

        public boolean isCommandKeyAsControl() {
            return chkCommandKeyAsControl.isSelected();
        }

        public void setCommandKeyAsControl( boolean value ) {
            chkCommandKeyAsControl.setSelected( value );
        }

        public String getPathResampler() {
            return txtResampler.getText();
        }

        public void setPathResampler( String value ) {
            txtResampler.setText( value );
        }

        public String getPathWavtool() {
            return txtWavtool.getText();
        }

        public void setPathWavtool( String value ) {
            txtWavtool.setText( value );
        }

        public String getPathAquesTone() {
            return txtAquesTone.getText();
        }

        public void setPathAquesTone( String value ) {
            txtAquesTone.setText( value );
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
        #endregion

        #region event handlers
        public void chkLoadSecondaryVOCALOID1_CheckedChanged( Object sender, EventArgs e ) {
            if ( VocaloSysUtil.isDSEVersion101Available() ) {
                if ( chkLoadSecondaryVOCALOID1.isSelected() ) {
                    chkLoadVocaloid100.setEnabled( true );
                    chkLoadVocaloid101.setEnabled( true );
                } else {
                    if ( VocaloSysUtil.getDefaultDseVersion() == 100 ) {
                        chkLoadVocaloid100.setEnabled( true );
                        chkLoadVocaloid101.setEnabled( false );
                    } else {
                        chkLoadVocaloid100.setEnabled( false );
                        chkLoadVocaloid101.setEnabled( true );
                    }
                }
            }
        }

        public void btnChangeMenuFont_Click( Object sender, BEventArgs e ) {
            fontDialog.setSelectedFont( getBaseFont() );
            fontDialog.setVisible( true );
            if ( fontDialog.getDialogResult() == BDialogResult.OK ) {
                m_base_font = fontDialog.getSelectedFont();
            }
        }

        public void btnOK_Click( Object sender, BEventArgs e ) {
            boolean was_modified = false;
            if ( AppManager.editorConfig.DoNotUseVocaloid2 != !isLoadVocaloid2() ) {
                was_modified = true;
            }
            if ( AppManager.editorConfig.DoNotUseVocaloid101 != !isLoadVocaloid101() ) {
                was_modified = true;
            }
            if ( AppManager.editorConfig.DoNotUseVocaloid100 != !isLoadVocaloid100() ) {
                was_modified = true;
            }
            if ( was_modified ) {
                AppManager.showMessageBox( _( "Restart Cadencii to complete your changes" ),
                                           "Cadencii",
                                           PortUtil.OK_OPTION,
                                           PortUtil.MSGBOX_INFORMATION_MESSAGE );
            }

            setDialogResult( BDialogResult.OK );
        }

        public void btnChangeScreenFont_Click( Object sender, BEventArgs e ) {
            fontDialog.setSelectedFont( m_screen_font );
            fontDialog.setVisible( true );
            if ( fontDialog.getDialogResult() == BDialogResult.OK ) {
                m_screen_font = fontDialog.getSelectedFont();
            }
        }

        public void comboPlatform_SelectedIndexChanged( Object sender, BEventArgs e ) {
            String title = (String)comboPlatform.getSelectedItem();
#if JAVA
            for( PlatformEnum p : PlatformEnum.values() )
#else
            foreach ( PlatformEnum p in Enum.GetValues( typeof( PlatformEnum ) ) )
#endif
 {
                if ( title.Equals( p + "" ) ) {
                    m_platform = p;
                    chkCommandKeyAsControl.setEnabled( p != PlatformEnum.Windows );
                    break;
                }
            }
        }

        public void btnResampler_Click( Object sender, BEventArgs e ) {
            if ( !txtResampler.getText().Equals( "" ) && PortUtil.isDirectoryExists( PortUtil.getDirectoryName( txtResampler.getText() ) ) ) {
                openUtauCore.setInitialDirectory( PortUtil.getDirectoryName( txtResampler.getText() ) );
            }
            openUtauCore.setSelectedFile( "resampler.exe" );
            int dr = openUtauCore.showOpenDialog( this );
            if ( dr == BFileChooser.APPROVE_OPTION ) {
                String path = openUtauCore.getSelectedFile();
                txtResampler.setText( path );
                if ( txtWavtool.getText().Equals( "" ) ) {
                    String wavtool = PortUtil.combinePath( PortUtil.getDirectoryName( path ), "wavtool.exe" );
                    if ( PortUtil.isFileExists( wavtool ) ) {
                        txtWavtool.setText( wavtool );
                    }
                }
            }
        }

        public void btnWavtool_Click( Object sender, BEventArgs e ) {
            if ( !txtWavtool.getText().Equals( "" ) && PortUtil.isDirectoryExists( PortUtil.getDirectoryName( txtWavtool.getText() ) ) ) {
                openUtauCore.setInitialDirectory( PortUtil.getDirectoryName( txtWavtool.getText() ) );
            }
            openUtauCore.setSelectedFile( "wavtool.exe" );
            int dr = openUtauCore.showOpenDialog( this );
            if ( dr == BFileChooser.APPROVE_OPTION ) {
                String path = openUtauCore.getSelectedFile();
                txtWavtool.setText( path );
                if ( txtResampler.getText().Equals( "" ) ) {
                    String resampler = PortUtil.combinePath( PortUtil.getDirectoryName( path ), "resampler.exe" );
                    if ( PortUtil.isFileExists( resampler ) ) {
                        txtResampler.setText( resampler );
                    }
                }
            }
        }

        public void btnAquesTone_Click( Object sender, BEventArgs e ) {
            BFileChooser dialog = new BFileChooser( "" );
            if ( !txtAquesTone.getText().Equals( "" ) && PortUtil.isDirectoryExists( PortUtil.getDirectoryName( txtAquesTone.getText() ) ) ) {
                dialog.setInitialDirectory( PortUtil.getDirectoryName( txtAquesTone.getText() ) );
            }
            dialog.setSelectedFile( "AquesTone.dll" );
            int dr = dialog.showOpenDialog( this );
            if ( dr == BFileChooser.APPROVE_OPTION ) {
                String path = dialog.getSelectedFile();
                txtAquesTone.setText( path );
            }
        }

        public void btnAdd_Click( Object sender, BEventArgs e ) {
            folderBrowserSingers.setVisible( true );
            if ( folderBrowserSingers.getDialogResult() == BDialogResult.OK ) {
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
                            String[] spl = PortUtil.splitString( line, '=' );
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

        public void listSingers_SelectedIndexChanged( Object sender, BEventArgs e ) {
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

        public void btnRemove_Click( Object sender, BEventArgs e ) {
            int index = getUtauSingersSelectedIndex();
            if ( 0 <= index && index < m_utau_singers.size() ) {
                m_utau_singers.removeElementAt( index );
            }
            UpdateUtauSingerList();
        }

        public void btnDown_Click( Object sender, BEventArgs e ) {
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

        public void btnUp_Click( Object sender, BEventArgs e ) {
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

        public void chkAutoBackup_CheckedChanged( Object sender, BEventArgs e ) {
            numAutoBackupInterval.setEnabled( chkAutoBackup.isSelected() );
        }

        public void Preference_FormClosing( Object sender, BFormClosingEventArgs e ) {
            columnWidthHeaderProgramChange = listSingers.getColumnWidth( 0 );
            columnWidthHeaderName = listSingers.getColumnWidth( 1 );
            columnWidthHeaderPath = listSingers.getColumnWidth( 2 );
        }

        public void btnCancel_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }
        #endregion

        #region helper methods
        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void UpdateFonts( String font_name ) {
            if ( font_name.Equals( "" ) ) {
                return;
            }
            Font font = new Font( font_name, java.awt.Font.PLAIN, (int)getFont().getSize() );
            Util.applyFontRecurse( this, font );
        }

        private void UpdateUtauSingerList() {
            listSingers.clear();
            for ( int i = 0; i < m_utau_singers.size(); i++ ) {
                m_utau_singers.get( i ).Program = i;
                listSingers.addItem( "", new BListViewItem( new String[] { m_utau_singers.get( i ).Program + "",
                                                                           m_utau_singers.get( i ).VOICENAME, 
                                                                           m_utau_singers.get( i ).VOICEIDSTR } ) );
            }
        }

        private int getUtauSingersSelectedIndex() {
            return listSingers.getSelectedIndex( "" );
        }

        private void registerEventHandlers() {
            btnChangeScreenFont.clickEvent.add( new BEventHandler( this, "btnChangeScreenFont_Click" ) );
            btnChangeMenuFont.clickEvent.add( new BEventHandler( this, "btnChangeMenuFont_Click" ) );
            btnWavtool.clickEvent.add( new BEventHandler( this, "btnWavtool_Click" ) );
            btnResampler.clickEvent.add( new BEventHandler( this, "btnResampler_Click" ) );
            btnAquesTone.clickEvent.add( new BEventHandler( this, "btnAquesTone_Click" ) );
            comboPlatform.selectedIndexChangedEvent.add( new BEventHandler( this, "comboPlatform_SelectedIndexChanged" ) );
            btnRemove.clickEvent.add( new BEventHandler( this, "btnRemove_Click" ) );
            btnAdd.clickEvent.add( new BEventHandler( this, "btnAdd_Click" ) );
            btnUp.clickEvent.add( new BEventHandler( this, "btnUp_Click" ) );
            btnDown.clickEvent.add( new BEventHandler( this, "btnDown_Click" ) );
            listSingers.selectedIndexChangedEvent.add( new BEventHandler( this, "listSingers_SelectedIndexChanged" ) );
            chkAutoBackup.checkedChangedEvent.add( new BEventHandler( this, "chkAutoBackup_CheckedChanged" ) );
            btnOK.clickEvent.add( new BEventHandler( this, "btnOK_Click" ) );
            formClosingEvent.add( new BFormClosingEventHandler( this, "Preference_FormClosing" ) );
            btnCancel.clickEvent.add( new BEventHandler( this, "btnCancel_Click" ) );
            chkLoadSecondaryVOCALOID1.checkedChangedEvent.add( new BEventHandler( this, "chkLoadSecondaryVOCALOID1_CheckedChanged" ) );
        }

        private void setResources() {
        }
        #endregion

        #region UI implementation
#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ..\BuildJavaUI\src\org\kbinani\Cadencii\Preference.java
        //INCLUDE-SECTION METHOD ..\BuildJavaUI\src\org\kbinani\Cadencii\Preference.java
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup10 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup11 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup12 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup13 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup14 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup15 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            this.tabPreference = new System.Windows.Forms.TabControl();
            this.tabSequence = new System.Windows.Forms.TabPage();
            this.label5 = new org.kbinani.windows.forms.BLabel();
            this.label4 = new org.kbinani.windows.forms.BLabel();
            this.label2 = new org.kbinani.windows.forms.BLabel();
            this.comboPeriod = new org.kbinani.windows.forms.BComboBox();
            this.comboAmplitude = new org.kbinani.windows.forms.BComboBox();
            this.comboDynamics = new org.kbinani.windows.forms.BComboBox();
            this.lblPeriod = new org.kbinani.windows.forms.BLabel();
            this.lblAmplitude = new org.kbinani.windows.forms.BLabel();
            this.lblDynamics = new org.kbinani.windows.forms.BLabel();
            this.label1 = new org.kbinani.windows.forms.BLabel();
            this.lblResolution = new org.kbinani.windows.forms.BLabel();
            this.label7 = new org.kbinani.windows.forms.BLabel();
            this.groupAutoVibratoConfig = new org.kbinani.windows.forms.BGroupBox();
            this.comboAutoVibratoType2 = new org.kbinani.windows.forms.BComboBox();
            this.lblAutoVibratoType2 = new org.kbinani.windows.forms.BLabel();
            this.label6 = new org.kbinani.windows.forms.BLabel();
            this.comboAutoVibratoType1 = new org.kbinani.windows.forms.BComboBox();
            this.comboAutoVibratoMinLength = new org.kbinani.windows.forms.BComboBox();
            this.lblAutoVibratoType1 = new org.kbinani.windows.forms.BLabel();
            this.lblAutoVibratoMinLength = new org.kbinani.windows.forms.BLabel();
            this.chkEnableAutoVibrato = new org.kbinani.windows.forms.BCheckBox();
            this.label3 = new org.kbinani.windows.forms.BLabel();
            this.comboVibratoLength = new org.kbinani.windows.forms.BComboBox();
            this.lblVibratoLength = new org.kbinani.windows.forms.BLabel();
            this.lblVibratoConfig = new org.kbinani.windows.forms.BLabel();
            this.tabAnother = new System.Windows.Forms.TabPage();
            this.bLabel2 = new org.kbinani.windows.forms.BLabel();
            this.numBuffer = new org.kbinani.cadencii.NumericUpDownEx();
            this.lblBuffer = new org.kbinani.windows.forms.BLabel();
            this.groupWaveFileOutput = new org.kbinani.windows.forms.BGroupBox();
            this.radioCurrentTrack = new org.kbinani.windows.forms.BRadioButton();
            this.radioMasterTrack = new org.kbinani.windows.forms.BRadioButton();
            this.lblChannel = new org.kbinani.windows.forms.BLabel();
            this.comboChannel = new org.kbinani.windows.forms.BComboBox();
            this.label13 = new org.kbinani.windows.forms.BLabel();
            this.label12 = new org.kbinani.windows.forms.BLabel();
            this.comboDefaultPremeasure = new org.kbinani.windows.forms.BComboBox();
            this.comboDefualtSinger = new org.kbinani.windows.forms.BComboBox();
            this.chkChasePastEvent = new org.kbinani.windows.forms.BCheckBox();
            this.lblDefaultPremeasure = new org.kbinani.windows.forms.BLabel();
            this.lblWait = new org.kbinani.windows.forms.BLabel();
            this.lblPreSendTime = new org.kbinani.windows.forms.BLabel();
            this.lblDefaultSinger = new org.kbinani.windows.forms.BLabel();
            this.numWait = new org.kbinani.cadencii.NumericUpDownEx();
            this.numPreSendTime = new org.kbinani.cadencii.NumericUpDownEx();
            this.tabAppearance = new System.Windows.Forms.TabPage();
            this.groupFont = new org.kbinani.windows.forms.BGroupBox();
            this.labelMenu = new org.kbinani.windows.forms.BLabel();
            this.labelScreenFontName = new org.kbinani.windows.forms.BLabel();
            this.btnChangeScreenFont = new org.kbinani.windows.forms.BButton();
            this.labelScreen = new org.kbinani.windows.forms.BLabel();
            this.labelMenuFontName = new org.kbinani.windows.forms.BLabel();
            this.btnChangeMenuFont = new org.kbinani.windows.forms.BButton();
            this.groupVisibleCurve = new org.kbinani.windows.forms.BGroupBox();
            this.chkEnvelope = new org.kbinani.windows.forms.BCheckBox();
            this.chkPbs = new org.kbinani.windows.forms.BCheckBox();
            this.chkReso4 = new org.kbinani.windows.forms.BCheckBox();
            this.chkReso3 = new org.kbinani.windows.forms.BCheckBox();
            this.chkReso2 = new org.kbinani.windows.forms.BCheckBox();
            this.chkReso1 = new org.kbinani.windows.forms.BCheckBox();
            this.chkFx2Depth = new org.kbinani.windows.forms.BCheckBox();
            this.chkHarmonics = new org.kbinani.windows.forms.BCheckBox();
            this.chkPit = new org.kbinani.windows.forms.BCheckBox();
            this.chkPor = new org.kbinani.windows.forms.BCheckBox();
            this.chkGen = new org.kbinani.windows.forms.BCheckBox();
            this.chkOpe = new org.kbinani.windows.forms.BCheckBox();
            this.chkCle = new org.kbinani.windows.forms.BCheckBox();
            this.chkBri = new org.kbinani.windows.forms.BCheckBox();
            this.chkBre = new org.kbinani.windows.forms.BCheckBox();
            this.chkDyn = new org.kbinani.windows.forms.BCheckBox();
            this.chkVel = new org.kbinani.windows.forms.BCheckBox();
            this.chkVibratoDepth = new org.kbinani.windows.forms.BCheckBox();
            this.chkVibratoRate = new org.kbinani.windows.forms.BCheckBox();
            this.chkDecay = new org.kbinani.windows.forms.BCheckBox();
            this.chkAccent = new org.kbinani.windows.forms.BCheckBox();
            this.lblTrackHeight = new org.kbinani.windows.forms.BLabel();
            this.comboLanguage = new org.kbinani.windows.forms.BComboBox();
            this.lblLanguage = new org.kbinani.windows.forms.BLabel();
            this.numTrackHeight = new org.kbinani.cadencii.NumericUpDownEx();
            this.tabOperation = new System.Windows.Forms.TabPage();
            this.groupMisc = new org.kbinani.windows.forms.BGroupBox();
            this.comboMtcMidiInPortNumber = new org.kbinani.windows.forms.BComboBox();
            this.labelMtcMidiInPort = new org.kbinani.windows.forms.BLabel();
            this.lblMaximumFrameRate = new org.kbinani.windows.forms.BLabel();
            this.comboMidiInPortNumber = new org.kbinani.windows.forms.BComboBox();
            this.numMaximumFrameRate = new org.kbinani.cadencii.NumericUpDownEx();
            this.lblMidiInPort = new org.kbinani.windows.forms.BLabel();
            this.lblMouseHoverTime = new org.kbinani.windows.forms.BLabel();
            this.lblMilliSecond = new org.kbinani.windows.forms.BLabel();
            this.numMouseHoverTime = new org.kbinani.cadencii.NumericUpDownEx();
            this.groupPianoroll = new org.kbinani.windows.forms.BGroupBox();
            this.chkUseSpaceKeyAsMiddleButtonModifier = new org.kbinani.windows.forms.BCheckBox();
            this.labelWheelOrder = new org.kbinani.windows.forms.BLabel();
            this.numericUpDownEx1 = new org.kbinani.cadencii.NumericUpDownEx();
            this.chkCursorFix = new org.kbinani.windows.forms.BCheckBox();
            this.chkCurveSelectingQuantized = new org.kbinani.windows.forms.BCheckBox();
            this.chkScrollHorizontal = new org.kbinani.windows.forms.BCheckBox();
            this.chkPlayPreviewWhenRightClick = new org.kbinani.windows.forms.BCheckBox();
            this.chkKeepLyricInputMode = new org.kbinani.windows.forms.BCheckBox();
            this.tabPlatform = new System.Windows.Forms.TabPage();
            this.groupUtauCores = new org.kbinani.windows.forms.BGroupBox();
            this.lblResampler = new org.kbinani.windows.forms.BLabel();
            this.chkInvokeWithWine = new org.kbinani.windows.forms.BCheckBox();
            this.btnWavtool = new org.kbinani.windows.forms.BButton();
            this.txtResampler = new org.kbinani.windows.forms.BTextBox();
            this.lblWavtool = new org.kbinani.windows.forms.BLabel();
            this.btnResampler = new org.kbinani.windows.forms.BButton();
            this.txtWavtool = new org.kbinani.windows.forms.BTextBox();
            this.groupPlatform = new org.kbinani.windows.forms.BGroupBox();
            this.chkTranslateRoman = new org.kbinani.windows.forms.BCheckBox();
            this.comboPlatform = new org.kbinani.windows.forms.BComboBox();
            this.lblPlatform = new org.kbinani.windows.forms.BLabel();
            this.chkCommandKeyAsControl = new org.kbinani.windows.forms.BCheckBox();
            this.tabUtauSingers = new System.Windows.Forms.TabPage();
            this.btnRemove = new org.kbinani.windows.forms.BButton();
            this.btnAdd = new org.kbinani.windows.forms.BButton();
            this.btnUp = new org.kbinani.windows.forms.BButton();
            this.btnDown = new org.kbinani.windows.forms.BButton();
            this.listSingers = new org.kbinani.windows.forms.BListView();
            this.tabFile = new System.Windows.Forms.TabPage();
            this.chkKeepProjectCache = new org.kbinani.windows.forms.BCheckBox();
            this.lblAutoBackupMinutes = new org.kbinani.windows.forms.BLabel();
            this.lblAutoBackupInterval = new org.kbinani.windows.forms.BLabel();
            this.chkAutoBackup = new org.kbinani.windows.forms.BCheckBox();
            this.numAutoBackupInterval = new org.kbinani.cadencii.NumericUpDownEx();
            this.tabSingingSynth = new System.Windows.Forms.TabPage();
            this.groupSynthesizerDll = new org.kbinani.windows.forms.BGroupBox();
            this.chkLoadAquesTone = new org.kbinani.windows.forms.BCheckBox();
            this.chkLoadSecondaryVOCALOID1 = new org.kbinani.windows.forms.BCheckBox();
            this.chkLoadVocaloid2 = new org.kbinani.windows.forms.BCheckBox();
            this.chkLoadVocaloid101 = new org.kbinani.windows.forms.BCheckBox();
            this.chkLoadVocaloid100 = new org.kbinani.windows.forms.BCheckBox();
            this.groupVsti = new org.kbinani.windows.forms.BGroupBox();
            this.btnAquesTone = new org.kbinani.windows.forms.BButton();
            this.txtAquesTone = new org.kbinani.windows.forms.BTextBox();
            this.lblAquesTone = new org.kbinani.windows.forms.BLabel();
            this.txtVOCALOID2 = new org.kbinani.windows.forms.BTextBox();
            this.txtVOCALOID1 = new org.kbinani.windows.forms.BTextBox();
            this.lblVOCALOID2 = new org.kbinani.windows.forms.BLabel();
            this.lblVOCALOID1 = new org.kbinani.windows.forms.BLabel();
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.tabPreference.SuspendLayout();
            this.tabSequence.SuspendLayout();
            this.groupAutoVibratoConfig.SuspendLayout();
            this.tabAnother.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBuffer)).BeginInit();
            this.groupWaveFileOutput.SuspendLayout();
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
            this.groupPlatform.SuspendLayout();
            this.tabUtauSingers.SuspendLayout();
            this.tabFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoBackupInterval)).BeginInit();
            this.tabSingingSynth.SuspendLayout();
            this.groupSynthesizerDll.SuspendLayout();
            this.groupVsti.SuspendLayout();
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
            this.tabPreference.Controls.Add( this.tabSingingSynth );
            this.tabPreference.Location = new System.Drawing.Point( 7, 7 );
            this.tabPreference.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 3 );
            this.tabPreference.Multiline = true;
            this.tabPreference.Name = "tabPreference";
            this.tabPreference.SelectedIndex = 0;
            this.tabPreference.Size = new System.Drawing.Size( 462, 434 );
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
            this.tabSequence.Size = new System.Drawing.Size( 454, 392 );
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
            this.tabAnother.Controls.Add( this.bLabel2 );
            this.tabAnother.Controls.Add( this.numBuffer );
            this.tabAnother.Controls.Add( this.lblBuffer );
            this.tabAnother.Controls.Add( this.groupWaveFileOutput );
            this.tabAnother.Controls.Add( this.label13 );
            this.tabAnother.Controls.Add( this.label12 );
            this.tabAnother.Controls.Add( this.comboDefaultPremeasure );
            this.tabAnother.Controls.Add( this.comboDefualtSinger );
            this.tabAnother.Controls.Add( this.chkChasePastEvent );
            this.tabAnother.Controls.Add( this.lblDefaultPremeasure );
            this.tabAnother.Controls.Add( this.lblWait );
            this.tabAnother.Controls.Add( this.lblPreSendTime );
            this.tabAnother.Controls.Add( this.lblDefaultSinger );
            this.tabAnother.Controls.Add( this.numWait );
            this.tabAnother.Controls.Add( this.numPreSendTime );
            this.tabAnother.Location = new System.Drawing.Point( 4, 38 );
            this.tabAnother.Name = "tabAnother";
            this.tabAnother.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabAnother.Size = new System.Drawing.Size( 454, 392 );
            this.tabAnother.TabIndex = 2;
            this.tabAnother.Text = "Other Settings";
            this.tabAnother.UseVisualStyleBackColor = true;
            // 
            // bLabel2
            // 
            this.bLabel2.AutoSize = true;
            this.bLabel2.Location = new System.Drawing.Point( 305, 182 );
            this.bLabel2.Name = "bLabel2";
            this.bLabel2.Size = new System.Drawing.Size( 88, 12 );
            this.bLabel2.TabIndex = 30;
            this.bLabel2.Text = "msec(100-1000)";
            // 
            // numBuffer
            // 
            this.numBuffer.Location = new System.Drawing.Point( 216, 180 );
            this.numBuffer.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.numBuffer.Minimum = new decimal( new int[] {
            100,
            0,
            0,
            0} );
            this.numBuffer.Name = "numBuffer";
            this.numBuffer.Size = new System.Drawing.Size( 68, 19 );
            this.numBuffer.TabIndex = 31;
            this.numBuffer.Value = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            // 
            // lblBuffer
            // 
            this.lblBuffer.AutoSize = true;
            this.lblBuffer.Location = new System.Drawing.Point( 29, 182 );
            this.lblBuffer.Name = "lblBuffer";
            this.lblBuffer.Size = new System.Drawing.Size( 78, 12 );
            this.lblBuffer.TabIndex = 29;
            this.lblBuffer.Text = "Buffer Size(&B)";
            // 
            // groupWaveFileOutput
            // 
            this.groupWaveFileOutput.Controls.Add( this.radioCurrentTrack );
            this.groupWaveFileOutput.Controls.Add( this.radioMasterTrack );
            this.groupWaveFileOutput.Controls.Add( this.lblChannel );
            this.groupWaveFileOutput.Controls.Add( this.comboChannel );
            this.groupWaveFileOutput.Location = new System.Drawing.Point( 23, 215 );
            this.groupWaveFileOutput.Name = "groupWaveFileOutput";
            this.groupWaveFileOutput.Size = new System.Drawing.Size( 407, 100 );
            this.groupWaveFileOutput.TabIndex = 28;
            this.groupWaveFileOutput.TabStop = false;
            this.groupWaveFileOutput.Text = "Wave File Output";
            // 
            // radioCurrentTrack
            // 
            this.radioCurrentTrack.AutoSize = true;
            this.radioCurrentTrack.Checked = true;
            this.radioCurrentTrack.Location = new System.Drawing.Point( 155, 64 );
            this.radioCurrentTrack.Name = "radioCurrentTrack";
            this.radioCurrentTrack.Size = new System.Drawing.Size( 61, 16 );
            this.radioCurrentTrack.TabIndex = 29;
            this.radioCurrentTrack.TabStop = true;
            this.radioCurrentTrack.Text = "Current";
            this.radioCurrentTrack.UseVisualStyleBackColor = true;
            // 
            // radioMasterTrack
            // 
            this.radioMasterTrack.AutoSize = true;
            this.radioMasterTrack.Location = new System.Drawing.Point( 24, 64 );
            this.radioMasterTrack.Name = "radioMasterTrack";
            this.radioMasterTrack.Size = new System.Drawing.Size( 91, 16 );
            this.radioMasterTrack.TabIndex = 28;
            this.radioMasterTrack.Text = "Master Track";
            this.radioMasterTrack.UseVisualStyleBackColor = true;
            // 
            // lblChannel
            // 
            this.lblChannel.AutoSize = true;
            this.lblChannel.Location = new System.Drawing.Point( 22, 27 );
            this.lblChannel.Name = "lblChannel";
            this.lblChannel.Size = new System.Drawing.Size( 66, 12 );
            this.lblChannel.TabIndex = 25;
            this.lblChannel.Text = "Channel (&C)";
            // 
            // comboChannel
            // 
            this.comboChannel.FormattingEnabled = true;
            this.comboChannel.Items.AddRange( new object[] {
            "Mono",
            "Stereo"} );
            this.comboChannel.Location = new System.Drawing.Point( 113, 24 );
            this.comboChannel.Name = "comboChannel";
            this.comboChannel.Size = new System.Drawing.Size( 97, 20 );
            this.comboChannel.TabIndex = 27;
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
            this.tabAppearance.Size = new System.Drawing.Size( 454, 392 );
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
            this.groupVisibleCurve.Location = new System.Drawing.Point( 23, 173 );
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
            this.lblTrackHeight.Location = new System.Drawing.Point( 31, 141 );
            this.lblTrackHeight.Name = "lblTrackHeight";
            this.lblTrackHeight.Size = new System.Drawing.Size( 107, 12 );
            this.lblTrackHeight.TabIndex = 13;
            this.lblTrackHeight.Text = "Track Height (pixel)";
            // 
            // comboLanguage
            // 
            this.comboLanguage.FormattingEnabled = true;
            this.comboLanguage.Location = new System.Drawing.Point( 148, 105 );
            this.comboLanguage.Name = "comboLanguage";
            this.comboLanguage.Size = new System.Drawing.Size( 121, 20 );
            this.comboLanguage.TabIndex = 44;
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point( 31, 108 );
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size( 68, 12 );
            this.lblLanguage.TabIndex = 9;
            this.lblLanguage.Text = "UI Language";
            // 
            // numTrackHeight
            // 
            this.numTrackHeight.Location = new System.Drawing.Point( 210, 139 );
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
            this.tabOperation.Size = new System.Drawing.Size( 454, 392 );
            this.tabOperation.TabIndex = 5;
            this.tabOperation.Text = "Operation";
            this.tabOperation.UseVisualStyleBackColor = true;
            // 
            // groupMisc
            // 
            this.groupMisc.Controls.Add( this.comboMtcMidiInPortNumber );
            this.groupMisc.Controls.Add( this.labelMtcMidiInPort );
            this.groupMisc.Controls.Add( this.lblMaximumFrameRate );
            this.groupMisc.Controls.Add( this.comboMidiInPortNumber );
            this.groupMisc.Controls.Add( this.numMaximumFrameRate );
            this.groupMisc.Controls.Add( this.lblMidiInPort );
            this.groupMisc.Controls.Add( this.lblMouseHoverTime );
            this.groupMisc.Controls.Add( this.lblMilliSecond );
            this.groupMisc.Controls.Add( this.numMouseHoverTime );
            this.groupMisc.Location = new System.Drawing.Point( 23, 230 );
            this.groupMisc.Name = "groupMisc";
            this.groupMisc.Size = new System.Drawing.Size( 407, 143 );
            this.groupMisc.TabIndex = 91;
            this.groupMisc.TabStop = false;
            this.groupMisc.Text = "Misc";
            // 
            // comboMtcMidiInPortNumber
            // 
            this.comboMtcMidiInPortNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboMtcMidiInPortNumber.FormattingEnabled = true;
            this.comboMtcMidiInPortNumber.Location = new System.Drawing.Point( 176, 101 );
            this.comboMtcMidiInPortNumber.Name = "comboMtcMidiInPortNumber";
            this.comboMtcMidiInPortNumber.Size = new System.Drawing.Size( 225, 20 );
            this.comboMtcMidiInPortNumber.TabIndex = 91;
            // 
            // labelMtcMidiInPort
            // 
            this.labelMtcMidiInPort.AutoSize = true;
            this.labelMtcMidiInPort.Location = new System.Drawing.Point( 16, 104 );
            this.labelMtcMidiInPort.Name = "labelMtcMidiInPort";
            this.labelMtcMidiInPort.Size = new System.Drawing.Size( 137, 12 );
            this.labelMtcMidiInPort.TabIndex = 90;
            this.labelMtcMidiInPort.Text = "MTC MIDI In Port Number";
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
            this.comboMidiInPortNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboMidiInPortNumber.FormattingEnabled = true;
            this.comboMidiInPortNumber.Location = new System.Drawing.Point( 176, 74 );
            this.comboMidiInPortNumber.Name = "comboMidiInPortNumber";
            this.comboMidiInPortNumber.Size = new System.Drawing.Size( 225, 20 );
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
            this.groupPianoroll.Location = new System.Drawing.Point( 23, 9 );
            this.groupPianoroll.Name = "groupPianoroll";
            this.groupPianoroll.Size = new System.Drawing.Size( 407, 215 );
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
            this.tabPlatform.Controls.Add( this.groupPlatform );
            this.tabPlatform.Location = new System.Drawing.Point( 4, 38 );
            this.tabPlatform.Name = "tabPlatform";
            this.tabPlatform.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabPlatform.Size = new System.Drawing.Size( 454, 392 );
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
            this.groupUtauCores.Location = new System.Drawing.Point( 23, 123 );
            this.groupUtauCores.Name = "groupUtauCores";
            this.groupUtauCores.Size = new System.Drawing.Size( 407, 105 );
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
            this.btnWavtool.Location = new System.Drawing.Point( 360, 47 );
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
            this.txtResampler.Size = new System.Drawing.Size( 254, 19 );
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
            this.btnResampler.Location = new System.Drawing.Point( 360, 20 );
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
            this.txtWavtool.Size = new System.Drawing.Size( 254, 19 );
            this.txtWavtool.TabIndex = 111;
            // 
            // groupPlatform
            // 
            this.groupPlatform.Controls.Add( this.chkTranslateRoman );
            this.groupPlatform.Controls.Add( this.comboPlatform );
            this.groupPlatform.Controls.Add( this.lblPlatform );
            this.groupPlatform.Controls.Add( this.chkCommandKeyAsControl );
            this.groupPlatform.Location = new System.Drawing.Point( 23, 9 );
            this.groupPlatform.Name = "groupPlatform";
            this.groupPlatform.Size = new System.Drawing.Size( 407, 108 );
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
            this.tabUtauSingers.Size = new System.Drawing.Size( 454, 392 );
            this.tabUtauSingers.TabIndex = 6;
            this.tabUtauSingers.Text = "UTAU Singers";
            this.tabUtauSingers.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Location = new System.Drawing.Point( 98, 319 );
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size( 75, 23 );
            this.btnRemove.TabIndex = 122;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point( 17, 319 );
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size( 75, 23 );
            this.btnAdd.TabIndex = 121;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point( 279, 319 );
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size( 75, 23 );
            this.btnUp.TabIndex = 123;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point( 360, 319 );
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
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup2.Header = "ListViewGroup";
            listViewGroup2.Name = null;
            listViewGroup3.Header = "ListViewGroup";
            listViewGroup3.Name = null;
            listViewGroup4.Header = "ListViewGroup";
            listViewGroup4.Name = null;
            listViewGroup5.Header = "ListViewGroup";
            listViewGroup5.Name = null;
            listViewGroup6.Header = "ListViewGroup";
            listViewGroup6.Name = null;
            listViewGroup7.Header = "ListViewGroup";
            listViewGroup7.Name = null;
            listViewGroup8.Header = "ListViewGroup";
            listViewGroup8.Name = null;
            listViewGroup9.Header = "ListViewGroup";
            listViewGroup9.Name = null;
            listViewGroup10.Header = "ListViewGroup";
            listViewGroup10.Name = null;
            listViewGroup11.Header = "ListViewGroup";
            listViewGroup11.Name = null;
            listViewGroup12.Header = "ListViewGroup";
            listViewGroup12.Name = null;
            listViewGroup13.Header = "ListViewGroup";
            listViewGroup13.Name = null;
            listViewGroup14.Header = "ListViewGroup";
            listViewGroup14.Name = null;
            listViewGroup15.Header = "ListViewGroup";
            listViewGroup15.Name = null;
            this.listSingers.Groups.AddRange( new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6,
            listViewGroup7,
            listViewGroup8,
            listViewGroup9,
            listViewGroup10,
            listViewGroup11,
            listViewGroup12,
            listViewGroup13,
            listViewGroup14,
            listViewGroup15} );
            this.listSingers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listSingers.Location = new System.Drawing.Point( 17, 23 );
            this.listSingers.MultiSelect = false;
            this.listSingers.Name = "listSingers";
            this.listSingers.Size = new System.Drawing.Size( 418, 281 );
            this.listSingers.TabIndex = 120;
            this.listSingers.UseCompatibleStateImageBehavior = false;
            this.listSingers.View = System.Windows.Forms.View.Details;
            // 
            // tabFile
            // 
            this.tabFile.Controls.Add( this.chkKeepProjectCache );
            this.tabFile.Controls.Add( this.lblAutoBackupMinutes );
            this.tabFile.Controls.Add( this.lblAutoBackupInterval );
            this.tabFile.Controls.Add( this.chkAutoBackup );
            this.tabFile.Controls.Add( this.numAutoBackupInterval );
            this.tabFile.Location = new System.Drawing.Point( 4, 38 );
            this.tabFile.Name = "tabFile";
            this.tabFile.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabFile.Size = new System.Drawing.Size( 454, 392 );
            this.tabFile.TabIndex = 7;
            this.tabFile.Text = "File";
            this.tabFile.UseVisualStyleBackColor = true;
            // 
            // chkKeepProjectCache
            // 
            this.chkKeepProjectCache.AutoSize = true;
            this.chkKeepProjectCache.Location = new System.Drawing.Point( 23, 54 );
            this.chkKeepProjectCache.Name = "chkKeepProjectCache";
            this.chkKeepProjectCache.Size = new System.Drawing.Size( 125, 16 );
            this.chkKeepProjectCache.TabIndex = 4;
            this.chkKeepProjectCache.Text = "Keep Project Cache";
            this.chkKeepProjectCache.UseVisualStyleBackColor = true;
            // 
            // lblAutoBackupMinutes
            // 
            this.lblAutoBackupMinutes.AutoSize = true;
            this.lblAutoBackupMinutes.Location = new System.Drawing.Point( 331, 24 );
            this.lblAutoBackupMinutes.Name = "lblAutoBackupMinutes";
            this.lblAutoBackupMinutes.Size = new System.Drawing.Size( 53, 12 );
            this.lblAutoBackupMinutes.TabIndex = 3;
            this.lblAutoBackupMinutes.Text = "minute(s)";
            // 
            // lblAutoBackupInterval
            // 
            this.lblAutoBackupInterval.AutoSize = true;
            this.lblAutoBackupInterval.Location = new System.Drawing.Point( 207, 24 );
            this.lblAutoBackupInterval.Name = "lblAutoBackupInterval";
            this.lblAutoBackupInterval.Size = new System.Drawing.Size( 43, 12 );
            this.lblAutoBackupInterval.TabIndex = 1;
            this.lblAutoBackupInterval.Text = "interval";
            // 
            // chkAutoBackup
            // 
            this.chkAutoBackup.AutoSize = true;
            this.chkAutoBackup.Location = new System.Drawing.Point( 23, 23 );
            this.chkAutoBackup.Name = "chkAutoBackup";
            this.chkAutoBackup.Size = new System.Drawing.Size( 127, 16 );
            this.chkAutoBackup.TabIndex = 0;
            this.chkAutoBackup.Text = "Automatical Backup";
            this.chkAutoBackup.UseVisualStyleBackColor = true;
            // 
            // numAutoBackupInterval
            // 
            this.numAutoBackupInterval.Location = new System.Drawing.Point( 256, 22 );
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
            // tabSingingSynth
            // 
            this.tabSingingSynth.Controls.Add( this.groupSynthesizerDll );
            this.tabSingingSynth.Controls.Add( this.groupVsti );
            this.tabSingingSynth.Location = new System.Drawing.Point( 4, 38 );
            this.tabSingingSynth.Name = "tabSingingSynth";
            this.tabSingingSynth.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabSingingSynth.Size = new System.Drawing.Size( 454, 392 );
            this.tabSingingSynth.TabIndex = 8;
            this.tabSingingSynth.Text = "Synthesizer";
            this.tabSingingSynth.UseVisualStyleBackColor = true;
            // 
            // groupSynthesizerDll
            // 
            this.groupSynthesizerDll.Controls.Add( this.chkLoadAquesTone );
            this.groupSynthesizerDll.Controls.Add( this.chkLoadSecondaryVOCALOID1 );
            this.groupSynthesizerDll.Controls.Add( this.chkLoadVocaloid2 );
            this.groupSynthesizerDll.Controls.Add( this.chkLoadVocaloid101 );
            this.groupSynthesizerDll.Controls.Add( this.chkLoadVocaloid100 );
            this.groupSynthesizerDll.Location = new System.Drawing.Point( 23, 122 );
            this.groupSynthesizerDll.Name = "groupSynthesizerDll";
            this.groupSynthesizerDll.Size = new System.Drawing.Size( 407, 147 );
            this.groupSynthesizerDll.TabIndex = 109;
            this.groupSynthesizerDll.TabStop = false;
            this.groupSynthesizerDll.Text = "Synthesizer DLL Usage";
            // 
            // chkLoadAquesTone
            // 
            this.chkLoadAquesTone.AutoSize = true;
            this.chkLoadAquesTone.Location = new System.Drawing.Point( 63, 116 );
            this.chkLoadAquesTone.Name = "chkLoadAquesTone";
            this.chkLoadAquesTone.Size = new System.Drawing.Size( 81, 16 );
            this.chkLoadAquesTone.TabIndex = 116;
            this.chkLoadAquesTone.Text = "AquesTone";
            this.chkLoadAquesTone.UseVisualStyleBackColor = true;
            // 
            // chkLoadSecondaryVOCALOID1
            // 
            this.chkLoadSecondaryVOCALOID1.AutoSize = true;
            this.chkLoadSecondaryVOCALOID1.Location = new System.Drawing.Point( 18, 23 );
            this.chkLoadSecondaryVOCALOID1.Name = "chkLoadSecondaryVOCALOID1";
            this.chkLoadSecondaryVOCALOID1.Size = new System.Drawing.Size( 224, 16 );
            this.chkLoadSecondaryVOCALOID1.TabIndex = 1;
            this.chkLoadSecondaryVOCALOID1.Text = "Load secondary VOCALOID1 VSTi DLL";
            this.chkLoadSecondaryVOCALOID1.UseVisualStyleBackColor = true;
            // 
            // chkLoadVocaloid2
            // 
            this.chkLoadVocaloid2.AutoSize = true;
            this.chkLoadVocaloid2.Location = new System.Drawing.Point( 63, 95 );
            this.chkLoadVocaloid2.Name = "chkLoadVocaloid2";
            this.chkLoadVocaloid2.Size = new System.Drawing.Size( 87, 16 );
            this.chkLoadVocaloid2.TabIndex = 115;
            this.chkLoadVocaloid2.Text = "VOCALOID2";
            this.chkLoadVocaloid2.UseVisualStyleBackColor = true;
            // 
            // chkLoadVocaloid101
            // 
            this.chkLoadVocaloid101.AutoSize = true;
            this.chkLoadVocaloid101.Location = new System.Drawing.Point( 63, 73 );
            this.chkLoadVocaloid101.Name = "chkLoadVocaloid101";
            this.chkLoadVocaloid101.Size = new System.Drawing.Size( 113, 16 );
            this.chkLoadVocaloid101.TabIndex = 114;
            this.chkLoadVocaloid101.Text = "VOCALOID1 [1.1]";
            this.chkLoadVocaloid101.UseVisualStyleBackColor = true;
            // 
            // chkLoadVocaloid100
            // 
            this.chkLoadVocaloid100.AutoSize = true;
            this.chkLoadVocaloid100.Location = new System.Drawing.Point( 63, 51 );
            this.chkLoadVocaloid100.Name = "chkLoadVocaloid100";
            this.chkLoadVocaloid100.Size = new System.Drawing.Size( 113, 16 );
            this.chkLoadVocaloid100.TabIndex = 113;
            this.chkLoadVocaloid100.Text = "VOCALOID1 [1.0]";
            this.chkLoadVocaloid100.UseVisualStyleBackColor = true;
            // 
            // groupVsti
            // 
            this.groupVsti.Controls.Add( this.btnAquesTone );
            this.groupVsti.Controls.Add( this.txtAquesTone );
            this.groupVsti.Controls.Add( this.lblAquesTone );
            this.groupVsti.Controls.Add( this.txtVOCALOID2 );
            this.groupVsti.Controls.Add( this.txtVOCALOID1 );
            this.groupVsti.Controls.Add( this.lblVOCALOID2 );
            this.groupVsti.Controls.Add( this.lblVOCALOID1 );
            this.groupVsti.Location = new System.Drawing.Point( 23, 9 );
            this.groupVsti.Name = "groupVsti";
            this.groupVsti.Size = new System.Drawing.Size( 407, 107 );
            this.groupVsti.TabIndex = 110;
            this.groupVsti.TabStop = false;
            this.groupVsti.Text = "VST Instruments";
            // 
            // btnAquesTone
            // 
            this.btnAquesTone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAquesTone.Location = new System.Drawing.Point( 351, 69 );
            this.btnAquesTone.Name = "btnAquesTone";
            this.btnAquesTone.Size = new System.Drawing.Size( 41, 23 );
            this.btnAquesTone.TabIndex = 111;
            this.btnAquesTone.Text = "...";
            this.btnAquesTone.UseVisualStyleBackColor = true;
            // 
            // txtAquesTone
            // 
            this.txtAquesTone.Location = new System.Drawing.Point( 99, 71 );
            this.txtAquesTone.Name = "txtAquesTone";
            this.txtAquesTone.Size = new System.Drawing.Size( 246, 19 );
            this.txtAquesTone.TabIndex = 109;
            // 
            // lblAquesTone
            // 
            this.lblAquesTone.AutoSize = true;
            this.lblAquesTone.Location = new System.Drawing.Point( 16, 74 );
            this.lblAquesTone.Name = "lblAquesTone";
            this.lblAquesTone.Size = new System.Drawing.Size( 62, 12 );
            this.lblAquesTone.TabIndex = 108;
            this.lblAquesTone.Text = "AquesTone";
            // 
            // txtVOCALOID2
            // 
            this.txtVOCALOID2.Location = new System.Drawing.Point( 99, 46 );
            this.txtVOCALOID2.Name = "txtVOCALOID2";
            this.txtVOCALOID2.ReadOnly = true;
            this.txtVOCALOID2.Size = new System.Drawing.Size( 246, 19 );
            this.txtVOCALOID2.TabIndex = 107;
            // 
            // txtVOCALOID1
            // 
            this.txtVOCALOID1.Location = new System.Drawing.Point( 99, 21 );
            this.txtVOCALOID1.Name = "txtVOCALOID1";
            this.txtVOCALOID1.ReadOnly = true;
            this.txtVOCALOID1.Size = new System.Drawing.Size( 246, 19 );
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
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 374, 457 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 88, 23 );
            this.btnCancel.TabIndex = 201;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 280, 457 );
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
            this.ClientSize = new System.Drawing.Size( 475, 496 );
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
            ((System.ComponentModel.ISupportInitialize)(this.numBuffer)).EndInit();
            this.groupWaveFileOutput.ResumeLayout( false );
            this.groupWaveFileOutput.PerformLayout();
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
            this.groupPlatform.ResumeLayout( false );
            this.groupPlatform.PerformLayout();
            this.tabUtauSingers.ResumeLayout( false );
            this.tabFile.ResumeLayout( false );
            this.tabFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoBackupInterval)).EndInit();
            this.tabSingingSynth.ResumeLayout( false );
            this.groupSynthesizerDll.ResumeLayout( false );
            this.groupSynthesizerDll.PerformLayout();
            this.groupVsti.ResumeLayout( false );
            this.groupVsti.PerformLayout();
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
        private BComboBox comboDefualtSinger;
        private BLabel label12;
        private BComboBox comboDefaultPremeasure;
        private NumericUpDownEx numWait;
        private NumericUpDownEx numPreSendTime;
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
        private BLabel lblChannel;
        private BComboBox comboChannel;
        private BGroupBox groupWaveFileOutput;
        private BRadioButton radioCurrentTrack;
        private BRadioButton radioMasterTrack;
        private BComboBox comboMtcMidiInPortNumber;
        private BLabel labelMtcMidiInPort;
        private BCheckBox chkKeepProjectCache;
        private System.Windows.Forms.TabPage tabSingingSynth;
        private BCheckBox chkLoadSecondaryVOCALOID1;
        private BGroupBox groupSynthesizerDll;
        private BCheckBox chkLoadVocaloid100;
        private BGroupBox groupVsti;
        private BButton btnAquesTone;
        private BTextBox txtAquesTone;
        private BLabel lblAquesTone;
        private BTextBox txtVOCALOID2;
        private BTextBox txtVOCALOID1;
        private BLabel lblVOCALOID2;
        private BLabel lblVOCALOID1;
        private BCheckBox chkLoadAquesTone;
        private BCheckBox chkLoadVocaloid2;
        private BCheckBox chkLoadVocaloid101;
        private BLabel bLabel2;
        private NumericUpDownEx numBuffer;
        private BLabel lblBuffer;
        #endregion
#endif
        #endregion

    }

#if !JAVA
}
#endif
