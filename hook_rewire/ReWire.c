#include "ReWire.h"

_RWDCloseImp                rw_d_close;
_RWDComBytesAvailableImp    rw_d_com_bytes_available;
_RWDComCheckConnectionImp   rw_d_com_check_connection;
_RWDComCreateImp            rw_d_com_create;
_RWDComDestroyImp           rw_d_com_destroy;
_RWDComDoesMessageFitImp    rw_d_com_does_message_fit;
_RWDComReadImp              rw_d_com_read;
_RWDComSendImp              rw_d_com_send;
_RWDIsCloseOKImp            rw_d_is_close_ok;
_RWDOpenImp                 rw_d_open;
_RWIsReWireMixerAppRunningImp   rw_is_rewire_mixer_app_running;
_RWM2CloseDeviceImp             rw2_close_device;
_RWM2CloseImp                   rw2_close;
_RWM2DriveAudioImp              rw2_drive_audio;
_RWM2GetControllerInfoImp       rw2_get_controller_info;
_RWM2GetDeviceCountImp          rw2_get_device_count;
_RWM2GetDeviceInfoByHandleImp   rw2_get_device_info_byte_handle;
_RWM2GetDeviceInfoImp           rw2_get_device_info;
_RWM2GetEventBusInfoImp         rw2_get_event_bus_info;
_RWM2GetEventChannelInfoImp     rw2_get_event_channel_info;
_RWM2GetEventInfoImp            rw2_get_event_info;
_RWM2GetNoteInfoImp             rw2_get_note_info;
_RWM2IdleImp                    rw2_idle;
_RWM2IsCloseDeviceOKImp         rw2_is_close_device_ok;
_RWM2IsCloseOKImp               rw2_is_close_ok;
_RWM2IsPanelAppLaunchedImp      rw2_is_panel_app_lauched;
_RWM2LaunchPanelAppImp          rw2_lauch_panel_app;
_RWM2OpenDeviceImp              rw2_open_device;
_RWM2OpenImp                    rw2_open;
_RWM2QuitPanelAppImp            rw2_quit_panel_app;
_RWM2SetAudioInfoImp            rw2_set_audio_info;
_RWMCloseDeviceImp              rw_m_close_device;
_RWMCloseImp                    rw_m_close;
_RWMDriveAudioImp               rw_m_drive_audio;
_RWMGetDeviceCountImp           rw_m_get_device_count;
_RWMGetDeviceInfoByHandleImp    rw_m_get_device_info_by_handle;
_RWMGetDeviceInfoImp            rw_m_get_device_info;
_RWMIdleImp                     rw_m_idle;
_RWMIsCloseDeviceOKImp          rw_m_is_close_device_ok;
_RWMIsCloseOKImp                rw_m_is_close_ok;
_RWMOpenDeviceImp               rw_m_open_device;
_RWMOpenImp                     rw_m_open;
_RWPCloseImp                    rw_p_close;
_RWPComBytesAvailableImp        rw_p_com_bytes_available;
_RWPComCheckConnectionImp       rw_p_com_check_connection;
_RWPComConnectImp               rw_p_com_connect;
_RWPComDisconnectImp            rw_p_com_disconnect;
_RWPComDoesMessageFitImp        rw_p_com_does_message_fit;
_RWPComReadImp                  rw_p_com_read;
_RWPComSendImp                  rw_p_com_send;
_RWPIsCloseOKImp                rw_p_is_close_ok;
_RWPLoadDeviceImp               rw_p_load_device;
_RWPOpenImp                     rw_p_open;
_RWPRegisterDeviceImp           rw_p_register_device;
_RWPUnloadDeviceImp             rw_p_unload_device;
_RWPUnregisterDeviceImp         rw_p_unregister_device;
_SmugglerDLL_UnitTest_AllocTestNames    smuggler_dll_unittest_alloc_test_names;
_SmugglerDLL_UnitTest_FreeTestNames     smuggler_dll_unittest_free_test_names;
_SmugglerDLL_UnitTest_RunTests          smuggler_dll_unittest_runtests;
_TopHatCloseDeviceImp       top_hat_close_device;
_TopHatCloseImp             top_hat_clsoe;
_TopHatDriveAudioImp        top_hat_drive_audio;
_TopHatGetDeviceCountImp    top_hat_get_device_count;
_TopHatGetDeviceInfoImp     top_hat_get_device_info;
_TopHatIdleImp              top_hat_idle;
_TopHatIsCloseDeviceOKImp   top_hat_is_close_device_ok;
_TopHatIsCloseOKImp         top_hat_is_close_ok;
_TopHatOpenDeviceImp        top_hat_open_device;
_TopHatOpenImp              top_hat_open;
BOOL g_initialized = FALSE;
FILE *g_log = NULL;

