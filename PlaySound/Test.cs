using System.Runtime.InteropServices;
using System;
using System.IO;

class Test{

    public static void Main( string[] args ){
        int size = 44100;
        PlaySound.Init( size, size );
        double[] left = new double[size];
        double[] right = new double[size];
        
        int freq = 441;
        for( int i = 0; i < size; i++ ){
            double t = i / (double)size;
            double v = 0.2 * Math.Sin( 2.0 * 3.14159 * freq * t );
            left[i] = v;
            right[i] = v;
        }
        for( int i = 0; i < 5; i++ ){
            PlaySound.Append( left, right, size );
        }
        PlaySound.WaitForExit();
    }

}

static class PlaySound {
    [DllImport( "PlaySound.dll" )]
    private static extern void SoundInit( int block_size, int sample_rate );

    [DllImport( "PlaySound.dll" )]
    private static extern unsafe void SoundAppend( double* left, double* right, int length );

    [DllImport( "PlaySound.dll" )]
    private static extern void SoundWaitForExit();

    [DllImport( "PlaySound.dll" )]
    private static extern double SoundGetPosition();

    [DllImport( "PlaySound.dll" )]
    private static extern bool SoundIsBusy();

    [DllImport( "PlaySound.dll" )]
    private static extern void SoundReset();

    public static void WaitForExit() {
        try {
            SoundWaitForExit();
        } catch {
        }
    }

    public static void Init( int block_size, int sample_rate ) {
        try {
            SoundInit( block_size, sample_rate );
        } catch ( Exception ex ){
            Console.WriteLine( "PlaySound.Init; ex=" + ex );
        }
    }

    public static unsafe void Append( double[] left, double[] right, int length ) {
        try{
            fixed ( double* pl = &left[0] )
            fixed ( double* pr = &right[0] ) {
                SoundAppend( pl, pr, length );
            }
        } catch ( Exception ex ) {
            Console.WriteLine( "PlaySound.Append; ex=" + ex );
        }
    }

    public static double GetPosition() {
        double ret = -1;
        try {
            ret = SoundGetPosition();
        } catch ( Exception ex ) {
        }
        return ret;
    }

    public static void Reset() {
        try {
            SoundReset();
        } catch {
        }
    }
}
