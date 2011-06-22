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

        // 1行ずつみていく
        for( int i = 0; i < lines; i++ )
        {
            String line = mLines.get( i );
            int indx = 0;
            while( true )
            {
                if( pos == null )
                {
                    // コメントが未だ始まっていないので"/*"または"//"を探す
                    int start_range = line.indexOf( "/*", indx );
                    int start_line = line.indexOf( "//", indx );
                    if( start_range < 0 && start_line < 0 ) break;
                    if( 0 <= start_range && 0 <= start_line ){
                        // "/*"と"//"の両方がある場合
                        if( start_range < start_line ){
                            // "/*"のほうが先にあるので採用
                            start_line = -1;
                        }else if( start_line < start_range ){
                            // "//"のほうが先にあるので採用
                            start_range = -1;
                        }
                    }
                    if( 0 <= start_range ){
                        // "/*"の場合
                        pos = new CommentPosition( i, start_range );
                        pos.setEnd( Integer.MAX_VALUE, Integer.MAX_VALUE );
                        mComments.add( pos );
                        indx = start_range + 2;
                    }else{
                        if( !isInComment( i, start_line ) ){
                            // "//"の場合
                            pos = new CommentPosition( i, start_line );
                            pos.setEnd( i, line.length() );
                            mComments.add( pos );
                            indx = line.length();
                            pos = null;
                        }
                    }
                }
                else
                {
                    // コメントの途中なので*/を探す
                    int end = line.indexOf( "*/", indx );
                    if( end < 0 ) break;
                    pos.setEnd( i, end + 2 );
                    pos = null;
                    indx = end + 2;
                    pos = null;
                }
                if( indx >= line.length() ) break;
            }
        }
    }
    
    /**
     * 指定した位置の文字がコメント文に属するものかどうかをチェックします．
     * コメントの開始・終了を指定する文字列もコメント文と判定します．
     * @return
     */
    public boolean isInComment( int line, int column )
    {
        for ( CommentPosition pos : mComments )
        {
            int start_line = pos.getStartLine();
            int start_column = pos.getStartColumn();
            int end_line = pos.getEndLine();
            int end_column = pos.getEndColumn();
            if( line < start_line ) continue;
            if( end_line < line ) continue;
            if( start_line < line && line < end_line ) return true;
            if( start_line == end_line )
            {
                if( line == start_line )
                {
                    if( start_column <= column && column < end_column )
                    {
                        return true;
                    }
                }
            }
            else
            {
                if( start_line == line )
                {
                    if( start_column <= column )
                    {
                        return true;
                    }
                }
                if( end_line == line )
                {
                    if( column < end_column )
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    public int getLineCount()
    {
        return mLines.size();
    }
}
