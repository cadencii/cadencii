package pp_cs2java;

/**
 * 1個のプリプロセッサ・ディレクティブを表現します
 */
class DirectiveUnit
{
    /**
     * ディレクティブの名前
     */
    public String name;
    /**
     * 否定かどうか
     */
    public boolean not;

    /**
     * パラメータを指定したコンストラクタ
     *
     * @param name
     * @param not
     */
    public DirectiveUnit( String name, boolean not )
    {
        this.name = name;
        this.not = not;
    }
}
