/*
 * main.cpp
 * Copyright © 2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#include "vocaloidrv.h"

int main( int argc, char* argv[] )
{
    if( argc < 5 ){
        print_help();
        return -1;
    }
    string dll_path = argv[1];// "C:\\Program Files\\Steinberg\\VSTplugins\\VOCALOID2\\VOCALOID2.dll";//argv[1];
    string midi_file[2] = { argv[2], argv[3] };
    long total_samples = atoi( argv[4] );
    int sample_rate = 44100;
	string wav = "";
	if( argc >= 6 ){
		wav = argv[5];
	}
	bool use_stdout = wav.compare( "" ) == 0;
    //cout << "dll_path=" << dll_path << endl;
    vocaloidrv drv( dll_path, wav );
    if( drv.open( sample_rate, sample_rate ) ){
        //cout << "drv.open; successed" << endl;
    }else{
        cout << "drv.open; failed" << endl;
        return 1;
    }

    // マスタートラックのMIDIデータを送信
    const int UNIT_LEN = 512;
    int clocks_length = UNIT_LEN;
    int *clocks = (int *)malloc( sizeof( int ) * clocks_length );
    unsigned char *dat = (unsigned char *)malloc( sizeof( unsigned char ) * (clocks_length * 3) );
    for( int track = 0; track < 2; track++ ){
		FILE *fp = fopen( midi_file[track].c_str(), "rb" );
        unsigned char buf[4];
        unsigned char mid[3];
        int i = 0;
        int j = 0;
        while( true ){
            // クロック，4 bytes
            if( fread( buf, sizeof( unsigned char ), 4, fp ) < 4 ){
                break;
            }
            // MIDI data 3 bytes
            if( fread( mid, sizeof( unsigned char ), 3, fp ) < 3 ){
                break;
            }
            int clock = buf[0] | (buf[1] << 8) | (buf[2] << 16) | (buf[3] << 24);
            if( i >= clocks_length ){
                clocks_length += UNIT_LEN;
                clocks = (int *)realloc( clocks, sizeof( int ) * clocks_length );
                dat = (unsigned char *)realloc( dat, sizeof( unsigned char ) * (clocks_length * 3) );
            }
            clocks[i] = clock;
            dat[j] = mid[0];
            dat[j + 1] = mid[1];
            dat[j + 2] = mid[2];
            i++;
            j += 3;
        }
        drv.sendEvent( dat, clocks, i, track );
        fclose( fp );
    }
	if( clocks ) free( clocks );
	if( dat ) free( dat );

	if( use_stdout ){
		_setmode( _fileno( stdout ), _O_BINARY );
	}
	drv.startRendering( total_samples, false, sample_rate );
    
    return 0;
}

void print_help()
{
    cout << "vocaloidrv" << endl;
    cout << "Copyright (C) 2011, kbinani" << endl;
    cout << "Usage:" << endl;
    cout << "    vocaloidrv [vsti path] [midi master] [midi body] [total samples]" << endl;
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
