#if !JAVA
/*
 * CommandTreeUnit.cs
 * Copyright (C) 2008-2009 kbinani
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
using System;
using System.Collections.Generic;
using org.kbinani.vsq;
using org.kbinani;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {

    [Serializable]
    public class CommandTreeUnit {
        public CadenciiCommand Command = null;
        public CadenciiCommand Parent = null;
        public Vector<CadenciiCommand> Children = null;
    }

}
#endif
