/*
 * IImageCachedComponentDrawer.cs
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
using cadencii.java.awt;

namespace cadencii
{

    /// <summary>
    /// ImageCachedComponentDrawerを使って描画するコンポーネントが実装しておかなければならないインターフェース
    /// </summary>
    public interface IImageCachedComponentDrawer
    {
        /// <summary>
        /// 指定したサイズの範囲にコンポーネントを描画します
        /// </summary>
        /// <param name="g">描画に用いるグラフィックス</param>
        /// <param name="width">描画幅</param>
        /// <param name="height">描画高さ</param>
        void draw(Graphics2D g, int width, int height);
    }

}
