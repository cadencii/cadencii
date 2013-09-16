/*
 * Preference.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Linq;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.java.awt.event_;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.javax.sound.midi;
using cadencii.media;
using cadencii.vsq;
using cadencii.windows.forms;

namespace cadencii
{
    using BEventArgs = System.EventArgs;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
    using BEventHandler = System.EventHandler;
    using BFormClosingEventHandler = System.Windows.Forms.FormClosingEventHandler;
    using boolean = System.Boolean;

    partial class Preference : BDialog
    {
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
#if DEBUG
        private BFileChooser folderBrowserSingers;
#else
        private BFolderBrowser folderBrowserSingers;
#endif

        public Preference()
        {
            InitializeComponent();
            fontDialog = new BFontChooser();
            fontDialog.dialog.AllowVectorFonts = false;
            fontDialog.dialog.AllowVerticalFonts = false;
            fontDialog.dialog.FontMustExist = true;
            fontDialog.dialog.ShowEffects = false;
            openUtauCore = new BFileChooser();

#if DEBUG
            folderBrowserSingers = new BFileChooser();
#else
            folderBrowserSingers = new BFolderBrowser();
            folderBrowserSingers.setNewFolderButtonVisible( false );
#endif
            applyLanguage();

            comboVibratoLength.removeAllItems();
            foreach ( DefaultVibratoLengthEnum dvl in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) ) {
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
            comboDefaultSynthesizer.Items.Clear();
            (from kind
                in Enum.GetValues( typeof( RendererKind ) ).Cast<RendererKind>()
                where kind != RendererKind.NULL
                select kind.getString()
            )
            .Distinct()
            .OrderBy( ( kind ) => kind ).ToList()
            .ForEach( ( kind ) => comboDefaultSynthesizer.Items.Add( kind ) );
            comboDefaultSynthesizer.setSelectedIndex( 0 );

            numBuffer.setMaximum( EditorConfig.MAX_BUFFER_MILLISEC );
            numBuffer.setMinimum( EditorConfig.MIN_BUFFER_MILLIXEC );

            registerEventHandlers();
            setResources();
        }

        #region public methods
        /// <summary>
        /// UseWideCharacterWorkaroundに対する設定値を取得します
        /// </summary>
        /// <returns></returns>
        public boolean isEnableWideCharacterWorkaround()
        {
            return checkEnableWideCharacterWorkaround.isSelected();
        }

        /// <summary>
        /// UseWideCharacterWorkaroundの設定値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setEnableWideCharacterWorkaround( boolean value )
        {
            checkEnableWideCharacterWorkaround.setSelected( value );
        }

        public override BDialogResult showDialog( System.Windows.Forms.Form parent )
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
            foreach ( RendererKind p in Enum.GetValues( typeof( RendererKind ) ) ) {
                String str = p.getString();
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

        public bool isAquesTone2Required() { return chkLoadAquesTone2.Checked; }
        public void setAquesTone2Requried( bool value ) { chkLoadAquesTone2.Checked = value; }

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

#if DEBUG
            folderBrowserSingers.setDialogTitle( _( "Select Singer Directory" ) );
#else
            folderBrowserSingers.setDescription( _( "Select Singer Directory" ) );
#endif

            #region tabのタイトル
            tabSequence.Text = _( "Sequence" );
            tabAnother.Text = _( "Other" );
            tabAppearance.Text = _( "Appearance" );
            tabOperation.Text = _( "Operation" );
            tabPlatform.Text = _( "Platform" );
            tabUtausingers.Text = _( "UTAU Singers" );
            tabFile.Text = _( "File" );
            tabSingingSynth.Text = _( "Synthesizer" );
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

            chkCursorFix.setText( _( "Fix Song position to Center" ) );
            chkScrollHorizontal.setText( _( "Horizontal Scroll when Mouse wheel" ) );
            chkKeepLyricInputMode.setText( _( "Keep Lyric Input Mode" ) );
            chkPlayPreviewWhenRightClick.setText( _( "Play Preview On Right Click" ) );
            chkCurveSelectingQuantized.setText( _( "Enable Quantize for Curve Selecting" ) );
            chkUseSpaceKeyAsMiddleButtonModifier.setText( _( "Use space key as Middle button modifier" ) );

            groupMisc.setTitle( _( "Misc" ) );
            lblMaximumFrameRate.setText( _( "Maximum Frame Rate" ) );
            lblMilliSecond.setText( _( "frame per second" ) );
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
            checkEnableWideCharacterWorkaround.setText( _( "Enable Workaround for Wide-Character Path" ) );
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
            foreach ( DefaultVibratoLengthEnum vt in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) ) {
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
            foreach ( DefaultVibratoLengthEnum dvl in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) ) {
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
                    new String[] { vec.get( path, i ) }, vec.get( with_wine, i ) );
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

        public string getPathAquesTone2() { return txtAquesTone2.Text; }

        public void setPathAquesTone2( string value ) { txtAquesTone2.Text = value; }

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
            if ( AppManager.editorConfig.DoNotUseVocaloid2 != (!isVocaloid2Required()) ) {
                was_modified = true;
            }
            if ( AppManager.editorConfig.DoNotUseVocaloid1 != (!isVocaloid1Required()) ) {
                was_modified = true;
            }
            if ( was_modified ) {
                AppManager.showMessageBox( _( "Restart Cadencii to complete your changes\n(restart will NOT be automatically done)" ),
                                           "Cadencii",
                                           cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                                           cadencii.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE );
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
                if ( is_mac ) {
                    check = isWindowsExecutable( path );
                }
                listResampler.addRow( new String[] { path }, check );
                if ( str.compare( txtWavtool.getText(), "" ) ) {
                    // wavtoolの欄が空欄だった場合のみ，
                    // wavtoolの候補を登録する(wavtoolがあれば)
                    String wavtool = fsys.combine( PortUtil.getDirectoryName( path ), "wavtool.exe" );
                    if ( fsys.isFileExists( wavtool ) ) {
                        txtWavtool.setText( wavtool );
                        check = false;
                        if ( is_mac ) {
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
            if ( index >= count - 1 ) {
                index--;
            }
            if ( 0 <= index && index < count - 1 ) {
                listResampler.setSelectedRow( index );
            }
        }

        public void btnWavtool_Click( Object sender, BEventArgs e )
        {
            if ( !txtWavtool.getText().Equals( "" ) && fsys.isDirectoryExists( PortUtil.getDirectoryName( txtWavtool.getText() ) ) ) {
                openUtauCore.setSelectedFile( txtWavtool.getText() );
            }
            int dr = AppManager.showModalDialog( openUtauCore, true, this );
            if ( dr == BFileChooser.APPROVE_OPTION ) {
                String path = openUtauCore.getSelectedFile();
                txtWavtool.setText( path );
                boolean is_mac = isMac();
                boolean check = false;
                if ( is_mac ) {
                    check = isWindowsExecutable( path );
                }
                chkWavtoolWithWine.setSelected( check );
                if ( listResampler.getItemCountRow() == 0 ) {
                    String resampler = fsys.combine( PortUtil.getDirectoryName( path ), "resampler.exe" );
                    if ( fsys.isFileExists( resampler ) ) {
                        check = false;
                        if ( is_mac ) {
                            check = isWindowsExecutable( resampler );
                        }
                        listResampler.addRow( new String[] { resampler }, check );
                    }
                }
            }
        }

        public void btnAquesTone_Click( object sender, EventArgs e ) { onAquesToneChooseButtonClicked( txtAquesTone ); }

        private void btnAquesTone2_Click( object sender, EventArgs e ) { onAquesToneChooseButtonClicked( txtAquesTone2 ); }

        private void onAquesToneChooseButtonClicked( System.Windows.Forms.TextBox text_box )
        {
            BFileChooser dialog = new BFileChooser();
            if ( text_box.Text != "" && fsys.isDirectoryExists( PortUtil.getDirectoryName( text_box.Text ) ) ) {
                dialog.setSelectedFile( text_box.Text );
            }
            int dr = AppManager.showModalDialog( dialog, true, this );
            if ( dr == BFileChooser.APPROVE_OPTION ) {
                String path = dialog.getSelectedFile();
                text_box.Text = path;
            }
        }

        public void btnAdd_Click( Object sender, BEventArgs e )
        {
#if DEBUG
            if ( folderBrowserSingers.showOpenDialog( this ) == BFileChooser.APPROVE_OPTION ) {
                String dir = folderBrowserSingers.getSelectedFile();
#else
            if ( folderBrowserSingers.showDialog( this ) == BDialogResult.OK ) {
                String dir = folderBrowserSingers.getSelectedPath();
#endif
#if DEBUG
                sout.println( "Preference#btnAdd_Click; dir=" + dir );
                sout.println( "Preference#btnAdd_Clicl; PortUtil.isDirectoryExists(dir)=" + fsys.isDirectoryExists( dir ) );
                sout.println( "Preference#btnAdd_Clicl; PortUtil.isFileExists(dir)=" + fsys.isFileExists( dir ) );
#endif
                if ( !fsys.isDirectoryExists( dir ) && fsys.isFileExists( dir ) ) {
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
            try {
                dialog = new BFileChooser();
                String dir = textWinePrefix.getText();
                if ( dir != null && str.length( dir ) > 0 ) {
                    dialog.setSelectedFile( fsys.combine( dir, "a" ) );
                }
                if ( AppManager.showModalDialog( dialog, true, this ) == BFileChooser.APPROVE_OPTION ) {
                    dir = dialog.getSelectedFile();
                    if ( fsys.isFileExists( dir ) ) {
                        // ファイルが選ばれた場合，その所属ディレクトリを値として用いる
                        dir = PortUtil.getDirectoryName( dir );
                    }
                    textWinePrefix.setText( dir );
                }
            } catch ( Exception ex ) {
            }
        }

        public void buttonWineTop_Click( Object sender, BEventArgs e )
        {
            BFileChooser dialog = null;
            try {
                dialog = new BFileChooser();
                String dir = textWineTop.getText();
                if ( dir != null && str.length( dir ) > 0 ) {
                    dialog.setSelectedFile( fsys.combine( dir, "a" ) );
                }
                if ( AppManager.showModalDialog( dialog, true, this ) == BFileChooser.APPROVE_OPTION ) {
                    dir = dialog.getSelectedFile();
                    if ( fsys.isFileExists( dir ) ) {
                        // ファイルが選ばれた場合，その所属ディレクトリを値として用いる
                        dir = PortUtil.getDirectoryName( dir );
                    }
                    textWineTop.setText( dir );
                }
            } catch ( Exception ex ) {
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
            return false;
        }

        private boolean isWindowsExecutable( String path )
        {
            if ( !fsys.isFileExists( path ) ) {
                return false;
            }
            RandomAccessFile fs = null;
            try {
                fs = new RandomAccessFile( path, "r" );
                int r0 = fs.read(); // 'M'
                int r1 = fs.read(); // 'Z'
                if ( 'M' == (char)r0 && 'Z' == (char)r1 ) {
                    return true;
                }
            } catch ( Exception ex ) {
                serr.println( "Preference#isWindowsExecutable; ex=" + ex );
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
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
            foreach ( MidiDevice.Info info in MidiSystem.getMidiDeviceInfo() ) {
#if DEBUG
                if ( info != null ) {
                    sout.println( "Preference#updateMidiDevice; info.getName()=" + info.getName() );
                }
#endif
                MidiDevice device = null;
                try {
                    device = MidiSystem.getMidiDevice( info );
                } catch ( Exception ex ) {
                    device = null;
                }
                if ( device == null ) continue;
#if DEBUG
                sout.println( "Preference#updateMidiDevice; (device is Receiver)=" + (device is Receiver) );
#endif
                // MIDI-OUTの最大接続数．-1は制限なしを表す
                int max = device.getMaxTransmitters();
                if ( max > 0 || max == -1 ) {
                    midiins.add( info );
                }
            }

            foreach ( MidiDevice.Info info in midiins ) {
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

    }

}
