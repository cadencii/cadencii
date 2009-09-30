/*
 * AppManager.java
 * Copyright (c) 2009 kbinani
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

import java.awt.*;
import java.util.*;
import com.boare.util.*;

/*
-コントロールカーブのテンプレートの登録＆貼付け機能
-選択したノートを指定したクロック数分一括シフトする機能
-前回レンダリングしたWAVEファイルをリサイクルする（起動ごとにレンダリングするのは面倒？）
-横軸を変更するショートカットキー
-音符の時間位置を秒指定で入力できるモード
-コントロールカーブのリアルタイムかつアナログ的な入力（ジョイスティックorゲームコントローラ）
-コントロールカーブエディタを幾つか積めるような機能
 */
public class AppManager {
    /// <summary>
    /// 鍵盤の表示幅(pixel)
    /// </summary>
    public static final int _KEY_LENGTH = 68;
    private static final String _CONFIG_FILE_NAME = "config.xml";
    private static final String _CONFIG_DIR_NAME = "Cadencii";
    /// <summary>
    /// OSのクリップボードに貼り付ける文字列の接頭辞．これがついていた場合，クリップボードの文字列をCadenciiが使用できると判断する．
    /// </summary>
    private static final String _CLIP_PREFIX = "CADENCIIOBJ";

    /// <summary>
    /// エディタの設定
    /// </summary>
    public static EditorConfig EditorConfig = new EditorConfig();
    /// <summary>
    /// AttachedCurve用のシリアライザ
    /// </summary>
    public static XmlSerializer XmlSerializerListBezierCurves = new XmlSerializer( AttachedCurve.class );
    public static Font BaseFont8 = null;
    public static Font BaseFont9 = null;
    /// <summary>
    /// ピアノロールの歌詞の描画に使用されるフォント。
    /// </summary>
    public static Font BaseFont10 = null;
    /// <summary>
    /// ピアノロールの歌詞の描画に使用されるフォント。（発音記号固定の物の場合）
    /// </summary>
    public static Font BaseFont10Bold = null;
    /// <summary>
    /// 歌詞を音符の（高さ方向の）真ん中に描画するためのオフセット
    /// </summary>
    public static int BaseFont10OffsetHeight = 0;
    public static int BaseFont8OffsetHeight = 0;
    public static int BaseFont9OffsetHeight = 0;
    public static PropertyPanel NotePropertyPanel;
    public static FormNoteProperty NotePropertyDlg;

    public static final Color[] s_HILIGHT = new Color[] { 
        new Color( 181, 220, 16 ),
        new Color( 231, 244, 49 ),
        new Color( 252, 230, 29 ),
        new Color( 247, 171, 20 ),
        new Color( 249, 94, 17 ),
        new Color( 234, 27, 47 ),
        new Color( 175, 20, 80 ),
        new Color( 183, 24, 149 ),
        new Color( 105, 22, 158 ),
        new Color( 22, 36, 163 ),
        new Color( 37, 121, 204 ),
        new Color( 29, 179, 219 ),
        new Color( 24, 239, 239 ),
        new Color( 25, 206, 175 ),
        new Color( 23, 160, 134 ),
        new Color( 79, 181, 21 ) };
    public static readonly new Color[] s_RENDER = new new Color[]{
        new Color( 19, 143, 52 ),
        new Color( 158, 154, 18 ),
        new Color( 160, 143, 23 ),
        new Color( 145, 98, 15 ),
        new Color( 142, 52, 12 ),
        new Color( 142, 19, 37 ),
        new Color( 96, 13, 47 ),
        new Color( 117, 17, 98 ),
        new Color( 62, 15, 99 ),
        new Color( 13, 23, 84 ),
        new Color( 25, 84, 132 ),
        new Color( 20, 119, 142 ),
        new Color( 19, 142, 139 ),
        new Color( 17, 122, 102 ),
        new Color( 13, 86, 72 ),
        new Color( 43, 91, 12 ) };
    public static final String[] _USINGS = new String[] { "using System;",
                                         "using System.IO;",
                                         "using Boare.Lib.Vsq;",
                                         "using Boare.Cadencii;",
                                         "using bocoree;",
                                         "using Boare.Lib.Media;",
                                         "using Boare.Lib.AppUtil;",
                                         "using System.Windows.Forms;",
                                         "using System.Collections.Generic;",
                                         "using System.Drawing;",
                                         "using System.Text;",
                                         "using System.Xml.Serialization;" };
    public static final Keys[] _SHORTCUT_ACCEPTABLE = new Keys[]{
        Keys.A,
        Keys.B,
        Keys.C,
        Keys.D,
        Keys.D0,
        Keys.D1,
        Keys.D2,
        Keys.D3,
        Keys.D4,
        Keys.D5,
        Keys.D6,
        Keys.D7,
        Keys.D8,
        Keys.D9,
        Keys.Down,
        Keys.E,
        Keys.F,
        Keys.F1,
        Keys.F2,
        Keys.F3,
        Keys.F4,
        Keys.F5,
        Keys.F6,
        Keys.F7,
        Keys.F8,
        Keys.F9,
        Keys.F10,
        Keys.F11,
        Keys.F12,
        Keys.F13,
        Keys.F14,
        Keys.F15,
        Keys.F16,
        Keys.F17,
        Keys.F18,
        Keys.F19,
        Keys.F20,
        Keys.F21,
        Keys.F22,
        Keys.F23,
        Keys.F24,
        Keys.G,
        Keys.H,
        Keys.I,
        Keys.J,
        Keys.K,
        Keys.L,
        Keys.Left,
        Keys.M,
        Keys.N,
        Keys.NumPad0,
        Keys.NumPad1,
        Keys.NumPad2,
        Keys.NumPad3,
        Keys.NumPad4,
        Keys.NumPad5,
        Keys.NumPad6,
        Keys.NumPad7,
        Keys.NumPad8,
        Keys.NumPad9,
        Keys.O,
        Keys.P,
        Keys.PageDown,
        Keys.PageUp,
        Keys.Q,
        Keys.R,
        Keys.Right,
        Keys.S,
        Keys.Space,
        Keys.T,
        Keys.U,
        Keys.Up,
        Keys.V,
        Keys.W,
        Keys.X,
        Keys.Y,
        Keys.Z };

    // Private Static Fields
    private static int s_base_tempo = 480000;
    private static SolidBrush s_hilight_brush = new SolidBrush( Color.CornflowerBlue );
    private static Object s_locker;

    // Imported from EditorManager
    private static int m_start_to_draw_y = 0;
    private static VsqFileEx m_vsq;
    private static String m_file = "";
    private static int m_selected = 1;
    private static int m_current_clock = 0;
    private static boolean m_playing = false;
    private static boolean m_repeat_mode = false;
    private static boolean m_saveavi = false;
    private static boolean m_grid_visible = false;
    private static EditMode m_edit_mode = EditMode.None;
    /// <summary>
    /// トラックのレンダリングが必要かどうかを表すフラグ
    /// </summary>
    private static boolean[] m_render_required = new boolean[16] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
    /// <summary>
    /// トラックのオーバーレイ表示
    /// </summary>
    private static boolean m_overlay = true;
    private static SelectedEventList m_selected_event = new SelectedEventList();
    /// <summary>
    /// 選択されているテンポ変更イベントのリスト
    /// </summary>
    private static TreeMap<Integer, SelectedTempoEntry> m_selected_tempo = new TreeMap<Integer, SelectedTempoEntry>();
    private static int m_last_selected_tempo = -1;
    /// <summary>
    /// 選択されている拍子変更イベントのリスト
    /// </summary>
    private static TreeMap<Integer, SelectedTimesigEntry> m_selected_timesig = new TreeMap<Integer, SelectedTimesigEntry>();
    private static int m_last_selected_timesig = -1;
    private static EditTool m_selected_tool = EditTool.Pencil;
    private static Vector<Command> m_commands = new Vector<Command>();
    private static int m_command_position = -1;
    /// <summary>
    /// 選択されているベジエ点のリスト
    /// </summary>
    private static Vector<SelectedBezierPoint> m_selected_bezier = new Vector<SelectedBezierPoint>();
    /// <summary>
    /// 最後に選択されたベジエ点
    /// </summary>
    private static SelectedBezierPoint m_last_selected_bezier = new SelectedBezierPoint();
    /// <summary>
    /// 現在の演奏カーソルの位置(m_current_clockと意味は同じ。CurrentClockが変更されると、自動で更新される)
    /// </summary>
    private static PlayPositionSpecifier m_current_play_position;

