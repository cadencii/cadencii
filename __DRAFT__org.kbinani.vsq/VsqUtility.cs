
#if JAVA
package org.kbinani.cadencii;
#else
#if !__cplusplus
using System;
using System.IO;
using System.Collections.Generic;

#endif
namespace org
{
    namespace kbinani
    {
        namespace vsq
        {
#if !__cplusplus
            using int8_t = System.SByte;
            using int16_t = System.Int16;
            using int32_t = System.Int32;
            using int64_t = System.Int64;
            using uint8_t = System.Byte;
            using uint16_t = System.UInt16;
            using uint32_t = System.UInt32;
            using uint64_t = System.UInt64;
#endif
#endif

            class vec
            {
                private vec()
                {
                }

#if __cplusplus
            public:
#endif

#if JAVA
                public static void ensureCapacity<T>( Vector<T> arr, int size )
#elif __cplusplus
                template<typename T>
                static void ensureCapacity( vector<T> &arr, int size )
#else
                public static void ensureCapacity<T>( List<T> arr, int size )
#endif
                {
#if JAVA
                    arr.ensureCapacity( size );
#else
                    int delta = size - vec.size( arr );
                    if( delta > 0 ){
                        // 増やす
                        for ( int i = 0; i < delta; i++ ) {
#if __cplusplus
                            add( arr, (T)NULL );
#else
                            add( arr, default( T ) );
#endif
#endif
                        }
                    }
                }

#if JAVA
                public static void sort<T>( Vector<T> arr )
#elif __cplusplus
                template<class T>
                static void sort( vector<T> &arr )
#else
                public static void sort<T>( List<T> arr )
#endif
                {
#if JAVA
                    Collections.sort( arr );
#elif __cplusplus
                    std::stable_sort( arr.begin(), arr.end() );
#else
                    arr.Sort();
#endif
                }

#if JAVA
                public static T get<T>( Vector<T> arr, int index )
#elif __cplusplus
                template<typename T>
                static T get( vector<T>& arr, int index )
#else
                public static T get<T>( List<T> arr, int index )
#endif
                {
#if JAVA
                    return arr.get( index );
#else
                    return arr[index];
#endif
                }

#if JAVA
                public static void set<T>( Vector<T> arr, int index, T item )
#elif __cplusplus
                template<typename T>
                static void set( vector<T>& arr, int index, T item )
#else
                public static void set<T>( List<T> arr, int index, T item )
#endif
                {
#if JAVA
                    arr.set( index ) = item;
#else
                    arr[index] = item;
#endif
                }

#if JAVA
                public static int size<T>( Vector<T> arr )
#elif __cplusplus
                template<typename T>
                static int size( vector<T>& arr )
#else
                public static int size<T>( List<T> arr )
#endif
                {
#if JAVA
                    return arr.size();
#elif __cplusplus
                    return arr.size();
#else
                    return arr.Count;
#endif
                }

#if JAVA
                public static void add<T>( Vector<T> arr, T item )
#elif __cplusplus
                template<typename T>
                static void add( vector<T>& arr, T item )
#else
                public static void add<T>( List<T> arr, T item )
#endif
                {
#if JAVA
                    arr.add( item );
#elif __cplusplus
                    arr.push_back( item );
#else
                    arr.Add( item );
#endif
                }

#if JAVA
                public static boolean contains<T>( Vector<T> arr, T item )
#elif __cplusplus
                template<typename T>
                static bool contains( vector<T>& arr, T item )
#else
                public static bool contains<T>( List<T> arr, T item )
#endif
                {
#if JAVA
                    return arr.contains( item );
#elif __cplusplus
                    if ( find( arr.begin(), arr.end(), item ) != arr.end() ) {
                        return true;
                    } else {
                        return false;
                    }
#else
                    return arr.Contains( item );
#endif
                }

#if JAVA
                public static void clear<T>( Vector<T> arr )
#elif __cplusplus
                template<typename T>
                static void clear( vector<T>& arr )
#else
                public static void clear<T>( List<T> arr )
#endif
                {
#if JAVA
                    arr.clear();
#elif __cplusplus
                    arr.clear();
#else
                    arr.Clear();
#endif
                }
            };

#if __cplusplus
            class conv
#else
            public class conv
#endif
            {
#if __cplusplus
                static const int64_t c4294967296 = 4294967296LL;
#else
                const long c4294967296 = 4294967296L;
#endif
#if __cplusplus
                static const int64_t c2147483647 = 2147483647LL;
#else
                const long c2147483647 = 2147483647L;
#endif

                private conv()
                {
                }
#if __cplusplus
            public:
#endif

                /// <summary>
                /// 右論理シフト
                /// </summary>
                /// <param name="d"></param>
                /// <returns></returns>
#if JAVA
                public static byte lshr( byte d, int num )
#elif __cplusplus
                static uint8_t lshr( uint8_t d, unsigned int num )
#else
                public static byte lshr( byte d, uint num )
#endif
                {
#if __cplusplus
                    typedef unsigned char byte;
#endif
#if JAVA
                    return d >>> num;
#else
                    return (byte)(0x7F & (d >> (int)num));
#endif
                }

#if JAVA
                public static int lshr( int d, int num )
#elif __cplusplus
                static int32_t lshr( int32_t d, unsigned int num )
#else
                public static int32_t lshr( int32_t d, uint num )
#endif
                {
#if JAVA
                    return d >>> num;
#else
                    return 0x7FFFFFFF & (d >> (int)num);
#endif
                }

#if !JAVA
#if __cplusplus
                static uint32_t lshr( uint32_t d, unsigned int num )
#else
                public static uint32_t lshr( uint32_t d, uint num )
#endif
                {
                    return 0x7FFFFFFF & (d >> (int)num);
                }
#endif

#if !JAVA
#if __cplusplus
                static uint16_t lshr( uint16_t d, unsigned int num )
#else
                public static uint16_t lshr( uint16_t d, uint num )
#endif
                {
                    return (uint16_t)(0x7FFF & (d >> (int)num));
                }
#endif

#if JAVA
                public static long lshr( long d, int num )
#elif __cplusplus
                static int64_t lshr( int64_t d, unsigned int num )
#else
                public static long lshr( long d, uint num )
#endif
                {
#if __cplusplus
                    return 0x7FFFFFFFFFFFFFFFLL & (d >> (int)num);
#else
                    return 0x7FFFFFFFFFFFFFFFL & (d >> (int)num);
#endif
                }

#if JAVA
                public static void getbytes_int64_le( long data, Vector<Byte> dat )
#elif __cplusplus
                static void getbytes_int64_le( int64_t data, vector<unsigned char> &dat )
#else
                public static void getbytes_int64_le( long data, List<byte> dat )
#endif
                {
#if __cplusplus
                    typedef unsigned char byte;
#endif
                    if ( vec.size( dat ) != 8 ) {
                        vec.clear( dat );
                        for ( int i = 0; i < 8; i++ ) {
                            vec.add( dat, (byte)0 );
                        }
                    }
                    vec.set( dat, 0, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 1, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 2, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 3, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 4, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 5, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 6, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 7, (byte)(data & (byte)0xff) );
                }

#if JAVA
                public static void getbytes_uint32_le( long data, Vector<Byte> dat )
#elif __cplusplus
                static void getbytes_uint32_le( int32_t data, vector<unsigned char> dat )
#else
                public static void getbytes_uint32_le( long data, List<byte> dat )
#endif
                {
#if __cplusplus
                    typedef unsigned char byte;
#endif
                    if ( vec.size( dat ) != 4 ) {
                        vec.clear( dat );
                        for ( int i = 0; i < 4; i++ ) {
                            vec.add( dat, (byte)0 );
                        }
                    }
                    data = 0xffffffff & data;
                    vec.set( dat, 0, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 1, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 2, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 3, (byte)(data & (byte)0xff) );
                }

#if JAVA
                public static void getbytes_int32_le( int data, Vector<Byte> dat )
#elif __cplusplus
                static void getbytes_int32_le( int32_t data, vector<unsigned char> dat )
#else
                public static void getbytes_int32_le( int data, List<byte> dat )
#endif
                {
#if __cplusplus
                    int64_t v = data;
#else
                    long v = data;
#endif
                    if ( v < 0 ) {
                        v += c4294967296;
                    }
                    getbytes_uint32_le( v, dat );
                }

#if JAVA
                public static void getbytes_int32_be( int data, Vector<Byte> dat )
#elif __cplusplus
                static void getbytes_int32_be( int32_t data, vector<unsigned char> dat )
#else
                public static void getbytes_int32_be( int data, List<byte> dat )
#endif
                {
#if __cplusplus
                    int64_t v = data;
#else
                    long v = data;
#endif
                    if ( v < 0 ) {
                        v += c4294967296;
                    }
#if JAVA
                    getbytes_uint32_be( v, dat );
#else
                    getbytes_uint32_be( (uint32_t)v, dat );
#endif
                }

#if JAVA
                public static void getbytes_int64_be( long data, Vector<Byte> dat )
#elif __cplusplus
                static void getbytes_int64_be( int64_t data, vector<unsigned char> dat )
#else
                public static void getbytes_int64_be( long data, List<byte> dat )
#endif
                {
#if __cplusplus
                    typedef unsigned char byte;
#endif
                    if ( vec.size( dat ) != 8 ) {
                        vec.clear( dat );
                        for ( int i = 0; i < 8; i++ ) {
                            vec.add( dat, (byte)0 );
                        }
                    }
                    vec.set( dat, 7, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 6, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 5, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 4, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 3, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 2, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 1, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 0, (byte)(data & (byte)0xff) );
                }

#if JAVA
                public static void getbytes_uint32_be( long data, Vector<Byte> dat )
#elif __cplusplus
                static void getbytes_uint32_be( uint32_t data, vector<unsigned char> dat )
#else
                public static void getbytes_uint32_be( uint data, List<byte> dat )
#endif
                {
#if __cplusplus
                    typedef unsigned char byte;
#endif
                    if ( vec.size( dat ) != 4 ) {
                        vec.clear( dat );
                        for ( int i = 0; i < 4; i++ ) {
                            vec.add( dat, (byte)0 );
                        }
                    }
                    data = 0xffffffff & data;
                    vec.set( dat, 3, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 2, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 1, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 0, (byte)(data & (byte)0xff) );
                }

#if JAVA
                public static void getbytes_int16_le( short data, Vector<Byte> dat )
#elif __cplusplus
                static void getbytes_int16_le( int16_t data, vector<unsigned char> dat )
#else
                public static void getbytes_int16_le( short data, List<byte> dat )
#endif
                {
                    int i = data;
                    if ( i < 0 ) {
                        i += 65536;
                    }
#if JAVA
                    getbytes_uint16_le( i, dat );
#else
                    getbytes_uint16_le( (uint16_t)i, dat );
#endif
                }

#if JAVA
                public static void getbytes_uint16_le( int data, Vector<Byte> dat )
#elif __cplusplus
                static void getbytes_uint16_le( uint16_t data, vector<unsigned char> dat )
#else
                public static void getbytes_uint16_le( ushort data, List<byte> dat )
#endif
                {
#if __cplusplus
                    typedef unsigned char byte;
#endif
                    if ( vec.size( dat ) != 2 ) {
                        vec.clear( dat );
                        vec.add( dat, (byte)0 );
                        vec.add( dat, (byte)0 );
                    }
                    vec.set( dat, 0, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 1, (byte)(data & (byte)0xff) );
                }

#if JAVA
                public static void getbytes_uint16_be( int data, Vector<Byte> dat )
#elif __cplusplus
                static void getbytes_uint16_be( uint16_t data, vector<unsigned char> dat )
#else
                public static void getbytes_uint16_be( ushort data, List<byte> dat )
#endif
                {
#if __cplusplus
                    typedef unsigned char byte;
#endif
                    if ( vec.size( dat ) != 2 ) {
                        vec.clear( dat );
                        vec.add( dat, (byte)0 );
                        vec.add( dat, (byte)0 );
                    }
                    vec.set( dat, 1, (byte)(data & (byte)0xff) );
                    data = lshr( data, 8 );
                    vec.set( dat, 0, (byte)(data & (byte)0xff) );
                }

#if JAVA
                public static long make_int64_le( Vector<Byte> buf )
#elif __cplusplus
                static int64_t make_int64_le( vector<unsigned char> buf )
#else
                public static int64_t make_int64_le( List<byte> buf )
#endif
                {
#if JAVA
                    return (long)((long)((long)((long)((long)((long)((long)((long)((long)((((0xff & buf[7]) << 8) | (0xff & buf[6])) << 8) | (0xff & buf[5])) << 8) | (0xff & buf[4])) << 8) | (0xff & buf[3])) << 8 | (0xff & buf[2])) << 8) | (0xff & buf[1])) << 8 | (0xff & buf[0]);
#else
                    return (int64_t)((int64_t)((int64_t)((int64_t)((int64_t)((int64_t)((int64_t)((int64_t)((int64_t)((((0xff & buf[7]) << 8) | (0xff & buf[6])) << 8) | (0xff & buf[5])) << 8) | (0xff & buf[4])) << 8) | (0xff & buf[3])) << 8 | (0xff & buf[2])) << 8) | (0xff & buf[1])) << 8 | (0xff & buf[0]);
#endif
                }

#if JAVA
                public static long make_int64_be( Vector<byte> buf )
#elif __cplusplus
                static int64_t make_int64_be( vector<uint8_t> buf )
#else
                public static long make_int64_be( List<byte> buf )
#endif
                {
#if JAVA
                    return (long)((long)((long)((long)((long)((long)((long)((long)((long)((((0xff & buf[0]) << 8) | (0xff & buf[1])) << 8) | (0xff & buf[2])) << 8) | (0xff & buf[3])) << 8) | (0xff & buf[4])) << 8 | (0xff & buf[5])) << 8) | (0xff & buf[6])) << 8 | (0xff & buf[7]);
#else
                    return (int64_t)((int64_t)((int64_t)((int64_t)((int64_t)((int64_t)((int64_t)((int64_t)((int64_t)((((0xff & buf[0]) << 8) | (0xff & buf[1])) << 8) | (0xff & buf[2])) << 8) | (0xff & buf[3])) << 8) | (0xff & buf[4])) << 8 | (0xff & buf[5])) << 8) | (0xff & buf[6])) << 8 | (0xff & buf[7]);
#endif
                }

#if JAVA
                public static long make_uint32_le( Vector<Byte> buf, int index )
#elif __cplusplus
                static int64_t make_uint32_le( vector<uint8_t> buf, int index )
#else
                public static long make_uint32_le( List<byte> buf, int index )
#endif
                {
#if JAVA
                    return (long)((long)((long)((long)(((0xff & buf[index + 3]) << 8) | (0xff & buf[index + 2])) << 8) | (0xff & buf[index + 1])) << 8) | (0xff & buf[index]);
#else
                    return (int64_t)((int64_t)((int64_t)((int64_t)(((0xff & buf[index + 3]) << 8) | (0xff & buf[index + 2])) << 8) | (0xff & buf[index + 1])) << 8) | (0xff & buf[index]);
#endif
                }

#if JAVA
                public static long make_uint32_le( Vector<Byte> buf )
#elif __cplusplus
                static int64_t make_uint32_le( vector<uint8_t> buf )
#else
                public static long make_uint32_le( List<byte> buf )
#endif
                {
                    return make_uint32_le( buf, 0 );
                }

#if JAVA
                public static long make_uint32_be( Vector<Byte> buf, int index )
#elif __cplusplus
                static int64_t make_uint32_be( vector<uint8_t> buf, int index )
#else
                public static long make_uint32_be( List<byte> buf, int index )
#endif
                {
#if JAVA
                    return (long)((long)((long)((long)(((0xff & buf[index]) << 8) | (0xff & buf[index + 1])) << 8) | (0xff & buf[index + 2])) << 8) | (0xff & buf[index + 3]);
#else
                    return (int64_t)((int64_t)((int64_t)((int64_t)(((0xff & buf[index]) << 8) | (0xff & buf[index + 1])) << 8) | (0xff & buf[index + 2])) << 8) | (0xff & buf[index + 3]);
#endif
                }

#if JAVA
                public static long make_uint32_be( Vector<Byte> buf )
#elif __cplusplus
                static int64_t make_uint32_be( vector<uint8_t> buf )
#else
                public static long make_uint32_be( List<byte> buf )
#endif
                {
                    return make_uint32_be( buf, 0 );
                }

#if JAVA
                public static int make_int32_le( Vector<Byte> buf )
#elif __cplusplus
                static int32_t make_int32_le( vector<uint8_t> buf )
#else
                public static int make_int32_le( List<byte> buf )
#endif
                {
#if __cplusplus
                    int64_t v = make_uint32_le( buf );
#else
                    long v = make_uint32_le( buf );
#endif
                    if ( v >= c2147483647 ) {
                        v -= c4294967296;
                    }
                    return (int)v;
                }

#if JAVA
                public static int make_uint16_le( Vector<Byte> buf, int index )
#elif __cplusplus
                static uint16_t make_uint16_le( vector<uint8_t> buf, int index )
#else
                public static ushort make_uint16_le( List<byte> buf, int index )
#endif
                {
                    return (uint16_t)(0xFFFF & ((int)((0xff & buf[index + 1]) << 8) | (0xff & buf[index])));
                }

#if JAVA
                public static int make_uint16_le( Vector<Byte> buf )
#elif __cplusplus
                static uint16_t make_uint16_le( vector<uint8_t> buf )
#else
                public static ushort make_uint16_le( List<byte> buf )
#endif
                {
                    return make_uint16_le( buf, 0 );
                }

#if JAVA
                public static int make_uint16_be( Vector<Byte> buf, int index )
#elif __cplusplus
                static uint16_t make_uint16_be( vector<uint8_t> buf, int index )
#else
                public static ushort make_uint16_be( List<byte> buf, int index )
#endif
                {
                    return (uint16_t)(0xFFFF & ((int)((0xff & buf[index]) << 8) | (0xff & buf[index + 1])));
                }

#if JAVA
                public static int make_uint16_be( Vector<Byte> buf )
#elif __cplusplus
                static uint16_t make_uint16_be( vector<uint8_t> buf )
#else
                public static ushort make_uint16_be( List<byte> buf )
#endif
                {
                    return make_uint16_be( buf, 0 );
                }

#if JAVA
                public static short make_int16_le( Vector<Byte> buf, int index )
#elif __cplusplus
                static int16_t make_int16_le( vector<uint8_t> buf, int index )
#else
                public static short make_int16_le( List<byte> buf, int index )
#endif
                {
                    int i = make_uint16_le( buf, index );
                    if ( i >= 32768 ) {
                        i = i - 65536;
                    }
                    return (short)i;
                }

#if JAVA
                public static short make_int16_le( Vector<Byte> buf )
#elif __cplusplus
                static int16_t make_int16_le( vector<uint8_t> buf )
#else
                public static short make_int16_le( List<byte> buf )
#endif
                {
                    return make_int16_le( buf, 0 );
                }

                /*
                public static double make_double_le( List<byte> buf )
                {
#if JAVA
                    long n = 0L;
                    for ( int i = 7; i >= 0; i-- ) {
                        n = (n << 8) | (buf[i] & 0xffL);
                    }
                    return Double.longBitsToDouble( n );
#else
                    if ( !BitConverter.IsLittleEndian ) {
                        for ( int i = 0; i < 4; i++ ) {
                            byte d = buf[i];
                            buf[i] = buf[7 - i];
                            buf[7 - i] = d;
                        }
                    }
                    return BitConverter.ToDouble( buf, 0 );
#endif
                }

                public static double make_double_be( List<byte> buf )
                {
#if JAVA
                    long n = 0L;
                    for ( int i = 0; i <= 7; i++ ) {
                        n = (n << 8) | (buf[i] & 0xffL);
                    }
                    return Double.longBitsToDouble( n );
#else
                    if ( BitConverter.IsLittleEndian ) {
                        for ( int i = 0; i < 4; i++ ) {
                            byte d = buf[i];
                            buf[i] = buf[7 - i];
                            buf[7 - i] = d;
                        }
                    }
                    return BitConverter.ToDouble( buf, 0 );
#endif
                }

                public static float make_float_le( List<byte> buf )
                {
#if JAVA
                    int n = 0;
                    for ( int i = 3; i >= 0; i-- ) {
                        n = (n << 8) | (buf[i] & 0xff);
                    }
                    return Float.intBitsToFloat( n );
#else
                    if ( !BitConverter.IsLittleEndian ) {
                        for ( int i = 0; i < 2; i++ ) {
                            byte d = buf[i];
                            buf[i] = buf[3 - i];
                            buf[3 - i] = d;
                        }
                    }
                    return BitConverter.ToSingle( buf, 0 );
#endif
                }

                public static float make_float_be( List<byte> buf )
                {
#if JAVA
                    int n = 0;
                    for ( int i = 0; i <= 3; i++ ) {
                        n = (n << 8) | (buf[i] & 0xff);
                    }
                    return Float.intBitsToFloat( n );
#else
                    if ( BitConverter.IsLittleEndian ) {
                        for ( int i = 0; i < 2; i++ ) {
                            byte d = buf[i];
                            buf[i] = buf[3 - i];
                            buf[3 - i] = d;
                        }
                    }
                    return BitConverter.ToSingle( buf, 0 );
#endif
                }

                public static byte[] getbytes_double_le( double value )
                {
#if JAVA
                    long n = Double.doubleToLongBits( value );
                    return getbytes_int64_le( n );
#else
                    byte[] ret = BitConverter.GetBytes( value );
                    if ( !BitConverter.IsLittleEndian ) {
                        for ( int i = 0; i < 4; i++ ) {
                            byte d = ret[i];
                            ret[i] = ret[7 - i];
                            ret[7 - i] = d;
                        }
                    }
                    return ret;
#endif
                }

                public static byte[] getbytes_double_be( double value )
                {
#if JAVA
                    long n = Double.doubleToLongBits( value );
                    return getbytes_int64_be( n );
#else
                    byte[] ret = BitConverter.GetBytes( value );
                    if ( BitConverter.IsLittleEndian ) {
                        for ( int i = 0; i < 4; i++ ) {
                            byte d = ret[i];
                            ret[i] = ret[7 - i];
                            ret[7 - i] = d;
                        }
                    }
                    return ret;
#endif
                }

                public static byte[] getbytes_float_le( float value )
                {
#if JAVA
                    int n = Float.floatToIntBits( value );
                    return getbytes_int32_le( n );
#else
                    byte[] ret = BitConverter.GetBytes( value );
                    if ( !BitConverter.IsLittleEndian ) {
                        for ( int i = 0; i < 2; i++ ) {
                            byte d = ret[i];
                            ret[i] = ret[3 - i];
                            ret[3 - i] = d;
                        }
                    }
                    return ret;
#endif
                }

                public static byte[] getbytes_float_be( float value )
                {
#if JAVA
                    int n = Float.floatToIntBits( value );
                    return getbytes_int32_be( n );
#else
                    byte[] ret = BitConverter.GetBytes( value );
                    if ( BitConverter.IsLittleEndian ) {
                        for ( int i = 0; i < 2; i++ ) {
                            byte d = ret[i];
                            ret[i] = ret[3 - i];
                            ret[3 - i] = d;
                        }
                    }
                    return ret;
#endif
                }*/
            };

#if __cplusplus
            class sout
#else
            public class sout
#endif
            {
                private sout()
                {
                }
#if __cplusplus
            public:
#endif

#if JAVA
                public static void println( String s )
#elif __cplusplus
                static void println( string s )
#else
                public static void println( string s )
#endif
                {
#if JAVA
                    System.out.println( s );
#elif __cplusplus
                    cout << s << endl;
#else
                    Console.Out.WriteLine( s );
#endif
                }
            };

#if __cplusplus
            class serr
#else
            public class serr
#endif
            {
                private serr()
                {
                }
#if __cplusplus
            public:
#endif

#if JAVA
                public static void println( String s )
#elif __cplusplus
                static void println( string s )
#else
                public static void println( string s )
#endif
                {
#if JAVA
                    System.err.println( s );
#elif __cplusplus
                    cerr << s << endl;
#else
                    Console.Error.WriteLine( s );
#endif
                }
            };

