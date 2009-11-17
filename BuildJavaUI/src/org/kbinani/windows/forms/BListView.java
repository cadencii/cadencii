package org.kbinani.windows.forms;

import javax.swing.JTable;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableModel;

public class BListView extends JTable{
    private TableModel tmodel;

    public BListView(){
        super();
        setModel( getTModel() );
    }

    public int getItemCount(){
        return getTModel().getColumnCount();
    }
    
    private TableModel getTModel(){
        if( tmodel == null ){
            tmodel = new DefaultTableModel();
        }
        return tmodel;
    }
}
