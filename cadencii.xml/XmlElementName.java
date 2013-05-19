package com.github.cadencii.xml;

import java.lang.annotation.ElementType;
import java.lang.annotation.Inherited;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Inherited
@Target( {ElementType.FIELD, ElementType.METHOD} )
@Retention( RetentionPolicy.RUNTIME )
public @interface XmlElementName
{
    String value();
}
