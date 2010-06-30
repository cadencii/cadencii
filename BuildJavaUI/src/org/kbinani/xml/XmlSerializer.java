package org.kbinani.xml;

import java.io.*;
import java.util.*;
import java.lang.reflect.*;
import javax.xml.parsers.*;
import javax.xml.transform.*;
import javax.xml.transform.dom.*;
import javax.xml.transform.stream.*;
import org.kbinani.PortUtil;
import org.w3c.dom.*;

/**
 * .NETのSystem.Xml.Serialization.XmlSerializerと同じ書式で入出力するためのXMLシリアライザ．<br>
 * シリアライズしたいクラスには，以下のメソッドを実装しておく必要があります．<br>
 * <dl>
 *   <dt>getXmlElementNameメソッド</dt>
 *   <dd>フィールド名やgetter/setter名からXMLのノード名を調べる</dd>
 *   <dt>isXmlIgnoredメソッド</dt>
 *   <dd>アイテムをXMLに出力するかどうかを決める</dd>
 *   <dt>getGenericTypeNameメソッド</dt>
 *   <dd>アイテムが総称型を含むクラスの場合に，その総称型名を調べる</dd>
 * </dl>
 * <pre>
 *  public class Test2{
    public float value = 1.0f;
    private boolean m_b = true;
    private int m_i = 2;
    public Vector<Integer> list = new Vector<Integer>();
    
    public boolean isHoge(){
        return m_b;
    }
    
    public void setHoge( boolean value ){
        m_b = value;
    }
    
    public int getInteger(){
        return m_i;
    }
    
    public void setInteger( int value ){
        m_i = value;
    }
    
    public static String getXmlElementName( String name ){
        if( name.equals( "value" ) ){
            return "Value";
        }else if( name.equals( "list" ) ){
            return "List";
        }
        return name;
    }

    public static boolean isXmlIgnored( String name ){
        if( name.equals( "Integer" ) ){
            return true;
        }
        return false;
    }

    public static String getGenericTypeName( String name ){
        if( name.equals( "list" ) ){
            return "java.lang.Integer";
        }
        return "";
    }
}
 * </pre>
 * このように実装しておくと，だいたい以下のようなXMLの入出力が可能になります．
 * <pre>
&lt;Test2&gt;
    &lt;Value&gt;1.0&lt;/Value&gt;
    &lt;Hoge&gt;true&lt;/Hoge&gt;
    &lt;List&gt;
        &lt;int&gt;1&lt;/int&gt;
        &lt;int&gt;2&lt;/int&gt;
    &lt;/List&gt;
&lt;/Test2&gt;
 * </pre>
 */
public class XmlSerializer{
    private Document m_document;
    private Class<?> m_class;
    private boolean m_static_mode = false;
    private boolean m_indent = true;
    private int m_indent_width = 2;

    public boolean isIndent(){
        return m_indent;
    }

    public void setIndent( boolean value ){
        m_indent = value;
    }

    public int getIndentWidth(){
        return m_indent_width;
    }

    public void setIndentWidth( int value ){
        if( value < 0 ){
            m_indent_width = 0;
        }else{
            m_indent_width = value;
        }
    }

    public XmlSerializer( Class<?> cls ){
        m_class = cls;
    }

    public Object deserialize( InputStream stream ){
        try{
            DocumentBuilderFactory fact = DocumentBuilderFactory.newInstance();
            DocumentBuilder builder = fact.newDocumentBuilder();
            Document doc = builder.parse( stream );
            Object ret = parseNode( m_class, null, doc.getDocumentElement() );
            return ret;
        }catch( Exception ex ){
            System.err.println( "XmlSerializer.deserialize; ex=" + ex );
            return null;
        }
    }

    /**
     * 指定したクラスの，指定した名前のプロパティが総称型引数を持つ型だった場合，
     * その型に定義されているstatic String getGenericTypeName(String property_name)メソッド
     * を呼び出すことによって，その型を表すClass<?>を返します．
     * 上記のメソッドが定義されていない場合，nullを返します．上記メソッドが定義されている場合であって，
     * 上記メソッドの戻り値が正確でない場合(正しい限定名でないなど)や呼び出しに失敗した場合も，
     * nullを返します．
     * @param cls 検査対象の型
     * @param property_name 検査対象のプロパティ名
     * @return 検査対象の型の，指定したプロパティの型の型引数の型を表すClass<?>
     */
    public static Class<?> getGenericType( Class<?> cls, String property_name ){
        for( Method m : cls.getMethods() ){
            int modifier = m.getModifiers();
            if( Modifier.isPublic( modifier ) && 
                Modifier.isStatic( modifier ) &&
                m.getName().equals( "getGenericTypeName" ) &&
                m.getReturnType().equals( String.class ) ){
                Class<?>[] param_types = m.getParameterTypes();
                if( param_types.length == 1 &&
                    param_types[0].equals( String.class ) ){
                    try{
                        String cls_name = (String)m.invoke( null, property_name );
                        Class<?> ret = Class.forName( cls_name );
                        return ret;
                    }catch( Exception ex ){
                        System.err.println( "XmlSerializer#getComponentType; ex=" + ex );
                        ex.printStackTrace();
                    }
                }
            }
        }
        return null;
    }

