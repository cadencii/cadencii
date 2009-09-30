#ifndef __VSQEventManager_h__
#define __VSQEventManager_h__

/////includes
//
#include <list>
#include <string>
#include <iostream>
#include <sstream>
#include <fstream>
using namespace std;

#include <math.h>

#include "UTAUManager.h"

#define A4_PITCH	440.0
#define A4_NOTE		57
//Tick per Second 960/60　or 480/60
#define TICK_PER_SEC 8.0


//プロトタイプ宣言
struct	ControlEvent;
class	EventManager;

/**
* ControlEventの値を格納する構造体。
*/
struct	ControlEvent
{
	long		nTick;				//!< @brief イベントの位置(Tick指定)
	int			iValue;				//!< @brief 値
};

/**
* イベント管理用クラス。
* データのリストを保持して適宜値を返します。
*/
class	EventManager
{
public:

	/**
	* @brief デストラクタ。確保したリソースを開放します。
	*/
	virtual ~EventManager();

	/**
	* @brief 指定されたTick時刻での値を返す。
	* @param nTick Tick時刻。
	* @return 成功したら値、失敗したら-1を返す。
	*/
	virtual	int		GetValue(long nTick);

	/**
	* @brief 指定されたTick時刻での値を記憶する。
	* @param iValue 入力する値 nTick Tick時刻。
	*/
	virtual	bool	SetValue(long nTick,int iValue);

	/**
	* @brief 指定された文字列から値を登録する。
	* @param sFront 前文字列 sBack 後文字列
	*/
	virtual	bool	SetValue(string sFront,string sBack);
protected:
	list<ControlEvent*>	EventList;	//!< @brief イベントの内容を格納するリスト。
private:
};

#endif
