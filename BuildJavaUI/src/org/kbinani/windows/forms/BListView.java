package org.kbinani.windows.forms;

import java.awt.Color;
import java.awt.Component;
import java.awt.Dimension;
import java.awt.Frame;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseMotionListener;
import java.util.Vector;
import javax.swing.DefaultCellEditor;
import javax.swing.JCheckBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import javax.swing.ListSelectionModel;
import javax.swing.border.EmptyBorder;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import javax.swing.event.TableModelEvent;
import javax.swing.event.TableModelListener;
import javax.swing.table.DefaultTableCellRenderer;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableCellEditor;
import javax.swing.table.TableCellRenderer;
import javax.swing.table.TableColumn;
import javax.swing.table.TableColumnModel;
import javax.swing.table.TableModel;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;
import java.awt.BorderLayout;

class TestBListView extends JFrame
{
    public static void main( String[] args )
    {
        TestBListView f = new TestBListView();
        System.out.println( "main" );

        f.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        f.setVisible( true );
    }
    
    public TestBListView(){
        BListView b = new BListView();
        JScrollPane sp = new JScrollPane(b);
        b.setColumnHeaders( new String[]{ "1", "2" } );
        b.fixLeftColumn();
        b.addItem( new String[]{ "one", "two" } );
        b.addItem( new String[]{ "I", "II" } );
        //sp.setPreferredSize(new Dimension(250, 90));

        //JPanel p = new JPanel();
        //p.add(sp);

        
        getContentPane().add(sp, BorderLayout.CENTER);
    }
}

