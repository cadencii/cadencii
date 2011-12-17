/*
 * ImageCachedComponentDrawer.cs
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
#if JAVA
package com.github.cadencii;

import java.awt.*;
import java.awt.image.*;
import com.github.cadencii.*;
#else
using System;
using com.github.cadencii;
using com.github.cadencii.java.awt;
using com.github.cadencii.java.awt.image;

namespace com.github.cadencii {
#endif

    /// <summary>
    /// 高さが一定で，横方向に長く，横方向にスクロールして使用するタイプで，描画ループが重いコンポーネントを，比較的高速に描画します．
    /// </summary>
    public class ImageCachedComponentDrawer {
        /// <summary>
        /// 1個の画像キャッシュの幅(単位はピクセル)
        /// </summary>
        private const int WIDTH = 512;
        /// <summary>
        /// コンポーネントの総幅(単位はピクセル)
        /// </summary>
        private int mWidth;
        /// <summary>
        /// コンポーネントの高さ(単位はピクセル)
        /// </summary>
        private int mHeight;
        /// <summary>
        /// コンポーネント画像のキャッシュ
        /// </summary>
        private BufferedImage[] mCache;

        /// <summary>
        /// コンストラクタ
        /// 初期のコンポーネントサイズを指定します。ここで指定した高さは変更できません。幅は後から変更可能です。
        /// </summary>
        /// <param name="width">コンポーネントの幅(単位はピクセル)</param>
        /// <param name="height">コンポーネントの高さ(単位はピクセル)</param>
        public ImageCachedComponentDrawer( int width, int height ) {
            mWidth = width;
            mHeight = height;

            int num = mWidth / WIDTH + 1;
            if ( mWidth % WIDTH == 0 ) {
                num--;
            }

            mCache = new BufferedImage[num];
            for ( int i = 0; i < num; i++ ) {
                mCache[i] = new BufferedImage( WIDTH, mHeight, BufferedImage.TYPE_INT_RGB );
            }
        }

        /// <summary>
        /// コンポーネント画像の総横幅を取得します
        /// </summary>
        /// <returns>コンポーネント画像の総幅(単位はピクセル)</returns>
        public int getWidth() {
            return mWidth;
        }

        /// <summary>
        /// コンポーネント画像の総横幅を設定します
        /// </summary>
        /// <param name="width">新しく設定する横幅(単位はピクセル)</param>
        public void setWidth( int width ) {
            // 必要なバッファの個数を計算
            int num = width / WIDTH + 1;
            if ( width % WIDTH == 0 ) {
                num--;
            }

            if ( mCache == null || (mCache != null && num != mCache.Length) ) {
                // 現在のバッファの個数と違うか、バッファがない場合バッファの長さを変更
                if ( mCache == null ) {
                    // バッファがない場合
                    mCache = new BufferedImage[num];
                } else {
                    // バッファがある場合
                    if ( mCache.Length > num ) {
                        // 短くする場合、削除する分の画像を削除
                        for ( int i = num; i < mCache.Length; i++ ) {
                            BufferedImage img = mCache[i];
                            if ( img != null ) {
#if JAVA
#else
                                if ( img.image != null ) {
                                    img.image.Dispose();
                                }
#endif
                            }
                        }
                    }
#if JAVA
                    BufferedImage[] old = mCache;
                    mCache = new BufferedImage[num];
                    for( int i = 0; i < old.Length && i < mCache.Length; i++ ){
                        mCache[i] = old[i];
                    }
#else
                    Array.Resize( ref mCache, num );
#endif
                }

                // 画像がnullの場合新しく作成
                for ( int i = 0; i < num; i++ ) {
                    if ( mCache[i] == null ) {
                        mCache[i] = new BufferedImage( WIDTH, mHeight, BufferedImage.TYPE_INT_RGB );
                    }
                }
            }

            mWidth = width;
        }

        /// <summary>
        /// キャッシュされたコンポーネント画像を、指定したグラフィックスを使って描画します。
        /// </summary>
        /// <param name="x_offset">画像の左方向のシフト量(単位はピクセル)</param>
        /// <param name="g">描画対象のグラフィックス</param>
        public void draw( int x_offset, Graphics g ) {
            if ( mCache == null ) {
                return;
            }

            for ( int i = 0; i < mCache.Length; i++ ) {
                BufferedImage img = mCache[i];
                g.drawImage( img, WIDTH * i - x_offset, 0, null );
            }
        }

        /// <summary>
        /// コンポーネント画像のキャッシュを再描画します
        /// </summary>
        /// <param name="component">再描画に使用するコンポーネント</param>
        public void updateCache( IImageCachedComponentDrawer component ) {
            if( mCache == null ){
                return;
            }

            for ( int i = 0; i < mCache.Length; i++ ) {
                Graphics2D g = mCache[i].createGraphics();
                g.translate( -i * WIDTH, 0 );
                component.draw( g, mWidth, mHeight );
            }
        }
    }

#if !JAVA
}
#endif
