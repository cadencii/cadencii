/*
 * EditorConfig.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.cadencii.
 *
 * com.boare.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * com.boare.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.cadencii;

import java.util.*;
import java.io.*;
import java.awt.*;
import javax.swing.*;

import com.boare.corlib.*;
import com.boare.vsq.*;
import com.boare.util.*;

/**
 * Cadenciiの環境設定
 */
public class EditorConfig {
    public int DefaultPreMeasure = 4;
    public String DefaultSingerName = "Miku";
    public int DefaultXScale = 65;
    public String BaseFontName = "MS UI Gothic";
    public float BaseFontSize = 9.0f;
    public String ScreenFontName = "MS UI Gothic";
    public String CounterFontName = "Arial";
    public int WheelOrder = 20;
    public boolean CursorFixed = false;
    /**
     * RecentFilesに登録することの出来る最大のファイル数
     */
    public int NumRecentFiles = 5;
    /**
     * 最近使用したファイルのリスト
     */
    public Vector<String> RecentFiles = new Vector<String>();
    public int DefaultPMBendDepth = 8;
    public int DefaultPMBendLength = 0;
    public int DefaultPMbPortamentoUse = 3;
    public int DefaultDEMdecGainRate = 50;
    public int DefaultDEMaccent = 50;
    public boolean ShowLyric = true;
    public boolean ShowExpLine = true;
    public com.boare.cadencii.DefaultVibratoLength DefaultVibratoLength = com.boare.cadencii.DefaultVibratoLength.L66;
    public AutoVibratoMinLength AutoVibratoMinimumLength = AutoVibratoMinLength.L1;
    public VibratoType AutoVibratoType = VibratoType.NormalType1;
    public boolean EnableAutoVibrato = true;
    public int PxTrackHeight = 14;
    public int MouseDragIncrement = 50;
    public int MouseDragMaximumRate = 600;
    public boolean MixerVisible = false;
    public int PreSendTime = 500;
    public ClockResolution ControlCurveResolution = ClockResolution.L30;
    public String Language = "";
    /// <summary>
    /// マウスの操作などの許容範囲。プリメジャーにPxToleranceピクセルめり込んだ入力を行っても、エラーにならない。(補正はされる)
    /// </summary>
    public int PxTolerance = 10;
    /// <summary>
    /// マウスホイールでピアノロールを水平方向にスクロールするかどうか。
    /// </summary>
    public boolean ScrollHorizontalOnWheel = true;
    /// <summary>
    /// 画面描画の最大フレームレート
    /// </summary>
    public int MaximumFrameRate = 15;
    /// <summary>
    /// ユーザー辞書のOn/Offと順序
    /// </summary>
    public Vector<String> UserDictionaries = new Vector<String>();
    /// <summary>
    /// 実行環境
    /// </summary>
    public com.boare.cadencii.Platform Platform = com.boare.cadencii.Platform.Windows;
    /// <summary>
    /// toolStripToolの表示位置
    /// </summary>
    public ToolStripLocation ToolEditTool = new ToolStripLocation( new Point( 3, 50 ), ToolStripLocation.ParentPanel.Top );
    /// <summary>
    /// toolStripPositionの表示位置
    /// </summary>
    public ToolStripLocation ToolPositionLocation = new ToolStripLocation( new Point( 3, 0 ), ToolStripLocation.ParentPanel.Top );
    /// <summary>
    /// toolStripMeasureの表示位置
    /// </summary>
    public ToolStripLocation ToolMeasureLocation = new ToolStripLocation( new Point( 3, 25 ), ToolStripLocation.ParentPanel.Top );
    public ToolStripLocation ToolFileLocation = new ToolStripLocation( new Point( 3, 75 ), ToolStripLocation.ParentPanel.Top );
    public ToolStripLocation ToolPaletteLocation = new ToolStripLocation( new Point( 3, 100 ), ToolStripLocation.ParentPanel.Top );
    public boolean WindowMaximized = false;
    public Rectangle WindowRect = new Rectangle( 0, 0, 756, 616 );
    /// <summary>
    /// hScrollのスクロールボックスの最小幅(px)
    /// </summary>
    public int MinimumScrollHandleWidth = 20;
    /// <summary>
    /// 発音記号入力モードを，維持するかどうか
    /// </summary>
    public boolean KeepLyricInputMode = false;
    /// <summary>
    /// 最後に使用したVSQファイルへのパス
    /// </summary>
    public String LastVsqPath = "";
    /// <summary>
    /// ピアノロールの何もないところをクリックした場合、右クリックでもプレビュー音を再生するかどうか
    /// </summary>
    public boolean PlayPreviewWhenRightClick = false;
    /// <summary>
    /// ゲームコントローラで、異なるイベントと識別する最小の時間間隔(millisec)
    /// </summary>
    public int GameControlerMinimumEventInterval = 100;
    /// <summary>
    /// カーブの選択範囲もクオンタイズするかどうか
    /// </summary>
    public boolean CurveSelectingQuantized = true;

