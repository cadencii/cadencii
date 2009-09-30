#ifndef __straightFrame_h__
#define __straightFrame_h__

#include <math.h>

#include <iostream>
#include <sstream>
#include <fstream>
#include <string>
#include <list>
#include <map>

using namespace std;

#define SAFE_DELETE_ARRAY(x)	if(x){delete[] x;x=NULL;}
#define SAFE_DELETE(x)			if(x){delete x;x=NULL;}

class straightFrame
{
public:

	/**
	* @brief コンストラクタ。
	*/
	straightFrame(){pApBuffer=NULL;pSpBuffer=NULL;}

	/**
	* @brief デストラクタ。
	*/
	~straightFrame(){SAFE_DELETE_ARRAY(pApBuffer);SAFE_DELETE_ARRAY(pSpBuffer);}

	/**
	* @brief F0を設定する。
	*/
	void	SetF0(double dSrcF0){dF0=dSrcF0;}

	/**
	* @brief 未実装
	* @return 成功：true　失敗：false
	*/
	bool	SetFrame(straightFrame* pSrcFrame);

	/**
	* @brief 与えられた情報を自身にコピーする。
	* @return 成功：true　失敗：false
	*/
	bool	SetFrame(double dSrcF0,double* pSrcApBuffer,long nSrcApLength,double* pSrcSpBuffer,long nSrcSpLength);

	/**
	* @brief 非周期性尺度バッファを与えられたポインタへ書き込む。
	* @param nDstApLength 書き込むバッファの配列長。
	* @return 成功：true　失敗：false
	*/
	bool	GetApBuffer(double* pDstApBuffer,long nDstApLength);

	/**
	* @brief スペクトログラムバッファを与えられたポインタへ書き込む。
	* @param nDstSpLength 書き込むバッファの配列長。
	* @return 成功：true　失敗：false
	*/
	bool	GetSpBuffer(double* pDstSpBuffer,long nDstSpLength);

	/**
	* @brief 自身の基本周波数を返す。
	* @return 自身の基本周波数
	*/
	double	GetF0(void){return dF0;}

	/**
	* @brief 自身の非周期性尺度バッファの配列長を返す。
	*/
	long	GetApLength(void){if(nApLength<=0)nApLength=0; return nApLength;}

	/**
	* @brief 自身のスペクトログラムバッファの配列長を返す。
	*/
	long	GetSpLength(void){if(nSpLength<=0)nSpLength=0; return nSpLength;}

	/**
	* @brief 与えられた比率に従ってフォルマントを変化させる。
	*/
	void	ApplyFormantChange(double dChangeRate);

	/**
	* @brief 与えられた比率に従ってダイナミクスを変化させる。
	*/
	void	ApplyDynamicsChange(double dChangeRate);

private:

	/**
	* @brief 与えられた配列長に従いバッファを作成する。
	* @return 成功：true　失敗：false
	*/
	bool	CreateBuffer(long nApLength,long nSpLength);

	/**
	* @brief 与えられた配列へのポインタと配列長に従い、バッファにデータを書き込む。
	* @return 成功：true　失敗：false
	*/
	bool	SetApBuffer(double* pSrcApBuffer,long nSrcApLength);

	/**
	* @brief 与えられた配列へのポインタと配列長に従い、バッファにデータを書き込む。
	* @return 成功：true　失敗：false
	*/
	bool	SetSpBuffer(double* pSrcSpBuffer,long nSrcSpLength);

	double	dF0;				//!< @brief 基本周波数
	double*	pApBuffer;			//!< @brief 非周期性尺度用バッファ
	long	nApLength;			//!< @brief 非周期性尺度の周波数分割数
	double*	pSpBuffer;			//!< @brief スペクトログラム用バッファ
	long	nSpLength;			//!< @brief スペクトログラムの周波数分割数
};

#endif
