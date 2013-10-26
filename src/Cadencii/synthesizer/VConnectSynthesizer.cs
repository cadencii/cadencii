/*
 * VConnectSynthesizer.cs
 * Copyright © 2013 kbinani
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using cadencii.dsp.v2.generator;
using cadencii.media;
using cadencii.utau;
using cadencii.vsq;
using System.Linq;

namespace cadencii.synthesizer
{
    /// <summary>
    /// vConnect-STANDを使って音声合成を行う波形生成器
    /// </summary>
    public class VConnectSynthesizer : ISingingSynthesizer
    {
        /// <summary>
        /// シンセサイザの実行ファイル名
        /// </summary>
        private const string STRAIGHT_SYNTH = "vConnect-STAND.exe";

        private const int BUFLEN = 1024;
        private const int TEMPO = 120;
        private const int MAX_CACHE = 512;

        private static SortedDictionary<string, double> cache_ = new SortedDictionary<string, double>();

        private int sample_rate_;

        private List<VConnectRenderingQueue> queue_;
        private List<SingerConfig> singer_configs_;

        private SortedDictionary<string, UtauVoiceDB> voice_db_configs_ = new SortedDictionary<string, UtauVoiceDB>();
        private long sequence_length_samples_;
        private VsqFile sequence_;

        public event RenderCallback Rendered;

        public VConnectSynthesizer(List<SingerConfig> singer_configs)
        {
            singer_configs_ = singer_configs;
        }

        public void beginSession(VsqFile sequence, int track_index, int sample_rate)
        {
            // VSTiProxyの実装より
            sequence_ = (VsqFile)sequence.clone();
            sequence_.updateTotalClocks();
            sample_rate_ = sample_rate;

            // StraightRenderingRunner.ctorの実装より
            queue_ = new List<VConnectRenderingQueue>();
            if (singer_configs_ == null) {
                singer_configs_ = new List<SingerConfig>();
            }
            int midi_tempo = 60000000 / TEMPO;
            VsqFile work = (VsqFile)sequence_.clone();
            TempoVector tempo = new TempoVector();
            tempo.Clear();
            tempo.Add(new TempoTableEntry(0, midi_tempo, 0.0));
            tempo.updateTempoInfo();
            work.adjustClockToMatchWith(tempo);
            // テンポテーブルをクリア
            work.TempoTable.Clear();
            work.TempoTable.Add(new TempoTableEntry(0, midi_tempo, 0.0));
            work.updateTempoInfo();
            VsqTrack vsq_track = work.Track[track_index];
            List<VsqEvent> events = new List<VsqEvent>(); // 順次取得はめんどくさいので，一度eventsに格納してから処理しよう
            int count = vsq_track.getEventCount();
            VsqEvent current_singer_event = null;

            for (int i = 0; i < count; i++) {
                VsqEvent item = vsq_track.getEvent(i);
                if (item.ID.type == VsqIDType.Singer) {
                    if (events.Count > 0 && current_singer_event != null) {
                        // eventsに格納されたノートイベントについて，StraightRenderingQueueを順次作成し，登録
                        appendQueue(work, track_index, events, current_singer_event);
                        events.Clear();
                    }
                    current_singer_event = item;
                } else if (item.ID.type == VsqIDType.Anote) {
                    events.Add(item);
                }
            }
            if (events.Count > 0 && current_singer_event != null) {
                appendQueue(work, track_index, events, current_singer_event);
            }
            if (queue_.Count > 0) {
                VConnectRenderingQueue q = queue_[queue_.Count - 1];
                sequence_length_samples_ = q.startSample + q.abstractSamples;
            }
        }

        public void endSession()
        { }

        public void start()
        {
            if (Rendered == null) {
                return;
            }
            float[] bufL = new float[BUFLEN];
            float[] bufR = new float[BUFLEN];
            string straight_synth = Path.Combine(PortUtil.getApplicationStartupPath(), STRAIGHT_SYNTH);
            if (!System.IO.File.Exists(straight_synth)) {
                return;
            }
            int count = queue_.Count;

            // 合計でレンダリングしなければならないサンプル数を計算しておく
            double total_samples = 0;
            for (int i = 0; i < count; i++) {
                total_samples += queue_[i].abstractSamples;
            }

            long max_next_wave_start = sequence_length_samples_;

            if (queue_.Count > 0) {
                // 最初のキューが始まるまでの無音部分
                VConnectRenderingQueue queue = queue_[0];
                if (queue.startSample > 0) {
                    for (int i = 0; i < BUFLEN; i++) {
                        bufL[i] = 0;
                        bufR[i] = 0;
                    }
                    long remain = queue.startSample;
                    while (remain > 0) {
                        int len = (remain > BUFLEN) ? BUFLEN : (int)remain;
                        Rendered(bufL, bufR, len);
                        remain -= len;
                    }
                }
            }

            float[] cached_data_l = new float[BUFLEN];
            float[] cached_data_r = new float[BUFLEN];
            int cached_data_length = 0;
            double processed_samples = 0.0;

            for (int i = 0; i < count; i++) {
                VConnectRenderingQueue queue = queue_[i];
                string tmp_dir = AppManager.getTempWaveDir();

                string tmp_file = Path.Combine(tmp_dir, "tmp.usq");
                string hash = "";
                using (var writer = new StreamWriter(tmp_file, false, Encoding.GetEncoding("Shift_JIS"))) {
                    prepareMetaText(writer, queue.track, queue.oto_ini, queue.endClock);
                }
                try {
                    hash = PortUtil.getMD5(tmp_file).Replace("_", "");
                } catch (Exception ex) {
                }
                try {
                    PortUtil.copyFile(tmp_file, Path.Combine(tmp_dir, hash + ".usq"));
                    PortUtil.deleteFile(tmp_file);
                } catch (Exception ex) {
                }
                tmp_file = Path.Combine(tmp_dir, hash);
                if (!cache_.ContainsKey(hash) || !System.IO.File.Exists(tmp_file + ".wav")) {
                    using (var process = new Process()) {
                        process.StartInfo.FileName = straight_synth;
                        process.StartInfo.Arguments = "\"" + tmp_file + ".usq\" \"" + tmp_file + ".wav\"";
                        process.StartInfo.WorkingDirectory = PortUtil.getApplicationStartupPath();
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        process.Start();
                        process.WaitForExit();
                    }

#if !DEBUG
                    try {
                        PortUtil.deleteFile( tmp_file + ".usq" );
                    } catch( Exception ex ){
                    }
#endif

                    if (cache_.Count > MAX_CACHE) {
                        // キャッシュの許容個数を超えたので、古いものを削除
                        bool first = true;
                        double old_date = PortUtil.getCurrentTime();
                        string old_key = "";
                        foreach (var key in cache_.Keys) {
                            double time = cache_[key];
                            if (first) {
                                old_date = time;
                                old_key = key;
                            } else {
                                if (old_date > time) {
                                    old_date = time;
                                    old_key = key;
                                }
                            }
                        }
                        cache_.Remove(old_key);
                        try {
                            PortUtil.deleteFile(Path.Combine(tmp_dir, old_key + ".wav"));
                        } catch (Exception ex) {
                        }
                    }
                    cache_[hash] = PortUtil.getCurrentTime();
                }

                long next_wave_start = max_next_wave_start;
                if (i + 1 < count) {
                    VConnectRenderingQueue next_queue = queue_[i + 1];
                    next_wave_start = next_queue.startSample;
                }

                try {
                    WaveReader wr = null;
                    if (File.Exists(tmp_file + ".wav")) {
                        wr = new WaveReader(tmp_file + ".wav");
                    }
                    int wave_samples = 0;
                    if (wr != null) wave_samples = (int)wr.getTotalSamples();
                    int overlapped = 0;
                    if (next_wave_start <= queue.startSample + wave_samples) {
                        // 次のキューの開始位置が、このキューの終了位置よりも早い場合
                        // オーバーラップしているサンプル数
                        overlapped = (int)(queue.startSample + wave_samples - next_wave_start);
                        wave_samples = (int)(next_wave_start - queue.startSample); //ここまでしか読み取らない
                    }

                    if (cached_data_length == 0) {
                        // キャッシュが残っていない場合
                        int remain = wave_samples;
                        long pos = 0;
                        while (remain > 0) {
                            int len = (remain > BUFLEN) ? BUFLEN : remain;
                            if (wr != null) {
                                wr.read(pos, len, bufL, bufR);
                            } else {
                                for (int j = 0; j < BUFLEN; j++) {
                                    bufL[j] = 0;
                                    bufR[j] = 0;
                                }
                            }
                            if (!Rendered(bufL, bufR, len)) {
                                return;
                            }
                            pos += len;
                            remain -= len;
                        }

                        int rendererd_length = 0;
                        if (wr != null) {
                            rendererd_length = (int)wr.getTotalSamples();
                        }
                        if (wave_samples < rendererd_length) {
                            // 次のキューのためにデータを残す
                            if (wr != null) {
                                // 必要ならキャッシュを追加
                                if (cached_data_l.Length < overlapped) {
                                    Array.Resize(ref cached_data_l, overlapped);
                                    Array.Resize(ref cached_data_r, overlapped);
                                }
                                // 長さが変わる
                                cached_data_length = overlapped;
                                // WAVEから読み込み
                                wr.read(pos, overlapped, cached_data_l, cached_data_r);
                            }
                        } else if (i + 1 < count) {
                            // 次のキューのためにデータを残す必要がない場合で、かつ、最後のキューでない場合。
                            // キュー間の無音部分を0で埋める
                            int silence_samples = (int)(next_wave_start - (queue.startSample + rendererd_length));
                            for (int j = 0; j < BUFLEN; j++) {
                                bufL[j] = 0;
                                bufR[j] = 0;
                            }
                            while (silence_samples > 0) {
                                int amount = (silence_samples > BUFLEN) ? BUFLEN : silence_samples;
                                if (!Rendered(bufL, bufR, amount)) {
                                    return;
                                }
                                silence_samples -= amount;
                            }
                        }
                    } else {
                        // キャッシュが残っている場合
                        int rendered_length = 0;
                        if (wr != null) {
                            rendered_length = (int)wr.getTotalSamples();
                        }
                        if (rendered_length < cached_data_length) {
                            if (next_wave_start < queue.startSample + cached_data_length) {
                                // PATTERN A
                                //  ----[*****************************]----------------->  cache
                                //  ----[*********************]------------------------->  renderd result
                                //  -----------------[******************************...->  next rendering queue (not rendered yet)
                                //                  ||
                                //                  \/
                                //  ----[***********]----------------------------------->  append
                                //  -----------------[********][******]----------------->  new cache
                                //  
                                //                         OR
                                // PATTERN B
                                //  ----[*****************************]----------------->   cache
                                //  ----[***************]------------------------------->   rendered result
                                //  ----------------------------[*******************...->   next rendering queue (not rendered yet)
                                //                  ||
                                //                  \/
                                //  ----[***************][*****]------------------------>   append
                                //  ----------------------------[*****]----------------->   new chache
                                //  
                                try {
                                    // レンダリング結果とキャッシュをMIX
                                    int remain = rendered_length;
                                    int offset = 0;
                                    while (remain > 0) {
                                        int amount = (remain > BUFLEN) ? BUFLEN : remain;
                                        wr.read(offset, amount, bufL, bufR);
                                        for (int j = 0; j < amount; j++) {
                                            cached_data_l[j + offset] += bufL[j];
                                            cached_data_r[j + offset] += bufR[j];
                                        }
                                        offset += amount;
                                        remain -= amount;
                                    }
                                    int append_len = (int)(next_wave_start - queue.startSample);
                                    if (!Rendered(cached_data_l, cached_data_r, append_len)) {
                                        return;
                                    }

                                    // 送信したキャッシュの部分をシフト
                                    // この場合，シフト後のキャッシュの長さは，元の長さより短くならないのでリサイズ不要
                                    for (int j = append_len; j < cached_data_length; j++) {
                                        cached_data_l[j - append_len] = cached_data_l[j];
                                        cached_data_r[j - append_len] = cached_data_r[j];
                                    }
                                    cached_data_length -= append_len;
                                } catch (Exception ex) {
                                    AppManager.debugWriteLine(typeof(VConnectSynthesizer) + "#begin; (A),(B); ex=" + ex);
                                }
                            } else {
                                // PATTERN C
                                //  ----[*****************************]----------------->   cache
                                //  ----[***************]------------------------------->   rendered result
                                //  -----------------------------------------[******...->   next rendering queue (not rendered yet)
                                //                  ||
                                //                  \/
                                //  ----[*****************************]----------------->   append
                                //  ---------------------------------------------------->   new chache -> NULL!!
                                //  -----------------------------------[****]----------->   append#2 (silence)
                                //  
                                try {
                                    // レンダリング結果とキャッシュをMIX
                                    int remain = rendered_length;
                                    int offset = 0;
                                    while (remain > 0) {
                                        int amount = (remain > BUFLEN) ? BUFLEN : remain;
                                        wr.read(offset, amount, bufL, bufR);
                                        for (int j = 0; j < amount; j++) {
                                            cached_data_l[j + offset] += bufL[j];
                                            cached_data_r[j + offset] += bufR[j];
                                        }
                                        remain -= amount;
                                        offset += amount;
                                    }
                                    // MIXした分を送信
                                    if (!Rendered(cached_data_l, cached_data_r, cached_data_length)) {
                                        return;
                                    }

                                    // 隙間を無音で埋める
                                    for (int j = 0; j < BUFLEN; j++) {
                                        bufL[j] = 0;
                                        bufR[j] = 0;
                                    }
                                    int silence_len = (int)(next_wave_start - (queue.startSample + cached_data_length));
                                    remain = silence_len;
                                    while (remain > 0) {
                                        int amount = (remain > BUFLEN) ? BUFLEN : remain;
                                        if (!Rendered(bufL, bufR, amount)) {
                                            return;
                                        }
                                        remain -= amount;
                                    }

                                    // キャッシュの長さは0になる
                                    cached_data_length = 0;
                                } catch (Exception ex) {
                                    AppManager.debugWriteLine(typeof(VConnectSynthesizer) + "#begin; (C); ex=" + ex);
                                }
                            }
                        } else {
                            if (next_wave_start < queue.startSample + cached_data_length) {
                                // PATTERN D
                                //  ----[*************]--------------------------------->  cache
                                //  ----[*********************]------------------------->  renderd result
                                //  ------------[***********************************...->  next rendering queue (not rendered yet)
                                //                  ||
                                //                  \/
                                //  ----[******]---------------------------------------->  append
                                //  ------------[*****][******]------------------------->  new cache
                                //  
                                try {
                                    // 次のキューの直前の部分まで，レンダリング結果を読み込んでMIX，送信
                                    int append_len = (int)(next_wave_start - queue.startSample);
                                    int remain = append_len;
                                    int offset = 0;
                                    while (remain > 0) {
                                        int amount = (remain > BUFLEN) ? BUFLEN : remain;
                                        wr.read(offset, amount, bufL, bufR);
                                        for (int j = 0; j < amount; j++) {
                                            bufL[j] += cached_data_l[j + offset];
                                            bufR[j] += cached_data_r[j + offset];
                                        }
                                        if (!Rendered(bufL, bufR, amount)) {
                                            return;
                                        }
                                        offset += amount;
                                        remain -= amount;
                                    }

                                    // まだMIXしていないcacheをシフト
                                    for (int j = append_len; j < cached_data_length; j++) {
                                        cached_data_l[j - append_len] = cached_data_l[j];
                                        cached_data_r[j - append_len] = cached_data_r[j];
                                    }
                                    // 0で埋める
                                    for (int j = cached_data_length - append_len; j < cached_data_l.Length; j++) {
                                        cached_data_l[j] = 0;
                                        cached_data_r[j] = 0;
                                    }

                                    // キャッシュの長さを更新
                                    int old_cache_length = cached_data_length;
                                    int new_cache_len = (int)((queue.startSample + rendered_length) - next_wave_start);
                                    if (cached_data_l.Length < new_cache_len) {
                                        Array.Resize(ref cached_data_l, new_cache_len);
                                        Array.Resize(ref cached_data_r, new_cache_len);
                                    }
                                    cached_data_length = new_cache_len;

                                    // 残りのレンダリング結果をMIX
                                    remain = rendered_length - append_len;
                                    offset = append_len;
                                    while (remain > 0) {
                                        int amount = (remain > BUFLEN) ? BUFLEN : remain;
                                        wr.read(offset, amount, bufL, bufR);
                                        for (int j = 0; j < amount; j++) {
                                            cached_data_l[j + offset - append_len] += bufL[j];
                                            cached_data_r[j + offset - append_len] += bufR[j];
                                        }
                                        remain -= amount;
                                        offset += amount;
                                    }
                                } catch (Exception ex) {
                                    AppManager.debugWriteLine(typeof(VConnectSynthesizer) + "#begin; (D); ex=" + ex);
                                }
                            } else if (next_wave_start < queue.startSample + rendered_length) {
                                // PATTERN E
                                //  ----[*************]--------------------------------->  cache
                                //  ----[*********************]------------------------->  renderd result
                                //  ----------------------[*************************...->  next rendering queue (not rendered yet)
                                //                  ||
                                //                  \/
                                //  ----[*************][*]------------------------------>  append
                                //  ----------------------[***]------------------------->  new cache
                                //  
                                try {
                                    // キャッシュとレンダリング結果をMIX
                                    int remain = cached_data_length;
                                    int offset = 0;
                                    while (remain > 0) {
                                        int amount = (remain > BUFLEN) ? BUFLEN : remain;
                                        wr.read(offset, amount, bufL, bufR);
                                        for (int j = 0; j < amount; j++) {
                                            cached_data_l[j + offset] += bufL[j];
                                            cached_data_r[j + offset] += bufR[j];
                                        }
                                        remain -= amount;
                                        offset += amount;
                                    }
                                    // 送信
                                    if (!Rendered(cached_data_l, cached_data_r, cached_data_length)) {
                                        return;
                                    }

                                    // キャッシュと，次のキューの隙間の部分
                                    // レンダリング結果をそのまま送信
                                    remain = (int)(next_wave_start - (queue.startSample + cached_data_length));
                                    while (remain > 0) {
                                        int amount = (remain > BUFLEN) ? BUFLEN : remain;
                                        wr.read(offset, amount, bufL, bufR);
                                        if (!Rendered(bufL, bufR, amount)) {
                                            return;
                                        }
                                        remain -= amount;
                                        offset += amount;
                                    }

                                    // レンダリング結果と，次のキューが重なっている部分をキャッシュに残す
                                    remain = (int)(queue.startSample + rendered_length - next_wave_start);
                                    // キャッシュが足りなければ更新
                                    if (cached_data_l.Length < remain) {
                                        Array.Resize(ref cached_data_l, remain);
                                        Array.Resize(ref cached_data_r, remain);
                                    }
                                    cached_data_length = remain;
                                    // レンダリング結果を読み込む
                                    wr.read(offset, remain, cached_data_l, cached_data_r);
                                } catch (Exception ex) {
                                    AppManager.debugWriteLine(typeof(VConnectSynthesizer) + "#begin; (E); ex=" + ex);
                                }
                            } else {
                                // PATTERN F
                                //  ----[*************]--------------------------------->  cache
                                //  ----[*********************]------------------------->  renderd result
                                //  --------------------------------[***************...->  next rendering queue (not rendered yet)
                                //                  ||
                                //                  \/
                                //  ----[*************][******]------------------------->  append
                                //  ---------------------------------------------------->  new cache -> NULL!!
                                //  ---------------------------[***]-------------------->  append#2 (silence)
                                //  
                                try {
                                    // レンダリング結果とキャッシュをMIXして送信
                                    int remain = cached_data_length;
                                    int offset = 0;
                                    while (remain > 0) {
                                        int amount = (remain > BUFLEN) ? BUFLEN : remain;
                                        wr.read(offset, amount, bufL, bufR);
                                        for (int j = 0; j < amount; j++) {
                                            bufL[j] += cached_data_l[j + offset];
                                            bufR[j] += cached_data_r[j + offset];
                                        }
                                        if (!Rendered(bufL, bufR, amount)) {
                                            return;
                                        }
                                        remain -= amount;
                                        offset += amount;
                                    }

                                    // 残りのレンダリング結果を送信
                                    remain = rendered_length - cached_data_length;
                                    while (remain > 0) {
                                        int amount = (remain > BUFLEN) ? BUFLEN : remain;
                                        wr.read(offset, amount, bufL, bufR);
                                        if (!Rendered(bufL, bufR, amount)) {
                                            return;
                                        }
                                        offset += amount;
                                        remain -= amount;
                                    }

                                    // 無音部分を送信
                                    remain = (int)(next_wave_start - (queue.startSample + rendered_length));
                                    for (int j = 0; j < BUFLEN; j++) {
                                        bufL[j] = 0;
                                        bufR[j] = 0;
                                    }
                                    while (remain > 0) {
                                        int amount = (remain > BUFLEN) ? BUFLEN : remain;
                                        if (!Rendered(bufL, bufR, amount)) {
                                            return;
                                        }
                                        remain -= amount;
                                    }

                                    // キャッシュは無くなる
                                    cached_data_length = 0;
                                } catch (Exception ex) {
                                    AppManager.debugWriteLine(typeof(VConnectSynthesizer) + "#begin; (F); ex=" + ex);
                                }
                            }
                        }
                    }
                } catch (Exception ex) {
                    serr.println(typeof(VConnectSynthesizer) + "#begin; ex=" + ex);
                }

                processed_samples += queue.abstractSamples;
            }

            // 足りない分を無音で埋める
            for (int i = 0; i < BUFLEN; i++) {
                bufL[i] = 0;
                bufR[i] = 0;
            }

            while (Rendered(bufL, bufR, BUFLEN)) ;
        }

        private void appendQueue(VsqFile vsq, int track, List<VsqEvent> events, VsqEvent singer_event)
        {
            int count = events.Count;
            if (count <= 0) {
                return;
            }
            VsqEvent current = events[0];
            VsqEvent next = null;

            string singer = singer_event.ID.IconHandle.IDS;
            int num_singers = singer_configs_.Count;
            string singer_path = "";
            for (int i = 0; i < num_singers; i++) {
                SingerConfig sc = singer_configs_[i];
                if (sc.VOICENAME.Equals(singer)) {
                    singer_path = sc.VOICEIDSTR;
                    break;
                }
            }
            // 歌手のパスが取得できないので離脱
            if (singer_path.Equals("")) {
                return;
            }
            string oto_ini = Path.Combine(singer_path, "oto.ini");
            if (!System.IO.File.Exists(oto_ini)) {
                // STRAIGHT合成用のoto.iniが存在しないので離脱
                return;
            }

            // 原音設定を取得
            UtauVoiceDB voicedb = null;
            if (voice_db_configs_.ContainsKey(oto_ini)) {
                voicedb = voice_db_configs_[oto_ini];
            } else {
                SingerConfig sc = new SingerConfig();
                sc.VOICEIDSTR = PortUtil.getDirectoryName(oto_ini);
                sc.VOICENAME = singer;
                voicedb = new UtauVoiceDB(sc);
                voice_db_configs_[oto_ini] = voicedb;
            }

            // eventsのなかから、音源が存在しないものを削除
            for (int i = count - 1; i >= 0; i--) {
                VsqEvent item = events[i];
                string search = item.ID.LyricHandle.L0.Phrase;
                OtoArgs oa = voicedb.attachFileNameFromLyric(search, item.ID.Note);
                if (oa.fileName == null || (oa.fileName != null && oa.fileName.Equals(""))) {
                    events.RemoveAt(i);
                }
            }

            List<VsqEvent> list = new List<VsqEvent>();

            count = events.Count;
            for (int i = 1; i < count + 1; i++) {
                if (i == count) {
                    next = null;
                } else {
                    next = events[i];
                }

                double current_sec_start = vsq.getSecFromClock(current.Clock) - current.UstEvent.getPreUtterance() / 1000.0;
                double current_sec_end = vsq.getSecFromClock(current.Clock + current.ID.getLength());
                double next_sec_start = double.MaxValue;
                if (next != null) {
                    // 次の音符の開始位置
                    next_sec_start = vsq.getSecFromClock(next.Clock) - current.UstEvent.getPreUtterance() / 1000.0 + current.UstEvent.getVoiceOverlap() / 1000.0;
                    if (next_sec_start < current_sec_end) {
                        // 先行発音によって，現在取り扱っている音符「current」の終了時刻がずれる.
                        current_sec_end = next_sec_start;
                    }
                }

                list.Add(current);
                // 前の音符との間隔が100ms以下なら，連続していると判断
                if (next_sec_start - current_sec_end > 0.1 && list.Count > 0) {
                    appendQueueCor(vsq, track, list, oto_ini);
                    list.Clear();
                }

                // 処理後
                current = next;
            }

            if (list.Count > 0) {
                appendQueueCor(vsq, track, list, oto_ini);
            }
        }

        /// <summary>
        /// 連続した音符を元に，StraightRenderingQueueを作成
        /// </summary>
        /// <param name="list"></param>
        private void appendQueueCor(VsqFile vsq, int track, List<VsqEvent> list, string oto_ini)
        {
            if (list.Count <= 0) {
                return;
            }

            int OFFSET = 1920;
            CurveType[] CURVE = new CurveType[]{
                CurveType.PIT,
                CurveType.PBS,
                CurveType.DYN,
                CurveType.BRE,
                CurveType.GEN,
                CurveType.CLE,
                CurveType.BRI,
            };

            VsqTrack vsq_track = (VsqTrack)vsq.Track[track].clone();
            VsqEvent ve0 = list[0];
            int first_clock = ve0.Clock;
            int last_clock = ve0.Clock + ve0.ID.getLength();
            if (list.Count > 1) {
                VsqEvent ve9 = list[list.Count - 1];
                last_clock = ve9.Clock + ve9.ID.getLength();
            }
            double start_sec = vsq.getSecFromClock(first_clock); // 最初の音符の，曲頭からの時間
            int clock_shift = OFFSET - first_clock; // 最初の音符が，ダミー・トラックのOFFSET clockから始まるようシフトする．

            // listの内容を転写
            vsq_track.MetaText.Events.clear();
            int count = list.Count;
            for (int i = 0; i < count; i++) {
                VsqEvent ev = (VsqEvent)list[i].clone();
                ev.Clock = ev.Clock + clock_shift;
                vsq_track.MetaText.Events.add(ev);
            }

            // コントロールカーブのクロックをシフトする
            count = CURVE.Length;
            for (int i = 0; i < count; i++) {
                CurveType curve = CURVE[i];
                VsqBPList work = vsq_track.getCurve(curve.getName());
                if (work == null) {
                    continue;
                }
                VsqBPList src = (VsqBPList)work.clone();
                int value_at_first_clock = work.getValue(first_clock);
                work.clear();
                work.add(0, value_at_first_clock);
                int num_points = src.size();
                for (int j = 0; j < num_points; j++) {
                    int clock = src.getKeyClock(j);
                    if (0 <= clock + clock_shift && clock + clock_shift <= last_clock + clock_shift + 1920) { // 4拍分の余裕を持って・・・
                        int value = src.getElementA(j);
                        work.add(clock + clock_shift, value);
                    }
                }
            }

            // 最後のクロックがいくつかを調べる
            int tlast_clock = 0;
            foreach (var item in vsq_track.getNoteEventIterator()) {
                tlast_clock = item.Clock + item.ID.getLength();
            }
            double abstract_sec = tlast_clock / (8.0 * TEMPO);

            VConnectRenderingQueue queue = new VConnectRenderingQueue();
            // レンダリング結果の何秒後に音符が始まるか？
            queue.startSample = (int)((start_sec - OFFSET / (8.0 * TEMPO)) * sample_rate_);
            queue.oto_ini = oto_ini;
            queue.abstractSamples = (long)(abstract_sec * sample_rate_);
            queue.endClock = last_clock + clock_shift + 1920;
            queue.track = vsq_track;
            queue_.Add(queue);
        }

        /// <summary>
        /// 合成用のメタテキストを生成します
        /// </summary>
        /// <param name="writer">テキストの出力先</param>
        /// <param name="vsq_track">出力対象のトラック</param>
        /// <param name="oto_ini">原音設定ファイルのパス</param>
        /// <param name="end_clock"></param>
        public static void prepareMetaText(StreamWriter writer, VsqTrack vsq_track, string oto_ini, int end_clock)
        {
            prepareMetaText(writer, vsq_track, oto_ini, end_clock, true);
        }

        /// <summary>
        /// 合成用のメタテキストを生成します
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="vsq_track"></param>
        /// <param name="oto_ini"></param>
        /// <param name="end_clock"></param>
        /// <param name="world_mode"></param>
        public static void prepareMetaText(StreamWriter writer, VsqTrack vsq_track, string oto_ini, int end_clock, bool world_mode)
        {
            SortedDictionary<string, string> dict_singername_otoini = new SortedDictionary<string, string>();
            dict_singername_otoini[""] = oto_ini;
            prepareMetaText(writer, vsq_track, dict_singername_otoini, end_clock, world_mode);
        }

        /// <summary>
        /// 合成用のメタテキストを生成します
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="vsq_track"></param>
        /// <param name="oto_ini"></param>
        /// <param name="end_clock"></param>
        private static void prepareMetaText(
            StreamWriter writer,
            VsqTrack vsq_track,
            SortedDictionary<string, string> dict_singername_otoini,
            int end_clock,
            bool world_mode)
        {
            CurveType[] CURVE = new CurveType[]{
                CurveType.PIT,
                CurveType.PBS,
                CurveType.DYN,
                CurveType.BRE,
                CurveType.GEN,
                CurveType.CLE,
                CurveType.BRI, };
            // メモリーストリームに出力
            try {
                writer.WriteLine("[Tempo]");
                writer.WriteLine(TEMPO + "");
                writer.WriteLine("[oto.ini]");
                foreach (var singername in dict_singername_otoini.Keys) {
                    string oto_ini = dict_singername_otoini[singername];
                    if (world_mode) {
                        writer.WriteLine(singername + "\t" + oto_ini);
                    } else {
                        writer.WriteLine(oto_ini);
                        break;
                    }
                }
                List<VsqHandle> handles = vsq_track.MetaText.writeEventList(writer, end_clock);
                List<string> print_targets = new List<string>(new string[]{ "Length",
                                                                            "Note#",
                                                                            "Dynamics",
                                                                            "DEMdecGainRate",
                                                                            "DEMaccent",
                                                                            "PreUtterance",
                                                                            "VoiceOverlap",
                                                                            "PMBendDepth",
                                                                            "PMBendLength",
                                                                            "PMbPortamentoUse", });
                vsq_track.events().ToList().ForEach(item => item.write(writer, print_targets));
                int count = handles.Count;
                for (int i = 0; i < count; i++) {
                    handles[i].write(writer);
                }
                count = CURVE.Length;
                for (int i = 0; i < count; i++) {
                    CurveType curve = CURVE[i];
                    VsqBPList src = vsq_track.getCurve(curve.getName());
                    if (src == null) {
                        continue;
                    }
                    string name = "";
                    if (curve.equals(CurveType.PIT)) {
                        name = "[PitchBendBPList]";
                    } else if (curve.equals(CurveType.PBS)) {
                        name = "[PitchBendSensBPList]";
                    } else if (curve.equals(CurveType.DYN)) {
                        name = "[DynamicsBPList]";
                    } else if (curve.equals(CurveType.BRE)) {
                        name = "[EpRResidualBPList]";
                    } else if (curve.equals(CurveType.GEN)) {
                        name = "[GenderFactorBPList]";
                    } else if (curve.equals(CurveType.BRI)) {
                        name = "[EpRESlopeBPList]";
                    } else if (curve.equals(CurveType.CLE)) {
                        name = "[EpRESlopeDepthBPList]";
                    } else {
                        continue;
                    }
                    src.print(writer, 0, name);
                }
            } catch (Exception ex) {
                Logger.write(typeof(VConnectSynthesizer) + ".prepareMetaText; ex=" + ex + "\n");
            }
        }

        public static void clearCache()
        {
            string tmp_dir = AppManager.getTempWaveDir();
            foreach (var key in cache_.Keys) {
                try {
                    PortUtil.deleteFile(Path.Combine(tmp_dir, key + ".wav"));
                } catch (Exception ex) {
                }
            }
            cache_.Clear();
        }
    }
}
