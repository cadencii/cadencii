package pp_cs2java;

import static org.junit.Assert.*;
import java.io.BufferedReader;
import java.io.InputStreamReader;
import org.junit.Test;

/**
 * SourceTextのテストクラスです．
 * @author kbinani
 */
public class SourceTextTest extends SourceText
{
    static SourceText mSingleLine = null;
    static SourceText mRange = null;

    static
    {
        mSingleLine = loadData( "test/data/SourceText_Comment_Single.txt" );
        mRange = loadData( "test/data/SourceText_Comment_Range.txt" );
    }
    
    public SourceTextTest()
    {
    }

    /**
     * 範囲コメントのテスト．
     * データの行数が14であることをテストします．
     */
    @Test
    public void testMultiLines()
    {
        assertEquals( 14, mRange.mLines.size() );
    }
    
    /**
     * 範囲コメントのテスト．
     * コメントの個数が4であることをテストします．
     */
    @Test
    public void testMultiComments()
    {
        assertEquals( 4, mRange.mComments.size() );
    }
    
    /**
     * 範囲コメントのテスト．
     * 各コメントの開始行等をテストします．
     */
    @Test
    public void testMultiPosition()
    {
        // 1個目のコメント
        // 2行1カラム～4行4カラム
        CommentPosition pos = mRange.mComments.get( 0 );
        assertEquals( 1, pos.getStartLine() );
        assertEquals( 0, pos.getStartColumn() );
        assertEquals( 3, pos.getEndLine() );
        assertEquals( 3, pos.getEndColumn() );
        
        // 2個目のコメント
        // 7行1カラム～10行4カラム
        pos = mRange.mComments.get( 1 );
        assertEquals( 6, pos.getStartLine() );
        assertEquals( 0, pos.getStartColumn() );
        assertEquals( 9, pos.getEndLine() );
        assertEquals( 3, pos.getEndColumn() );
        
        // 3個目のコメント
        // 12行1カラム～12行5カラム
        pos = mRange.mComments.get( 2 );
        assertEquals( 11, pos.getStartLine() );
        assertEquals( 0, pos.getStartColumn() );
        assertEquals( 11, pos.getEndLine() );
        assertEquals( 4, pos.getEndColumn() );
        
        // 4個目のコメント
        // 12行5カラム～14行3カラム
        pos = mRange.mComments.get( 3 );
        assertEquals( 11, pos.getStartLine() );
        assertEquals( 4, pos.getStartColumn() );
        assertEquals( 13, pos.getEndLine() );
        assertEquals( 2, pos.getEndColumn() );
    }
    
    /**
     * シンプルな行コメントのテストを行います
     * 行数が1であることをテスト
     */
    @Test
    public void testSingleLines()
    {
        assertEquals( 1, mSingleLine.mLines.size() );
    }
    
    @Test
    public void testSingleLineCommentCount()
    {
        // 1個のコメントがあることをテスト
        assertEquals( 1, mSingleLine.mComments.size() );
    }
    
    @Test
    public void testSingleLineStartColumn()
    {
        // 開始カラムが2であることをテスト．2カラム目はインデックスにすると1であることに注意
        assertEquals( 1, mSingleLine.mComments.get( 0 ).getStartColumn() );
    }
    
    @Test
    public void testSingleLineEndColumn()
    {
        // 終了カラムが-1であることをテスト
        assertEquals( -1, mSingleLine.mComments.get( 0 ).getEndColumn() );
    }
    
    @Test
    public void testSingleLineStartLine()
    {        
        // 開始行が1であることをテスト
        assertEquals( 0, mSingleLine.mComments.get( 0 ).getStartLine() );
    }
     
    @Test
    public void testSingleLineEndLine()
    {
        // 終了行が1であることをテスト．
        assertEquals( 0, mSingleLine.mComments.get( 0 ).getEndLine() );
    }
    
    /**
     * テスト用のデータを読み込みます．
     * @return
     */
    private static SourceText loadData( String path )
    {
        BufferedReader reader = null;
        SourceText obj = null;
        try
        {
            reader = new BufferedReader( 
                    new InputStreamReader( 
                        new BOMSkipFileInputStream( path ) ) );
            obj = new SourceText( reader );
        }
        catch( Exception ex ){}
        finally
        {
            if( reader != null )
            {
                try
                {
                    reader.close();
                }
                catch( Exception ex2 ){}
            }
        }
        if( obj == null )
        {
            fail();
            return null;
        }
        else
        {
            return obj;
        }
    }
    
}
