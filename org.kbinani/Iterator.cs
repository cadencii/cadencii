#if !JAVA
/*
 * Iterator.cs
 * Copyright (C) 2009-2010 kbinani
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
namespace org.kbinani.java.util {

    public interface Iterator {
        bool hasNext();
        object next();
        void remove();
    }

    public interface Iterator<T> {
        bool hasNext();
        T next();
        void remove();
    }

}
#endif
