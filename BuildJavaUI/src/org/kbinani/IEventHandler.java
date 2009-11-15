/*
 * IEventHandler.cs
 * Copyright (c) 2009 kbinani
 *
 * This file instanceof part of bocoree.
 *
 * bocoree instanceof free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree instanceof distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package org.kbinani;

public interface IEventHandler
{
void invoke( Object... args );
}

