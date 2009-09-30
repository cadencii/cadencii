#ifndef __libvsq_h__
#define __libvsq_h__
#include <string>
#include <sstream>
#include <fstream>
#include <vector>
#include <stdlib.h>
#include "cp932.h"

using namespace std;

namespace vsq{

inline vector<string> util_split( string str, string delim ){
    vector<string> result;
    int cutAt;
    while( (cutAt = str.find_first_of( delim )) != str.npos ){
        if( cutAt > 0 ){
            result.push_back( str.substr( 0, cutAt ) );
        }
        str = str.substr( cutAt + 1 );
    }
    if( str.length() > 0 ){
        result.push_back( str );
    }
    return result;
}

inline string string_replace( string str, string before, string after ){
    int start = 0;
    int index = str.find( before, start );
    string newone = str;
    while( index < 0 ){
        newone = newone.substr( 0, index ) + after + newone.substr( index + before.length() + 1 );
        start = index + before.length() + 1 + (after.length() - before.length());
        index = newone.find( before, start );
    }
    return newone;
}

template<class T> T parse( string s ){
    ostringstream oss( "" );
    T ret;
    oss >> ret;
    return ret;
}

typedef unsigned char byte;

struct TColor{
    int R;
    int G;
    int B;
};

class StreamReader{
public:
    StreamReader( string path ){
        m_ifs.open( path.c_str() );
    };

    void Close(){
        m_ifs.close();
    };

    string ReadLine(){
        string ret;
        if( get_line( &m_ifs, &ret ) ){
            return ret;
        }else{
            return NULL;
        }
    };
    int Peek(){
        return m_ifs.peek();
    };
private:
    ifstream m_ifs;
    static bool get_line( ifstream *ifs, string *str ){
        char ch;
        if( ifs->eof() ){
            return false;
        }
        while( ifs->get( ch ) ){
            if( ch == 0x0d ){
                if( ifs->get( ch ) ){
                    if( ch != 0x0a ){
                        ifs->seekg( -1, ios::cur );
                    }
                }
                break;
            }else if( ch == 0x0a ){
                break;
            }
            str->append( 1, ch );
        }
        return true;
    };
};

/// <summary>
/// メモリー上でテキストファイルを扱うためのクラス．
/// </summary>
class TextMemoryStream {
public:
    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    void Write( string value ){
        m_ms << value;
    };
    void Rewind(){
        m_ms.seekg( 0, ios::beg );
    };
    void WriteLine( string s ){
        m_ms << s << "\x0d\x0a";
    };
    void Close(){
        m_ms.str( "" );
    };
    int Peek(){
        long current = m_ms.tellg();
        int ret = m_ms.get();
        if( m_ms.gcount() <= 0 ){
            m_ms.seekg( current, ios::beg );
            return -1;
        }
        m_ms.seekg( current, ios::beg );
        return ret;
    };
    string ReadLine(){
        ostringstream buffer;
        char value;
        char ret;
        ret = m_ms.get();
        while ( ret >= 0 ) {
            value = (char)ret;
            if ( value == NEW_LINE[0] ) {
                char next;
                long current = m_ms.tellg(); //0x0Dを検出した直後のストリームの位置
                for ( int i = 1; i < 2; i++ ) {
                    ret = m_ms.get();
                    if ( ret >= 0 ) {
                        next = ret;
                        if ( next != NEW_LINE[i] ) {
                            m_ms.seekg( current, ios::beg );
                            break;
                        }
                    }
                }
                break;
            }
            buffer << value;
            ret = m_ms.get();
        }
        return buffer.str();
    };
    void Dispose(){
        m_ms.str( "" );
    };
    TextMemoryStream( string path ){
        m_ms.str( "" );
        //if ( File.Exists( path ) ) {
        StreamReader sr( path );
        while( sr.Peek() >= 0 ){
            string line = sr.ReadLine();
            m_ms << line << "\x0d\x0a";
        }
        sr.Close();
        m_ms.seekg( 0, ios::beg );
        NEW_LINE[0] = '\x0d';
        NEW_LINE[1] = '\x0a';
    };
    TextMemoryStream(){
        m_ms.str( "" );
        NEW_LINE[0] = '\x0d';
        NEW_LINE[1] = '\x0a';
    };
private:
    //FileAccess m_access;
    stringstream m_ms;
    //Encoding m_enc;
    char NEW_LINE[2];
};

class VsqCommon {
public:
    string Version;
    string Name;
    string Color;
    int DynamicsMode;
    int PlayMode;

