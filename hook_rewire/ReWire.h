#include <windows.h>
#include <stdio.h>

BOOL WINAPI DllEntryPoint(
    HINSTANCE hinstDLL,  // handle to DLL module
    DWORD fdwReason,     // reason for calling function
    LPVOID lpvReserved   // reserved
);

void load_entry_points();

typedef void (*_RWDCloseImp)();
typedef void (*_RWDComBytesAvailableImp)();
typedef void (*_RWDComCheckConnectionImp)();
typedef void (*_RWDComCreateImp)();
typedef void (*_RWDComDestroyImp)();
typedef void (*_RWDComDoesMessageFitImp)();
typedef void (*_RWDComReadImp)();
typedef void (*_RWDComSendImp)();
typedef void (*_RWDIsCloseOKImp)();
typedef void (*_RWDOpenImp)();
typedef void (*_RWIsReWireMixerAppRunningImp)();
typedef void (*_RWM2CloseDeviceImp)();
typedef void (*_RWM2CloseImp)();
typedef void (*_RWM2DriveAudioImp)();
typedef void (*_RWM2GetControllerInfoImp)();
typedef void (*_RWM2GetDeviceCountImp)();
typedef void (*_RWM2GetDeviceInfoByHandleImp)();
typedef void (*_RWM2GetDeviceInfoImp)();
typedef void (*_RWM2GetEventBusInfoImp)();
typedef void (*_RWM2GetEventChannelInfoImp)();
typedef void (*_RWM2GetEventInfoImp)();
typedef void (*_RWM2GetNoteInfoImp)();
typedef void (*_RWM2IdleImp)();
typedef void (*_RWM2IsCloseDeviceOKImp)();
typedef void (*_RWM2IsCloseOKImp)();
typedef void (*_RWM2IsPanelAppLaunchedImp)();
typedef void (*_RWM2LaunchPanelAppImp)();
typedef void (*_RWM2OpenDeviceImp)();
typedef void (*_RWM2OpenImp)();
typedef void (*_RWM2QuitPanelAppImp)();
typedef void (*_RWM2SetAudioInfoImp)();
typedef void (*_RWMCloseDeviceImp)();
typedef void (*_RWMCloseImp)();
typedef void (*_RWMDriveAudioImp)();
typedef void (*_RWMGetDeviceCountImp)();
typedef void (*_RWMGetDeviceInfoByHandleImp)();
typedef void (*_RWMGetDeviceInfoImp)();
typedef void (*_RWMIdleImp)();
typedef void (*_RWMIsCloseDeviceOKImp)();
typedef void (*_RWMIsCloseOKImp)();
typedef void (*_RWMOpenDeviceImp)();
typedef void (*_RWMOpenImp)();
typedef void (*_RWPCloseImp)();
typedef void (*_RWPComBytesAvailableImp)();
typedef void (*_RWPComCheckConnectionImp)();
typedef void (*_RWPComConnectImp)();
typedef void (*_RWPComDisconnectImp)();
typedef void (*_RWPComDoesMessageFitImp)();
typedef void (*_RWPComReadImp)();
typedef void (*_RWPComSendImp)();
typedef void (*_RWPIsCloseOKImp)();
typedef void (*_RWPLoadDeviceImp)();
typedef void (*_RWPOpenImp)();
typedef void (*_RWPRegisterDeviceImp)();
typedef void (*_RWPUnloadDeviceImp)();
typedef void (*_RWPUnregisterDeviceImp)();
typedef void (*_SmugglerDLL_UnitTest_AllocTestNames)();
typedef void (*_SmugglerDLL_UnitTest_FreeTestNames)();
typedef void (*_SmugglerDLL_UnitTest_RunTests)();
typedef void (*_TopHatCloseDeviceImp)();
typedef void (*_TopHatCloseImp)();
typedef void (*_TopHatDriveAudioImp)();
typedef void (*_TopHatGetDeviceCountImp)();
typedef void (*_TopHatGetDeviceInfoImp)();
typedef void (*_TopHatIdleImp)();
typedef void (*_TopHatIsCloseDeviceOKImp)();
typedef void (*_TopHatIsCloseOKImp)();
typedef void (*_TopHatOpenDeviceImp)();
typedef void (*_TopHatOpenImp)();
