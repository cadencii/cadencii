using org.kbinani.java.util;

namespace org.kbinani.vsq {
    using boolean = System.Boolean;

    public class TempoVector : Vector<TempoTableEntry> {
        private boolean updated = false;

        public TempoVector()
            : base() {
        }

        public void updateTempoInfo() {

        }

        /*public double getSecFromClock() {
        }*/
    }

}
