/*
 * FormMainController.cs
 * Copyright © 2011 kbinani
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

namespace cadencii
{


    /// <summary>
    /// メイン画面のコントローラ
    /// </summary>
    public class FormMainController : ControllerBase, FormMainUiListener
    {
        /// <summary>
        /// x方向の表示倍率(pixel/clock)
        /// </summary>
        private float mScaleX;

        /// <summary>
        /// mScaleXの逆数
        /// </summary>
        private float mInvScaleX;

        /// <summary>
        /// 画面左端位置での、仮想画面上の画面左端から測ったピクセル数．
        /// FormMain.hScroll.ValueとFormMain.trackBar.Valueで決まる．
        /// </summary>
        private int mStartToDrawX;

        /// <summary>
        /// 画面上端位置での、仮想画面上の画面上端から図ったピクセル数．
        /// FormMain.vScroll.Value，FormMain.vScroll.Height，FormMain.vScroll.Maximum,AppManager.editorConfig.PxTrackHeightによって決まる
        /// </summary>
        private int mStartToDrawY;

        /// <summary>
        /// MIDIステップ入力モードがONかどうか
        /// </summary>
        private bool mStepSequencerEnabled = false;

        private FormMainUi ui;

        public FormMainController()
        {
            mScaleX = 0.1f;
            mInvScaleX = 1.0f / mScaleX;
        }

        #region FormMainUiListenerの実装

        public void navigationPanelGotFocus()
        {
            ui.focusPianoRoll();
        }

        #endregion

        /// <summary>
        /// MIDIステップ入力モードがONかどうかを取得します
        /// </summary>
        /// <returns></returns>
        public bool isStepSequencerEnabled()
        {
            return mStepSequencerEnabled;
        }

        /// <summary>
        /// MIDIステップ入力モードがONかどうかを設定する
        /// </summary>
        /// <param name="value"></param>
        public void setStepSequencerEnabled(bool value)
        {
            mStepSequencerEnabled = value;
        }

        public void setupUi(FormMainUi ui)
        {
            this.ui = ui;
        }

        /// <summary>
        /// ピアノロールの，X方向のスケールを取得します(pixel/clock)
        /// </summary>
        /// <returns></returns>
        public float getScaleX()
        {
            return mScaleX;
        }

        /// <summary>
        /// ピアノロールの，X方向のスケールの逆数を取得します(clock/pixel)
        /// </summary>
        /// <returns></returns>
        public float getScaleXInv()
        {
            return mInvScaleX;
        }

        /// <summary>
        /// ピアノロールの，X方向のスケールを設定します
        /// </summary>
        /// <param name="scale_x"></param>
        public void setScaleX(float scale_x)
        {
            mScaleX = scale_x;
            mInvScaleX = 1.0f / mScaleX;
        }

        /// <summary>
        /// ピアノロールの，Y方向のスケールを取得します(pixel/cent)
        /// </summary>
        /// <returns></returns>
        public float getScaleY()
        {
            if (AppManager.editorConfig.PianoRollScaleY < EditorConfig.MIN_PIANOROLL_SCALEY) {
                AppManager.editorConfig.PianoRollScaleY = EditorConfig.MIN_PIANOROLL_SCALEY;
            } else if (EditorConfig.MAX_PIANOROLL_SCALEY < AppManager.editorConfig.PianoRollScaleY) {
                AppManager.editorConfig.PianoRollScaleY = EditorConfig.MAX_PIANOROLL_SCALEY;
            }
            if (AppManager.editorConfig.PianoRollScaleY == 0) {
                return AppManager.editorConfig.PxTrackHeight / 100.0f;
            } else if (AppManager.editorConfig.PianoRollScaleY > 0) {
                return (2 * AppManager.editorConfig.PianoRollScaleY + 5) * AppManager.editorConfig.PxTrackHeight / 5 / 100.0f;
            } else {
                return (AppManager.editorConfig.PianoRollScaleY + 8) * AppManager.editorConfig.PxTrackHeight / 8 / 100.0f;
            }
        }

        /// <summary>
        /// ピアノロール画面の，ビューポートと仮想スクリーンとの横方向のオフセットを取得します
        /// </summary>
        /// <returns></returns>
        public int getStartToDrawX()
        {
            return mStartToDrawX;
        }

        /// <summary>
        /// ピアノロール画面の，ビューポートと仮想スクリーンとの横方向のオフセットを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setStartToDrawX(int value)
        {
            mStartToDrawX = value;
        }

        /// <summary>
        /// ピアノロール画面の，ビューポートと仮想スクリーンとの縦方向のオフセットを取得します
        /// </summary>
        /// <returns></returns>
        public int getStartToDrawY()
        {
            return mStartToDrawY;
        }

        /// <summary>
        /// ピアノロール画面の，ビューポートと仮想スクリーンとの縦方向のオフセットを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setStartToDrawY(int value)
        {
            mStartToDrawY = value;
        }
    }

}
