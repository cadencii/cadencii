/*
 * Wave.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Lib.Media.
 *
 * Boare.Lib.Media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Collections.Generic;

using bocoree;

namespace Boare.Lib.Media {

    public enum Channel {
        Right,
        Left,
    }

    public enum WaveChannel : int {
        Monoral = 1,
        Stereo = 2
    }

    /// <summary>
    /// Parameters of first formanto detection algorithm
    /// </summary>
    public class FormantoDetectionArguments {
        private double m_peak_detection_threashold = 0.2;
        private double m_moving_average_width = 25.0;

        /// <summary>
        /// ピークを検出する閾値。フーリエ変換後の周波数の強度分布関数の値がこの値よりも大きい箇所で
        /// 密度関数の導関数の符号が正から負へ変わる位置をピークとみなす。周波数密度関数は、最大値で
        /// 規格化したのを使う。従って、0～1の値を指定する。
        /// </summary>
        public double PeakDetectionThreshold {
            get {
                return m_peak_detection_threashold;
            }
            set {
                m_peak_detection_threashold = value;
            }
        }

        /// <summary>
        /// フーリエ変換した生の周波数の強度分布を平滑化する幅を2で割った値。Hzで指定。
        /// </summary>
        public double MovingAverageWidth {
            get {
                return m_moving_average_width;
            }
            set {
                m_moving_average_width = value;
            }
        }
    }

    public class Wave : IDisposable {
        private WaveChannel m_channel;
        private ushort m_bit_per_sample;
        private uint m_sample_rate;
        private uint m_total_samples;
        private byte[] L8;
        private byte[] R8;
        private short[] L16;
        private short[] R16;

        //public void 
#if DEBUG
        private static bool s_test = false;
        public static bool TestEnabled {
            get {
                return s_test;
            }
            set {
                s_test = value;
            }
        }
#endif
        public unsafe double TEST_GetF0( uint time, double[] window_function ) {
            int window_size = window_function.Length;
            double[] formanto = GetFormanto( time, window_function );
#if DEBUG
            if ( s_test ) {
                using ( StreamWriter sw = new StreamWriter( @"C:\TEST_GetF0_formanto.txt" ) ) {
                    for ( int i = 0; i < formanto.Length; i++ ) {
                        sw.WriteLine( Math.Abs( formanto[i] ) );
                    }
                }
            }
#endif
            int size = formanto.Length;
            double[] wv = new double[size + 1];
            for ( int i = 0; i < size; i++ ) {
                wv[i] = formanto[i];
            }
            int nmaxsqrt = (int)Math.Sqrt( size );
            int[] ip_ = new int[nmaxsqrt + 2];
            double[] w_ = new double[size * 5 / 4];
            ip_[0] = 0;

            fixed ( int* ip = &ip_[0] )
            fixed ( double* w = &w_[0] )
            fixed ( double* a = &wv[0] ) {
                fft.rdft( size, 1, a, ip, w );
            }
#if DEBUG
            if ( s_test ) {
                using ( StreamWriter sw = new StreamWriter( @"C:\TEST_GetF0_fft_formanto.txt" ) ) {
                    for ( int i = 0; i < wv.Length; i++ ) {
                        sw.WriteLine( Math.Abs( wv[i] ) );
                    }
                }
            }
#endif
            return 0;
        }

        public double GetF0( uint time, double[] window_function ) {
            return GetF0( time, window_function, new FormantoDetectionArguments() );
        }

        /// <summary>
        /// 第timeサンプルにおけるフォルマントを取得する
        /// </summary>
        /// <param name="time"></param>
        /// <param name="window_function"></param>
        /// <returns></returns>
        public double GetF0(  uint time, double[] window_function, FormantoDetectionArguments argument ) {
            int window_size = window_function.Length;
            double[] formanto = GetFormanto( time, window_function );
            int length = formanto.Length;
            double max = 0.0;
            for ( int i = 0; i < length; i++ ) {
                formanto[i] = Math.Abs( formanto[i] );
                max = Math.Max( max, formanto[i] );
            }
            double inv_max = 1.0 / max;
            for ( int i = 0; i < length; i++ ) {
                formanto[i] = formanto[i] * inv_max;
            }

            double hz_from_index = 1.0 / (double)window_size * m_sample_rate * 0.5;
#if DEBUG
            Console.WriteLine( "Wave+GetF0" );
            if ( s_test ) {
                using ( StreamWriter sw = new StreamWriter( "formanto.txt" ) ) {
                    for ( int i = 0; i < length; i++ ) {
                        sw.WriteLine( (i * hz_from_index) + "\t" + formanto[i] );
                    }
                }
            }
#endif
            // 移動平均を計算
            double ma_width = argument.MovingAverageWidth; // 移動平均を取るHzを2で割ったもの
            int ma_width_sample = (int)(window_size * 2 * ma_width / (double)m_sample_rate);
            if ( ma_width_sample < 1 ) {
                ma_width_sample = 1;
            }
#if DEBUG
            Console.WriteLine( "ma_width_sample=" + ma_width_sample );
#endif
            double[] ma = new double[length];
            for ( int i = 0; i < length; i++ ) {
                int count = 0;
                double sum = 0.0;
                for ( int j = i - ma_width_sample; j < i + 2 * ma_width_sample; j++ ) {
                    if ( 0 <= j && j < length ) {
                        sum += formanto[j];
                        count++;
                    }
                }
                ma[i] = sum / (double)count;
            }
#if DEBUG
            if ( s_test ) {
                using ( StreamWriter sw = new StreamWriter( "ma.txt" ) ) {
                    for ( int i = 0; i < length; i++ ) {
                        sw.WriteLine( (i * hz_from_index) + "\t" + ma[i] );
                    }
                }
            }
#endif
            // ピークの位置を探す
            double threshold = argument.PeakDetectionThreshold;
            double last_slope = 0.0;
            int index = -1;
            List<double> peak_positions = new List<double>();
            bool first = true;
            for ( int i = 1; i < window_size - 1; i++ ) {
                double slope = ma[i + 1] - ma[i - 1];
                if ( ma[i] > threshold && slope <= 0.0 && 0.0 < last_slope ) {
                    if ( first ) {
                        index = i;
                        first = false;
                    }
                    //break;
                    peak_positions.Add( i * hz_from_index );
                }
                last_slope = slope;
            }
#if DEBUG
            if ( s_test ) {
                using ( StreamWriter sw = new StreamWriter( "peak_positions.txt" ) ) {
                    for ( int i = 0; i < peak_positions.Count; i++ ) {
                        sw.WriteLine( peak_positions[i].ToString() );
                    }
                }
            }
#endif
            if ( index > 0 ) {
                List<double> peaks = new List<double>();
                double peak_distance_tolerance = index * hz_from_index / 5.0; //最小の周波数の5分の1
                double last_peak_pos = index * hz_from_index;
                peaks.Add( last_peak_pos );
                for ( int i = 1; i < peak_positions.Count; i++ ) {
                    if ( peak_positions[i] - last_peak_pos > peak_distance_tolerance ) {
                        peaks.Add( peak_positions[i] );
                        last_peak_pos = peak_positions[i];
                    }
                }
#if DEBUG
                if ( s_test ) {
                    using ( StreamWriter sw = new StreamWriter( "peaks.txt" ) ) {
                        for ( int i = 0; i < peaks.Count; i++ ) {
                            sw.WriteLine( peaks[i] );
                        }
                    }
                }
#endif
                double min_peak_distance = index * hz_from_index * 2.0 / 3.0;
                /*for ( int i = 1; i < peaks.Count; i++ ) {
                    min_peak_distance = Math.Min( min_peak_distance, peaks[i] - peaks[i - 1] );
                }*/
