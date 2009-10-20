/*
 * StreamWriter.java
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

public class StreamWriter{
    private FileOutputStream m_fs;
    private OutputStreamWriter m_out;
    private BufferedWriter m_bw;
    
    public StreamWriter( String path ) throws IOException{
        this( path, "UTF-8" );
    }
    
    public StreamWriter( String path, String encoding ) throws IOException{
        m_fs = new FileOutputStream( path );
        m_out = new OutputStreamWriter( m_fs, encoding );
        m_bw = new BufferedWriter( m_out );
    }
    
    public void writeLine( String s ) throws IOException{
        m_bw.write( s );
        m_bw.newLine();
    }

    public void write( String s ) throws IOException{
        m_bw.write( s );
    }

    public void writeLine() throws IOException{
        m_bw.newLine();
    }

    public void close() throws IOException{
        m_bw.close();
    }
}
