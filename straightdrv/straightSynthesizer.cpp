#include "straightSynthesizer.h"

straightSynthesizer::~straightSynthesizer()
{
	if(synth)		straightSynthDestroy(synth);
	if(source)		straightSourceDestroy(source);
	if(specgram)	straightSpecgramDestroy(specgram);
	if(straight)	straightDestroy(straight);
}


bool straightSynthesizer::Initialize(string sFileName)
{
	bool bResult=false;

	straightInitConfig(&config);
	config.samplingFrequency=44100.0;
	config.f0Ceil=1000.0;

	if((straight=straightInitialize(&config))!=NULL){
		if((source=straightSourceInitialize(straight,NULL))!=NULL){
			if((specgram=straightSpecgramInitialize(straight,NULL))!=NULL){
				if((synth=straightSynthInitialize(straight,NULL))!=NULL){
					bResult=true;
				}else
					cout << "error; straightSynthInitialize" << endl;
			}else
				cout << "error; straigthSpecgramInitialize" << endl;
		}else
			cout << "error; straightSourceInitialize" <<endl;
	}else
		cout << "error; straightInitialize" << endl;

	straightSetCallbackFunc(straight, STRAIGHT_PERCENTAGE_CALLBACK, callbackFunc, NULL);

	if(bResult)
		bResult=Sequencer.LoadFile(sFileName,config.frameShift);

	sFilePath=Sequencer.GetFilePath();

	return bResult;
}

bool straightSynthesizer::Synthesize(string sFileName)
{
	bool bResult=false;

	long nBeginFrame=0;//Sequencer.GetBeginFrame();
	long nEndFrame=Sequencer.GetEndFrame() + 500;
	long nFrameLength=nEndFrame-nBeginFrame;

	long nApLength,nSpLength;

	double dF0,dDynamics,dFormant;
	NoteEvent*     pNoteEvent;
	straightFrame  stFrame;
	straightNoteManager stManager;

	straightSourceCreateF0(straight,source,nFrameLength);
	straightSourceCreateAperiodicity(straight,source,nFrameLength);
	straightSpecgramCreate(straight,specgram,nFrameLength);

	nApLength=straightSourceGetAperiodicityFrequencyLength(source);
	nSpLength=straightSpecgramGetFrequencyLength(specgram);

	double *pApBuffer = new double[nApLength];
	double *pSpBuffer = new double[nSpLength];

	memset(pApBuffer,0,sizeof(double)*nApLength);
	memset(pSpBuffer,0,sizeof(double)*nSpLength);
	dF0=0.0;

	stManager.Initialize(sFilePath,&straight);

	stFrame.SetFrame(dF0,pApBuffer,nApLength,pSpBuffer,nSpLength);

	for(long n=0;n<nFrameLength;n++){

		//ここに合成アルゴリズム
		long nFrame = n+nBeginFrame;
		long nSrcFrameLength;

		pNoteEvent=Sequencer.GetNoteEvent(nFrame);

		memset(pApBuffer,0,sizeof(double)*nApLength);
		memset(pSpBuffer,0,sizeof(double)*nSpLength);
		dF0=0.0;

		stFrame.SetFrame(dF0,pApBuffer,nApLength,pSpBuffer,nSpLength);

		if(pNoteEvent!=NULL){

			bool bReadResult=true;

			//ここで各波形データからデータを持ってくる
			long nAbsoluteFrame;

			nSrcFrameLength=stManager.GetSrcFrameLength(pNoteEvent->sLyric);
			nAbsoluteFrame=CalculateAbsoluteFrame(pNoteEvent,nFrame,nSrcFrameLength);
			
			bReadResult&=stManager.GetStraightFrame(&stFrame,pNoteEvent->sLyric,nAbsoluteFrame);
			bReadResult&=stFrame.GetApBuffer(pApBuffer,nApLength);
			bReadResult&=stFrame.GetSpBuffer(pSpBuffer,nSpLength);

			if(pNoteEvent->bIsContinuousBack){
				if(pNoteEvent->pNextNote->nBeginFrame < nFrame){

					double* pTempAp = new double[nApLength];
					double* pTempSp = new double[nSpLength];
					
					nSrcFrameLength=stManager.GetSrcFrameLength(pNoteEvent->sLyric);
					nAbsoluteFrame=CalculateAbsoluteFrame(pNoteEvent->pNextNote,nFrame,nSrcFrameLength);

					bReadResult&=stManager.GetStraightFrame(&stFrame,pNoteEvent->pNextNote->sLyric,nAbsoluteFrame);
					bReadResult&=stFrame.GetApBuffer(pTempAp,nApLength);
					bReadResult&=stFrame.GetSpBuffer(pTempSp,nSpLength);

					if(bReadResult){
						double dMorphRate=(double)(pNoteEvent->nEndFrame-nFrame)/(double)(pNoteEvent->nEndFrame-pNoteEvent->pNextNote->nBeginFrame);

						for(long i=0;i<nApLength;i++)
							pApBuffer[i]=pTempAp[i]*(1.0-dMorphRate)+dMorphRate*pApBuffer[i];

						for(long i=0;i<nSpLength;i++)
							pSpBuffer[i]=pow(pTempSp[i],1.0-dMorphRate)*pow(pSpBuffer[i],dMorphRate);
					}

					SAFE_DELETE_ARRAY(pTempAp);
					SAFE_DELETE_ARRAY(pTempSp);
				}
			}

			//各種計算をさせる
			if(bReadResult){
				dF0=Sequencer.GetF0(nFrame,config.frameShift);
				dDynamics=Sequencer.GetDynamics(nFrame,config.frameShift);
				dFormant=pow(0.75,((double)(64.0-Sequencer.GetControlValue(GENDOR,nFrame,config.frameShift)))/64.0);

				stFrame.SetFrame(dF0,pApBuffer,nApLength,pSpBuffer,nSpLength);

				stFrame.ApplyDynamicsChange(dDynamics);
				stFrame.ApplyFormantChange(dFormant);

				stFrame.GetApBuffer(pApBuffer,nApLength);
				stFrame.GetSpBuffer(pSpBuffer,nSpLength);
			}else{
				memset(pApBuffer,0,sizeof(double)*nApLength);
				memset(pSpBuffer,0,sizeof(double)*nSpLength);
			}

			//最終フレームに自身と同じ発音のファイルを開放
			if(nFrame==pNoteEvent->nEndFrame-1){
				if(pNoteEvent->bIsContinuousBack){
					if(pNoteEvent->pNextNote->sLyric.compare(pNoteEvent->sLyric)!=0){
						stManager.ReleaseSTF(pNoteEvent->sLyric);
					}
				}else{
					stManager.ReleaseSTF(pNoteEvent->sLyric);
				}
			}

		}

		straightSourceSetF0(straight,source,n,dF0);
		straightSourceSetAperiodicity(straight,source,n,pApBuffer,nApLength);
		straightSpecgramSetSpectrum(straight,specgram,n,pSpBuffer,nSpLength);
	}

	SAFE_DELETE_ARRAY(pApBuffer);
	SAFE_DELETE_ARRAY(pSpBuffer);

	
	if (straightSynthCompute(straight, source, specgram, synth, 0.5, 1.0, 1.0) == ST_TRUE) {

		Normalize();

		char cBuf[256];
#ifdef __GNUC__
        strcpy( cBuf, sFileName.c_str() );
#else
		strcpy_s(cBuf,sFileName.c_str());
#endif
		straightWriteSynthAudioFile(straight,synth,cBuf,"output_wav",16);
	}
	
	return bResult;
}

