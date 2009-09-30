#ifndef __common_h__
#define __common_h__

#include <vector>
#include <string>
#include <fstream>
#include <sstream>

using namespace std;

namespace Boare{ namespace Lib{ namespace Vsq{

typedef struct Color_t{
    int R;
    int G;
    int B;
} Color;

vector<string> split( string src, string delim );

int int_parse( string str );

typedef class StreamReader_t{
public:
        //StreamReader( Stream stream );
        StreamReader_t( string path );
        //StreamReader( Stream stream, bool detectEncodingFromByteOrderMarks );
        //StreamReader( Stream stream, Encoding encoding );
        //StreamReader( string path, bool detectEncodingFromByteOrderMarks );
        //StreamReader( string path, Encoding encoding );
        //StreamReader( Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks );
        //StreamReader( string path, Encoding encoding, bool detectEncodingFromByteOrderMarks );
        //StreamReader( Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize );
        //StreamReader( string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize );
        void Close();

        string ReadLine();
        
        int Peek();
private:
    ifstream m_ifs;
    static bool get_line( ifstream *ifs, string *str );
} StreamReader;

} } }
#endif
