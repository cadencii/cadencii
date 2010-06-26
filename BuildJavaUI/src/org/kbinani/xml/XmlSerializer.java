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

    private Object parseNode( Class t, Class<?> parent_class, Node node ){
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
            return Enum.valueOf( t, str );
        }else if( t.isArray() || t.equals( Vector.class ) ){
PortUtil.println( "XmlSerializer#parseNode; t is array or Vector" );
            // Class tがstatic String getGenericTypeName( String )を実装しているかどうか調べる
            Method method = null;
            if( parent_class == null ){
PortUtil.println( "XmlSerializer#parseNode; parent_class is null; abort" );
                return null;
            }
            for( Method m : parent_class.getDeclaredMethods() ){
                if( !m.getName().equals( "getGenericTypeName" ) ){
                    continue;
                }
                int modifier = m.getModifiers();
                if( !Modifier.isStatic( modifier ) || !Modifier.isPublic( modifier ) ){
                    continue;
                }
                if( !m.getReturnType().equals( String.class ) ){
                    continue;
                }
                Class<?>[] args = m.getParameterTypes();
                if( args.length != 1 ){
                    continue;
                }
                if( !args[0].equals( String.class ) ){
                    continue;
                }
                method = m;
                break;
            }
            if( method == null ){
PortUtil.println( "XmlSerializer#parseNode; type t does not have method named 'getGenericTypeName'; abort" );
                return null;
            }
            try{
                String content_class_name = (String)method.invoke( null, node.getNodeName() );
                Class<?> content_class = Class.forName( content_class_name );
                Vector<Object> vec = new Vector<Object>();
                String element_name = getCliTypeName( content_class );
                if( element_name.equals( "" ) ){
                    element_name = content_class.getSimpleName();
                }
                for( int i = 0; i < numChild; i++ ){
                    Node c = childs.item( i );
                    if( c.getNodeType() == Node.ELEMENT_NODE ){
                        Element f = (Element)c;
                        if( !f.getTagName().equals( element_name ) ){
                            continue;
                        }
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

    private void printItemRecurse( Class t, Object obj, Element parent ) throws IllegalAccessException{
        try{
            if ( !tryWriteValueType( t, obj, parent ) ){
                if( t.isArray() || t.equals( Vector.class ) ){
                    Object[] array = null;
                    if( t.isArray() ){
                        array = (Object[])obj;
                    }else if( t.equals( Vector.class ) ){
                        array = ((Vector)obj).toArray();
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
            System.err.println( "printItemRecurse; ex=" + ex );
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
