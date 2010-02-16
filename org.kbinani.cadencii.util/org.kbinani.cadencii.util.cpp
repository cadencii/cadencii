// これは メイン DLL ファイルです。

#include "stdafx.h"

#include "org.kbinani.cadencii.util.h"

using namespace System;
using namespace org::kbinani::cadencii::util;

typedef int (*PADDFUNC)(int, int);
typedef int (*PSUBFUNC)(int, int);

int APIENTRY WinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{
	DllLoad::initialize();
	IntPtr hHandle = DllLoad::loadDll( "test_dll" );
	PADDFUNC pAdd = (PADDFUNC)DllLoad::getProcAddress(hHandle, _T("add_num")).ToPointer();
	int a = (*pAdd)(3, 5);  // 3 + 5 = 8
	/*PSUBFUNC pSub = (PSUBFUNC)DllLoad::getProcAddress(
		GetDLLHandle(_T("test_dll.dll")), _T("sub_num"));
	int b = (*pSub)(8, 5);  // 8 - 5 = 3
	TCHAR szFileName[MAX_PATH];
 	GetDLLFileName(hHandle, szFileName, sizeof(szFileName));
	TCHAR szBuffer[1024];
	wsprintf(szBuffer, _T(
		"FileName = %s\r\na = %d, b = %d\r\n"), szFileName, a, b);
	MessageBox(GetActiveWindow(), szBuffer, _T("Message"), MB_OK);*/
	DllLoad::freeDll(hHandle);
	DllLoad::terminate();
	return 0;
}