    private QuantizeMode m_position_quantize = QuantizeMode.p32;
    private boolean m_position_quantize_triplet = false;
    private QuantizeMode m_length_quantize = QuantizeMode.p32;
    private boolean m_length_quantize_triplet = false;
    private int m_mouse_hover_time = 500;
    private int m_gamectrl_tr = 0;
    private int m_gamectrl_o = 1;
    private int m_gamectrl_x = 2;
    private int m_gamectrl_re = 3;
    private int m_gamectrl_L1 = 4;
    private int m_gamectrl_R1 = 5;
    private int m_gamectrl_L2 = 6;
    private int m_gamectrl_R2 = 7;
    private int m_gamectrl_select = 8;
    private int m_gamectrl_start = 9;
    private int m_gamectrl_stickL = 10;
    private int m_gamectrl_stickR = 11;
    public boolean CurveVisibleVelocity = true;
    public boolean CurveVisibleAccent = true;
    public boolean CurveVisibleDecay = true;
    public boolean CurveVisibleVibratoRate = true;
    public boolean CurveVisibleVibratoDepth = true;
    public boolean CurveVisibleDynamics = true;
    public boolean CurveVisibleBreathiness = true;
    public boolean CurveVisibleBrightness = true;
    public boolean CurveVisibleClearness = true;
    public boolean CurveVisibleOpening = true;
    public boolean CurveVisibleGendorfactor = true;
    public boolean CurveVisiblePortamento = true;
    public boolean CurveVisiblePit = true;
    public boolean CurveVisiblePbs = true;
    public boolean CurveVisibleHarmonics = false;
    public boolean CurveVisibleFx2Depth = false;
    public boolean CurveVisibleReso1 = false;
    public boolean CurveVisibleReso2 = false;
    public boolean CurveVisibleReso3 = false;
    public boolean CurveVisibleReso4 = false;
    public int GameControlPovRight = 9000;
    public int GameControlPovLeft = 27000;
    public int GameControlPovUp = 0;
    public int GameControlPovDown = 18000;
    /// <summary>
    /// wave波形を表示するかどうか
    /// </summary>
    public boolean ViewWaveform = false;
    /// <summary>
    /// キーボードからの入力に使用するデバイス
    /// </summary>
    public MidiPortConfig MidiInPort = new MidiPortConfig();
    public RgbColor PianorollColorVocalo2Black = new RgbColor( 212, 212, 212 );
    public RgbColor PianorollColorVocalo2White = new RgbColor( 240, 240, 240 );
    public RgbColor PianorollColorVocalo1Black = new RgbColor( 210, 205, 172 );
    public RgbColor PianorollColorVocalo1White = new RgbColor( 240, 235, 214 );
    public RgbColor PianorollColorUtauBlack = new RgbColor( 212, 212, 212 );
    public RgbColor PianorollColorUtauWhite = new RgbColor( 240, 240, 240 );
    public RgbColor PianorollColorVocalo1Bar = new RgbColor( 161, 157, 136 );
    public RgbColor PianorollColorVocalo1Beat = new RgbColor( 209, 204, 172 );
    public RgbColor PianorollColorVocalo2Bar = new RgbColor( 161, 157, 136 );
    public RgbColor PianorollColorVocalo2Beat = new RgbColor( 209, 204, 172 );
    public RgbColor PianorollColorUtauBar = new RgbColor( 255, 64, 255 );
    public RgbColor PianorollColorUtauBeat = new RgbColor( 128, 128, 255 );
    public boolean ViewAtcualPitch = false;
    public boolean InvokeUtauCoreWithWine = false;
    public Vector<SingerConfig> UtauSingers = new Vector<SingerConfig>();
    public String PathResampler = "";
    public String PathWavtool = "";
    public boolean UseCustomFileDialog = false;
    /// <summary>
    /// ベジエ制御点を掴む時の，掴んだと判定する際の誤差．制御点の外輪からPxToleranceBezierピクセルずれていても，掴んだと判定する．
    /// </summary>
    public int PxToleranceBezier = 10;
    /// <summary>
    /// 歌詞入力においてローマ字が入力されたとき，Cadencii側でひらがなに変換するかどうか
    /// </summary>
    public boolean SelfDeRomanization = false;
    /// <summary>
    /// openMidiDialogで最後に選択された拡張子
    /// </summary>
    public String LastUsedExtension = ".vsq";
    /// <summary>
    /// ミキサーダイアログを常に手前に表示するかどうか
    /// </summary>
    public boolean MixerTopMost = false;
    public Vector<ValuePairOfStringArrayOfKeys> ShortcutKeys = new Vector<ValuePairOfStringArrayOfKeys>();
    /// <summary>
    /// リアルタイム再生時の再生速度
    /// </summary>
    private float m_realtime_input_speed = 1.0f;
    public byte MidiProgramNormal = 115;
    public byte MidiProgramBell = 9;
    public byte MidiNoteNormal = 65;
    public byte MidiNoteBell = 65;
    public boolean MidiRingBell = true;
    public int MidiPreUtterance = 0;
    public MidiPortConfig MidiDeviceMetronome = new MidiPortConfig();
    public MidiPortConfig MidiDeviceGeneral = new MidiPortConfig();
    public boolean MetronomeEnabled = true;
    public boolean PropertyWindowVisible = true;
    public Rectangle PropertyWindowBounds = new Rectangle( 0, 0, 200, 300 );
    public double PropertyWindowMinimumOpacity = 0.5;
//#if VER22
    //public FormConfigUtauVoiceConfig FormConfigUtauVoiceConfig = new FormConfigUtauVoiceConfig( new Size( 714, 533 ), 70.0f, 60.0f, 20 );
//#endif

