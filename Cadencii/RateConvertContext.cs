/*
 * RateConvertContext.cs
 * Copyright © 2010 kbinani
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
using System.Collections.Generic;
using System.Text;

namespace org.kbinani.cadencii
{
    public class RateConvertContext
    {
        /// <summary>
        /// 受け取ったデータの個数
        /// </summary>
        private long bCount = 0;
        /// <summary>
        /// receiverに送ったデータの個数
        /// </summary>
        private long aCount = 0;
        /// <summary>
        /// 変換前のサンプリングレート
        /// </summary>
        private int bRate = 44100;
        /// <summary>
        /// 変換後のサンプリングレート
        /// </summary>
        private int aRate = 44100;
        /// <summary>
        /// bRateの逆数
        /// </summary>
        private double invBRate = 1.0 / 44100.0;
        /// <summary>
        /// aRateの逆数
        /// </summary>
        private double invARate = 1.0 / 44100.0;
        private double[] bBufLeft;
        private double[] bBufRight;
        private long bBufBase;
        /// <summary>
        /// データのチャンネル数
        /// </summary>
        private int mChannels = 2;

        public RateConvertContext( int sample_rate_from, int sample_rate_to )
        {
            if ( sample_rate_from <= 0 ) {
                throw new ArgumentOutOfRangeException( "sample_rate_from" );
            }
            if ( sample_rate_to <= 0 ) {
                throw new ArgumentOutOfRangeException( "sample_rate_to" );
            }
            bRate = sample_rate_from;
            aRate = sample_rate_to;
            invARate = 1.0 / aRate;
            invBRate = 1.0 / bRate;
        }

        /*public static void convert( RateConvertContext context, double[] )
        {
        }*/
    }
}
