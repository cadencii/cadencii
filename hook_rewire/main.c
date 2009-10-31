#include <windows.h>
#include <stdio.h>
#include <malloc.h>

#define malloc_usable_size(a) _msize(a)

typedef int (*RWDEF_CLOSE_DEVICE)( int handle );
typedef void (*RWDEF_DRIVE_AUDIO)();
typedef void (*RWDEF_GET_DEVICE_INFO)();
typedef void (*RWDEF_GET_DEVICE_NAME_AND_VERSION)( int* version, char* name ); // OK
typedef void (*RWDEF_GET_EVENT_BUS_INFO)();
typedef void (*RWDEF_GET_EVENT_CHANNEL_INFO)();
typedef void (*RWDEF_GET_EVENT_CONTROLLER_INFO)();
typedef int (*RWDEF_GET_EVENT_INFO)( int i1 );
typedef void (*RWDEF_GET_EVENT_NOTE_INFO)();
typedef int (*RWDEF_IDLE)();                                                   // OK
typedef int (*RWDEF_IS_CLOSE_OK)();                                            // OK
typedef int (*RWDEF_IS_PANEL_APP_LAUNCHED)();                                  // OK
typedef int (*RWDEF_LAUNCH_PANEL_APP)();                                       // OK
typedef int (*RWDEF_OPEN_DEVICE)();                                            // OK
typedef int (*RWDEF_QUIT_PANEL_APP)();                                         // OK
typedef void (*RWDEF_SET_AUDIO_INFO)();

static int s_initialized = 0;
static RWDEF_CLOSE_DEVICE                rwdef_close_device = 0;
static RWDEF_DRIVE_AUDIO                 rwdef_drive_audio = 0;
static RWDEF_GET_DEVICE_INFO             rwdef_get_device_info = 0;
static RWDEF_GET_DEVICE_NAME_AND_VERSION rwdef_get_device_name_and_version = 0;
static RWDEF_GET_EVENT_BUS_INFO          rwdef_get_event_bus_info = 0;
static RWDEF_GET_EVENT_CHANNEL_INFO      rwdef_get_event_channel_info = 0;
static RWDEF_GET_EVENT_CONTROLLER_INFO   rwdef_get_event_controller_info = 0;
static RWDEF_GET_EVENT_INFO              rwdef_get_event_info = 0;
static RWDEF_GET_EVENT_NOTE_INFO         rwdef_get_event_note_info = 0;
static RWDEF_IDLE                        rwdef_idle = 0;
static RWDEF_IS_CLOSE_OK                 rwdef_is_close_ok = 0;
static RWDEF_IS_PANEL_APP_LAUNCHED       rwdef_is_panel_app_launched = 0;
static RWDEF_LAUNCH_PANEL_APP            rwdef_launch_panel_app = 0;
static RWDEF_OPEN_DEVICE                 rwdef_open_device = 0;
static RWDEF_QUIT_PANEL_APP              rwdef_quit_panel_app = 0;
static RWDEF_SET_AUDIO_INFO              rwdef_set_audio_info = 0;

FILE *logger = 0;

int main(){
    init();
}

