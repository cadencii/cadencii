#if !JAVA
/*
 * Collections.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
//#define VECTOR_TEST
namespace bocoree {

    public static class Collections {
        public static void sort<T>( Vector<T> list ) {
#if !VECTOR_TEST
            list.Sort();
#endif
        }
    }

}
#endif