    /// <summary>
    /// 各パラメータを指定したコンストラクタ
    /// </summary>
    /// <param name="name">トラック名</param>
    /// <param name="color">Color値（意味は不明）</param>
    /// <param name="dynamics_mode">DynamicsMode（デフォルトは1）</param>
    /// <param name="play_mode">PlayMode（デフォルトは1）</param>
    VsqCommon( string name, TColor color, int dynamics_mode, int play_mode );

    /// <summary>
    /// MetaTextのテキストファイルからのコンストラクタ
    /// </summary>
    /// <param name="sr">読み込むテキストファイル</param>
    /// <param name="last_line">読み込んだ最後の行が返される</param>
    VsqCommon( TextMemoryStream sr, string& last_line );

    /// <summary>
    /// インスタンスの内容をテキストファイルに出力します
    /// </summary>
    /// <param name="sw">出力先</param>
    void write( TextMemoryStream& sw );

    /// <summary>
    /// VsqCommon構造体を構築するテストを行います
    /// </summary>
    /// <returns>テストに成功すればtrue、そうでなければfalse</returns>
    static bool test();
};

enum VsqHandleType {
    Lyric,
    Vibrato,
    Singer
};

const int _NUM_SYMBOL_VOWEL_JP = 5;
static string _SYMBOL_VOWEL_JP[_NUM_SYMBOL_VOWEL_JP] = {
    "a",
    "i",
    "M",
    "e",
    "o",
};

const int _NUM_SYMBOL_CONSONANT_JP = 36;
static string _SYMBOL_CONSONANT_JP[_NUM_SYMBOL_CONSONANT_JP] = {
    "k",
    "k'",
    "g",
    "g'",
    "N",
    "N'",
    "s",
    "S",
    "z",
    "Z",
    "dz",
    "dZ",
    "t",
    "t'",
    "ts",
    "tS",
    "d",
    "d'",
    "n",
    "J",
    "h",
    "h\\",
    "C",
    "p\\",
    "p\\'",
    "b",
    "b'",
    "p",
    "p'",
    "m",
    "m'",
    "j",
    "4",
    "4'",
    "w",
    "N\\",
};

const int _NUM_SYMBOL_EN = 53;
static string _SYMBOL_EN[_NUM_SYMBOL_EN] = {
    "@",
    "V",
    "e",
    "e",
    "I",
    "i:",
    "{",
    "O:",
    "Q",
    "U",
    "u:",
    "@r",
    "eI",
    "aI",
    "OI",
    "@U",
    "aU",
    "I@",
    "e@",
    "U@",
    "O@",
    "Q@",
    "w",
    "j",
    "b",
    "d",
    "g",
    "bh",
    "dh",
    "gh",
    "dZ",
    "v",
    "D",
    "z",
    "Z",
    "m",
    "n",
    "N",
    "r",
    "l",
    "l0",
    "p",
    "t",
    "k",
    "ph",
    "th",
    "kh",
    "tS",
    "f",
    "T",
    "s",
    "S",
    "h",
};

class VsqPhoneticSymbol {
public:
    static bool IsConsonant( string symbol ) {
        for( int i = 0; i < _NUM_SYMBOL_CONSONANT_JP; i++ ){
            string s = _SYMBOL_CONSONANT_JP[i];
            if ( s == symbol ) {
                return true;
            }
        }
        return false;
    }
    static bool IsValidSymbol( string symbol ) {
        for( int i = 0; i < _NUM_SYMBOL_VOWEL_JP; i++ ){
            string s = _SYMBOL_VOWEL_JP[i];
            if ( s == symbol ) {
                return true;
            }
        }
        for( int i = 0; i < _NUM_SYMBOL_CONSONANT_JP; i++ ){
            string s = _SYMBOL_CONSONANT_JP[i];
            if ( s == symbol ) {
                return true;
            }
        }
        for( int i = 0; i < _NUM_SYMBOL_EN; i++ ){
            string s = _SYMBOL_EN[i];
            if ( s == symbol ) {
                return true;
            }
        }
        return false;
    }
};


/// <summary>
/// VsqHandleに格納される歌詞の情報を扱うクラス。
/// </summary>
class Lyric {
private:
    string m_phrase;
    vector<string> m_phonetic_symbol;
    float d1;
    vector<int> m_consonant_adjustment;
    bool m_protected;

    Lyric(){
    }

