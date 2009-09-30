#ifndef __VSQNoteManager_h__
#define __VSQNoteManager_h__

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

#include "UTAUManager.h"
#include "VSQEventManager.h"

#define A4_PITCH	440.0
#define A4_NOTE		57
//Tick per Second 960/60　or 480/60
#define TICK_PER_SEC 8.0


struct	NoteEvent
{
	string		sLyric;				//!< @brief 発音
	unsigned	char ucNote;		//!< @brief 音高
	long		nTick;				//!< @brief 位置(Tick指定)
	long		nLength;			//!< @brief 長さ(Tick指定)
	long		nBeginFrame;
	long		nEndFrame;
	unsigned	char ucVelocity;	//!< @brief Consonant領域の長さの比
	unsigned	char ucDecay;		//!< @brief Consonant領域以降の減衰具合
	unsigned	char ucAccent;		//!< @brief Consonant領域の音の強さ
	long		nVibratoDelay;		//!< @brief Vibratoの開始位置

	bool		bIsContinuousFront;	//!< @brief 前に音符があるかどうか
	bool		bIsContinuousBack;	//!< @brief 後ろに音符があるかどうか

	UTAUSetting	Setting;			//!< @brief UTAUの設定を格納する構造体

	NoteEvent*	pPreviousNote;		//!< @brief 連続する場合前の音符データを格納
	NoteEvent*	pNextNote;			//!< @brief 連続する場合次の音符データを格納
	int			iID;

	EventManager emVibratoDepth;	//!< @brief Vibratoの深さを記録する
	EventManager emVibratoRate;		//!< @brief Vibratoの速さを記録する
};


class	NoteManager		:	public	EventManager
{
public:

	/**
	* @brief コンストラクタ。管理用変数の初期化をする。
	*/
	NoteManager(){pEditTarget=NULL;iCurrentID=-1;}

	/**
	* @brief デストラクタ。確保したリソースを開放する。
	*/
	~NoteManager();

	/**
	* @brief 指定されたIDを編集用に指定する。
	* @param iID ID番号
	*/
	void	SetEditNoteID(int iID){bHandle=false;iCurrentID=iID;pEditTarget=GetNoteEvent(iID);}

	/**
	* @brief 指定されたHandleを編集用に指定する。
	* @param iID Handle番号
	*/
	void	SetHandleID(int iID){bHandle=true;iCurrentID=iID;}

	/**
	* @brief 指定されたIDに対応するノートイベントへのポインタを探す。
	* @param iID ID番号。
	* @return 成功したらポインタ、失敗したらNULLを返す。
	*/
	NoteEvent*	GetNoteEvent(int iID);

	/**
	* @brief 指定されたTick時刻のノートイベントへのポインタを探す。
	* @param nTick Tick時刻。
	* @return 成功したらポインタ、失敗したらNULLを返す。
	*/
	NoteEvent*	GetNoteEvent(long nTick);

	/**
	* @brief 指定されたフレーム時刻のノートイベントへのポインタを探す。
	* @param nFrame フレーム時刻。
	* @return 失敗時にはNULL。
	*/
	NoteEvent*	GetNoteEventByFrame(long nFrame);

	/**
	* @brief ノート間の連続性をチェックする。
	* @param dTempo 曲のテンポ
	*/
	bool	CheckContinuity(double dTempo,UTAUManager* pUtauManager,double dFrameShift);

	/**
	* @brief 末尾の音符のEventIDを返す。
	*/
	long	GetEndID(void);

	/**
	* @brief シーケンスデータの開始フレーム時刻を返す。
	*/
	long	GetBeginFrame(void);
	/**

	* @brief シーケンスデータの終了フレーム時刻を返す。
	*/
	long	GetEndFrame(void);


protected:

	/**
	* @brief 指定された文字列から値を登録する。
	* @param sFront 前文字列 sBack 後文字列
	*/
	bool	SetValue(string sFront,string sBack);

private:

	/**
	* @brief 指定されたTick時刻にノートイベントを登録する。
	* @param iID ID番号 nTick Tick時刻。
	*/
	void	RegisterNote(long nTick,int iID);

	/**
	* @brief 指定されたIDのノートイベントを削除する。
	* @param iTargetID 削除するID番号。
	*/
	void	UnregisterNoteByID(int iTargetID);

	/**
	* @brief 指定されたIDに対応するノートイベントの実体を登録する。
	* @param iID ID番号 pNoteEvent 実体へのポインタ。
	*/
	void	SetNoteEvent(int iID,NoteEvent* pNoteEvent);

	/**
	* @brief 指定された文字列からノートのプロパティを登録する。
	* @param sFront 前文字列 sBack 後文字列
	*/
	bool	SetNoteProperty(string sFront,string sBack);

	/**
	* @brief ハンドルに設定されたデータを読み込む。
	* @param sBuffer 読み込まれた文字列
	*/
	bool	SetHandleProperty(string sFront,string sBack);

	list<NoteEvent*>		NoteList;			//!< @brief ノート情報。
	MAP_TYPE<int,NoteEvent*> NoteMap;			//!< @brief ノートをIDと対応させたハッシュ。
	MAP_TYPE<int,NoteEvent*> HandleToNoteMap;	//!< @brief Handleとノートを対応させたハッシュ。

	int	iCurrentID;						//!< @brief 現在編集中のノートID保持用変数。
	NoteEvent*	pEditTarget;			//!< @brief 現在編集中のノートイベントへのポインタ。

	bool				bHandle;		//!< @brief Handleを登録中か否か

};

#endif
