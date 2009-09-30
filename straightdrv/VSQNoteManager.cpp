#include "VSQNoteManager.h"


NoteManager::~NoteManager()
{
	for(list<NoteEvent*>::iterator i=NoteList.begin();i!=NoteList.end();i++){
		if((*i)!=NULL)
			delete (*i);
	}
}

void	NoteManager::RegisterNote(long nTick,int iID)
{
	NoteEvent* pNote = new NoteEvent;
	pNote->nTick=nTick;
	pNote->bIsContinuousBack=false;
	pNote->bIsContinuousFront=false;
	pNote->pNextNote=NULL;
	pNote->pPreviousNote=NULL;
	pNote->nLength=0;
	pNote->iID=iID;

	pNote->emVibratoDepth.SetValue(0,0);
	pNote->emVibratoRate.SetValue(0,0);

	//UTAU設定用構造体を初期化
	pNote->Setting.dBaseFrequency=0.0;
	pNote->Setting.iConsonant=0;
	pNote->Setting.iLeftBlank=0;
	pNote->Setting.iOverlapped=INT_MAX;
	pNote->Setting.iPrepronounce=INT_MAX;
	pNote->Setting.iRightblank=0;
	pNote->Setting.sName="";

	
	if(NoteList.size()==0){
		NoteList.push_front(pNote);
	}else{
		bool bMatch=false;
		for(list<NoteEvent*>::iterator i=NoteList.begin();i!=NoteList.end();i++){
			if((*i)->nTick==nTick){
				delete pNote;
				return;
			}else if((*i)->nTick>nTick){
				NoteList.insert(i,pNote);
				bMatch=true;
				break;
			}
		}
		if(!bMatch)
			NoteList.push_back(pNote);
	}
	NoteMap.insert(make_pair(iID,pNote));

	return;
}


void	NoteManager::SetNoteEvent(int iID, NoteEvent *pNoteEvent)
{
	MAP_TYPE<int,NoteEvent*>::iterator i=NoteMap.find(iID);

	//ハッシュに一致するものがなければ何もしない。
	if(i==NoteMap.end())
		return;

	i->second->sLyric=pNoteEvent->sLyric;
	i->second->bIsContinuousBack=pNoteEvent->bIsContinuousBack;
	i->second->bIsContinuousFront=pNoteEvent->bIsContinuousFront;
	i->second->nLength=pNoteEvent->nLength;
	i->second->ucVelocity=pNoteEvent->ucVelocity;
	i->second->ucNote=pNoteEvent->ucNote;
	i->second->ucDecay=pNoteEvent->ucDecay;
	i->second->ucAccent=pNoteEvent->ucAccent;

	return;

}

void	NoteManager::UnregisterNoteByID(int iTargetID)
{
	NoteEvent* pNoteEvent;
	MAP_TYPE<int,NoteEvent*>::iterator i=NoteMap.find(iTargetID);

	//そんなID知りません
	if(i==NoteMap.end())
		return;
	pNoteEvent=i->second;
	NoteMap.erase(i);

	for(list<NoteEvent*>::iterator i=NoteList.begin();i!=NoteList.end();i++)
		if((*i)==pNoteEvent){
			NoteList.erase(i);
			break;
		}

	if(pNoteEvent)delete pNoteEvent;

}

bool	NoteManager::SetValue(string sFront,string sBack)
{
	//ID#の設定
	if(sBack.find("ID#")!=string::npos)
		RegisterNote(atol(sFront.c_str()),atoi(sBack.substr(3,sBack.find("]")).c_str()));
	else if(!bHandle)
		return SetNoteProperty(sFront,sBack);
	else
		return SetHandleProperty(sFront,sBack);

	return true;
}