    /**
     * このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
     * 要素名を取得します．
     * @param cls 検査対象のクラス
     * @param property_name 検査対象のプロパティ名
     * @return
     */
    public static String getXmlElementName( Class<?> cls, String property_name ){
        for( Method m : cls.getMethods() ){
            int modifier = m.getModifiers();
            if( Modifier.isPublic( modifier ) && 
                Modifier.isStatic( modifier ) &&
                m.getName().equals( "getXmlElementName" ) &&
                m.getReturnType().equals( String.class ) ){
                Class<?>[] param_types = m.getParameterTypes();
                if( param_types.length == 1 &&
                    param_types[0].equals( String.class ) ){
                    try{
                        String element_name = (String)m.invoke( null, property_name );
                        return element_name;
                    }catch( Exception ex ){
                        System.err.println( "XmlSerializer#getXmlElementName; ex=" + ex );
                        ex.printStackTrace();
                    }
                }
            }
        }
        return property_name;
    }

    /**
     * このクラスの指定した名前のプロパティを，XMLシリアライズ時に無視するかどうかを表す
     * ブール値を返します．デフォルトの実装では戻り値は全てfalseです．
     * @param cls 検査対象のクラス
     * @param property_name 検査対象のプロパティ名
     * @return
     */
    public static boolean isXmlIgnored( Class<?> cls, String property_name ){
        for( Method m : cls.getMethods() ){
            int modifier = m.getModifiers();
            if( Modifier.isPublic( modifier ) && 
                Modifier.isStatic( modifier ) &&
                m.getName().equals( "isXmlIgnored" ) &&
                m.getReturnType().equals( Boolean.TYPE ) ){
                Class<?>[] param_types = m.getParameterTypes();
                if( param_types.length == 1 &&
                    param_types[0].equals( String.class ) ){
                    try{
                        Boolean ret = (Boolean)m.invoke( null, property_name );
                        return ret.booleanValue();
                    }catch( Exception ex ){
                        System.err.println( "XmlSerializer#isXmlIgnored; ex=" + ex );
                        ex.printStackTrace();
                    }
                }
            }
        }
        return false;
    }
    
    private Object parseNode( Class t, Class<?> parent_class, Node node )
        throws Exception
    {
PortUtil.println( "XmlSerializer#parseNode; t=" + t + "; parent_class=" + parent_class + ", node.getNodeName()=" + node.getNodeName() );
        NodeList childs = node.getChildNodes();
        int numChild = childs.getLength();
        Object obj;
        String str = node.getTextContent() + "";
        if( t.equals( Integer.TYPE ) || t.equals( Integer.class ) ){
            return Integer.parseInt( str );
        }else if( t.equals( Byte.TYPE ) || t.equals( Byte.class ) ){
            return Byte.parseByte( str );
        }else if( t.equals( Short.TYPE ) || t.equals( Short.class ) ){
            return Short.parseShort( str );
        }else if( t.equals( Float.TYPE ) || t.equals( Float.class ) ){
            return Float.parseFloat( str );
        }else if( t.equals( Double.TYPE ) || t.equals( Double.class ) ){
            return Double.parseDouble( str );
        }else if( t.equals( Boolean.TYPE ) || t.equals( Boolean.class ) ){
            return Boolean.parseBoolean( str );
        }else if( t.equals( String.class ) ){
            return str;
        }else if( t.isEnum() ){
PortUtil.println( "XmlSerializer#parseNode; t.isEnum()=true" );
            Object ret = null;
            try{
                ret = Enum.valueOf( t, str );
            }catch( Exception ex ){
                PortUtil.stderr.println( "XmlSerializer#parseNode; ex=" + ex );
                ex.printStackTrace();
            }
PortUtil.println( "XmlSerializer#parseNode; ret=" + ret );
            return ret;
        }else if( t.isArray() || t.equals( Vector.class ) ){
            try{
                Class<?> content_class = null;
                if( t.isArray() ){
                    // 配列の場合
                    content_class = t.getComponentType();
                }else{
                    content_class = getGenericType( parent_class, node.getNodeName() );
                }
                if( content_class == null ){
                    throw new Exception(
                        "XmlSerializer#parseNode; error; " + 
                        "cannot specify generic type of property named '" + 
                        node.getNodeName() + "' in class '" + parent_class + "'.\n" +
                        "please implement 'static String' method named 'getGenericTypeName( String property_name )'" );
                }
                Vector<Object> vec = new Vector<Object>();
                for( int i = 0; i < numChild; i++ ){
                    Node c = childs.item( i );
                    if( c.getNodeType() == Node.ELEMENT_NODE ){
                        /*Element f = (Element)c;
                        if( !f.getTagName().equals( element_name ) ){
                            continue;
                        }*/
                        vec.add( parseNode( content_class, t, c ) );
                    }
                }
                if( t.isArray() ){
                    int length = vec.size();
                    Object arr = Array.newInstance( content_class, length );
                    for( int i = 0; i < length; i++ ){
                        Array.set( arr, i, vec.get( i ) );
                    }
                    return arr;
                }else if( t.equals( Vector.class ) ){
                    return vec;
                }
            }catch( Exception ex ){
                System.err.println( "XmlSerializer.parseNode; ex=" + ex );
                return null;
            }
        }
        try{
            obj = t.newInstance();
        }catch( Exception ex ){
            return null;
        }
        XmlMember[] members = XmlMember.extractMembers( t );
        for( int i = 0; i < numChild; i++ ){
            Node c = childs.item( i );
            if( c.getNodeType() == Node.ELEMENT_NODE ) {
                Element f = (Element)c;
                String name = f.getTagName();
                for( XmlMember xm : members ){
                    if( f.getTagName().equals( xm.getName() ) ) {
                        xm.set( obj, parseNode( xm.getType(), t, c ) );
                        break;
                    }
                }
            }
        }
        return obj;
    }

