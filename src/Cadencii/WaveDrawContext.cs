/*
 * WaveDrawContext.cs
 * Copyright © 2009-2011 kbinani
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
using cadencii.media;
using cadencii;
using cadencii.java.awt;
using cadencii.java.util;
using cadencii.vsq;
using cadencii.apputil;



namespace cadencii.new_
{

    public class WaveDrawContext : IDisposable
    {
        private short[] mEnvOut = null;

        public void load(string file)
        {

        }

        public void dispose()
        {
        }

        /// <summary>
        /// このWAVE描画コンテキストが保持しているWAVEデータを、ゲートタイム基準でグラフィクスに描画します。
        /// 縦軸の拡大率は引数<paramref name="scale_y"/>で指定します。
        /// </summary>
        /// <param name="g">描画に使用するグラフィクスオブジェクト</param>
        /// <param name="pen">描画に使用するペン</param>
        /// <param name="rect">描画範囲</param>
        /// <param name="clock_start">描画開始位置のゲートタイム</param>
        /// <param name="clock_end">描画終了位置のゲートタイム</param>
        /// <param name="tempo_table">ゲートタイムから秒数を調べる際使用するテンポ・テーブル</param>
        /// <param name="pixel_per_clock">ゲートタイムあたりの秒数</param>
        /// <param name="scale_y">Y軸方向の描画スケール。デフォルトは1.0</param>
        public void draw(
            Graphics2D g,
            Color pen,
            Rectangle rect,
            int clock_start,
            int clock_end,
            TempoVector tempo_table,
            float pixel_per_clock,
            float scale_y)
        {
            drawCore(g, pen, rect, clock_start, clock_end, tempo_table, pixel_per_clock, scale_y, false);
        }

        /// <summary>
        /// このWAVE描画コンテキストが保持しているWAVEデータを、ゲートタイム基準でグラフィクスに描画します。
        /// 縦軸は最大振幅がちょうど描画範囲に収まるよう調節されます。
        /// </summary>
        /// <param name="g">描画に使用するグラフィクスオブジェクト</param>
        /// <param name="pen">描画に使用するペン</param>
        /// <param name="rect">描画範囲</param>
        /// <param name="clock_start">描画開始位置のゲートタイム</param>
        /// <param name="clock_end">描画終了位置のゲートタイム</param>
        /// <param name="tempo_table">ゲートタイムから秒数を調べる際使用するテンポ・テーブル</param>
        /// <param name="pixel_per_clock">ゲートタイムあたりの秒数</param>
        public void draw(
            Graphics2D g,
            Color pen,
            Rectangle rect,
            int clock_start,
            int clock_end,
            TempoVector tempo_table,
            float pixel_per_clock)
        {
            drawCore(g, pen, rect, clock_start, clock_end, tempo_table, pixel_per_clock, 1.0f, true);
        }

        /// <summary>
        /// このWAVE描画コンテキストが保持しているWAVEデータを、ゲートタイム基準でグラフィクスに描画します。
        /// </summary>
        /// <param name="g">描画に使用するグラフィクスオブジェクト</param>
        /// <param name="pen">描画に使用するペン</param>
        /// <param name="rect">描画範囲</param>
        /// <param name="clock_start">描画開始位置のゲートタイム</param>
        /// <param name="clock_end">描画終了位置のゲートタイム</param>
        /// <param name="tempo_table">ゲートタイムから秒数を調べる際使用するテンポ・テーブル</param>
        /// <param name="pixel_per_clock">ゲートタイムあたりの秒数</param>
        /// <param name="scale_y">Y軸方向の描画スケール。デフォルトは1.0</param>
        /// <param name="auto_maximize">自動で最大化するかどうか</param>
        private void drawCore(
            Graphics2D g,
            Color pen,
            Rectangle rect,
            int clock_start,
            int clock_end,
            TempoVector tempo_table,
            float pixel_per_clock,
            float scale_y,
            bool auto_maximize)
        {
        }

        public void Dispose()
        {
            dispose();
        }
    }

}

namespace cadencii
{

    /// <summary>
    /// WAVEファイルのデータをグラフィクスに書き込む操作を行うクラス
    /// </summary>
    public class WaveDrawContext : IDisposable
    {
        private sbyte[] mWave;
        private int mSampleRate = 44100;
        private string mName;
        private float mLength;
        private PolylineDrawer mDrawer = null;
        private float mMaxAmplitude = 0.0f;
        private float mActualMaxAmplitude = 0.0f;

        /// <summary>
        /// 読み込むWAVEファイルを指定したコンストラクタ。初期化と同時にWAVEファイルの読込みを行います。
        /// </summary>
        /// <param name="file">読み込むWAVEファイルのパス</param>
        public WaveDrawContext(string file)
        {
            load(file);
            mDrawer = new PolylineDrawer(null, 1024);
        }

        /// <summary>
        /// デフォルトのコンストラクタ。
        /// </summary>
        public WaveDrawContext()
        {
            mWave = new sbyte[0];
            mLength = 0.0f;
            mDrawer = new PolylineDrawer(null, 1024);
        }

        /// <summary>
        /// 保持しているWAVEデータを破棄します。
        /// </summary>
        public void unload()
        {
            mDrawer.clear();
            mWave = new sbyte[0];
            mLength = 0.0f;
        }

        /// <summary>
        /// 指定したファイルの指定した区間を追加で読み込みます
        /// </summary>
        /// <param name="file"></param>
        /// <param name="sec_from"></param>
        /// <param name="sec_to"></param>
        public void reloadPartial(string file, double sec_from, double sec_to)
        {
            if (!System.IO.File.Exists(file)) {
                return;
            }

            WaveRateConverter wr = null;
            try {
                wr = new WaveRateConverter(new WaveReader(file), mSampleRate);
                int saFrom = (int)(sec_from * mSampleRate);
                int saTo = (int)(sec_to * mSampleRate);

                // バッファを確保
                int buflen = 1024;
                double[] left = new double[buflen];
                double[] right = new double[buflen];

                // まず、読み込んだ区間の最大振幅を調べる
                int remain = saTo - saFrom;
                int pos = saFrom;
                double max = 0.0;
                while (remain > 0) {
                    int delta = remain > buflen ? buflen : remain;
                    wr.read(pos, delta, left, right);
                    for (int i = 0; i < delta; i++) {
                        double d = Math.Abs((left[i] + right[i]) * 0.5);
                        max = d > max ? d : max;
                    }
                    remain -= delta;
                    pos += delta;
                }

                // バッファが足りなければ確保
                int oldLength = mWave.Length;
                if (oldLength < saTo) {
                    Array.Resize(ref mWave, saTo);
                    saFrom = oldLength;
                }

                if (mMaxAmplitude < max) {
                    // 既存の波形の最大振幅より、読み込み部分の最大波形が大きいようなら、
                    // 既存波形の縮小を行う
                    double ampall = 1.0 / max;
                    for (int i = 0; i < mWave.Length; i++) {
                        double vold = mWave[i] / 127.0 * mMaxAmplitude;
                        double vnew = vold * ampall;
                        mWave[i] = (sbyte)(vnew * 127);
                    }

                }

                // 最大振幅の値を更新
                mMaxAmplitude = (max > mMaxAmplitude) ? (float)max : mMaxAmplitude;

                // 今度は波形を取得するために読み込む
                double amp = (mMaxAmplitude > 0.0f) ? (1.0 / mMaxAmplitude) : 0.0;
                remain = saTo - saFrom;
                pos = saFrom;
                while (remain > 0) {
                    int delta = remain > buflen ? buflen : remain;
                    wr.read(pos, delta, left, right);

                    for (int i = 0; i < delta; i++) {
                        double d = (left[i] + right[i]) * 0.5 * amp;
                        sbyte b = (sbyte)(d * 127);
                        mWave[pos + i] = b;
                    }

                    pos += delta;
                    remain -= delta;
                }
                left = null;
                right = null;

                // mActualMaxAmplitudeの値を更新
                mActualMaxAmplitude = 0.0f;
                for (int i = 0; i < mWave.Length; i++) {
                    double d = Math.Abs(mWave[i] / 127.0 * mMaxAmplitude);
                    mActualMaxAmplitude = (d > mActualMaxAmplitude) ? (float)d : mActualMaxAmplitude;
                }
            } catch (Exception ex) {
                serr.println("WaveDrawContext#reloadPartial; ex=" + ex);
            } finally {
                if (wr != null) {
                    try {
                        wr.close();
                    } catch (Exception ex2) {
                        serr.println("WaveDrawContext#reloadPartial; ex2=" + ex2);
                    }
                }
            }
        }

        /// <summary>
        /// WAVEファイルを読み込みます。
        /// </summary>
        /// <param name="file">読み込むWAVEファイルのパス</param>
        public void load(string file)
        {
            if (!System.IO.File.Exists(file)) {
                mWave = new sbyte[0];
                mLength = 0.0f;
                return;
            }

            Wave wr = null;
            try {
                wr = new Wave(file);
                int len = (int)wr.getTotalSamples();
                mWave = new sbyte[len];
                mSampleRate = (int)wr.getSampleRate();
                mLength = wr.getTotalSamples() / (float)wr.getSampleRate();
                int count = (int)wr.getTotalSamples();

                // 最大振幅を検出
                double max = 0.0;
                for (int i = 0; i < count; i++) {
                    double b = Math.Abs(wr.getDouble(i));
                    max = b > max ? b : max;
                }

                // 最大振幅の値を更新
                mMaxAmplitude = (float)max;
                mActualMaxAmplitude = mMaxAmplitude;

                // 波形を読み込む
                double amp = (max > 0.0) ? (1.0 / max) : 0.0;
                for (int i = 0; i < count; i++) {
                    double b = wr.getDouble(i) * amp;
                    mWave[i] = (sbyte)(127 * b);
                }
            } catch (Exception ex) {
            } finally {
                if (wr != null) {
                    try {
                        wr.dispose();
                    } catch (Exception ex2) {
                    }
                }
            }
            if (mWave == null) {
                mWave = new sbyte[0];
                mSampleRate = 44100;
                mLength = 0.0f;
            }
        }

        /// <summary>
        /// このWAVE描画コンテキストの名前を取得します。
        /// </summary>
        /// <returns>この描画コンテキストの名前</returns>
        public string getName()
        {
            return mName;
        }

        /// <summary>
        /// このWAVE描画コンテキストの名前を設定します。
        /// </summary>
        /// <param name="value">この描画コンテキストの名前</param>
        public void setName(string value)
        {
            mName = value;
        }

        /// <summary>
        /// このWAVE描画コンテキストが保持しているWAVEデータの、秒数を取得します。
        /// </summary>
        /// <returns>保持しているWAVEデータの長さ(秒)</returns>
        public float getLength()
        {
            return mLength;
        }

        /// <summary>
        /// デストラクタ。disposeメソッドを呼び出します。
        /// </summary>
        ~WaveDrawContext()
        {
            dispose();
        }

        /// <summary>
        /// このWAVE描画コンテキストが使用しているリソースを開放します。
        /// </summary>
        public void Dispose()
        {
            dispose();
        }

        /// <summary>
        /// このWAVE描画コンテキストが使用しているリソースを開放します。
        /// </summary>
        public void dispose()
        {
            mWave = null;
            GC.Collect();
        }

        /// <summary>
        /// このWAVE描画コンテキストが保持しているWAVEデータを、ゲートタイム基準でグラフィクスに描画します。
        /// 縦軸の拡大率は引数<paramref name="scale_y"/>で指定します。
        /// </summary>
        /// <param name="g">描画に使用するグラフィクスオブジェクト</param>
        /// <param name="pen">描画に使用するペン</param>
        /// <param name="rect">描画範囲</param>
        /// <param name="clock_start">描画開始位置のゲートタイム</param>
        /// <param name="clock_end">描画終了位置のゲートタイム</param>
        /// <param name="tempo_table">ゲートタイムから秒数を調べる際使用するテンポ・テーブル</param>
        /// <param name="pixel_per_clock">ゲートタイムあたりの秒数</param>
        /// <param name="scale_y">Y軸方向の描画スケール。デフォルトは1.0</param>
        public void draw(
            Graphics2D g,
            Color pen,
            Rectangle rect,
            int clock_start,
            int clock_end,
            TempoVector tempo_table,
            float pixel_per_clock,
            float scale_y)
        {
            drawCore(g, pen, rect, clock_start, clock_end, tempo_table, pixel_per_clock, scale_y, false);
        }

        /// <summary>
        /// このWAVE描画コンテキストが保持しているWAVEデータを、ゲートタイム基準でグラフィクスに描画します。
        /// 縦軸は最大振幅がちょうど描画範囲に収まるよう調節されます。
        /// </summary>
        /// <param name="g">描画に使用するグラフィクスオブジェクト</param>
        /// <param name="pen">描画に使用するペン</param>
        /// <param name="rect">描画範囲</param>
        /// <param name="clock_start">描画開始位置のゲートタイム</param>
        /// <param name="clock_end">描画終了位置のゲートタイム</param>
        /// <param name="tempo_table">ゲートタイムから秒数を調べる際使用するテンポ・テーブル</param>
        /// <param name="pixel_per_clock">ゲートタイムあたりの秒数</param>
        public void draw(
            Graphics2D g,
            Color pen,
            Rectangle rect,
            int clock_start,
            int clock_end,
            TempoVector tempo_table,
            float pixel_per_clock)
        {
            drawCore(g, pen, rect, clock_start, clock_end, tempo_table, pixel_per_clock, 1.0f, true);
        }

        /// <summary>
        /// このWAVE描画コンテキストが保持しているWAVEデータを、ゲートタイム基準でグラフィクスに描画します。
        /// </summary>
        /// <param name="g">描画に使用するグラフィクスオブジェクト</param>
        /// <param name="pen">描画に使用するペン</param>
        /// <param name="rect">描画範囲</param>
        /// <param name="clock_start">描画開始位置のゲートタイム</param>
        /// <param name="clock_end">描画終了位置のゲートタイム</param>
        /// <param name="tempo_table">ゲートタイムから秒数を調べる際使用するテンポ・テーブル</param>
        /// <param name="pixel_per_clock">ゲートタイムあたりの秒数</param>
        /// <param name="scale_y">Y軸方向の描画スケール。デフォルトは1.0</param>
        /// <param name="auto_maximize">自動で最大化するかどうか</param>
        private void drawCore(
            Graphics2D g,
            Color pen,
            Rectangle rect,
            int clock_start,
            int clock_end,
            TempoVector tempo_table,
            float pixel_per_clock,
            float scale_y,
            bool auto_maximize)
        {
            if (mWave.Length == 0) {
                return;
            }
#if DEBUG
            double startedTime = PortUtil.getCurrentTime();
#endif
            mDrawer.setGraphics(g);
            mDrawer.clear();
            double secStart = tempo_table.getSecFromClock(clock_start);
            double secEnd = tempo_table.getSecFromClock(clock_end);
            int sStart0 = (int)(secStart * mSampleRate) - 1;
            int sEnd0 = (int)(secEnd * mSampleRate) + 1;

            int count = tempo_table.Count;
            int sStart = 0;
            double cStart = 0.0;
            float order_y = 1.0f;
            if (auto_maximize) {
                order_y = rect.height / 2.0f / 127.0f * mMaxAmplitude / mActualMaxAmplitude;
            } else {
                order_y = rect.height / 127.0f * scale_y * mMaxAmplitude;
            }
            int ox = rect.x;
            int oy = rect.height / 2;
            int last = mWave[0];
            int lastx = ox;
            int lastYMax = oy - (int)(last * order_y);
            int lastYMin = lastYMax;
            int lasty = lastYMin;
            int lasty2 = lastYMin;
            bool skipped = false;
            mDrawer.append(ox, lasty);
            int xmax = rect.x + rect.width;
            int lastTempo = 500000;
            for (int i = 0; i <= count; i++) {
                double time = 0.0;
                int tempo = 500000;
                int cEnd = 0;
                if (i < count) {
                    TempoTableEntry entry = tempo_table[i];
                    time = entry.Time;
                    tempo = entry.Tempo;
                    cEnd = entry.Clock;
                } else {
                    time = tempo_table.getSecFromClock(clock_end);
                    tempo = tempo_table[i - 1].Tempo;
                    cEnd = clock_end;
                }
                int sEnd = (int)(time * mSampleRate);

                // sStartサンプルからsThisEndサンプルまでを描画する(必要なら!)
                if (sEnd < sStart0) {
                    sStart = sEnd;
                    cStart = cEnd;
                    lastTempo = tempo;
                    continue;
                }
                if (sEnd0 < sStart) {
                    break;
                }

                // 
                int xoffset = (int)(cStart * pixel_per_clock) - AppManager.mMainWindowController.getStartToDrawX() + AppManager.keyOffset;
                double sec_per_clock = lastTempo * 1e-6 / 480.0;
                lastTempo = tempo;
                double pixel_per_sample = 1.0 / mSampleRate / sec_per_clock * pixel_per_clock;
                int j0 = sStart;
                if (j0 < 0) {
                    j0 = 0;
                }
                int j1 = sEnd;
                if (mWave.Length < j1) {
                    j1 = mWave.Length;
                }

                // 第j0サンプルのデータを画面に描画したときのx座標がいくらになるか？
                int draftStartX = xoffset + (int)((j0 - sStart) * pixel_per_sample);
                if (draftStartX < rect.x) {
                    j0 = (int)((rect.x - xoffset) / pixel_per_sample) + sStart;
                }
                // 第j1サンプルのデータを画面に描画した時のx座標がいくらになるか？
                int draftEndX = xoffset + (int)((j1 - sStart) * pixel_per_sample);
                if (rect.x + rect.width < draftEndX) {
                    j1 = (int)((rect.x + rect.width - xoffset) / pixel_per_sample) + sStart;
                }

                bool breakRequired = false;
                for (int j = j0; j < j1; j++) {
                    int v = mWave[j];
                    if (v == last) {
                        skipped = true;
                        continue;
                    }
                    int x = xoffset + (int)((j - sStart) * pixel_per_sample);
                    if (xmax < x) {
                        breakRequired = true;
                        break;
                    }
                    if (x < rect.x) {
                        continue;
                    }
                    int y = oy - (int)(v * order_y);
                    if (lastx == x) {
                        lastYMax = Math.Max(lastYMax, y);
                        lastYMin = Math.Min(lastYMin, y);
                        continue;
                    }

                    if (skipped) {
                        mDrawer.append(x - 1, lasty);
                        lastx = x - 1;
                    }
                    if (lastYMax == lastYMin) {
                        mDrawer.append(x, y);
                    } else {
                        if (lasty2 != lastYMin) {
                            mDrawer.append(lastx, lastYMin);
                        }
                        mDrawer.append(lastx, lastYMax);
                        if (lastYMax != lasty) {
                            mDrawer.append(lastx, lasty);
                        }
                        mDrawer.append(x, y);
                    }
                    lasty2 = lasty;
                    lastx = x;
                    lastYMin = y;
                    lastYMax = y;
                    lasty = y;
                    last = v;
                    skipped = false;
                }
                sStart = sEnd;
                cStart = cEnd;
                if (breakRequired) {
                    break;
                }
            }

            mDrawer.append(rect.x + rect.width, lasty);
            mDrawer.flush();
        }
    }

}
