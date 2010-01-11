namespace org.kbinani.media.impl {

    /// <summary>
    /// 接頭辞b: 単位が変換前のサンプル数になっている変数
    /// 接頭辞a: 単位が変換後のサンプル数になっている変数
    /// </summary>
    public class WaveRateConvertAdapter {
        private IWaveReceiver receiver = null;
        private long bCount = 0; // 受け取ったデータの個数
        private long aCount = 0; // receiverに送ったデータの個数
        private int bRate = 44100;
        private int aRate = 44100;
        private double invBRate = 1.0 / 44100.0;
        private double invARate = 1.0 / 44100.0;

        public WaveRateConvertAdapter( IWaveReceiver receiver, int sample_rate ) {
            this.receiver = receiver;
            bRate = sample_rate;
            aRate = receiver.getSampleRate();
            invBRate = 1.0 / (double)bRate;
            invARate = 1.0 / (double)aRate;
        }

        public void append( double[] left, double[] right ) {
            if ( aRate == bRate ) {
                receiver.append( left, right );
                aCount += left.Length;
                bCount += left.Length;
                return;
            }

            double secStart = (bCount + 1) * invBRate;
            double secEnd = (bCount + left.Length) * invBRate;

            // 送られてきたデータで、aStartからaEndまでのデータを作成できる
            long aStart = (long)(secStart * aRate) + 1;
            long aEnd = (long)(secEnd * aRate);
            double[] aLeft = new double[aEnd - aCount + 1];
            double[] aRight = new double[aEnd - aCount + 1];
            for ( long a = aCount; a <= aEnd; a++ ) {
                if ( a < aStart ) {

                } else {
                    double x = a * invARate;
                    long bRequired = (long)(x * bRate);
                    double x0 = bRequired * invBRate;
                    double x1 = (bRequired + 1) * invBRate;

                    double y0 = left[bRequired - bCount];
                    double y1 = left[bRequired - bCount + 1];
                    double s = (y1 - y0) / (x1 - x0);
                    double y = y0 + s * (x - x0);
                    aLeft[a - aCount] = y;

                    y0 = right[bRequired - bCount];
                    y1 = right[bRequired - bCount + 1];
                    s = (y1 - y0) / (x1 - x0);
                    y = y0 + s * (x - x0);
                    aRight[a - aCount] = y;
                }
            }

            receiver.append( aLeft, aRight );
        }
    }

    public interface IWaveReceiver {
        void append( double[] left, double[] right );
        int getSampleRate();
    }

    public class WaveRateConverter : IWaveReceiver {
        public int getSampleRate() {
            return 0;
        }

        public void append( double[] left, double[] right ) {
        }
    }

}
