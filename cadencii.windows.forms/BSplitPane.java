package com.github.cadencii.windows.forms;

import java.awt.Component;
import java.awt.Dimension;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import javax.swing.JSplitPane;

public class BSplitPane extends JSplitPane implements ComponentListener {
    private static final long serialVersionUID = -7485943135051893345L;
    private boolean panel2Hidden = false;
    private boolean panel1Hidden = false;

    public BSplitPane(){
        super();
        addComponentListener( this );
    }

    public boolean isPanel1Hidden()
    {
        return panel1Hidden;
    }
    
    public void setPanel1Hidden( boolean value )
    {
        panel1Hidden = value;
    }
    
    public boolean isPanel2Hidden()
    {
        return panel2Hidden;
    }
    
    public void setPanel2Hidden( boolean value )
    {
        panel2Hidden = value;
    }

    public boolean isSplitterFixed()
    {
        return !super.isEnabled();
    }
    
    public void setSplitterFixed( boolean value )
    {
        super.setEnabled( !value );
    }
    
    public void setPanel1MinSize( int value )
    {
        Component left = super.getLeftComponent();
        Dimension old_minsize = left.getMinimumSize();
        if( orientation == JSplitPane.HORIZONTAL_SPLIT ){
            left.setMinimumSize( new Dimension( value, old_minsize.height ) );
        }else{
            left.setMinimumSize( new Dimension( old_minsize.width, value ) );
        }
    }

    public int getPanel1MinSize()
    {
        if( orientation == JSplitPane.HORIZONTAL_SPLIT ){
            return super.getLeftComponent().getMinimumSize().width;
        }else{
            return super.getLeftComponent().getMinimumSize().height;
        }
    }
    
    public void setPanel2MinSize( int value )
    {
        Component right = super.getRightComponent();
        Dimension old_minsize = right.getMinimumSize();
        if( orientation == JSplitPane.HORIZONTAL_SPLIT ){
            right.setMinimumSize( new Dimension( value, old_minsize.height ) );
        }else{
            right.setMinimumSize( new Dimension( old_minsize.width, value ) );
        }
    }
    
    public int getPanel2MinSize(){
        if( orientation == JSplitPane.HORIZONTAL_SPLIT ){
            return super.getRightComponent().getMinimumSize().width;
        }else{
            return super.getRightComponent().getMinimumSize().height;
        }
    }

    public void componentHidden(ComponentEvent e) {
    }

    public void componentMoved(ComponentEvent e) {
    }

    public void componentResized(ComponentEvent e) {
        if( panel2Hidden ){
            this.setDividerLocation( this.getHeight() );
        }else if( panel1Hidden ){
            this.setDividerLocation( 0 );
        }
    }

    public void componentShown(ComponentEvent e) {
    }

}
