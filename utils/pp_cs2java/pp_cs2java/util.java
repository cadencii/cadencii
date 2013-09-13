package pp_cs2java;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.channels.FileChannel;
import java.util.Vector;

class util
{
    private static String mSeparator;

    public static String separator()
    {
        mSeparator = File.separator;
        return mSeparator;
    }

    public static String combine( String path1, String path2 )
    {
        separator();
        if( path1.endsWith( mSeparator ) )
        {
            path1 = path1.substring( 0, path1.length() - 1 );
        }
        if( path2.startsWith( mSeparator ) )
        {
            path2 = path2.substring( 1 );
        }
        return path1 + mSeparator + path2;
    }

    public static String getFileNameWithoutExtension( String path )
    {
        String file = getFileName( path );
        int index = file.lastIndexOf( "." );
        if( index > 0 )
        {
            file = file.substring( 0, index );
        }
        return file;
    }

    public static String getFileName( String path )
    {
        File f = new File( path );
        return f.getName();
    }

    public static String getDirectoryName( String path )
    {
        File f = new File( path );
        return f.getParent();
    }

    public static boolean isDirectoryExists( String path )
    {
        File f = new File( path );
        if( f.exists() )
        {
            if( f.isFile() )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public static boolean isFileExists( String path )
    {
        File f = new File( path );
        if( f.exists() )
        {
            if( f.isFile() )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public static void createDirectory( String path )
    {
        File f = new File( path );
        f.mkdir();
    }

    public static String[] listFiles( String directory, String extension )
    {
        File f = new File( directory );
        File[] list = f.listFiles();
        if( list == null )
        {
            return new String[]{};
        }
        Vector<String> ret = new Vector<String>();
        for( int i = 0; i < list.length; i++ )
        {
            File t = list[i];
            if( !t.isDirectory() )
            {
                String name = t.getName();
                if( name.endsWith( extension ) )
                {
                    ret.add( name );
                }
            }
        }
        return ret.toArray( new String[]{} );
    }

    public static void copyFile( String file1, String file2 )
            throws FileNotFoundException, IOException
    {
        FileChannel sourceChannel = new FileInputStream( new File( file1 ) )
                .getChannel();
        FileChannel destinationChannel = new FileOutputStream( new File( file2 ) )
                .getChannel();
        sourceChannel.transferTo( 0, sourceChannel.size(), destinationChannel );
        sourceChannel.close();
        destinationChannel.close();
    }
}
