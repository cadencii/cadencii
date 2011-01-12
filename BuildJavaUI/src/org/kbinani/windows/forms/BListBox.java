package org.kbinani.windows.forms;

import javax.swing.DefaultListModel;
import javax.swing.JList;

public class BListBox extends JList{
    private DefaultListModel mModel = null;
    
    public BListBox(){
        super();
        mModel = new DefaultListModel();
        super.setModel( mModel );
    }
    
    public int getItemCount(){
        return mModel.getSize();
    }

    public Object getItemAt( int index ){
        return mModel.getElementAt( index );
    }

    public void setItemAt( int index, Object item ){
        mModel.setElementAt( item, index );
    }

    public void removeItemAt( int index ){
        mModel.removeElementAt( index );
    }

    public void addItem( Object item ){
        mModel.addElement( item );
    }
}
