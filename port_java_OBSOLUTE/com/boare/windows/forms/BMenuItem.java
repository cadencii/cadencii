package com.boare.windows.forms;

import javax.swing.*;
import java.awt.event.*;
import com.boare.*;

public class BMenuItem extends JMenuItem implements ActionListener{
    public BEvent clickEvent = new BEvent();

    public BMenuItem(){
        addActionListener( this );
    }

    public void actionPerformed( ActionEvent e ){
        try{
            clickEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BMenuItem#actionPerformed; ex=" + ex );
        }
    }
}
