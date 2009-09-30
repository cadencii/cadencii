/*
 * utauvsti.cpp
 * Copyright (c) 2009 kbinani
 *
 * This file is part of utauvsti
 *
 * utauvsti is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * utauvsti is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#include "utauvsti.h"
#ifdef _BENCHMARK
#include <time.h>
#endif

//-----------------------------------------------------------------------------------------------
// Platform Dependent
#ifdef WIN32
#ifdef __GNUC__
#define WINVER 0x0600   //mingwのg++でGetLongPathNameを有効にするために必要
#endif
#include <windows.h>
#include <winbase.h>
#include <shlwapi.h>
#pragma comment(lib, "shlwapi.lib")

void* hInstance;

extern "C" {
    // SDKのvstplugmain.cppのDllMainを消す必要アリ
    BOOL WINAPI DllMain( HINSTANCE hInst, DWORD dwReason, LPVOID lpvReserved ){
        hInstance = hInst;
        return 1;
    }
} // extern "C"

string get_filename_without_extension( string path ){
    if( path.size() <= 0 ){
        return "";
    }
    string::size_type pos_dot = path.rfind( "." );
    string::size_type pos_bslash = path.rfind( "\\" );
    if( pos_dot < pos_bslash ){
        return path.substr( pos_bslash + 1 );
    }else{
        return path.substr( pos_bslash + 1, pos_dot - pos_bslash - 1 );
    }
}

string get_directory_name( string path ){
    if( path.size() <= 0 ){
        return "";
    }
    string::size_type pos_bslash = path.rfind( "\\" );
    return path.substr( 0, pos_bslash );
}

bool path_file_exists( string path ){
    BOOL ret = PathFileExistsA( path.c_str() );
    if( ret == FALSE ){
        return false;
    }else{
        return true;
    }
}

bool path_file_exists( wstring path ){
    BOOL ret = PathFileExistsW( path.c_str() );
    if( ret == FALSE ){
        return false;
    }else{
        return true;
    }
}

bool path_directory_exists( wstring path ){
    BOOL ret = PathIsDirectoryW( path.c_str() );
    if( ret == FALSE ){
        return false;
    }else{
        return true;
    }
}

void create_directory( wstring path ){
    if( !PathIsDirectoryW( path.c_str() ) ){
        CreateDirectoryW( path.c_str(), NULL );
    }
}

string get_temp_path(){
    char buf[_MAX_PATH] = "";
    GetTempPathA( _MAX_PATH, buf );
#if (_WIN32_WINNT >= 0x0500 || _WIN32_WINDOWS >= 0x0410)
    char buf2[_MAX_PATH] = "";
    GetLongPathNameA( buf, buf2, _MAX_PATH );//win95では使えない
    return string( buf2 );
#else
    return string( buf );
#endif
}

string get_dll_path(){
    char module_name[_MAX_PATH] = "";
    if( 0 != GetModuleFileNameA( (HMODULE)hInstance, module_name, _MAX_PATH ) ){
        return string( module_name );
    }else{
        return "";
    }
}

string path_combine( string path1, string path2 ){
    if( path1.size() <= 0 ){
        return path2;
    }else if ( path2.size() <= 0 ){
        return path1;
    }
    string s1, s2;
    if( path1[path1.size() - 1] == '\\' ){
        s1 = path1.substr( 0, path1.size() - 1 );
    }else{
        s1 = path1;
    }
    if( path2[0] == '\\' ){
        s2 = path2;
    }else{
        s2 = "\\" + path2;
    }
    return s1 + s2;
}

void create_process( string filename, string argument, string working_directory ){
#ifdef _TEST
    g_logger << "create_process" << endl;
    g_logger << "    filename=" << filename << endl;
    g_logger << "    argument=" << argument << endl;
    g_logger << "    working_directory=" << working_directory << endl;
#endif
    PROCESS_INFORMATION pi;
    STARTUPINFOA si;

    memset( &si, 0, sizeof( si ) );
    si.cb = sizeof( si );

    string narg = filename + " " + argument;
    CreateProcessA( NULL, (LPSTR)narg.c_str(), NULL, NULL, FALSE, HIGH_PRIORITY_CLASS | CREATE_NO_WINDOW, NULL, NULL, &si, &pi );
    CloseHandle( pi.hThread );
    WaitForSingleObject( pi.hProcess, INFINITE );
    CloseHandle( pi.hProcess );
}

void remove_file( wstring path ){
    DeleteFileW( path.c_str() );
}

void remove_directory( wstring path ){
    RemoveDirectoryW( path.c_str() );
}
#else
#error Please implement platform dependent functions
#endif

//-----------------------------------------------------------------------------------------------
// Platform Independent
int g_num_programs = 0;
int g_num_params = 0;
string g_resampler;     // resampler.exeのパス
string g_wavtool;       // wavtool.exeのパス
bool g_initialized;     // utauvsti.confを正しく読み込めたかどうか
vector<SingerConfig> g_singers;     // 音源フォルダのリスト
string g_dll_path;      // 自分自身のファイル名
// 一時ファイルを保存するフォルダ
string g_temp_dir;
float utauvsti::k_inv32768 = 1.0f / 32768.0f;
float utauvsti::k_inv64 = 1.0f / 64.0f;
string g_locale;  // プラットフォームの言語
const string LOCK_FILE = ".lock_utauvsti";
const string BASE_RESULT = "result.wav";

AudioEffect* createEffectInstance( audioMasterCallback audioMaster ){
    const int buf_len = 260;
    char buf[buf_len];
#if defined( _TEST ) || defined( _BENCHMARK )
    g_logger.open( "c:\\utauvsti.log", ios::out | ios::app );
    g_logger << "--------------------------------------------------------------------------" << std::endl;
    float *foo = new float[100];
    g_logger << "sizeof(foo)=" << sizeof( foo ) << endl;
    delete [] foo;
#endif

    g_initialized = false;
    g_singers.clear();
    g_dll_path = get_dll_path();
    string path = get_directory_name( g_dll_path );

    string dll = get_filename_without_extension( g_dll_path );
    string config = path_combine( path, dll + ".conf" );

#ifdef _TEST
    g_logger << "path=" << path << endl;
    g_logger << "dll=" << dll << endl;
    g_logger << "config=" << config << endl;
#endif
    g_temp_dir = get_temp_path();
    g_locale = "japanese";

    if( path_file_exists( config ) ){
        ifstream ifs;
        try{
            ifs.open( config.c_str(), ios::in );

            ifs.getline( buf, buf_len );
            stringstream iss( buf );
            int config_file_version;
            iss >> config_file_version;

#ifdef _TEST
            g_logger << "config_file_version=" << config_file_version << endl << flush;
#endif
            ifs.getline( buf, buf_len );
            g_resampler = buf;

            ifs.getline( buf, buf_len );
            g_wavtool = buf;

            if( config_file_version == 2 ){
                ifs.getline( buf, buf_len );
                g_temp_dir = buf;

                ifs.getline( buf, buf_len );
#ifdef _TEST
                g_logger << "g_locale=" << g_locale << endl;
#endif
                g_locale = buf;
            }

            ifs.getline( buf, buf_len );
            iss.clear();
            iss.str( buf );
            iss >> g_num_programs; //音源の個数=MIDI上のプログラムの個数
#ifdef _TEST
            g_logger << "g_num_programs=" << g_num_programs << endl;
#endif
            for( int i = 0; i < g_num_programs; i++ ){
                ifs.getline( buf, buf_len );
                SingerConfig sc;
                sc.Path = string( buf );
                g_singers.push_back( sc );
            }
            g_initialized = true;
        }catch( string ex ){
            g_initialized = false;
#ifdef _TEST
            g_logger << "ex=" << ex << endl;
#endif
        }
        ifs.close();
    }
    if( g_num_programs <= 0 ){
        g_initialized = false;
    }

    string base_temp_dir = g_temp_dir; // ～～Local Settings\Temp
    int count = 0;
    ostringstream oss;
    wstring wlock_file_name;
    string lock_file_name;
    while( true ){
        oss.str( "" );
        oss << "utauvsti" << count;
        g_temp_dir = path_combine( base_temp_dir, oss.str() );
        wstring wg_temp_dir = wstring_from_string( g_temp_dir, g_locale );
        lock_file_name = path_combine( g_temp_dir, LOCK_FILE );
        wlock_file_name = wstring_from_string( lock_file_name, g_locale );
        if( !path_directory_exists( wg_temp_dir ) ){
            create_directory( wg_temp_dir );
            break;
        }else{
            if( !path_file_exists( wlock_file_name ) ){
                break;
            }
            count++;
        }
    }
#ifdef __GNUC__
    ofstream ofs( lock_file_name.c_str(), ios::out );
    ofs.close();
#else
    wofstream wofs( wlock_file_name.c_str(), ios::out );
    wofs.close();
#endif

    // 各音源の"character.txt"から，キャラクタ名を取得．プログラム名=キャラクタ名となる．
    // character.txtが見つからない場合，フォルダ名をプログラム名として使う
    for( int i = 0; i < g_singers.size(); i++ ){
        string character = path_combine( g_singers[i].Path, "character.txt" );
        if( path_file_exists( character ) ){
            ifstream cconfig( character.c_str(), ios::in );
            while( cconfig.peek() >= 0 ){
                cconfig.getline( buf, buf_len );
                string line( buf );
                if( line.find( "name=" ) == 0 ){
                    g_singers[i].Name = line.substr( 5 );
                    break;
                }
            }
        }else{
            g_singers[i].Name = get_filename_without_extension( g_singers[i].Path );
        }
    }
    return new utauvsti( audioMaster );
}

void wstring_from_string_cor( wstring& dst, 
                              const string& src,
                              const std::codecvt<wchar_t, char, mbstate_t>& cvt) {
    const unsigned int buflen = 0x100;
    typedef std::codecvt<wchar_t, char, mbstate_t> cvt_type;
    wstring temp;
    wchar_t buffer[buflen];
    mbstate_t state( 0 );
    cvt_type::result result;
  
    const char* const pbegin = src.c_str();
    const char* const pend = pbegin + src.length();
    const char* pnext = pbegin;
    wchar_t* const pwbegin = buffer;
    wchar_t* const pwend = buffer + buflen;
    wchar_t* pwnext = pwbegin;

    while( true ){
        result = cvt.in( state, pbegin, pend, pnext, pwbegin, pwend, pwnext );
        temp.append( pwbegin, pwnext - pwbegin );
        if( result == cvt_type::ok ){
            break;
        }else if( result == cvt_type::error ){
            break;
        }
    }
    dst.swap( temp );
}

wstring wstring_from_string( string src, string loc ) {
    wstring dst;
    typedef std::codecvt<wchar_t, char, mbstate_t> cvt_type;
    wstring_from_string_cor( dst, src, std::use_facet<cvt_type>( std::locale( loc.c_str() ) ) ); 
    return dst;
}

void string_from_wstring_cor( string& dst, 
                              wstring& src,
                              const std::codecvt<wchar_t, char, mbstate_t>& cvt) {
    const unsigned int buflen = 0x100;
    typedef std::codecvt<wchar_t, char, mbstate_t> cvt_type;

    string temp;
    char buffer[buflen];
    mbstate_t state(0);
    cvt_type::result result;

    const wchar_t* const pwbegin = src.c_str();
    const wchar_t* const pwend = pwbegin + src.length();
    const wchar_t* pwnext = pwbegin;
    char* const pbegin = buffer;
    char* const pend = buffer + buflen;
    char* pnext = pbegin;

    while( true ){
        result = cvt.out( state, pwbegin, pwend, pwnext, pbegin, pend, pnext );
        temp.append( pbegin, pnext - pbegin );
        if( result == cvt_type::ok ){
            break;
        }else if( result == cvt_type::error ){
            break;
        }
    }
  
    dst.swap( temp );
}

string string_from_wstring( wstring s, string loc ){
    typedef std::codecvt<wchar_t, char, mbstate_t> cvt_t;
    string ret( "" );
    string_from_wstring_cor( ret, s, std::use_facet<cvt_t>( std::locale( loc.c_str() ) ) );
    return ret;
}

utauvsti::utauvsti( audioMasterCallback audioMaster ) : AudioEffectX( audioMaster, g_num_programs, g_num_params ){
    m_current_singer = 0;
    m_dict_singer = -1;
    for( int track = 0; track < k_num_track; track++ ){
        m_last_rendered[track] = -1;
        m_dynamics[track].set_default( 64 );
        m_pitchbend[track].set_default( 0 );
        m_pitchbend_sensitivity[track].set_default( 2 );
    }
    string tmpwav = path_combine( g_temp_dir, "result.wav" );
    if( path_file_exists( tmpwav ) ){
        remove( tmpwav.c_str() );
    }
#ifdef WAVTOOL_ON_THE_CODE
    m_buf_length = 0;
    m_buf_left = (float *)0;
    m_buf_right = (float *)0;
    m_buf_start_sample = 0;
#endif

    this->cEffect.flags = effFlagsCanReplacing | effFlagsIsSynth;
#ifdef _TEST
    g_logger << "g_dll_path=" << g_dll_path << endl;
    g_logger << "g_resampler=" << g_resampler << endl;
    g_logger << "g_wavtool=" << g_wavtool << endl;
    g_logger << "g_num_programs=" << g_num_programs << endl;
    for( int i = 0; i < g_singers.size(); i++ ){
        g_logger << "    " << g_singers[i].Path << endl;
    }
    g_logger << "g_temp_dir=" << g_temp_dir << endl;
#endif
#ifdef _BENCHMARK
    /*string lyric = "あ";
    ostringstream oss( "" );
    string filename = "1.wav";
    string target = path_combine( m_temp_dir, filename );
    string note = "C4";
    string millisec = "550";
    OtoArgs oa;
    oss << "\"" << path_combine( g_singers[0].Path, lyric + ".wav" ) << "\" \"" << target + "\" \"" << note << "\" 100 L " << oa.msOffset << " " + millisec << " " << oa.msConsonant << " " << oa.msBlank << " 100 100";
    string arg = oss.str();
    clock_t start = clock();
    const int num = 10;
    for( int i = 0; i < num; i++ ){
        create_process( "\"" + g_resampler + "\"", arg, m_temp_dir );
    }
    clock_t end = clock();
    g_logger << "_BENCHMARK: " << ((double)(end - start) / CLOCKS_PER_SEC / (double)num * 1000.0) << " milliseconds per render 550ms note; resampler is:" << g_resampler << endl << flush;*/
    
    const int num = 50000;
    unsigned char b1 = 0xFF;
    unsigned char b2 = 0xFF;
    unsigned char b3 = 0xFF;
    unsigned char b4 = 0xFF;
    clock_t start = clock();
    double t = 0.0;
    for( int k = 0; k < num; k++ ){
        for( int i = 0; i < num; i++ ){
            int j = b1 << 24 | b2 << 16 | b3 << 8 | b4;
            t += j;
        }
    }
    clock_t end = clock();
    double t2 = 0.0;
    clock_t start2 = clock();
    for( int k = 0; k < num; k++ ){
        for( int i = 0; i < num; i++ ){
            int j = ((b1 << 8 | b2) << 8 | b3) << 8 | b4;
            t2 += j;
        }
    }
    clock_t end2 = clock();
    g_logger << "_BENCHMARK: t=" << t << endl << flush;
    g_logger << "_BENCHMARK: t2=" << t2 << endl << flush;
    g_logger << "_BENCHMARK: elapsed time=" << (end - start) << endl << flush;
    g_logger << "_BENCHMARK: elapsed time2=" << (end2 - start2) << endl << flush;
