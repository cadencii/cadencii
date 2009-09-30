/*
 * FileFilterWithExtension.java
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

public class FileFilterWithExtension implements FileFilter{
	private String m_ext;
	
	public FileFilterWithExtension( String ext ){
		m_ext = ext;
	}
	
	public boolean accept( File file ){
		if ( file.getAbsolutePath().endsWith( m_ext ) ){
			return true;
		}else{
			return false;
		}
	}
}
