#include <windows.h>
#include <stdio.h>

#define WORDS_PER_FILE 2500

HWND hMainWindow = NULL;
UINT idMenuFileOpen = 0;
UINT idMenuFileSaveNamed = 0;
UINT idMenuJobImportLyric = 0;
HWND hLabelMeasure = NULL;
HWND hPianoroll = NULL;
HWND hHScroll = NULL;

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

void init(){
    hMainWindow = FindWindowFromTitle( NULL, "VOCALOID Editor - " );
    printf( "hMainWindow=0x%X\n", hMainWindow );
    if( hMainWindow == NULL ){
        return;
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
    idMenuFileOpen = FindMenuIdFromText( hMenuFile, TEXT( "Open" ) );
    if( idMenuFileOpen == 0 ){
        idMenuFileOpen = FindMenuIdFromText( hMenuFile, TEXT( "開く" ) );
    }
    printf( "idMenuFileOpen=0x%X\n", idMenuFileOpen );

    idMenuFileSaveNamed = FindMenuIdFromText( hMenuFile, TEXT( "(&A)" ) );
    printf( "idMenuFileSaveNamed=0x%X\n", idMenuFileSaveNamed );

    // Job->歌詞の流し込みを取得
    idMenuJobImportLyric = FindMenuIdFromText( hMenuJob, TEXT( "(&L)" ) );
    printf( "idMenuJobImportLyric=0x%X\n", idMenuJobImportLyric );

    // マウス位置のゲートタイムを表示しているラベルを取得
    HWND hToolbar = NULL;
    HWND lastChild = NULL;
    // まず、ツールバー(3つある)を保持しているコンテナを取得
    while( 1 ){
        HWND child = FindWindowEx( hMainWindow, lastChild, TEXT( "AfxControlBar70" ), NULL );
        if( NULL == child ){
            break;
        }
        printf( "child=0x%X\n", child );
        // 子を持ってるか？
        HWND child2 = FindWindowEx( child, NULL, NULL, NULL );
        printf( "child2=0x%X\n", child2 );
        lastChild = child;
        if( child2 ){
            hToolbar = child;
            break;
        }
    }
    // mesaureツールバーを取得(直下の子に、"MEASURE"というStaticクラスを持つものを探す)
    printf( "hToolbar=0x%X\n", hToolbar );
    HWND hToolbarMeasure = NULL;
    lastChild = NULL;
    while( 1 ){
        HWND child = FindWindowEx( hToolbar, lastChild, NULL, NULL );
        if( NULL == child ){
            break;
        }
        printf( "child=0x%X\n", child );
        HWND lastChild2 = NULL;
        while( 1 ){
            HWND child2 = FindWindowEx( child, lastChild2, TEXT( "Static" ), NULL );
            if( NULL == child2 ){
                break;
            }
            TCHAR obtained[MAX_PATH];
            if( GetWindowText( child2, obtained, MAX_PATH ) ){
                if( strstr( obtained, TEXT( "MEASURE" ) ) ){
                    hToolbarMeasure = child;
                    break;
                }
            }
            lastChild2 = child;
        }
        lastChild = child;
        if( NULL != hToolbarMeasure ){
            break;
        }
    }
    printf( "hToolbarMeasure=0x%X\n", hToolbarMeasure );
    lastChild = NULL;
    while( 1 ){
        HWND child = FindWindowEx( hToolbarMeasure, lastChild, TEXT( "Static" ), NULL );
        if( NULL == child ){
            break;
        }
        TCHAR obtained[MAX_PATH];
        if( GetWindowText( child, obtained, MAX_PATH ) ){
            if( strstr( obtained, TEXT( " : " ) ) ){
                hLabelMeasure = child;
                break;
            }
        }
        lastChild = child;
    }
    printf( "hLabelMeasure=0x%X\n", hLabelMeasure );

    // ピアノロール画面を取得
    hPianoroll = FindWindowEx( hMainWindow, NULL, TEXT( "Afx:00400000:b" ), NULL );
    printf( "hPianoroll=0x%X\n", hPianoroll );

    // ピアノロール画面の水平スクロールを取得
    lastChild = NULL;
    while( 1 ){
        HWND child = FindWindowEx( hPianoroll, lastChild, TEXT( "ScrollBar" ), NULL );
        if( NULL == child ){
            break;
        }
        RECT rc;
        GetWindowRect( child, &rc );
        if( rc.bottom - rc.top < 20 ){
            // 高さが20以下だったら多分水平スクロールバーだろう
            hHScroll = child;
            break;
        }
        lastChild = child;
    }
    printf( "hHScroll=0x%X\n", hHScroll );
}

HWND GetLyricTextBoxHandle( HWND *hLyricDialog ){
    HWND ret = NULL;
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
    Sleep( 200 );

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

    // 先頭の音符を自動で選択状態にするのを試みる
    SetForegroundWindow( hMainWindow );
    SetWindowPos( hMainWindow, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE );
    //Sleep( 10000 );
    RECT rc;
    GetWindowRect( hPianoroll, &rc );
    printf( "rc={x=%d,y=%d,width=%d,height=%d}\n", rc.left, rc.top, (rc.right - rc.left), (rc.bottom - rc.top) );
    int posx = rc.left + 60;
    int posy = rc.top + 55;
    SetCursorPos( posx, posy );
    Sleep( 200 );
    TCHAR text[MAX_PATH];
    int destination_clock = (1 * 4 + 1) * 1920 + 60;
    while( 1 ){
        // 1:1:60までマウスを動かすよ
        if( GetWindowText( hLabelMeasure, text, MAX_PATH ) ){
            int measure, beat, gate;
            sscanf( text, " %d : %d : %d", &measure, &beat, &gate );
            if( measure == 1 && beat == 1 && gate == 60 ){
                break;
            }
            int clock = (measure * 4 + beat) * 1920 + gate;
            if( clock < destination_clock ){
                posx++;
            }else{
                posx--;
            }
            if( posx < rc.left + 10 ){
                posx = rc.left + 10;
            }else if( rc.right - 30 < posx ){
                posx = rc.right - 30;
            }
            SetCursorPos( posx, posy );
            Sleep( 1 );
        }
    }
    RECT rcHScroll;
    GetWindowRect( hHScroll, &rcHScroll );
    //SetCapture( hMainWindow );
    POINT p;
    p.x = posx;
    p.y = posy;
    ScreenToClient( hPianoroll, &p );
    PostMessage( hPianoroll, WM_LBUTTONDOWN, MK_LBUTTON, (0xffff0000 & (p.y << 16)) | (0xffff & p.x) );
    SetCursorPos( posx, posy );
    int ix, iy;
    for( ix = posx; ix < posx + 10; ix++ ){
        p.x = ix;
        p.y = posy;
        ScreenToClient( hPianoroll, &p );
        PostMessage( hPianoroll, WM_MOUSEMOVE, MK_LBUTTON, (0xffff0000 & (p.y << 16)) | (0xffff & p.x) );
        SetCursorPos( ix, posy );
        Sleep( 1 );
    }
    posx += 10;
    for( iy = posy; iy < rcHScroll.bottom - 20; iy++ ){
        p.x = posx;
        p.y = iy;
        ScreenToClient( hPianoroll, &p );
        PostMessage( hPianoroll, WM_MOUSEMOVE, MK_LBUTTON, (0xffff0000 & (p.y << 16)) | (0xffff & p.x) );
        SetCursorPos( posx, iy );
        Sleep( 1 );
    }
    posy = rcHScroll.bottom - 20;
    SetCursorPos( posx, posy );
    p.x = posx;
    p.y = posy;
    ScreenToClient( hPianoroll, &p );
    PostMessage( hPianoroll, WM_LBUTTONUP, MK_LBUTTON, (0xffff0000 & (p.y << 16)) | (0xffff & p.x) );

    // 音符の流し込みメニューをクリック
    PostMessage( hMainWindow, WM_COMMAND, idMenuJobImportLyric, 0 );

    // Max:5000[notes]というStaticクラスの子を持つダイアログをしらみつぶしに探す
    HWND hDialog = NULL;
    while( NULL == hDialog ){
        lastChild = NULL;
        while( 1 ){
            HWND child = FindWindowEx( NULL, lastChild, NULL, NULL );
            if( NULL == child ){
                break;
            }
            printf( "child=0x%X\n", child );
            HWND lastChild2 = NULL;
            while( 1 ){
                HWND child2 = FindWindowEx( child, lastChild2, TEXT( "Static" ), NULL );
                if( NULL == child2 ){
                    break;
                }
                printf( "child2=0x%X\n", child2 );
                TCHAR obtained[MAX_PATH];
                if( GetWindowText( child2, obtained, MAX_PATH ) ){
                    printf( "obtained=%s\n", obtained );
                    if( strstr( obtained, TEXT( "Max:" ) ) &&
                        strstr( obtained, TEXT( "[notes]" ) ) ){
                        hDialog = child;
                        break;
                    }
                }
                lastChild2 = child2;
            }
            if( NULL != hDialog ){
                break;
            }
            lastChild = child;
        }
        Sleep( 200 );
    }
    printf( "hDialog=0x%X\n", hDialog );
    *hLyricDialog = hDialog;

    // テキストボックスを特定する
    ret = FindWindowEx( hDialog, NULL, TEXT( "Edit" ), NULL );
    printf( "hLyricTextbox=0x%X\n", ret );

    return ret;
}

int main(){
    init();

    if( NULL == hMainWindow ){
        MessageBox( NULL, TEXT( "VOCALOID Editorが起動していないようです" ), TEXT( "AutoLyricInsert" ), 0 );
        return 0;
    }

    FILE *fp = fopen( "data.txt", "r" );
    if( !fp ){
        MessageBox( NULL, TEXT( "data.txtが見つかりません" ), TEXT( "AutoLyricInsert" ), 0 );
        printf( "file 'data.txt' not found\n" );
        return 0;
    }
    const int BUFLEN = 512;
    TCHAR line[BUFLEN];
    int file_number = 0;
    while( 1 ){
        file_number++;
        HWND hLyricDialog = NULL;
        HWND hTextbox = GetLyricTextBoxHandle( &hLyricDialog );
        if( NULL == hTextbox ){
            MessageBox( NULL, TEXT( "歌詞を流し込む相手先のテキストボックスが見つかりません" ), TEXT( "AutoLyricInsert" ), 0 );
            if( fp ){
                fclose( fp );
                return 0;
            }
        }
        int i;
        BOOL eof = FALSE;
        for( i = 0; i < WORDS_PER_FILE; i++ ){
            if( fgets( line, BUFLEN, fp ) ){
                int len = strlen( line );
                int j;
                for( j = 0; j < len; j++ ){
                    PostMessage( hTextbox, WM_CHAR, line[j], 0 );
                }
            }else{
                eof = TRUE;
                break;
            }
        }
        Sleep( 500 );

        // OKボタンを押す
        PostMessage( hLyricDialog, WM_COMMAND, IDOK, 0 );
        Sleep( 500 );

        // 名前を付けて保存
        PostMessage( hMainWindow, WM_COMMAND, idMenuFileSaveNamed, 0 );

        // 名前を付けて保存のダイアログのHWNDを探す
        HWND hSaveFileDialog = NULL;
        int wait_ms = 100;
        int max_wait_ms = 1 * 60 * 1000;
        int max_count = max_wait_ms / wait_ms;
        int count = 0;
        while( hSaveFileDialog == NULL && count < max_count ){
            hSaveFileDialog = FindWindowEx( NULL, NULL, NULL, TEXT( "名前を付けて保存" ) );
            Sleep( wait_ms );
            count++;
        }
        if( NULL == hSaveFileDialog ){
            printf( "error; save file dialog not found\n" );
            break;
        }
        HWND child = FindWindowEx( hSaveFileDialog, NULL, TEXT( "ComboBoxEx32" ), NULL );
        if( NULL == child ){
            printf( "error; ComboBoxEx32 not found in dialog: hSaveFileDialog=0x%X\n", hSaveFileDialog );
            break;
        }
        HWND child2 = FindWindowEx( child, NULL, TEXT( "ComboBox" ), NULL );
        if( NULL == child2 ){
            printf( "error; ComboBox not found in dialog: hSaveFileDialog=0x%X; child=0x%X\n", hSaveFileDialog, child );
            break;
        }
        HWND hFileName = FindWindowEx( child2, NULL, TEXT( "Edit" ), NULL );
        if( NULL == hFileName ){
            printf( "error; Edit not found in dialog: hSaveFileDialog=0x%X; child=0x%X; child2=0x%X\n", hSaveFileDialog, child, child2 );
        }
        printf( "hFileName=0x%X\n", hFileName );

        // ファイル名を打ち込む
        TCHAR fname[MAX_PATH];
        sprintf( fname, "%d.vsq", file_number );
        int len = strlen( fname );
        for( i = 0; i < len; i++ ){
            PostMessage( hFileName, WM_CHAR, fname[i], 0 );
        }

        // ＯＫを押す
        PostMessage( hSaveFileDialog, WM_COMMAND, IDOK, 0 );

        // ウィンドウのタイトルが 「略%d.vsq*」から「略%d.vsq」に変わるまで待機
        TCHAR title[MAX_PATH];
        sprintf( title, "VOCALOID Editor - %d.vsq  ", file_number );
        printf( "title=%s\n", title );
        HWND hTemp = NULL;
        while( hTemp != hMainWindow ){
            Sleep( 100 );
            // シーケンスが大きいため保存に時間がかかりますダイアログ
            HWND hTakeSomeMinutesDialog = FindWindowEx( NULL, NULL, NULL, TEXT( "VOCALOID Editor" ) );
            if( hTakeSomeMinutesDialog ){
                PostMessage( hTakeSomeMinutesDialog, WM_COMMAND, IDOK, 0 );
            }
            hTemp = FindWindowFromTitle( NULL, title );
        }

        if( eof ){
            break;
        }
    }

    fclose( fp );

    SetWindowPos( hMainWindow, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE );
    MessageBox( NULL, TEXT( "完了しました\nご協力ありがとうございました(。＿。)" ), TEXT( "AutoLyricInsert" ), 0 );
}
