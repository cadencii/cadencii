/*
 * ISO639.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Lib.AppUtil.
 *
 * Boare.Lib.AppUtil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.AppUtil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System.Globalization;

namespace Boare.Lib.AppUtil {

    internal class iso639 {
        public static bool CheckValidity( string code_string ) {
            try {
                CultureInfo c = CultureInfo.CreateSpecificCulture( code_string );
            } catch {
                return false;
            }
            return true;
        }
    }

}