class __BListView extends JPanel
                       implements KeyListener
{
    public BEvent<BEventHandler> selectedIndexChangedEvent = new BEvent<BEventHandler>();
    private final int FIRST_COLUMN_WIDTH = 25;
    private DefaultTableModel mModel = null;
    private boolean mMultiSelect = false;
    private boolean mCheckBoxes = false;
    private JScrollPane mScroll;
    private JTable mTable;
    
    public __BListView()
    {
        super( new BorderLayout() );
        mModel = new DefaultTableModel()
        {
            public Class<?> getColumnClass( int column ){
                if( mCheckBoxes && column == 0 ){
                    return Boolean.class;
                }else{
                    return String.class;
                }
            }
    
            public boolean isCellEditable( int row, int column )
            {
                return mCheckBoxes && (column == 0);
            }
        };
        mTable = new JTable( mModel ){
            public Component prepareEditor( TableCellEditor editor, int row, int column )
            {
                if( mCheckBoxes && column == 0 ){
                    JCheckBox c = new JCheckBox();
                    return c;
                }else{
                    return super.prepareEditor( editor, row, column );
                }
            }
            
            public Component prepareRenderer( TableCellRenderer renderer, int row, int column )
            {
                if( mCheckBoxes && column == 0 ){
                    JCheckBox c = new JCheckBox();
                    return c;
                }else{
                    return super.prepareRenderer( renderer, row, column );
                }
            }
        };
        mTable.setAutoResizeMode( JTable.AUTO_RESIZE_OFF );
        mTable.addComponentListener( new ComponentListener (){
            public void componentHidden(ComponentEvent e) {}
            public void componentMoved(ComponentEvent e) {};
            public void componentResized(ComponentEvent e) {
                mScroll.setPreferredSize( getSize() );
            };
            public void componentShown(ComponentEvent e) {};
        } );
        addColumnCore();
        TableColumn column = mTable.getColumnModel().getColumn( 0 );
        mTable.getTableHeader().setReorderingAllowed( false );
        mTable.addKeyListener( this );
        mScroll = new JScrollPane( mTable );
        mScroll.setHorizontalScrollBarPolicy( JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS );
        mScroll.setVerticalScrollBarPolicy( JScrollPane.VERTICAL_SCROLLBAR_ALWAYS );
        this.add( mScroll, BorderLayout.CENTER );
        
        this.addComponentListener( new ComponentListener(){
            public void componentHidden(ComponentEvent e) {};
            public void componentMoved(ComponentEvent e) {};
            public void componentResized(ComponentEvent e) {
                //mTable.setPreferredScrollableViewportSize( mScroll.  )
                //mTable.setPreferredSize( getSize() );
                //mTable.setPreferredScrollableViewportSize( getSize() );
            };
            public void componentShown(ComponentEvent e) {};
        } );
    }
    
    private void addColumnCore()
    {
        mModel.addColumn( "" );
        fixLeftColumn();
    }
    
    public void fixLeftColumn()
    {
        if( mTable.getColumnModel().getColumnCount() <= 0 ){
            return;
        }
        TableColumn column = mTable.getColumnModel().getColumn( 0 );
        mTable.setAutoResizeMode( JTable.AUTO_RESIZE_OFF );
        column.setPreferredWidth( FIRST_COLUMN_WIDTH );
        column.setResizable( false );
    }
    
    // root impl of KeyListener is in BButton
    public BEvent<BPreviewKeyDownEventHandler> previewKeyDownEvent = new BEvent<BPreviewKeyDownEventHandler>();
    public BEvent<BKeyEventHandler> keyDownEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyEventHandler> keyUpEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyPressEventHandler> keyPressEvent = new BEvent<BKeyPressEventHandler>();
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
        try{
            selectedIndexChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BListView#valueChanged; ex=" + ex );
        }
    }
    
    public void ensureRowVisible( int row )
    {
        mTable.scrollRectToVisible( mTable.getCellRect( row, 0, true ) );
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
        ListSelectionModel model = mTable.getSelectionModel();
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
    
    public void addItem( String[] items, boolean selected )
    {
        System.out.println( "addItem; before" );
        printData();
        Object[] data = new Object[items.length + 1];
        data[0] = selected;//selected;
        for( int i = 0; i < items.length; i++ ){
            data[i + 1] = items[i];
        }
        if( mModel.getColumnCount() < data.length ){
            for( int i = mModel.getColumnCount(); i < data.length; i++ ){
                addColumnCore();
            }
        }
        System.out.println( "addItem; data=" );
        for( int i = 0; i < data.length; i++ ){
            System.out.println( "    \"" + data[i] + "\"" );
        }
        mModel.addRow( data );
        System.out.println( "addItem; before" );
        printData();
    }
    
    public void addItem( String[] items )
    {
        addItem( items, false );
    }
    
    public boolean isMultiSelect()
    {
        return mMultiSelect;
    }
    
    public void setMultiSelect( boolean value )
    {
        mMultiSelect = value;
        if( mMultiSelect ){
            mTable.setSelectionMode( ListSelectionModel.MULTIPLE_INTERVAL_SELECTION );
        }else{
            mTable.setSelectionMode( ListSelectionModel.SINGLE_SELECTION );
        }
    }
    
    public void setColumnHeaders( String[] headers )
    {
        String[] act = new String[headers.length + 1];
        for( int i = mTable.getColumnCount(); i < act.length; i++ ){
            addColumnCore();
        }
        act[0] = "";
        for( int i = 0; i < headers.length; i++ ){
            act[i + 1] = headers[i];
        }
        mModel.setColumnIdentifiers( act );
    }
    
    public String[] getColumnHeaders()
    {
        int count = mModel.getColumnCount();
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
        mTable.getColumnModel().getColumn( column + 1 ).setWidth( width );
    }
    
    public int getColumnWidth( int column )
    {
        return mTable.getColumnModel().getColumn( column + 1 ).getWidth();
    }
    
    public boolean isCheckBoxes()
    {
        return mCheckBoxes;
    }
    
    public void setCheckBoxes( boolean value )
    {
        mCheckBoxes = value;
    }
}

