/*
 * MessageBody.cs
 * Copyright (c) 2009 kbinani
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
using System;
using System.Collections.Generic;

namespace Boare.Lib.AppUtil {

    public class MessageBodyEntry {
        public string Message;
        public List<string> Location = new List<string>();

        public MessageBodyEntry( string message, string[] location ) {
            Message = message;
            for ( int i = 0; i < location.Length; i++ ) {
                Location.Add( location[i] );
            }
        }
    }

}
