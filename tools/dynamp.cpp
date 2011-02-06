#include <stdio.h>
#include <math.h>
#include <io.h>
#include <fcntl.h>

int main( int argc, char *argv[] )
{
    _setmode( _fileno( stdin ), _O_BINARY );
    _setmode( _fileno( stdout ), _O_BINARY );
    int l0, l1, r0, r1;
    const double HZ = 0.2;
    const double RATE = 44100.0;
    const double PI2 = 3.141592653589793238462643383279 * 2.0;
    unsigned long indx = 0;
    while( (l0 = getchar()) != EOF &&
           (l1 = getchar()) != EOF &&
           (r0 = getchar()) != EOF &&
           (r1 = getchar()) != EOF ){
        short l = (short)(l0 << 8 | (0xff & l1));
        short r = (short)(r0 << 8 | (0xff & r1));
        double vl = l / 32768.0;
        double vr = r / 32768.0;
        double t = indx / RATE;
        double amp = 1.0 * sin( t * HZ * PI2 );
        vl *= amp;
        vr *= amp;
        short sv = (short)(vl * 32768.0);
        unsigned char b0 = (unsigned char)(0xff & (sv >> 8));
        unsigned char b1 = (unsigned char)(0xff & sv);
        putchar( b0 );
        putchar( b1 );

        sv = (short)(vr * 32768.0);
        b0 = (unsigned char)(0xff & (sv >> 8));
        b1 = (unsigned char)(0xff & sv);
        putchar( b0 );
        putchar( b1 );
        indx++;
    }
}
