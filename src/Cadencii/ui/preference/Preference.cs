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
using System.Windows.Forms;
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

            comboVibratoLength.Items.Clear();
            foreach ( DefaultVibratoLengthEnum dvl in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) ) {
                comboVibratoLength.Items.Add( DefaultVibratoLengthUtil.toString( dvl ) );
            }
            comboVibratoLength.SelectedIndex = 1;

            txtAutoVibratoThresholdLength.setText( "480" );

            comboAutoVibratoType1.Items.Clear();
            for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( SynthesizerType.VOCALOID1 ); itr.hasNext(); ) {
                VibratoHandle vconfig = itr.next();
                comboAutoVibratoType1.Items.Add( vconfig );
            }
            if ( comboAutoVibratoType1.Items.Count > 0 ) {
                comboAutoVibratoType1.SelectedIndex = 0;
            }

            comboAutoVibratoType2.Items.Clear();
            for ( Iterator<VibratoHandle> itr = VocaloSysUtil.vibratoConfigIterator( SynthesizerType.VOCALOID2 ); itr.hasNext(); ) {
                VibratoHandle vconfig = itr.next();
                comboAutoVibratoType2.Items.Add( vconfig );
            }
            if ( comboAutoVibratoType2.Items.Count > 0 ) {
                comboAutoVibratoType2.SelectedIndex = 0;
            }

            updateCustomVibrato();

            comboResolControlCurve.Items.Clear();
            for ( Iterator<ClockResolution> itr = ClockResolutionUtility.iterator(); itr.hasNext(); ) {
                ClockResolution cr = itr.next();
                comboResolControlCurve.Items.Add( ClockResolutionUtility.toString( cr ) );
            }
            comboResolControlCurve.SelectedIndex = 0;

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
            comboDefaultSynthesizer.SelectedIndex = 0;

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
            return checkEnableWideCharacterWorkaround.Checked;
        }

        /// <summary>
        /// UseWideCharacterWorkaroundの設定値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setEnableWideCharacterWorkaround( boolean value )
        {
            checkEnableWideCharacterWorkaround.Checked = value;
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
            int c = comboDefaultSynthesizer.Items.Count;
            int select_indx = 0;
            for ( int i = 0; i < c; i++ ) {
                String str = (String)comboDefaultSynthesizer.Items[i];
                RendererKind p = RendererKindUtil.fromString( str );
                if ( p == value ) {
                    select_indx = i;
                    break;
                }
            }
            comboDefaultSynthesizer.SelectedIndex = select_indx;
        }

        /// <summary>
        /// デフォルトの音声合成システムを取得します
        /// </summary>
        /// <returns></returns>
        public RendererKind getDefaultSynthesizer()
        {
            String selstr = (String)comboDefaultSynthesizer.SelectedItem;
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
            if ( chkLoadVocaloid1.Enabled ) {
                return chkLoadVocaloid1.Checked;
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
            if ( chkLoadVocaloid1.Enabled ) {
                chkLoadVocaloid1.Checked = value;
            }
        }

        /// <summary>
        /// VOCALOID2 DLLを読み込むかどうかを表すブール値を取得します
        /// </summary>
        /// <returns></returns>
        public boolean isVocaloid2Required()
        {
            return chkLoadVocaloid2.Checked;
        }

        /// <summary>
        /// VOCALOID2 DLLを読み込むかどうかを表すブール値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setVocaloid2Required( boolean value )
        {
            chkLoadVocaloid2.Checked = value;
        }

        /// <summary>
        /// AquesTone DLLを読み込むかどうかを表すブール値を取得します
        /// </summary>
        /// <returns></returns>
        public boolean isAquesToneRequired()
        {
            return chkLoadAquesTone.Checked;
        }

        /// <summary>
        /// AquesTone DLLを読み込むかどうかを表すブール値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setAquesToneRequired( boolean value )
        {
            chkLoadAquesTone.Checked = value;
        }

        public bool isAquesTone2Required() { return chkLoadAquesTone2.Checked; }
        public void setAquesTone2Requried( bool value ) { chkLoadAquesTone2.Checked = value; }

        public boolean isUseProjectCache()
        {
            return chkKeepProjectCache.Checked;
        }

        public void setUseProjectCache( boolean value )
        {
            chkKeepProjectCache.Checked = value;
        }

        public boolean isUseSpaceKeyAsMiddleButtonModifier()
        {
            return chkUseSpaceKeyAsMiddleButtonModifier.Checked;
        }

        public void setUseSpaceKeyAsMiddleButtonModifier( boolean value )
        {
            chkUseSpaceKeyAsMiddleButtonModifier.Checked = value;
        }

        public int getAutoBackupIntervalMinutes()
        {
            if ( chkAutoBackup.Checked ) {
                return (int)numAutoBackupInterval.getFloatValue();
            } else {
                return 0;
            }
        }

        public void setAutoBackupIntervalMinutes( int value )
        {
            if ( value <= 0 ) {
                chkAutoBackup.Checked = false;
            } else {
                chkAutoBackup.Checked = true;
                numAutoBackupInterval.setFloatValue( value );
            }
        }

        public boolean isSelfDeRomantization()
        {
            return chkTranslateRoman.Checked;
        }

        public void setSelfDeRomantization( boolean value )
        {
            chkTranslateRoman.Checked = value;
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
            if ( comboMidiInPortNumber.Enabled ) {
                if ( 0 <= value && value < comboMidiInPortNumber.Items.Count ) {
                    comboMidiInPortNumber.SelectedIndex = value;
                } else {
                    comboMidiInPortNumber.SelectedIndex = 0;
                }
            }
        }
#endif

        public boolean isCurveVisibleVel()
        {
            return chkVel.Checked;
        }

        public void setCurveVisibleVel( boolean value )
        {
            chkVel.Checked = value;
        }

        public boolean isCurveVisibleAccent()
        {
            return chkAccent.Checked;
        }

        public void setCurveVisibleAccent( boolean value )
        {
            chkAccent.Checked = value;
        }

        public boolean isCurveVisibleDecay()
        {
            return chkDecay.Checked;
        }

        public void setCurveVisibleDecay( boolean value )
        {
            chkDecay.Checked = value;
        }

        public boolean isCurveVisibleVibratoRate()
        {
            return chkVibratoRate.Checked;
        }

        public void setCurveVisibleVibratoRate( boolean value )
        {
            chkVibratoRate.Checked = value;
        }

        public boolean isCurveVisibleVibratoDepth()
        {
            return chkVibratoDepth.Checked;
        }

        public void setCurveVisibleVibratoDepth( boolean value )
        {
            chkVibratoDepth.Checked = value;
        }

        public boolean isCurveVisibleDyn()
        {
            return chkDyn.Checked;
        }

        public void setCurveVisibleDyn( boolean value )
        {
            chkDyn.Checked = value;
        }

        public boolean isCurveVisibleBre()
        {
            return chkBre.Checked;
        }

        public void setCurveVisibleBre( boolean value )
        {
            chkBre.Checked = value;
        }

        public boolean isCurveVisibleBri()
        {
            return chkBri.Checked;
        }

        public void setCurveVisibleBri( boolean value )
        {
            chkBri.Checked = value;
        }

        public boolean isCurveVisibleCle()
        {
            return chkCle.Checked;
        }

        public void setCurveVisibleCle( boolean value )
        {
            chkCle.Checked = value;
        }

        public boolean isCurveVisibleOpe()
        {
            return chkOpe.Checked;
        }

        public void setCurveVisibleOpe( boolean value )
        {
            chkOpe.Checked = value;
        }

        public boolean isCurveVisiblePor()
        {
            return chkPor.Checked;
        }

        public void setCurveVisiblePor( boolean value )
        {
            chkPor.Checked = value;
        }

        public boolean isCurveVisibleGen()
        {
            return chkGen.Checked;
        }

        public void setCurveVisibleGen( boolean value )
        {
            chkGen.Checked = value;
        }

        public boolean isCurveVisiblePit()
        {
            return chkPit.Checked;
        }

        public void setCurveVisiblePit( boolean value )
        {
            chkPit.Checked = value;
        }

        public boolean isCurveVisiblePbs()
        {
            return chkPbs.Checked;
        }

        public void setCurveVisiblePbs( boolean value )
        {
            chkPbs.Checked = value;
        }

        public boolean isCurveVisibleFx2Depth()
        {
            return chkFx2Depth.Checked;
        }

        public void setCurveVisibleFx2Depth( boolean value )
        {
            chkFx2Depth.Checked = value;
        }

        public boolean isCurveVisibleHarmonics()
        {
            return chkHarmonics.Checked;
        }

        public void setCurveVisibleHarmonics( boolean value )
        {
            chkHarmonics.Checked = value;
        }

        public boolean isCurveVisibleReso1()
        {
            return chkReso1.Checked;
        }

        public void setCurveVisibleReso1( boolean value )
        {
            chkReso1.Checked = value;
        }

        public boolean isCurveVisibleReso2()
        {
            return chkReso2.Checked;
        }

        public void setCurveVisibleReso2( boolean value )
        {
            chkReso2.Checked = value;
        }

        public boolean isCurveVisibleReso3()
        {
            return chkReso3.Checked;
        }

        public void setCurveVisibleReso3( boolean value )
        {
            chkReso3.Checked = value;
        }

        public boolean isCurveVisibleReso4()
        {
            return chkReso4.Checked;
        }

        public void setCurveVisibleReso4( boolean value )
        {
            chkReso4.Checked = value;
        }

        public boolean isCurveVisibleEnvelope()
        {
            return chkEnvelope.Checked;
        }

        public void setCurveVisibleEnvelope( boolean value )
        {
            chkEnvelope.Checked = value;
        }

        public boolean isCurveSelectingQuantized()
        {
            return chkCurveSelectingQuantized.Checked;
        }

        public void setCurveSelectingQuantized( boolean value )
        {
            chkCurveSelectingQuantized.Checked = value;
        }

        public boolean isPlayPreviewWhenRightClick()
        {
            return chkPlayPreviewWhenRightClick.Checked;
        }

        public void setPlayPreviewWhenRightClick( boolean value )
        {
            chkPlayPreviewWhenRightClick.Checked = value;
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
            return chkKeepLyricInputMode.Checked;
        }

        public void setKeepLyricInputMode( boolean value )
        {
            chkKeepLyricInputMode.Checked = value;
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
            return chkScrollHorizontal.Checked;
        }

        public void setScrollHorizontalOnWheel( boolean value )
        {
            chkScrollHorizontal.Checked = value;
        }

        public void applyLanguage()
        {
            setTitle( _( "Preference" ) );
            btnCancel.Text = _( "Cancel" );
            btnOK.Text = _( "OK" );
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

            chkEnableAutoVibrato.Text = BMenuItem.setMnemonicFromText(_("Enable Automatic Vibrato"), KeyEvent.VK_E);
            lblVibratoLength.setText( _( "Default Vibrato Length" ) );
            lblVibratoLength.setMnemonic( KeyEvent.VK_L, comboVibratoLength );
            lblAutoVibratoThresholdLength.setText( _( "Minimum note length for Automatic Vibrato" ) );
            lblAutoVibratoThresholdLength.setMnemonic( KeyEvent.VK_M, txtAutoVibratoThresholdLength );

            lblAutoVibratoType.setText( _( "Auto Vibrato Type" ) );
            groupVocaloidEditorCompatible.setTitle( _( "VOCALOID Editor Compatible" ) );
            groupUserDefined.setTitle( _( "User Defined" ) );
            radioVocaloidEditorCompatible.setText( _( "VOCALOID Editor Compatible" ) );
            radioUserDefined.setText( _( "User Defined" ) );
            chkEnableAutoVibrato.Text = BMenuItem.setMnemonicFromText(_("Enable Automatic Vibrato"), KeyEvent.VK_E);
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
            chkChasePastEvent.Text = BMenuItem.setMnemonicFromText(_("Chase Event"), KeyEvent.VK_C);
            lblBuffer.setText( _( "Buffer Size" ) );
            lblBuffer.setMnemonic( KeyEvent.VK_B, numBuffer );
            lblBufferSize.setText( "msec(" + EditorConfig.MIN_BUFFER_MILLIXEC + "-" + EditorConfig.MAX_BUFFER_MILLISEC + ")" );
            #endregion

            #region tabAppearance
            groupFont.setTitle( _( "Font" ) );
            labelMenu.setText( _( "Menu / Lyrics" ) );
            labelScreen.setText( _( "Screen" ) );
            lblLanguage.setText( _( "UI Language" ) );
            btnChangeMenuFont.Text = _( "Change" );
            btnChangeScreenFont.Text = _( "Change" );
            lblTrackHeight.setText( _( "Track Height (pixel)" ) );
            groupVisibleCurve.setTitle( _( "Visible Control Curve" ) );
            #endregion

            #region tabOperation
            groupPianoroll.setTitle( _( "Piano Roll" ) );
            labelWheelOrder.setText( _( "Mouse wheel Rate" ) );

            chkCursorFix.Text = _( "Fix Song position to Center" );
            chkScrollHorizontal.Text = _( "Horizontal Scroll when Mouse wheel" );
            chkKeepLyricInputMode.Text = _( "Keep Lyric Input Mode" );
            chkPlayPreviewWhenRightClick.Text = _( "Play Preview On Right Click" );
            chkCurveSelectingQuantized.Text = _( "Enable Quantize for Curve Selecting" );
            chkUseSpaceKeyAsMiddleButtonModifier.Text = _( "Use space key as Middle button modifier" );

            groupMisc.setTitle( _( "Misc" ) );
            lblMaximumFrameRate.setText( _( "Maximum Frame Rate" ) );
            lblMilliSecond.setText( _( "frame per second" ) );
            lblMouseHoverTime.setText( _( "Waiting Time for Preview" ) );
            lblMidiInPort.setText( _( "MIDI In Port Number" ) );
            labelMtcMidiInPort.setText( _( "MTC MIDI In Port Number" ) );
            chkTranslateRoman.Text = _( "Translate Roman letters into Kana" );
            #endregion

            #region tabPlatform
            groupUtauCores.setTitle( _( "UTAU Cores" ) );
            labelWavtoolPath.setText( _( "Path:" ) );
            chkWavtoolWithWine.Text = _( "Invoke wavtool with Wine" );
            listResampler.setColumnHeaders( new String[] { _( "path" ) } );
            labelResamplerWithWine.setText( _( "Check the box to use Wine" ) );
            checkEnableWideCharacterWorkaround.Text = _( "Enable Workaround for Wide-Character Path" );
            #endregion

            #region tabUtausingers
            listSingers.setColumnHeaders( new String[] { _( "Program Change" ), _( "Name" ), _( "Path" ) } );
            btnAdd.Text = _( "Add" );
            btnRemove.Text = _( "Remove" );
            btnUp.Text = _( "Up" );
            btnDown.Text = _( "Down" );
            #endregion

            #region tabFile
            chkAutoBackup.Text = _( "Automatical Backup" );
            lblAutoBackupInterval.setText( _( "interval" ) );
            lblAutoBackupMinutes.setText( _( "minute(s)" ) );
            chkKeepProjectCache.Text = _( "Keep Project Cache" );
            #endregion

            #region tabSingingSynth
            groupSynthesizerDll.setTitle( _( "Synthesizer DLL Usage" ) );

            groupDefaultSynthesizer.setTitle( _( "Default Synthesizer" ) );
            #endregion
        }

        public String getLanguage()
        {
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

        /// <summary>
        /// コントロールカーブの時間解像度の設定値を取得します
        /// </summary>
        /// <returns>コントロールカーブの時間解像度の設定値</returns>
        public ClockResolution getControlCurveResolution()
        {
            int count = -1;
            int index = comboResolControlCurve.SelectedIndex;
            for ( Iterator<ClockResolution> itr = ClockResolutionUtility.iterator(); itr.hasNext(); ) {
                ClockResolution vt = itr.next();
                count++;
                if ( count == index ) {
                    return vt;
                }
            }
            comboResolControlCurve.SelectedIndex = 0;
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
                    comboResolControlCurve.SelectedIndex = count;
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
            return chkEnableAutoVibrato.Checked;
        }

        public void setEnableAutoVibrato( boolean value )
        {
            chkEnableAutoVibrato.Checked = value;
        }

        public String getAutoVibratoType1()
        {
            int count = -1;
            int index = comboAutoVibratoType1.SelectedIndex;
            if ( 0 <= index ) {
                VibratoHandle vconfig = (VibratoHandle)comboAutoVibratoType1.SelectedItem;
                return vconfig.IconID;
            } else {
                return "$04040001";
            }
        }

        public void setAutoVibratoType1( String value )
        {
            for ( int i = 0; i < comboAutoVibratoType1.Items.Count; i++ ) {
                VibratoHandle vconfig = (VibratoHandle)comboAutoVibratoType1.Items[i];
                if ( vconfig.IconID.Equals( value ) ) {
                    comboAutoVibratoType1.SelectedIndex = i;
                    return;
                }
            }
            if ( comboAutoVibratoType1.Items.Count > 0 ) {
                comboAutoVibratoType1.SelectedIndex = 0;
            }
        }

        public String getAutoVibratoType2()
        {
            int count = -1;
            int index = comboAutoVibratoType2.SelectedIndex;
            if ( 0 <= index ) {
                VibratoHandle vconfig = (VibratoHandle)comboAutoVibratoType2.SelectedItem;
                return vconfig.IconID;
            } else {
                return "$04040001";
            }
        }

        public void setAutoVibratoType2( String value )
        {
            for ( int i = 0; i < comboAutoVibratoType2.Items.Count; i++ ) {
                VibratoHandle vconfig = (VibratoHandle)comboAutoVibratoType2.Items[i];
                if ( vconfig.IconID.Equals( value ) ) {
                    comboAutoVibratoType2.SelectedIndex = i;
                    return;
                }
            }
            if ( comboAutoVibratoType2.Items.Count > 0 ) {
                comboAutoVibratoType2.SelectedIndex = 0;
            }
        }

        public String getAutoVibratoTypeCustom()
        {
            int count = -1;
            int index = comboAutoVibratoTypeCustom.SelectedIndex;
            if ( 0 <= index ) {
                VibratoHandle vconfig = (VibratoHandle)comboAutoVibratoTypeCustom.SelectedItem;
                return vconfig.IconID;
            } else {
                return "$04040001";
            }
        }

        public void setAutoVibratoTypeCustom( String icon_id )
        {
            for ( int i = 0; i < comboAutoVibratoTypeCustom.Items.Count; i++ ) {
                VibratoHandle handle = (VibratoHandle)comboAutoVibratoTypeCustom.Items[i];
                if ( handle.IconID.Equals( icon_id ) ) {
                    comboAutoVibratoTypeCustom.SelectedIndex = i;
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
            int index = comboVibratoLength.SelectedIndex;
            foreach ( DefaultVibratoLengthEnum vt in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) ) {
                count++;
                if ( index == count ) {
                    return vt;
                }
            }
            comboVibratoLength.SelectedIndex = 1;
            return DefaultVibratoLengthEnum.L66;
        }

        public void setDefaultVibratoLength( DefaultVibratoLengthEnum value )
        {
            int count = -1;
            foreach ( DefaultVibratoLengthEnum dvl in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) ) {
                count++;
                if ( dvl == value ) {
                    comboVibratoLength.SelectedIndex = count;
                    break;
                }
            }
        }

        public boolean isCursorFixed()
        {
            return chkCursorFix.Checked;
        }

        public void setCursorFixed( boolean value )
        {
            chkCursorFix.Checked = value;
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
            if ( comboDefualtSinger.SelectedIndex >= 0 ) {
                return m_program_change.get( comboDefualtSinger.SelectedIndex );
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
                comboDefualtSinger.SelectedIndex = index;
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
            chkWavtoolWithWine.Checked = value;
        }

        public boolean isWavtoolWithWine()
        {
            return chkWavtoolWithWine.Checked;
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
        public void btnChangeMenuFont_Click( Object sender, EventArgs e )
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

        public void btnOK_Click( Object sender, EventArgs e )
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

        public void btnChangeScreenFont_Click( Object sender, EventArgs e )
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

        public void buttonResamplerAdd_Click( Object sender, EventArgs e )
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
                        chkWavtoolWithWine.Checked = check;
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

        public void btnWavtool_Click( Object sender, EventArgs e )
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
                chkWavtoolWithWine.Checked = check;
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

        public void btnAdd_Click( Object sender, EventArgs e )
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

        public void listSingers_SelectedIndexChanged( Object sender, EventArgs e )
        {
            int index = getUtausingersSelectedIndex();
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

        public void btnRemove_Click( Object sender, EventArgs e )
        {
            int index = getUtausingersSelectedIndex();
            if ( 0 <= index && index < m_utau_singers.size() ) {
                m_utau_singers.removeElementAt( index );
            }
            UpdateUtausingerList();
        }

        public void btnDown_Click( Object sender, EventArgs e )
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

        public void btnUp_Click( Object sender, EventArgs e )
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

        public void chkAutoBackup_CheckedChanged( Object sender, EventArgs e )
        {
            numAutoBackupInterval.setEnabled( chkAutoBackup.Checked );
        }

        public void Preference_FormClosing( Object sender, FormClosingEventArgs e )
        {
            columnWidthHeaderProgramChange = listSingers.getColumnWidth( 0 );
            columnWidthHeaderName = listSingers.getColumnWidth( 1 );
            columnWidthHeaderPath = listSingers.getColumnWidth( 2 );
        }

        public void btnCancel_Click( Object sender, EventArgs e )
        {
            setDialogResult( BDialogResult.CANCEL );
        }

        public void commonChangeAutoVibratoType( Object sender, EventArgs e )
        {
            boolean v = radioVocaloidEditorCompatible.isSelected();
            boolean ud = radioUserDefined.isSelected();
            groupVocaloidEditorCompatible.setEnabled( v );
            groupUserDefined.setEnabled( ud );
            comboAutoVibratoType1.Enabled = v;
            comboAutoVibratoType2.Enabled = v;
            comboAutoVibratoTypeCustom.Enabled = ud;
            lblAutoVibratoType1.setEnabled( v );
            lblAutoVibratoType2.setEnabled( v );
            lblAutoVibratoTypeCustom.setEnabled( ud );
        }

        public void buttonWinePrefix_Click( Object sender, EventArgs e )
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

        public void buttonWineTop_Click( Object sender, EventArgs e )
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

        public void radioWineBuiltin_CheckedChanged( Object sender, EventArgs e )
        {
            boolean enable = !radioWineBuiltin.isSelected();
            textWineTop.setEnabled( enable );
            buttonWineTop.Enabled = enable;
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
            comboAutoVibratoTypeCustom.Items.Clear();
            for ( int i = 0; i < size; i++ ) {
                VibratoHandle handle = AppManager.editorConfig.AutoVibratoCustom.get( i );
                comboAutoVibratoTypeCustom.Items.Add( handle );
            }
        }

        /// <summary>
        /// MIDIデバイスの選択肢の欄を更新します
        /// </summary>
        private void updateMidiDevice()
        {
            int sel_midi = comboMidiInPortNumber.SelectedIndex;
            int sel_mtc = comboMtcMidiInPortNumber.SelectedIndex;

            comboMidiInPortNumber.Items.Clear();
            comboMtcMidiInPortNumber.Items.Clear();
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
                comboMidiInPortNumber.Items.Add( info );
                comboMtcMidiInPortNumber.Items.Add( info );
            }
            if ( vec.size( midiins ) <= 0 ) {
                comboMtcMidiInPortNumber.Enabled = false;
                comboMidiInPortNumber.Enabled = false;
            } else {
#if ENABLE_MTC
                comboMtcMidiInPortNumber.setEnabled( true );
#else // ENABLE_MTC
                comboMtcMidiInPortNumber.Enabled = false;
#endif // ENABLE_MTC
                comboMidiInPortNumber.Enabled = true;
            }
#else // ENABLE_MIDI
            comboMtcMidiInPortNumber.setEnabled( false );
            comboMidiInPortNumber.setEnabled( false );
#endif // ENABLE_MIDI

            // 可能なら選択状態を復帰
            if ( sel_midi >= 0 ) {
                if ( comboMidiInPortNumber.Items.Count <= sel_midi ) {
                    sel_midi = comboMidiInPortNumber.Items.Count - 1;
                }
                comboMidiInPortNumber.SelectedIndex = sel_midi;
            }

            if ( sel_mtc >= 0 ) {
                if ( comboMtcMidiInPortNumber.Items.Count <= sel_mtc ) {
                    sel_mtc = comboMtcMidiInPortNumber.Items.Count - 1;
                }
                comboMtcMidiInPortNumber.SelectedIndex = sel_mtc;
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
            btnChangeScreenFont.Click += new EventHandler( btnChangeScreenFont_Click );
            btnChangeMenuFont.Click += new EventHandler( btnChangeMenuFont_Click );
            btnWavtool.Click += new EventHandler( btnWavtool_Click );
            buttonResamplerAdd.Click += new EventHandler( buttonResamplerAdd_Click );
            buttonResamplerRemove.Click += new EventHandler( buttonResamplerRemove_Click );
            buttonResamplerUp.Click += new EventHandler( buttonResamplerUpDown_Click );
            buttonResamplerDown.Click += new EventHandler( buttonResamplerUpDown_Click );
            btnAquesTone.Click += new EventHandler( btnAquesTone_Click );
            btnRemove.Click += new EventHandler( btnRemove_Click );
            btnAdd.Click += new EventHandler( btnAdd_Click );
            btnUp.Click += new EventHandler( btnUp_Click );
            btnDown.Click += new EventHandler( btnDown_Click );
            listSingers.SelectedIndexChanged += new EventHandler( listSingers_SelectedIndexChanged );
            chkAutoBackup.CheckedChanged += new EventHandler( chkAutoBackup_CheckedChanged );
            btnOK.Click += new EventHandler( btnOK_Click );
            this.FormClosing += new FormClosingEventHandler( Preference_FormClosing );
            btnCancel.Click += new EventHandler( btnCancel_Click );
            radioVocaloidEditorCompatible.CheckedChanged += new EventHandler( commonChangeAutoVibratoType );
            buttonWinePrefix.Click += new EventHandler( buttonWinePrefix_Click );
            buttonWineTop.Click += new EventHandler( buttonWineTop_Click );
            radioWineBuiltin.CheckedChanged += new EventHandler( radioWineBuiltin_CheckedChanged );
        }

        private void setResources()
        {
        }
        #endregion

    }

}
