/*
 * PlayMode.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/
namespace cadencii.vsq
{

    public class PlayMode
    {
        /// <summary>
        /// トラックはミュートされる．(-1)
        /// </summary>
        public const int Off = -1;
        /// <summary>
        /// トラックは合成された後再生される(0)
        /// </summary>
        public const int PlayAfterSynth = 0;
        /// <summary>
        /// トラックは合成しながら再生される(1)
        /// </summary>
        public const int PlayWithSynth = 1;

        private PlayMode()
        {
        }
    }

}
