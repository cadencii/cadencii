package org.kbinani.windows.forms;

import java.awt.Color;
import java.awt.Component;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import java.awt.event.MouseEvent;
import java.awt.event.MouseMotionListener;
import java.util.Vector;
import javax.swing.DefaultCellEditor;
import javax.swing.JCheckBox;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import javax.swing.ListSelectionModel;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import javax.swing.table.DefaultTableCellRenderer;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableColumn;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BListView extends JPanel implements MouseMotionListener{
    private static final long serialVersionUID = 1L;
    private boolean isMultiSelect = true;
    private boolean isCheckBoxes = false;
    private final int FIRST_COLUMN_WIDTH = 25;
    protected Vector<Group> groups = new Vector<Group>();
    protected Group defaultGroup = null;
    //private JScrollPane jScrollPane0 = null;
    private JScrollPane jScrollPane10 = null;
    private JPanel panel = null;
    private JLabel label = null;
    private String[] headers;

    class Group extends JScrollPane implements ListSelectionListener{
        private static final long serialVersionUID = 1L;
        public JTable table;
        public DefaultTableModel tableModel;
        private boolean isColumnModelUpdated = true;
        public JLabel label;
        public Component headerView = null;
        //private boolean isHeaderVisible = true;
        
        public Group( String groupTitle ){
            super();
            setName( groupTitle );
            tableModel = new DefaultTableModel(){
                            /**
                 * 
                 */
                private static final long serialVersionUID = 1L;

                            public boolean isCellEditable(int row, int column) {
                                 if( isCheckBoxes && column == 0 ){
                                     return true;
                                 }else{
                                     return false;
                                 }
                             }
                         }; 
            table = new JTable();
            table.getSelectionModel().addListSelectionListener( this );
            table.setModel( tableModel );
            table.setAutoResizeMode( JTable.AUTO_RESIZE_OFF );
            setPreferredSize( new Dimension( 200, 100 ) );
            setVerticalScrollBarPolicy( JScrollPane.VERTICAL_SCROLLBAR_NEVER );
            setHorizontalScrollBarPolicy( JScrollPane.HORIZONTAL_SCROLLBAR_NEVER );
            setViewportView( table );
            label = new JLabel( groupTitle );
        }

        public BListViewItem getItemAt( int index ){
            int columns = tableModel.getColumnCount() - 1;
            String[] sub = new String[columns];
            for( int i = 0; i < columns; i++ ){
                sub[i] = (String)tableModel.getValueAt( index, i + 1 );
            }
            BListViewItem ret = new BListViewItem( sub );
            return ret;
        }

        public boolean isItemChecked( int index ){
            return (Boolean)tableModel.getValueAt( index, 0 );
        }
        
        public void setItemChecked( int index, boolean value ){
            tableModel.setValueAt( value, index, 0 );
        }
        
        public int getItemCountCor(){
            return tableModel.getRowCount();
        }
        
        public void removeItemAtCor( int index ){
            tableModel.removeRow( index );
        }
        
        public void setItemAtCor( int index, BListViewItem value ){
            int count = value.getSubItemCount();
            tableModel.setValueAt( false, index, 0 );
            for( int i = 0; i < count; i++ ){
                tableModel.setValueAt( value.getSubItemAt( i ), index, i + 1 );
            }
        }

        public String getHeader(){
            return label.getText();
        }
        
        public void setHeader( String value ){
            label.setText( value );
        }
        
        public void addItemCor( BListViewItem item ){
            addItemCor( item, false, true );
        }
        
        public void addItemCor( BListViewItem item, boolean enabled ){
            addItemCor( item, false, enabled );
        }
        
        public void addItemCor( BListViewItem item, boolean checked, boolean enabled ){
            System.err.println( "info; BListView#$Group#addItemCor; not implemented yet; \"enabled\"" );
            if( tableModel.getColumnCount() <= 0 ){
                tableModel.setColumnCount( item.getSubItemCount() + 1 );
            }
            int columns = Math.min( tableModel.getColumnCount() - 1, item.getSubItemCount() );
            Object[] sub = new Object[columns + 1];
            sub[0] = checked;
            for ( int i = 0; i < columns; i++ ){
                sub[i + 1] = item.getSubItemAt( i );
            }
            boolean isFirst = tableModel.getColumnCount() <= 0;
            tableModel.addRow( sub );
            if( isFirst ){
                table.getColumnModel().getColumn( 0 ).setPreferredWidth( FIRST_COLUMN_WIDTH );
            }
            if ( !isColumnModelUpdated ){
                updateColumnModel();
            }
            updatePaneHeight();
        }
        
        private void updatePaneHeight(){
            setPreferredSize( new Dimension( 0, (tableModel.getRowCount() + 1) * (table.getRowHeight() + table.getRowMargin()) + horizontalScrollBar.getHeight() ) );
        }
        
        public void updateColumnModel(){
            if( table.getColumnModel().getColumnCount() > 0 ){
                TableColumn column = table.getColumnModel().getColumn( 0 );
                column.setCellEditor( new DefaultCellEditor( new JCheckBox() ) );
                column.setCellRenderer( new CheckCellRenderer() );
                column.setResizable( true );
                column.setPreferredWidth( FIRST_COLUMN_WIDTH );
                column.setResizable( false );
                table.getTableHeader().setReorderingAllowed( false );
                isColumnModelUpdated = true;
            }else{
                isColumnModelUpdated = false;
            }
        }

        public void setColumnHeaderCor( String[] headers ){
            int count = Math.max( tableModel.getColumnCount(), headers.length + 1 );
            String[] actual = new String[count];
            actual[0] = "";
            for( int i = 1; i < count && i - 1 < headers.length; i++ ){
                actual[i] = headers[i - 1];
            }
            tableModel.setColumnIdentifiers( actual );
            updateColumnModel();
        }
        
        public void setColumnWidth( int index, int width ){
            table.getColumnModel().getColumn( index + 1 ).setPreferredWidth( width );
        }
        
        public int getColumnHeight( int index ){
            return table.getColumnModel().getColumn( index + 1 ).getWidth();
        }

        public void valueChanged( ListSelectionEvent e ){
            handleListSelection( this, e );
        }
    }
    
    private class CheckCellRenderer extends DefaultTableCellRenderer{
        /**
         * 
         */
        private static final long serialVersionUID = 1L;

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
        addGroup( "" );   
    }
    
    public BEvent<BEventHandler> selectedIndexChangedEvent = new BEvent<BEventHandler>();
    private void handleListSelection( Group g, ListSelectionEvent e ){
        int c = groups.size();
        for( int i = 0; i < c; i++ ){
            Group gr = groups.get( i );
            if ( gr != g ){
                gr.table.clearSelection();
            }
        }
        try{
            selectedIndexChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BListView#handleListSelection; ex=" + ex );
        }
    }
    
    public void ensureRowVisible( String group, int row ){
        Group g = getGroupFromName( group );
        g.table.scrollRectToVisible( g.table.getCellRect( row, 0, true ) );
    }

    public void setGroupHeader( String group, String header ){
        Group g = getGroupFromName( group );
        g.setHeader( header );        
    }
    
    public void setItemBackColorAt( String group, int index, Color color ){
        //Group g = getGroupFromName( group );
        // TODO: BListView#setItemBackColorAt
    }
    
    public void clear(){
        clearItems( defaultGroup );
        int count = groups.size();
        for( int i = 0; i < count; i++ ){
            clearItems( groups.get( i ) );
        }
    }
    
    private void clearItems( Group g ){
        int count = g.tableModel.getRowCount();
        for( int i = 0; i < count; i++ ){
            g.tableModel.removeRow( 0 );
        }
    }
    
    public int getSelectedIndex( String group ){
        Group g = getGroupFromName( group );
        return g.table.getSelectedRow();
    }
    
    public void setItemSelectedAt( String group, int index, boolean value ){
        Group g = getGroupFromName( group );
        Vector<Integer> selection = new Vector<Integer>();
        int[] list = g.table.getSelectedRows();
        for( int i = 0; i < list.length; i++ ){
            if( list[i] != index || (list[i] == index && !value) ){
                selection.add( i );
            }
        }
        g.table.clearSelection();
        int count = selection.size();
        for( int i = 0; i < count; i++ ){
            int indx = selection.get( i );
            g.table.addRowSelectionInterval( indx, indx );
        }
    }
    
    public void clearSelection( String group ){
        Group g = getGroupFromName( group );
        g.table.clearSelection();
    }
    
    public void mouseDragged( MouseEvent e ){
        System.out.println( "BListView_DRAFT#mouseDragged" );
        TableColumn column = null;
        Group g = null;
        if( groups.size() == 0 ){
            g = defaultGroup;
        }else{
            g = groups.get( 0 );
        }
        column = g.table.getTableHeader().getResizingColumn();
        if( column == null ){
            return;
        }
        int count = g.table.getColumnModel().getColumnCount();
        int[] w = new int[count];
        for( int i = 0; i < count; i++ ){
            w[i] = g.table.getColumnModel().getColumn( i ).getWidth();
        }
        
        int num_group = groups.size();
        for( int i = 0; i < num_group; i++ ){
            Group gr = groups.get( i );
            updateColumnWidth( w, gr );
        }
        updateColumnWidth( w, defaultGroup );
    }
    
    public void mouseMoved( MouseEvent e ){
    }

    private void updateColumnWidth( int[] width, Group gr ){
        int cols = gr.table.getColumnCount();// Math.min( width.length, gr.table.getColumnCount() );
        int total_width = 0;
        for( int j = 0; j < cols; j++ ){
            gr.table.getColumnModel().getColumn( j ).setPreferredWidth( width[j] );
            total_width += width[j];
        }
        gr.updatePaneHeight();
        //panel.setPreferredSize( new Dimension( total_width, 0 ) );
    }

    public int getGroupCount(){
        return groups.size() + 1;
    }

    public String getGroupNameAt( int index ){
        if( index < groups.size() ){
            return groups.get( index ).getName(); 
        }else{
            return defaultGroup.getName();
        }
    }
    
    public void initialize(){
        GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
        gridBagConstraints2.fill = GridBagConstraints.BOTH;
        gridBagConstraints2.weighty = 1.0;
        gridBagConstraints2.gridx = 0;
        gridBagConstraints2.gridy = 0;
        gridBagConstraints2.weightx = 1.0;
        GridBagConstraints gridBagConstraints = new GridBagConstraints();
        gridBagConstraints.fill = GridBagConstraints.BOTH;
        gridBagConstraints.gridx = 0;
        gridBagConstraints.gridy = 0;
        gridBagConstraints.weightx = 1.0;
        gridBagConstraints.weighty = 1.0;
        gridBagConstraints.insets = new Insets(0, 0, 0, 0);
        this.setLayout(new GridBagLayout());
        this.setSize(new Dimension(365, 179));
        this.add(getJScrollPane10(), gridBagConstraints2);
    }

    public boolean isCheckBoxes(){
        return isCheckBoxes;
    }
    
    public void setCheckBoxes( boolean value ){
        isCheckBoxes = value;
        if( defaultGroup != null ){
            defaultGroup.updateColumnModel();
        }
        int count = groups.size();
        for( int i = 0; i < count; i++ ){
            groups.get( i ).updateColumnModel();
        }
    }
    
    private Group getGroupFromName( String name ){
        if( name == null || (name != null && name.equals( "" ) ) ){
            return defaultGroup;
        }else{
            int count = groups.size();
            for( int i = 0; i < count; i++ ){
                Group g = groups.get( i );
                if( g.getName().equals( name ) ){
                    return g;
                }
            }
            return defaultGroup;
        }
    }
    
    public void setItemAt( String group, int index, BListViewItem value ){
        Group g = getGroupFromName( group );
        g.setItemAtCor( index, value );
    }

    public void removeItemAt( String group, int index ){
        Group g = getGroupFromName( group );
        g.removeItemAtCor( index );
    }
    
    public BListViewItem getItemAt( String group, int index ){
        //Group g = getGroupFromName( group );
        /*int columns = tmodel.getColumnCount() - 1;
        String[] sub = new String[columns];
        for( int i = 0; i < columns; i++ ){
            sub[i] = (String)tmodel.getValueAt( index, i + 1 );
        }*/
        return null;
    }
    
    public int getItemCount( String group ){
        Group g = getGroupFromName( group );
        return g.getItemCountCor();
    }
    
    public boolean isItemCheckedAt( String group, int index ){
        Group g = getGroupFromName( group );
        return g.isItemChecked( index );
    }
    
    public void setItemCheckedAt( String group, int index, boolean value ){
        Group g = getGroupFromName( group );
        g.setItemChecked( index, value );
    }
    
    public Group addGroup( String name ){
        if( name == null || (name != null && name.equals( "" ) ) ){
            if( defaultGroup == null ){
                defaultGroup = new Group( "" );
                defaultGroup.updateColumnModel();
                defaultGroup.table.getTableHeader().addMouseMotionListener( this );
                defaultGroup.setHeader( "Another" );

                GridBagConstraints gcGroupLabel = new GridBagConstraints();
                gcGroupLabel.fill = GridBagConstraints.HORIZONTAL;
                gcGroupLabel.gridx = 0;
                gcGroupLabel.gridy = 0;
                gcGroupLabel.ipadx = 0;
                gcGroupLabel.weightx = 1.0;
                gcGroupLabel.weighty = 0.0D;
                gcGroupLabel.anchor = GridBagConstraints.NORTH;
                panel.add( defaultGroup.label, gcGroupLabel );

                GridBagConstraints gcNewGroup = new GridBagConstraints();
                gcNewGroup.fill = GridBagConstraints.BOTH;
                gcNewGroup.gridx = 0;
                gcNewGroup.gridy = 1;
                gcNewGroup.ipadx = 0;
                gcNewGroup.weightx = 1.0;
                gcNewGroup.weighty = 0.0D;
                gcNewGroup.anchor = GridBagConstraints.NORTH;
                panel.add( defaultGroup, gcNewGroup );

                GridBagConstraints gcLabel = new GridBagConstraints();
                gcLabel.gridx = 0;
                gcLabel.gridy = 2;
                gcLabel.weighty = 1.0D;
                gcLabel.weightx = 1.0D;
                panel.add( label, gcLabel );
                updateHeaders();
            }
            return defaultGroup;
        }else{
            int count = groups.size();
            for( int i = 0; i < count; i++ ){
                Group itemi = groups.get( i );
                if( itemi.getName().equals( name ) ){
                    return itemi;
                }
            }
            panel.remove( label );
            panel.remove( defaultGroup );
            panel.remove( defaultGroup.label );
            defaultGroup.setColumnHeader( null );
    
            Group g = new Group( name );
            g.updateColumnModel();
            g.table.getTableHeader().addMouseMotionListener( this );
            if( groups.size() > 0 ){
                System.out.println( "BListView_DRAFT#addGroup; header set to null" );
                g.setColumnHeader( null );
            }

            GridBagConstraints gcGroupLabel = new GridBagConstraints();
            gcGroupLabel.fill = GridBagConstraints.HORIZONTAL;
            gcGroupLabel.gridx = 0;
            gcGroupLabel.gridy = groups.size() * 2;
            gcGroupLabel.ipadx = 0;
            gcGroupLabel.weightx = 1.0;
            gcGroupLabel.weighty = 0.0D;
            gcGroupLabel.anchor = GridBagConstraints.NORTH;
            panel.add( g.label, gcGroupLabel );

            GridBagConstraints gcNewGroup = new GridBagConstraints();
            gcNewGroup.fill = GridBagConstraints.BOTH;
            gcNewGroup.gridx = 0;
            gcNewGroup.gridy = groups.size() * 2 + 1;
            gcNewGroup.ipadx = 0;
            gcNewGroup.weightx = 1.0;
            gcNewGroup.weighty = 0.0D;
            gcNewGroup.anchor = GridBagConstraints.NORTH;
            panel.add( g, gcNewGroup );
            groups.add( g );
    
            GridBagConstraints gcDefaultGroupLabel = new GridBagConstraints();
            gcDefaultGroupLabel.fill = GridBagConstraints.HORIZONTAL;
            gcDefaultGroupLabel.gridx = 0;
            gcDefaultGroupLabel.gridy = groups.size() * 2;
            gcDefaultGroupLabel.ipadx = 0;
            gcDefaultGroupLabel.weightx = 1.0;
            gcDefaultGroupLabel.weighty = 0.0D;
            gcDefaultGroupLabel.anchor = GridBagConstraints.NORTH;
            panel.add( defaultGroup.label, gcDefaultGroupLabel );

            GridBagConstraints gcDefaultGroup = new GridBagConstraints();
            gcDefaultGroup.fill = GridBagConstraints.BOTH;
            gcDefaultGroup.gridx = 0;
            gcDefaultGroup.gridy = groups.size() * 2 + 1;
            gcDefaultGroup.ipadx = 0;
            gcDefaultGroup.weightx = 1.0;
            gcDefaultGroup.weighty = 0.0D;
            gcDefaultGroup.anchor = GridBagConstraints.NORTH;
            panel.add( defaultGroup, gcDefaultGroup );
            
            GridBagConstraints gcLabel = new GridBagConstraints();
            gcLabel.gridx = 0;
            gcLabel.gridy =  groups.size() * 2 + 2;
            gcLabel.weighty = 1.0D;
            gcLabel.weightx = 1.0D;
            panel.add( label, gcLabel );
    
            updateHeaders();
            return g;
        }
    }
    
    public void addItem( String group, BListViewItem item ){
        addItem( group, item, true );
    }
    
    public void addItem( String group, BListViewItem item, boolean enabled ){
        if ( group == null || (group != null && group.equals( "" )) ){
            // defaultGroup‚É’Ç‰Á
            defaultGroup.addItemCor( item );
        }else{
            //  “¯–¼‚Ìgroup‚ª‚ ‚é‚©‚Ç‚¤‚©
            int count = groups.size();
            Group g = null;
            for( int i = 0; i < count; i++ ){
                Group tbl = groups.get( i );
                if( tbl.getName().equals( group ) ){
                    g = tbl;
                    break;
                }
            }
            if ( g == null ){
                g = addGroup( group );
            }
            g.addItemCor( item );
        }
        defaultGroup.updateColumnModel();
        int c = groups.size();
        for ( int i = 0; i < c; i++ ){
            groups.get( i ).updateColumnModel();
        }
        /*if( tmodel.getColumnCount() <= 0 ){
            tmodel.setColumnCount( item.getSubItemCount() + 1 );
        }
        int columns = Math.min( tmodel.getColumnCount() - 1, item.getSubItemCount() );
        Object[] sub = new Object[columns + 1];
        sub[0] = item.isSelected();
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
        }*/
    }
    
    public boolean isMultiSelect(){
        return isMultiSelect;
    }
    
    public void setMultiSelect( boolean value ){
        isMultiSelect = value;
        int model;
        if( isMultiSelect ){
            model = ListSelectionModel.MULTIPLE_INTERVAL_SELECTION;
        }else{
            model = ListSelectionModel.SINGLE_SELECTION;
        }
        defaultGroup.table.setSelectionMode( model );
        int count = groups.size();
        for( int i = 0; i < count; i++ ){
            groups.get( i ).table.setSelectionMode( model );
        }
    }
    
    public void setColumnHeaders( String[] headers ) {
        this.headers = headers;
        updateHeaders();
    }

    public void setColumnWidth( int index, int width ) {
        defaultGroup.setColumnWidth( index, width );
        int count = groups.size();
        for( int i = 0; i < count; i++ ){
            groups.get( i ).setColumnWidth( index, width );
        }
    }

    public int getColumnWidth( int index ) {
        if( groups.size() > 0 ){
            return groups.get( 0 ).table.getColumnModel().getColumn( index + 1 ).getWidth();
        }else{
            return defaultGroup.table.getColumnModel().getColumn( index + 1 ).getWidth();
        }
    }

    private void updateHeaders(){
        if( headers == null ){
            return;
        }
        //String[] empty = new String[headers.length];
        int count = groups.size();
        if ( count > 0 ){
            groups.get( 0 ).setColumnHeaderCor( headers );
        }else{
            defaultGroup.setColumnHeaderCor( headers );
        }
    }
    
    public String[] getColumnHeaders() {
        /*int len = tmodel.getColumnCount();
        String[] ret = new String[len - 1];
        for( int i = 1; i < len; i++ ){
            ret[i - 1] = tmodel.getColumnName( i );
        }
        return ret;*/
        return null;
    }

    /*
    private JScrollPane getJScrollPane0() {
        if (jScrollPane0 == null) {
            jScrollPane0 = new JScrollPane();
            jScrollPane0.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_NEVER);
            jScrollPane0.setPreferredSize(new Dimension(200, 100));
            jScrollPane0.setBorder(null);
        }
        return jScrollPane0;
    }*/

    /**
     * This method initializes jScrollPane10	
     * 	
     * @return javax.swing.JScrollPane	
     */
    private JScrollPane getJScrollPane10() {
        if (jScrollPane10 == null) {
            jScrollPane10 = new JScrollPane();
            jScrollPane10.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_AS_NEEDED);
            jScrollPane10.setViewportView(getPanel());
        }
        return jScrollPane10;
    }

    /**
     * This method initializes panel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getPanel() {
        if (panel == null) {
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.weighty = 1.0D;
            gridBagConstraints4.weightx = 1.0D;
            gridBagConstraints4.gridy = 2;
            label = new JLabel();
            label.setText(" ");
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.fill = GridBagConstraints.BOTH;
            gridBagConstraints3.gridy = 1;
            gridBagConstraints3.ipadx = 0;
            gridBagConstraints3.weightx = 1.0;
            gridBagConstraints3.weighty = 0.0D;
            gridBagConstraints3.anchor = GridBagConstraints.NORTH;
            gridBagConstraints3.gridx = 0;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.BOTH;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.ipadx = 0;
            gridBagConstraints1.weightx = 1.0;
            gridBagConstraints1.weighty = 0.0D;
            gridBagConstraints1.anchor = GridBagConstraints.NORTH;
            gridBagConstraints1.gridx = 0;
            panel = new JPanel();
            panel.setLayout(new GridBagLayout());
            panel.add(label, gridBagConstraints4);
        }
        return panel;
    }

}  //  @jve:decl-index=0:visual-constraint="10,10"
