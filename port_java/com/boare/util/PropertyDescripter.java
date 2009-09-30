package com.boare.util;

public interface PropertyDescripter{
    String getPropertyCategory( String name );
    String getPropertyName( String name );
    boolean canConvertTo( Class destinationClass );
    Object convertTo( Object value, Class destinationType );
    boolean canConvertFrom( Class sourceType );
    Object convertFrom( Object value );
}
