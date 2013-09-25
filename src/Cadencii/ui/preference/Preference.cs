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
using System.IO;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.javax.sound.midi;
using cadencii.media;
using cadencii.vsq;
using cadencii.windows.forms;

namespace cadencii
{
    using boolean = System.Boolean;

    partial class Preference : Form
    {
        private static int columnWidthHeaderProgramChange = 60;
        private static int columnWidthHeaderName = 100;
        private static int columnWidthHeaderPath = 250;

        Font m_base_font;
        Font m_screen_font;
        Vector<String> m_program_change = null;
        private PlatformEnum m_platform = PlatformEnum.Windows;
        private Vector<SingerConfig> m_utau_singers = new Vector<SingerConfig>();

        private OpenFileDialog openUtauCore;
        private FontDialog fontDialog;
        private FolderBrowserDialog folderBrowserSingers;

        public Preference()
        {
            InitializeComponent();
            fontDialog = new FontDialog();
            fontDialog.AllowVectorFonts = false;
            fontDialog.AllowVerticalFonts = false;
            fontDialog.FontMustExist = true;
            fontDialog.ShowEffects = false;
            openUtauCore = new OpenFileDialog();

            folderBrowserSingers = new FolderBrowserDialog();
            folderBrowserSingers.ShowNewFolderButton = false;
            applyLanguage();

            comboVibratoLength.Items.Clear();
            foreach ( DefaultVibratoLengthEnum dvl in Enum.GetValues( typeof( DefaultVibratoLengthEnum ) ) ) {
                comboVibratoLength.Items.Add( DefaultVibratoLengthUtil.toString( dvl ) );
            }
            comboVibratoLength.SelectedIndex = 1;

            txtAutoVibratoThresholdLength.Text = "480";

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

            txtVOCALOID1.Text = VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID1 );
            txtVOCALOID2.Text = VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 );

            listSingers.Columns[0].Width = columnWidthHeaderProgramChange;
            listSingers.Columns[1].Width = columnWidthHeaderName;
            listSingers.Columns[2].Width = columnWidthHeaderPath;

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

            numBuffer.Maximum = EditorConfig.MAX_BUFFER_MILLISEC;
            numBuffer.Minimum = EditorConfig.MIN_BUFFER_MILLIXEC;

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

        public DialogResult ShowDialog( System.Windows.Forms.Form parent )
        {
            updateMidiDevice();
            updateCustomVibrato();
            return base.ShowDialog( parent );
        }

        /// <summary>
        /// WINEPREFIXの設定値を取得します
        /// </summary>
        public String getWinePrefix()
        {
            return textWinePrefix.Text;
        }

        /// <summary>
        /// WINEPREFIXの設定値を設定します
        /// </summary>
        public void setWinePrefix( String value )
        {
            textWinePrefix.Text = value;
        }

        /// <summary>
        /// WINETOPの設定値を取得します
        /// </summary>
        public String getWineTop()
        {
            return textWineTop.Text;
        }

        /// <summary>
        /// WINETOPの設定値を設定します
        /// </summary>
        public void setWineTop( String value )
        {
            textWineTop.Text = value;
        }

        /// <summary>
        /// Cadencii付属のWineを使うかどうかを表す設定値を取得します
        /// </summary>
        public boolean isWineBuiltin()
        {
            return radioWineBuiltin.Checked;
        }

        /// <summary>
        /// Cadencii付属のWineを使うかどうかを表す設定値を設定します
        /// </summary>        
        public void setWineBuiltin( boolean value )
        {
            radioWineBuiltin.Checked = value;
            radioWineCustom.Checked = !value;
        }

        /// <summary>
        /// 自動ビブラートを作成するとき，ユーザー定義タイプのビブラートを利用するかどうか，を表す値を取得します
        /// </summary>
        /// <returns></returns>
        public boolean isUseUserDefinedAutoVibratoType()
        {
            return radioUserDefined.Checked;
        }

        /// <summary>
        /// 自動ビブラートを作成するとき，ユーザー定義タイプのビブラートを利用するかどうか，を表す値を設定します
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void setUseUserDefinedAutoVibratoType( boolean value )
        {
            radioUserDefined.Checked = value;
            radioVocaloidEditorCompatible.Checked = !value;
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
            return (int)numBuffer.Value;
        }

        /// <summary>
        /// バッファーサイズの設定値（単位：ミリ秒）を設定します。
        /// </summary>
        /// <param name="value"></param>
        public void setBufferSize( int value )
        {
            if ( value < numBuffer.Minimum ) {
                value = (int)numBuffer.Minimum;
            } else if ( numBuffer.Maximum < value ) {
                value = (int)numBuffer.Maximum;
            }
            numBuffer.Value = value;
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
                return (int)numAutoBackupInterval.Value;
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
                numAutoBackupInterval.Value = value;
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
            return (int)numMouseHoverTime.Value;
        }

