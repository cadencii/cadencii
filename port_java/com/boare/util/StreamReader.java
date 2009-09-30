/*
 * StreamReader.java
 * Copyright (c) 2009 kbinani
 *
 * This file is part of com.boare.util.
 *
 * com.boare.util is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.util is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.util;

import java.io.*;

public class StreamReader{
    private FileInputStream m_fs;
    private InputStreamReader m_in;
    private BufferedReader m_br;

    public StreamReader( String path ) throws IOException{
        this( path, "UTF-8" );
    }

    public StreamReader( String path, String encoding ) throws IOException{
        m_fs = new FileInputStream( path );
        m_in = new InputStreamReader( m_fs, encoding );
        m_br = new BufferedReader( m_in );
    }

    public String readLine() throws IOException{
        return m_br.readLine();
    }

    public void close() throws IOException{
        m_br.close();
    }
}
