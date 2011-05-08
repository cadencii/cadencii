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
#if defined( _DEBUG ) && defined( WIN32 )
#define _CRTDBG_MAP_ALLOC
#include <stdlib.h>
#include <crtdbg.h>
#endif

#include <windows.h>
#include <stdio.h>

void print( HKEY, TCHAR *, FILE * );

int main( int argc, TCHAR *argv[] ){
#if defined( _DEBUG ) && defined( WIN32 )
	_CrtSetDbgFlag( _CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF );
#endif
    if( argc < 2 ){
	    printf( "error; too few argument\n" );
        return 1;
    }
    FILE *fp = fopen( argv[1], "w" );
    HKEY v1;
    long ret = RegOpenKeyEx( HKEY_LOCAL_MACHINE, TEXT( "SOFTWARE\\VOCALOID" ), 0, KEY_READ, &v1 );
    if( ret == ERROR_SUCCESS ){
        print( v1, "HKLM\\SOFTWARE\\VOCALOID", fp );
        RegCloseKey( v1 );
    }

    HKEY v2;
    ret = RegOpenKeyEx( HKEY_LOCAL_MACHINE, TEXT( "SOFTWARE\\VOCALOID2" ), 0, KEY_READ, &v2 );
    if( ret == ERROR_SUCCESS ){
        print( v2, "HKLM\\SOFTWARE\\VOCALOID2", fp );
    }
    fclose( fp );
}

// レジストリkey内の値を再帰的に検索し、ファイルfpに順次出力する
void print( HKEY key, TCHAR *parent_name, FILE *fp ){
    DWORD i;
    for( i = 0; ; i++ ){
        TCHAR key_name[MAX_PATH];
        key_name[0] = 0;
        DWORD key_name_size = MAX_PATH;
        FILETIME ft;
        LONG ret = RegEnumKeyEx( key, i, key_name, &key_name_size, NULL, NULL, NULL, &ft );
        if( ret != ERROR_SUCCESS ){
            break;
        }
        HKEY key_child;
        ret = RegOpenKeyEx( key, key_name, 0, KEY_READ, &key_child );
        if( ret != ERROR_SUCCESS ){
            RegCloseKey( key_child );
            continue;
        }
        TCHAR next_parent[MAX_PATH];
        next_parent[0] = 0;
        strcat( next_parent, parent_name );
        strcat( next_parent, TEXT("\\") );
        strcat( next_parent, key_name );
        print( key_child, next_parent, fp );
        RegCloseKey( key_child );
    }
    for( i = 0; ; i++ ){
        TCHAR value_name[MAX_PATH];
        value_name[0] = 0;
        DWORD value_name_size = MAX_PATH;
        DWORD value_type = REG_SZ;
        BYTE value[MAX_PATH];
        value[0] = 0;
        DWORD value_size = MAX_PATH;
        LONG ret = RegEnumValue( key, i, value_name, &value_name_size, NULL, &value_type, value, &value_size );
        if( ret != ERROR_SUCCESS ){
            break;
        }
        if( value_type == REG_SZ ){
            fprintf( fp, "%s\t%s\t%s\n", parent_name, value_name, (char *)value );
        }else{
            //fprintf( fp, "%s\\%s\n", parent_name, value_name );
        }
    }
}
