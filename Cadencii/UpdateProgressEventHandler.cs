#if !JAVA
/*
 * UpdateProgressEventHandler.cs
 * Copyright (C) 2009-2010 kbinani
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
namespace org.kbinani.cadencii {

    /// <summary>
    /// 進捗状況の報告を行うためのイベントハンドラ．
    /// </summary>
    /// <param name="sender">イベントの送信元</param>
    /// <param name="value">進捗状況を表す値</param>
    public delegate void UpdateProgressEventHandler( object sender, int value );

}
#endif
