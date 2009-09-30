#include <windows.h>
#include <stdio.h>

typedef int (*RWDEF_CLOSE_DEVICE)( int handle );
typedef void (*RWDEF_DRIVE_AUDIO)();
typedef void (*RWDEF_GET_DEVICE_INFO)();
typedef void (*RWDEF_GET_DEVICE_NAME_AND_VERSION)( int* version, char* name ); // OK
typedef void (*RWDEF_GET_EVENT_BUS_INFO)();
typedef void (*RWDEF_GET_EVENT_CHANNEL_INFO)();
typedef void (*RWDEF_GET_EVENT_CONTROLLER_INFO)();
typedef void (*RWDEF_GET_EVENT_INFO)();
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

int main(){
    init();
}

int init(){
    HMODULE rewire = LoadLibraryA( "_vocaloiddevice2.dll" );
    printf( "rewire=0x%X\n", rewire );

    RWDEF_CLOSE_DEVICE                rwdef_close_device                =                (RWDEF_CLOSE_DEVICE)GetProcAddress( rewire, "RWDEFCloseDevice" );
    RWDEF_DRIVE_AUDIO                 rwdef_drive_audio                 =                 (RWDEF_DRIVE_AUDIO)GetProcAddress( rewire, "RWDEFDriveAudio" );
    RWDEF_GET_DEVICE_INFO             rwdef_get_device_info             =             (RWDEF_GET_DEVICE_INFO)GetProcAddress( rewire, "RWDEFGetDeviceInfo" );
    RWDEF_GET_DEVICE_NAME_AND_VERSION rwdef_get_device_name_and_version = (RWDEF_GET_DEVICE_NAME_AND_VERSION)GetProcAddress( rewire, "RWDEFGetDeviceNameAndVersion" );
    RWDEF_GET_EVENT_BUS_INFO          rwdef_get_event_bus_info          =          (RWDEF_GET_EVENT_BUS_INFO)GetProcAddress( rewire, "RWDEFGetEventBusInfo" );
    RWDEF_GET_EVENT_CHANNEL_INFO      rwdef_get_event_channel_info      =      (RWDEF_GET_EVENT_CHANNEL_INFO)GetProcAddress( rewire, "RWDEFGetEventChannelInfo" );
    RWDEF_GET_EVENT_CONTROLLER_INFO   rwdef_get_event_controller_info   =   (RWDEF_GET_EVENT_CONTROLLER_INFO)GetProcAddress( rewire, "RWDEFGetEventControllerInfo" );
    RWDEF_GET_EVENT_INFO              rwdef_get_event_info              =              (RWDEF_GET_EVENT_INFO)GetProcAddress( rewire, "RWDEFGetEventInfo" );
    RWDEF_GET_EVENT_NOTE_INFO         rwdef_get_event_note_info         =         (RWDEF_GET_EVENT_NOTE_INFO)GetProcAddress( rewire, "RWDEFGetEventNoteInfo" );
    RWDEF_IDLE                        rwdef_idle                        =                        (RWDEF_IDLE)GetProcAddress( rewire, "RWDEFIdle" );
    RWDEF_IS_CLOSE_OK                 rwdef_is_close_ok                 =                 (RWDEF_IS_CLOSE_OK)GetProcAddress( rewire, "RWDEFIsCloseOK" );
    RWDEF_IS_PANEL_APP_LAUNCHED       rwdef_is_panel_app_launched       =       (RWDEF_IS_PANEL_APP_LAUNCHED)GetProcAddress( rewire, "RWDEFIsPanelAppLaunched" );
    RWDEF_LAUNCH_PANEL_APP            rwdef_launch_panel_app            =            (RWDEF_LAUNCH_PANEL_APP)GetProcAddress( rewire, "RWDEFLaunchPanelApp" );
    RWDEF_OPEN_DEVICE                 rwdef_open_device                 =                 (RWDEF_OPEN_DEVICE)GetProcAddress( rewire, "RWDEFOpenDevice" );
    RWDEF_QUIT_PANEL_APP              rwdef_quit_panel_app              =              (RWDEF_QUIT_PANEL_APP)GetProcAddress( rewire, "RWDEFQuitPanelApp" );
    RWDEF_SET_AUDIO_INFO              rwdef_set_audio_info              =              (RWDEF_SET_AUDIO_INFO)GetProcAddress( rewire, "RWDEFSetAudioInfo" );

    s_initialized = 1;

    printf( "rwdef_close_device=0x%X\n", rwdef_close_device );
    printf( "rwdef_drive_audio=0x%X\n", rwdef_drive_audio );
    printf( "rwdef_get_device_info=0x%X\n", rwdef_get_device_info );
    printf( "rwdef_get_device_name_and_version=0x%X\n", rwdef_get_device_name_and_version );
    printf( "rwdef_get_event_bus_info=0x%X\n", rwdef_get_event_bus_info );
    printf( "rwdef_get_event_channel_info=0x%X\n", rwdef_get_event_channel_info );
    printf( "rwdef_get_event_controller_info=0x%X\n", rwdef_get_event_controller_info );
    printf( "rwdef_get_event_info=0x%X\n", rwdef_get_event_info );
    printf( "rwdef_get_event_note_info=0x%X\n", rwdef_get_event_note_info );
    printf( "rwdef_idle=0x%X\n", rwdef_idle );
    printf( "rwdef_is_close_ok=0x%X\n", rwdef_is_close_ok );
    printf( "rwdef_is_panel_app_launched=0x%X\n", rwdef_is_panel_app_launched );
    printf( "rwdef_launch_panel_app=0x%X\n", rwdef_launch_panel_app );
    printf( "rwdef_open_device=0x%X\n", rwdef_open_device );
    printf( "rwdef_quit_panel_app=0x%X\n", rwdef_quit_panel_app );
    printf( "rwdef_set_audio_info=0x%X\n", rwdef_set_audio_info );

    printf( "calling rwdef_open_device..." );
	int handle = rwdef_open_device();
    printf( " done(return=0x%X)\n", handle );

    printf( "calling rwdef_get_device_name_and_version..." );
    char name[260] = "";
    int version;
    rwdef_get_device_name_and_version( &version, name );
    printf( "\nname=%s,version=%d\n", name, version );
    printf( " done\n" );

    printf( "calling rwdef_launch_panel_app..." );
	int ret_rwdef_launch_panel_app = rwdef_launch_panel_app();
    printf( " done(return=0x%X)\n", ret_rwdef_launch_panel_app );
	int i;
	for( i = 0; i < 5; i++ ){
        printf( "calling rwdef_idle..." );
        int ret_rwdef_idle = rwdef_idle();
        printf( " done(return=0x%X)\n", ret_rwdef_idle );
		Sleep( 1000 );
	}
    
    printf( "calling rwdef_is_panel_app_launched..." );
    int ret_rwdef_is_panel_app_launched = rwdef_is_panel_app_launched();
    printf( " done(return=0x%X)\n", ret_rwdef_is_panel_app_launched );
    
    printf( "calling rwdef_is_close_ok..." );
    int ret_rwdef_is_close_ok = rwdef_is_close_ok();
    printf( " done(return=0x%X)\n", ret_rwdef_is_close_ok );
    
    printf( "calling rwdef_quit_panel_app..." );
    int ret_rwdef_quit_panel_app = rwdef_quit_panel_app();
    printf( " done(return=0x%X)\n", ret_rwdef_quit_panel_app );

    printf( "calling rwdef_close_device..." );
    int ret_rwdef_close_device = rwdef_close_device( handle );
    printf( " done(return=0x%X)\n", ret_rwdef_close_device );
    return 0;
}