int init(){
    HMODULE rewire = LoadLibraryA( "E:\\Program Files\\VOCALOID2\\_vocaloiddevice2.dll" );
    logger = fopen( "E:\\Program Files\\VOCALOID2\\hook_rewire.log", "w" );
    fprintf( logger, "hook rewire log\n" );
    fprintf( logger, "[-] : log info.\n" );
    fprintf( logger, "[<] : call from host\n" );
    fprintf( logger, "[>] : calling slave\n" );
    fprintf( logger, "------------------------------------------------------------------------------\n" );
    fprintf( logger, "[-] rewire=0x%X\n", rewire );

    rwdef_close_device                =                (RWDEF_CLOSE_DEVICE)GetProcAddress( rewire, "RWDEFCloseDevice" );
    rwdef_drive_audio                 =                 (RWDEF_DRIVE_AUDIO)GetProcAddress( rewire, "RWDEFDriveAudio" );
    rwdef_get_device_info             =             (RWDEF_GET_DEVICE_INFO)GetProcAddress( rewire, "RWDEFGetDeviceInfo" );
    rwdef_get_device_name_and_version = (RWDEF_GET_DEVICE_NAME_AND_VERSION)GetProcAddress( rewire, "RWDEFGetDeviceNameAndVersion" );
    rwdef_get_event_bus_info          =          (RWDEF_GET_EVENT_BUS_INFO)GetProcAddress( rewire, "RWDEFGetEventBusInfo" );
    rwdef_get_event_channel_info      =      (RWDEF_GET_EVENT_CHANNEL_INFO)GetProcAddress( rewire, "RWDEFGetEventChannelInfo" );
    rwdef_get_event_controller_info   =   (RWDEF_GET_EVENT_CONTROLLER_INFO)GetProcAddress( rewire, "RWDEFGetEventControllerInfo" );
    rwdef_get_event_info              =              (RWDEF_GET_EVENT_INFO)GetProcAddress( rewire, "RWDEFGetEventInfo" );
    rwdef_get_event_note_info         =         (RWDEF_GET_EVENT_NOTE_INFO)GetProcAddress( rewire, "RWDEFGetEventNoteInfo" );
    rwdef_idle                        =                        (RWDEF_IDLE)GetProcAddress( rewire, "RWDEFIdle" );
    rwdef_is_close_ok                 =                 (RWDEF_IS_CLOSE_OK)GetProcAddress( rewire, "RWDEFIsCloseOK" );
    rwdef_is_panel_app_launched       =       (RWDEF_IS_PANEL_APP_LAUNCHED)GetProcAddress( rewire, "RWDEFIsPanelAppLaunched" );
    rwdef_launch_panel_app            =            (RWDEF_LAUNCH_PANEL_APP)GetProcAddress( rewire, "RWDEFLaunchPanelApp" );
    rwdef_open_device                 =                 (RWDEF_OPEN_DEVICE)GetProcAddress( rewire, "RWDEFOpenDevice" );
    rwdef_quit_panel_app              =              (RWDEF_QUIT_PANEL_APP)GetProcAddress( rewire, "RWDEFQuitPanelApp" );
    rwdef_set_audio_info              =              (RWDEF_SET_AUDIO_INFO)GetProcAddress( rewire, "RWDEFSetAudioInfo" );

    s_initialized = 1;

    fprintf( logger, "[-] rwdef_close_device=0x%X\n", rwdef_close_device );
    fprintf( logger, "[-] rwdef_drive_audio=0x%X\n", rwdef_drive_audio );
    fprintf( logger, "[-] rwdef_get_device_info=0x%X\n", rwdef_get_device_info );
    fprintf( logger, "[-] rwdef_get_device_name_and_version=0x%X\n", rwdef_get_device_name_and_version );
    fprintf( logger, "[-] rwdef_get_event_bus_info=0x%X\n", rwdef_get_event_bus_info );
    fprintf( logger, "[-] rwdef_get_event_channel_info=0x%X\n", rwdef_get_event_channel_info );
    fprintf( logger, "[-] rwdef_get_event_controller_info=0x%X\n", rwdef_get_event_controller_info );
    fprintf( logger, "[-] rwdef_get_event_info=0x%X\n", rwdef_get_event_info );
    fprintf( logger, "[-] rwdef_get_event_note_info=0x%X\n", rwdef_get_event_note_info );
    fprintf( logger, "[-] rwdef_idle=0x%X\n", rwdef_idle );
    fprintf( logger, "[-] rwdef_is_close_ok=0x%X\n", rwdef_is_close_ok );
    fprintf( logger, "[-] rwdef_is_panel_app_launched=0x%X\n", rwdef_is_panel_app_launched );
    fprintf( logger, "[-] rwdef_launch_panel_app=0x%X\n", rwdef_launch_panel_app );
    fprintf( logger, "[-] rwdef_open_device=0x%X\n", rwdef_open_device );
    fprintf( logger, "[-] rwdef_quit_panel_app=0x%X\n", rwdef_quit_panel_app );
    fprintf( logger, "[-] rwdef_set_audio_info=0x%X\n", rwdef_set_audio_info );

    /*fprintf( logger, "[-]calling rwdef_get_device_name_and_version..." );
    char name[260] = "";
    int version;
    rwdef_get_device_name_and_version( &version, name );
    fprintf( logger, "[-]\nname=%s,version=%d\n", name, version );
    fprintf( logger, " done\n" );

    fprintf( logger, "[-]calling rwdef_launch_panel_app..." );
	int ret_rwdef_launch_panel_app = rwdef_launch_panel_app();
    fprintf( logger, " done(return=0x%X)\n", ret_rwdef_launch_panel_app );
	int i;
	for( i = 0; i < 5; i++ ){
        fprintf( logger, "[-]calling rwdef_idle..." );
        int ret_rwdef_idle = rwdef_idle();
        fprintf( logger, " done(return=0x%X)\n", ret_rwdef_idle );
		Sleep( 1000 );
	}
    
    fprintf( logger, "[-]calling rwdef_is_panel_app_launched..." );
    int ret_rwdef_is_panel_app_launched = rwdef_is_panel_app_launched();
    fprintf( logger, " done(return=0x%X)\n", ret_rwdef_is_panel_app_launched );
    
    fprintf( logger, "[-]calling rwdef_is_close_ok..." );
    int ret_rwdef_is_close_ok = rwdef_is_close_ok();
    fprintf( logger, " done(return=0x%X)\n", ret_rwdef_is_close_ok );
    
    fprintf( logger, "[-]calling rwdef_quit_panel_app..." );
    int ret_rwdef_quit_panel_app = rwdef_quit_panel_app();
    fprintf( logger, " done(return=0x%X)\n", ret_rwdef_quit_panel_app );*/
    return 0;
}

