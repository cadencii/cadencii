/*
 * XmlStaticMemberSerializerEx.cs
 * Copyright © 2010-2011 kbinani
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
using cadencii.java.util;
using cadencii.xml;



namespace cadencii
{

    public class XmlStaticMemberSerializerEx : XmlStaticMemberSerializer
    {
        public XmlStaticMemberSerializerEx(Type item)
            : base(item)
        {
        }

        protected override System.Reflection.Assembly Compile(string code)
        {
#if ENABLE_SCRIPT
            List<string> errors = new List<string>();
            return (new PluginLoader()).compileScript(code, errors);
#else
            return null;
#endif
        }

        public void deserialize(System.IO.Stream stream)
        {
            base.Deserialize(stream);
        }

        public void serialize(System.IO.Stream stream, Object dumy_null_argument)
        {
            base.Serialize(stream);
        }
    }

}