bool	NoteManager::SetNoteProperty(string sFront,string sBack)
{
	if(pEditTarget==NULL){
		//読み飛ばし設定
		cout << "warning; cannot find target; property=" << sFront.c_str() << endl;
		return true;
	}

	//今回タイプは登録しない。
	if(sFront.compare("Type")==0){
		//歌手情報の場合該当ノートは使わないので削除
		if(sBack.compare("Singer")==0){
			UnregisterNoteByID(iCurrentID);
			iCurrentID=-1;
			pEditTarget=NULL;
		}
		return true;
	}else if(sFront.compare("Length")==0){
		pEditTarget->nLength=atol(sBack.c_str());
		//順番依存でごめん
		pEditTarget->nVibratoDelay=pEditTarget->nLength;
	}else if(sFront.compare("Note#")==0){
		pEditTarget->ucNote=atoi(sBack.c_str());
	}else if(sFront.compare("Dynamics")==0){
		pEditTarget->ucVelocity=atoi(sBack.c_str());
	}else if(sFront.compare("DEMdecGainRate")==0){
		pEditTarget->ucDecay=atoi(sBack.c_str());
	}else if(sFront.compare("DEMaccent")==0){
		pEditTarget->ucAccent=atoi(sBack.c_str());
	}else if(sFront.compare("VibratoDelay")==0){
		pEditTarget->nVibratoDelay=atol(sBack.c_str());
	}else if(sFront.compare("Lyric")==0){
		pEditTarget->sLyric=sBack;
	}else if(sBack.find("h#")!=string::npos){
		//ハンドル情報を登録する。
		sBack=sBack.substr(2);
		int iHandle=atoi(sBack.c_str());
		HandleToNoteMap.insert(make_pair(iHandle,pEditTarget));
    // CHANGED:kbinani
	}else if( sFront.compare( "PreUtterance" ) == 0 ){
		pEditTarget->Setting.iPrepronounce = atoi( sBack.c_str() );
	}else if( sFront.compare( "VoiceOverlap" ) == 0 ){
		pEditTarget->Setting.iOverlapped = atoi( sBack.c_str() );
	}else{
        cout << "warning; don't know property named: " << sFront.c_str() << endl;
		return true;
	}

	return true;
}
bool	NoteManager::SetHandleProperty(string sFront,string sBack)
{
	MAP_TYPE<int,NoteEvent*>::iterator i=HandleToNoteMap.find(iCurrentID);
	if(i==HandleToNoteMap.end())
		return true;
	pEditTarget=i->second;

	static	string	sTemp;					//static使っちゃったo　rz

	//L0があれば歌詞情報を読み込む
	if(sFront.compare("L0")==0){
		sBack=sBack.substr(1);
		sBack=sBack.substr(0,sBack.find("\""));
		pEditTarget->sLyric=sBack;
	//それ以降の歌詞は後ろに付け加える
/*	}else if(sFront.find("L")==0 && sFront.compare("Length")!=0){
		sBack=sBack.substr(1);
		sBack=sBack.substr(0,sBack.find("\""));
		pEditTarget->sLyric.append(sBack.c_str());
*/
	//StartDepthの設定
	}else if(sFront.compare("StartDepth")==0){
		pEditTarget->emVibratoDepth.SetValue(pEditTarget->nVibratoDelay,atoi(sBack.c_str()));

	//StartRateの設定
	}else if(sFront.compare("StartRate")==0){
		pEditTarget->emVibratoRate.SetValue(pEditTarget->nVibratoDelay,atoi(sBack.c_str()));

	//やむを得なかった・ω・
	}else if(sFront.compare("DepthBPX")==0 || sFront.compare("RateBPX")==0){
		sTemp=sBack;

	//Vibratoの設定を書き込む
	}else if(sFront.compare("DepthBPY")==0){
		while(sBack.find(",")!=string::npos && sTemp.find(",")!=string::npos){
			long nTick =atol(sTemp.substr(0,sTemp.find(",")).c_str())*(pEditTarget->nLength-pEditTarget->nVibratoDelay)+pEditTarget->nVibratoDelay;
			int	 iValue=atoi(sBack.substr(0,sBack.find(",")).c_str());
			pEditTarget->emVibratoDepth.SetValue(nTick,iValue);
			sTemp=sTemp.substr(sTemp.find(",")+1);
			sBack=sBack.substr(sBack.find(",")+1);
		}
		long nTick =atol(sTemp.c_str())*(pEditTarget->nLength-pEditTarget->nVibratoDelay)+pEditTarget->nVibratoDelay;
		int	 iValue=atoi(sBack.c_str());
		pEditTarget->emVibratoDepth.SetValue(nTick,iValue);
	}else if(sFront.compare("RateBPY")==0){
		while(sBack.find(",")!=string::npos && sTemp.find(",")!=string::npos){
			long nTick =atol(sTemp.substr(0,sTemp.find(",")).c_str())*(pEditTarget->nLength-pEditTarget->nVibratoDelay)+pEditTarget->nVibratoDelay;
			int	 iValue=atoi(sBack.substr(0,sBack.find(",")).c_str());
			pEditTarget->emVibratoRate.SetValue(nTick,iValue);
			sTemp=sTemp.substr(sTemp.find(",")+1);
			sBack=sBack.substr(sBack.find(",")+1);
		}
		long nTick =atol(sTemp.c_str())*(pEditTarget->nLength-pEditTarget->nVibratoDelay)+pEditTarget->nVibratoDelay;
		int	 iValue=atoi(sBack.c_str());
		pEditTarget->emVibratoDepth.SetValue(nTick,iValue);
	}

	return true;
}


NoteEvent*	NoteManager::GetNoteEvent(int iID)
{
	MAP_TYPE<int,NoteEvent*>::iterator i=NoteMap.find(iID);

	//ハッシュに一致するものがなければNULLを返す。
	if(i==NoteMap.end())
		return NULL;

	return i->second;
}

