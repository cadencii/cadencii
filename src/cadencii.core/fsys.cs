/*
 * fsys.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii;

import java.io.*;
#else
#if !__cplusplus
using System;
using System.IO;
using System.Collections.Generic;

#endif
    namespace cadencii
    {
#if !__cplusplus
        using int8_t = System.SByte;
        using int16_t = System.Int16;
        using int32_t = System.Int32;
        using int64_t = System.Int64;
        using uint8_t = System.Byte;
        using uint16_t = System.UInt16;
        using uint32_t = System.UInt32;
        using uint64_t = System.UInt64;
#endif
#endif

#if __cplusplus
        class fsys
#else
        public class fsys
#endif
        {
#if JAVA
            private static String mSeparator = "";
#elif __cplusplus
            private: static string mSeparator;
#else
            private static string mSeparator = "";
#endif

            private fsys()
            {
            }
#if __cplusplus
        public:
#endif

#if __cplusplus
            static string separator()
#else
            public static String separator()
#endif
            {
                if ( str.compare( mSeparator, "" ) ) {
#if JAVA
                    mSeparator = File.separator;
#elif __cplusplus
                    // requires "<direct.h>"
                    char *buf = new char[_MAX_PATH];
                    char *p = _getcwd( buf, _MAX_PATH );
                    if( p ){
                        string s = buf;
                        if( s.find( "\\" ) != string::npos ){
                            mSeparator = "\\";
                        }else{
                            mSeparator = "/";
                        }
                    }
                    delete [] buf;
#else
                    mSeparator = "" + Path.DirectorySeparatorChar;
#endif
                }
                return mSeparator;
            }

#if JAVA
            public static String combine( String path1, String path2 )
#elif __cplusplus
            static string combine( string path1, string path2 )
#else
            public static string combine( string path1, string path2 )
#endif
            {
#if !__cplusplus
                if ( path1 == null ) path1 = "";
                if ( path2 == null ) path2 = "";
#endif
#if DEBUG
#if !JAVA
                String ret2 = System.IO.Path.Combine( path1, path2 );
#endif
#endif
                separator();
                if ( str.endsWith( path1, mSeparator ) ) {
                    path1 = str.sub( path1, 0, str.length( path1 ) - 1 );
                }
                if ( str.startsWith( path2, mSeparator ) ) {
                    path2 = str.sub( path2, 1 );
                }
                return path1 + mSeparator + path2;
            }

#if JAVA
            public static boolean isDirectoryExists( String path )
#elif __cplusplus
            // requires <dirent.h>
            static bool isDirectoryExists( string path )
#else
            public static bool isDirectoryExists( string path )
#endif
            {
#if JAVA
                File f = new File( path );
                if( f.exists() ){
                    if( f.isFile() ){
                        return false;
                    }else{
                        return true;
                    }
                }else{
                    return false;
                }
#elif __cplusplus
                DIR *dir = opendir( path.c_str() );
                bool ret = (dir != NULL);
                if( ret ){
                    closedir( dir );
                }
                return ret;
#else
                return Directory.Exists( path );
#endif
            }

#if JAVA
            public static boolean isFileExists( String path )
#elif __cplusplus
            static bool isFileExists( string path )
#else
            public static bool isFileExists( string path )
#endif
            {
#if JAVA
                File f = new File( path );
                return f.isFile();
#elif __cplusplus
                // requires "#include <io.h>"
                return (access( path.c_str(), 0 ) == 0);
#else
                return File.Exists( path );
#endif
            }
        };

#if !JAVA
    }

// staticメンバーの実体の宣言
#if __cplusplus
std::string org::kbinani::vsq::fsys::mSeparator = "";
#endif

#endif
