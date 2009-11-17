package org.kbinani.windows.forms;

import java.util.Vector;

public class BListViewItem {
    private Vector<String> subItems = new Vector<String>();
    private Object tag;
    
    public BListViewItem( String[] values ){
        subItems.clear();
        for( int i = 0; i < values.length; i++ ){
            subItems.add( values[i] );
        }
    }
    
    public Object getTag(){
        return tag;
    }
    
    public void setTag( Object value ){
        tag = value;
    }
    
    public int getSubItemCount(){
        return subItems.size();
    }

    public String getSubItemAt( int index ){
        return subItems.get( index );
    }

    public void setSubItemAt( int index, String value ){
        subItems.set( index, value );
    }

}