void load_entry_points(){
    g_log = fopen( "C:\\ReWire.log", "w" );
    HMODULE dll_handle = LoadLibraryA( "C:\\WINDOWS\\system32\\_ReWire.dll" );//, NULL, LOAD_WITH_ALTERED_SEARCH_PATH );
    fprintf( g_log, "dll_handle=0x%X\n", (int)dll_handle );
    rw_d_close                                                     = (_RWDCloseImp)GetProcAddress( dll_handle, "RWDCloseImp" );
    fprintf( g_log, "rw_d_close=0x%X\n", rw_d_close );
    rw_d_com_bytes_available                           = (_RWDComBytesAvailableImp)GetProcAddress( dll_handle, "RWDComBytesAvailableImp" );
    fprintf( g_log, "rw_d_com_bytes_available=0x%X\n", rw_d_com_bytes_available );
    rw_d_com_check_connection                          =(_RWDComCheckConnectionImp)GetProcAddress( dll_handle, "RWDComCheckConnectionImp" );
    fprintf( g_log, "rw_d_com_check_connection=0x%X\n", rw_d_com_check_connection );
    rw_d_com_create                                            = (_RWDComCreateImp)GetProcAddress( dll_handle, "RWDComCreateImp" );
    fprintf( g_log, "rw_d_com_create=0x%X\n", rw_d_com_create );
    rw_d_com_destroy                                          = (_RWDComDestroyImp)GetProcAddress( dll_handle, "RWDComDestroyImp" );
    fprintf( g_log, "rw_d_com_destroy=0x%X\n", rw_d_com_destroy );
    rw_d_com_does_message_fit                          = (_RWDComDoesMessageFitImp)GetProcAddress( dll_handle, "RWDComDoesMessageFitImp" );
    fprintf( g_log, "rw_d_com_does_message_fit=0x%X\n", rw_d_com_does_message_fit );
    rw_d_com_read                                                = (_RWDComReadImp)GetProcAddress( dll_handle, "RWDComReadImp" );
    fprintf( g_log, "rw_d_com_read=0x%X\n", rw_d_com_read );
    rw_d_com_send                                                = (_RWDComSendImp)GetProcAddress( dll_handle, "RWDComSendImp" );
    fprintf( g_log, "rw_d_com_send=0x%X\n", rw_d_com_send );
    rw_d_is_close_ok                                           = (_RWDIsCloseOKImp)GetProcAddress( dll_handle, "RWDIsCloseOKImp" );
    fprintf( g_log, "rw_d_is_close_ok=0x%X\n", rw_d_is_close_ok );
    rw_d_open                                                       = (_RWDOpenImp)GetProcAddress( dll_handle, "RWDOpenImp" );
    fprintf( g_log, "rw_d_open=0x%X\n", rw_d_open );
    rw_is_rewire_mixer_app_running                = (_RWIsReWireMixerAppRunningImp)GetProcAddress( dll_handle, "RWIsReWireMixerAppRunningImp" );
    fprintf( g_log, "rw_is_rewire_mixer_app_running=0x%X\n", rw_is_rewire_mixer_app_running );
    rw2_close_device                                        = (_RWM2CloseDeviceImp)GetProcAddress( dll_handle, "RWM2CloseDeviceImp" );
    fprintf( g_log, "rw2_close_device=0x%X\n", rw2_close_device );
    rw2_close                                                     = (_RWM2CloseImp)GetProcAddress( dll_handle, "RWM2CloseImp" );
    fprintf( g_log, "rw2_close=0x%X\n", rw2_close );
    rw2_drive_audio                                          = (_RWM2DriveAudioImp)GetProcAddress( dll_handle, "RWM2DriveAudioImp" );
    fprintf( g_log, "rw2_drive_audio=0x%X\n", rw2_drive_audio );
    rw2_get_controller_info                           = (_RWM2GetControllerInfoImp)GetProcAddress( dll_handle, "RWM2GetControllerInfoImp" );
    fprintf( g_log, "rw2_get_controller_info=0x%X\n", rw2_get_controller_info );
    rw2_get_device_count                                 = (_RWM2GetDeviceCountImp)GetProcAddress( dll_handle, "RWM2GetDeviceCountImp" );
    fprintf( g_log, "rw2_get_device_count=0x%X\n", rw2_get_device_count );
    rw2_get_device_info_byte_handle               = (_RWM2GetDeviceInfoByHandleImp)GetProcAddress( dll_handle, "RWM2GetDeviceInfoByHandleImp" );
    fprintf( g_log, "rw2_get_device_info_byte_handle=0x%X\n", rw2_get_device_info_byte_handle );
    rw2_get_device_info                                   = (_RWM2GetDeviceInfoImp)GetProcAddress( dll_handle, "RWM2GetDeviceInfoImp" );
    fprintf( g_log, "rw2_get_device_info=0x%X\n", rw2_get_device_info );
    rw2_get_event_bus_info                              = (_RWM2GetEventBusInfoImp)GetProcAddress( dll_handle, "RWM2GetEventBusInfoImp" );
    fprintf( g_log, "rw2_get_event_bus_info=0x%X\n", rw2_get_event_bus_info );
    rw2_get_event_channel_info                      = (_RWM2GetEventChannelInfoImp)GetProcAddress( dll_handle, "RWM2GetEventChannelInfoImp" );
    fprintf( g_log, "rw2_get_event_channel_info=0x%X\n", rw2_get_event_channel_info );
    rw2_get_event_info                                     = (_RWM2GetEventInfoImp)GetProcAddress( dll_handle, "RWM2GetEventInfoImp" );
    fprintf( g_log, "rw2_get_event_info=0x%X\n", rw2_get_event_info );
    rw2_get_note_info                                       = (_RWM2GetNoteInfoImp)GetProcAddress( dll_handle, "RWM2GetNoteInfoImp" );
    fprintf( g_log, "rw2_get_note_info=0x%X\n", rw2_get_note_info );
    rw2_idle                                                       = (_RWM2IdleImp)GetProcAddress( dll_handle, "RWM2IdleImp" );
    fprintf( g_log, "rw2_idle=0x%X\n", rw2_idle );
    rw2_is_close_device_ok                              = (_RWM2IsCloseDeviceOKImp)GetProcAddress( dll_handle, "RWM2IsCloseDeviceOKImp" );
    fprintf( g_log, "rw2_is_close_device_ok=0x%X\n", rw2_is_close_device_ok );
    rw2_is_close_ok                                           = (_RWM2IsCloseOKImp)GetProcAddress( dll_handle, "RWM2IsCloseOKImp" );
    fprintf( g_log, "rw2_is_close_ok=0x%X\n", rw2_is_close_ok );
    rw2_is_panel_app_lauched                         = (_RWM2IsPanelAppLaunchedImp)GetProcAddress( dll_handle, "RWM2IsPanelAppLaunchedImp" );
    fprintf( g_log, "rw2_is_panel_app_lauched=0x%X\n", rw2_is_panel_app_lauched );
    rw2_lauch_panel_app                                  = (_RWM2LaunchPanelAppImp)GetProcAddress( dll_handle, "RWM2LaunchPanelAppImp" );
    fprintf( g_log, "rw2_lauch_panel_app=0x%X\n", rw2_lauch_panel_app );
    rw2_open_device                                          = (_RWM2OpenDeviceImp)GetProcAddress( dll_handle, "RWM2OpenDeviceImp" );
    fprintf( g_log, "rw2_open_device=0x%X\n", rw2_open_device );
    rw2_open                                                       = (_RWM2OpenImp)GetProcAddress( dll_handle, "RWM2OpenImp" );
    fprintf( g_log, "rw2_open=0x%X\n", rw2_open );
    rw2_quit_panel_app                                     = (_RWM2QuitPanelAppImp)GetProcAddress( dll_handle, "RWM2QuitPanelAppImp" );
    fprintf( g_log, "rw2_quit_panel_app=0x%X\n", rw2_quit_panel_app );
    rw2_set_audio_info                                     = (_RWM2SetAudioInfoImp)GetProcAddress( dll_handle, "RWM2SetAudioInfoImp" );
    fprintf( g_log, "rw2_set_audio_info=0x%X\n", rw2_set_audio_info );
    rw_m_close_device                                        = (_RWMCloseDeviceImp)GetProcAddress( dll_handle, "RWMCloseDeviceImp" );
    fprintf( g_log, "rw_m_close_device=0x%X\n", rw_m_close_device );
    rw_m_close                                                     = (_RWMCloseImp)GetProcAddress( dll_handle, "RWMCloseImp" );
    fprintf( g_log, "rw_m_close=0x%X\n", rw_m_close );
    rw_m_drive_audio                                          = (_RWMDriveAudioImp)GetProcAddress( dll_handle, "RWMDriveAudioImp" );
    fprintf( g_log, "rw_m_drive_audio=0x%X\n", rw_m_drive_audio );
    rw_m_get_device_count                                 = (_RWMGetDeviceCountImp)GetProcAddress( dll_handle, "RWMGetDeviceCountImp" );
    fprintf( g_log, "rw_m_get_device_count=0x%X\n", rw_m_get_device_count );
    rw_m_get_device_info_by_handle                 = (_RWMGetDeviceInfoByHandleImp)GetProcAddress( dll_handle, "RWMGetDeviceInfoByHandleImp" );
    fprintf( g_log, "rw_m_get_device_info_by_handle=0x%X\n", rw_m_get_device_info_by_handle );
    rw_m_get_device_info                                   = (_RWMGetDeviceInfoImp)GetProcAddress( dll_handle, "RWMGetDeviceInfoImp" );
    fprintf( g_log, "rw_m_get_device_info=0x%X\n", rw_m_get_device_info );
    rw_m_idle                                                       = (_RWMIdleImp)GetProcAddress( dll_handle, "RWMIdleImp" );
    fprintf( g_log, "rw_m_idle=0x%X\n", rw_m_idle );
    rw_m_is_close_device_ok                              = (_RWMIsCloseDeviceOKImp)GetProcAddress( dll_handle, "RWMIsCloseDeviceOKImp" );
    fprintf( g_log, "rw_m_is_close_device_ok=0x%X\n", rw_m_is_close_device_ok );
    rw_m_is_close_ok                                           = (_RWMIsCloseOKImp)GetProcAddress( dll_handle, "RWMIsCloseOKImp" );
    fprintf( g_log, "rw_m_is_close_ok=0x%X\n", rw_m_is_close_ok );
    rw_m_open_device                                          = (_RWMOpenDeviceImp)GetProcAddress( dll_handle, "RWMOpenDeviceImp" );
    fprintf( g_log, "rw_m_open_device=0x%X\n", rw_m_open_device );
    rw_m_open                                                       = (_RWMOpenImp)GetProcAddress( dll_handle, "RWMOpenImp" );
    fprintf( g_log, "rw_m_open=0x%X\n", rw_m_open );
    rw_p_close                                                     = (_RWPCloseImp)GetProcAddress( dll_handle, "RWPCloseImp" );
    fprintf( g_log, "rw_p_close=0x%X\n", rw_p_close );
    rw_p_com_bytes_available                           = (_RWPComBytesAvailableImp)GetProcAddress( dll_handle, "RWPComBytesAvailableImp" );
    fprintf( g_log, "rw_p_com_bytes_available=0x%X\n", rw_p_com_bytes_available );
    rw_p_com_check_connection                         = (_RWPComCheckConnectionImp)GetProcAddress( dll_handle, "RWPComCheckConnectionImp" );
    fprintf( g_log, "rw_p_com_check_connection=0x%X\n", rw_p_com_check_connection );
    rw_p_com_connect                                          = (_RWPComConnectImp)GetProcAddress( dll_handle, "RWPComConnectImp" );
    fprintf( g_log, "rw_p_com_connect=0x%X\n", rw_p_com_connect );
    rw_p_com_disconnect                                    = (_RWPComDisconnectImp)GetProcAddress( dll_handle, "RWPComDisconnectImp" );
    fprintf( g_log, "rw_p_com_disconnect=0x%X\n", rw_p_com_disconnect );
    rw_p_com_does_message_fit                          = (_RWPComDoesMessageFitImp)GetProcAddress( dll_handle, "RWPComDoesMessageFitImp" );
    fprintf( g_log, "rw_p_com_does_message_fit=0x%X\n", rw_p_com_does_message_fit );
    rw_p_com_read                                                = (_RWPComReadImp)GetProcAddress( dll_handle, "RWPComReadImp" );
    fprintf( g_log, "rw_p_com_read=0x%X\n", rw_p_com_read );
    rw_p_com_send                                                = (_RWPComSendImp)GetProcAddress( dll_handle, "RWPComSendImp" );
    fprintf( g_log, "rw_p_com_send=0x%X\n", rw_p_com_send );
    rw_p_is_close_ok                                           = (_RWPIsCloseOKImp)GetProcAddress( dll_handle, "RWPIsCloseOKImp" );
    fprintf( g_log, "rw_p_is_close_ok=0x%X\n", rw_p_is_close_ok );
    rw_p_load_device                                          = (_RWPLoadDeviceImp)GetProcAddress( dll_handle, "RWPLoadDeviceImp" );
    fprintf( g_log, "rw_p_load_device=0x%X\n", rw_p_load_device );
    rw_p_open                                                       = (_RWPOpenImp)GetProcAddress( dll_handle, "RWPOpenImp" );
    fprintf( g_log, "rw_p_open=0x%X\n", rw_p_open );
    rw_p_register_device                                  = (_RWPRegisterDeviceImp)GetProcAddress( dll_handle, "RWPRegisterDeviceImp" );
    fprintf( g_log, "rw_p_register_device=0x%X\n", rw_p_register_device );
    rw_p_unload_device                                      = (_RWPUnloadDeviceImp)GetProcAddress( dll_handle, "RWPUnloadDeviceImp" );
    fprintf( g_log, "rw_p_unload_device=0x%X\n", rw_p_unload_device );
    rw_p_unregister_device                              = (_RWPUnregisterDeviceImp)GetProcAddress( dll_handle, "RWPUnregisterDeviceImp" );
    fprintf( g_log, "rw_p_unregister_device=0x%X\n", rw_p_unregister_device );
    smuggler_dll_unittest_alloc_test_names = (_SmugglerDLL_UnitTest_AllocTestNames)GetProcAddress( dll_handle, "SmugglerDLL_UnitTest_AllocTestNames" );
    fprintf( g_log, "smuggler_dll_unittest_alloc_test_names=0x%X\n", smuggler_dll_unittest_alloc_test_names );
    smuggler_dll_unittest_free_test_names   = (_SmugglerDLL_UnitTest_FreeTestNames)GetProcAddress( dll_handle, "SmugglerDLL_UnitTest_FreeTestNames" );
    fprintf( g_log, "smuggler_dll_unittest_free_test_names=0x%X\n", smuggler_dll_unittest_free_test_names );
    smuggler_dll_unittest_runtests               = (_SmugglerDLL_UnitTest_RunTests)GetProcAddress( dll_handle, "SmugglerDLL_UnitTest_RunTests" );
    fprintf( g_log, "smuggler_dll_unittest_runtests=0x%X\n", smuggler_dll_unittest_runtests );
    top_hat_close_device                                  = (_TopHatCloseDeviceImp)GetProcAddress( dll_handle, "TopHatCloseDeviceImp" );
    fprintf( g_log, "top_hat_close_device=0x%X\n", top_hat_close_device );
    top_hat_clsoe                                               = (_TopHatCloseImp)GetProcAddress( dll_handle, "TopHatCloseImp" );
    fprintf( g_log, "top_hat_clsoe=0x%X\n", top_hat_clsoe );
    top_hat_drive_audio                                    = (_TopHatDriveAudioImp)GetProcAddress( dll_handle, "TopHatDriveAudioImp" );
    fprintf( g_log, "top_hat_drive_audio=0x%X\n", top_hat_drive_audio );
    top_hat_get_device_count                           = (_TopHatGetDeviceCountImp)GetProcAddress( dll_handle, "TopHatGetDeviceCountImp" );
    fprintf( g_log, "top_hat_get_device_count=0x%X\n", top_hat_get_device_count );
    top_hat_get_device_info                             = (_TopHatGetDeviceInfoImp)GetProcAddress( dll_handle, "TopHatGetDeviceInfoImp" );
    fprintf( g_log, "top_hat_get_device_info=0x%X\n", top_hat_get_device_info );
    top_hat_idle                                                 = (_TopHatIdleImp)GetProcAddress( dll_handle, "TopHatIdleImp" );
    fprintf( g_log, "top_hat_idle=0x%X\n", top_hat_idle );
    top_hat_is_close_device_ok                        = (_TopHatIsCloseDeviceOKImp)GetProcAddress( dll_handle, "TopHatIsCloseDeviceOKImp" );
    fprintf( g_log, "top_hat_is_close_device_ok=0x%X\n", top_hat_is_close_device_ok );
    top_hat_is_close_ok                                     = (_TopHatIsCloseOKImp)GetProcAddress( dll_handle, "TopHatIsCloseOKImp" );
    fprintf( g_log, "top_hat_is_close_ok=0x%X\n", top_hat_is_close_ok );
    top_hat_open_device                                    = (_TopHatOpenDeviceImp)GetProcAddress( dll_handle, "TopHatOpenDeviceImp" );
    fprintf( g_log, "top_hat_open_device=0x%X\n", top_hat_open_device );
    top_hat_open                                                 = (_TopHatOpenImp)GetProcAddress( dll_handle, "TopHatOpenImp" );
    fprintf( g_log, "top_hat_open=0x%X\n", top_hat_open );
    g_initialized = TRUE;
}

