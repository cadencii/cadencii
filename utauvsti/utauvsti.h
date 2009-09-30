/*
 * utauvsti.h
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
#ifndef __cplusplus
#error please compile as c++
#endif

#ifndef __utauvsti_h__
#define __utauvsti_h__

//#define WAVTOOL_ON_THE_CODE
//#define _BENCHMARK
#define _TEST
#include "audioeffectx.h"
#include <fstream>
#include <string>
#include <sstream>
#include <vector>
#include <map>
#include <locale>
#include "NRPN.h"
#include "symboltable.h"
#include "wavereader.h"

using namespace std;

#ifdef __GNUC__
typedef basic_ofstream<wchar_t, char_traits<wchar_t> > wofstream;
typedef basic_ifstream<wchar_t, char_traits<wchar_t> > wifstream;
#endif

typedef AEffect* (*PVSTMAIN)( audioMasterCallback audioMaster );
VstIntPtr AudioMaster( AEffect* effect, VstInt32 opcode, VstInt32 index, VstIntPtr value, void* ptr, float opt );
#if defined( _TEST ) || defined( _BENCHMARK )
std::ofstream g_logger; // デバッグ用のロガー
#endif

wstring wstring_from_string( string src, string loc );

//-------------------------------------------------------------------------------------------------------
/** 原音設定の引数．*/
//-------------------------------------------------------------------------------------------------------
struct OtoArgs{
    string Alias;   // エイリアス
    int msOffset;     // オフセット
    int msConsonant;  // 子音部
    int msBlank;      // ブランク
    int msPreUtterance;      // 先行発声
    int msOverlap;   // オーバーラップ
    OtoArgs(){
        Alias = "";
        msOffset = 0;
        msConsonant = 0;
        msBlank = 0;
        msPreUtterance = 0;
        msOverlap = 0;
    };
};

//-------------------------------------------------------------------------------------------------------
/** 歌詞イベント */
//-------------------------------------------------------------------------------------------------------
struct UtauEvent{
public:
    static const int MAX_ENV_POINTS = 10; //UTAU 0.2.36以降では10
    int msTime;
    int msLength;
    int Note;
    int Velocity;
    int Program;    // プログラムチェンジ．utauvsti.confの(3 + Program + 1)行目に書かれたフォルダの音源が，レンダリングに使用される
    string Result;  // resamplerの結果が保存されているファイル
    int msActualLength; //先行発声などにより，実際の長さとNRPNが指示した長さとは異なる．こっちが実際の音符長
    int msActualTime;   //同上．こちらが実際の音符の開始時刻
#ifndef WAVTOOL_ON_THE_CODE
    int clEndTime;
#endif
    int envP1;
    int envP2;
    int envP3;
    int envV1;
    int envV2;
    int envV3;
    int envV4;
    int envP4;
    int envP5;
    int envV5;
    // 原音設定
    OtoArgs Config;
    // 実際に使用するファイルの名前「あ↓」など．拡張子とフォルダ名は含まない
    string File;
    bool mode_r;
    int Moduration;

    UtauEvent(){
        for( int i = 0; i < LYRIC_LEN; i++ ){
            m_lyric[i] = '\0';
        }
        Program = 0;
        Result = "";
        Velocity = 64;
#ifndef WAVTOOL_ON_THE_CODE
        clEndTime = 0;
#endif
        // デフォルトのエンベロープはフラット
        //p1 p2 p3 v1 v2  v3  v4 ovr p4 p5 v5
        //0  5  35 0  100 100 0  0   0  0  100
        envP1 = 0;
        envP2 = 5;
        envP3 = 35;
        envV1 = 100;
        envV2 = 100;
        envV3 = 100;
        envV4 = 100;
        envP4 = 0;
        envP5 = 0;
        envV5 = 100;
        File = "";
        Moduration = 0;
        m_flags = "";
    };

    void set_envelope( int index, int value ){
        switch( index ){
            case 0:
                envP1 = value;
                break;
            case 1:
                envP2 = value;
                break;
            case 2:
                envP3 = value;
                break;
            case 3:
                envV1 = value;
                break;
            case 4:
                envV2 = value;
                break;
            case 5:
                envV3 = value;
                break;
            case 6:
                envV4 = value;
                break;
            case 7:
                envP4 = value;
                break;
            case 8:
                envP5 = value;
                break;
            case 9:
                envV5 = value;
                break;
        }
    };

