/*
 * ImageCachedComponentDrawer.cs
 * Copyright (C) 2010 kbinani
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
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.awt.image;

namespace org.kbinani.cadencii {

    public class ImageCachedComponentDrawer {
        private const int WIDTH = 512;
        private int mWidth;
        private int mHeight;
        private BufferedImage[] mCache;

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

        public int getWidth() {
            return mWidth;
        }

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
                                if ( img.m_image != null ) {
                                    img.m_image.Dispose();
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

        public void draw( int x_offset, Graphics g ) {
            if ( mCache == null ) {
                return;
            }

            for ( int i = 0; i < mCache.Length; i++ ) {
                BufferedImage img = mCache[i];
                g.drawImage( img, WIDTH * i - x_offset, 0, null );
            }
        }

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

}
