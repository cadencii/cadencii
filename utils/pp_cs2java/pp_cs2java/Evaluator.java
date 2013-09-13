package pp_cs2java;

import java.util.Arrays;
import java.util.Comparator;
import java.util.Vector;

public class Evaluator{
    private static final String VALID_SYNTAX = "! |&()";
    private static final String VALID_DIRECTIVE = "_0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    static class StringComparator implements Comparator<String>{
        @Override
        public int compare( String arg0, String arg1 ){
            return arg0.compareTo( arg1 );
        }
    }

    /**
     * プリプロセッサディレクティブからなる式を解釈します．
     * 
     * @param equation
     *            評価する式．例えば"A && B"など．
     * @param defined
     *            trueとして定義されたディレクティブの一覧． この中に無いディレクティブは全てfalseと解釈されます．
     * @return 解釈した結果を返します．
     * @throws Exception
     *             式にエラーがあるなどの原因で解釈できなかった場合例外がスローされます．
     */
    public static boolean eval( String equation, String[] defined )
            throws Exception{
        // 無効な文字が含まれている場合は例外を投げる
        if( !isValidEquation( equation ) ){
            throw new Exception();
        }
        equation = "(" + equation.replace( " ", "" ) + ")";
        Arrays.sort( defined, new StringComparator() );
        String ret = evalRecursive( equation, defined );
        if( ret.equals( "+" ) ){
            return true;
        }else if( ret.equals( "-" ) ){
            return false;
        }else{
            throw new Exception();
        }
    }

    public static boolean eval( String equation, Vector<String> defined )
            throws Exception{
        return eval( equation, defined.toArray( new String[]{} ) );
    }

    /**
     * 式を再帰的に評価します．式の内部表現では，真は+，偽は-で表現します．
     * 
     * @param equation
     *            評価する式．
     * @param defined
     *            定義済みのディレクティブ．
     * @return 評価結果．
     */
    protected static String evalRecursive( String equation, String[] defined )
            throws Exception{
        // カッコがなくなるまで処理する
        int index_open = equation.indexOf( "(" );
        while( index_open >= 0 ){
            // 対応する閉じカッコを調べます．
            int length = equation.length();
            int num_open = 1;
            int index_close = -1;
            for( int i = index_open + 1; i < length; i++ ){
                char c = equation.charAt( i );
                if( c == '(' ){
                    num_open++;
                }
                if( c == ')' ){
                    num_open--;
                }
                if( num_open == 0 ){
                    index_close = i;
                    break;
                }
            }
            if( index_close < 0 ){
                throw new Exception();
            }
            String inner_equation = equation.substring( index_open + 1,
                    index_close );
            String result = evalRecursive( inner_equation, defined );
            if( index_open > 0 ){
                // 否定の!がカッコの直前についてないかチェック
                if( equation.substring( index_open - 1, index_open ).equals( "!" ) ){
                    index_open--;
                    if( result.equals( "-" ) ){
                        result = "+";
                    }else if( result.equals( "+" ) ){
                        result = "-";
                    }else{
                        throw new Exception();
                    }
                }
            }
            equation = equation.substring( 0, index_open ) + result
                    + equation.substring( index_close + 1 );
            index_open = equation.indexOf( "(" );
        }
        // ディレクティブを検出して"+"または"-"に変換する
        boolean changed = true;
        while( changed ){
            changed = false;
            int length = equation.length();
            for( int i = 0; i < length; i++ ){
                char c = equation.charAt( i );
                int indx = VALID_DIRECTIVE.indexOf( c, 0 );
                if( indx < 0 ){
                    continue;
                }
                String name = "";
                for( int j = i; j < length; j++ ){
                    char d = equation.charAt( j );
                    if( VALID_DIRECTIVE.indexOf( d, 0 ) < 0 || j == length - 1 ){
                        if( j == length - 1 ){
                            name += String.valueOf( d );
                        }
                        // ディレクティブが終了したので，+か-で置換する
                        String replace = "";
                        int index_defined = Arrays.binarySearch( defined, name, new StringComparator() );
                        if( index_defined >= 0 ){
                            // 定義に含まれているのでtrue(+)
                            replace = "+";
                        }else{
                            // 定義に含まれていないのでfalse(-)
                            replace = "-";
                        }
                        String prefix = equation.substring( 0, i );
                        String suffix = "";
                        if( j != length - 1 && j < length ){
                            suffix = equation.substring( j );
                        }
                        equation = prefix + replace + suffix;
                        changed = true;
                        break;
                    }else{
                        name = name + String.valueOf( d );
                    }
                }
                if( changed ){
                    break;
                }
            }
        }

        // カッコがなく，&&と||と!+-だけで構成された式を処理します
        // まず否定を処理
        // 連続した!!は何も無いのと一緒
        int index_double_not = equation.indexOf( "!!" );
        while( index_double_not >= 0 ){
            equation = equation.replace( "!!", "" );
            index_double_not = equation.indexOf( "!!" );
        }

        // !の後ろにある+や-を反転させます
        int index_not = equation.indexOf( "!" );
        while( index_not >= 0 ){
            equation = equation.replace( "!-", "+" );
            equation = equation.replace( "!+", "-" );
            index_not = equation.indexOf( "!" );
        }

        changed = true;
        String[] TRUE = new String[]{ "+&&+", "+||+", "+||-", "-||+" };
        String[] FALSE = new String[]{ "+&&-", "-&&+", "-&&-", "-||-" };
        while( changed ){
            changed = false;
            for( String search : TRUE ){
                int index = equation.indexOf( search );
                if( index >= 0 ){
                    equation = equation.substring( 0, index ) + "+"
                            + equation.substring( index + 4 );
                    changed = true;
                }
            }

            for( String search : FALSE ){
                int index = equation.indexOf( search );
                if( index >= 0 ){
                    equation = equation.substring( 0, index ) + "-"
                            + equation.substring( index + 4 );
                    changed = true;
                }
            }
        }

        return equation;
    }

    /**
     * 式の構文が正しいかどうかをチェックします．
     */
    protected static boolean isValidEquation( String equation ){
        if( !isValidCharacters( equation ) ){
            return false;
        }
        if( !isValidBooleanExpression( equation ) ){
            return false;
        }
        if( !isValidBrackets( equation ) ){
            return false;
        }
        if( !isValidDirectiveName( equation ) ){
            return false;
        }
        return true;
    }

    /**
     * 式の文字列中に，想定していない文字が含まれていないことをチェックします．
     * 
     * @param equation
     *            チェックする文字列．
     * @return 文字列が正常であればtrue，そうでなければfalseを返します．
     */
    protected static boolean isValidCharacters( String equation ){
        if( equation == null )
            return true;
        int length = equation.length();
        for( int i = 0; i < length; i++ ){
            char c = equation.charAt( i );
            if( VALID_SYNTAX.indexOf( c, 0 ) < 0
                    && VALID_DIRECTIVE.indexOf( c, 0 ) < 0 ){
                return false;
            }
        }
        return true;
    }

    /**
     * 式の中のブール演算子が正しい記法となっているかどうかをチェックします．
     */
    protected static boolean isValidBooleanExpression( String equation ){
        // &があるのに&&が無い場合
        if( equation.indexOf( "&" ) != equation.indexOf( "&&" ) )
            return false;
        // |があるのに||がない場合
        if( equation.indexOf( "|" ) != equation.indexOf( "||" ) )
            return false;

        return true;
    }

    /**
     * 括弧の対応付けが正しいかどうかをチェックします．
     */
    protected static boolean isValidBrackets( String equation ){
        int num_open = 0;
        int num_close = 0;
        int length = equation.length();
        for( int i = 0; i < length; i++ ){
            char c = equation.charAt( i );
            if( c == '(' )
                num_open++;
            if( c == ')' )
                num_close++;
        }
        if( num_open != num_close )
            return false;
        return equation.indexOf( "()" ) < 0;
    }

    /**
     * 式の中に現れるディレクティブ名が正しいものかどうかをチェックします．
     */
    protected static boolean isValidDirectiveName( String equation ){
        int length = equation.length();
        // 0: 変数名の先頭の文字を探している状態
        // 1: 変数名の中を検索している状態
        int status = 0;
        for( int i = 0; i < length; i++ ){
            // 変数名の最初の文字を検出
            char c = equation.charAt( i );
            if( status == 0 ){
                if( VALID_DIRECTIVE.indexOf( c, 0 ) >= 0 ){
                    status = 1;
                    if( "0123456789".indexOf( c, 0 ) >= 0 ){
                        // 数字で始まっているディレクティブは無効
                        return false;
                    }
                }
            }else{
                if( VALID_SYNTAX.indexOf( c, 0 ) >= 0 ){
                    status = 0;
                }
            }
        }
        return true;
    }
}