long straightSynthesizer::CalculateAbsoluteFrame(NoteEvent *pNoteEvent, long nFrame, long nSrcFrameLength)
{
	if(pNoteEvent==NULL)
		return false;

	long nAbsoluteFrame=0;
	long nRelativeFrame=nFrame-pNoteEvent->nBeginFrame;
	long nFrameLength=pNoteEvent->nEndFrame-pNoteEvent->nBeginFrame;

	long nConsonantEndFrame=(long)((double)(pNoteEvent->Setting.iConsonant)*pow(2.0,(64-pNoteEvent->ucVelocity)/64.0)/config.frameShift);

	if(nConsonantEndFrame > nRelativeFrame){
		//現在Consonant中
		nAbsoluteFrame=(long)(pNoteEvent->Setting.iLeftBlank/config.frameShift); //LeftBlankにいくらか加える

		nAbsoluteFrame+=(long)((double)nRelativeFrame/(double)nConsonantEndFrame*(double)(pNoteEvent->Setting.iConsonant)/config.frameShift);
	}else{
		//現在持続音中
		nAbsoluteFrame=(long)((pNoteEvent->Setting.iLeftBlank+pNoteEvent->Setting.iConsonant)/config.frameShift);

		nAbsoluteFrame+=(long)((double)(nRelativeFrame-nConsonantEndFrame)/(double)(nFrameLength-nConsonantEndFrame)
					*(double)(nSrcFrameLength-(pNoteEvent->Setting.iRightblank+nConsonantEndFrame)/config.frameShift));
	}

	return nAbsoluteFrame;

}

void straightSynthesizer::Normalize(void)
{
	long nWaveLength;
	double* pWaveBuffer;

	//波形の振幅の伸張が必要になる場合以外は伸張しない。
	//0.0でもよいがその場合Dynamicsで音量を揃えているとDAW側で対応しなくてはいけなくなってしまう。
	//外部から音量Volume指定に対応させるほうがよいかも…？
	double dMaxAbsoluteValue=1.0;

	pWaveBuffer=straightSynthGetOutputWave(synth,&nWaveLength);

	if(pWaveBuffer!=NULL){

		for(long n=0;n<nWaveLength;n++){
			double dTemp=pWaveBuffer[n];
			if(dTemp<0)
				dTemp*=-1;

			if(dMaxAbsoluteValue<dTemp)
				dMaxAbsoluteValue=dTemp;
		}

		for(long n=0;n<nWaveLength;n++)
			pWaveBuffer[n]/=dMaxAbsoluteValue;
	}

	return;
}
