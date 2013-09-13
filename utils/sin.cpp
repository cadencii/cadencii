#include <math.h>
#include <stdio.h>
#include <io.h>
#include <fcntl.h>

int main( int argc, char *argv[] )
{
    if( _setmode( _fileno( stdout ), _O_BINARY ) == -1 ){
        return -1;
    }
    const double HZ = 220.0;
    const double RATE = 44100.0;
    const double I_RATE = 1.0 / RATE;
    const double AMP = 0.5;
    const double PI2 = 3.141592653589793238462643383279 * 2.0;
    unsigned long indx = 0;
    while( true ){
        double v = AMP * sin( indx * I_RATE * HZ * PI2 );
        indx++;
        short sv = (short)(v * 32768.0);
        unsigned char b0 = (unsigned char)(0xff & (sv >> 8));
        unsigned char b1 = (unsigned char)(0xff & sv);
//printf( "v=%e\n", v );
        putchar( b0 );
        putchar( b1 );
        putchar( b0 );
        putchar( b1 );
    }
    return 0;
}
