#if JAVA
package org.kbinani.windows.forms;

import javax.swing.*;
import javax.swing.event.*;
import org.kbinani.*;

public class BSlider extends JSlider implements ChangeListener{
    public BEvent<BEventHandler> valueChangedEvent = new BEvent<BEventHandler>();

    public void stateChanged( ChangeEvent e ){
        try{
            valueChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BSlider#stateChanged; ex=" + ex );
        }
    }
}
#else
namespace bocoree.windows.forms {

    public class BSlider : System.Windows.Forms.TrackBar {
        public int getValue() {
            return base.Value;
        }

        public void setValue( int value ) {
            base.Value = value;
        }
    }

}
#endif