    int get_envelope( int index ){
        switch( index ){
            case 0:
                return envP1;
            case 1:
                return envP2;
            case 2:
                return envP3;
            case 3:
                return envV1;
            case 4:
                return envV2;
            case 5:
                return envV3;
            case 6:
                return envV4;
            case 7:
                return envP4;
            case 8:
                return envP5;
            case 9:
                return envV5;
        }
    };

    string get_lyric(){
        ostringstream os;
        for( int i = 0; i < LYRIC_LEN; i++ ){
            if( m_lyric[i] != '\0' ){
                os << m_lyric[i];
            }else{
                break;
            }
        }
        return os.str();
    };

    void set_lyric( string s ){
        for( int i = 0; i < LYRIC_LEN; i++ ){
            m_lyric[i] = '\0';
        }
        for( int i = 0; i < s.length() && i < LYRIC_LEN; i++ ){
            set_lyric( i, s[i] );
        }
    };

    void set_lyric( int index, char c ){
        if( 0 <= index && index < LYRIC_LEN ){
            m_lyric[index] = c;
        }
    };

    void set_flags( string value ){
        m_flags = value;
    };

    string get_flags(){
        return m_flags;
    };

    void set_flags( int index, char c ){
        if( 0 <= index && index < m_flags.length() ){
            m_flags[index] = c;
        }else{
            for( int i = m_flags.length(); i <= index; i++ ){
                m_flags = m_flags + " ";
            }
            m_flags[index] = c;
        }
    };
private:
    static const int LYRIC_LEN = 60; //∵発音記号用のNRPNが0x5013から0x504eまで使えるから。
    char m_lyric[LYRIC_LEN];
    string m_flags;
};

//-------------------------------------------------------------------------------------------------------
/** 音源のディレクトリと名前 */
//-------------------------------------------------------------------------------------------------------
struct SingerConfig{
    string Path;
    string Name;
    SingerConfig(){
        Path = "";
        Name = "";
    };
};

//-------------------------------------------------------------------------------------------------------
/** BPListのデータ点 */
//-------------------------------------------------------------------------------------------------------
struct BPPair{
    double msTime;
    int Value;
    BPPair( double ms_time, int value ){
        msTime = ms_time;
        Value = value;
    };
};

//-------------------------------------------------------------------------------------------------------
/** DynamicsなどのBPPairのリスト */
//-------------------------------------------------------------------------------------------------------
class BPList{
private:
    int m_default;
#ifdef _TEST
public:
#endif
    vector<BPPair> m_list;
public:
    // このBPListのディレイ(millisecond)
    int msDelay;

    BPList(){
        m_default = 0;
    };

    // リストをクリアする．ディレイも0に戻す
    void clear(){
        m_list.clear();
        msDelay = 0;
    };

    // リストの末尾にデータ点を追加する
    void push_back( double ms_time, int value ){
        m_list.push_back( BPPair( ms_time, value ) );
    };

    // 現在リストに登録されているデータ点の個数
    int count(){
        return m_list.size();
    };

    // 指定した時刻における値を取得する
    int get_value( double ms_time ){
        static int seek_start;
        if( m_list.size() == 0 ){
            return m_default;
        }
        if( seek_start < 0 || m_list.size() <= seek_start ){
            seek_start = 0;
        }
        double msec = m_list[seek_start].msTime;
        int val = m_list[seek_start].Value;
        double nmsec = msec;
        int nval = val;
        for( int i = seek_start; i < m_list.size() - 1; i++ ){
            nmsec = m_list[i + 1].msTime;
            nval = m_list[i + 1].Value;
            if( msec <= ms_time && ms_time < nmsec ){
                seek_start = i;
                return val;
            }
            msec = nmsec;
            val = nval;
        }
        seek_start = m_list.size() - 1;
        return nval;
    };

    // このBPListのデフォルト値
    int get_default(){
        return m_default;
    };

    void set_default( int value ){
        m_default = value;
    };
};

//-------------------------------------------------------------------------------------------------------
/** DynamicsなどのBPPairのリスト */
//-------------------------------------------------------------------------------------------------------
struct PreSuffix{
    int Note;
    string Prefix;
    string Suffix;
    PreSuffix( int note, string prefix, string suffix ){
        Note = note;
        Prefix = prefix;
        Suffix = suffix;
    };
};

