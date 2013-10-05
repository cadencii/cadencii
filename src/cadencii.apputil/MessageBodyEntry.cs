/*
 * MessageBodyEntry.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using cadencii.java.util;

namespace cadencii.apputil
{

    public class MessageBodyEntry
    {
        public string message;
        public List<string> location = new List<string>();

        public MessageBodyEntry(string message_, string[] location_)
        {
            message = message_;
            for (int i = 0; i < location_.Length; i++) {
                location.Add(location_[i]);
            }
        }
    }

}
