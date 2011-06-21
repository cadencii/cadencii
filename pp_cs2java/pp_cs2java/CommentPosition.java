package pp_cs2java;

public class CommentPosition
{
    private int mStartLine;
    private int mStartColumn;
    private int mEndLine;
    private int mEndColumn;
    
    public CommentPosition( int start_line, int start_column )
    {
        mStartLine = start_line;
        mStartColumn = start_column;
        mEndLine = -1;
    }
    
    public void setEnd( int end_line, int end_column )
    {
        mEndLine = end_line;
        mEndColumn = end_column;
    }

    public int getStartLine()
    {
        return mStartLine;
    }
    
    public int getStartColumn()
    {
        return mStartColumn;
    }
    
    public int getEndLine()
    {
        return mEndLine;
    }
    
    public int getEndColumn()
    {
        return mEndColumn;
    }
}
