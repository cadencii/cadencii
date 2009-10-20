#if JAVA
/*
 * XmlMember.java
 * Copyright (c) 2009 kbinani
 *
 * This file is part of org.kbinani.util.
 *
 * org.kbinani.util is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.util is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package org.kbinani.xml;

import java.util.*;
import java.lang.reflect.*;

public class XmlMember{
    private String m_name;
    private Method m_getter = null;
    private Method m_setter = null;
    private Field m_field = null;
    private Class m_type = null;
    private Method m_elementname_getter = null;
    private Method m_isignored_getter = null;

    private XmlMember(){
    }

    public String getName(){
        return m_name;
    }

    public Class getType(){
        return m_type;
    }

    public static XmlMember[] extractMembers( Class t ){
        XmlSerializable descripter = null;
        try
        {
            Object tinstance = t.newInstance();
            if( tinstance instanceof XmlSerializable )
            {
                descripter = (XmlSerializable)tinstance;
            }
        }
        catch( Exception ex )
        {
            System.out.println( "XmlMember#extractMembers; ex=" + ex );
        }
        //Method elementname_getter = null;
        //Method isignored_getter = null;
        /*try{
            elementname_getter = t.getMethod( "getXmlElementName", String.class );
            int m = elementname_getter.getModifiers();
            if( !Modifier.isPublic( m ) || !Modifier.isStatic( m ) ){
                elementname_getter = null;
            }
        }catch( Exception ex ){
            elementname_getter = null;
        }
        try{
            isignored_getter = t.getMethod( "isXmlIgnored", String.class );
            int m = isignored_getter.getModifiers();
            if( !Modifier.isPublic( m ) || !Modifier.isStatic( m ) ){
                isignored_getter = null;
            }
        }catch( Exception ex ){
            isignored_getter = null;
        }*/
        Vector<XmlMember> members = new Vector<XmlMember>();

        // superクラスのプロパティを取得
        if( t.getSuperclass() != null ){
            XmlMember[] super_members = extractMembers( t.getSuperclass() );
            for( XmlMember xm : super_members ){
                members.add( xm );
            }
        }
        Vector<String> props = new Vector<String>();
        for( Field f : t.getDeclaredFields() ){
            int m = f.getModifiers();
            if( !Modifier.isPublic( m ) || Modifier.isStatic( m ) ){
                continue;
            }
            props.add( f.getName() );
        }

        // get, set, isで始まるメソッド名を持つ、publicでstaticでないメソッドを抽出
        for( Method method : t.getDeclaredMethods() ){
            int m = method.getModifiers();
            if( !Modifier.isPublic( m ) || Modifier.isStatic( m ) ){
                continue;
            }
            String name = method.getName();
            if( name.startsWith( "get" ) ){
                name = name.substring( 3 );
                if( !props.contains( name ) ){
                    props.add( name );
                }
            }else if( name.startsWith( "set" ) ){
                name = name.substring( 3 );
                if( !props.contains( name ) ){
                    props.add( name );
                }
            }else if( name.startsWith( "is" ) ){
                name = name.substring( 2 );
                if( !props.contains( name ) ){
                    props.add( name );
                }
            }
        }

        for( String name : props ){
            boolean ignore = false;
            if( descripter != null ){
                try{
                    ignore = descripter.isXmlIgnored( name );
                }catch( Exception ex ){
                }
            }
            if( ignore ){
                continue;
            }
            String xmlname = name;
            if( descripter != null ){
                try{
                    xmlname = descripter.getXmlElementName( name );
                }catch( Exception ex ){
                }
            }
            XmlMember xm = extract( t, name );
            if( xm != null ){
                xm.m_name = xmlname;
                members.add( xm );
            }
        }

        return members.toArray( new XmlMember[]{} );
    }

    public static XmlMember extract( Class cls, String property_name ){
        for( Field f : cls.getDeclaredFields() ){
            int m = f.getModifiers();
            if( !Modifier.isPublic( m ) || Modifier.isStatic( m ) ){
                continue;
            }
            String name = f.getName();
            if( name.equals( property_name ) ){
                XmlMember xm = new XmlMember();
                xm.m_name = property_name;
                xm.m_field = f;
                xm.m_getter = null;
                xm.m_setter = null;
                xm.m_type = f.getType();
                return xm;
            }
        }

        // get, set, isで始まるメソッド名を持つ、publicでstaticでないメソッドを抽出
        Method getter = null;
        Method setter = null;
        Class prop_type = null;
        for( Method method : cls.getDeclaredMethods() ){
            int m = method.getModifiers();
            if( !Modifier.isPublic( m ) || Modifier.isStatic( m ) ){
                continue;
            }
            String name = method.getName();
            Class ret_type = method.getReturnType();
            if( name.startsWith( "set" ) && setter == null ){
                if( !name.substring( 3 ).equals( property_name ) ){
                    continue;
                }
                // setterなので、戻り値の型はvoid
                if( !ret_type.equals( Void.TYPE ) && !ret_type.equals( Void.class ) ){
                    continue;
                }

                // 引数の個数は1
                Class[] args = method.getParameterTypes();
                if( args.length != 1 ){
                    continue;
                }

                // 探している型と合致するか
                if( prop_type == null ){
                    prop_type = args[0];
                }else{
                    if( !prop_type.equals( args[0] ) ){
                        continue;
                    }
                }
                setter = method;
            }else if( name.startsWith( "is" ) && getter == null ){
                if( !name.substring( 2 ).equals( property_name ) ){
                    continue;
                }
                if( setter != null ){
                    // setterが既に見つかっていて、ret_typeがboolean/Booleanじゃない場合
                    if( !ret_type.equals( Boolean.TYPE ) || !ret_type.equals( Boolean.class ) ){
                        return null;
                    }
                }
                // isで始まるgetterは、戻り値の型がBoolean or boolean
                if( !ret_type.equals( Boolean.TYPE ) && !ret_type.equals( Boolean.class ) ){
                    continue;
                }

                // 引数の個数は0
                Class[] args = method.getParameterTypes();
                if( args.length != 0 ){
                    continue;
                }

                if( prop_type == null ){
                    prop_type = ret_type;
                }else{
                    if( !prop_type.equals( ret_type ) ){
                        continue;
                    }
                }
                getter = method;
            }else if( name.startsWith( "get" ) && getter == null ){
                if( !name.substring( 3 ).equals( property_name ) ){
                    continue;
                }
                // 引数の個数は0
                Class[] args = method.getParameterTypes();
                if( args.length != 0 ){
                    continue;
                }

                if( prop_type == null ){
                    prop_type = ret_type;
                }else{
                    if( !prop_type.equals( ret_type ) ){
                        continue;
                    }
                }
                getter = method;
            }
            if( getter != null && setter != null ){
                break;
            }
        }
        if( getter != null && setter != null ){
            XmlMember xm = new XmlMember();
            xm.m_name = property_name;
            xm.m_field = null;
            xm.m_getter = getter;
            xm.m_setter = setter;
            xm.m_type = prop_type;
            return xm;
        }else{
            return null;
        }
    }

    public Object get( Object obj ){
        try{
            if( m_field != null ){
                return m_field.get( obj );
            }else{
                return m_getter.invoke( obj );
            }
        }catch( Exception ex ){
            System.out.println( "org.kbinani.util.XmlMember.get; ex=" + ex );
            return null;
        }
    }

    public void set( Object obj, Object value ){
        try{
            if( m_field != null ){
                m_field.set( obj, value );
            }else{
                m_setter.invoke( obj, value );
            }
        }catch( Exception ex ){
            System.out.println( "org.kbinani.util.XmlMember.set; ex=" + ex );
        }
    }
}
#endif
