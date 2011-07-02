#if !__FormAskKeySoundGenerationUiListener__
#define __FormAskKeySoundGenerationUiListener__
/*
 * FormAskKeySoundGenerationUiListener.cs
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

#if CSHARP
            using System;
            using org.kbinani.windows.forms;
            using org.kbinani.apputil;

            using boolean = System.Boolean;
            using BEventArgs = System.EventArgs;
            using BEventHandler = System.EventHandler;
#endif // CSHARP

#endif

#if __cplusplus
            class FormAskKeySoundGenerationUiListener
#else
            public interface FormAskKeySoundGenerationUiListener
#endif
            {
#if __cplusplus
            public:
#endif
                [PureVirtualFunction]
                void buttonCancelClickedSlot();

                [PureVirtualFunction]
                void buttonOkClickedSlot();
            };

#if JAVA

#else

        }
    }
}

#endif

#endif