#endif
}

utauvsti::~utauvsti(){
    for( int i = 0; i < k_num_track; i++ ){
        m_events[i].clear();
    }
    string lock_file_name = path_combine( g_temp_dir, LOCK_FILE );
    wstring wlock_file_name = wstring_from_string( lock_file_name, g_locale );
#ifndef _TEST
    for( int track = 0; track < k_num_track; track++ ){
        ostringstream oss( "" );
        oss << track << "_" << BASE_RESULT;
        string whd = path_combine( g_temp_dir, oss.str() + ".whd" );
        remove_file( wstring_from_string( whd, g_locale ) );
        string dat = path_combine( g_temp_dir, oss.str() + ".dat" );
        remove_file( wstring_from_string( dat, g_locale ) );
    }
#endif
    if( path_file_exists( wlock_file_name ) ){
        remove_file( wlock_file_name );
    }
    remove_directory( wstring_from_string( g_temp_dir, g_locale ) );
    //if( path_file_exists( 
#ifdef _TEST
    g_logger << "~utauvsti" << flush;
    g_logger.close();
#endif
}

VstIntPtr utauvsti::dispatcher( VstInt32 opcode, VstInt32 index, VstIntPtr value, void* ptr, float opt ){
    VstIntPtr ret = 0;
    int count = 0;
    switch( opcode ){
        case effSetSampleRate:
            m_sample_rate = opt;
#ifdef _TEST
            g_logger << "m_sample_rate=" << m_sample_rate << endl << flush;
#endif
            break;
        case effMainsChanged:
#ifdef _TEST
            /*g_logger << "m_pitchbend" << endl;
            for( int i = 0; i < m_pitchbend[track].m_list.size(); i++ ){
                g_logger << "    " << m_pitchbend[track].m_list[i].msTime << "\t" << m_pitchbend[track].m_list[i].Value << endl;
            }
            g_logger << "m_pitchbend_sensitivity" << endl;
            for( int i = 0; i < m_pitchbend_sensitivity.m_list.size(); i++ ){
                g_logger << "    " << m_pitchbend_sensitivity.m_list[i].msTime << "\t" << m_pitchbend_sensitivity.m_list[i].Value << endl;
            }*/
#endif
            for( int i = 0; i < k_num_track; i++ ){
                m_events[i].clear();
                m_dynamics[i].clear();
                m_pitchbend[i].clear();
                m_pitchbend_sensitivity[i].clear();
                m_last_rendered[i] = -1;
                m_current_program[i] = 0;
            }
            m_processed_sample = 0;
            count = 0;
            while( true ){
                stringstream ss( "" );
                ss << count << ".wav";
                string file = path_combine( g_temp_dir, ss.str() );
                if( path_file_exists( file ) ){
                    remove( file.c_str() );
                }else{
                    break;
                }
                count++;
            }
#ifdef WAVTOOL_ON_THE_CODE
            if( m_buf_left ){
                free( m_buf_left );
            }
            if( m_buf_right ){
                free( m_buf_right );
            }
            m_buf_length = 0;
            m_buf_start_sample = 0;
#endif
            break;
        case effSetBlockSize:
            m_block_size = value;
            break;
        case effCanDo:
            if( string( (char *)ptr ) == "receiveVstEvents" || string( (char *)ptr ) == "receiveVstMidiEvent" ){
                ret = 1;
            }else{
                ret = 0;
            }
            break;
        case effGetVendorString:
            return getVendorString( (char *)ptr );
            break;
        case effGetVendorVersion:
            return getVendorVersion();
            break;
        case effGetProductString:
            return getProductString( (char *)ptr );
            break;
        case effGetProgramNameIndexed:
            if( 0 <= index && index < g_singers.size() ){
                vst_strncpy( (char *)ptr, g_singers[index].Name.c_str(), kVstMaxProgNameLen );
                return true;
            }else{
                vst_strncpy( (char *)ptr, "", kVstMaxProgNameLen );
                return false;
            }
        case effProcessEvents:
            VstEvents *incoming = (VstEvents*)ptr;
            vector<VsqNrpn> nrpns[k_num_track];
            bool first = true;
            unsigned char addr_msb = 0x0;
            unsigned char addr_lsb = 0x0;
            unsigned char data_msb = 0x0;
            unsigned char data_lsb = 0x0;

            int base_sample = m_processed_sample;
            int last_lsb_sample = base_sample; //最後のnrpn lsbの発生時刻を表すサンプル数

            // VstEventsに入っているデータをVsqNrpnに変換
            bool lsb_received = true;
            for( int i = 0; i < incoming->numEvents; i++ ){
                if( incoming->events[i]->type != kVstMidiType ){
                    continue;
                }
                VstMidiEvent *vme = (VstMidiEvent*)incoming->events[i];
                unsigned int pn = 0x0;
                int ctrl = vme->midiData[0] & 0xff;
#ifdef _TEST
                //g_logger << "ctrl=0x" << hex << ctrl << dec << endl;
                //g_logger << "(ctrl & 0xf0)=0x" << hex << (ctrl & 0xf0) << dec << endl; 
#endif
                int track = (int)((ctrl & 0xf0) - 0xb0);
                if( (ctrl & 0xf0) == 0xb0 ){
#ifdef _TEST
                    //g_logger << "track=" << track << endl << flush;
#endif
                    // NRPNの場合
                    switch( vme->midiData[1] ){
                        case 0x63:
                            if( !lsb_received ){
                                nrpns[track].push_back( VsqNrpn( last_lsb_sample, pn, data_msb ) );
                                lsb_received = true;
                            }
                            addr_msb = vme->midiData[2];
                            addr_lsb = 0x0;
                            break;
                        case 0x62:
                            if( !lsb_received ){
                                nrpns[track].push_back( VsqNrpn( last_lsb_sample, pn, data_msb ) );
                                lsb_received = true;
                            }
                            addr_lsb = vme->midiData[2];
                            last_lsb_sample = base_sample + vme->deltaFrames;
                            break;
                        case 0x06:
                            data_msb = vme->midiData[2];
                            pn = addr_msb << 8 | addr_lsb;
                            if( !is_require_data_lsb( pn ) ){
                                // nrpnがdata lsbを使用しない場合，datalsbの出現を待たずにイベントを登録
                                nrpns[track].push_back( VsqNrpn( last_lsb_sample, pn, data_msb ) );
                            }
                            if( CVM_NM_PHONETIC_SYMBOL1 <= pn && pn <= CVM_NM_PHONETIC_SYMBOL60 ){
                                // lsbがあるかどうかわからないNRPN
                                lsb_received = false;
                            }
                            break;
                        case 0x26:
                            data_lsb = vme->midiData[2];
                            pn = addr_msb << 8 | addr_lsb;
                            nrpns[track].push_back( VsqNrpn( last_lsb_sample, pn, data_msb, data_lsb ) );
                            lsb_received = true;
                            break;
                    }
                }
            }

            // 歌詞情報を再構成
            for( int track = 0; track < k_num_track; track++ ){
                int note_number = 60;
                int ms_delay = 0;
                int ms_duration = 0;
                int velocity = 64;
                unsigned char val1msb, val1lsb, val2msb, val2lsb, val3msb;
                int envelope_count = 0;
                UtauEvent work; //発音記号を集計するための作業用
                int note_message_version = 0; //UTAUの歌詞がそのまま送られてくるモードは1，VOCALOID2 NRPNの場合は0
                bool overlap_specified = false;
                bool preutterance_specified = false;
                int flags_count = 0;
                for( int i = 0; i < nrpns[track].size(); i++ ){
#ifdef _TEST
                    //g_logger << "nrpns[i].Nrpn=" << hex << nrpns[i].Nrpn << dec << endl << flush;
#endif
                    if( CVM_NM_VERSION_AND_DEVICE == nrpns[track][i].Nrpn ){
                        note_message_version = nrpns[track][i].DataMsb;

                    }else if( CVM_NM_NOTE_NUMBER == nrpns[track][i].Nrpn ){
                        // ノートナンバー
                        note_number = (int)nrpns[track][i].DataMsb;

                    }else if( CVM_NM_PHONETIC_SYMBOL1 <= nrpns[track][i].Nrpn && nrpns[track][i].Nrpn < CVM_NM_PHONETIC_SYMBOL_CONTINUATION ) {
                        // 発音記号
                        int index = (int)nrpns[track][i].Nrpn - (int)CVM_NM_PHONETIC_SYMBOL1;
                        work.set_lyric( index, (char)nrpns[track][i].DataMsb );

                    }else if( CVM_NM_NOTE_DURATION == nrpns[track][i].Nrpn ){
                        // ノートの持続時間
                        ms_duration = nrpns[track][i].gvalue();

                    }else if( CVM_NM_DELAY == nrpns[track][i].Nrpn ){
                        // saTimeから，実際に発音されるまでの時間
                        ms_delay = nrpns[track][i].gvalue();

                    }else if( CVM_NM_NOTE_MESSAGE_CONTINUATION == nrpns[track][i].Nrpn && 0x7f == nrpns[track][i].DataMsb ){
                        // ノートメッセージの終了フラグ
                        UtauEvent ue;
                        ue.msTime = (int)ms_from_sa( nrpns[track][i].saTime ) + ms_delay;
                        ue.msLength = ms_duration;
                        ue.Note = note_number;
                        ue.Velocity = velocity;
                        int len = m_events[track].size();
                        int ms_end = 0;
                        if( len > 0 ){
                            ms_end = m_events[track][len - 1].msTime + m_events[track][len - 1].msLength;
                        }
                        if( ue.msTime > ms_end + 1 ){
                            // 休符の挿入
                            UtauEvent ue2;
                            ue2.set_lyric( "R" );
                            ue2.File = "R";
                            ue2.Note = 60;
                            ue2.msLength = ue.msTime - ms_end;
                            ue2.msTime = ms_end;
                            ue2.Program = m_current_program[track];
                            ue2.Velocity = 0; //休符のベロシティは0
                            ue2.mode_r = true;
#ifdef _TEST
                            g_logger << "ue2.msTime=" << ue2.msTime << "; ue2.msLength=" << ue2.msLength << endl;
#endif
                            m_events[track].push_back( ue2 );
                        }
                        
                        string lyric = work.get_lyric();
                        load_singer_config();
                        //bool mode_r = false;
                        if( lyric == "R" ){
                            //mode_r = true;
                            ue.File = "R";
                            ue.mode_r = true;
                        }else{
                            // 休符で無い場合
                            if( note_message_version == 0 ){
                                // VOCALOID2 NRPNが送られてきた場合
                                // 発音記号からひらがなの歌詞を逆算する
                                lyric = symboltable_attatch( lyric );
                                lyric = apply_prefix_map( lyric, note_number );
                            }else if( note_message_version == 1 ){
                                // UTAU対応用のNRPNが送られてきた場合
                                if( lyric.find( "?" ) == 0 ){
                                    lyric = lyric.substr( 1 );
                                }else{
                                    // prefix.mapの設定を反映
                                    lyric = apply_prefix_map( lyric, note_number );
                                }
                            }
                            ue.File = lyric;
                            ue.mode_r = false;
                        }
                        if( !ue.mode_r ){
                            OtoArgs oa = m_singer_config[lyric + ".wav"];
                            OtoArgs add = oa;
                            if( overlap_specified ){
                                add.msOverlap = work.Config.msOverlap;
                            }
                            if( preutterance_specified ){
                                add.msPreUtterance = work.Config.msPreUtterance;
                            }
                            ue.Config = add;
                        }

                        ue.Moduration = work.Moduration;
                        ue.set_flags( work.get_flags() );

                        ue.set_lyric( lyric );
                        ue.Program = m_current_program[track];
                        for( int i = 0; i < ue.MAX_ENV_POINTS; i++ ){
                            ue.set_envelope( i, work.get_envelope( i ) );
                        }
                        envelope_count = 0;
                        work.set_lyric( "" );
                        m_events[track].push_back( ue );
                        overlap_specified = false;
                        preutterance_specified = false;
                        flags_count = 0;

                    }else if( PB_DELAY == nrpns[track][i].Nrpn ){
                        // Pitch Bendのディレイ
                        m_pitchbend[track].msDelay = nrpns[track][i].gvalue();

                    }else if( PB_PITCH_BEND == nrpns[track][i].Nrpn ){
                        // Pitch Bendの値
                        int pb = nrpns[track][i].gvalue() - 0x2000;
                        m_pitchbend[track].push_back( ms_from_sa( nrpns[track][i].saTime ) + m_pitchbend[track].msDelay, pb );
#ifdef _TEST
                        g_logger << "Pitch Bend=" << pb << endl << flush;
#endif

                    }else if( CC_PBS_DELAY == nrpns[track][i].Nrpn ){
                        // Pitch Bend Sensitivityのディレイ
                        m_pitchbend_sensitivity[track].msDelay = nrpns[track][i].gvalue();

                    }else if( CC_PBS_PITCH_BEND_SENSITIVITY == nrpns[track][i].Nrpn ){
                        // Pitch Bend Sensitivityの値
                        m_pitchbend_sensitivity[track].push_back( ms_from_sa( nrpns[track][i].saTime ) + m_pitchbend_sensitivity[track].msDelay, (int)nrpns[track][i].DataMsb ); //datalsbは常に不使用
#ifdef _TEST
                        g_logger << "PBS=" << (int)nrpns[track][i].DataMsb << endl << flush;
#endif

                    }else if( CC_E_DELAY == nrpns[track][i].Nrpn ){
                        // Expression（Dynamics）のディレイ
                        m_dynamics[track].msDelay = nrpns[track][i].gvalue();

                    }else if( CC_E_EXPRESSION == nrpns[track][i].Nrpn ){
                        // Expressionの値
                        m_dynamics[track].push_back( ms_from_sa( nrpns[track][i].saTime ) + m_dynamics[track].msDelay, nrpns[track][i].DataMsb );

                    }else if( PC_VOICE_TYPE == nrpns[track][i].Nrpn ){
                        // program change
                        m_current_program[track] = nrpns[track][i].DataMsb;

                    }else if( CVM_NM_VELOCITY == nrpns[track][i].Nrpn ){
                        // velocity
                        int draft_vel = nrpns[track][i].DataMsb;
                        velocity = (int)(draft_vel / 127.0f * 200.0f);

                    }else if( CVM_EXNM_ENV_DATA1 == nrpns[track][i].Nrpn ){
                        val1msb = nrpns[track][i].DataMsb;
                        val1lsb = nrpns[track][i].DataLsb;

                    }else if( CVM_EXNM_ENV_DATA2 == nrpns[track][i].Nrpn ){
                        val2msb = nrpns[track][i].DataMsb;
                        val2lsb = nrpns[track][i].DataLsb;

                    }else if( CVM_EXNM_ENV_DATA3 == nrpns[track][i].Nrpn ){
                        val3msb = nrpns[track][i].DataMsb;

                    }else if( CVM_EXNM_ENV_DATA_CONTINUATION ==nrpns[track][i].Nrpn ){
                        int value = ((((val3msb & 0xf) << 7 | (val2msb & 0x7f)) << 7 | (val2lsb & 0x7f)) << 7 | (val1msb & 0x7f)) << 7 | (val1lsb & 0x7f);
                        work.set_envelope( envelope_count, value );
                        envelope_count++;

                    }else if( CVM_EXNM_VOICE_OVERLAP == nrpns[track][i].Nrpn  ){
                        work.Config.msOverlap = ((nrpns[track][i].DataMsb & 0x7f) << 7) | (nrpns[track][i].DataLsb & 0x7f) - 8192;
                        overlap_specified = true;

                    }else if( CVM_EXNM_MODURATION == nrpns[track][i].Nrpn ){
                        work.Moduration = ((nrpns[track][i].DataMsb & 0x7f) << 7) | (nrpns[track][i].DataLsb & 0x7f) - 100;

                    }else if( CVM_EXNM_FLAGS == nrpns[track][i].Nrpn ){
                        work.set_flags( flags_count, (char)nrpns[track][i].DataMsb );
                        flags_count++;

                    }else if( CVM_EXNM_PRE_UTTERANCE == nrpns[track][i].Nrpn ){
                        work.Config.msPreUtterance = ((nrpns[track][i].DataMsb & 0x7f) << 7) | (nrpns[track][i].DataLsb & 0x7f) - 8192;
                        preutterance_specified = true;

                    }
                }
            }

            break;
    }
    return ret;
}