    /// <summary>
    /// SelectedRegionが有効かどうかを表すフラグ
    /// </summary>
    public static boolean SelectedRegionEnabled = false;
    /// <summary>
    /// Ctrlキーを押しながらのマウスドラッグ操作による選択が行われた範囲
    /// </summary>
    public static SelectedRegion SelectedRegion;
    /// <summary>
    /// 自動ノーマライズモードかどうかを表す値を取得、または設定します。
    /// </summary> 
    public static boolean AutoNormalize = false;
    /// <summary>
    /// エンドマーカーの位置(clock)
    /// </summary>
    public static int EndMarker = 0;
    public static boolean EndMarkerEnabled = false;
    /// <summary>
    /// x方向の表示倍率(pixel/clock)
    /// </summary>
    public static float ScaleX = 0.1f;
    /// <summary>
    /// スタートマーカーの位置(clock)
    /// </summary>
    public static int StartMarker = 0;
    /// <summary>
    /// Bezierカーブ編集モードが有効かどうかを表す
    /// </summary>
    public static boolean m_is_curve_mode = false;
    public static boolean StartMarkerEnabled = false;
    /// <summary>
    /// 再生時に自動スクロールするかどうか
    /// </summary>
    public static boolean AutoScroll = true;
    /// <summary>
    /// 最初のバッファが書き込まれたかどうか
    /// </summary>
    public static boolean FirstBufferWritten = false;
    /// <summary>
    /// リアルタイム再生で使用しようとしたVSTiが有効だったかどうか。
    /// </summary>
    public static boolean RendererAvailable = false;
    /// <summary>
    /// プレビュー再生が開始された時刻
    /// </summary>
    public static DateTime PreviewStartedTime;
    /// <summary>
    /// 画面左端位置での、仮想画面上の仮想画面左端から測ったピクセル数．FormMain.hScroll.ValueとFormMain.trackBar.Valueで決まる．
    /// </summary>
    public static int StartToDrawX;
    public static String SelectedPaletteTool = "";
    public static ScreenStatus LastScreenStatus = new ScreenStatus();
    public static String m_id = "";

    /// <summary>
    /// グリッド線を表示するか否かを表す値が変更された時発生します
    /// </summary>
    public static Event GridVisibleChanged = new Event();
    public static Event PreviewStarted = new Event();
    public static Event PreviewAborted = new Event();
    public static Event SelectedEventChanged = new Event();
    public static Event SelectedToolChanged = new Event();
    /// <summary>
    /// CurrentClockプロパティの値が変更された時発生します
    /// </summary>
    public static Event CurrentClockChanged = new Event();

    private static final String _TEMPDIR_NAME = "cadencii";

    private AppManager(){
    }

