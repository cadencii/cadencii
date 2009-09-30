
#ifndef __winmmhlp_h__
#define __winmmhlp_h__

#include <windows.h>
#include <mmsystem.h>
#pragma comment(lib, "winmm.lib")
#include <iostream>
#include <iomanip>
#include <string>

namespace Boare{ namespace Cadencii{

class winmmhelp{
private:
    // ゲームパッドの個数
    static unsigned int s_num_joydev;
    // 各ゲームパッドが接続されているかどうか
    static bool *s_joy_attatched;
    // 各ゲームパッドが認識できるボタンの個数
    static int *s_button_num;
    // 初期化されているかどうか
    static bool s_initialized;
    // 接続されているゲームパッドのインデクスのリスト
    static int *s_joy_available;
public:
    // 初期化。戻り値は、接続されているゲームパッドの個数
    static int Init();
    // 第index番目のゲームパッドが接続されているかどうかを調べる
    static bool IsJoyAttatched( int index );
    // 第index番目のゲームパッドの状態を取得する。
    static bool GetStatus( int index, unsigned char *buttons, int len, int *pov );
    // 第index番目のゲームパッドが認識できるボタンの個数を取得する。
    static int GetNumButtons( int index );
    // リセット。
    static void Reset();
    static int GetNumJoyDev();
};

} }

#endif
