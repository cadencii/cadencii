/*
 * MessageBodyEntry.java
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

import java.util.*;

public class MessageBodyEntry {
    public String Message;
    public Vector<String> Location = new Vector<String>();

    public MessageBodyEntry( String message, String[] location ) {
        Message = message;
        for ( int i = 0; i < location.length; i++ ) {
            Location.add( location[i] );
        }
    }
}