    // Static Fields
    public static final ValuePairOfStringArrayOfKeys[] DEFAULT_SHORTCUT_KEYS = new ValuePairOfStringArrayOfKeys[]{
        new ValuePairOfStringArrayOfKeys( "menuFileNew", new Keys[]{ Keys.Control, Keys.N } ),
        new ValuePairOfStringArrayOfKeys( "menuFileOpen", new Keys[]{ Keys.Control, Keys.O } ),
        new ValuePairOfStringArrayOfKeys( "menuFileOpenVsq", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuFileSave", new Keys[]{ Keys.Control, Keys.S } ),
        new ValuePairOfStringArrayOfKeys( "menuFileQuit", new Keys[]{ Keys.Control, Keys.Q } ),
        new ValuePairOfStringArrayOfKeys( "menuEditUndo", new Keys[]{ Keys.Control, Keys.Z } ),
        new ValuePairOfStringArrayOfKeys( "menuEditRedo", new Keys[]{ Keys.Control, Keys.Shift, Keys.Z } ),
        new ValuePairOfStringArrayOfKeys( "menuEditCut", new Keys[]{ Keys.Control, Keys.X } ),
        new ValuePairOfStringArrayOfKeys( "menuEditCopy", new Keys[]{ Keys.Control, Keys.C } ),
        new ValuePairOfStringArrayOfKeys( "menuEditPaste", new Keys[]{ Keys.Control, Keys.V } ),
        new ValuePairOfStringArrayOfKeys( "menuEditSelectAll", new Keys[]{ Keys.Control, Keys.A } ),
        new ValuePairOfStringArrayOfKeys( "menuEditSelectAllEvents", new Keys[]{ Keys.Control, Keys.Shift, Keys.A } ),
        new ValuePairOfStringArrayOfKeys( "menuVisualMixer", new Keys[]{ Keys.F3 } ),
        new ValuePairOfStringArrayOfKeys( "menuJobRealTime", new Keys[]{ Keys.F5 } ),
        new ValuePairOfStringArrayOfKeys( "menuHiddenEditLyric", new Keys[]{ Keys.F2 } ),
        new ValuePairOfStringArrayOfKeys( "menuHiddenEditFlipToolPointerPencil", new Keys[]{ Keys.Control, Keys.W } ),
        new ValuePairOfStringArrayOfKeys( "menuHiddenEditFlipToolPointerEraser", new Keys[]{ Keys.Control, Keys.E } ),
        new ValuePairOfStringArrayOfKeys( "menuHiddenVisualForwardParameter", new Keys[]{ Keys.Control, Keys.Alt, Keys.PageDown } ),
        new ValuePairOfStringArrayOfKeys( "menuHiddenVisualBackwardParameter", new Keys[]{ Keys.Control, Keys.Alt, Keys.PageUp } ),
        new ValuePairOfStringArrayOfKeys( "menuHiddenTrackNext", new Keys[]{ Keys.Control, Keys.PageDown } ),
        new ValuePairOfStringArrayOfKeys( "menuHiddenTrackBack", new Keys[]{ Keys.Control, Keys.PageUp } ),
        new ValuePairOfStringArrayOfKeys( "menuFileSaveNamed", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuFileImportVsq", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuFileOpenUst", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuFileImportMidi", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuFileExportWave", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuFileExportMidi", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuFileDelete", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuVisualWaveform", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuVisualProperty", new Keys[]{ Keys.F6 } ),
        new ValuePairOfStringArrayOfKeys( "menuVisualGridline", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuVisualStartMarker", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuVisualEndMarker", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuVisualLyrics", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuVisualNoteProperty", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuVisualPitchLine", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuJobNormalize", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuJobInsertBar", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuJobDeleteBar", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuJobRandomize", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuJobConnect", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuJobLyric", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuTrackOn", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuTrackAdd", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuTrackCopy", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuTrackChangeName", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuTrackDelete", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuTrackRenderCurrent", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuTrackRenderAll", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuTrackOverlay", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuTrackRendererVOCALOID1", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuTrackRendererVOCALOID2", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuTrackRendererUtau", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuTrackMasterTuning", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuLyricExpressionProperty", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuLyricVibratoProperty", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuLyricSymbol", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuLyricDictionary", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuScriptUpdate", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuSettingPreference", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuSettingGameControlerSetting", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuSettingGameControlerReload", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuSettingPaletteTool", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuSettingShortcut", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuSettingSingerProperty", new Keys[]{} ),
        new ValuePairOfStringArrayOfKeys( "menuHelpAbout", new Keys[]{} ) };
    private static XmlSerializer s_serializer = new XmlSerializer( EditorConfig.class );

    /// <summary>
    /// PositionQuantize, PositionQuantizeTriplet, LengthQuantize, LengthQuantizeTripletの描くプロパティのいずれかが
    /// 変更された時発生します
    /// </summary>
    public static com.boare.cadencii.Event QuantizeModeChanged = new Event();

    public Keys[] getShortcutKeyFor( JMenuItem menu_item ){
        String name = menu_item.getName();
        for( ValuePairOfStringArrayOfKeys item : ShortcutKeys ){
            if( name.equals( item.Key ) ){
                return item.Value;
            }
        }
        return new Keys[]{};
    }

    /// <summary>
    /// リアルタイム再生時の再生速度
    /// </summary>
    public float getRealtimeInputSpeed(){
        if ( m_realtime_input_speed <= 0.0f ) {
            m_realtime_input_speed = 1.0f;
        }
        return m_realtime_input_speed;
    }

    public void setRealtimeInputSpeed( float value ){
        m_realtime_input_speed = value;
        if ( m_realtime_input_speed <= 0.0f ) {
            m_realtime_input_speed = 1.0f;
        }
    }

    public TreeMap<String, Keys[]> getShortcutKeysDictionary() {
        TreeMap<String, Keys[]> ret = new TreeMap<String, Keys[]>();
        for ( int i = 0; i < ShortcutKeys.size(); i++ ) {
            ret.put( ShortcutKeys.get( i ).Key, ShortcutKeys.get( i ).Value );
        }
        return ret;
    }

    public static void serialize( EditorConfig instance, String file ) {
        try{
            FileOutputStream fs = new FileOutputStream( file );
            s_serializer.serialize( fs, instance );
            fs.close();
        }catch( Exception ex ){
            System.out.println( "EditorConfig.serialize; ex=" + ex );
        }
    }

    public static EditorConfig deserialize( EditorConfig old_instance, String file ) {
        EditorConfig ret = null;
        try{
            FileInputStream fs = new FileInputStream( file );
            ret = (EditorConfig)s_serializer.deserialize( fs );
            fs.close();
        }catch( Exception ex ){
            System.out.println( "EditorConfig.deserialize; ex=" + ex );
        }
        for ( int j = 0; j < DEFAULT_SHORTCUT_KEYS.length; j++ ) {
            boolean found = false;
            for ( int i = 0; i < ret.ShortcutKeys.size(); i++ ) {
                if ( DEFAULT_SHORTCUT_KEYS[j].Key == ret.ShortcutKeys.get( i ).Key ) {
                    found = true;
                    break;
                }
            }
            if ( !found ) {
                ret.ShortcutKeys.add( DEFAULT_SHORTCUT_KEYS[j] );
            }
        }
        return ret;
    }

    public Font getBaseFont(){
        return new Font( BaseFontName, Font.PLAIN, (int)BaseFontSize );
    }

    /// <summary>
    /// Button index of Left Stick
    /// </summary>
    public int getGameControlStirckL() {
        return m_gamectrl_stickL;
    }

    public void setGameControlStirckL( int value ){
        m_gamectrl_stickL = value;
    }

    /// <summary>
    /// Button index of Right Stick
    /// </summary>
    public int getGameControlStirckR(){
        return m_gamectrl_stickR;
    }

    public void setGameControlStirckR( int value ){
        m_gamectrl_stickR = value;
    }

    /// <summary>
    /// Button index of "START"
    /// </summary>
    public int getGameControlStart() {
        return m_gamectrl_start;
    }

    public void setGameControlStart( int value ){
        m_gamectrl_start = value;
    }

    /// <summary>
    /// Button index of "R2"
    /// </summary>
    public int getGameControlR2(){
        return m_gamectrl_R2;
    }

    public void setGameControlR2( int value ){
        m_gamectrl_R2 = value;
    }

    /// <summary>
    /// Button index of "R1"
    /// </summary>
    public int getGameControlR1(){
        return m_gamectrl_R1;
    }

    public void setGameControlR1( int value ){
        m_gamectrl_R1 = value;
    }

    /// <summary>
    /// Button index of "L2"
    /// </summary>
    public int getGameControlL2(){
        return m_gamectrl_L2;
    }

    public void setGameControlL2( int value ){
        m_gamectrl_L2 = value;
    }

    /// <summary>
    /// Button index of "L1"
    /// </summary>
    public int getGameControlL1(){
        return m_gamectrl_L1;
    }

    public void setGameControlL1( int value ){
        m_gamectrl_L1 = value;
    }

    /// <summary>
    /// Button index of "SELECT"
    /// </summary>
    public int getGameControlSelect(){
        return m_gamectrl_select;
    }

    public void setGameControlSelect( int value ){
        m_gamectrl_select = value;
    }

    /// <summary>
    /// Button index of "□"
    /// </summary>
    public int getGameControlerRectangle(){
        return m_gamectrl_re;
    }

    public void setGameControlRectangle( int value ){
        m_gamectrl_re = value;
    }

    /// <summary>
    /// Button index of "×"
    /// </summary>
    public int getGameControlerCross(){
        return m_gamectrl_x;
    }

    public void setGameControlCross( int value ){
        m_gamectrl_x = value;
    }

    /// <summary>
    /// Button index of "○"
    /// </summary>
    public int getGameControlerCircle(){
        return m_gamectrl_o;
    }

    public void setGameControlerCircle( int value ){
        m_gamectrl_o = value;
    }

    /// <summary>
    /// Button index of "△"
    /// </summary>
    public int getGameControlerTriangle(){
        return m_gamectrl_tr;
    }

    public void setGameControlerTriangle( int value ){
        m_gamectrl_tr = value;
    }

    /// <summary>
    /// ピアノロール上でマウスホバーイベントが発生するまでの時間(millisec)
    /// </summary>
    public int getMouseHoverTime(){
        return m_mouse_hover_time;
    }

    public void setMouseHoverTime( int value ){
        if ( value < 0 ) {
            m_mouse_hover_time = 0;
        } else if ( 2000 < m_mouse_hover_time ) {
            m_mouse_hover_time = 2000;
        } else {
            m_mouse_hover_time = value;
        }
    }

    public QuantizeMode getPositionQuantize(){
        return m_position_quantize;
    }

    public void setPositionQuantize( QuantizeMode value ){
        if ( m_position_quantize != value ) {
            m_position_quantize = value;
            if ( QuantizeModeChanged != null ) {
                QuantizeModeChanged.invoke( EditorConfig.class, null );
            }
        }
    }

    public boolean getPositionQuantizeTriplet(){
        return m_position_quantize_triplet;
    }

    public void setPositionQuantizeTriplet( boolean value ){
        if ( m_position_quantize_triplet != value ) {
            m_position_quantize_triplet = value;
            if ( QuantizeModeChanged != null ) {
                QuantizeModeChanged.invoke( EditorConfig.class, null );
            }
        }
    }

    public QuantizeMode getLengthQuantize(){
        return m_length_quantize;
    }

    public void setLengthQuantize( QuantizeMode value ){
        if ( m_length_quantize != value ) {
            m_length_quantize = value;
            if ( QuantizeModeChanged != null ) {
                QuantizeModeChanged.invoke( EditorConfig.class, null );
            }
        }
    }

    public boolean getLengthQuantizeTriplet(){
        return m_length_quantize_triplet;
    }

    public void setLengthQuantizeTriplet( boolean value ){
        if ( m_length_quantize_triplet != value ) {
            m_length_quantize_triplet = value;
            if ( QuantizeModeChanged != null ) {
                QuantizeModeChanged.invoke( EditorConfig.class, null );
            }
        }
    }

    /// <summary>
    /// 「最近使用したファイル」のリストに、アイテムを追加します
    /// </summary>
    /// <param name="new_file"></param>
    public void pushRecentFiles( String new_file ) {
        // NumRecentFilesは0以下かも知れない
        if ( NumRecentFiles <= 0 ) {
            NumRecentFiles = 5;
        }

        // RecentFilesはnullかもしれない．
        if ( RecentFiles == null ) {
            RecentFiles = new Vector<String>();
        }

        // 重複があれば消す
        Vector<String> dict = new Vector<String>();
        for ( String s : RecentFiles ) {
            boolean found = false;
            for ( int i = 0; i < dict.size(); i++ ) {
                if ( s.equals( dict.get( i ) ) ) {
                    found = true;
                }
            }
            if ( !found ) {
                dict.add( s );
            }
        }
        RecentFiles.clear();
        for ( String s : dict ) {
            RecentFiles.add( s );
        }

        // 現在登録されているRecentFilesのサイズが規定より大きければ，下の方から消す
        if ( RecentFiles.size() > NumRecentFiles ) {
            for ( int i = RecentFiles.size() - 1; i > NumRecentFiles; i-- ) {
                RecentFiles.removeElementAt( i );
            }
        }

        // 登録しようとしているファイルは，RecentFilesの中に既に登録されているかs？
        int index = -1;
        for ( int i = 0; i < RecentFiles.size(); i++ ) {
            if ( RecentFiles.get( i ).equals( new_file ) ) {
                index = i;
                break;
            }
        }

        if ( index >= 0 ) {  // 登録されてる場合
            RecentFiles.removeElementAt( index );
        }
        RecentFiles.insertElementAt( new_file, 0 );
    }
}
