package org.kbinani.windows.forms;

import javax.swing.JPanel;
import javax.swing.JTable;
import javax.swing.ListSelectionModel;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableModel;
import java.awt.Dimension;
import javax.swing.JScrollPane;
import java.awt.GridBagLayout;
import java.awt.GridBagConstraints;
import java.awt.Insets;
import java.awt.FlowLayout;

public class BListView extends JPanel{
    private DefaultTableModel tmodel;
    private boolean isMultiSelect = true;
    private JScrollPane scrollPane = null;
    private JTable table = null;
    
    public BListView(){
        super();
        initialize();
        getTable().setModel( getTModel() );
    }

    public void initialize(){
        GridBagConstraints gridBagConstraints = new GridBagConstraints();
        gridBagConstraints.fill = GridBagConstraints.BOTH;
        gridBagConstraints.gridx = 0;
        gridBagConstraints.gridy = 0;
        gridBagConstraints.weightx = 1.0;
        gridBagConstraints.weighty = 1.0;
        gridBagConstraints.insets = new Insets(0, 0, 0, 0);
        this.setLayout(new GridBagLayout());
        this.setSize(new Dimension(231, 192));
        this.add(getScrollPane(), gridBagConstraints);
        
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
    
    public void addItem( BListViewItem item ){
        if( tmodel.getColumnCount() <= 0 ){
            tmodel.setColumnCount( item.getSubItemCount() );
        }
        int columns = Math.min( tmodel.getColumnCount(), item.getSubItemCount() );
        String[] sub = new String[columns];
        for ( int i = 0; i < columns; i++ ){
            sub[i] = item.getSubItemAt( i );
        }
        tmodel.addRow( sub );
    }
    
    public boolean isMultiSelect(){
        return isMultiSelect;
    }
    
    public void setMultiSelect( boolean value ){
        isMultiSelect = value;
        if( isMultiSelect ){
            table.setSelectionMode( ListSelectionModel.MULTIPLE_INTERVAL_SELECTION );
        }else{
            table.setSelectionMode( ListSelectionModel.SINGLE_SELECTION );
        }
    }
    
    public void setColumnHeaders( String[] headers ) {
        tmodel.setColumnIdentifiers( headers );
    }

    public String[] getColumnHeaders() {
        int len = tmodel.getColumnCount();
        String[] ret = new String[len];
        for( int i = 0; i < len; i++ ){
            ret[i] = tmodel.getColumnName( i );
        }
        return ret;
    }

    private DefaultTableModel getTModel(){
        if( tmodel == null ){
            tmodel = new DefaultTableModel(){
                public boolean isCellEditable(int row, int column) {
                    return false;
                }
            };
        }
        return tmodel;
    }

    /**
     * This method initializes jScrollPane	
     * 	
     * @return javax.swing.JScrollPane	
     */
    private JScrollPane getScrollPane() {
        if (scrollPane == null) {
            scrollPane = new JScrollPane();
            scrollPane.setViewportView(getTable());
        }
        return scrollPane;
    }

    /**
     * This method initializes jTable	
     * 	
     * @return javax.swing.JTable	
     */
    private JTable getTable() {
        if (table == null) {
            table = new JTable();
            table.setShowGrid(false);
        }
        return table;
    }
}  //  @jve:decl-index=0:visual-constraint="10,10"
