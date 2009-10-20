package com.boare.windows.forms;

import com.boare.*;
import javax.swing.*;
import java.awt.event.*;
import java.lang.reflect.*;

public class BForm extends JFrame implements WindowListener{
    public BEvent formClosingEvent = new BEvent();
    public BEvent formClosedEvent = new BEvent();
    public BEvent activatedEvent = new BEvent();
    public BEvent deactivateEvent = new BEvent();
    public BEvent loadEvent = new BEvent();

    public BForm(){
        this( "" );
    }

    public BForm( String title ){
        super( title );
        addWindowListener( this );
    }

    public void windowActivated( WindowEvent e ){
        try{
            activatedEvent.raise( this, e );
        }catch( Exception ex ){
            System.out.println( "BForm#windowActivated; ex=" + ex );
        }
    }

    public void windowClosed( WindowEvent e ){
        try{
            formClosedEvent.raise( this, e );
        }catch( Exception ex ){
            System.out.println( "BForm#windowClosed; ex=" + ex );
        }
    }

    public void windowClosing( WindowEvent e ){
        try{
            formClosingEvent.raise( this, e );
        }catch( Exception ex ){
            System.out.println( "BForm#windowClosing; ex=" + ex );
        }
    }

    public void windowDeactivated( WindowEvent e ){
        try{
            deactivateEvent.raise( this, e );
        }catch( Exception ex ){
            System.out.println( "BForm#windowDeactivated; ex=" + ex );
        }
    }

    public void windowDeiconified( WindowEvent e ){
    }

    public void windowIconified( WindowEvent e ){
    }

    public void windowOpened( WindowEvent e ){
        try{
            loadEvent.raise( this, e );
        }catch( Exception ex ){
            System.out.println( "BForm#windowOpened; ex=" + ex );
        }
    }
}
