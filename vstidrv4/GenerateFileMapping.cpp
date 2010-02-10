#include <windows.h>
#include <stdio.h>

void start_process( char *obj_name ){
    PROCESS_INFORMATION pi;
    STARTUPINFO si;

    ZeroMemory( &pi, sizeof( pi ) );
    ZeroMemory( &si, sizeof( si ) );

    si.cb = sizeof( si );
    si.dwFlags = STARTF_USESHOWWINDOW;
    si.wShowWindow = SW_SHOW;
    
    char *path = "E:\\Program Files\\Steinberg\\VSTplugins\\VOCALOID\\vocaloid.dll";
    char lpArg[260];
    sprintf( lpArg, "\".\\vstidrv4.exe\" \"%s\" \"%s\"", obj_name, path );
    CreateProcess( NULL, (LPTSTR)lpArg, NULL, NULL, FALSE, NORMAL_PRIORITY_CLASS | CREATE_NEW_CONSOLE, NULL, NULL, &si, &pi );
}

int main( int argc, char* argv[] ){
    if( argc != 2 ){
        return 0;
    }
    char *obj_name = argv[1];
    printf( "obj_name=%s\n", obj_name );
    HANDLE handle = CreateFileMapping( (HANDLE)-1, NULL, PAGE_READWRITE, 0, 1024, obj_name );
    if( GetLastError() == ERROR_ALREADY_EXISTS ){
        return 0;
    }

    for( int i = 0; i < 3; i++ ){
        start_process( obj_name );
        Sleep( 3000 );
    }

    char *ptr = (char*)MapViewOfFile( handle, FILE_MAP_ALL_ACCESS, 0, 0, 0 );
    char code = 0;
    while( 1 ){
        Sleep( 10 );
        ptr[0] = code;
        code++;
        printf( "\rcode=%d", code );
    }
    UnmapViewOfFile( ptr );
}
