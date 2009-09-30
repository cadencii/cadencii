//UTAU原音設定管理用クラス

#ifndef __UTAUManager_h__
#define __UTAUManager_h__

#include "straightdrv.h"
//
#include <list>
#include <string>
#include <iostream>
#include <sstream>
#include <fstream>
using namespace std;

/**
* UTAU原音設定格納用構造体。
*/
struct UTAUSetting
{
	string	sName;			//!< @brief 発音名
	int		iLeftBlank;		//!< @brief 左ブランク(ms)
	int		iConsonant;		//!< @brief 子音固定長(ms)
	int		iRightblank;	//!< @brief 右ブランク(ms)
	int		iPrepronounce;	//!< @brief 先行発音長(ms)
	int		iOverlapped;	//!< @brief オーバーラップ(ms)
	double	dBaseFrequency;	//!< @brief 基本周波数(Hz)
};



/**
* UTAUの原音設定を管理するクラス。
*/
class UTAUManager
{
public:


	/**
	* @brief UTAUManagerを初期化する。
	* @param sOtoName 原音設定ファイル名を格納したstring型。
	* @return 成功したらtrue、失敗したらfalse。
	*/
	bool	Initialize(string sOtoName);

	/**
	* @brief UTAUManagerを開放する。
	*/
	void	Uninitialize(void);

	/**
	* @brief 指定された歌詞の原音設定を取得する。
	* @param sLyric 歌詞を格納したstring型。
	* @return 成功したら設定へのポインタ、失敗したらNULL。
	*/
	UTAUSetting*	GetDefaultSetting(string sLyric);

protected:
private:
    static list<UTAUSetting*> SettingList;		//!< @brief メモリ管理用リスト
    static MAP_TYPE<string,UTAUSetting*> SettingMap;	//!< @brief 原音設定hyou(`hyou' wo kanji de kakuto g++ de compile dekinai yo!! :( )
};

#endif
