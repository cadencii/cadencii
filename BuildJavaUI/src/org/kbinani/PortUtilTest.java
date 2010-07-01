package org.kbinani;

import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStreamReader;
import java.util.*;
import org.kbinani.xml.XmlSerializer;

public class PortUtilTest extends Vector<Integer> {
    public int a = 0;
    public Integer b = 0;
    public Vector<PortUtilTest> vector = null;
        
    public static void main( String[] args ){
        System.out.println( XmlSerializer.isInterfaceDeclared( PortUtilTest.class, AbstractList.class ) );
    }

    public static String getGenericTypeName( String name ){
        if( name != null ){
            if( name.equals( "vector" ) ){
                return "org.kbinani.PortUtilTest";
            }
        }
        return "";
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
