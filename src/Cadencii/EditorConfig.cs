/*
 * EditorConfig.cs
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
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using cadencii;
using cadencii.java.awt;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.windows.forms;
using cadencii.xml;
using cadencii.vsq;
using cadencii.apputil;



namespace cadencii
{

    /// <summary>
    /// Cadenciiの環境設定
    /// </summary>
    public class EditorConfig
    {
        /// <summary>
        /// デフォルトで使用する歌手の名前
        /// </summary>
        public string DefaultSingerName = "Miku";
        /// <summary>
        /// デフォルトの横軸方向のスケール
        /// </summary>
        public int DefaultXScale = 65;
        public string BaseFontName = "MS UI Gothic";
        public float BaseFontSize = 9.0f;
        public string ScreenFontName = "MS UI Gothic";
        public int WheelOrder = 20;
        public bool CursorFixed = false;
        /// <summary>
        /// RecentFilesに登録することの出来る最大のファイル数
        /// </summary>
        public int NumRecentFiles = 5;
        /// <summary>
        /// 最近使用したファイルのリスト
        /// </summary>
        public List<string> RecentFiles = new List<string>();
        public int DefaultPMBendDepth = 8;
        public int DefaultPMBendLength = 0;
        public int DefaultPMbPortamentoUse = 3;
        public int DefaultDEMdecGainRate = 50;
        public int DefaultDEMaccent = 50;
        /// <summary>
        /// ピアノロール上に歌詞を表示するかどうか
        /// </summary>
        public bool ShowLyric = true;
        /// <summary>
        /// ピアノロール上に，ビブラートとアタックの概略を表す波線を表示するかどうか
        /// </summary>
        public bool ShowExpLine = true;
        public DefaultVibratoLengthEnum DefaultVibratoLength = DefaultVibratoLengthEnum.L66;
        /// <summary>
        /// デフォルトビブラートのRate
        /// バージョン3.3で廃止
        /// </summary>
        [Obsolete]
        private int __revoked__DefaultVibratoRate = 64;
        /// <summary>
        /// デフォルトビブラートのDepth
        /// バージョン3.3で廃止
        /// </summary>
        [Obsolete]
        private int __revoked__DefaultVibratoDepth = 64;
        /// <summary>
        /// ビブラートの自動追加を行うかどうかを決める音符長さの閾値．単位はclock
        /// <version>3.3+</version>
        /// </summary>
        public int AutoVibratoThresholdLength = 480;
        /// <summary>
        /// VOCALOID1用のデフォルトビブラート設定
        /// </summary>
        public string AutoVibratoType1 = "$04040001";
        /// <summary>
        /// VOCALOID2用のデフォルトビブラート設定
        /// </summary>
        public string AutoVibratoType2 = "$04040001";
        /// <summary>
        /// カスタムのデフォルトビブラート設定
        /// <version>3.3+</version>
        /// </summary>
        public string AutoVibratoTypeCustom = "$04040001";
        /// <summary>
        /// ユーザー定義のビブラート設定．
        /// <version>3.3+</version>
        /// </summary>
        public List<VibratoHandle> AutoVibratoCustom = new List<VibratoHandle>();
        /// <summary>
        /// ビブラートの自動追加を行うかどうか
        /// </summary>
        public bool EnableAutoVibrato = true;
        /// <summary>
        /// ピアノロール上での，音符の表示高さ(ピクセル)
        /// </summary>
        public int PxTrackHeight = 14;
        public int MouseDragIncrement = 50;
        public int MouseDragMaximumRate = 600;
        /// <summary>
        /// ミキサーウィンドウが表示された状態かどうか
        /// </summary>
        public bool MixerVisible = false;
        /// <summary>
        /// アイコンパレットが表示された状態かどうか
        /// <version>3.3+</version>
        /// </summary>
        public bool IconPaletteVisible = false;
        public int PreSendTime = 500;
        public ClockResolution ControlCurveResolution = ClockResolution.L30;
        /// <summary>
        /// 言語設定
        /// </summary>
        public string Language = "";
        /// <summary>
        /// マウスの操作などの許容範囲。プリメジャーにPxToleranceピクセルめり込んだ入力を行っても、エラーにならない。(補正はされる)
        /// </summary>
        public int PxTolerance = 10;
        /// <summary>
        /// マウスホイールでピアノロールを水平方向にスクロールするかどうか。
        /// </summary>
        public bool ScrollHorizontalOnWheel = true;
        /// <summary>
        /// 画面描画の最大フレームレート
        /// </summary>
        public int MaximumFrameRate = 15;
        /// <summary>
        /// ユーザー辞書のOn/Offと順序
        /// </summary>
        public List<string> UserDictionaries = new List<string>();
        /// <summary>
        /// 実行環境
        /// </summary>
        private PlatformEnum __revoked__Platform = PlatformEnum.Windows;
        /// <summary>
        /// ウィンドウが最大化された状態かどうか
        /// </summary>
        public bool WindowMaximized = false;
        /// <summary>
        /// ウィンドウの位置とサイズ．
        /// 最小化された状態での値は，この変数に代入されない(ことになっている)
        /// </summary>
        public Rectangle WindowRect = new Rectangle(0, 0, 970, 718);
        /// <summary>
        /// hScrollのスクロールボックスの最小幅(px)
        /// </summary>
        public int MinimumScrollHandleWidth = 20;
        /// <summary>
        /// 発音記号入力モードを，維持するかどうか
        /// </summary>
        public bool KeepLyricInputMode = false;
        /// <summary>
        /// ピアノロールの何もないところをクリックした場合、右クリックでもプレビュー音を再生するかどうか
        /// </summary>
        public bool PlayPreviewWhenRightClick = false;
        /// <summary>
        /// ゲームコントローラで、異なるイベントと識別する最小の時間間隔(millisec)
        /// </summary>
        public int GameControlerMinimumEventInterval = 100;
        /// <summary>
        /// カーブの選択範囲もクオンタイズするかどうか
        /// </summary>
        public bool CurveSelectingQuantized = true;

        private QuantizeMode m_position_quantize = QuantizeMode.p32;
        private bool m_position_quantize_triplet = false;
        private QuantizeMode m_length_quantize = QuantizeMode.p32;
        private bool m_length_quantize_triplet = false;
        private int m_mouse_hover_time = 500;
        /// <summary>
        /// Button index of "△"
        /// </summary>
        public int GameControlerTriangle = 0;
        /// <summary>
        /// Button index of "○"
        /// </summary>
        public int GameControlerCircle = 1;
        /// <summary>
        /// Button index of "×"
        /// </summary>
        public int GameControlerCross = 2;
        /// <summary>
        /// Button index of "□"
        /// </summary>
        public int GameControlerRectangle = 3;
        /// <summary>
        /// Button index of "L1"
        /// </summary>
        public int GameControlL1 = 4;
        /// <summary>
        /// Button index of "R1"
        /// </summary>
        public int GameControlR1 = 5;
        /// <summary>
        /// Button index of "L2"
        /// </summary>
        public int GameControlL2 = 6;
        /// <summary>
        /// Button index of "R2"
        /// </summary>
        public int GameControlR2 = 7;
        /// <summary>
        /// Button index of "SELECT"
        /// </summary>
        public int GameControlSelect = 8;
        /// <summary>
        /// Button index of "START"
        /// </summary>
        public int GameControlStart = 9;
        /// <summary>
        /// Button index of Left Stick
        /// </summary>
        public int GameControlStirckL = 10;
        /// <summary>
        /// Button index of Right Stick
        /// </summary>
        public int GameControlStirckR = 11;
        public bool CurveVisibleVelocity = true;
        public bool CurveVisibleAccent = true;
        public bool CurveVisibleDecay = true;
        public bool CurveVisibleVibratoRate = true;
        public bool CurveVisibleVibratoDepth = true;
        public bool CurveVisibleDynamics = true;
        public bool CurveVisibleBreathiness = true;
        public bool CurveVisibleBrightness = true;
        public bool CurveVisibleClearness = true;
        public bool CurveVisibleOpening = true;
        public bool CurveVisibleGendorfactor = true;
        public bool CurveVisiblePortamento = true;
        public bool CurveVisiblePit = true;
        public bool CurveVisiblePbs = true;
        public bool CurveVisibleHarmonics = false;
        public bool CurveVisibleFx2Depth = false;
        public bool CurveVisibleReso1 = false;
        public bool CurveVisibleReso2 = false;
        public bool CurveVisibleReso3 = false;
        public bool CurveVisibleReso4 = false;
        public bool CurveVisibleEnvelope = false;
        public int GameControlPovRight = 9000;
        public int GameControlPovLeft = 27000;
        public int GameControlPovUp = 0;
        public int GameControlPovDown = 18000;
        /// <summary>
        /// wave波形を表示するかどうか
        /// </summary>
        public bool ViewWaveform = false;
        /// <summary>
        /// スプリットコンテナのディバイダの位置
        /// <version>3.3+</version>
        /// </summary>
        public int SplitContainer2LastDividerLocation = -1;
        /// <summary>
        /// キーボードからの入力に使用するデバイス
        /// </summary>
        public MidiPortConfig MidiInPort = new MidiPortConfig();

        public bool ViewAtcualPitch = false;
        public List<SingerConfig> UtauSingers = new List<SingerConfig>();
        /// <summary>
        /// UTAU互換の合成器のパス(1個目)
        /// </summary>
        public string PathResampler = "";
        /// <summary>
        /// UTAU互換の合成器のパス(2個目以降)
        /// </summary>
        public List<string> PathResamplers = new List<string>();
        /// <summary>
        /// UTAU用のwave切り貼りツール
        /// </summary>
        public string PathWavtool = "";
        /// <summary>
        /// ベジエ制御点を掴む時の，掴んだと判定する際の誤差．制御点の外輪からPxToleranceBezierピクセルずれていても，掴んだと判定する．
        /// </summary>
        public int PxToleranceBezier = 10;
        /// <summary>
        /// 歌詞入力においてローマ字が入力されたとき，Cadencii側でひらがなに変換するかどうか
        /// </summary>
        public bool SelfDeRomanization = false;
        /// <summary>
        /// openMidiDialogで最後に選択された拡張子
        /// </summary>
        public string LastUsedExtension = ".vsq";
        /// <summary>
        /// ミキサーダイアログを常に手前に表示するかどうか
        /// 3.3で廃止
        /// </summary>
        private bool __revoked__MixerTopMost = true;
        public List<ValuePairOfStringArrayOfKeys> ShortcutKeys = new List<ValuePairOfStringArrayOfKeys>();
        public PropertyPanelState PropertyWindowStatus = new PropertyPanelState();
        /// <summary>
        /// 概観ペインが表示されているかどうか
        /// </summary>
        public bool OverviewEnabled = false;
        public int OverviewScaleCount = 5;
        public FormMidiImExportConfig MidiImExportConfigExport = new FormMidiImExportConfig();
        public FormMidiImExportConfig MidiImExportConfigImport = new FormMidiImExportConfig();
        public FormMidiImExportConfig MidiImExportConfigImportVsq = new FormMidiImExportConfig();
        /// <summary>
        /// 自動バックアップする間隔．単位は分
        /// </summary>
        public int AutoBackupIntervalMinutes = 10;
        /// <summary>
        /// 鍵盤の表示幅、ピクセル,AppManager.keyWidthに代入。
        /// </summary>
        public int KeyWidth = 136;
        /// <summary>
        /// スペースキーを押しながら左クリックで、中ボタンクリックとみなす動作をさせるかどうか。
        /// </summary>
        public bool UseSpaceKeyAsMiddleButtonModifier = false;
        /// <summary>
        /// AquesToneのVSTi dllへのパス
        /// </summary>
        public string PathAquesTone = "";
        /// <summary>
        /// AquesTone2 の VSTi dll へのパス
        /// </summary>
        public string PathAquesTone2 = "";
        /// <summary>
        /// アイコンパレット・ウィンドウの位置
        /// </summary>
        public XmlPoint FormIconPaletteLocation = new XmlPoint(0, 0);
        /// <summary>
        /// アイコンパレット・ウィンドウを常に手前に表示するかどうか
        /// 3.3で廃止
        /// </summary>
        private bool __revoked__FormIconTopMost = true;
        /// <summary>
        /// 最初に戻る、のショートカットキー
        /// </summary>
        [XmlArrayItem("BKeys")]
        public Keys[] SpecialShortcutGoToFirst = new Keys[] { Keys.Home };
        /// <summary>
        /// waveファイル出力時のチャンネル数（1または2）
        /// 3.3で廃止
        /// </summary>
        private int __revoked__WaveFileOutputChannel = 2;
        /// <summary>
        /// waveファイル出力時に、全トラックをmixして出力するかどうか
        /// 3.3で廃止
        /// </summary>
        private bool __revoked__WaveFileOutputFromMasterTrack = false;
        /// <summary>
        /// MTCスレーブ動作を行う際使用するMIDI INポートの設定
        /// </summary>
        public MidiPortConfig MidiInPortMtc = new MidiPortConfig();
        /// <summary>
        /// プロジェクトごとのキャッシュディレクトリを使うかどうか
        /// </summary>
        public bool UseProjectCache = true;
        /// <summary>
        /// 鍵盤用のキャッシュが無いとき、FormGenerateKeySoundを表示しないかどうか。
        /// trueなら表示しない、falseなら表示する（デフォルト）
        /// </summary>
        public bool DoNotAskKeySoundGeneration = false;
        /// <summary>
        /// VOCALOID1 (1.0)のDLLを読み込まない場合true。既定ではfalse
        /// 3.3で廃止
        /// </summary>
        private bool __revoked__DoNotUseVocaloid100 = false;
        /// <summary>
        /// VOCALOID1 (1.1)のDLLを読み込まない場合true。既定ではfalse
        /// 3.3で廃止
        /// </summary>
        private bool __revoked__DoNotUseVocaloid101 = false;
        /// <summary>
        /// VOCALOID2のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        public bool DoNotUseVocaloid2 = false;
        /// <summary>
        /// AquesToneのDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        public bool DoNotUseAquesTone = false;
        /// <summary>
        /// AquesTone2のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        public bool DoNotUseAquesTone2 = false;
        /// <summary>
        /// 2個目のVOCALOID1 DLLを読み込むかどうか。既定ではfalse
        /// 3.3で廃止
        /// </summary>
        private bool __revoked__LoadSecondaryVocaloid1Dll = false;
        /// <summary>
        /// VOALOID1のDLLを読み込まない場合はtrue．既定ではfalse
        /// </summary>
        public bool DoNotUseVocaloid1 = false;
        /// <summary>
        /// WAVE再生時のバッファーサイズ。既定では1000ms。
        /// </summary>
        public int BufferSizeMilliSeconds = 1000;
        /// <summary>
        /// トラックを新規作成するときのデフォルトの音声合成システム
        /// </summary>
        public RendererKind DefaultSynthesizer
#if ENABLE_VOCALOID
 = RendererKind.VOCALOID2;
#else
            = RendererKind.VCNT;
#endif
        /// <summary>
        /// 自動ビブラートを作成するとき，ユーザー定義タイプのビブラートを利用するかどうか．デフォルトではfalse
        /// </summary>
        public bool UseUserDefinedAutoVibratoType = false;
        /// <summary>
        /// 再生中に画面を描画するかどうか。デフォルトはfalse
        /// <version>3.3+</version>
        /// </summary>
        public bool SkipDrawWhilePlaying = false;
        /// <summary>
        /// ピアノロール画面の縦方向のスケール.
        /// <verssion>3.3+</verssion>
        /// </summary>
        public int PianoRollScaleY = 0;
        /// <summary>
        /// ファイル・ツールバーのサイズ
        /// <version>3.3+</version>
        /// </summary>
        public int BandSizeFile = 236;
        /// <summary>
        /// ツール・ツールバーのサイズ
        /// <version>3.3+</version>
        /// </summary>
        public int BandSizeTool = 712;
        /// <summary>
        /// メジャー・ツールバーのサイズ
        /// <version>3.3+</version>
        /// </summary>
        public int BandSizeMeasure = 714;
        /// <summary>
        /// ポジション・ツールバーのサイズ
        /// <version>3.3+</version>
        /// </summary>
        public int BandSizePosition = 234;
        /// <summary>
        /// ファイル・ツールバーを新しい行に追加するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public bool BandNewRowFile = false;
        /// <summary>
        /// ツール・ツールバーを新しい行に追加するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public bool BandNewRowTool = false;
        /// <summary>
        /// メジャー・ツールバーを新しい行に追加するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public bool BandNewRowMeasure = false;
        /// <summary>
        /// ポジション・ツールバーを新しい行に追加するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public bool BandNewRowPosition = true;
        /// <summary>
        /// ファイル・ツールバーの順番
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int BandOrderFile = 0;
        /// <summary>
        /// ツール・ツールバーの順番
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int BandOrderTool = 1;
        /// <summary>
        /// メジャー・ツールバーの順番
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int BandOrderMeasure = 3;
        /// <summary>
        /// ポジション・ツールバーの順番
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int BandOrderPosition = 2;
        /// <summary>
        /// ツールバーのChevronの幅．
        /// Winodws 7(Aero): 17px
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int ChevronWidth = 17;
        /// <summary>
        /// 最後に入力したファイルパスのリスト
        /// リストに入る文字列は，拡張子+タブ文字+パスの形式にする
        /// 拡張子はピリオドを含めない
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public List<string> LastUsedPathIn = new List<string>();
        /// <summary>
        /// 最後に出力したファイルパスのリスト
        /// リストに入る文字列は，拡張子+タブ文字+パスの形式にする
        /// 拡張子はピリオドを含めない
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public List<string> LastUsedPathOut = new List<string>();
        /// <summary>
        /// UTAUのresampler用に，ジャンクション機能を使うかどうか
        /// version 3.3+
        /// </summary>
        public bool UseWideCharacterWorkaround = false;
        public bool DoNotAutomaticallyCheckForUpdates = false;

        /// <summary>
        /// バッファーサイズに設定できる最大値
        /// </summary>
        public const int MAX_BUFFER_MILLISEC = 1000;
        /// <summary>
        /// バッファーサイズに設定できる最小値
        /// </summary>
        public const int MIN_BUFFER_MILLIXEC = 100;
        /// <summary>
        /// ピアノロールの縦軸の拡大率を表す整数値の最大値
        /// </summary>
        public const int MAX_PIANOROLL_SCALEY = 10;
        /// <summary>
        /// ピアノロールの縦軸の拡大率を表す整数値の最小値
        /// </summary>
        public const int MIN_PIANOROLL_SCALEY = -4;

        #region static fields
        private static cadencii.xml.XmlSerializer s_serializer = new cadencii.xml.XmlSerializer(typeof(EditorConfig));
        #endregion

        /// <summary>
        /// PositionQuantize, PositionQuantizeTriplet, LengthQuantize, LengthQuantizeTripletの描くプロパティのいずれかが
        /// 変更された時発生します
        /// </summary>
        public static event EventHandler QuantizeModeChanged;

        /// <summary>
        /// コンストラクタ．起動したOSによって動作を変える場合がある
        /// </summary>
        public EditorConfig()
        {
            // デフォルトのフォントを，システムのメニューフォントと同じにする
            System.Drawing.Font f = System.Windows.Forms.SystemInformation.MenuFont;
            if (f != null) {
                this.BaseFontName = f.Name;
                this.ScreenFontName = f.Name;
            }

            // 言語設定を，システムのデフォルトの言語を用いる
            this.Language = Messaging.getRuntimeLanguageName();
        }

        #region public static method
        /// <summary>
        /// EditorConfigのインスタンスをxmlシリアライズするためのシリアライザを取得します
        /// </summary>
        /// <returns></returns>
        public static cadencii.xml.XmlSerializer getSerializer()
        {
            return s_serializer;
        }
        #endregion

        #region private static method
        private static string getLastUsedPathCore(List<string> list, string extension)
        {
            if (extension == null) return "";
            if (PortUtil.getStringLength(extension) <= 0) return "";
            if (extension.Equals(".")) return "";

            if (extension.StartsWith(".")) {
                extension = extension.Substring(1);
            }

            int c = list.Count;
            for (int i = 0; i < c; i++) {
                string s = list[i];
                if (s.StartsWith(extension)) {
                    string[] spl = PortUtil.splitString(s, '\t');
                    if (spl.Length >= 2) {
                        return spl[1];
                    }
                    break;
                }
            }
            return "";
        }

        private static void setLastUsedPathCore(List<string> list, string path, string ext_with_dot)
        {
            string extension = ext_with_dot;
            if (extension == null) return;
            if (extension.Equals(".")) return;
            if (extension.StartsWith(".")) {
                extension = extension.Substring(1);
            }

            int c = list.Count;
            string entry = extension + "\t" + path;
            for (int i = 0; i < c; i++) {
                string s = list[i];
                if (s.StartsWith(extension)) {
                    list[i] = entry;
                    return;
                }
            }
            list.Add(entry);
        }
        #endregion

        #region public method
        /// <summary>
        /// 音符イベントに，デフォルトの歌唱スタイルを適用します
        /// </summary>
        /// <param name="item"></param>
        public void applyDefaultSingerStyle(VsqID item)
        {
            if (item == null) return;
            item.PMBendDepth = this.DefaultPMBendDepth;
            item.PMBendLength = this.DefaultPMBendLength;
            item.PMbPortamentoUse = this.DefaultPMbPortamentoUse;
            item.DEMdecGainRate = this.DefaultDEMdecGainRate;
            item.DEMaccent = this.DefaultDEMaccent;
        }

        /// <summary>
        /// 登録されているUTAU互換合成器の個数を調べます
        /// </summary>
        /// <returns></returns>
        public int getResamplerCount()
        {
            int ret = PathResamplers.Count;
            if (!PathResampler.Equals("")) {
                ret++;
            }
            return ret;
        }

        /// <summary>
        /// 登録されているUTAU互換合成器の登録を全て解除します
        /// </summary>
        public void clearResampler()
        {
            PathResamplers.Clear();
            PathResampler = "";
        }

        /// <summary>
        /// 第index番目に登録されているUTAU互換合成器のパスを取得します
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string getResamplerAt(int index)
        {
            if (index == 0) {
                return PathResampler;
            } else {
                index--;
                if (0 <= index && index < PathResamplers.Count) {
                    return PathResamplers[index];
                }
            }
            return "";
        }

        public IEnumerable<string> resamplers()
        {
            yield return PathResampler;
            foreach (var path in PathResamplers) {
                yield return path;
            }
            yield break;
        }

        /// <summary>
        /// 第index番目のUTAU互換合成器のパスを設定します
        /// </summary>
        /// <param name="index"></param>
        /// <param name="path"></param>
        public void setResamplerAt(int index, string path)
        {
            if (path == null) {
                return;
            }
            if (path.Equals("")) {
                return;
            }
            if (index == 0) {
                PathResampler = path;
            } else {
                index--;
                if (0 <= index && index < PathResamplers.Count) {
                    PathResamplers[index] = path;
                }
            }
        }

        /// <summary>
        /// 第index番目のUTAU互換合成器を登録解除します
        /// </summary>
        /// <param name="index"></param>
        public void removeResamplerAt(int index)
        {
            int size = PathResamplers.Count;
            if (index == 0) {
                if (size > 0) {
                    PathResampler = PathResamplers[0];
                    for (int i = 0; i < size - 1; i++) {
                        PathResamplers[i] = PathResamplers[i + 1];
                    }
                    PathResamplers.RemoveAt(size - 1);
                } else {
                    PathResampler = "";
                }
            } else {
                index--;
                if (0 <= index && index < size) {
                    for (int i = 0; i < size - 1; i++) {
                        PathResamplers[i] = PathResamplers[i + 1];
                    }
                    PathResamplers.RemoveAt(size - 1);
                }
            }
        }

        /// <summary>
        /// 新しいUTAU互換合成器のパスを登録します
        /// </summary>
        /// <param name="path"></param>
        public void addResampler(string path)
        {
            int count = getResamplerCount();
            if (count == 0) {
                PathResampler = path;
            } else {
                PathResamplers.Add(path);
            }
        }

        /// <summary>
        /// 最後に出力したファイルのパスのうち，拡張子が指定したものと同じであるものを取得します
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string getLastUsedPathIn(string extension)
        {
            string ret = getLastUsedPathCore(LastUsedPathIn, extension);
            if (ret.Equals("")) {
                return getLastUsedPathCore(LastUsedPathOut, extension);
            }
            /*if ( !ret.Equals( "" ) ) {
                ret = PortUtil.getDirectoryName( ret );
            }*/
            return ret;
        }

        /// <summary>
        /// 最後に出力したファイルのパスを設定します
        /// </summary>
        /// <param name="path"></param>
        public void setLastUsedPathIn(string path, string ext_with_dot)
        {
            setLastUsedPathCore(LastUsedPathIn, path, ext_with_dot);
        }

        /// <summary>
        /// 最後に入力したファイルのパスのうち，拡張子が指定したものと同じであるものを取得します．
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string getLastUsedPathOut(string extension)
        {
            string ret = getLastUsedPathCore(LastUsedPathOut, extension);
            if (ret.Equals("")) {
                ret = getLastUsedPathCore(LastUsedPathIn, extension);
            }
            /*if ( !ret.Equals( "" ) ) {
                ret = PortUtil.getDirectoryName( ret );
            }*/
            return ret;
        }

        /// <summary>
        /// 最後に入力したファイルのパスを設定します
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ext_with_dot">ピリオド付きの拡張子（ex. ".txt"）</param>
        public void setLastUsedPathOut(string path, string ext_with_dot)
        {
            setLastUsedPathCore(LastUsedPathOut, path, ext_with_dot);
        }

        /// <summary>
        /// 自動ビブラートを作成します
        /// </summary>
        /// <param name="type"></param>
        /// <param name="vibrato_clocks"></param>
        /// <returns></returns>
        public VibratoHandle createAutoVibrato(SynthesizerType type, int vibrato_clocks)
        {
            if (UseUserDefinedAutoVibratoType) {
                if (AutoVibratoCustom == null) {
                    AutoVibratoCustom = new List<VibratoHandle>();
                }

                // 下4桁からインデックスを取得
                int index = 0;
                if (this.AutoVibratoTypeCustom == null) {
                    index = 0;
                } else {
                    int trimlen = 4;
                    int len = PortUtil.getStringLength(this.AutoVibratoTypeCustom);
                    if (len < 4) {
                        trimlen = len;
                    }
                    if (trimlen > 0) {
                        string s = this.AutoVibratoTypeCustom.Substring(len - trimlen, trimlen);
                        try {
                            index = (int)PortUtil.fromHexString(s);
                            index--;
                        } catch (Exception ex) {
                            serr.println(typeof(EditorConfig) + ".createAutoVibrato; ex=" + ex + "; AutoVibratoTypeCustom=" + AutoVibratoTypeCustom + "; s=" + s);
                            index = 0;
                        }
                    }
                }

#if DEBUG
                sout.println("EditorConfig.createAutoVibrato; AutoVibratoTypeCustom=" + AutoVibratoTypeCustom + "; index=" + index);
#endif
                VibratoHandle ret = null;
                if (0 <= index && index < this.AutoVibratoCustom.Count) {
                    ret = this.AutoVibratoCustom[index];
                    if (ret != null) {
                        ret = (VibratoHandle)ret.clone();
                    }
                }
                if (ret == null) {
                    ret = new VibratoHandle();
                }
                ret.IconID = "$0404" + PortUtil.toHexString(index + 1, 4);
                ret.setLength(vibrato_clocks);
                return ret;
            } else {
                string iconid = type == SynthesizerType.VOCALOID1 ? AutoVibratoType1 : AutoVibratoType2;
                VibratoHandle ret = VocaloSysUtil.getDefaultVibratoHandle(iconid,
                                                                           vibrato_clocks,
                                                                           type);
                if (ret == null) {
                    ret = new VibratoHandle();
                    ret.IconID = "$04040001";
                    ret.setLength(vibrato_clocks);
                }
                return ret;
            }
        }

        public int getControlCurveResolutionValue()
        {
            return ClockResolutionUtility.getValue(ControlCurveResolution);
        }

        public SortedDictionary<string, Keys[]> getShortcutKeysDictionary(List<ValuePairOfStringArrayOfKeys> defs)
        {
            SortedDictionary<string, Keys[]> ret = new SortedDictionary<string, Keys[]>();
            for (int i = 0; i < ShortcutKeys.Count; i++) {
                ret[ShortcutKeys[i].Key] = ShortcutKeys[i].Value;
            }
            foreach (var item in defs) {
                if (!ret.ContainsKey(item.Key)) {
                    ret[item.Key] = item.Value;
                }
            }
            return ret;
        }

        public Font getBaseFont()
        {
            return new Font(BaseFontName, Font.PLAIN, (int)BaseFontSize);
        }

        public int getMouseHoverTime()
        {
            return m_mouse_hover_time;
        }

        public void setMouseHoverTime(int value)
        {
            if (value < 0) {
                m_mouse_hover_time = 0;
            } else if (2000 < m_mouse_hover_time) {
                m_mouse_hover_time = 2000;
            } else {
                m_mouse_hover_time = value;
            }
        }

        // XMLシリアライズ用
        /// <summary>
        /// ピアノロール上でマウスホバーイベントが発生するまでの時間(millisec)
        /// </summary>
        public int MouseHoverTime
        {
            get
            {
                return getMouseHoverTime();
            }
            set
            {
                setMouseHoverTime(value);
            }
        }

        public QuantizeMode getPositionQuantize()
        {
            return m_position_quantize;
        }

        public void setPositionQuantize(QuantizeMode value)
        {
            if (m_position_quantize != value) {
                m_position_quantize = value;
                try {
                    invokeQuantizeModeChangedEvent();
                } catch (Exception ex) {
                    Logger.write(typeof(EditorConfig) + ".getPositionQuantize; ex=" + ex + "\n");
                    serr.println("EditorConfig#setPositionQuantize; ex=" + ex);
                }
            }
        }

        // XMLシリアライズ用
        public QuantizeMode PositionQuantize
        {
            get
            {
                return getPositionQuantize();
            }
            set
            {
                setPositionQuantize(value);
            }
        }

        public bool isPositionQuantizeTriplet()
        {
            return m_position_quantize_triplet;
        }

        public void setPositionQuantizeTriplet(bool value)
        {
            if (m_position_quantize_triplet != value) {
                m_position_quantize_triplet = value;
                try {
                    invokeQuantizeModeChangedEvent();
                } catch (Exception ex) {
                    serr.println("EditorConfig#setPositionQuantizeTriplet; ex=" + ex);
                    Logger.write(typeof(EditorConfig) + ".setPositionQuantizeTriplet; ex=" + ex + "\n");
                }
            }
        }

        // XMLシリアライズ用
        public bool PositionQuantizeTriplet
        {
            get
            {
                return isPositionQuantizeTriplet();
            }
            set
            {
                setPositionQuantizeTriplet(value);
            }
        }

        public QuantizeMode getLengthQuantize()
        {
            return m_length_quantize;
        }

        public void setLengthQuantize(QuantizeMode value)
        {
            if (m_length_quantize != value) {
                m_length_quantize = value;
                try {
                    invokeQuantizeModeChangedEvent();
                } catch (Exception ex) {
                    serr.println("EditorConfig#setLengthQuantize; ex=" + ex);
                    Logger.write(typeof(EditorConfig) + ".setLengthQuantize; ex=" + ex + "\n");
                }
            }
        }

        public QuantizeMode LengthQuantize
        {
            get
            {
                return getLengthQuantize();
            }
            set
            {
                setLengthQuantize(value);
            }
        }

        public bool isLengthQuantizeTriplet()
        {
            return m_length_quantize_triplet;
        }

        public void setLengthQuantizeTriplet(bool value)
        {
            if (m_length_quantize_triplet != value) {
                m_length_quantize_triplet = value;
                try {
                    invokeQuantizeModeChangedEvent();
                } catch (Exception ex) {
                    serr.println("EditorConfig#setLengthQuantizeTriplet; ex=" + ex);
                    Logger.write(typeof(EditorConfig) + ".setLengthQuantizeTriplet; ex=" + ex + "\n");
                }
            }
        }

        /// <summary>
        /// QuantizeModeChangedイベントを発行します
        /// </summary>
        private void invokeQuantizeModeChangedEvent()
        {
            if (QuantizeModeChanged != null) {
                QuantizeModeChanged.Invoke(typeof(EditorConfig), new EventArgs());
            }
        }

        // XMLシリアライズ用
        public bool LengthQuantizeTriplet
        {
            get
            {
                return isLengthQuantizeTriplet();
            }
            set
            {
                setLengthQuantizeTriplet(value);
            }
        }

        /// <summary>
        /// 「最近使用したファイル」のリストに、アイテムを追加します
        /// </summary>
        /// <param name="new_file"></param>
        public void pushRecentFiles(string new_file)
        {
            // NumRecentFilesは0以下かも知れない
            if (NumRecentFiles <= 0) {
                NumRecentFiles = 5;
            }

            // RecentFilesはnullかもしれない．
            if (RecentFiles == null) {
                RecentFiles = new List<string>();
            }

            // 重複があれば消す
            List<string> dict = new List<string>();
            foreach (var s in RecentFiles) {
                bool found = false;
                for (int i = 0; i < dict.Count; i++) {
                    if (s.Equals(dict[i])) {
                        found = true;
                    }
                }
                if (!found) {
                    dict.Add(s);
                }
            }
            RecentFiles.Clear();
            foreach (var s in dict) {
                RecentFiles.Add(s);
            }

            // 現在登録されているRecentFilesのサイズが規定より大きければ，下の方から消す
            if (RecentFiles.Count > NumRecentFiles) {
                for (int i = RecentFiles.Count - 1; i > NumRecentFiles; i--) {
                    RecentFiles.RemoveAt(i);
                }
            }

            // 登録しようとしているファイルは，RecentFilesの中に既に登録されているかs？
            int index = -1;
            for (int i = 0; i < RecentFiles.Count; i++) {
                if (RecentFiles[i].Equals(new_file)) {
                    index = i;
                    break;
                }
            }

            if (index >= 0) {  // 登録されてる場合
                RecentFiles.RemoveAt(index);
            }
            RecentFiles.Insert(0, new_file);
        }
        #endregion

        #region private method
        /// <summary>
        /// このインスタンスの整合性をチェックします．
        /// </summary>
        public void check()
        {
            int count = SymbolTable.getCount();
            for (int i = 0; i < count; i++) {
                SymbolTable st = SymbolTable.getSymbolTable(i);
                bool found = false;
                foreach (var s in UserDictionaries) {
                    string[] spl = PortUtil.splitString(s, new char[] { '\t' }, 2);
                    if (st.getName().Equals(spl[0])) {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    UserDictionaries.Add(st.getName() + "\tT");
                }
            }

            // key_widthを最大，最小の間に収める
            int draft_key_width = this.KeyWidth;
            if (draft_key_width < AppManager.MIN_KEY_WIDTH) {
                draft_key_width = AppManager.MIN_KEY_WIDTH;
            } else if (AppManager.MAX_KEY_WIDTH < draft_key_width) {
                draft_key_width = AppManager.MAX_KEY_WIDTH;
            }

            if (PathResamplers == null) {
                PathResamplers = new List<string>();
            }

            // SynthEngineの違いを識別しないように変更．VOALOID1に縮約する
            if (DefaultSynthesizer == RendererKind.VOCALOID1_100 ||
                DefaultSynthesizer == RendererKind.VOCALOID1_101) {
                DefaultSynthesizer = RendererKind.VOCALOID1;
            }
        }
        #endregion
    }

}
