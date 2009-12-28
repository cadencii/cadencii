#if JAVA
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BTimer.java
#else
using System;

namespace org.kbinani.windows.forms {

    public class BTimer : System.Windows.Forms.Timer {
        // root impl of Click event
        #region event impl Tick
        // root impl of Click event is in BTimer
        public BEvent<BEventHandler> tickEvent = new BEvent<BEventHandler>();
        protected override void OnTick( EventArgs e ) {
            base.OnTick( e );
            tickEvent.raise( this, e );
        }
        #endregion

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