        public void setMouseHoverTime( int value )
        {
            numMouseHoverTime.Value = value;
        }

        public int getPxTrackHeight()
        {
            return (int)numTrackHeight.Value;
        }

        public void setPxTrackHeight( int value )
        {
            numTrackHeight.Value = value;
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
            return (int)numMaximumFrameRate.Value;
        }

        public void setMaximumFrameRate( int value )
        {
            numMaximumFrameRate.Value = value;
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
            this.Text = _( "Preference" );
            btnCancel.Text = _( "Cancel" );
            btnOK.Text = _( "OK" );
            openUtauCore.Filter = string.Empty;
            try {
                openUtauCore.Filter = string.Join("|", new[] { _( "Executable(*.exe)|*.exe" ), _( "All Files(*.*)|*.*" ) });
            } catch ( Exception ex ) {
                openUtauCore.Filter = string.Join("|", new[] { "Executable(*.exe)|*.exe", "All Files(*.*)|*.*" });
            }

            folderBrowserSingers.Description = _( "Select Singer Directory" );

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
            lblResolution.Text = _( "Resolution(VSTi)" );
            lblResolControlCurve.Text = _( "Control Curve" );
            lblResolControlCurve.Mnemonic( Keys.C );

            chkEnableAutoVibrato.Text = _("Enable Automatic Vibrato");
            chkEnableAutoVibrato.Mnemonic(Keys.E);
            lblVibratoLength.Text = _( "Default Vibrato Length" );
            lblVibratoLength.Mnemonic( Keys.L );
            lblAutoVibratoThresholdLength.Text = _( "Minimum note length for Automatic Vibrato" );
            lblAutoVibratoThresholdLength.Mnemonic( Keys.M );

            lblAutoVibratoType.Text = _( "Auto Vibrato Type" );
            groupVocaloidEditorCompatible.Text = _( "VOCALOID Editor Compatible" );
            groupUserDefined.Text = _( "User Defined" );
            radioVocaloidEditorCompatible.Text = _( "VOCALOID Editor Compatible" );
            radioUserDefined.Text = _( "User Defined" );
            chkEnableAutoVibrato.Text = _("Enable Automatic Vibrato");
            chkEnableAutoVibrato.Mnemonic(Keys.E);
            lblAutoVibratoType1.Text = _( "Vibrato Type" ) + ": VOCALOID1";
            lblAutoVibratoType1.Mnemonic( Keys.T );
            lblAutoVibratoType2.Text = _( "Vibrato Type" ) + ": VOCALOID2";
            lblAutoVibratoType2.Mnemonic( Keys.T );
            #endregion

            #region tabAnother
            lblDefaultSinger.Text = _( "Default Singer" );
            lblDefaultSinger.Mnemonic( Keys.S );
            lblPreSendTime.Text = _( "Pre-Send time" );
            lblPreSendTime.Mnemonic( Keys.P );
            lblWait.Text = _( "Waiting Time" );
            lblWait.Mnemonic( Keys.W );
            chkChasePastEvent.Text = _("Chase Event");
            chkChasePastEvent.Mnemonic(Keys.C);
            lblBuffer.Text = _( "Buffer Size" );
            lblBuffer.Mnemonic( Keys.B );
            lblBufferSize.Text = "msec(" + EditorConfig.MIN_BUFFER_MILLIXEC + "-" + EditorConfig.MAX_BUFFER_MILLISEC + ")";
            #endregion

            #region tabAppearance
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
            groupPianoroll.Text = _( "Piano Roll" );
            labelWheelOrder.Text = _( "Mouse wheel Rate" );

            chkCursorFix.Text = _( "Fix Song position to Center" );
            chkScrollHorizontal.Text = _( "Horizontal Scroll when Mouse wheel" );
            chkKeepLyricInputMode.Text = _( "Keep Lyric Input Mode" );
            chkPlayPreviewWhenRightClick.Text = _( "Play Preview On Right Click" );
            chkCurveSelectingQuantized.Text = _( "Enable Quantize for Curve Selecting" );
            chkUseSpaceKeyAsMiddleButtonModifier.Text = _( "Use space key as Middle button modifier" );

            groupMisc.Text = _( "Misc" );
            lblMaximumFrameRate.Text = _( "Maximum Frame Rate" );
            lblMilliSecond.Text = _( "frame per second" );
            lblMouseHoverTime.Text = _( "Waiting Time for Preview" );
            lblMidiInPort.Text = _( "MIDI In Port Number" );
            labelMtcMidiInPort.Text = _( "MTC MIDI In Port Number" );
            chkTranslateRoman.Text = _( "Translate Roman letters into Kana" );
            #endregion

            #region tabPlatform
            groupUtauCores.Text = _( "UTAU Cores" );
            labelWavtoolPath.Text = _( "Path:" );
            chkWavtoolWithWine.Text = _( "Invoke wavtool with Wine" );
            listResampler.SetColumnHeaders( new String[] { _( "path" ) } );
            labelResamplerWithWine.Text = _( "Check the box to use Wine" );
            checkEnableWideCharacterWorkaround.Text = _( "Enable Workaround for Wide-Character Path" );
            #endregion

            #region tabUtausingers
            listSingers.SetColumnHeaders( new String[] { _( "Program Change" ), _( "Name" ), _( "Path" ) } );
            btnAdd.Text = _( "Add" );
            btnRemove.Text = _( "Remove" );
            btnUp.Text = _( "Up" );
            btnDown.Text = _( "Down" );
            #endregion

            #region tabFile
            chkAutoBackup.Text = _( "Automatical Backup" );
            lblAutoBackupInterval.Text = _( "interval" );
            lblAutoBackupMinutes.Text = _( "minute(s)" );
            chkKeepProjectCache.Text = _( "Keep Project Cache" );
            #endregion

            #region tabSingingSynth
            groupSynthesizerDll.Text = _( "Synthesizer DLL Usage" );

            groupDefaultSynthesizer.Text = _( "Default Synthesizer" );
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
            return (int)numPreSendTime.Value;
        }

