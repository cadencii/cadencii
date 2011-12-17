package com.github.cadencii.xml;

import java.awt.Point;
import java.awt.Rectangle;
import java.io.InputStream;
import java.io.OutputStream;
import java.lang.reflect.Array;
import java.lang.reflect.Field;
import java.util.AbstractList;
import java.util.Vector;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import org.w3c.dom.DOMImplementation;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

class TestDummy{
    public int intValue = 0;
}

class TestXmlSerializer
{
    public static void main( String[] args )
    {
        TestDummy td = new TestDummy();
    }
}

/**
 * .NETのSystem.Xml.Serialization.XmlSerializerと同じ書式で入出力するためのXMLシリアライザ．<br>
 * Vectorなどの型引数をとる型の場合，型引数の型を特定するためのアノテーションを追加しなければなりません．<br>
 * 例えば以下の例ではlistフィールドはInteger型のVectorであることを示すために
 * XmlGenericType( Integer.class )が追加されています．
 * また，XMLの要素名にプロパティ名以外のものを使いたい場合，@XmlElementNameアノテーションを追加します．
 * 以下の例では，valueをValueに，listをListに変更しています
 * <pre>
 *  public class Test2{
    @XmlElementName( "Value" )
    public float value = 1.0f;
    private int m_a = 2;
    private boolean m_b = true;
    @XmlElementName( "List" )
    @XmlGenericType( Integer.class )
    public Vector<Integer> list = new Vector<Integer>();
    
    public boolean isHoge(){
        return m_b;
    }
    
    public void setHoge( boolean value ){
        m_b = value;
    }

    @XmlIgnore
    public int getA(){
        return m_a;
    }
    
    public void setA( int value ){
        m_a = value;
    }
}
 * </pre>
 * このように実装しておくと，だいたい以下のようなXMLの入出力が可能になります．
 * この例では，プロパティAはXmlIgnoreあのテーションがついているためXMLには現れないようになっています．
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
        XmlMember xm = XmlMember.extract( cls, property_name );
        if( xm != null ){
            XmlGenericType gt = xm.getGetterAnnotation( XmlGenericType.class );
            if( gt != null ){
                return gt.value();
            }else{
                gt = xm.getSetterAnnotation( XmlGenericType.class );
                if( gt != null ){
                    return gt.value();
                }
            }
        }
        return null;
    }

    private Object parseNode( Class<?> t, Class<?> parent_class, Node node )
        throws Exception
    {
        NodeList childs = node.getChildNodes();
        int numChild = childs.getLength();
        Object obj;
        if( t.equals( Integer.TYPE ) || t.equals( Integer.class ) ){
            return Integer.parseInt( node.getTextContent() + "" );
        }else if( t.equals( Byte.TYPE ) || t.equals( Byte.class ) ){
            return Byte.parseByte( node.getTextContent() + "" );
        }else if( t.equals( Short.TYPE ) || t.equals( Short.class ) ){
            return Short.parseShort( node.getTextContent() + "" );
        }else if( t.equals( Float.TYPE ) || t.equals( Float.class ) ){
            return Float.parseFloat( node.getTextContent() + "" );
        }else if( t.equals( Double.TYPE ) || t.equals( Double.class ) ){
            return Double.parseDouble( node.getTextContent() + "" );
        }else if( t.equals( Boolean.TYPE ) || t.equals( Boolean.class ) ){
            return Boolean.parseBoolean( node.getTextContent() + "" );
        }else if( t.equals( String.class ) ){
            return node.getTextContent() + "";
        }else if( t.isEnum() ){
            String str = node.getTextContent() + "";
            Object ret = null;
            for( Object o : t.getEnumConstants() ){
                if( o.toString().equals( str ) ){
                    ret = o;
                    break;
                }
            }
            return ret;
        }else if( t.equals( Rectangle.class ) ){
            // C#のXmlSerializerとの互換性のために必要
            Rectangle ret = new Rectangle();
            for( int i = 0; i < numChild; i++ ){
                Node n = childs.item( i );
                String tc = n.getTextContent();
                int inte = 0;
                try{
                    inte = Integer.parseInt( tc );
                }catch( Exception ex ){
                }
                String nodename = n.getNodeName();
                if( nodename.equals( "X" ) ){
                    ret.x = inte;
                }else if( nodename.equals( "Y" ) ){
                    ret.y = inte;
                }else if( nodename.equals( "Width" ) ){
                    ret.width = inte;
                }else if( nodename.equals( "Height" ) ){
                    ret.height = inte;
                }
            }
            return ret;
        }else if( t.equals( Point.class ) ){
            // C#のXmlSerializerとの互換性のために必要
            Point ret = new Point();
            for( int i = 0; i < numChild; i++ ){
                Node n = childs.item( i );
                String tc = n.getTextContent();
                int inte = 0;
                try{
                    inte = Integer.parseInt( tc );
                }catch( Exception ex ){
                }
                String nodename = n.getNodeName();
                if( nodename.equals( "X" ) ){
                    ret.x = inte;
                }else if( nodename.equals( "Y" ) ){
                    ret.y = inte;
                }
            }
            return ret;
        }else if( t.isArray() || t.equals( Vector.class ) || isInterfaceDeclared( t, AbstractList.class ) ){
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
                        "please add @XmlGenericType annotation" );
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
                    // 配列の場合
                    int length = vec.size();
                    Object arr = Array.newInstance( content_class, length );
                    for( int i = 0; i < length; i++ ){
                        Array.set( arr, i, vec.get( i ) );
                    }
                    return arr;
                }else if( t.equals( Vector.class ) ){
                    // Vectorの場合
                    return vec;
                }else{
                    // AbstractListを実装した型の場合
                    Object ret = null;
                    try{
                        ret = t.newInstance();
                    }catch( Exception ex ){
                        throw new Exception( "XmlSerializer#parseNode; error; " +
                                             "cannot make new instance of '" + t + "'." +
                                             "in order to enable XML serialization, " + 
                                             "please enable constructor which has no parameter" );
                    }
                    AbstractList abst_list = (AbstractList<?>)ret;
                    int length = vec.size();
                    for( int i = 0; i < length; i++ ){
                        abst_list.add( vec.get( i ) );
                    }
                    return abst_list;
                }
            }catch( Exception ex ){
                System.err.println( "XmlSerializer.parseNode; ex=" + ex );
                return null;
            }
        }
        try{
            obj = t.newInstance();
        }catch( Exception ex ){
            throw new Exception( "XmlSerializer#parseNode; error; " +
                    "cannot make new instance of '" + t + "'." +
                    "in order to enable XML serialization, " + 
                    "please enable constructor which has no parameter" );
        }
        XmlMember[] members = XmlMember.extractMembers( t );
        for( int i = 0; i < numChild; i++ ){
            Node c = childs.item( i );
            if( c.getNodeType() == Node.ELEMENT_NODE ) {
                Element f = (Element)c;
                for( XmlMember xm : members ){
                    if( f.getTagName().equals( xm.getXmlName() ) ) {
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

    private void parseFieldAndProperty( Class<?> t, Object obj, Element el )
        throws IllegalAccessException
    {
        if( obj == null ){
            return;
        }
        XmlMember[] members = XmlMember.extractMembers( t );
        for( XmlMember xm : members ){
            String name = xm.getXmlName();
            Element el2 = m_document.createElement( name );
            printItemRecurse( xm.getType(), xm.get( obj ), el2 );
            el.appendChild( el2 );
        }
    }

    /**
     * 指定したクラスが、指定したインターフェースを実装しているかどうかを調べます
     * @param cls 検査対象のクラス
     * @param itfc 検出するインターフェースのクラス
     * @return 指定したインターフェースを実装していればtrue，そうでなければfalse
     */
    public static boolean isInterfaceDeclared( Class<?> cls, Class<?> itfc ){
        if( cls.equals( itfc ) ){
            return true;
        }
        for( Class<?> c : cls.getInterfaces() ){
            if( c.equals( itfc ) ){
                return true;
            }
            boolean ret = isInterfaceDeclared( c, itfc );
            if( ret ){
                return ret;
            }
        }
        
        Class<?> super_class = cls.getSuperclass();
        if( super_class != null ){
            if( super_class.equals( itfc ) ){
                return true;
            }
            return isInterfaceDeclared( super_class, itfc );
        }
        return false;
    }
    
    private void printItemRecurse( Class<?> t, Object obj, Element parent ) throws IllegalAccessException{
        try{
            if ( !tryWriteValueType( t, obj, parent ) ){
                if( t.isArray() || t.equals( Vector.class ) || isInterfaceDeclared( t, AbstractList.class ) ){
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
                            array = ((Vector<?>)obj).toArray();
                        }else{
                            // AbstractListを実装している型である場合
                            AbstractList<?> abst_list = (AbstractList<?>)obj;
                            Vector<Object> vec = new Vector<Object>();
                            int size = abst_list.size();
                            for( int i = 0; i < size; i++ ){
                                vec.add( abst_list.get( i ) );
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
                }else if( t.equals( java.awt.Rectangle.class ) ){
                    // 特例．ここで処理しないと，stack overflowになる
                    Element e_x = m_document.createElement( "X" );
                    Element e_y = m_document.createElement( "Y" );
                    Element e_width = m_document.createElement( "Width" );
                    Element e_height = m_document.createElement( "Height" );
                    java.awt.Rectangle rc = null;
                    if( obj instanceof java.awt.Rectangle ){
                        rc = (java.awt.Rectangle)obj;
                    }else{
                        rc = new java.awt.Rectangle();
                    }
                    e_x.appendChild( m_document.createTextNode( rc.x + "" ) );
                    e_y.appendChild( m_document.createTextNode( rc.y + "" ) );
                    e_width.appendChild( m_document.createTextNode( rc.width + "" ) );
                    e_height.appendChild( m_document.createTextNode( rc.height + "") );
                    parent.appendChild( e_x );
                    parent.appendChild( e_y );
                    parent.appendChild( e_width );
                    parent.appendChild( e_height );
                }else if( t.equals( java.awt.Point.class ) ){
                    // ここで処理しないと，x, yの他にX, Yが追加されてしまう
                    Element e_x = m_document.createElement( "X" );
                    Element e_y = m_document.createElement( "Y" );
                    java.awt.Point p = null;
                    if( obj instanceof java.awt.Point ){
                        p = (java.awt.Point)obj;
                    }else{
                        p = new java.awt.Point();
                    }
                    e_x.appendChild( m_document.createTextNode( p.x + "" ) );
                    e_y.appendChild( m_document.createTextNode( p.y + "" ) );
                    parent.appendChild( e_x );
                    parent.appendChild( e_y );
                }else{
                    parseFieldAndProperty( t, obj, parent );
                }
            }
        }catch( Exception ex ){
            System.err.println( "XmlSerializer#printItemRecurse; ex=" + ex );
            ex.printStackTrace();
        }
    }

    /**
     * 指定した型の，対応するCLI型の(C#での)名称を取得します．
     * 該当するものがなければ空文字を返します．
     * @param t
     * @return
     */
    public static String getCliTypeName( Class<?> t ){
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
    
    private boolean tryWriteValueType( Class<?> t, Object obj, Element element ){
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
