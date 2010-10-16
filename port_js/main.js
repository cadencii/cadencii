var _PX_ACCENT_HEADER = 21;
var _PICT_POSITION_INDICATOR_HEIGHT = 48;
var _MAX_FPS = 30.0;
var _MIN_PAINT_INTERVAL = 1.0 / _MAX_FPS;
var _SCROLL_WIDTH = 14;
var _SCROLL_SPEED = 50;
var _MENUBAR_HEIGHT = 25;

MouseMode = {};
MouseMode.NONE = 0;
MouseMode.DOWN_ON_PIANOROLL = 1;
MouseMode.DOWN_ON_HSCROLL = 2;
MouseMode.DOWN_ON_VSCROLL = 3;

var trackselector_height = 0;
var dropbox;
var pictPianoRoll, divPictPianoRoll;
var trackSelector, divTrackSelector;
var hScroll = { value: 0, minimum: 0, maximum: 100, visibleAmount: 10, width: 100, height: _SCROLL_WIDTH, left: 68, top: 100, box_pos: _SCROLL_WIDTH, box_length: 1 };
var vScroll = { value: 0, minimum: 0, maximum: 100, visibleAmount: 10, width: _SCROLL_WIDTH, height: 100, left: 100, top: _PICT_POSITION_INDICATOR_HEIGHT, box_pos: _SCROLL_WIDTH, box_length: 1 };
var trackBar = { value: 0, minimum: 0, maximum: 100, width: 100, height: _SCROLL_WIDTH, left: 100, top: 100 };
var divMenuHolder;

var m_mouse_down_x, m_mouse_down_y;
var m_stdx, m_stdy; //マウスが降りたときのstartToDraw(X|Y)の値
var m_hscroll_value, m_vscroll_valule;//マウスが降りたときの(h|v)Scrollの値
var mouseMode = MouseMode.NONE;
var mouseX = 0, mouseY = 0;
var _last_paint_time = 0.0;

/**
 * スクロールの場所を決める
 */
function updateScrollLocation( draft_start_to_draw_x, draft_start_to_draw_y ){
    var scalex = org.kbinani.cadencii.AppManager.scaleX;
    var draft_hscroll_value = draft_start_to_draw_x / scalex;
    var draft_vscroll_value = draft_start_to_draw_y;
    
    if( draft_hscroll_value < hScroll.minimum ){
        draft_hscroll_value = hScroll.minimum;
    }else if( hScroll.maximum < draft_hscroll_value ){
        draft_hscroll_value = hScroll.maximum;
    }
    if( draft_vscroll_value < vScroll.minimum ){
        draft_vscroll_value = vScroll.minimum;
    }else if( vScroll.maximum < draft_vscroll_value ){
        draft_vscroll_value = vScroll.maximum;
    }
    draft_start_to_draw_x = draft_hscroll_value * scalex;
    draft_start_to_draw_y = draft_vscroll_value;
    hScroll.value = draft_hscroll_value; 
    vScroll.value = draft_vscroll_value;
    org.kbinani.cadencii.AppManager.setStartToDrawX( draft_start_to_draw_x );
    org.kbinani.cadencii.AppManager.setStartToDrawY( draft_start_to_draw_y );
}

function updateScrollRange(){
    hScroll.minimum = 0;
    hScroll.maximum = org.kbinani.cadencii.AppManager.getVsqFile().TotalClocks + 1920;
    var visible_clocks = org.kbinani.cadencii.AppManager.clockFromXCoord( window.innerWidth ) - org.kbinani.cadencii.AppManager.clockFromXCoord( org.kbinani.cadencii.AppManager.keyWidth );
    hScroll.visibleAmount = visible_clocks;

    vScroll.minimum = -_PICT_POSITION_INDICATOR_HEIGHT;
    var visible_px = window.innerHeight - _PICT_POSITION_INDICATOR_HEIGHT - trackselector_height - _SCROLL_WIDTH
    vScroll.maximum = 128 * org.kbinani.cadencii.AppManager.editorConfig.PxTrackHeight - window.innerHeight + trackselector_height + _SCROLL_WIDTH;
    vScroll.visibleAmount = visible_px;
}

function dragenter( e ){
    dropbox.setAttribute( "dragenter", true );
}

function updateLayout(){
    var width = window.innerWidth;
    var height = window.innerHeight;
    pictPianoRoll.setAttribute( "width", width );
    pictPianoRoll.setAttribute( "height", height - trackselector_height - _MENUBAR_HEIGHT );
    var s = divPictPianoRoll.getAttribute( "style" );
    s = set_style_attribute( s, "width", width + "px" );
    s = set_style_attribute( s, "height", (height - trackselector_height - _MENUBAR_HEIGHT) + "px" );
    s = set_style_attribute( s, "top", _MENUBAR_HEIGHT + "px" );
    divPictPianoRoll.setAttribute( "style", s );

    s = divMenuHolder.getAttribute( "style" );
    s = set_style_attribute( s, "width", width + "px" );
    s = set_style_attribute( s, "height", _MENUBAR_HEIGHT + "px" );
    divMenuHolder.setAttribute( "style", s );

    trackSelector.setAttribute( "width", width );
    trackSelector.setAttribute( "height", trackselector_height );

    var key_width = org.kbinani.cadencii.AppManager.keyWidth;

    trackBar.top = height - trackselector_height - _SCROLL_WIDTH - _MENUBAR_HEIGHT;
    trackBar.left = width - _SCROLL_WIDTH - trackBar.width;

    vScroll.top = _PICT_POSITION_INDICATOR_HEIGHT + _MENUBAR_HEIGHT;
    vScroll.left = width - _SCROLL_WIDTH;
    vScroll.height = height - trackselector_height - _PICT_POSITION_INDICATOR_HEIGHT - _SCROLL_WIDTH;

    hScroll.top = height - trackselector_height - _SCROLL_WIDTH - _MENUBAR_HEIGHT;
    hScroll.left = key_width;
    hScroll.width = width - key_width - trackBar.width - _SCROLL_WIDTH;
}

function init(){
    window.addEventListener( "dragenter", dragenter, true );
    dropbox = document.getElementById( "dropbox" );
    window.addEventListener( "dragleave", dragleave, true );
    dropbox.addEventListener( "dragover", dragover, true );
    dropbox.addEventListener( "drop", drop, true );
    updateDrawObjectList();
    pictPianoRoll = document.getElementById( "pictPianoRoll" );
    trackSelector = document.getElementById( "trackSelector" );
    divPictPianoRoll = document.getElementById( "divPictPianoRoll" );
    divTrackSelector = document.getElementById( "divTrackSelector" );
    divMenuHolder = document.getElementById( "divMenuHolder" );
    pictPianoRoll.addEventListener( "mousemove", pictPianoRoll_mouseMove, true );
    pictPianoRoll.addEventListener( "mousedown", pictPianoRoll_mouseDown, true );
    pictPianoRoll.addEventListener( "mouseup", pictPianoRoll_mouseUp, true );
    window.addEventListener( "resize", formMain_resize, true );
    formMain_resize( null );
    window.addEventListener( "mousemove", window_mouseMove, true );
    window.addEventListener('DOMMouseScroll', window_mouseWheel, false);
    window.onmousewheel = document.onmousewheel = window_mouseWheel;
}

function window_mouseWheel( e ){
    // 1回のデルタが8．一回のデルタにつき_SCROLLSPEEDピクセルスクロールする
    var dx = e.detail / 8 * _SCROLL_SPEED;
    var hscroll_scale = hScroll.box_length / hScroll.visibleAmount;
    var scalex = org.kbinani.cadencii.AppManager.scaleX;
    updateScrollLocation( (hScroll.value + dx / hscroll_scale) * scalex, org.kbinani.cadencii.AppManager.getStartToDrawY() );
    repaintScreen();
}

function window_mouseMove( e ){
    mouseX = e.pageX;
    mouseY = e.pageY - _MENUBAR_HEIGHT;
}

function pictPianoRoll_mouseDown( e ){
    var x = e.pageX;
    var y = e.pageY - _MENUBAR_HEIGHT;
    m_mouse_down_x = x;
    m_mouse_down_y = y;

    // 横スクロールのボックスに入ってないかどうか
    if( hScroll.box_pos <= x && x <= hScroll.box_pos + hScroll.box_length &&
        hScroll.top <= y && y <= hScroll.top + hScroll.height ){
        mouseMode = MouseMode.DOWN_ON_HSCROLL;
        m_hscroll_value = hScroll.value;
        return;
    }else if( vScroll.left <= x && x <= vScroll.left + vScroll.width &&
              vScroll.box_pos <= y && y <= vScroll.box_pos + vScroll.box_length ){
        mouseMode = MouseMode.DOWN_ON_VSCROLL;
        m_vscroll_value = vScroll.value;
        return;
    }else{
        mouseMode = MouseMode.DOWN_ON_PIANOROLL;
        m_stdx = org.kbinani.cadencii.AppManager.getStartToDrawX();
        m_stdy = org.kbinani.cadencii.AppManager.getStartToDrawY();
        return;
    }
}

function pictPianoRoll_mouseUp( e ){
    mouseMode = MouseMode.NONE;
}

function pictPianoRoll_mouseMove( e ){
    var x = e.pageX;
    var y = e.pageY - _MENUBAR_HEIGHT;
    var stdx = org.kbinani.cadencii.AppManager.getStartToDrawX();
    var stdy = org.kbinani.cadencii.AppManager.getStartToDrawY();
    if( mouseMode == MouseMode.DOWN_ON_PIANOROLL ){
        updateScrollLocation( m_stdx - (x - m_mouse_down_x), m_stdy - (y - m_mouse_down_y) );
    }else if( mouseMode == MouseMode.DOWN_ON_HSCROLL ){
        var dx = x - m_mouse_down_x;
        var hscroll_scale = hScroll.box_length / hScroll.visibleAmount;
        var scalex = org.kbinani.cadencii.AppManager.scaleX;
        updateScrollLocation( (m_hscroll_value + dx / hscroll_scale) * scalex, stdy );
    }else if( mouseMode == MouseMode.DOWN_ON_VSCROLL ){
        var dy = y - m_mouse_down_y;
        var vscroll_scale = vScroll.box_length / vScroll.visibleAmount; // pixel/[.valueの単位]
        updateScrollLocation( stdx, m_vscroll_value + dy / vscroll_scale );
    }
    repaintScreen();
}

