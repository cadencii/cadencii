#ifndef __straightSynthesizer_h__
#define __straightSynthesizer_h__

/////includes
//
#define _CRTDBG_MAP_ALLOC
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#ifndef __GNUC__
#include <crtdbg.h>
#endif

#include <straight/straight.h>

//
#include <list>
#include <string>
#include <iostream>
#include <sstream>
#include <fstream>
using namespace std;

//
#include <math.h>

#include "straightFrame.h"
#include "straightNote.h"
#include "straightNoteMAnager.h"
#include "UTAUManager.h"
#include "VSQEventManager.h"
#include "VSQNoteManager.h"
#include "VSQSequencer.h"

class straightSynthesizer{
public:
	/**
	* @brief コンストラクタ。STRAIGHT用の変数を初期化。
	*/
	straightSynthesizer(){straight=NULL;source=NULL;specgram=NULL;synth=NULL;}

	/**
	* @brief デストラクタ。開放処理を行う。
	*/
	~straightSynthesizer();

	/**
	* @brief 初期化関数。
	* @param sFileName 読み込むファイル名
	* @return 成功：true　失敗：false
	*/
	bool Initialize(string sFileName);

	/**
	* @brief 合成用関数
	* @param sFileName 書き出すファイル名
	* @return 成功：true　失敗：false
	*/
	bool Synthesize(string sFileName);

	/**
	* @brief 開放用関数。
	*/
	void Uninitialize();
protected:
private:

	/**
	* @brief ノート情報と現在フレーム等から、元音声上での位置を計算する。
	* @param pNoteEvent ノート情報へのポインタ nFrame 現在の位置 nSrcFrameLength 元音声のフレーム長
	* @return 元音声上でのフレーム位置。
	*/
	long CalculateAbsoluteFrame(NoteEvent* pNoteEvent,long nFrame,long nSrcFrameLength);

	/**
	* @brief クリッピングしないようにノーマライズ処理を行う。
	*/
	void Normalize(void);

	VSQSequencer		Sequencer;					//!< @brief VSQ由来の情報管理
	string				sFilePath;					//!< @brief 音声セットへのファイルパス

	Straight			straight;					//!< @brief STRAIGHT用
	StraightSource		source;						//!< @brief STRAIGHT用
	StraightSpecgram	specgram;					//!< @brief STRAIGHT用
	StraightSynth		synth;						//!< @brief STRAIGHT用
	StraightConfig		config;						//!< @brief STRAIGHT用
};

static stBool callbackFunc(Straight straight, stCallbackType callbackType,
                           void *callbackData, void *userData)
{
    int percent;
    
    if (callbackType == STRAIGHT_F0_PERCENTAGE_CALLBACK) {
        percent = (int)callbackData;
        fprintf(stderr, "STRAIGHT F0: %d %%\r", percent);
        if (percent >= 100) {
            fprintf(stderr, "\n");
        }
    } else if (callbackType == STRAIGHT_AP_PERCENTAGE_CALLBACK) {
        percent = (int)callbackData;
        fprintf(stderr, "STRAIGHT AP: %d %%\r", percent);
        if (percent >= 100) {
            fprintf(stderr, "\n");
        }
    } else if (callbackType == STRAIGHT_SPECGRAM_PERCENTAGE_CALLBACK) {
        percent = (int)callbackData;
        fprintf(stderr, "STRAIGHT spectrogram: %d %%\r", percent);
        if (percent >= 100) {
            fprintf(stderr, "\n");
        }
    } else if (callbackType == STRAIGHT_SYNTH_PERCENTAGE_CALLBACK) {
        percent = (int)callbackData;
        fprintf(stderr, "STRAIGHT synthesis: %d %%\r", percent);
        if (percent >= 100) {
            fprintf(stderr, "\n");
        }
    }

    return ST_TRUE;
}

#endif
