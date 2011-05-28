package org.kbinani.windows.forms;

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Component;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import javax.swing.JFrame;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import javax.swing.ListSelectionModel;
import javax.swing.event.ListSelectionEvent;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableCellEditor;
import javax.swing.table.TableCellRenderer;
import javax.swing.table.TableColumn;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

class TestBListView extends JFrame
{
    private static final long serialVersionUID = 1L;

    public static void main( String[] args )
    {
        TestBListView f = new TestBListView();
        System.out.println( "main" );

        f.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        f.setVisible( true );
    }
    
    public TestBListView(){
        BListView b = new BListView();
        b.setColumnHeaders( new String[]{ "1A", "2A" } );
        b.addRow( new String[]{ "one", "two" } );
        b.addRow( new String[]{ "I", "II" } );
        b.setColumnWidth( 0, 120 );
        b.setColumnHeaders( new String[]{ "1B", "2B" } );
        b.setCheckBoxes( true );
        JScrollPane sp = new JScrollPane(b);
        //sp.setPreferredSize(new Dimension(250, 90));

        //JPanel p = new JPanel();
        //p.add(sp);

        
        getContentPane().add(sp, BorderLayout.CENTER);
    }
}

public class BListView extends JTable
                       implements KeyListener
{
    private static final long serialVersionUID = 3654926773702743627L;

    public final BEvent<BEventHandler> selectedIndexChangedEvent = new BEvent<BEventHandler>();
    private final int FIRST_COLUMN_WIDTH = 25;
    private DefaultTableModel mModel = null;
    private boolean mMultiSelect = false;
    private boolean mCheckBoxes = true;
    
    public BListView()
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
        addKeyListener( this );
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
    
    // root impl of KeyListener is in BButton
    public final BEvent<BPreviewKeyDownEventHandler> previewKeyDownEvent = new BEvent<BPreviewKeyDownEventHandler>();
    public final BEvent<BKeyEventHandler> keyDownEvent = new BEvent<BKeyEventHandler>();
    public final BEvent<BKeyEventHandler> keyUpEvent = new BEvent<BKeyEventHandler>();
    public final BEvent<BKeyPressEventHandler> keyPressEvent = new BEvent<BKeyPressEventHandler>();
    public void keyPressed( KeyEvent e ) {
        try{
            previewKeyDownEvent.raise( this, new BPreviewKeyDownEventArgs( e ) );
            keyDownEvent.raise( this, new BKeyEventArgs( e ) );
        }catch( Exception ex ){
            System.err.println( "BButton#keyPressed; ex=" + ex );
        }
    }
    public void keyReleased(KeyEvent e) {
        try{
            keyUpEvent.raise( this, new BKeyEventArgs( e ) );
        }catch( Exception ex ){
            System.err.println( "BButton#keyReleased; ex=" + ex );
        }
    }
    public void keyTyped(KeyEvent e) {
        try{
            previewKeyDownEvent.raise( this, new BPreviewKeyDownEventArgs( e ) );
            keyPressEvent.raise( this, new BKeyPressEventArgs( e ) );
        }catch( Exception ex ){
            System.err.println( "BButton#keyType; ex=" + ex );
        }
    }

    public void valueChanged( ListSelectionEvent e ){
        this.repaint();
        try{
            selectedIndexChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BListView#valueChanged; ex=" + ex );
        }
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

