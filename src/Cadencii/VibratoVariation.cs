#if ENABLE_PROPERTY
/*
 * VibratoVariation.cs
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
using System;
using System.ComponentModel;



namespace cadencii
{

    [TypeConverter(typeof(VibratoVariationConverter))]
    public class VibratoVariation
    {
        public static readonly VibratoVariation empty = new VibratoVariation();

        public string description = "";

        private VibratoVariation()
        {
            description = "-";
        }

        public VibratoVariation(string description)
        {
            this.description = description;
        }

        public bool equals(Object obj)
        {
            if (obj != null && obj is VibratoVariation) {
                return ((VibratoVariation)obj).description.Equals(description);
            } else {
                return base.Equals(obj);
            }
        }

        public override bool Equals(Object obj)
        {
            return equals(obj);
        }

        public override int GetHashCode()
        {
            if (description == null) {
                return "".GetHashCode();
            } else {
                return description.GetHashCode();
            }
        }

        public Object clone()
        {
            return new VibratoVariation(this.description);
        }

        public Object Clone()
        {
            return clone();
        }

    }

}
#endif
