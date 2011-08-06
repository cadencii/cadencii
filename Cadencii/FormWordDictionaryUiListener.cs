#if !__FormWordDictionaryUiListener__
#define __FormWordDictionaryUiListener__
/*
 * FormWordDictionaryUiListener.cs
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

package org.kbinani.cadencii;

#else

namespace org
{
    namespace kbinani
    {
        namespace cadencii
        {

#endif

#if __cplusplus
            class FormWordDictionaryUiListener
#else
            public interface FormWordDictionaryUiListener
#endif
            {
                [PureVirtualFunction]
                void formClosing();

                [PureVirtualFunction]
                void formLoad();

                [PureVirtualFunction]
                void buttonOkClick();

                [PureVirtualFunction]
                void buttonUpClick();

                [PureVirtualFunction]
                void buttonDownClick();

                [PureVirtualFunction]
                void buttonCancelClick();
            };

#if !JAVA
        }
    }
}
#endif

#endif