        public void setPreSendTime( int value )
        {
            numPreSendTime.Value = value;
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
                int ret = int.Parse( txtAutoVibratoThresholdLength.Text );
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
            txtAutoVibratoThresholdLength.Text = value + "";
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
            return (int)numericUpDownEx1.Value;
        }

        public void setWheelOrder( int value )
        {
            if ( value < numericUpDownEx1.Minimum ) {
                numericUpDownEx1.Value = numericUpDownEx1.Minimum;
            } else if ( numericUpDownEx1.Maximum < value ) {
                numericUpDownEx1.Value = numericUpDownEx1.Maximum;
            } else {
                numericUpDownEx1.Value = value;
            }
        }

        public Font getScreenFont()
        {
            return m_screen_font;
        }

        public void setScreenFont( Font value )
        {
            m_screen_font = value;
            labelScreenFontName.Text = m_screen_font.getName();
        }

        public java.awt.Font getBaseFont()
        {
            return m_base_font;
        }

        public void setBaseFont( java.awt.Font value )
        {
            m_base_font = value;
            labelMenuFontName.Text = m_base_font.getName();
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
            for ( int i = 0; i < listResampler.Items.Count; i++ ) {
                ret.add( (String)listResampler.Items[i].SubItems[0].Text );
                with_wine.add( listResampler.Items[i].Checked );
            }
        }

