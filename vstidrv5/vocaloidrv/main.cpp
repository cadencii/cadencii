#include "vocaloidrv.h"

int main( int argc, char* argv[] )
{
    /*if( argc < 3 ){
        print_help();
        return 0;
    }*/
    string dll_path = "C:\\Program Files\\Steinberg\\VSTplugins\\VOCALOID2\\VOCALOID2.dll";//argv[1];
    int sample_rate = 44100;// atoi( argv[2] );
    cout << "dll_path=" << dll_path << endl;
    cout << "sample_rate=" << sample_rate << endl;
    vocaloidrv drv( dll_path.c_str() );
	bool ret = drv.open( sample_rate, sample_rate );
	cout << "_tmain; ret=" << (ret ? "True" : "False") << endl;
	int i;
	cin >> i;
	return 0;
}

void print_help()
{
    cout << "vocaloidrv" << endl;
    cout << "Copyright (C) 2011, kbinani" << endl;
    cout << "Usage:" << endl;
    cout << "    vocaloidrv [vsti path] [sample rate]" << endl;
}

VstIntPtr AudioMaster( AEffect* effect, VstInt32 opcode, VstInt32 index, VstIntPtr value, void* ptr, float opt )
{
    VstIntPtr result = 0;

    switch( opcode ){
        case audioMasterVersion:{
            result = kVstVersion;
            break;
        }
    }
    return result;
}