    /// <summary>
    /// pがrcの中にあるかどうかを判定します
    /// </summary>
    /// <param name="p"></param>
    /// <param name="rc"></param>
    /// <returns></returns>
    public static boolean IsInRect( Point p, Rectangle rc ) {
        if ( rc.X <= p.X ) {
            if ( p.X <= rc.X + rc.Width ) {
                if ( rc.Y <= p.Y ) {
                    if ( p.Y <= rc.Y + rc.Height ) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// fontを使って文字を描画したとき，文字の縦方向の中心線と，描画時に指定した座標との間にどれだけのオフセットが生じるかを調べる
    /// </summary>
    /// <param name="font"></param>
    /// <returns></returns>
    public static int getStringOffset( Font font ) {
        Dimension str_size = measureString( "Qjp", font );
        if ( str_size.width <= 0 || str_size.height <= 0 ) {
            return 0;
        }
        int draw_pos = (int)(str_size.Height / 2);
        BufferedImage test = new BufferedImage( (int)str_size.width * 2,
                                                (int)str_size.height * 2,
                                                BufferedImage.TYPE_INT_RGB );
        Graphics g = test.getGraphics();
        g.setFont( font );
        g.setColor( Color.BLACK );
        g.drawString( "Qjp", str_size.width / 2, draw_pos );
        g.dispose();
        boolean found = false;
        int w = test.getWidth();
        int h = test.getHeight();
        int firsty = draw_pos;
        int black = Color.BLACK.getRGB();
        for ( int y = 0; y < h; y++ ) {
            for ( int x = 0; x < w; x++ ) {
                if ( test.getRGB( x, y ) == black ) {
                    found = true;
                    break;
                }
            }
            if ( found ) {
                firsty = y;
                break;
            }
        }
        found = false;
        int lasty = draw_pos;
        for ( int y = h - 1; y >= 0; y-- ) {
            for ( int x = 0; x < w; x++ ) {
                if ( test.getRGB( x, y ) == black ) {
                    found = true;
                    break;
                }
            }
            if ( found ) {
                lasty = y;
                break;
            }
        }
        return (lasty + firsty) / 2 - draw_pos;
    }

    public static Dimension measureString( String item, Font font ) {
        BufferedImage test = new BufferedImage( 10, 10, BufferedImage.TYPE_INT_RGB );
        Graphics g = test.getGraphics();
        FontMetrics fm = new FontMetrics( font );
        Rectangle2D rc = fm.getStringBounds( item, g );
        g.dispose();
        test = null;
        return new Dimension( rc.width, rc.height );
    }

    /// <summary>
    /// 文字列itemをfontを用いて描画したとき、幅widthピクセルに収まるようにitemを調節したものを返します。
    /// 例えば"1 Voice"→"1 Voi..."ナド。
    /// </summary>
    /// <param name="item"></param>
    /// <param name="font"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static String trimString( String item, Font font, int width ) {
        String edited = item;
        int delete_count = item.Length;
        while ( true ) {
            Dimension measured = measureString( edited, font );
            if ( measured.width <= width ) {
                return edited;
            }
            delete_count -= 1;
            if ( delete_count > 0 ) {
                edited = item.substring( 0, delete_count ) + "...";
            } else {
                return edited;
            }
        }
    }

    public static void debugWriteLine( String message ) {
        System.out.println( message );
    }

    /// <summary>
    /// FormMainを識別するID
    /// </summary>
    public static String getID {
        return m_id;
    }

    public static String _( String id ) {
        return Messaging.GetMessage( id );
    }

    public static ScriptInvoker LoadScript( String file ) {
        throw new Exception( "Not implemented. AppManager.LoadSctipr" );
        /*ScriptInvoker ret = new ScriptInvoker();
        ret.ScriptFile = file;
        ret.FileTimestamp = File.GetLastWriteTimeUtc( file );
        // スクリプトの記述のうち、以下のリストに当てはまる部分は空文字に置換される
        String config_file = ConfigFileNameFromScriptFileName( file );
        String script = "";
        using ( StreamReader sr = new StreamReader( file ) ) {
            script = sr.ReadToEnd();
        }
        foreach ( String s in AppManager._USINGS ) {
            script = script.Replace( s, "" );
        }

        String code = "";
        foreach ( String s in AppManager._USINGS ) {
            code += s;
        }
        code += "namespace Boare.CadenciiScript{";
        code += script;
        code += "}";
        ret.ErrorMessage = "";
        CompilerResults results;
        Assembly testAssembly = AppManager.CompileScript( code, out results );
        if ( testAssembly == null | results == null ) {
            ret.ErrorMessage = "failed compiling";
            return ret;
        }
        if ( results.Errors.Count != 0 ) {
            for ( int i = 0; i < results.Errors.Count; i++ ) {
                ret.ErrorMessage += _( "line" ) + ":" + results.Errors[i].Line + " " + results.Errors[i].ErrorText + Environment.NewLine;
            }
            if ( results.Errors.HasErrors ) {
                return ret;
            }
        }

        if ( testAssembly != null ) {
            foreach ( Type implemented in testAssembly.GetTypes() ) {
                MethodInfo tmi = implemented.GetMethod( "Edit", new Type[] { typeof( VsqFile ) } );
                if ( tmi != null && tmi.IsStatic && tmi.IsPublic && tmi.ReturnType.Equals( typeof( boolean ) ) ) {
                    ret.ScriptType = implemented;
                    ret.EditVsqDelegate = (EditVsqScriptDelegate)Delegate.CreateDelegate( typeof( EditVsqScriptDelegate ), tmi );
                    ret.Serializer = new XmlStaticMemberSerializer( implemented );

                    if ( !File.Exists( config_file ) ) {
                        continue;
                    }

                    // 設定ファイルからDeserialize
                    using ( FileStream fs = new FileStream( config_file, FileMode.Open ) ) {
                        ret.Serializer.Deserialize( fs );
                    }
                }
                tmi = implemented.GetMethod( "Edit", new Type[] { typeof( VsqFileEx ) } );
                if ( tmi != null && tmi.IsStatic && tmi.IsPublic && tmi.ReturnType.Equals( typeof( boolean ) ) ) {
                    ret.ScriptType = implemented;
                    ret.EditVsqDelegate2 = (EditVsqScriptDelegate2)Delegate.CreateDelegate( typeof( EditVsqScriptDelegate2 ), tmi );
                    ret.Serializer = new XmlStaticMemberSerializer( implemented );

                    if ( !File.Exists( config_file ) ) {
                        continue;
                    }

                    // 設定ファイルからDeserialize
                    using ( FileStream fs = new FileStream( config_file, FileMode.Open ) ) {
                        ret.Serializer.Deserialize( fs );
                    }
                }
            }
        }
        return ret;*/
    }

    /// <summary>
    /// スクリプトを実行します
    /// </summary>
    /// <param name="evsd"></param>
    public static boolean InvokeScript( ScriptInvoker script_invoker ) {
        throw new Exception( "Not implemented. AppManager.InvokeScript" );
        /*if ( script_invoker != null && (script_invoker.EditVsqDelegate != null || script_invoker.EditVsqDelegate2 != null) ) {
            try {
                VsqFileEx work = (VsqFileEx)m_vsq.clone();
                boolean ret = false;
                if ( script_invoker.EditVsqDelegate != null ) {
                    ret = script_invoker.EditVsqDelegate.Invoke( work );
                } else if ( script_invoker.EditVsqDelegate2 != null ) {
                    ret = script_invoker.EditVsqDelegate2.Invoke( work );
                }
                if ( !ret ) {
                    MessageBox.Show( _( "Script aborted" ), "Cadencii", MessageBoxButtons.OK, MessageBoxIcon.Information );
                } else {
                    CadenciiCommand run = VsqFileEx.generateCommandReplace( work );
                    Register( m_vsq.executeCommand( run ) );
                }
                String config_file = ConfigFileNameFromScriptFileName( script_invoker.ScriptFile );
                using ( FileStream fs = new FileStream( config_file, FileMode.Create ) ) {
                    script_invoker.Serializer.Serialize( fs );
                }
                return ret;
            } catch ( Exception ex ) {
                MessageBox.Show( _( "Script runtime error:" ) + " " + ex, _( "Error" ), MessageBoxButtons.OK, MessageBoxIcon.Information );
#if DEBUG
                AppManager.DebugWriteLine( "    " + ex );
#endif
            }
        } else {
            MessageBox.Show( _( "Script compilation failed." ), _( "Error" ), MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
        }
        return false;*/
    }

    private static String configFileNameFromScriptFileName( String script_file ) {
        String dir = Path.combine( getApplicationDataPath(), "script" );
        File fdir = new File( dir );
        if ( !fdir.exists() ) {
            fdir.mkdir();
        }
        return Path.combine( dir, Path.getFileNameWithoutExtension( script_file ) + ".config" );
    }

    public static String getTempWaveDir {
        File t = File.createTempFile( "tmp", ".tmp" );
        String temp = Path.combine( t.getParent(), _TEMPDIR_NAME );
        t.delete();
        String dir = Path.combine( temp, m_id );
        File ftemp = new File( temp );
        if ( !ftemp.exists() ) {
            ftemp.mkdir();
        }
        File fdir = new File( dir );
        if ( !fdir.exists() ) {
            fdir.mkdir();
        }
        return dir;
    }

    public static boolean isCurveMode() {
        return m_is_curve_mode;
    }

    public static void isCurveMode( boolean value ){
        boolean old = m_is_curve_mode;
        m_is_curve_mode = value;
        if ( old != m_is_curve_mode && SelectedToolChanged != null ) {
            SelectedToolChanged.invoke( AppManager.class, null );
        }
    }

    /// <summary>
    /// コマンドの履歴を削除します
    /// </summary>
    public static void clearCommandBuffer() {
        m_commands.clear();
        m_command_position = -1;
    }

    /// <summary>
    /// アンドゥ処理を行います
    /// </summary>
    public static void undo() {
        if ( isUndoAvailable() ) {
            Command run_src = m_commands.get( m_command_position );
            CadenciiCommand run = (CadenciiCommand)run_src;
            if ( run.vsqCommand != null ) {
                if ( run.vsqCommand.type == VsqCommandType.DeleteTrack ) {
                    int track = (int)run.vsqCommand.args[0];
                    if ( track == Selected && track >= 2 ) {
                        Selected = track - 1;
                    }
                }
            }
            Command inv = m_vsq.executeCommand( run );
            m_commands.set( m_command_position, inv );
            m_command_position--;
        }
    }

    /// <summary>
    /// リドゥ処理を行います
    /// </summary>
    public static void redo() {
        if ( isRedoAvailable() ) {
            Command run_src = m_commands.get( m_command_position + 1 );
            CadenciiCommand run = (CadenciiCommand)run_src;
            if ( run.vsqCommand != null ) {
                if ( run.vsqCommand.type == VsqCommandType.DeleteTrack ) {
                    int track = (int)run.args[0];
                    if ( track == Selected && track >= 2 ) {
                        Selected = track - 1;
                    }
                }
            }
            Command inv = m_vsq.executeCommand( run );
            m_commands.set( m_command_position + 1, inv );
            m_command_position++;
        }
    }

    /// <summary>
    /// コマンドバッファに指定されたコマンドを登録します
    /// </summary>
    /// <param name="command"></param>
    public static void register( Command command ) {
        if ( m_command_position == m_commands.size() - 1 ) {
            // 新しいコマンドバッファを追加する場合
            m_commands.add( command );
            m_command_position = m_commands.size() - 1;
        } else {
            // 既にあるコマンドバッファを上書きする場合
            m_commands.set( m_command_position + 1, command );
            for ( int i = m_commands.size() - 1; i >= m_command_position + 2; i-- ) {
                m_commands.removeElementAt( i );
            }
            m_command_position++;
        }
    }

    /// <summary>
    /// リドゥ操作が可能かどうかを表すプロパティ
    /// </summary>
    public static boolean isRedoAvailable(){
        if( m_commands.size() > 0 && 0 <= m_command_position + 1 && m_command_position + 1 < m_commands.size() ){
            return true;
        } else {
            return false;
        }
    }

    /// <summary>
    /// アンドゥ操作が可能かどうかを表すプロパティ
    /// </summary>
    public static boolean isUndoAvailable(){
        if ( m_commands.size() > 0 && 0 <= m_command_position && m_command_position < m_commands.size() ){
            return true;
        } else {
            return false;
        }
    }

    public static EditTool getSelectedTool(){
        return m_selected_tool;
    }

    public static void setSelectedTool( EditTool value ){
        EditTool old = m_selected_tool;
        m_selected_tool = value;
        if ( old != m_selected_tool && SelectedToolChanged != null ) {
            SelectedToolChanged.invoke( AppManager.class, null );
        }
    }

    // SelectedBezier
    public static Iterator getSelectedBezierIterator() {
        return m_selected_bezier.iterator();
    }

    /// <summary>
    /// ベジエ点のリストに最後に追加された点の情報を取得します
    /// </summary>
    public static SelectedBezierPoint getLastSelectedBezier(){
        if ( m_last_selected_bezier.chainID < 0 || m_last_selected_bezier.pointID < 0 ) {
            return null;
        } else {
            return m_last_selected_bezier;
        }
    }

    /// <summary>
    /// 選択されているベジエ点のリストに、アイテムを追加します
    /// </summary>
    /// <param name="selected">追加する点</param>
    public static void addSelectedBezier( SelectedBezierPoint selected ) {
        m_last_selected_bezier = selected;
        int index = -1;
        for ( int i = 0; i < m_selected_bezier.size(); i++ ) {
            SelectedBezierPoint item = m_selected_bezier.get( i );
            if ( item.chainID == selected.chainID &&
                item.pointID == selected.pointID ) {
                index = i;
                break;
            }
        }
        if ( index >= 0 ) {
            m_selected_bezier.set( index, selected );
        } else {
            m_selected_bezier.add( selected );
        }
    }

    /// <summary>
    /// 選択されているベジエ点のリストを初期化します
    /// </summary>
    public static void clearSelectedBezier() {
        m_selected_bezier.clear();
        m_last_selected_bezier.chainID = -1;
        m_last_selected_bezier.pointID = -1;
    }

    // SelectedTimesig
    /// <summary>
    /// 選択されている拍子変更イベントのリスト
    /// </summary>
    public static TreeMap<Integer, SelectedTimesigEntry> getSelectedTimesig(){
        return m_selected_timesig;
    }

    public static SelectedTimesigEntry getLastSelectedTimesig(){
        if ( m_selected_timesig.containsKey( m_last_selected_timesig ) ) {
            return m_selected_timesig.get( m_last_selected_timesig );
        } else {
            return null;
        }
    }

    public static int getLastSelectedTimesigBarcount(){
        return m_last_selected_timesig;
    }

    public static void addSelectedTimesig( int barcount ) {
        clearSelectedEvent(); //ここ注意！
        clearSelectedTempo();
        m_last_selected_timesig = barcount;
        if ( !m_selected_timesig.containsKey( barcount ) ) {
            for( TimeSigTableEntry tte : m_vsq.timeSigTable ) {
                if ( tte.barCount == barcount ) {
                    m_selected_timesig.add( barcount, new SelectedTimesigEntry( tte, (TimeSigTableEntry)tte.clone() ) );
                    break;
                }
            }
        }
    }

    public static void clearSelectedTimesig() {
        m_selected_timesig.clear();
        m_last_selected_timesig = -1;
    }

    // SelectedTempo
    /// <summary>
    /// 選択されているテンポ変更イベントのリスト
    /// </summary>
    public static TreeMap<Integer, SelectedTempoEntry> getSelectedTempo(){
        return m_selected_tempo;
    }

    public static SelectedTempoEntry getLastSelectedTempo(){
        if ( m_selected_tempo.containsKey( m_last_selected_tempo ) ) {
            return m_selected_tempo.get( m_last_selected_tempo );
        } else {
            return null;
        }
    }

    public static int getLastSelectedTempoClock(){
        return m_last_selected_tempo;
    }

    public static void addSelectedTempo( int clock ) {
        clearSelectedEvent(); //ここ注意！
        clearSelectedTimesig();
        m_last_selected_tempo = clock;
        if ( !m_selected_tempo.containsKey( clock ) ) {
            for( TempoTableEntry tte : m_vsq.tempoTable ) {
                if ( tte.clock == clock ) {
                    m_selected_tempo.add( clock, new SelectedTempoEntry( tte, (TempoTableEntry)tte.clone() ) );
                    break;
                }
            }
        }
    }

    public static void clearSelectedTempo() {
        m_selected_tempo.clear();
        m_last_selected_tempo = -1;
    }

    // SelectedEvent
    public static SelectedEventList getSelectedEvent(){
        return m_selected_event;
    }

    public static void removeSelectedEvent( int id ) {
        removeSelectedEventCor( id, false );
    }

    public static void removeSelectedEventSilent( int id ) {
        removeSelectedEventCor( id, true );
    }

    private static void removeSelectedEventCor( int id, boolean silent ) {
        m_selected_event.remove( id );
        NotePropertyPanel.updateValue( m_selected );
    }

    public static void removeSelectedEventRange( int[] ids ) {
        m_selected_event.removeRange( ids );
        NotePropertyPanel.updateValue( m_selected );
    }

    public static void addSelectedEventRange( int[] ids ) {
        clearSelectedTempo();
        clearSelectedTimesig();
        Vector<Integer> list = new Vector<Integer>( ids );
        VsqEvent[] index = new VsqEvent[ids.length];
        int count = 0;
        for ( Iterator itr = m_vsq.tracks.get( m_selected ).getEventIterator(); itr.hasNext(); ) {
            VsqEvent ev = (VsqEvent)itr.next();
            int find = -1;
            int c = list.size();
            for( int i = 0; i < c; i++ ){
                if( list.get( i ) == ev.internalID ){
                    find = i;
                    break;
                }
            }
            if ( 0 <= find ) {
                index[find] = ev;
                count++;
            }
            if ( count == ids.length ) {
                break;
            }
        }
        for ( int i = 0; i < index.length; i++ ) {
            m_selected_event.add( new SelectedEventEntry( m_selected, index[i], (VsqEvent)index[i].clone() ) );
        }
        if ( SelectedEventChanged != null ) {
            SelectedEventChanged.invoke( false );
        }
        NotePropertyPanel.updateValue( m_selected );
    }

    public static void addSelectedEvent( int id ) {
        addSelectedEventCor( id, false );
    }

    public static void addSelectedEventSilent( int id ) {
        addSelectedEventCor( id, true );
    }

    private static void addSelectedEventCor( int id, boolean silent ) {
        clearSelectedTempo();
        clearSelectedTimesig();
        for ( Iterator itr = m_vsq.tracks.get( m_selected ).getEventIterator(); itr.hasNext(); ) {
            VsqEvent ev = (VsqEvent)itr.next();
            if ( ev.InternalID == id ) {
                m_selected_event.add( new SelectedEventEntry( m_selected, ev, (VsqEvent)ev.clone() ) );
                if ( !silent && SelectedEventChanged != null ) {
                    SelectedEventChanged.invoke( false );
                }
                break;
            }
        }
        if ( !silent ) {
            NotePropertyPanel.updateValue( m_selected );
        }
    }

    public static void clearSelectedEvent() {
        m_selected_event.clear();
        NotePropertyPanel.updateValue( m_selected );
        if ( SelectedEventChanged != null ) {
            SelectedEventChanged.invoke( true );
        }
    }

    public static boolean isOverlay(){
        return m_overlay;
    }

    public static void setOverlay( boolean value ){
        m_overlay = value;
    }

    public static boolean getRenderRequired( int track ) {
        return m_render_required[track - 1];
    }

    public static void setRenderRequired( int track, boolean value ) {
        m_render_required[track - 1] = value;
    }

    /// <summary>
    /// 現在の編集モード
    /// </summary>
    public static EditMode getEditMode(){
        return m_edit_mode;
    }

    public static void setEditMode( EditMode value ){
        m_edit_mode = value;
    }

    /// <summary>
    /// グリッドを表示するか否かを表すフラグを取得または設定します
    /// </summary>
    public static boolean isGridVisible(){
        return m_grid_visible;
    }

    public static void setGridVisible( boolean value ){
        if ( value != m_grid_visible ) {
            m_grid_visible = value;
            if ( GridVisibleChanged != null ) {
                GridVisibleChanged.invoke( AppManager.class, null );
            }
        }
    }

    /// <summary>
    /// 現在のプレビューがリピートモードであるかどうかを表す値を取得または設定します
    /// </summary>
    public static boolean isRepeatMode(){
        return m_repeat_mode;
    }

	public static void setRepeatMode( boolean value ){
        m_repeat_mode = value;
    }

    /// <summary>
    /// 現在プレビュー中かどうかを示す値を取得または設定します
    /// </summary>
    public static boolean isPlaying(){
        return m_playing;
    }

	public static void setPlaying( boolean value ){
        boolean previous = m_playing;
        m_playing = value;
        if ( previous != m_playing ) {
            if ( m_playing && PreviewStarted != null ) {
                int clock = getCurrentClock();
                PreviewStarted.invoke( AppManager.class, null );
            } else if ( !m_playing && PreviewAborted != null ) {
                PreviewAborted.invoke( AppManager, null );
            }
        }
    }

    /// <summary>
    /// _vsq_fileにセットされたvsqファイルの名前を取得します。
    /// </summary>
    public static String getFileName(){
        return m_file;
    }

    public static void saveTo( String file ) {
        if ( m_vsq != null ) {
            String path = Path.getDirectoryName( file );
            String file2 = Path.combine( path, Path.getFileNameWithoutExtension( file ) + ".vsq" );
            m_vsq.writeAsXml( file );
            m_vsq.write( file2 );
            m_file = file;
            EditorConfig.pushRecentFiles( m_file );
        }
    }

    /* // <summary>
    /// 位置clockにおけるテンポ、拍子、小節数を取得します
    /// </summary>
    /// <param name="clock"></param>
    /// <param name="numerator"></param>
    /// <param name="denominator"></param>
    /// <param name="bar_count"></param>
    /// <param name="tempo"></param>
    public static void getTempoAndTimeSignature( 
    	int clock,
    	out int numerator,
        out int denominator,
        out int bar_count,
        out int tempo,
        out int time_sig_index,
        out int tempo_index,
        out int last_clock ) {
        numerator = 4;
        denominator = 4;
        bar_count = 0;
        tempo = 480000;
        time_sig_index = -1;
        tempo_index = -1;
        last_clock = 0;
        if ( m_vsq != null ) {
            int index = -1;
            for ( int i = 0; i < m_vsq.TimesigTable.Count; i++ ) {
                if ( m_vsq.TimesigTable[i].Clock > clock ) {
                    index = i - 1;
                    break;
                }
            }
            if ( index >= 0 ) {
                denominator = m_vsq.TimesigTable[index].Denominator;
                numerator = m_vsq.TimesigTable[index].Numerator;
                time_sig_index = index;
                last_clock = m_vsq.TimesigTable[index].Clock;
            }
            bar_count = m_vsq.getBarCountFromClock( clock );

            index = -1;
            for ( int i = 0; i < m_vsq.TempoTable.Count; i++ ) {
                if ( m_vsq.TempoTable[i].Clock > clock ) {
                    index = i - 1;
                    break;
                }
            }
            if ( index >= 0 ) {
                tempo = m_vsq.TempoTable[index].Tempo;
                tempo_index = index;
            }
        }
    }*/

    /// <summary>
    /// 現在の演奏マーカーの位置を取得または設定します。
    /// </summary>
    public static int getCurrentClock(){
        return m_current_clock;
    }

	public static void setCurrentClock( int value ){
        int old = m_current_clock;
        m_current_clock = value;
        int barcount = m_vsq.getBarCountFromClock( m_current_clock );
        int bar_top_clock = m_vsq.getClockFromBarCount( barcount );
        int numerator = 4;
        int denominator = 4;
        m_vsq.getTimesigAt( m_current_clock, out numerator, out denominator );
        int clock_per_beat = 480 / 4 * denominator;
        int beat = (m_current_clock - bar_top_clock) / clock_per_beat;
        m_current_play_position.barCount = barcount - m_vsq.getPreMeasure() + 1;
        m_current_play_position.beat = beat + 1;
        m_current_play_position.clock = m_current_clock - bar_top_clock - clock_per_beat * beat;
        m_current_play_position.denominator = denominator;
        m_current_play_position.numerator = numerator;
        m_current_play_position.tempo = m_vsq.getTempoAt( m_current_clock );
        if ( old != m_current_clock && CurrentClockChanged != null ) {
            CurrentClockChanged.invoke( AppManager.class, null );
        }
    }

    /// <summary>
    /// 現在の演奏カーソルの位置(m_current_clockと意味は同じ。CurrentClockが変更されると、自動で更新される)
    /// </summary>
    public static PlayPositionSpecifier getPlayPosition(){
        return m_current_play_position;
    }

    /// <summary>
    /// 現在選択されているトラックを取得または設定します
    /// </summary>
    public static int getSelected(){
        int tracks = m_vsq.tracks.size();
        if ( tracks <= m_selected ) {
            m_selected = tracks - 1;
        }
        return m_selected;
    }

	public static void setSelected( int value ){
        m_selected = value;
    }

    /// <summary>
    /// vsqファイルを読込みます
    /// </summary>
    /// <param name="file"></param>
    public static void readVsq( String file ) {
        m_selected = 1;
        m_file = file;
        VsqFileEx newvsq = null;
        try {
            newvsq = VsqFileEx.readFromXml( file );
        } catch ( Exception ex ) {
            debugWriteLine( "EditorManager.ReadVsq; ex=" + ex );
            return;
        }
        if ( newvsq == null ) {
            return;
        }
        m_vsq = newvsq;
        for ( int i = 0; i < m_render_required.length; i++ ) {
            if ( i < m_vsq.tracks.size() - 1 ) {
                m_render_required[i] = true;
            } else {
                m_render_required[i] = false;
            }
        }
        setStartMarker( m_vsq.getPreMeasureClocks() );
        int bar = m_vsq.getPreMeasure() + 1;
        setEndMarker( m_vsq.getClockFromBarCount( bar ) );
        if ( m_vsq.tracks.size() >= 1 ) {
            m_selected = 1;
        } else {
            m_selected = -1;
        }
    }

    /// <summary>
    /// vsqファイル。
    /// </summary>
    public static VsqFileEx getVsqFile(){
        return m_vsq;
    }

    public static void setVsqFile( VsqFileEx vsq ) {
        m_vsq = vsq;
        for ( int i = 0; i < m_render_required.length; i++ ) {
            if ( i < m_vsq.tracks.size() - 1 ) {
                m_render_required[i] = true;
            } else {
                m_render_required[i] = false;
            }
        }
        m_file = "";
        setStartMarker( m_vsq.getPreMeasureClocks() );
        int bar = m_vsq.getPreMeasure() + 1;
        setEndMarker( m_vsq.getClockFromBarCount( bar ) );
    }

    /// <summary>
    /// 現在表示されているピアノロール画面の右上の、仮想スクリーン上座標で見たときのy座標(pixel)
    /// </summary>
    public static int getStartToDrawY(){
        return m_start_to_draw_y;
    }

	public static void setStartToDrawY( int value ){
        m_start_to_draw_y = value;
    }

    public static void init() {
        loadConfig();
        VSTiProxy.init();
        s_locker = new Object();
        SymbolTable.loadDictionary();
        VSTiProxy.CurrentUser = "";

        #region Apply User Dictionary Configuration
        Vector<ValuePair<String, boolean>> current = new Vector<ValuePair<String, boolean>>();
        for ( int i = 0; i < SymbolTable.getCount(); i++ ) {
            current.add( new ValuePair<String, boolean>( SymbolTable.getSymbolTable( i ).getName(), false ) );
        }
        Vector<ValuePair<String, boolean>> config_data = new Vector<ValuePair<String, boolean>>();
        for ( int i = 0; i < EditorConfig.UserDictionaries.Count; i++ ) {
            String[] spl = EditorConfig.UserDictionaries[i].Split( "\t".ToCharArray(), 2 );
            config_data.add( new ValuePair<String, boolean>( spl[0], (spl[1] == "T" ? true : false) ) );
#if DEBUG
            AppManager.DebugWriteLine( "    " + spl[0] + "," + spl[1] );
#endif
        }
        Vector<KeyValuePair<String, boolean>> common = new Vector<KeyValuePair<String, boolean>>();
        for ( int i = 0; i < config_data.Count; i++ ) {
            for ( int j = 0; j < current.Count; j++ ) {
                if ( config_data[i].Key == current[j].Key ) {
                    current[j].Value = true; //こっちのbooleanは、AppManager.EditorConfigのUserDictionariesにもKeyが含まれているかどうかを表すので注意
                    common.add( new KeyValuePair<String, boolean>( config_data[i].Key, config_data[i].Value ) );
                    break;
                }
            }
        }
        for ( int i = 0; i < current.Count; i++ ) {
            if ( !current[i].Value ) {
                common.add( new KeyValuePair<String, boolean>( current[i].Key, false ) );
            }
        }
        SymbolTable.changeOrder( common.ToArray() );
        #endregion

        Boare.Lib.AppUtil.Messaging.LoadMessages();
        Boare.Lib.AppUtil.Messaging.Language = EditorConfig.Language;

        KeySoundPlayer.Init();
        PaletteToolServer.Init();

#if !TREECOM
        m_id = bocoree.misc.getmd5( DateTime.Now.ToBinary().ToString() );
        String log = Path.Combine( TempWaveDir, "run.log" );
#endif
        NotePropertyPanel = new PropertyPanel();
        NotePropertyDlg = new FormNoteProperty();
        NotePropertyDlg.Controls.add( NotePropertyPanel );
        NotePropertyPanel.Dock = DockStyle.Fill;
    }

    public static String GetShortcutDisplayString( Keys[] keys ) {
        String ret = "";
        Vector<Keys> list = new Vector<Keys>( keys );
        if ( list.Contains( Keys.Menu ) ) {
            ret = new String( '\x2318', 1 );
        }
        if ( list.Contains( Keys.Control ) ) {
            ret += (ret == "" ? "" : "+") + "Ctrl";
        }
        if ( list.Contains( Keys.Shift ) ) {
            ret += (ret == "" ? "" : "+") + "Shift";
        }
        if ( list.Contains( Keys.Alt ) ) {
            ret += (ret == "" ? "" : "+") + "Alt";
        }
        Vector<Keys> list2 = new Vector<Keys>();
        foreach ( Keys key in keys ) {
            AppManager.DebugWriteLine( "    " + key );
            if ( key != Keys.Control && key != Keys.Shift && key != Keys.Alt ) {
                list2.add( key );
            }
        }
        list2.Sort();
        for ( int i = 0; i < list2.Count; i++ ) {
            ret += (ret == "" ? "" : "+") + GetKeyDisplayString( list2[i] );
        }
        return ret;
    }

    private static String GetKeyDisplayString( Keys key ) {
        switch ( key ) {
            case Keys.PageDown:
                return "PgDn";
            case Keys.PageUp:
                return "PgUp";
            case Keys.D0:
                return "0";
            case Keys.D1:
                return "1";
            case Keys.D2:
                return "2";
            case Keys.D3:
                return "3";
            case Keys.D4:
                return "4";
            case Keys.D5:
                return "5";
            case Keys.D6:
                return "6";
            case Keys.D7:
                return "7";
            case Keys.D8:
                return "8";
            case Keys.D9:
                return "9";
            case Keys.Menu:
                return new String( '\x2318', 1 );
            default:
                return key.ToString();
        }
    }

    /// <summary>
    /// オブジェクトをシリアライズし，クリップボードに格納するための文字列を作成します
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static String GetSerializedText( object obj ) {
        String str = "";
        using ( MemoryStream ms = new MemoryStream() ) {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize( ms, obj );
            ms.Seek( 0, SeekOrigin.Begin );
            byte[] arr = new byte[ms.Length];
            ms.Read( arr, 0, arr.Length );
            str = _CLIP_PREFIX + ":" + obj.GetType().FullName + ":" + Convert.ToBase64String( arr );
        }
        return str;
    }

    /// <summary>
    /// クリップボードに格納された文字列を元に，デシリアライズされたオブジェクトを取得します
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static object GetDeserializedObjectFromText( String s ) {
        if ( s.StartsWith( _CLIP_PREFIX ) ) {
            int index = s.IndexOf( ":" );
            index = s.IndexOf( ":", index + 1 );
            object ret = null;
            try {
                using ( MemoryStream ms = new MemoryStream( Convert.FromBase64String( s.Substring( index + 1 ) ) ) ) {
                    BinaryFormatter bf = new BinaryFormatter();
                    ret = bf.Deserialize( ms );
                }
            } catch {
                ret = null;
            }
            return ret;
        } else {
            return null;
        }
    }

    public static void ClearClipBoard() {
        if ( Clipboard.ContainsText() ) {
            if ( Clipboard.GetText().StartsWith( _CLIP_PREFIX ) ) {
                Clipboard.Clear();
            }
        }
    }

    public static void SetClipboard( ClipboardEntry item ) {
        String clip = GetSerializedText( item );
        Clipboard.Clear();
        Clipboard.SetText( clip );
    }

    #region VsqEvent用のクリップボード管理
    public static Vector<VsqEvent> GetCopiedEvent() {
        if ( Clipboard.ContainsText() ) {
            String clip = Clipboard.GetText();
            if ( clip.StartsWith( _CLIP_PREFIX ) ) {
                int index1 = clip.IndexOf( ":" );
                int index2 = clip.IndexOf( ":", index1 + 1 );
                String typename = clip.Substring( index1 + 1, index2 - index1 - 1 );
#if DEBUG
                AppManager.DebugWriteLine( "typename=" + typename );
#endif
                if ( typename == typeof( ClipboardEntry ).FullName ) {
                    Vector<VsqEvent> ret = null;
                    try {
                        ClipboardEntry ce = (ClipboardEntry)GetDeserializedObjectFromText( clip );
                        ret = ce.Event;
                    } catch {
                        ret = null;
                    }
                    if ( ret != null ) {
                        return ret;
                    }
                }
            }
        }
        return new Vector<VsqEvent>();
    }

    public static void SetCopiedEvent( Vector<VsqEvent> item ) {
        ClipboardEntry ce = new ClipboardEntry();
        ce.Event = item;
        String clip = GetSerializedText( ce );
        Clipboard.Clear();
        Clipboard.SetText( clip );
    }
    #endregion

    #region TempoTableEntry用のクリップボード管理
    public static Vector<TempoTableEntry> GetCopiedTempo( out int copy_started_clock ) {
        Vector<TempoTableEntry> tempo_table = null;
        copy_started_clock = 0;
        if ( Clipboard.ContainsText() ) {
            String clip = Clipboard.GetText();
            if ( clip.StartsWith( _CLIP_PREFIX ) ) {
                int index1 = clip.IndexOf( ":" );
                int index2 = clip.IndexOf( ":", index1 + 1 );
                String typename = clip.Substring( index1 + 1, index2 - index1 - 1 );
                if ( typename == typeof( ClipboardEntry ).FullName ) {
                    try {
                        ClipboardEntry ce = (ClipboardEntry)GetDeserializedObjectFromText( clip );
                        tempo_table = ce.Tempo;
                        copy_started_clock = ce.CopyStartedClock;
                    } catch {
                        tempo_table = null;
                    }
                }
            }
        }
        if ( tempo_table == null ) {
            tempo_table = new Vector<TempoTableEntry>();
            copy_started_clock = 0;
        }
        return tempo_table;
    }

    public static void SetCopiedTempo( Vector<TempoTableEntry> item, int copy_started_clock ) {
        ClipboardEntry ce = new ClipboardEntry();
        ce.Tempo = item;
        String clip = GetSerializedText( ce );
        Clipboard.Clear();
        Clipboard.SetText( clip );
    }
    #endregion

    #region TimeSigTableEntry用のクリップボード管理
    public static Vector<TimeSigTableEntry> GetCopiedTimesig( out int copy_started_clock ) {
        Vector<TimeSigTableEntry> tempo_table = null;
        copy_started_clock = 0;
        if ( Clipboard.ContainsText() ) {
            String clip = Clipboard.GetText();
            if ( clip.StartsWith( _CLIP_PREFIX ) ) {
                int index1 = clip.IndexOf( ":" );
                int index2 = clip.IndexOf( ":", index1 + 1 );
                String typename = clip.Substring( index1 + 1, index2 - index1 - 1 );
                if ( typename == typeof( ClipboardEntry ).FullName ) {
                    try {
                        ClipboardEntry ce = (ClipboardEntry)GetDeserializedObjectFromText( clip );
                        tempo_table = ce.Timesig;
                        copy_started_clock = ce.CopyStartedClock;
                    } catch {
                        tempo_table = null;
                    }
                }
            }
        }
        if ( tempo_table == null ) {
            tempo_table = new Vector<TimeSigTableEntry>();
            copy_started_clock = 0;
        }
        return tempo_table;
    }

    public static void SetCopiedTimesig( Vector<TimeSigTableEntry> item, int copy_started_clock ) {
        ClipboardEntry ce = new ClipboardEntry();
        ce.Timesig = item;
        String clip = GetSerializedText( ce );
        Clipboard.Clear();
        Clipboard.SetText( clip );
    }
    #endregion

    #region CurveType, Vector<BPPair>用のクリップボード管理
    public static Dictionary<CurveType, Vector<BPPair>> GetCopiedCurve( out int copy_started_clock ) {
        Dictionary<CurveType, Vector<BPPair>> tempo_table = null;
        copy_started_clock = 0;
        if ( Clipboard.ContainsText() ) {
            String clip = Clipboard.GetText();
            if ( clip.StartsWith( _CLIP_PREFIX ) ) {
                int index1 = clip.IndexOf( ":" );
                int index2 = clip.IndexOf( ":", index1 + 1 );
                String typename = clip.Substring( index1 + 1, index2 - index1 - 1 );
                if ( typename == typeof( ClipboardEntry ).FullName ) {
                    try {
                        ClipboardEntry ce = (ClipboardEntry)GetDeserializedObjectFromText( clip );
                        tempo_table = ce.Curve;
                        copy_started_clock = ce.CopyStartedClock;
                    } catch {
                        tempo_table = null;
                    }
                }
            }
        }
        if ( tempo_table == null ) {
            tempo_table = new Dictionary<CurveType, Vector<BPPair>>();
            copy_started_clock = 0;
        }
        return tempo_table;
    }

    public static void SetCopiedCurve( Dictionary<CurveType, Vector<BPPair>> item, int copy_started_clock ) {
        ClipboardEntry ce = new ClipboardEntry();
        ce.Curve = item;
        String clip = GetSerializedText( ce );
        Clipboard.Clear();
        Clipboard.SetText( clip );
    }
    #endregion

    #region CurveType, Vector<BezierChain>用のクリップボード管理
    public static Dictionary<CurveType, Vector<BezierChain>> GetCopiedBezier( out int copy_started_clock ) {
        Dictionary<CurveType, Vector<BezierChain>> tempo_table = null;
        copy_started_clock = 0;
        if ( Clipboard.ContainsText() ) {
            String clip = Clipboard.GetText();
            if ( clip.StartsWith( _CLIP_PREFIX ) ) {
                int index1 = clip.IndexOf( ":" );
                int index2 = clip.IndexOf( ":", index1 + 1 );
                String typename = clip.Substring( index1 + 1, index2 - index1 - 1 );
                if ( typename == typeof( ClipboardEntry ).FullName ) {
                    try {
                        ClipboardEntry ce = (ClipboardEntry)GetDeserializedObjectFromText( clip );
                        tempo_table = ce.Bezier;
                        copy_started_clock = ce.CopyStartedClock;
                    } catch {
                        tempo_table = null;
                    }
                }
            }
        }
        if ( tempo_table == null ) {
            tempo_table = new Dictionary<CurveType, Vector<BezierChain>>();
            copy_started_clock = 0;
        }
        return tempo_table;
    }

    public static void SetCopiedBezier( Dictionary<CurveType, Vector<BezierChain>> item, int copy_started_clock ) {
        ClipboardEntry ce = new ClipboardEntry();
        ce.Bezier = item;
        String clip = GetSerializedText( ce );
        Clipboard.Clear();
        Clipboard.SetText( clip );
    }
    #endregion

    public static Assembly CompileScript( String code, out CompilerResults results ) {
        CSharpCodeProvider provider = new CSharpCodeProvider();
        String path = Application.StartupPath;
        CompilerParameters parameters = new CompilerParameters( new String[] {
            Path.Combine( path, "Boare.Lib.Vsq.dll" ),
            Path.Combine( path, "Cadencii.exe" ),
            Path.Combine( path, "Boare.Lib.Media.dll" ),
            Path.Combine( path, "Boare.Lib.AppUtil.dll" ),
            Path.Combine( path, "bocoree.dll" ) } );
        parameters.ReferencedAssemblies.add( "System.Windows.Forms.dll" );
        parameters.ReferencedAssemblies.add( "System.dll" );
        parameters.ReferencedAssemblies.add( "System.Drawing.dll" );
        parameters.ReferencedAssemblies.add( "System.Xml.dll" );
        parameters.GenerateInMemory = true;
        parameters.GenerateExecutable = false;
        parameters.IncludeDebugInformation = true;
        try {
            results = provider.CompileAssemblyFromSource( parameters, code );
            return results.CompiledAssembly;
        } catch ( Exception ex ){
#if DEBUG
            AppManager.DebugWriteLine( "AppManager.CompileScript; ex=" + ex );
#endif
            results = null;
            return null;
        }
    }

    /// <summary>
    /// アプリケーションデータの保存位置を取得します
    /// Gets the path for application data
    /// </summary>
    public static String ApplicationDataPath {
        get {
            String dir = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Boare" );
            if ( !Directory.Exists( dir ) ) {
                Directory.CreateDirectory( dir );
            }
            String dir2 = Path.Combine( dir, _CONFIG_DIR_NAME );
            if ( !Directory.Exists( dir2 ) ) {
                Directory.CreateDirectory( dir2 );
            }
            return dir2;
        }
    }

    /// <summary>
    /// 位置クオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
    /// </summary>
    /// <returns></returns>
    public static int GetPositionQuantizeClock() {
        return QuantizeModeUtil.GetQuantizeClock( EditorConfig.PositionQuantize, EditorConfig.PositionQuantizeTriplet );
    }

    /// <summary>
    /// 音符長さクオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
    /// </summary>
    /// <returns></returns>
    public static int GetLengthQuantizeClock() {
        return QuantizeModeUtil.GetQuantizeClock( EditorConfig.LengthQuantize, EditorConfig.LengthQuantizeTriplet );
    }

    public static void SaveConfig() {
        // ユーザー辞書の情報を取り込む
        EditorConfig.UserDictionaries.Clear();
        for ( int i = 0; i < SymbolTable.getCount(); i++ ) {
            EditorConfig.UserDictionaries.add( SymbolTable.getSymbolTable( i ).getName() + "\t" + (SymbolTable.getSymbolTable( i ).isEnabled() ? "T" : "F") );
        }

        String file = Path.Combine( ApplicationDataPath, _CONFIG_FILE_NAME );
        try {
            EditorConfig.Serialize( EditorConfig, file );
        } catch {
        }
    }

    public static void LoadConfig() {
        String config_file = Path.Combine( ApplicationDataPath, _CONFIG_FILE_NAME );
        EditorConfig ret = null;
        if ( File.Exists( config_file ) ) {
            try {
                ret = EditorConfig.Deserialize( EditorConfig, config_file );
            } catch {
                ret = null;
            }
        } else {
            config_file = Path.Combine( Application.StartupPath, _CONFIG_FILE_NAME );
            if ( File.Exists( config_file ) ) {
                try {
                    ret = EditorConfig.Deserialize( EditorConfig, config_file );
                } catch {
                    ret = null;
                }
            }
        }
        if ( ret == null ) {
            ret = new EditorConfig();
        }
        EditorConfig = ret;
        for ( int i = 0; i < SymbolTable.getCount(); i++ ) {
            SymbolTable st = SymbolTable.getSymbolTable( i );
            boolean found = false;
            foreach ( String s in EditorConfig.UserDictionaries ) {
                String[] spl = s.Split( "\t".ToCharArray(), 2 );
                if ( st.getName() == spl[0] ) {
                    found = true;
                    break;
                }
            }
            if ( !found ) {
                EditorConfig.UserDictionaries.add( st.getName() + "\tT" );
            }
        }
        MidiPlayer.DeviceGeneral = (uint)EditorConfig.MidiDeviceGeneral.PortNumber;
        MidiPlayer.DeviceMetronome = (uint)EditorConfig.MidiDeviceMetronome.PortNumber;
        MidiPlayer.NoteBell = EditorConfig.MidiNoteBell;
        MidiPlayer.NoteNormal = EditorConfig.MidiNoteNormal;
        MidiPlayer.PreUtterance = EditorConfig.MidiPreUtterance;
        MidiPlayer.ProgramBell = EditorConfig.MidiProgramBell;
        MidiPlayer.ProgramNormal = EditorConfig.MidiProgramNormal;
        MidiPlayer.RingBell = EditorConfig.MidiRingBell;
    }

    public static VsqID getSingerIDUtau( String name ) {
        VsqID ret = new VsqID( 0 );
        ret.type = VsqIDType.Singer;
        int index = -1;
        for ( int i = 0; i < EditorConfig.UtauSingers.Count; i++ ) {
            if ( EditorConfig.UtauSingers[i].VOICENAME == name ) {
                index = i;
                break;
            }
        }
        if ( index >= 0 ) {
            SingerConfig sc = EditorConfig.UtauSingers[index];
            int lang = 0;//utauは今のところ全部日本語
            ret.IconHandle = new IconHandle();
            ret.IconHandle.IconID = "$0701" + index.ToString( "0000" );
            ret.IconHandle.IDS = sc.VOICENAME;
            ret.IconHandle.Index = 0;
            ret.IconHandle.Language = lang;
            ret.IconHandle.Length = 1;
            ret.IconHandle.Original = sc.Original;
            ret.IconHandle.Program = sc.Program;
            ret.IconHandle.Caption = "";
            return ret;
        } else {
            ret.IconHandle = new IconHandle();
            ret.IconHandle.Program = 0;
            ret.IconHandle.Language = 0;
            ret.IconHandle.IconID = "$0701" + 0.ToString( "0000" );
            ret.IconHandle.IDS = "Unknown";
            ret.type = VsqIDType.Singer;
            return ret;
        }
    }

    public static SingerConfig getSingerInfoUtau( int program_change ) {
        if ( 0 <= program_change && program_change < EditorConfig.UtauSingers.Count ) {
            return EditorConfig.UtauSingers[program_change];
        } else {
            return null;
        }
    }

    public static String _VERSION {
        get {
            String prefix = "";
            String rev = "";
            // $Id: AppManager.cs 299 2009-07-01 16:54:59Z kbinani $
            String id = GetAssemblyConfigurationAttribute();
            String[] spl0 = id.Split( " ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
            if ( spl0.Length >= 3 ) {
                String s = spl0[2];
#if DEBUG
                AppManager.DebugWriteLine( "AppManager.get__VERSION; s=" + s );
#endif
                String[] spl = s.Split( " ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
                if ( spl.Length > 0 ) {
                    rev = spl[0];
                }
            }
            if ( rev == "" ) {
                rev = "?";
            }
#if DEBUG
            prefix = "(rev: " + rev + "; build: debug)";
#else
            prefix = "(rev: " + rev + "; build: release)";
#endif
            return GetAssemblyFileVersion( typeof( AppManager ) ) + " " + prefix;
        }
    }

    public static String GetAssemblyConfigurationAttribute() {
        Assembly a = Assembly.GetAssembly( typeof( AppManager ) );
        AssemblyConfigurationAttribute attr = (AssemblyConfigurationAttribute)Attribute.GetCustomAttribute( a, typeof( AssemblyConfigurationAttribute ) );
#if DEBUG
        AppManager.DebugWriteLine( "GetAssemblyConfigurationAttribute; attr.Configuration=" + attr.Configuration );
#endif
        return attr.Configuration;
    }

    public static String GetAssemblyFileVersion( Type t ) {
        Assembly a = Assembly.GetAssembly( t );
        AssemblyFileVersionAttribute afva = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute( a, typeof( AssemblyFileVersionAttribute ) );
        return afva.Version;
    }

    public static String GetAssemblyNameAndFileVersion( Type t ) {
        Assembly a = Assembly.GetAssembly( t );
        AssemblyFileVersionAttribute afva = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute( a, typeof( AssemblyFileVersionAttribute ) );
        return a.GetName().Name + " v" + afva.Version;
    }

    public static SolidBrush HilightBrush {
        get {
            return s_hilight_brush;
        }
    }

    public static Color HilightColor {
        get {
            return s_hilight_brush.Color;
        }
        set {
            s_hilight_brush = new SolidBrush( value );
        }
    }

    /// <summary>
    /// ベースとなるテンポ。
    /// </summary>
    public static int BaseTempo {
        get {
            return s_base_tempo;
        }
        set {
            s_base_tempo = value;
        }
    }
}
