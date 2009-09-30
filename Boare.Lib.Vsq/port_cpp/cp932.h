#ifndef __cp932_h__
#define __cp932_h__

#include <map>
#include <vector>
#include <sstream>

using namespace std;

wstring cp932_convert( vector<char> dat );
vector<char> cp932_convert( wstring str );

#endif // __cp932_h__
