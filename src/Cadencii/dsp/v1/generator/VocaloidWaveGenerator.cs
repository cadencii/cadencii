#if ENABLE_VOCALOID
/*
 * VocaloidWaveGenerator.cs
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
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Text;
using cadencii;
using cadencii.java.awt;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.media;
using cadencii.vsq;


namespace cadencii
{

    /// <summary>
    /// ドライバーからの波形を受け取るためのインターフェース
    /// </summary>
    public interface IWaveIncoming
    {
        /// <summary>
        /// ドライバから波形を受け取るためのコールバック関数
        /// </summary>
        /// <param name="l">左チャンネルの波形データ</param>
        /// <param name="r">右チャンネルの波形データ</param>
        /// <param name="length">波形データの長さ。配列の長さよりも短い場合がある</param>
        void waveIncomingImpl(double[] l, double[] r, int length, WorkerState state);
    }
}

namespace cadencii
{

    public class VocaloidWaveGenerator : WaveUnit, WaveGenerator, IWaveIncoming
    {
        private const int BUFLEN = 1024;
        private const int VERSION = 0;

        private long mTotalAppend = 0;
        private VsqFileEx mVsq = null;
        private int mTrack;
        private int mStartClock;
        private int mEndClock;
        private long mTotalSamples;
        //private bool mAbortRequired = false;
        private double[] mBufferL = new double[BUFLEN];
        private double[] mBufferR = new double[BUFLEN];
        private WaveReceiver mReceiver = null;
        private int mTrimRemain = 0;
        private bool mRunning = false;
        private VocaloidDriver mDriver = null;
        /// <summary>
        /// 波形処理ラインのサンプリング周波数
        /// </summary>
        private int mSampleRate;
        /// <summary>
        /// VOCALOID VSTiの実際のサンプリング周波数
        /// </summary>
        private int mDriverSampleRate;
        /// <summary>
        /// サンプリング周波数変換器
        /// </summary>
        private RateConvertContext mContext;
        //private WorkerState mState;

        public int getSampleRate()
        {
            return mSampleRate;
        }

        public bool isRunning()
        {
            return mRunning;
        }

        public long getTotalSamples()
        {
            return mTotalSamples;
        }

        public double getProgress()
        {
            if (mTotalSamples > 0) {
                return mTotalAppend / (double)mTotalSamples;
            } else {
                return 0.0;
            }
        }

        public override void setConfig(string parameter)
        {
            // do nothing
        }

        /// <summary>
        /// 初期化メソッド．
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track"></param>
        /// <param name="start_clock"></param>
        /// <param name="end_clock"></param>
        /// <param name="sample_rate">波形処理ラインのサンプリング周波数</param>
        public void init(VsqFileEx vsq, int track, int start_clock, int end_clock, int sample_rate)
        {
            mVsq = vsq;
            mTrack = track;
            mStartClock = start_clock;
            mEndClock = end_clock;
            mSampleRate = sample_rate;
            mDriverSampleRate = 44100;
            try {
                mContext = new RateConvertContext(mDriverSampleRate, mSampleRate);
            } catch (Exception ex) {
                try {
                    // 苦肉の策
                    mContext = new RateConvertContext(mDriverSampleRate, mDriverSampleRate);
                } catch (Exception ex2) {
                }
            }
        }

        public override int getVersion()
        {
            return VERSION;
        }

        public void setReceiver(WaveReceiver r)
        {
            if (mReceiver != null) {
                mReceiver.end();
            }
            mReceiver = r;
        }

        /// <summary>
        /// VSTiドライバに呼んでもらう波形受け渡しのためのコールバック関数にして、IWaveIncomingインターフェースの実装。
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <param name="length"></param>
        public void waveIncomingImpl(double[] l, double[] r, int length, WorkerState state)
        {
            int offset = 0;
            if (mTrimRemain > 0) {
                // トリムしなくちゃいけない分がまだ残っている場合。トリム処理を行う。
                if (length <= mTrimRemain) {
                    // 受け取った波形の長さをもってしても、トリム分が0にならない場合
                    mTrimRemain -= length;
                    return;
                } else {
                    // 受け取った波形の内の一部をトリムし、残りを波形レシーバに渡す
                    offset = mTrimRemain;
                    // これにてトリム処理は終了なので。
                    mTrimRemain = 0;
                }
            }
            int remain = length - offset;
            while (remain > 0) {
                if (state.isCancelRequested()) {
                    return;
                }
                int amount = (remain > BUFLEN) ? BUFLEN : remain;
                for (int i = 0; i < amount; i++) {
                    mBufferL[i] = l[i + offset];
                    mBufferR[i] = r[i + offset];
                }
                while (RateConvertContext.convert(mContext, mBufferL, mBufferR, amount)) {
                    mReceiver.push(mContext.bufferLeft, mContext.bufferRight, mContext.length);
                    mTotalAppend += mContext.length;
                    state.reportProgress(mTotalAppend);
                }
                remain -= amount;
                offset += amount;
            }
            return;
        }

        /// <summary>
        /// beginメソッドを抜けるときに共通する処理を行います
        /// </summary>
        private void exitBegin()
        {
            mReceiver.end();
            mRunning = false;
        }

        public void begin(long total_samples, WorkerState state)
        {
            // 渡されたVSQの、合成に不要な部分を削除する
            VsqFileEx split = (VsqFileEx)mVsq.clone();
            VsqTrack vsq_track = split.Track[mTrack];
            split.updateTotalClocks();
            if (mEndClock < mVsq.TotalClocks) {
                split.removePart(mEndClock, split.TotalClocks + 480);
            }
            double start_sec = mVsq.getSecFromClock(mStartClock);
            double end_sec = mVsq.getSecFromClock(mEndClock);

            // トラックの合成エンジンの種類
            RendererKind s_working_renderer = VsqFileEx.getTrackRendererKind(vsq_track);

            // VOCALOIDのドライバの場合，末尾に余分な音符を入れる
            int extra_note_clock = (int)mVsq.getClockFromSec((float)end_sec + 10.0f);
            int extra_note_clock_end = (int)mVsq.getClockFromSec((float)end_sec + 10.0f + 3.1f); //ブロックサイズが1秒分で、バッファの個数が3だから +3.1f。0.1fは安全のため。
            VsqEvent extra_note = new VsqEvent(extra_note_clock, new VsqID(0));
            extra_note.ID.type = VsqIDType.Anote;
            extra_note.ID.Note = 60;
            extra_note.ID.setLength(extra_note_clock_end - extra_note_clock);
            extra_note.ID.VibratoHandle = null;
            extra_note.ID.LyricHandle = new LyricHandle("a", "a");
            vsq_track.addEvent(extra_note);

            // VSTiが渡してくる波形のうち、先頭からtrim_sec秒分だけ省かないといけない
            // プリセンドタイムがあるので、無条件に合成開始位置以前のデータを削除すると駄目なので。
            double trim_sec = 0.0;
            if (mStartClock < split.getPreMeasureClocks()) {
                // 合成開始位置が、プリメジャーよりも早い位置にある場合。
                // VSTiにはクロック0からのデータを渡し、クロック0から合成開始位置までをこのインスタンスでトリム処理する
                trim_sec = split.getSecFromClock(mStartClock);
            } else {
                // 合成開始位置が、プリメジャー以降にある場合。
                // プリメジャーの終了位置から合成開始位置までのデータを削除する
                split.removePart(mVsq.getPreMeasureClocks(), mStartClock);
                trim_sec = split.getSecFromClock(split.getPreMeasureClocks());
            }
            split.updateTotalClocks();
            // 対象のトラックの合成を担当するVSTiを検索
            mDriver = null;
            for (int i = 0; i < VSTiDllManager.vocaloidDriver.Count; i++) {
                if (VSTiDllManager.vocaloidDriver[i].getRendererKind() == s_working_renderer) {
                    mDriver = VSTiDllManager.vocaloidDriver[i];
                    break;
                }
            }
            // ドライバー見つからなかったらbail out
            if (mDriver == null) {
                exitBegin();
                return;
            }
            // ドライバーが読み込み完了していなかったらbail out
            if (!mDriver.loaded) {
                exitBegin();
                return;
            }

            // NRPNを作成
            int ms_present = mConfig.PreSendTime;
#if DEBUG
            sout.println("VocaloidWaveGenerator#begin; ms_present=" + ms_present);
#endif
            VsqNrpn[] vsq_nrpn = VsqFile.generateNRPN(split, mTrack, ms_present);
#if DEBUG
            string suffix = "_win";
            string path = Path.Combine(PortUtil.getApplicationStartupPath(), "vocaloid_wave_generator_begin_data_" + mTrack + suffix + ".txt");
            StreamWriter bw = null;
            try {
                bw = new StreamWriter(path, false, new UTF8Encoding(false));
                for (int i = 0; i < vsq_nrpn.Length; i++) {
                    VsqNrpn item = vsq_nrpn[i];
                    string name = NRPN.getName(item.Nrpn);
                    int len = name.Length;
                    for (int j = len; j < 35; j++) {
                        name += " ";
                    }
                    bw.WriteLine("     " + item.Clock.ToString("D8") + " 0x" + item.Nrpn.ToString("X4") + " " + name + " 0x" + item.DataMsb.ToString("X2") + " 0x" + item.DataLsb.ToString("X2"));
                }
            } catch (Exception ex) {
            } finally {
                if (bw != null) {
                    try {
                        bw.Close();
                    } catch (Exception ex2) {
                    }
                }
                bw = null;
            }
#endif
            NrpnData[] nrpn = VsqNrpn.convert(vsq_nrpn);

            // 最初のテンポ指定を検索
            // VOCALOID VSTiが返してくる波形にはなぜかずれがある。このズレは最初のテンポで決まるので。
            float first_tempo = 125.0f;
            if (split.TempoTable.Count > 0) {
                first_tempo = (float)(60e6 / (double)split.TempoTable[0].Tempo);
            }
            // ずれるサンプル数
            int errorSamples = VSTiDllManager.getErrorSamples(first_tempo);
            // 今後トリムする予定のサンプル数と、
            mTrimRemain = errorSamples + (int)(trim_sec * mDriverSampleRate);
#if DEBUG
            sout.println("VocaloidWaveGenerator#begin; trim_sec=" + trim_sec + "; mTrimRemain=" + mTrimRemain);
#endif
            // 合計合成する予定のサンプル数を決める
            mTotalSamples = (long)((end_sec - start_sec) * mDriverSampleRate) + errorSamples;
#if DEBUG
            sout.println("VocaloidWaveGenerator#begin; mTotalSamples=" + mTotalSamples + "; start_sec,end_sec=" + start_sec + "," + end_sec + "; errorSamples=" + errorSamples);
#endif

            // アボート要求フラグを初期化
            //mAbortRequired = false;
            // 使いたいドライバーが使用中だった場合、ドライバーにアボート要求を送る。
            // アボートが終了するか、このインスタンス自身にアボート要求が来るまで待つ。
            if (mDriver.isRendering()) {
                // ドライバーにアボート要求
                //mDriver.abortRendering();
                while (mDriver.isRendering() && !state.isCancelRequested()) {
                    // 待つ
                    Thread.Sleep(100);
                }
            }

            // ここにきて初めて再生中フラグが立つ
            mRunning = true;

            // 古いイベントをクリア
            mDriver.clearSendEvents();
            // ドライバーに渡すイベントを準備
            // まず、マスタートラックに渡すテンポ変更イベントを作成
            int tempo_count = split.TempoTable.Count;
            byte[] masterEventsSrc = new byte[tempo_count * 3];
            int[] masterClocksSrc = new int[tempo_count];
            int count = -3;
            for (int i = 0; i < tempo_count; i++) {
                count += 3;
                TempoTableEntry itemi = split.TempoTable[i];
                masterClocksSrc[i] = itemi.Clock;
                byte b0 = (byte)(0xff & (itemi.Tempo >> 16));
                long u0 = (long)(itemi.Tempo - (b0 << 16));
                byte b1 = (byte)(0xff & (u0 >> 8));
                byte b2 = (byte)(0xff & (u0 - (u0 << 8)));
                masterEventsSrc[count] = b0;
                masterEventsSrc[count + 1] = b1;
                masterEventsSrc[count + 2] = b2;
            }

            // 送る
            mDriver.sendEvent(masterEventsSrc, masterClocksSrc, 0);

            // 次に、合成対象トラックの音符イベントを作成
            int numEvents = nrpn.Length;
            byte[] bodyEventsSrc = new byte[numEvents * 3];
            int[] bodyClocksSrc = new int[numEvents];
            count = -3;
            int last_clock = 0;
            for (int i = 0; i < numEvents; i++) {
                int c = nrpn[i].getClock();
                count += 3;
                bodyEventsSrc[count] = (byte)0xb0;
                bodyEventsSrc[count + 1] = (byte)(0xff & nrpn[i].getParameter());
                bodyEventsSrc[count + 2] = (byte)(0xff & nrpn[i].Value);
                bodyClocksSrc[i] = c;
                last_clock = c;
            }

            // 送る
            mDriver.sendEvent(bodyEventsSrc, bodyClocksSrc, 1);

            // 合成を開始
            // 合成が終わるか、ドライバへのアボート要求が来るまでは制御は返らない
#if DEBUG
            // master
            Stream fos_master =
                new FileStream(
                    Path.Combine(
                        PortUtil.getApplicationStartupPath(),
                        "src_master.bin"), FileMode.OpenOrCreate, FileAccess.Write);
            fos_master.WriteByte(0x01);
            fos_master.WriteByte(0x04);
            byte[] buf = PortUtil.getbytes_uint32_le(tempo_count);
            fos_master.Write(buf, 0, 4);
            count = 0;
            for (int i = 0; i < tempo_count; i++) {
                buf = PortUtil.getbytes_uint32_le(masterClocksSrc[i]);
                fos_master.Write(buf, 0, 4);
                fos_master.Write(masterEventsSrc, count, 3);
                count += 3;
            }
            fos_master.Close();
            // body
            Stream fos_body =
                new FileStream(
                    Path.Combine(
                        PortUtil.getApplicationStartupPath(),
                        "src_body.bin"), FileMode.OpenOrCreate, FileAccess.ReadWrite);
            buf = PortUtil.getbytes_uint32_le(numEvents);
            fos_body.WriteByte(0x02);
            fos_body.WriteByte(0x04);
            fos_body.Write(buf, 0, 4);
            count = 0;
            for (int i = 0; i < numEvents; i++) {
                buf = PortUtil.getbytes_uint32_le(bodyClocksSrc[i]);
                fos_body.Write(buf, 0, 4);
                fos_body.Write(bodyEventsSrc, count, 3);
                count += 3;
            }
            fos_body.Close();
            // synth
            long act_total_samples = mTotalSamples + mTrimRemain;
            buf = PortUtil.getbytes_int64_le(act_total_samples);
            Stream fos_synth =
                new FileStream(
                    Path.Combine(
                        PortUtil.getApplicationStartupPath(),
                        "src_synth.bin"), FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fos_synth.WriteByte(0x03);
            fos_synth.WriteByte(0x08);
            fos_synth.Write(buf, 0, 8);
            fos_synth.Close();
#endif
            mDriver.startRendering(
                mTotalSamples + mTrimRemain + (int)(ms_present / 1000.0 * mDriverSampleRate),
                false,
                mDriverSampleRate,
                this,
                state);

            // ここに来るということは合成が終わったか、ドライバへのアボート要求が実行されたってこと。
            // このインスタンスが受け持っている波形レシーバに、処理終了を知らせる。
            exitBegin();
            if (state.isCancelRequested() == false) {
                state.reportComplete();
            }
        }

        public long getPosition()
        {
            return mTotalAppend;
        }
    }

}
#endif
