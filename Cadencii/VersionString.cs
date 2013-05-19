/*
 * VersionString.cs
 * Copyright © 2011 kbinani
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
package cadencii;

import cadencii.*;
#else
using System;

    namespace cadencii
    {
#endif

            /// <summary>
            /// メジャー，マイナー，およびメンテナンス番号によるバージョン番号を表すクラス
            /// </summary>
#if JAVA
            class VersionString implements Comparable<VersionString>
#else
            class VersionString : IComparable<VersionString>
#endif
            {
                /// <summary>
                /// メジャーバージョンを表す
                /// </summary>
                public int major;
                /// <summary>
                /// マイナーバージョンを表す
                /// </summary>
                public int minor;
                /// <summary>
                /// メンテナンス番号を表す
                /// </summary>
                public int build;
                /// <summary>
                /// コンストラクタに渡された文字列のキャッシュ
                /// </summary>
                private String mRawString = "0.0.0";

                /// <summary>
                /// 「メジャー.マイナー.メンテナンス」の記法に基づく文字列をパースし，新しいインスタンスを作成します
                /// </summary>
                /// <param name="str"></param>
                public VersionString( String s )
                {
                    mRawString = s;
                    String[] spl = PortUtil.splitString( s, '.' );
                    if ( spl.Length >= 1 ) {
                        try {
                            major = str.toi( spl[0] );
                        } catch ( Exception ex ) {
                        }
                    }
                    if ( spl.Length >= 2 ) {
                        try {
                            minor = str.toi( spl[1] );
                        } catch ( Exception ex ) {
                        }
                    }
                    if ( spl.Length >= 3 ) {
                        try {
                            build = str.toi( spl[2] );
                        } catch ( Exception ex ) {
                        }
                    }
                }

                /// <summary>
                /// このインスタンス生成時に渡された文字列を取得します
                /// </summary>
                /// <returns></returns>
                public String getRawString()
                {
                    return mRawString;
                }

                /// <summary>
                /// このインスタンスを文字列で表現したものを取得します
                /// </summary>
                /// <returns></returns>
                public String toString()
                {
                    return major + "." + minor + "." + build;
                }

                /// <summary>
                /// このインスタンスと，指定したバージョンを比較します
                /// </summary>
                /// <param name="item"></param>
                /// <returns>このインスタンスの表すバージョンに対して，指定したバージョンが同じであれば0，新しければ正の値，それ以外は負の値を返します</returns>
                public int compareTo( VersionString item )
                {
                    if ( item == null ) {
                        return -1;
                    }
                    if ( this.major == item.major ) {
                        if ( this.minor == item.minor ) {
                            return this.build - item.build;
                        } else {
                            return this.minor - item.minor;
                        }
                    } else {
                        return this.major - item.major;
                    }
                }

#if !JAVA
                public override String ToString()
                {
                    return this.toString();
                }

                public int CompareTo( VersionString item )
                {
                    return this.compareTo( item );
                }
#endif
            }

#if !JAVA
    }
#endif
