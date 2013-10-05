/*
 * AttackVariation.cs
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
using System.Drawing.Design;

namespace cadencii
{

    [TypeConverter(typeof(AttackVariationConverter))]
    public class AttackVariation
    {
        public string mDescription = "";

        public AttackVariation()
        {
            mDescription = "-";
        }

        public AttackVariation(string description)
        {
            this.mDescription = description;
        }

        public bool equals(Object obj)
        {
            if (obj != null && obj is AttackVariation) {
                return ((AttackVariation)obj).mDescription.Equals(mDescription);
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
            if (mDescription == null) {
                return "".GetHashCode();
            } else {
                return mDescription.GetHashCode();
            }
        }

    }

}
