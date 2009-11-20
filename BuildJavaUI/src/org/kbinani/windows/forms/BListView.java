package org.kbinani.windows.forms;

import java.awt.Component;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import java.util.Vector;
import javax.swing.DefaultCellEditor;
import javax.swing.JCheckBox;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import javax.swing.ListSelectionModel;
import javax.swing.table.DefaultTableCellRenderer;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableCellRenderer;
import javax.swing.table.TableColumn;
import javax.swing.table.TableColumnModel;

public class BListView extends JPanel{
    private static final long serialVersionUID = -8159742081426120737L;
    private DefaultTableModel tmodel;
    private boolean isMultiSelect = true;
    private JScrollPane scrollPane = null;
    private JTable table = null;
    private boolean isCheckBoxes = false;
    private boolean isColumnModelUpdated = true;
    private final int FIRST_COLUMN_WIDTH = 25;
    private Vector<SubTable> subTables = new Vector<SubTable>();

    private class SubTable extends JTable{
        public DefaultTableModel tableModel;
        
        public SubTable(){
            super();
            tableModel = new DefaultTableModel(){
                             public boolean isCellEditable(int row, int column) {
                                 if( isCheckBoxes && column == 0 ){
                                     return true;
                                 }else{
                                     return false;
                                 }
                             }
                         }; 
            setModel( tableModel );
            setAutoResizeMode( JTable.AUTO_RESIZE_OFF );
        }
    }
    
    private class CheckCellRenderer extends DefaultTableCellRenderer{
        public CheckCellRenderer(){
            super();
        }

        public Component getTableCellRendererComponent( JTable table,
                                                        Object value,
                                                        boolean isSelected,
                                                        boolean hasFocus,
                                                        int row,
                                                        int column ){
            if( value == null ){
                return this;
            }

            Component col = null;
            String text = "";
            if( value != null  ){
                if( value instanceof String ){
                    text = (String)value;
                }else if( value instanceof Boolean ){
                    if( (Boolean)value ){
                        text = "true";
                    }
                }
            }
            if( column == 0 ){
                if( isCheckBoxes ){
                    JCheckBox select = new JCheckBox();
                    Boolean val = false;
                    if( text.toLowerCase().equals( "true" ) ){
                        val = true;
                    }
                    select.setSelected( val.booleanValue() );
                    col = select;
                }else{
                    return null;
                }
            }else{
                JLabel cell = new JLabel();
                col = cell;
            }
            return(col);
        }
    }

    public BListView(){
        super();
        initialize();
        getTable().setModel( getTModel() );
        getTable().setAutoResizeMode( JTable.AUTO_RESIZE_OFF );
    }

    public DefaultTableModel getTModel(){
        if( tmodel == null ){
            tmodel = new DefaultTableModel(){
                         public boolean isCellEditable(int row, int column) {
                             if( isCheckBoxes && column == 0 ){
                                 return true;
                             }else{
                                 return false;
                             }
                         }
                     };
        }
        return tmodel;
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

    public boolean isCheckBoxes(){
        return isCheckBoxes;
    }
    
    public void setCheckBoxes( boolean value ){
        if( isCheckBoxes != value ){
            isCheckBoxes = value;
            if( tmodel.getColumnCount() > 0 ){
                updateColumnModel();
            }else{
                isColumnModelUpdated = false;
            }
        }
        System.out.println( "isCheckBoxes=" + isCheckBoxes );
        System.out.println( "isColumnModelUpdated=" + isColumnModelUpdated );
    }
    
    private void updateColumnModel(){
        TableColumn column = table.getColumnModel().getColumn( 0 );
        column.setCellEditor( new DefaultCellEditor( new JCheckBox() ) );
        column.setCellRenderer( new CheckCellRenderer() );
        column.setResizable( true );
        column.setPreferredWidth( FIRST_COLUMN_WIDTH );
        column.setResizable( false );
        table.getTableHeader().setReorderingAllowed( false );
        isColumnModelUpdated = true;
    }
    
    /*public void setItemAt( int index, BListViewItem value ){
        int count = value.getSubItemCount();
        tmodel.setValueAt( value.isSelected(), index, 0 );
        for( int i = 0; i < count; i++ ){
            tmodel.setValueAt( value.getSubItemAt( i ), index, i + 1 );
        }
    }*/

    public void removeElementAt( int index ){
        tmodel.removeRow( index );
    }
    
    public BListViewItem getItemAt( int index ){
        /*int columns = tmodel.getColumnCount() - 1;
        String[] sub = new String[columns];
        for( int i = 0; i < columns; i++ ){
            sub[i] = (String)tmodel.getValueAt( index, i + 1 );
        }*/
        return new BListViewItem( tmodel, index );
    }
    
    public int getItemCount(){
        return tmodel.getRowCount();
    }
    
    public void addItem( BListViewItem item ){
        if( tmodel.getColumnCount() <= 0 ){
            tmodel.setColumnCount( item.getSubItemCount() + 1 );
        }
        int columns = Math.min( tmodel.getColumnCount() - 1, item.getSubItemCount() );
        Object[] sub = new Object[columns + 1];
        sub[0] = false;
        for ( int i = 0; i < columns; i++ ){
            sub[i + 1] = item.getSubItemAt( i );
        }
        boolean isFirst = tmodel.getColumnCount() <= 0;
        tmodel.addRow( sub );
        if( isFirst ){
            table.getColumnModel().getColumn( 0 ).setPreferredWidth( FIRST_COLUMN_WIDTH );
        }
        if ( !isColumnModelUpdated ){
            updateColumnModel();
        }
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
        int count = tmodel.getColumnCount();
        String[] actual = new String[count];
        actual[0] = "";
        for( int i = 0; i < headers.length && i + 1 < count; i++ ){
            actual[i + 1] = headers[i];
        }
        tmodel.setColumnIdentifiers( actual );
        updateColumnModel();
    }

    public String[] getColumnHeaders() {
        int len = tmodel.getColumnCount();
        String[] ret = new String[len - 1];
        for( int i = 1; i < len; i++ ){
            ret[i - 1] = tmodel.getColumnName( i );
        }
        return ret;
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
