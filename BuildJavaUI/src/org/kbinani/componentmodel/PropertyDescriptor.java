package org.kbinani.componentmodel;

public class PropertyDescriptor {
    public String getDisplayName( String property_name ){
        return property_name;
    }

    public TypeConverter<?, ?> getTypeConverter( String property_name )
    {
        return null;
    }
}
