#include "straightdrv.h"

#include "straightNote.h"

class straightNoteManager{
public:

	/**
	* @brief デストラクタ。開放処理を行う。
	*/
	~straightNoteManager();

	/**
	* @brief 初期化関数。
	*/
	void	Initialize(string sFilePath,Straight* pStraight){this->sFilePath=sFilePath;this->pStraight=pStraight;}

	/**
	* @brief 原音の特定フレームから情報を読み出す。
	* @return 成功：true　失敗：false
	*/
	bool	GetStraightFrame(straightFrame* pDstFrame,string sLyric,long nAbsoluteFrame);

	/**
	* @brief 歌詞データから原音のフレーム長を読み出す。
	* @return 原音のフレーム長
	*/
	long	GetSrcFrameLength(string sLyric);

	/**
	* @brief 与えられた歌詞データに該当するデータを開放する。
	*/
	void	ReleaseSTF(string sLyric);

protected:
private:

	MAP_TYPE<string,straightNote*> NoteMap;	//!< @brief 読み込んだSTFファイル管理用ハッシュマップ
	list<straightNote*> NoteList;			//!< @brief メモリ管理用

	string sFilePath;						//!< @brief 音声セットへのファイルパス
	Straight* pStraight;					//!< @brief STRAIGHT用
};
