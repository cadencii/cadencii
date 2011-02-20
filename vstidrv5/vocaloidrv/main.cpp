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

uint64_t read_uint64_le( FILE *fp )
{
#ifdef _DEBUG
    cerr << "read_uint64_le" << endl;
#endif
    unsigned char buf[8];
    int a = 0;
    for( int k = 0; k < 8; k++ ){
        int t = fgetc( fp );
        if( t == EOF ){
            continue;
        }
        a++;
        buf[k] = (unsigned char)(0xff & t);
    }
    if( a < 8 ){
        return 0;
    }
    uint64_t ret = 0;
    int shift = 0;
    for( int i = 0; i < 8; i++ ){
        ret |= (buf[i] << shift);
        shift += 8;
    }
    return ret;
}

uint32_t read_uint32_le( FILE *fp )
{
#ifdef _DEBUG
    cerr << "read_uint32_le" << endl;
#endif
    unsigned char buf[4];
    int a = 0;
    for( int k = 0; k < 4; k++ ){
        int t = fgetc( fp );
        if( t == EOF ){
            continue;
        }
        a++;
        buf[k] = (unsigned char)(0xff & t);
    }
    if( a < 4 ){
        return 0;
    }
    return buf[0] | (buf[1] << 8) | (buf[2] << 16) | (buf[3] << 24);
}

void load_midi_from_file( FILE *fp, unsigned char *midi, int *clock, int *buffer_num, int *clock_num )
{
#ifdef _DEBUG
    cerr << "load_midi_from_file" << endl;
#endif
	if( NULL == fp ){
		return;
	}
    unsigned char buf[4];
    unsigned char mid[3];
	const int UNIT_LEN = 512;
    int i = 0;
    int j = 0;
    int maxcount = *clock_num;
	*clock_num = 0;
    bool consider_maxcount = maxcount > 0;
    while( consider_maxcount && i < maxcount ){
        // クロック，4 bytes
        int c = (int)read_uint32_le( fp );
        // MIDI data 3 bytes
        int a = 0;
        for( int k = 0; k < 3; k++ ){
            int t = fgetc( fp );
            if( t == EOF ){
                continue;
            }
            a++;
            mid[k] = (unsigned char)(0xff & t);
        }
        if( a < 3 ){
            break;
        }
        if( i >= (*buffer_num) ){
            (*buffer_num) += UNIT_LEN;
            clock = (int *)realloc( clock, sizeof( int ) * (*buffer_num) );
            midi = (unsigned char *)realloc( midi, sizeof( unsigned char ) * ((*buffer_num) * 3) );
        }
        clock[i] = c;
        midi[j] = mid[0];
        midi[j + 1] = mid[1];
        midi[j + 2] = mid[2];
        i++;
        j += 3;
    }
	*clock_num = i;
}

