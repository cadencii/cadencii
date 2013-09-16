#if !__UiBase_h__
#define __UiBase_h__
/*
 * UiBase.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA

package cadencii;

#else

    namespace cadencii
    {

#if __cplusplus
#else
            using System;
#endif

#endif

#if __cplusplus
            class UiBase
#else
            public interface UiBase
#endif
            {
#if __cplusplus
                virtual int showDialog( QObject *parent_form );
#else
                int showDialog( Object parent_form );
#endif
            };

#if !JAVA
    }
#endif

#endif
