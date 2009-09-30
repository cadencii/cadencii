/*
 * CalculatableString.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.ComponentModel;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    [TypeConverter( typeof( CalculatableStringConverter ) )]
    public class CalculatableString {
        private String m_value = "0";
        private int m_int = 0;

        public CalculatableString() : this( 0 ) {
        }

        public CalculatableString( int value ) {
            m_int = value;
            m_value = "" + value;
        }

        public override boolean Equals( object obj ) {
            if ( obj is CalculatableString ) {
                if ( m_int == ((CalculatableString)obj).m_int ) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return base.Equals( obj );
            }
        }

        public String str {
            get {
                return m_value;
            }
            set {
                int i = m_int;
                try {
                    i = (int)eval( 0.0, value );
                    String trim = value.Trim();
                    if ( trim.StartsWith( "-" ) ) {
                        m_int -= i;
                    } else if ( trim.StartsWith( "+" ) ) {
                        m_int += i;
                    } else {
                        m_int = i;
                    }
                    m_value = "" + m_int;
                } catch ( Exception ex ) {
#if DEBUG
                    Console.WriteLine( "CalculatableString#set_str; ex=" + ex );
#endif
                }
            }
        }

        public int getIntValue() {
            return m_int;
        }

        public static double eval( double x, String equation ) {
            String equ = "(" + equation + ")"; // ( )でくくる
            equ = equ.Replace( "Math.PI", Math.PI + "" ); // πを数値に置換
            equ = equ.Replace( "Math.E", Math.E + "" ); // eを数値に置換
            equ = equ.Replace( "exp", "ezp" ); // exp を ezp に置換しておく
            equ = equ.Replace( "x", x + "" ); // xを数字に置換
            equ = equ.Replace( "ezp", "exp" ); // ezp を exp に戻す

            int m0 = 0; // -- の処理-------（注釈：x を数値に置換したので、x が負値のとき  --3.1 のようになっている）
            while ( true ) {
                int m1 = equ.IndexOf( "--", m0 );
                if ( m1 < 0 ) { 
                    break;
                }
                if ( m1 == 0 || equ[m1 - 1] == 40 || equ[m1 - 1] == 42 || equ[m1 - 1] == 47 || equ[m1 - 1] == 43 || equ[m1 - 1] == 44 ) {
                    equ = equ.Substring( 0, m1 - 0 ) + equ.Substring( m1 + 2 ); // -- を 取る
                } else {
                    equ = equ.Substring( 0, m1 - 0 ) + "+" + equ.Substring( m1 + 2 ); // -- を + に置換
                }
                m0 = m1;
                if ( m0 > equ.Length - 1 ) { 
                    break;
                }
            }

            m0 = 0; // - の処理-------
            while ( true ) {
                int m1 = equ.IndexOf( "-", m0 ); 
                if ( m1 < 0 ) { 
                    break;
                }
                if ( m1 == 0 || equ[m1 - 1] == 40 || equ[m1 - 1] == 42 || equ[m1 - 1] == 47 || equ[m1 - 1] == 43 || equ[m1 - 1] == 44 ) {
                    m0 = m1 + 1;
                } else {
                    equ = equ.Substring( 0, m1 - 0 ) + "+(-1)*" + equ.Substring( m1 + 1 ); // -a、-Math.sin(A) などを +(-1)*a、 +(-1)*Math.sin(A) などに置き換える
                    m0 = m1 + 6;
                }
                if ( m0 > equ.Length - 1 ) { break; }
            }
            double valResult = double.Parse( evalMy0( equ ) );
            return valResult;
        }

        //----------------------------------------------------------------------------------
        private static String evalMy0( String equation ) {
            String equ = equation;
            while ( true ) {
                // 最内側の（ ） から計算する（注釈：最内側（ ）内には、Math.…() のようなものはない）
                int n1 = equ.IndexOf( ")" );
                if ( n1 < 0 ) {
                    break;
                } // ) の検索
                int n2 = equ.LastIndexOf( "(", n1 - 1 ); // ( に対応する ) の検索
                if ( n2 < 0 ) {
                    break;
                }
                String str = equ.Substring( n2 + 1, n1 - n2 - 1 ); // ( )内の文字
                int ne0 = str.IndexOf( "," ); // ( )内の , の検索
                double val = 0;

                if ( ne0 >= 0 ) {
                    // ( )内に , があるので、 Math.log(A,B) or Math.Pow(A,B) の処理
                    if ( equ.Substring( n2 - 3, n2 - n2 + 3 ).Equals( "log" ) ) {
                        // Math.log(A,B) のとき
                        String strA = str.Substring( 0, ne0 - 0 ); // Math.log(A,B)の A の文字
                        double valA = double.Parse( evalMy0( "(" + strA + ")" ) ); // （注：再帰である）
                        String strB = str.Substring( ne0 + 1 ); // Math.log(A,B)の B の文字
                        double valB = double.Parse( evalMy0( "(" + strB + ")" ) ); //（注：再帰である）
                        val = Math.Log( valB ) / Math.Log( valA );
                        equ = equ.Replace( "Math.log(" + strA + "," + strB + ")", "" + val );
                    } else if ( equ.Substring( n2 - 3, n2 - n2 + 3 ).Equals( "pow" ) ) { // Math.Pow(A,B) のとき
                        String strA = str.Substring( 0, ne0 - 0 ); // Math.Pow(A,B)の A の文字
                        double valA = double.Parse( evalMy0( "(" + strA + ")" ) ); // （注：再帰である）
                        String strB = str.Substring( ne0 + 1 ); // Math.Pow(A,B)の B の文字
                        double valB = double.Parse( evalMy0( "(" + strB + ")" ) ); //（注：再帰である）
                        val = Math.Pow( valA, valB );
                        equ = equ.Replace( "Math.Pow(" + strA + "," + strB + ")", "" + val );
                    }
                } else {
                    int check0 = 0; // strが数値（数字）かどうかチェック（str="-3.7" なら 数値なので 0 とする）
                    for ( int i = 0; i < str.Length; i++ ) {
                        if ( i == 0 ) {
                            if ( (str[0] < 48 || str[0] > 57) && str[0] != 46 && str[0] != 43 && str[0] != 45 ) { 
                                check0 = 1;
                                break;
                            }
                        } else {
                            if ( (str[i] < 48 || str[i] > 57) && str[i] != 46 ) { 
                                check0 = 1;
                                break; 
                            }
                        }
                    }

                    if ( check0 == 1 ) {
                        val = evalMy1( str ); // ( ) の処理をし数値をもとめる
                    } else {
                        val = double.Parse( str ); // 文字を数値に変換
                    }
                    if ( n2 - 8 >= 0 ) {
                        String str1 = equ.Substring( n2 - 8, n2 - (n2 - 8) );
                        if ( str1.Equals( "Math.Sin" ) ) {
                            val = Math.Sin( val );
                            equ = equ.Replace( "Math.Sin(" + str + ")", "" + val );
                            n2 -= 8;
                        } else if ( str1.Equals( "Math.Cos" ) ) {
                            val = Math.Cos( val );
                            equ = equ.Replace( "Math.Cos(" + str + ")", "" + val );
                            n2 -= 8;
                        } else if ( str1.Equals( "Math.Tan" ) ) {
                            val = Math.Tan( val );
                            equ = equ.Replace( "Math.Tan(" + str + ")", "" + val );
                            n2 -= 8;
                        } else if ( str1.Equals( "ath.Asin" ) ) {
                            val = Math.Asin( val );
                            equ = equ.Replace( "Math.Asin(" + str + ")", "" + val );
                            n2 -= 9;
                        } else if ( str1.Equals( "ath.Acos" ) ) {
                            val = Math.Acos( val );
                            equ = equ.Replace( "Math.Acos(" + str + ")", "" + val );
                            n2 -= 9;
                        } else if ( str1.Equals( "ath.Atan" ) ) {
                            val = Math.Atan( val );
                            equ = equ.Replace( "Math.Atan(" + str + ")", "" + val );
                            n2 -= 9;
                        } else if ( str1.Equals( "Math.Log" ) ) {
                            val = Math.Log( val );
                            equ = equ.Replace( "Math.Log(" + str + ")", "" + val );
                            n2 -= 8;
                        } else if ( str1.Equals( "Math.Exp" ) ) {
                            val = Math.Exp( val );
                            equ = equ.Replace( "Math.Exp(" + str + ")", "" + val );
                            n2 -= 8;
                        } else if ( str1.Equals( "Math.Abs" ) ) {
                            val = Math.Abs( val );
                            equ = equ.Replace( "Math.Abs(" + str + ")", "" + val );
                            n2 -= 8;
                        } else if ( str1.Equals( "ath.Sqrt" ) ) {
                            val = Math.Sqrt( val );
                            equ = equ.Replace( "Math.Sqrt(" + str + ")", "" + val );
                            n2 -= 9;
                        } else {
                            equ = equ.Replace( "(" + str + ")", "" + val );
                        } // ( ) を取る
                    } else {
                        equ = equ.Replace( "(" + str + ")", "" + val ); // ( ) を取る
                    }
                }
            }
            return equ;
        }

        //　* と / のみからなる数式の いくつかの和、差からなる式の処理----------------
        private static double evalMy1( String equation ) {
            double val = 0;
            while ( true ) {
                String equ0 = "";
                int n0 = equation.IndexOf( "+" );
                if ( n0 < 0 ) { 
                    equ0 = equation;
                } else { 
                    equ0 = equation.Substring( 0, n0 - 0 );
                } // 最初の + より前の項
                val += evalMy2( equ0 );
                if ( n0 < 0 ) { 
                    break;
                } else { 
                    equation = equation.Substring( n0 + 1 );
                } // 最初の + より以降の項
            }
            return val;
        }

        //　* と / のみからなる数式についての処理-----------------------------------
        private static double evalMy2( String equation ) {
            double val0 = 1;
            while ( true ) {
                String equ0 = "";
                int n0 = equation.IndexOf( "*" );
                if ( n0 < 0 ) { 
                    equ0 = equation;
                } else { 
                    equ0 = equation.Substring( 0, n0 ); 
                } // 最初の * より前の項

                int kai = 0;
                double val1 = 1;
                while ( true ) { // / を含んだ項の計算
                    String equ1 = "";
                    int n1 = equ0.IndexOf( "/" );
                    if ( n1 < 0 ) {
                        equ1 = equ0;
                    } else { 
                        equ1 = equ0.Substring( 0, n1 - 0 ); 
                    } // 最初の / より前の項
                    if ( kai == 0 ) { 
                        val1 = double.Parse( equ1 );
                    } else { 
                        val1 /= double.Parse( equ1 );
                    }
                    if ( n1 < 0 ) { 
                        break; 
                    } else {
                        kai++; 
                        equ0 = equ0.Substring( n1 + 1 ); 
                    } // 最初の / より以降の項
                }
                val0 *= val1;
                if ( n0 < 0 ) {
                    break; 
                } else {
                    equation = equation.Substring( n0 + 1 );
                } // 最初の * より以降の項
            }
            return val0;
        }

    }
}