int main( int argc, char* argv[] )
{
#if defined( _DEBUG ) && defined( WIN32 )
	_CrtSetDbgFlag( _CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF );
#endif
#ifdef TEST
    vocaloidrv::flog = fopen( "log.txt", "w" );
#endif
    if( argc < 3 ){
        print_help();
        return -1;
    }
	bool mode_e = false;
	string s = argv[2];
	if( s.compare( "-e" ) == 0 ){
		mode_e = true;
	}
    string dll_path = argv[1];// "C:\\Program Files\\Steinberg\\VSTplugins\\VOCALOID2\\VOCALOID2.dll";//argv[1];
	string midi_file[2] = { "", "" };
	long total_samples = 0;
    int sample_rate = 44100;
	string wav = "";
	if( argc >= 5 ){
		midi_file[0] = argv[2];
		midi_file[1] = argv[3];
		total_samples = atoi( argv[4] );
	}
	if( argc >= 6 ){
		wav = argv[5];
	}
	bool use_stdout = wav.compare( "" ) == 0;
    //cout << "dll_path=" << dll_path << endl;
    vocaloidrv drv( dll_path, wav );
    if( drv.open( sample_rate, sample_rate ) ){
        cerr << "drv.open; successed" << endl;
    }else{
        cerr << "drv.open; failed" << endl;
        return 1;
    }

	if( use_stdout ){
		_setmode( _fileno( stdout ), _O_BINARY );
	}

	const int UNIT_LEN = 512;
	int clocks_length = UNIT_LEN;
	int *clocks = (int *)malloc( sizeof( int ) * clocks_length );
	unsigned char *dat = (unsigned char *)malloc( sizeof( unsigned char ) * (clocks_length * 3) );

    cerr << "mode_e=" << (mode_e ? "True" : "False") << endl;

    if( mode_e ){
        //_setmode( _fileno( stdin ), _O_BINARY );
		// データの書式
		//   0 byte: コマンド種類
		//   1 byte: コマンドバイトの長さ(len)
		//   2-(2+len) byte: コマンド
		//   (2+len+1)- byte: コマンド依存のデータ

		// コマンド種類
		// 0x01  マスタートラックのmidiデータ読み込み
		//     コマンドバイトの長さ: 4 byte
		//     コマンド: 読み込むmidiデータの個数をunsigned int, little endianで指示
		//     コマンド依存データ: コマンドで指定した個数分，{4byte, 3byte}のデータが繰り返し送られてくる．4byteはクロック，3byteはMIDIデータである
		// 0x02  本体トラックのmidiデータ読み込み
		//     コマンドの内容は上と同じ
		// 0x03  レンダリング開始の指示
		//     コマンドバイトの長さ: 8 byte
		//     コマンド: レンダリングするサンプル数をuint64_tで指示
		//     コマンド依存のデータ: 無し
		while( true ){
			int kind = fgetc( stdin );
            if( kind == EOF ){
                Sleep( 100 );
                continue;
            }
            int len = fgetc( stdin );
            while( len == EOF ){
                len = fgetc( stdin );
            }
#ifdef TEST
            fprintf( vocaloidrv::flog, "kind=%d; len=%d\n", kind, len );
#endif
            switch( kind ){
				case 0x01:
				case 0x02:{
                    int size = read_uint32_le( stdin );
#ifdef TEST
                    fprintf( vocaloidrv::flog, "before; size=%d\n", size );
                    fflush( vocaloidrv::flog );
#endif
                    load_midi_from_file( stdin, dat, clocks, &clocks_length, &size );
#ifdef TEST
                    fprintf( vocaloidrv::flog, "after; size=%d; clocks_length=%d\n", size, clocks_length );
                    fflush( vocaloidrv::flog );
#endif
                    drv.sendEvent( dat, clocks, size, kind - 1 );
                    break;
                }
                case 0x03:{
                    uint64_t total_samples = read_uint64_le( stdin );
#ifdef TEST
                    fprintf( vocaloidrv::flog, "total_samples=%llu\n", total_samples );
                    fflush( vocaloidrv::flog );
#endif
                    uint64_t ret = drv.startRendering( total_samples, false, sample_rate );
                    fflush( stdout );
#ifdef TEST
                    fprintf( vocaloidrv::flog, "rendering done; ret=%llu\n", ret );
                    fflush( vocaloidrv::flog );
#endif
                    break;
                }
			}
		}
	}else{
		// マスタートラックのMIDIデータを送信
		int num;
		for( int track = 0; track < 2; track++ ){
			FILE *fp = fopen( midi_file[track].c_str(), "rb" );
			if( NULL == fp ){
				continue;
			}
			load_midi_from_file( fp, dat, clocks, &clocks_length, &num );
			fclose( fp );
			drv.sendEvent( dat, clocks, num, track );
		}
		if( clocks ) free( clocks );
		if( dat ) free( dat );

		drv.startRendering( total_samples, false, sample_rate );
	}
    return 0;
}

void print_help()
{
    cout << "vocaloidrv" << endl;
    cout << "Copyright (C) 2011, kbinani" << endl;
    cout << "Usage1:" << endl;
	cout << "    vocaloidrv [vsti path] [midi master] [midi body] [total samples] {output wave}" << endl;
	cout << "Usage2:" << endl;
	cout << "    vocaloidrv [vsti path] -e" << endl;
	cout << "    (input midi data from stdin, output wave to stdout)" << endl;
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
