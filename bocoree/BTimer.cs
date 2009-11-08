#if JAVA
package org.kbinani.windows.forms;

import java.awt.*;
import java.awt.event.*;
import javax.swing.*;
import org.kbinani.*;

public class BTimer extends Timer implements ActionListener {
    public BEvent<BEventHandler> tickEvent = new BEvent<BEventHandler>();

    public BTimer(){
        super( 100, null );
        addActionListener( this );
    }
    
    public void actionPerformed( ActionEvent e ){
        try{
            tickEvent.raise( new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BTimer#actionPerformed; ex=" + ex );
        }
    }
}
#else
using System;

namespace bocoree.windows.forms {

    public class BTimer : System.Windows.Forms.Timer {
        public void start() {
            base.Start();
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
    }

}
#endif