    /// <summary>
    /// バイト並びsearchの中に含まれるバイト並びvalueの位置を探します。
    /// </summary>
    /// <param name="search">検索対象のバイト並び</param>
    /// <param name="value">検索するバイト並び</param>
    /// <returns>valueが見つかればそのインデックスを、見つからなければ-1を返します</returns>
    static int mIndexOf( vector<unsigned char> search, vector<unsigned char> value ) {
        int i, j;
        int search_length = search.size();
        int value_length = value.size();
        
        // 検索するバイト並びが、検索対象のバイト並びより長いとき。
        // 見つかるわけない
        if ( value_length > search_length ) {
            return -1;
        }

        // i : 検索の基点
        for ( i = 0; i <= search_length - value_length; i++ ) {
            bool failed = false;
            for ( j = 0; j < value_length; j++ ) {
                if ( search[i + j] != value[j] ) {
                    failed = true;
                    break;
                }
            }
            if ( !failed ) {
                return i;
            }
        }
        return -1;
    }

    
    /// <summary>
    /// 文字がプリント出力可能かどうかを判定します
    /// </summary>
    /// <param name="ch"></param>
    /// <returns></returns>
    static bool isprint( char ch ) {
        if ( 32 <= (int)ch && (int)ch <= 126 ) {
            return true;
        } else {
            return false;
        }
    }

public:
    bool PhoneticSymbolProtected() {
            return m_protected;
    }

    void PhoneticSymbolProtected( bool value ){
        m_protected = value;
    }

    float UnknownFloat() {
        return d1;
    }

    void UnknownFloat( float value ){
        d1 = value;
    }

    vector<int> ConsonantAdjustment() {
        return m_consonant_adjustment;
    }

    /// <summary>
    /// 歌詞、発音記号を指定したコンストラクタ
    /// </summary>
    /// <param name="phrase">歌詞</param>
    /// <param name="phonetic_symbol">発音記号</param>
    Lyric( string phrase, string phonetic_symbol ) {
        m_phrase = phrase;
        PhoneticSymbol( phonetic_symbol );
        d1 = 0.000000f;
    }
    
    /// <summary>
    /// この歌詞のフレーズを取得または設定します。
    /// </summary>
    string Phrase() {
        return m_phrase;
    }
    
    void Phrase( string value ){
        m_phrase = value;
    }

    /// <summary>
    /// この歌詞の発音記号を取得または設定します。
    /// </summary>
    string PhoneticSymbol() {
        string ret = m_phonetic_symbol[0];
        for ( int i = 1; i < m_phonetic_symbol.size(); i++ ) {
            ret += " " + m_phonetic_symbol[i];
        }
        return ret;
    }

    void PhoneticSymbol( string value ){
        string s = string_replace( value, "  ", " " );
        m_phonetic_symbol = util_split( s, " " );
        for ( int i = 0; i < m_phonetic_symbol.size(); i++ ) {
            m_phonetic_symbol[i] = string_replace(  m_phonetic_symbol[i], "\\\\", "\\" );
        }
        m_consonant_adjustment = vector<int>( m_phonetic_symbol.size() );
        for ( int i = 0; i < m_phonetic_symbol.size(); i++ ) {
            if ( VsqPhoneticSymbol::IsConsonant( m_phonetic_symbol[i] ) ) {
                m_consonant_adjustment[i] = 64;
            } else {
                m_consonant_adjustment[i] = 0;
            }
        }
    }

    vector<string> PhoneticSymbolList(){
        vector<string> ret = vector<string>( m_phonetic_symbol.size() );
        for ( int i = 0; i < m_phonetic_symbol.size(); i++ ) {
            ret[i] = m_phonetic_symbol[i];
        }
        return ret;
    }

    /// <summary>
    /// 文字列からのコンストラクタ
    /// </summary>
    /// <param name="_line">生成元の文字列</param>
    Lyric( string _line ) {
        if ( _line.length() <= 0 ) {
            m_phrase = "a";
            PhoneticSymbol( "a" );
            d1 = 1.0f;
            m_protected = false;
        } else {
            vector<string> spl = util_split( _line, "," );
            int c_length = spl.size() - 3;
            if ( spl.size() < 4 ) {
                m_phrase = "a";
                PhoneticSymbol( "a" );
                d1 = 0.0f;
                m_protected = false;
            } else {
                m_phrase = decode( spl[0] );
                PhoneticSymbol( decode( spl[1] ) );
                d1 = parse<float>( spl[2] );
                m_protected = (spl[spl.size() - 1] == "0") ? false : true;
            }
        }
    }