    public void serialize( OutputStream stream, Object obj ) throws TransformerConfigurationException,
                                                                    ParserConfigurationException,
                                                                    TransformerException,
                                                                    IllegalAccessException{
        DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
        DocumentBuilder builder = factory.newDocumentBuilder();
        DOMImplementation domImpl=builder.getDOMImplementation();
        m_document = domImpl.createDocument( null, m_class.getSimpleName(), null );
        Element root = m_document.getDocumentElement();
        parseFieldAndProperty( m_class, obj, root );
        TransformerFactory tfactory = TransformerFactory.newInstance(); 
        Transformer transformer = tfactory.newTransformer(); 
        if( m_indent ){
            transformer.setOutputProperty( OutputKeys.INDENT, "yes" );
        }
        transformer.setOutputProperty( OutputKeys.METHOD, "xml" );
        if( m_indent ){
            transformer.setOutputProperty( "{http://xml.apache.org/xalan}indent-amount", "" + m_indent_width );
        }
        transformer.transform( new DOMSource( m_document ), new StreamResult( stream ) ); 
    }

    private void parseFieldAndProperty( Class t, Object obj, Element el ) throws IllegalAccessException{
        if( obj == null ){
            return;
        }
        XmlMember[] members = XmlMember.extractMembers( t );
        for( XmlMember xm : members ){
            String name = xm.getName();
            Element el2 = m_document.createElement( name );
            printItemRecurse( xm.getType(), xm.get( obj ), el2 );
            el.appendChild( el2 );
        }
    }

    //TODO: この辺が未だ
    /**
     * 指定したクラスが、指定したインターフェースを実装しているかどうかを調べます
     * @param cls
     * @param itfc
     * @return
     */
    public static boolean isInterfaceDeclared( Class<?> cls, Class<?> itfc ){
System.out.println( "XmlSerializer#isInterfaceDeclared; cls=" + cls + "; itfc=" + itfc );
        Type[] classes = cls.getGenericInterfaces();
System.out.println( "XmlSerializer#isInterfaceDeclared; classes.length=" + classes.length );
        for( Type t : classes ){
System.out.println( "XmlSerializer#isInterfaceDeclared; t=" + t );
            if( t.equals( itfc ) ){
                return true;
            }
System.out.println( "XmlSerializer#isInterfaceDeclared; (t instanceof Class)=" + (t instanceof Class) );
            if( t instanceof Class ){
                Class c = (Class)t;
                boolean ret = isInterfaceDeclared( c, itfc );
                if( ret ){
                    return true;
                }
            }
        }
        Class<?> super_class = cls.getSuperclass();
        if( super_class != null ){
            return isInterfaceDeclared( super_class, itfc );
        }
        return false;
    }
    
