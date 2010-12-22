#if !JAVA
/*
 * Iterator.cs
 * Copyright (C) 2009-2010 kbinani
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
namespace org {
    namespace kbinani {
        namespace vsq {

#if __cplusplus
            template<class E>
            class Iterator
#else
            public interface Iterator<E>
#endif
            {
                bool hasNext();
                E next();
                void remove();
            }

        }
    }
}
#endif
