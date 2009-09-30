#include "straightdrv.h"
#include "straightNoteManager.h"

straightNoteManager::~straightNoteManager()
{
	for(list<straightNote*>::iterator i=NoteList.begin();i!=NoteList.end();i++)
		SAFE_DELETE(*i);
}

bool straightNoteManager::GetStraightFrame(straightFrame *pDstFrame,string sLyric, long nAbsoluteFrame)
{
	if(pDstFrame==NULL || pStraight==NULL)
		return false;

	bool bResult=false;
    MAP_TYPE<string,straightNote*>::iterator h_i;
	straightNote* pTemp;

    h_i = NoteMap.find(sLyric);

	if(h_i==NoteMap.end()){
		pTemp=new straightNote();

		string sFileName=sFilePath+sLyric+".stf";

		pTemp->LoadSTFFile(pStraight,sFileName);
		NoteMap.insert(make_pair(sLyric,pTemp));
		NoteList.push_back(pTemp);
	}else{
		pTemp=h_i->second;
	}

	bResult=pTemp->GetFrame(pDstFrame,nAbsoluteFrame);

	return bResult;
}

long straightNoteManager::GetSrcFrameLength(string sLyric)
{

	MAP_TYPE<string,straightNote*>::iterator h_i;
	straightNote* pTemp;

	h_i=NoteMap.find(sLyric);

	if(h_i==NoteMap.end()){
		pTemp=new straightNote();

		string sFileName=sFilePath+sLyric+".stf";

		pTemp->LoadSTFFile(pStraight,sFileName);
		NoteMap.insert(make_pair(sLyric,pTemp));
		NoteList.push_back(pTemp);
	}else{
		pTemp=h_i->second;
	}

	return pTemp->GetFrameLength();
}

void straightNoteManager::ReleaseSTF(string sLyric)
{
	MAP_TYPE<string,straightNote*>::iterator h_i;

	h_i=NoteMap.find(sLyric);

	if(h_i!=NoteMap.end()){
		for(list<straightNote*>::iterator i=NoteList.begin();i!=NoteList.end();i++){
			if((*i)==h_i->second){
				NoteList.erase(i);
				break;
			}
		}
		SAFE_DELETE(h_i->second);
		NoteMap.erase(h_i);
	}

	return;
}