    private void printItemRecurse( Class t, Object obj, Element parent ) throws IllegalAccessException{
        try{
            if ( !tryWriteValueType( t, obj, parent ) ){
                if( t.isArray() || t.equals( Vector.class ) || isInterfaceDeclared( t, Iterable.class ) ){
                    Object[] array = null;
                    if( obj != null ){
                        if( t.isArray() ){
                            // 配列の場合
                            int length = Array.getLength( obj );
                            array = new Object[length];
                            for( int i = 0; i < length; i++ ){
                                array[i] = Array.get( obj, i );
                            }
                        }else if( t.equals( Vector.class ) ){
                            // ベクターの場合
                            array = ((Vector)obj).toArray();
                        }else{
                            // 配列でもベクターでもない場合
                            // Iterableを実装してないか？
                            Iterator<?> iterator = ((Iterable<?>)obj).iterator();
                            // Iteratorを取得できた場合
                            // Vectorにまず格納
                            Vector<Object> vec = new Vector<Object>();
                            for( ;iterator.hasNext(); ){
                                vec.add( iterator.next() );
                            }
                            // 配列に変換
                            array = vec.toArray();
                        }
                    }
                    if( array != null ){
                        for( Object o : array ){
                            if( o != null ){
                                String name = getCliTypeName( o.getClass() );
                                if( name.equals( "" ) ){
                                    name = o.getClass().getSimpleName();
                                }
                                Element element = m_document.createElement( name );
                                printItemRecurse( o.getClass(), o, element );
                                parent.appendChild( element );
                            }
                        }
                    }
                }else{
                    parseFieldAndProperty( t, obj, parent );
                }
            }
        }catch( Exception ex ){
            System.err.println( "XmlSerializer#printItemRecurse; ex=" + ex );
            ex.printStackTrace();
        }
    }

    private static String getCliTypeName( Class t ){
        if( t.equals( Boolean.class ) || t.equals( Boolean.TYPE ) ){
            return "bool";
        }else if( t.equals( Double.class ) || t.equals( Double.TYPE ) ){
            return "double";
        }else if( t.equals( Integer.class ) || t.equals( Integer.TYPE ) ){
            return "int";
        }else if( t.equals( Long.class ) || t.equals( Long.TYPE ) ){
            return "long";
        }else if( t.equals( Short.class ) || t.equals( Short.TYPE ) ){
            return "short";
        }else if( t.equals( Float.class ) || t.equals( Float.TYPE ) ){
            return "float";
        }else if( t.equals( String.class ) ){
            return "string";
        }else{
            return "";
        }
    }

    private static Class<?> getClassForCliTypeName( String name ){
        if( name == null ){
            return null;
        }
        if( name.equals( "bool" ) ){
            return Boolean.TYPE; 
        }else if( name.equals( "double" ) ){
            return Double.TYPE;
        }else if( name.equals( "int" ) ){
            return Integer.TYPE;
        }else if( name.equals( "long" ) ){
            return Long.TYPE;
        }else if( name.equals( "short" ) ){
            return Short.TYPE;
        }else if( name.equals( "float" ) ){
            return Float.TYPE;
        }else if( name.equals( "string" ) ){
            return String.class;
        }else{
            return null;
        }
    }
    
    private boolean tryWriteValueType( Class t, Object obj, Element element ){
        if( t.equals( Boolean.class ) || t.equals( Boolean.TYPE ) ){
            element.appendChild( m_document.createTextNode( (Boolean)obj + "" ) );
            return true;
        }else if( t.equals( Double.class ) || t.equals( Double.TYPE ) ){
            element.appendChild( m_document.createTextNode( (Double)obj + "" ) );
            return true;
        }else if( t.equals( Integer.class ) || t.equals( Integer.TYPE ) ){
            element.appendChild( m_document.createTextNode( (Integer)obj + "" ) );
            return true;
        }else if( t.equals( Long.class ) || t.equals( Long.TYPE ) ){
            element.appendChild( m_document.createTextNode( (Long)obj + "" ) );
            return true;
        }else if( t.equals( Short.class ) || t.equals( Short.TYPE ) ){
            element.appendChild( m_document.createTextNode( (Short)obj + "" ) );
            return true;
        }else if( t.equals( Float.class ) || t.equals( Float.TYPE ) ){
            element.appendChild( m_document.createTextNode( (Float)obj + "" ) );
            return true;
        }else if( t.equals( String.class ) ){
            if( obj == null ){
                element.appendChild( m_document.createTextNode( "" ) );
            }else{
                element.appendChild( m_document.createTextNode( (String)obj ) );
            }
            return true;
        }else if( t.isEnum() ){
            if( obj == null ){
                for( Field f : t.getDeclaredFields() ){
                    String name = f.getName();
                    if( !name.startsWith( "$" ) ){
                        element.appendChild( m_document.createTextNode( name ) );
                        break;
                    }
                }
            }else{
                element.appendChild( m_document.createTextNode( obj + "" ) );
            }
            return true;
        }else{
            return false;
        }
    }
}
