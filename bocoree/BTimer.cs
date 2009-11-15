#if JAVA
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BTimer.java
#else
using System;

namespace bocoree.windows.forms {

    public class BTimer : System.Windows.Forms.Timer {
        public void start() {
            base.Start();
        }

        public BTimer()
            : base() {
        }

        public BTimer( System.ComponentModel.IContainer container )
            : base( container ) {
        }

        public void stop() {
            base.Stop();
        }

        public int getDelay() {
            return base.Interval;
        }

        public void setDelay( int value ) {
            base.Interval = value;
        }

        public bool isRunning() {
            return base.Enabled;
        }
    }

}
#endif
