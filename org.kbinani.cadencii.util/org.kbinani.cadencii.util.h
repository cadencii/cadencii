/*
 * Copyright (C) 2003-2005 Kenji Aiko, 
 * based on http://ruffnex.oc.to/kenji/text/load_dll/ (browse: 13 Feb, 2010)
 */
#pragma once

#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <windowsx.h>
#include <tchar.h>
#include <winnt.h>
#include <stdio.h>
#include <vcclr.h>
#include <stdlib.h>

// DllMain時のアタッチ、デタッチ判定
#define DLL_ATTACH    0
#define DLL_DETACH    1

#define SIZE_OF_NT_SIGNATURE       (sizeof(DWORD))
#define SIZE_OF_PARAMETER_BLOCK    4096
#define IMAGE_PARAMETER_MAGIC      0xCDC31337

#define RVATOVA(base, offset) ( \
    (LPVOID)((DWORD)(base) + (DWORD)(offset)))

// NTシグネチャ
#define NTSIGNATURE(ptr) (  \
    (LPVOID)((PBYTE)(ptr) + \
    ((PIMAGE_DOS_HEADER)(ptr))->e_lfanew))

// PEヘッダオフセット
#define PEFHDROFFSET(ptr) ( \
    (LPVOID)((PBYTE)(ptr) + \
    ((PIMAGE_DOS_HEADER)(ptr))->e_lfanew + \
    SIZE_OF_NT_SIGNATURE))

// オプションヘッダオフセット
#define OPTHDROFFSET(ptr) ( \
    (LPVOID)((PBYTE)(ptr) + \
    ((PIMAGE_DOS_HEADER)(ptr))->e_lfanew + \
    SIZE_OF_NT_SIGNATURE +  \
    sizeof(IMAGE_FILE_HEADER)))

// セクションヘッダオフセット
#define SECHDROFFSET(ptr) ( \
    (LPVOID)((PBYTE)(ptr) + \
    ((PIMAGE_DOS_HEADER)(ptr))->e_lfanew + \
    SIZE_OF_NT_SIGNATURE +  \
    sizeof(IMAGE_FILE_HEADER) + \
    sizeof(IMAGE_OPTIONAL_HEADER)))

// 構造体の境界を1バイト設定
#pragma pack(push, 1)

typedef struct{
    DWORD dwPageRVA;
    DWORD dwBlockSize;
} IMAGE_FIXUP_BLOCK, *PIMAGE_FIXUP_BLOCK;

typedef struct{
    WORD offset:12;
    WORD type:4;
} IMAGE_FIXUP_ENTRY, *PIMAGE_FIXUP_ENTRY;

// DLLイメージデータの構造体
typedef struct __imageparameters{
    PVOID pImageBase;
    TCHAR svName[MAX_PATH];
    DWORD dwFlags;
    int nLockCount;
    struct __imageparameters *next;
} IMAGE_PARAMETERS, *PIMAGE_PARAMETERS;

#pragma pack(pop)

// DllMainのポインタ関数
typedef BOOL (WINAPI *DLLMAIN_T)(HMODULE, DWORD, LPVOID);

typedef struct{
    union{
        DWORD Function;
        DWORD Ordinal;
        DWORD AddressOfData;
    } u1;
} MY_IMAGE_THUNK_DATA, *PMY_IMAGE_THUNK_DATA;

// DLLデータベースのトップ
PIMAGE_PARAMETERS g_pImageParamHead;
// クリティカルセクション変数
CRITICAL_SECTION g_DLLCrit;
bool g_initialized = false;

using namespace System;
using namespace System::Runtime::InteropServices;

namespace org{ namespace kbinani{ namespace cadencii{ namespace util {

