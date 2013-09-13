/*
 * ParameterEvent.cs
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
namespace cadencii
{
    /// <summary>
    /// AquesTone VSTi へパラメータの変更要求
    /// </summary>
    public class ParameterEvent
    {
        /// <summary>
        /// パラメータの番号
        /// </summary>
        public int index;
        /// <summary>
        /// 値
        /// </summary>
        public float value;
    }
}
