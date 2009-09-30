#include "stdafx.h"

#ifdef __cplusplus_cli
using namespace System;
using namespace System::IO;
#endif

namespace Boare{ namespace Cadencii{

#ifndef __cplusplus_cli
    ofstream debug::logger;
#endif

#ifdef __cplusplus_cli
    void debug::push_log( System::String ^s ){
#ifdef TEST
        if ( logger == nullptr ){
            String ^dir = System::Windows::Forms::Application::StartupPath; //Path::Combine( Environment::GetFolderPath( Environment::SpecialFolder::ApplicationData ), "Cadencii" );
            String ^err = "";
            if ( !Directory::Exists( dir ) ) {
                try {
                    Directory::CreateDirectory( dir );
                } catch ( Exception ^ex ) {
                    err = ex->ToString();
                }
            }
            try {
                logger = gcnew StreamWriter( Path::Combine( dir, "err_vsti3.txt" ) );
                logger->AutoFlush = true;
                if ( err != "" ) { 
                    logger->WriteLine( "PushLog" );
                    logger->WriteLine( "    " + err );
                }
            } catch ( Exception ^ex) {
                logger = nullptr;
            }
        }
        if ( logger != nullptr ){
            System::Console::WriteLine( s );
            logger->WriteLine( s );
        }
#endif
    };
#else
    void debug::push_log( string s ){
#ifdef TEST
        if( !debug::logger.is_open() ){
            debug::logger.open( "err_vsti3.txt", ios::out );
        }
        debug::logger << s << std::endl;
#endif
    };
#endif

} }