void utauvsti::processReplacing( float** inputs, float** outputs, VstInt32 sampleFrames ){
	try{
#ifdef _TEST
		g_logger << "utauvsti::processReplacing" << endl << flush;
#endif
		const string filebase = "result.wav";
		const double sec_per_clock = k_tempo * 1e-6 / 480.0;
		const string dumy_tempo = "120.00";
		ostringstream oss( "" );

		for( int i = 0; i < sampleFrames; i++ ){
			outputs[0][i] = 0.0f;
			outputs[1][i] = 0.0f;
		}
		if( !g_initialized ){
			// 初期化が正しく行われていなかった場合．無音にする
			m_processed_sample += sampleFrames;
			return;
		}

		int sa_from = m_processed_sample + 1;
		int sa_to   = m_processed_sample + sampleFrames;
		for( int track = 0; track < k_num_track; track++ ){
			oss.str( "" );
			oss << track << "_" << filebase << ".whd";
			string path_whd = path_combine( g_temp_dir, oss.str() );
			oss.str( "" );
			oss << track << "_" << filebase << ".dat";
			string path_dat = path_combine( g_temp_dir, oss.str() );
			oss.str( "" );
			oss << track << "_" << filebase;
			string path_result = path_combine( g_temp_dir, oss.str() );
			if( m_processed_sample == 0 ){
				if( path_file_exists( path_whd ) ){
					remove( path_whd.c_str() );
				}
				if( path_file_exists( path_dat ) ){
					remove( path_dat.c_str() );
				}
			}
			int sa_rendered_last = 0; // どこまでレンダリングが済んでいるか。
			if( 0 <= m_last_rendered[track] && m_last_rendered[track] < m_events[track].size() ){
                int index = m_last_rendered[track];
                int ms_rendered_last = m_events[track][index].msActualTime + m_events[track][index].msActualLength - m_events[track][index].Config.msOverlap;
				sa_rendered_last = sa_from_ms( ms_rendered_last );
			}
#ifdef _TEST
	        g_logger << "track=" << track << "; m_last_rendered[track]=" << m_last_rendered[track] << "; sa_rendered_last=" << sa_rendered_last << endl << flush;
#endif
			if( sa_to > sa_rendered_last ){
				//要求された部分のレンダリングが行われていない場合
				while( sa_to > sa_rendered_last && m_last_rendered[track] + 1 < m_events[track].size() ){
					m_last_rendered[track] = m_last_rendered[track] + 1;
					int last = m_last_rendered[track];
					// このループ内では，m_events[m_last_rendered[track]]がレンダリングされる
					m_current_singer = m_events[track][last].Program;
					if( m_current_singer < 0 || g_singers.size() <= m_current_singer ){
						// 範囲外の場合、とりあえず0にして歌わせる
						m_current_singer = 0;
					}
					string singer = g_singers[m_current_singer].Path;

					// 必要があれば，原音設定を読み直す
					if( m_current_singer != m_dict_singer ){
						load_singer_config();
					}

					// 次の音符の先行発声とオーバーラップを取得
					OtoArgs oa_next;
					if ( last + 1 < m_events[track].size() ) {
						oa_next = m_events[track][last + 1].Config;
					}
					OtoArgs oa = m_events[track][last].Config;
#ifdef _TEST
					g_logger << "file=" << m_events[track][last].File << ".wav; offset=" << oa.msOffset << ";consonant=" << oa.msConsonant << ";blank=" << oa.msBlank << ";ahead=" << oa.msPreUtterance << ";overwrap=" << oa.msOverlap << endl;
#endif

					// resampler呼び出し
					string note = note_string_from_note_number( m_events[track][last].Note );
					//bool mode_r = false;
					oss.str( "" );
					oss << track << "_" << last << ".wav";
					string filename = oss.str();
					int mten = oa.msPreUtterance + oa_next.msOverlap - oa_next.msPreUtterance; //先行発声，オーバーラップによって，本来の音符の長さから変化する量
					int actual_msec = m_events[track][last].msLength + mten;
					m_events[track][last].msActualLength = actual_msec;
					m_events[track][last].Result = path_combine( g_temp_dir, filename );
					m_events[track][last].msActualTime = m_events[track][last].msTime - oa.msPreUtterance;
					oss.str( "" );
					oss << actual_msec + 50; //ここの50は一定？
					string millisec = oss.str();
					oss.str( "" );
					string flags = m_events[track][last].get_flags() + "L";
					int time_percent = 100;
					int velocity = 100;
					int moduration = m_events[track][last].Moduration;
					//                                                                                                                                 C4               100                    L               0                     550                0                        0                    100                100
					oss << "\"" << path_combine( singer, m_events[track][last].File + ".wav" ) << "\" \"" << m_events[track][last].Result + "\" \"" << note << "\" " << time_percent << " " << flags << " " << oa.msOffset << " " << millisec << " " << oa.msConsonant << " " << oa.msBlank << " " << velocity << " " << moduration;

                    // ピッチを取得
                    if( !m_events[track][last].mode_r ){
					    const int delta_clock = 5;
					    int tempo = 120;
                        double delta_msec = delta_clock / (8.0 * tempo) * 1000.0; //ピッチを取得する時間間隔
                        double pit_start = m_events[track][last].msActualTime;
                        int pit_count = (int)(m_events[track][last].msActualLength / delta_msec) + 1;
                        ostringstream pitch( "" );
                        bool allzero = true;
#ifdef _TEST
                        for( int i = 0; i < m_pitchbend[track].m_list.size(); i++ ){
                            //g_logger << "msTime=" << m_pitchbend[track].m_list[i].msTime << "; Value=" << m_pitchbend[track].m_list[i].Value << endl;
                        }
#endif
					    for( int i = 0; i < pit_count; i++ ){
						    double gtime = pit_start + delta_msec * i;
						    int pit = m_pitchbend[track].get_value( gtime );
						    int pbs = m_pitchbend_sensitivity[track].get_value( gtime );
						    int pvalue = pit_from_nrpnpit( pit, pbs );
#ifdef _TEST
    	                    //g_logger << gtime << "\t" << pit << "\t" << pbs << "\t" << pvalue << endl;
#endif
						    if( pvalue != 0 ){
							    allzero = false;
						    }
						    pitch << " " << pvalue;
						    if( i == 0 ){
							    pitch << "Q" << tempo;
						    }
					    }
					    if( !allzero ){
						    oss << pitch.str();
					    }
                    }
					string arg = oss.str();
					if( m_events[track][last].mode_r ){
						m_events[track][last].Result = "";
					}
#ifdef _TEST
	                g_logger << "resampler arg=" << arg << endl;
#endif
					create_process( "\"" + g_resampler + "\"", arg, g_temp_dir );

					// resampler結果を元に結合
#ifdef WAVTOOL_ON_THE_CODE
					int sa_mix_start = m_events[m_last_rendered].saActualTime - m_buf_start_sample;
					int sa_mix_end = m_events[m_last_rendered].saActualTime + m_events[m_last_rendered].saActualLength - m_buf_start_sample;
					if( sa_mix_end > m_buf_length ){
#ifdef _TEST
	                    g_logger << "realloc; sa_mix_start=" << sa_mix_start << "; sa_mix_end=" << sa_mix_end << "..." << flush;
#endif
						m_buf_left = (float*)realloc( m_buf_left, sa_mix_end * sizeof( float ) );
						m_buf_right = (float*)realloc( m_buf_right, sa_mix_end * sizeof( float ) );
#ifdef _TEST
	                    g_logger << "done" << endl << flush;
#endif
						for( int i = m_buf_length; i < sa_mix_end; i++ ){
							m_buf_left[i] = 0.0f;
							m_buf_right[i] = 0.0f;
						}
						m_buf_length = sa_mix_end;
					}
					wavereader reader;
					reader.open( m_events[m_last_rendered].Result.c_str() );
					const int buflen = 512;
					float buf[buflen];
					int sa_remain = sa_mix_end - sa_mix_start;
					int count = 0;
#ifdef _TEST
	                g_logger << "sa_remain=" << sa_remain << endl << flush;
#endif
	                while( sa_remain > 0 ){
#ifdef _TEST
						g_logger << "sa_remain=" << sa_remain << "; count=" << count << endl << flush;
#endif
						int start = count * buflen;
						reader.read( start, buflen, buf );
						int copy_count = (sa_remain > buflen) ? buflen : sa_remain;
						for( int i = 0; i < copy_count; i++ ){
							// TODO: Dynamicsの反映
							m_buf_left[sa_mix_start + start + i] += buf[i];
							m_buf_right[sa_mix_start + start + i] += buf[i];
						}
						sa_remain -= copy_count;
						count++;
					}
					reader.close();
#else
					// wavtool呼び出し
					// クロックには整数しか入れられない。既にレンダリング済みの秒時との誤差を補正する。
					// NRPNではテンポ情報を考慮せずすべてサンプル数で時間を取り扱う．したがって，
					// ダミーのテンポ値とダミーのクロック数を使用する必要がある．ここではテンポ=120としている
					int prev_clocks = 0;
					if( last == 0 ){
						m_events[track][last].clEndTime = 0;
						prev_clocks = 0;
					}else{
						prev_clocks = m_events[track][last - 1].clEndTime;
					}
					double prev_seconds = prev_clocks * sec_per_clock;
					int ms_length = (m_events[track][last].msTime + m_events[track][last].msLength) - prev_seconds * 1000;
					//int sample_length = m_events[track][last].saLength;
					int dumy_clocks = (int)(ms_length / 1000.0 / sec_per_clock);
					m_events[track][last].clEndTime = prev_clocks + dumy_clocks;
					oss.str( "" );
					oss << "\"" << path_result << "\" ";
					if( m_events[track][last].mode_r ){
						oss << "\"" << path_combine( singer, "R.wav" ) << "\"";
					}else{
						oss << "\"" << path_combine( g_temp_dir, filename ) << "\"";
					}
					oss << " 0 " << dumy_clocks << "@" << dumy_tempo << ((mten >= 0) ? "+" : "-") << abs( mten );
					if( m_events[track][last].mode_r ){
						oss << " 0 0";
					}else{
						//p1 p2 p3 v1  v2  v3  v4  ovr p4 p5 v5
						//0  5  35 0   100 100 0   *   0  0  100
						//フラットにするなら
						//p1 p2 p3 v1  v2  v3  v4 ovr p4 p5 v5
						//0  5  35 100 100 100 100 *   0  0  100";
						int p1 = m_events[track][last].envP1;
						int p2 = m_events[track][last].envP2;
						int p3 = m_events[track][last].envP3;
						int v1 = m_events[track][last].envV1;
						int v2 = m_events[track][last].envV2;
						int v3 = m_events[track][last].envV3;
						int v4 = m_events[track][last].envV4;
						int p4 = m_events[track][last].envP4;
						int p5 = m_events[track][last].envP5;
						int v5 = m_events[track][last].envV5;
						//480@120.00+0 0 5 35 100 100 100 100 0 0 0 100 
						oss << " " << p1 << " " << p2 << " " << p3 << " " << v1 << " " << v2 << " " << v3 << " " << v4;
						oss << " " << oa.msOverlap << " " << p4 << " " << p5 << " " << v5;
					}
					arg = oss.str();
#ifdef _TEST
	                g_logger << "wavtool arg=" << arg << endl;
#endif
	                create_process( "\"" + g_wavtool + "\"", arg, g_temp_dir );
#ifndef _TEST
					if( !m_events[track][last].mode_r ){
						remove_file( wstring_from_string( path_combine( g_temp_dir, filename ), g_locale ) );
					}
#endif
#endif
					sa_rendered_last = sa_from_ms( m_events[track][last].msActualTime + m_events[track][last].msActualLength - oa_next.msOverlap );
				}
			}

			int pos = 0; //outputsへの書き込みインデックスカウンタ
#ifdef WAVTOOL_ON_THE_CODE
			int copy_start = sa_from - m_buf_start_sample;
			int copy_end   = sa_to - m_buf_start_sample;
#ifdef _TEST
	        g_logger << "memcpy: copy_start=" << copy_start << "; copy_end=" << copy_end << endl << flush;
			g_logger << "m_buf_length=" << m_buf_length << endl << flush;
#endif
			if( m_buf_length > 0 && 0 <= copy_start && m_buf_length > copy_start ){
				int copy_length = (copy_end < m_buf_length) ? sampleFrames : m_buf_length - copy_start;
#ifdef _TEST
	            g_logger << "copy_length=" << copy_length << endl << flush;
#endif
				memcpy( &outputs[0][0], &m_buf_left[copy_start], copy_length * sizeof( float ) );
				memcpy( &outputs[1][0], &m_buf_right[copy_start], copy_length * sizeof( float ) );
				int remain = m_buf_length - copy_end;
#ifdef _TEST
	            g_logger << "remain=" << remain << endl << flush;
#endif
				if( remain <= 0 ){
					free( m_buf_left );
					free( m_buf_right );
					m_buf_length = 0;
				}else{
					memmove( &m_buf_left[0], &m_buf_left[copy_end + 1], remain * sizeof( float ) );
					m_buf_left = (float *)realloc( m_buf_left, remain * sizeof( float ) );
					memmove( &m_buf_right[0], &m_buf_right[copy_end + 1], remain * sizeof( float ) );
					m_buf_right = (float *)realloc( m_buf_right, remain * sizeof( float ) );
					m_buf_length = remain;
				}
				m_buf_start_sample = sa_from + 1;
				pos = sampleFrames;
			}
#ifdef _TEST
			g_logger << "m_buf_length=" << m_buf_length << endl << flush;
			g_logger << "m_buf_start_sample=" << m_buf_start_sample << endl << flush;
#endif

#else

#ifdef __GNUC__
			ifstream whd;
			ifstream dat;
			string file;
#else
			wifstream whd; // result.wav.whd用のファイルストリーム
			wifstream dat; // result.wav.dat用のファイルストリーム
			wstring file;
#endif

#ifdef _TEST
#ifdef __GNUC__
	        ofstream wofs( "C:\\wfile.txt", ios::out | ios::app );
#else
	        wofstream wofs( L"C:\\wfile.txt", ios::out | ios::app );
#endif
#endif
			const int buflen = 1024;
#ifdef __GNUC__
			char wavbuf[buflen];
#else
			wchar_t wavbuf[buflen];
#endif

			// result.wav.whdからサンプリングレートを取得
			wchar_t *array_wfile = new wchar_t[260];
			if( !path_file_exists( path_whd ) ){
				goto fillzero;
			}
#ifdef __GNUC__
	        file = path_whd;
#else
		    file = wstring_from_string( path_whd, g_locale );
#endif
#ifdef _TEST
			g_logger << "result.wav.whd=" << path_whd << endl;
			g_logger << "exists=" << (path_file_exists( path_whd ) ? "True" : "False") << endl;
			wofs << "wfile=" << file << endl;
			wofs.close();
#endif
			whd.open( file.c_str(), ios::binary );
			whd.seekg( 0 );
			// RIFF
#ifdef __GNUC__
	        char buf[4];
#else
	        wchar_t buf[4];
#endif
			whd.read( buf, 4 );
			if( buf[0] != 'R' || buf[1] != 'I' || buf[2] != 'F' || buf[3] != 'F' ){
				int gcount = whd.gcount();
				whd.close();
#ifdef _TEST
#ifdef __GNUC__
	            g_logger << "header error:" << string( buf ) << " must be RIFF" << endl;
#else
				g_logger << "header error:" << string_from_wstring( wstring( buf ), g_locale ) << " must be RIFF" << endl;
#endif
	            g_logger << "gcount=" << gcount << endl;
#endif
				goto fillzero;
			}
			// ファイルサイズ
			whd.read( buf, 4 );
			// WAVE
			whd.read( buf, 4 );
			if( buf[0] != 'W' || buf[1] != 'A' || buf[2] != 'V' || buf[3] != 'E' ){
				whd.close();
#ifdef _TEST
#ifdef __GNUC__
	            g_logger << "header error:" << string( buf ) << " must be WAVE" << endl;
#else
				g_logger << "header error:" << string_from_wstring( wstring( buf ), g_locale ) << " must be WAVE" << endl;
#endif
#endif
	            goto fillzero;
			}
			// fmt 
			whd.read( buf, 4 );
			if( buf[0] != 'f' || buf[1] != 'm' || buf[2] != 't' || buf[3] != ' ' ){
				whd.close();
#ifdef _TEST
#ifdef __GNUC__
	            g_logger << "header error:" << string( buf ) << " must be fmt " << endl;
#else
		        g_logger << "header error:" << string_from_wstring( wstring( buf ), g_locale ) << " must be fmt " << endl;
#endif
#endif
				goto fillzero;
			}
			// fmt チャンクのサイズ
			whd.read( buf, 4 );
			int loc_end_of_fmt = whd.tellg(); //fmtチャンクの終了位置．ここは一定値でない可能性があるので読込み
			loc_end_of_fmt += buf[0] | buf[1] << 8 | buf[2] << 16 | buf[3] << 24;
			// format ID
			whd.read( buf, 2 );
			int id = buf[0] | buf[1] << 8;
			if( id != 0x0001 ){ //0x0001はリニアPCM
				whd.close();
#ifdef _TEST
	            g_logger << "header error: format id does not specify linear PCM; formatID=0x" << hex << id << dec << endl;
#endif
				goto fillzero;
			}
			// チャンネル数
			whd.read( buf, 2 );
			int channel = buf[1] << 8 | buf[0];
			// サンプリングレート
			whd.read( buf, 4 );
			int this_sample_rate = buf[0] | buf[1] << 8 | buf[2] << 16 | buf[3] << 24;
			// データ速度
			whd.read( buf, 4 );
			// ブロックサイズ
			whd.read( buf, 2 );
			// 1チャンネル、1サンプルあたりのビット数
			whd.read( buf, 2 );
			int bit_per_sample = buf[1] << 8 | buf[0];
			int byte_per_sample = bit_per_sample / 8;
			whd.seekg( loc_end_of_fmt, ios::beg );
			// data
			whd.read( buf, 4 );
			if( buf[0] != 'd' || buf[1] != 'a' || buf[2] != 't' || buf[3] != 'a' ){
				whd.close();
#ifdef _TEST
#ifdef __GNUC__
	            g_logger << "header error:" << string( buf ) << " must be data" << endl;
#else
		        g_logger << "header error:" << string_from_wstring( wstring( buf ), g_locale ) << " must be data" << endl;
#endif
#endif
				goto fillzero;
			}
			// size of data chunk
			whd.read( buf, 4 );
			int size = buf[3] << 24 | buf[2] << 16 | buf[1] << 8 | buf[0];
			int total_samples = size / (channel * byte_per_sample);
			whd.close();

#ifdef _TEST
			g_logger << "channel=" << channel << endl;
			g_logger << "byte_per_sample=" << byte_per_sample << endl;
			g_logger << "sa_from=" << sa_from << endl;
			g_logger << "sa_to=" << sa_to << endl;
#endif

	        // datから読み込み
#ifdef __GNUC__
			file = path_dat;
#else
			file = wstring_from_string( path_dat, g_locale );
#endif
			dat.open( file.c_str(), ios::binary );
			dat.seekg( sa_from * channel * byte_per_sample, ios::beg );
			double ms_start = ms_from_sa( sa_from );
			double ms_per_sa = 1000.0 / m_sample_rate;
			if( byte_per_sample == 1 ){
				if( channel == 1 ){
					while( pos < sampleFrames ){
						dat.read( wavbuf, buflen );
						int len = dat.gcount();
						if( len <= 0 ){
                            dat.close();
#ifdef _TEST
                            g_logger << "BAIL-OUT" << endl;
#endif
							goto fillzero;
						}
						int count = 0;
						while( len > 0 && pos < sampleFrames ){
							double gtime_dyn = ms_start + pos * ms_per_sa;
							int dyn = m_dynamics[track].get_value( gtime_dyn );
							float amp = (float)dyn * k_inv64;
							float v = (wavbuf[count++] - 64.0f) * k_inv64 * amp;
							outputs[0][pos] += v;
							outputs[1][pos] += v;
							len -= 1;
							pos++;
						}
					}
				}else{
					while( pos < sampleFrames ){
						dat.read( wavbuf, buflen );
						int len = dat.gcount();
						if( len <= 0 ){
                            dat.close();
#ifdef _TEST
                            g_logger << "BAIL-OUT" << endl;
#endif
							goto fillzero;
						}
						int count = 0;
						while( len > 0 && pos < sampleFrames ){
							double gtime_dyn = ms_start + pos * ms_per_sa;
							int dyn = m_dynamics[track].get_value( gtime_dyn );
							float amp = (float)dyn * k_inv64;
							outputs[0][pos] += (wavbuf[count++] - 64.0f) * k_inv64 * amp;
							outputs[1][pos] += (wavbuf[count++] - 64.0f) * k_inv64 * amp;
							len -= 2;
							pos++;
						}
					}
				}
			}else if( byte_per_sample == 2 ){
				if( channel == 1 ){
					while( pos < sampleFrames ){
						dat.read( wavbuf, buflen );
						int len = dat.gcount();
						if( len <= 0 ){
                            dat.close();
#ifdef _TEST
                            g_logger << "BAIL-OUT" << endl;
#endif
							goto fillzero;
						}
						int count = 0;
						while( len > 0 && pos < sampleFrames ){
							double gtime_dyn = ms_start + pos * ms_per_sa;
							int dyn = m_dynamics[track].get_value( gtime_dyn );
							float amp = (float)dyn * k_inv64;
							float v = ((signed short int)(wavbuf[count] | wavbuf[count + 1] << 8)) * k_inv32768 * amp;
							outputs[0][pos] += v;
							outputs[1][pos] += v;
							count += 2;
							len -= 2;
							pos++;
						}
					}
				}else{
					while( pos < sampleFrames ){
						dat.read( wavbuf, buflen );
						int len = dat.gcount();
						if( len <= 0 ){
                            dat.close();
#ifdef _TEST
                            g_logger << "BAIL-OUT" << endl;
#endif
							goto fillzero;
						}
						int count = 0;
						while( len > 0 && pos < sampleFrames ){
							double gtime_dyn = ms_start + pos * ms_per_sa;
							int dyn = m_dynamics[track].get_value( gtime_dyn );
							float amp = (float)dyn * k_inv64;
							outputs[0][pos] += ((signed short int)(wavbuf[count]     | wavbuf[count + 1] << 8)) * k_inv32768 * amp;
							outputs[1][pos] += ((signed short int)(wavbuf[count + 2] | wavbuf[count + 3] << 8)) * k_inv32768 * amp;
							count += 4;
							len -= 4;
							pos++;
						}
					}
				}
			}
			dat.close();
#endif
		}
	fillzero:
		m_processed_sample += sampleFrames;
	}catch( std::exception ex ){
#ifdef _TEST
		g_logger << "utauvsti::processReplacing; ex=" << ex.what() << endl;
#endif
	}
}