function formMain_resize( e ){
    updateLayout();
    updateScrollRange();
    repaintScreen();
}

function repaintScreen(){
    var t = org.kbinani.PortUtil.getCurrentTime();
    if( t - _last_paint_time < _MIN_PAINT_INTERVAL ){
        return;
    }
    _last_paint_time = t;
    pictPianoRoll_paint( pictPianoRoll.getContext( "2d" ) );
}

function pictPianoRoll_paint( context ){
    var g = new org.kbinani.java.awt.Graphics( context );
    var c180_180_180 = new org.kbinani.java.awt.Color( 180,180,180);
    var c106_108_108 = new org.kbinani.java.awt.Color( 106, 108, 108 );
    var c212_212_212 = new org.kbinani.java.awt.Color( 212, 212, 212 );
    var c240_240_240 = new org.kbinani.java.awt.Color( 240, 240, 240 );
    var c153_153_153 = new org.kbinani.java.awt.Color( 153, 153, 153 );
    var c112_112_112 = new org.kbinani.java.awt.Color( 112, 112, 112 );
    var c212_212_212 = new org.kbinani.java.awt.Color( 212, 212, 212 );
    var c125_123_124 = new org.kbinani.java.awt.Color( 125, 123, 124 );
    var c072_077_098 = new org.kbinani.java.awt.Color( 72, 77, 98 );
    var c160_160_160 = new org.kbinani.java.awt.Color( 160, 160, 160 );
    var c105_105_105 = new org.kbinani.java.awt.Color( 105, 105, 105 );
    var s_note_fill = new org.kbinani.java.awt.Color( 181, 220, 86 );
    var c144_144_144 = new org.kbinani.java.awt.Color( 144, 144, 144 );
    var c194_194_194 = new org.kbinani.java.awt.Color( 194, 194, 194 );


    /*PolylineDrawer commonDrawer = new PolylineDrawer( g, 1024 );*/
    var vsq = org.kbinani.cadencii.AppManager.getVsqFile();
    var selected = org.kbinani.cadencii.AppManager.getSelected();
    var vsq_track = vsq.Track[selected];

    var width = pictPianoRoll.getAttribute( "width" );
    var height = pictPianoRoll.getAttribute( "height" );
    var window_size = new org.kbinani.java.awt.Dimension( width, height );
    var mouse_position = new org.kbinani.java.awt.Point( mouseX, mouseY );
    var stdy = org.kbinani.cadencii.AppManager.getStartToDrawY();
    var stdx = org.kbinani.cadencii.AppManager.getStartToDrawX();
    var key_width = org.kbinani.cadencii.AppManager.keyWidth;
    var track_height = org.kbinani.cadencii.AppManager.editorConfig.PxTrackHeight;
    var half_track_height = track_height / 2;
    context.font = track_height + "px 'メイリオ'";
    /*
    // [screen_x] = 67 + [clock] * ScaleX - StartToDrawX + 6
    // [screen_y] = -1 * ([note] - 127) * TRACK_HEIGHT - StartToDrawY
    //
    // [screen_x] = [clock] * _scalex + 73 - StartToDrawX
    // [screen_y] = -[note] * TRACK_HEIGHT + 127 * TRACK_HEIGHT - StartToDrawY*/
    var xoffset = org.kbinani.cadencii.AppManager.keyOffset + key_width - stdx;
    var yoffset = 127 * track_height - stdy;
    //      ↓
    // [screen_x] = [clock] * _scalex + xoffset
    // [screen_y] = -[note] * TRACK_HEIGHT + yoffset
    var y, dy;
    var scalex = org.kbinani.cadencii.AppManager.scaleX;
    var inv_scalex = 1.0 / scalex;

    /*if ( AppManager.getSelectedEventCount() > 0 && AppManager.inputTextBox.isVisible() ) {
        VsqEvent original = AppManager.getLastSelectedEvent().original;
        int event_x = (int)(original.Clock * scalex + xoffset);
        int event_y = -original.ID.Note * track_height + yoffset;
#if JAVA
        AppManager.inputTextBox.setLocation( pointToScreen( new Point( event_x + 4, event_y + 2 ) ) );
#else
        AppManager.inputTextBox.Left = event_x + 4;
        AppManager.inputTextBox.Top = event_y + 2;
#endif
    }*/

    var black = new org.kbinani.java.awt.Color( 212, 212, 212 ); //AppManager.editorConfig.PianorollColorVocalo2Black.getColor();
    var white = new org.kbinani.java.awt.Color( 240, 240, 240 ); //AppManager.editorConfig.PianorollColorVocalo2White.getColor();
    var bar = org.kbinani.cadencii.AppManager.editorConfig.PianorollColorVocalo2Bar.getColor();
    var beat = org.kbinani.cadencii.AppManager.editorConfig.PianorollColorVocalo2Beat.getColor();
    /*RendererKind renderer = RendererKind.VOCALOID2;

    EditMode edit_mode = AppManager.getEditMode();

    if ( vsq != null ) {
        renderer = VsqFileEx.getTrackRendererKind( vsq_track );
    }

    if ( renderer == RendererKind.UTAU ) {
        black = AppManager.editorConfig.PianorollColorUtauBlack.getColor();
        white = AppManager.editorConfig.PianorollColorUtauWhite.getColor();
        bar = AppManager.editorConfig.PianorollColorUtauBar.getColor();
        beat = AppManager.editorConfig.PianorollColorUtauBeat.getColor();
    } else if ( renderer == RendererKind.VOCALOID1_100 || renderer == RendererKind.VOCALOID1_101 ) {
        black = AppManager.editorConfig.PianorollColorVocalo1Black.getColor();
        white = AppManager.editorConfig.PianorollColorVocalo1White.getColor();
        bar = AppManager.editorConfig.PianorollColorVocalo1Bar.getColor();
        beat = AppManager.editorConfig.PianorollColorVocalo1Beat.getColor();
    } else if ( renderer == RendererKind.STRAIGHT_UTAU ) {
        black = AppManager.editorConfig.PianorollColorStraightBlack.getColor();
        white = AppManager.editorConfig.PianorollColorStraightWhite.getColor();
        bar = AppManager.editorConfig.PianorollColorStraightBar.getColor();
        beat = AppManager.editorConfig.PianorollColorStraightBeat.getColor();
    } else if ( renderer == RendererKind.AQUES_TONE ) {
        black = AppManager.editorConfig.PianorollColorAquesToneBlack.getColor();
        white = AppManager.editorConfig.PianorollColorAquesToneWhite.getColor();
        bar = AppManager.editorConfig.PianorollColorAquesToneBar.getColor();
        beat = AppManager.editorConfig.PianorollColorAquesToneBeat.getColor();
    }*/

    // ピアノロール周りのスクロールバーなど
    // スクロール画面背景
    if ( height > 0 ) {
        g.setColor( org.kbinani.java.awt.Color.white );
        g.fillRect( 3, 0, width, height );
    }
    // ピアノロールとカーブエディタの境界
    g.setColor( c112_112_112 );
    g.drawLine( 2, height - 1, width - 1, height - 1 );

    // ピアノロール本体
    if ( vsq != null ) {
        var odd = -1;
        y = 128 * track_height - stdy;
        dy = -track_height;
        for ( var i = 0; i <= 127; i++ ) {
            odd++;
            if ( odd == 12 ) {
                odd = 0;
            }
            var order = (i - odd) / 12 - 2;
            y += dy;
            if ( y > height ) {
                continue;
            } else if ( 0 > y + track_height ) {
                break;
            }
            var note_is_whitekey = org.kbinani.vsq.VsqNote.isNoteWhiteKey( i );

            // ピアノロール背景
            var b = new org.kbinani.java.awt.Color( 0, 0, 0 );
            var border;
            var paint_required = true;
            if ( order == -2 || order == -1 || (6 <= order && order <= 8) ) {
                if ( note_is_whitekey ) {
                    b = c180_180_180;
                } else {
                    b = c106_108_108;
                }
                border = c106_108_108;
            } else if ( order == 5 || order == 0 ) {
                if ( note_is_whitekey ) {
                    b = c212_212_212;
                } else {
                    b = c180_180_180;
                }
                border = new org.kbinani.java.awt.Color( 150, 152, 150 );
            } else {
                if ( note_is_whitekey ) {
                    //paint_required = false;
                    b = white;// c240_240_240;
                } else {
                    b = black;// c212_212_212;
                }
                border = new org.kbinani.java.awt.Color( 210, 205, 172 );
            }
            if ( paint_required ) {
                g.setColor( b );
                g.fillRect( key_width, y, width - key_width, track_height + 1 );
            }
            if ( odd == 0 || odd == 5 ) {
                g.setColor( border );
                g.drawLine( key_width, y + track_height, width, y + track_height );
            }

            // プリメジャー部分のピアノロール背景
            var premeasure_start_x = xoffset;
            if ( premeasure_start_x < key_width ) {
                premeasure_start_x = key_width;
            }
            var premeasure_end_x = org.kbinani.PortUtil.castToInt( vsq.getPreMeasureClocks() * scalex + xoffset );
            if ( premeasure_end_x >= key_width ) {
                if ( note_is_whitekey ) {
                    g.setColor( c153_153_153 );
                    g.fillRect( premeasure_start_x, y,
                                premeasure_end_x - premeasure_start_x, track_height + 1 );
                } else {
                    g.setColor( c106_108_108 );
                    g.fillRect( premeasure_start_x, y,
                                premeasure_end_x - premeasure_start_x, track_height + 1 );
                }
                if ( odd == 0 || odd == 5 ) {
                    g.setColor( c106_108_108 );
                    g.drawLine( premeasure_start_x, y + track_height, premeasure_end_x, y + track_height );
                }
            }

        }
    }

    //g.clipRect( key_width, 0, width - key_width, height );
    // 小節ごとの線
    var barline_iterator = null;
    if ( vsq != null ) {
        var dashed_line_step = org.kbinani.cadencii.AppManager.getPositionQuantizeClock();
        var endclock = (width - xoffset) * inv_scalex;
        barline_iterator = vsq.getBarLineIterator( endclock );
        for ( ; barline_iterator.hasNext(); ) {
            var blt = barline_iterator.next();
            var local_clock_step = 1920 / blt.getLocalDenominator();
            var x = org.kbinani.PortUtil.castToInt( blt.clock() * scalex + xoffset );
            //g.setStroke( getStrokeDefault() );
            if ( blt.isSeparator() ) {
                //ピアノロール上
                g.setColor( bar );
                g.drawLine( x, 0, x, height );
            } else {
                //ピアノロール上
                g.setColor( beat );
                g.drawLine( x, 0, x, height );
            }
            if ( dashed_line_step > 1 && org.kbinani.cadencii.AppManager.isGridVisible() ) {
                var numDashedLine = org.kbinani.PortUtil.castToInt( local_clock_step / dashed_line_step );
                g.setColor( beat );
                //g.setStroke( getStrokeDashed() );
                for ( var i = 1; i < numDashedLine; i++ ) {
                    var x2 = org.kbinani.PortUtil.castToInt( (blt.clock() + i * dashed_line_step) * scalex + xoffset );
                    g.drawLine( x2, 0, x2, height );
                }
                //g.setStroke( getStrokeDefault() );
            }
        }
    }

    /*
    // 現在選択されている歌声合成システムの名前をオーバーレイ表示する
    if ( AppManager.drawOverSynthNameOnPianoroll ) {
        g.setFont( AppManager.baseFont50Bold );
        g.setColor( new Color( 0, 0, 0, 32 ) );
        String str = "VOCALOID2";
        if ( renderer == RendererKind.AQUES_TONE ) {
            str = "AquesTone";
        } else if ( renderer == RendererKind.VOCALOID1_100 ) {
            str = "VOCALOID1 [1.0]";
        } else if ( renderer == RendererKind.VOCALOID1_101 ) {
            str = "VOCALOID1 [1.1]";
        } else if ( renderer == RendererKind.STRAIGHT_UTAU ) {
            str = "STRAIGHT X UTAU";
        } else if ( renderer == RendererKind.UTAU ) {
            str = "UTAU";
        }
        g.drawString( str, key_width + 10, 10 + AppManager.baseFont50Height / 2 - AppManager.baseFont50OffsetHeight + 1 );
    }*/

    // トラックのエントリを描画
    if ( org.kbinani.cadencii.AppManager.drawObjects != null ) {
        /*if ( org.kbinani.cadencii.AppManager.isOverlay() ) {
            // まず、選択されていないトラックの簡易表示を行う
            var c = AppManager.drawObjects.size();
            for ( var i = 0; i < c; i++ ) {
                if ( i == selected - 1 ) {
                    continue;
                }
                var target_list = org.kbinani.cadencii.AppManager.drawObjects[i];
                var j_start = org.kbinani.cadencii.AppManager.drawStartIndex[i];
                var first = true;
                var shift_center = half_track_height;
                var target_list_count = target_list.size();
                for ( var j = j_start; j < target_list_count; j++ ) {
                    var dobj = target_list[j];
                    if ( dobj.type != org.kbinani.cadencii.DrawObjectType.Note ) {
                        continue;
                    }
                    var x = dobj.pxRectangle.x + key_width - stdx;
                    y = dobj.pxRectangle.y - stdy;
                    var lyric_width = dobj.pxRectangle.width;
                    if ( x + lyric_width < 0 ) {
                        continue;
                    } else if ( width < x ) {
                        break;
                    }
                    if ( org.kbinani.cadencii.AppManager.isPlaying() && first ) {
                        org.kbinani.cadencii.AppManager.drawStartIndex[i] = j;
                        first = false;
                    }
                    if ( y + track_height < 0 || y > height ) {
                        continue;
                    }
                    g.setColor( org.kbinani.cadencii.AppManager.HILIGHT[i] );
                    g.drawLine( x + 1, y + shift_center,
                                x + lyric_width - 1, y + shift_center );
                    g.setColor( s_HIDDEN[i] );
                    g.drawPolyline( new int[] { x, x + 1, x + lyric_width - 1, x + lyric_width, x + lyric_width - 1, x + 1, x },
                                    new int[] { y + shift_center, y + shift_center - 1, y + shift_center - 1, y + shift_center, y + shift_center + 1, y + shift_center + 1, y + shift_center },
                                    7 );
                }
            }
        }*/

        // 選択されているトラックの表示を行う
        var show_lyrics = org.kbinani.cadencii.AppManager.editorConfig.ShowLyric;
        var show_exp_line = org.kbinani.cadencii.AppManager.editorConfig.ShowExpLine;
        if ( selected >= 1 ) {
            /*Shape r = g.getClip();
            g.clipRect( key_width, 0,
                        width - key_width, height );*/
            var j_start = 0;/* org.kbinani.cadencii.AppManager.drawStartIndex[selected - 1];*/

            var first = true;
            //lock ( AppManager.drawObjects ) { //ここでロックを取得しないと、描画中にUpdateDrawObjectのサイズが0になる可能性がある
            if ( selected - 1 < org.kbinani.cadencii.AppManager.drawObjects.length ) {
                var target_list = org.kbinani.cadencii.AppManager.drawObjects[selected - 1];
                var pit = vsq_track.MetaText.PIT;
                var pbs = vsq_track.MetaText.PBS;

                var c = target_list.length;
                for ( var j = j_start; j < c; j++ ) {
                    var dobj = target_list[j];
                    var x = dobj.pxRectangle.x + key_width - stdx;
                    y = dobj.pxRectangle.y - stdy;
                    var lyric_width = dobj.pxRectangle.width;
                    if ( x + lyric_width < 0 ) {
                        continue;
                    } else if ( width < x ) {
                        break;
                    }
                    if ( org.kbinani.cadencii.AppManager.isPlaying() && first ) {
                        org.kbinani.cadencii.AppManager.drawStartIndex[selected - 1] = j;
                        first = false;
                    }
                    if ( y + 2 * track_height < 0 || y > height ) {
                        continue;
                    }

                    if ( dobj.type == org.kbinani.cadencii.DrawObjectType.Note ) {
                        // Note
                        var id_fill;
                        if ( org.kbinani.cadencii.AppManager.getSelectedEventCount() > 0 ) {
                            var found = org.kbinani.cadencii.AppManager.isSelectedEventContains( selected, dobj.internalID );
                            if ( found ) {
                                id_fill = Aorg.kbinani.cadencii.AppManager.getHilightColor();
                            } else {
                                id_fill = s_note_fill;
                            }
                        } else {
                            id_fill = s_note_fill;
                        }
                        g.setColor( id_fill );
                        g.fillRect( x, y + 1, lyric_width, track_height - 1 );
                        /*var lyric_font = dobj.symbolProtected ? AppManager.baseFont10Bold : AppManager.baseFont10;*/
                        if ( dobj.overlappe ) {
                            g.setColor( c125_123_124 );
                            g.drawRect( x, y + 1, lyric_width, track_height - 1 );
                            if ( show_lyrics ) {
                                //g.setFont( lyric_font );
                                g.setColor( s_brs_147_147_147 );
                                g.drawString( dobj.text, x + 1, y + track_height + 1 );
                            }
                        } else {
                            g.setColor( c125_123_124 );
                            g.drawRect( x, y + 1, lyric_width, track_height - 1 );
                            if ( show_lyrics ) {
                                //g.setFont( lyric_font );
                                g.setColor( org.kbinani.java.awt.Color.black );
                                g.drawString( dobj.text, x + 1, y + track_height - 2 );
                            }
                            if ( show_exp_line && lyric_width > 21 ) {
                                // 表情線
                                /*DrawAccentLine( g, new Point( x, y + track_height + 1 ), dobj.accent );
                                int vibrato_start = x + lyric_width;
                                int vibrato_end = x;
                                if ( dobj.pxVibratoDelay <= lyric_width ) {
                                    int vibrato_delay = dobj.pxVibratoDelay;
                                    int vibrato_width = dobj.pxRectangle.width - vibrato_delay;
                                    vibrato_start = x + vibrato_delay;
                                    vibrato_end = x + vibrato_delay + vibrato_width;
                                    if ( vibrato_start - x < 21 ) {
                                        vibrato_start = x + 21;
                                    }
                                }
                                g.setColor( s_pen_051_051_000 );
                                g.drawLine( x + 21, y + track_height + 7,
                                            vibrato_start, y + track_height + 7 );
                                if ( dobj.pxVibratoDelay <= lyric_width ) {
                                    int next_draw = vibrato_start;
                                    if ( vibrato_start < vibrato_end ) {
                                        drawVibratoLine( g,
                                                         new Point( vibrato_start, y + track_height + 1 ),
                                                         vibrato_end - vibrato_start );
                                    }
                                }*/
                            }
                            // ビブラートがあれば
                            if ( org.kbinani.cadencii.AppManager.editorConfig.ViewAtcualPitch ) {
                                /*if ( dobj.vibRate != null ) {
                                    int vibrato_delay = dobj.pxVibratoDelay;
                                    int vibrato_width = dobj.pxRectangle.width - vibrato_delay;
                                    int vibrato_start = x + vibrato_delay;
                                    int vibrato_end = x + vibrato_delay + vibrato_width;
                                    int cl_sx = AppManager.clockFromXCoord( vibrato_start );
                                    int cl_ex = AppManager.clockFromXCoord( vibrato_end );
                                    drawVibratoPitchbend( g,
                                                          dobj.vibRate,
                                                          dobj.vibStartRate,
                                                          dobj.vibDepth,
                                                          dobj.vibStartDepth,
                                                          dobj.note,
                                                          vibrato_start,
                                                          vibrato_width );
                                }*/
                            }

                            // ピッチベンド
                            /*if ( AppManager.editorConfig.ViewAtcualPitch || AppManager.curveOnPianoroll ) {
                                int cl_start = dobj.clock;
                                int cl_end = cl_start + dobj.length;

                                commonDrawer.clear();
                                g.setColor( Color.blue );
                                g.setStroke( getStroke2px() );
                                // この音符の範囲についてのみ，ピッチベンド曲線を描く
                                int lasty = int.MinValue;
                                ByRef<Integer> indx_pit = new ByRef<Integer>( 0 );
                                ByRef<Integer> indx_pbs = new ByRef<Integer>( 0 );
                                for ( int cl = cl_start; cl < cl_end; cl++ ) {
                                    int vpit = pit.getValue( cl, indx_pit );
                                    int vpbs = pbs.getValue( cl, indx_pbs );

                                    float delta = vpit * (float)vpbs / 8192.0f;
                                    float note = dobj.note + delta;

                                    int py = AppManager.yCoordFromNote( note ) + half_track_height;
                                    if ( cl + 1 == cl_end ) {
                                        int px = AppManager.xCoordFromClocks( cl + 1 );
                                        commonDrawer.append( px, lasty );
                                    } else {
                                        if ( py == lasty ) {
                                            continue;
                                        }
                                        int px = AppManager.xCoordFromClocks( cl );
                                        if ( cl != cl_start ) {
                                            commonDrawer.append( px, lasty );
                                        }
                                        commonDrawer.append( px, py );
                                        lasty = py;
                                    }
                                }
                                commonDrawer.flush();
#if !JAVA
                                g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
#endif
                                g.setStroke( getStrokeDefault() );
                            }*/
                        }
                    } else if ( dobj.type == org.kbinani.cadencii.DrawObjectType.Dynaff ) {
                        // Dynaff
                        var fill = s_dynaff_fill;
                        if ( org.kbinani.cadencii.AppManager.isSelectedEventContains( selected, dobj.internalID ) ) {
                            fill = s_dynaff_fill_highlight;
                        }
                        g.setColor( fill );
                        g.fillRect( x, y, 40, track_height );
                        g.setColor( c125_123_124 );
                        g.drawRect( x, y, 40, track_height );
                        g.setColor( Color.black );
                        g.setFont( AppManager.baseFont10 );
                        if ( dobj.overlappe ) {
                            g.setColor( s_brs_147_147_147 );
                        }
                        var str = dobj.text;
                        /*g.drawString( str, x + 1, y + half_track_height - AppManager.baseFont10OffsetHeight + 1 );*/
                    } else {
                        // Crescend and Descrescend
                        var xend = x + lyric_width;
                        var fill = s_dynaff_fill;
                        if ( AppManager.isSelectedEventContains( selected, dobj.internalID ) ) {
                            fill = s_dynaff_fill_highlight;
                        }
                        g.setColor( fill );
                        g.fillRect( x, y, xend - x, track_height );
                        g.setColor( c125_123_124 );
                        g.drawRect( x, y, xend - x, track_height );
                        if ( dobj.overlappe ) {
                            g.setColor( s_brs_147_147_147 );
                        } else {
                            g.setColor( Color.black );
                        }
                        g.setFont( AppManager.baseFont10 );
                        var str = dobj.text;
                        /*g.drawString( str, x + 1, y + track_height + half_track_height - AppManager.baseFont10OffsetHeight + 1 );*/
                        if ( dobj.type == DrawObjectType.Crescend ) {
                            g.drawLine( xend - 2, y + 4, x + 3, y + half_track_height );
                            g.drawLine( x + 3, y + half_track_height, xend - 2, y + track_height - 3 );
                        } else if ( dobj.type == DrawObjectType.Decrescend ) {
                            g.drawLine( x + 3, y + 4, xend - 2, y + half_track_height );
                            g.drawLine( xend - 2, y + half_track_height, x + 3, y + track_height - 3 );
                        }
                    }
                }
            }
            /*g.setClip( r );*/
        }

        // 編集中のエントリを表示
        /*if ( edit_mode == EditMode.ADD_ENTRY ||
             edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY ||
             edit_mode == EditMode.REALTIME ||
             edit_mode == EditMode.DRAG_DROP ) {
            if ( AppManager.addingEvent != null ) {
                int x = (int)(AppManager.addingEvent.Clock * scalex + xoffset);
                y = -AppManager.addingEvent.ID.Note * track_height + yoffset + 1;
                int length = (int)(AppManager.addingEvent.ID.getLength() * scalex);
                if ( AppManager.addingEvent.ID.type == VsqIDType.Aicon ) {
                    if ( AppManager.addingEvent.ID.IconDynamicsHandle.isDynaffType() ) {
                        length = AppManager.DYNAFF_ITEM_WIDTH;
                    }
                }
                if ( AppManager.addingEvent.ID.getLength() <= 0 ) {
                    g.setColor( new Color( 171, 171, 171 ) );
                    g.drawRect( x, y, 10, track_height - 1 );
                } else {
                    g.setColor( s_pen_a136_000_000_000 );
                    g.drawRect( x, y, length, track_height - 1 );
                }
            }
        } else if ( edit_mode == EditMode.EDIT_VIBRATO_DELAY ) {
            int x = (int)(AppManager.addingEvent.Clock * scalex + xoffset);
            y = -AppManager.addingEvent.ID.Note * track_height + yoffset + 1;
            int length = (int)(AppManager.addingEvent.ID.getLength() * scalex);
            g.setColor( s_pen_a136_000_000_000 );
            g.drawRect( x, y, length, track_height - 1 );
        } else if ( (edit_mode == EditMode.MOVE_ENTRY ||
                     edit_mode == EditMode.MOVE_ENTRY_WHOLE ||
                     edit_mode == EditMode.EDIT_LEFT_EDGE ||
                     edit_mode == EditMode.EDIT_RIGHT_EDGE) && AppManager.getSelectedEventCount() > 0 ) {
            for ( Iterator<SelectedEventEntry> itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                SelectedEventEntry ev = itr.next();
                int x = (int)(ev.editing.Clock * scalex + xoffset);
                y = -ev.editing.ID.Note * track_height + yoffset + 1;
                if ( ev.editing.ID.type == VsqIDType.Aicon ) {
                    if ( ev.editing.ID.IconDynamicsHandle == null ) {
                        continue;
                    }
                    int length = 0;
                    if ( ev.editing.ID.IconDynamicsHandle.isDynaffType() ) {
                        length = AppManager.DYNAFF_ITEM_WIDTH;
                    } else {
                        length = (int)(ev.editing.ID.getLength() * scalex);
                    }
                    g.setColor( s_pen_a136_000_000_000 );
                    g.drawRect( x, y, length, track_height - 1 );
                } else {
                    if ( ev.editing.ID.getLength() == 0 ) {
                        g.setColor( new Color( 171, 171, 171 ) );
                        g.setStroke( s_pen_dashed_171_171_171 );
                        g.drawRect( x, y, 10, track_height - 1 );
                        g.setStroke( getStrokeDefault() );
                    } else {
                        int length = (int)(ev.editing.ID.getLength() * scalex);
                        g.setColor( s_pen_a136_000_000_000 );
                        g.drawRect( x, y, length, track_height - 1 );
                    }
                }
            }

            if ( edit_mode == EditMode.MOVE_ENTRY_WHOLE ) {
                int clock_start = AppManager.wholeSelectedInterval.getStart();
                int clock_end = AppManager.wholeSelectedInterval.getEnd();
                int x_start = AppManager.xCoordFromClocks( AppManager.wholeSelectedIntervalStartForMoving );
                int x_end = AppManager.xCoordFromClocks( AppManager.wholeSelectedIntervalStartForMoving + (clock_end - clock_start) );
                g.setColor( s_brs_a098_000_000_000 );
                g.drawLine( x_start, 0, x_start, height );
                g.drawLine( x_end, 0, x_end, height );
            }
        }*/
    }
    /*g.setClip( null );*/

    //ピアノロールと鍵盤部分の縦線
    var hilighted_note = -1;
    g.setColor( c240_240_240 );
    g.fillRect( 0, 0, key_width, height );
    g.setColor( c212_212_212 );
    g.drawLine( key_width, 0, key_width, height );
    var odd2 = -1;
    y = 128 * track_height - stdy;
    dy = -track_height;
    for ( var i = 0; i <= 127; i++ ) {
        odd2++;
        if ( odd2 == 12 ) {
            odd2 = 0;
        }
        y += dy;
        if ( y > height ) {
            continue;
        } else if ( y + track_height < 0 ) {
            break;
        }

        // 鍵盤部分
        g.setColor( c212_212_212 );
        g.drawLine( 3, y, key_width, y );
        var hilighted = false;
        /*if ( edit_mode == EditMode.ADD_ENTRY ) {
            if ( AppManager.addingEvent.ID.Note == i ) {
                hilighted = true;
                hilighted_note = i;
            }
        } else if ( edit_mode == EditMode.EDIT_LEFT_EDGE || edit_mode == EditMode.EDIT_RIGHT_EDGE ) {
#if DEBUG
            //org.kbinani.debug.push_log( "(AppManager.LastSelectedEvent==null)=" + (AppManager.LastSelectedEvent == null) );
            //org.kbinani.debug.push_log( "(AppManager.LastSelectedEvent.Original==null)=" + (AppManager.LastSelectedEvent.Original == null) );
#endif
            if ( AppManager.getLastSelectedEvent().original.ID.Note == i ) { //TODO: ここでNullpointer exception
                hilighted = true;
                hilighted_note = i;
            }
        } else {*/
            if ( 3 <= mouse_position.x && mouse_position.x <= width - 17 &&
                0 <= mouse_position.y && mouse_position.y <= height - 1 ) {
                if ( y <= mouse_position.y && mouse_position.y < y + track_height ) {
                    hilighted = true;
                    hilighted_note = i;
                }
            }
        /*}*/
        if ( hilighted ) {
            g.setColor( org.kbinani.cadencii.AppManager.getHilightColor() );
            g.fillRect( 35, y, key_width - 35, track_height );
        }
        if ( odd2 == 0 || hilighted ) {
            g.setColor( c072_077_098 );
            //g.setFont( AppManager.baseFont8 );
            g.drawString( org.kbinani.vsq.VsqNote.getNoteString( i ), 42, y + track_height - 2 );
        }
        if ( !org.kbinani.vsq.VsqNote.isNoteWhiteKey( i ) ) {
            g.setColor( c125_123_124 );
            g.fillRect( 0, y, 34, track_height );
        }
    }
    //g.setClip( null );

    // 音符編集時の補助線
    /*if ( edit_mode == EditMode.ADD_ENTRY ) {
        #region EditMode.AddEntry
        int x = (int)(AppManager.addingEvent.Clock * scalex + xoffset);
        y = -AppManager.addingEvent.ID.Note * track_height + yoffset + 1;
        int length;
        if ( AppManager.addingEvent.ID.getLength() == 0 ) {
            length = 10;
        } else {
            length = (int)(AppManager.addingEvent.ID.getLength() * scalex);
        }
        x += length;
        g.setColor( s_pen_LU );
        g.drawLine( x, 0, x, y - 1 );
        g.drawLine( x, y + track_height, x, height );
        g.setColor( s_pen_RD );
        g.drawLine( x + 1, 0, x + 1, y - 1 );
        g.drawLine( x + 1, y + track_height, x + 1, height );
        #endregion
    } else if ( edit_mode == EditMode.MOVE_ENTRY || edit_mode == EditMode.MOVE_ENTRY_WAIT_MOVE ) {
        #region EditMode.MoveEntry || EditMode.MoveEntryWaitMove
        if ( AppManager.getSelectedEventCount() > 0 ) {
            VsqEvent last = AppManager.getLastSelectedEvent().editing;
            int x = (int)(last.Clock * scalex + xoffset);
            y = -last.ID.Note * track_height + yoffset + 1;
            int length = (int)(last.ID.getLength() * scalex);

            if ( last.ID.type == VsqIDType.Aicon ) {
                if ( last.ID.IconDynamicsHandle.isDynaffType() ) {
                    length = AppManager.DYNAFF_ITEM_WIDTH;
                }
            }

            // 縦線
            g.setColor( s_pen_LU );
            g.drawLine( x, 0, x, y - 1 );
            g.drawLine( x, y + track_height, x, height );
            // 横線
            g.drawLine( key_width, y + half_track_height - 2,
                        x - 1, y + half_track_height - 2 );
            g.drawLine( x + length + 1, y + half_track_height - 2,
                        width, y + half_track_height - 2 );
            // 縦線
            g.setColor( s_pen_RD );
            g.drawLine( x + 1, 0, x + 1, y - 1 );
            g.drawLine( x + 1, y + track_height,
                        x + 1, height );
            // 横線
            g.drawLine( key_width, y + half_track_height - 1,
                        x - 1, y + half_track_height - 1 );
            g.drawLine( x + length + 1, y + half_track_height - 1,
                        width, y + half_track_height - 1 );
        }
        #endregion
    } else if ( edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY || edit_mode == EditMode.DRAG_DROP ) {
        #region ADD_FIXED_LENGTH_ENTRY | DRAG_DROP
        int x = (int)(AppManager.addingEvent.Clock * scalex + xoffset);
        y = -AppManager.addingEvent.ID.Note * track_height + yoffset + 1;
        int length = (int)(AppManager.addingEvent.ID.getLength() * scalex);

        if ( AppManager.addingEvent.ID.type == VsqIDType.Aicon ) {
            if ( AppManager.addingEvent.ID.IconDynamicsHandle.isDynaffType() ) {
                length = AppManager.DYNAFF_ITEM_WIDTH;
            }
        }

        // 縦線
        g.setColor( s_pen_LU );
        g.drawLine( x, 0, x, y - 1 );
        g.drawLine( x, y + track_height, x, height );
        // 横線
        g.drawLine( key_width, y + half_track_height - 2,
                    x - 1, y + half_track_height - 2 );
        g.drawLine( x + length + 1, y + half_track_height - 2,
                    width, y + half_track_height - 2 );
        // 縦線
        g.setColor( s_pen_RD );
        g.drawLine( x + 1, 0, x + 1, y - 1 );
        g.drawLine( x + 1, y + track_height, x + 1, height );
        // 横線
        g.drawLine( key_width, y + half_track_height - 1,
                    x - 1, y + half_track_height - 1 );
        g.drawLine( x + length + 1, y + half_track_height - 1,
                    width, y + half_track_height - 1 );
        #endregion
    } else if ( edit_mode == EditMode.EDIT_LEFT_EDGE ) {
        #region EditMode.EditLeftEdge
        VsqEvent last = AppManager.getLastSelectedEvent().editing;
        int x = (int)(last.Clock * scalex + xoffset);
        y = -last.ID.Note * track_height + yoffset + 1;
        g.setColor( s_pen_LU );
        g.drawLine( x, 0, x, y - 1 );
        g.drawLine( x, y + track_height, x, height );
        g.setColor( s_pen_RD );
        g.drawLine( x + 1, 0, x + 1, y - 1 );
        g.drawLine( x + 1, y + track_height, x + 1, height );
        #endregion
    } else if ( edit_mode == EditMode.EDIT_RIGHT_EDGE ) {
        #region EditMode.EditRightEdge
        VsqEvent last = AppManager.getLastSelectedEvent().editing;
        int x = (int)(last.Clock * scalex + xoffset);
        y = -last.ID.Note * track_height + yoffset + 1;
        int length = (int)(last.ID.getLength() * scalex);
        x += length;
        g.setColor( s_pen_LU );
        g.drawLine( x, 0, x, y - 1 );
        g.drawLine( x, y + track_height, x, height );
        g.setColor( s_pen_RD );
        g.drawLine( x + 1, 0, x + 1, y - 1 );
        g.drawLine( x + 1, y + track_height, x + 1, height );
        #endregion
    } else if ( edit_mode == EditMode.EDIT_VIBRATO_DELAY ) {
        #region EditVibratoDelay
        int x = (int)(AppManager.addingEvent.Clock * scalex + xoffset);
        y = -AppManager.addingEvent.ID.Note * track_height + yoffset + 1;
        g.setColor( s_pen_LU );
        g.drawLine( x, 0, x, y - 1 );
        g.drawLine( x, y + track_height, x, height );
        g.setColor( s_pen_RD );
        g.drawLine( x + 1, 0, x + 1, y - 1 );
        g.drawLine( x + 1, y + track_height, x + 1, height );
        double max_length = AppManager.addingEventLength - _PX_ACCENT_HEADER / scalex;
        double drate = AppManager.addingEvent.ID.getLength() / max_length;
        if ( drate > 0.99 ) {
            drate = 1.00;
        }
        int rate = (int)(drate * 100.0);
        String percent = rate + "%";
        Dimension size = Util.measureString( percent, s_F9PT );
        int delay_x = (int)((AppManager.addingEvent.Clock + AppManager.addingEvent.ID.getLength() - AppManager.addingEventLength + AppManager.addingEvent.ID.VibratoDelay) * scalex + xoffset);
        Rectangle pxArea = new Rectangle( delay_x,
                                          (int)(y + track_height * 2.5),
                                          (int)(size.width * 1.2),
                                          (int)(size.height * 1.2) );
        g.setColor( s_brs_192_192_192 );
        g.fillRect( pxArea.x, pxArea.y, pxArea.width, pxArea.height );
        g.setColor( Color.black );
        g.drawRect( pxArea.x, pxArea.y, pxArea.width, pxArea.height );
        // StringFormat sf = new StringFormat();
        //sf.Alignment = StringAlignment.Center;
        //sf.LineAlignment = StringAlignment.Center;
        g.setFont( s_F9PT );
        g.drawString( percent, pxArea.x, pxArea.y );// , sf );
        #endregion
    }*/

    // マウス位置での音階名
    /*if ( hilighted_note >= 0 ) {
        int align = 1;
        int valign = 0;
        g.setColor( Color.black );
        PortUtil.drawStringEx( g,
                               VsqNote.getNoteString( hilighted_note ),
                               AppManager.baseFont10Bold,
                               new Rectangle( mouse_position.x - 110, mouse_position.y - 50, 100, 100 ),
                               align,
                               valign );
    }*/

    // 外枠
    // 左(外側)
    g.setColor( c160_160_160 );
    g.drawLine( 0, 0, 0, height );
    // 左(内側)
    g.setColor( c105_105_105 );
    g.drawLine( 1, 0, 1, height );

    // pictPianoRoll_Paintより
    /*if ( AppManager.isWholeSelectedIntervalEnabled() ) {
        int start = (int)(AppManager.wholeSelectedInterval.getStart() * scalex) + xoffset;
        if ( start < key_width ) {
            start = key_width;
        }
        int end = (int)(AppManager.wholeSelectedInterval.getEnd() * scalex) + xoffset;
        if ( start < end ) {
            g.setColor( new Color( 0, 0, 0, 98 ) );
            g.fillRect( start, 0, end - start, getHeight() );
        }
    } else if ( AppManager.isPointerDowned ) {
        Point pmouse = pointToClient( PortUtil.getMousePosition() );
        Point mouse = new Point( pmouse.x, pmouse.y );
        int tx, ty, twidth, theight;
        int lx = AppManager.mouseDownLocation.x - stdx;
        if ( lx < mouse.x ) {
            tx = lx;
            twidth = mouse.x - lx;
        } else {
            tx = mouse.x;
            twidth = lx - mouse.x;
        }
        int ly = AppManager.mouseDownLocation.y - stdy;
        if ( ly < mouse.y ) {
            ty = ly;
            theight = mouse.y - ly;
        } else {
            ty = mouse.y;
            theight = ly - mouse.y;
        }
        if ( tx < key_width ) {
            int txold = tx;
            tx = key_width;
            twidth -= (tx - txold);
        }
        Rectangle rc = new Rectangle( tx, ty, twidth, theight );
        Color pen = new Color( 0, 0, 0, 200 );
        g.setColor( new Color( 0, 0, 0, 100 ) );
        g.fillRect( rc.x, rc.y, rc.width, rc.height );
#if !JAVA
        g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
#endif
        g.setStroke( new BasicStroke( 1.0f, 0, BasicStroke.JOIN_ROUND ) );
        g.setColor( pen );
        g.drawRect( rc.x, rc.y, rc.width, rc.height );
    }*/

    // コントロールカーブのオーバーレイ表示
    /*if ( AppManager.curveOnPianoroll ) {
        g.setClip( null ); 
       
        Area fillarea = new Area( new Rectangle( key_width, 0, width - key_width, height ) ); // 塗りつぶす領域．最後に処理する
        //g.setColor( new Color( 255, 255, 255, 64 ) );
        //g.fillRect( key_width, 0, width - key_width, height );

        VsqBPList pbs = vsq_track.MetaText.PBS;
        if ( pbs == null ) {
            pbs = new VsqBPList( CurveType.PBS.getName(), 
                                 CurveType.PBS.getDefault(), 
                                 CurveType.PBS.getMinimum(), 
                                 CurveType.PBS.getMaximum() );
        }

        Color pitline = PortUtil.MidnightBlue;
        g.setStroke( getStroke2px() );
        lock ( AppManager.drawObjects ) {
            Vector<DrawObject> list = AppManager.drawObjects.get( selected - 1 );
            int j_start = AppManager.drawStartIndex[selected - 1];
            int c = list.size();
            int last_x = key_width;
            
            int pbs_count = pbs.size();
            double a = 1.0 / 8192.0;

            //ByRef<Integer> pit_index = new ByRef<Integer>( 0 );
            //ByRef<Integer> pbs_index_for_pit = new ByRef<Integer>( 0 );

            for ( int j = j_start; j < c; j++ ) {
                DrawObject dobj = list.get( j );
                int clock = dobj.clock;
                int x_at_clock = (int)(clock * scalex + xoffset);
                last_x = x_at_clock;

                // 音符の区間中に，PBSがあるかもしれないのでPBSのデータ点を探しながら塗りつぶす
                ByRef<Integer> pbs_index = new ByRef<Integer>( 0 );
                int last_pbs_value = pbs.getValue( clock, pbs_index );

                if ( pbs_count <= 0 ) {
                    // データ点が無い場合
                    double delta_note = 8192.0 * last_pbs_value * a;
                    int y_top = (int)(-(dobj.note + delta_note - 0.5) * track_height + yoffset);
                    int y_bottom = (int)(-(dobj.note - delta_note - 0.5) * track_height + yoffset);

                    if ( last_x < key_width ) {
                        last_x = key_width;
                    }
                    int x = (int)((clock + dobj.length) * scalex + xoffset);
                    fillarea.subtract( new Area( new Rectangle( last_x, y_top, x - last_x, y_bottom - y_top ) ) );
                    last_x = x;
                } else {
                    // データ点がある場合
                    for ( ; pbs_index.value < pbs_count; pbs_index.value++ ) {
                        int pbs_clock;
                        int pbs_value;
                        if ( 0 <= pbs_index.value + 1 && pbs_index.value + 1 < pbs_count ) {
                            pbs_clock = pbs.getKeyClock( pbs_index.value + 1 );
                            if ( pbs_clock > clock + dobj.length ) {
                                pbs_clock = clock + dobj.length;
                            }
                            pbs_value = pbs.getElement( pbs_index.value + 1 );
                        } else {
                            pbs_clock = clock + dobj.length;
                            pbs_value = last_pbs_value;
                        }

                        double delta_note = 8192.0 * last_pbs_value * a;

                        int y_top = (int)(-(dobj.note + delta_note - 0.5) * track_height + yoffset);
                        int y_bottom = (int)(-(dobj.note - delta_note - 0.5) * track_height + yoffset);
                        int x = (int)(pbs_clock * scalex + xoffset);
                        if ( x < key_width ) {
                            x = key_width;
                            last_x = x;
                            last_pbs_value = pbs_value;
                            continue;
                        }
                        if ( last_x < key_width ) {
                            last_x = key_width;
                        }

                        fillarea.subtract( new Area( new Rectangle( last_x, y_top, x - last_x, y_bottom - y_top ) ) );

                        last_x = x;
                        last_pbs_value = pbs_value;
                    }
                }
            }
        }

        Color fill = new Color( 0, 0, 0, 128 );
        g.setColor( fill );
        g.fill( fillarea );

        if ( mouseTracer.size() > 1 ) {
            commonDrawer.clear();
            g.setColor( Color.red );
            g.setStroke( getStroke2px() );
            for ( Iterator<Point> itr = mouseTracer.iterator(); itr.hasNext(); ) {
                Point pt = itr.next();
                commonDrawer.append( pt.x - stdx, pt.y - stdy );
            }
            commonDrawer.flush();
        }
    }*/

    // pictPositionIndicatorをペイント
    var c169_169_169 = new org.kbinani.java.awt.Color( 169, 169, 169 );
    var c104_104_104 = new org.kbinani.java.awt.Color( 104, 104, 104 );
    g.setColor( c169_169_169 );
    g.fillRect( key_width, 0, width - key_width, _PICT_POSITION_INDICATOR_HEIGHT );
    if( vsq != null ){
        var premeasure = vsq.getPreMeasure();
        barline_iterator.reset();
        var dashed_line_step = org.kbinani.cadencii.AppManager.getPositionQuantizeClock();
        var endclock = (width - xoffset) * inv_scalex;
        barline_iterator = vsq.getBarLineIterator( endclock );
        for ( ; barline_iterator.hasNext(); ) {
            var blt = barline_iterator.next();
            var local_clock_step = 1920 / blt.getLocalDenominator();
            var x = org.kbinani.PortUtil.castToInt( blt.clock() * scalex + xoffset );

            g.setColor( c104_104_104 );
            //g.setStroke( getStrokeDefault() );
            if ( blt.isSeparator() ) {
                var current = blt.getBarCount() - premeasure + 1;
                g.setColor( org.kbinani.java.awt.Color.black );
                g.drawString( current + "", x + 4, 13 );
                g.setColor( c104_104_104 );
                //ピアノロール上
                g.drawLine( x, 8, x, 16 );
                g.drawLine( x, 24, x, 32 );
                g.drawLine( x, 40, x, 48 );
            } else {
                //ピアノロール上
                g.drawLine( x, 11, x, 16 );
                g.drawLine( x, 27, x, 32 );
                g.drawLine( x, 43, x, 48 );
            }
        }
    }

    // 拍子の変更
    if( vsq != null ){
        for ( var i = 0; i < vsq.TimesigTable.length; i++ ) {
            var itemi = vsq.TimesigTable[i];
            var clock = itemi.Clock;
            var barcount = itemi.BarCount;
            var x = org.kbinani.cadencii.AppManager.xCoordFromClocks( clock );
            if ( width < x ) {
                break;
            }
            var s = itemi.Numerator + "/" + itemi.Denominator;
            /*g.setFont( SMALL_FONT );*/
            /*if ( AppManager.isSelectedTimesigContains( barcount ) ) {
                g.setColor( AppManager.getHilightColor() );
                g.drawString( s, x + 4, 46 );
            } else {*/
                g.setColor( org.kbinani.java.awt.Color.black );
                g.drawString( s, x + 4, 46 );
            /*}*/

            /*if ( m_position_indicator_mouse_down_mode == PositionIndicatorMouseDownMode.TIMESIG ) {
                if ( AppManager.isSelectedTimesigContains( barcount ) ) {
                    int edit_clock_x = AppManager.xCoordFromClocks( AppManager.getVsqFile().getClockFromBarCount( AppManager.getSelectedTimesig( barcount ).editing.BarCount ) );
                    g.setColor( s_pen_187_187_255 );
                    g.drawLine( edit_clock_x - 1, 32,
                                edit_clock_x - 1, picturePositionIndicator.getHeight() - 1 );
                    g.setColor( s_pen_007_007_151 );
                    g.drawLine( edit_clock_x, 32,
                                edit_clock_x, picturePositionIndicator.getHeight() - 1 );
                }
            }*/
        }

        // テンポの変更
        /*g.setFont( SMALL_FONT );*/
        for ( var i = 0; i < vsq.TempoTable.size(); i++ ) {
            var itemi = vsq.TempoTable.get( i );
            var clock = itemi.Clock;
            var x = org.kbinani.cadencii.AppManager.xCoordFromClocks( clock );
            if ( width < x ) {
                break;
            }
            var tempo = 60000000.0 / itemi.Tempo;
            var int_part = org.kbinani.PortUtil.castToInt( tempo );
            var small_part = org.kbinani.PortUtil.castToInt( (tempo - int_part) * 100 );
            var s = int_part + "." + org.kbinani.PortUtil.sprintf( "%02d", small_part );
            /*if ( AppManager.isSelectedTempoContains( clock ) ) {
                g.setColor( AppManager.getHilightColor() );
                g.drawString( s, x + 4, 30 );
            } else {*/
                g.setColor( org.kbinani.java.awt.Color.black );
                g.drawString( s, x + 4, 30 );
            /*}*/

            /*if ( m_position_indicator_mouse_down_mode == PositionIndicatorMouseDownMode.TEMPO ) {
                if ( AppManager.isSelectedTempoContains( clock ) ) {
                    int edit_clock_x = AppManager.xCoordFromClocks( AppManager.getSelectedTempo( clock ).editing.Clock );
                    g.setColor( s_pen_187_187_255 );
                    g.drawLine( edit_clock_x - 1, 18,
                                edit_clock_x - 1, 32 );
                    g.setColor( s_pen_007_007_151 );
                    g.drawLine( edit_clock_x, 18,
                                edit_clock_x, 32 );
                }
            }*/
        }
    }

    // マーカー
    var marker_x = org.kbinani.PortUtil.castToInt( 
        org.kbinani.cadencii.AppManager.getCurrentClock() * scalex
      + org.kbinani.cadencii.AppManager.keyOffset + key_width - stdx );
    if ( key_width <= marker_x && marker_x <= width ) {
        g.setColor( org.kbinani.java.awt.Color.white );
        /*g.setStroke( getStroke2px() );*/
        g.drawLine( marker_x, 0, marker_x, height );
        /*g.setStroke( getStrokeDefault() );*/
    }

    g.setColor( c169_169_169 );
    g.fillRect( 0, 0, key_width, _PICT_POSITION_INDICATOR_HEIGHT );
    g.setColor( c104_104_104 );
    g.drawLine( 0, 16, width, 16 );
    g.drawLine( 0, 32, width, 32 );
    g.drawLine( 0, 48, width, 48 );
    g.drawLine( key_width, 0, key_width, 48 );
    g.setColor( org.kbinani.java.awt.Color.black );
    g.drawString( "TEMPO", 6, 32 - 2 );
    g.drawString( "BEAT", 6, 48 - 2 );

    /* 横スクロールの左のスキマ */
    g.setColor( c194_194_194 );
    g.fillRect( 0, height - _SCROLL_WIDTH, key_width, _SCROLL_WIDTH );
    g.drawRect( 0, height - _SCROLL_WIDTH, key_width, _SCROLL_WIDTH );

    /* 横スクロールバー */
    // スクロールの背景
    g.setColor( c144_144_144 );
    g.fillRect( hScroll.left, hScroll.top, hScroll.width, hScroll.height );
    // スクロールの左ボックス
    g.setColor( c194_194_194 );
    g.fillRect( hScroll.left, hScroll.top, _SCROLL_WIDTH, _SCROLL_WIDTH );
    g.setColor( c144_144_144 );
    g.drawRect( hScroll.left, hScroll.top, _SCROLL_WIDTH, _SCROLL_WIDTH );
    g.setColor( org.kbinani.java.awt.Color.black );
    g.nativeGraphics.beginPath();
    var _SPACE = 4;
    g.nativeGraphics.moveTo( hScroll.left + _SCROLL_WIDTH - _SPACE, hScroll.top + _SPACE );
    g.nativeGraphics.lineTo( hScroll.left + _SPACE, hScroll.top + hScroll.height / 2 );
    g.nativeGraphics.lineTo( hScroll.left + _SCROLL_WIDTH - _SPACE, hScroll.top + hScroll.height - _SPACE );
    g.nativeGraphics.stroke();
    // スクロール右ボックス
    g.setColor( c194_194_194 );
    g.fillRect( hScroll.left + hScroll.width - _SCROLL_WIDTH, hScroll.top, _SCROLL_WIDTH, _SCROLL_WIDTH );
    g.setColor( c144_144_144 );
    g.drawRect( hScroll.left + hScroll.width - _SCROLL_WIDTH, hScroll.top, _SCROLL_WIDTH, _SCROLL_WIDTH );
    g.setColor( org.kbinani.java.awt.Color.black );
    g.nativeGraphics.beginPath();
    g.nativeGraphics.moveTo( hScroll.left + hScroll.width + _SPACE - _SCROLL_WIDTH, hScroll.top + _SPACE );
    g.nativeGraphics.lineTo( hScroll.left + hScroll.width - _SPACE, hScroll.top + hScroll.height / 2 );
    g.nativeGraphics.lineTo( hScroll.left + hScroll.width + _SPACE - _SCROLL_WIDTH, hScroll.top + hScroll.height - _SPACE );
    g.nativeGraphics.stroke();
    var hscroll_scale = (hScroll.width - _SCROLL_WIDTH * 2) / (hScroll.visibleAmount + hScroll.maximum - hScroll.minimum);
    hScroll.box_length = hScroll.visibleAmount * hscroll_scale;
    hScroll.box_pos = (hScroll.value -  hScroll.minimum) * hscroll_scale + hScroll.left + _SCROLL_WIDTH;
    g.setColor( c194_194_194 );
    g.fillRect( hScroll.box_pos, hScroll.top, hScroll.box_length, hScroll.height );
    g.setColor( c144_144_144 );
    g.drawRect( hScroll.box_pos, hScroll.top, hScroll.box_length, hScroll.height );

    /* 縦スクロールバー */
    // スクロール背景
    g.setColor( c144_144_144 );
    g.fillRect( vScroll.left, vScroll.top, vScroll.width, vScroll.height );
    g.drawRect( vScroll.left, vScroll.top, vScroll.width, vScroll.height );
    // スクロール上ボックス
    g.setColor( c194_194_194 );
    g.fillRect( vScroll.left + vScroll.width - _SCROLL_WIDTH, vScroll.top, _SCROLL_WIDTH, _SCROLL_WIDTH );
    g.setColor( c144_144_144 );
    g.drawRect( vScroll.left + vScroll.width - _SCROLL_WIDTH, vScroll.top, _SCROLL_WIDTH, _SCROLL_WIDTH );
    g.setColor( org.kbinani.java.awt.Color.black );
    g.nativeGraphics.beginPath();
    g.nativeGraphics.moveTo( vScroll.left + _SPACE, vScroll.top + _SCROLL_WIDTH - _SPACE );
    g.nativeGraphics.lineTo( vScroll.left + vScroll.width / 2, vScroll.top + _SPACE );
    g.nativeGraphics.lineTo( vScroll.left + vScroll.width - _SPACE, vScroll.top + _SCROLL_WIDTH - _SPACE );
    g.nativeGraphics.stroke();
    // スクロール下ボックス
    g.setColor( c194_194_194 );
    g.fillRect( vScroll.left, vScroll.top + vScroll.height - _SCROLL_WIDTH, _SCROLL_WIDTH, _SCROLL_WIDTH );
    g.setColor( c144_144_144 );
    g.drawRect( vScroll.left, vScroll.top + vScroll.height - _SCROLL_WIDTH, _SCROLL_WIDTH, _SCROLL_WIDTH );
    g.setColor( org.kbinani.java.awt.Color.black );
    g.nativeGraphics.beginPath();
    g.nativeGraphics.moveTo( vScroll.left + _SPACE, vScroll.top + vScroll.height - _SCROLL_WIDTH + _SPACE );
    g.nativeGraphics.lineTo( vScroll.left + vScroll.width / 2, vScroll.top + vScroll.height - _SPACE );
    g.nativeGraphics.lineTo( vScroll.left + vScroll.width - _SPACE, vScroll.top + vScroll.height - _SCROLL_WIDTH + _SPACE );
    g.nativeGraphics.stroke();
    var vscroll_scale = (vScroll.height - _SCROLL_WIDTH * 2) / (vScroll.visibleAmount + vScroll.maximum - vScroll.minimum);
    vScroll.box_length = vScroll.visibleAmount * vscroll_scale;
    vScroll.box_pos = (vScroll.value -  vScroll.minimum) * vscroll_scale + vScroll.top + _SCROLL_WIDTH;
    g.setColor( c194_194_194 );
    g.fillRect( vScroll.left, vScroll.box_pos, vScroll.width, vScroll.box_length );
    g.setColor( c144_144_144 );
    g.drawRect( vScroll.left, vScroll.box_pos, vScroll.width, vScroll.box_length );

    /* ズームスライダ */
    g.setColor( c194_194_194 );
    g.fillRect( trackBar.left, trackBar.top, trackBar.width, trackBar.height );
    g.setColor( c144_144_144 );
    g.drawRect( trackBar.left, trackBar.top, trackBar.width, trackBar.height );

    /* ズームスライダの右のスキマ */
    g.setColor( c194_194_194 );
    g.fillRect( vScroll.left, trackBar.top, _SCROLL_WIDTH, _SCROLL_WIDTH );
    g.setColor( c144_144_144 );
    g.drawRect( vScroll.left, trackBar.top, _SCROLL_WIDTH, _SCROLL_WIDTH );
}

