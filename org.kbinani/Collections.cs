#if !JAVA
/*
 * Collections.cs
 * Copyright © 2009-2010 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
//#define VECTOR_TEST
namespace org.kbinani.java.util {

    public static class Collections {
        public static void sort<T>( Vector<T> list ) {
#if !VECTOR_TEST
            list.Sort();
#endif
        }
    }

}
#endif