        public void setResamplersConfig( Vector<String> path, Vector<Boolean> with_wine )
        {
            int size = listResampler.Items.Count;
            for ( int i = 0; i < size; i++ ) {
                listResampler.Items.RemoveAt( 0 );
            }
            if ( path == null ) {
                return;
            }
            for ( int i = 0; i < path.Count; i++ ) {
                listResampler.AddRow(
                    new String[] { path[i] }, with_wine[i] );
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
            return txtWavtool.Text;
        }

        public void setPathWavtool( String value )
        {
            txtWavtool.Text = value;
        }

        public String getPathAquesTone()
        {
            return txtAquesTone.Text;
        }

        public void setPathAquesTone( String value )
        {
            txtAquesTone.Text = value;
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
            fontDialog.Font = getBaseFont().font;
            if ( fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
                if ( fontDialog.Font != null ) {
                    java.awt.Font f = new java.awt.Font(fontDialog.Font);
                    m_base_font = f;
                    labelMenuFontName.Text = f.getName();
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

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public void btnChangeScreenFont_Click( Object sender, EventArgs e )
        {
            fontDialog.Font = m_screen_font.font;
            if ( fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
                if ( fontDialog.Font != null ) {
                    java.awt.Font f = new java.awt.Font(fontDialog.Font);
                    m_screen_font = f;
                    labelScreenFontName.Text = f.getName();
                }
            }
        }

        public void buttonResamplerAdd_Click( Object sender, EventArgs e )
        {
            openUtauCore.SetSelectedFile("resampler.exe");
            var dr = AppManager.showModalDialog( openUtauCore, true, this );
            if ( dr == System.Windows.Forms.DialogResult.OK ) {
                String path = openUtauCore.FileName;
                boolean check = false;
                boolean is_mac = isMac();
                if ( is_mac ) {
                    check = isWindowsExecutable( path );
                }
                listResampler.AddRow( new String[] { path }, check );
                if ( txtWavtool.Text == "" ) {
                    // wavtoolの欄が空欄だった場合のみ，
                    // wavtoolの候補を登録する(wavtoolがあれば)
                    String wavtool = Path.Combine( PortUtil.getDirectoryName( path ), "wavtool.exe" );
                    if (System.IO.File.Exists(wavtool)) {
                        txtWavtool.Text = wavtool;
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
            int count = listResampler.Items.Count;
            if ( listResampler.SelectedIndices.Count == 0 ) {
                return;
            }
            int index = listResampler.SelectedIndices[0];
            if (index + delta < 0 || count <= index + delta) {
                return;
            }

            String sel = (String)listResampler.Items[index].SubItems[0].Text;
            boolean chk = listResampler.Items[index].Checked;
            listResampler.Items[index].SubItems[0].Text = listResampler.Items[index + delta].SubItems[0].Text;
            listResampler.Items[index].Checked = listResampler.Items[index + delta].Checked;
            listResampler.Items[index + delta].SubItems[0].Text = sel;
            listResampler.Items[index + delta].Checked = chk;
            if (!listResampler.Items[index + delta].Selected) {
                listResampler.SelectedIndices.Clear();
                listResampler.Items[index + delta].Selected = true;
            }
        }

        public void buttonResamplerRemove_Click( Object sender, EventArgs e )
        {
            int count = listResampler.Items.Count;
            if ( listResampler.SelectedIndices.Count == 0 ) {
                return;
            }
            int index = listResampler.SelectedIndices[0];
            listResampler.Items.RemoveAt(index);
            // 選択し直す
            if ( index >= count - 1 ) {
                index--;
            }
            if ( 0 <= index && index < count - 1 ) {
                if (!listResampler.Items[index].Selected) {
                    listResampler.SelectedIndices.Clear();
                    listResampler.Items[index].Selected = true;
                }
            }
        }

        public void btnWavtool_Click( Object sender, EventArgs e )
        {
            if (!txtWavtool.Text.Equals("") && Directory.Exists(PortUtil.getDirectoryName(txtWavtool.Text))) {
                openUtauCore.SetSelectedFile(txtWavtool.Text);
            }
            var dr = AppManager.showModalDialog( openUtauCore, true, this );
            if ( dr == System.Windows.Forms.DialogResult.OK ) {
                String path = openUtauCore.FileName;
                txtWavtool.Text = path;
                boolean is_mac = isMac();
                boolean check = false;
                if ( is_mac ) {
                    check = isWindowsExecutable( path );
                }
                chkWavtoolWithWine.Checked = check;
                if ( listResampler.Items.Count == 0 ) {
                    String resampler = Path.Combine( PortUtil.getDirectoryName( path ), "resampler.exe" );
                    if (System.IO.File.Exists(resampler)) {
                        check = false;
                        if ( is_mac ) {
                            check = isWindowsExecutable( resampler );
                        }
                        listResampler.AddRow( new String[] { resampler }, check );
                    }
                }
            }
        }

        public void btnAquesTone_Click( object sender, EventArgs e ) { onAquesToneChooseButtonClicked( txtAquesTone ); }

        private void btnAquesTone2_Click( object sender, EventArgs e ) { onAquesToneChooseButtonClicked( txtAquesTone2 ); }

        private void onAquesToneChooseButtonClicked( System.Windows.Forms.TextBox text_box )
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (text_box.Text != "" && Directory.Exists(PortUtil.getDirectoryName(text_box.Text))) {
                dialog.SetSelectedFile(text_box.Text);
            }
            var dr = AppManager.showModalDialog( dialog, true, this );
            if ( dr == System.Windows.Forms.DialogResult.OK ) {
                String path = dialog.FileName;
                text_box.Text = path;
            }
        }

        public void btnAdd_Click( Object sender, EventArgs e )
        {
            if ( folderBrowserSingers.ShowDialog( this ) == DialogResult.OK ) {
                String dir = folderBrowserSingers.SelectedPath;
#if DEBUG
                sout.println( "Preference#btnAdd_Click; dir=" + dir );
                sout.println("Preference#btnAdd_Clicl; PortUtil.isDirectoryExists(dir)=" + Directory.Exists(dir));
                sout.println("Preference#btnAdd_Clicl; PortUtil.isFileExists(dir)=" + System.IO.File.Exists(dir));
#endif
                if (!Directory.Exists(dir) && System.IO.File.Exists(dir)) {
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
                m_utau_singers.RemoveAt( index );
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
                if (!listSingers.Items[index + 1].Selected) {
                    listSingers.SelectedIndices.Clear();
                    listSingers.Items[index + 1].Selected = true;
                }
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
                if (!listSingers.Items[index - 1].Selected) {
                    listSingers.SelectedIndices.Clear();
                    listSingers.Items[index - 1].Selected = true;
                }
            }
        }

        public void chkAutoBackup_CheckedChanged( Object sender, EventArgs e )
        {
            numAutoBackupInterval.Enabled = chkAutoBackup.Checked;
        }

        public void Preference_FormClosing( Object sender, FormClosingEventArgs e )
        {
            columnWidthHeaderProgramChange = listSingers.Columns[0].Width;
            columnWidthHeaderName = listSingers.Columns[1].Width;
            columnWidthHeaderPath = listSingers.Columns[2].Width;
        }

        public void btnCancel_Click( Object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public void commonChangeAutoVibratoType( Object sender, EventArgs e )
        {
            boolean v = radioVocaloidEditorCompatible.Checked;
            boolean ud = radioUserDefined.Checked;
            groupVocaloidEditorCompatible.Enabled = v;
            groupUserDefined.Enabled = ud;
            comboAutoVibratoType1.Enabled = v;
            comboAutoVibratoType2.Enabled = v;
            comboAutoVibratoTypeCustom.Enabled = ud;
            lblAutoVibratoType1.Enabled = v;
            lblAutoVibratoType2.Enabled = v;
            lblAutoVibratoTypeCustom.Enabled = ud;
        }

        public void buttonWinePrefix_Click( Object sender, EventArgs e )
        {
            OpenFileDialog dialog = null;
            try {
                dialog = new OpenFileDialog();
                String dir = textWinePrefix.Text;
                if ( dir != null && dir.Length > 0 ) {
                    dialog.SetSelectedFile(Path.Combine( dir, "a" ));
                }
                if ( AppManager.showModalDialog( dialog, true, this ) == DialogResult.OK ) {
                    dir = dialog.FileName;
                    if (System.IO.File.Exists(dir)) {
                        // ファイルが選ばれた場合，その所属ディレクトリを値として用いる
                        dir = PortUtil.getDirectoryName( dir );
                    }
                    textWinePrefix.Text = dir;
                }
            } catch ( Exception ex ) {
            }
        }

        public void buttonWineTop_Click( Object sender, EventArgs e )
        {
            OpenFileDialog dialog = null;
            try {
                dialog = new OpenFileDialog();
                String dir = textWineTop.Text;
                if ( dir != null && dir.Length > 0 ) {
                    dialog.SetSelectedFile(Path.Combine( dir, "a" ));
                }
                if ( AppManager.showModalDialog( dialog, true, this ) == DialogResult.OK ) {
                    dir = dialog.FileName;
                    if (System.IO.File.Exists(dir)) {
                        // ファイルが選ばれた場合，その所属ディレクトリを値として用いる
                        dir = PortUtil.getDirectoryName( dir );
                    }
                    textWineTop.Text = dir;
                }
            } catch ( Exception ex ) {
            }
        }

        public void radioWineBuiltin_CheckedChanged( Object sender, EventArgs e )
        {
            boolean enable = !radioWineBuiltin.Checked;
            textWineTop.Enabled = enable;
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
            if (!System.IO.File.Exists(path)) {
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
            if ( midiins.Count <= 0 ) {
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
            var f = this.Font;
            if ( f == null ) {
                return;
            }
            Font font = new Font( font_name, java.awt.Font.PLAIN, (int)f.SizeInPoints );
            Util.applyFontRecurse( this, font );
        }

        private void UpdateUtausingerList()
        {
            listSingers.Items.Clear();
            for ( int i = 0; i < m_utau_singers.size(); i++ ) {
                m_utau_singers.get( i ).Program = i;
                listSingers.AddRow(
                    new String[] { 
                        m_utau_singers.get( i ).Program + "",
                        m_utau_singers.get( i ).VOICENAME, 
                        m_utau_singers.get( i ).VOICEIDSTR } );
            }
        }

        private int getUtausingersSelectedIndex()
        {
            if (listSingers.SelectedIndices.Count == 0) {
                return -1;
            } else {
                return listSingers.SelectedIndices[0];
            }
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
