#if !__FormWordDictionaryUiListener__
#define __FormWordDictionaryUiListener__
/*
 * FormWordDictionaryUiListener.cs
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
#endif

#endif
