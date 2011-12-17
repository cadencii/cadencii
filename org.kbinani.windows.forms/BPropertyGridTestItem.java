package com.github.cadencii.windows.forms;

import java.awt.Point;
import java.awt.Rectangle;
import java.util.Vector;
import org.kbinani.componentmodel.Category;
import org.kbinani.componentmodel.IPropertyDescriptor;
import org.kbinani.componentmodel.PropertyDescriptor;
import org.kbinani.componentmodel.TypeConverter;
import org.kbinani.componentmodel.TypeConverterAnnotation;

public class BPropertyGridTestItem implements IPropertyDescriptor {
    public String B = "text";
    public boolean A = true;
    //public int C = 10;
    public Point D = new Point( 1, 2 );
    //public Rectangle E = new Rectangle( 1, 2, 3, 4 );
    //public FooEnum F = FooEnum.VALUE1;
    @Category( "Category!!" )
    public BPropertyGridTestItemG G = new BPropertyGridTestItemG( "G2" );
    public BPropertyGridTestH H = new BPropertyGridTestH( 101, 103 );

    @Override
    public String toString()
    {
        return "[A=" + A + ",B=" + B + ",D=[x=" + D.x + ",y=" + D.y + "],G=" + G + ",H=[x=" + H.x + ",y=" + H.y + "]]";
    }
    
    @Override
    public PropertyDescriptor getDescriptor() {
        return new BPropertyGridTestItemPropertyDescriptor();
    }
}

class BPropertyGridTestHTypeConverter extends TypeConverter<BPropertyGridTestH>
{
    public String convertTo( Object obj )
    {
        if( obj == null ){
            return "";
        }else if( obj instanceof BPropertyGridTestH ){
            BPropertyGridTestH item = (BPropertyGridTestH)obj;
            return item.x + "," + item.y;
        }else{
            return "";
        }
    }

    public BPropertyGridTestH convertFrom( String s )
    {
        int indx = s.indexOf( ',' );
        String sx = s.substring( 0, indx );
        String sy = s.substring( indx + 1 );
        int x = Integer.parseInt( sx );
        int y = Integer.parseInt( sy );
        return new BPropertyGridTestH( x, y );
    }
}

@TypeConverterAnnotation( BPropertyGridTestHTypeConverter.class )
class BPropertyGridTestH{
    public int x;
    public int y;

    public BPropertyGridTestH( int x, int y )
    {
        this.x = x;
        this.y = y;
    }
}

class BPropertyGridTestItemPropertyDescriptor extends PropertyDescriptor
{
    @Override
    public String getDisplayName( String name )
    {
        return name;
    }
}

@TypeConverterAnnotation( BPropertyGridTestItemGTypeConverter.class )
class BPropertyGridTestItemG{
    private String mValue;
    
    public BPropertyGridTestItemG( String value )
    {
        mValue = value;
    }
    
    @Override
    public String toString(){
        return mValue;
    }
    
    @Override
    public boolean equals( Object o )
    {
        if( o == null ){
            return false;
        }
        if( o instanceof BPropertyGridTestItemG ){
            String s = ((BPropertyGridTestItemG)o).mValue;
            if( s == null ){
                return false;
            }else{
                return s.equals( mValue );
            }
        }else{
            return false;
        }
    }
}

class BPropertyGridTestItemGTypeConverter extends TypeConverter<BPropertyGridTestItemG>
{
    @Override
    public String convertTo( Object o )
    {
        if( o == null ){
            return "";
        }
        if( o instanceof BPropertyGridTestItemG ){
            return ((BPropertyGridTestItemG)o).toString();
        }else{
            return "";
        }
    }
    
    @Override
    public BPropertyGridTestItemG convertFrom( String s )
    {
        for( BPropertyGridTestItemG i : getStandardValues() ){
            if( i.toString().equals( s ) ){
                return i;
            }
        }
        return new BPropertyGridTestItemG( "G1" );
    }
    
    @Override
    public boolean isStandardValuesSupported()
    {
        return true;
    }

    @Override
    public Vector<BPropertyGridTestItemG> getStandardValues()
    {
        Vector<BPropertyGridTestItemG> ret = new Vector<BPropertyGridTestItemG>();
        ret.add( new BPropertyGridTestItemG( "G1" ) );
        ret.add( new BPropertyGridTestItemG( "G2" ) );
        ret.add( new BPropertyGridTestItemG( "G3" ) );
        ret.add( new BPropertyGridTestItemG( "G4" ) );
        ret.add( new BPropertyGridTestItemG( "G5" ) );
        ret.add( new BPropertyGridTestItemG( "G6" ) );
        ret.add( new BPropertyGridTestItemG( "G7" ) );
        ret.add( new BPropertyGridTestItemG( "G8" ) );
        ret.add( new BPropertyGridTestItemG( "G9" ) );
        ret.add( new BPropertyGridTestItemG( "G10" ) );
        ret.add( new BPropertyGridTestItemG( "G11" ) );
        ret.add( new BPropertyGridTestItemG( "G12" ) );
        ret.add( new BPropertyGridTestItemG( "G13" ) );
        return ret;
    }

}
