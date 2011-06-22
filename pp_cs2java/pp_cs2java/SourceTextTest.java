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
    /**
     * 行コメントのテストデータを保持します．
     */
    static SourceText mSingle = null;
    /**
     * 範囲コメントのテストデータを保持します．
     */
    static SourceText mRange = null;
    /**
     * 行コメントと範囲コメントを組み合わせたテストデータを保持します．
     */
    static SourceText mComplex = null; 
    /**
     * #if構文の検索テストのためのデータを保持します．
     */
    static SourceText mIfSyntax = null;
    
    static
    {
        mSingle = loadData( "test/data/SourceText_Comment_Single.txt" );
        mRange = loadData( "test/data/SourceText_Comment_Range.txt" );
        mComplex = loadData( "test/data/SourceText_Comment_Complex.cs" );
        mIfSyntax = loadData( "test/data/SourceText_IfSyntax.cs" );
    }
    
    public SourceTextTest()
    {
    }

    /**
     * #if構文の検索テスト
     */
    @Test
    public void findIfSyntax()
    {
    	// 最初の#if文
    	assertEquals( 6, mIfSyntax.findIfSyntax( 0 ) );
    	
    	// ネストされた#if文
    	assertEquals( 17, mIfSyntax.findIfSyntax( 8 ) );
    	
    	// ネストされた#if文のうち，途中から検索開始した場合
    	assertEquals( 15, mIfSyntax.findIfSyntax( 9 ) );
    	
    	// #ifで始まっていない行の行番号を渡された場合
    	assertEquals( -1, mIfSyntax.findIfSyntax( 19 ) );
    }
    
    /**
     * 範囲と行コメントの組み合わせのテスト．
     * テストデータの行数
     */
    @Test
    public void testComplexLines()
    {
        assertEquals( 6, mComplex.mLines.size() );
    }
    
    /**
     * 範囲と行コメントの組み合わせのテスト．
     * コメント箇所の個数
     */
    @Test
    public void testComplexComments()
    {
        assertEquals( 2, mComplex.mComments.size() );
    }
    
    /**
     * 範囲と行コメントの組み合わせのテスト．
     * コメント範囲
     */
    @Test
    public void testComplexPosition()
    {
        CommentPosition pos = mComplex.mComments.get( 0 );
        assertEquals( 1, pos.getStartLine() );
        assertEquals( 0, pos.getStartColumn() );
        assertEquals( 1, pos.getEndLine() );
        assertEquals( 44, pos.getEndColumn() );
        
        pos = mComplex.mComments.get( 1 );
        assertEquals( 3, pos.getStartLine() );
        assertEquals( 0, pos.getStartColumn() );
        assertEquals( 5, pos.getEndLine() );
        assertEquals( 2, pos.getEndColumn() );
    }
    
    /**
     * 範囲コメントのテスト．
     * 何箇所かについて，isInCommentメソッドのテストを行います．
     */
    @Test
    public void testRangeIsInComment()
    {
        assertEquals( false, mRange.isInComment( 0, 0 ) );
        assertEquals( true, mRange.isInComment( 2, 0 ) );
        assertEquals( true, mRange.isInComment( 8, 4 ) );
        assertEquals( true, mRange.isInComment( 12, 0 ) );
        assertEquals( false, mRange.isInComment( 10, 0 ) );
        assertEquals( true, mRange.isInComment( 3, 0 ) );
    }
    
    /**
     * 範囲コメントのテスト．
     * データの行数が14であることをテストします．
     */
    @Test
    public void testRangeLines()
    {
        assertEquals( 14, mRange.mLines.size() );
    }
    
    /**
     * 範囲コメントのテスト．
     * コメントの個数が4であることをテストします．
     */
    @Test
    public void testRangeComments()
    {
        assertEquals( 4, mRange.mComments.size() );
    }
    
    /**
     * 範囲コメントのテスト．
     * 各コメントの開始行等をテストします．
     */
    @Test
    public void testRangePosition()
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
        assertEquals( 1, mSingle.mLines.size() );
    }
    
    /**
     * 行コメントのテスト．
     * 何箇所か場所を指定してisInCommentメソッドをテストします
     */
    @Test
    public void testSingleIsInComment()
    {
        assertEquals( false, mSingle.isInComment( 0, 0 ) );
        assertEquals( true, mSingle.isInComment( 0, 1 ) );
        assertEquals( true, mSingle.isInComment( 0, 2 ) );
    }
    
    @Test
    public void testSingleComments()
    {
        // 1個のコメントがあることをテスト
        assertEquals( 1, mSingle.mComments.size() );
    }
    
    @Test
    public void testSingleStartColumn()
    {
        // 開始カラムが2であることをテスト．2カラム目はインデックスにすると1であることに注意
        assertEquals( 1, mSingle.mComments.get( 0 ).getStartColumn() );
    }
    
    @Test
    public void testSingleEndColumn()
    {
        // 終了カラムが-1であることをテスト
        assertEquals( 3, mSingle.mComments.get( 0 ).getEndColumn() );
    }
    
    @Test
    public void testSingleStartLine()
    {        
        // 開始行が1であることをテスト
        assertEquals( 0, mSingle.mComments.get( 0 ).getStartLine() );
    }
     
    @Test
    public void testSingleEndLine()
    {
        // 終了行が1であることをテスト．
        assertEquals( 0, mSingle.mComments.get( 0 ).getEndLine() );
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
