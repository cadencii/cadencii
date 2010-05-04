/*
 * SymbolTableEntry.cs
 * Copyright (C) 2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.io.*;
#else
using System;

namespace org.kbinani.vsq{
#endif

#if JAVA
    public class SymbolTableEntry implements Cloneable {
#else
    public class SymbolTableEntry : ICloneable {
#endif
        public String Word = "";
        public String Symbol = "";

        public SymbolTableEntry( String word, String symbol ) {
            Word = word;
            if ( Word == null ) {
                Word = "";
            }
            Symbol = symbol;
            if ( Symbol == null ) {
                Symbol = "";
            }
        }

        public Object clone() {
            return new SymbolTableEntry( Word, Symbol );
        }

#if !JAVA
        public Object Clone(){
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