            class str
            {
                private str()
                {
                }
#if __cplusplus
            public:
#endif

#if JAVA
                public static boolean endsWith( String s, String search )
#elif __cplusplus
                static bool endsWith( string s, string search )
#else
                public static bool endsWith( string s, string search )
#endif
                {
#if JAVA
                    return s.endsWith( search );
#elif __cplusplus
                    string::size_type indx = s.rfind( search );
                    if( indx == string::npos ){
                        return false;
                    }else{
                        return (indx == s.length() - search.length());
                    }
#else
                    return s.EndsWith( search );
#endif
                }

#if JAVA
                public static boolean startsWith( String s, String search )
#elif __cplusplus
                static bool startsWith( string s, string search )
#else
                public static bool startsWith( string s, string search )
#endif
                {
#if JAVA
                    return s.startsWith( search );
#elif __cplusplus
                    return (s.find( search ) == 0);
#else
                    return s.StartsWith( search );
#endif
                }

#if __cplusplus
                static string sub( string s, int start )
#else
                public static String sub( String s, int start )
#endif
                {
#if JAVA
                    return s.substring( start );
#elif __cplusplus
                    return s.substr( start );
#else
                    return s.Substring( start );
#endif
                }

#if __cplusplus
                static string sub( string s, int start, int length )
#else
                public static String sub( String s, int start, int length )
#endif
                {
#if JAVA
                    return s.substring( start, start + length );
#elif __cplusplus
                    return s.substr( start, length );
#else
                    return s.Substring( start, length );
#endif
                }

#if __cplusplus
                static int find( string s, string search )
#else
                public static int find( String s, String search )
#endif
                {
                    return find( s, search, 0 );
                }


#if __cplusplus
                static int find( string s, string search, int index )
#else
                public static int find( String s, String search, int index )
#endif
                {
#if JAVA
                    return s.indexOf( search, index );
#elif __cplusplus
                    string::size_type indx = s.find( search, (string::size_type)index );
                    if( indx == string::npos ){
                        return -1;
                    }else{
                        return (int)indx;
                    }
#else
                    return s.IndexOf( search, index );
#endif
                }

#if JAVA
                public static int split( String s, Vector<String> dst, Vector<String> splitter, boolean ignore_empty )
#elif __cplusplus
                static int split( string s, vector<string> &dst, vector<string> &splitter, bool ignore_empty )
#else
                public static int split( string s, List<string> dst, List<string> splitter, bool ignore_empty )
#endif
                {
                    int len = vec.size( splitter );
                    vec.clear( dst );
                    if ( len == 0 ) {
                        vec.add( dst, s );
                        return 1;
                    }
#if JAVA
                    String remain = s;
#else
                    string remain = s;
#endif
                    int index = find( remain, vec.get( splitter, 0 ), 0 );
                    int i = 1;
                    while ( index < 0 && i < len ) {
                        index = find( remain, vec.get( splitter, i ), 0 );
                        i++;
                    }
                    int added_count = 0;
                    while ( index >= 0 ) {
                        if ( !ignore_empty || (ignore_empty && index > 0) ) {
                            vec.add( dst, sub( remain, 0, index ) );
                            added_count++;
                        }
                        remain = sub( remain, index + len );
                        index = find( remain, vec.get( splitter, 0 ) );
                        i = 1;
                        while ( index < 0 && i < len ) {
                            index = find( remain, vec.get( splitter, i ) );
                            i++;
                        }
                    }
                    if ( !ignore_empty || (ignore_empty && length( remain ) > 0) ) {
                        vec.add( dst, remain );
                    }
                    return added_count;
                }

#if JAVA
                public static int split( String s, Vector<String> dst, String s1, boolean ignore_empty )
#elif __cplusplus
                static int split( string s, vector<string> &dst, string s1, bool ignore_empty )
#else
                public static int split( string s, List<string> dst, string s1, bool ignore_empty )
#endif
                {
#if JAVA
                    Vector<String> splitter = new Vector<String>();
#elif __cplusplus
                    vector<string> splitter;
#else
                    List<String> splitter = new List<string>();
#endif
                    vec.add( splitter, s1 );
                    return split( s, dst, splitter, ignore_empty );
                }

#if JAVA
                public static double tof( String s )
#elif __cplusplus
                static double tof( string s )
#else
                public static double tof( string s )
#endif
                {
#if JAVA
                    return Double.parseDouble( s );
#elif __cplusplus
                    return atof( s.c_str() );
#else
                    return double.Parse( s );
#endif
                }

#if JAVA
                public static int toi( String s )
#elif __cplusplus
                static int toi( string s )
#else
                public static int toi( string s )
#endif
                {
#if JAVA
                    return Integer.parseInt( s );
#elif __cplusplus
                    return atoi( s.c_str() );
#else
                    return int.Parse( s );
#endif
                }

#if JAVA
                public static int length( String a )
#elif __cplusplus
                static int length( string a )
#else
                public static int length( string a )
#endif
                {
#if JAVA
                    return a.length();
#elif __cplusplus
                    return a.length();
#else
                    return a.Length;
#endif
                }

#if __cplusplus
                static bool compare( string a, string b )
#else
                public static bool compare( string a, string b )
#endif
                {
#if JAVA
                    if ( a == null || b == null ) {
                        return false;
                    }
                    return a.Equals( b );
#else
                    return a == b;
#endif
                }

