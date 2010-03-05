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
        static void initialize();

		static bool isInitialized();

		// -------------------------------------------------------------
        // 終了処理
        // -------------------------------------------------------------
        static void terminate();

        // -------------------------------------------------------------
        // DLL内にあるエクスポート関数を検索する
        // 引数　：DLLハンドル、関数名
        // 戻り値：成功なら関数アドレス、失敗ならNULL
        // -------------------------------------------------------------
        static IntPtr getProcAddress( IntPtr hModule, String ^lpProcName );

        // -------------------------------------------------------------
        // DLLをロードする関数
        // 引数　：DLLファイル名、予約語（NULL固定）、フラグ
        // 戻り値：成功DLLハンドル、失敗NULL
        // -------------------------------------------------------------
        static IntPtr loadDllEx( String^ lpLibFileName,
                                 IntPtr hReserved,
                                 DWORD dwFlags );

        // -------------------------------------------------------------
        // DLLをロードする関数（LoadDLLExへの橋渡し）
        // 引数　：DLLファイル名
        // 戻り値：成功DLLハンドル、失敗NULL
        // -------------------------------------------------------------
        static IntPtr loadDll( String^ lpLibFileName );

        // -------------------------------------------------------------
        // DLLを開放する関数
        // 引数　：DLLハンドル
        // 戻り値：成功TRUE、失敗FALSE
        // -------------------------------------------------------------
        static BOOL freeDll( IntPtr hLibModule );

	private:
		static void lptstrFromString( String ^from, LPTSTR to );

		// -------------------------------------------------------------
        // データベースに新しいDLLを追加
        // 引数　：DLLハンドル、DLL名（識別子）
        // 戻り値：error -1, success(find 0, make 1)
        // -------------------------------------------------------------
        static int AddDllReference( PVOID pImageBase, 
                             PTCHAR szName,
                             DWORD dwFlags );

        // -------------------------------------------------------------
        // データベースからDLLを削除
        // 引数　：DLLハンドル、DLL名（識別子）
        // 戻り値：error -1, success(keep 0, delete 1)
        // -------------------------------------------------------------
        static int RemoveDllReference( PVOID pImageBase, 
                                PTCHAR svName,
                                PDWORD pdwFlags );

        // -------------------------------------------------------------
        // パラメータテーブルからDLLを検索してそのハンドルを返す
        // 引数　：DLLファイル名
        // 戻り値：見つかればそのDLLのハンドル、見つからなければNULL
        // -------------------------------------------------------------
        static IntPtr^ GetDllHandle( PTCHAR svName );

        // -------------------------------------------------------------
        // パラメータテーブルからDLLを検索してそのファイル名を返す
        // 引数　：DLLハンドル、格納先ポインタ、格納領域のサイズ
        // 戻り値：見つかればファイル名のサイズ、見つからなければ0
        // -------------------------------------------------------------
        static DWORD GetDllFileName( HMODULE hModule, 
                              LPTSTR lpFileName, 
                              DWORD dwSize );

		// -------------------------------------------------------------
        // DLLのDLLMain関数を走らせる関数
        // 引数　：DLLハンドル、DLLサイズ、Attach or Detachのフラグ
        // 戻り値：error -1, success(keep 0, delete 1)
        // -------------------------------------------------------------
        static BOOL RunDllMain( PVOID pImageBase, 
                                DWORD dwImageSize, 
                                BOOL bDetach );

        // -------------------------------------------------------------
        // インポート関数のアドレス解決関数
        // 引数　：DLLファイルイメージ、DLLファイルイメージのサイズ
        // 戻り値：成功TRUE、失敗FALSE
        // -------------------------------------------------------------
        static BOOL PrepareDllImage( PVOID pMemoryImage, 
                              DWORD dwImageSize );

		// -------------------------------------------------------------
        // DLLイメージをプロテクトする
        // 引数　：DLLファイルイメージ
        // 戻り値：成功TRUE、失敗FALSE
        // -------------------------------------------------------------
        static BOOL ProtectDllImage( PVOID pMemoryImage );

        // -------------------------------------------------------------
        // DLLイメージをコピーする関数
        // 引数　：DLLファイルイメージ、コピー先ポインタ
        // 戻り値：成功TRUE、失敗FALSE
        // -------------------------------------------------------------
        static BOOL MapDllFromImage( PVOID pDLLFileImage, 
                              PVOID pMemoryImage );

		// -------------------------------------------------------------
        // DLLイメージからDLLをロードする関数
        // 引数　：DLLファイルイメージ、マッピング名（識別子）、フラグ
        // 戻り値：成功DLLハンドル、失敗NULL
        // -------------------------------------------------------------
        static HMODULE LoadDllFromImage( LPVOID pDLLFileImage, 
                                  PTCHAR szMappingName,
                                  DWORD dwFlags );
	};

} } } }
