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
#if JAVA
package cadencii;

#else
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle( "Cadencii" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( cadencii.BAssemblyInfo.id )]
[assembly: AssemblyCompany( "" )]
[assembly: AssemblyProduct( "Cadencii" )]
[assembly: AssemblyCopyright("Copyright © 2008-2013 kbinani. All Rights Reserved.")]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]
[assembly: ComVisible( false )]
[assembly: Guid( "5028b296-d7be-4278-a799-ffaf50026128" )]
[assembly: AssemblyVersion( "1.0.0.0" )]
[assembly: AssemblyFileVersion( cadencii.BAssemblyInfo.fileVersion )]

namespace cadencii {
#endif

    public class BAssemblyInfo {
        public const String id = "$Id$";
        public const String fileVersionMeasure = "3";
        public const String fileVersionMinor = "5";
        public const String fileVersion = fileVersionMeasure + "." + fileVersionMinor + ".2";
    }

#if !JAVA
}
#endif
