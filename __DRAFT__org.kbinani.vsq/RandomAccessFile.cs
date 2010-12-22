#if !JAVA
/*
 * RandomAccessFile.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
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

#if __cplusplus
            class RandomAccessFile
#else
            public class RandomAccessFile
#endif
            {
#if __cplusplus
                FILE *mStream;
                int64_t mLength;
#else
                private System.IO.FileStream mStream;
                private long mLength;
#endif

                public RandomAccessFile( string name, string mode )
                {
#if __cplusplus
                    mode += "b";
                    mStream = fopen( name.c_str(), mode.c_str() );
                    fseek( mStream, 0, SEEK_END );
                    fpos_t pos;
                    fgetpos( mStream, &pos );
                    mLength = (int64_t)pos;
                    fseek( mStream, 0, SEEK_SET );
#else
                    if ( mode == "r" ) {
                        mStream = new System.IO.FileStream( name, System.IO.FileMode.Open, System.IO.FileAccess.Read );
                    } else if ( mode == "rw" ) {
                        mStream = new System.IO.FileStream( name, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite );
                    } else {
                        throw new ArgumentException( "mode: \"" + mode + "\" is not supported", "mode" );
                    }
                    mLength = mStream.Length;
#endif
                }

                public void close()
                {
#if __cplusplus
                    fclose( mStream );
#else
                    mStream.Close();
#endif
                }

#if __cplusplus
                public int64_t length()
#else
                public long length()
#endif
                {
                    return mLength;
                }

                public int read()
                {
#if __cplusplus
                    return fgetc( mStream );
#else
                    return mStream.ReadByte();
#endif
                }

#if __cplusplus
                public int read( vector<uint8_t> b )
#else
                public int read( List<byte> b )
#endif
                {
                    return read( b, 0, vec.size( b ) );
                }

#if __cplusplus
                public int read( vector<uint8_t> b, int off, int len )
#else
                public int read( List<byte> b, int off, int len )
#endif
                {
#if __cplusplus
                    typedef uint8_t byte;
#endif
                    int ret = 0;
                    for ( int i = 0; i < len; i++ ) {
                        int v = read();
                        if ( v >= 0 ) {
                            vec.set( b, i + off, (byte)v );
                            ret++;
                        } else {
                            break;
                        }
                    }
                    return ret;
                }

#if __cplusplus
                public void seek( int64_t pos )
#else
                public void seek( long pos )
#endif
                {
#if __cplusplus
                    fseek( mStream, pos, SEEK_SET );
#else
                    mStream.Seek( pos, System.IO.SeekOrigin.Begin );
#endif
                }

#if __cplusplus
                public void write( vector<uint8_t> b )
#else
                public void write( List<byte> b )
#endif
                {
                    write( b, 0, vec.size( b ) );
                }

#if __cplusplus
                public void write( vector<uint8_t> b, int off, int len )
#else
                public void write( List<byte> b, int off, int len )
#endif
                {
                    for ( int i = 0; i < len; i++ ) {
                        write( vec.get( b, off + i ) );
                    }
                }

                public void write( int b )
                {
#if __cplusplus
                    fputc( b, mStream );
#else
                    mStream.WriteByte( (byte)b );
#endif
                }

                public void writeByte( int b )
                {
                    write( b );
                }

#if __cplusplus
                public int64_t getFilePointer()
#else
                public long getFilePointer()
#endif
                {
#if __cplusplus
                    fpos_t pos;
                    fgetpos( mStream, &pos );
                    return (int64_t)pos;
#else
                    return mStream.Position;
#endif
                }
            };

        }
    }
}
#endif
