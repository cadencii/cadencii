#if !JAVA
/*
 * CommandTreeUnit.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.java.util;

namespace Boare.Cadencii {

    [Serializable]
    public class CommandTreeUnit {
        public CadenciiCommand Command = null;
        public CadenciiCommand Parent = null;
        public Vector<CadenciiCommand> Children = null;
    }

}
#endif
