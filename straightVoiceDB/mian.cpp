#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include <iostream>
#include <fstream>

using namespace std;

#include <straight/straight.h>

int main( int argc, char *argv[] ){
    if( argc < 5 ){
        cout << "straightVoiceDB" << endl;
        cout << "usage:" << endl;
        cout << "    straightVoiceDB [wav_path] [stf_path] [offset] [blank]" << endl;
        return 0;
    }

    string wav_path = argv[1];
    string stf_path = argv[2];
    int offset = atoi( argv[3] ); // == iLeftBlank
    int blank = atoi( argv[4] ); // == iRightBlank

	StraightConfig config;  /* STRAIGHTエンジン全体の設定用構造体 */
	Straight straight;
	StraightSource source;
	StraightSpecgram specgram;
	StraightFile sf;

	straightInitConfig( &config );
	config.samplingFrequency = 44100.0;
	config.f0Ceil = 1000.0;

	if( (straight = straightInitialize( &config )) == NULL ){
		cout << "error; failed initializing straight." << endl;
		return 0;
	}

	char szWavPath[260];
    strcpy_s( szWavPath, 260, wav_path.c_str() );
    if( straightReadAudioFile( straight, szWavPath, NULL ) == ST_FALSE ){
        cout << "error; failed reading wave file: \"" << wav_path.c_str() << "\"" << endl;
		return 0;
	}

    cout << "start analyzing wave file: \"" << wav_path.c_str() << "\" ..." << endl;

    cout << "  source compute... ";
	source = straightSourceInitialize( straight, NULL );
	straightSourceCompute( straight, source, NULL, 0 );
	cout << "done." << endl;

    cout << "  specgram compute... ";
	specgram = straightSpecgramInitialize( straight, NULL );
	straightSpecgramCompute( straight, source, specgram, NULL, 0 );
	cout << "done." << endl;

    cout << "  info; f0Ceil: " << config.f0Ceil << " f0Floor: " << config.f0Floor << " FFTLength: " << config.FFTLength << endl;
	cout << "  info; frameLength: " << config.frameLength << " frameShift: " << config.frameShift << " samplingFrequency: " << config.samplingFrequency << endl;
	cout << "done." << endl;
	cout << "start cutting blanks... ";

	StraightSource tempSource;
	StraightSpecgram tempSpecgram;
	double dF0, *pTempApBuffer, *pTempSpBuffer;
	double *pSrcApBuffer, *pSrcSpBuffer;
	long nTempBegin, nTempEnd, nTempLength, nTempApLength, nTempSpLength;
	long nSrcApLength, nSrcSpLength;

	tempSource = straightSourceInitialize( straight, NULL );
	tempSpecgram = straightSpecgramInitialize( straight, NULL );

	nTempLength = straightSpecgramGetNumFrames( specgram );
	nTempBegin = (long)(offset / config.frameShift);
	nTempEnd = nTempLength - (long)(blank / config.frameShift);

	nTempLength = nTempEnd - nTempBegin;

	straightSourceCreateF0( straight, tempSource, nTempLength );
	straightSourceCreateAperiodicity( straight, tempSource, nTempLength );
	straightSpecgramCreate( straight, tempSpecgram, nTempLength );

	nTempApLength = straightSourceGetAperiodicityFrequencyLength( tempSource );
	nTempSpLength = straightSpecgramGetFrequencyLength( tempSpecgram );
	nSrcApLength  = straightSourceGetAperiodicityFrequencyLength( source );
	nSrcSpLength  = straightSpecgramGetFrequencyLength( specgram );

	for( long n = 0; n < nTempLength; n++ ){
		long nSrc = n + nTempBegin;
		pTempApBuffer = straightSourceGetAperiodicityPointer( straight, tempSource, n );
		pSrcApBuffer  = straightSourceGetAperiodicityPointer( straight, source, nSrc );
		pTempSpBuffer = straightSpecgramGetSpectrumPointer( straight, tempSpecgram, n );
		pSrcSpBuffer  = straightSpecgramGetSpectrumPointer( straight, specgram, nSrc );

		straightSourceGetF0( straight, source, nSrc, &dF0 );

        for( long i = 0; i < nTempApLength; i++ ){
            long index = (long)((i * (double)nSrcApLength / (double)nTempApLength));
			pTempApBuffer[i] = pSrcApBuffer[index];
        }
        for( long i = 0; i < nTempSpLength; i++ ){
            long index = (long)((i * (double)nSrcSpLength / (double)nTempSpLength));
			pTempSpBuffer[i] = pSrcSpBuffer[index];
        }

		straightSourceSetF0( straight, tempSource, n, dF0 );
	}

	cout << "done." << endl;

    char szStfPath[260];
    strcpy_s( szStfPath, 260, stf_path.c_str() );
    if( (sf = straightFileOpen( szStfPath, "w" )) != NULL ){
		straightFileSetChunkList( sf, "F0  AP  SPEC" );
		straightFileWriteHeader( sf, straight );
		straightFileWriteF0Chunk( sf, tempSource );
		straightFileWriteAperiodicityChunk( sf, tempSource );
		straightFileWriteSpecgramChunk( sf, tempSpecgram );

		straightFileClose( sf );

        cout << "info; created STF file: \"" << stf_path.c_str() << "\"" << endl;
	}else{
		cout << "error; cannot write STF file: \"" << stf_path.c_str() << "\"" << endl;
	}

	straightSourceDestroy( tempSource );
	straightSpecgramDestroy( tempSpecgram );
	straightSourceDestroy( source );
	straightSpecgramDestroy( specgram );

	straightDestroy( straight );

	return 0;
}
