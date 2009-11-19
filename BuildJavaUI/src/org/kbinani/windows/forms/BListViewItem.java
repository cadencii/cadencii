package org.kbinani.windows.forms;

import java.util.Vector;
import javax.swing.table.TableModel;

public class BListViewItem implements Cloneable{
    private Vector<String> subItems = new Vector<String>();
    private Object tag;
    protected TableModel tmodel = null;
    protected int row;
    private boolean isSelected = false;
    
    public BListViewItem( String[] values ){
        subItems.clear();
        for( int i = 0; i < values.length; i++ ){
            subItems.add( values[i] );
        }
    }

    protected BListViewItem( TableModel tmodel, int row ){
        this.tmodel = tmodel;
        this.row = row;
        int count = tmodel.getColumnCount() - 1;
        subItems.clear();
        for( int i = 0; i < count; i++ ){
            subItems.add( tmodel.getValueAt( row, i + 1 ) + "" );
        }
    }
    
    public Object clone(){
        updateStatusFromTableModel();
        int count = subItems.size();
        String[] values = new String[count];
        for( int i = 0; i < count; i++ ){
            values[i] = subItems.get( i );
        }
        BListViewItem ret = new BListViewItem( values );
        ret.isSelected = isSelected();
        return ret;
    }
    
    public boolean isSelected(){
        if( tmodel != null ){
            isSelected = (Boolean)tmodel.getValueAt( row, 0 );
        }
        return isSelected;
    }

    public void setSelected( boolean value ){
        if( tmodel != null ){
            tmodel.setValueAt( value, row, 0 );
        }
        isSelected = value;
    }
    
    public Object getTag(){
        return tag;
    }
    
    public void setTag( Object value ){
        tag = value;
    }
    
    public int getSubItemCount(){
        updateStatusFromTableModel();
        return subItems.size();
    }

    private void updateStatusFromTableModel(){
        if( tmodel == null ){
            return;
        }
        isSelected = (Boolean)tmodel.getValueAt( row, 0 );
        int count = tmodel.getColumnCount() - 1;
        if( subItems.size() != count ){
            subItems.clear();
            subItems.setSize( count );
        }
        for( int i = 0; i < count; i++ ){
            subItems.set( i, tmodel.getValueAt( row, i + 1 ) + "" );
        }
    }
    
    public String getSubItemAt( int index ){
        updateStatusFromTableModel();
        return subItems.get( index );
    }

    public void setSubItemAt( int index, String value ){
        updateStatusFromTableModel();
        subItems.set( index, value );
    }

}
