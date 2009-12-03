package org.kbinani.windows.forms;

import java.awt.Component;
import java.awt.Dimension;
import javax.swing.JSplitPane;

public class BSplitPane extends JSplitPane {
    private static final long serialVersionUID = -7485943135051893345L;

    public boolean isSplitterFixed(){
        return super.isEnabled();
    }
    
    public void setSplitterFixed( boolean value ){
        super.setEnabled( value );
    }
    
    public void setPanel1MinSize( int value ){
        Component left = super.getLeftComponent();
        Dimension old_minsize = left.getMinimumSize();
        if( orientation == JSplitPane.HORIZONTAL_SPLIT ){
            left.setMinimumSize( new Dimension( value, old_minsize.height ) );
        }else{
            left.setMinimumSize( new Dimension( old_minsize.width, value ) );
        }
    }

    public int getPanel1MinSize(){
        if( orientation == JSplitPane.HORIZONTAL_SPLIT ){
            return super.getLeftComponent().getMinimumSize().width;
        }else{
            return super.getLeftComponent().getMinimumSize().height;
        }
    }
    
    public void setPanel2MinSize( int value ){
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
}
