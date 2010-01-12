using System;
using org.kbinani.media;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using Integer = System.Int32;

    public class WaveReceiver : IWaveReceiver {
        private int sampleRate;
        private int m_trim_remain;
        private boolean m_abort_required = false;
        private Object m_locker = new Object();
        private long totalSamples = 0;
        private long m_total_append = 0;
        private int renderingTrack;
        private boolean reflectAmp2Wave = true;
        private WaveWriter waveWriter;
        private int waveReadOffsetSeconds;
        private Vector<WaveRateConverter> readers;
        private boolean directPlay;

        public WaveReceiver( int sample_rate, int trim_remain ) {
            sampleRate = sample_rate;
            m_trim_remain = trim_remain;
        }

        public int getSampleRate() {
            return sampleRate;
        }

        public void append( double[] t_L, double[] t_R ) {
            lock ( m_locker ) {

                double[] L = t_L;
                double[] R = t_R;
                if ( m_trim_remain > 0 ) {
                    if ( L.Length <= m_trim_remain ) {
                        m_trim_remain -= L.Length;
                        return;
                    } else {
                        L = new double[t_L.Length - m_trim_remain];
                        R = new double[t_L.Length - m_trim_remain];
                        for ( int i = m_trim_remain; i < t_L.Length; i++ ) {
                            if ( m_abort_required ) return;
                            L[i - m_trim_remain] = t_L[i];
                            R[i - m_trim_remain] = t_R[i];
                        }
                        m_trim_remain = 0;
                    }
                }
                int length = L.Length;
                if ( length > totalSamples - m_total_append ) {
                    length = (int)(totalSamples - m_total_append);
                    if ( length <= 0 ) {
                        return;
                    }
                    double[] br = R;
                    double[] bl = L;
                    L = new double[length];
                    R = new double[length];
                    for ( int i = 0; i < length; i++ ) {
                        if ( m_abort_required ) return;
                        L[i] = bl[i];
                        R[i] = br[i];
                    }
                    br = null;
                    bl = null;
                }

                AmplifyCoefficient amplify = AppManager.getAmplifyCoeffNormalTrack( renderingTrack );
                if ( reflectAmp2Wave ) {
                    for ( int i = 0; i < length; i++ ) {
                        if ( m_abort_required ) return;
                        if ( i % 100 == 0 ) {
                            amplify = AppManager.getAmplifyCoeffNormalTrack( renderingTrack );
                        }
                        L[i] = L[i] * amplify.left;
                        R[i] = R[i] * amplify.right;
                    }
                    if ( waveWriter != null ) {
                        try {
                            waveWriter.append( L, R );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "RenderingRunner#waveIncoming; ex=" + ex );
                        }
                    }
                } else {
                    if ( waveWriter != null ) {
                        try {
                            waveWriter.append( L, R );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "RenderingRunner#waveIncoming; ex=" + ex );
                        }
                    }
                    for ( int i = 0; i < length; i++ ) {
                        if ( m_abort_required ) return;
                        if ( i % 100 == 0 ) {
                            amplify = AppManager.getAmplifyCoeffNormalTrack( renderingTrack );
                        }
                        L[i] = L[i] * amplify.left;
                        R[i] = R[i] * amplify.right;
                    }
                }
                long start = m_total_append + (long)(waveReadOffsetSeconds * VSTiProxy.SAMPLE_RATE);

                int count = readers.size();
                double[] reader_r = new double[length];
                double[] reader_l = new double[length];
                for ( int i = 0; i < count; i++ ) {
                    try {
                        WaveRateConverter wr = readers.get( i );
                        amplify.left = 1.0;
                        amplify.right = 1.0;
                        if ( wr.getTag() != null && wr.getTag() is Integer ) {
                            int track = (Integer)wr.getTag();
                            if ( 0 < track ) {
                                amplify = AppManager.getAmplifyCoeffNormalTrack( track );
                            } else if ( 0 > track ) {
                                amplify = AppManager.getAmplifyCoeffBgm( -track - 1 );
                            }
                        }
                        wr.read( start, length, reader_l, reader_r );
                        for ( int j = 0; j < length; j++ ) {
                            if ( m_abort_required ) return;
                            L[j] += reader_l[j] * amplify.left;
                            R[j] += reader_r[j] * amplify.right;
                        }
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "RenderingRunner_DRAFT#waveIncoming; ex=" + ex );
                    }
                }
                reader_l = null;
                reader_r = null;
                if ( directPlay ) {
                    PlaySound.append( L, R, L.Length );
                }
                m_total_append += length;
                for ( int i = 0; i < t_L.Length; i++ ) {
                    t_L[i] = 0.0;
                    t_R[i] = 0.0;
                }
            }
        }
    }

}
