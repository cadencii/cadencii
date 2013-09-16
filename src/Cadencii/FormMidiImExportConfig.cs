/*
 * FormMidiImExportConfig.cs
 * Copyright © 2009-2011 kbinani
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
using System;

namespace cadencii {
    using boolean = Boolean;
#endif

    public class FormMidiImExportConfig {
        public boolean LastPremeasureCheckStatus = true;
        public boolean LastMetatextCheckStatus = true;
    }

#if !JAVA
}
#endif