NoteEvent*	NoteManager::GetNoteEvent(long nTick)
{
	for(list<NoteEvent*>::iterator i=NoteList.begin();i!=NoteList.end();i++){
		if((*i)->nTick+(*i)->nLength > nTick && nTick >= (*i)->nTick)		//間に挟まってるなら値を返す。
			return (*i);
	}

	return NULL;
}

NoteEvent*	NoteManager::GetNoteEventByFrame(long nFrame)
{
	for(list<NoteEvent*>::iterator i=NoteList.begin();i!=NoteList.end();i++){
		if((*i)->nEndFrame > nFrame && nFrame >= (*i)->nBeginFrame)		//間に挟まってるなら値を返す。
			return (*i);
	}

	return NULL;
}

bool	NoteManager::CheckContinuity(double dTempo,UTAUManager* pUtauManager,double dFrameShift)
{
	//Noteが一つも登録されていない場合
	if(NoteList.empty())
		return false;

	for(list<NoteEvent*>::iterator i=NoteList.begin();i!=NoteList.end();i++){

		NoteEvent* pPresentNote=(*i);
		//UTAUを設定
		UTAUSetting *pSetting=pUtauManager->GetDefaultSetting(pPresentNote->sLyric);
		if(pSetting){
			pPresentNote->Setting.dBaseFrequency=pSetting->dBaseFrequency;
			pPresentNote->Setting.iConsonant+=pSetting->iConsonant;
			pPresentNote->Setting.iLeftBlank+=pSetting->iLeftBlank;
			pPresentNote->Setting.iRightblank+=pSetting->iRightblank;

			if(pPresentNote->Setting.iPrepronounce==INT_MAX)
				pPresentNote->Setting.iPrepronounce=pSetting->iPrepronounce;

			if(pPresentNote->Setting.iOverlapped==INT_MAX)
				pPresentNote->Setting.iOverlapped=pSetting->iOverlapped;

		}else{
            cout << "warning; lyric: " << pPresentNote->sLyric << " is not registerd in voice DB." << endl;

			memset(&(pPresentNote->Setting),0,sizeof(UTAUSetting));
		}
	}

	for(list<NoteEvent*>::iterator i=NoteList.begin();i!=NoteList.end();i++){

		NoteEvent* pPresentNote=(*i);

		if(pPresentNote==NULL)
			break;

		//ノート開始位置を計算
		//(音符の位置)=(Tick指定位置)－(先行発音分)＊(Velocityによる比率)
		//(音符の長さ)=(Tick指定長)＋(先行発音分)＊(Velocityによる比率)＋(次の音符のオーバーラップ指定分)－(次の音符の先行発音分)＊(Velocityによる比率)
		pPresentNote->nBeginFrame=(long)((double)(pPresentNote->nTick)/TICK_PER_SEC/dTempo*1000.0/dFrameShift
									- (double)(pPresentNote->Setting.iPrepronounce)/dFrameShift * pow(2.0,(64.0-(double)(pPresentNote->ucVelocity))/64.0));

		long	nFrameLength=(long)((double)(pPresentNote->nLength)/TICK_PER_SEC/dTempo*1000.0/dFrameShift
								+ (double)(pPresentNote->Setting.iPrepronounce)/dFrameShift * pow(2.0,(64.0-(double)(pPresentNote->ucVelocity))/64.0));

		//後ろをチェックしていく。
		if(++i!=NoteList.end()){
			NoteEvent* pNextNote=(*i);


			if(pPresentNote->nTick+pPresentNote->nLength
				>= pNextNote->nTick - (double)(pNextNote->Setting.iPrepronounce)/1000.0*TICK_PER_SEC*dTempo){
					pPresentNote->bIsContinuousBack=true;
					pNextNote->bIsContinuousFront=true;
					pPresentNote->pNextNote=pNextNote;
					pNextNote->pPreviousNote=pPresentNote;

					nFrameLength+=(long)((double)(pNextNote->Setting.iOverlapped)/dFrameShift
								- (double)(pNextNote->Setting.iPrepronounce)/dFrameShift * pow(2.0,(64.0-(double)(pNextNote->ucVelocity))/64.0));
			}
		}

		pPresentNote->nEndFrame=pPresentNote->nBeginFrame+nFrameLength;
		i--;

		//位置と長さを計算
	}
	return true;
}

long	NoteManager::GetEndID(void)
{
	if(NoteList.empty())
		return -1;

	list<NoteEvent*>::iterator i=NoteList.end();

	if(i==NoteList.begin())
		return -1;
	i--;
	return (*(i))->iID;
}

long	NoteManager::GetBeginFrame(void)
{
	if(NoteList.empty())
		return -1;

	return (*NoteList.begin())->nBeginFrame;
}

long	NoteManager::GetEndFrame(void)
{
	if(NoteList.empty())
		return -1;
	
	list<NoteEvent*>::iterator i=NoteList.end();

	if(i==NoteList.begin())
		return -1;
	i--;
	return (*i)->nEndFrame;
}
