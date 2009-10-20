package com.boare.windows.forms;

import java.swing.*;
import com.boare.*;

public class BButton extends JButton implements ActionListener{
    public BEvent clickedEvent = new BEvent();

    public BButton(){
        addActionListener( this );
    }

    public void actionPerformed( ActionEvent e ){
        try{
            clickedEvent.raise( this, null );
        }catch( Exception ex ){
            System.out.println( "BButton#actionPerformed; ex=" + ex );
        }
    }
}
