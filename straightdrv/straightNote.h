#ifndef __straightNote_h__
#define __straightNote_h__

#include <iostream>
#include <sstream>
#include <fstream>
#include <string>
#include <list>
#include <map>

using namespace std;

#include <straight/straight.h>

#include "straightFrame.h"
#include "VSQNoteManager.h"

#define SAFE_DELETE_ARRAY(x)	if(x){delete[] x;x=NULL;}
#define SAFE_DELETE(x)			if(x){delete x;x=NULL;}

class	straightNote{
public:

	/**
	* @brief コンストラクタ。各種変数を初期化。
	*/
	straightNote(){source=NULL;specgram=NULL;pStraight=NULL;nFrameLength=-1;}

	/**
	* @brief デストラクタ。開放処理。
	*/
	~straightNote(){if(source)straightSourceDestroy(source);if(specgram)straightSpecgramDestroy(specgram);}

	/**
	* @brief フレーム番号から自身の該当する情報を読み出す。
	* @param pDstFrame 読み出した情報を書き込むstraightFrameへのポインタ。
	* @return 成功：true　失敗：false
	*/
	bool	GetFrame(straightFrame* pDstFrame,long nFrame);

	/**
	* @brief STFファイル読み込み関数
	* @param sFileName 読み込むファイル名
	* @return 成功：true　失敗：false
	*/
	bool	LoadSTFFile(Straight* pStraight,string sFileName);

	/**
	* @brief 自身のフレーム長を返す。
	* @return 自身のフレーム長
	*/
	long	GetFrameLength(void){return nFrameLength;}

private:
	long	nFrameLength;					//!< @brief 確保したデータのフレーム長

	Straight*			pStraight;			//!< @brief STRAIGHT用
	StraightSource		source;				//!< @brief 確保した音源情報
	StraightSpecgram	specgram;			//!< @brief 確保したスペクトログラム情報
};

#endif
