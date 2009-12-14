#if JAVA
package org.kbinani.media;

import java.io.*;
import javax.sound.sampled.*;

public class BSoundPlayer{
    private static final int BUFLEN = 128000;
    private AudioInputStream m_stream = null;
    private boolean m_stop_required = false;
    private boolean m_playing = false;
    private String m_sound_location = "";

    public BSoundPlayer( String sound_location ){
        setSoundLocation( sound_location );
    }

    public BSoundPlayer(){
    }

    public String getSoundLocation(){
        return m_sound_location;
    }

    public void setSoundLocation( String value ){
        stop();
        m_sound_location = value;
        if( m_stream != null ){
            try{
                m_stream.close();
            }catch( Exception ex0 ){
                System.err.println( "BSoundPlayer#setSoundLocation; ex0=" + ex0 );
            }
        }
        try {
            File sound_file = new File( m_sound_location );
            m_stream = AudioSystem.getAudioInputStream( sound_file );
        }catch( Exception ex ){
            System.err.println( "BSoundPlayer#.ctor; ex=" + ex );
        }
    }

    public void play(){
        try{
            Thread t = new Thread( new PlaySoundRunner() );
            t.start();
            t.join();
        }catch( Exception ex ){
            System.out.println( "BSoundPlayer#play; ex=" + ex );
        }
    }

    public void stop(){
        if( m_playing ){
            try{
                m_stop_required = true;
                while( m_playing ){
                    Thread.sleep( 0 );
                }
                m_stop_required = false;
            }catch( Exception ex ){
                System.err.println( "BSoundPlayer#stop; ex=" + ex );
            }
        }
    }

    public class PlaySoundRunner implements Runnable{
        public void run(){
            SourceDataLine line = null;
            m_playing = true;
            try{
                AudioFormat audioFormat = m_stream.getFormat();

                DataLine.Info info = new DataLine.Info( SourceDataLine.class, audioFormat );
                line = (SourceDataLine)AudioSystem.getLine( info );
                line.open( audioFormat );
                line.start();

                int len = 1;
                byte[] buf = new byte[BUFLEN];
                while( len > 0 && !m_stop_required ){
                    len = m_stream.read( buf, 0, buf.length );
                    if( len >= 0 ){
                        int nBytesWritten = line.write( buf, 0, len );
                    }else{
                        break;
                    }
                }
            } catch( Exception ex ){
                System.err.println( "BSoundPlayer.PlaySoundRunner#run; ex=" + ex );
            } finally {
                if( line != null ){
                    try{
                        line.drain();
                        line.close();
                    }catch( Exception ex2 ){
                        System.err.println( "BSoundPlayer.PlaySoundRunner#run; ex2=" + ex2 );
                    }
                }
            }
            m_playing = false;
        }
    }
}
#else
namespace org.kbinani.media {

    public class BSoundPlayer : System.Media.SoundPlayer {
        public BSoundPlayer( string sound_location )
            : base( sound_location ) {
        }

        public BSoundPlayer()
            : base() {
        }

        public void play() {
            base.Play();
        }

        public string getSoundLocation() {
            return base.SoundLocation;
        }

        public void setSoundLocation( string value ) {
            base.SoundLocation = value;
        }
    }

}
#endif
