#include "winmmhelp.h"

namespace Boare{ namespace Cadencii{

    unsigned int winmmhelp::s_num_joydev = 0;
    bool *winmmhelp::s_joy_attatched;
    int *winmmhelp::s_button_num;
    bool winmmhelp::s_initialized = false;
    int *winmmhelp::s_joy_available;

    int winmmhelp::Init(){
        if( s_initialized ){
            Reset();
        }
        s_initialized = true;
        int num_joydev = joyGetNumDevs();
        if ( num_joydev <= 0 ) {
            num_joydev = 0;
            return num_joydev;
        }
        s_joy_attatched = new bool[num_joydev];
        s_button_num = new int[num_joydev];
        int count = 0;
        for( int k = 0; k < num_joydev; k++ ){
            JOYINFO ji;
            if( joyGetPos( k, &ji ) == JOYERR_NOERROR ){
                s_joy_attatched[k] = true;
                JOYCAPS jc;
                joyGetDevCaps( k, &jc, sizeof( JOYCAPS ) );
                s_button_num[k] = jc.wNumButtons;
                count++;
            }else{
                s_joy_attatched[k] = false;
                s_button_num[k] = 0;
            }
        }
        if( count > 0 ){
            s_joy_available = new int[count];
            int c = -1;
            for( int i = 0; i < num_joydev; i++ ){
                if( s_joy_attatched[i] ){
                    c++;
                    if( c >= count ){
                        break; //Ç±Ç±Ç…óàÇÈÇÃÇÕÉGÉâÅ[
                    }
                    s_joy_available[c] = i;
                }
            }
        }
        s_num_joydev = count;
        return s_num_joydev;
    };

    bool winmmhelp::IsJoyAttatched( int index ){
        if( !s_initialized ){
            Init();
        }
        if( s_num_joydev == 0 || index < 0 || (int)s_num_joydev <= index ){
            return false;
        }
        return s_joy_attatched[index];
    };
    
    bool winmmhelp::GetStatus( int index_, unsigned char *buttons, int len, int *pov ){
        const int _BTN[32] = { 0x0001,
                               0x0002,
                               0x0004,
                               0x0008,
                               0x00000010l,
                               0x00000020l,
                               0x00000040l,
                               0x00000080l,
                               0x00000100l,
                               0x00000200l,
                               0x00000400l,
                               0x00000800l,
                               0x00001000l,
                               0x00002000l,
                               0x00004000l,
                               0x00008000l,
                               0x00010000l,
                               0x00020000l,
                               0x00040000l,
                               0x00080000l,
                               0x00100000l,
                               0x00200000l,
                               0x00400000l,
                               0x00800000l,
                               0x01000000l,
                               0x02000000l,
                               0x04000000l,
                               0x08000000l,
                               0x10000000l,
                               0x20000000l,
                               0x40000000l,
                               0x80000000l };
        if( !s_initialized ){
            return false;
        }
        if( s_num_joydev == 0 || index_ < 0 || (int)s_num_joydev <= index_ ){
            return false;
        }
        int index = s_joy_available[index_];
        JOYINFOEX ji_ex;
        ji_ex.dwSize = sizeof( JOYINFOEX );
        ji_ex.dwFlags = JOY_RETURNPOV | JOY_RETURNBUTTONS;

        if( s_joy_attatched[index] ){
            joyGetPosEx( index, &ji_ex );
            *pov = ji_ex.dwPOV;
            if( *pov == 0xffff ){
                *pov = -1;
            }
            for( int i = 0; i < len && i < s_button_num[index]; i++ ){
                buttons[i] = (ji_ex.dwButtons & _BTN[i]) ? 0x80 : 0x00;
            }
            return true;
        }else{
            return false;
        }
    };

    int winmmhelp::GetNumButtons( int index ){
        if( !s_initialized ){
            Init();
        }
        if( s_num_joydev == 0 || index < 0 || (int)s_num_joydev <= index ){
            return 0;
        }
        return s_button_num[s_joy_available[index]];
    };

    void winmmhelp::Reset(){
        if( s_initialized ){
            delete [] s_button_num;
            delete [] s_joy_attatched;
            delete [] s_joy_available;
            s_initialized = false;
            s_num_joydev = 0;
        }
    };

    int winmmhelp::GetNumJoyDev(){
        if( !s_initialized ){
            return Init();
        }else{
            return s_num_joydev;
        }
    };

} }

/*int main(){
    Boare::Cadencii::winmmhelp::Init();
    std::cout << "main" << std::endl;
    unsigned char *status;
    int num_joy = Boare::Cadencii::winmmhelp::Init();
    int num_button;
    if( num_joy <= 0 ){
        std::cout << "num_joy=" << num_joy << std::endl;
        goto final;
    }
    std::cout << "num_joy=" << num_joy << std::endl;
    for( int k = 0; k < num_joy; k++ ){
        std::cout << "winmmhelp::IsJoy" << k << "Attatched=" << Boare::Cadencii::winmmhelp::IsJoyAttatched( k ) << std::endl;
    }
    num_button = Boare::Cadencii::winmmhelp::GetNumButtons( 0 );
    std::cout << "num_button=" << num_button << std::endl;
    if( num_button <= 0 ){
        goto final;
    }
    status = new unsigned char[num_button];
    int pov;
    while( true ){
        Boare::Cadencii::winmmhelp::GetStatus( 0, status, num_button, &pov );
        std::cout << "\r" << std::dec << std::setw( 5 ) << pov << std::hex;
        for( int i = 0; i < num_button; i++ ){
            int v = status[i];
            std::cout << " 0x" << std::setw( 2 ) << std::setfill( '0' ) << v;
        }
    }
final:
    if( status ){
        delete [] status;
    }
    int i;
    std::cin >> i;
    return 0;
}*/
