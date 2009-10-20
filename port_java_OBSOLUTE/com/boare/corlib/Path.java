/*
 * Path.java
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

import java.io.*;

public class Path{
    public static String getDirectoryName( String path ){
        String filename = getFileName( path );
        int index = path.lastIndexOf( filename );
        return path.substring( 0, index );
    }

    public static String getFileName( String fname ){
        int i = fname.lastIndexOf( File.pathSeparator );
        if( i > 0 ){
            return fname.substring( 0, i );
        }else{
            return fname;
        }
    }

    public static String getFileNameWithoutExtension( String file ){
        String fname = getFileName( file );
        int i = fname.lastIndexOf( "." );
        if( i > 0 ){
            return fname.substring( 0, i );
        }else{
            return fname;
        }
    }

    public static String combine( String file1, String file2 ){
        if( file1.endsWith( File.pathSeparator ) ){
            file1 = file1.substring( 0, file1.length() - 1 );
        }
        if( file2.startsWith( File.pathSeparator ) ){
            file2 = file2.substring( 1 );
        }
        return file1 + File.separator + file2;
    }
}