    /// <summary>
    /// mIndexOfのテストメソッド。search, valueをいろいろ変えてテストする事。
    /// </summary>
    /// <returns></returns>
    static bool test_mIndexOf() {
        vector<unsigned char> search;
        vector<unsigned char> value;
        unsigned char src_search[6] = { 0, 12, 3, 5, 16, 34 };
        unsigned char src_value[2] = { 16, 34 };
        for( int i = 0; i < 6; i++ ){
            search.push_back( src_search[i] );
        }
        for( int i = 0; i < 2; i++ ){
            value.push_back( src_value[i] );
        }
        if ( mIndexOf( search, value ) == 4 ) {
            return true;
        } else {
            return false;
        }
    }

    /// <summary>
    /// エスケープされた\"や、\x**を復帰させます
    /// </summary>
    /// <param name="_string">デコード対象の文字列</param>
    /// <returns>デコード後の文字列</returns>
    static string decode( string _string ) {
        string result = _string;
        result = string_replace( result, "\\\"", "" );
        vector<unsigned char> str;
        for( int i = 0; i < result.length(); i++ ){
            str.push_back( (unsigned char)result[i] );
        }

        vector<unsigned char> x16;
        string xx = "\\x";
        for( int i = 0; i < xx.length(); i++ ){
            x16.push_back( (unsigned char)xx[i] );
        }
        int index = mIndexOf( str, x16 );
        while ( index >= 0 ) {
            char chr_byte[2];
            chr_byte[0] = str[index + 2];
            chr_byte[1] = str[index + 3];
            string chr( chr_byte );
            char *endstr;
            int chrcode = strtol( chr.c_str(), &endstr, 16 );
            str[index] = (byte)chrcode;
            for ( int i = index + 4; i < str.size(); i++ ) {
                str[i - 3] = str[i];
            }
            int length = str.size() - 3;
            vector<byte> new_str;
            for ( int i = 0; i < length; i++ ) {
                new_str.push_back( str[i] );
            }
            Array.Resize( ref str, length );
            str = new_str;
            index = mIndexOf( str, x16 );
        }
        return cp932_convert( str );
    }


    /// <summary>
    /// 与えられた文字列の中の2バイト文字を\x**の形式にエンコードします。
    /// </summary>
    /// <param name="item">エンコード対象</param>
    /// <returns>エンコードした文字列</returns>
    static vector<wchar_t> encode( string item ) {
        //Encoding sjis = Encoding.GetEncoding( 932 );
        unsigned char[] bytea = cp932_convert( item );//            sjis.GetBytes( item );
        string result = "";
        for ( int i = 0; i < bytea.Length; i++ ) {
            if ( isprint( (char)bytea[i] ) ) {
                result += (char)bytea[i];
            } else {
                result += "\\x" + Convert.ToString( bytea[i], 16 );
            }
        }
        wchar_t[] res = result.ToCharArray();
        return res;
    }


    /// <summary>
    /// 与えられた文字列をShift_JISとみなし、byte[]に変換しさらにchar[]に変換したもの返します
    /// </summary>
    /// <param name="item">変換元の文字列</param>
    /// <returns>変換後のchar[]</returns>
    static vector<wchar_t> encodeEx( string item ) {
        //Encoding sjis = Encoding.GetEncoding( 932 );
        byte[] dat = cp932_convert( item );//            sjis.GetBytes( item );
        char[] result = new char[dat.Length];
        for ( int i = 0; i < dat.Length; i++ ) {
            result[i] = (char)dat[i];
        }
        return result;
    }


    /// <summary>
    /// このインスタンスを文字列に変換します
    /// </summary>
    /// <param name="a_encode">2バイト文字をエンコードするか否かを指定するフラグ</param>
    /// <returns>変換後の文字列</returns>
    string ToString( bool a_encode ) {
        string result;
        if ( a_encode ) {
            string njp = new string( encode( this.Phrase ) );
            result = "\"" + njp + "\",\"" + this.PhoneticSymbol + "\"," + d1.ToString( "0.000000" );
        } else {
            result = "\"";
            //Encoding sjis = Encoding.GetEncoding( 932 );
            byte[] dat = cp932.convert( this.Phrase );//                sjis.GetBytes( this.Phrase );
            for ( int i = 0; i < dat.Length; i++ ) {
                result += (char)dat[i];
            }
            result += "\",\"" + this.PhoneticSymbol + "\"," + d1.ToString( "0.000000" );
            result = result.Replace( "\\\\", "\\" );
        }
        for ( int i = 0; i < m_consonant_adjustment.Length; i++ ) {
            result += "," + m_consonant_adjustment[i];
        }
        if ( m_protected ) {
            result += ",1";
        } else {
            result += ",0";
        }
        return result;
    }

