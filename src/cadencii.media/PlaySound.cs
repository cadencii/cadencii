/*
 * PlaySound.cs
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
#define USE_PLAYSOUND_DLL

using System;
using System.Runtime.InteropServices;
using cadencii;

namespace cadencii.media
{
    using DWORD = System.UInt32;
    using UINT = System.UInt32;
    using WORD = System.UInt16;

    public class PlaySound
    {
#if USE_PLAYSOUND_DLL
        [DllImport("cadencii.media.helper.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SoundInit();
        [DllImport("cadencii.media.helper.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SoundPrepare(int sample_rate);
        [DllImport("cadencii.media.helper.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SoundAppend(IntPtr left, IntPtr right, int length);
        [DllImport("cadencii.media.helper.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SoundAppendInterleaved(IntPtr stream, int length);
        [DllImport("cadencii.media.helper.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SoundExit();
        [DllImport("cadencii.media.helper.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double SoundGetPosition();
        /*[DllImport("cadencii.media.helper.dll")]
        private static extern bool SoundIsBusy();*/
        [DllImport("cadencii.media.helper.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SoundWaitForExit();
        [DllImport("cadencii.media.helper.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SoundSetResolution(int resolution);
        [DllImport("cadencii.media.helper.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SoundKill();
        [DllImport("cadencii.media.helper.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SoundUnprepare();
#else
//#error 途中で詰まる場合があるので使わないでね(org.kbinani.cadencii.PlaySound)
        private static void SoundInit() {
            impl.PlaySound.SoundInit();
        }

        private static void SoundPrepare( int sample_rate ) {
            impl.PlaySound.SoundPrepare( sample_rate );
        }

        private static void SoundAppend( IntPtr left, IntPtr right, int length ) {
            unsafe {
                impl.PlaySound.SoundAppend( (double*)left.ToPointer(), (double*)right.ToPointer(), length );
            }
        }

        private static void SoundExit() {
            impl.PlaySound.SoundExit();
        }

        private static double SoundGetPosition() {
            return impl.PlaySound.SoundGetPosition();
        }

        private static void SoundWaitForExit() {
            impl.PlaySound.SoundWaitForExit();
        }

        private static void SoundSetResolution( int resolution ) {
            impl.PlaySound.SoundSetResolution( resolution );
        }

        private static void SoundKill() {
            impl.PlaySound.SoundKill();
        }

        private static void SoundUnprepare() {
            impl.PlaySound.SoundUnprepare();
        }
#endif

        private static bool _is_initialized = false;

        public static void setResolution(int value)
        {
            try {
                SoundSetResolution(value);
            } catch (Exception ex) {
                serr.println("PlaySound#setResolution; ex=" + ex);
            }
        }

        public static void init()
        {
            if (_is_initialized) {
                return;
            }
            try {
                SoundInit();
                _is_initialized = true;
            } catch (Exception ex) {
                sout.println("PlaySound#init; ex=" + ex);
            }
        }

        public static void kill()
        {
            try {
                SoundKill();
            } catch (Exception ex) {
                sout.println("PlaySound#kill; ex=" + ex);
            }
        }

        public static double getPosition()
        {
            try {
                return SoundGetPosition();
            } catch (Exception ex) {
                sout.println("PlaySound#getPosition; ex=" + ex);
                return 0.0;
            }
        }

        public static void waitForExit()
        {
            try {
                SoundWaitForExit();
            } catch (Exception ex) {
                sout.println("PlaySound#waitForExit; ex=" + ex);
            }
        }

        public static void append(double[] left, double[] right, int length)
        {
            try {
                IntPtr l = Marshal.UnsafeAddrOfPinnedArrayElement(left, 0);
                IntPtr r = Marshal.UnsafeAddrOfPinnedArrayElement(right, 0);
                SoundAppend(l, r, length);
            } catch (Exception ex) {
                sout.println("PlaySound#append; ex=" + ex);
            }
        }

        public static void appendInterleaved(float[] stream, int length)
        {
            try {
                IntPtr p = Marshal.UnsafeAddrOfPinnedArrayElement(stream, 0);
                SoundAppendInterleaved(p, length);
            } catch {}
        }

        /// <summary>
        /// デバイスを初期化する
        /// </summary>
        /// <param name="sample_rate"></param>
        public static void prepare(int sample_rate)
        {
            try {
                int ret = SoundPrepare(sample_rate);
#if DEBUG
                sout.println("PlaySound#prepare; ret=" + ret);
#endif
            } catch (Exception ex) {
                sout.println("PlaySound#prepare; ex=" + ex);
            }
        }

        /// <summary>
        /// 再生をとめる。
        /// </summary>
        public static void exit()
        {
            try {
                SoundExit();
            } catch (Exception ex) {
                sout.println("PlaySound#exit; ex=" + ex);
            }
        }

        public static void unprepare()
        {
            try {
                SoundUnprepare();
            } catch (Exception ex) {
                sout.println("PlaySound#unprepare; ex=" + ex);
            }
        }
    }

}

namespace cadencii.media.impl
{
    using DWORD = System.UInt32;
    using UINT = System.UInt32;
    using WORD = System.UInt16;

    /// <summary>
    /// waveOutをC#から直接呼ぶことで実装したPlaySound。
    /// org.kbinani.media.PlaySoundと互換性があるが、たぶんクラッシュしたと思うので使っちゃだめ。
    /// </summary>
    public static unsafe class PlaySound
    {
        const int NUM_BUF = 3;
        static IntPtr wave_out = IntPtr.Zero;
        static WAVEFORMATEX wave_format;
        static WAVEHDR[] wave_header = new WAVEHDR[NUM_BUF];
        static DWORD*[] wave = new DWORD*[NUM_BUF];
        static bool[] wave_done = new bool[NUM_BUF];
        static int buffer_index = 0; // 次のデータを書き込むバッファの番号
        static int buffer_loc = 0; // 次のデータを書き込む位置
        static IntPtr locker; // CRITICAL_SECTION
        static bool abort_required;
        static int block_size = 4410; // ブロックサイズ
        static int block_size_used; // SoundPrepareで初期化されたブロックサイズ
        static delegateWaveOutProc proc = null;

        public static void SoundUnprepare()
        {
            if (IntPtr.Zero == wave_out) {
                return;
            }

            win32.EnterCriticalSection(ref locker);
            for (int i = 0; i < NUM_BUF; i++) {
                win32.waveOutUnprepareHeader(
                    wave_out,
                    ref wave_header[i],
                    (uint)sizeof(WAVEHDR));
                Marshal.FreeHGlobal(wave_header[i].lpData);
            }
            win32.waveOutClose(wave_out);
            wave_out = IntPtr.Zero;
            win32.LeaveCriticalSection(ref locker);
        }

        public static void SoundInit()
        {
            locker = Marshal.AllocHGlobal(sizeof(CRITICAL_SECTION));
            win32.InitializeCriticalSection(ref locker);
            proc = new delegateWaveOutProc(SoundCallback);
        }

        public static void SoundKill()
        {
            SoundExit();
            win32.DeleteCriticalSection(ref locker);
        }

        public static double SoundGetPosition()
        {
            if (IntPtr.Zero == wave_out) {
                return -1.0;
            }

            MMTIME mmt = new MMTIME();
            mmt.wType = win32.TIME_MS;
            win32.waveOutGetPosition(wave_out, ref mmt, (uint)sizeof(MMTIME));
            float ms = 0.0f;
            switch (mmt.wType) {
                case win32.TIME_MS:
                return mmt.ms * 0.001;
                case win32.TIME_SAMPLES:
                return (double)mmt.sample / (double)wave_format.nSamplesPerSec;
                case win32.TIME_BYTES:
                return (double)mmt.cb / (double)wave_format.nAvgBytesPerSec;
                default:
                return -1.0;
            }
            return 0.0;
        }

        public static void SoundWaitForExit()
        {
            if (IntPtr.Zero == wave_out) {
                return;
            }

            win32.EnterCriticalSection(ref locker);
            // buffer_indexがNUM_BUF未満なら、まだ1つもwaveOutWriteしていないので、書き込む
            if (buffer_index < NUM_BUF) {
                for (int i = 0; i < buffer_index; i++) {
                    if (abort_required)
                        break;
                    wave_done[i] = false;
                    win32.waveOutWrite(wave_out, ref wave_header[i], (uint)sizeof(WAVEHDR));
                }
            }

            // まだ書き込んでないバッファがある場合、残りを書き込む
            if (buffer_loc != 0) {
                int act_buffer_index = buffer_index % NUM_BUF;

                // バッファが使用中の場合、使用終了となるのを待ち受ける
                while (!wave_done[act_buffer_index]) {
                    if (abort_required)
                        break;
                    System.Threading.Thread.Sleep(0);
                }

                if (!abort_required) {
                    // 後半部分を0で埋める
                    for (int i = buffer_loc; i < block_size_used; i++) {
                        wave[act_buffer_index][i] = 0;// MAKELONG( 0, 0 );
                    }

                    buffer_loc = 0;
                    buffer_index++;

                    wave_done[act_buffer_index] = false;
                    win32.waveOutWrite(wave_out, ref wave_header[act_buffer_index], (uint)sizeof(WAVEHDR));
                }
            }

            // NUM_BUF個のバッファすべてがwave_doneとなるのを待つ。
            while (!abort_required) {
                bool all_done = true;
                for (int i = 0; i < NUM_BUF; i++) {
                    if (!wave_done[i]) {
                        all_done = false;
                        break;
                    }
                }
                if (all_done) {
                    break;
                }
            }
            win32.LeaveCriticalSection(ref locker);

            // リセット処理
            SoundExit();
        }

        public static void SoundSetResolution(int resolution)
        {
            block_size = resolution;
        }

        public static void SoundAppend(double* left, double* right, int length)
        {
            if (IntPtr.Zero == wave_out) {
                return;
            }
            win32.EnterCriticalSection(ref locker);
            int appended = 0; // 転送したデータの個数
            while (appended < length) {
                // このループ内では、バッファに1個づつデータを転送する

                // バッファが使用中の場合、使用終了となるのを待ち受ける
                int act_buffer_index = buffer_index % NUM_BUF;
                while (!wave_done[act_buffer_index] && !abort_required) {
                    System.Threading.Thread.Sleep(0);
                }

                int t_length = block_size_used - buffer_loc; // 転送するデータの個数
                if (t_length > length - appended) {
                    t_length = length - appended;
                }
                for (int i = 0; i < t_length && !abort_required; i++) {
                    wave[act_buffer_index][buffer_loc + i] = (uint)win32.MAKELONG((WORD)(left[appended + i] * 32768.0), (WORD)(right[appended + i] * 32768.0));
                }
                appended += t_length;
                buffer_loc += t_length;
                if (buffer_loc == block_size_used) {
                    // バッファがいっぱいになったようだ
                    buffer_index++;
                    buffer_loc = 0;
                    if (buffer_index >= NUM_BUF) {
                        // 最初のNUM_BUF個のバッファは、すべてのバッファに転送が終わるまで
                        // waveOutWriteしないようにしているので、ここでwaveOutWriteする。
                        if (buffer_index == NUM_BUF) {
                            for (int i = 0; i < NUM_BUF; i++) {
                                if (abort_required)
                                    break;
                                wave_done[i] = false;
                                win32.waveOutWrite(wave_out, ref wave_header[i], (uint)sizeof(WAVEHDR));
                            }
                        } else {
                            wave_done[act_buffer_index] = false;
                            if (!abort_required) {
                                win32.waveOutWrite(wave_out, ref wave_header[act_buffer_index], (uint)sizeof(WAVEHDR));
                            }
                        }
                    }
                }
            }
            win32.LeaveCriticalSection(ref locker);
        }

        /// <summary>
        /// コールバック関数。バッファの再生終了を検出するために使用。
        /// </summary>
        /// <param name="hwo"></param>
        /// <param name="uMsg"></param>
        /// <param name="dwInstance"></param>
        /// <param name="dwParam1"></param>
        /// <param name="dwParam2"></param>
        public static void SoundCallback(
            IntPtr hwo,
            UINT uMsg,
            DWORD dwInstance,
            DWORD dwParam1,
            DWORD dwParam2)
        {
            if (uMsg != win32.MM_WOM_DONE) {
                return;
            }

            for (int i = 0; i < NUM_BUF; i++) {
                fixed (WAVEHDR* p = &wave_header[i]) {
                    if (p != (WAVEHDR*)dwParam1) {
                        continue;
                    }
                }
                wave_done[i] = true;
                break;
            }
        }

        /// <summary>
        /// デバイスを初期化する
        /// </summary>
        /// <param name="sample_rate"></param>
        public static void SoundPrepare(int sample_rate)
        {
            // デバイスを使用中の場合、使用を停止する
            if (IntPtr.Zero != wave_out) {
                SoundExit();
                SoundUnprepare();
            }

            win32.EnterCriticalSection(ref locker);
            // フォーマットを指定
            wave_format.wFormatTag = win32.WAVE_FORMAT_PCM;
            wave_format.nChannels = 2;
            wave_format.wBitsPerSample = 16;
            wave_format.nBlockAlign
                = (ushort)(wave_format.nChannels * wave_format.wBitsPerSample / 8);
            wave_format.nSamplesPerSec = (uint)sample_rate;
            wave_format.nAvgBytesPerSec
                = wave_format.nSamplesPerSec * wave_format.nBlockAlign;

            // デバイスを開く
            win32.waveOutOpen(ref wave_out,
                         win32.WAVE_MAPPER,
                         ref wave_format,
                         proc,
                         IntPtr.Zero,
                         win32.CALLBACK_FUNCTION);

            // バッファを準備
            block_size_used = block_size;
            for (int i = 0; i < NUM_BUF; i++) {
                IntPtr p = Marshal.AllocHGlobal((int)(sizeof(DWORD) * block_size_used));
                wave[i] = (DWORD*)p.ToPointer();
                wave_header[i].lpData = p;//                (LPSTR)wave[i];
                wave_header[i].dwBufferLength = (uint)(sizeof(DWORD) * block_size_used);
                wave_header[i].dwFlags = win32.WHDR_BEGINLOOP | win32.WHDR_ENDLOOP;
                wave_header[i].dwLoops = 1;
                win32.waveOutPrepareHeader(wave_out, ref wave_header[i], (uint)sizeof(WAVEHDR));

                wave_done[i] = true;
            }

            buffer_index = 0;
            buffer_loc = 0;
            abort_required = false;

            win32.LeaveCriticalSection(ref locker);
        }

        /// <summary>
        /// 再生をとめる。
        /// </summary>
        public static void SoundExit()
        {
            if (IntPtr.Zero != wave_out) {
                abort_required = true;
                win32.EnterCriticalSection(ref locker);
                win32.waveOutReset(wave_out);
                win32.LeaveCriticalSection(ref locker);
            }
        }

    }
}