//-------------------------------------------------------------------------------------------------------
/** utauvsti */
//-------------------------------------------------------------------------------------------------------
class utauvsti : public AudioEffectX {
public:
    utauvsti( audioMasterCallback audioMaster );
    ~utauvsti();

    //-----------------------------------------------------------------------------------------------
    // AEffect
    virtual VstIntPtr dispatcher( VstInt32 opcode, VstInt32 index, VstIntPtr value, void* ptr, float opt );

    virtual void processReplacing( float** inputs, float** outputs, VstInt32 sampleFrames );

    virtual bool getEffectName( char* name );
    virtual bool getVendorString( char* text );
    virtual bool getProductString( char* text );
    virtual VstInt32 getVendorVersion();

    virtual VstPlugCategory getPlugCategory();

    //-----------------------------------------------------------------------------------------------
    // utility
    static string get_aeffect_opcode_string( int opcode ){
        switch( opcode ){
            case 0:
                return "effOpen";
            case 1:
                return "effClose";
            case 2:
                return "effSetProgram";
            case 3:
                return "effGetProgram";
            case 4:
                return "effSetProgramName";
            case 5:
                return "effGetProgramName";
            case 6:
                return "effGetParamLabel";
            case 7:
                return "effGetParamDisplay";
            case 8:
                return "effGetParamName";
            case 9:
                return "__effGetVuDeprecated";
            case 10:
                return "effSetSampleRate";
            case 11:
                return "effSetBlockSize";
            case 12:
                return "effMainsChanged";
            case 13:
                return "effEditGetRect";
            case 14:
                return "effEditOpen";
            case 15:
                return "effEditClose";
            case 16:
                return "__effEditDrawDeprecated";
            case 17:
                return "__effEditMouseDeprecated";
            case 18:
                return "__effEditKeyDeprecated";
            case 19:
                return "effEditIdle";
            case 20:
                return "__effEditTopDeprecated";
            case 21:
                return "__effEditSleepDeprecated";
            case 22:
                return "__effIdentifyDeprecated";
            case 23:
                return "effGetChunk";
            case 24:
                return "effSetChunk";
            case 25:
                return "effProcessEvents";
            case 26:
                return "effCanBeAutomated";
            case 27:
                return "effString2Parameter";
            case 28:
                return "__effGetNumProgramCategoriesDeprecated";
            case 29:
                return "effGetProgramNameIndexed";
            case 30:
                return "__effCopyProgramDeprecated";
            case 31:
                return "__effConnectInputDeprecated";
            case 32:
                return "__effConnectOutputDeprecated";
            case 33:
                return "effGetInputProperties";
            case 34:
                return "effGetOutputProperties";
            case 35:
                return "effGetPlugCategory";
            case 36:
                return "__effGetCurrentPositionDeprecated";
            case 37:
                return "__effGetDestinationBufferDeprecated";
            case 38:
                return "effOfflineNotify";
            case 39:
                return "effOfflinePrepare";
            case 40:
                return "effOfflineRun";
            case 41:
                return "effProcessVarIo";
            case 42:
                return "effSetSpeakerArrangement";
            case 43:
                return "__effSetBlockSizeAndSampleRateDeprecated";
            case 44:
                return "effSetBypass";
            case 45:
                return "effGetEffectName";
            case 46:
                return "__effGetErrorTextDeprecated";
            case 47:
                return "effGetVendorString";
            case 48:
                return "effGetProductString";
            case 49:
                return "effGetVendorVersion";
            case 50:
                return "effVendorSpecific";
            case 51:
                return "effCanDo";
            case 52:
                return "effGetTailSize";
            case 53:
                return "__effIdleDeprecated";
            case 54:
                return "__effGetIconDeprecated";
            case 55:
                return "__effSetViewPositionDeprecated";
            case 56:
                return "effGetParameterProperties";
            case 57:
                return "__effKeysRequiredDeprecated";
            case 58:
                return "effGetVstVersion";
            case 59:
                return "effEditKeyDown";
            case 60:
                return "effEditKeyUp";
            case 61:
                return "effSetEditKnobMode";
            case 62:
                return "effGetMidiProgramName";
            case 63:
                return "effGetCurrentMidiProgram";
            case 64:
                return "effGetMidiProgramCategory";
            case 65:
                return "effHasMidiProgramsChanged";
            case 66:
                return "effGetMidiKeyName";
            case 67:
                return "effBeginSetProgram";
            case 68:
                return "effEndSetProgram";
            case 69:
                return "effGetSpeakerArrangement";
            case 70:
                return "effShellGetNextPlugin";
            case 71:
                return "effStartProcess";
            case 72:
                return "effStopProcess";
            case 73:
                return "effSetTotalSampleToProcess";
            case 74:
                return "effSetPanLaw";
            case 75:
                return "effBeginLoadBank";
            case 76:
                return "effBeginLoadProgram";
            case 77:
                return "effSetProcessPrecision";
            case 78:
                return "effGetNumMidiInputChannels";
            case 79:
                return "effGetNumMidiOutputChannels";
        }
        return "";
    };

