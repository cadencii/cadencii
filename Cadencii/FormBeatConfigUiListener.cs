#if !__FormBeatConfigUiListener__
#define __FormBeatConfigUiListener__
/*
 * FormBeatConfigUiListener.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA

package com.github.cadencii;

#else

namespace com.github
{
    namespace cadencii
    {

#endif

#if __cplusplus
            class FormBeatConfigUiListener
#else
            public interface FormBeatConfigUiListener
#endif
            {
#if __cplusplus
            public:
#endif
                [PureVirtualFunction]
                void buttonCancelClickedSlot();

                [PureVirtualFunction]
                void buttonOkClickedSlot();

                [PureVirtualFunction]
                void checkboxEndCheckedChangedSlot();
            };

#if !JAVA
    }
}
#endif
#endif
