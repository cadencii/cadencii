/*
 * ScriptProcessor.cs
 * Copyright © 2013 kbinani
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
using System.Text;
using System.IO;
using System.Linq;

namespace cadencii
{
    abstract class ScriptProcessor
    {
        protected abstract string getPrefix();

        protected abstract string getSuffix();

        public string process( string code )
        {
            var targetCode = getPrefix() + code + getSuffix();
            return targetCode
                .Replace( "Boare.Lib.Vsq", "cadencii.vsq" )
                .Replace( "Boare.Lib.AppUtil", "cadencii.apputil" )
                .Replace( "Boare.Lib.Media", "cadencii.media" )
                .Replace( "Boare.Cadencii", "cadencii" )
                .Replace( "org.kbinani.vsq", "cadencii.vsq" )
                .Replace( "org.kbinani.apputil", "cadencii.apputil" )
                .Replace( "org.kbinani.cadencii", "cadencii" )
                .Replace( "org.kbinani.java", "cadencii.java" )
                .Replace( "org.kbinani", "cadencii" )
                .Replace( "bocoreex", "cadencii.javax" )
                .Replace( "bocoree", "cadencii.java" )
                .Replace( "InputBox", typeof( cadencii.windows.forms.InputBox ).FullName )
                ;
        }
    }

    class ScriptProcessorVersion1 : ScriptProcessor
    {
        protected override string getPrefix() {
            return
                "using System;" +
                "using System.IO;" +
                "using Boare.Lib.Vsq;" +
                "using Boare.Cadencii;" +
                "using bocoree;" +
                "using bocoree.io;" +
                "using bocoree.util;" +
                "using bocoree.awt;" +
                "using Boare.Lib.Media;" +
                "using Boare.Lib.AppUtil;" +
                "using System.Windows.Forms;" +
                "using System.Collections.Generic;" +
                "using System.Drawing;" +
                "using System.Text;" +
                "using System.Xml.Serialization;" +
                "namespace cadencii.plugin {";
        }
        protected override string getSuffix() { return "}"; }
    }

    class ScriptProcessorVersion2 : ScriptProcessor
    {
        protected override string getPrefix() {
            return
                "using System;" + 
                "using System.IO;" +
                "using org.kbinani.vsq;" +
                "using org.kbinani.cadencii;" +
                "using org.kbinani;" +
                "using org.kbinani.java.io;" +
                "using org.kbinani.java.util;" +
                "using org.kbinani.java.awt;" +
                "using org.kbinani.media;" +
                "using org.kbinani.apputil;" +
                "using System.Windows.Forms;" +
                "using System.Collections.Generic;" +
                "using System.Drawing;" +
                "using System.Text;" +
                "using System.Xml.Serialization;" +
                "namespace cadencii.plugin {";
        }
        protected override string getSuffix() { return "}"; }
    }

    class ScriptProcessorVersion3 : ScriptProcessor
    {
        protected override string getPrefix()
        {
            return
                "using System;" +
                "using System.IO;" +
                "using cadencii.vsq;" +
                "using cadencii;" +
                "using cadencii.java.io;" +
                "using cadencii.java.util;" +
                "using cadencii.java.awt;" +
                "using cadencii.media;" +
                "using cadencii.apputil;" +
                "using System.Windows.Forms;" +
                "using System.Collections.Generic;" +
                "using System.Drawing;" +
                "using System.Text;" +
                "using System.Xml.Serialization;" +
                "namespace cadencii.plugin {";
        }
        protected override string getSuffix() { return "}"; }
    }
}
