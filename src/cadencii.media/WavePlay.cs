/*
 * WavePlay.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.media.
 *
 * cadencii.media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Runtime.InteropServices;

using cadencii;

namespace cadencii.media
{

    public delegate void FirstBufferWrittenCallback();

    public unsafe class WavePlay
    {
        const int _NUM_BUF = 3;              // バッファの数
        int s_block_size;           // 1個のバッファのサイズ(サンプル)
        int s_sample_rate;          // サンプリングレート
        WAVEFORMATEX s_wave_formatx;         // WAVEファイルヘッダ
        //IntPtr s_ptr_wave_formatx;
        IntPtr s_hwave_out;            // WAVE再生デバイス

        WAVEHDR[] s_wave_header = new WAVEHDR[_NUM_BUF];// WAVEヘッダ
        IntPtr[] s_ptr_wave_header = new IntPtr[_NUM_BUF];

        uint*[] s_wave = new uint*[_NUM_BUF];     // バッファ
        IntPtr[] s_ptr_wave = new IntPtr[_NUM_BUF];

        bool[] s_done = new bool[_NUM_BUF];
        int s_current_buffer;       // 次回書き込むべきバッファのインデクス
        uint s_processed_count;      // 初回はバッファを_NUM_BUF個全部埋めなければいけないので、最初の _NUM_BUF + 1 回はカウントを行う。そのためのカウンタ
        bool s_abort_required;       // 再生の中断が要求された時立つフラグ
        int s_buffer_loc;           // 書き込み中のバッファ内の、現在位置
        bool s_playing;              // 再生中かどうかを表すフラグ
        int s_error_samples;        // appendされた波形データの内、先頭のs_error_samples分を省く。通常の使い方なら常に0だが、vocaloid2 vstiで使う場合、プリセンド分を除いてwaveOutWriteしなければいけないので非0になる。
        int s_last_buffer;          // 最後に再生されるバッファの番号。負値の場合、append_lastが未だ呼ばれていないことを意味する。
        FirstBufferWrittenCallback s_first_buffer_written_callback; // 最初のバッファが書き込まれたとき呼び出されるコールバック関数
        WaveReader[] s_wave_reader;
        int s_num_wave_reader; // s_wave_readerの個数
        float*[] s_another_wave_l;
        IntPtr[] s_ptr_another_wave_l;
        float*[] s_another_wave_r;
        IntPtr[] s_ptr_another_wave_r;
        long s_wave_read_offset_samples;
        float* s_wave_buffer_l;
        IntPtr s_ptr_wave_buffer_l;
        float* s_wave_buffer_r;
        IntPtr s_ptr_wave_buffer_r;

        delegateWaveOutProc s_wave_callback;

        /// コールバック関数
        void wave_callback(IntPtr hwo, uint uMsg, uint dwInstance, uint dwParam1, uint dwParam2)
        {
#if DEBUG
            Console.WriteLine("WavePlay.wave_callback; uMsg=" + uMsg);
#endif
            if (uMsg == win32.MM_WOM_DONE) {
                int index_done = 0;
                WAVEHDR whdr = (WAVEHDR)Marshal.PtrToStructure(new IntPtr(dwParam1), typeof(WAVEHDR));
                int dwuser = whdr.dwUser.ToInt32();
                if (dwuser >= _NUM_BUF) {
                    index_done = dwuser - _NUM_BUF;
                } else {
                    index_done = dwuser;
                }
                if (0 <= index_done && index_done < _NUM_BUF) {
                    s_done[index_done] = true;
                    if (s_last_buffer == index_done) {
                        s_playing = false;
                    }
                    if (dwuser >= _NUM_BUF) {
                        s_wave_header[index_done].dwUser = new IntPtr(index_done);
                    }
                }
            }
        }

        void append_cor(float** a_data, uint length, double amp_left, double amp_right, bool is_last_mode)
        {
#if DEBUG

            cadencii.debug.push_log("append_cor *************************************************************");
            cadencii.debug.push_log("    length=" + length);
            cadencii.debug.push_log("    s_hwave_out=0x" + Convert.ToString(s_hwave_out.ToInt32(), 16));
#endif
            s_playing = true;
            int jmax = (int)length;
            int remain = 0;
            IntPtr ptr_data = IntPtr.Zero;
            IntPtr ptr_data0 = IntPtr.Zero;
            IntPtr ptr_data1 = IntPtr.Zero;
            ptr_data = Marshal.AllocHGlobal(sizeof(float*) * 2);
            float** data = (float**)ptr_data.ToPointer();// new float*[2];
            bool cleaning_required = false;
            if (s_error_samples > 0) {
                if (s_error_samples >= length) {
                    s_error_samples -= (int)length;
                    return;
                }
                cleaning_required = true;
                int actual_length = (int)length - s_error_samples;
#if DEBUG
                cadencii.debug.push_log("    actual_length=" + actual_length);
#endif
                ptr_data0 = Marshal.AllocHGlobal(sizeof(float) * actual_length);
                ptr_data1 = Marshal.AllocHGlobal(sizeof(float) * actual_length);
                data[0] = (float*)ptr_data0.ToPointer();
                data[1] = (float*)ptr_data1.ToPointer();
                for (int i = 0; i < actual_length; i++) {
                    data[0][i] = a_data[0][i + s_error_samples];
                    data[1][i] = a_data[1][i + s_error_samples];
                }
                s_error_samples = 0;
                length = (uint)actual_length;
                jmax = (int)length;
            } else {
                data = a_data;
            }

            if (length + s_buffer_loc >= s_block_size) {
                jmax = s_block_size - s_buffer_loc;
                remain = (int)length - (int)jmax;
            }
            float aright = (float)amp_right;
            float aleft = (float)amp_left;

            for (int j = 0; j < jmax; j++) {
                s_wave_buffer_l[j + s_buffer_loc] = data[1][j];
                s_wave_buffer_r[j + s_buffer_loc] = data[0][j];
            }
            s_buffer_loc += jmax;

            if (s_buffer_loc >= s_block_size) {
                // バッファー充填完了．バッファーを転送し、waveOutWriteが書き込めるタイミングまで待機
#if DEBUG
                cadencii.debug.push_log("append_cor; waiting(1) " + s_current_buffer + "...");
#endif
                while (true) {
                    if (s_abort_required) {
                        s_abort_required = false;
                        goto clean_and_exit;
                    }
                    if (s_done[s_current_buffer]) {
                        break;
                    }
                }
#if DEBUG
                cadencii.debug.push_log("append_cor; ...exit");
#endif

                s_processed_count++;
                mix((int)s_processed_count, aleft, aright);

                if (s_processed_count == _NUM_BUF) {
                    s_done[0] = false;
#if DEBUG
                    cadencii.debug.push_log("calling waveOutWrite...; s_hawve_out=0x" + Convert.ToString(s_hwave_out.ToInt32(), 16));
#endif
                    uint ret = win32.waveOutWrite(s_hwave_out, ref s_wave_header[0], (uint)sizeof(WAVEHDR));
#if DEBUG
                    cadencii.debug.push_log("...done; ret=" + ret);
#endif
#if DEBUG
                    cadencii.debug.push_log("(s_first_buffer_wirtten_callback==null)=" + (s_first_buffer_written_callback == null));
#endif
                    if (s_first_buffer_written_callback != null) {
#if DEBUG
                        cadencii.debug.push_log("append_cor; calling s_first_buffer_written_callback");
#endif
                        s_first_buffer_written_callback();
                    }
                    for (int buffer_index = 1; buffer_index < _NUM_BUF; buffer_index++) {
                        s_done[buffer_index] = false;
#if DEBUG
                        cadencii.debug.push_log("calling waveOutWrite...; s_hawve_out=0x" + Convert.ToString(s_hwave_out.ToInt32(), 16));
#endif
                        uint ret2 = win32.waveOutWrite(s_hwave_out, ref s_wave_header[buffer_index], (uint)sizeof(WAVEHDR));
#if DEBUG
                        cadencii.debug.push_log("...done; ret2=" + ret2);
#endif
                    }
                    s_current_buffer = _NUM_BUF - 1;
                } else if (s_processed_count > _NUM_BUF) {
                    s_done[s_current_buffer] = false;
#if DEBUG
                    cadencii.debug.push_log("calling waveOutWrite...; s_hawve_out=0x" + Convert.ToString(s_hwave_out.ToInt32(), 16));
#endif
                    uint ret3 = win32.waveOutWrite(s_hwave_out, ref s_wave_header[s_current_buffer], (uint)sizeof(WAVEHDR));
#if DEBUG
                    cadencii.debug.push_log("...done; ret3=" + ret3);
#endif
                }
                s_current_buffer++;
                if (s_current_buffer >= _NUM_BUF) {
                    s_current_buffer = 0;
                }

                s_buffer_loc = 0;
            }

            if (remain > 0) {
                for (int j = jmax; j < length; j++) {
                    s_wave_buffer_l[j - jmax] = data[1][j];
                    s_wave_buffer_r[j - jmax] = data[0][j];
                }
                if (is_last_mode) {
                    for (int j = (int)length - jmax; j < s_block_size; j++) {
                        s_wave_buffer_l[j] = 0.0f;
                        s_wave_buffer_r[j] = 0.0f;
                    }
                }
                s_buffer_loc = remain;
            }

            if (is_last_mode) {
                if (s_processed_count < _NUM_BUF) {
                    // _NUM_BUFブロック分のデータを未だ全て受信していない場合。バッファが未だひとつも書き込まれていないので
                    // 0番のブロックから順に書き込む
                    s_processed_count++;
                    mix((int)s_processed_count, aleft, aright);
                    s_done[0] = false;
#if DEBUG
                    cadencii.debug.push_log("calling waveOutWrite...; s_hawve_out=0x" + Convert.ToString(s_hwave_out.ToInt32(), 16));
#endif
                    uint ret35 = win32.waveOutWrite(s_hwave_out, ref s_wave_header[0], (uint)sizeof(WAVEHDR));
#if DEBUG
                    cadencii.debug.push_log("...done; ret35=" + ret35);
                    cadencii.debug.push_log("(s_first_buffer_written_callback==null)=" + (s_first_buffer_written_callback == null));
#endif
                    if (s_first_buffer_written_callback != null) {
#if DEBUG
                        cadencii.debug.push_log("append_cor; calling s_first_buffer_written_callback");
#endif
                        s_first_buffer_written_callback();
                    }
                    for (int i = 1; i < _NUM_BUF - 1; i++) {
                        s_processed_count++;
                        mix((int)s_processed_count, aleft, aright);
                        s_done[i] = false;
#if DEBUG
                        cadencii.debug.push_log("calling waveOutWrite...; s_hawve_out=0x" + Convert.ToString(s_hwave_out.ToInt32(), 16));
#endif
                        uint ret36 = win32.waveOutWrite(s_hwave_out, ref s_wave_header[i], (uint)sizeof(WAVEHDR));
#if DEBUG
                        cadencii.debug.push_log("...done; ret36=" + ret36);
#endif
                    }
                }
                ulong zero = MAKELONG(0, 0);
                for (int j = s_buffer_loc; j < s_block_size; j++) {
                    s_wave_buffer_l[j] = 0.0f;
                    s_wave_buffer_r[j] = 0.0f;
                }
#if DEBUG
                cadencii.debug.push_log("append_cor; waiting(3) " + s_current_buffer + "...");
#endif
                while (!s_done[s_current_buffer]) {
                    if (s_abort_required) {
                        s_abort_required = false;
                        goto clean_and_exit;
                    }
                }
#if DEBUG
                cadencii.debug.push_log("append_cor; ...exit");
#endif
                s_processed_count++;
                mix((int)s_processed_count, aleft, aright);
                s_done[s_current_buffer] = false;
#if DEBUG
                cadencii.debug.push_log("calling waveOutWrite...; s_hawve_out=0x" + Convert.ToString(s_hwave_out.ToInt32(), 16));
#endif
                uint ret4 = win32.waveOutWrite(s_hwave_out, ref s_wave_header[s_current_buffer], (uint)sizeof(WAVEHDR));
#if DEBUG
                cadencii.debug.push_log("...done; ret4=" + ret4);
#endif
            }
        clean_and_exit:
            if (is_last_mode) {
                s_last_buffer = s_current_buffer;
            }
            if (cleaning_required) {
                Marshal.FreeHGlobal(ptr_data0); //delete [] data[0];
                Marshal.FreeHGlobal(ptr_data1); //delete [] data[1];
                Marshal.FreeHGlobal(ptr_data); //delete [] data;
            }
        }

        void mix(int processed_count, float amp_left, float amp_right)
        {
            int current_buffer = (processed_count - 1) % _NUM_BUF;
            for (int k = 0; k < s_num_wave_reader; k++) {
                s_wave_reader[k].read(s_block_size * (processed_count - 1) + (int)s_wave_read_offset_samples,
                                       s_block_size,
                                       ref s_ptr_another_wave_l[k],
                                       ref s_ptr_another_wave_r[k]);
            }
            for (int i = 0; i < s_block_size; i++) {
                float l = s_wave_buffer_l[i] * amp_left;
                float r = s_wave_buffer_r[i] * amp_right;
                for (int k = 0; k < s_num_wave_reader; k++) {
                    l += s_another_wave_l[k][i];
                    r += s_another_wave_r[k][i];
                }
                s_wave[current_buffer][i] = MAKELONG((ushort)(r * 32768.0f), (ushort)(l * 32768.0f));
            }
        }

        string util_get_errmsg(uint msg)
        {
            //IntPtr ptr_err = Marshal.AllocHGlobal( sizeof( byte ) * 260 );
            //byte* err = (byte*)ptr_err.ToPointer();
            //System.Text.StringBuilder sb = new System.Text.StringBuilder( 260 );
            string ret = "";
            win32.mciGetErrorStringA(msg, ret, 260);
            /*int len = 260;
            for ( int i = 1; i < 260; i++ ) {
                if ( err[i] == '\0' ) {
                    len = i - 1;
                    break;
                }
            }*/
            //string ret = new string( err );
            //string ret = sb.ToString();
            //Marshal.FreeHGlobal( ptr_err );
            return ret;
        }

        private WavePlay()
        {
        }

        /// 初期化関数
        public WavePlay(int block_size, int sample_rate)
        {
#if DEBUG
            Console.WriteLine("waveplay..ctor");
#endif
            s_block_size = block_size;
            s_sample_rate = sample_rate;

            //s_ptr_wave_formatx = Marshal.AllocHGlobal( sizeof( WAVEFORMATEX ) );
            s_wave_formatx = new WAVEFORMATEX();
            //Marshal.PtrToStructure( s_ptr_wave_formatx, s_wave_formatx );
            s_wave_formatx.cbSize = (ushort)sizeof(WAVEFORMATEX);
#if DEBUG
            Console.WriteLine("    s_wave_fomratx.cbSize=" + s_wave_formatx.cbSize);
            Console.WriteLine("    sizeof( WAVEHDR )=" + sizeof(WAVEHDR));
#endif
            s_wave_formatx.wFormatTag = win32.WAVE_FORMAT_PCM;
            s_wave_formatx.nChannels = 2;
            s_wave_formatx.wBitsPerSample = 16;
            s_wave_formatx.nBlockAlign = (ushort)(s_wave_formatx.nChannels * s_wave_formatx.wBitsPerSample / 8);
            s_wave_formatx.nSamplesPerSec = (uint)s_sample_rate;
            s_wave_formatx.nAvgBytesPerSec = s_wave_formatx.nSamplesPerSec * s_wave_formatx.nBlockAlign;

            s_wave_callback = new delegateWaveOutProc(wave_callback);
            s_hwave_out = IntPtr.Zero;
            Console.WriteLine("    calling waveOutOpen...");
            uint ret = win32.waveOutOpen(ref s_hwave_out,
                                            win32.WAVE_MAPPER,
                                            ref s_wave_formatx,
                                            s_wave_callback,
                                            IntPtr.Zero,
                                            (uint)win32.CALLBACK_FUNCTION);
            Console.WriteLine("    ...done; ret=" + ret);
#if DEBUG
            cadencii.debug.push_log("    s_hwave_out=0x" + Convert.ToString(s_hwave_out.ToInt32(), 16));
#endif

            for (int k = 0; k < _NUM_BUF; k++) {
                s_ptr_wave[k] = Marshal.AllocHGlobal(sizeof(uint) * s_block_size);
                s_wave[k] = (uint*)s_ptr_wave[k];// = (ulong*)calloc( sizeof( ulong ), s_block_size );
                s_ptr_wave_header[k] = Marshal.AllocHGlobal(sizeof(WAVEHDR));
                s_wave_header[k] = (WAVEHDR)Marshal.PtrToStructure(s_ptr_wave_header[k], typeof(WAVEHDR));
                s_wave_header[k].lpData = s_ptr_wave[k];
                s_wave_header[k].dwBufferLength = (uint)(sizeof(uint) * s_block_size);
                s_wave_header[k].dwFlags = win32.WHDR_BEGINLOOP | win32.WHDR_ENDLOOP;
                s_wave_header[k].dwLoops = 1;

#if DEBUG
                Console.WriteLine("calling waveOutPrepareHeader...");
#endif
                uint ret2 = win32.waveOutPrepareHeader(s_hwave_out, ref s_wave_header[k], (uint)sizeof(WAVEHDR));
#if DEBUG
                Console.WriteLine("...done; ret2=" + ret2);
#endif
                s_wave_header[k].dwUser = new IntPtr(k);
            }
#if DEBUG
            cadencii.debug.push_log("   exit waveplay..ctor; s_hwave_out=0x" + Convert.ToString(s_hwave_out.ToInt32(), 16));
#endif
        }

        /// 波形データをバッファに追加する。バッファが再生中などの理由で即座に書き込めない場合、バッファが書き込み可能となるまで待機させられる
        public void append(float** data, uint length, double amp_left, double amp_right)
        {
            append_cor(data, length, amp_left, amp_right, false);
        }

        public void flush_and_exit(double amp_left, double amp_right)
        {
            append_cor((float**)0, 0, amp_left, amp_right, true);
        }

        /// 再生中断を要求する
        public void abort()
        {
            s_abort_required = true;
            reset();
            for (int k = 0; k < _NUM_BUF; k++) {
                if (s_ptr_wave[k] != IntPtr.Zero) {
                    for (int i = 0; i < s_block_size; i++) {
                        s_wave[k][i] = 0;
                    }
                    //memset( s_wave[k], 0, s_block_size * sizeof( ulong ) );
                }
            }
            s_buffer_loc = 0;
            s_current_buffer = 0;
            s_processed_count = 0;
        }

        /// 現在の再生位置を取得する。再生中でない場合負の値となる。
        public float get_play_time()
        {
#if DEBUG
            cadencii.debug.push_log("WavePlay.get_play_time");
#endif
            if (s_playing) {
                MMTIME mmt = new MMTIME();
                mmt.cb = (uint)sizeof(MMTIME);
                mmt.wType = win32.TIME_MS;
                uint ret = win32.waveOutGetPosition(s_hwave_out, ref mmt, (uint)sizeof(MMTIME));
#if DEBUG
                cadencii.debug.push_log("    ret=" + ret);
#endif
                float ms = 0.0f;
                switch (mmt.wType) {
                    case win32.TIME_MS:
                    return mmt.ms * 0.001f;
                    case win32.TIME_SAMPLES:
                    return (float)mmt.sample / (float)s_wave_formatx.nSamplesPerSec;
                    case win32.TIME_BYTES:
                    return (float)mmt.cb / (float)s_wave_formatx.nAvgBytesPerSec;
                    default:
                    return -1.0f;
                }
            } else {
                return -1.0f;
            }
        }

        /// リセットする。abort関数でも呼び出される。
        public void reset()
        {
            s_playing = false;
            if (s_hwave_out.ToInt32() != 0) {
                for (int k = 0; k < _NUM_BUF; k++) {
                    s_wave_header[k].dwUser = new IntPtr(_NUM_BUF + k);
                }
                win32.waveOutReset(s_hwave_out);
                uint zero = MAKELONG(0, 0);
                for (int k = 0; k < _NUM_BUF; k++) {
                    for (int i = 0; i < s_block_size; i++) {
                        s_wave[k][i] = zero;
                    }
                }
            }
            for (int i = 0; i < s_num_wave_reader; i++) {
                s_wave_reader[i].close();
            }
        }

        /// 再生のための準備を行う。この関数を呼び出した後は、バッファが再生開始されるまでget_play_timeの戻り値は0となる（負値にならない）。
        /// 戻り値は、filesに指定されたファイルの内、最も再生時間の長いwaveファイルの、合計サンプル数
        public int on_your_mark(string[] files, long wave_read_offset_samples)
        {
#if DEBUG
            cadencii.debug.push_log("on_your_mark; s_hwave_out=0x" + Convert.ToString(s_hwave_out.ToInt32(), 16));
#endif
            int num_files = files.Length;
            reset();
            s_wave_read_offset_samples = wave_read_offset_samples;
            for (int k = 0; k < _NUM_BUF; k++) {
                s_wave_header[k].dwUser = new IntPtr(k);
                s_done[k] = true;
            }
            s_abort_required = false;
            s_buffer_loc = 0;
            s_current_buffer = 0;
            s_processed_count = 0;
            s_playing = true;
            s_last_buffer = -1;

            if ((int)s_ptr_wave_buffer_l.ToPointer() == 0) {
                s_ptr_wave_buffer_l = Marshal.AllocHGlobal(sizeof(float) * s_block_size);//        s_wave_buffer_l = new float[s_block_size];
                s_wave_buffer_l = (float*)s_ptr_wave_buffer_l.ToPointer();
            }
            if ((int)s_ptr_wave_buffer_r.ToPointer() == 0) {
                s_ptr_wave_buffer_r = Marshal.AllocHGlobal(sizeof(float) * s_block_size); //s_wave_buffer_r = new float[s_block_size];
                s_wave_buffer_r = (float*)s_ptr_wave_buffer_r.ToPointer();
            }

            if (s_wave_reader != null) {
                for (int i = 0; i < s_num_wave_reader; i++) {
                    s_wave_reader[i].close();
                }
                //delete [] s_wave_reader;
            }
            s_wave_reader = new WaveReader[num_files];

            if (s_another_wave_l != null) {
                for (int i = 0; i < s_num_wave_reader; i++) {
                    Marshal.FreeHGlobal(s_ptr_another_wave_l[i]);// delete [] s_another_wave_l[i];
                }
                //delete [] s_another_wave_l;
            }
            if (s_another_wave_r != null) {
                for (int i = 0; i < s_num_wave_reader; i++) {
                    Marshal.FreeHGlobal(s_ptr_another_wave_r[i]); //delete [] s_another_wave_r[i];
                }
                //delete [] s_another_wave_r;
            }
            s_another_wave_l = new float*[num_files];
            s_another_wave_r = new float*[num_files];
            int max_samples = 0;
            for (int i = 0; i < num_files; i++) {
                // waveファイルヘッダを読込む
                /*int len = files[i].Length;
                wchar_t *name = new wchar_t[len + 1];
                array<wchar_t> ^buf = files[i]->ToCharArray();
                for( int k = 0; k < len; k++ ){
                    name[k] = buf[k];
                }
                name[len] = '\0';*/
                s_wave_reader[i].open(files[i]);
                int samples = s_wave_reader[i].getTotalSamples();
                if (samples > max_samples) {
                    max_samples = samples;
                }
                //delete [] name;

                // バッファを用意
                s_ptr_another_wave_l[i] = Marshal.AllocHGlobal(sizeof(float) * s_block_size);
                s_another_wave_l[i] = (float*)s_ptr_another_wave_l[i].ToPointer();
                s_ptr_another_wave_r[i] = Marshal.AllocHGlobal(sizeof(float) * s_block_size);
                s_another_wave_r[i] = (float*)s_ptr_another_wave_r[i].ToPointer();
                //s_another_wave_l[i] = new float[s_block_size];
                //s_another_wave_r[i] = new float[s_block_size];
            }
            s_num_wave_reader = num_files;
            return max_samples;
        }

        public void set_error_samples(int error_samples)
        {
            s_error_samples = error_samples;
        }

        /// コールバック関数を設定する
        public void set_first_buffer_written_callback(FirstBufferWrittenCallback proc)
        {
            s_first_buffer_written_callback = proc;
        }

        public void terminate()
        {
            if (s_hwave_out.ToInt32() != 0) {
                win32.waveOutReset(s_hwave_out);
#if DEBUG
                cadencii.debug.push_log("waveplay::terminate; waveOutReset");
#endif
                for (int k = 0; k < _NUM_BUF; k++) {
                    win32.waveOutUnprepareHeader(s_hwave_out, ref s_wave_header[k], (uint)sizeof(WAVEHDR));
                }
                win32.waveOutClose(s_hwave_out);
            }
            for (int i = 0; i < _NUM_BUF; i++) {
                if (s_ptr_wave[i].ToInt32() != 0) {
                    Marshal.FreeHGlobal(s_ptr_wave[i]); //delete [] s_wave[i];
                }
            }
        }

        /// 現在再生中かどうかを取得する
        public bool is_alive()
        {
            return s_playing;
        }

        /// ブロックサイズを変更します
        public bool change_block_size(int block_size)
        {
            if (s_playing) {
                return false;
            }
            if (block_size <= 0) {
                return false;
            }

            for (int k = 0; k < _NUM_BUF; k++) {
                if (s_ptr_wave[k].ToInt32() != 0) {
                    Marshal.FreeHGlobal(s_ptr_wave[k]);// delete [] s_wave[k];
                }
                s_ptr_wave[k] = Marshal.AllocHGlobal(sizeof(uint) * block_size);
                s_wave[k] = (uint*)s_ptr_wave[k].ToPointer();// calloc( sizeof( ulong ), block_size );
                s_wave_header[k].lpData = s_ptr_wave[k];
                s_wave_header[k].dwBufferLength = (uint)(sizeof(uint) * block_size);
            }

            // s_wave_buffer_l, s_wave_buffer_rは、NULLならばon_your_markで初期化されるので、開放だけやっておけばOK
            if (s_ptr_wave_buffer_l.ToInt32() != 0) {
                Marshal.FreeHGlobal(s_ptr_wave_buffer_l); //delete [] s_wave_buffer_l;
            }
            if (s_ptr_wave_buffer_r.ToInt32() != 0) {
                Marshal.FreeHGlobal(s_ptr_wave_buffer_r); //delete[] s_wave_buffer_r;
            }
            // s_another_wave_l, s_another_wave_rは、on_your_markで全自動で初期化されるので特に操作の必要なし
            s_block_size = block_size;
            return true;
        }

        uint MAKELONG(ushort a, ushort b)
        {
            return (uint)(a & 0xffff) | (uint)((b & 0xffff) << 16);
        }
    }

}