#if DEBUG
                Console.WriteLine( "min_peak_distance=" + min_peak_distance );
                if ( s_test ) {
                    using ( StreamWriter sw = new StreamWriter( "evaluation.txt" ) ) {
                        for ( int i = (int)min_peak_distance; i < (int)(4 * min_peak_distance); i++ ) {
                            sw.WriteLine( i + "\t" + GetFormantoGetEvaluationValue( peaks, i ) );
                        }
                    }
                }
#endif
                int smallest = (int)min_peak_distance;
                double min_eval = GetFormantoGetEvaluationValue( peaks, smallest );
                for ( int i = (int)min_peak_distance; i < (int)(4 * min_peak_distance); i++ ) {
                    double eval = GetFormantoGetEvaluationValue( peaks, i );
                    if ( min_eval > eval ) {
                        min_eval = eval;
                        smallest = i;
                    }
                }
                return smallest;
            } else {
                return 0;
            }
        }

        private static double GetFormantoGetEvaluationValue( List<double> peaks, double t ) {
            double ret = 0.0;
            for ( int i = 0; i < peaks.Count; i++ ) {
                int n_i = (int)(peaks[i] / t + 0.5);
                double dt_i = peaks[i] - n_i * t;
                ret += Math.Abs( dt_i );
            }
            return ret / t;
        }

        /*public unsafe double[,] GetFormanto( uint time, uint count, double[] window_function ) {
            int size = window_function.Length;
            double[] buffer = new double[size];    // 波形のバッファ
            int nmaxsqrt = (int)Math.Sqrt( size );
            int[] ip_ = new int[nmaxsqrt + 2];
            double[] w_ = new double[size * 5 / 4];
            ip_[0] = 0;

            double[,] ret = new double[count, size];

            for ( uint j = time; j < time + count; j++ ) {
                GetNormalizedWave( (uint)(time - size / 2), ref buffer );
                // 窓関数をかける
                for ( int i = 0; i < size; i++ ) {
                    buffer[i] *= window_function[i];
                }

                fixed ( int* ip = &ip_[0] )
                fixed ( double* w = &w_[0] )
                fixed ( double* a = &buffer[0] ) {
                    fft.rdft( size, 1, a, ip, w );
                }
                for ( int i = 0; i < size; i++ ) {
                    ret[j - time, i] = buffer[i];
                }
            }
            return ret;
        }*/

        /// <summary>
        /// 第timeサンプルにおけるフォルマントを取得する
        /// </summary>
        /// <param name="time"></param>
        /// <param name="window_function"></param>
        /// <returns></returns>
        public unsafe double[] GetFormanto( uint time, double[] window_function ) {
            int size = window_function.Length;
            double[] wv = GetNormalizedWave( (int)(time - size / 2), (uint)(size + 1) );
            // 窓関数をかける
            for ( int i = 0; i < size; i++ ) {
                wv[i] = wv[i] * window_function[i];
            }
            int nmaxsqrt = (int)Math.Sqrt( size );
            int[] ip_ = new int[nmaxsqrt + 2];
            double[] w_ = new double[size * 5 / 4];
            ip_[0] = 0;

            fixed ( int* ip = &ip_[0] )
            fixed ( double* w = &w_[0] )
            fixed ( double* a = &wv[0] ) {
                fft.rdft( size, 1, a, ip, w );
            }
            return wv;
        }


        public double GetVolume( int start_sample, double[] window_function ) {
            int window_size = window_function.Length;
            double[] conv = new double[window_size];
            GetNormalizedWave( start_sample - window_size / 2, ref conv );
            double ret = 0.0;
            for ( int i = 0; i < window_size; i++ ) {
                ret += Math.Abs( conv[i] ) * window_function[i];
            }
            return ret / (double)window_size;
        }


        /// <summary>
        /// 指定したサンプル位置における音量を計算します
        /// </summary>
        /// <param name="start_sample"></param>
        /// <param name="window_size"></param>
        /// <param name="window_function_type"></param>
        /// <returns></returns>
        public double GetVolume( int start_sample, int window_size, math.WindowFunctionType window_function_type ) {
            double[] conv = new double[window_size];
            GetNormalizedWave( start_sample - window_size / 2, ref conv );
            double ret = 0.0;
            for ( int i = 0; i < window_size; i++ ) {
                double x = i / (double)window_size;
                ret += Math.Abs( conv[i] ) * math.window_func( window_function_type, x );
            }
            return ret / (double)window_size;
        }


        /// <summary>
        /// 音量の時間変化を取得します
        /// </summary>
        /// <param name="window_size">窓の幅（サンプル数）</param>
        /// <param name="window_function_type">使用する窓関数</param>
        /// <param name="resulution">解像度（サンプル数）</param>
        /// <returns></returns>
        public double[] GetVolume( int window_size, math.WindowFunctionType window_function_type ) {
            double[] conv = GetNormalizedWave();
            for ( int i = 0; i < conv.Length; i++ ) {
                conv[i] = Math.Abs( conv[i] );
            }
            // 最初に重み関数を取得
            double[] w = new double[window_size];
            for ( int i = 0; i < window_size; i++ ) {
                double x = i / (double)window_size;
                w[i] = math.window_func( window_function_type, x );
            }
            double[] ret = new double[m_total_samples];
            double inv = 1.0 / (double)window_size;
            int ws2 = window_size / 2;
            for ( int i = 0; i < m_total_samples; i++ ) {
                int start0 = i - ws2;
                int start = start0;
                int end = i + ws2;
                double tinv = inv;
                if ( start < 0 ) {
                    start = 0;
                    tinv = 1.0 / (double)(end - start + 1);
                }
                if ( m_total_samples <= end ) {
                    end = (int)m_total_samples - 1;
                    tinv = 1.0 / (double)(end - start + 1);
                }
                ret[i] = 0.0;
                for ( int j = start; j <= end; j++ ) {
                    ret[i] += conv[j] * w[i - start0];
                }
                ret[i] = ret[i] * tinv;
            }
            return ret;
        }


        public void GetNormalizedWave( int start_index, ref double[] conv ) {
            int count = conv.Length;
            int cp_start = start_index;
            int cp_end = start_index + count;
            if ( start_index < 0 ) {
                for ( int i = 0; i < cp_start - start_index; i++ ) {
                    conv[i] = 0.0;
                }
                cp_start = 0;
            }
            if ( m_total_samples <= cp_end ) {
                for ( int i = cp_end - start_index; i < count; i++ ) {
                    conv[i] = 0.0;
                }
                cp_end = (int)m_total_samples - 1;
            }
            if ( m_channel == WaveChannel.Monoral ) {
                if ( m_bit_per_sample == 8 ) {
                    for ( int i = cp_start; i < cp_end; i++ ) {
                        conv[i - start_index] = (L8[i] - 64.0) / 64.0;
                    }
                } else {
                    for (int i = cp_start; i < cp_end; i++ ) {
                        conv[i - start_index] = L16[i] / 32768.0;
                    }
                }
            } else {
                if ( m_bit_per_sample == 8 ) {
                    for (int i = cp_start; i < cp_end; i++ ) {
                        conv[i - start_index] = ((L8[i] + R8[i]) * 0.5 - 64.0) / 64.0;
                    }
                } else {
                    for (int i = cp_start; i < cp_end; i++ ) {
                        conv[i - start_index] = (L16[i] + R16[i]) * 0.5 / 32768.0;
                    }
                }
            }
        }


        public double[] GetNormalizedWave() {
            return GetNormalizedWave( 0, m_total_samples );
        }



        /// <summary>
        /// -1から1までに規格化された波形を取得します
        /// </summary>
        /// <returns></returns>
        public double[] GetNormalizedWave( int start_index, uint count ) {
            double[] conv = new double[count];
            GetNormalizedWave( start_index, ref conv );
            return conv;
        }


        public int this[int index, Channel channel] {
            get {
                if ( m_channel == WaveChannel.Monoral || channel == Channel.Left ) {
                    if ( m_bit_per_sample == 8 ) {
                        return L8[index];
                    } else {
                        return L16[index];
                    }
                } else {
                    if ( m_bit_per_sample == 8 ) {
                        return R8[index];
                    } else {
                        return R16[index];
                    }
                }
            }
            set {
                if ( m_channel == WaveChannel.Monoral || channel == Channel.Left ) {
                    if ( m_bit_per_sample == 8 ) {
                        L8[index] = (byte)value;
                    } else {
                        L16[index] = (short)value;
                    }
                } else {
                    if ( m_bit_per_sample == 8 ) {
                        R8[index] = (byte)value;
                    } else {
                        R16[index] = (short)value;
                    }
                }
            }
        }


        public int this[int index] {
            get {
                if ( m_bit_per_sample == 8 ) {
                    return L8[index];
                } else {
                    return L16[index];
                }
            }
            set {
                if ( m_bit_per_sample == 8 ) {
                    L8[index] = (byte)value;
                    if ( m_channel == WaveChannel.Stereo ) {
                        R8[index] = (byte)value;
                    }
                } else {
                    L16[index] = (short)value;
                    if ( m_channel == WaveChannel.Stereo ) {
                        R16[index] = (short)value;
                    }
                }
            }
        }

        public double Get( int index ) {
            if ( m_channel == WaveChannel.Monoral ) {
                if ( m_bit_per_sample == 8 ) {
                    return (L8[index] - 64.0) / 64.0;
                } else {
                    return L16[index] / 32768.0;
                }
            } else {
                if ( m_bit_per_sample == 8 ) {
                    return ((L8[index] + R8[index]) * 0.5 - 64.0) / 64.0;
                } else {
                    return (L16[index] + R16[index]) * 0.5 / 32768.0;
                }
            }
        }

        public uint SampleRate {
            get {
                return m_sample_rate;
            }
        }


        public uint TotalSamples {
            get {
                return m_total_samples;
            }
            set {
                m_total_samples = value;
                if ( m_channel == WaveChannel.Monoral ) {
                    if ( m_bit_per_sample == 8 ) {
                        if ( L8 == null ) {
                            L8 = new byte[m_total_samples];
                        } else {
                            Array.Resize( ref L8, (int)m_total_samples );
                        }
                    } else {
                        if ( L16 == null ) {
                            L16 = new short[m_total_samples];
                        } else {
                            Array.Resize( ref L16, (int)m_total_samples );
                        }
                    }
                } else {
                    if ( m_bit_per_sample == 8 ) {
                        if ( L8 == null ) {
                            L8 = new byte[m_total_samples];
                            R8 = new byte[m_total_samples];
                        } else {
                            Array.Resize( ref L8, (int)m_total_samples );
                            Array.Resize( ref R8, (int)m_total_samples );
                        }
                    } else {
                        if ( L16 == null ) {
                            L16 = new short[m_total_samples];
                            R16 = new short[m_total_samples];
                        } else {
                            Array.Resize( ref L16, (int)m_total_samples );
                            Array.Resize( ref R16, (int)m_total_samples );
                        }
                    }
                }
            }
        }


        public void Replace( Wave srcWave, uint srcStart, uint destStart, uint count ) {
#if DEBUG
            Console.WriteLine( "Wave.Replace(Wave,uint,uint,uint)" );
#endif
            if ( m_channel != srcWave.m_channel || m_bit_per_sample != srcWave.m_bit_per_sample ) {
                return;
            }
            if ( m_channel == WaveChannel.Monoral ) {
                if ( m_bit_per_sample == 8 ) {
                    if ( L8 == null || srcWave.L8 == null ) {
                        return;
                    }
                } else {
                    if ( L16 == null || srcWave.L16 == null ) {
                        return;
                    }
                }
            } else {
                if ( m_bit_per_sample == 8 ) {
                    if ( L8 == null || R8 == null || srcWave.L8 == null || srcWave.R8 == null ) {
                        return;
                    }
                } else {
                    if ( L16 == null || R16 == null || srcWave.L16 == null || srcWave.R16 == null ) {
                        return;
                    }
                }
            }

            count = (count > srcWave.TotalSamples - srcStart) ? srcWave.TotalSamples - srcStart : count;
            uint new_last_index = destStart + count;
            if ( m_total_samples < new_last_index ) {
                if ( m_channel == WaveChannel.Monoral ) {
                    if ( m_bit_per_sample == 8 ) {
                        Array.Resize( ref L8, (int)new_last_index );
                    } else {
                        Array.Resize( ref L16, (int)new_last_index );
                    }
                } else {
                    if ( m_bit_per_sample == 8 ) {
                        Array.Resize( ref L8, (int)new_last_index );
                        Array.Resize( ref R8, (int)new_last_index );
                    } else {
                        Array.Resize( ref L16, (int)new_last_index );
                        Array.Resize( ref R16, (int)new_last_index );
                    }
                }
                m_total_samples = new_last_index;
            }
            if ( m_channel == WaveChannel.Monoral ) {
                if ( m_bit_per_sample == 8 ) {
                    Array.Copy( srcWave.L8, srcStart, L8, destStart, count );
                } else {
                    Array.Copy( srcWave.L16, srcStart, L16, destStart, count );
                }
            } else {
                if ( m_bit_per_sample == 8 ) {
                    Array.Copy( srcWave.L8, srcStart, L8, destStart, count );
                    Array.Copy( srcWave.R8, srcStart, R8, destStart, count );
                } else {
                    Array.Copy( srcWave.L16, srcStart, L16, destStart, count );
                    Array.Copy( srcWave.R16, srcStart, R16, destStart, count );
                }
            }
        }


        public void Replace( byte[] data, uint start_index ) {
            if ( m_channel != WaveChannel.Monoral || m_bit_per_sample != 8 || L8 == null ) {
                return;
            }
            uint new_last_index = (uint)(start_index + data.Length);
            if ( m_total_samples < new_last_index ) {
                Array.Resize( ref L8, (int)new_last_index );
                m_total_samples = new_last_index;
            }
            for ( int i = 0; i < data.Length; i++ ) {
                L8[i + start_index] = data[i];
            }
        }


        public void Replace( short[] data, uint start_index ) {
            if ( m_channel != WaveChannel.Monoral || m_bit_per_sample != 16 || L16 == null ) {
                return;
            }
            uint new_last_index = (uint)(start_index + data.Length);
            if ( m_total_samples < new_last_index ) {
                Array.Resize( ref L16, (int)new_last_index );
                m_total_samples = new_last_index;
            }
            for ( int i = 0; i < data.Length; i++ ) {
                L16[i + start_index] = data[i];
            }
        }


        public void Replace( byte[] left, byte[] right, uint start_index ) {
            if ( m_channel != WaveChannel.Stereo || m_bit_per_sample != 8 || L8 == null || R8 == null ) {
                return;
            }
            uint new_last_index = (uint)(start_index + left.Length);
            if ( m_total_samples < new_last_index ) {
                Array.Resize( ref L8, (int)new_last_index );
                Array.Resize( ref R8, (int)new_last_index );
                m_total_samples = new_last_index;
            }
            for ( int i = 0; i < left.Length; i++ ) {
                L8[i + start_index] = left[i];
                R8[i + start_index] = right[i];
            }
        }


        public void Replace( short[] left, short[] right, uint start_index ) {
            if ( m_channel != WaveChannel.Stereo || m_bit_per_sample != 16 || L16 == null || R16 == null ) {
                return;
            }
            uint new_last_index = (uint)(start_index + left.Length);
            if ( m_total_samples < new_last_index ) {
                Array.Resize( ref L16, (int)new_last_index );
                Array.Resize( ref R16, (int)new_last_index );
                m_total_samples = new_last_index;
            }
            for ( int i = 0; i < left.Length; i++ ) {
                L16[i + start_index] = left[i];
                R16[i + start_index] = right[i];
            }
        }

        public void Replace( float[] left, float[] right, uint start_index ) {
            uint new_last_index = (uint)(start_index + left.Length);
            if ( m_total_samples < new_last_index ) {
                if ( m_channel == WaveChannel.Monoral ) {
                    if ( m_bit_per_sample == 8 ) {
                        if ( L8 == null ) {
                            return;
                        }
                        Array.Resize( ref L8, (int)new_last_index );
                    } else {
                        if ( L16 == null ) {
                            return;
                        }
                        Array.Resize( ref L16, (int)new_last_index );
                    }
                } else {
                    if ( m_bit_per_sample == 8 ) {
                        if ( L8 == null || R8 == null ) {
                            return;
                        }
                        Array.Resize( ref L8, (int)new_last_index );
                        Array.Resize( ref R8, (int)new_last_index );
                    } else {
                        if ( L16 == null || R16 == null ) {
                            return;
                        }
                        Array.Resize( ref L16, (int)new_last_index );
                        Array.Resize( ref R16, (int)new_last_index );
                    }
                }
                m_total_samples = new_last_index;
            }
            if ( m_channel == WaveChannel.Monoral ) {
                float[] mono = new float[left.Length];
                for( int i = 0; i < left.Length; i++ ){
                    mono[i] = (left[i] + right[i]) / 2f;
                }
                if ( m_bit_per_sample == 8 ) {
                    for ( int i = 0; i < mono.Length; i++ ) {
                        L8[i + start_index] = (byte)((mono[i] + 1.0f) / 2f * 255f);
                    }
                } else {
                    for ( int i = 0; i < mono.Length; i++ ) {
                        L16[i + start_index] = (short)(mono[i] * 32768f);
                    }
                }
            } else {
                if ( m_bit_per_sample == 8 ) {
                    for ( int i = 0; i < left.Length; i++ ) {
                        L8[i + start_index] = (byte)((left[i] + 1.0f) / 2f * 255f);
                        R8[i + start_index] = (byte)((right[i] + 1.0f) / 2f * 255f);
                    }
                } else {
                    for ( int i = 0; i < left.Length; i++ ) {
                        L16[i + start_index] = (short)(left[i] * 32768f);
                        R16[i + start_index] = (short)(right[i] * 32768f);
                    }
                }
            }
        }

        public void PrintToText( string path ) {
            using ( StreamWriter sw = new StreamWriter( path ) ) {
                if ( m_channel == WaveChannel.Monoral ) {
                    if ( m_bit_per_sample == 8 ) {
                        for ( int i = 0; i < m_total_samples; i++ ) {
                            sw.WriteLine( L8[i] );
                        }
                    } else {
                        for ( int i = 0; i < m_total_samples; i++ ) {
                            sw.WriteLine( L16[i] );
                        }
                    }
                } else {
                    if ( m_bit_per_sample == 8 ) {
                        for ( int i = 0; i < m_total_samples; i++ ) {
                            sw.WriteLine( L8[i] + "\t" + R8[i] );
                        }
                    } else {
                        for ( int i = 0; i < m_total_samples; i++ ) {
                            sw.WriteLine( L16[i] + "\t" + R16[i] );
                        }
                    }
                }
            }
        }

        public void Dispose() {
            L8 = null;
            R8 = null;
            L16 = null;
            R16 = null;
            GC.Collect();
        }

        /// <summary>
        /// サンプルあたりのビット数を8に変更する
        /// </summary>
        public void ConvertTo8Bit() {
            if ( m_bit_per_sample == 8 ) {
                return;
            }

            // 先ず左チャンネル
            L8 = new byte[L16.Length];
            for ( int i = 0; i < L16.Length; i++ ) {
                double new_val = (L16[i] + 32768.0) / 65535.0 * 255.0;
                L8[i] = (byte)new_val;
            }
            L16 = null;

            // 存在すれば右チャンネルも
            if ( m_channel == WaveChannel.Stereo ) {
                R8 = new byte[R16.Length];
                for ( int i = 0; i < R16.Length; i++ ) {
                    double new_val = (R16[i] + 32768.0) / 65535.0 * 255.0;
                    R8[i] = (byte)new_val;
                }
                R16 = null;
            }

            m_bit_per_sample = 8;
        }

        /// <summary>
        /// ファイルに保存
        /// </summary>
        /// <param name="file"></param>
        public void Write( string file ) {
            using ( FileStream fs = new FileStream( file, FileMode.Create ) ) {
                // RIFF
                fs.WriteByte( 0x52 );
                fs.WriteByte( 0x49 );
                fs.WriteByte( 0x46 );
                fs.WriteByte( 0x46 );

                // ファイルサイズ - 8最後に記入
                fs.WriteByte( 0x00 );
                fs.WriteByte( 0x00 );
                fs.WriteByte( 0x00 );
                fs.WriteByte( 0x00 );

                // WAVE
                fs.WriteByte( 0x57 );
                fs.WriteByte( 0x41 );
                fs.WriteByte( 0x56 );
                fs.WriteByte( 0x45 );

                // fmt 
                fs.WriteByte( 0x66 );
                fs.WriteByte( 0x6d );
                fs.WriteByte( 0x74 );
                fs.WriteByte( 0x20 );

                // fmt チャンクのサイズ
                fs.WriteByte( 0x12 );
                fs.WriteByte( 0x00 );
                fs.WriteByte( 0x00 );
                fs.WriteByte( 0x00 );

                // format ID
                fs.WriteByte( 0x01 );
                fs.WriteByte( 0x00 );

                // チャンネル数
                if ( m_channel == WaveChannel.Monoral ) {
                    fs.WriteByte( 0x01 );
                    fs.WriteByte( 0x00 );
                } else {
                    fs.WriteByte( 0x02 );
                    fs.WriteByte( 0x00 );
                }

                // サンプリングレート
                byte[] buf = BitConverter.GetBytes( m_sample_rate );
                WriteByteArray( fs, buf, 4 );

                // データ速度
                ushort block_size = (ushort)(m_bit_per_sample / 8 * (int)m_channel);
                uint data_rate = m_sample_rate * block_size;
                buf = BitConverter.GetBytes( data_rate );
                WriteByteArray( fs, buf, 4 );

                // ブロックサイズ
                buf = BitConverter.GetBytes( block_size );
                WriteByteArray( fs, buf, 2 );

                // サンプルあたりのビット数
                buf = BitConverter.GetBytes( m_bit_per_sample );
                WriteByteArray( fs, buf, 2 );

                // 拡張部分
                fs.WriteByte( 0x00 );
                fs.WriteByte( 0x00 );

                // data
                fs.WriteByte( 0x64 );
                fs.WriteByte( 0x61 );
                fs.WriteByte( 0x74 );
                fs.WriteByte( 0x61 );

                // size of data chunk
                uint size = block_size * m_total_samples;
                buf = BitConverter.GetBytes( size );
                WriteByteArray( fs, buf, 4 );

                // 波形データ
                for ( int i = 0; i < m_total_samples; i++ ) {
                    if ( m_bit_per_sample == 8 ) {
                        fs.WriteByte( L8[i] );
                        if ( m_channel == WaveChannel.Stereo ) {
                            fs.WriteByte( R8[i] );
                        }
                    } else {
                        buf = BitConverter.GetBytes( L16[i] );
                        WriteByteArray( fs, buf, 2 );
                        if ( m_channel == WaveChannel.Stereo ) {
                            buf = BitConverter.GetBytes( R16[i] );
                            WriteByteArray( fs, buf, 2 );
                        }
                    }
                }

                // 最後にWAVEチャンクのサイズ
                uint position = (uint)fs.Position;
                fs.Seek( 4, SeekOrigin.Begin );
                buf = BitConverter.GetBytes( position - 8 );
                WriteByteArray( fs, buf, 4 );
            }
        }

        private static void WriteByteArray( FileStream fs, byte[] dat, int limit ) {
            fs.Write( dat, 0, (dat.Length > limit) ? limit : dat.Length );
            if ( dat.Length < limit ) {
                for ( int i = 0; i < limit - dat.Length; i++ ) {
                    fs.WriteByte( 0x00 );
                }
            }
        }

        /// <summary>
        /// ステレオをモノラル化
        /// </summary>
        public void Monoralize() {
            if ( m_channel != WaveChannel.Stereo ) {
                return;
            }
            if ( m_bit_per_sample == 8 ) {
                for ( int i = 0; i < L8.Length; i++ ) {
                    L8[i] = (byte)((L8[i] + R8[i]) / 2);
                }
                R8 = null;
                m_channel = WaveChannel.Monoral;
            } else {
                for ( int i = 0; i < L16.Length; i++ ) {
                    L16[i] = (short)((L16[i] + R16[i]) / 2);
                }
                R16 = null;
                m_channel = WaveChannel.Monoral;
            }
        }


        /// <summary>
        /// 前後の無音部分を削除します
        /// </summary>
        public void TrimSilence() {
#if DEBUG
            Console.WriteLine( "Wave.TrimSilence" );
#endif
            if ( m_bit_per_sample == 8 ) {
                if ( m_channel == WaveChannel.Monoral ) {
                    int non_silence_begin = 1;
                    for ( int i = 1; i < L8.Length; i++ ) {
                        if ( L8[i] != 0x80 ) {
                            non_silence_begin = i;
                            break;
                        }
                    }
                    int non_silence_end = L8.Length - 1;
                    for ( int i = L8.Length - 1; i >= 0; i-- ) {
                        if ( L8[i] != 0x80 ) {
                            non_silence_end = i;
                            break;
                        }
                    }
                    int count = non_silence_end - non_silence_begin + 1;
                    R8 = new byte[count];
                    Array.Copy( L8, non_silence_begin, R8, 0, count );
                    L8 = null;
                    L8 = new byte[count];
                    Array.Copy( R8, L8, count );
                    R8 = null;
                } else {
                    int non_silence_begin = 1;
                    for ( int i = 1; i < L8.Length; i++ ) {
                        if ( L8[i] != 0x80 || R8[i] != 0x80 ) {
                            non_silence_begin = i;
                            break;
                        }
                    }
                    int non_silence_end = L8.Length - 1;
                    for ( int i = L8.Length - 1; i >= 0; i-- ) {
                        if ( L8[i] != 0x80 || R8[i] != 0x80 ) {
                            non_silence_end = i;
                            break;
                        }
                    }
                    int count = non_silence_end - non_silence_begin + 1;
                    byte[] buf = new byte[count];
                    Array.Copy( L8, non_silence_begin, buf, 0, count );
                    L8 = null;
                    L8 = new byte[count];
                    Array.Copy( buf, L8, count );
                    Array.Copy( R8, non_silence_begin, buf, 0, count );
                    R8 = null;
                    R8 = new byte[count];
                    Array.Copy( buf, R8, count );
                    buf = null;
                }
            } else {
                if ( m_channel == WaveChannel.Monoral ) {
                    int non_silence_begin = 1;
                    for ( int i = 1; i < L16.Length; i++ ) {
                        if ( L16[i] != 0 ) {
                            non_silence_begin = i;
                            break;
                        }
                    }
                    int non_silence_end = L16.Length - 1;
                    for ( int i = L16.Length - 1; i >= 0; i-- ) {
                        if ( L16[i] != 0 ) {
                            non_silence_end = i;
                            break;
                        }
                    }
                    int count = non_silence_end - non_silence_begin + 1;
                    R16 = new short[count];
                    Array.Copy( L16, non_silence_begin, R16, 0, count );
                    Array.Resize<short>( ref L16, count );
                    Array.Copy( R16, L16, count );
                    R16 = null;
                } else {
                    int non_silence_begin = 1;
                    for ( int i = 1; i < L16.Length; i++ ) {
                        if ( L16[i] != 0 || R16[i] != 0 ) {
                            non_silence_begin = i;
                            break;
                        }
                    }
                    int non_silence_end = L16.Length - 1;
                    for ( int i = L16.Length - 1; i >= 0; i-- ) {
                        if ( L16[i] != 0 || R16[i] != 0 ) {
                            non_silence_end = i;
                            break;
                        }
                    }
#if DEBUG
                    Console.WriteLine( "    non_silence_beign=" + non_silence_begin );
                    Console.WriteLine( "    non_silence_end=" + non_silence_end );
#endif
                    int count = non_silence_end - non_silence_begin + 1;
                    short[] buf = new short[count];
                    Array.Copy( L16, non_silence_begin, buf, 0, count );
                    Array.Resize<short>( ref L16, count );
                    Array.Copy( buf, 0, L16, 0, count );
                    
                    Array.Copy( R16, non_silence_begin, buf, 0, count );
                    Array.Resize<short>( ref R16, count );
                    Array.Copy( buf, 0, R16, 0, count );
                    buf = null;
                }
            }

            if ( m_bit_per_sample == 8 ) {
                m_total_samples = (uint)L8.Length;
            } else {
                m_total_samples = (uint)L16.Length;
            }
        }

        public Wave() {
            m_channel = WaveChannel.Stereo;
            m_bit_per_sample = 16;
            m_sample_rate = 44100;
        }

        public Wave( WaveChannel channels, ushort bit_per_sample, uint sample_rate, uint initial_capacity ) {
            m_channel = channels;
            m_bit_per_sample = bit_per_sample;
            m_sample_rate = sample_rate;
            m_total_samples = initial_capacity;
            if ( m_bit_per_sample == 8 ) {
                L8 = new byte[m_total_samples];
                if ( m_channel == WaveChannel.Stereo ) {
                    R8 = new byte[m_total_samples];
                }
            } else if ( m_bit_per_sample == 16 ) {
                L16 = new short[m_total_samples];
                if ( m_channel == WaveChannel.Stereo ) {
                    R16 = new short[m_total_samples];
                }
            }
        }

        public void Append( double[] L, double[] R ) {
            int length = Math.Min( L.Length, R.Length );
            int old_length = L16.Length;
            if ( m_bit_per_sample == 16 && m_channel == WaveChannel.Stereo ) {
                Array.Resize( ref L16, (int)(m_total_samples + length) );
                Array.Resize( ref R16, (int)(m_total_samples + length) );
                m_total_samples += (uint)length;
                for ( int i = old_length; i < m_total_samples; i++ ) {
                    L16[i] = (short)(L[i - old_length] * 32768f);
                    R16[i] = (short)(R[i - old_length] * 32768f);
                }
            } //else ... TODO: Wave+Append他のbitpersec, channelのとき
        }

        /*public void Append( short[] L, short[] R ) {
            int length = Math.Min( L.Length, R.Length );
            int old_length = L16.Length;
            if ( m_bit_per_sample == 16 && m_channel == WaveChannel.Stereo ) {
                Array.Resize( ref L16, (int)(m_total_samples + length) );
                Array.Resize( ref R16, (int)(m_total_samples + length) );
                m_total_samples += (uint)length;
                for ( int i = old_length; i < m_total_samples; i++ ) {
                    L16[i] = L[i - old_length];
                    R16[i] = R[i - old_length];
                }
            } //else ... TODO: Wave+Append他のbitpersec, channelのとき
        }*/

        public Wave( string path ) {
            Read( path );
        }

        private bool parseAiffHeader( FileStream fs ) {
            try {
                byte[] buf = new byte[4];
                fs.Read( buf, 0, 4 );
                uint chunk_size_form = make_uint_big( buf );
                fs.Read( buf, 0, 4 ); // AIFF
                string tag = new string( new char[] { (char)buf[0], (char)buf[1], (char)buf[2], (char)buf[3] } );
                if ( !tag.Equals( "AIFF" ) ) {
#if DEBUG
                    Console.WriteLine( "Wave#parseAiffHeader; error; tag=" + tag + " and must be AIFF" );
#endif
                    return false;
                }
                fs.Read( buf, 0, 4 ); // COMM
                tag = new string( new char[] { (char)buf[0], (char)buf[1], (char)buf[2], (char)buf[3] } );
                if ( !tag.Equals( "COMM" ) ) {
#if DEBUG
                    Console.WriteLine( "Wave#parseAiffHeader; error; tag=" + tag + " and must be COMM" );
#endif
                    return false;
                }
                fs.Read( buf, 0, 4 ); // COMM chunk size
                uint chunk_size_comm = make_uint_big( buf );
                long chunk_loc_comm = fs.Position;
                fs.Read( buf, 0, 2 ); // number of channel
                ushort num_channel = make_ushort_big( buf );
                if ( num_channel == 1 ) {
                    m_channel = WaveChannel.Monoral;
                } else {
                    m_channel = WaveChannel.Stereo;
                }
                fs.Read( buf, 0, 4 ); // number of samples
                m_total_samples = make_uint_big( buf );
#if DEBUG
                Console.WriteLine( "Wave#parseAiffHeader; m_total_samples=" + m_total_samples );
#endif
                fs.Read( buf, 0, 2 ); // block size
                m_bit_per_sample = make_ushort_big( buf );
                byte[] buf10 = new byte[10];
                fs.Read( buf10, 0, 10 ); // sample rate
                m_sample_rate = (uint)make_double_from_extended( buf10 );
#if DEBUG
                Console.WriteLine( "Wave#parseAiffHeader; m_sample_rate=" + m_sample_rate );
#endif
                fs.Seek( chunk_loc_comm + (long)chunk_size_comm, SeekOrigin.Begin );

                fs.Read( buf, 0, 4 ); // SSND
                tag = new string( new char[] { (char)buf[0], (char)buf[1], (char)buf[2], (char)buf[3] } );
                if ( !tag.Equals( "SSND" ) ) {
#if DEBUG
                    Console.WriteLine( "Wave#parseAiffHeader; error; tag=" + tag + " and must be SSND" );
#endif
                    return false;
                }
                fs.Read( buf, 0, 4 ); // SSND chunk size
                uint chunk_size_ssnd = make_uint_big( buf );
            } catch ( Exception ex ) {
                return false;
            }
            return true;
        }

        public uint make_uint_big( byte[] buf ) {
            return (uint)((uint)((uint)((uint)((buf[0] << 8) | buf[1]) << 8) | buf[2]) << 8) | buf[3];
        }

        public ushort make_ushort_big( byte[] buf ) {
            return (ushort)((ushort)(buf[0] << 8) | buf[1]);
        }

        private static double make_double_from_extended( byte[] bytes ) {
            double f;
            uint hiMant, loMant;

            int expon = ((bytes[0] & 0x7F) << 8) | (bytes[1] & 0xFF);
            hiMant = ((uint)(bytes[2] & 0xFF) << 24)
                   | ((uint)(bytes[3] & 0xFF) << 16)
                   | ((uint)(bytes[4] & 0xFF) << 8)
                   | ((uint)(bytes[5] & 0xFF));
            loMant = ((uint)(bytes[6] & 0xFF) << 24)
                   | ((uint)(bytes[7] & 0xFF) << 16)
                   | ((uint)(bytes[8] & 0xFF) << 8)
                   | ((uint)(bytes[9] & 0xFF));

            if ( expon == 0 && hiMant == 0 && loMant == 0 ) {
                f = 0;
            } else {
                if ( expon == 0x7FFF ) {
                    f = double.MaxValue;
                } else {
                    expon -= 16383;
                    //f = ldexp( double_from_uint( hiMant ), expon -= 31 );
                    f = Math.Pow( 2.0, expon - 31 ) * double_from_uint( hiMant );
                    //f += ldexp( double_from_uint( loMant ), expon -= 32 );
                    f += Math.Pow( 2.0, expon - 32 ) * double_from_uint( loMant );
                }
            }

            if ( (bytes[0] & 0x80) != 0 ) {
                return -f;
            } else {
                return f;
            }
        }

        private static double double_from_uint( uint u ) {
            return (((double)((int)(u - 2147483647 - 1))) + 2147483648.0);
        }

        private bool parseWaveHeader( FileStream fs ) {
            try {
                byte[] buf = new byte[4];
                // detect size of RIFF chunk
                fs.Read( buf, 0, 4 );
                uint riff_chunk_size = GetUIntFrom4Bytes( buf );
#if DEBUG
                System.Diagnostics.Debug.WriteLine( "riff_chunk_size=" + riff_chunk_size );
#endif

                // check wave header
                fs.Seek( 8, SeekOrigin.Begin );
                fs.Read( buf, 0, 4 );
                if ( buf[0] != 0x57 ||
                    buf[1] != 0x41 ||
                    buf[2] != 0x56 ||
                    buf[3] != 0x45 ) {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine( "invalid wave header" );
#endif
                    fs.Close();
                    return false;
                }

                // check fmt chunk header
                fs.Read( buf, 0, 4 );
                if ( buf[0] != 0x66 ||
                    buf[1] != 0x6d ||
                    buf[2] != 0x74 ||
                    buf[3] != 0x20 ) {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine( "invalid fmt header" );
#endif
                    fs.Close();
                    return false;
                }

                // detect size of fmt chunk
                uint fmt_chunk_bytes;
                fs.Read( buf, 0, 4 );
                fmt_chunk_bytes = GetUIntFrom4Bytes( buf );
#if DEBUG
                System.Diagnostics.Debug.WriteLine( "fmt_chunk_bytes=" + fmt_chunk_bytes );
#endif

                // get format ID
                fs.Read( buf, 0, 2 );
                ushort format_id = GetUShortFrom2Bytes( buf );
                if ( format_id != 1 ) {
                    fs.Close();
                    return false;
                }
#if DEBUG
                System.Diagnostics.Debug.WriteLine( "format_id=" + format_id );
#endif

                // get the number of channel(s)
                fs.Read( buf, 0, 2 );
                ushort num_channels = GetUShortFrom2Bytes( buf );
                if ( num_channels == 1 ) {
                    m_channel = WaveChannel.Monoral;
                } else if ( num_channels == 2 ) {
                    m_channel = WaveChannel.Stereo;
                } else {
                    fs.Close();
                    return false;
                }
#if DEBUG
                System.Diagnostics.Debug.WriteLine( "num_channels=" + num_channels );
#endif

                // get sampling rate
                fs.Read( buf, 0, 4 );
                m_sample_rate = GetUIntFrom4Bytes( buf );
#if DEBUG
                System.Diagnostics.Debug.WriteLine( "m_sample_rate=" + m_sample_rate );
#endif

                // get bit per sample
                fs.Seek( 0x22, SeekOrigin.Begin );
                fs.Read( buf, 0, 2 );
                m_bit_per_sample = GetUShortFrom2Bytes( buf );
#if DEBUG
                System.Diagnostics.Debug.WriteLine( "m_bit_per_sample=" + m_bit_per_sample );
#endif
                if ( m_bit_per_sample != 0x08 && m_bit_per_sample != 0x10 ) {
                    fs.Close();
                    return false;
                }

                // move to the end of fmt chunk
                fs.Seek( 0x14 + fmt_chunk_bytes, SeekOrigin.Begin );

                // move to the top of data chunk
                fs.Read( buf, 0, 4 );
                string tag = new string( new char[] { (char)buf[0], (char)buf[1], (char)buf[2], (char)buf[3] } );
#if DEBUG
                System.Diagnostics.Debug.WriteLine( "tag=" + tag );
#endif
                while ( tag != "data" ) {
                    fs.Read( buf, 0, 4 );
                    uint size = GetUIntFrom4Bytes( buf );
                    fs.Seek( size, SeekOrigin.Current );
                    fs.Read( buf, 0, 4 );
                    tag = new string( new char[] { (char)buf[0], (char)buf[1], (char)buf[2], (char)buf[3] } );
#if DEBUG
                    System.Diagnostics.Debug.WriteLine( "tag=" + tag );
#endif
                }

                // get size of data chunk
                fs.Read( buf, 0, 4 );
                uint data_chunk_bytes = GetUIntFrom4Bytes( buf );
                m_total_samples = (uint)(data_chunk_bytes / (num_channels * m_bit_per_sample / 8));
#if DEBUG
                System.Diagnostics.Debug.WriteLine( "m_total_samples=" + m_total_samples );
#endif

            } catch ( Exception ex ) {
#if DEBUG
                System.Diagnostics.Debug.Write( ex.StackTrace );
#endif
            }
            return true;
        }

        public bool Read( string path ) {
            FileStream fs = null;
            bool ret = false;
            try {
                fs = new FileStream( path, FileMode.Open, FileAccess.Read );
                // check RIFF header
                byte[] buf = new byte[4];
                fs.Read( buf, 0, 4 );
                bool change_byte_order = false;
                if ( buf[0] == 0x52 && buf[1] == 0x49 && buf[2] == 0x46 && buf[3] == 0x46 ) {
                    ret = parseWaveHeader( fs );
                } else if ( buf[0] == 0x46 && buf[1] == 0x4f && buf[2] == 0x52 && buf[3] == 0x4d ) {
                    ret = parseAiffHeader( fs );
                    change_byte_order = true;
                } else {
                    ret = false;
                }
                // prepare data
                if ( m_bit_per_sample == 8 ) {
                    L8 = new byte[m_total_samples];
                    if ( m_channel == WaveChannel.Stereo ) {
                        R8 = new byte[m_total_samples];
                    }
                } else {
                    L16 = new short[m_total_samples];
                    if ( m_channel == WaveChannel.Stereo ) {
                        R16 = new short[m_total_samples];
                    }
                }

                // read data
                // TODO: big endianのときの読込み。
                byte[] buf2 = new byte[2];
                for ( int i = 0; i < m_total_samples; i++ ) {
                    if ( m_bit_per_sample == 8 ) {
                        fs.Read( buf, 0, 1 );
                        L8[i] = buf[0];
                        if ( m_channel == WaveChannel.Stereo ) {
                            fs.Read( buf, 0, 1 );
                            R8[i] = buf[0];
                        }
                    } else {
                        fs.Read( buf2, 0, 2 );
                        if ( change_byte_order ) {
                            byte b = buf2[0];
                            buf2[0] = buf2[1];
                            buf2[1] = b;
                        }
                        L16[i] = BitConverter.ToInt16( buf2, 0 );
                        if ( m_channel == WaveChannel.Stereo ) {
                            fs.Read( buf2, 0, 2 );
                            if ( change_byte_order ) {
                                byte b = buf2[0];
                                buf2[0] = buf2[1];
                                buf2[1] = b;
                            }
                            R16[i] = BitConverter.ToInt16( buf2, 0 );
                        }
                    }
                }
            } catch ( Exception ex ) {
            } finally {
                if ( fs != null ) {
                    fs.Close();
                }
            }
            return ret;
        }

        private uint GetUIntFrom4Bytes( byte[] dat ) {
            return (uint)(dat[3] << 24) + (uint)(dat[2] << 16) + (uint)(dat[1] << 8) + (uint)dat[0];
        }

        private ushort GetUShortFrom2Bytes( byte[] dat ) {
            return (ushort)((dat[1] << 8) | dat[0]);
        }
    }

}
