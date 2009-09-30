/*
 * UstPortamentoPoint.java
 * Copyright (c) 2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

public class UstPortamentoPoint {
    public int step;
    public float value;
    public UstPortamentoType type;

    public static String getXmlElementName( String name ){
        if( name.equals( "step" ) ){
            return "Step";
        }else if( name.equals( "value" ) ){
            return "Value";
        }else if( name.equals( "type" ) ){
            return "Type";
        }
        return "";
    }
}
