package org.kbinani.windows.forms;

import java.util.Vector;
import javax.swing.table.TableModel;

public class BListViewItem implements Cloneable{
    private Vector<String> subItems = new Vector<String>();
    private Object tag;
    protected TableModel tmodel = null;
    protected int row;
    //private String group = "";
    private String name = "";
    
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

    public String getName(){
        return name;
    }
    
    public void setName( String value ){
        name = value;
    }
    
    public Object clone(){
        updateStatusFromTableModel();
        int count = subItems.size();
        String[] values = new String[count];
        for( int i = 0; i < count; i++ ){
            values[i] = subItems.get( i );
        }
        BListViewItem ret = new BListViewItem( values );
        return ret;
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

    /**
     * テーブルモデルへの参照がある場合、テーブルモデルからデータを読み込み更新する
     */
    private void updateStatusFromTableModel(){
        if( tmodel == null ){
            return;
        }
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
