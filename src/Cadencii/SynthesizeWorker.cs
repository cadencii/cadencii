/*
 * SynthesizeWorker.cs
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
using System;
using System.IO;
using System.Collections.Generic;
using cadencii.media;
using cadencii.vsq;
using cadencii.java.util;

namespace cadencii
{
    public class SynthesizeWorker
    {
        private WaveGenerator mGenerator = null;
        private FormMain mMainWindow = null;

        public SynthesizeWorker(FormMain main_window)
        {
            mMainWindow = main_window;
        }

        public void patchWork(WorkerState state, Object arg)
        {
#if DEBUG
            sout.println("SynthesizeWorker#patchWork");
#endif
            VsqFileEx vsq = AppManager.getVsqFile();
            Object[] args = (Object[])arg;
            List<PatchWorkQueue> queue = (List<PatchWorkQueue>)args[0];
            List<int> tracks = (List<int>)args[1];
            int finished = queue.Count;
            string temppath = AppManager.getTempWaveDir();
            for (int k = 0; k < tracks.Count; k++) {
                int track = tracks[k];
                string wavePath = Path.Combine(temppath, track + ".wav");
                List<int> queueIndex = new List<int>();

                for (int i = 0; i < queue.Count; i++) {
                    if (queue[i].track == track) {
                        queueIndex.Add(i);
                    }
                }

                if (queueIndex.Count <= 0) {
                    // 第trackトラックに対してパッチワークを行う必要無し
                    continue;
                }

#if DEBUG
                sout.println("AppManager#pathWorkToFreeze; wavePath=" + wavePath + "; queue.get( queueIndex.get( 0 ) ).file=" + queue[queueIndex[0]].file);
                sout.println("AppManager#pathWorkToFreeze; queueIndex.size()=" + queueIndex.Count);
#endif
                if (queueIndex.Count == 1 && wavePath.Equals(queue[queueIndex[0]].file)) {
                    // 第trackトラック全体の合成を指示するキューだった場合．
                    // このとき，パッチワークを行う必要なし．
                    AppManager.mLastRenderedStatus[track - 1] =
                        new RenderedStatus((VsqTrack)vsq.Track[track].clone(), vsq.TempoTable, (SequenceConfig)vsq.config.clone());
                    AppManager.serializeRenderingStatus(temppath, track);
                    AppManager.invokeWaveViewReloadRequiredEvent(track, wavePath, 1, -1);
                    continue;
                }

                WaveWriter writer = null;
                try {
                    int sampleRate = vsq.config.SamplingRate;
                    long totalLength = (long)((vsq.getSecFromClock(vsq.TotalClocks) + 1.0) * sampleRate);
                    writer = new WaveWriter(wavePath, vsq.config.WaveFileOutputChannel, 16, sampleRate);
                    int BUFLEN = 1024;
                    double[] bufl = new double[BUFLEN];
                    double[] bufr = new double[BUFLEN];
                    double total = 0.0;
                    for (int m = 0; m < queueIndex.Count; m++) {
                        int i = queueIndex[m];
                        if (finished <= i) {
                            break;
                        }

                        // パッチワークの開始秒時
                        double secStart = vsq.getSecFromClock(queue[i].clockStart);
                        long sampleStart = (long)(secStart * sampleRate);

                        // パッチワークの終了秒時
                        int clockEnd = queue[i].clockEnd;
                        if (clockEnd == int.MaxValue) {
                            clockEnd = vsq.TotalClocks + 240;
                        }
                        double secEnd = vsq.getSecFromClock(clockEnd);
                        long sampleEnd = (long)(secEnd * sampleRate);

                        WaveReader wr = null;
                        try {
                            wr = new WaveReader(queue[i].file);
                            long remain2 = sampleEnd - sampleStart;
                            long proc = 0;
                            while (remain2 > 0) {
                                int delta = remain2 > BUFLEN ? BUFLEN : (int)remain2;
                                wr.read(proc, delta, bufl, bufr);
                                writer.replace(sampleStart + proc, delta, bufl, bufr);
                                proc += delta;
                                remain2 -= delta;
                                total += delta;
                                state.reportProgress(total);
                            }
                        } catch (Exception ex) {
                            Logger.write(typeof(AppManager) + ".patchWorkToFreeze; ex=" + ex + "\n");
                            serr.println("AppManager#patchWorkToFreeze; ex=" + ex);
                        } finally {
                            if (wr != null) {
                                try {
                                    wr.close();
                                } catch (Exception ex2) {
                                    Logger.write(typeof(AppManager) + ".patchWorkToFreeze; ex=" + ex2 + "\n");
                                    serr.println("AppManager#patchWorkToFreeze; ex2=" + ex2);
                                }
                            }
                        }

                        try {
                            PortUtil.deleteFile(queue[i].file);
                        } catch (Exception ex) {
                            Logger.write(typeof(AppManager) + ".patchWorkToFreeze; ex=" + ex + "\n");
                            serr.println("AppManager#patchWorkToFreeze; ex=" + ex);
                        }
                    }

                    VsqTrack vsq_track = vsq.Track[track];
                    if (queueIndex[queueIndex.Count - 1] <= finished) {
                        // 途中で終了せず，このトラックの全てのパッチワークが完了した．
                        AppManager.mLastRenderedStatus[track - 1] =
                            new RenderedStatus((VsqTrack)vsq_track.clone(), vsq.TempoTable, (SequenceConfig)vsq.config.clone());
                        AppManager.serializeRenderingStatus(temppath, track);
                        AppManager.setRenderRequired(track, false);
                    } else {
                        // パッチワークの作成途中で，キャンセルされた
                        // キャンセルされたやつ以降の範囲に、プログラムチェンジ17の歌手変更イベントを挿入する。→AppManager#detectTrackDifferenceに必ず検出してもらえる。
                        VsqTrack copied = (VsqTrack)vsq_track.clone();
                        VsqEvent dumy = new VsqEvent();
                        dumy.ID.type = VsqIDType.Singer;
                        dumy.ID.IconHandle = new IconHandle();
                        dumy.ID.IconHandle.Program = 17;
                        for (int m = 0; m < queueIndex.Count; m++) {
                            int i = queueIndex[m];
                            if (i < finished) {
                                continue;
                            }
                            int start = queue[i].clockStart;
                            int end = queue[i].clockEnd;
                            VsqEvent singerAtEnd = vsq_track.getSingerEventAt(end);

                            // startの位置に歌手変更が既に指定されていないかどうかを検査
                            int foundStart = -1;
                            int foundEnd = -1;
                            for (Iterator<int> itr = copied.indexIterator(IndexIteratorKind.SINGER); itr.hasNext(); ) {
                                int j = itr.next();
                                VsqEvent ve = copied.getEvent(j);
                                if (ve.Clock == start) {
                                    foundStart = j;
                                }
                                if (ve.Clock == end) {
                                    foundEnd = j;
                                }
                                if (end < ve.Clock) {
                                    break;
                                }
                            }

                            VsqEvent dumyStart = (VsqEvent)dumy.clone();
                            dumyStart.Clock = start;
                            if (foundStart >= 0) {
                                copied.setEvent(foundStart, dumyStart);
                            } else {
                                copied.addEvent(dumyStart);
                            }

                            if (end != int.MaxValue) {
                                VsqEvent dumyEnd = (VsqEvent)singerAtEnd.clone();
                                dumyEnd.Clock = end;
                                if (foundEnd >= 0) {
                                    copied.setEvent(foundEnd, dumyEnd);
                                } else {
                                    copied.addEvent(dumyEnd);
                                }
                            }

                            copied.sortEvent();
                        }

                        AppManager.mLastRenderedStatus[track - 1] = new RenderedStatus(copied, vsq.TempoTable, (SequenceConfig)vsq.config.clone());
                        AppManager.serializeRenderingStatus(temppath, track);
                    }

                    state.reportComplete();
                } catch (Exception ex) {
                    Logger.write(typeof(AppManager) + ".patchWorkToFreeze; ex=" + ex + "\n");
                    serr.println("AppManager#patchWorkToFreeze; ex=" + ex);
                } finally {
                    if (writer != null) {
                        try {
                            writer.close();
                        } catch (Exception ex2) {
                            Logger.write(typeof(AppManager) + ".patchWorkToFreeze; ex=" + ex2 + "\n");
                            serr.println("AppManager#patchWorkToFreeze; ex2=" + ex2);
                        }
                    }
                }

                // 波形表示用のWaveDrawContextの内容を更新する。
                /*for ( int j = 0; j < queueIndex.size(); j++ ) {
                    int i = queueIndex.get( j );
                    if ( i >= finished ) {
                        continue;
                    }
                    double secStart = mVsq.getSecFromClock( queue.get( i ).clockStart );
                    int clockEnd = queue.get( i ).clockEnd;
                    if ( clockEnd == int.MaxValue ) {
                        clockEnd = mVsq.TotalClocks + 240;
                    }
                    double secEnd = mVsq.getSecFromClock( clockEnd );

                    invokeWaveViewReloadRequiredEvent( tracks.get( k ), wavePath, secStart, secEnd );
                }*/
                AppManager.invokeWaveViewReloadRequiredEvent(track, wavePath, 1, -1);
            }
