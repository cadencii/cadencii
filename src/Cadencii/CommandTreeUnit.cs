/*
 * CommandTreeUnit.cs
 * Copyright © 2008-2011 kbinani
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
using System;
using System.Collections.Generic;
using cadencii.vsq;
using cadencii;
using cadencii.java.util;

namespace cadencii
{

    [Serializable]
    public class CommandTreeUnit
    {
        public CadenciiCommand Command = null;
        public CadenciiCommand Parent = null;
        public List<CadenciiCommand> Children = null;
    }

}