void utauvsti::load_singer_config(){
#ifdef _TEST
    g_logger << "utauvsti::load_singer_config" << endl << flush;
	g_logger << "(m_dict_singer==m_current_singer)=" << (m_dict_singer == m_current_singer ? "True" : "False") << endl;
#endif
    if( m_dict_singer == m_current_singer ){
        return;
    }
    m_singer_config.clear();
    m_singer_prefix.clear();
    string path = g_singers[m_current_singer].Path; // c:\ ... \voice\oto
    string dir = get_directory_name( path ); // c:\ ... \voice
    string name = get_filename_without_extension( path ); // oto
    string config_name = path_combine( path, "oto.ini" );
    string prefix_name = path_combine( path, "prefix.map" );
#ifdef _TEST
    g_logger << "config_name=" << config_name << endl;
#endif
    const int buflen = 256;
    char buf[buflen];

    // oto.iniを読み込み
    if( path_file_exists( config_name ) ){
#ifdef _TEST
        g_logger << "oto.ini was not found" << endl;
#endif
        ifstream ifs( config_name.c_str(), ios::in );
        while( ifs.peek() >= 0 ){
            ifs.getline( buf, buflen );
            string line( buf );
            string::size_type pos_eq = line.rfind( "=" );
            string lyric = line.substr( 0, pos_eq );
            line = line.substr( pos_eq + 1 );
            OtoArgs oa;
            int pos0 = -1;
            int vals[6];
            for( int i = 0; i < 6; i++ ){
                int pos1 = line.find( ",", pos0 + 1 );
                if( i == 0 ){
                    oa.Alias = line.substr( 0, pos1 );
                }else{
                    string spl = line.substr( pos0 + 1, pos1 - (pos0 + 1) + 1 );
                    stringstream ss( spl );
                    ss >> vals[i];
                }
                pos0 = pos1;
            }
            oa.msOffset = vals[1];
            oa.msConsonant = vals[2];
            oa.msBlank = vals[3];
            oa.msPreUtterance = vals[4];
            oa.msOverlap = vals[5];
#ifdef _TEST
			g_logger << "lyric=" << lyric << "; offset=" << oa.msOffset << ";consonant=" << oa.msConsonant << ";blank=" << oa.msBlank << ";ahead=" << oa.msPreUtterance << ";overwrap=" << oa.msOverlap << endl;
#endif
            m_singer_config.insert( map<string, OtoArgs>::value_type( lyric, oa ) );
        }
        ifs.close();
    }

    // prfix.mapを読み込み
    if( path_file_exists( prefix_name ) ){
        ifstream ifs( prefix_name.c_str(), ios::in );
        while( ifs.peek() >= 0 ){
            ifs.getline( buf, buflen );
            string line( buf );
            int tab1 = line.find( '\t' );
            int tab2 = line.find( '\t', tab1 + 1 );
            string note = line.substr( 0, tab1 );
            string prefix = (tab1 + 1 == tab2) ? "" : line.substr( tab1 + 1, tab2 - tab1 - 1 );
            string suffix = (tab2 + 1 == line.size()) ? "" : line.substr( tab2 + 1 );
            int note_number = note_number_from_note_string( note );
            m_singer_prefix.push_back( PreSuffix( note_number, prefix, suffix ) );
        }
        ifs.close();
    }
#ifdef _TEST
    g_logger << "PreSuffix" << endl;
    for( int i = 0; i < m_singer_prefix.size(); i++ ){
        g_logger << m_singer_prefix[i].Note << "\t" << m_singer_prefix[i].Prefix << "\t" << m_singer_prefix[i].Suffix << endl << flush;
    }
#endif
    m_dict_singer = m_current_singer;
}

bool utauvsti::getEffectName( char* name ){
    vst_strncpy( name, "utauvsti", kVstMaxEffectNameLen );
    return true;
}

bool utauvsti::getVendorString( char* text ){
    vst_strncpy( text, "boare", kVstMaxVendorStrLen );
    return true;
}

bool utauvsti::getProductString( char* text ){
    vst_strncpy( text, "utauvsti", kVstMaxProductStrLen );
    return true;
}

VstInt32 utauvsti::getVendorVersion () {
    return k_version;
}
	
VstPlugCategory utauvsti::getPlugCategory() {
    return kPlugCategSynth;
}
