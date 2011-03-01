/*
 * Preference.cs
 * Copyright © 2008-2011 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/Cadencii/Preference.java

import java.awt.*;
import java.awt.event.*;
import java.util.*;
import java.io.*;
import javax.sound.midi.MidiDevice;
import javax.sound.midi.MidiSystem;
import javax.sound.midi.Receiver;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.media.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.apputil;
using org.kbinani.java.awt;
using org.kbinani.java.awt.event_;
using org.kbinani.java.io;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
    using BEventArgs = System.EventArgs;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
    using BEventHandler = System.EventHandler;
    using BFormClosingEventHandler = System.Windows.Forms.FormClosingEventHandler;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class Preference extends BDialog {
#else
    class Preference : BDialog
    {
#endif
        private static int columnWidthHeaderProgramChange = 60;
        private static int columnWidthHeaderName = 100;
        private static int columnWidthHeaderPath = 250;

        Font m_base_font;
        Font m_screen_font;
        Vector<String> m_program_change = null;
        private PlatformEnum m_platform = PlatformEnum.Windows;
        private Vector<SingerConfig> m_utau_singers = new Vector<SingerConfig>();

        private BFileChooser openUtauCore;
        private BFontChooser fontDialog;
#if JAVA
        private BFileChooser folderBrowserSingers;
#else
#if DEBUG
        private BFileChooser folderBrowserSingers;
#else
        private BFolderBrowser folderBrowserSingers;
#endif
#endif

        public Preference()
        {
#if JAVA
            //INCLUDE-SECTION CTOR ../BuildJavaUI/src/org/kbinani/Cadencii/Preference.java
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
            openUtauCore = new BFileChooser();

#if JAVA
            folderBrowserSingers = new BFileChooser();
#else
#if DEBUG
            folderBrowserSingers = new BFileChooser();
#else
            folderBrowserSingers = new BFolderBrowser();
            folderBrowserSingers.setNewFolderButtonVisible( false );
#endif
#endif
            applyLanguage();
#if JAVA
            listResampler.setColumnWidth( 0, 258 );
#endif

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

            txtAutoVibratoThresholdLength.setText( "480" );

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

            updateCustomVibrato();

            comboResolControlCurve.removeAllItems();
            for ( Iterator<ClockResolution> itr = ClockResolutionUtility.iterator(); itr.hasNext(); ) {
                ClockResolution cr = itr.next();
                comboResolControlCurve.addItem( ClockResolutionUtility.toString( cr ) );
            }
            comboResolControlCurve.setSelectedIndex( 0 );

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

            updateMidiDevice();

            txtVOCALOID1.setText( VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID1 ) );
            txtVOCALOID2.setText( VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 ) );

            listSingers.setColumnWidth( 0, columnWidthHeaderProgramChange );
            listSingers.setColumnWidth( 1, columnWidthHeaderName );
            listSingers.setColumnWidth( 2, columnWidthHeaderPath );

            // default synthesizer
            comboDefaultSynthesizer.removeAllItems();
            Vector<String> added = new Vector<String>();
#if JAVA
            for( RendererKind p : RendererKind.values() ) {
#else
            foreach ( RendererKind p in Enum.GetValues( typeof( RendererKind ) ) ) {
#endif
                if ( p == RendererKind.NULL ) {
                    continue;
                }
                String s = RendererKindUtil.getString( p );
                if ( added.contains( s ) ) {
                    continue;
                }
                added.add( s );
                comboDefaultSynthesizer.addItem( s );
            }
            comboDefaultSynthesizer.setSelectedIndex( 0 );

            numBuffer.setMaximum( EditorConfig.MAX_BUFFER_MILLISEC );
            numBuffer.setMinimum( EditorConfig.MIN_BUFFER_MILLIXEC );

            registerEventHandlers();
            setResources();
        }

        #region public methods
#if JAVA
        public override BDialogResult showDialog( Component parent )
#else
        public override BDialogResult showDialog( System.Windows.Forms.Form parent )
#endif
        {
            updateMidiDevice();
            updateCustomVibrato();
            return base.showDialog( parent );
        }

        /// <summary>
        /// WINEPREFIXの設定値を取得します
        /// </summary>
        public String getWinePrefix()
        {
            return textWinePrefix.getText();
        }
        
        /// <summary>
        /// WINEPREFIXの設定値を設定します
        /// </summary>
        public void setWinePrefix( String value )
        {
            textWinePrefix.setText( value );
        }
        
        /// <summary>
        /// WINETOPの設定値を取得します
        /// </summary>
        public String getWineTop()
        {
            return textWineTop.getText();
        }

        /// <summary>
        /// WINETOPの設定値を設定します
        /// </summary>
        public void setWineTop( String value )
        {
            textWineTop.setText( value );
        }

        /// <summary>
        /// Cadencii付属のWineを使うかどうかを表す設定値を取得します
        /// </summary>
        public boolean isWineBuiltin()
        {
            return radioWineBuiltin.isSelected();
        }

        /// <summary>
        /// Cadencii付属のWineを使うかどうかを表す設定値を設定します
        /// </summary>        
        public void setWineBuiltin( boolean value )
        {
            radioWineBuiltin.setSelected( value );
            radioWineCustom.setSelected( !value );
        }

        /// <summary>
        /// 自動ビブラートを作成するとき，ユーザー定義タイプのビブラートを利用するかどうか，を表す値を取得します
        /// </summary>
        /// <returns></returns>
        public boolean isUseUserDefinedAutoVibratoType()
        {
            return radioUserDefined.isSelected();
        }

        /// <summary>
        /// 自動ビブラートを作成するとき，ユーザー定義タイプのビブラートを利用するかどうか，を表す値を設定します
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void setUseUserDefinedAutoVibratoType( boolean value )
        {
            radioUserDefined.setSelected( value );
            radioVocaloidEditorCompatible.setSelected( !value );
        }

        /// <summary>
        /// デフォルトの音声合成システムを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setDefaultSynthesizer( RendererKind value )
        {
            int c = comboDefaultSynthesizer.getItemCount();
            int select_indx = 0;
            for ( int i = 0; i < c; i++ ) {
                String str = (String)comboDefaultSynthesizer.getItemAt( i );
                RendererKind p = RendererKindUtil.fromString( str );
                if ( p == value ) {
                    select_indx = i;
                    break;
                }
            }
            comboDefaultSynthesizer.setSelectedIndex( select_indx );
        }

        /// <summary>
        /// デフォルトの音声合成システムを取得します
        /// </summary>
        /// <returns></returns>
        public RendererKind getDefaultSynthesizer()
        {
            String selstr = (String)comboDefaultSynthesizer.getSelectedItem();
#if JAVA
            for( RendererKind p : RendererKind.values() ){
#else
            foreach ( RendererKind p in Enum.GetValues( typeof( RendererKind ) ) ) {
#endif
                String str = RendererKindUtil.getString( p );
                if ( str.Equals( selstr ) ) {
                    return p;
                }
            }
            return RendererKind.VOCALOID2;
        }

        /// <summary>
        /// バッファーサイズの設定値（単位：ミリ秒）を取得します。
        /// </summary>
        /// <returns></returns>
        public int getBufferSize()
        {
            return (int)numBuffer.getFloatValue();
        }

        /// <summary>
        /// バッファーサイズの設定値（単位：ミリ秒）を設定します。
        /// </summary>
        /// <param name="value"></param>
        public void setBufferSize( int value )
        {
            if ( value < numBuffer.getMinimum() ) {
                value = (int)numBuffer.getMinimum();
            } else if ( numBuffer.getMaximum() < value ) {
                value = (int)numBuffer.getMaximum();
            }
            numBuffer.setFloatValue( value );
        }

        /// <summary>
        /// VOCALOID1DLLを読み込むかどうかを表すブール値を取得します
        /// </summary>
        /// <returns></returns>
        public boolean isVocaloid1Required()
        {
            if ( chkLoadVocaloid1.isEnabled() ) {
                return chkLoadVocaloid1.isSelected();
            } else {
                return false;
            }
        }

        /// <summary>
        /// VOCALOID1DLLを読み込むかどうかを表すブール値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setVocaloid1Required( boolean value )
        {
            if ( chkLoadVocaloid1.isEnabled() ) {
                chkLoadVocaloid1.setSelected( value );
            }
        }

        /// <summary>
        /// VOCALOID2 DLLを読み込むかどうかを表すブール値を取得します
        /// </summary>
        /// <returns></returns>
        public boolean isVocaloid2Required()
        {
            return chkLoadVocaloid2.isSelected();
        }

        /// <summary>
        /// VOCALOID2 DLLを読み込むかどうかを表すブール値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setVocaloid2Required( boolean value )
        {
            chkLoadVocaloid2.setSelected( value );
        }

        /// <summary>
        /// AquesTone DLLを読み込むかどうかを表すブール値を取得します
        /// </summary>
        /// <returns></returns>
        public boolean isAquesToneRequired()
        {
            return chkLoadAquesTone.isSelected();
        }

        /// <summary>
        /// AquesTone DLLを読み込むかどうかを表すブール値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setAquesToneRequired( boolean value )
        {
            chkLoadAquesTone.setSelected( value );
        }

        public boolean isUseProjectCache()
        {
            return chkKeepProjectCache.isSelected();
        }

        public void setUseProjectCache( boolean value )
        {
            chkKeepProjectCache.setSelected( value );
        }

        public boolean isUseSpaceKeyAsMiddleButtonModifier()
        {
            return chkUseSpaceKeyAsMiddleButtonModifier.isSelected();
        }

        public void setUseSpaceKeyAsMiddleButtonModifier( boolean value )
        {
            chkUseSpaceKeyAsMiddleButtonModifier.setSelected( value );
        }

        public int getAutoBackupIntervalMinutes()
        {
            if ( chkAutoBackup.isSelected() ) {
                return (int)numAutoBackupInterval.getFloatValue();
            } else {
                return 0;
            }
        }

        public void setAutoBackupIntervalMinutes( int value )
        {
            if ( value <= 0 ) {
                chkAutoBackup.setSelected( false );
            } else {
                chkAutoBackup.setSelected( true );
                numAutoBackupInterval.setFloatValue( value );
            }
        }

        public boolean isSelfDeRomantization()
        {
            return chkTranslateRoman.isSelected();
        }

        public void setSelfDeRomantization( boolean value )
        {
            chkTranslateRoman.setSelected( value );
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
        public int getMidiInPort()
        {
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
        public void setMidiInPort( int value )
        {
            if ( comboMidiInPortNumber.isEnabled() ) {
                if ( 0 <= value && value < comboMidiInPortNumber.getItemCount() ) {
                    comboMidiInPortNumber.setSelectedIndex( value );
                } else {
                    comboMidiInPortNumber.setSelectedIndex( 0 );
                }
            }
        }
#endif

        public boolean isCurveVisibleVel()
        {
            return chkVel.isSelected();
        }

        public void setCurveVisibleVel( boolean value )
        {
            chkVel.setSelected( value );
        }

        public boolean isCurveVisibleAccent()
        {
            return chkAccent.isSelected();
        }

        public void setCurveVisibleAccent( boolean value )
        {
            chkAccent.setSelected( value );
        }

        public boolean isCurveVisibleDecay()
        {
            return chkDecay.isSelected();
        }

        public void setCurveVisibleDecay( boolean value )
        {
            chkDecay.setSelected( value );
        }

        public boolean isCurveVisibleVibratoRate()
        {
            return chkVibratoRate.isSelected();
        }

        public void setCurveVisibleVibratoRate( boolean value )
        {
            chkVibratoRate.setSelected( value );
        }

        public boolean isCurveVisibleVibratoDepth()
        {
            return chkVibratoDepth.isSelected();
        }

        public void setCurveVisibleVibratoDepth( boolean value )
        {
            chkVibratoDepth.setSelected( value );
        }

        public boolean isCurveVisibleDyn()
        {
            return chkDyn.isSelected();
        }

        public void setCurveVisibleDyn( boolean value )
        {
            chkDyn.setSelected( value );
        }

        public boolean isCurveVisibleBre()
        {
            return chkBre.isSelected();
        }

        public void setCurveVisibleBre( boolean value )
        {
            chkBre.setSelected( value );
        }

        public boolean isCurveVisibleBri()
        {
            return chkBri.isSelected();
        }

        public void setCurveVisibleBri( boolean value )
        {
            chkBri.setSelected( value );
        }

        public boolean isCurveVisibleCle()
        {
            return chkCle.isSelected();
        }

        public void setCurveVisibleCle( boolean value )
        {
            chkCle.setSelected( value );
        }

        public boolean isCurveVisibleOpe()
        {
            return chkOpe.isSelected();
        }

        public void setCurveVisibleOpe( boolean value )
        {
            chkOpe.setSelected( value );
        }

        public boolean isCurveVisiblePor()
        {
            return chkPor.isSelected();
        }

        public void setCurveVisiblePor( boolean value )
        {
            chkPor.setSelected( value );
        }

        public boolean isCurveVisibleGen()
        {
            return chkGen.isSelected();
        }

        public void setCurveVisibleGen( boolean value )
        {
            chkGen.setSelected( value );
        }

        public boolean isCurveVisiblePit()
        {
            return chkPit.isSelected();
        }

        public void setCurveVisiblePit( boolean value )
        {
            chkPit.setSelected( value );
        }

        public boolean isCurveVisiblePbs()
        {
            return chkPbs.isSelected();
        }

        public void setCurveVisiblePbs( boolean value )
        {
            chkPbs.setSelected( value );
        }

        public boolean isCurveVisibleFx2Depth()
        {
            return chkFx2Depth.isSelected();
        }

        public void setCurveVisibleFx2Depth( boolean value )
        {
            chkFx2Depth.setSelected( value );
        }

        public boolean isCurveVisibleHarmonics()
        {
            return chkHarmonics.isSelected();
        }

        public void setCurveVisibleHarmonics( boolean value )
        {
            chkHarmonics.setSelected( value );
        }

        public boolean isCurveVisibleReso1()
        {
            return chkReso1.isSelected();
        }

        public void setCurveVisibleReso1( boolean value )
        {
            chkReso1.setSelected( value );
        }

        public boolean isCurveVisibleReso2()
        {
            return chkReso2.isSelected();
        }

        public void setCurveVisibleReso2( boolean value )
        {
            chkReso2.setSelected( value );
        }

        public boolean isCurveVisibleReso3()
        {
            return chkReso3.isSelected();
        }

        public void setCurveVisibleReso3( boolean value )
        {
            chkReso3.setSelected( value );
        }

        public boolean isCurveVisibleReso4()
        {
            return chkReso4.isSelected();
        }

        public void setCurveVisibleReso4( boolean value )
        {
            chkReso4.setSelected( value );
        }

        public boolean isCurveVisibleEnvelope()
        {
            return chkEnvelope.isSelected();
        }

        public void setCurveVisibleEnvelope( boolean value )
        {
            chkEnvelope.setSelected( value );
        }

        public boolean isCurveSelectingQuantized()
        {
            return chkCurveSelectingQuantized.isSelected();
        }

        public void setCurveSelectingQuantized( boolean value )
        {
            chkCurveSelectingQuantized.setSelected( value );
        }

        public boolean isPlayPreviewWhenRightClick()
        {
            return chkPlayPreviewWhenRightClick.isSelected();
        }

        public void setPlayPreviewWhenRightClick( boolean value )
        {
            chkPlayPreviewWhenRightClick.setSelected( value );
        }

        public int getMouseHoverTime()
        {
            return (int)numMouseHoverTime.getFloatValue();
        }

        public void setMouseHoverTime( int value )
        {
            numMouseHoverTime.setFloatValue( value );
        }

        public int getPxTrackHeight()
        {
            return (int)numTrackHeight.getFloatValue();
        }

        public void setPxTrackHeight( int value )
        {
            numTrackHeight.setFloatValue( value );
        }

        public boolean isKeepLyricInputMode()
        {
            return chkKeepLyricInputMode.isSelected();
        }

        public void setKeepLyricInputMode( boolean value )
        {
            chkKeepLyricInputMode.setSelected( value );
        }

        public int getMaximumFrameRate()
        {
            return (int)numMaximumFrameRate.getFloatValue();
        }

        public void setMaximumFrameRate( int value )
        {
            numMaximumFrameRate.setFloatValue( value );
        }

        public boolean isScrollHorizontalOnWheel()
        {
            return chkScrollHorizontal.isSelected();
        }

        public void setScrollHorizontalOnWheel( boolean value )
        {
            chkScrollHorizontal.setSelected( value );
        }

        public void applyLanguage()
        {
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

#if JAVA
            folderBrowserSingers.setDialogTitle( _( "Select Singer Directory" ) );
#else
#if DEBUG
            folderBrowserSingers.setDialogTitle( _( "Select Singer Directory" ) );
#else
            folderBrowserSingers.setDescription( _( "Select Singer Directory" ) );
#endif
#endif

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
                    tabPane.setTitleAt( i, _( "Other" ) );
                }else if( p == tabAppearance ){
                    tabPane.setTitleAt( i, _( "Appearance" ) );
                }else if( p == tabOperation ){
                    tabPane.setTitleAt( i, _( "Operation" ) );
                }else if( p == tabPlatform ){
                    tabPane.setTitleAt( i, _( "Platform" ) );
                }else if( p == tabUtausingers ){
                    tabPane.setTitleAt( i, _( "UTAU Singers" ) );
                }else if( p == tabFile ){
                    tabPane.setTitleAt( i, _( "File" ) );
                }else if( p == tabSingingSynth ){
                    tabPane.setTitleAt( i, _( "Synthesizer" ) );
                }
            }
#else
            tabSequence.Text = _( "Sequence" );
            tabAnother.Text = _( "Other" );
            tabAppearance.Text = _( "Appearance" );
            tabOperation.Text = _( "Operation" );
            tabPlatform.Text = _( "Platform" );
            tabUtausingers.Text = _( "UTAU Singers" );
            tabFile.Text = _( "File" );
            tabSingingSynth.Text = _( "Synthesizer" );
#endif
            #endregion

            #region tabSequence
            lblResolution.setText( _( "Resolution(VSTi)" ) );
            lblResolControlCurve.setText( _( "Control Curve" ) );
            lblResolControlCurve.setMnemonic( KeyEvent.VK_C, comboResolControlCurve );

            chkEnableAutoVibrato.setText( _( "Enable Automatic Vibrato" ) );
            chkEnableAutoVibrato.setMnemonic( KeyEvent.VK_E );
            lblVibratoLength.setText( _( "Default Vibrato Length" ) );
            lblVibratoLength.setMnemonic( KeyEvent.VK_L, comboVibratoLength );
            lblAutoVibratoThresholdLength.setText( _( "Minimum note length for Automatic Vibrato" ) );
            lblAutoVibratoThresholdLength.setMnemonic( KeyEvent.VK_M, txtAutoVibratoThresholdLength );

            lblAutoVibratoType.setText( _( "Auto Vibrato Type" ) );
            groupVocaloidEditorCompatible.setTitle( _( "VOCALOID Editor Compatible" ) );
            groupUserDefined.setTitle( _( "User Defined" ) );
            radioVocaloidEditorCompatible.setText( _( "VOCALOID Editor Compatible" ) );
            radioUserDefined.setText( _( "User Defined" ) );
            chkEnableAutoVibrato.setText( _( "Enable Automatic Vibrato" ) );
            chkEnableAutoVibrato.setMnemonic( KeyEvent.VK_E );
            lblAutoVibratoType1.setText( _( "Vibrato Type" ) + ": VOCALOID1" );
            lblAutoVibratoType1.setMnemonic( KeyEvent.VK_T, comboAutoVibratoType1 );
            lblAutoVibratoType2.setText( _( "Vibrato Type" ) + ": VOCALOID2" );
            lblAutoVibratoType2.setMnemonic( KeyEvent.VK_T, comboAutoVibratoType2 );
            #endregion

            #region tabAnother
            lblDefaultSinger.setText( _( "Default Singer" ) );
            lblDefaultSinger.setMnemonic( KeyEvent.VK_S, comboDefualtSinger );
            lblPreSendTime.setText( _( "Pre-Send time" ) );
            lblPreSendTime.setMnemonic( KeyEvent.VK_P, numPreSendTime );
            lblWait.setText( _( "Waiting Time" ) );
            lblWait.setMnemonic( KeyEvent.VK_W, numWait );
            chkChasePastEvent.setText( _( "Chase Event" ) );
            chkChasePastEvent.setMnemonic( KeyEvent.VK_C );
            lblBuffer.setText( _( "Buffer Size" ) );
            lblBuffer.setMnemonic( KeyEvent.VK_B, numBuffer );
            lblBufferSize.setText( "msec(" + EditorConfig.MIN_BUFFER_MILLIXEC + "-" + EditorConfig.MAX_BUFFER_MILLISEC + ")" );
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
            groupPianoroll.setTitle( _( "Piano Roll" ) );
            labelWheelOrder.setText( _( "Mouse wheel Rate" ) );

            chkCursorFix.setText( _( "Fix Play Cursor to Center" ) );
            chkScrollHorizontal.setText( _( "Horizontal Scroll when Mouse wheel" ) );
            chkKeepLyricInputMode.setText( _( "Keep Lyric Input Mode" ) );
            chkPlayPreviewWhenRightClick.setText( _( "Play Preview On Right Click" ) );
            chkCurveSelectingQuantized.setText( _( "Enable Quantize for Curve Selecting" ) );
            chkUseSpaceKeyAsMiddleButtonModifier.setText( _( "Use space key as Middle button modifier" ) );

            groupMisc.setTitle( _( "Misc" ) );
            lblMaximumFrameRate.setText( _( "Maximum Frame Rate" ) );
            lblMilliSecond.setText( _( "milli second" ) );
            lblMouseHoverTime.setText( _( "Waiting Time for Preview" ) );
            lblMidiInPort.setText( _( "MIDI In Port Number" ) );
            labelMtcMidiInPort.setText( _( "MTC MIDI In Port Number" ) );
            chkTranslateRoman.setText( _( "Translate Roman letters into Kana" ) );
            #endregion

            #region tabPlatform
            groupUtauCores.setTitle( _( "UTAU Cores" ) );
            labelWavtoolPath.setText( _( "Path:" ) );
            chkWavtoolWithWine.setText( _( "Invoke wavtool with Wine" ) );
            listResampler.setColumnHeaders( new String[] { _( "path" ) } );
            labelResamplerWithWine.setText( _( "Check the box to use Wine" ) );
            #endregion

            #region tabUtausingers
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

            groupDefaultSynthesizer.setTitle( _( "Default Synthesizer" ) );
            #endregion
        }

        public String getLanguage()
        {
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

        /// <summary>
        /// コントロールカーブの時間解像度の設定値を取得します
        /// </summary>
        /// <returns>コントロールカーブの時間解像度の設定値</returns>
        public ClockResolution getControlCurveResolution()
        {
            int count = -1;
            int index = comboResolControlCurve.getSelectedIndex();
            for ( Iterator<ClockResolution> itr = ClockResolutionUtility.iterator(); itr.hasNext(); ) {
                ClockResolution vt = itr.next();
                count++;
                if ( count == index ) {
                    return vt;
                }
            }
            comboResolControlCurve.setSelectedIndex( 0 );
            return ClockResolution.L30;
        }

        /// <summary>
        /// コントロールカーブの時間解像度の設定値を設定します
        /// </summary>
        /// <param name="value">設定する時間解像度</param>
        public void setControlCurveResolution( ClockResolution value )
        {
            int count = -1;
            for ( Iterator<ClockResolution> itr = ClockResolutionUtility.iterator(); itr.hasNext(); ) {
                ClockResolution vt = itr.next();
                count++;
                if ( vt.Equals( value ) ) {
                    comboResolControlCurve.setSelectedIndex( count );
                    break;
                }
            }
        }

        public int getPreSendTime()
        {
            return (int)numPreSendTime.getFloatValue();
        }

        public void setPreSendTime( int value )
        {
            numPreSendTime.setFloatValue( value );
        }

        public boolean isEnableAutoVibrato()
        {
            return chkEnableAutoVibrato.isSelected();
        }

        public void setEnableAutoVibrato( boolean value )
        {
            chkEnableAutoVibrato.setSelected( value );
        }

        public String getAutoVibratoType1()
        {
            int count = -1;
            int index = comboAutoVibratoType1.getSelectedIndex();
            if ( 0 <= index ) {
                VibratoHandle vconfig = (VibratoHandle)comboAutoVibratoType1.getSelectedItem();
                return vconfig.IconID;
            } else {
                return "$04040001";
            }
        }

        public void setAutoVibratoType1( String value )
        {
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

        public String getAutoVibratoType2()
        {
            int count = -1;
            int index = comboAutoVibratoType2.getSelectedIndex();
            if ( 0 <= index ) {
                VibratoHandle vconfig = (VibratoHandle)comboAutoVibratoType2.getSelectedItem();
                return vconfig.IconID;
            } else {
                return "$04040001";
            }
        }

        public void setAutoVibratoType2( String value )
        {
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

        public String getAutoVibratoTypeCustom()
        {
            int count = -1;
            int index = comboAutoVibratoTypeCustom.getSelectedIndex();
            if ( 0 <= index ) {
                VibratoHandle vconfig = (VibratoHandle)comboAutoVibratoTypeCustom.getSelectedItem();
                return vconfig.IconID;
            } else {
                return "$04040001";
            }
        }

        public void setAutoVibratoTypeCustom( String icon_id )
        {
            for ( int i = 0; i < comboAutoVibratoTypeCustom.getItemCount(); i++ ) {
                VibratoHandle handle = (VibratoHandle)comboAutoVibratoTypeCustom.getItemAt( i );
                if ( handle.IconID.Equals( icon_id ) ) {
                    comboAutoVibratoTypeCustom.setSelectedIndex( i );
                    return;
                }
            }
        }

        public int getAutoVibratoThresholdLength()
        {
            try {
                int ret = str.toi( txtAutoVibratoThresholdLength.getText() );
                if ( ret < 0 ) {
                    ret = 0;
                }
                return ret;
            } catch ( Exception ex ) {
                return 480;
            }
        }

        public void setAutoVibratoThresholdLength( int value )
        {
            if ( value < 0 ) {
                value = 0;
            }
            txtAutoVibratoThresholdLength.setText( value + "" );
        }

        public DefaultVibratoLengthEnum getDefaultVibratoLength()
        {
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

        public void setDefaultVibratoLength( DefaultVibratoLengthEnum value )
        {
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

        public boolean isCursorFixed()
        {
            return chkCursorFix.isSelected();
        }

        public void setCursorFixed( boolean value )
        {
            chkCursorFix.setSelected( value );
        }

        public int getWheelOrder()
        {
            return (int)numericUpDownEx1.getFloatValue();
        }

        public void setWheelOrder( int value )
        {
            if ( value < numericUpDownEx1.getMinimum() ) {
                numericUpDownEx1.setFloatValue( numericUpDownEx1.getMinimum() );
            } else if ( numericUpDownEx1.getMaximum() < value ) {
                numericUpDownEx1.setFloatValue( numericUpDownEx1.getMaximum() );
            } else {
                numericUpDownEx1.setFloatValue( value );
            }
        }

        public Font getScreenFont()
        {
            return m_screen_font;
        }

        public void setScreenFont( Font value )
        {
            m_screen_font = value;
            labelScreenFontName.setText( m_screen_font.getName() );
        }

        public java.awt.Font getBaseFont()
        {
            return m_base_font;
        }

        public void setBaseFont( java.awt.Font value )
        {
            m_base_font = value;
            labelMenuFontName.setText( m_base_font.getName() );
            UpdateFonts( m_base_font.getName() );
        }

        public String getDefaultSingerName()
        {
            if ( comboDefualtSinger.getSelectedIndex() >= 0 ) {
                return m_program_change.get( comboDefualtSinger.getSelectedIndex() );
            } else {
                return "Miku";
            }
        }

        public void setDefaultSingerName( String value )
        {
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

        public void copyResamplersConfig( Vector<String> ret, Vector<Boolean> with_wine )
        {
            for ( int i = 0; i < listResampler.getItemCountRow(); i++ ) {
                ret.add( (String)listResampler.getItemAt( i, 0 ) );
                with_wine.add( listResampler.isRowChecked( i ) );
            }
        }

        public void setResamplersConfig( Vector<String> path, Vector<Boolean> with_wine )
        {
            int size = listResampler.getItemCountRow();
            for ( int i = 0; i < size; i++ ) {
                listResampler.removeRow( 0 );
            }
            if ( path == null ) {
                return;
            }
            for ( int i = 0; i < vec.size( path ); i++ ) {
                listResampler.addRow(
                    new String[]{ vec.get( path, i ) }, vec.get( with_wine, i ) );
            }
        }

        public void setWavtoolWithWine( boolean value )
        {
            chkWavtoolWithWine.setSelected( value );
        }

        public boolean isWavtoolWithWine()
        {
            return chkWavtoolWithWine.isSelected();
        }

        public String getPathWavtool()
        {
            return txtWavtool.getText();
        }

        public void setPathWavtool( String value )
        {
            txtWavtool.setText( value );
        }

        public String getPathAquesTone()
        {
            return txtAquesTone.getText();
        }

        public void setPathAquesTone( String value )
        {
            txtAquesTone.setText( value );
        }

        public Vector<SingerConfig> getUtausingers()
        {
            return m_utau_singers;
        }

        public void setUtausingers( Vector<SingerConfig> value )
        {
            m_utau_singers.clear();
            for ( int i = 0; i < value.size(); i++ ) {
                m_utau_singers.add( (SingerConfig)value.get( i ).clone() );
            }
            UpdateUtausingerList();
        }
        #endregion

        #region event handlers
        public void btnChangeMenuFont_Click( Object sender, BEventArgs e )
        {
            fontDialog.setSelectedFont( getBaseFont() );
            fontDialog.setVisible( true );
            if ( fontDialog.getDialogResult() == BDialogResult.OK ) {
                java.awt.Font f = fontDialog.getSelectedFont();
                if ( f != null ) {
                    m_base_font = f;
                    labelMenuFontName.setText( f.getName() );
                }
            }
        }

        public void btnOK_Click( Object sender, BEventArgs e )
        {
            boolean was_modified = false;
#if !JAVA
            if ( AppManager.editorConfig.DoNotUseVocaloid2 != (!isVocaloid2Required()) ) {
                was_modified = true;
            }
            if ( AppManager.editorConfig.DoNotUseVocaloid1 != (!isVocaloid1Required()) ) {
                was_modified = true;
            }
#endif
            if ( was_modified ) {
                AppManager.showMessageBox( _( "Restart Cadencii to complete your changes\n(restart will NOT be automatically done)" ),
                                           "Cadencii",
                                           org.kbinani.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                                           org.kbinani.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE );
            }

            setDialogResult( BDialogResult.OK );
        }

        public void btnChangeScreenFont_Click( Object sender, BEventArgs e )
        {
            fontDialog.setSelectedFont( m_screen_font );
            fontDialog.setVisible( true );
            if ( fontDialog.getDialogResult() == BDialogResult.OK ) {
                java.awt.Font f = fontDialog.getSelectedFont();
                if ( f != null ) {
                    m_screen_font = f;
                    labelScreenFontName.setText( f.getName() );
                }
            }
        }

        public void buttonResamplerAdd_Click( Object sender, BEventArgs e )
        {
            openUtauCore.setSelectedFile( "resampler.exe" );
            int dr = AppManager.showModalDialog( openUtauCore, true, this );
            if ( dr == BFileChooser.APPROVE_OPTION ) {
                String path = openUtauCore.getSelectedFile();
                boolean check = false;
                boolean is_mac = isMac();
                if( is_mac ){
                    check = isWindowsExecutable( path );
                }
                listResampler.addRow( new String[]{ path }, check );
                if ( str.compare( txtWavtool.getText(), "" ) ) {
                    // wavtoolの欄が空欄だった場合のみ，
                    // wavtoolの候補を登録する(wavtoolがあれば)
                    String wavtool = fsys.combine( PortUtil.getDirectoryName( path ), "wavtool.exe" );
                    if ( PortUtil.isFileExists( wavtool ) ) {
                        txtWavtool.setText( wavtool );
                        check = false;
                        if( is_mac ){
                            check = isWindowsExecutable( wavtool );
                        }
                        chkWavtoolWithWine.setSelected( check );
                    }
                }
            }
        }

        public void buttonResamplerUpDown_Click( Object sender, EventArgs e )
        {
            int delta = 1;
            if ( sender == buttonResamplerUp ) {
                delta = -1;
            }
            int index = listResampler.getSelectedRow();
            int count = listResampler.getItemCountRow();
            if ( index < 0 || count <= index ) {
                return;
            }
            if ( index + delta < 0 || count <= index + delta ) {
                return;
            }

            String sel = (String)listResampler.getItemAt( index, 0 );
            boolean chk = listResampler.isRowChecked( index );
            listResampler.setItemAt( index, 0, listResampler.getItemAt( index + delta, 0 ) );
            listResampler.setRowChecked( index, listResampler.isRowChecked( index + delta ) );
            listResampler.setItemAt( index + delta, 0, sel );
            listResampler.setRowChecked( index + delta, chk );
            listResampler.setSelectedRow( index + delta );
        }

        public void buttonResamplerRemove_Click( Object sender, EventArgs e )
        {
            int index = listResampler.getSelectedRow();
            int count = listResampler.getItemCountRow();
            if ( index < 0 || count <= index ) {
                return;
            }
            listResampler.removeRow( index );
            // 選択し直す
            if( index >= count - 1 ){
                index--;
            }
            if( 0 <= index && index < count - 1 ){
                listResampler.setSelectedRow( index );
            }
        }

        public void btnWavtool_Click( Object sender, BEventArgs e )
        {
            if ( !txtWavtool.getText().Equals( "" ) && PortUtil.isDirectoryExists( PortUtil.getDirectoryName( txtWavtool.getText() ) ) ) {
                openUtauCore.setSelectedFile( txtWavtool.getText() );
            }
            int dr = AppManager.showModalDialog( openUtauCore, true, this );
            if ( dr == BFileChooser.APPROVE_OPTION ) {
                String path = openUtauCore.getSelectedFile();
                txtWavtool.setText( path );
                boolean is_mac = isMac();
                boolean check = false;
                if( is_mac ){
                    check = isWindowsExecutable( path );
                }
                chkWavtoolWithWine.setSelected( check );
                if ( listResampler.getItemCountRow() == 0 ) {
                    String resampler = fsys.combine( PortUtil.getDirectoryName( path ), "resampler.exe" );
                    if ( PortUtil.isFileExists( resampler ) ) {
                        check = false;
                        if( is_mac ){
                            check = isWindowsExecutable( resampler );
                        }
                        listResampler.addRow( new String[]{ resampler }, check );
                    }
                }
            }
        }

        public void btnAquesTone_Click( Object sender, BEventArgs e )
        {
            BFileChooser dialog = new BFileChooser();
            if ( !txtAquesTone.getText().Equals( "" ) && PortUtil.isDirectoryExists( PortUtil.getDirectoryName( txtAquesTone.getText() ) ) ) {
                dialog.setSelectedFile( txtAquesTone.getText() );
            }
            int dr = AppManager.showModalDialog( dialog, true, this );
            if ( dr == BFileChooser.APPROVE_OPTION ) {
                String path = dialog.getSelectedFile();
                txtAquesTone.setText( path );
            }
        }

        public void btnAdd_Click( Object sender, BEventArgs e )
        {
#if JAVA
            if( folderBrowserSingers.showOpenDialog( this ) == BFileChooser.APPROVE_OPTION ){
                String dir = folderBrowserSingers.getSelectedFile();
#else
#if DEBUG
            if ( folderBrowserSingers.showOpenDialog( this ) == BFileChooser.APPROVE_OPTION ) {
                String dir = folderBrowserSingers.getSelectedFile();
#else
            if ( folderBrowserSingers.showDialog( this ) == BDialogResult.OK ) {
                String dir = folderBrowserSingers.getSelectedPath();
#endif
#endif
#if DEBUG
                sout.println( "Preference#btnAdd_Click; dir=" + dir );
                sout.println( "Preference#btnAdd_Clicl; PortUtil.isDirectoryExists(dir)=" + PortUtil.isDirectoryExists( dir ) );
                sout.println( "Preference#btnAdd_Clicl; PortUtil.isFileExists(dir)=" + PortUtil.isFileExists( dir ) );
#endif
                if ( !PortUtil.isDirectoryExists( dir ) && PortUtil.isFileExists( dir ) ) {
                    // dirの指すパスがフォルダではなくファイルだった場合、
                    // そのファイルの存在するパスに修正
                    dir = PortUtil.getDirectoryName( dir );
                }
                SingerConfig sc = new SingerConfig();
                Utility.readUtauSingerConfig( dir, sc );
                m_utau_singers.add( sc );
                UpdateUtausingerList();
            }
        }

        public void listSingers_SelectedIndexChanged( Object sender, BEventArgs e )
        {
            int index = getUtausingersSelectedIndex();
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

        public void btnRemove_Click( Object sender, BEventArgs e )
        {
            int index = getUtausingersSelectedIndex();
            if ( 0 <= index && index < m_utau_singers.size() ) {
                m_utau_singers.removeElementAt( index );
            }
            UpdateUtausingerList();
        }

        public void btnDown_Click( Object sender, BEventArgs e )
        {
            int index = getUtausingersSelectedIndex();
#if DEBUG
            AppManager.debugWriteLine( "Preference.btnDown_Click; index=" + index );
#endif
            if ( 0 <= index && index + 1 < m_utau_singers.size() ) {
                SingerConfig buf = (SingerConfig)m_utau_singers.get( index ).clone();
                m_utau_singers.set( index, (SingerConfig)m_utau_singers.get( index + 1 ).clone() );
                m_utau_singers.set( index + 1, buf );
                UpdateUtausingerList();
                listSingers.setSelectedRow( index + 1 );
            }
        }

        public void btnUp_Click( Object sender, BEventArgs e )
        {
            int index = getUtausingersSelectedIndex();
#if DEBUG
            AppManager.debugWriteLine( "Preference.btnUp_Click; index=" + index );
#endif
            if ( 0 <= index - 1 && index < m_utau_singers.size() ) {
                SingerConfig buf = (SingerConfig)m_utau_singers.get( index ).clone();
                m_utau_singers.set( index, (SingerConfig)m_utau_singers.get( index - 1 ).clone() );
                m_utau_singers.set( index - 1, buf );
                UpdateUtausingerList();
                listSingers.setSelectedRow( index - 1 );
            }
        }

        public void chkAutoBackup_CheckedChanged( Object sender, BEventArgs e )
        {
            numAutoBackupInterval.setEnabled( chkAutoBackup.isSelected() );
        }

        public void Preference_FormClosing( Object sender, BFormClosingEventArgs e )
        {
            columnWidthHeaderProgramChange = listSingers.getColumnWidth( 0 );
            columnWidthHeaderName = listSingers.getColumnWidth( 1 );
            columnWidthHeaderPath = listSingers.getColumnWidth( 2 );
        }

        public void btnCancel_Click( Object sender, BEventArgs e )
        {
            setDialogResult( BDialogResult.CANCEL );
        }

        public void commonChangeAutoVibratoType( Object sender, BEventArgs e )
        {
            boolean v = radioVocaloidEditorCompatible.isSelected();
            boolean ud = radioUserDefined.isSelected();
            groupVocaloidEditorCompatible.setEnabled( v );
            groupUserDefined.setEnabled( ud );
            comboAutoVibratoType1.setEnabled( v );
            comboAutoVibratoType2.setEnabled( v );
            comboAutoVibratoTypeCustom.setEnabled( ud );
            lblAutoVibratoType1.setEnabled( v );
            lblAutoVibratoType2.setEnabled( v );
            lblAutoVibratoTypeCustom.setEnabled( ud );
        }
        
        public void buttonWinePrefix_Click( Object sender, BEventArgs e )
        {
            BFileChooser dialog = null;
            try{
                dialog = new BFileChooser();
                String dir = textWinePrefix.getText();
                if( dir != null && str.length( dir ) > 0 ){
                    dialog.setSelectedFile( fsys.combine( dir, "a" ) );
                }
                if( AppManager.showModalDialog( dialog, true, this ) == BFileChooser.APPROVE_OPTION ){
                    dir = dialog.getSelectedFile();
                    if( fsys.isFileExists( dir ) ){
                        // ファイルが選ばれた場合，その所属ディレクトリを値として用いる
                        dir = PortUtil.getDirectoryName( dir );
                    }
                    textWinePrefix.setText( dir );
                }
            }catch( Exception ex ){
            }
        }
        
        public void buttonWineTop_Click( Object sender, BEventArgs e )
        {
            BFileChooser dialog = null;
            try{
                dialog = new BFileChooser();
                String dir = textWineTop.getText();
                if( dir != null && str.length( dir ) > 0 ){
                    dialog.setSelectedFile( fsys.combine( dir, "a" ) );
                }
                if( AppManager.showModalDialog( dialog, true, this ) == BFileChooser.APPROVE_OPTION ){
                    dir = dialog.getSelectedFile();
                    if( fsys.isFileExists( dir ) ){
                        // ファイルが選ばれた場合，その所属ディレクトリを値として用いる
                        dir = PortUtil.getDirectoryName( dir );
                    }
                    textWineTop.setText( dir );
                }
            }catch( Exception ex ){
            }
        }

        public void radioWineBuiltin_CheckedChanged( Object sender, BEventArgs e )
        {
            boolean enable = !radioWineBuiltin.isSelected();
            textWineTop.setEnabled( enable );
            buttonWineTop.setEnabled( enable );
        }
        #endregion

        #region helper methods
        private boolean isMac()
        {
#if JAVA
            String osname = System.getProperty( "os.name" );
            if( osname == null ){
                osname = "";
            }
            return (osname.indexOf( "Mac" ) >= 0);
#else
            return false;
#endif
        }
        
        private boolean isWindowsExecutable( String path )
        {
            if( !fsys.isFileExists( path ) ){
                return false;
            }
            RandomAccessFile fs = null;
            try{
                fs = new RandomAccessFile( path, "r" );
                int r0 = fs.read(); // 'M'
                int r1 = fs.read(); // 'Z'
                if( 'M' == (char)r0 && 'Z' == (char)r1 ){
                    return true;
                }
            }catch( Exception ex ){
                serr.println( "Preference#isWindowsExecutable; ex=" + ex );
            }finally{
                if( fs != null ){
                    try{
                        fs.close();
                    }catch( Exception ex2 ){
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// カスタムビブラートの選択肢の欄を更新します
        /// </summary>
        private void updateCustomVibrato()
        {
            int size = AppManager.editorConfig.AutoVibratoCustom.size();
            comboAutoVibratoTypeCustom.removeAllItems();
            for ( int i = 0; i < size; i++ ) {
                VibratoHandle handle = AppManager.editorConfig.AutoVibratoCustom.get( i );
                comboAutoVibratoTypeCustom.addItem( handle );
            }
        }

        /// <summary>
        /// MIDIデバイスの選択肢の欄を更新します
        /// </summary>
        private void updateMidiDevice()
        {
            int sel_midi = comboMidiInPortNumber.getSelectedIndex();
            int sel_mtc = comboMtcMidiInPortNumber.getSelectedIndex();

            comboMidiInPortNumber.removeAllItems();
            comboMtcMidiInPortNumber.removeAllItems();
#if ENABLE_MIDI
            Vector<MidiDevice.Info> midiins = new Vector<MidiDevice.Info>();
            foreach( MidiDevice.Info info in MidiSystem.getMidiDeviceInfo() ) {
#if DEBUG
                sout.println( "Preference#updateMidiDevice; info.getName()=" + info.getName() );
#endif
                MidiDevice device = null;
                try{
                    device = MidiSystem.getMidiDevice( info );
                }catch( Exception ex ){
#if JAVA
                    ex.printStackTrace();
#endif
                    device = null;
                }
                if( device == null ) continue;
#if DEBUG
                sout.println( "Preference#updateMidiDevice; (device is Receiver)=" + (device is Receiver) );
#endif
                // MIDI-OUTの最大接続数．-1は制限なしを表す
                int max = device.getMaxTransmitters();
                if( max > 0 || max == -1 ){
                    midiins.add( info );
                }
            }
            
            foreach( MidiDevice.Info info in midiins ){
                comboMidiInPortNumber.addItem( info );
                comboMtcMidiInPortNumber.addItem( info );
            }
            if ( vec.size( midiins ) <= 0 ) {
                comboMtcMidiInPortNumber.setEnabled( false );
                comboMidiInPortNumber.setEnabled( false );
            } else {
#if ENABLE_MTC
                comboMtcMidiInPortNumber.setEnabled( true );
#else // ENABLE_MTC
                comboMtcMidiInPortNumber.setEnabled( false );
#endif // ENABLE_MTC
                comboMidiInPortNumber.setEnabled( true );
            }
#else // ENABLE_MIDI
            comboMtcMidiInPortNumber.setEnabled( false );
            comboMidiInPortNumber.setEnabled( false );
#endif // ENABLE_MIDI

            // 可能なら選択状態を復帰
            if ( sel_midi >= 0 ) {
                if ( comboMidiInPortNumber.getItemCount() <= sel_midi ) {
                    sel_midi = comboMidiInPortNumber.getItemCount() - 1;
                }
                comboMidiInPortNumber.setSelectedIndex( sel_midi );
            }

            if ( sel_mtc >= 0 ) {
                if ( comboMtcMidiInPortNumber.getItemCount() <= sel_mtc ) {
                    sel_mtc = comboMtcMidiInPortNumber.getItemCount() - 1;
                }
                comboMtcMidiInPortNumber.setSelectedIndex( sel_mtc );
            }
        }

        private static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        private void UpdateFonts( String font_name )
        {
            if ( font_name == null ) {
                return;
            }
            if ( font_name.Equals( "" ) ) {
                return;
            }
            Font f = this.getFont();
            if ( f == null ) {
                return;
            }
            Font font = new Font( font_name, java.awt.Font.PLAIN, (int)f.getSize() );
            Util.applyFontRecurse( this, font );
        }

        private void UpdateUtausingerList()
        {
            listSingers.clear();
            for ( int i = 0; i < m_utau_singers.size(); i++ ) {
                m_utau_singers.get( i ).Program = i;
                listSingers.addRow( 
                    new String[] { 
                        m_utau_singers.get( i ).Program + "",
                        m_utau_singers.get( i ).VOICENAME, 
                        m_utau_singers.get( i ).VOICEIDSTR } );
            }
        }

        private int getUtausingersSelectedIndex()
        {
            return listSingers.getSelectedRow();
        }

        private void registerEventHandlers()
        {
            btnChangeScreenFont.Click += new BEventHandler( btnChangeScreenFont_Click );
            btnChangeMenuFont.Click += new BEventHandler( btnChangeMenuFont_Click );
            btnWavtool.Click += new BEventHandler( btnWavtool_Click );
            buttonResamplerAdd.Click += new BEventHandler( buttonResamplerAdd_Click );
            buttonResamplerRemove.Click += new BEventHandler( buttonResamplerRemove_Click );
            buttonResamplerUp.Click += new BEventHandler( buttonResamplerUpDown_Click );
            buttonResamplerDown.Click += new BEventHandler( buttonResamplerUpDown_Click );
            btnAquesTone.Click += new BEventHandler( btnAquesTone_Click );
            btnRemove.Click += new BEventHandler( btnRemove_Click );
            btnAdd.Click += new BEventHandler( btnAdd_Click );
            btnUp.Click += new BEventHandler( btnUp_Click );
            btnDown.Click += new BEventHandler( btnDown_Click );
            listSingers.SelectedIndexChanged += new BEventHandler( listSingers_SelectedIndexChanged );
            chkAutoBackup.CheckedChanged += new BEventHandler( chkAutoBackup_CheckedChanged );
            btnOK.Click += new BEventHandler( btnOK_Click );
            this.FormClosing += new BFormClosingEventHandler( Preference_FormClosing );
            btnCancel.Click += new BEventHandler( btnCancel_Click );
            radioVocaloidEditorCompatible.CheckedChanged += new BEventHandler( commonChangeAutoVibratoType );
            buttonWinePrefix.Click += new BEventHandler( buttonWinePrefix_Click );
            buttonWineTop.Click += new BEventHandler( buttonWineTop_Click );
            radioWineBuiltin.CheckedChanged += new BEventHandler( radioWineBuiltin_CheckedChanged );
        }

        private void setResources()
        {
        }
        #endregion

        #region ui implementation
#if JAVA
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/cadencii/Preference.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/cadencii/Preference.java
#else
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing )
        {
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
        private void InitializeComponent()
        {
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
            System.Windows.Forms.ListViewGroup listViewGroup16 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup17 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup18 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup19 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup20 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup21 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup22 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup23 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup24 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup25 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup26 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup27 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup28 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup29 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup30 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup31 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup32 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup33 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup34 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup35 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup36 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup37 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup38 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup39 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup40 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup41 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup42 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup43 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            System.Windows.Forms.ListViewGroup listViewGroup44 = new System.Windows.Forms.ListViewGroup( "ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left );
            this.tabPreference = new System.Windows.Forms.TabControl();
            this.tabSequence = new System.Windows.Forms.TabPage();
            this.txtAutoVibratoThresholdLength = new org.kbinani.cadencii.NumberTextBox();
            this.bLabel1 = new org.kbinani.windows.forms.BLabel();
            this.radioUserDefined = new org.kbinani.windows.forms.BRadioButton();
            this.lblAutoVibratoType = new org.kbinani.windows.forms.BLabel();
            this.radioVocaloidEditorCompatible = new org.kbinani.windows.forms.BRadioButton();
            this.groupUserDefined = new org.kbinani.windows.forms.BGroupBox();
            this.comboAutoVibratoTypeCustom = new org.kbinani.windows.forms.BComboBox();
            this.lblAutoVibratoTypeCustom = new org.kbinani.windows.forms.BLabel();
            this.label6 = new org.kbinani.windows.forms.BLabel();
            this.lblAutoVibratoThresholdLength = new org.kbinani.windows.forms.BLabel();
            this.label2 = new org.kbinani.windows.forms.BLabel();
            this.chkEnableAutoVibrato = new org.kbinani.windows.forms.BCheckBox();
            this.comboResolControlCurve = new org.kbinani.windows.forms.BComboBox();
            this.lblResolControlCurve = new org.kbinani.windows.forms.BLabel();
            this.label1 = new org.kbinani.windows.forms.BLabel();
            this.lblResolution = new org.kbinani.windows.forms.BLabel();
            this.label7 = new org.kbinani.windows.forms.BLabel();
            this.groupVocaloidEditorCompatible = new org.kbinani.windows.forms.BGroupBox();
            this.comboAutoVibratoType2 = new org.kbinani.windows.forms.BComboBox();
            this.lblAutoVibratoType2 = new org.kbinani.windows.forms.BLabel();
            this.comboAutoVibratoType1 = new org.kbinani.windows.forms.BComboBox();
            this.lblAutoVibratoType1 = new org.kbinani.windows.forms.BLabel();
            this.label3 = new org.kbinani.windows.forms.BLabel();
            this.comboVibratoLength = new org.kbinani.windows.forms.BComboBox();
            this.lblVibratoLength = new org.kbinani.windows.forms.BLabel();
            this.tabAnother = new System.Windows.Forms.TabPage();
            this.lblBufferSize = new org.kbinani.windows.forms.BLabel();
            this.lblBuffer = new org.kbinani.windows.forms.BLabel();
            this.label13 = new org.kbinani.windows.forms.BLabel();
            this.label12 = new org.kbinani.windows.forms.BLabel();
            this.comboDefualtSinger = new org.kbinani.windows.forms.BComboBox();
            this.chkChasePastEvent = new org.kbinani.windows.forms.BCheckBox();
            this.lblWait = new org.kbinani.windows.forms.BLabel();
            this.lblPreSendTime = new org.kbinani.windows.forms.BLabel();
            this.lblDefaultSinger = new org.kbinani.windows.forms.BLabel();
            this.numBuffer = new org.kbinani.cadencii.NumericUpDownEx();
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
            this.chkTranslateRoman = new org.kbinani.windows.forms.BCheckBox();
            this.chkUseSpaceKeyAsMiddleButtonModifier = new org.kbinani.windows.forms.BCheckBox();
            this.labelWheelOrder = new org.kbinani.windows.forms.BLabel();
            this.numericUpDownEx1 = new org.kbinani.cadencii.NumericUpDownEx();
            this.chkCursorFix = new org.kbinani.windows.forms.BCheckBox();
            this.chkCurveSelectingQuantized = new org.kbinani.windows.forms.BCheckBox();
            this.chkScrollHorizontal = new org.kbinani.windows.forms.BCheckBox();
            this.chkPlayPreviewWhenRightClick = new org.kbinani.windows.forms.BCheckBox();
            this.chkKeepLyricInputMode = new org.kbinani.windows.forms.BCheckBox();
            this.tabPlatform = new System.Windows.Forms.TabPage();
            this.groupWine = new org.kbinani.windows.forms.BGroupBox();
            this.radioWineCustom = new org.kbinani.windows.forms.BRadioButton();
            this.radioWineBuiltin = new org.kbinani.windows.forms.BRadioButton();
            this.labelWineTop = new org.kbinani.windows.forms.BLabel();
            this.buttonWineTop = new org.kbinani.windows.forms.BButton();
            this.textWineTop = new org.kbinani.windows.forms.BTextBox();
            this.labelWinePrefix = new org.kbinani.windows.forms.BLabel();
            this.buttonWinePrefix = new org.kbinani.windows.forms.BButton();
            this.textWinePrefix = new org.kbinani.windows.forms.BTextBox();
            this.groupUtauCores = new org.kbinani.windows.forms.BGroupBox();
            this.labelResamplerWithWine = new org.kbinani.windows.forms.BLabel();
            this.labelWavtoolPath = new org.kbinani.windows.forms.BLabel();
            this.buttonResamplerUp = new org.kbinani.windows.forms.BButton();
            this.buttonResamplerDown = new org.kbinani.windows.forms.BButton();
            this.buttonResamplerRemove = new org.kbinani.windows.forms.BButton();
            this.buttonResamplerAdd = new org.kbinani.windows.forms.BButton();
            this.listResampler = new org.kbinani.windows.forms.BListView();
            this.columnHeaderPath = new System.Windows.Forms.ColumnHeader();
            this.lblResampler = new org.kbinani.windows.forms.BLabel();
            this.chkWavtoolWithWine = new org.kbinani.windows.forms.BCheckBox();
            this.btnWavtool = new org.kbinani.windows.forms.BButton();
            this.lblWavtool = new org.kbinani.windows.forms.BLabel();
            this.txtWavtool = new org.kbinani.windows.forms.BTextBox();
            this.tabUtausingers = new System.Windows.Forms.TabPage();
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
            this.groupDefaultSynthesizer = new org.kbinani.windows.forms.BGroupBox();
            this.comboDefaultSynthesizer = new org.kbinani.windows.forms.BComboBox();
            this.groupSynthesizerDll = new org.kbinani.windows.forms.BGroupBox();
            this.chkLoadAquesTone = new org.kbinani.windows.forms.BCheckBox();
            this.chkLoadVocaloid2 = new org.kbinani.windows.forms.BCheckBox();
            this.chkLoadVocaloid1 = new org.kbinani.windows.forms.BCheckBox();
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
            this.groupUserDefined.SuspendLayout();
            this.groupVocaloidEditorCompatible.SuspendLayout();
            this.tabAnother.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBuffer)).BeginInit();
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
            this.groupWine.SuspendLayout();
            this.groupUtauCores.SuspendLayout();
            this.tabUtausingers.SuspendLayout();
            this.tabFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoBackupInterval)).BeginInit();
            this.tabSingingSynth.SuspendLayout();
            this.groupDefaultSynthesizer.SuspendLayout();
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
            this.tabPreference.Controls.Add( this.tabUtausingers );
            this.tabPreference.Controls.Add( this.tabFile );
            this.tabPreference.Controls.Add( this.tabSingingSynth );
            this.tabPreference.Location = new System.Drawing.Point( 7, 7 );
            this.tabPreference.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 3 );
            this.tabPreference.Multiline = true;
            this.tabPreference.Name = "tabPreference";
            this.tabPreference.SelectedIndex = 0;
            this.tabPreference.Size = new System.Drawing.Size( 462, 483 );
            this.tabPreference.TabIndex = 0;
            // 
            // tabSequence
            // 
            this.tabSequence.Controls.Add( this.txtAutoVibratoThresholdLength );
            this.tabSequence.Controls.Add( this.bLabel1 );
            this.tabSequence.Controls.Add( this.radioUserDefined );
            this.tabSequence.Controls.Add( this.lblAutoVibratoType );
            this.tabSequence.Controls.Add( this.radioVocaloidEditorCompatible );
            this.tabSequence.Controls.Add( this.groupUserDefined );
            this.tabSequence.Controls.Add( this.label6 );
            this.tabSequence.Controls.Add( this.lblAutoVibratoThresholdLength );
            this.tabSequence.Controls.Add( this.label2 );
            this.tabSequence.Controls.Add( this.chkEnableAutoVibrato );
            this.tabSequence.Controls.Add( this.comboResolControlCurve );
            this.tabSequence.Controls.Add( this.lblResolControlCurve );
            this.tabSequence.Controls.Add( this.label1 );
            this.tabSequence.Controls.Add( this.lblResolution );
            this.tabSequence.Controls.Add( this.label7 );
            this.tabSequence.Controls.Add( this.groupVocaloidEditorCompatible );
            this.tabSequence.Controls.Add( this.label3 );
            this.tabSequence.Controls.Add( this.comboVibratoLength );
            this.tabSequence.Controls.Add( this.lblVibratoLength );
            this.tabSequence.Location = new System.Drawing.Point( 4, 38 );
            this.tabSequence.Name = "tabSequence";
            this.tabSequence.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabSequence.Size = new System.Drawing.Size( 454, 441 );
            this.tabSequence.TabIndex = 0;
            this.tabSequence.Text = "Sequence";
            this.tabSequence.UseVisualStyleBackColor = true;
            // 
            // txtAutoVibratoThresholdLength
            // 
            this.txtAutoVibratoThresholdLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAutoVibratoThresholdLength.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(240)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))) );
            this.txtAutoVibratoThresholdLength.ForeColor = System.Drawing.Color.White;
            this.txtAutoVibratoThresholdLength.Location = new System.Drawing.Point( 329, 122 );
            this.txtAutoVibratoThresholdLength.Name = "txtAutoVibratoThresholdLength";
            this.txtAutoVibratoThresholdLength.Size = new System.Drawing.Size( 55, 19 );
            this.txtAutoVibratoThresholdLength.TabIndex = 6;
            this.txtAutoVibratoThresholdLength.Type = org.kbinani.cadencii.NumberTextBox.ValueType.Integer;
            // 
            // bLabel1
            // 
            this.bLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bLabel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.bLabel1.Location = new System.Drawing.Point( 121, 161 );
            this.bLabel1.Name = "bLabel1";
            this.bLabel1.Size = new System.Drawing.Size( 314, 1 );
            this.bLabel1.TabIndex = 36;
            // 
            // radioUserDefined
            // 
            this.radioUserDefined.AutoSize = true;
            this.radioUserDefined.Location = new System.Drawing.Point( 246, 176 );
            this.radioUserDefined.Name = "radioUserDefined";
            this.radioUserDefined.Size = new System.Drawing.Size( 90, 16 );
            this.radioUserDefined.TabIndex = 9;
            this.radioUserDefined.Text = "User Defined";
            this.radioUserDefined.UseVisualStyleBackColor = true;
            // 
            // lblAutoVibratoType
            // 
            this.lblAutoVibratoType.AutoSize = true;
            this.lblAutoVibratoType.Location = new System.Drawing.Point( 13, 155 );
            this.lblAutoVibratoType.Name = "lblAutoVibratoType";
            this.lblAutoVibratoType.Size = new System.Drawing.Size( 99, 12 );
            this.lblAutoVibratoType.TabIndex = 35;
            this.lblAutoVibratoType.Text = "Auto Vibrato Type";
            // 
            // radioVocaloidEditorCompatible
            // 
            this.radioVocaloidEditorCompatible.AutoSize = true;
            this.radioVocaloidEditorCompatible.Checked = true;
            this.radioVocaloidEditorCompatible.Location = new System.Drawing.Point( 37, 176 );
            this.radioVocaloidEditorCompatible.Name = "radioVocaloidEditorCompatible";
            this.radioVocaloidEditorCompatible.Size = new System.Drawing.Size( 175, 16 );
            this.radioVocaloidEditorCompatible.TabIndex = 8;
            this.radioVocaloidEditorCompatible.TabStop = true;
            this.radioVocaloidEditorCompatible.Text = "VOCALOID Editor Compatible";
            this.radioVocaloidEditorCompatible.UseVisualStyleBackColor = true;
            // 
            // groupUserDefined
            // 
            this.groupUserDefined.Controls.Add( this.comboAutoVibratoTypeCustom );
            this.groupUserDefined.Controls.Add( this.lblAutoVibratoTypeCustom );
            this.groupUserDefined.Enabled = false;
            this.groupUserDefined.Location = new System.Drawing.Point( 63, 291 );
            this.groupUserDefined.Name = "groupUserDefined";
            this.groupUserDefined.Size = new System.Drawing.Size( 372, 58 );
            this.groupUserDefined.TabIndex = 15;
            this.groupUserDefined.TabStop = false;
            this.groupUserDefined.Text = "User Defined";
            // 
            // comboAutoVibratoTypeCustom
            // 
            this.comboAutoVibratoTypeCustom.FormattingEnabled = true;
            this.comboAutoVibratoTypeCustom.Location = new System.Drawing.Point( 225, 22 );
            this.comboAutoVibratoTypeCustom.Name = "comboAutoVibratoTypeCustom";
            this.comboAutoVibratoTypeCustom.Size = new System.Drawing.Size( 131, 20 );
            this.comboAutoVibratoTypeCustom.TabIndex = 14;
            // 
            // lblAutoVibratoTypeCustom
            // 
            this.lblAutoVibratoTypeCustom.AutoSize = true;
            this.lblAutoVibratoTypeCustom.Location = new System.Drawing.Point( 18, 25 );
            this.lblAutoVibratoTypeCustom.Name = "lblAutoVibratoTypeCustom";
            this.lblAutoVibratoTypeCustom.Size = new System.Drawing.Size( 131, 12 );
            this.lblAutoVibratoTypeCustom.TabIndex = 13;
            this.lblAutoVibratoTypeCustom.Text = "Vibrato Type: Custom(&T)";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 390, 125 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 38, 12 );
            this.label6.TabIndex = 26;
            this.label6.Text = "clocks";
            // 
            // lblAutoVibratoThresholdLength
            // 
            this.lblAutoVibratoThresholdLength.AutoSize = true;
            this.lblAutoVibratoThresholdLength.Location = new System.Drawing.Point( 35, 125 );
            this.lblAutoVibratoThresholdLength.Name = "lblAutoVibratoThresholdLength";
            this.lblAutoVibratoThresholdLength.Size = new System.Drawing.Size( 243, 12 );
            this.lblAutoVibratoThresholdLength.TabIndex = 6;
            this.lblAutoVibratoThresholdLength.Text = "Minimum note length for Automatic Vibrato(&M)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 317, 38 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 38, 12 );
            this.label2.TabIndex = 32;
            this.label2.Text = "clocks";
            // 
            // chkEnableAutoVibrato
            // 
            this.chkEnableAutoVibrato.AutoSize = true;
            this.chkEnableAutoVibrato.Location = new System.Drawing.Point( 15, 67 );
            this.chkEnableAutoVibrato.Name = "chkEnableAutoVibrato";
            this.chkEnableAutoVibrato.Size = new System.Drawing.Size( 170, 16 );
            this.chkEnableAutoVibrato.TabIndex = 3;
            this.chkEnableAutoVibrato.Text = "Enable Automatic Vibrato(&E)";
            this.chkEnableAutoVibrato.UseVisualStyleBackColor = true;
            // 
            // comboResolControlCurve
            // 
            this.comboResolControlCurve.FormattingEnabled = true;
            this.comboResolControlCurve.Location = new System.Drawing.Point( 210, 35 );
            this.comboResolControlCurve.Name = "comboResolControlCurve";
            this.comboResolControlCurve.Size = new System.Drawing.Size( 101, 20 );
            this.comboResolControlCurve.TabIndex = 1;
            // 
            // lblResolControlCurve
            // 
            this.lblResolControlCurve.AutoSize = true;
            this.lblResolControlCurve.Location = new System.Drawing.Point( 35, 38 );
            this.lblResolControlCurve.Name = "lblResolControlCurve";
            this.lblResolControlCurve.Size = new System.Drawing.Size( 92, 12 );
            this.lblResolControlCurve.TabIndex = 26;
            this.lblResolControlCurve.Text = "Control Curve(&C)";
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
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label7.Location = new System.Drawing.Point( 189, 74 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 246, 1 );
            this.label7.TabIndex = 5;
            // 
            // groupVocaloidEditorCompatible
            // 
            this.groupVocaloidEditorCompatible.Controls.Add( this.comboAutoVibratoType2 );
            this.groupVocaloidEditorCompatible.Controls.Add( this.lblAutoVibratoType2 );
            this.groupVocaloidEditorCompatible.Controls.Add( this.comboAutoVibratoType1 );
            this.groupVocaloidEditorCompatible.Controls.Add( this.lblAutoVibratoType1 );
            this.groupVocaloidEditorCompatible.Location = new System.Drawing.Point( 63, 204 );
            this.groupVocaloidEditorCompatible.Name = "groupVocaloidEditorCompatible";
            this.groupVocaloidEditorCompatible.Size = new System.Drawing.Size( 372, 81 );
            this.groupVocaloidEditorCompatible.TabIndex = 10;
            this.groupVocaloidEditorCompatible.TabStop = false;
            this.groupVocaloidEditorCompatible.Text = "VOCALOID Editor Compatible";
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
            this.comboAutoVibratoType2.Location = new System.Drawing.Point( 225, 51 );
            this.comboAutoVibratoType2.Name = "comboAutoVibratoType2";
            this.comboAutoVibratoType2.Size = new System.Drawing.Size( 131, 20 );
            this.comboAutoVibratoType2.TabIndex = 14;
            // 
            // lblAutoVibratoType2
            // 
            this.lblAutoVibratoType2.AutoSize = true;
            this.lblAutoVibratoType2.Location = new System.Drawing.Point( 18, 54 );
            this.lblAutoVibratoType2.Name = "lblAutoVibratoType2";
            this.lblAutoVibratoType2.Size = new System.Drawing.Size( 155, 12 );
            this.lblAutoVibratoType2.TabIndex = 13;
            this.lblAutoVibratoType2.Text = "Vibrato Type: VOCALOID2(&T)";
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
            this.comboAutoVibratoType1.Location = new System.Drawing.Point( 225, 22 );
            this.comboAutoVibratoType1.Name = "comboAutoVibratoType1";
            this.comboAutoVibratoType1.Size = new System.Drawing.Size( 131, 20 );
            this.comboAutoVibratoType1.TabIndex = 12;
            // 
            // lblAutoVibratoType1
            // 
            this.lblAutoVibratoType1.AutoSize = true;
            this.lblAutoVibratoType1.Location = new System.Drawing.Point( 18, 25 );
            this.lblAutoVibratoType1.Name = "lblAutoVibratoType1";
            this.lblAutoVibratoType1.Size = new System.Drawing.Size( 155, 12 );
            this.lblAutoVibratoType1.TabIndex = 11;
            this.lblAutoVibratoType1.Text = "Vibrato Type: VOCALOID1(&T)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 313, 96 );
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
            this.comboVibratoLength.Location = new System.Drawing.Point( 213, 93 );
            this.comboVibratoLength.Name = "comboVibratoLength";
            this.comboVibratoLength.Size = new System.Drawing.Size( 86, 20 );
            this.comboVibratoLength.TabIndex = 5;
            // 
            // lblVibratoLength
            // 
            this.lblVibratoLength.AutoSize = true;
            this.lblVibratoLength.Location = new System.Drawing.Point( 35, 96 );
            this.lblVibratoLength.Name = "lblVibratoLength";
            this.lblVibratoLength.Size = new System.Drawing.Size( 135, 12 );
            this.lblVibratoLength.TabIndex = 4;
            this.lblVibratoLength.Text = "Default Vibrato Length(&L)";
            // 
            // tabAnother
            // 
            this.tabAnother.Controls.Add( this.lblBufferSize );
            this.tabAnother.Controls.Add( this.lblBuffer );
            this.tabAnother.Controls.Add( this.label13 );
            this.tabAnother.Controls.Add( this.label12 );
            this.tabAnother.Controls.Add( this.comboDefualtSinger );
            this.tabAnother.Controls.Add( this.chkChasePastEvent );
            this.tabAnother.Controls.Add( this.lblWait );
            this.tabAnother.Controls.Add( this.lblPreSendTime );
            this.tabAnother.Controls.Add( this.lblDefaultSinger );
            this.tabAnother.Controls.Add( this.numBuffer );
            this.tabAnother.Controls.Add( this.numWait );
            this.tabAnother.Controls.Add( this.numPreSendTime );
            this.tabAnother.Location = new System.Drawing.Point( 4, 38 );
            this.tabAnother.Name = "tabAnother";
            this.tabAnother.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabAnother.Size = new System.Drawing.Size( 454, 441 );
            this.tabAnother.TabIndex = 2;
            this.tabAnother.Text = "Other Settings";
            this.tabAnother.UseVisualStyleBackColor = true;
            // 
            // lblBufferSize
            // 
            this.lblBufferSize.AutoSize = true;
            this.lblBufferSize.Location = new System.Drawing.Point( 305, 150 );
            this.lblBufferSize.Name = "lblBufferSize";
            this.lblBufferSize.Size = new System.Drawing.Size( 88, 12 );
            this.lblBufferSize.TabIndex = 30;
            this.lblBufferSize.Text = "msec(100-1000)";
            // 
            // lblBuffer
            // 
            this.lblBuffer.AutoSize = true;
            this.lblBuffer.Location = new System.Drawing.Point( 29, 150 );
            this.lblBuffer.Name = "lblBuffer";
            this.lblBuffer.Size = new System.Drawing.Size( 78, 12 );
            this.lblBuffer.TabIndex = 29;
            this.lblBuffer.Text = "Buffer Size(&B)";
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
            this.chkChasePastEvent.Location = new System.Drawing.Point( 31, 119 );
            this.chkChasePastEvent.Name = "chkChasePastEvent";
            this.chkChasePastEvent.Size = new System.Drawing.Size( 105, 16 );
            this.chkChasePastEvent.TabIndex = 24;
            this.chkChasePastEvent.Text = "Chase Event(&C)";
            this.chkChasePastEvent.UseVisualStyleBackColor = true;
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
            // numBuffer
            // 
            this.numBuffer.Location = new System.Drawing.Point( 216, 148 );
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
            this.tabAppearance.Size = new System.Drawing.Size( 454, 441 );
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
            this.tabOperation.Size = new System.Drawing.Size( 454, 441 );
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
            this.groupMisc.Location = new System.Drawing.Point( 23, 253 );
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
            this.lblMilliSecond.Location = new System.Drawing.Point( 302, 50 );
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
            this.groupPianoroll.Controls.Add( this.chkTranslateRoman );
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
            this.groupPianoroll.Size = new System.Drawing.Size( 407, 238 );
            this.groupPianoroll.TabIndex = 90;
            this.groupPianoroll.TabStop = false;
            this.groupPianoroll.Text = "Piano Roll";
            // 
            // chkTranslateRoman
            // 
            this.chkTranslateRoman.AutoSize = true;
            this.chkTranslateRoman.Location = new System.Drawing.Point( 16, 208 );
            this.chkTranslateRoman.Name = "chkTranslateRoman";
            this.chkTranslateRoman.Size = new System.Drawing.Size( 200, 16 );
            this.chkTranslateRoman.TabIndex = 104;
            this.chkTranslateRoman.Text = "Translate Roman letters into Kana";
            this.chkTranslateRoman.UseVisualStyleBackColor = true;
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
            this.tabPlatform.Controls.Add( this.groupWine );
            this.tabPlatform.Controls.Add( this.groupUtauCores );
            this.tabPlatform.Location = new System.Drawing.Point( 4, 38 );
            this.tabPlatform.Name = "tabPlatform";
            this.tabPlatform.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabPlatform.Size = new System.Drawing.Size( 454, 441 );
            this.tabPlatform.TabIndex = 4;
            this.tabPlatform.Text = "Platform";
            this.tabPlatform.UseVisualStyleBackColor = true;
            // 
            // groupWine
            // 
            this.groupWine.Controls.Add( this.radioWineCustom );
            this.groupWine.Controls.Add( this.radioWineBuiltin );
            this.groupWine.Controls.Add( this.labelWineTop );
            this.groupWine.Controls.Add( this.buttonWineTop );
            this.groupWine.Controls.Add( this.textWineTop );
            this.groupWine.Controls.Add( this.labelWinePrefix );
            this.groupWine.Controls.Add( this.buttonWinePrefix );
            this.groupWine.Controls.Add( this.textWinePrefix );
            this.groupWine.Location = new System.Drawing.Point( 23, 311 );
            this.groupWine.Name = "groupWine";
            this.groupWine.Size = new System.Drawing.Size( 407, 124 );
            this.groupWine.TabIndex = 109;
            this.groupWine.TabStop = false;
            this.groupWine.Text = "Wine";
            // 
            // radioWineCustom
            // 
            this.radioWineCustom.AutoSize = true;
            this.radioWineCustom.Location = new System.Drawing.Point( 46, 94 );
            this.radioWineCustom.Name = "radioWineCustom";
            this.radioWineCustom.Size = new System.Drawing.Size( 60, 16 );
            this.radioWineCustom.TabIndex = 125;
            this.radioWineCustom.Text = "custom";
            this.radioWineCustom.UseVisualStyleBackColor = true;
            // 
            // radioWineBuiltin
            // 
            this.radioWineBuiltin.AutoSize = true;
            this.radioWineBuiltin.Location = new System.Drawing.Point( 46, 72 );
            this.radioWineBuiltin.Name = "radioWineBuiltin";
            this.radioWineBuiltin.Size = new System.Drawing.Size( 60, 16 );
            this.radioWineBuiltin.TabIndex = 124;
            this.radioWineBuiltin.Text = "built-in";
            this.radioWineBuiltin.UseVisualStyleBackColor = true;
            // 
            // labelWineTop
            // 
            this.labelWineTop.AutoSize = true;
            this.labelWineTop.Location = new System.Drawing.Point( 18, 54 );
            this.labelWineTop.Name = "labelWineTop";
            this.labelWineTop.Size = new System.Drawing.Size( 54, 12 );
            this.labelWineTop.TabIndex = 123;
            this.labelWineTop.Text = "WINETOP";
            // 
            // buttonWineTop
            // 
            this.buttonWineTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWineTop.Location = new System.Drawing.Point( 326, 91 );
            this.buttonWineTop.Name = "buttonWineTop";
            this.buttonWineTop.Size = new System.Drawing.Size( 75, 23 );
            this.buttonWineTop.TabIndex = 122;
            this.buttonWineTop.Text = "Browse";
            this.buttonWineTop.UseVisualStyleBackColor = true;
            // 
            // textWineTop
            // 
            this.textWineTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textWineTop.Location = new System.Drawing.Point( 112, 93 );
            this.textWineTop.Name = "textWineTop";
            this.textWineTop.Size = new System.Drawing.Size( 208, 19 );
            this.textWineTop.TabIndex = 121;
            // 
            // labelWinePrefix
            // 
            this.labelWinePrefix.AutoSize = true;
            this.labelWinePrefix.Location = new System.Drawing.Point( 17, 25 );
            this.labelWinePrefix.Name = "labelWinePrefix";
            this.labelWinePrefix.Size = new System.Drawing.Size( 71, 12 );
            this.labelWinePrefix.TabIndex = 120;
            this.labelWinePrefix.Text = "WINEPREFIX";
            // 
            // buttonWinePrefix
            // 
            this.buttonWinePrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWinePrefix.Location = new System.Drawing.Point( 326, 20 );
            this.buttonWinePrefix.Name = "buttonWinePrefix";
            this.buttonWinePrefix.Size = new System.Drawing.Size( 75, 23 );
            this.buttonWinePrefix.TabIndex = 112;
            this.buttonWinePrefix.Text = "Browse";
            this.buttonWinePrefix.UseVisualStyleBackColor = true;
            // 
            // textWinePrefix
            // 
            this.textWinePrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textWinePrefix.Location = new System.Drawing.Point( 100, 22 );
            this.textWinePrefix.Name = "textWinePrefix";
            this.textWinePrefix.Size = new System.Drawing.Size( 220, 19 );
            this.textWinePrefix.TabIndex = 111;
            // 
            // groupUtauCores
            // 
            this.groupUtauCores.Controls.Add( this.labelResamplerWithWine );
            this.groupUtauCores.Controls.Add( this.labelWavtoolPath );
            this.groupUtauCores.Controls.Add( this.buttonResamplerUp );
            this.groupUtauCores.Controls.Add( this.buttonResamplerDown );
            this.groupUtauCores.Controls.Add( this.buttonResamplerRemove );
            this.groupUtauCores.Controls.Add( this.buttonResamplerAdd );
            this.groupUtauCores.Controls.Add( this.listResampler );
            this.groupUtauCores.Controls.Add( this.lblResampler );
            this.groupUtauCores.Controls.Add( this.chkWavtoolWithWine );
            this.groupUtauCores.Controls.Add( this.btnWavtool );
            this.groupUtauCores.Controls.Add( this.lblWavtool );
            this.groupUtauCores.Controls.Add( this.txtWavtool );
            this.groupUtauCores.Location = new System.Drawing.Point( 23, 9 );
            this.groupUtauCores.Name = "groupUtauCores";
            this.groupUtauCores.Size = new System.Drawing.Size( 407, 296 );
            this.groupUtauCores.TabIndex = 108;
            this.groupUtauCores.TabStop = false;
            this.groupUtauCores.Text = "UTAU Cores";
            // 
            // labelResamplerWithWine
            // 
            this.labelResamplerWithWine.AutoSize = true;
            this.labelResamplerWithWine.Location = new System.Drawing.Point( 49, 266 );
            this.labelResamplerWithWine.Name = "labelResamplerWithWine";
            this.labelResamplerWithWine.Size = new System.Drawing.Size( 143, 12 );
            this.labelResamplerWithWine.TabIndex = 121;
            this.labelResamplerWithWine.Text = "Check the box to use Wine";
            // 
            // labelWavtoolPath
            // 
            this.labelWavtoolPath.AutoSize = true;
            this.labelWavtoolPath.Location = new System.Drawing.Point( 44, 49 );
            this.labelWavtoolPath.Name = "labelWavtoolPath";
            this.labelWavtoolPath.Size = new System.Drawing.Size( 30, 12 );
            this.labelWavtoolPath.TabIndex = 120;
            this.labelWavtoolPath.Text = "Path:";
            // 
            // buttonResamplerUp
            // 
            this.buttonResamplerUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonResamplerUp.Location = new System.Drawing.Point( 326, 211 );
            this.buttonResamplerUp.Name = "buttonResamplerUp";
            this.buttonResamplerUp.Size = new System.Drawing.Size( 75, 23 );
            this.buttonResamplerUp.TabIndex = 119;
            this.buttonResamplerUp.Text = "Up";
            this.buttonResamplerUp.UseVisualStyleBackColor = true;
            // 
            // buttonResamplerDown
            // 
            this.buttonResamplerDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonResamplerDown.Location = new System.Drawing.Point( 326, 240 );
            this.buttonResamplerDown.Name = "buttonResamplerDown";
            this.buttonResamplerDown.Size = new System.Drawing.Size( 75, 23 );
            this.buttonResamplerDown.TabIndex = 118;
            this.buttonResamplerDown.Text = "Down";
            this.buttonResamplerDown.UseVisualStyleBackColor = true;
            // 
            // buttonResamplerRemove
            // 
            this.buttonResamplerRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonResamplerRemove.Location = new System.Drawing.Point( 326, 158 );
            this.buttonResamplerRemove.Name = "buttonResamplerRemove";
            this.buttonResamplerRemove.Size = new System.Drawing.Size( 75, 23 );
            this.buttonResamplerRemove.TabIndex = 117;
            this.buttonResamplerRemove.Text = "Remove";
            this.buttonResamplerRemove.UseVisualStyleBackColor = true;
            // 
            // buttonResamplerAdd
            // 
            this.buttonResamplerAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonResamplerAdd.Location = new System.Drawing.Point( 326, 129 );
            this.buttonResamplerAdd.Name = "buttonResamplerAdd";
            this.buttonResamplerAdd.Size = new System.Drawing.Size( 75, 23 );
            this.buttonResamplerAdd.TabIndex = 116;
            this.buttonResamplerAdd.Text = "Add";
            this.buttonResamplerAdd.UseVisualStyleBackColor = true;
            // 
            // listResampler
            // 
            this.listResampler.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listResampler.CheckBoxes = true;
            this.listResampler.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPath} );
            this.listResampler.FullRowSelect = true;
            this.listResampler.HideSelection = false;
            this.listResampler.Location = new System.Drawing.Point( 46, 129 );
            this.listResampler.MultiSelect = false;
            this.listResampler.Name = "listResampler";
            this.listResampler.ShowGroups = false;
            this.listResampler.Size = new System.Drawing.Size( 274, 134 );
            this.listResampler.TabIndex = 115;
            this.listResampler.UseCompatibleStateImageBehavior = false;
            this.listResampler.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderPath
            // 
            this.columnHeaderPath.Text = "path";
            this.columnHeaderPath.Width = 280;
            // 
            // lblResampler
            // 
            this.lblResampler.AutoSize = true;
            this.lblResampler.Location = new System.Drawing.Point( 17, 109 );
            this.lblResampler.Name = "lblResampler";
            this.lblResampler.Size = new System.Drawing.Size( 55, 12 );
            this.lblResampler.TabIndex = 111;
            this.lblResampler.Text = "resampler";
            // 
            // chkWavtoolWithWine
            // 
            this.chkWavtoolWithWine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkWavtoolWithWine.AutoSize = true;
            this.chkWavtoolWithWine.Location = new System.Drawing.Point( 46, 79 );
            this.chkWavtoolWithWine.Name = "chkWavtoolWithWine";
            this.chkWavtoolWithWine.Size = new System.Drawing.Size( 153, 16 );
            this.chkWavtoolWithWine.TabIndex = 113;
            this.chkWavtoolWithWine.Text = "Invoke wavtool with Wine";
            this.chkWavtoolWithWine.UseVisualStyleBackColor = true;
            // 
            // btnWavtool
            // 
            this.btnWavtool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWavtool.Location = new System.Drawing.Point( 326, 44 );
            this.btnWavtool.Name = "btnWavtool";
            this.btnWavtool.Size = new System.Drawing.Size( 75, 23 );
            this.btnWavtool.TabIndex = 112;
            this.btnWavtool.Text = "Browse";
            this.btnWavtool.UseVisualStyleBackColor = true;
            // 
            // lblWavtool
            // 
            this.lblWavtool.AutoSize = true;
            this.lblWavtool.Location = new System.Drawing.Point( 17, 25 );
            this.lblWavtool.Name = "lblWavtool";
            this.lblWavtool.Size = new System.Drawing.Size( 44, 12 );
            this.lblWavtool.TabIndex = 114;
            this.lblWavtool.Text = "wavtool";
            // 
            // txtWavtool
            // 
            this.txtWavtool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWavtool.Location = new System.Drawing.Point( 100, 46 );
            this.txtWavtool.Name = "txtWavtool";
            this.txtWavtool.Size = new System.Drawing.Size( 220, 19 );
            this.txtWavtool.TabIndex = 111;
            // 
            // tabUtausingers
            // 
            this.tabUtausingers.AutoScroll = true;
            this.tabUtausingers.Controls.Add( this.btnRemove );
            this.tabUtausingers.Controls.Add( this.btnAdd );
            this.tabUtausingers.Controls.Add( this.btnUp );
            this.tabUtausingers.Controls.Add( this.btnDown );
            this.tabUtausingers.Controls.Add( this.listSingers );
            this.tabUtausingers.Location = new System.Drawing.Point( 4, 38 );
            this.tabUtausingers.Name = "tabUtausingers";
            this.tabUtausingers.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabUtausingers.Size = new System.Drawing.Size( 454, 441 );
            this.tabUtausingers.TabIndex = 6;
            this.tabUtausingers.Text = "UTAU Singers";
            this.tabUtausingers.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Location = new System.Drawing.Point( 98, 401 );
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size( 75, 23 );
            this.btnRemove.TabIndex = 122;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point( 17, 401 );
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size( 75, 23 );
            this.btnAdd.TabIndex = 121;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point( 279, 401 );
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size( 75, 23 );
            this.btnUp.TabIndex = 123;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point( 360, 401 );
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
            listViewGroup1.Name = null;
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
            listViewGroup16.Header = "ListViewGroup";
            listViewGroup16.Name = null;
            listViewGroup17.Header = "ListViewGroup";
            listViewGroup17.Name = null;
            listViewGroup18.Header = "ListViewGroup";
            listViewGroup18.Name = null;
            listViewGroup19.Header = "ListViewGroup";
            listViewGroup19.Name = null;
            listViewGroup20.Header = "ListViewGroup";
            listViewGroup20.Name = null;
            listViewGroup21.Header = "ListViewGroup";
            listViewGroup21.Name = null;
            listViewGroup22.Header = "ListViewGroup";
            listViewGroup22.Name = null;
            listViewGroup23.Header = "ListViewGroup";
            listViewGroup23.Name = null;
            listViewGroup24.Header = "ListViewGroup";
            listViewGroup24.Name = null;
            listViewGroup25.Header = "ListViewGroup";
            listViewGroup25.Name = null;
            listViewGroup26.Header = "ListViewGroup";
            listViewGroup26.Name = null;
            listViewGroup27.Header = "ListViewGroup";
            listViewGroup27.Name = null;
            listViewGroup28.Header = "ListViewGroup";
            listViewGroup28.Name = null;
            listViewGroup29.Header = "ListViewGroup";
            listViewGroup29.Name = null;
            listViewGroup30.Header = "ListViewGroup";
            listViewGroup30.Name = null;
            listViewGroup31.Header = "ListViewGroup";
            listViewGroup31.Name = null;
            listViewGroup32.Header = "ListViewGroup";
            listViewGroup32.Name = null;
            listViewGroup33.Header = "ListViewGroup";
            listViewGroup33.Name = null;
            listViewGroup34.Header = "ListViewGroup";
            listViewGroup34.Name = null;
            listViewGroup35.Header = "ListViewGroup";
            listViewGroup35.Name = null;
            listViewGroup36.Header = "ListViewGroup";
            listViewGroup36.Name = null;
            listViewGroup37.Header = "ListViewGroup";
            listViewGroup37.Name = null;
            listViewGroup38.Header = "ListViewGroup";
            listViewGroup38.Name = null;
            listViewGroup39.Header = "ListViewGroup";
            listViewGroup39.Name = null;
            listViewGroup40.Header = "ListViewGroup";
            listViewGroup40.Name = null;
            listViewGroup41.Header = "ListViewGroup";
            listViewGroup41.Name = null;
            listViewGroup42.Header = "ListViewGroup";
            listViewGroup42.Name = null;
            listViewGroup43.Header = "ListViewGroup";
            listViewGroup43.Name = null;
            listViewGroup44.Header = "ListViewGroup";
            listViewGroup44.Name = null;
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
            listViewGroup15,
            listViewGroup16,
            listViewGroup17,
            listViewGroup18,
            listViewGroup19,
            listViewGroup20,
            listViewGroup21,
            listViewGroup22,
            listViewGroup23,
            listViewGroup24,
            listViewGroup25,
            listViewGroup26,
            listViewGroup27,
            listViewGroup28,
            listViewGroup29,
            listViewGroup30,
            listViewGroup31,
            listViewGroup32,
            listViewGroup33,
            listViewGroup34,
            listViewGroup35,
            listViewGroup36,
            listViewGroup37,
            listViewGroup38,
            listViewGroup39,
            listViewGroup40,
            listViewGroup41,
            listViewGroup42,
            listViewGroup43,
            listViewGroup44} );
            this.listSingers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listSingers.HideSelection = false;
            this.listSingers.Location = new System.Drawing.Point( 17, 23 );
            this.listSingers.MultiSelect = false;
            this.listSingers.Name = "listSingers";
            this.listSingers.Size = new System.Drawing.Size( 418, 372 );
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
            this.tabFile.Size = new System.Drawing.Size( 454, 441 );
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
            this.tabSingingSynth.Controls.Add( this.groupDefaultSynthesizer );
            this.tabSingingSynth.Controls.Add( this.groupSynthesizerDll );
            this.tabSingingSynth.Controls.Add( this.groupVsti );
            this.tabSingingSynth.Location = new System.Drawing.Point( 4, 38 );
            this.tabSingingSynth.Name = "tabSingingSynth";
            this.tabSingingSynth.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabSingingSynth.Size = new System.Drawing.Size( 454, 441 );
            this.tabSingingSynth.TabIndex = 8;
            this.tabSingingSynth.Text = "Synthesizer";
            this.tabSingingSynth.UseVisualStyleBackColor = true;
            // 
            // groupDefaultSynthesizer
            // 
            this.groupDefaultSynthesizer.Controls.Add( this.comboDefaultSynthesizer );
            this.groupDefaultSynthesizer.Location = new System.Drawing.Point( 23, 224 );
            this.groupDefaultSynthesizer.Name = "groupDefaultSynthesizer";
            this.groupDefaultSynthesizer.Size = new System.Drawing.Size( 407, 71 );
            this.groupDefaultSynthesizer.TabIndex = 111;
            this.groupDefaultSynthesizer.TabStop = false;
            this.groupDefaultSynthesizer.Text = "Default Synthesizer";
            // 
            // comboDefaultSynthesizer
            // 
            this.comboDefaultSynthesizer.FormattingEnabled = true;
            this.comboDefaultSynthesizer.Items.AddRange( new object[] {
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
            this.comboDefaultSynthesizer.Location = new System.Drawing.Point( 18, 29 );
            this.comboDefaultSynthesizer.Name = "comboDefaultSynthesizer";
            this.comboDefaultSynthesizer.Size = new System.Drawing.Size( 182, 20 );
            this.comboDefaultSynthesizer.TabIndex = 11;
            // 
            // groupSynthesizerDll
            // 
            this.groupSynthesizerDll.Controls.Add( this.chkLoadAquesTone );
            this.groupSynthesizerDll.Controls.Add( this.chkLoadVocaloid2 );
            this.groupSynthesizerDll.Controls.Add( this.chkLoadVocaloid1 );
            this.groupSynthesizerDll.Location = new System.Drawing.Point( 23, 122 );
            this.groupSynthesizerDll.Name = "groupSynthesizerDll";
            this.groupSynthesizerDll.Size = new System.Drawing.Size( 407, 96 );
            this.groupSynthesizerDll.TabIndex = 109;
            this.groupSynthesizerDll.TabStop = false;
            this.groupSynthesizerDll.Text = "Synthesizer DLL Usage";
            // 
            // chkLoadAquesTone
            // 
            this.chkLoadAquesTone.AutoSize = true;
            this.chkLoadAquesTone.Location = new System.Drawing.Point( 18, 66 );
            this.chkLoadAquesTone.Name = "chkLoadAquesTone";
            this.chkLoadAquesTone.Size = new System.Drawing.Size( 81, 16 );
            this.chkLoadAquesTone.TabIndex = 116;
            this.chkLoadAquesTone.Text = "AquesTone";
            this.chkLoadAquesTone.UseVisualStyleBackColor = true;
            // 
            // chkLoadVocaloid2
            // 
            this.chkLoadVocaloid2.AutoSize = true;
            this.chkLoadVocaloid2.Location = new System.Drawing.Point( 18, 44 );
            this.chkLoadVocaloid2.Name = "chkLoadVocaloid2";
            this.chkLoadVocaloid2.Size = new System.Drawing.Size( 87, 16 );
            this.chkLoadVocaloid2.TabIndex = 115;
            this.chkLoadVocaloid2.Text = "VOCALOID2";
            this.chkLoadVocaloid2.UseVisualStyleBackColor = true;
            // 
            // chkLoadVocaloid1
            // 
            this.chkLoadVocaloid1.AutoSize = true;
            this.chkLoadVocaloid1.Location = new System.Drawing.Point( 18, 22 );
            this.chkLoadVocaloid1.Name = "chkLoadVocaloid1";
            this.chkLoadVocaloid1.Size = new System.Drawing.Size( 87, 16 );
            this.chkLoadVocaloid1.TabIndex = 113;
            this.chkLoadVocaloid1.Text = "VOCALOID1";
            this.chkLoadVocaloid1.UseVisualStyleBackColor = true;
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
            this.btnCancel.Location = new System.Drawing.Point( 374, 506 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 88, 23 );
            this.btnCancel.TabIndex = 201;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 280, 506 );
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
            this.ClientSize = new System.Drawing.Size( 475, 545 );
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
            this.groupUserDefined.ResumeLayout( false );
            this.groupUserDefined.PerformLayout();
            this.groupVocaloidEditorCompatible.ResumeLayout( false );
            this.groupVocaloidEditorCompatible.PerformLayout();
            this.tabAnother.ResumeLayout( false );
            this.tabAnother.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBuffer)).EndInit();
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
            this.groupWine.ResumeLayout( false );
            this.groupWine.PerformLayout();
            this.groupUtauCores.ResumeLayout( false );
            this.groupUtauCores.PerformLayout();
            this.tabUtausingers.ResumeLayout( false );
            this.tabFile.ResumeLayout( false );
            this.tabFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoBackupInterval)).EndInit();
            this.tabSingingSynth.ResumeLayout( false );
            this.groupDefaultSynthesizer.ResumeLayout( false );
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
        private BComboBox comboVibratoLength;
        private BLabel lblVibratoLength;
        private BGroupBox groupVocaloidEditorCompatible;
        private BLabel label3;
        private BLabel lblAutoVibratoType1;
        private BLabel lblAutoVibratoThresholdLength;
        private BCheckBox chkEnableAutoVibrato;
        private BComboBox comboAutoVibratoType1;
        private BLabel label7;
        private BCheckBox chkChasePastEvent;
        private BLabel lblWait;
        private BLabel lblPreSendTime;
        private BLabel lblDefaultSinger;
        private BComboBox comboDefualtSinger;
        private BLabel label12;
        private NumericUpDownEx numWait;
        private NumericUpDownEx numPreSendTime;
        private BLabel label13;
        private BLabel lblResolControlCurve;
        private BLabel label1;
        private BLabel lblResolution;
        private BComboBox comboResolControlCurve;
        private BLabel label2;
        private BComboBox comboLanguage;
        private BLabel lblLanguage;
        private System.Windows.Forms.TabPage tabPlatform;
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
        private System.Windows.Forms.TabPage tabUtausingers;
        private BListView listSingers;
        private BGroupBox groupUtauCores;
        private BLabel lblResampler;
        private BCheckBox chkWavtoolWithWine;
        private BButton btnWavtool;
        private BLabel lblWavtool;
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
        private BComboBox comboMtcMidiInPortNumber;
        private BLabel labelMtcMidiInPort;
        private BCheckBox chkKeepProjectCache;
        private System.Windows.Forms.TabPage tabSingingSynth;
        private BGroupBox groupSynthesizerDll;
        private BCheckBox chkLoadVocaloid1;
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
        private BLabel lblBufferSize;
        private NumericUpDownEx numBuffer;
        private BLabel lblBuffer;
        private BGroupBox groupDefaultSynthesizer;
        private BComboBox comboDefaultSynthesizer;
        private BLabel label6;
        private BGroupBox groupUserDefined;
        private BLabel lblAutoVibratoType;
        private BRadioButton radioVocaloidEditorCompatible;
        private BRadioButton radioUserDefined;
        private BLabel bLabel1;
        private NumberTextBox txtAutoVibratoThresholdLength;
        private BComboBox comboAutoVibratoTypeCustom;
        private BLabel lblAutoVibratoTypeCustom;
        private BListView listResampler;
        private BButton buttonResamplerRemove;
        private BButton buttonResamplerAdd;
        private BButton buttonResamplerUp;
        private BButton buttonResamplerDown;
        private BLabel labelWavtoolPath;
        private System.Windows.Forms.ColumnHeader columnHeaderPath;
        private BLabel labelResamplerWithWine;
        private BGroupBox groupWine;
        private BLabel labelWinePrefix;
        private BButton buttonWinePrefix;
        private BTextBox textWinePrefix;
        private BLabel labelWineTop;
        private BButton buttonWineTop;
        private BTextBox textWineTop;
        private BRadioButton radioWineCustom;
        private BRadioButton radioWineBuiltin;
#endif
        #endregion

    }

#if !JAVA
}
#endif
