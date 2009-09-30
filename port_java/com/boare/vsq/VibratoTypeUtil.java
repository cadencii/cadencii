/*
 * VibratoTypeUtil.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

/// <summary>
/// VibratoTypeのためのユーティリティを集めたスタティック・クラス
/// </summary>
public class VibratoTypeUtil {
    /// <summary>
    /// IconID文字列から，VibratoTypeを調べます
    /// </summary>
    /// <param name="icon_id"></param>
    /// <returns></returns>
    public static VibratoType getVibratoTypeFromIconID( String icon_id ) {
        if( icon_id.equals( "$04040001" ) ){
            return VibratoType.NormalType1;
        }else if( icon_id.equals( "$04040002" ) ){
            return VibratoType.NormalType2;
        }else if( icon_id.equals( "$04040003" ) ){
            return VibratoType.NormalType3;
        }else if( icon_id.equals( "$0400004" ) ){
            return VibratoType.NormalType4;
        }else if( icon_id.equals( "$04040005" ) ){
            return VibratoType.ExtremeType1;
        }else if( icon_id.equals( "$04040006" ) ){
            return VibratoType.ExtremeType2;
        }else if( icon_id.equals( "$04040007" ) ){
            return VibratoType.ExtremeType3;
        }else if( icon_id.equals( "$04040008" ) ){
            return VibratoType.ExtremeType4;
        }else if( icon_id.equals( "$04040009" ) ){
            return VibratoType.FastType1;
        }else if( icon_id.equals( "$0404000a" ) ){
            return VibratoType.FastType2;
        }else if( icon_id.equals( "$0404000b" ) ){
            return VibratoType.FastType3;
        }else if( icon_id.equals( "$0404000c" ) ){
            return VibratoType.FastType4;
        }else if( icon_id.equals( "$0404000d" ) ){
            return VibratoType.SlightType1;
        }else if( icon_id.equals( "$0404000e" ) ){
            return VibratoType.SlightType2;
        }else if( icon_id.equals( "$0404000f" ) ){
            return VibratoType.SlightType3;
        }else if( icon_id.equals( "$04040010" ) ){
            return VibratoType.SlightType4;
        }
        return VibratoType.NormalType1;
    }

    /// <summary>
    /// 指定されたVibratoTypeを表すIconIDを取得します
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static String getIconIDFromVibratoType( VibratoType type ) {
        switch ( type ) {
            case NormalType1:
                return "$04040001";
            case NormalType2:
                return "$04040002";
            case NormalType3:
                return "$04040003";
            case NormalType4:
                return "$0400004";
            case ExtremeType1:
                return "$04040005";
            case ExtremeType2:
                return "$04040006";
            case ExtremeType3:
                return "$04040007";
            case ExtremeType4:
                return "$04040008";
            case FastType1:
                return "$04040009";
            case FastType2:
                return "$0404000a";
            case FastType3:
                return "$0404000b";
            case FastType4:
                return "$0404000c";
            case SlightType1:
                return "$0404000d";
            case SlightType2:
                return "$0404000e";
            case SlightType3:
                return "$0404000f";
            case SlightType4:
                return "$04040010";
        }
        return "";
    }

    /// <summary>
    /// ビブラートのプリセットタイプから，VibratoHandleを作成します
    /// </summary>
    /// <param name="type"></param>
    /// <param name="vibrato_clocks"></param>
    /// <returns></returns>
    public static VibratoHandle getDefaultVibratoHandle( VibratoType type, int vibrato_clocks ) {
        VibratoHandle res = new VibratoHandle();
        res.length = vibrato_clocks;
        res.original = 1;
        //res.DepthBPNum = 0;
        //res.RateBPNum = 0;
        res.caption = toString( type );
        res.iconID = getIconIDFromVibratoType( type );
        switch ( type ) {
            case NormalType1:
                res.IDS = "normal";
                res.startDepth = 64;
                res.startRate = 50;
                break;
            case NormalType2:
                res.IDS = "normal";
                res.startDepth = 40;
                res.startRate = 40;
                break;
            case NormalType3:
                res.IDS = "normal";
                res.startDepth = 127;
                res.startRate = 50;
                break;
            case NormalType4:
                res.IDS = "normal";
                res.startDepth = 64;
                //res.DepthBPNum = 57;
                res.depthBP = new VibratoBPList( new float[] { 0.603900f, 0.612500f, 0.616400f, 0.621100f, 0.625000f, 0.633600f, 0.637500f, 0.641400f, 0.646100f, 0.653900f, 0.658600f, 0.666400f, 0.670300f, 0.675000f, 0.678900f, 0.683600f, 0.691400f, 0.696100f, 0.703900f, 0.708600f, 0.712500f, 0.716400f, 0.721100f, 0.725000f, 0.728900f, 0.737500f, 0.746100f, 0.750000f, 0.758600f, 0.762500f, 0.766400f, 0.771100f, 0.775000f, 0.783600f, 0.791400f, 0.795300f, 0.800000f, 0.803900f, 0.808600f, 0.812500f, 0.821100f, 0.828900f, 0.837500f, 0.841400f, 0.846100f, 0.850000f, 0.853900f, 0.862500f, 0.866400f, 0.875000f, 0.878900f, 0.883600f, 0.887500f, 0.891400f, 0.896100f, 0.900000f, 1.000000f },
                                                 new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
                res.startRate = 50;
                //res.RateBPNum = 52;
                res.rateBP = new VibratoBPList( new float[] { 0.600000f, 0.612500f, 0.616400f, 0.621100f, 0.628900f, 0.633600f, 0.637500f, 0.641400f, 0.653900f, 0.658600f, 0.662500f, 0.666400f, 0.675000f, 0.683600f, 0.687500f, 0.691400f, 0.700000f, 0.703900f, 0.708600f, 0.712500f, 0.725000f, 0.728900f, 0.732800f, 0.737500f, 0.746100f, 0.750000f, 0.758600f, 0.762500f, 0.771100f, 0.775000f, 0.778900f, 0.783600f, 0.795300f, 0.800000f, 0.803900f, 0.808600f, 0.816400f, 0.821100f, 0.828900f, 0.833600f, 0.841400f, 0.846100f, 0.850000f, 0.853900f, 0.866400f, 0.871100f, 0.875000f, 0.878900f, 0.887500f, 0.891400f, 0.900000f, 1.000000f },
                                                new int[] { 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
                break;
            case ExtremeType1:
                res.IDS = "extreme";
                res.startDepth = 64;
                res.startRate = 64;
                break;
            case ExtremeType2:
                res.IDS = "extreme";
                res.startDepth = 32;
                res.startRate = 32;
                break;
            case ExtremeType3:
                res.IDS = "extreme";
                res.startDepth = 100;
                res.startRate = 50;
                break;
            case ExtremeType4:
                res.IDS = "extreme";
                res.startDepth = 64;
                //res.DepthBPNum = 57;
                res.depthBP = new VibratoBPList( new float[] { 0.603900f, 0.612500f, 0.616400f, 0.621100f, 0.625000f, 0.633600f, 0.637500f, 0.641400f, 0.646100f, 0.653900f, 0.658600f, 0.666400f, 0.670300f, 0.675000f, 0.678900f, 0.683600f, 0.691400f, 0.696100f, 0.703900f, 0.708600f, 0.712500f, 0.716400f, 0.721100f, 0.725000f, 0.728900f, 0.737500f, 0.746100f, 0.750000f, 0.758600f, 0.762500f, 0.766400f, 0.771100f, 0.775000f, 0.783600f, 0.791400f, 0.795300f, 0.800000f, 0.803900f, 0.808600f, 0.812500f, 0.821100f, 0.828900f, 0.837500f, 0.841400f, 0.846100f, 0.850000f, 0.853900f, 0.862500f, 0.866400f, 0.875000f, 0.878900f, 0.883600f, 0.887500f, 0.891400f, 0.896100f, 0.900000f, 1.000000f },
                                                 new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
                res.startRate = 64;
                //res.RateBPNum = 57;
                res.rateBP = new VibratoBPList( new float[] { 0.603900f, 0.612500f, 0.616400f, 0.621100f, 0.625000f, 0.633600f, 0.637500f, 0.641400f, 0.646100f, 0.653900f, 0.658600f, 0.666400f, 0.670300f, 0.675000f, 0.678900f, 0.683600f, 0.691400f, 0.696100f, 0.703900f, 0.708600f, 0.712500f, 0.716400f, 0.721100f, 0.725000f, 0.728900f, 0.737500f, 0.746100f, 0.750000f, 0.758600f, 0.762500f, 0.766400f, 0.771100f, 0.775000f, 0.783600f, 0.791400f, 0.795300f, 0.800000f, 0.803900f, 0.808600f, 0.812500f, 0.821100f, 0.828900f, 0.837500f, 0.841400f, 0.846100f, 0.850000f, 0.853900f, 0.862500f, 0.866400f, 0.875000f, 0.878900f, 0.883600f, 0.887500f, 0.891400f, 0.896100f, 0.900000f, 1.000000f },
                                                new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
                break;
            case FastType1:
                res.IDS = "fast";
                res.startDepth = 64;
                res.startRate = 64;
                break;
            case FastType2:
                res.IDS = "fast";
                res.startDepth = 40;
                res.startRate = 50;
                break;
            case FastType3:
                res.IDS = "fast";
                res.startDepth = 80;
                res.startRate = 70;
                break;
            case FastType4:
                res.IDS = "fast";
                res.startDepth = 64;
                //res.DepthBPNum = 57;
                res.depthBP = new VibratoBPList( new float[] { 0.603900f, 0.612500f, 0.616400f, 0.621100f, 0.625000f, 0.633600f, 0.637500f, 0.641400f, 0.646100f, 0.653900f, 0.658600f, 0.666400f, 0.670300f, 0.675000f, 0.678900f, 0.683600f, 0.691400f, 0.696100f, 0.703900f, 0.708600f, 0.712500f, 0.716400f, 0.721100f, 0.725000f, 0.728900f, 0.737500f, 0.746100f, 0.750000f, 0.758600f, 0.762500f, 0.766400f, 0.771100f, 0.775000f, 0.783600f, 0.791400f, 0.795300f, 0.800000f, 0.803900f, 0.808600f, 0.812500f, 0.821100f, 0.828900f, 0.837500f, 0.841400f, 0.846100f, 0.850000f, 0.853900f, 0.862500f, 0.866400f, 0.875000f, 0.878900f, 0.883600f, 0.887500f, 0.891400f, 0.896100f, 0.900000f, 1.000000f },
                                                 new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
                res.startRate = 64;
                //res.RateBPNum = 57;
                res.rateBP = new VibratoBPList( new float[] { 0.603900f, 0.612500f, 0.616400f, 0.621100f, 0.625000f, 0.633600f, 0.637500f, 0.641400f, 0.646100f, 0.653900f, 0.658600f, 0.666400f, 0.670300f, 0.675000f, 0.678900f, 0.683600f, 0.691400f, 0.696100f, 0.703900f, 0.708600f, 0.712500f, 0.716400f, 0.721100f, 0.725000f, 0.728900f, 0.737500f, 0.746100f, 0.750000f, 0.758600f, 0.762500f, 0.766400f, 0.771100f, 0.775000f, 0.783600f, 0.791400f, 0.795300f, 0.800000f, 0.803900f, 0.808600f, 0.812500f, 0.821100f, 0.828900f, 0.837500f, 0.841400f, 0.846100f, 0.850000f, 0.853900f, 0.862500f, 0.866400f, 0.875000f, 0.878900f, 0.883600f, 0.887500f, 0.891400f, 0.896100f, 0.900000f, 1.000000f },
                                                new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
                break;
            case SlightType1:
                res.IDS = "slight";
                res.startDepth = 64;
                res.startRate = 64;
                break;
            case SlightType2:
                res.IDS = "slight";
                res.startDepth = 40;
                res.startRate = 64;
                break;
            case SlightType3:
                res.IDS = "slight";
                res.startDepth = 72;
                res.startRate = 64;
                break;
            case SlightType4:
                res.IDS = "slight";
                res.startDepth = 64;
                //res.DepthBPNum = 57;
                res.depthBP = new VibratoBPList( new float[] { 0.604300f, 0.612500f, 0.616800f, 0.620700f, 0.625000f, 0.633200f, 0.637500f, 0.641800f, 0.645700f, 0.654300f, 0.658200f, 0.666800f, 0.670700f, 0.675000f, 0.679300f, 0.683200f, 0.691800f, 0.695700f, 0.704300f, 0.708200f, 0.712500f, 0.716800f, 0.720700f, 0.725000f, 0.729300f, 0.737500f, 0.745700f, 0.750000f, 0.758200f, 0.762500f, 0.766800f, 0.770700f, 0.775000f, 0.783200f, 0.791800f, 0.795700f, 0.800000f, 0.804300f, 0.808200f, 0.812500f, 0.820700f, 0.829300f, 0.837500f, 0.841800f, 0.845700f, 0.850000f, 0.854300f, 0.862500f, 0.866800f, 0.875000f, 0.879300f, 0.883200f, 0.887500f, 0.891800f, 0.895700f, 0.900000f, 1.000000f },
                                                 new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
                res.startRate = 64;
                //res.RateBPNum = 57;
                res.rateBP = new VibratoBPList( new float[] { 0.604300f, 0.612500f, 0.616800f, 0.620700f, 0.625000f, 0.633200f, 0.637500f, 0.641800f, 0.645700f, 0.654300f, 0.658200f, 0.666800f, 0.670700f, 0.675000f, 0.679300f, 0.683200f, 0.691800f, 0.695700f, 0.704300f, 0.708200f, 0.712500f, 0.716800f, 0.720700f, 0.725000f, 0.729300f, 0.737500f, 0.745700f, 0.750000f, 0.758200f, 0.762500f, 0.766800f, 0.770700f, 0.775000f, 0.783200f, 0.791800f, 0.795700f, 0.800000f, 0.804300f, 0.808200f, 0.812500f, 0.820700f, 0.829300f, 0.837500f, 0.841800f, 0.845700f, 0.850000f, 0.854300f, 0.862500f, 0.866800f, 0.875000f, 0.879300f, 0.883200f, 0.887500f, 0.891800f, 0.895700f, 0.900000f, 1.000000f },
                                                new int[] { 64, 63, 62, 61, 59, 58, 57, 56, 55, 54, 52, 51, 50, 49, 48, 47, 45, 44, 43, 42, 41, 40, 39, 38, 37, 35, 34, 32, 31, 30, 29, 28, 27, 25, 24, 23, 22, 21, 20, 19, 17, 15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0 } );
                break;
        }
        return res;
    }

    /// <summary>
    /// 指定されたVibratoTypeを文字列に変換します
    /// </summary>
    /// <param name="value"></param>
    /// <example>
    /// <code>
    /// string str = VibratoTypeUtil.ToString( VibratoType.NormalType1 );
    /// // str = "[Normal] Type 1"
    /// </code>
    /// </example>
    /// <returns></returns>
    public static String toString( VibratoType value ) {
        switch ( value ) {
            case NormalType1:
                return "[Normal] Type 1";
            case NormalType2:
                return "[Normal] Type 2";
            case NormalType3:
                return "[Normal] Type 3";
            case NormalType4:
                return "[Normal] Type 4";
            case ExtremeType1:
                return "[Extreme] Type 1";
            case ExtremeType2:
                return "[Extreme] Type 2";
            case ExtremeType3:
                return "[Extreme] Type 3";
            case ExtremeType4:
                return "[Extreme] Type 4";
            case FastType1:
                return "[Fast] Type 1";
            case FastType2:
                return "[Fast] Type 2";
            case FastType3:
                return "[Fast] Type 3";
            case FastType4:
                return "[Fast] Type 4";
            case SlightType1:
                return "[Slight] Type 1";
            case SlightType2:
                return "[Slight] Type 2";
            case SlightType3:
                return "[Slight] Type 3";
            case SlightType4:
                return "[Slight] Type 4";
        }
        return "";
    }
}