                public static string format( long value, int digits )
                {
                    return format( value, digits, 10 );
                }

#if JAVA
                public static String format( logn value, int digits, int base_num )
#elif __cplusplus
                static string format( long value, int digits, int base_num )
#else
                public static string format( long value, int digits, int base_num )
#endif
                {
#if JAVA
                    String format = new String( "0", digits );
                    DecimalFormat df = new DecimalFormat( format );
                    return df.format( value );
#elif __cplusplus
                    char b[20] = "";
                    string t = itoa( digits, b, 10 );
                    string fmt = "%0" + t + (base_num == 16 ? "X" : "d");
                    char *buf = new char[digits + 1];
                    sprintf( buf, fmt.c_str(), value );
                    string ret = buf;
                    delete [] buf;
                    return ret;
#else
                    string format = "#";
                    switch ( base_num ) {
                        case 10: {
                            format = "D" + digits;
                            break;
                        }
                        case 16: {
                            format = "X" + digits;
                            break;
                        }
                    }
                    return value.ToString( format );
#endif
                }
            };

#if __cplusplus
            class fsys
#else
            public class fsys
#endif
            {
#if JAVA
                private static String mSeparator = "";
#elif __cplusplus
                private: static string mSeparator;
#else
                private static string mSeparator = "";
#endif

                private fsys()
                {
                }
#if __cplusplus
            public:
#endif

#if __cplusplus
                static string separator()
#else
                public static String separator()
#endif
                {
                    if ( str.compare( mSeparator, "" ) ) {
#if JAVA
                        mSeparator = File.separator;
#elif __cplusplus
                        // requires "<direct.h>"
                        char *buf = new char[_MAX_PATH];
                        char *p = _getcwd( buf, _MAX_PATH );
                        if( p ){
                            string s = buf;
                            if( s.find( "\\" ) != string::npos ){
                                mSeparator = "\\";
                            }else{
                                mSeparator = "/";
                            }
                        }
                        delete [] buf;
#else
                        mSeparator = "" + Path.DirectorySeparatorChar;
#endif
                    }
                    return mSeparator;
                }

#if JAVA
                public static String combine( String path1, String path2 )
#elif __cplusplus
                static string combine( string path1, string path2 )
#else
                public static string combine( string path1, string path2 )
#endif
                {
#if !__cplusplus
                    if ( path1 == null ) path1 = "";
                    if ( path2 == null ) path2 = "";
#endif
                    separator();
                    if ( str.endsWith( path1, mSeparator ) ) {
                        path1 = str.sub( path1, 0, str.length( path1 ) - 1 );
                    }
                    if ( str.startsWith( path2, mSeparator ) ) {
                        path2 = str.sub( path2, 1 );
                    }
                    return path1 + mSeparator + path2;
                }

#if JAVA
                public static boolean isFileExists( String path )
#elif __cplusplus
                static bool isFileExists( string path )
#else
                public static bool isFileExists( string path )
#endif
                {
#if JAVA
                    File f = new File( path );
                    return f.isFile();
#elif __cplusplus
                    // requires "#include <io.h>"
                    return (access( path.c_str(), 0 ) == 0);
#else
                    return File.Exists( path );
#endif
                }
            };

