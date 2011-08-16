package pp_cs2java;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * 文字列のリプレース方法を表現する
 */
class Replacement
{
	private String search;
	private String replace;
	private Pattern pattern = null;
	private boolean matchWord;

	public Replacement( String search, String replace, boolean matchWord )
	{
		this.matchWord = matchWord;
		if( matchWord ){
            this.pattern = Pattern.compile( "\\b" + search + "\\b" );
		}
		this.search = search;
		this.replace = replace;
	}

	public int findFrom( String line, int start )
	{
		if( matchWord ){
			Matcher matcher = pattern.matcher( line );
			if( matcher.find( start ) ){
				return matcher.start();
			}else{
				return -1;
			}
		}else{
			return line.indexOf( this.search, start );
		}
	}
	
	public String getSearch()
	{
		return this.search;
	}
	
	public String getReplace()
	{
		return this.replace;
	}
}