    /// <summary>
    /// Lyricインスタンスを構築するテストを行います
    /// </summary>
    /// <returns>テストに成功すればtrue、そうでなければfalseを返します</returns>
    static bool test() {
        string line = "\\\"\\x82\\xe7\\\",\\\"4 a\\\",1.000000,64,1,1";
        //Console.WriteLine( "Lyric.test; line=" + line );
        Lyric lyric( line );
        if ( lyric.Phrase == "ら" &&
            lyric.PhoneticSymbol == "4 a" &&
            lyric.d1 == 1.0 &&
            lyric.m_consonant_adjustment[0] == 64 &&
            lyric.m_consonant_adjustment[1] == 1 &&
            lyric.m_consonant_adjustment[2] == 1 ) {
            return true;
        } else {
            return false;
        }
    }

};

/// <summary>
/// ハンドルを取り扱います。ハンドルにはLyricHandle、VibratoHandleおよびIconHandleがある
/// </summary>
class VsqHandle {
private:
    VsqHandleType m_type;
    /// <summary>
    /// インスタンスをコンソール画面に出力します
    /// </summary>
    void Print() {
        string result = this.ToString();
        Console.WriteLine( result );
    }
public:
    int Index;
    string IconID;
    string IDS;
    Lyric L0;
    int Original;
    string Caption;
    int Length;
    int StartDepth;
    VibratoBPList DepthBP;
    int StartRate;
    VibratoBPList RateBP;
protected:
    int m_language;
    int m_program;
    protected VsqHandle() {
    }
public:
    LyricHandle ConvertToLyricHandle() {
        LyricHandle ret = new LyricHandle();
        ret.L0 = (Lyric)L0;
        ret.m_type = m_type;
        ret.Index = Index;
        return ret;
    }

    VibratoHandle ConvertToVibratoHandle() {
        VibratoHandle ret = new VibratoHandle();
        ret.m_type = m_type;
        ret.Index = Index;
        ret.Caption = Caption;
        ret.DepthBP = (VibratoBPList)DepthBP.Clone();
        ret.IconID = IconID;
        ret.IDS = IDS;
        ret.Index = Index;
        ret.Length = Length;
        ret.Original = Original;
        ret.RateBP = (VibratoBPList)RateBP.Clone();
        ret.StartDepth = StartDepth;
        ret.StartRate = StartRate;
        return ret;
    }

    IconHandle ConvertToIconHandle() {
        IconHandle ret = new IconHandle();
        ret.m_type = m_type;
        ret.Index = Index;
        ret.Caption = Caption;
        ret.IconID = IconID;
        ret.IDS = IDS;
        ret.Index = Index;
        ret.Language( m_language );
        ret.Length = Length;
        ret.Original = Original;
        ret.Program( m_program );
        return ret;
    }

    VsqHandleType type() {
        return m_type;
    }

    void type( VsqHandleType value ) {
        m_type = value;
    }

    /// <summary>
    /// インスタンスをストリームに書き込みます。
    /// encode=trueの場合、2バイト文字をエンコードして出力します。
    /// </summary>
    /// <param name="sw">書き込み対象</param>
    /// <param name="encode">2バイト文字をエンコードするか否かを指定するフラグ</param>
    void write( TextMemoryStream sw, bool encode ) {
        sw.WriteLine( this.ToString( encode ) );
    }