void RWDEFCloseDevice(){
    if( !s_initialized ){
        init();
    }
}

void RWDEFDriveAudio(){
    if( !s_initialized ){
        init();
    }
}

void RWDEFGetDeviceInfo(){
    if( !s_initialized ){
        init();
    }
}

void RWDEFGetDeviceNameAndVersion( int* version, char* name ){
    if( !s_initialized ){
        init();
    }
    rwdef_get_device_name_and_version( version, name );
}

void RWDEFGetEventBusInfo(){
    if( !s_initialized ){
        init();
    }
}

void RWDEFGetEventChannelInfo(){
    if( !s_initialized ){
        init();
    }
}

void RWDEFGetEventControllerInfo(){
    if( !s_initialized ){
        init();
    }
}

void RWDEFGetEventInfo(){
    if( !s_initialized ){
        init();
    }
}

void RWDEFGetEventNoteInfo(){
    if( !s_initialized ){
        init();
    }
}

void RWDEFIdle(){
    if( !s_initialized ){
        init();
    }
    rwdef_idle();
}

int RWDEFIsCloseOK(){
    if( !s_initialized ){
        init();
    }
    return rwdef_is_close_ok();
}

int RWDEFIsPanelAppLaunched(){
    if( !s_initialized ){
        init();
    }
    return rwdef_is_panel_app_launched();
}

int RWDEFLaunchPanelApp(){
    if( !s_initialized ){
        init();
    }
    return rwdef_launch_panel_app();
}

int RWDEFOpenDevice(){
    if( !s_initialized ){
        init();
    }
    return rwdef_open_device();
}

int RWDEFQuitPanelApp(){
    if( !s_initialized ){
        init();
    }
    return rwdef_quit_panel_app();
}

void RWDEFSetAudioInfo(){
    if( !s_initialized ){
        init();
    }
}