            public class dicitr<K, V>
            {
                Dictionary<K, V> mDict;
                Dictionary<K, V>.KeyCollection.Enumerator itr;
                bool mHasNext = false;

                public dicitr( Dictionary<K, V> dict )
                {
                    mDict = dict;
                    itr = mDict.Keys.GetEnumerator();
                    mHasNext = itr.MoveNext();
                }

                public bool hasNext()
                {
                    return mHasNext;
                }

                public K next()
                {
                    K ret = itr.Current;
                    mHasNext = itr.MoveNext();
                    return ret;
                }
            };

#if __cplusplus
            template<typename T>
            class vecitr
#else
            public class vecitr<T>
#endif
            {
#if JAVA
                Vector<T> list;
#elif __cplusplus
#else
                List<T> list;
#endif
#if __cplusplus
                typename vector<T>::iterator itr;
                typename vector<T>::iterator itr_end;
#else
                int index = 0;
#endif

#if JAVA
                public vecitr( Vector<T> list )
#elif __cplusplus
                public vecitr( vector<T>& list )
#else
                public vecitr( List<T> list )
#endif
                {
#if __cplusplus
                    itr = list.begin();
                    itr_end = list.end();
#else
                    this.list = list;
                    index = 0;
#endif
                }

                public bool hasNext()
                {
#if JAVA
                    if ( index + 1 < list.size() ) {
                        return true;
                    } else {
                        return false;
                    }
#elif __cplusplus
                    itr++;
                    bool ret = false;
                    if( itr != itr_end ){
                        ret = true;
                    }
                    itr--;
                    return ret;
#else
                    if ( index + 1 < list.Count ) {
                        return true;
                    } else {
                        return false;
                    }
#endif
                }

#if __cplusplus
                public T *next()
#else
                public T next()
#endif
                {
#if JAVA
                    index++;
                    return list.get( index );
#elif __cplusplus
                    itr++;
                    return &*itr;
#else
                    index++;
                    return list[index];
#endif

                }
            };