public class BListView extends JTable
                       implements KeyListener
{
    private static final long serialVersionUID = 3654926773702743627L;

    public BEvent<BEventHandler> selectedIndexChangedEvent = new BEvent<BEventHandler>();
    private final int FIRST_COLUMN_WIDTH = 25;
    private DefaultTableModel mModel = null;
    private boolean mMultiSelect = false;
    private boolean mCheckBoxes = true;
    
    public BListView()
    {
        super();
        mModel = new DefaultTableModel()
        {
            public Class<?> getColumnClass( int column ){
                if( mCheckBoxes && (column == 0) ){
                    return Boolean.class;
                }else{
                    return String.class;
                }
            }
            
            public boolean isCellEditable( int row, int column )
            {
                return mCheckBoxes && (column == 0);
            }
        };
        final BListView t = this;
        /*mModel.addTableModelListener( new TableModelListener(){
            public void tableChanged( TableModelEvent e )
            {
                if(e.getType()==TableModelEvent.UPDATE) {
                    t.repaint();
                }
            }
        } );*/
        setModel( mModel );
        setAutoResizeMode( JTable.AUTO_RESIZE_OFF );
        addColumnCore();
        fixLeftColumn();
        TableColumn column = this.getColumnModel().getColumn( 0 );
        //column.setCellEditor( new DefaultCellEditor( new JCheckBox() ) );
        //column.setCellRenderer( new CheckCellRenderer() );
        getTableHeader().setReorderingAllowed( false );
        //column.setMaxWidth( FIRST_COLUMN_WIDTH );
        //column.setMinWidth( FIRST_COLUMN_WIDTH );
        //column.setResizable( true );
        //column.setPreferredWidth( FIRST_COLUMN_WIDTH );
        //column.setResizable( false );
        addKeyListener( this );
    }

    public Component prepareEditor( TableCellEditor editor, int row, int column )
    {
        Component cmp = super.prepareEditor( editor, row, column );
        if( convertColumnIndexToModel( column ) == 0 ){
            
        }
        return cmp;
        /*if( column == 0 ){
            if( mCheckBoxes ){
                JCheckBox c = new JCheckBox();
                Object obj = mModel.getValueAt( row, column );
                boolean v = false;
                if( obj instanceof Boolean ){
                    v = (boolean)((Boolean)obj);
                }
                c.setSelected( v );
                return c;
            }else{
                return null;
            }
        }else{
            return super.prepareEditor( editor, row, column );
        }*/
    }
    
    public Component prepareRenderer( TableCellRenderer renderer, int row, int column )
    {
        Component cmp = super.prepareRenderer( renderer, row, column );
        return cmp;
        /*if( column == 0 ){
            if( mCheckBoxes ){
                JCheckBox c = new JCheckBox();
                Object obj = mModel.getValueAt( row, column );
                boolean v = false;
                if( obj instanceof Boolean ){
                    v = (boolean)((Boolean)obj);
                }
                c.setSelected( v );
                return c;
            }else{
                return null;
            }
        }else{
            return super.prepareRenderer( renderer, row, column );
        }*/
    }
    
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
    
    public void fixLeftColumn()
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
    public BEvent<BPreviewKeyDownEventHandler> previewKeyDownEvent = new BEvent<BPreviewKeyDownEventHandler>();
    public BEvent<BKeyEventHandler> keyDownEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyEventHandler> keyUpEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyPressEventHandler> keyPressEvent = new BEvent<BKeyPressEventHandler>();
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
    
    public void addItem( String[] items, boolean selected )
    {
        System.out.println( "addItem; before" );
        printData();
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
        System.out.println( "addItem; data=" );
        for( int i = 0; i < data.length; i++ ){
            System.out.println( "    \"" + data[i] + "\"" );
        }
        mModel.addRow( data );
        System.out.println( "addItem; before" );
        printData();
    }
    
    public void addItem( String[] items )
    {
        addItem( items, false );
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
        String[] act = new String[headers.length + 1];
        for( int i = super.getColumnCount(); i < act.length; i++ ){
            addColumnCore();
        }
        act[0] = "";
        for( int i = 0; i < headers.length; i++ ){
            act[i + 1] = headers[i];
        }
        mModel.setColumnIdentifiers( act );
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
        this.getColumnModel().getColumn( column + 1 ).setWidth( width );
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

