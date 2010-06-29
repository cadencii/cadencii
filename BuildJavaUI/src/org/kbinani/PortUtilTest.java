package org.kbinani;

import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStreamReader;
import java.util.Vector;
import org.kbinani.xml.XmlSerializer;

public class PortUtilTest {
    public int a = 0;
    public Integer b = 0;
    public Vector<PortUtilTest> vector = null;
        
    public static void main( String[] args ) throws Exception{
        PortUtilTest pt  = new PortUtilTest();
        pt.a = 10;
        pt.vector = new Vector<PortUtilTest>();
        pt.vector.add( new PortUtilTest() );
        
        System.out.println( "pt=" + pt );
        XmlSerializer xs = new XmlSerializer( PortUtilTest.class );
        FileOutputStream fos = new FileOutputStream( "foo.xml" );
        xs.serialize( fos, pt );
        fos.close();

        BufferedReader bsr = new BufferedReader( new InputStreamReader( new FileInputStream( "foo.xml" ) ) );
        while( bsr.ready() ){
            System.out.println( bsr.readLine() );
        }
        bsr.close();
        
        FileInputStream fis = new FileInputStream( "foo.xml" );
        PortUtilTest pt2 = (PortUtilTest)xs.deserialize( fis );
        fis.close();
        System.out.println( "pt2=" + pt2 );
    }

    public String toString(){
        String ret = "{a=" + this.a + ", b=" + this.b + ", vector={";
        if( vector != null ){
            for( int i = 0; i < vector.size(); i++ ){
                ret += (i == 0 ? "" : ", ") + vector.get( i );
            }
        }
        return ret + "}}";
    }
}
