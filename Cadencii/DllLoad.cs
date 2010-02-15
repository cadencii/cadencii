#if ENABLE_VOCALOID
/*
 * DllLoad.cs
 * Copyright (C) 2010 kbinani
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
using System;
using System.Runtime.InteropServices;
using org.kbinani;

namespace org.kbinani.cadencii.implB {
    using WORD = System.UInt16;
    using DWORD = System.UInt32;
    using LONG = System.Int32;
    using BYTE = System.Byte;

    public unsafe class DllLoad {
        // DllMain時のアタッチ、デタッチ判定
        public const int DLL_ATTACH = 0;
        public const int DLL_DETACH = 1;

        public const int SIZE_OF_NT_SIGNATURE = 4;//       (sizeof(DWORD))
        public const int SIZE_OF_PARAMETER_BLOCK = 4096;
        public const uint IMAGE_PARAMETER_MAGIC = 0xCDC31337;

        private static WORD LOWORD( DWORD value ) {
            return (WORD)(0xffff & value);
        }

        private static WORD HIWORD( DWORD value ) {
            return (WORD)(0xffff & (value >> 16));
        }

        private static IntPtr RVATOVA( IntPtr base_, DWORD offset ) {
            return new IntPtr( base_.ToInt32() + offset );
        }

        // NTシグネチャ
        private static IntPtr NTSIGNATURE( IntPtr ptr ) {
            IMAGE_DOS_HEADER* pimg = (IMAGE_DOS_HEADER*)ptr.ToPointer();
            return new IntPtr( ptr.ToInt32() + pimg->e_lfanew );
        }

        // PEヘッダオフセット
        private static IntPtr PEFHDROFFSET( IntPtr ptr ) {
            IMAGE_DOS_HEADER* pimg = (IMAGE_DOS_HEADER*)ptr.ToPointer();
            return new IntPtr( ptr.ToInt32() + pimg->e_lfanew + SIZE_OF_NT_SIGNATURE );
            //(LPVOID)((PBYTE)(ptr) +  ((PIMAGE_DOS_HEADER)(ptr))->e_lfanew +  SIZE_OF_NT_SIGNATURE))
        }

        // オプションヘッダオフセット
        private static IntPtr OPTHDROFFSET( IntPtr ptr ) {
            IMAGE_DOS_HEADER* pimg = (IMAGE_DOS_HEADER*)ptr.ToPointer();
            return new IntPtr( ptr.ToInt32() + pimg->e_lfanew + SIZE_OF_NT_SIGNATURE + sizeof( IMAGE_FILE_HEADER ) );
            //(LPVOID)((PBYTE)(ptr) +     ((PIMAGE_DOS_HEADER)(ptr))->e_lfanew +     SIZE_OF_NT_SIGNATURE +      sizeof(IMAGE_FILE_HEADER)))
        }

        // セクションヘッダオフセット
        public static IntPtr SECHDROFFSET( IntPtr ptr ) {
            IMAGE_DOS_HEADER* pimg = (IMAGE_DOS_HEADER*)ptr.ToPointer();
            return new IntPtr( ptr.ToInt32() + pimg->e_lfanew + SIZE_OF_NT_SIGNATURE + sizeof( IMAGE_FILE_HEADER ) + sizeof( IMAGE_OPTIONAL_HEADER ) );
            //(LPVOID)((PBYTE)(ptr) +     ((PIMAGE_DOS_HEADER)(ptr))->e_lfanew +     SIZE_OF_NT_SIGNATURE +      sizeof(IMAGE_FILE_HEADER) +     sizeof(IMAGE_OPTIONAL_HEADER)))
        }

        // 構造体の境界を1バイト設定
        //#pragma pack(push, 1)
        [StructLayout( LayoutKind.Sequential )]
        private struct IMAGE_FIXUP_BLOCK {
            public DWORD dwPageRVA;
            public DWORD dwBlockSize;
        }

        private struct IMAGE_FIXUP_ENTRY {
            private WORD body;
            //WORD offset:12;
            //WORD type:4;
            public WORD getOffset() {
                return (WORD)(0xfff & body);
            }

            public WORD getType() {
                return (WORD)(0xf & (body >> 12));
            }
        }

        // DLLイメージデータの構造体
        [StructLayout( LayoutKind.Sequential )]
        private struct IMAGE_PARAMETERS {
            public void* pImageBase;
            public fixed byte svName[win32.MAX_PATH];
            public DWORD dwFlags;
            public int nLockCount;
            /// <summary>
            /// (IMAGE_PARAMETERS*)
            /// </summary>
            public IntPtr next;
        }

        //#pragma pack(pop)

        // DllMainのポインタ関数
        //typedef BOOL (WINAPI *DLLMAIN_T)(HMODULE, DWORD, LPVOID);
        delegate bool DLLMAIN_T( IntPtr hModule, DWORD d, void* lpvoid );

        /// <summary>
        /// DLLデータベースのトップ(IMAGE_PARAMETERS*)
        /// </summary>
        private static IntPtr g_pImageParamHead;
        /// <summary>
        /// クリティカルセクション変数(CRITICAL_SECTION)
        /// </summary>
        private static IntPtr g_DLLCrit;
        private static bool g_initialized = false;

        /// <summary>
        /// 初期化処理
        /// </summary>
        public static void InitializeDllLoad(){
            if( g_initialized ){
                return;
            }
            g_DLLCrit = Marshal.AllocHGlobal( sizeof( CRITICAL_SECTION ) );
            win32.InitializeCriticalSection( g_DLLCrit );
            g_pImageParamHead = IntPtr.Zero;
            g_initialized = true;
        }

        public static bool IsInitialized(){
            return g_initialized;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public static void KillDllLoad() {
            if ( !g_initialized ) {
                return;
            }
            // IMAGE_PARAMETERS*
            IntPtr cur = g_pImageParamHead;

            while ( cur != IntPtr.Zero ) {
                IntPtr next = ((IMAGE_PARAMETERS*)cur.ToPointer())->next;
                Marshal.FreeHGlobal( cur );
                cur = next;
            }
            win32.DeleteCriticalSection( g_DLLCrit );
        }

        /// <summary>
        /// データベースに新しいDLLを追加
        /// </summary>
        /// <param name="pImageBase">DLLハンドル</param>
        /// <param name="szName">DLL名（識別子）</param>
        /// <param name="dwFlags"></param>
        /// <returns>error -1, success(find 0, make 1)</returns>
        private static int AddDllReference( void* pImageBase, 
                             IntPtr szName,
                             DWORD dwFlags ){
            // szNameがなければエラー
            if ( szName == IntPtr.Zero ) {
                return -1;
            }

            win32.EnterCriticalSection( g_DLLCrit );

            // IMAGE_PARAMETERS*
            IntPtr cur = g_pImageParamHead;
    
            // DLLを検索
            while( cur != IntPtr.Zero ){
                if( ((IMAGE_PARAMETERS*)cur.ToPointer())->pImageBase != pImageBase ){
                    cur = ((IMAGE_PARAMETERS*)cur.ToPointer())->next;
                }else{
                    ((IMAGE_PARAMETERS*)cur.ToPointer())->nLockCount++;
                    win32.LeaveCriticalSection( g_DLLCrit );
                    return 0;
                }
            }
     
            // 新しいDLLの生成
            if( null == (cur = Marshal.AllocHGlobal( sizeof( IMAGE_PARAMETERS ) ) ) ){
                win32.LeaveCriticalSection( g_DLLCrit );
                return -1;
            }
            ((IMAGE_PARAMETERS*)cur.ToPointer())->pImageBase = pImageBase;
            ((IMAGE_PARAMETERS*)cur.ToPointer())->nLockCount = 1;
            ((IMAGE_PARAMETERS*)cur.ToPointer())->dwFlags    = dwFlags;
            ((IMAGE_PARAMETERS*)cur.ToPointer())->next       = g_pImageParamHead;
            lstrcpyn( szName, ((IMAGE_PARAMETERS*)cur.ToPointer())->svName, win32.MAX_PATH );

            g_pImageParamHead = cur;

            win32.LeaveCriticalSection( g_DLLCrit );
            return 1;
        }

        private static void lstrcpyn( IntPtr dest, byte* src, int length ) {
            byte[] arr = new byte[length];
            for ( int i = 0; i < length; i++ ) {
                arr[i] = src[i];
            }
            Marshal.Copy( arr, 0, dest, length );
        }

        /// <summary>
        /// データベースからDLLを削除
        /// </summary>
        /// <param name="pImageBase">DLLハンドル</param>
        /// <param name="svName">DLL名（識別子）</param>
        /// <param name="pdwFlags"></param>
        /// <returns>error -1, success(keep 0, delete 1)</returns>
        public static int RemoveDllReference( void* pImageBase,
                                IntPtr svName,
                                DWORD* pdwFlags ) {
            win32.EnterCriticalSection( g_DLLCrit );

            IntPtr prev = IntPtr.Zero;
            IntPtr cur = g_pImageParamHead;

            // DLLを検索
            while ( cur != IntPtr.Zero ) {
                if ( ((IMAGE_PARAMETERS*)cur.ToPointer())->pImageBase == pImageBase ) {
                    break;
                }
                prev = cur;
                cur = ((IMAGE_PARAMETERS*)cur.ToPointer())->next;
            }

            // 発見できなかったらエラー
            if ( IntPtr.Zero == cur ) {
                win32.LeaveCriticalSection( g_DLLCrit );
                return -1;
            }

            ((IMAGE_PARAMETERS*)cur.ToPointer())->nLockCount--;
            *pdwFlags = ((IMAGE_PARAMETERS*)cur.ToPointer())->dwFlags;
            lstrcpyn( svName, ((IMAGE_PARAMETERS*)cur.ToPointer())->svName, win32.MAX_PATH );

            // カウンタがまだ0じゃないなら終了
            if ( ((IMAGE_PARAMETERS*)cur.ToPointer())->nLockCount != 0 ) {
                win32.LeaveCriticalSection( g_DLLCrit );
                return 0;
            }

            // 連結を更新
            if ( IntPtr.Zero == prev ) {
                g_pImageParamHead = ((IMAGE_PARAMETERS*)g_pImageParamHead)->next;
            } else {
                ((IMAGE_PARAMETERS*)prev.ToPointer())->next = ((IMAGE_PARAMETERS*)cur.ToPointer())->next;
            }

            Marshal.FreeHGlobal( cur );
            win32.LeaveCriticalSection( g_DLLCrit );
            return 1;
        }

        /// <summary>
        /// パラメータテーブルからDLLを検索してそのハンドルを返す
        /// </summary>
        /// <param name="svName">DLLファイル名</param>
        /// <returns>見つかればそのDLLのハンドル、見つからなければNULL</returns>
        public static IntPtr GetDllHandle( string svName ){
            if ( null == svName || (svName != null && svName != "") ) {
                return IntPtr.Zero;
            }

            win32.EnterCriticalSection( g_DLLCrit );

            // パラーメータテーブルのトップを取得
            IntPtr cur = g_pImageParamHead;
    
            // DLLを検索
            while( cur != IntPtr.Zero ){
                string cur_svname = Marshal.PtrToStringAnsi( new IntPtr( (void*)((IMAGE_PARAMETERS*)cur.ToPointer())->svName ), win32.MAX_PATH );
                if( cur_svname != svName ){
                    cur = ((IMAGE_PARAMETERS*)cur.ToPointer())->next;
                }else{
                    // 見つかったらハンドルを返す
                    win32.LeaveCriticalSection( g_DLLCrit );
                    return new IntPtr( ((IMAGE_PARAMETERS*)cur.ToPointer())->pImageBase );
                }
            }

            // 見つからなければ終了
            win32.LeaveCriticalSection( g_DLLCrit );
            return IntPtr.Zero;
        }

        /// <summary>
        /// パラメータテーブルからDLLを検索してそのファイル名を返す
        /// </summary>
        /// <param name="hModule">DLLハンドル</param>
        /// <param name="lpFileName">格納先ポインタ</param>
        /// <param name="dwSize">格納領域のサイズ</param>
        /// <returns>見つかればファイル名のサイズ、見つからなければ0</returns>
        public static DWORD GetDllFileName( IntPtr hModule, 
                              IntPtr lpFileName, 
                              DWORD dwSize ){
            if ( IntPtr.Zero == hModule || IntPtr.Zero == lpFileName || 0 == dwSize ) {
                return 0;
            }
    
            // まずは通常のGetModuleFileNameで調べる
            DWORD dwRet = win32.GetModuleFileName( hModule, lpFileName, dwSize );
            if( dwRet != 0 ){
                return dwRet;
            }

            win32.EnterCriticalSection( g_DLLCrit );

            IntPtr cur = g_pImageParamHead;
    
            // DLLを検索
            while( cur != IntPtr.Zero ){
                if( new IntPtr( ((IMAGE_PARAMETERS*)cur.ToPointer())->pImageBase ) != hModule ){
                    cur = ((IMAGE_PARAMETERS*)cur.ToPointer())->next;
                }else{
                    // 見つかったら文字列とサイズを返す
                    win32.LeaveCriticalSection( g_DLLCrit );
                    lstrcpyn( lpFileName, ((IMAGE_PARAMETERS*)cur.ToPointer())->svName, (int)dwSize );
                    string str = Marshal.PtrToStringAnsi( lpFileName, win32.MAX_PATH );
                    return (DWORD)str.Length;
                }
            } 

            win32.LeaveCriticalSection( g_DLLCrit );
            return 0;
        }

        /// <summary>
        /// DLL内にあるエクスポート関数を検索する
        /// </summary>
        /// <param name="hModule">DLLハンドル</param>
        /// <param name="lpProcName">関数名</param>
        /// <returns>成功なら関数アドレス、失敗ならNULL</returns>
        public static IntPtr GetDllProcAddress( IntPtr hModule, 
                                                IntPtr lpProcName ){
            // hModuleがNULLならばエラー
            if( IntPtr.Zero == hModule ){
                return IntPtr.Zero;
            }
    
            // ディレクトリカウント取得
            IMAGE_OPTIONAL_HEADER *poh = (IMAGE_OPTIONAL_HEADER*)OPTHDROFFSET( hModule ).ToPointer();
            int nDirCount = (int)poh->NumberOfRvaAndSizes;
            if( nDirCount < 16 ){
                return IntPtr.Zero;
            }

            // エクスポートディレクトリテーブル取得
            DWORD dwIDEE = win32.IMAGE_DIRECTORY_ENTRY_EXPORT;
            if( poh->getDataDirectory( (int)dwIDEE ).Size == 0 ){
                return IntPtr.Zero;
            }
            DWORD dwAddr = poh->getDataDirectory( (int)dwIDEE ).VirtualAddress;
            IMAGE_EXPORT_DIRECTORY *ped = (IMAGE_EXPORT_DIRECTORY*)RVATOVA( hModule, dwAddr ).ToPointer();
    
            // 序数取得
            int nOrdinal = ((int)LOWORD( (DWORD)lpProcName.ToInt32() )) - (int)ped->Base;
    
            if( HIWORD( (DWORD)lpProcName .ToInt32() ) != 0 ){
                int count = (int)ped->NumberOfNames;
                // 名前と序数を取得
                DWORD *pdwNamePtr = (DWORD*)RVATOVA( hModule, ped->AddressOfNames );
                WORD *pwOrdinalPtr = (WORD*)RVATOVA( hModule, ped->AddressOfNameOrdinals );
                // 関数検索
                int i;
                for( i = 0; i < count; i++, pdwNamePtr++, pwOrdinalPtr++ ){
                    IntPtr svName = RVATOVA( hModule, *pdwNamePtr );
                    string str_sv_name = Marshal.PtrToStringAnsi( svName );
                    string str_lp_proc_name = Marshal.PtrToStringAnsi( lpProcName );
                    if( str_sv_name == str_lp_proc_name ){
                        nOrdinal = *pwOrdinalPtr;
                        break;
                    }
                }
                // 見つからなければNULLを返却
                if( i == count ){
                    return IntPtr.Zero;
                }
            }
    
            // 発見した関数を返す
            DWORD *pAddrTable = (DWORD*)RVATOVA( hModule, ped->AddressOfFunctions );
            return RVATOVA( hModule, pAddrTable[nOrdinal] );
        }
        /*
        /// <summary>
        /// DLLのDLLMain関数を走らせる関数
        /// </summary>
        /// <param name="pImageBase">DLLハンドル</param>
        /// <param name="dwImageSize">DLLサイズ</param>
        /// <param name="bDetach">Attach or Detachのフラグ</param>
        /// <returns>error -1, success(keep 0, delete 1)</returns>
        public static bool RunDllMain( IntPtr pImageBase, 
                                DWORD dwImageSize, 
                                bool bDetach ){
            // フラグの検査
            IMAGE_FILE_HEADER *pfh = (IMAGE_FILE_HEADER*)PEFHDROFFSET( pImageBase ).ToPointer();
            if( (pfh->Characteristics & win32.IMAGE_FILE_DLL) == 0 ){
                return true;
            }

            // DLLMain関数のアドレス取得
            //PIMAGE_OPTIONAL_HEADER
            IMAGE_OPTIONAL_HEADER* poh = (IMAGE_OPTIONAL_HEADER*)OPTHDROFFSET( pImageBase ).ToPointer();
            IMAGE_OPTIONAL_HEADER* ptr_main = (IMAGE_OPTIONAL_HEADER*)RVATOVA( pImageBase, ((IMAGE_OPTIONAL_HEADER*)poh.ToInt32())->AddressOfEntryPoint ).ToPointer();
            DLLMAIN_T pMain = (DLLMAIN_T)Marshal.GetDelegateForFunctionPointer( ptr_main, typeof( DLLMAIN_T ) );

            // デタッチ時orアタッチ時
            if( bDetach ){
                return pMain( pImageBase, win32.DLL_PROCESS_DETACH, (void*)0 );
            }else{
                return pMain( pImageBase, win32.DLL_PROCESS_ATTACH, (void*)0 );
            }
        }

        private struct MY_IMAGE_THUNK_DATA {
            public DWORD u1;

            public DWORD Function {
                get {
                    return u1;
                }
                set {
                    u1 = value;
                }
            }

            public DWORD Ordinal {
                get {
                    return u1;
                }
                set {
                    u1 = value;
                }
            }

            public DWORD AddressOfData {
                get {
                    return u1;
                }
                set {
                    u1 = value;
                }
            }
        }
        /*
        // -------------------------------------------------------------
        // インポート関数のアドレス解決関数
        // 引数　：DLLファイルイメージ、DLLファイルイメージのサイズ
        // 戻り値：成功TRUE、失敗FALSE
        // -------------------------------------------------------------
        public static bool PrepareDllImage( IntPtr pMemoryImage, 
                              DWORD dwImageSize ){
            IMAGE_OPTIONAL_HEADER* poh = (IMAGE_OPTIONAL_HEADER*)OPTHDROFFSET( pMemoryImage ).ToPointer();
            int nDirCount = (int)poh->NumberOfRvaAndSizes;
            if( nDirCount < 16 ){
                return false;
            }

            //PIMAGE_SECTION_HEADER
            IntPtr psh = SECHDROFFSET( pMemoryImage );

            DWORD dwIDEI = win32.IMAGE_DIRECTORY_ENTRY_IMPORT;

            if( poh->getDataDirectory( (int)dwIDEI ).Size != 0 ){
                IMAGE_IMPORT_DESCRIPTOR* pid = (IMAGE_IMPORT_DESCRIPTOR*)RVATOVA( pMemoryImage, 
                                                                                  (uint)poh->getDataDirectory( (int)dwIDEI ).VirtualAddress ).ToPointer();

                for( ; pid->OriginalFirstThunk != 0; pid++ ){
                    //PTCHAR
                    IntPtr svDllName = RVATOVA( pMemoryImage, pid->Name );
                    //HMODULE
                    IntPtr hDll = win32.GetModuleHandle( svDllName );
                    if( hDll == IntPtr.Zero ){
                        if( (hDll = win32.LoadLibrary( svDllName )) == null ){
                            return false;
                        }
                    }

                    if( pid->TimeDateStamp != 0 ){
                        continue;
                    }
            
                    pid->ForwarderChain = (DWORD)hDll;
                    pid->TimeDateStamp  = IMAGE_PARAMETER_MAGIC;

                    //PMY_IMAGE_THUNK_DATA 
                    MY_IMAGE_THUNK_DATA* ptd_in = (MY_IMAGE_THUNK_DATA*)RVATOVA( pMemoryImage, pid->OriginalFirstThunk ).ToPointer();
                    MY_IMAGE_THUNK_DATA* ptd_out = (MY_IMAGE_THUNK_DATA*)RVATOVA( pMemoryImage, pid->FirstThunk ).ToPointer();
                
                    for( ; ptd_in->u1.Function != NULL; ptd_in++, ptd_out++ ){
                        FARPROC func;
                        if( ptd_in->u1.Ordinal & 0x80000000 ){
                            func = GetProcAddress( hDll, MAKEINTRESOURCE( ptd_in->u1.Ordinal ) );
                        }else{
                            PIMAGE_IMPORT_BY_NAME pibn = (PIMAGE_IMPORT_BY_NAME)RVATOVA( pMemoryImage, ptd_in->u1.AddressOfData );
                            func = GetProcAddress( hDll, (PTCHAR)pibn->Name );
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

        /*

        // -------------------------------------------------------------
        // DLLイメージをプロテクトする
        // 引数　：DLLファイルイメージ
        // 戻り値：成功TRUE、失敗FALSE
        // -------------------------------------------------------------
        BOOL ProtectDllImage( PVOID pMemoryImage ){
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
        BOOL MapDllFromImage( PVOID pDLLFileImage, 
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
        HMODULE LoadDllFromImage( LPVOID pDLLFileImage, 
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
            if( (pImageBase = GetDllHandle( szMappingName )) == NULL ){
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


        // -------------------------------------------------------------
        // DLLをロードする関数
        // 引数　：DLLファイル名、予約語（NULL固定）、フラグ
        // 戻り値：成功DLLハンドル、失敗NULL
        // -------------------------------------------------------------
        #ifdef UNICODE
        HMODULE LoadDllExW( LPCWSTR lpLibFileName,
                            HANDLE hReserved,
                            DWORD dwFlags )
        #else
        HMODULE LoadDllExA( LPCSTR lpLibFileName,
                            HANDLE hReserved,
                            DWORD dwFlags )
        #endif
        {
            // 代替ファイル検索方法指定
            // （LOAD_WITH_ALTERED_SEARCH_PATH）はサポートしない
            if( dwFlags & LOAD_WITH_ALTERED_SEARCH_PATH ){
                return NULL;
            }

            // DLLパス取得
            TCHAR szPath[MAX_PATH + 1], *szFilePart;
            int nLen = SearchPath( NULL, lpLibFileName, ".dll", MAX_PATH, szPath, &szFilePart );
            if( nLen == 0 ){
                return NULL;
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
                return NULL;
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
                return NULL;
            }

            // DLLイメージの読み込み
            HMODULE hRet = LoadDllFromImage( pBaseAddr,
                                             szFilePart,
                                             dwFlags & ~LOAD_WITH_ALTERED_SEARCH_PATH );

            // ファイルマッピング解除
            UnmapViewOfFile( pBaseAddr );
            CloseHandle( hMapping );
            return hRet;
        }


        // -------------------------------------------------------------
        // DLLをロードする関数（LoadDLLExへの橋渡し）
        // 引数　：DLLファイル名
        // 戻り値：成功DLLハンドル、失敗NULL
        // -------------------------------------------------------------
        #ifdef UNICODE
        HMODULE LoadDllW( LPCWSTR lpLibFileName ){
            return LoadDllExW( lpLibFileName, NULL, 0 );
        }
        #else
        HMODULE LoadDllA( LPCSTR lpLibFileName ){
            return LoadDllExA( lpLibFileName, NULL, 0 );
        }
        #endif


        // -------------------------------------------------------------
        // DLLを開放する関数
        // 引数　：DLLハンドル
        // 戻り値：成功TRUE、失敗FALSE
        // -------------------------------------------------------------
        BOOL FreeDll( HMODULE hLibModule ){
            // hLibModuleがNULLなら問題外
            if( hLibModule == NULL ){
                return FALSE;
            }
    
            // PEデータの識別
            PIMAGE_DOS_HEADER doshead = (PIMAGE_DOS_HEADER)hLibModule;
            if( doshead->e_magic != IMAGE_DOS_SIGNATURE ){
                return FALSE;
            }
            if( *(PDWORD)NTSIGNATURE(hLibModule) != IMAGE_NT_SIGNATURE ){
                return FALSE;
            }
            PIMAGE_OPTIONAL_HEADER poh = (PIMAGE_OPTIONAL_HEADER)OPTHDROFFSET( hLibModule );
            if( poh->Magic != 0x010B ){
                return FALSE;
            }

            DWORD dwFlags;
            TCHAR szName[MAX_PATH];
            // DLLデータベースからはずす
            int dllaction = RemoveDllReference( hLibModule, szName, &dwFlags );
            if( dllaction == -1 ){
                return FALSE;
            }

            // DLLのデタッチ
            if( !(dwFlags & (LOAD_LIBRARY_AS_DATAFILE | DONT_RESOLVE_DLL_REFERENCES)) ){
                // カウンタが0（dllaction=1）ならばDLLをデタッチして終了
                if( dllaction ){
                    RunDllMain( hLibModule, poh->SizeOfImage, DLL_DETACH );
                    return UnmapViewOfFile( hLibModule );
                }
            }
            return TRUE;
        }
        #ifdef __cplusplus
        }
        #endif*/
    }

}

