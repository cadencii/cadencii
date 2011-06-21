package pp_cs2java;

import java.io.BufferedReader;
import java.util.Vector;

public class SourceText
{
    protected Vector<CommentPosition> mComments = new Vector<CommentPosition>();    
    protected Vector<String> mLines = new Vector<String>();

    protected SourceText()
    {
    }
    
    public SourceText( BufferedReader reader )
    {
        // 読み込む
        try
        {
            String line;
            while( (line = reader.readLine()) != null )
            {
                mLines.add( line );
            }
        }
        catch( Exception ex )
        {
            
        }
        
        // コメントの範囲を調べる
        CommentPosition pos = null;
        int lines = mLines.size();
        
        // まず/**/のコメント
        for( int i = 0; i < lines; i++ )
        {
            String line = mLines.get( i );
            int indx = 0;
            while( true )
            {
                if( pos == null )
                {
                    // コメントが未だ始まっていないので/*を探す
                    int start = line.indexOf( "/*", indx );
                    if( start < 0 ) break;
                    pos = new CommentPosition( i, start );
                    indx = start + 2;
                }
                else
                {
                    // コメントの途中なので*/を探す
                    int end = line.indexOf( "*/", indx );
                    if( end < 0 ) break;
                    pos.setEnd( i, end + 2 );
                    mComments.add( pos );
                    pos = null;
                    indx = end + 2;
                    pos = null;
                }
                if( indx >= line.length() ) break;
            }
        }
    }
    
    public int getLineCount()
    {
        return mLines.size();
    }
}
