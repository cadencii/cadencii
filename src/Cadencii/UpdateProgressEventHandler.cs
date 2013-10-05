/*
 * UpdateProgressEventHandler.cs
 * Copyright © 2009-2011 kbinani
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

namespace cadencii
{

    /// <summary>
    /// 進捗状況の報告を行うためのイベントハンドラ．
    /// </summary>
    /// <param name="sender">イベントの送信元</param>
    /// <param name="value">進捗状況を表す値</param>
    public delegate void UpdateProgressEventHandler(Object sender, int value);

}
