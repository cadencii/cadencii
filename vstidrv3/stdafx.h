#include <windows.h>
#include <mmsystem.h>
#include <iomanip>
#include <string>
#include <iostream>
#include <fstream>
#include <climits>
#include <sstream>
#pragma comment(lib, "winmm.lib")
#pragma comment(lib, "user32.lib")

using namespace std;

#ifndef __stdafx_h__
#define __stdafx_h__

#ifdef __cplusplus_cli
#define __PFX_INTERFACE__ virtual
#define __PFX_MEMBER__
#else
#define __PFX_INTERFACE__ static
#define __PFX_MEMBER__ static
#endif

//#define USE_DOUBLE
//#define TEST

namespace Boare{ namespace Cadencii{

#ifdef __cplusplus_cli
    ref class debug{
    public:
        static System::IO::StreamWriter ^logger;
        static void push_log( System::String ^s );
    };
#else
    class debug{
    public:
        static ofstream logger;
        static void push_log( std::string s );
    };

#endif
} }
#endif // __stdafx_h__

using namespace Boare::Cadencii;