    /// <summary>
    /// FileStreamから読み込みながらコンストラクト
    /// </summary>
    /// <param name="sr">読み込み対象</param>
    VsqHandle( TextMemoryStream sr, int value, ref string last_line ) {
        this.Index = value;
        string[] spl;
        string[] spl2;

        // default値で梅
        this.type( VsqHandleType.Vibrato );
        IconID = "";
        IDS = "normal";
        L0 = new Lyric( "" );
        Original = 0;
        Caption = "";
        Length = 0;
        StartDepth = 0;
        DepthBP = null;
        int depth_bp_num = 0;
        StartRate = 0;
        RateBP = null;
        int rate_bp_num = 0;
        m_language = 0;
        m_program = 0;

        string tmpDepthBPX = "";
        string tmpDepthBPY = "";
        string tmpRateBPX = "";
        string tmpRateBPY = "";

        // "["にぶち当たるまで読込む
        last_line = sr.ReadLine();
        while ( !last_line.StartsWith( "[" ) ) {
            spl = last_line.Split( new char[] { '=' } );
            switch ( spl[0] ) {
                case "Language":
                    m_language = int.Parse( spl[1] );
                    break;
                case "Program":
                    m_program = int.Parse( spl[1] );
                    break;
                case "IconID":
                    IconID = spl[1];
                    break;
                case "IDS":
                    IDS = spl[1];
                    break;
                case "Original":
                   Original = int.Parse( spl[1] );
                    break;
                case "Caption":
                    Caption = spl[1];
                    for ( int i = 2; i < spl.Length; i++ ) {
                        Caption += "=" + spl[i];
                    }
                    break;
                case "Length":
                    Length = int.Parse( spl[1] );
                    break;
                case "StartDepth":
                    StartDepth = int.Parse( spl[1] );
                    break;
                case "DepthBPNum":
                    depth_bp_num = int.Parse( spl[1] );
                    break;
                case "DepthBPX":
                    tmpDepthBPX = spl[1];
                    break;
                case "DepthBPY":
                    tmpDepthBPY = spl[1];
                    break;
                case "StartRate":
                    StartRate = int.Parse( spl[1] );
                    break;
                case "RateBPNum":
                    rate_bp_num = int.Parse( spl[1] );
                    break;
                case "RateBPX":
                    tmpRateBPX = spl[1];
                    break;
                case "RateBPY":
                    tmpRateBPY = spl[1];
                    break;
                case "L0":
                    m_type = VsqHandleType.Lyric;
                    L0 = new Lyric( spl[1] );
                    break;
            }
            if ( sr.Peek() < 0 ) {
                break;
            }
            last_line = sr.ReadLine();
        }
        if ( IDS != "normal" ) {
            type( VsqHandleType.Singer );
        } else if ( IconID != "" ) {
            type( VsqHandleType.Vibrato );
        } else {
            type( VsqHandleType.Lyric );
        }

        // RateBPX, RateBPYの設定
        if ( this.type() == VsqHandleType.Vibrato ) {
            if ( rate_bp_num > 0 ) {
                float[] rate_bp_x = new float[rate_bp_num];
                spl2 = tmpRateBPX.Split( new char[] { ',' } );
                for ( int i = 0; i < rate_bp_num; i++ ) {
                    rate_bp_x[i] = float.Parse( spl2[i] );
                }

                int[] rate_bp_y = new int[rate_bp_num];
                spl2 = tmpRateBPY.Split( new char[] { ',' } );
                for ( int i = 0; i < rate_bp_num; i++ ) {
                    rate_bp_y[i] = int.Parse( spl2[i] );
                }
                RateBP = new VibratoBPList( rate_bp_x, rate_bp_y );
            } else {
                //m_rate_bp_x = null;
                //m_rate_bp_y = null;
                RateBP = new VibratoBPList();
            }

            // DepthBPX, DepthBPYの設定
            if ( depth_bp_num > 0 ) {
                float[] depth_bp_x = new float[depth_bp_num];
                spl2 = tmpDepthBPX.Split( new char[] { ',' } );
                for ( int i = 0; i < depth_bp_num; i++ ) {
                    depth_bp_x[i] = float.Parse( spl2[i] );
                }

                int[] depth_bp_y = new int[depth_bp_num];
                spl2 = tmpDepthBPY.Split( new char[] { ',' } );
                for ( int i = 0; i < depth_bp_num; i++ ) {
                    depth_bp_y[i] = int.Parse( spl2[i] );
                }
                DepthBP = new VibratoBPList( depth_bp_x, depth_bp_y );
            } else {
                DepthBP = new VibratoBPList();
                //m_depth_bp_x = null;
                //m_depth_bp_y = null;
            }
        } else {
            DepthBP = new VibratoBPList();
            RateBP = new VibratoBPList();
        }
    }

    /// <summary>
    /// ハンドル指定子（例えば"h#0123"という文字列）からハンドル番号を取得します
    /// </summary>
    /// <param name="_string">ハンドル指定子</param>
    /// <returns>ハンドル番号</returns>
    static int HandleIndexFromString( string _string ) {
        string[] spl = _string.Split( new char[] { '#' } );
        return int.Parse( spl[1] );
    }


    /// <summary>
    /// インスタンスをテキストファイルに出力します
    /// </summary>
    /// <param name="sw">出力先</param>
    void Print( StreamWriter sw ) {
        string result = this.ToString();
        sw.WriteLine( result );
    }