/// <summary>
/// 描画すべきオブジェクトのリスト，AppManager.drawObjectsを更新します
/// </summary>
function updateDrawObjectList() {
    if ( org.kbinani.cadencii.AppManager.drawObjects == null ) {
        org.kbinani.cadencii.AppManager.drawObjects = new Array();
    }
    if ( org.kbinani.cadencii.AppManager.getVsqFile() == null ) {
        return;
    }
    for ( var i = 0; i < org.kbinani.cadencii.AppManager.drawStartIndex.length; i++ ) {
        org.kbinani.cadencii.AppManager.drawStartIndex[i] = 0;
    }
    if ( org.kbinani.cadencii.AppManager.drawObjects != null ) {
        for ( var itr = new org.kbinani.ArrayIterator( org.kbinani.cadencii.AppManager.drawObjects ); itr.hasNext(); ) {
            var list = itr.next();
            list.splice( 0, list.length );
        }
        org.kbinani.cadencii.AppManager.drawObjects.splice( org.kbinani.cadencii.AppManager.drawObjects.length );
    }

    var xoffset = 6;
    var yoffset = 127 * org.kbinani.cadencii.AppManager.editorConfig.PxTrackHeight;
    var scalex = org.kbinani.cadencii.AppManager.scaleX;
    var SMALL_FONT = null;
    /*SMALL_FONT = new Font( org.kbinani.cadencii.AppManager.editorConfig.ScreenFontName, java.awt.Font.PLAIN, org.kbinani.cadencii.AppManager.FONT_SIZE8 );*/
    var track_height = org.kbinani.cadencii.AppManager.editorConfig.PxTrackHeight;
    var vsq = org.kbinani.cadencii.AppManager.getVsqFile();
    var track_count = vsq.Track.length;
    for ( var track = 1; track < track_count; track++ ) {
        var vsq_track = vsq.Track[track];
        org.kbinani.cadencii.AppManager.drawObjects[track - 1] = new Array();

        // 音符イベント
        for ( var itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
            var ev = itr.next();
            var timesig = ev.Clock;
            if ( ev.ID.LyricHandle != null ) {
                var length = ev.ID.getLength();
                var note = ev.ID.Note;
                var x = org.kbinani.PortUtil.castToInt( timesig * scalex + xoffset );
                var y = -note * track_height + yoffset;
                var lyric_width = org.kbinani.PortUtil.castToInt( length * scalex );
                var lyric_jp = ev.ID.LyricHandle.L0.Phrase;
                var lyric_en = ev.ID.LyricHandle.L0.getPhoneticSymbol();
//alert( "updateDrawObjectList; lyric_jp=" + lyric_jp + "; lyric_en=" + lyric_en );
                var title = lyric_jp + " [" + lyric_en + "]";/*Utility.trimString( lyric_jp + " [" + lyric_en + "]", SMALL_FONT, lyric_width );*/
                var accent = ev.ID.DEMaccent;
                var vibrato_start = x + lyric_width;
                var vibrato_end = x;
                var vibrato_delay = lyric_width * 2;
                if ( ev.ID.VibratoHandle != null ) {
                    var rate = ev.ID.VibratoDelay / length;
                    vibrato_delay = _PX_ACCENT_HEADER + org.kbinani.PortUtil.castToInt( (lyric_width - _PX_ACCENT_HEADER) * rate );
                }
                var rate_bp = null;
                var depth_bp = null;
                var rate_start = 0;
                var depth_start = 0;
                if ( ev.ID.VibratoHandle != null ) {
                    rate_bp = ev.ID.VibratoHandle.getRateBP();
                    depth_bp = ev.ID.VibratoHandle.getDepthBP();
                    rate_start = ev.ID.VibratoHandle.getStartRate();
                    depth_start = ev.ID.VibratoHandle.getStartDepth();
                }
                org.kbinani.cadencii.AppManager.drawObjects[track - 1].push( new org.kbinani.cadencii.DrawObject( 
                                         org.kbinani.cadencii.DrawObjectType.Note,
                                         new org.kbinani.java.awt.Rectangle( x, y, lyric_width, track_height ),
                                         title,
                                         accent,
                                         ev.InternalID,
                                         vibrato_delay,
                                         false,
                                         ev.ID.LyricHandle.L0.PhoneticSymbolProtected,
                                         rate_bp,
                                         depth_bp,
                                         rate_start,
                                         depth_start,
                                         ev.ID.Note,
                                         null/*ev.UstEvent.Envelope*/,
                                         length,
                                         timesig,
                                         true ) );
            }
        }

        // Dynaff, Crescendイベント
        for ( var itr = vsq_track.getDynamicsEventIterator(); itr.hasNext(); ) {
            var item = itr.next();
            var handle = item.ID.IconDynamicsHandle;
            if ( handle == null ) {
                continue;
            }
            var clock = item.Clock;
            var length = item.ID.getLength();
            if ( length <= 0 ) {
                length = 1;
            }
            var raw_width = (int)(length * scalex);
            var type = org.kbinani.cadencii.DrawObjectType.Note;
            var width = 0;
            var str = "";
            if ( handle.isDynaffType() ) {
                // 強弱記号
                type = org.kbinani.cadencii.DrawObjectType.Dynaff;
                width = org.kbinani.cadencii.AppManager.DYNAFF_ITEM_WIDTH;
                var startDyn = handle.getStartDyn();
                if ( startDyn == 120 ) {
                    str = "fff";
                } else if ( startDyn == 104 ) {
                    str = "ff";
                } else if ( startDyn == 88 ) {
                    str = "f";
                } else if ( startDyn == 72 ) {
                    str = "mf";
                } else if ( startDyn == 56 ) {
                    str = "mp";
                } else if ( startDyn == 40 ) {
                    str = "p";
                } else if ( startDyn == 24 ) {
                    str = "pp";
                } else if ( startDyn == 8 ) {
                    str = "ppp";
                } else {
                    str = "?";
                }
            } else if ( handle.isCrescendType() ) {
                // クレッシェンド
                type = org.kbinani.cadencii.DrawObjectType.Crescend;
                width = raw_width;
                str = handle.IDS;
            } else if ( handle.isDecrescendType() ) {
                // デクレッシェンド
                type = org.kbinani.cadencii.DrawObjectType.Decrescend;
                width = raw_width;
                str = handle.IDS;
            }
            if ( type == DrawObjectType.Note ) {
                continue;
            }
            var note = item.ID.Note;
            var x = org.kbinani.PortUtil.castToInt( clock * scalex + xoffset );
            var y = -note * org.kbinani.cadencii.AppManager.editorConfig.PxTrackHeight + yoffset;
            org.kbinani.cadencii.AppManager.drawObjects[track - 1].push( new org.kbinani.cadencii.DrawObject( type,
                                     new org.kbinani.java.awt.Rectangle( x, y, width, track_height ),
                                     str,
                                     0,
                                     item.InternalID,
                                     0,
                                     false,
                                     false,
                                     null,
                                     null,
                                     0,
                                     0,
                                     item.ID.Note,
                                     null,
                                     length,
                                     clock,
                                     true ) );
        }

        // 重複部分があるかどうかを判定
        var count = org.kbinani.cadencii.AppManager.drawObjects[track - 1].length;
        for ( var i = 0; i < count - 1; i++ ) {
            var itemi = org.kbinani.cadencii.AppManager.drawObjects[track - 1][i];
            var parent_type = itemi.type;
            /*if ( itemi.type != DrawObjectType.Note ) {
                continue;
            }*/
            var overwrapped = false;
            var istart = itemi.clock;
            var iend = istart + itemi.length;
            if ( itemi.overlappe ) {
                continue;
            }
            for ( var j = i + 1; j < count; j++ ) {
                var itemj = org.kbinani.cadencii.AppManager.drawObjects[track - 1][j];
                if ( (itemj.type == org.kbinani.cadencii.DrawObjectType.Note && parent_type != org.kbinani.cadencii.DrawObjectType.Note) ||
                     (itemj.type != org.kbinani.cadencii.DrawObjectType.Note && parent_type == org.kbinani.cadencii.DrawObjectType.Note) ) {
                    continue;
                }
                var jstart = itemj.clock;
                var jend = jstart + itemj.length;
                if ( jstart <= istart ) {
                    if ( istart < jend ) {
                        overwrapped = true;
                        itemj.overlappe = true;
                        // breakできない．2個以上の重複を検出する必要があるので．
                    }
                }
                if ( istart <= jstart ) {
                    if ( jstart < iend ) {
                        overwrapped = true;
                        itemj.overlappe = true;
                    }
                }
            }
            if ( overwrapped ) {
                itemi.overlappe = true;
            }
        }
        org.kbinani.cadencii.AppManager.drawObjects[track - 1].sort( org.kbinani.cadencii.DrawObject.compare );
        //org.kbinani.cadencii.AppManager.drawObjects.push( tmp );
    }
}

