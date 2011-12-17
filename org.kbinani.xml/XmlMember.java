package com.github.cadencii.xml;

import java.awt.Point;
import java.awt.Rectangle;
import java.awt.geom.Point2D;
import java.awt.geom.Rectangle2D;
import java.lang.annotation.Annotation;
import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.lang.reflect.Modifier;
import java.util.Vector;

public class XmlMember
{
    /**
     * スーパークラスに対する再帰的なメンバー検索を行わないクラスのリスト
     */
    private static Class<?>[] ignoreRecursiveMemberExtraction = 
        new Class<?>[]{ Object.class, Rectangle.class, Rectangle2D.class, Point.class, Point2D.class };
    private String m_name;
    private Method m_getter = null;
    private Method m_setter = null;
    private Field m_field = null;
    private Class<?> m_type = null;
    private String m_xmlname;
    
    private XmlMember(){
    }
    
    /**
     * このプロパティの，xml上でのエレメント名を取得します
     * @return このプロパティのxmlでのエレメント名
     */
    public String getXmlName()
    {
        return m_xmlname;
    }
    
    /**
     * プロパティの名前を取得します
     * @return このプロパティの名前
     */
    public String getName(){
        return m_name;
    }

    /**
     * このプロパティの型を取得します
     * @return このプロパティの型
     */
    public Class<?> getType(){
        return m_type;
    }

    /**
     * 指定したメソッドに付加されているアノテーションを取得します．
     * スーパークラスについても遡って検出を試みます．
     * @param m 検出対象のメソッド
     * @param annotation 取得するアノテーションの型
     * @return アノテーションが見つかればアノテーションのインスタンス，見つからなければnull
     */
    private static <T extends Annotation> T getMethodAnnotationRecurse( Method m, Class<T> annotation )
    {
        T ret = m.getAnnotation( annotation );
        if( ret != null ){
            return ret;
        }
        Class<?> super_class = m.getDeclaringClass().getSuperclass();
        if( super_class.equals( Object.class ) ){
            return null;
        }
        // スーパークラスにも同じ名前，型のメソッドが宣言されているか？
        Method mi = null;
        try{
            mi = super_class.getMethod( m.getName(), m.getParameterTypes() );
        }catch( Exception ex ){
            mi = null;
        }
        if( mi == null ){
            // そんなメソッドは無い
            return null;
        }
        if( !mi.getReturnType().equals( m.getReturnType() ) ){
            // 型が違う
            return null;
        }
        return getMethodAnnotationRecurse( mi, annotation );
    }
    
    /**
     * 指定したフィールドに付加されているアノテーションを取得します．
     * スーパークラスについても遡って検出を試みます．
     * @param f 検出対象のフィールド
     * @param annotation 取得するアノテーションの型
     * @return アノテーションが見つかればアノテーションのインスタンス，見つからなければnull
     */
    private static <T extends Annotation> T getFieldAnnotationRecurse( Field f, Class<T> annotation )
    {
        T ret = f.getAnnotation( annotation );
        if( ret != null ){
            return ret;
        }
        Class<?> super_class = f.getDeclaringClass().getSuperclass();
        if( super_class.equals( Object.class ) ){
            return null;
        }
        // スーパークラスにも同じ名前，型のフィールドが宣言されているか？
        Field fi = null;
        try{
            fi = super_class.getField( f.getName() );
        }catch( Exception ex ){
            fi = null;
        }
        if( fi == null ){
            // そんなフィールドは無い
            return null;
        }
        if( !fi.getType().equals( f.getType() ) ){
            // 型が違う
            return null;
        }
        return getFieldAnnotationRecurse( fi, annotation );
    }
    
    /**
     * getterメソッドまたはフィールドのアノテーションを取得します
     * @param annotation 取得するアノテーションの型
     * @return アノテーションが見つかればアノテーションのインスタンス，見つからなければnull
     */
    public <T extends Annotation> T getGetterAnnotation( Class<T> annotation )
    {
        T ret = null;
        if( m_field != null ){
            ret = getFieldAnnotationRecurse( m_field, annotation );
        }else{
            if( m_getter != null ){
                ret = getMethodAnnotationRecurse( m_getter, annotation );
            }
        }
        return ret;
    }

    /**
     * setterメソッド，またはフィールドのアノテーションを取得します
     * @param annotation 取得するアノテーションの型
     * @return アノテーションが見つかればアノテーションのインスタンス，見つからなければnull
     */
    public <T extends Annotation> T getSetterAnnotation( Class<T> annotation )
    {
        T ret = null;
        if( m_field != null ){
            ret = getFieldAnnotationRecurse( m_field, annotation );
        }else{
            if( m_setter != null ){
                ret = getMethodAnnotationRecurse( m_setter, annotation );
            }
        }
        return ret;
    }

