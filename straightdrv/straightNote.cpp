#include "straightNote.h"

bool straightNote::GetFrame(straightFrame *pDstFrame, long nAbsoluteFrame){
	
	bool bResult=false;
	stBool stResult;
	long nApLength,nSpLength;
	double *pApBuffer,*pSpBuffer;
	double dF0;

	if(pDstFrame!=NULL && nAbsoluteFrame>= 0 && nAbsoluteFrame < nFrameLength){

		if(pStraight!=NULL && source!=NULL && specgram!=NULL){

			if(ST_TRUE==straightSourceGetF0(*pStraight,source,nAbsoluteFrame,&dF0)){

				nApLength=straightSourceGetAperiodicityFrequencyLength(source);
				nSpLength=straightSpecgramGetFrequencyLength(specgram);

				pApBuffer=new double[nApLength];
				pSpBuffer=new double[nSpLength];

				if(pApBuffer!=NULL && pSpBuffer!=NULL){
					stResult=straightSourceGetAperiodicity(*pStraight,source,nAbsoluteFrame,pApBuffer,nApLength);

					if(ST_TRUE==stResult){
						stResult=straightSpecgramGetSpectrum(*pStraight,specgram,nAbsoluteFrame,pSpBuffer,nSpLength);

						if(ST_TRUE==stResult){
							bResult=pDstFrame->SetFrame(dF0,pApBuffer,nApLength,pSpBuffer,nSpLength);
						}
					}
				}

				SAFE_DELETE_ARRAY(pApBuffer);
				SAFE_DELETE_ARRAY(pSpBuffer);
			}
		}
	}

	return bResult;
}

bool straightNote::LoadSTFFile(Straight *pStraight, string sFileName){

	if(pStraight==NULL)
		return false;

	bool bResult=false;
	StraightFile	sf;
	char inputFileName[256];

#ifdef __GNUC__
    strcpy( inputFileName, sFileName.c_str() );
#else
	strcpy_s(inputFileName,256,sFileName.c_str());
#endif

	this->pStraight=pStraight;

	if ((sf = straightFileOpen(inputFileName, "r")) != NULL) {
		if (straightFileLoadHeader(sf, *pStraight) == ST_TRUE) {
			char chunkId[5];
			long chunkSize;

			source = straightSourceInitialize(*pStraight, NULL);
			specgram = straightSpecgramInitialize(*pStraight, NULL);
            
			while (straightFileLoadChunkInfo(sf, chunkId, &chunkSize) == ST_TRUE) {
				if (strcmp(chunkId, "F0  ") == 0) {
					straightFileLoadF0(sf, source);
				} else if (strcmp(chunkId, "AP  ") == 0) {
					straightFileLoadAperiodicity(sf, source);
				} else if (strcmp(chunkId, "SPEC") == 0) {
					straightFileLoadSpecgram(sf, specgram);
				} else {
					straightFileSkipChunk(sf, chunkSize);
				}
			}
			straightFileClose(sf);
			nFrameLength=straightSpecgramGetNumFrames(specgram);
			bResult=true;
        }
	}else{
		source=NULL;
		specgram=NULL;
	}

	return true;
}