function get_style_attribute( style, attr ){
    attr = attr + ":";
    var indx = style.indexOf( attr );
    var indx2 = style.indexOf( ";", indx + attr.length );
    return oldvalue = style.substring( indx + attr.length, indx2 );
}

function set_style_attribute( style, attr, value ){
    attr = attr + ":";
    var indx = style.indexOf( attr );
    var indx2 = style.indexOf( ";", indx + attr.length );
    var prefix = "";
    if( indx > 0 ){
        prefix = style.substring( 0, indx - 1 );
    }
    return style = prefix + attr + value + style.substring( indx2 );
}

function drop( e ){
    var files = e.dataTransfer.files;

    // ブラウザのデフォルトの動作を抑制
    e.preventDefault();

    // ファイルでないものがドロップされた場合
    if( files.length == 0 ){
        handleStream( e.dataTransfer.getData( "application/octet-stream" ) );
        return;
    }

    // 1個目のファイルだけ取り扱う
    handleFile( files[0] );
}

function handleStream( dat ){
    var search = ";base64,";
    var indx = dat.indexOf( search );
    if( indx >= 0 ){
        var b64 = dat.substring( indx + search.length );
        var decoded = org.kbinani.Base64.decode( b64 );
        var stream = new org.kbinani.ByteArrayInputStream( decoded );
        var vsq = new org.kbinani.vsq.VsqFile( stream, "Shift_JIS" );
        org.kbinani.cadencii.AppManager.setVsqFile( vsq );
        updateDrawObjectList();
        updateScrollRange();
    }
}

function handleFile( file ){
    var reader = new FileReader();

    reader.onloadend = function(){
        var dat = reader.result;
        handleStream( dat );
        var title = document.getElementsByTagName( "title" )[0];
        title.innerHTML = file.name + " - Cadencii";
    }

    reader.readAsDataURL( file );
}

function dragover( e ){
    e.preventDefault();
}

function dragleave( e ){
    dropbox.removeAttribute( "dragenter" );
}

window.addEventListener( "load", init, true );