#if DEBUG
            sout.println("SynthesizeWorker#patchWork; done");
#endif
            state.reportComplete();
        }

        public void processQueue(WorkerState state, Object arg)
        {
#if DEBUG
            sout.println("SynthesizeWorker#processQueue");
#endif
            PatchWorkQueue q = (PatchWorkQueue)arg;
            VsqFileEx vsq = q.vsq;
            int channel = vsq.config.WaveFileOutputChannel == 1 ? 1 : 2;
            double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder(vsq.Mixer.MasterFeder);
            double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft(vsq.Mixer.MasterPanpot);
            double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight(vsq.Mixer.MasterPanpot);
            int numTrack = vsq.Track.Count;
            string tmppath = AppManager.getTempWaveDir();
            int track = q.track;

            VsqTrack vsq_track = vsq.Track[track];
            int count = vsq_track.getEventCount();
            if (count <= 0) {
                return;// false;
            }
            double amp_track = VocaloSysUtil.getAmplifyCoeffFromFeder(vsq.Mixer.Slave[track - 1].Feder);
            double pan_left_track = VocaloSysUtil.getAmplifyCoeffFromPanLeft(vsq.Mixer.Slave[track - 1].Panpot);
            double pan_right_track = VocaloSysUtil.getAmplifyCoeffFromPanRight(vsq.Mixer.Slave[track - 1].Panpot);
            double amp_left = amp_track * pan_left_track;
            double amp_right = amp_track * pan_right_track;
            int total_clocks = vsq.TotalClocks;
            double total_sec = vsq.getSecFromClock(total_clocks);

            RendererKind kind = VsqFileEx.getTrackRendererKind(vsq_track);
            mGenerator = VSTiDllManager.getWaveGenerator(kind);
            Amplifier amp = new Amplifier();
            amp.setRoot(mGenerator);
            if (q.renderAll) {
                amp.setAmplify(amp_left, amp_right);
            }
            mGenerator.setReceiver(amp);
            mGenerator.setGlobalConfig(AppManager.editorConfig);
            mGenerator.setMainWindow(mMainWindow);

            Mixer mixer = new Mixer();
            mixer.setRoot(mGenerator);
            mixer.setGlobalConfig(AppManager.editorConfig);
            amp.setReceiver(mixer);

            if (q.renderAll && vsq.config.WaveFileOutputFromMasterTrack) {
                // トラック全体を合成するモードで，かつ，他トラックを合成して出力するよう指示された場合
                if (numTrack > 2) {
                    for (int i = 1; i < numTrack; i++) {
                        if (i == track) continue;
                        string file = Path.Combine(tmppath, i + ".wav");
                        if (!File.Exists(file)) {
                            // mixするべきファイルが揃っていないのでbailout
                            return;// true;
                        }
                        WaveReader r = null;
                        try {
                            r = new WaveReader(file);
                        } catch (Exception ex) {
                            Logger.write(typeof(SynthesizeWorker) + ".processQueue; ex=" + ex + "\n");
                            r = null;
                        }
                        if (r == null) {
                            return;// true;
                        }
                        double end_sec = vsq.getSecFromClock(q.clockStart);
                        r.setOffsetSeconds(end_sec);
                        Amplifier amp_i_unit = new Amplifier();
                        amp_i_unit.setRoot(mGenerator);
                        double amp_i = VocaloSysUtil.getAmplifyCoeffFromFeder(vsq.Mixer.Slave[i - 1].Feder);
                        double pan_left_i = VocaloSysUtil.getAmplifyCoeffFromPanLeft(vsq.Mixer.Slave[i - 1].Panpot);
                        double pan_right_i = VocaloSysUtil.getAmplifyCoeffFromPanRight(vsq.Mixer.Slave[i - 1].Panpot);
                        double amp_left_i = amp_i * pan_left_i;
                        double amp_right_i = amp_i * pan_right_i;
#if DEBUG
                        sout.println("FormSynthesize#bgWork_DoWork; #" + i + "; amp_left_i=" + amp_left_i + "; amp_right_i=" + amp_right_i);
#endif
                        amp_i_unit.setAmplify(amp_left_i, amp_right_i);
                        FileWaveSender wave_sender = new FileWaveSender(r);
                        wave_sender.setRoot(mGenerator);
                        wave_sender.setGlobalConfig(AppManager.editorConfig);

                        amp_i_unit.setSender(wave_sender);
                        mixer.addSender(amp_i_unit);
                    }
                }
            }

            PortUtil.deleteFile(q.file);
            int sample_rate = vsq.config.SamplingRate;
#if DEBUG
            sout.println("FormSynthesize#bgWork_DoWork; q.file=" + q.file);
#endif
            FileWaveReceiver wave_receiver = new FileWaveReceiver(q.file, channel, 16, sample_rate);
            wave_receiver.setRoot(mGenerator);
            wave_receiver.setGlobalConfig(AppManager.editorConfig);
            Amplifier amp_unit_master = new Amplifier();
            amp_unit_master.setRoot(mGenerator);
            if (q.renderAll) {
                double l = amp_master * pan_left_master;
                double r = amp_master * pan_right_master;
                amp_unit_master.setAmplify(l, r);
            }
            mixer.setReceiver(amp_unit_master);
            amp_unit_master.setReceiver(wave_receiver);

            int end = q.clockEnd;
            if (end == int.MaxValue) end = vsq.TotalClocks + 240;
            mGenerator.init(vsq, track, q.clockStart, end, sample_rate);

            double sec_start = vsq.getSecFromClock(q.clockStart);
            double sec_end = vsq.getSecFromClock(end);
            long samples = (long)((sec_end - sec_start) * sample_rate);
            mGenerator.begin(samples, state);

            return;// false;
        }
    }

}
