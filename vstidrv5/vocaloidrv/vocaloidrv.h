/*
 * vocaloidrv.h
 * Copyright © 2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#pragma once

// 標準出力にwaveをバイナリモードで出力する場合に定義
//#define USE_STDOUT 1

#include "../vstidrv.h"
#include <io.h>
#include <fcntl.h>

void print_help();

class vocaloidrv : public vstidrv
{
public:
    vocaloidrv( string path, string wave ) : vstidrv( path ){
        mIsRendering = false;
		mFile = NULL;
		mUseStdOut = wave.length() == 0;
		mFileName = wave;
        mBuffer = NULL;
        mBufferCount = 0;
#ifdef _DEBUG
		cout << "vocaloidrv#.ctor; mUseStdOut=" << (mUseStdOut ? "True" : "False") << "; mFileName=" << mFileName << endl;
#endif
	};

    ~vocaloidrv();

    bool open( int block_size, int sample_rate );

    bool vocaloidrv::sendEvent( unsigned char *midi_data, int *clock_data, int num_data, int targetTrack );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="total_samples"></param>
    /// <param name="mode_infinite"></param>
    /// <param name="sample_rate"></param>
    /// <param name="runner">このドライバを駆動しているRenderingRunnerのオブジェクト</param>
    /// <returns></returns>
    int startRendering( long total_samples, bool mode_infinite, int sample_rate );

    bool isRendering()
    {
        return mIsRendering;
    };

    void abortRendering()
    {
        mIsCancelRequired = true;
    };

private:
    /// <summary>
    /// 指定したタイムコードにおける，曲頭から測った時間を調べる
    /// </summary>
    double msec_from_clock( int timeCode );

    // 波形の出力処理を行う．戻り値がtrueの場合，波形処理中に中断要求が行われたことを表す
    bool waveIncoming( double *left, double *right, int length );

    static void merge_events( vector<MidiEvent *> &x0, vector<MidiEvent *> &y0, vector<MidiEvent *> &dst );

private:
    static const int TIME_FORMAT = 480;
    static const int DEF_TEMPO = 500000;           // デフォルトのテンポ．

    vector<MidiEvent *> mEvents0;
    vector<MidiEvent *> mEvents1;
    vector<TempoInfo *> mTempoList;
    bool mIsCancelRequired;
    /// <summary>
    /// StartRenderingメソッドが回っている最中にtrue
    /// </summary>
    bool mIsRendering;
	FILE *mFile;
	bool mUseStdOut;
	string mFileName;
    DWORD *mBuffer;
    int mBufferCount;

};
