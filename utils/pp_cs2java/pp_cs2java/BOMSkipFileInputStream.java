package pp_cs2java;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;

class BOMSkipFileInputStream extends InputStream
{
    private FileInputStream mStream = null;

    public BOMSkipFileInputStream( String path ) throws IOException
    {
        mStream = new FileInputStream( path );
        // 最初の3バイトを読み込むEF BB BF
        int b0 = mStream.read();
        int b1 = mStream.read();
        int b2 = mStream.read();
        if( b0 != 0xEF || b1 != 0xBB || b2 != 0xBF )
        {
            mStream.close();
            mStream = new FileInputStream( path );
        }
    }

    @Override
    public int read() throws IOException
    {
        return mStream.read();
    }
}