int main(){
    load_entry_points();
    return 0;
}

void RWDCloseImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWDCloseImp\n" );
    rw_d_close();
}

void RWDComBytesAvailableImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWDComBytesAvailableImp\n" );
    rw_d_com_bytes_available();
}

void RWDComCheckConnectionImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWDComCheckConnectionImp\n" );
    rw_d_com_check_connection();
}

void RWDComCreateImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWDComCreateImp\n" );
    rw_d_com_create();
}

void RWDComDestroyImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWDComDestroyImp\n" );
    rw_d_com_destroy();
}

void RWDComDoesMessageFitImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWDComDoesMessageFitImp\n" );
    rw_d_com_does_message_fit();
}

void RWDComReadImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWDComReadImp\n" );
    rw_d_com_read();
}

void RWDComSendImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWDComSendImp\n" );
    rw_d_com_send();
}

void RWDIsCloseOKImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWDIsCloseOKImp\n" );
    rw_d_is_close_ok();
}

void RWDOpenImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWDOpenImp\n" );
    rw_d_open();
}

void RWIsReWireMixerAppRunningImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWIsReWireMixerAppRunningImp\n" );
    rw_is_rewire_mixer_app_running();
}

void RWM2CloseDeviceImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2CloseDeviceImp\n" );
    rw2_close_device();
}

