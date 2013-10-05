/*
 * WaveformZoomController.cs
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
using cadencii.java.awt;

namespace cadencii
{

    class WaveformZoomController : ControllerBase, WaveformZoomUiListener
    {
        /// <summary>
        /// 波形表示部の拡大ボタン上でマウスが下りた状態かどうか
        /// </summary>
        private bool mWaveViewButtonZoomMouseDowned = false;
        /// <summary>
        /// 波形表示部のAutoMaximizeボタン上でマウスが下りた状態かどうか
        /// </summary>
        private bool mWaveViewButtonAutoMaximizeMouseDowned = false;
        /// <summary>
        /// 波形表示部の縦軸の拡大率を自動最大化するかどうか
        /// </summary>
        private bool mWaveViewAutoMaximize = false;
        /// <summary>
        /// 波形表示部分のズーム時に，マウスが下りた瞬間のY座標
        /// </summary>
        private int mWaveViewMouseDownedLocationY;
        /// <summary>
        /// 波形表示部の拡大ボタン上でマウスが下りた瞬間の，波形表示部の縦軸拡大率．
        /// </summary>
        private float mWaveViewInitScale;

        private WaveView mWaveView = null;
        private FormMain mFormMain = null;
        private WaveformZoomUi mUi = null;

        /// <summary>
        /// Wave表示部等のボタンと他のコンポーネントの間のスペース
        /// </summary>
        const int SPACE = 4;

        public WaveformZoomController(FormMain form_main, WaveView wave_view)
        {
            mWaveView = wave_view;
            mFormMain = form_main;

            mUi = (WaveformZoomUi)new WaveformZoomUiImpl();
            mUi.setListener(this);
        }

        public void refreshScreen()
        {
            mFormMain.refreshScreen();
        }

        public void setAutoMaximize(bool value)
        {
            mWaveView.setAutoMaximize(value);
        }

        public float getScale()
        {
            return mWaveView.getScale();
        }

        public void setScale(float value)
        {
            mWaveView.setScale(value);
        }

        public WaveformZoomUi getUi()
        {
            return mUi;
        }

        public void receivePaintSignal(Graphics g)
        {
            int key_width = AppManager.keyWidth;
            int width = key_width - 1;
            int height = mUi.getHeight() - 1;

            // 背景を塗る
            g.setColor(PortUtil.DarkGray);
            g.fillRect(0, 0, width, height);

            // AutoMaximizeのチェックボックスを描く
            g.setColor(mWaveViewButtonAutoMaximizeMouseDowned ? PortUtil.Gray : PortUtil.LightGray);
            g.fillRect(SPACE, SPACE, 16, 16);
            g.setColor(PortUtil.Gray);
            g.drawRect(SPACE, SPACE, 16, 16);
            if (mWaveViewAutoMaximize) {
                g.setColor(PortUtil.Gray);
                g.fillRect(SPACE + 3, SPACE + 3, 11, 11);
            }
            g.setColor(Color.black);
            g.setFont(AppManager.baseFont8);
            g.drawString(
                "Auto Maximize",
                SPACE + 16 + SPACE,
                SPACE + AppManager.baseFont8Height / 2 - AppManager.baseFont8OffsetHeight + 1);

            // ズーム用ボタンを描く
            int zoom_button_y = SPACE + 16 + SPACE;
            int zoom_button_height = height - SPACE - zoom_button_y;
            Rectangle rc = getButtonBoundsWaveViewZoom();
            if (!mWaveViewAutoMaximize) {
                g.setColor(mWaveViewButtonZoomMouseDowned ? PortUtil.Gray : PortUtil.LightGray);
                g.fillRect(rc.x, rc.y, rc.width, rc.height);
            }
            g.setColor(PortUtil.Gray);
            g.drawRect(rc.x, rc.y, rc.width, rc.height);
            g.setColor(mWaveViewAutoMaximize ? PortUtil.Gray : Color.black);
            rc.y = rc.y + 1;
            PortUtil.drawStringEx(
                g, (mWaveViewButtonZoomMouseDowned ? "↑Move Mouse↓" : "Zoom"), AppManager.baseFont9,
                rc, PortUtil.STRING_ALIGN_CENTER, PortUtil.STRING_ALIGN_CENTER);
        }

        public void receiveMouseDownSignal(int x, int y)
        {
            Point p = new Point(x, y);

            int width = AppManager.keyWidth - 1;
            int height = mUi.getHeight();

            // AutoMaximizeボタン
            Rectangle rc = new Rectangle(SPACE, SPACE, width - SPACE - SPACE, 16);
            if (Utility.isInRect(p, rc)) {
                mWaveViewButtonAutoMaximizeMouseDowned = true;
                mWaveViewButtonZoomMouseDowned = false;

                mUi.repaint();
                return;
            }

            if (!mWaveViewAutoMaximize) {
                // Zoomボタン
                rc = getButtonBoundsWaveViewZoom();
                if (Utility.isInRect(p, rc)) {
                    mWaveViewMouseDownedLocationY = p.y;
                    mWaveViewButtonZoomMouseDowned = true;
                    mWaveViewButtonAutoMaximizeMouseDowned = false;
                    mWaveViewInitScale = mWaveView.getScale();

                    mUi.repaint();
                    return;
                }
            }

            mWaveViewButtonAutoMaximizeMouseDowned = false;
            mWaveViewButtonZoomMouseDowned = false;
            mUi.repaint();
        }

        public void receiveMouseMoveSignal(int x, int y)
        {
            if (!mWaveViewButtonZoomMouseDowned) {
                return;
            }

            int height = mUi.getHeight();
            int delta = mWaveViewMouseDownedLocationY - y;
            float scale = mWaveViewInitScale + delta * 3.0f / height * mWaveViewInitScale;
            mWaveView.setScale(scale);

            mFormMain.refreshScreen();
        }

        public void receiveMouseUpSignal(int x, int y)
        {
            int width = AppManager.keyWidth - 1;
            int height = mUi.getHeight();

            // AutoMaximizeボタン
            if (Utility.isInRect(x, y, SPACE, SPACE, width - SPACE - SPACE, 16)) {
                if (mWaveViewButtonAutoMaximizeMouseDowned) {
                    mWaveViewAutoMaximize = !mWaveViewAutoMaximize;
                    mWaveView.setAutoMaximize(mWaveViewAutoMaximize);
                }
            }

            mWaveViewButtonAutoMaximizeMouseDowned = false;
            mWaveViewButtonZoomMouseDowned = false;
            mUi.repaint();
        }

        /// <summary>
        /// 波形表示部のズームボタンの形を取得します
        /// </summary>
        /// <returns></returns>
        private Rectangle getButtonBoundsWaveViewZoom()
        {
            int width = AppManager.keyWidth - 1;
            int height = mUi.getHeight() - 1;

            int y = SPACE + 16 + SPACE;
            return new Rectangle(SPACE, y, width - SPACE - SPACE, height - SPACE - y);
        }
    }

}
