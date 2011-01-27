/*
 * FormVibratoPreset.cs
 * Copyright © 2010 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/Cadencii/FormVibratoPreset.java

import java.awt.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.apputil;
using org.kbinani.vsq;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
    using BEventArgs = System.EventArgs;
    using BPaintEventArgs = System.Windows.Forms.PaintEventArgs;
    using BEventHandler = System.EventHandler;
    using BPaintEventHandler = System.Windows.Forms.PaintEventHandler;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormVibratoPreset extends BDialog
#else
    public class FormVibratoPreset : BDialog
#endif
    {
        /// <summary>
        /// プレビューの各グラフにおいて，上下に追加するマージンの高さ(ピクセル)
        /// </summary>
        private const int MARGIN = 3;
        /// <summary>
        /// 折れ線の描画時に，描画するかどうかを決める閾値
        /// </summary>
        private const int MIN_DELTA = 2;
        /// <summary>
        /// 前回サイズ変更時の，フォームの幅
        /// </summary>
        private static int mPreviousWidth = 527;
        /// <summary>
        /// 前回サイズ変更時の，フォームの高さ
        /// </summary>
        private static int mPreviousHeight = 418;

        /// <summary>
        /// AppManager.editorConfig.AutoVibratoCustomからコピーしてきた，
        /// ビブラートハンドルのリスト
        /// </summary>
        private Vector<VibratoHandle> mHandles;
        /// <summary>
        /// 選択状態のビブラートハンドル
        /// </summary>
        private VibratoHandle mSelected = null;
        /// <summary>
        /// Rateカーブを描画するのに使う描画器
        /// </summary>
        private LineGraphDrawer mDrawerRate = null;
        /// <summary>
        /// Depthカーブを描画するのに使う描画器
        /// </summary>
        private LineGraphDrawer mDrawerDepth = null;
        /// <summary>
        /// 結果として得られるピッチベンドカーブを描画するのに使う描画器
        /// </summary>
        private LineGraphDrawer mDrawerResulting = null;

        /// <summary>
        /// コンストラクタ．
        /// </summary>
        /// <param name="handles"></param>
        public FormVibratoPreset( Vector<VibratoHandle> handles )
        {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            applyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
            this.setSize( mPreviousWidth, mPreviousHeight );
            registerEventHandlers();

            // ハンドルのリストをクローン
            mHandles = new Vector<VibratoHandle>();
            int size = handles.size();
            for ( int i = 0; i < size; i++ ) {
                mHandles.add( (VibratoHandle)handles.get( i ).clone() );
            }

            // 表示状態を更新
            updateStatus();
            if ( size > 0 ) {
                listPresets.setSelectedIndex( 0 );
            }
        }

        #region public methods
        /// <summary>
        /// ダイアログによる設定結果を取得します
        /// </summary>
        /// <returns></returns>
        public Vector<VibratoHandle> getResult()
        {
            // iconIDを整える
            if ( mHandles == null ) {
                mHandles = new Vector<VibratoHandle>();
            }
            int size = mHandles.size();
            for ( int i = 0; i < size; i++ ) {
                mHandles.get( i ).IconID = "$0404" + PortUtil.toHexString( i + 1, 4 );
            }
            return mHandles;
        }
        #endregion

        #region event handlers
        public void buttonOk_Click( Object sender, EventArgs e )
        {
            setDialogResult( BDialogResult.OK );
        }
        
        public void buttonCancel_Click( Object sender, EventArgs e )
        {
            setDialogResult( BDialogResult.CANCEL );
        }
        
        public void listPresets_SelectedIndexChanged( Object sender, EventArgs e )
        {
            // インデックスを取得
            int index = listPresets.getSelectedIndex();
#if DEBUG
            sout.println( "FormVibratoPreset#listPresets_SelectedIndexChanged; index=" + index );
#endif

            // 範囲外ならbailout
            if ( (index < 0) || (mHandles.size() <= index) ) {
#if DEBUG
                sout.println( "FormVibratoPreset#listPresets_SelectedIndexChanged; bail-out, mSelected -> null; index=" + index );
#endif
                mSelected = null;
                return;
            }

            // イベントハンドラを一時的に取り除く
            textDepth.TextChanged -= new BEventHandler( textDepth_TextChanged );
            textRate.TextChanged -= new BEventHandler( textRate_TextChanged );
            textName.TextChanged -= new BEventHandler( textName_TextChanged );

            // テクストボックスに値を反映
            mSelected = mHandles.get( index );
            textDepth.setText( mSelected.getStartDepth() + "" );
            textRate.setText( mSelected.getStartRate() + "" );
            textName.setText( mSelected.getCaption() );

            // イベントハンドラを再登録
            textDepth.TextChanged += new BEventHandler( textDepth_TextChanged );
            textRate.TextChanged += new BEventHandler( textRate_TextChanged );
            textName.TextChanged += new BEventHandler( textName_TextChanged );

            // 再描画
            repaintPictures();
        }

        public void textName_TextChanged( Object sender, EventArgs e )
        {
            if ( mSelected == null ) {
                return;
            }

            mSelected.setCaption( textName.getText() );
            int index = listPresets.getSelectedIndex();
            if( index >= 0 ){
                listPresets.setItemAt( index, mSelected.getCaption() );
            }
        }

        public void textRate_TextChanged( Object sender, EventArgs e )
        {
            if ( mSelected == null ) {
                return;
            }

            int old = mSelected.getStartRate();
            int value = old;
            String s = textRate.getText();
            try {
                value = str.toi( s );
            } catch ( Exception ex ) {
                value = old;
            }
            if ( value < 0 ) {
                value = 0;
            }
            if ( 127 < value ) {
                value = 127;
            }
            mSelected.setStartRate( value );
            String nstr = value + "";
            if ( !str.compare( s, nstr ) ) {
                textRate.setText( nstr );
#if JAVA
                textRate.setCaretPosition( nstr.length() );
#else
                textRate.SelectionStart = textRate.Text.Length;
#endif
            }

            repaintPictures();
        }

        public void textDepth_TextChanged( Object sender, EventArgs e )
        {
            if ( mSelected == null ) {
                return;
            }

            int old = mSelected.getStartDepth();
            int value = old;
            String s = textDepth.getText();
            try {
                value = str.toi( s );
            } catch ( Exception ex ) {
                value = old;
            }
            if ( value < 0 ) {
                value = 0;
            }
            if ( 127 < value ) {
                value = 127;
            }
            mSelected.setStartDepth( value );
            String nstr = value + "";
            if ( !str.compare( s, nstr ) ) {
                textDepth.setText( nstr );
#if JAVA
                textDepth.setCaretPosition( nstr.length() );
#else
                textDepth.SelectionStart = textDepth.Text.Length;
#endif
            }

            repaintPictures();
        }

        public void buttonAdd_Click( Object sender, EventArgs e )
        {
            // 追加し，
            VibratoHandle handle = new VibratoHandle();
            handle.setCaption( "No-Name" );
            mHandles.add( handle );
            listPresets.clearSelection();
            // 表示反映させて
            updateStatus();
            // 追加したのを選択状態にする
            listPresets.setSelectedIndex( mHandles.size() - 1 );
        }

        public void buttonRemove_Click( Object sender, EventArgs e )
        {
            int index = listPresets.getSelectedIndex();
            if ( index < 0 || listPresets.getItemCount() <= index ) {
                return;
            }

            mHandles.removeElementAt( index );
            updateStatus();
        }

        public void handleUpDownButtonClick( Object sender, EventArgs e )
        {
            // 送信元のボタンによって，選択インデックスの増分を変える
            int delta = 1;
            if ( sender == buttonUp ) {
                delta = -1;
            }

            // 移動後のインデックスは？
            int index = listPresets.getSelectedIndex();
            int move_to = index + delta;

            // 範囲内かどうか
            if ( index < 0 ) {
                return;
            }
            if ( move_to < 0 || mHandles.size() <= move_to ) {
                // 範囲外なら何もしない
                return;
            }

            // 入れ替える
            VibratoHandle buff = mHandles.get( index );
            mHandles.set( index, mHandles.get( move_to ) );
            mHandles.set( move_to, buff );

            // 選択状態を変える
            listPresets.clearSelection();
            updateStatus();
            listPresets.setSelectedIndex( move_to );
        }

        public void pictureResulting_Paint( Object sender, BPaintEventArgs e )
        {
            // 背景を描画
            int raw_width = pictureResulting.getWidth();
            int raw_height = pictureResulting.getHeight();
#if JAVA
            Graphics g = e.Graphics;
            g.setColor( PortUtil.LightGray );
            g.fillRect( 0, 0, raw_width, raw_height );
#else
            System.Drawing.Graphics g = e.Graphics;
            g.FillRectangle( System.Drawing.Brushes.LightGray, 0, 0, raw_width, raw_height );
#endif

            // 選択中のハンドルを取得
            VibratoHandle handle = mSelected;
            if ( handle == null ) {
                return;
            }

            // 描画の準備
            LineGraphDrawer d = getDrawerResulting();
            d.setGraphics( g );

            // ビブラートのピッチベンドを取得するイテレータを取得
            int width = raw_width;
            int vib_length = 960;
            int tempo = 500000;
            double vib_seconds = tempo * 1e-6 / 480.0 * vib_length;
            // 480クロックは0.5秒
            VsqFileEx vsq = new VsqFileEx( "Miku", 1, 4, 4, tempo );
            VibratoBPList list_rate = handle.getRateBP();
            VibratoBPList list_depth = handle.getDepthBP();
            int start_rate = handle.getStartRate();
            int start_depth = handle.getStartDepth();
            if ( list_rate == null ) {
                list_rate = new VibratoBPList( new float[] { 0.0f }, new int[] { start_rate } );
            }
            if ( list_depth == null ) {
                list_depth = new VibratoBPList( new float[] { 0.0f }, new int[] { start_depth } );
            }
            // 解像度
            float resol = (float)(vib_seconds / width);
            if ( resol <= 0.0f ) {
                return;
            }
            VibratoPointIteratorBySec itr =
                new VibratoPointIteratorBySec(
                    vsq,
                    list_rate, start_rate,
                    list_depth, start_depth,
                    0, vib_length, resol );

            // 描画
            int height = raw_height - MARGIN * 2;
            d.clear();
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int x = 0;
            int lastx = 0;
            int lasty = -10;
            int tx = 0, ty = 0;
            for ( ; itr.hasNext(); x++ ) {
                double pitch = itr.next().getY();
                int y = height - (int)((pitch + 1.25) / 2.5 * height) + MARGIN - 1;
                int dx = x - lastx; // xは単調増加
                int dy = Math.Abs( y - lasty );
                tx = x;
                ty = y;
                //if ( dx > MIN_DELTA || dy > MIN_DELTA ) {
                    d.append( x, y );
                    lastx = x;
                    lasty = y;
                //}
            }
            d.append( tx, ty );
            d.flush();
        }

        public void pictureRate_Paint( Object sender, BPaintEventArgs e )
        {
            // 背景を描画
            int width = pictureRate.getWidth();
            int height = pictureRate.getHeight();
#if JAVA
            Graphics g = e.Graphics;
            g.setColor( PortUtil.LightGray );
            g.fillRect( 0, 0, width, height );
#else
            System.Drawing.Graphics g = e.Graphics;
            g.FillRectangle( System.Drawing.Brushes.LightGray, 0, 0, width, height );
#endif

            // 選択中のハンドルを取得
            VibratoHandle handle = mSelected;
            if( handle == null ) {
                return;
            }

            // 描画の準備
            LineGraphDrawer d = getDrawerRate();
            d.clear();
            d.setGraphics( g );
            drawVibratoCurve( 
                handle.getRateBP(), 
                handle.getStartRate(),
                d, 
                width, height );
        }

        public void pictureDepth_Paint( Object sender, BPaintEventArgs e )
        {
            // 背景を描画
            int width = pictureDepth.getWidth();
            int height = pictureDepth.getHeight();
#if JAVA
            Graphics g = e.Graphics;
            g.setColor( PortUtil.LightGray );
            g.fillRect( 0, 0, width, height );
#else
            System.Drawing.Graphics g = e.Graphics;
            g.FillRectangle( System.Drawing.Brushes.LightGray, 0, 0, width, height );
#endif

            // 選択中のハンドルを取得
            VibratoHandle handle = mSelected;
            if ( handle == null ) {
                return;
            }

            // 描画の準備
            LineGraphDrawer d = getDrawerDepth();
            d.clear();
            d.setGraphics( g );
            drawVibratoCurve(
                handle.getDepthBP(),
                handle.getStartDepth(),
                d,
                width, height );
        }

        public void FormVibratoPreset_Resize( Object sender, EventArgs e )
        {
#if !JAVA
            if ( this.WindowState == System.Windows.Forms.FormWindowState.Normal ) {
                mPreviousWidth = this.getWidth();
                mPreviousHeight = this.getHeight();
            }
#endif
            repaintPictures();
        }
        #endregion

        #region helper methods
        /// <summary>
        /// イベントハンドラを登録します
        /// </summary>
        private void registerEventHandlers()
        {
            listPresets.SelectedIndexChanged += new BEventHandler( listPresets_SelectedIndexChanged );
            textDepth.TextChanged += new BEventHandler( textDepth_TextChanged );
            textRate.TextChanged += new BEventHandler( textRate_TextChanged );
            textName.TextChanged += new BEventHandler( textName_TextChanged );
            buttonAdd.Click += new BEventHandler( buttonAdd_Click );
            buttonRemove.Click += new BEventHandler( buttonRemove_Click );
            buttonUp.Click += new BEventHandler( handleUpDownButtonClick );
            buttonDown.Click += new BEventHandler( handleUpDownButtonClick );

            pictureDepth.Paint += new BPaintEventHandler( pictureDepth_Paint );
            pictureRate.Paint += new BPaintEventHandler( pictureRate_Paint );
            pictureResulting.Paint += new BPaintEventHandler( pictureResulting_Paint );

            this.Resize += new BEventHandler( FormVibratoPreset_Resize );
            buttonOk.Click += new BEventHandler( buttonOk_Click );
            buttonCancel.Click += new BEventHandler( buttonCancel_Click );
        }

        private static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        private void applyLanguage()
        {
            this.setTitle( _( "Vibrato preset" ) );

            labelPresets.setText( _( "List of vibrato preset" ) );

            groupEdit.setTitle( _( "Edit" ) );
            labelName.setText( _( "Name" ) );

            groupPreview.setTitle( _( "Preview" ) );
            labelDepthCurve.setText( _( "Depth curve" ) );
            labelRateCurve.setText( _( "Rate curve" ) );
            labelResulting.setText( _( "Resulting pitch bend" ) );

            buttonAdd.setText( _( "Add" ) );
            buttonRemove.setText( _( "Remove" ) );
            buttonUp.setText( _( "Up" ) );
            buttonDown.setText( _( "Down" ) );

            buttonOk.setText( _( "OK" ) );
            buttonCancel.setText( _( "Cancel" ) );
        }

        /// <summary>
        /// Rate, Depth, Resulting pitchの各グラフを強制描画します
        /// </summary>
        private void repaintPictures()
        {
            pictureDepth.repaint();
            pictureRate.repaint();
            pictureResulting.repaint();
        }

        /// <summary>
        /// ビブラートのRateまたはDepthカーブを指定したサイズで描画します
        /// </summary>
        /// <param name="list">描画するカーブ</param>
        /// <param name="start_value"></param>
        /// <param name="drawer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void drawVibratoCurve( VibratoBPList list, int start_value, LineGraphDrawer drawer, int width, int height )
        {
            int size = 0;
            if ( list != null ) {
                size = list.getCount();
            }
            drawer.clear();
            drawer.setBaseLineY( height );
            int iy0 = height - (int)(start_value / 127.0 * height);
            drawer.append( 0, iy0 );
            int lasty = iy0;
            for ( int i = 0; i < size; i++ ) {
                VibratoBPPair p = list.getElement( i );
                int ix = (int)(p.X * width);
                int iy = height - (int)(p.Y / 127.0 * height);
                drawer.append( ix, iy );
                lasty = iy;
            }
            drawer.append( width + drawer.getDotSize() * 2, lasty );
            drawer.flush();
        }

        /// <summary>
        /// Rateカーブを描画するのに使う描画器を取得します
        /// </summary>
        /// <returns></returns>
        private LineGraphDrawer getDrawerRate()
        {
            if ( mDrawerRate == null ) {
                mDrawerRate = new LineGraphDrawer( LineGraphDrawer.TYPE_STEP );
                mDrawerRate.setDotMode( LineGraphDrawer.DOTMODE_ALWAYS );
                mDrawerRate.setFillColor( PortUtil.CornflowerBlue );
            }
            return mDrawerRate;
        }

        /// <summary>
        /// Depthカーブを描画するのに使う描画器を取得します
        /// </summary>
        /// <returns></returns>
        private LineGraphDrawer getDrawerDepth()
        {
            if ( mDrawerDepth == null ) {
                mDrawerDepth = new LineGraphDrawer( LineGraphDrawer.TYPE_STEP );
                mDrawerDepth.setDotMode( LineGraphDrawer.DOTMODE_ALWAYS );
                mDrawerDepth.setFillColor( PortUtil.CornflowerBlue );
            }
            return mDrawerDepth;
        }

        /// <summary>
        /// 結果として得られるピッチベンドカーブを描画するのに使う描画器を取得します
        /// </summary>
        /// <returns></returns>
        private LineGraphDrawer getDrawerResulting()
        {
            if ( mDrawerResulting == null ) {
                mDrawerResulting = new LineGraphDrawer( LineGraphDrawer.TYPE_LINEAR );
                mDrawerResulting.setDotMode( LineGraphDrawer.DOTMODE_NO );
                mDrawerResulting.setFill( false );
                mDrawerResulting.setLineWidth( 2 );
                mDrawerResulting.setLineColor( PortUtil.ForestGreen );
            }
            return mDrawerResulting;
        }

        /// <summary>
        /// 画面の表示状態を更新します
        /// </summary>
        private void updateStatus()
        {
            int old_select = listPresets.getSelectedIndex();
            listPresets.clearSelection();

            // アイテムの個数に過不足があれば数を整える
            int size = mHandles.size();
            int delta = size - listPresets.getItemCount();
#if DEBUG
            sout.println( "FormVibratoPreset#updateStatus; delta=" + delta );
#endif
            if ( delta > 0 ) {
                for ( int i = 0; i < delta; i++ ) {
                    listPresets.addItem( "" );
                }
            } else if ( delta < 0 ) {
                for ( int i = 0; i < -delta; i++ ) {
                    listPresets.removeItemAt( 0 );
                }
            }

            // アイテムを更新
            for ( int i = 0; i < size; i++ ) {
                VibratoHandle handle = mHandles.get( i );
                listPresets.setItemAt( i, handle.getCaption() );
            }

            // 選択状態を復帰
            if ( size <= old_select ) {
                old_select = size - 1;
            }
#if DEBUG
            sout.println( "FormVibratoPreset#updateStatus; A; old_selected=" + old_select );
#endif
            if ( old_select >= 0 ) {
#if DEBUG
                sout.println( "FormVibratoPreset#updateStatus; B; old_selected=" + old_select );
#endif
                //listPresets.SelectedIndexChanged -= new BEventHandler( listPresets_SelectedIndexChanged );
                listPresets.setSelectedIndex( old_select );
                //listPresets.SelectedIndexChanged += new BEventHandler( listPresets_SelectedIndexChanged );
            }
        }
        #endregion

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/Cadencii/FormVibratoPreset.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/Cadencii/FormVibratoPreset.java
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
            this.buttonCancel = new org.kbinani.windows.forms.BButton();
            this.buttonOk = new org.kbinani.windows.forms.BButton();
            this.buttonRemove = new org.kbinani.windows.forms.BButton();
            this.buttonAdd = new org.kbinani.windows.forms.BButton();
            this.buttonUp = new org.kbinani.windows.forms.BButton();
            this.buttonDown = new org.kbinani.windows.forms.BButton();
            this.labelRate = new org.kbinani.windows.forms.BLabel();
            this.labelDepth = new org.kbinani.windows.forms.BLabel();
            this.labelPresets = new org.kbinani.windows.forms.BLabel();
            this.pictureRate = new org.kbinani.windows.forms.BPictureBox();
            this.labelRateCurve = new org.kbinani.windows.forms.BLabel();
            this.labelDepthCurve = new org.kbinani.windows.forms.BLabel();
            this.pictureDepth = new org.kbinani.windows.forms.BPictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.labelResulting = new org.kbinani.windows.forms.BLabel();
            this.pictureResulting = new org.kbinani.windows.forms.BPictureBox();
            this.groupEdit = new org.kbinani.windows.forms.BGroupBox();
            this.textName = new org.kbinani.windows.forms.BTextBox();
            this.labelName = new org.kbinani.windows.forms.BLabel();
            this.groupPreview = new org.kbinani.windows.forms.BGroupBox();
            this.listPresets = new BListBox();
            this.textDepth = new org.kbinani.cadencii.NumberTextBox();
            this.textRate = new org.kbinani.cadencii.NumberTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDepth)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureResulting)).BeginInit();
            this.groupEdit.SuspendLayout();
            this.groupPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point( 424, 345 );
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size( 75, 23 );
            this.buttonCancel.TabIndex = 11;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point( 343, 345 );
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size( 75, 23 );
            this.buttonOk.TabIndex = 10;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemove.Location = new System.Drawing.Point( 12, 301 );
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size( 75, 23 );
            this.buttonRemove.TabIndex = 3;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.Location = new System.Drawing.Point( 12, 272 );
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size( 75, 23 );
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            // 
            // buttonUp
            // 
            this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUp.Location = new System.Drawing.Point( 102, 272 );
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size( 75, 23 );
            this.buttonUp.TabIndex = 4;
            this.buttonUp.Text = "Up";
            this.buttonUp.UseVisualStyleBackColor = true;
            // 
            // buttonDown
            // 
            this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDown.Location = new System.Drawing.Point( 102, 301 );
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size( 75, 23 );
            this.buttonDown.TabIndex = 5;
            this.buttonDown.Text = "Down";
            this.buttonDown.UseVisualStyleBackColor = true;
            // 
            // labelRate
            // 
            this.labelRate.AutoSize = true;
            this.labelRate.Location = new System.Drawing.Point( 10, 15 );
            this.labelRate.Name = "labelRate";
            this.labelRate.Size = new System.Drawing.Size( 54, 12 );
            this.labelRate.TabIndex = 129;
            this.labelRate.Text = "Start rate";
            // 
            // labelDepth
            // 
            this.labelDepth.AutoSize = true;
            this.labelDepth.Location = new System.Drawing.Point( 10, 40 );
            this.labelDepth.Name = "labelDepth";
            this.labelDepth.Size = new System.Drawing.Size( 62, 12 );
            this.labelDepth.TabIndex = 130;
            this.labelDepth.Text = "Start depth";
            // 
            // labelPresets
            // 
            this.labelPresets.AutoSize = true;
            this.labelPresets.Location = new System.Drawing.Point( 12, 15 );
            this.labelPresets.Name = "labelPresets";
            this.labelPresets.Size = new System.Drawing.Size( 113, 12 );
            this.labelPresets.TabIndex = 134;
            this.labelPresets.Text = "List of preset vibrato";
            // 
            // pictureRate
            // 
            this.pictureRate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureRate.Location = new System.Drawing.Point( 12, 20 );
            this.pictureRate.Name = "pictureRate";
            this.pictureRate.Size = new System.Drawing.Size( 133, 74 );
            this.pictureRate.TabIndex = 135;
            this.pictureRate.TabStop = false;
            // 
            // labelRateCurve
            // 
            this.labelRateCurve.AutoSize = true;
            this.labelRateCurve.Location = new System.Drawing.Point( 10, 5 );
            this.labelRateCurve.Name = "labelRateCurve";
            this.labelRateCurve.Size = new System.Drawing.Size( 61, 12 );
            this.labelRateCurve.TabIndex = 136;
            this.labelRateCurve.Text = "Rate curve";
            // 
            // labelDepthCurve
            // 
            this.labelDepthCurve.AutoSize = true;
            this.labelDepthCurve.Location = new System.Drawing.Point( 10, 5 );
            this.labelDepthCurve.Name = "labelDepthCurve";
            this.labelDepthCurve.Size = new System.Drawing.Size( 67, 12 );
            this.labelDepthCurve.TabIndex = 137;
            this.labelDepthCurve.Text = "Depth curve";
            // 
            // pictureDepth
            // 
            this.pictureDepth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureDepth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureDepth.Location = new System.Drawing.Point( 12, 20 );
            this.pictureDepth.Name = "pictureDepth";
            this.pictureDepth.Size = new System.Drawing.Size( 146, 73 );
            this.pictureDepth.TabIndex = 138;
            this.pictureDepth.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.pictureRate );
            this.splitContainer1.Panel1.Controls.Add( this.labelRateCurve );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.pictureDepth );
            this.splitContainer1.Panel2.Controls.Add( this.labelDepthCurve );
            this.splitContainer1.Size = new System.Drawing.Size( 310, 97 );
            this.splitContainer1.SplitterDistance = 148;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 139;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point( 3, 15 );
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.splitContainer1 );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.pictureResulting );
            this.splitContainer2.Panel2.Controls.Add( this.labelResulting );
            this.splitContainer2.Size = new System.Drawing.Size( 310, 195 );
            this.splitContainer2.SplitterDistance = 97;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 140;
            // 
            // labelResulting
            // 
            this.labelResulting.AutoSize = true;
            this.labelResulting.Location = new System.Drawing.Point( 10, 5 );
            this.labelResulting.Name = "labelResulting";
            this.labelResulting.Size = new System.Drawing.Size( 110, 12 );
            this.labelResulting.TabIndex = 137;
            this.labelResulting.Text = "Resulting pitch bend";
            // 
            // pictureResulting
            // 
            this.pictureResulting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureResulting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureResulting.Location = new System.Drawing.Point( 12, 20 );
            this.pictureResulting.Name = "pictureResulting";
            this.pictureResulting.Size = new System.Drawing.Size( 295, 71 );
            this.pictureResulting.TabIndex = 136;
            this.pictureResulting.TabStop = false;
            // 
            // groupEdit
            // 
            this.groupEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupEdit.Controls.Add( this.textName );
            this.groupEdit.Controls.Add( this.labelName );
            this.groupEdit.Controls.Add( this.labelDepth );
            this.groupEdit.Controls.Add( this.textDepth );
            this.groupEdit.Controls.Add( this.textRate );
            this.groupEdit.Controls.Add( this.labelRate );
            this.groupEdit.Location = new System.Drawing.Point( 183, 15 );
            this.groupEdit.Name = "groupEdit";
            this.groupEdit.Size = new System.Drawing.Size( 316, 90 );
            this.groupEdit.TabIndex = 6;
            this.groupEdit.TabStop = false;
            this.groupEdit.Text = "Edit";
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point( 86, 62 );
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size( 169, 19 );
            this.textName.TabIndex = 9;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point( 10, 65 );
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size( 34, 12 );
            this.labelName.TabIndex = 133;
            this.labelName.Text = "Name";
            // 
            // groupPreview
            // 
            this.groupPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPreview.Controls.Add( this.splitContainer2 );
            this.groupPreview.Location = new System.Drawing.Point( 183, 111 );
            this.groupPreview.Name = "groupPreview";
            this.groupPreview.Size = new System.Drawing.Size( 316, 213 );
            this.groupPreview.TabIndex = 142;
            this.groupPreview.TabStop = false;
            this.groupPreview.Text = "Preview";
            // 
            // listPresets
            // 
            this.listPresets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listPresets.FormattingEnabled = true;
            this.listPresets.ItemHeight = 12;
            this.listPresets.Location = new System.Drawing.Point( 12, 30 );
            this.listPresets.Name = "listPresets";
            this.listPresets.Size = new System.Drawing.Size( 165, 232 );
            this.listPresets.TabIndex = 1;
            // 
            // textDepth
            // 
            this.textDepth.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(240)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))) );
            this.textDepth.ForeColor = System.Drawing.Color.White;
            this.textDepth.Location = new System.Drawing.Point( 86, 37 );
            this.textDepth.Name = "textDepth";
            this.textDepth.Size = new System.Drawing.Size( 72, 19 );
            this.textDepth.TabIndex = 8;
            this.textDepth.Type = org.kbinani.cadencii.NumberTextBox.ValueType.Integer;
            // 
            // textRate
            // 
            this.textRate.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(240)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))) );
            this.textRate.ForeColor = System.Drawing.Color.White;
            this.textRate.Location = new System.Drawing.Point( 86, 12 );
            this.textRate.Name = "textRate";
            this.textRate.Size = new System.Drawing.Size( 72, 19 );
            this.textRate.TabIndex = 7;
            this.textRate.Type = org.kbinani.cadencii.NumberTextBox.ValueType.Integer;
            // 
            // FormVibratoPreset
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 96F, 96F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size( 511, 380 );
            this.Controls.Add( this.listPresets );
            this.Controls.Add( this.groupPreview );
            this.Controls.Add( this.groupEdit );
            this.Controls.Add( this.labelPresets );
            this.Controls.Add( this.buttonRemove );
            this.Controls.Add( this.buttonAdd );
            this.Controls.Add( this.buttonUp );
            this.Controls.Add( this.buttonDown );
            this.Controls.Add( this.buttonOk );
            this.Controls.Add( this.buttonCancel );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVibratoPreset";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Vibrato preset";
            ((System.ComponentModel.ISupportInitialize)(this.pictureRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDepth)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.pictureResulting)).EndInit();
            this.groupEdit.ResumeLayout( false );
            this.groupEdit.PerformLayout();
            this.groupPreview.ResumeLayout( false );
            this.ResumeLayout( false );
            this.PerformLayout();

        }
        #endregion

        private BButton buttonCancel;
        private BButton buttonOk;
        private org.kbinani.windows.forms.BButton buttonRemove;
        private org.kbinani.windows.forms.BButton buttonAdd;
        private org.kbinani.windows.forms.BButton buttonUp;
        private org.kbinani.windows.forms.BLabel labelRate;
        private org.kbinani.windows.forms.BLabel labelDepth;
        private NumberTextBox textRate;
        private NumberTextBox textDepth;
        private BLabel labelPresets;
        private BPictureBox pictureRate;
        private BLabel labelRateCurve;
        private BLabel labelDepthCurve;
        private BPictureBox pictureDepth;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private BLabel labelResulting;
        private BPictureBox pictureResulting;
        private BGroupBox groupEdit;
        private BLabel labelName;
        private BTextBox textName;
        private BGroupBox groupPreview;
        private BListBox listPresets;
        private org.kbinani.windows.forms.BButton buttonDown;
        #endregion
#endif
    }

#if !JAVA
}
#endif
