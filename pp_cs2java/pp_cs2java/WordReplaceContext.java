package pp_cs2java;

class WordReplaceContext
{
    // "/*"によるコメントの途中かどうか
    public boolean isCommentStarted = false;
    // 文字列が開始されたかどうか
    public boolean isStringLiteralStarted = false;

    public Object clone()
    {
        WordReplaceContext ret = new WordReplaceContext();
        ret.isCommentStarted = this.isCommentStarted;
        ret.isStringLiteralStarted = this.isStringLiteralStarted;
        return ret;
    }
}
