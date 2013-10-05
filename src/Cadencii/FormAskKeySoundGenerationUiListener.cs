/*
 * FormAskKeySoundGenerationUiListener.cs
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

    namespace cadencii
    {

#if CSHARP
            using System;
            using cadencii.windows.forms;
            using cadencii.apputil;
#endif // CSHARP

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


    }
