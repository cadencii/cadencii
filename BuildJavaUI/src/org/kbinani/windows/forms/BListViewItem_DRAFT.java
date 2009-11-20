package org.kbinani.windows.forms;

import java.util.Vector;
import javax.swing.table.TableModel;

public class BListViewItem_DRAFT implements Cloneable{
    /*
    private Vector<String> subItems = new Vector<String>();
    private Object tag;
    protected BListView_DRAFT table = null;
    protected int row;
    private boolean isSelected = false;
    private String group = "";
    
    public BListViewItem_DRAFT( String[] values ){
        subItems.clear();
        for( int i = 0; i < values.length; i++ ){
            subItems.add( values[i] );
        }
    }

    protected BListViewItem_DRAFT( BListView_DRAFT table, int row ){
        this.table = table;
        this.row = row;
        BListViewItem_DRAFT item = table.getItemAt( row );
        int count = item.getSubItemCount();
        subItems.clear();
        for( int i = 0; i < count; i++ ){
            subItems.add( item.getSubItemAt( i ) );
        }
    }

    public String getGroup(){
        return group;
    }
    
    public void setGroup( String value ){
        group = value;
        
    }
    
    public Object clone(){
        updateStatusFromTableModel();
        int count = subItems.size();
        String[] values = new String[count];
        for( int i = 0; i < count; i++ ){
            values[i] = subItems.get( i );
        }
        BListViewItem_DRAFT ret = new BListViewItem_DRAFT( values );
        ret.isSelected = isSelected();
        return ret;
    }
    
    public boolean isSelected(){
        if( table != null ){
            isSelected = (Boolean)table.getItemAt( row ).isSelected();
        }
        return isSelected;
    }

    public void setSelected( boolean value ){
        if( table != null ){
            table.getItemAt( row ).setSelected( value );
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
        if( table == null ){
            return;
        }
        BListViewItem_DRAFT item = table.getItemAt( row );
        isSelected = (Boolean)item.isSelected();
        int count = item.getSubItemCount();
        if( subItems.size() != count ){
            subItems.clear();
            subItems.setSize( count );
        }
        for( int i = 0; i < count; i++ ){
            subItems.set( i, item.getSubItemAt( i ) );
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
*/
}
