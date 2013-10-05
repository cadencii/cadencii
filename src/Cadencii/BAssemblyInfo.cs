/*
 * BAssemblyInfo.cs
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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Cadencii")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration(cadencii.BAssemblyInfo.id)]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Cadencii")]
[assembly: AssemblyCopyright("Copyright © 2008-2013 kbinani. All Rights Reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("5028b296-d7be-4278-a799-ffaf50026128")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion(cadencii.BAssemblyInfo.fileVersion)]

namespace cadencii
{

    public class BAssemblyInfo
    {
        public const string id = "$Id$";
        public const string fileVersionMeasure = "3";
        public const string fileVersionMinor = "5";
        public const string fileVersion = fileVersionMeasure + "." + fileVersionMinor + ".4";
        public const string downloadUrl = "http://sourceforge.jp/projects/cadencii/releases/59580/" + "Cadencii_v" + fileVersion;
    }

}
