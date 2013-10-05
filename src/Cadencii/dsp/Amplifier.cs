/*
 * Amplifier.cs
 * Copyright © 2010-2011 kbinani
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
using cadencii.java.util;
using cadencii.java.awt;

namespace cadencii
{


    /// <summary>
    /// 増幅器の実装
    /// </summary>
    public class Amplifier : WaveUnit, WaveSender, WaveReceiver
    {
        private const int _BUFLEN = 1024;
        private double[] mBufferL = new double[_BUFLEN];
        private double[] mBufferR = new double[_BUFLEN];
        private double mAmpL = 1.0;
        private double mAmpR = 1.0;
        private long mPosition = 0;
        private WaveReceiver mReceiver = null;
        private WaveSender mSender = null;
        private int mVersion = 0;
        private BasicStroke mStroke = null;
        private IAmplifierView mView = null;

        public void setAmplifierView(IAmplifierView view)
        {
            mView = view;
        }

        public override void paintTo(Graphics2D graphics, int x, int y, int width, int height)
        {
            // 現在の描画時のストローク、色を保存しておく
            Stroke old_stroke = graphics.getStroke();
            Color old_color = graphics.getColor();

            // 描画用のストロークが初期化してなかったら初期化
            if (mStroke == null) {
                mStroke = new BasicStroke();
            }

            // 枠と背景を描画
            paintBackground(graphics, mStroke, x, y, width, height, Color.black, PortUtil.Pink);

            // デバイス名を書く
            PortUtil.drawStringEx(
                (Graphics)graphics, "Amplifier", AppManager.baseFont10,
                new Rectangle(x, y, width, height),
                PortUtil.STRING_ALIGN_CENTER, PortUtil.STRING_ALIGN_CENTER);

            // 描画時のストローク、色を元に戻す
            graphics.setStroke(old_stroke);
            graphics.setColor(old_color);
        }

        public override int getVersion()
        {
            return mVersion;
        }

        public override void setConfig(string parameter)
        {
            // do nothing (ı _ ı )
        }

        public void setReceiver(WaveReceiver r)
        {
            if (mReceiver != null) {
                mReceiver.end();
            }
            mReceiver = r;
        }

        public void setSender(WaveSender s)
        {
            if (mSender != null) {
                mSender.end();
            }
            mSender = s;
        }

        public long getPosition()
        {
            return mPosition;
        }

        public void end()
        {
            if (mReceiver != null) {
                mReceiver.end();
            }
            if (mSender != null) {
                mSender.end();
            }
        }

        public void setAmplify(double amp_left, double amp_right)
        {
            mAmpL = amp_left;
            mAmpR = amp_right;
        }

        public void push(double[] l, double[] r, int length)
        {
            if (mReceiver == null) {
                mPosition += length;
                return;
            }

            int remain = length;
            while (remain > 0) {
                int amount = (remain > _BUFLEN) ? _BUFLEN : remain;
                for (int i = 0; i < amount; i++) {
                    mBufferL[i] = 0.0;
                    mBufferR[i] = 0.0;
                }
                int offset = length - remain;
                if (mView != null) {
                    mAmpL = mView.getAmplifyL();
                    mAmpR = mView.getAmplifyR();
                }

                // 左チャンネル
                if (mAmpL != 0.0) {
                    if (mAmpL == 1.0) {
                        // 増幅率1の場合
                        for (int i = 0; i < amount; i++) {
                            mBufferL[i] = l[i + offset];
                        }
                    } else {
                        for (int i = 0; i < amount; i++) {
                            mBufferL[i] = l[i + offset] * mAmpL;
                        }
                    }
                    for (int i = 0; i < amount; i++) {
                        if (mBufferL[i] > 1.0) {
                            mBufferL[i] = 1.0;
                        } else if (mBufferL[i] < -1.0) {
                            mBufferL[i] = -1.0;
                        }
                    }
                }

                // 右チャンネル
                if (mAmpR != 0.0) {
                    if (mAmpR == 1.0) {
                        // 増幅率1の場合
                        for (int i = 0; i < amount; i++) {
                            mBufferR[i] = r[i + offset];
                        }
                    } else {
                        for (int i = 0; i < amount; i++) {
                            mBufferR[i] = r[i + offset] * mAmpR;
                        }
                    }
                    for (int i = 0; i < amount; i++) {
                        if (mBufferR[i] > 1.0) {
                            mBufferR[i] = 1.0;
                        } else if (mBufferR[i] < -1.0) {
                            mBufferR[i] = -1.0;
                        }
                    }
                }

                remain -= amount;
                mPosition += amount;
                mReceiver.push(mBufferL, mBufferR, amount);
            }
        }

        public void pull(double[] l, double[] r, int length)
        {
            for (int i = 0; i < length; i++) {
                r[i] = 0.0;
                l[i] = 0.0;
            }
            int remain = length;
            while (remain > 0) {
                int amount = (remain > _BUFLEN) ? _BUFLEN : remain;
                int offset = length - remain;
                mSender.pull(mBufferL, mBufferR, amount);
                if (mView != null) {
                    mAmpL = mView.getAmplifyL();
                    mAmpR = mView.getAmplifyR();
                }
                for (int i = 0; i < amount; i++) {
                    l[i + offset] += mBufferL[i] * mAmpL;
                    r[i + offset] += mBufferR[i] * mAmpR;
                }
                remain -= amount;
            }
            for (int i = 0; i < length; i++) {
                if (l[i] > 1.0) {
                    l[i] = 1.0;
                } else if (l[i] < -1.0) {
                    l[i] = -1.0;
                }
                if (r[i] > 1.0) {
                    r[i] = 1.0;
                } else if (r[i] < -1.0) {
                    r[i] = -1.0;
                }
            }
        }
    }

}
