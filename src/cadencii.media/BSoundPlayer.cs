/*
 * BSoundPlayer.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.media.
 *
 * cadencii.media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace cadencii.media
{

    public class BSoundPlayer : System.Media.SoundPlayer
    {
        public BSoundPlayer(string sound_location)
            : base(sound_location)
        {
        }

        public BSoundPlayer()
            : base()
        {
        }

        public void play()
        {
            base.Play();
        }

        public string getSoundLocation()
        {
            return base.SoundLocation;
        }

        public void setSoundLocation(string value)
        {
            base.SoundLocation = value;
        }
    }

}