void RWM2CloseImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2CloseImp\n" );
    rw2_close();
}

void RWM2DriveAudioImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2DriveAudioImp\n" );
    rw2_drive_audio();
}

void RWM2GetControllerInfoImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2GetControllerInfoImp\n" );
    rw2_get_controller_info();
}

void RWM2GetDeviceCountImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2GetDeviceCountImp\n" );
    rw2_get_device_count();
}

void RWM2GetDeviceInfoByHandleImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2GetDeviceInfoByHandleImp\n" );
    rw2_get_device_info_byte_handle();
}

void RWM2GetDeviceInfoImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2GetDeviceInfoImp\n" );
    rw2_get_device_info();
}

void RWM2GetEventBusInfoImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2GetEventBusInfoImp\n" );
    rw2_get_event_bus_info();
}

void RWM2GetEventChannelInfoImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2GetEventChannelInfoImp\n" );
    rw2_get_event_channel_info();
}

void RWM2GetEventInfoImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2GetEventInfoImp\n" );
    rw2_get_event_info();
}

void RWM2GetNoteInfoImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2GetNoteInfoImp\n" );
    rw2_get_note_info();
}

void RWM2IdleImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2IdleImp\n" );
    rw2_idle();
}

void RWM2IsCloseDeviceOKImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2IsCloseDeviceOKImp\n" );
    rw2_is_close_device_ok();
}