            class dic
            {
                private dic()
                {
                }
#if __cplusplus
            public:
#endif

#if JAVA
                public static int count<K, V>( TreeMap<K, V> dict )
#elif __cplusplus
                template<typename K, typename V>
                static int count( map<K, V> &dict )
#else
                public static int count<K, V>( Dictionary<K, V> dict )
#endif
                {
#if JAVA
                    reutrn dict.size();
#elif __cplusplus
                    reutrn dict.count();
#else
                    return dict.Count;
#endif
                }

#if JAVA
                public static bool containsKey<K, V>( TreeMap<K, V> dict, K key )
#elif __cplusplus
                template<typename K, typename V>
                static bool containsKey( map<K, V> &dict, K key )
#else
                public static bool containsKey<K, V>( Dictionary<K, V> dict, K key )
#endif
                {
#if JAVA
                    return dict.containsKey( key );
#elif __cplusplus
                    return dict.count( key ) > 0;
#else
                    return dict.ContainsKey( key );
#endif
                }

#if JAVA
                public static void put<K, V>( TreeMap<K, V> dict, K key, V value )
#elif __cplusplus
                template<typename K, typename V>
                static void put( map<K, V> &dict, K key, V value )
#else
                public static void put<K, V>( Dictionary<K, V> dict, K key, V value )
#endif
                {
#if JAVA
                    dict.put( key, value );
#else
                    dict[key] = value;
#endif
                }

#if JAVA
                public static V get<K, V>( TreeMap<K, V> dict, K key )
#elif __cplusplus
                template<typename K, typename V>
                static V get( map<K, V> &dict, K key )
#else
                public static V get<K, V>( Dictionary<K, V> dict, K key )
#endif
                {
#if JAVA
                    return dict.get( key );
#else
                    return dict[key];
#endif
                }
            };

#if !JAVA
        }
    }
}

// staticメンバーの実体の宣言
#if __cplusplus
std::string org::kbinani::vsq::fsys::mSeparator = "";
#endif

#endif
