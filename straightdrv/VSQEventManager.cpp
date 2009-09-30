#include "VSQEventManager.h"

EventManager::~EventManager()
{
	for(list<ControlEvent*>::iterator i=EventList.begin();i!=EventList.end();i++){
		if((*i)!=NULL)
			delete (*i);
	}
}
int		EventManager::GetValue(long nTick)
{
	if(nTick>10890){
		int a=0;
		a++;
	}
	if(EventList.size()==0)
		return -1;
	//線形に値を探す。
	list<ControlEvent*>::iterator i=EventList.begin();

	//最初のイベントより前
	if(nTick<(*i)->nTick)
		return -1;

	for(i=EventList.begin();i!=EventList.end();i++){
		if((*i)->nTick>nTick){
			if(i!=EventList.begin() && (*i)->nTick!=nTick)
				i--;
			return (*i)->iValue;
		}
	}
	if(i!=EventList.begin())
		i--;

	//見つからなければ最後の要素の値を返す
	return (*i)->iValue;
}


bool	EventManager::SetValue(long nTick,int iValue)
{
	//挿入したいデータを構造体に格納する。
	ControlEvent* pEvent = new ControlEvent;
	pEvent->nTick=nTick;
	pEvent->iValue=iValue;

	//メモリ不足
	if(NULL==pEvent)
		return false;

	//リストが空なら先頭に挿入する。
	if(EventList.size()==0){
		EventList.push_front(pEvent);
		return true;
	}

	//時刻が一致したら値を変更するだけ
	for(list<ControlEvent*>::iterator i=EventList.begin();i!=EventList.end();i++){
		if((*i)->nTick==nTick){
			delete pEvent;				//余分に確保したので開放
			(*i)->iValue=iValue;
			return true;
		}else if((*i)->nTick>nTick){

			//時刻が一致しなければ値を設定する
			EventList.insert(i,pEvent);
			return true;
		}
	}

	//入れる場所がどこにもなければ一番最後に入れる
	EventList.push_back(pEvent);

	return true;
}

bool	EventManager::SetValue(string sFront,string sBack)
{
	return SetValue(atol(sFront.c_str()),atoi(sBack.c_str()));
}

