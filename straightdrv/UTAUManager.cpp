//UTAU原音設定管理用クラス

#include "UTAUManager.h"

/*    static map<string,UTAUSetting*> SettingMap;	//!< @brief 原音設定表
    static list<UTAUSetting*>	SettingList;			//!< @brief メモリ管理用リスト
*/
MAP_TYPE<string,UTAUSetting*> UTAUManager::SettingMap;
std::list<UTAUSetting*> UTAUManager::SettingList;

bool	UTAUManager::Initialize(string sOtoName){
    const int BUF_LEN = 256;
	char	cBuffer[BUF_LEN];
	string	sBuffer;
    FILE *ifs;
#ifdef __GNUC__
    ifs = fopen( sOtoName.c_str(), "r" );
#else
    fopen_s( &ifs, sOtoName.c_str(), "r" );
#endif

    if( NULL == ifs ){
        cout << "error; sOtoName=" << sOtoName << endl;
        cout << "error; io-error on UtauManager::Initialize; bail-out" << endl;
		return false;
    }

	SettingMap.clear();

    while( NULL != fgets( cBuffer, BUF_LEN, ifs ) ){
		sBuffer = cBuffer;
		
		//オブジェクトにデータを追加
		UTAUSetting* pTarget=new UTAUSetting;

		pTarget->sName=sBuffer.substr(0,sBuffer.find("."));

		sBuffer=sBuffer.substr(sBuffer.find(",")+1,256);
		pTarget->iLeftBlank=atoi(sBuffer.substr(0,sBuffer.find(",")).c_str());
		sBuffer=sBuffer.substr(sBuffer.find(",")+1,256);
		pTarget->iConsonant=atoi(sBuffer.substr(0,sBuffer.find(",")).c_str());
		sBuffer=sBuffer.substr(sBuffer.find(",")+1,256);
		pTarget->iRightblank=atoi(sBuffer.substr(0,sBuffer.find(",")).c_str());
		sBuffer=sBuffer.substr(sBuffer.find(",")+1,256);
		pTarget->iPrepronounce=atoi(sBuffer.substr(0,sBuffer.find(",")).c_str());
		sBuffer=sBuffer.substr(sBuffer.find(",")+1,256);
		pTarget->iOverlapped=atoi(sBuffer.substr(0,sBuffer.find(",")).c_str());
		sBuffer=sBuffer.substr(sBuffer.find(",")+1,256);
		pTarget->dBaseFrequency=atof(sBuffer.c_str());								//平均周波数、UTAU原音設定を拡張している。

		//ハッシュマップに突っ込む
		SettingMap.insert(make_pair(pTarget->sName,pTarget));
		//管理用リストにも登録
        SettingList.push_back(pTarget);

	}

	return true;
}

void	UTAUManager::Uninitialize(void)
{
	for(list<UTAUSetting*>::iterator i=SettingList.begin();i!=SettingList.end();i++)
		delete (*i);
}

UTAUSetting*	UTAUManager::GetDefaultSetting(string sLyric)
{
	MAP_TYPE<string,UTAUSetting*>::iterator i;
	i=SettingMap.find(sLyric);

	//末尾と一緒の場合はキーに一致しないのでNULLを返す。
	if(i!=SettingMap.end())
		return i->second;
	else
		return NULL;
}
