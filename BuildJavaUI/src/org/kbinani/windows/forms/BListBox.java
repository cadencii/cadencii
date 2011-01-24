package org.kbinani.windows.forms;

import javax.swing.DefaultListModel;
import javax.swing.JList;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import org.kbinani.*;

public class BListBox extends JList
                      implements ListSelectionListener
{
    private static final long serialVersionUID = 3749301116724119106L;
    private DefaultListModel mModel = null;
    
    public BListBox(){
        super();
        mModel = new DefaultListModel();
        super.setModel( mModel );
        addListSelectionListener( this );
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

    /* root impl of SelectedIndexChanged event */
    // root impl of SelectedIndexChanged event is in BListBox
    public BEvent<BEventHandler> selectedIndexChangedEvent = new BEvent<BEventHandler>();
    public void valueChanged(ListSelectionEvent e) {
        try{
            selectedIndexChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BListBox#valueChanged; ex=" + ex );
        }
    }
}
