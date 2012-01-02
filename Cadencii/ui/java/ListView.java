package com.github.cadencii.ui;

import java.awt.Color;
import javax.swing.JTable;
import javax.swing.ListSelectionModel;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableColumn;

public class ListView extends JTable
{
    private static final long serialVersionUID = -5007676758622022217L;
    private final int FIRST_COLUMN_WIDTH = 25;
    private DefaultTableModel mModel = null;
    private boolean mMultiSelect = false;
    private boolean mCheckBoxes = true;

    public ListView()
    {
        super();
        mModel = new DefaultTableModel()
        {
            private static final long serialVersionUID = 1444372218140865006L;

            public Class<?> getColumnClass( int column ){
                if( column == 0 ){
                    if( mCheckBoxes )
                    {
                        return Boolean.class;
                    }
                    else
                    {
                        return Object.class;
                    }
                }else{
                    return String.class;
                }
            }

            public Object getValueAt( int row, int column )
            {
                if( column == 0 )
                {
                    if( mCheckBoxes )
                    {
                        return super.getValueAt( row, column );
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return super.getValueAt( row, column );
                }
            }

            public boolean isCellEditable( int row, int column )
            {
                return mCheckBoxes && (column == 0);
            }
        };
        setModel( mModel );
        setAutoResizeMode( JTable.AUTO_RESIZE_OFF );
        addColumnCore();
        fixLeftColumn();
        getTableHeader().setReorderingAllowed( false );
        showHorizontalLines = false;
        showVerticalLines = false;
    }

    /*public Component prepareEditor( TableCellEditor editor, int row, int column )
    {
        if( !mCheckBoxes && convertColumnIndexToModel( column ) == 0 ){
            return null;
        }else{
            return super.prepareEditor( editor, row, column );
        }
    }

    public Component prepareRenderer( TableCellRenderer renderer, int row, int column )
    {
        if( !mCheckBoxes && convertColumnIndexToModel( column ) == 0 ){
            return null;
        }else{
            return super.prepareRenderer( renderer, row, column );
        }
    }*/

    public int getItemCountRow()
    {
        return mModel.getRowCount();
    }

    public int getItemCountColumn()
    {
        return mModel.getColumnCount();
    }

    private void addColumnCore()
    {
        mModel.addColumn( "" );
        fixLeftColumn();
    }

    private void fixLeftColumn()
    {
        if( super.getColumnModel().getColumnCount() <= 0 ){
            return;
        }
        TableColumn column = super.getColumnModel().getColumn( 0 );
        column.setResizable( true );
        setAutoResizeMode( JTable.AUTO_RESIZE_OFF );
        column.setPreferredWidth( mCheckBoxes ? FIRST_COLUMN_WIDTH : 0 );
        column.setResizable( false );
    }


    public void ensureRowVisible( int row )
    {
        scrollRectToVisible( getCellRect( row, 0, true ) );
    }

    public void setRowBackColor( int row, Color color )
    {

    }

    public void clear()
    {
        mModel.setRowCount( 0 );
    }

    public void setItemAt( int row, int column, String item )
    {
        mModel.setValueAt( item, row, column + 1 );
    }

    public String getItemAt( int row, int column ){
        Object obj = mModel.getValueAt( row, column + 1 );
        if( obj == null ){
            return "";
        }else if( obj instanceof String ){
            return (String)obj;
        }else{
            return "";
        }
    }

    public boolean isRowChecked( int row )
    {
        boolean ret = (Boolean)mModel.getValueAt( row, 0 );
        return ret;
    }

    public void setRowChecked( int row, boolean value )
    {
        Boolean v = value;
        mModel.setValueAt( v, row, 0 );
    }

    public void setSelectedRow( int row )
    {
        ListSelectionModel model = getSelectionModel();
        model.clearSelection();
        model.addSelectionInterval( row, row );
    }

    @SuppressWarnings("unused")
    private void printData()
    {
        System.out.println( "BListView#printData; mModel.getColumnCount()=" + mModel.getColumnCount()+ "; mModel.getRowCount()=" + mModel.getRowCount() );
        for( int row = 0; row < mModel.getRowCount(); row++ ){
            for( int column = 0; column < mModel.getColumnCount(); column++ ){
                System.out.print( (column == 0 ? "" : ",") + "\"" + mModel.getValueAt( row, column ) + "\"" );
            }
            System.out.println();
        }
    }

    public void addRow( String[] items, boolean selected )
    {
        Object[] data = new Object[items.length + 1];
        data[0] = Boolean.valueOf( selected );//selected;
        for( int i = 0; i < items.length; i++ ){
            data[i + 1] = items[i];
        }
        if( mModel.getColumnCount() < data.length ){
            for( int i = mModel.getColumnCount(); i < data.length; i++ ){
                addColumnCore();
            }
        }
        mModel.addRow( data );
    }

    public void addRow( String[] items )
    {
        addRow( items, false );
    }

    public void removeRow( int index )
    {
        mModel.removeRow( index );
    }

    public boolean isMultiSelect()
    {
        return mMultiSelect;
    }

    public void setMultiSelect( boolean value )
    {
        mMultiSelect = value;
        if( mMultiSelect ){
            setSelectionMode( ListSelectionModel.MULTIPLE_INTERVAL_SELECTION );
        }else{
            setSelectionMode( ListSelectionModel.SINGLE_SELECTION );
        }
    }

    public void setColumnHeaders( String[] headers )
    {
System.out.println( "BListView#setColumnHeaders; before; width is..." );
for( int i = 0; i < super.getColumnModel().getColumnCount(); i++ ){
    TableColumn col = super.getColumnModel().getColumn( i );
    System.out.println( "    #" + i + "; " + col.getWidth() + "; resizable=" + col.getResizable() );
}
        String[] act = new String[headers.length + 1];
        for( int i = super.getColumnCount(); i < act.length; i++ ){
            addColumnCore();
        }
        act[0] = "";
        getColumnModel().getColumn( 0 ).setHeaderValue( "" );
        for( int i = 0; i < headers.length; i++ ){
            getColumnModel().getColumn( i + 1 ).setHeaderValue( headers[i] );
            //act[i + 1] = headers[i];
        }
        //mModel.setColumnIdentifiers( act );
//        mModel.get
System.out.println( "BListView#setColumnHeaders; after; width is..." );
for( int i = 0; i < super.getColumnModel().getColumnCount(); i++ ){
    TableColumn col = super.getColumnModel().getColumn( i );
    System.out.println( "    #" + i + "; " + col.getWidth() + "; resizable=" + col.getResizable() );
}
        fixLeftColumn();
    }

    public String[] getColumnHeaders()
    {
        int count = getColumnCount();
        if( count <= 0 ){
            return new String[0];
        }
        String[] ret = new String[count - 1];
        for( int i = 0; i < count - 1; i++ ){
            ret[i] = mModel.getColumnName( i + 1 );
        }
        return ret;
    }

    public void setColumnWidth( int column, int width )
    {
System.out.println( "BListView#setColumnWidth; width=" + width + "; before; width=" + getColumnWidth( column ) );
        this.getColumnModel().getColumn( column + 1 ).setPreferredWidth( width );
System.out.println( "BListView#setColumnWidth; width=" + width + "; after; width=" + getColumnWidth( column ) );
    }

    public int getColumnWidth( int column )
    {
        return getColumnModel().getColumn( column + 1 ).getWidth();
    }

    public boolean isCheckBoxes()
    {
        return mCheckBoxes;
    }

    public void setCheckBoxes( boolean value )
    {
        mCheckBoxes = value;
        if( super.getColumnCount() < 1 ){
            addColumnCore();
        }
        fixLeftColumn();
    }
}
