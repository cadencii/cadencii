#include <stdio.h>

static FILE *logger = NULL;

void prepareLogger(){
    if( logger == NULL ){
        logger = fopen( "resampler10.log", "w" );
    }
}

int exec( char *c1, char *c2, char *c3, char *c4, char *c5, char *c6, char *c7, char *c8, 
           char *c9, char *c10, char *c11, char *c12, char *c13, float f1, int i0/*,
           long int l1, int i3, int i4, int i5, int i6/*, int i7, int i8, int i9, int i10*/ ){
    prepareLogger();
    fprintf( logger, "exec; c1=%s\n", c1 );
    fprintf( logger, "      c2=%s\n", c2 );
    fprintf( logger, "      c3=%s\n", c3 );
    fprintf( logger, "      c4=%s\n", c4 );
    fprintf( logger, "      c5=%s\n", c5 );
    fprintf( logger, "      c6=%s\n", c6 );
    fprintf( logger, "      c7=%s\n", c7 );
    fprintf( logger, "      c8=%s\n", c8 );
    fprintf( logger, "      c9=%s\n", c9 );
    fprintf( logger, "      c10=%s\n", c10 );
    fprintf( logger, "      c11=%s\n", c11 );
    fprintf( logger, "      c12=%s\n", c12 );
    fprintf( logger, "      c13=%s\n", c13 );
    fprintf( logger, "      f1=%f\n", f1 );
    fprintf( logger, "      i0=%d\n", i0 );
    /*fprintf( logger, "      l1=%d\n", l1 );
    fprintf( logger, "      d2=%d\n", d2 );
    fprintf( logger, "      i3=%d\n", i3 );
    fprintf( logger, "      i4=%d\n", i4 );
    fprintf( logger, "      i5=%d\n", i5 );
    fprintf( logger, "      i6=%d\n", i6 );
    fprintf( logger, "      i7=%d\n", i7 );
    fprintf( logger, "      i8=%d\n", i8 );
    fprintf( logger, "      i9=%d\n", i9 );
    fprintf( logger, "      i10=%d\n", i10 );*/
    fflush( logger );
    return 0;
}

void setScaleBase(){
    prepareLogger();
    fprintf( logger, "setScaleBase\n" );
    fflush( logger );
}
