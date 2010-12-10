#if JAVA
package org.kbinani.cadencii;
#else
#if !__cplusplus
using System;
using System.Collections.Generic;

#endif
namespace org
{
    namespace kbinani
    {
        namespace vsq
        {
#endif

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
                vector<T> *list;
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
#if __cplusplus
            };
#else
            }
#endif

            class dic
            {
                private dic()
                {
                }

#if JAVA
                public static bool containsKey<K, V>( TreeMap<K, V> dict, K key )
#elif __cplusplus
                template<typename K, typename V>
                static bool containsKey( map<K, V> dict, K key )
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
                static void put( map<K, V> dict, K key, V value )
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
                static V get( map<K, V> dict, K key )
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
#if __cplusplus
            };
#else
            }
#endif

            class str
            {
                private str()
                {
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
            public:
#endif
                /* string utility */
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

#if JAVA
                public static String format( logn value, int digits )
#elif __cplusplus
                static string format( long value, int digits )
#else
                public static string format( long value, int digits )
#endif
                {
#if JAVA
                    String format = new String( "0", digits );
                    DecimalFormat df = new DecimalFormat( format );
                    return df.format( value );
#elif __cplusplus
                    char b[20] = "";
                    string t = itoa( digits, b, 10 );
                    string fmt = "%0" + t + "d";
                    char *buf = new char[digits + 1];
                    sprintf( buf, fmt.c_str(), value );
                    string ret = buf;
                    delete [] buf;
                    return ret;
#else
                    string format = new string( '0', digits );
                    return value.ToString( format );
#endif
                }
#if __cplusplus
            };
#else
            }
#endif

            class vec 
            {
                private vec()
                {
                }

#if __cplusplus
            public:
#endif
                /* vector utility */
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
#if __cplusplus
            };
#else
            }
#endif

#if !JAVA
        }
    }
}
#endif