    /**
     * 指定した型から，プロパティー一覧を抽出します
     * @param t 抽出対象のクラス
     * @return 指定したクラスに含まれるプロパティの一覧
     */
    public static XmlMember[] extractMembers( Class<?> t ){
        Vector<XmlMember> members = new Vector<XmlMember>();
    
        // superクラスのプロパティを取得
        Class<?> superclass = t.getSuperclass();
        if( superclass != null ){
            boolean check_this_superclass = true;
            for( Class<?> cls : ignoreRecursiveMemberExtraction ){
                if( t.equals( cls ) ){
                    check_this_superclass = false;
                    break;
                }
            }
            if( check_this_superclass ){
                XmlMember[] super_members = extractMembers( superclass );
                for( XmlMember xm : super_members ){
                    members.add( xm );
                }
            }
        }
        Vector<String> props = new Vector<String>();
        for( Field f : t.getFields() ){
            int m = f.getModifiers();
            if( !Modifier.isPublic( m ) || Modifier.isStatic( m ) ){
                continue;
            }
            props.add( f.getName() );
        }
    
        // get, set, isで始まるメソッド名を持つ、publicでstaticでないメソッドを抽出
        for( Method method : t.getMethods() ){
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
            XmlMember xm = extract( t, name );
            if( xm == null ){
                continue;
            }
            XmlIgnore xi = xm.getGetterAnnotation( XmlIgnore.class );
            if( xi != null ){
                continue;
            }
            xi = xm.getSetterAnnotation( XmlIgnore.class );
            if( xi != null ){
                continue;
            }
            members.add( xm );
        }
    
        return members.toArray( new XmlMember[]{} );
    }

    /**
     * 指定した型から，指定した名前のプロパティを抽出します．該当するプロパティがなければnullを返します．
     * @param cls 抽出対象のクラス
     * @param property_name 抽出するプロパティの名前
     * @return プロパティが見つかればそのプロパティを表現するXmlMemberクラスのインスタンス，見つからなければnull
     */
    public static XmlMember extract( Class<?> cls, String property_name ){
        for( Field f : cls.getFields() ){
            int m = f.getModifiers();
            if( !Modifier.isPublic( m ) || Modifier.isStatic( m ) ){
                continue;
            }
            String name = f.getName();
            if( name.equals( property_name ) ){
                XmlMember xm = new XmlMember();
                xm.m_name = property_name;
                xm.m_xmlname = property_name;
                xm.m_field = f;
                xm.m_getter = null;
                xm.m_setter = null;
                xm.m_type = f.getType();
                XmlElementName en = xm.getGetterAnnotation( XmlElementName.class );
                if( en != null ){
                    xm.m_xmlname = en.value();
                }else{
                    en = xm.getSetterAnnotation( XmlElementName.class );
                    if( en != null ){
                        xm.m_xmlname = en.value();
                    }
                }
                return xm;
            }
        }
    
        // get, set, isで始まるメソッド名を持つ、publicでstaticでないメソッドを抽出
        Method getter = null;
        Method setter = null;
        Class<?> prop_type = null;
        for( Method method : cls.getMethods() ){
            int m = method.getModifiers();
            if( !Modifier.isPublic( m ) || Modifier.isStatic( m ) ){
                continue;
            }
            String name = method.getName();
            Class<?> ret_type = method.getReturnType();
            if( name.startsWith( "set" ) && setter == null ){
                if( !name.substring( 3 ).equals( property_name ) ){
                    continue;
                }
                // setterなので、戻り値の型はvoid
                if( !ret_type.equals( Void.TYPE ) && !ret_type.equals( Void.class ) ){
                    continue;
                }
    
                // 引数の個数は1
                Class<?>[] args = method.getParameterTypes();
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
                Class<?>[] args = method.getParameterTypes();
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
                Class<?>[] args = method.getParameterTypes();
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
            xm.m_xmlname = property_name;
            xm.m_field = null;
            xm.m_getter = getter;
            xm.m_setter = setter;
            xm.m_type = prop_type;
            XmlElementName en = xm.getGetterAnnotation( XmlElementName.class );
            if( en != null ){
                xm.m_xmlname = en.value();
            }else{
                en = xm.getSetterAnnotation( XmlElementName.class );
                if( en != null ){
                    xm.m_xmlname = en.value();
                }
            }
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
            System.err.println( "XmlMember#get; ex=" + ex );
            ex.printStackTrace();
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
            System.err.println( "XmlMember#set; ex=" + ex );
            ex.printStackTrace();
        }
    }
}
