#ifndef __comhdr_h__
#define __comhdr_h__
#include <string>
#include <list>
#include <vector>
#include <sstream>
#include <fstream>
#include <map>
using namespace std;

#if __cplusplus
template<typename O, typename T> class Property{
public:
    Property( O *owner_, T (O::*get_)( void ), void (O::*set_)( T ) ) : 
        owner( owner_ ),
        getMethod( get_ ),
        setMethod( set_ ){
    }

    operator T(){
        return (owner->*getMethod)();
    }

    void operator=( const T value ){
        (owner->*setMethod)( value );
    }

private:
    O    *owner;
    T    (O::*getMethod)( void );
    void (O::*setMethod)( T );
};

template<typename O, typename T> class ReadOnlyProperty{
public:
    ReadOnlyProperty( O *owner_, T (O::*get_)( void ) ) :
        owner( owner_ ),
        getMethod( get_ ){
    }

    operator T(){
        return (owner->*getMethod)();
    }
 
private:
    O *owner;
    T (O::*getMethod)( void );
};

template<typename O, typename T> class WriteOnlyProperty{
public:
    WriteOnlyProperty( O *owner_, void (O::*set_)( T ) ) : 
        owner( owner_ ),
        setMethod( set_ ){
    }

    void operator=( const T value ){
        (owner->*setMethod)( value );
    }

private:
    O    *owner;
    void (O::*setMethod)( T );
};


#define null NULL
typedef unsigned char byte;
string util_string_from_array( vector<wchar_t> str ){
    string ret;
    for( int i = 0; i < str.size(); i++ ){
        ret += str[i];
    }
    return ret;
}

string util_string_from_array( vector<wchar_t> str, int start, int length ){
    string ret;
    for( int i = start; i < str.size() && i - start < length ; i++ ){
        ret += str[i];
    }
    return ret;
}

vector<wchar_t> util_string_to_array( string str ){
    std::vector<wchar_t> ret( str.size() );
    for( int i = 0; i < str.size(); i++ ){
        ret[i] = str[i];
    }
    return ret;
}

vector<string> util_split( string str, string delim ){
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

int util_parse_int( string str ){
    istringstream s( str );
    int ret = 0;
    s >> ret;
    return ret;
}
#endif

namespace Boare{ namespace System{ namespace IO{

class StreamReader{
public:
        //StreamReader( Stream stream );
        StreamReader( string path ){
            m_ifs.open( path.c_str() );
        }
        //StreamReader( Stream stream, bool detectEncodingFromByteOrderMarks );
        //StreamReader( Stream stream, Encoding encoding );
        //StreamReader( string path, bool detectEncodingFromByteOrderMarks );
        //StreamReader( string path, Encoding encoding );
        //StreamReader( Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks );
        //StreamReader( string path, Encoding encoding, bool detectEncodingFromByteOrderMarks );
        //StreamReader( Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize );
        //StreamReader( string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize );
        void Close(){
            m_ifs.close();
        }

        string ReadLine(){
            string ret;
            if( get_line( &m_ifs, &ret ) ){
                return ret;
            }else{
                return NULL;
            }
        }
        
        int Peek(){
            return m_ifs.peek();
        }
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
    }
};

} } }
#endif // __comhdr_h__
