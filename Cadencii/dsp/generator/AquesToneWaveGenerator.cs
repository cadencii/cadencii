#if ENABLE_AQUESTONE
/*
 * AquesToneWaveGenerator.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Threading;
using com.github.cadencii.java.awt;
using com.github.cadencii.java.util;
using com.github.cadencii.media;
using com.github.cadencii.vsq;

namespace com.github.cadencii
{
    using boolean = System.Boolean;
    using Float = System.Single;
    using Integer = System.Int32;

#if JAVA
    public class AquesToneWaveGenerator implements WaveGenerator
#else
    public class AquesToneWaveGenerator : AquesToneWaveGeneratorBase
#endif
    {
    }

}
#endif
