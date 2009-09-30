/*
 * PropertyGrid.java
 * Copyright (c) 2009 kbinani
 *
 * This file is part of com.boare.util.
 *
 * com.boare.util is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.util is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.util;

import java.awt.*;
import javax.swing.*;
import javax.swing.table.*;
import java.lang.reflect.*;
import java.util.*;

public class PropertyGrid extends JPanel{
    private Vector<Object> m_selected_objects = new Vector<Object>();
    private Vector<JTable> m_tables;
    private Vector<JLabel> m_labels;
    private GridBagLayout m_layout;
    private GridBagConstraints m_constraints;

    enum PropertyEntryType{
        pseudoProperty,
        field,
    }

    class PropertyEntry{
        public String name;
        public String displayName;
        public PropertyEntryType type;
        public String category = "";
        public Object value;

        public PropertyEntry( String aName, PropertyEntryType aType ){
            name = aName;
            type = aType;
        }
    }

    public PropertyGrid(){
        initializeComponent();
    }

    private void initializeComponent(){
        m_tables = new Vector<JTable>();
        m_tables.add( new JTable() );
        m_labels = new Vector<JLabel>();
        add( m_tables.get( 0 ) );
    }

    public void setSelectedObject( Object item ){
        m_selected_objects.clear();
        m_selected_objects.add( item );
        updateView();
    }

    private void clearTables(){
        for( JTable jt : m_tables ){
            remove( jt );
        }
        for( JLabel jl : m_labels ){
            remove( jl );
        }
        m_tables.clear();
        m_labels.clear();
    }

    private void updateView(){
        if( m_selected_objects.size() <= 0 ){
            clearTables();
            return;
        }
        try{
            Vector<PropertyEntry> work = new Vector<PropertyEntry>();
            Vector<String> categories = new Vector<String>();
            Class cls = m_selected_objects.get( 0 ).getClass();
            Vector<String> getters = new Vector<String>();
            Method[] methods = cls.getDeclaredMethods();
            Field[] fields = cls.getDeclaredFields();
            PropertyDescripter category_getter = null;
            for( Class c : cls.getInterfaces() ){
                if( c.equals( PropertyDescripter.class ) ){
                    category_getter = (PropertyDescripter)m_selected_objects.get( 0 );
                    break;
                }
            }
            categories.add( "" );
            for( Field f : fields ){
                PropertyEntry pf = new PropertyEntry( f.getName(), PropertyEntryType.field );
                if( category_getter != null ){
                    pf.category = category_getter.getPropertyCategory( f.getName() );
                    if( !categories.contains( pf.category ) ){
                        categories.add( pf.category );
                    }
                    pf.displayName = category_getter.getPropertyName( f.getName() );
                }
                pf.value = f.get( m_selected_objects.get( 0 ) );
                work.add( pf );
            }
            clearTables();

            BoxLayout bl = new BoxLayout( this, BoxLayout.Y_AXIS);
            setLayout( bl );

            Collections.sort( categories );
            for( String title : categories ){
                Vector<PropertyEntry> ext = new Vector<PropertyEntry>();
                for( PropertyEntry pe : work ){
                    if( pe.category.equals( title ) ){
                        ext.add( pe );
                    }
                }
                JLabel jl = new JLabel();
                if( title.equals( "" ) ){
                    jl.setText( "Another" );
                }else{
                    jl.setText( title );
                }
                add( jl );
                JTable jt = new JTable( ext.size(), 2 );
                for( int col = 0; col < ext.size(); col++ ){
                    jt.setValueAt( ext.get( col ).displayName, col, 0 );
                    jt.setValueAt( ext.get( col ).value, col, 1 );
                }
                add( jt );
            }
        }catch( Exception ex ){
            System.out.println( "PropertyGrid.updateView; ex=" + ex );
        }
    }
}
