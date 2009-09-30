/*
 * waveplay.h
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#ifndef __waveplay_h__
#define __waveplay_h__
#include "stdafx.h"
#include "wavereader.h"
    
using namespace std;

// 最初のバッファが書き込まれたとき呼び出されるコールバック関数
typedef void (*FirstBufferWrittenCallback)();

class waveplay{
private:
    static const int _NUM_BUF = 3;              // バッファの数
    static int          s_block_size;           // 1個のバッファのサイズ(サンプル)
    static int          s_sample_rate;          // サンプリングレート
    static WAVEFORMATEX s_wave_formatx;         // WAVEファイルヘッダ
    static HWAVEOUT     s_hwave_out;            // WAVE再生デバイス
    static WAVEHDR      s_wave_header[_NUM_BUF];// WAVEヘッダ
    static unsigned long* s_wave[_NUM_BUF];     // バッファ
    static bool         s_done[_NUM_BUF];
    static int          s_current_buffer;       // 次回書き込むべきバッファのインデクス
    static unsigned int s_processed_count;      // 初回はバッファを_NUM_BUF個全部埋めなければいけないので、最初の _NUM_BUF + 1 回はカウントを行う。そのためのカウンタ
    static bool         s_abort_required;       // 再生の中断が要求された時立つフラグ
    static int          s_buffer_loc;           // 書き込み中のバッファ内の、現在位置
    static bool         s_playing;              // 再生中かどうかを表すフラグ
    static int          s_error_samples;        // appendされた波形データの内、先頭のs_error_samples分を省く。通常の使い方なら常に0だが、vocaloid2 vstiで使う場合、プリセンド分を除いてwaveOutWriteしなければいけないので非0になる。
    static int          s_last_buffer;          // 最後に再生されるバッファの番号。負値の場合、append_lastが未だ呼ばれていないことを意味する。
    static FirstBufferWrittenCallback s_first_buffer_written_callback; // 最初のバッファが書き込まれたとき呼び出されるコールバック関数

    /// コールバック関数
    static void CALLBACK wave_callback( HWAVEOUT hwo, unsigned int uMsg, unsigned long dwInstance, unsigned long dwParam1, unsigned long dwParam2 );
    static void append_cor( float** a_data, unsigned int length, double amp_left, double amp_right, bool is_last_mode );
    static wavereader *s_wave_reader;
    static int s_num_wave_reader; // s_wave_readerの個数
    static float **s_another_wave_l;
    static float **s_another_wave_r;
    static __int64 s_wave_read_offset_samples;
    static float *s_wave_buffer_l;
    static float *s_wave_buffer_r;
    static void mix( int processed_count, float amp_left, float amp_right );
#ifdef __cplusplus_cli
    static System::String ^waveplay::util_get_errmsg( MMRESULT msg );
#else
    static string waveplay::util_get_errmsg( MMRESULT msg );
#endif
public:
    /// 初期化関数
    static void init( int block_size, int sample_rate );
    /// 波形データをバッファに追加する。バッファが再生中などの理由で即座に書き込めない場合、バッファが書き込み可能となるまで待機させられる
    static void append( float** data, unsigned int length, double amp_left, double amp_right );
	static void append( double** data, unsigned int length, double amp_left, double amp_right );
    static void flush_and_exit( double amp_left, double amp_right );
    /// 再生中断を要求する
    static void abort();
    /// 現在の再生位置を取得する。再生中でない場合負の値となる。
    static float get_play_time();
    /// リセットする。abort関数でも呼び出される。
    static void reset();
    /// 再生のための準備を行う。この関数を呼び出した後は、バッファが再生開始されるまでget_play_timeの戻り値は0となる（負値にならない）。
    /// 戻り値は、filesに指定されたファイルの内、最も再生時間の長いwaveファイルの、合計サンプル数
#ifdef __cplusplus_cli
	static int on_your_mark( array<System::String ^> ^files, __int64 wave_read_offset_samples );
#else
	static int on_your_mark( char **files, int num_files, __int64 wave_read_offset_samples );
#endif
    static void set_error_samples( int error_samples );
    /// コールバック関数を設定する
    static void set_first_buffer_written_callback( FirstBufferWrittenCallback proc );
    static void terminate();
    /// 現在再生中かどうかを取得する
    static bool is_alive();
    /// ブロックサイズを変更します
    static bool change_block_size( int block_size );
};

#endif // __waveplay_h__
