#include <windows.h>
#include <stdio.h>

/**
 * 指定したタイトルを持つウィンドウを探し、最初に見つかったウィンドウのハンドルを返す
 */
HWND FindWindowFromTitle( HWND parent, TCHAR *title ){
    HWND lastChild = NULL;
    HWND ret = NULL;
    while( 1 ){
        HWND child = FindWindowEx( parent, lastChild, 0, 0 );
        if( NULL == child ){
            break;
        }
        TCHAR obtained[MAX_PATH];
        if( GetWindowText( child, obtained, MAX_PATH ) ){
            if( strstr( obtained, title ) ){
                ret = child;
                break;
            }
        }
        lastChild = child;
    }
    return ret;
}

/**
 * 指定されたテキストを持つメニュー項目を探す
 */
UINT FindMenuIdFromText( HMENU parent, TCHAR *text ){
    UINT id = 0;
    int numMenu = GetMenuItemCount( parent );
    int i;
    for( i = 0; i < numMenu; i++ ){
        TCHAR txt[MAX_PATH];
        MENUITEMINFO menuInfo;
        menuInfo.cbSize = sizeof( MENUITEMINFO );
        menuInfo.fMask = MIIM_TYPE;
        menuInfo.dwTypeData = txt;
        menuInfo.cch = MAX_PATH;
        if( GetMenuItemInfo( parent, i, TRUE, &menuInfo ) ){
            if( menuInfo.dwTypeData ){
                if( strstr( menuInfo.dwTypeData, text ) ){
                    id = GetMenuItemID( parent, i );
                    break;
                }
            }
        }
    }
    return id;
}

