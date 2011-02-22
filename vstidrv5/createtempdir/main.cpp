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
#include <windows.h>
#include <stdint.h>
#include <stdio.h>
#include <string>

using namespace std;

// 新しいディレクトリを一時ディレクトリ内に作成し，そのパスを標準出力に出力する
int main( int argc, char *argv[] )
{
    const int LEN = 1024;
    char short_temp_path[LEN];
    if( !GetTempPath( LEN, short_temp_path ) ){
        return 0;
    }
    char temp_path[LEN];
    if( !GetLongPathName( short_temp_path, temp_path, LEN ) ){
        return 0;
    }
    string str_temp_path = temp_path;
    char dirname[50];
    for( uint64_t i = 0; i < UINT64_MAX; i++ ){
        sprintf_s( dirname, 50 * sizeof( char ), "tmp%016X", i );
        string str_dirname = dirname;
        string str_ret = str_temp_path + str_dirname;
        if( CreateDirectory( str_ret.c_str(), NULL ) ){
            printf( str_ret.c_str() );
            break;
        }
    }
    return 0;
}
