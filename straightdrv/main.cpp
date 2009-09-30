#include "straightSynthesizer.h"

int main( int argc, char *argv[] ){
#ifndef __GNUC__
	_CrtSetDbgFlag( _CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF );
#endif

	straightSynthesizer Synthesizer;

	string sUsqPath = "";
	string sWavPath = "";

	if( argc == 3 ){
		sUsqPath = argv[1];
		sWavPath = argv[2];
	}else{
        return 0;
	}

	if( sUsqPath.compare( "" ) == 0 ){
        return 0;
	}
    if( sWavPath.compare( "" ) == 0 ){
        return 0;
    }

	if( Synthesizer.Initialize( sUsqPath ) ){
		Synthesizer.Synthesize( sWavPath );
	}

	return 0;
}