int main(){
    HWND hMainWindow = FindWindowFromTitle( NULL, "VOCALOID Editor - " );
    printf( "hMainWindow=0x%X\n", hMainWindow );
    if( hMainWindow == NULL ){
        return 0;
    }

    HMENU hMenu = GetMenu( hMainWindow );
    printf( "hMenu=0x%X\n", hMenu );
    HMENU hMenuFile = NULL;
    HMENU hMenuEdit = NULL;
    HMENU hMenuJob = NULL;
    int i;
    int numMenu = GetMenuItemCount( hMenu );
    for( i = 0; i < numMenu ; i++ ){
        HMENU sub = GetSubMenu( hMenu, i );
        if( NULL == sub ){
            break;
        }
        TCHAR txt[MAX_PATH];
        MENUITEMINFO menuInfo;
        menuInfo.cbSize = sizeof( MENUITEMINFO );
        menuInfo.fMask = MIIM_TYPE;
        menuInfo.dwTypeData = txt;
        menuInfo.cch = MAX_PATH;
        GetMenuItemInfo( hMenu, i, TRUE, &menuInfo );
        if( strstr( menuInfo.dwTypeData, TEXT( "編集" ) ) || 
            strstr( menuInfo.dwTypeData, TEXT( "Edit" ) ) ){
            hMenuEdit = sub;
        }
        if( strstr( menuInfo.dwTypeData, TEXT( "ファイル" ) ) || 
            strstr( menuInfo.dwTypeData, TEXT( "File" ) ) ){
            hMenuFile = sub;
        }
        if( strstr( menuInfo.dwTypeData, TEXT( "ジョブ" ) ) ||
            strstr( menuInfo.dwTypeData, TEXT( "Job" ) ) ){
            hMenuJob = sub;
        }
    }
    printf( "hMenuFile=0x%X\n", hMenuFile );
    printf( "hMenuEdit=0x%X\n", hMenuEdit );

    // File->Openを取得
    UINT idMenuFileOpen = FindMenuIdFromText( hMenuFile, TEXT( "Open" ) );
    if( idMenuFileOpen == 0 ){
        idMenuFileOpen = FindMenuIdFromText( hMenuFile, TEXT( "開く" ) );
    }
    printf( "idMenuFileOpen=0x%X\n", idMenuFileOpen );

    // Edit->Select All Eventsを取得
    UINT idMenuEditSelectAllEvents = FindMenuIdFromText( hMenuEdit, TEXT( "Ctrl+Shift+A" ) );
    printf( "idMenuEditSelectAllEvents=0x%X\n", idMenuEditSelectAllEvents );

    // Job->歌詞の流し込みを取得
    UINT idMenuJobImportLyric = FindMenuIdFromText( hMenuJob, TEXT( "(&L)" ) );
    printf( "idMenuJobImportLyric=0x%X\n", idMenuJobImportLyric );

    //File->Openを実行
    PostMessage( hMainWindow, WM_COMMAND, idMenuFileOpen, 0 );

    //ファイルを開くダイアログのHWNDを取得
    HWND hOpenFileDialog = NULL;
    int count = 0;
    int wait_ms = 100;
    int max_wait_ms = 10 * 60 * 1000; // 10分間だけ待ってやる！！
    int max_count = max_wait_ms / wait_ms;
    while( NULL == hOpenFileDialog && count < max_count ){
        Sleep( wait_ms );
        hOpenFileDialog = FindWindowFromTitle( NULL, TEXT( "ファイルを開く" ) );
        count++;
    }
    printf( "hOpenFileDialog=0x%X\n", hOpenFileDialog );
    HWND hButtonOpen = FindWindowFromTitle( hOpenFileDialog, TEXT( "(&O)" ) );
    printf( "hButtonOpen=0x%X\n", hButtonOpen );
    
    HWND hFileName = NULL;
    HWND lastChild = NULL;
    while( 1 ){
        HWND child = FindWindowEx( hOpenFileDialog, lastChild, TEXT( "ComboBoxEx32" ), NULL );
        if( NULL == child ){
            break;
        }
        // ComboBoxクラスの子を持つComboBoxEx32を探す
        HWND lastChild2 = NULL;
        while( 1 ){
            HWND child2 = FindWindowEx( child, lastChild2, TEXT( "ComboBox" ), NULL );
            if( NULL == child2 ){
                break;
            }
            // Editクラスの子を持つComboBoxを探す
            HWND lastChild3 = NULL;
            while( 1 ){
                HWND child3 = FindWindowEx( child2, lastChild3, TEXT( "Edit" ), NULL );
                if( NULL == child3 ){
                    break;
                }
                hFileName = child3;
                break;
            }
            break;
        }
        if( hFileName ){
            break;
        }
        lastChild = child;
    }
    printf( "hFileName=0x%x\n", hFileName );

    // ファイル名を打ち込む
    PostMessage( hFileName, WM_CHAR, 'b', 0 );
    PostMessage( hFileName, WM_CHAR, 'a', 0 );
    PostMessage( hFileName, WM_CHAR, 's', 0 );
    PostMessage( hFileName, WM_CHAR, 'e', 0 );
    PostMessage( hFileName, WM_CHAR, '.', 0 );
    PostMessage( hFileName, WM_CHAR, 'v', 0 );
    PostMessage( hFileName, WM_CHAR, 's', 0 );
    PostMessage( hFileName, WM_CHAR, 'q', 0 );

    // 開くボタンを押す
    UINT idButtonOpen = GetDlgCtrlID( hButtonOpen );
    PostMessage( hOpenFileDialog, WM_COMMAND, idButtonOpen, 0 );

    HWND hTemp = NULL;
    while( hTemp != hMainWindow ){
        Sleep( 100 );
        // 時間がかかりますダイアログ
        HWND costTimeDialog = FindWindowEx( NULL, NULL, NULL, TEXT( "VOCALOID Editor" ) );
        if( costTimeDialog ){
            PostMessage( costTimeDialog, WM_COMMAND, IDOK, 0 );
        }
        hTemp = FindWindowFromTitle( NULL, TEXT( "VOCALOID Editor - base.vsq" ) );
    }

    // 先頭の音符を選択状態にしてくださいダイアログを表示
    int retDialog = 0;
    while( retDialog != IDOK ){
        retDialog = MessageBox( NULL, TEXT( "先頭の音符を選択して下さい" ), TEXT( "AutoLyricInsert" ), 0 );
    }

    // 音符の流し込みメニューをクリック
    PostMessage( hMainWindow, WM_COMMAND, idMenuJobImportLyric, 0 );

    // Max:5000[notes]というStaticクラスの子を持つダイアログをしらみつぶしに探す
    lastChild = NULL;
    HWND hLyricDialog = NULL;
    while( 1 ){
        HWND child = FindWindowEx( NULL, lastChild, NULL, NULL );
        if( NULL == child ){
            break;
        }
        HWND lastChild2 = NULL;
        while( 1 ){
            HWND child2 = FindWindowEx( child, lastChild2, TEXT( "Static" ), NULL );
            if( NULL == child2 ){
                break;
            }
            TCHAR obtained[MAX_PATH];
            if( GetWindowText( child2, obtained, MAX_PATH ) ){
                printf( "obtained=%s\n", obtained );
                if( strstr( obtained, TEXT( "Max:5000[notes]" ) ) ){
                    hLyricDialog = child;
                    break;
                }
            }
            lastChild2 = child2;
        }
        if( hLyricDialog ){
            break;
        }
        lastChild = child;
    }
    printf( "hLyricDialog=0x%X\n", hLyricDialog );

    // テキストボックスを特定する
    HWND hLyricTextbox = FindWindowEx( hLyricDialog, NULL, TEXT( "Edit" ), NULL );
    printf( "hLyricTextbox=0x%X\n", hLyricTextbox );

    return 0;
}

