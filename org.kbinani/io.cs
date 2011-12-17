#if !JAVA
/*
 * io.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;

namespace com.github.cadencii.java.io
{

    public interface OutputStream
    {
    }

    public interface InputStream
    {
        void close();
        int read( byte[] b );
        int read( byte[] b, int off, int len );
    }

    public class ObjectInputStream : InputStream
    {
        private System.Runtime.Serialization.Formatters.Binary.BinaryFormatter m_formatter;
        private System.IO.Stream m_stream;

        public ObjectInputStream( System.IO.Stream stream )
        {
            m_stream = stream;
            m_formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        }

        public object readObject()
        {
            return m_formatter.Deserialize( m_stream );
        }

        public int read( byte[] b )
        {
            return m_stream.Read( b, 0, b.Length );
        }

        public int read( byte[] b, int off, int len )
        {
            return m_stream.Read( b, off, len );
        }

        public void close()
        {
            m_stream.Close();
        }
    }

    public class ObjectOutputStream : OutputStream
    {
        private System.Runtime.Serialization.Formatters.Binary.BinaryFormatter m_formatter;
        private System.IO.Stream m_stream;

        public ObjectOutputStream( System.IO.Stream stream )
        {
            m_formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            m_stream = stream;
        }

        public void writeObject( Object obj )
        {
            m_formatter.Serialize( m_stream, obj );
        }
    }

    public class ByteArrayOutputStream : System.IO.MemoryStream, OutputStream
    {
        public ByteArrayOutputStream()
            : base()
        {
        }

        public byte[] toByteArray()
        {
            base.Seek( 0, System.IO.SeekOrigin.Begin );
            byte[] ret = new byte[base.Length];
            base.Read( ret, 0, ret.Length );
            return ret;
        }
    }

    public class ByteArrayInputStream : System.IO.MemoryStream, InputStream
    {
        public ByteArrayInputStream( byte[] buf )
            : base( buf )
        {
        }

        public void close()
        {
            base.Close();
        }

        public int read( byte[] b )
        {
            return base.Read( b, 0, b.Length );
        }

        public int read( byte[] b, int off, int len )
        {
            return base.Read( b, off, len );
        }
    }

    public class File
    {
        public readonly string separator = System.IO.Path.DirectorySeparatorChar + "";
        public readonly char separatorChar = System.IO.Path.DirectorySeparatorChar;

        private string m_path;

        /// <summary>
        /// 指定されたパス名文字列を抽象パス名に変換して、新しい File のインスタンスを生成します。
        /// </summary>
        /// <param name="pathname"></param>
        public File( String pathname )
        {
            m_path = pathname;
        }

        /// <summary>
        /// この抽象パス名が示すファイルをアプリケーションが実行できるかどうかを判定します。
        /// </summary>
        public bool canExecute()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名が示すファイルをアプリケーションが読み込めるかどうかを判定します。
        /// </summary>
        public bool canRead()
        {
            System.IO.FileStream fs = null;
            bool ret = false;
            try {
                fs = System.IO.File.OpenRead( m_path );
                ret = true;
            } catch {
                ret = false;
            } finally {
                if ( fs != null ) {
                    try {
                        fs.Close();
                    } catch {
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// この抽象パス名が示すファイルをアプリケーションが変更できるかどうかを判定します。
        /// </summary>
        public bool canWrite()
        {
            System.IO.FileStream fs = null;
            bool ret = false;
            try {
                fs = System.IO.File.OpenWrite( m_path );
                ret = true;
            } catch {
                ret = false;
            } finally {
                if ( fs != null ) {
                    try {
                        fs.Close();
                    } catch {
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 2 つの抽象パス名を語彙的に比較します。
        /// </summary>
        public int compareTo( File pathname )
        {
            return System.IO.Path.GetFullPath( this.m_path ).CompareTo( System.IO.Path.GetFullPath( pathname.m_path ) );
        }

        /// <summary>
        /// この抽象パス名が示す空の新しいファイルを不可分 (atomic) に生成します (その名前のファイルがまだ存在しない場合だけ)。
        /// </summary>
        public bool createNewFile()
        {
            if ( System.IO.File.Exists( m_path ) ) {
                return false;
            } else {
                System.IO.File.Create( m_path );
                return true;
            }
        }

        /// <summary>
        /// 指定された接頭辞と接尾辞をファイル名の生成に使用して、デフォルトの一時ファイルディレクトリに空のファイルを生成します。
        /// </summary>
        public static File createTempFile( string prefix, string suffix )
        {
            string tmp = System.IO.Path.GetTempFileName();
            string dir = System.IO.Path.GetDirectoryName( tmp );
            string file = System.IO.Path.GetFileName( tmp );
            return new File( System.IO.Path.Combine( dir, prefix + file + suffix ) );
        }

        /// <summary>
        /// 指定されたディレクトリで新しい空のファイルを生成し、その名前には、指定された接頭辞および接尾辞の文字列が使用されます。
        /// </summary>
        public static File createTempFile( string prefix, string suffix, File directory )
        {
            String dir = System.IO.Path.GetTempPath();
            if ( directory != null ) {
                dir = directory.m_path;
            }
            if ( !System.IO.Directory.Exists( dir ) ) {
                throw new System.IO.IOException();
            }
            while ( true ) {
                String f = prefix + System.IO.Path.GetRandomFileName() + suffix;
                String full = System.IO.Path.Combine( dir, f );
                if ( !System.IO.File.Exists( full ) ) {
                    System.IO.File.Create( full );
                    return new File( full );
                }
            }
            throw new System.IO.IOException();
        }

        /// <summary>
        /// この抽象パス名が示すファイルまたはディレクトリを削除します。
        /// </summary>
        public bool delete()
        {
            try {
                System.IO.File.Delete( m_path );
                return true;
            } catch {
            }
            return false;
        }

        /// <summary>
        /// この抽象パス名が示すファイルまたはディレクトリが、仮想マシンが終了したときに削除されるように要求します。
        /// </summary>
        public void deleteOnExit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名が指定されたオブジェクトと等しいかどうかを判定します。
        /// </summary>
        public bool equals( object obj )
        {
            return base.Equals( obj );
        }

        /// <summary>
        /// この抽象パス名が示すファイルまたはディレクトリが存在するかどうかを判定します。
        /// </summary>
        public bool exists()
        {
            bool file = System.IO.File.Exists( m_path );
            bool dir = System.IO.Directory.Exists( m_path );
            return file || dir;
        }

        /// <summary>
        /// この抽象パス名の絶対形式を返します。
        /// </summary>
        public File getAbsoluteFile()
        {
            return new File( getAbsolutePath() );
        }

        /// <summary>
        /// この抽象パス名の絶対パス名文字列を返します。
        /// </summary>
        public string getAbsolutePath()
        {
            return System.IO.Path.GetFullPath( m_path );
        }

        /// <summary>
        /// この抽象パス名の正規の形式を返します。
        /// </summary>
        public File getCanonicalFile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名の正規のパス名文字列を返します。
        /// </summary>
        public string getCanonicalPath()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名で指定されるパーティション内で未割り当てのバイト数を返します。
        /// </summary>
        public long getFreeSpace()
        {
            string drive = System.IO.Path.GetPathRoot( m_path );
            foreach ( System.IO.DriveInfo di in System.IO.DriveInfo.GetDrives() ) {
                if ( di.RootDirectory.FullName == drive ) {
                    return di.TotalFreeSpace;
                }
            }
            return 0;
        }

        /// <summary>
        /// この抽象パス名が示すファイルまたはディレクトリの名前を返します。
        /// </summary>
        public string getName()
        {
            return System.IO.Path.GetFileName( m_path );
        }

        /// <summary>
        /// この抽象パス名の親のパス名文字列を返します。
        /// </summary>
        public string getParent()
        {
            return System.IO.Path.GetDirectoryName( m_path );
        }

        /// <summary>
        /// この抽象パス名の親の抽象パス名を返します。
        /// </summary>
        public File getParentFile()
        {
            return new File( getParent() );
        }

        /// <summary>
        /// この抽象パス名をパス名文字列に変換します。
        /// </summary>
        public string getPath()
        {
            return m_path;
        }

        /// <summary>
        /// この抽象パス名で指定されるパーティションのサイズを返します。
        /// </summary>
        public long getTotalSpace()
        {
            return getFreeSpace();
        }

        /// <summary>
        /// この抽象パス名で指定されるパーティション上で、この仮想マシンが利用できるバイト数を返します。
        /// </summary>
        public long getUsableSpace()
        {
            return getFreeSpace();
        }

        /// <summary>
        /// この抽象パス名のハッシュコードを計算します。
        /// </summary>
        public int hashCode()
        {
            return m_path.GetHashCode();
        }

        /// <summary>
        /// この抽象パス名が絶対かどうかを判定します。
        /// </summary>
        public bool isAbsolute()
        {
            return System.IO.Path.IsPathRooted( m_path );
        }

        /// <summary>
        /// この抽象パス名が示すファイルがディレクトリであるかどうかを判定します。
        /// </summary>
        public bool isDirectory()
        {
            bool dir = System.IO.Directory.Exists( m_path );
            if ( dir ) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// この抽象パス名が示すファイルが普通のファイルかどうかを判定します。
        /// </summary>
        public bool isFile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名が示すファイルが隠しファイルかどうかを判定します。
        /// </summary>
        public bool isHidden()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名が示すファイルが最後に変更された時刻を返します。
        /// </summary>
        public long lastModified()
        {
            System.IO.FileInfo f = new System.IO.FileInfo( m_path );
            return f.LastWriteTimeUtc.Ticks;
        }

        /// <summary>
        /// この抽象パス名に指定されているファイルの長さを返します。
        /// </summary>
        public long length()
        {
            System.IO.FileInfo f = new System.IO.FileInfo( m_path );
            return f.Length;
        }

        /// <summary>
        /// この抽象パス名が示すディレクトリにあるファイルおよびディレクトリを示す文字列の配列を返します。
        /// </summary>
        public string[] list()
        {
            List<string> list = new List<string>();
            list.AddRange( System.IO.Directory.GetDirectories( m_path ) );
            list.AddRange( System.IO.Directory.GetFiles( m_path ) );
            return list.ToArray();
        }

        /// <summary>
        /// この抽象パス名が示すディレクトリにあるファイルおよびディレクトリの中で、指定されたフィルタの基準を満たすものの文字列の配列を返します。
        /// </summary>
        public string[] list( FilenameFilter filter )
        {
            List<string> ret = new List<string>();
            foreach ( string s in list() ) {
                if ( filter.accept( this, s ) ) {
                    ret.Add( s );
                }
            }
            return ret.ToArray();
        }

        /// <summary>
        /// この抽象パス名が示すディレクトリ内のファイルを示す抽象パス名の配列を返します。
        /// </summary>
        public File[] listFiles()
        {
            string[] files = System.IO.Directory.GetFiles( m_path );
            File[] ret = new File[files.Length];
            for ( int i = 0; i < files.Length; i++ ) {
                ret[i] = new File( m_path + separator + files[i] );
            }
            return ret;
        }

        /// <summary>
        /// この抽象パス名が示すディレクトリにあるファイルおよびディレクトリの中で、指定されたフィルタの基準を満たすものの抽象パス名の配列を返します。
        /// </summary>
        public File[] listFiles( FileFilter filter )
        {
            List<File> ret = new List<File>();
            foreach ( string s in list() ) {
                File f = new File( m_path + separator + s );
                if ( filter.accept( f ) ) {
                    ret.Add( f );
                }
            }
            return ret.ToArray();
        }

        /// <summary>
        /// この抽象パス名が示すディレクトリにあるファイルおよびディレクトリの中で、指定されたフィルタの基準を満たすものの抽象パス名の配列を返します。
        /// </summary>
        public File[] listFiles( FilenameFilter filter )
        {
            List<File> ret = new List<File>();
            foreach ( string s in list() ) {
                if ( filter.accept( this, s ) ) {
                    File f = new File( m_path + separator + s );
                    ret.Add( f );
                }
            }
            return ret.ToArray();
        }

        /// <summary>
        /// 有効なファイルシステムのルートをリスト表示します。
        /// </summary>
        public static File[] listRoots()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名が示すディレクトリを生成します。
        /// </summary>
        public bool mkdir()
        {
            try {
                System.IO.Directory.CreateDirectory( m_path );
                return true;
            } catch {
            }
            return false;
        }

        /// <summary>
        /// この抽象パス名が示すディレクトリを生成します。
        /// </summary>
        public bool mkdirs()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名が示すファイルの名前を変更します。
        /// </summary>
        public bool renameTo( File dest )
        {
            try {
                System.IO.File.Replace( m_path, dest.m_path, m_path + "BAK" );
                return true;
            } catch {
            }
            return false;
        }

        /// <summary>
        /// この抽象パス名に所有者の実行権を設定する簡易メソッドです。
        /// </summary>
        public bool setExecutable( bool executable )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名に所有者または全員の実行権を設定します。
        /// </summary>
        public bool setExecutable( bool executable, bool ownerOnly )
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// この抽象パス名が示すファイルまたはディレクトリが変更された時刻を設定します。
        /// </summary>
        public bool setLastModified( long time )
        {
            try {
                System.IO.FileInfo f = new System.IO.FileInfo( m_path );
                f.LastWriteTimeUtc = new DateTime( time );
                return true;
            } catch {
            }
            return false;
        }

        /// <summary>
        /// この抽象パス名に所有者の読み取り権を設定する簡易メソッドです。
        /// </summary>
        public bool setReadable( bool readable )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名に所有者または全員の読み取り権を設定します。
        /// </summary>
        public bool setReadable( bool readable, bool ownerOnly )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名が示すファイルまたはディレクトリにマークを設定し、読み込みオペレーションだけが許可されるようにします。
        /// </summary>
        public bool setReadOnly()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名に所有者の書き込み権を設定する簡易メソッドです。
        /// </summary>
        public bool setWritable( bool writable )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名に所有者または全員の書き込み権を設定します。
        /// </summary>
        public bool setWritable( bool writable, bool ownerOnly )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// この抽象パス名のパス名文字列を返します。
        /// </summary>
        public string toString()
        {
            return m_path;
        }
    }

    public interface FilenameFilter
    {
        bool accept( File dir, String name );
    }

    public interface FileFilter
    {
        bool accept( File filepath );
    }
}
#endif
