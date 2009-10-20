package com.boare.cadencii;

import java.util.*;
import com.boare.util.*;

public class Test implements PropertyDescripter{
    public int foo;
    public int bar;
    public String val;
    public Enumerated enumValue;
    public Test2 test2 = new Test2();
    public String[] testStringArray = new String[2];
    public Vector<Integer> testIntVector = new Vector<Integer>();

    public enum Enumerated{
        entry1,
        entry2,
    }

    public Test(){
        testStringArray[0] = "foo1";
        testStringArray[1] = "foo2";
        testIntVector.add( -1 );
        testIntVector.add( 100 );
    }

    public boolean canConvertTo( Class destination ){
        if( destination.equals( String.class ) ){
            return true;
        }
        return false;
    }

    public Object convertTo( Object value, Class destinationType ){
        if( value == null ){
            return null;
        }
        if( destinationType.equals( String.class ) && value instanceof Test ){
            return ((Test)value).toString();
        }else{
            return null;
        }
    }

    public boolean canConvertFrom( Class sourceType ){
        if( sourceType.equals( String.class ) ){
            return true;
        }
        return false;
    }

    public Object convertFrom( Object value ){
        if( value == null ){
            return null;
        }
        if( value.getClass().equals( String.class ) ){
            return new Test();
        }
        return new Test();
    }

    public String toString(){
        return "{foo=" + foo + 
               ",bar=" + bar + 
               ",val=" + val +
               ",enumValue=" + enumValue + 
               ",test2=" + test2.toString() + 
               ",testStringArray=" + Arrays.toString( testStringArray ) + 
               ",testIntVector=" + Arrays.toString( testIntVector.toArray( new Integer[]{} ) ) + "}";
    }

    public static String getGenericTypeName( String name ){
        if( name.equals( "testStringArray" ) ){
            return "java.lang.String";
        }else if( name.equals( "testIntVector" ) ){
            return "java.lang.Integer";
        }
        return "";
    }

    public String getPropertyCategory( String name ){
        if( name.equals( "foo" ) ){
            return "";
        }else if( name.equals( "bar" ) ){
            return "categ";
        }else if( name.equals( "val" ) ){
            return "categ";
        }else if( name.equals( "test2" ) ){
            return "あ";
        }
        return "";
    }

    public String getPropertyName( String name ){
        if( name.equals( "foo" ) ){
            return "Foo";
        }else if( name.equals( "bar" ) ){
            return "Bar";
        }else if( name.equals( "val" ) ){
            return "Val";
        }
        return name;
    }
}