void RWM2IsCloseOKImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2IsCloseOKImp\n" );
    rw2_is_close_ok();
}

void RWM2IsPanelAppLaunchedImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2IsPanelAppLaunchedImp\n" );
    rw2_is_panel_app_lauched();
}

void RWM2LaunchPanelAppImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2LaunchPanelAppImp\n" );
    rw2_lauch_panel_app();
}

void RWM2OpenDeviceImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2OpenDeviceImp\n" );
    rw2_open_device();
}

void RWM2OpenImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2OpenImp\n" );
    rw2_open();
}

void RWM2QuitPanelAppImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2QuitPanelAppImp\n" );
    rw2_quit_panel_app();
}

void RWM2SetAudioInfoImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWM2SetAudioInfoImp\n" );
    rw2_set_audio_info();
}

void RWMCloseDeviceImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWMCloseDeviceImp\n" );
    rw_m_close_device();
}

void RWMCloseImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWMCloseImp\n" );
    rw_m_close();
}

void RWMDriveAudioImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWMDriveAudioImp\n" );
    rw_m_drive_audio();
}

void RWMGetDeviceCountImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWMGetDeviceCountImp\n" );
    rw_m_get_device_count();
}

void RWMGetDeviceInfoByHandleImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWMGetDeviceInfoByHandleImp\n" );
    rw_m_get_device_info_by_handle();
}

void RWMGetDeviceInfoImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWMGetDeviceInfoImp\n" );
    rw_m_get_device_info();
}

void RWMIdleImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWMIdleImp\n" );
    rw_m_idle();
}

void RWMIsCloseDeviceOKImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWMIsCloseDeviceOKImp\n" );
    rw_m_is_close_device_ok();
}

void RWMIsCloseOKImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWMIsCloseOKImp\n" );
    rw_m_is_close_ok();
}

void RWMOpenDeviceImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWMOpenDeviceImp\n" );
    rw_m_open_device();
}

void RWMOpenImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWMOpenImp\n" );
    rw_m_open();
}

void RWPCloseImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPCloseImp\n" );
    rw_p_close();
}

void RWPComBytesAvailableImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPComBytesAvailableImp\n" );
    rw_p_com_bytes_available();
}

void RWPComCheckConnectionImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPComCheckConnectionImp\n" );
    rw_p_com_check_connection();
}

void RWPComConnectImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPComConnectImp\n" );
    rw_p_com_connect();
}

void RWPComDisconnectImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPComDisconnectImp\n" );
    rw_p_com_disconnect();
}

void RWPComDoesMessageFitImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPComDoesMessageFitImp\n" );
    rw_p_com_does_message_fit();
}

void RWPComReadImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPComReadImp\n" );
    rw_p_com_read();
}

void RWPComSendImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPComSendImp\n" );
    rw_p_com_send();
}

void RWPIsCloseOKImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPIsCloseOKImp\n" );
    rw_p_is_close_ok();
}

void RWPLoadDeviceImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPLoadDeviceImp\n" );
    rw_p_load_device();
}

void RWPOpenImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPOpenImp\n" );
    rw_p_open();
}

void RWPRegisterDeviceImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPRegisterDeviceImp\n" );
    rw_p_register_device();
}

void RWPUnloadDeviceImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPUnloadDeviceImp\n" );
    rw_p_unload_device();
}

void RWPUnregisterDeviceImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "RWPUnregisterDeviceImp\n" );
    rw_p_unregister_device();
}

void SmugglerDLL_UnitTest_AllocTestNames(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "SmugglerDLL_UnitTest_AllocTestNames\n" );
    smuggler_dll_unittest_alloc_test_names();
}

void SmugglerDLL_UnitTest_FreeTestNames(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "SmugglerDLL_UnitTest_FreeTestNames\n" );
    smuggler_dll_unittest_free_test_names();
}

void SmugglerDLL_UnitTest_RunTests(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "SmugglerDLL_UnitTest_RunTests\n" );
    smuggler_dll_unittest_runtests();
}

void TopHatCloseDeviceImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "TopHatCloseDeviceImp\n" );
    top_hat_close_device();
}

void TopHatCloseImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "TopHatCloseImp\n" );
    top_hat_clsoe();
}

void TopHatDriveAudioImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "TopHatDriveAudioImp\n" );
    top_hat_drive_audio();
}

void TopHatGetDeviceCountImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "TopHatGetDeviceCountImp\n" );
    top_hat_get_device_count();
}

void TopHatGetDeviceInfoImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "TopHatGetDeviceInfoImp\n" );
    top_hat_get_device_info();
}

void TopHatIdleImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "TopHatIdleImp\n" );
    top_hat_idle();
}

void TopHatIsCloseDeviceOKImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "TopHatIsCloseDeviceOKImp\n" );
    top_hat_is_close_device_ok();
}

void TopHatIsCloseOKImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "TopHatIsCloseOKImp\n" );
    top_hat_is_close_ok();
}

void TopHatOpenDeviceImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "TopHatOpenDeviceImp\n" );
    top_hat_open_device();
}

void TopHatOpenImp(){
    if( !g_initialized ) load_entry_points();
    fprintf( g_log, "TopHatOpenImp\n" );
    top_hat_open();
}
