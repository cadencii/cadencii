#include "straightFrame.h"

bool	straightFrame::SetFrame(straightFrame *pSrcFrame)
{

	bool bResult=false;

	if(pSrcFrame!=NULL){

		bResult=true;
	}

	return bResult;
}

bool	straightFrame::SetFrame(double dSrcF0, double *pSrcApBuffer, long nSrcApLength, double *pSrcSpBuffer, long nSrcSpLength)
{
	SAFE_DELETE_ARRAY(pApBuffer);
	SAFE_DELETE_ARRAY(pSpBuffer);

	bool bResult=false;

	if(pSrcApBuffer!=NULL && pSrcSpBuffer!=NULL){
		if((bResult=CreateBuffer(nSrcApLength,nSrcSpLength))){
			bResult =SetApBuffer(pSrcApBuffer,nSrcApLength);
			bResult&=SetSpBuffer(pSrcSpBuffer,nSrcSpLength);
		}
	}

	return bResult;
}

bool	straightFrame::CreateBuffer(long nApLength, long nSpLength)
{

	SAFE_DELETE_ARRAY(pApBuffer);
	SAFE_DELETE_ARRAY(pSpBuffer);

	this->nApLength=nApLength;
	this->nSpLength=nSpLength;

	if(nApLength > 0 && nSpLength > 0){
		pApBuffer=new double[nApLength];
		pSpBuffer=new double[nSpLength];
	}

	if(pApBuffer!=NULL && pSpBuffer!=NULL)
		return true;
	else
		return false;

}

bool	straightFrame::GetApBuffer(double* pDstApBuffer,long nDstApLength)
{
	
	bool bResult=false;

	if(pDstApBuffer!=NULL && nDstApLength>0 && pApBuffer!=NULL && nApLength>0){
		for(long n=0;n<nDstApLength;n++){
			long nSrc=long((double)n*(double)nApLength/(double)nDstApLength);
			pDstApBuffer[n]=pApBuffer[nSrc];
		}
		bResult=true;
	}

	return bResult;
}

bool	straightFrame::GetSpBuffer(double* pDstSpBuffer,long nDstSpLength)
{
	
	bool bResult=false;

	if(pDstSpBuffer!=NULL && nDstSpLength>0 && pSpBuffer!=NULL && nSpLength>0){
		for(long n=0;n<nDstSpLength;n++){
			long nSrc=long((double)n*(double)nSpLength/(double)nDstSpLength);
			pDstSpBuffer[n]=pSpBuffer[nSrc];
		}
		bResult=true;
	}

	return bResult;
}

bool	straightFrame::SetApBuffer(double *pSrcApBuffer, long nSrcApLength){

	bool bResult=false;

	if(pSrcApBuffer!=NULL && nSrcApLength>0 && pApBuffer!=NULL && nApLength>0){
		for(long n=0;n<nApLength;n++){
			//FrequencyLengthが異なれば最も近い位置の値で代用。
			//平均値にしたければ式を変えるべし。
			long nSrc=long((double)n*(double)nSrcApLength/(double)nApLength);
			pApBuffer[n]=pSrcApBuffer[nSrc];
		}
		bResult=true;
	}

	return bResult;
}

bool	straightFrame::SetSpBuffer(double *pSrcSpBuffer, long nSrcSpLength)
{

	bool bResult=false;

	if(pSrcSpBuffer!=NULL && nSrcSpLength>0 && pSpBuffer!=NULL && nSpLength>0){
		for(long n=0;n<nSpLength;n++){
			//FrequencyLengthが異なれば最も近い位置の値で代用。
			//平均値にしたければ式を変えるべし。
			long nSrc=long((double)n*(double)nSrcSpLength/(double)nSpLength);
			pSpBuffer[n]=pSrcSpBuffer[nSrc];
		}
		bResult=true;
	}

	return bResult;
}


void	straightFrame::ApplyFormantChange(double dChangeRate)
{
	if(NULL!=pSpBuffer && NULL!=pApBuffer && dChangeRate>0){
		double* pTempApBuffer=new double[nApLength];
		double* pTempSpBuffer=new double[nSpLength];

		memcpy(pTempApBuffer,pApBuffer,sizeof(double)*nApLength);
		memcpy(pTempSpBuffer,pSpBuffer,sizeof(double)*nSpLength);

		//非周期性指標
		for(long n=0;n<nApLength;n++){
			double dPos=(double)n*dChangeRate;
			if(dPos<nApLength-1){
				pApBuffer[n]=pTempApBuffer[(long)dPos]*((long)dPos+1.0-dPos) + pTempApBuffer[(long)dPos+1]*(dPos-(long)dPos);
			}else if(dPos==nApLength-1){
				pApBuffer[n]=pTempApBuffer[nApLength-1];
			}else{
				pApBuffer[n]=0.0;
			}
		}

		//スペクトルを変化比率によって伸縮させる。
		for(long n=0;n<nSpLength;n++){
			double dPos=(double)n*dChangeRate;
			if(dPos<nSpLength-1){
				pSpBuffer[n]=pow(pTempSpBuffer[(long)dPos],((long)dPos+1.0-dPos))*pow(pTempSpBuffer[(long)dPos+1],(dPos-(long)dPos));
			}else if(dPos==nSpLength-1){
				pSpBuffer[n]=pTempSpBuffer[nSpLength-1];
			}else{
				pSpBuffer[n]=0.0;
			}
		}
		SAFE_DELETE_ARRAY(pTempApBuffer);
		SAFE_DELETE_ARRAY(pTempSpBuffer);
	}
}


void	straightFrame::ApplyDynamicsChange(double dChangeRate)
{

	if(NULL!=pSpBuffer && NULL!=pApBuffer && dChangeRate>=0){
		for(long n=0;n<nApLength;n++)
			pApBuffer[n]*=dChangeRate;

		for(long n=0;n<nSpLength;n++)
			pSpBuffer[n]*=dChangeRate;
	}
}