    /// <summary>
    /// インスタンスを文字列に変換します
    /// </summary>
    /// <param name="encode">2バイト文字をエンコードするか否かを指定するフラグ</param>
    /// <returns>インスタンスを変換した文字列</returns>
    string ToString( bool encode ) {
        string result = "";
        result += "[h#" + Index.ToString( "0000" ) + "]";
        switch ( type() ) {
            case VsqHandleType.Lyric:
                result += Environment.NewLine + "L0=" + L0.ToString( encode );
                break;
            case VsqHandleType.Vibrato:
                result += Environment.NewLine + "IconID=" + IconID + Environment.NewLine;
                result += "IDS=" + IDS + Environment.NewLine;
                result += "Original=" + Original + Environment.NewLine;
                result += "Caption=" + Caption + Environment.NewLine;
                result += "Length=" + Length + Environment.NewLine;
                result += "StartDepth=" + StartDepth + Environment.NewLine;
                result += "DepthBPNum=" + DepthBP.Num() + Environment.NewLine;
                if ( DepthBP.Num() > 0 ) {
                    result += "DepthBPX=" + DepthBP.getElement( 0 ).X.ToString( "0.000000" );
                    for ( int i = 1; i < DepthBP.Num(); i++ ) {
                        result += "," + DepthBP.getElement( i ).X.ToString( "0.000000" );
                    }
                    result += Environment.NewLine + "DepthBPY=" + DepthBP.getElement( 0 ).Y;
                    for ( int i = 1; i < DepthBP.Num(); i++ ) {
                        result += "," + DepthBP.getElement( i ).Y;
                    }
                    result += Environment.NewLine;
                }
                result += "StartRate=" + StartRate + Environment.NewLine;
                result += "RateBPNum=" + RateBP.Num();
                if ( RateBP.Num() > 0 ) {
                    result += Environment.NewLine + "RateBPX=" + RateBP.getElement( 0 ).X.ToString( "0.000000" );
                    for ( int i = 1; i < RateBP.Num(); i++ ) {
                        result += "," + RateBP.getElement( i ).X.ToString( "0.000000" );
                    }
                    result += Environment.NewLine + "RateBPY=" + RateBP.getElement( 0 ).Y;
                    for ( int i = 1; i < RateBP.Num(); i++ ) {
                        result += "," + RateBP.getElement( i ).Y;
                    }
                }
                break;
            case VsqHandleType.Singer:
                result += Environment.NewLine + "IconID=" + IconID + Environment.NewLine;
                result += "IDS=" + IDS + Environment.NewLine;
                result += "Original=" + Original + Environment.NewLine;
                result += "Caption=" + Caption + Environment.NewLine;
                result += "Length=" + Length + Environment.NewLine;
                result += "Language=" + m_language + Environment.NewLine;
                result += "Program=" + m_program;
                break;
            default:
                break;
        }
        return result;
    }
};


class VsqID {
public:
    int value;
    VsqIDType type;
    int IconHandle_index;
    IconHandle IconHandle;
    int Length;
    int Note;
    int Dynamics;
    int PMBendDepth;
    int PMBendLength;
    int PMbPortamentoUse;
    int DEMdecGainRate;
    int DEMaccent;
    int LyricHandle_index;
    LyricHandle LyricHandle;
    int VibratoHandle_index;
    VibratoHandle VibratoHandle;
    int VibratoDelay;

    static VsqID EOS( -1 );

    /// <summary>
    /// IDの番号（ID#****の****）を指定したコンストラクタ。
    /// </summary>
    /// <param name="a_value">IDの番号</param>
    VsqID( int a_value ) {
        value = a_value;
    }


