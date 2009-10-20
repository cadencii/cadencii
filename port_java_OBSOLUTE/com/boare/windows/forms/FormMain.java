package com.boare.windows.forms;

import com.boare.*;
import javax.swing.*;
import java.awt.event.*;

public class FormMain extends BForm{
    public FormMain(){
        initializeComponent();
    }

    void initializeComponent(){
        try{
            formClosingEvent.add( new BEventHandler( this, "FormMain_formClosing", Object.class, WindowEvent.class ) );
        }catch( Exception ex ){
        }
    }
    
    public void FormMain_formClosing( Object sender, WindowEvent e ){
        System.out.println( "FormMain#FormMain_formClosing" );
    }

    public static void main( String[] args ) throws Exception{
        FormMain form = new FormMain();
        form.setDefaultCloseOperation( JFrame.EXIT_ON_CLOSE );
        form.setBounds( 10, 10, 300, 200 );
        form.setVisible( true );
    }
}