    static string get_audio_master_opcode_string( int opcode ){
        switch( opcode ){
            case 0:
                return "audioMasterAutomate";
            case 1:
                return "audioMasterVersion";
            case 2:
                return "audioMasterCurrentId";
            case 3:
                return "audioMasterIdle";
            case 4:
                return "__audioMasterPinConnectedDeprecated";
            case 6:
                return "__audioMasterWantMidiDeprecated";
            case 7:
                return "audioMasterGetTime";
            case 8:
                return "audioMasterProcessEvents";
            case 9:
                return "__audioMasterSetTimeDeprecated";
            case 10:
                return "__audioMasterTempoAtDeprecated";
            case 11:
                return "__audioMasterGetNumAutomatableParametersDeprecated";
            case 12:
                return "__audioMasterGetParameterQuantizationDeprecated";
            case 13:
                return "audioMasterIOChanged";
            case 14:
                return "__audioMasterNeedIdleDeprecated";
            case 15:
                return "audioMasterSizeWindow";
            case 16:
                return "audioMasterGetSampleRate";
            case 17:
                return "audioMasterGetBlockSize";
            case 18:
                return "audioMasterGetInputLatency";
            case 19:
                return "audioMasterGetOutputLatency";
            case 20:
                return "__audioMasterGetPreviousPlugDeprecated";
            case 21:
                return "__audioMasterGetNextPlugDeprecated";
            case 22:
                return "__audioMasterWillReplaceOrAccumulateDeprecated";
            case 23:
                return "audioMasterGetCurrentProcessLevel";
            case 24:
                return "audioMasterGetAutomationState";
            case 25:
                return "audioMasterOfflineStart";
            case 26:
                return "audioMasterOfflineRead";
            case 27:
                return "audioMasterOfflineWrite";
            case 28:
                return "audioMasterOfflineGetCurrentPass";
            case 29:
                return "audioMasterOfflineGetCurrentMetaPass";
            case 30:
                return "__audioMasterSetOutputSampleRateDeprecated";
            case 31:
                return "__audioMasterGetOutputSpeakerArrangementDeprecated";
            case 32:
                return "audioMasterGetVendorString";
            case 33:
                return "audioMasterGetProductString";
            case 34:
                return "audioMasterGetVendorVersion";
            case 35:
                return "audioMasterVendorSpecific";
            case 36:
                return "__audioMasterSetIconDeprecated";
            case 37:
                return "audioMasterCanDo";
            case 38:
                return "audioMasterGetLanguage";
            case 39:
                return "__audioMasterOpenWindowDeprecated";
            case 40:
                return "__audioMasterCloseWindowDeprecated";
            case 41:
                return "audioMasterGetDirectory";
            case 42:
                return "audioMasterUpdateDisplay";
            case 43:
                return "audioMasterBeginEdit";
            case 44:
                return "audioMasterEndEdit";
            case 45:
                return "audioMasterOpenFileSelector";
            case 46:
                return "audioMasterCloseFileSelector";
            case 47:
                return "__audioMasterEditFileDeprecated";
            case 48:
                return "__audioMasterGetChunkFileDeprecated";
            case 49:
                return "__audioMasterGetInputSpeakerArrangementDeprecated";
        }
        return "";
    };
    static string note_string_from_note_number( int note_number ){
        int odd = note_number % 12;
        string list[12] = {"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"};
        string head = list[odd];
        ostringstream oss( "" );
        oss << head << (note_number / 12 - 1);
        return oss.str();
    };
    static int note_number_from_note_string( string note_string ){
        char let1 = (note_string.size() >= 1) ? note_string[0] : ' ';
        char let2 = (note_string.size() >= 2) ? note_string[1] : ' ';
        char let3 = (note_string.size() >= 3) ? note_string[2] : ' ';
        int odd;
        switch( let1 ){
            case 'C':
            case 'c':
                odd = 0;
                break;
            case 'D':
            case 'd':
                odd = 2;
                break;
            case 'E':
            case 'e':
                odd = 4;
                break;
            case 'F':
            case 'f':
                odd = 5;
                break;
            case 'G':
            case 'g':
                odd = 7;
                break;
            case 'A':
            case 'a':
                odd = 9;
                break;
            case 'B':
            case 'b':
            case 'H':
            case 'h':
                odd = 11;
                break;
        }
        char n[2];
        n[0] = let2;
        n[1] = '\0';
        if( let2 == '#' ){
            odd++;
            n[0] = let3;
        }
        string s( n );
        stringstream ss( s );
        int b = 0;
        ss >> b;
        return (b + 1) * 12 + odd;
    };
    double sa_from_ms( double millisec ){
        return millisec / 1000.0 * m_sample_rate;
    };
    double ms_from_sa( double sample_time ){
        return 1000.0 * sample_time / m_sample_rate;
    };
    // VOCALOID NRPNのPITとPBSから，UTAUのピッチ値を計算する
    int pit_from_nrpnpit( int pit, int pbs ){
        static const float inv8192 = 100.0f / 8192.0f;
        return (int)((float)pit * (float)pbs * inv8192);
    };
private:
    static const int k_version = 1;
    static const int k_tempo = 500000;
    static float k_inv32768;
    static float k_inv64;
    static const int k_num_track = 16;
    // サンプリングレート
    float m_sample_rate;
    // ブロックサイズ．使わない？
    int m_block_size;
    // effProcessEventにて取り出したイベント情報
    vector<UtauEvent> m_events[k_num_track];
    // processReplacingによって処理されたサンプル数
    int m_processed_sample;
    // 現在している音源の、原音設定
    map<string, OtoArgs> m_singer_config;
    // 原音設定のprefix.mapの内容
    vector<PreSuffix> m_singer_prefix;
    // m_singer_configに入っている音源のインデックス
    int m_dict_singer;
    // 現在使用している音源のインデックス（プログラムチェンジで変更）
    int m_current_singer;
    // 最後にレンダリングされたm_eventsのインデックス
    int m_last_rendered[k_num_track];
    // Dynamics
    BPList m_dynamics[k_num_track];
    // PitchBend
    BPList m_pitchbend[k_num_track];
    // PitchBend Sensitivity
    BPList m_pitchbend_sensitivity[k_num_track];
    // 現在指定されているプログラムナンバー
    int m_current_program[k_num_track];
#ifdef WAVTOOL_ON_THE_CODE
    // 波形のバッファ
    float *m_buf_left;
    float *m_buf_right;
    // 波形バッファの0番目が担当している時刻
    int m_buf_start_sample;
    // 現在のバッファの長さ
    int m_buf_length;
#endif

    void load_singer_config();
    string apply_prefix_map( string lyric, int note ){
        for( int i = 0; i < m_singer_prefix.size(); i++ ){
            if( m_singer_prefix[i].Note == note ){
                return m_singer_prefix[i].Prefix + lyric + m_singer_prefix[i].Suffix;
            }
        }
        return lyric;
    };
};

struct VsqNrpn {
    int saTime;
    unsigned int Nrpn;
    unsigned char DataMsb;
    unsigned char DataLsb;

    VsqNrpn( int sample_time, unsigned int nrpn, unsigned char data_msb ){
        saTime = sample_time;
        Nrpn = nrpn;
        DataMsb = data_msb;
        DataLsb = 0x0;
    };

    VsqNrpn( int sample_time, unsigned int nrpn, unsigned char data_msb, unsigned char data_lsb ){
        saTime = sample_time;
        Nrpn = nrpn;
        DataMsb = data_msb;
        DataLsb = data_lsb;
    };

    // datamsbとdatalsbを元にこのNrpnが持っている値を計算します
    int gvalue(){
        return (int)(DataMsb << 7 | DataLsb);
    };
};

#endif // __utauvsti_h__