    /// <summary>
    /// テキストファイルからのコンストラクタ
    /// </summary>
    /// <param name="sr">読み込み対象</param>
    /// <param name="value"></param>
    /// <param name="last_line">読み込んだ最後の行が返されます</param>
    VsqID( TextMemoryStream sr, int value, string& last_line ) {
        string[] spl;
        this.value = value;
        this.type = VsqIDType.Unknown;
        this.IconHandle_index = -2;
        this.LyricHandle_index = -1;
        this.VibratoHandle_index = -1;
        this.Length = 0;
        this.Note = 0;
        this.Dynamics = 0;
        this.PMBendDepth = 0;
        this.PMBendLength = 0;
        this.PMbPortamentoUse = 0;
        this.DEMdecGainRate = 0;
        this.DEMaccent = 0;
        //this.LyricHandle_index = -2;
        //this.VibratoHandle_index = -2;
        this.VibratoDelay = 0;
        last_line = sr.ReadLine();
        while ( !last_line.StartsWith( "[" ) ) {
            spl = last_line.Split( new char[] { '=' } );
            switch ( spl[0] ) {
                case "Type":
                    if ( spl[1] == "Anote" ) {
                        type = VsqIDType.Anote;
                    } else if ( spl[1] == "Singer" ) {
                        type = VsqIDType.Singer;
                    } else {
                        type = VsqIDType.Unknown;
                    }
                    break;
                case "Length":
                    this.Length = int.Parse( spl[1] );
                    break;
                case "Note#":
                    this.Note = int.Parse( spl[1] );
                    break;
                case "Dynamics":
                    this.Dynamics = int.Parse( spl[1] );
                    break;
                case "PMBendDepth":
                    this.PMBendDepth = int.Parse( spl[1] );
                    break;
                case "PMBendLength":
                    this.PMBendLength = int.Parse( spl[1] );
                    break;
                case "DEMdecGainRate":
                    this.DEMdecGainRate = int.Parse( spl[1] );
                    break;
                case "DEMaccent":
                    this.DEMaccent = int.Parse( spl[1] );
                    break;
                case "LyricHandle":
                    this.LyricHandle_index = VsqHandle.HandleIndexFromString( spl[1] );
                    break;
                case "IconHandle":
                    this.IconHandle_index = VsqHandle.HandleIndexFromString( spl[1] );
                    break;
                case "VibratoHandle":
                    this.VibratoHandle_index = VsqHandle.HandleIndexFromString( spl[1] );
                    break;
                case "VibratoDelay":
                    this.VibratoDelay = int.Parse( spl[1] );
                    break;
                case "PMbPortamentoUse":
                    PMbPortamentoUse = int.Parse( spl[1] );
                    break;
            }
            if ( sr.Peek() < 0 ) {
                break;
            }
            last_line = sr.ReadLine();
        }
    }

    string ToString() {
        string ret = "{Type=" + type;
        switch ( type ) {
            case VsqIDType.Anote:
                ret += ", Length=" + Length;
                ret += ", Note#=" + Note;
                ret += ", Dynamics=" + Dynamics;
                ret += ", PMBendDepth=" + PMBendDepth ;
                ret += ", PMBendLength=" + PMBendLength ;
                ret += ", PMbPortamentoUse=" + PMbPortamentoUse ;
                ret += ", DEMdecGainRate=" + DEMdecGainRate ;
                ret += ", DEMaccent=" + DEMaccent ;
                if ( LyricHandle != null ) {
                    ret += ", LyricHandle=h#" + LyricHandle_index.ToString( "0000" ) ;
                }
                if ( VibratoHandle != null ) {
                    ret += ", VibratoHandle=h#" + VibratoHandle_index.ToString( "0000" );
                    ret += ", VibratoDelay=" + VibratoDelay ;
                }
                break;
            case VsqIDType.Singer:
                ret += ", IconHandle=h#" + IconHandle_index.ToString( "0000" );
                break;
        }
        ret += "}";
        return ret;
    }


    /// <summary>
    /// インスタンスをテキストファイルに出力します
    /// </summary>
    /// <param name="sw">出力先</param>
    void write( TextMemoryStream sw ) {
        sw.WriteLine( "[ID#" + value.ToString( "0000" ) + "]" );
        sw.WriteLine( "Type=" + type );
        switch( type ){
            case VsqIDType.Anote:
                sw.WriteLine( "Length=" + Length );
                sw.WriteLine( "Note#=" + Note );
                sw.WriteLine( "Dynamics=" + Dynamics );
                sw.WriteLine( "PMBendDepth=" + PMBendDepth );
                sw.WriteLine( "PMBendLength=" + PMBendLength );
                sw.WriteLine( "PMbPortamentoUse=" + PMbPortamentoUse );
                sw.WriteLine( "DEMdecGainRate=" + DEMdecGainRate );
                sw.WriteLine( "DEMaccent=" + DEMaccent );
                if ( LyricHandle != null ) {
                    sw.WriteLine( "LyricHandle=h#" + LyricHandle_index.ToString( "0000" ) );
                }
                if ( VibratoHandle != null ) {
                    sw.WriteLine( "VibratoHandle=h#" + VibratoHandle_index.ToString( "0000" ) );
                    sw.WriteLine( "VibratoDelay=" + VibratoDelay );
                }
                break;
            case VsqIDType.Singer:
                sw.WriteLine( "IconHandle=h#" + IconHandle_index.ToString( "0000" ) );
                break;
        }
    }
};

}
#endif // __libvsq_h__
