/*
 * TextMemoryStream.java
 * Copyright (c) 2008-2009 kbinani
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

import java.util.*;
import java.io.*;
import com.boare.corlib.*;

public class TextMemoryStream {
    private Vector<String> m_lines;
    private int m_index;

    public TextMemoryStream() {
        m_lines = new Vector<String>();
        m_lines.add( "" );
        m_index = 0;
    }

    public TextMemoryStream( String path, String encoding ) {
        m_lines = new Vector<String>();
        m_index = 0;
        if ( (new File( path )).exists() ) {
            try{
                StreamReader sr = new StreamReader( path, encoding );
                String line;
                while ( (line = sr.readLine()) != null ) {
                    m_lines.add( line );
                    m_index++;
                }
            }catch( Exception ex ){
            }
        }
    }

    public void write( String value ) {
        m_lines.set( m_index, m_lines.get( m_index ) + value );
    }

    public void writeLine( String value ) {
        m_lines.set( m_index, m_lines.get( m_index ) + value );
        m_lines.add( "" );
        m_index++;
    }

    public void rewind() {
        m_index = 0;
    }

    public String readLine() {
        m_index++;
        return m_lines.get( m_index - 1 );
    }

    public int peek() {
        if ( m_index < m_lines.size() ) {
            if ( m_lines.get( m_index ).equals( "" ) ) {
                return -1;
            } else {
                return (int)m_lines.get( m_index ).charAt( 0 );
            }
        } else {
            return -1;
        }
    }

    public void close() {
        m_lines.clear();
    }
}
