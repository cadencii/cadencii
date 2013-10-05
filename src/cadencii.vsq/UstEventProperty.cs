/*
 * UstEventProperty.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace cadencii.vsq
{

    [Serializable]
    public class UstEventProperty
    {
        public string Name;
        public string Value;

        public UstEventProperty()
        {
        }

        public UstEventProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

}
