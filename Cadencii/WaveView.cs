/*
 * WaveView.cs
 * Copyright (C) 2009-2010 kbinani
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

import java.awt.*;
import java.awt.image.*;
import org.kbinani.*;
import org.kbinani.media.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.awt.image;
using org.kbinani.media;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using BEventArgs = System.EventArgs;
#endif

    /// <summary>
    /// トラック16個分の波形描画コンテキストを保持し、それらの描画を行うコンポーネントです。
    /// </summary>
#if JAVA
    public class WaveView extends BPanel {
#else
    public class WaveView : BPanel {
#endif
        /// <summary>
        /// 波形描画用のコンテキスト
        /// </summary>
        private WaveDrawContext[] mDrawer = new WaveDrawContext[16];
        /// <summary>
        /// グラフィクスオブジェクトのキャッシュ
        /// </summary>
        private Graphics2D mGraphics = null;
        /// <summary>
        /// 縦軸方向のスケール
        /// </summary>
        private int mScale = MIN_SCALE;
        private const int MAX_SCALE = 100;
        private const int MIN_SCALE = 10;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WaveView()
#if JAVA
        {
#else
            :
#endif
            base()
#if JAVA
            ;
#else
        {
#endif
#if !JAVA
            this.SetStyle( ControlStyles.DoubleBuffer, true );
            this.SetStyle( ControlStyles.UserPaint, true );
            this.DoubleBuffered = true;
#endif
        }

        private void paint( Graphics2D g ) {
            int width = getWidth();
            int height = getHeight();
            Rectangle rc = new Rectangle( 0, 0, width, height );

            // 背景を塗りつぶす
            g.setColor( Color.gray );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );

            // スケール線を描く
            int half_height = height / 2;
            g.setColor( Color.black );
            g.drawLine( 0, half_height, width, half_height );
            
            if ( AppManager.skipDrawingWaveformWhenPlaying && AppManager.isPlaying() ) {
                PortUtil.drawStringEx( g, 
                                       "(hidden for performance)", 
                                       AppManager.baseFont10,
                                       rc, 
                                       PortUtil.STRING_ALIGN_CENTER,
                                       PortUtil.STRING_ALIGN_CENTER );
                return;
            }
            int selected = AppManager.getSelected();
            WaveDrawContext context = mDrawer[selected - 1];
            if ( context == null ) {
                return;
            }
            context.draw( g,
                          Color.black,
                          rc,
                          AppManager.clockFromXCoord( AppManager.keyWidth ),
                          AppManager.clockFromXCoord( AppManager.keyWidth + width ),
                          AppManager.getVsqFile().TempoTable,
                          AppManager.getScaleX(),
                          mScale / 10.0f );
        }

        /// <summary>
        /// 縦方向の描画倍率を取得します。
        /// </summary>
        /// <seealso cref="getScale"/>
        /// <returns></returns>
        public int scale() {
            return mScale;
        }

        /// <summary>
        /// 縦方向の描画倍率を取得します。
        /// </summary>
        /// <returns></returns>
        public int getScale() {
            return mScale;
        }

        /// <summary>
        /// 縦方向の描画倍率を設定します。
        /// </summary>
        /// <param name="value"></param>
        public void setScale( int value ) {
            if ( value < MIN_SCALE ) {
                mScale = MIN_SCALE;
            } else if ( MAX_SCALE < value ) {
                mScale = MAX_SCALE;
            } else {
                mScale = value;
            }
        }

        /// <summary>
        /// 縦方向の拡大率を一段階上げます
        /// </summary>
        public void zoom() {
            setScale( mScale + 1 );
        }

        /// <summary>
        /// 縦方向の拡大率を一段階下げます
        /// </summary>
        public void mooz() {
            setScale( mScale - 1 );
        }

        /// <summary>
        /// 全ての波形描画コンテキストが保持しているデータをクリアします
        /// </summary>
        public void unloadAll() {
            for ( int i = 0; i < mDrawer.Length; i++ ) {
                WaveDrawContext context = mDrawer[i];
                if ( context == null ) {
                    continue;
                }
                context.unload();
            }
        }

        /// <summary>
        /// 波形描画コンテキストに、指定したWAVEファイルの指定区間を再度読み込みます。
        /// </summary>
        /// <param name="index">読込を行わせる波形描画コンテキストのインデックス</param>
        /// <param name="file">読み込むWAVEファイルのパス</param>
        /// <param name="sec_from">読み込み区間の開始秒時</param>
        /// <param name="sec_to">読み込み区間の終了秒時</param>
        public void reloadPartial( int index, String file, double sec_from, double sec_to ) {
            if ( index < 0 || mDrawer.Length <= index ) {
                return;
            }
            if ( mDrawer[index] == null ) {
                mDrawer[index] = new WaveDrawContext();
                mDrawer[index].load( file );
            } else {
                mDrawer[index].reloadPartial( file, sec_from, sec_to );
            }
        }

        /// <summary>
        /// 波形コンテキストに、指定したWAVEファイルを読み込みます。
        /// </summary>
        /// <param name="index">読込を行わせる波形描画コンテキストのインデックス</param>
        /// <param name="wave_path">読み込むWAVEファイルのパス</param>
        public void load( int index, String wave_path ) {
            if ( index < 0 || mDrawer.Length <= index ) {
                return;
            }
            if ( mDrawer[index] == null ) {
                mDrawer[index] = new WaveDrawContext();
            }
            mDrawer[index].load( wave_path );
        }

#if !JAVA
        protected override void OnPaint( PaintEventArgs e ) {
            base.OnPaint( e );
            if ( mGraphics == null ) {
                mGraphics = new Graphics2D( null );
            }
            mGraphics.nativeGraphics = e.Graphics;
            paint( mGraphics );
        }
#endif
    }

#if !JAVA
}
#endif
