package org.kbinani.windows.forms;

import javax.swing.JTable;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableModel;

public class BListView extends JTable{
    private DefaultTableModel tmodel;

    public BListView(){
        super();
        setModel( getTModel() );
    }

    public void setItemAt( int index, BListViewItem value ){
        int count = value.getSubItemCount();
        Object[] sub = new Object[count];
        for( int i = 0; i < count; i++ ){
            value.getSubItemAt( i );
        }
        tmodel.addRow( sub );
    }

    public void removeElementAt( int index ){
        tmodel.removeRow( index );
    }
    
    public BListViewItem getItemAt( int index ){
        int columns = tmodel.getColumnCount();
        String[] sub = new String[columns];
        for( int i = 0; i < columns; i++ ){
            sub[i] = tmodel.getValueAt( index, i ).toString();
        }
        return new BListViewItem( sub );
    }
    
    public int getItemCount(){
        return tmodel.getColumnCount();
    }
    
    private DefaultTableModel getTModel(){
        if( tmodel == null ){
            tmodel = new DefaultTableModel();
        }
        return tmodel;
    }
}
