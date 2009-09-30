/*
 * Collections.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
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
