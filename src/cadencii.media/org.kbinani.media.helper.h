#include <windows.h>
#include <stdio.h>

#ifdef __cplusplus
extern "C" {
#endif

void SoundInit();
int SoundPrepare( int sample_rate );
void SoundAppend( double *left, double *right, int length );
void SoundExit();
double SoundGetPosition();
bool SoundIsBusy();
void SoundWaitForExit();
void SoundSetResolution( int resolution );
void SoundKill();
void SoundUnprepare();
void CALLBACK SoundCallback( HWAVEOUT hwo, unsigned int uMsg, unsigned long dwInstance, unsigned long dwParam1, unsigned long dwParam2 );

#define NUM_BUF 3

#ifdef __cplusplus
}
#endif