namespace org.kbinani.cadencii.implA{
    using boolean = System.Boolean;

    public class DllLoad {
        [DllImport( "util" )]
        private static extern boolean IsInitialized();
        [DllImport( "util" )]
        private static extern void InitializeDllLoad();
        [DllImport( "util" )]
        private static extern void KillDllLoad();
        [DllImport( "util" )]
        private static extern IntPtr GetDllProcAddress( IntPtr hModule, string lpProcName );
        [DllImport( "util" )]
        private static extern IntPtr LoadDllW( [MarshalAs( UnmanagedType.LPWStr )]string lpLibFileName );
        [DllImport( "util" )]
        private static extern IntPtr LoadDllA( [MarshalAs( UnmanagedType.LPStr )]string lpLibFileName );
        [DllImport( "util" )]
        private static extern boolean FreeDll( IntPtr hLibModule );

        private DllLoad(){
        }

        public static void terminate() {
            try {
                KillDllLoad();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "DllLoad#terminate; ex=" + ex );
            }
        }

        public static IntPtr getProcAddress( IntPtr hModule, string lpProcName ) {
            try {
                return GetDllProcAddress( hModule, lpProcName );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "DllLoad#getProcAddress; ex=" + ex );
            }
            return IntPtr.Zero;
        }

        public static boolean isInitialized() {
            try {
                return IsInitialized();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "DllLoad#isInitialized; ex=" + ex );
            }
            return false;
        }

        public static void initialize() {
            try {
                InitializeDllLoad();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "DllLoad#initialize; ex=" + ex );
            }
        }

        public static IntPtr loadDll( string lpLibFileName ) {
            IntPtr ret = IntPtr.Zero;
            try {
                ret = LoadDllA( lpLibFileName );
                return ret;
            } catch ( EntryPointNotFoundException ex ) {
                ret = IntPtr.Zero;
            } catch ( Exception ex1 ) {
                PortUtil.stderr.println( "DllLoad#loadDll; ex1=" + ex1 );
            }
            if ( ret == IntPtr.Zero ) {
                try {
                    ret = LoadDllW( lpLibFileName );
                } catch ( Exception ex ) {
                    ret = IntPtr.Zero;
                    PortUtil.stderr.println( "DllLoad#loadDll; ex=" + ex );
                }
            }
            return ret;
        }

        public static boolean freeDll( IntPtr hModule ) {
            try {
                return FreeDll( hModule );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "DllLoad#freeDll; ex=" + ex );
            }
            return false;
        }
    }

}
#endif