	public ref class DllLoad{
    public:
        // -------------------------------------------------------------
        // 初期化処理
        // -------------------------------------------------------------
        static void initialize(){
            if( g_initialized ){
                return;
            }
            InitializeCriticalSection( &g_DLLCrit );
            g_pImageParamHead = NULL;
            g_initialized = true;
        }

        static bool isInitialized(){
            return g_initialized;
        }

        // -------------------------------------------------------------
        // 終了処理
        // -------------------------------------------------------------
        static void terminate(){
            if( !g_initialized ) return;
            PIMAGE_PARAMETERS cur = g_pImageParamHead;

            while( cur != NULL ){
                PIMAGE_PARAMETERS next = cur->next;
                delete [] cur;
                cur = next;
            }

            DeleteCriticalSection( &g_DLLCrit );
        }


        // -------------------------------------------------------------
        // DLL内にあるエクスポート関数を検索する
        // 引数　：DLLハンドル、関数名
        // 戻り値：成功なら関数アドレス、失敗ならNULL
        // -------------------------------------------------------------
        static IntPtr getProcAddress( IntPtr ^hModule, 
                                         String ^lpProcName ){
            // hModuleがNULLならばエラー
			if( IntPtr::Zero == hModule ){
                return IntPtr::Zero;
            }
            
            // ディレクトリカウント取得
			PIMAGE_OPTIONAL_HEADER poh = (PIMAGE_OPTIONAL_HEADER)OPTHDROFFSET( hModule->ToPointer() );
            int nDirCount = poh->NumberOfRvaAndSizes;
            if( nDirCount < 16 ){
                return IntPtr::Zero;
            }

            // エクスポートディレクトリテーブル取得
            DWORD dwIDEE = IMAGE_DIRECTORY_ENTRY_EXPORT;
            if( poh->DataDirectory[dwIDEE].Size == 0 ){
                return IntPtr::Zero;
            }
            DWORD dwAddr = poh->DataDirectory[dwIDEE].VirtualAddress;
            PIMAGE_EXPORT_DIRECTORY ped = (PIMAGE_EXPORT_DIRECTORY)RVATOVA( hModule->ToInt32(), dwAddr );
            
            // 序数取得
			TCHAR char_proc_name[MAX_PATH];
			_stprintf_s( char_proc_name, _T( "%s" ), lpProcName );
            int nOrdinal = (LOWORD(char_proc_name)) - ped->Base;
            
            if( HIWORD( char_proc_name ) != 0 ){
                int count = ped->NumberOfNames;
                // 名前と序数を取得
                DWORD *pdwNamePtr = (PDWORD)RVATOVA( hModule->ToInt32(), ped->AddressOfNames );
                WORD *pwOrdinalPtr = (PWORD)RVATOVA( hModule->ToInt32(), ped->AddressOfNameOrdinals );
                // 関数検索
                int i;
                for( i = 0; i < count; i++, pdwNamePtr++, pwOrdinalPtr++ ){
                    PTCHAR svName = (PTCHAR)RVATOVA( hModule->ToInt32(), *pdwNamePtr );
                    if( lstrcmp(svName, char_proc_name ) == 0 ){
                        nOrdinal = *pwOrdinalPtr;
                        break;
                    }
                }
                // 見つからなければNULLを返却
                if( i == count ){
                    return IntPtr::Zero;
                }
            }
            
            // 発見した関数を返す
            PDWORD pAddrTable = (PDWORD)RVATOVA( hModule->ToInt32(), ped->AddressOfFunctions );
			IntPtr ret( RVATOVA( hModule->ToInt32(), pAddrTable[nOrdinal] ) );
            return ret;
        }

        // -------------------------------------------------------------
        // DLLをロードする関数
        // 引数　：DLLファイル名、予約語（NULL固定）、フラグ
        // 戻り値：成功DLLハンドル、失敗NULL
        // -------------------------------------------------------------
        static IntPtr loadDllEx( String^ lpLibFileName,
                            IntPtr^ hReserved,
                            DWORD dwFlags ){
            // 代替ファイル検索方法指定
            // （LOAD_WITH_ALTERED_SEARCH_PATH）はサポートしない
            if( dwFlags & LOAD_WITH_ALTERED_SEARCH_PATH ){
#ifdef _DEBUG
				Console::WriteLine( "DllLoad#LoadDllEx; error: not supported LOAD_WITH_ALTERED_PATH" );
#endif
                return IntPtr::Zero;
            }

            // DLLパス取得
            TCHAR szPath[MAX_PATH + 1], *szFilePart;

			const size_t newsize = 100;
			pin_ptr<const wchar_t> wch = PtrToStringChars( lpLibFileName );
#ifdef UNICODE
			wchar_t char_lib_file_name[newsize];
			wcscpy_s( char_lib_file_name, wch );
#else
			size_t origsize = wcslen(wch) + 1;
			size_t convertedChars = 0;
			char char_lib_file_name[newsize];
			wcstombs_s(&convertedChars, char_lib_file_name, origsize, wch, _TRUNCATE);
#endif

#ifdef _DEBUG
			Console::WriteLine( "DllLoad#LoadDllEx; lpLibFileName=" + lpLibFileName );
			Console::WriteLine( "DllLoad#LoadDllEx; char_lib_file_name=" + gcnew String( char_lib_file_name ) );
#endif
			int nLen = SearchPath( NULL, char_lib_file_name, _T( ".dll" ), MAX_PATH, szPath, &szFilePart );
            if( nLen == 0 ){
#ifdef _DEBUG
				Console::WriteLine( "DllLoad#LoadDllEx; SearchPath returns 0" );
#endif
                return IntPtr::Zero;
            }

            // ファイルマッピング
            HANDLE hFile = CreateFile( szPath,
                                       GENERIC_READ,
                                       FILE_SHARE_READ,
                                       NULL,
                                       OPEN_EXISTING,
                                       0,
                                       NULL );
            if( hFile == INVALID_HANDLE_VALUE ){
#ifdef _DEBUG
				Console::WriteLine( "DllLoad#LoadDllEx; CreateFile returns INVALID_HANDLE_VALUE" );
#endif
                return IntPtr::Zero;
            }
            HANDLE hMapping = CreateFileMapping( hFile,
                                                 NULL,
                                                 PAGE_READONLY,
                                                 0,
                                                 0,
                                                 NULL );
            CloseHandle( hFile );
            LPVOID pBaseAddr = MapViewOfFile( hMapping, FILE_MAP_READ, 0, 0, 0 );
            if( pBaseAddr == NULL ){
                CloseHandle( hMapping );
#ifdef _DEBUG
				Console::WriteLine( "DllLoad#LoadDllEx; MapViewOfFile returns NULL" );
#endif
				return IntPtr::Zero;
            }

            // DLLイメージの読み込み
            HMODULE hRet = LoadDllFromImage( pBaseAddr,
                                             szFilePart,
                                             dwFlags & ~LOAD_WITH_ALTERED_SEARCH_PATH );

            // ファイルマッピング解除
            UnmapViewOfFile( pBaseAddr );
            CloseHandle( hMapping );
			IntPtr ptr( hRet );
            return ptr;
        }


        // -------------------------------------------------------------
        // DLLをロードする関数（LoadDLLExへの橋渡し）
        // 引数　：DLLファイル名
        // 戻り値：成功DLLハンドル、失敗NULL
        // -------------------------------------------------------------
        static IntPtr loadDll( String^ lpLibFileName ){
			return loadDllEx( lpLibFileName, IntPtr::Zero, 0 );
        }


        // -------------------------------------------------------------
        // DLLを開放する関数
        // 引数　：DLLハンドル
        // 戻り値：成功TRUE、失敗FALSE
        // -------------------------------------------------------------
        static BOOL freeDll( IntPtr ^hLibModule ){
            // hLibModuleがNULLなら問題外
			if( hLibModule == IntPtr::Zero ){
                return FALSE;
            }
            
            // PEデータの識別
			PIMAGE_DOS_HEADER doshead = (PIMAGE_DOS_HEADER)hLibModule->ToPointer();
            if( doshead->e_magic != IMAGE_DOS_SIGNATURE ){
                return FALSE;
            }
			if( *(PDWORD)NTSIGNATURE(hLibModule->ToPointer()) != IMAGE_NT_SIGNATURE ){
                return FALSE;
            }
            PIMAGE_OPTIONAL_HEADER poh = (PIMAGE_OPTIONAL_HEADER)OPTHDROFFSET( hLibModule->ToPointer() );
            if( poh->Magic != 0x010B ){
                return FALSE;
            }

            DWORD dwFlags;
            TCHAR szName[MAX_PATH];
            // DLLデータベースからはずす
            int dllaction = RemoveDllReference( hLibModule->ToPointer(), szName, &dwFlags );
            if( dllaction == -1 ){
                return FALSE;
            }

            // DLLのデタッチ
            if( !(dwFlags & (LOAD_LIBRARY_AS_DATAFILE | DONT_RESOLVE_DLL_REFERENCES)) ){
                // カウンタが0（dllaction=1）ならばDLLをデタッチして終了
                if( dllaction ){
                    RunDllMain( hLibModule->ToPointer(), poh->SizeOfImage, DLL_DETACH );
                    return UnmapViewOfFile( hLibModule->ToPointer() );
                }
            }
            return TRUE;
        }

	private:
        // -------------------------------------------------------------
        // データベースに新しいDLLを追加
        // 引数　：DLLハンドル、DLL名（識別子）
        // 戻り値：error -1, success(find 0, make 1)
        // -------------------------------------------------------------
        static int AddDllReference( PVOID pImageBase, 
                             PTCHAR szName,
                             DWORD dwFlags ){
            // szNameがなければエラー
            if( szName == NULL ) return -1;

            EnterCriticalSection( &g_DLLCrit );

            PIMAGE_PARAMETERS cur = g_pImageParamHead;
            
            // DLLを検索
            while( cur != NULL ){
                if( cur->pImageBase != pImageBase ){
                    cur = cur->next;
                }else{
                    cur->nLockCount++;
                    LeaveCriticalSection( &g_DLLCrit );
                    return 0;
                }
            }
             
            // 新しいDLLの生成
            if( NULL == (cur = (PIMAGE_PARAMETERS)new IMAGE_PARAMETERS[1]) ){
                LeaveCriticalSection( &g_DLLCrit );
                return -1;
            }
            cur->pImageBase = pImageBase;
            cur->nLockCount = 1;
            cur->dwFlags    = dwFlags;
            cur->next       = g_pImageParamHead;
            lstrcpyn( cur->svName, szName, MAX_PATH );

            g_pImageParamHead = cur;

            LeaveCriticalSection( &g_DLLCrit );
            return 1;
        }


        // -------------------------------------------------------------
        // データベースからDLLを削除
        // 引数　：DLLハンドル、DLL名（識別子）
        // 戻り値：error -1, success(keep 0, delete 1)
        // -------------------------------------------------------------
        static int RemoveDllReference( PVOID pImageBase, 
                                PTCHAR svName,
                                PDWORD pdwFlags ){
            EnterCriticalSection( &g_DLLCrit );

            PIMAGE_PARAMETERS prev, cur = g_pImageParamHead;

            // DLLを検索
            while( cur != NULL ){
                if( cur->pImageBase == pImageBase ){
                    break;
                }
                prev = cur;
                cur = cur->next;
            }

            // 発見できなかったらエラー
            if( NULL == cur ){
                LeaveCriticalSection( &g_DLLCrit );
                return -1;
            }
            
            cur->nLockCount--;
            *pdwFlags = cur->dwFlags;
            lstrcpyn( svName, cur->svName, MAX_PATH );

            // カウンタがまだ0じゃないなら終了
            if( cur->nLockCount != 0 ){
                LeaveCriticalSection( &g_DLLCrit );
                return 0;
            }

            // 連結を更新
            if( NULL == prev ){
                g_pImageParamHead = g_pImageParamHead->next;
            }else{
                prev->next = cur->next;
            }

            delete [] cur;
            LeaveCriticalSection( &g_DLLCrit );
            return 1;
        }


        // -------------------------------------------------------------
        // パラメータテーブルからDLLを検索してそのハンドルを返す
        // 引数　：DLLファイル名
        // 戻り値：見つかればそのDLLのハンドル、見つからなければNULL
        // -------------------------------------------------------------
        static IntPtr^ GetDllHandle( PTCHAR svName ){
            if( NULL == svName ) return IntPtr::Zero;

            EnterCriticalSection( &g_DLLCrit );

            // パラーメータテーブルのトップを取得
            PIMAGE_PARAMETERS cur = g_pImageParamHead;
            
            // DLLを検索
            while( cur != NULL ){
                if( lstrcmpi( cur->svName, svName ) != 0 ){
                    cur = cur->next;
                }else{
                    // 見つかったらハンドルを返す
                    LeaveCriticalSection( &g_DLLCrit );
                    return gcnew IntPtr( cur->pImageBase );
                }
            }

            // 見つからなければ終了
            LeaveCriticalSection( &g_DLLCrit );
            return IntPtr::Zero;    
        }


        // -------------------------------------------------------------
        // パラメータテーブルからDLLを検索してそのファイル名を返す
        // 引数　：DLLハンドル、格納先ポインタ、格納領域のサイズ
        // 戻り値：見つかればファイル名のサイズ、見つからなければ0
        // -------------------------------------------------------------
        static DWORD GetDllFileName( HMODULE hModule, 
                              LPTSTR lpFileName, 
                              DWORD dwSize ){
            if( NULL == hModule || NULL == lpFileName || 0 == dwSize ){
                return 0;
            }
            
            // まずは通常のGetModuleFileNameで調べる
			DWORD dwRet = GetModuleFileName( hModule, lpFileName, dwSize );
            if( dwRet != 0 ){
                return dwRet;
            }

            EnterCriticalSection( &g_DLLCrit );

            PIMAGE_PARAMETERS cur = g_pImageParamHead;
            
            // DLLを検索
            while( cur != NULL ){
                if( cur->pImageBase != hModule ){
                    cur = cur->next;
                }else{
                    // 見つかったら文字列とサイズを返す
                    LeaveCriticalSection( &g_DLLCrit );
                    lstrcpyn( lpFileName, cur->svName, dwSize );
                    return lstrlen( lpFileName );
                }
            } 

            LeaveCriticalSection( &g_DLLCrit );
            return 0;
        }

		// -------------------------------------------------------------
        // DLLのDLLMain関数を走らせる関数
        // 引数　：DLLハンドル、DLLサイズ、Attach or Detachのフラグ
        // 戻り値：error -1, success(keep 0, delete 1)
        // -------------------------------------------------------------
        static BOOL RunDllMain( PVOID pImageBase, 
                                DWORD dwImageSize, 
                                BOOL bDetach ){
			// フラグの検査
            PIMAGE_FILE_HEADER pfh = (PIMAGE_FILE_HEADER)PEFHDROFFSET( pImageBase );
            if( (pfh->Characteristics & IMAGE_FILE_DLL) == 0 ){
                return TRUE;
            }

            // DLLMain関数のアドレス取得
            PIMAGE_OPTIONAL_HEADER poh = (PIMAGE_OPTIONAL_HEADER)OPTHDROFFSET( pImageBase );
			void* rawPtrMain = RVATOVA( pImageBase, poh->AddressOfEntryPoint );
			DLLMAIN_T pMain = (DLLMAIN_T)rawPtrMain;			
#ifdef _DEBUG
			Console::WriteLine( "DllLoad#RunDllMain; pMain=0x" + String::Format( "{0:X}", (int)pMain ) );
#endif
            // デタッチ時orアタッチ時
            if( bDetach ){
                return pMain( (HMODULE)pImageBase, DLL_PROCESS_DETACH, NULL );
            }else{
                return pMain( (HMODULE)pImageBase, DLL_PROCESS_ATTACH, NULL );
            }
        }


        // -------------------------------------------------------------
        // インポート関数のアドレス解決関数
        // 引数　：DLLファイルイメージ、DLLファイルイメージのサイズ
        // 戻り値：成功TRUE、失敗FALSE
        // -------------------------------------------------------------
        static BOOL PrepareDllImage( PVOID pMemoryImage, 
                              DWORD dwImageSize ){
            PIMAGE_OPTIONAL_HEADER poh = (PIMAGE_OPTIONAL_HEADER)OPTHDROFFSET( pMemoryImage );
            int nDirCount = poh->NumberOfRvaAndSizes;
            if( nDirCount < 16 ){
                return FALSE;
            }

            PIMAGE_SECTION_HEADER psh = (PIMAGE_SECTION_HEADER)SECHDROFFSET( pMemoryImage );

            DWORD dwIDEI = IMAGE_DIRECTORY_ENTRY_IMPORT;

            if( poh->DataDirectory[dwIDEI].Size != 0 ){
                PIMAGE_IMPORT_DESCRIPTOR pid = (PIMAGE_IMPORT_DESCRIPTOR)RVATOVA( pMemoryImage, 
                                                                                  poh->DataDirectory[dwIDEI].VirtualAddress );

                for( ; pid->OriginalFirstThunk != 0; pid++ ){
                    PTCHAR svDllName = (PTCHAR)RVATOVA( pMemoryImage, pid->Name );
                    HMODULE hDll = GetModuleHandle( svDllName );
                    if( hDll == NULL ){
                        if( (hDll = LoadLibrary( svDllName )) == NULL ){
                            return FALSE;
                        }
                    }

                    if( pid->TimeDateStamp != 0 ){
                        continue;
                    }
                    
                    pid->ForwarderChain = (DWORD)hDll;
                    pid->TimeDateStamp  = IMAGE_PARAMETER_MAGIC;

                    PMY_IMAGE_THUNK_DATA ptd_in = (PMY_IMAGE_THUNK_DATA)RVATOVA( pMemoryImage, pid->OriginalFirstThunk );
                    PMY_IMAGE_THUNK_DATA ptd_out = (PMY_IMAGE_THUNK_DATA)RVATOVA( pMemoryImage, pid->FirstThunk );
                        
                    for( ; ptd_in->u1.Function != NULL; ptd_in++, ptd_out++ ){
                        FARPROC func;
                        if( ptd_in->u1.Ordinal & 0x80000000 ){
                            func = GetProcAddress( hDll, MAKEINTRESOURCEA( ptd_in->u1.Ordinal ) );
                        }else{
                            PIMAGE_IMPORT_BY_NAME pibn = (PIMAGE_IMPORT_BY_NAME)RVATOVA( pMemoryImage, ptd_in->u1.AddressOfData );
                            func = GetProcAddress( hDll, (PCHAR)pibn->Name );
                        }
                        
                        if( func == NULL ){
                            return FALSE;
                        }
                            
                        ptd_out->u1.Function = (DWORD)func;
                    }
                }
            }

            DWORD dwIDEB = IMAGE_DIRECTORY_ENTRY_BASERELOC;
            DWORD delta = (DWORD)pMemoryImage - (DWORD)poh->ImageBase;

            if( (delta == 0) || (poh->DataDirectory[dwIDEB].Size == 0) ){
                return TRUE;
            }
            
            PIMAGE_FIXUP_BLOCK pfb = (PIMAGE_FIXUP_BLOCK)RVATOVA( pMemoryImage, poh->DataDirectory[dwIDEB].VirtualAddress );

            while( pfb->dwPageRVA != 0 ){
                
                int count = (pfb->dwBlockSize - sizeof( IMAGE_FIXUP_BLOCK )) / sizeof( IMAGE_FIXUP_ENTRY );
                PIMAGE_FIXUP_ENTRY pfe = (PIMAGE_FIXUP_ENTRY)((PTCHAR)pfb + sizeof( IMAGE_FIXUP_BLOCK ));

                for( int i = 0; i < count; i++, pfe++ ){
                    PVOID fixaddr = RVATOVA( pMemoryImage, pfb->dwPageRVA + pfe->offset );
                    
                    switch( pfe->type ){
                        case IMAGE_REL_BASED_ABSOLUTE:{
                            break;
                        }
                        case IMAGE_REL_BASED_HIGH:{
                            *((WORD *)fixaddr) += HIWORD( delta );
                            break;
                        }
                        case IMAGE_REL_BASED_LOW:{
                            *((WORD *)fixaddr) += LOWORD( delta );
                            break;
                        }
                        case IMAGE_REL_BASED_HIGHLOW:{
                            *((DWORD *)fixaddr) += delta;
                            break;
                        }
                        case IMAGE_REL_BASED_HIGHADJ:{
                            *((WORD *)fixaddr) = HIWORD( ((*((WORD *)fixaddr)) << 16) | (*(WORD *)(pfe+1))+ delta + 0x00008000);
                            pfe++;
                            break;
                        }
                        default:{
                            return FALSE;
                        }
                    }
                }

                pfb = (PIMAGE_FIXUP_BLOCK)((PTCHAR)pfb + pfb->dwBlockSize);
            }
            return TRUE;
        }


        // -------------------------------------------------------------
        // DLLイメージをプロテクトする
        // 引数　：DLLファイルイメージ
        // 戻り値：成功TRUE、失敗FALSE
        // -------------------------------------------------------------
        static BOOL ProtectDllImage( PVOID pMemoryImage ){
            // セクション数取得
            PIMAGE_FILE_HEADER pfh = (PIMAGE_FILE_HEADER)PEFHDROFFSET( pMemoryImage );
            int nSectionCount = pfh->NumberOfSections;

            // セクションヘッダ取得
            PIMAGE_SECTION_HEADER psh = (PIMAGE_SECTION_HEADER)SECHDROFFSET( pMemoryImage );

            for( int i = 0; i < nSectionCount; i++, psh++ ){

                // セクションアドレスとサイズの取得
                PVOID secMemAddr = (PTCHAR)RVATOVA( pMemoryImage, psh->VirtualAddress );
                
                DWORD chr = psh->Characteristics;
                // プロテクトフラグの設定
                BOOL bWrite  = (chr & IMAGE_SCN_MEM_WRITE)   ? TRUE : FALSE;
                BOOL bRead   = (chr & IMAGE_SCN_MEM_READ)    ? TRUE : FALSE;
                BOOL bExec   = (chr & IMAGE_SCN_MEM_EXECUTE) ? TRUE : FALSE;
                BOOL bShared = (chr & IMAGE_SCN_MEM_SHARED)  ? TRUE : FALSE;
                
                DWORD newProtect = 0;
                // フラグ整理
                if( bWrite && bRead && bExec && bShared ){
                    newProtect = PAGE_EXECUTE_READWRITE;
                }else if( bWrite && bRead && bExec ){
                    newProtect = PAGE_EXECUTE_WRITECOPY;
                }else if( bRead && bExec ){
                    newProtect = PAGE_EXECUTE_READ;
                }else if( bExec ){
                    newProtect = PAGE_EXECUTE;
                }else if( bWrite && bRead && bShared ){
                    newProtect = PAGE_READWRITE; 
                }else if( bWrite && bRead ){
                    newProtect = PAGE_WRITECOPY;
                }else if( bRead ){
                    newProtect = PAGE_READONLY;
                }

                if( chr & IMAGE_SCN_MEM_NOT_CACHED ){
                    newProtect |= PAGE_NOCACHE;
                }

                if( newProtect == 0 ){
                    return FALSE;
                }

                DWORD oldProtect;
                // プロテクト実行
                VirtualProtect( secMemAddr, psh->SizeOfRawData, newProtect, &oldProtect );
            }
            return TRUE;
        }


        // -------------------------------------------------------------
        // DLLイメージをコピーする関数
        // 引数　：DLLファイルイメージ、コピー先ポインタ
        // 戻り値：成功TRUE、失敗FALSE
        // -------------------------------------------------------------
        static BOOL MapDllFromImage( PVOID pDLLFileImage, 
                              PVOID pMemoryImage ){
            // PEヘッダとセクションヘッダをコピー
            PIMAGE_OPTIONAL_HEADER poh = (PIMAGE_OPTIONAL_HEADER)OPTHDROFFSET( pDLLFileImage );
            memcpy( pMemoryImage, pDLLFileImage, poh->SizeOfHeaders );

            // セクション数を取得
            PIMAGE_FILE_HEADER pfh = (PIMAGE_FILE_HEADER)PEFHDROFFSET( pDLLFileImage );
            int nSectionCount = pfh->NumberOfSections;

            // セクションヘッダポインタ取得
            PIMAGE_SECTION_HEADER psh = (PIMAGE_SECTION_HEADER)SECHDROFFSET( pDLLFileImage );

            // すべてのセクションのコピー
            for( int i = 0; i < nSectionCount; i++, psh++ ){
                PTCHAR secMemAddr  = (PTCHAR)((PTCHAR)pMemoryImage + psh->VirtualAddress);
                PTCHAR secFileAddr = (PTCHAR)((PTCHAR)pDLLFileImage + psh->PointerToRawData);
                int secLen = psh->SizeOfRawData;
                memcpy( secMemAddr, secFileAddr, secLen );
            }
            return TRUE;
        }


        // -------------------------------------------------------------
        // DLLイメージからDLLをロードする関数
        // 引数　：DLLファイルイメージ、マッピング名（識別子）、フラグ
        // 戻り値：成功DLLハンドル、失敗NULL
        // -------------------------------------------------------------
        static HMODULE LoadDllFromImage( LPVOID pDLLFileImage, 
                                  PTCHAR szMappingName,
                                  DWORD dwFlags ){
            // マッピング名がなければエラー
            if( szMappingName == NULL ){
                return NULL;
            }

            // マッピング名のサイズを判定
            if( lstrlen( szMappingName ) >= MAX_PATH ){
                return NULL;
            }
            
            // PEデータの判定
            PIMAGE_DOS_HEADER doshead = (PIMAGE_DOS_HEADER)pDLLFileImage;
            if( doshead->e_magic != IMAGE_DOS_SIGNATURE ){
                return NULL;
            }
            if( *(DWORD *)NTSIGNATURE( pDLLFileImage ) != IMAGE_NT_SIGNATURE ){
                return NULL;
            }
            PIMAGE_OPTIONAL_HEADER poh = (PIMAGE_OPTIONAL_HEADER)OPTHDROFFSET( pDLLFileImage );
            if( poh->Magic != 0x010B ){
                return NULL;
            }

            // セクション数取得
            PIMAGE_FILE_HEADER pfh = (PIMAGE_FILE_HEADER)PEFHDROFFSET( pDLLFileImage );
            int nSectionCount = pfh->NumberOfSections;

            DWORD pPreferredImageBase = poh->ImageBase;
            DWORD dwImageSize = poh->SizeOfImage;

            PVOID pImageBase;
            HANDLE hmapping = NULL;
            // DLLハンドルが見つからなければ新しく生成
            if( (pImageBase = GetDllHandle( szMappingName )->ToPointer() ) == NULL ){
                BOOL bCreated = FALSE;
                // すでにマッピングされているかどうか
                hmapping = OpenFileMapping( FILE_MAP_WRITE, TRUE, szMappingName );
                // されていないなら生成
                if( hmapping == NULL ){
                    hmapping = CreateFileMapping( INVALID_HANDLE_VALUE,
                                                  NULL,
                                                  PAGE_READWRITE,
                                                  0,
                                                  dwImageSize + SIZE_OF_PARAMETER_BLOCK,
                                                  szMappingName );
                    if( hmapping == NULL ){
                        return NULL;
                    }
                    bCreated = TRUE;
                }

                // マッピングされているデータの先頭をpImageBaseへ
                pImageBase = MapViewOfFileEx( hmapping,
                                              FILE_MAP_WRITE,
                                              0,
                                              0,
                                              0,
                                              (LPVOID)pPreferredImageBase );
                if( pImageBase == NULL ){
                    pImageBase = MapViewOfFileEx( hmapping,
                                                  FILE_MAP_WRITE,
                                                  0,
                                                  0,
                                                  0,
                                                  NULL );
                }
                CloseHandle( hmapping );
                if( pImageBase == NULL ){
                    return NULL;
                }

                // 新しく生成されたか、ベースアドレスが変わっていたら
                if( bCreated || (pImageBase != (LPVOID)pPreferredImageBase) ){
                    // DLLイメージをマッピング
                    if( ! MapDllFromImage( pDLLFileImage, pImageBase ) ){
                        UnmapViewOfFile( pImageBase );
                        return NULL;
                    }
                }
                
                // LOAD_LIBRARY_AS_DATAFILEが立ってないならば
                if( !(dwFlags & LOAD_LIBRARY_AS_DATAFILE) ){
                    // 
                    if( ! PrepareDllImage( pImageBase, dwImageSize ) ){
                        UnmapViewOfFile( pImageBase );
                        return NULL;
                    }
                    
                    // フラグにDONT_RESOLVE_DLL_REFERENCESが立ってなければ
                    if( !(dwFlags & DONT_RESOLVE_DLL_REFERENCES) ){
                        // DLLMainを実行（アタッチ）
                        if( !RunDllMain( pImageBase, dwImageSize, DLL_ATTACH ) ){
                            UnmapViewOfFile( pImageBase );
                            return NULL;
                        }
                    }

                    // プロテクトを実行
                    if( !ProtectDllImage( pImageBase ) ){
                        UnmapViewOfFile( pImageBase );
                        return NULL;
                    }
                }
            }
            
            // DLLデータベースへ追加
            if( AddDllReference( pImageBase, szMappingName, dwFlags ) == -1 ){
                if( hmapping != NULL ){
                    UnmapViewOfFile( pImageBase );
                }
                return NULL;
            }    

            return (HMODULE)pImageBase;    
        }
	};

} } } }
