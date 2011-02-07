package org.kbinani.windows.forms;

import java.awt.Point;
import java.awt.Rectangle;
import java.util.Vector;
import org.kbinani.componentmodel.IPropertyDescriptor;
import org.kbinani.componentmodel.PropertyDescriptor;
import org.kbinani.componentmodel.TypeConverter;

public class BPropertyGridTestItem implements IPropertyDescriptor {
    public boolean A = true;
    public String B = "text";
    //public int C = 10;
    //public Point D = new Point( 1, 2 );
    //public Rectangle E = new Rectangle( 1, 2, 3, 4 );
    //public FooEnum F = FooEnum.VALUE1;
    public String G = "a";

    @Override
    public PropertyDescriptor getDescriptor() {
        return new BPropertyGridTestItemPropertyDescriptor();
    }
}

class BPropertyGridTestGTypeConverter extends TypeConverter
{
    @Override
    public boolean isStandardValuesSupported()
    {
        return true;
    }

    @Override
    public Vector<Object> getStandardValues()
    {
        Vector<Object> ret = new Vector<Object>();
        ret.add( "G1" );
        ret.add( "G2" );
        ret.add( "G3" );
        return ret;
    }
}

class BPropertyGridTestItemPropertyDescriptor extends PropertyDescriptor
{
    @Override
    public String getDisplayName( String name )
    {
        return name;
    }
    
    @Override
    public TypeConverter<?, ?> getTypeConverter( String name )
    {
        if( name.equals( "G" ) ){
            return new BPropertyGridTestGTypeConverter();
        }else{
            return null;
        }
    }
}
