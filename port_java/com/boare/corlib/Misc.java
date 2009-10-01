/*
 * Misc.java
 * Copyright (c) 2009 kbinani
 *
 * This file is part of com.boare.corlib.
 *
 * com.boare.corlib is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.corlib is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.corlib;

import java.util.*;

public class Misc{
    /**
     * 先頭と末尾の空要素を無視せずに，文字列itemをsplitterで区切ります．
     */
    public static String[] splitString( String item, String[] splitter ){
        Vector<String> ret = new Vector<String>();
        String remain = item;
        int len = splitter.length();
        int index = remain.indexOf( splitter );
        while( index >= 0 ){
            ret.add( remain.substring( 0, index ) );
            remain = remain.substring( index + len );
            index = remain.indexOf( splitter );
        }
        ret.add( remain );
        return ret.toArray( new String[]{} );
    }
}
