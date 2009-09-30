#ifndef __VSQSequencer_h__
#define __VSQSequencer_h__
/////includes
//
#include <list>
#include <string>
#include <iostream>
#include <sstream>
#include <fstream>
#include <map>
using namespace std;

#include <math.h>

#include "straightFrame.h"
#include "straightNote.h"
#include "UTAUManager.h"
#include "VSQEventManager.h"
#include "VSQNoteManager.h"

//コントロールトラックとその名称
#define CONTROL_TRACK_NUM 5
#define ST_PI 3.14159216

enum{
	PITCH_BEND=0,
	PITCH_SENS=1,
	DYNAMICS=2,
	BRETHINESS=3,
	GENDOR=4
};

const string sControlNames[CONTROL_TRACK_NUM]=
{"[PitchBendBPList]"
,"[PitchBendSensBPList]"
,"[DynamicsBPList]"
,"[EpRResidualBPList]"
,"[GenderFactorBPList]"
};

const int iControlDefaultValue[CONTROL_TRACK_NUM]=
{0
,2
,64
,0
,64
};

const string sNoteTrackName="[EventList]";

class VSQSequencer{
public:

	/**
	* @brief コンストラクタ。
	*/
	VSQSequencer();

	/**
	* @brief デストラクタ。
	*/
	~VSQSequencer();

	/**
	* @brief 指定されたファイルを読み込む。
	* @param dFrameShift １フレームの実時間長(ms)。
	* @return 成功：true　失敗：false
	*/
	bool		LoadFile(string sFileName,double dFrameShift);

	/**
	* @brief 指定されたフレーム時刻のノートイベントへのポインタを返す。
	* @return 失敗時はNULL。
	*/
	NoteEvent*	GetNoteEvent(long nFrame);

	/**
	* @brief 指定されたフレーム時刻の基本周波数を返す。
	* @param dFrameShift １フレームの実時間長(ms)。
	*/
	double		GetF0(long nFrame,double dFrameShift);

	/**
	* @brief 指定されたフレーム時刻のダイナミクスを返す。
	* @param dFrameShift １フレームの実時間長(ms)。
	*/
	double		GetDynamics(long nFrame,double dFrameShift);

	/**
	* @brief シーケンスの開始フレーム時刻を返す。
	*/
	long		GetBeginFrame(void){return nBeginFrame;}

	/**
	* @brief シーケンスの終了フレーム時刻を返す。
	*/
	long		GetEndFrame(void){return nEndFrame;}

	/**
	* @brief 指定されたコントロールトラックの指定されたフレーム時刻の値を返す。
	* @param dFrameShift １フレームの実時間長(ms)。
	*/
	int			GetControlValue(int iControlNum,long nFrame,double dFrameShift);

	/**
	* @brief 現在設定されているファイルパスを返す。
	*/
	string		GetFilePath(void){return sFilePath;}

protected:
private:

	/**
	* @brief 指定されたフレーム時刻でのピッチベンドの割合を返す。
	*/
	double		GetPitchBendRate(long nFrame,double dFrameShift);

	/**
	* @brief 指定されたノート由来の指定されたフレーム時刻での『基本周波数』を返す
	*/
	double		GetPitchFromNote(NoteEvent* pNoteEvent,long nFrame,double dFrameShift);

	/**
	* @brief 指定された時刻でのボリューム変化率を返す。
	*/
	double		GetDynamicsRate(long nFrame,double dFrameShift);

	/**
	* @brief 指定されたノート由来の指定されたフレーム時刻でのボリューム変化率を返す。
	*/
	double		GetDynamicsFromNote(NoteEvent* pNoteEvent,long nFrame,double dFrameShift);

	/**
	* @brief 指定されたノートの指定時刻でのVibratoによる変化率を返す
	*/
	double		GetVibratoRate(NoteEvent* pNoteEvent,long nFrame,double dFrameShift);

	UTAUManager UtauManager;					//!< @brief 原音設定管理クラス

	MAP_TYPE<string,EventManager*>	EventMap;	//!< @brief イベント呼び出し用ハッシュ
	list<EventManager*>				ManagerList;//!< @brief メモリ管理用リスト

	long				nBeginFrame;			//!< @brief 開始Frame
	long				nEndFrame;				//!< @brief 終端Frame
	long				nEndTick;
	long				nEndNoteID;				//!< @brief 終端NoteID
	double				dNoteFrequency[129];	//!< @brief 各ノートの周波数。128は無声音用。

	string				sFilePath;
	double				dTempo;
	double				dFrameShift;
};

#endif
