/*
 * OtoArgs.cs
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



namespace cadencii.utau
{

    /// <summary>
    /// 原音設定の引数．
    /// </summary>
    public struct OtoArgs
    {
        public string fileName;
        public string Alias;
        public float msOffset;
        public float msConsonant;
        public float msBlank;
        public float msPreUtterance;
        public float msOverlap;

        public bool equals(OtoArgs obj)
        {
            if (this.fileName != null && this.fileName.Equals(obj.fileName) &&
                 this.Alias != null && this.Alias.Equals(obj.Alias) &&
                 this.msOffset == obj.msOffset &&
                 this.msConsonant == obj.msConsonant &&
                 this.msBlank == obj.msBlank &&
                 this.msPreUtterance == obj.msPreUtterance &&
                 this.msOverlap == obj.msOverlap) {
                return true;
            } else {
                return false;
            }
        }

        public bool isEmpty()
        {
            return fileName == null || Alias == null;
        }
    }

}
