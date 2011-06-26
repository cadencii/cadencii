package pp_cs2java;

import java.io.BufferedReader;
import java.util.Vector;

public class SourceText{
    protected Vector<CommentPosition> mComments = new Vector<CommentPosition>();
    protected Vector<String> mLines = new Vector<String>();

    protected SourceText(){
    }

    public SourceText( BufferedReader reader ){
        // 読み込む
        load( reader );

        // コメントの範囲を調べる
        CommentPosition pos = null;
        int lines = mLines.size();

        // 1行ずつみていく
        for( int i = 0; i < lines; i++ ){
            String line = mLines.get( i );
            int indx = 0;
            while( true ){
                if( pos == null ){
                    // コメントが未だ始まっていないので"/*"または"//"を探す
                    int start_range = line.indexOf( "/*", indx );
                    int start_line = line.indexOf( "//", indx );
                    if( start_range < 0 && start_line < 0 ){
                        break;
                    }
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
                }else{
                    // コメントの途中なので*/を探す
                    int end = line.indexOf( "*/", indx );
                    if( end < 0 ){
                        break;
                    }
                    pos.setEnd( i, end + 2 );
                    pos = null;
                    indx = end + 2;
                    pos = null;
                }
                if( indx >= line.length() ){
                    break;
                }
            }
        }
    }

    public String getLine(
        int line_number ){
        return mLines.get( line_number );
    }

    /**
     * 指定した位置の文字がコメント文に属するものかどうかをチェックします． コメントの開始・終了を指定する文字列もコメント文と判定します．
     * 
     * @return
     */
    public boolean isInComment(
        int line,
        int column ){
        for( CommentPosition pos : mComments ){
            int start_line = pos.getStartLine();
            int start_column = pos.getStartColumn();
            int end_line = pos.getEndLine();
            int end_column = pos.getEndColumn();
            if( line < start_line ){
                continue;
            }
            if( end_line < line ){
                continue;
            }
            if( start_line < line && line < end_line ){
                return true;
            }
            if( start_line == end_line ){
                if( line == start_line ){
                    if( start_column <= column && column < end_column ){
                        return true;
                    }
                }
            }else{
                if( start_line == line ){
                    if( start_column <= column ){
                        return true;
                    }
                }
                if( end_line == line ){
                    if( column < end_column ){
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /**
     * 読み込んだファイルの行数を取得します．
     * 
     * @return 行数
     */
    public int getLineCount(){
        return mLines.size();
    }

    /**
     * 指定した行の#ifまたは#elifについて，対応する#elseまたは#elifの行番号を調べます．
     * 
     * @param start
     *            検索開始する行番号．
     * @return 
     *         start行目にある#if文または#elif文に対応する最初の#else文または#elif文の行番号を返します．start行目に#ifまたは
     *         #elifが見つからなければ-1を返します． また，対応する#elseまたは#elifが見つからなかった場合も-1を返します．
     */
    public int findElseSentence(
        int start ){
        int draft = findSentence( start, new String[]{ "#if", "#elif" }, new String[]{
            "#else", "#elif" } );
        if( draft < 0 ){
            // elseが見つからないならそのまま帰す.
            return draft;
        }
        if( findEndIfSentence( start ) < draft ){
            // elseの発見位置が，endifよりも後ろにある場合，
            // 別の階層のif構文のものを検出していることになるので，
            // 見つからなかったのと一緒
            return -1;
        }else{
            return draft;
        }
    }

    /**
     * 指定した行の#if,#else,または#elifについて，対応する#endifの行番号を調べます．
     * 
     * @param start
     *            検索開始する行番号
     * @return start行目にある#if文,#else文,または#elif文に対応する#endif文の行番号を返します．start行目に#
     *         ifが見つからなければ- 1を返します．
     */
    public int findEndIfSentence(
        int start ){
        return findSentence( start, new String[]{ "#if", "#elif", "#else" }, new String[]{ "#endif" } );
    }

    /**
     * 指定した検索開始行にあるstart_sentencesに対応するsearchの行番号を調べます．
     * 
     * @param start
     *            検索開始する行番号．
     * @param start_sentences
     *            開始行に存在することが想定される構文のリスト．
     * @param search
     *            終了行に存在することが想定される構文のリスト．
     * @return start行の構文に対応する終了行の行番号を返します．見つからなければ-1を返します．
     */
    protected int findSentence(
        int start,
        String[] start_sentences,
        String[] search ){
        int num_lines = mLines.size();

        // 行が範囲外の場合
        if( start < 0 || num_lines <= start ){
            return -1;
        }

        // 検索開始行に
        boolean valid_start_line = false;
        for( String sentence : start_sentences ){
            if( isContainsPreprocessorDirective( start, sentence ) ){
                valid_start_line = true;
                break;
            }
        }
        if( !valid_start_line ){
            return -1;
        }
        if( !isContainsPreprocessorDirective( start, "#if" )
            && !isContainsPreprocessorDirective( start, "#elif" )
            && !isContainsPreprocessorDirective( start, "#else" ) ){
            return -1;
        }

        // #ifまたは#endifにぶち当たるまで読み飛ばします
        for( int i = start + 1; i < num_lines; i++ ){
            if( isContainsPreprocessorDirective( i, "#if" ) ){
                i = findEndIfSentence( i );
            }else{
                for( String s : search ){
                    if( isContainsPreprocessorDirective( i, s ) ){
                        return i;
                    }
                }
            }
        }

        return -1;
    }

    /**
     * 指定した行に有効なプリプロセッサディレクティブが記述されているかどうかを判定します
     * 
     * @param line_number
     *            判定する行の行番号，
     * @param 検索するプリプロセッサディレクティブ
     *            ．例えば"#if"．
     * @return 該当行に有効なプリプロセッサディレクティブが含まれていればtrueを，そうでなければfalseを返します．
     */
    public boolean isContainsPreprocessorDirective(
        int line_number,
        String directive ){
        String line = mLines.get( line_number );
        int indx = line.indexOf( directive );
        if( indx < 0 ){
            return false;
        }
        String indent = line.substring( 0, indx ).trim();
        if( !indent.equals( "" ) ){
            return false;
        }
        if( isInComment( line_number, indx ) ){
            return false;
        }
        return true;
    }

    /**
     * ファイルをロードします．
     * 
     * @param path
     *            ロードするファイルのパス．
     */
    private void load(
        BufferedReader reader ){
        try{
            String line;
            while( (line = reader.readLine()) != null ){
                mLines.add( line );
            }
        }catch( Exception ex ){
            ex.printStackTrace( System.err );
        }
    }

}