void RWDEFCloseDevice(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFCloseDevice\n" );
    fflush( logger );
}

void RWDEFDriveAudio(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFDriveAudio\n" );
    fflush( logger );
}

void RWDEFGetDeviceInfo(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFGetDeviceInfo\n" );
    fprintf( logger, "[>] calling RWDEFGetDeviceInfo..." );
    rwdef_get_device_info();
    fprintf( logger, " done\n" );
    fflush( logger );
}

void RWDEFGetDeviceNameAndVersion( int* version, char* name ){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFGetDeviceNameAndVersion\n" );
    fprintf( logger, "[-]   malloc_usable_size(name)=%d\n", malloc_usable_size( name ) );
    fprintf( logger, "[>] calling RWDEFGetDeviceNameAndVersion..." );
    rwdef_get_device_name_and_version( version, name );
    fprintf( logger, " done\n" );
    fprintf( logger, "[-]   version=%d, name=%s\n", *version, name );
    fflush( logger );
}

void RWDEFGetEventBusInfo(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFGetEventBusInfo\n" );
    fflush( logger );
}

void RWDEFGetEventChannelInfo(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFGetEventChannelInfo\n" );
    fflush( logger );
}

void RWDEFGetEventControllerInfo(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFGetEventControllerInfo\n" );
    fflush( logger );
}

int RWDEFGetEventInfo( int i1 ){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFGetEventInfo\n" );
    fprintf( logger, "[-]   i1=%d\n", i1 );
    fprintf( logger, "[>] calling RWDEFGetEventInfo..." );
    int ret = rwdef_get_event_info( i1 );
    fprintf( logger, " done\n" );
    fprintf( logger, "[-]   ret=%d\n", ret );
    fflush( logger );
    return ret;
}

void RWDEFGetEventNoteInfo(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFGetEventNoteInfo\n" );
    fflush( logger );
}

void RWDEFIdle(){
    if( !s_initialized ){
        init();
    }
    /*fprintf( logger, "[<] RWDEFIdle\n" );
    fprintf( logger, "[>] calling RWDEFIdle..." );*/
    rwdef_idle();
    /*fprintf( logger, " done\n" );
    fflush( logger );*/
}

int RWDEFIsCloseOK(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFIsCloseOK\n" );
    fprintf( logger, "[>] calling RWDEFIsCloseOK..." );
    int ret = rwdef_is_close_ok();
    fprintf( logger, " done\n" );
    fprintf( logger, "[-]   ret=%d\n", ret );
    fflush( logger );
    return ret;
}

int RWDEFIsPanelAppLaunched(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFIsPanelAppLaunched\n" );
    fprintf( logger, "[>] calling RWDEFIsPanelAppLaunched..." );
    int ret = rwdef_is_panel_app_launched();
    fprintf( logger, " done\n" );
    fprintf( logger, "[-]   ret=%d\n", ret );
    fflush( logger );
    return ret;
}

int RWDEFLaunchPanelApp(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFLaunchPanelApp\n" );
    fprintf( logger, "[>] calling RWDEFLaunchPanelApp..." );
    int ret = rwdef_launch_panel_app();
    fprintf( logger, " done\n" );
    fprintf( logger, "[-]   ret=%d\n", ret );
    fflush( logger );
    return ret;
}

int RWDEFOpenDevice(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFOpenDevice\n" );
    fprintf( logger, "[>] calling RWDEFOpenDevice..." );
    int ret = rwdef_open_device();
    fprintf( logger, " done\n" );
    fprintf( logger, "[-]   ret=%d\n", ret );
    fflush( logger );
    return ret;
}

int RWDEFQuitPanelApp(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFQuitPanelApp\n" );
    fprintf( logger, "[>] calling RWDEFQuitPanelApp..." );
    int ret = rwdef_quit_panel_app();
    fprintf( logger, " done\n" );
    fprintf( logger, "[-]   ret=%d\n", ret );
    fflush( logger );
  	return ret;
}

void RWDEFSetAudioInfo(){
    if( !s_initialized ){
        init();
    }
    fprintf( logger, "[<] RWDEFSetAudioInfo\n" );
}
